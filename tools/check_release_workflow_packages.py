#!/usr/bin/env python3
"""Refuse a release workflow whose hard-coded package list has drifted from src/ (#610).

Two workflows carry a list of the packages they will release, written by hand:

  .github/workflows/release-nuget-org.yml  `options:` under a `type: choice` input
  .github/workflows/release.yml            `case "$PACKAGE" in A|B|C) ;; *) exit 1`

Measured on the 0.6.0 release (#609): four of the eight packages the milestone was
meant to publish could not be published at all. Lodestar.Cluster, Lodestar.Preprocessing,
Lodestar.Extensions.AI and Lodestar.Extensions.MathNet were each added by a pull request
that had no reason to think about a release workflow, and neither list had heard of any
of them. GitHub refuses a dispatch naming a package outside `options:` before the job
starts, and a tag for a package outside the `case` arm publishes nothing and reports
"Unknown package" -- both of which are found by trying to cut a release, which is the
worst moment to find anything.

That is the shape #586 and #597 already found twice, and tools/check_bench_map.py and
tools/check_readme_pack_loop.py are the model: read both sides from the file that owns
each and compare, so the divergence fails on the commit that introduces it.

Neither list can be replaced by reading a source of truth, which is what wiki.yml did
instead in #609. GitHub requires `options:` to be a literal sequence in the workflow
file, and the `case` arm is deliberately fixed in a job that mints a publishing key --
deriving what it accepts from anything the tag controls is the one thing it must not do.
So they stay hand-written and this keeps them honest.

src/ is the source of truth: a directory src/Lodestar.* holding a Version.props declares
a version and therefore expects to be released. Order is not compared -- both lists read
in a presentation order of their own, and neither workflow cares.

Standard library only, and the small reader below rather than PyYAML, for two reasons.
.githooks/pre-commit runs the guards through whichever of `python3` or `python` the
contributor's machine resolves, not through .venv-oracles, so an import outside the
standard library turns a missing package into a failed commit. And `on:` is the key this
file has to descend through: PyYAML reads YAML 1.1, where a bare `on` is the boolean
True, so `safe_load(...)["on"]` raises KeyError on every GitHub workflow ever written.

Usage:  python tools/check_release_workflow_packages.py
        python tools/check_release_workflow_packages.py --help

Exit:   0 clean, 1 findings printed, 2 bad usage
"""

from __future__ import annotations

import pathlib
import re
import sys

ROOT = pathlib.Path(__file__).resolve().parent.parent
WORKFLOWS = ROOT / ".github" / "workflows"
DISPATCH = WORKFLOWS / "release-nuget-org.yml"
TAG = WORKFLOWS / "release.yml"

# Where the dispatch workflow keeps the list, as the key path a reader descends.
OPTIONS_PATH = ("on", "workflow_dispatch", "inputs", "package", "options")

KEY = re.compile(r"^\s*([\w.-]+):")
# A lazy `(.*?)` closed by `\s*$` is the pair S5852 reports; one greedy run is not.
ITEM = re.compile(r"^\s*-\s+(\S.*)$")
# The one `case` in release.yml that judges a package name. release-nuget-org.yml has a
# `case "$status" in` of its own, so the subject is what tells them apart.
CASE_OPEN = re.compile(r'^\s*case\s+"\$PACKAGE"\s+in\s*$')
# `Lodestar.A|Lodestar.B) ;;` -- the allow-list arm, whatever it holds.
CASE_ARM = re.compile(r"^\s*(\S.*?)\)\s*;;\s*$")


def label(path: pathlib.Path) -> str:
    """A repository-relative path, so a finding reads the same on every machine."""
    try:
        return path.relative_to(ROOT).as_posix()
    except ValueError:
        # A path outside the tree, which only the unit tests hand this. Reporting it
        # whole beats raising out of a function whose job is to format a message.
        return path.as_posix()


def _significant(line: str) -> bool:
    """A line that carries structure, rather than blank or a comment."""
    stripped = line.strip()
    return bool(stripped) and not stripped.startswith("#")


def _child(lines: list[str], start: int, indent: int, key: str) -> tuple[int, int] | None:
    """Where `key` sits in the mapping opening at `lines[start]`, with that mapping's indent.

    The mapping's own indentation is whatever its first significant line carries; a line
    deeper than that belongs to a sibling's subtree, and one at or above `indent` means
    the mapping has ended. That is what keeps an `options:` elsewhere in the file from
    answering for the one under `inputs.package`.
    """
    child_indent = None
    for index in range(start, len(lines)):
        line = lines[index]
        if not _significant(line):
            continue
        here = len(line) - len(line.lstrip())
        if here <= indent:
            return None
        if child_indent is None:
            child_indent = here
        match = KEY.match(line) if here == child_indent else None
        if match and match.group(1) == key:
            return index, child_indent
    return None


