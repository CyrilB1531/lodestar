#!/usr/bin/env python3
"""Refuse a spec that does not say when it was written, and a plan that is committed (#1104).

A spec and a plan age in opposite directions. A spec records what was decided and
what was measured, so it is still a record when it is written late -- issues #202
to #446 were backfilled from the commits that closed them, and they are worth
keeping. A plan is an instrument for work that has not started: checkbox steps and
a `Branch:` line. Once the work merges, that branch is gone and the boxes are
checkboxes nobody may tick. 116 such plans, 88,745 lines, were tracked here before
this guard, and no file outside `docs/superpowers/` cited one of them -- against 88
citations into `specs/`. So a plan lives beside the work and is never committed.

What a reader of a spec needs first is its relation to the work it describes, and
the three answers are the whole vocabulary:

  written before the work            the spec led; the work followed it
  written with the work              spec and work moved together, one commit
  **retrospective**                  the work merged first; the spec caught up

A retrospective spec is dated by the work rather than by the day it was written,
per CLAUDE.md's *Workflow*, so its status line is the only place saying that the
file arrived late. That is why the vocabulary is closed and why this reads the
first clause rather than looking for a word anywhere in the line: "accepted,
2026-09-16. Written after the measurement it records." buries the one fact a
reader is after behind a lifecycle word every spec in the tree would carry.

Anything after the opening clause is free text -- a date, an amendment, a note
that the work was never done -- and is not read here.

Standard library only: `.githooks/pre-commit` runs this through whichever of
`python3` or `python` a contributor's machine resolves, so an import outside the
standard library would turn a missing package into a failed commit.

Usage:  python tools/check_spec_status.py
        python tools/check_spec_status.py --help

Exit:   0 clean, 1 findings printed, 2 bad usage
"""

from __future__ import annotations

import pathlib
import re
import subprocess
import sys

ROOT = pathlib.Path(__file__).resolve().parent.parent
SPECS = "docs/superpowers/specs/"
PLANS = "docs/superpowers/plans/"

# <date>_<issue padded to four>_<kebab slug>, the repository's naming (CLAUDE.md,
# *Workflow*). The optional letter takes a second spec on one issue: 0122b, 0134a.
NAME = re.compile(r"\d{4}-\d{2}-\d{2}_\d{4}[a-z]?_[a-z0-9-]+\.md")

# Not anchored: the field sits inside the metadata paragraph as often as it opens
# a line, and that paragraph wraps, so the value may begin on the line below.
STATUS = re.compile(r"\*\*Status:\*\*\s+([^\n]*)")

# A spec quoting the field inside an example block is describing the rule, not
# declaring a status, so fenced blocks come out before anything is counted.
FENCE = re.compile(r"^```.*?^```", re.MULTILINE | re.DOTALL)

TIMINGS = (
    "written before the work",
    "written with the work",
    "**retrospective**",
)


def label(path: pathlib.Path) -> str:
    """A repository-relative path, so a finding reads the same on every machine."""
    try:
        return path.relative_to(ROOT).as_posix()
    except ValueError:
        return path.as_posix()


def status_findings(path: pathlib.Path) -> list[str]:
    """One spec, against the shape every spec carries."""
    where = label(path)
    found = []
    if not NAME.fullmatch(path.name):
        found.append(
            f"{where}: the name is not <date>_<issue padded to four>_<kebab slug>.md.")

    matches = STATUS.findall(FENCE.sub("", path.read_text(encoding="utf-8")))
    if not matches:
        found.append(
            f"{where}: no `**Status:**` field. Open it with one of "
            f"{', '.join(TIMINGS)} -- what a reader needs first is whether the spec "
            "led the work or caught up with it.")
    elif len(matches) > 1:
        found.append(f"{where}: {len(matches)} `**Status:**` fields, and a spec has one.")
    elif not matches[0].startswith(TIMINGS):
        opening = matches[0].split(" · ")[0]
        found.append(
            f"{where}: `**Status:** {opening}` does not open with one of "
            f"{', '.join(TIMINGS)}. Anything after that clause is free text; the "
            "clause itself is the vocabulary.")
    return found


def tracked_files() -> list[str]:
    """Every tracked or untracked-but-not-ignored path, relative to ROOT.

    Git's file set rather than the filesystem's: a plan kept under
    `.git/info/exclude`, which is where CONTRIBUTING.md sends one, is not
    committed and is not this guard's business. Untracked-and-not-ignored is,
    because the hook runs before the commit that would add it.
    """
    listing = subprocess.run(
        ["git", "ls-files", "--cached", "--others", "--exclude-standard"],
        capture_output=True, text=True, check=True, cwd=ROOT)
    paths = listing.stdout.split("\n")[:-1] if listing.stdout else []

    # A tracked path deleted from the working tree is listed and cannot be read.
    return [p for p in paths if (ROOT / p).is_file()]


def findings() -> list[str]:
    tracked = tracked_files()

    plans = sorted(p for p in tracked if p.startswith(PLANS))
    if plans:
        return [
            f"{PLANS}: {len(plans)} file(s), the first {plans[0]!r}. A plan is an "
            "instrument for work that has not started, so it lives beside the work and "
            "is not committed -- the branch it names does not outlive the pull request "
            "that closes its issue. Move it to the scratch directory, or under "
            "`.git/info/exclude`; keep the spec."
        ]

    paths = sorted(ROOT / p for p in tracked if p.startswith(SPECS) and p.endswith(".md"))
    if not paths:
        return [f"{SPECS}: no specs at all, which cannot be right."]
    return [finding for path in paths for finding in status_findings(path)]


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
        print(f"ok  {sum(1 for p in tracked_files() if p.startswith(SPECS))} specs say "
              "when they were written, and no plan is committed")
    return 1 if found else 0


if __name__ == "__main__":
    raise SystemExit(main())
