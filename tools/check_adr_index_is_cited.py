#!/usr/bin/env python3
"""Refuse to lose the sentence that sends a reader to the ADR index (#630).

`docs/decisions/index.yaml` solves nothing on its own. The failure it exists for is
a reader citing a decision that was later amended -- `0101` without `0103`, which
says the opposite -- and that reader is not looking for an index. The two documents
that tell somebody how to work here have to send them, or the index is a file that
is correct and unread.

So this holds `CLAUDE.md` and `CONTRIBUTING.md` to a paragraph that names the index
and both reverse edges. Not an exact sentence: rewording prose is allowed and
normal, and a guard that forbids it gets deleted. What cannot go missing is the
substance -- where to look, and what to follow when you get there, which is both
edges and not just the obvious one. `amended_by` says the decision changed;
`applied_by` says it was used again unchanged, and a reader who follows only the
first reads `0095` as never revisited when `0097` and `0098` both exercise it.

The same shape as `tools/check_claude_md_packages.py` and
`tools/check_readme_pack_loop.py`: prose that is load-bearing is asserted, because
every hand-written list in this repository that was true when written has since
drifted (#586, #597, #610).

Standard library only: `.githooks/pre-commit` runs this through whichever of
`python3` or `python` a contributor's machine resolves, so an import outside the
standard library would turn a missing package into a failed commit.

Usage:  python tools/check_adr_index_is_cited.py
        python tools/check_adr_index_is_cited.py --help

Exit:   0 clean, 1 findings printed, 2 bad usage
"""

from __future__ import annotations

import pathlib
import re
import sys

ROOT = pathlib.Path(__file__).resolve().parent.parent
DOCUMENTS = ("CLAUDE.md", "CONTRIBUTING.md")

# What a paragraph has to carry: the index, and both edges a reader follows from it.
INDEX_PATH = "docs/decisions/index.yaml"
EDGES = ("amended_by", "applied_by")

PARAGRAPH = re.compile(r"\n\s*\n")


def paragraphs(text: str) -> list[str]:
    """The document's paragraphs, which is the unit the substance has to fit in.

    A sentence naming the index fifty lines from one naming the edges is two
    statements a reader meets separately, and the one that matters is whichever
    they find first. One paragraph is what makes it a single instruction.
    """
    return PARAGRAPH.split(text)


def findings_for(name: str, text: str) -> list[str]:
    """One document, against the paragraph it has to carry."""
    citing = [block for block in paragraphs(text) if INDEX_PATH in block]
    if not citing:
        return [
            f"{name}: no paragraph names `{INDEX_PATH}`. It is where a reader learns that "
            "a decision was amended -- an ADR is immutable and cannot say so itself -- so "
            "this document has to send them there."
        ]

    complete = [block for block in citing if all(edge in block for edge in EDGES)]
    if complete:
        return []

    missing = sorted({edge for edge in EDGES
                      if not any(edge in block for block in citing)})
    return [
        f"{name}: names `{INDEX_PATH}` but no one paragraph also names "
        f"{', '.join(f'`{edge}`' for edge in EDGES)}"
        + (f" (absent: {', '.join(missing)})" if missing else "")
        + ". Both edges or neither: `amended_by` says the decision changed, `applied_by` "
        "says it was used again unchanged, and a reader following one is told half of it."
    ]


def findings() -> list[str]:
    found = []
    for name in DOCUMENTS:
        path = ROOT / name
        if not path.exists():
            found.append(f"{name}: missing")
            continue
        found += findings_for(name, path.read_text(encoding="utf-8"))
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
        print(f"ok  {' and '.join(DOCUMENTS)} both send a reader to {INDEX_PATH} "
              f"and to {' and '.join(EDGES)}")
    return 1 if found else 0


if __name__ == "__main__":
    raise SystemExit(main())
