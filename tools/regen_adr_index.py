#!/usr/bin/env python3
"""Regenerate docs/decisions/index.yaml from the records themselves (#630).

The index is derived, never written by hand: each record declares `supersedes`,
`amends` and `applies` in its own frontmatter, and this crosses those edges to
give every decision the reverse ones it cannot carry -- an ADR is immutable, so
the decision that gets amended can never be edited to name its amendment.

Run it after adding an ADR or changing one's frontmatter.
`tools/check_adr_index_sync.py` is what fails when somebody forgets.

Idempotent: running it twice writes the same bytes, which is what makes comparing
against the committed file a usable test rather than a diff of formatting.

Usage:  python tools/regen_adr_index.py
        python tools/regen_adr_index.py --check
        python tools/regen_adr_index.py --help

  --check  Print what would change and exit 1 instead of writing. The guard
           wraps this, so a contributor can reach the same answer by hand.
  --help   Print this message to stdout and exit 0.

Exit:   0 written (or already current), 1 --check found drift, 2 bad usage
"""

from __future__ import annotations

import pathlib
import sys

# PYTHONSAFEPATH=1 keeps this script's own directory off sys.path, the way
# generate_oracles.py documents -- appended, so nothing here shadows a package.
sys.path.append(str(pathlib.Path(__file__).resolve().parent))

import adr_index  # noqa: E402


def main(argv: list[str]) -> int:
    arguments = argv[1:]
    if "--help" in arguments or "-h" in arguments:
        print(__doc__)
        return 0
    if arguments not in ([], ["--check"]):
        print(__doc__, file=sys.stderr)
        return 2

    try:
        generated = adr_index.generate()
    except adr_index.AdrError as error:
        print(f"docs/decisions/: {error}", file=sys.stderr)
        return 1

    current = (
        adr_index.INDEX.read_text(encoding="utf-8") if adr_index.INDEX.exists() else None)
    count = generated.count("\n  title: ")

    if current == generated:
        print(f"ok  docs/decisions/index.yaml is current, {count} records")
        return 0

    if "--check" in arguments:
        print("docs/decisions/index.yaml is out of date -- run "
              "`python tools/regen_adr_index.py`")
        return 1

    adr_index.INDEX.write_text(generated, encoding="utf-8")
    print(f"wrote docs/decisions/index.yaml, {count} records")
    return 0


if __name__ == "__main__":
    raise SystemExit(main(sys.argv))
