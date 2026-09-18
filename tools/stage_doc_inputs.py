#!/usr/bin/env python3
"""Copy the Markdown and map a test project copies to its output, from the checkout, into a built output (#997).

A docs-only pull request runs the documentation tests on the net10.0 binaries main's own CI run
published for the pull request's base commit, so nothing is compiled. Those binaries were built
with main's docs; the tests read the files beside the assembly, so this puts the pull request's
own docs there, exactly where MSBuild would have: every ``<None Include=...>`` item of the project
whose ``Include`` reaches ``docs/`` and whose ``CopyToOutputDirectory`` is ``Always`` or
``PreserveNewest``, at its ``Link``, or at ``LinkBase`` plus the path below the glob's fixed part
(with neither, at the file's own name), minus any ``Exclude``.

Reading the project rather than assuming its layout is the point: ``Lodestar.Stats.Tests`` copies
``docs/reference/stats.md`` beside the pages of its own folder, and a hard-coded pattern missed it.

Usage:  python tools/stage_doc_inputs.py tests/Lodestar.Stats.Tests/Lodestar.Stats.Tests.csproj OUTPUT_DIR
Replaces the ``docs``, ``reference`` and copied files under OUTPUT_DIR; exits 1 on a project it
cannot read.
"""

from __future__ import annotations

import fnmatch
import pathlib
import shutil
import sys
import xml.etree.ElementTree as ET


# The two values MSBuild's copy targets match, lowered because it matches them case-insensitively:
# measured on a built project, `preservenewest` and `ALWAYS` copy, `Never` and `false` do not (#1062).
COPIED = ("always", "preservenewest")


def _split(pattern: str) -> tuple[str, str]:
    """The pattern's fixed directory part and the wildcard remainder."""
    parts = pattern.split("/")
    fixed: list[str] = []
    for part in parts:
        if any(ch in part for ch in "*?["):
            break
        fixed.append(part)
    return "/".join(fixed), "/".join(parts[len(fixed):])


def _matches(relative: str, remainder: str) -> bool:
    """Whether a path below the fixed part matches a remainder such as ``**/*.md``."""
    if remainder.startswith("**/"):
        tail = remainder[3:]
        return fnmatch.fnmatch(pathlib.PurePosixPath(relative).name, tail) or fnmatch.fnmatch(relative, tail)
    return fnmatch.fnmatch(relative, remainder)


def _excluded(source: pathlib.Path, excludes: list[str]) -> bool:
    """Whether a file matches any of the item's ``Exclude`` patterns."""
    absolute = source.as_posix()
    return any(
        fnmatch.fnmatch(absolute, pattern) or absolute.startswith(pattern.rstrip("*").rstrip("/") + "/")
        for pattern in excludes
    )


def _destination(relative: str, link_base: str | None, name: str) -> str:
    """Where one copied file lands, from the item's ``LinkBase`` and the path below the glob."""
    return f"{link_base}/{relative}" if link_base else name


def _item_copies(item: ET.Element, base: pathlib.Path) -> list[tuple[pathlib.Path, str]]:
    """Every (source, destination) one ``<None>`` item copies out of ``docs/``."""
    include = (item.get("Include") or "").replace("\\", "/")
    link_base = item.get("LinkBase")
    # Stripped because a wrapped attribute arrives with a space where its newline was, and
    # MSBuild trims each entry: unstripped, a second pattern excludes nothing (#1062).
    excludes = [
        (base / pattern.strip().replace("\\", "/")).resolve().as_posix()
        for pattern in (item.get("Exclude") or "").split(";")
        if pattern.strip()
    ]
    fixed, remainder = _split(include)
    fixed_dir = (base / fixed).resolve()
    if not remainder:
        if not fixed_dir.is_file():
            return []
        link = (item.get("Link") or "").replace("\\", "/")
        return [(fixed_dir, link or _destination(fixed_dir.name, link_base, fixed_dir.name))]

    copies: list[tuple[pathlib.Path, str]] = []
    for source in sorted(fixed_dir.rglob("*")):
        relative = source.relative_to(fixed_dir).as_posix()
        if source.is_file() and _matches(relative, remainder) and not _excluded(source, excludes):
            copies.append((source, _destination(relative, link_base, source.name)))
    return copies


def plan(project: pathlib.Path) -> list[tuple[pathlib.Path, str]]:
    """Each (source file, destination relative to the output) the project copies from ``docs/``."""
    base = project.parent
    copies: list[tuple[pathlib.Path, str]] = []
    for item in ET.parse(project).getroot().iter("None"):
        include = (item.get("Include") or "").replace("\\", "/")
        if "docs/" in include and (item.get("CopyToOutputDirectory") or "").strip().lower() in COPIED:
            copies.extend(_item_copies(item, base))
    return copies


def stage(project: pathlib.Path, output: pathlib.Path) -> int:
    """Replace the output's copies with the checkout's, returning how many files were copied."""
    copies = plan(project)
    for top in {destination.split("/", 1)[0] for _, destination in copies if "/" in destination}:
        shutil.rmtree(output / top, ignore_errors=True)
    for source, destination in copies:
        target = output / destination
        target.parent.mkdir(parents=True, exist_ok=True)
        shutil.copyfile(source, target)
    return len(copies)


def main(argv: list[str]) -> int:
    if len(argv) != 3:
        print(__doc__, file=sys.stderr)
        return 2
    try:
        count = stage(pathlib.Path(argv[1]), pathlib.Path(argv[2]))
    except (OSError, ET.ParseError) as error:
        print(f"{argv[1]}: {error}", file=sys.stderr)
        return 1
    print(f"{argv[1]}: {count} files")
    return 0


if __name__ == "__main__":
    raise SystemExit(main(sys.argv))
