#!/usr/bin/env python3
"""Refuse a pull request that rewrites an already-accepted ADR.

A deletion is allowed. Issue #1103 keeps the records that state an axis of the
project and deletes the rest, so "never touched" would refuse the pass that the
directory's own rule asks for; what immutability protects is a body being
rewritten under a number a reader has already cited, and a deleted body stays
readable in git. The number is never reused: `.next-adr` counts every ref.

A numbering epoch is the one way a number comes to mean something else. When a
diff raises docs/decisions/.numbering-epoch, an accepted record may be rewritten
or renamed in it: the raised line is the explicit, reviewed act that tells every
reader a number now holds a different record, which is what #1103 did to all
seven in epoch 2 and did to 0003 alone in epoch 3. In any other diff the rule
below stands unchanged.

docs/decisions/README.md states the rule directly: a record is never edited,
and may only be deleted. The convention that came before it -- appending a
`> **#NNN update:**` blockquote next to a stale claim -- was itself superseded:
"a decision record is not edited; an amendment is its own record", which pulled
three such blockquotes back out of the record they had been added to. That is
the whole rule, addition included, not just removal -- and nothing enforced any
version of it before this script. An edit to an already-merged
ADR is still valid markdown, still passes every other gate, and says nothing
about itself.

Only files that already existed at --base are covered: a brand-new ADR in the
same pull request is unrestricted, and so is docs/decisions/README.md, which is
the index rather than a decision and is expected to gain a row on every ADR.

One narrow exception, tools/regen_adr_index.py: inserting a YAML frontmatter block above the
title is allowed when the body below it is byte-identical. docs/decisions/index.yaml
is generated from those blocks, and an immutable record cannot be edited to name the
decision that later amended it -- so the 105 records that predate the index were
given their block in one pass, verified the only way that claim can be: the body did
not move by one byte. Changing a block that is already there is refused like any
other edit, which makes the exception self-limiting -- once a record carries
frontmatter it can never take this path again, and tools/check_adr_frontmatter.py
refuses a new ADR without one.

Usage:  python tools/check_adr_immutable.py --base <commit>
        python tools/check_adr_immutable.py --help

  --base <commit>  What the pull request is compared against. Required: an
                    implicit default would silently compare against the wrong
                    thing on a rebased or force-pushed branch.
  --help, -h       Print this message to stdout and exit 0.

Exit:   0 clean, 1 findings printed, 2 bad usage
"""

from __future__ import annotations

import pathlib
import re
import subprocess
import sys

# PYTHONSAFEPATH=1 keeps this script's own directory off sys.path, the way
# generate_oracles.py documents -- appended, so nothing here shadows a package.
sys.path.append(str(pathlib.Path(__file__).resolve().parent))

import adr_index  # noqa: E402

ROOT = pathlib.Path(__file__).resolve().parent.parent

# The index (docs/decisions/README.md) is deliberately unmatched: it is not an
# ADR body, and every new decision adds a row to it by design.
ADR_PATH = re.compile(r"^docs/decisions/\d{4}-.*\.md$")

# A leading '-' would read to git as an option rather than a revision -- the
# same guard tools/select_benchmarks.py's REVISION applies to its own --since.
REVISION = re.compile(r"^[0-9A-Za-z][0-9A-Za-z._/~^-]{0,254}$")


EPOCH_FILE = "docs/decisions/.numbering-epoch"
ENCODING = "utf-8"


def epoch_of(text: str | None) -> int:
    """The epoch a `.numbering-epoch` text declares on its first line; 1 when it declares none.

    The same reading `.next-adr` makes, so the two cannot disagree about which epoch a tree is in.
    """
    if not text or not text.strip():
        return 1
    first = text.strip().splitlines()[0].strip()
    return int(first) if first.isdigit() else 1


def epoch_raised(base: str) -> bool:
    """Whether this diff raises the numbering epoch above the one `base` declares."""
    try:
        now = (ROOT / EPOCH_FILE).read_text(encoding=ENCODING)
    except OSError:
        now = None
    return epoch_of(now) > epoch_of(text_at(base, EPOCH_FILE))


def existed_at(base: str, path: str) -> bool:
    """Whether `path` was already a file at `base`, not introduced by this PR."""
    result = subprocess.run(
        ["git", "cat-file", "-e", f"{base}:{path}"],
        cwd=ROOT, capture_output=True, check=False)
    return result.returncode == 0


def changed_adr_files(base: str) -> list[str]:
    """Every docs/decisions/ ADR the diff against `base` touches, README.md excluded."""
    out = subprocess.run(
        ["git", "diff", "--name-only", base, "--", "docs/decisions/"],
        cwd=ROOT, capture_output=True, text=True, check=True)
    return [line for line in out.stdout.splitlines() if ADR_PATH.match(line)]


def deleted_now(path: str) -> bool:
    """Whether the record is gone from the working tree rather than rewritten."""
    return not (ROOT / path).is_file()