def _items(lines: list[str], start: int, indent: int) -> list[str]:
    """The scalars of the block sequence opening at `lines[start]`."""
    found = []
    for line in lines[start:]:
        if not _significant(line):
            continue
        match = ITEM.match(line) if len(line) - len(line.lstrip()) > indent else None
        if match is None:
            break
        found.append(match.group(1).strip().strip("'\""))
    return found


def block_sequence(text: str, path: tuple[str, ...]) -> list[str] | None:
    """The scalar items of the block sequence at `path`, or None if it is not there.

    A reader for the subset these two files use -- block mappings of plain keys, one
    block sequence of plain scalars -- not a YAML implementation.
    """
    lines = text.splitlines()
    start, indent = 0, -1

    for key in path:
        found = _child(lines, start, indent, key)
        if found is None:
            return None
        start, indent = found[0] + 1, found[1]

    return _items(lines, start, indent)


def declared_packages() -> set[str]:
    """Every package src/ declares a version for, and therefore expects to release."""
    return {
        path.parent.name
        for path in (ROOT / "src").glob("Lodestar.*/Version.props")
    }


def case_packages(text: str) -> tuple[list[str] | None, list[str]]:
    """The names release.yml's allow-list accepts, with any finding about its shape.

    The arm is shell inside a `run:` block scalar rather than YAML structure, so it is
    read as the shell it is. The one-line rule is the point of reading it at all beyond
    its contents: `\\` inside a `case` pattern does not join the alternatives the way it
    joins a command's arguments. The continuation keeps the next line's leading
    whitespace, so ` Lodestar.Gpu` is what the pattern then holds -- an alternative no
    package name can ever equal, and every name below the break stops matching in
    silence. The line is long on purpose.
    """
    lines = text.splitlines()
    for index, line in enumerate(lines):
        if not CASE_OPEN.match(line):
            continue
        arm = next((lines[i] for i in range(index + 1, len(lines)) if _significant(lines[i])), "")
        if arm.rstrip().endswith("\\"):
            return None, [
                f"{label(TAG)}: the `case \"$PACKAGE\" in` allow-list is wrapped with a "
                "backslash. A continuation inside a `case` pattern keeps the next line's "
                "indentation instead of joining the alternatives, so every name below the "
                "break stops matching and its tag publishes nothing. Keep it on one line."
            ]
        match = CASE_ARM.match(arm)
        if match is None:
            break
        return [name.strip() for name in match.group(1).split("|")], []

    return None, [
        f"{label(TAG)}: no `case \"$PACKAGE\" in` allow-list to check. It is what "
        "decides which tags release, and one this file cannot read cannot be vouched for."
    ]


def compare(listed: list[str], declared: set[str], where: str, cost: str) -> list[str]:
    """One list against src/, in both directions."""
    missing = sorted(declared - set(listed))
    extra = sorted(set(listed) - declared)

    findings = []
    if missing:
        findings.append(
            f"{where} does not list {', '.join(missing)}, which src/ declares a version "
            f"for. {cost}"
        )
    if extra:
        findings.append(
            f"{where} lists {', '.join(extra)}, and there is no src/<name>/Version.props "
            "for it. The list offers a release that cannot be packed."
        )
    return findings


def findings() -> list[str]:
    for path in (DISPATCH, TAG):
        if not path.exists():
            return [f"{label(path)}: missing"]

    declared = declared_packages()
    if not declared:
        return ["src/: no Lodestar.*/Version.props at all, so there is nothing to compare."]

    found = []

    options = block_sequence(DISPATCH.read_text(encoding="utf-8"), OPTIONS_PATH)
    if options is None:
        found.append(
            f"{label(DISPATCH)}: no `{'.'.join(OPTIONS_PATH)}` list to check. It is what "
            "the Actions tab offers, and one this file cannot read cannot be vouched for."
        )
    else:
        found += compare(
            options, declared, f"{label(DISPATCH)}'s `options:` list",
            "GitHub refuses a dispatch naming a package outside the list before the job "
            "starts, so that package cannot be published from the Actions tab at all.")

    accepted, shape = case_packages(TAG.read_text(encoding="utf-8"))
    found += shape
    if accepted is not None:
        found += compare(
            accepted, declared, f"{label(TAG)}'s `case` allow-list",
            "A <PackageId>/v<Version> tag for it publishes nothing and the job reports "
            "\"Unknown package\".")

    return found


def main() -> int:
    if len(sys.argv) > 1:
        if sys.argv[1] in ("--help", "-h"):
            print(__doc__)
            return 0
        print(__doc__)
        return 2

    found = findings()
    for finding in found:
        print(finding)
    if not found:
        print(f"ok  both release workflows list the {len(declared_packages())} packages "
              "src/ declares a version for")
    return 1 if found else 0


if __name__ == "__main__":
    raise SystemExit(main())
