#!/usr/bin/env python3
"""Refuse a docs/decisions/index.yaml that has drifted from the records (#630).

The index is the one place a reader learns that a decision was later amended,
because an ADR is immutable and cannot be edited to say so. It is generated from
every record's frontmatter by `tools/regen_adr_index.py` -- and a generated file
nobody regenerates is worse than no file, because it is believed.

So the comparison is the whole text, byte for byte: regenerate in memory, compare
against what is committed, and print the first lines that differ. That is stricter
than comparing parsed structures and it is the right strictness -- the emitter is
deterministic, so any difference at all means the file was hand-edited or left
behind by a new record.

`#586`, `#597` and `#610` are the same bug in a workflow, a README and a release
list: a hand-maintained copy of something derivable, true when written. This is the
index's version of it, caught on the commit rather than by a reader citing a
decision that moved.

Standard library only: `.githooks/pre-commit` runs this through whichever of
`python3` or `python` a contributor's machine resolves, so an import outside the
standard library would turn a missing package into a failed commit.

Usage:  python tools/check_adr_index_sync.py
        python tools/check_adr_index_sync.py --help

Exit:   0 clean, 1 findings printed, 2 bad usage
"""

from __future__ import annotations

import difflib
import pathlib
import sys

# PYTHONSAFEPATH=1 keeps this script's own directory off sys.path, the way
# generate_oracles.py documents -- appended, so nothing here shadows a package.
sys.path.append(str(pathlib.Path(__file__).resolve().parent))

import adr_index  # noqa: E402

# Enough of a diff to name what moved, not the whole file on a wholesale rewrite.
MAX_DIFF_LINES = 20

REMEDY = ("Run `python tools/regen_adr_index.py`. The index is generated from every "
          "record's frontmatter and is never edited by hand.")


def findings() -> list[str]:
    try:
        generated = adr_index.generate()
    except adr_index.AdrError as error:
        return [f"docs/decisions/: {error}",
                "The index cannot be generated, so it cannot be compared. "
                "tools/check_adr_frontmatter.py names the record."]

    if not adr_index.INDEX.exists():
        return ["docs/decisions/index.yaml: missing. " + REMEDY]

    committed = adr_index.INDEX.read_text(encoding="utf-8")
    if committed == generated:
        return []

    diff = list(difflib.unified_diff(
        committed.splitlines(), generated.splitlines(),
        fromfile="docs/decisions/index.yaml (committed)",
        tofile="docs/decisions/index.yaml (generated)", lineterm=""))
    shown = diff[:MAX_DIFF_LINES]
    if len(diff) > MAX_DIFF_LINES:
        shown.append(f"... {len(diff) - MAX_DIFF_LINES} further diff line(s)")

    return ["docs/decisions/index.yaml has drifted from the records it is generated from.",
            *shown, REMEDY]


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
        print(f"ok  docs/decisions/index.yaml matches the "
              f"{len(adr_index.adr_paths())} records it is generated from")
    return 1 if found else 0


if __name__ == "__main__":
    raise SystemExit(main())