def renamed_records(base: str) -> dict[str, str]:
    """Every accepted record this diff renames, old path to new.

    A rename is how a rewrite would otherwise walk through the deletion allowance:
    the old path reads as deleted and the new one as a record this pull request
    introduced, so neither half is checked. Both halves are the same record under
    a new name, and renaming one is editing it.
    """
    out = subprocess.run(
        ["git", "diff", "--name-status", "--find-renames=50%", base, "--", "docs/decisions/"],
        cwd=ROOT, capture_output=True, text=True, check=True)
    found = {}
    for line in out.stdout.splitlines():
        parts = line.split("\t")
        if len(parts) == 3 and parts[0].startswith("R") and ADR_PATH.match(parts[1]):
            found[parts[1]] = parts[2]
    return found


def line_counts(base: str, path: str) -> tuple[int, int]:
    """(added, removed) lines the diff against `base` reports for `path`."""
    out = subprocess.run(
        ["git", "diff", "--numstat", base, "--", path],
        cwd=ROOT, capture_output=True, text=True, check=True)
    added, removed, _ = out.stdout.strip().split("\t", 2)
    return int(added), int(removed)


def text_at(base: str, path: str) -> str | None:
    """`path` as it reads at `base`, or None when it cannot be read as text."""
    result = subprocess.run(
        ["git", "show", f"{base}:{path}"],
        cwd=ROOT, capture_output=True, check=False)
    if result.returncode != 0:
        return None
    try:
        return result.stdout.decode(ENCODING)
    except UnicodeDecodeError:
        return None


def is_frontmatter_insertion(base: str, path: str) -> bool:
    """Whether the only change is a frontmatter block added above an untouched body.

    tools/regen_adr_index.py's one exception, and the whole of it. The block is metadata for
    docs/decisions/index.yaml; the body is the decision, and what immutability is
    for is that the historical reasoning is not rewritten. So the test is exactly
    that: the record had no block, it has one now, and the text below it is the
    same bytes it was. A record that already carries a block fails here, which is
    what keeps this from becoming a way to edit one.
    """
    was = text_at(base, path)
    if was is None:
        return False
    now_path = ROOT / path
    if not now_path.is_file():
        return False
    try:
        now = now_path.read_text(encoding=ENCODING)
    except (OSError, UnicodeDecodeError):
        return False

    had_block, old_body = adr_index.split_frontmatter(was)
    has_block, new_body = adr_index.split_frontmatter(now)
    return had_block is None and has_block is not None and old_body == new_body


def edited_records(base: str, renames: dict[str, str]) -> list[tuple[str, int, int]]:
    """Every accepted record this diff rewrites in place, with its (added, removed) counts.

    The destination of a rename is skipped: `renamed_records` already reports that
    pair, and counting it here would name the same record twice.
    """
    edited = []
    for path in changed_adr_files(base):
        if path in renames.values() or not existed_at(base, path):
            continue
        if is_frontmatter_insertion(base, path) or deleted_now(path):
            continue
        added, removed = line_counts(base, path)
        edited.append((path, added, removed))
    return edited


def main(argv: list[str]) -> int:
    arguments = argv[1:]
    if "--help" in arguments or "-h" in arguments:
        print(__doc__)
        return 0
    if len(arguments) != 2 or arguments[0] != "--base":
        print(__doc__, file=sys.stderr)
        return 2
    base = arguments[1]
    if not REVISION.match(base):
        print(f"--base {base!r} is not a usable revision", file=sys.stderr)
        return 2

    if epoch_raised(base):
        print(f"{EPOCH_FILE} raised against {base}: an accepted record may be rewritten in this diff.")
        return 0
    findings = []
    # `git diff --name-only` reports a rename under its destination alone, so the renamed
    # records are collected here rather than found by `edited_records`.
    renames = renamed_records(base)
    for old_path, new_path in sorted(renames.items()):
        findings.append((old_path, 0, 0))
        print(f"{old_path}: renamed to {new_path} -- an accepted record keeps the name it was cited under.")

    findings.extend(edited_records(base, renames))

    for path, added, removed in findings:
        if path in renames:
            continue
        print(f"{path}: {added} line(s) added, {removed} removed -- already accepted.")

    if findings:
        print(
            f"\n{len(findings)} ADR file(s) touched past what an accepted decision "
            "allows -- not even a `> **#<issue> update:**` blockquote, per "
            "\"Amend 0004 in a decision of its own instead of editing it\". Revert "
            "the change and record it as a new ADR instead, indexed in "
            "docs/decisions/README.md. Adding a YAML frontmatter block above an "
            "untouched body is one exception (tools/regen_adr_index.py); raising "
            f"{EPOCH_FILE} in the same diff is the other, and these changes are neither.",
            file=sys.stderr)
        return 1
    return 0


if __name__ == "__main__":
    sys.exit(main(sys.argv))
