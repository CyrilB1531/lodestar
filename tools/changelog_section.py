#!/usr/bin/env python3
"""Print the CHANGELOG.md section for one package release, for a GitHub Release body (#626).

Measured on 2026-09-11: 35 release tags, 4 GitHub Releases. `release.yml` parses the tag,
checks it against the declared version, tests, packs and pushes -- and never creates a
Release. The four that exist were made by hand on one day in September. So the Releases page
shows nothing of the 0.6.0 milestone, and the only way to see what a version contains is to
read CHANGELOG.md and match headings by hand.

The notes are already written. CHANGELOG.md carries a `### <Package> -- <Version>` heading per
release, and this prints the body under one so the workflow can hand it to `gh release create`.

A section runs from its own heading to the next heading at the same level or above -- the next
`### `, or the `## Released -- <date>` that opens the following release block. Anything deeper
(`#### Added`, `#### Fixed`) belongs to the section and is kept.

**A missing section is an error, not an empty body.** CONTRIBUTING.md's definition of done makes
the changelog entry item 7 and says in as many words that nothing gates it, "precisely because
four lots shipped without it". A Release published with a blank body is that same silence, one
step further downstream, so this refuses instead and the caller decides.

Usage:  python tools/changelog_section.py <Package> <Version>
        python tools/changelog_section.py --list
        python tools/changelog_section.py --help

  --list   Print every `<Package> <Version>` the changelog carries, one per line, and exit 0.
           What the backfill reads to find what it can write.

Exit:   0 printed, 1 no such section, 2 bad usage
"""

from __future__ import annotations

import pathlib
import re
import sys

ROOT = pathlib.Path(__file__).resolve().parent.parent
CHANGELOG = ROOT / "CHANGELOG.md"

# `### Lodestar.Text — 0.6.0`, with the em dash the file actually uses. `DataNet.*` is matched
# too: four tags predate the rename and are part of the record.
HEADING = re.compile(
    r"^### ((?:Lodestar|DataNet)\.[A-Za-z.]+) — (\d[^\s]*)\s*$", re.MULTILINE)
# Any heading at `###` or above ends a section; `####` and deeper are part of it.
BOUNDARY = re.compile(r"^#{1,3} ", re.MULTILINE)


def sections() -> dict[tuple[str, str], str]:
    """Every release section the changelog carries, keyed by package and version."""
    text = CHANGELOG.read_text(encoding="utf-8")
    found = {}
    for match in HEADING.finditer(text):
        start = match.end()
        nxt = BOUNDARY.search(text, start)
        found[(match.group(1), match.group(2))] = text[start:nxt.start() if nxt else len(text)].strip()
    return found


def main() -> int:
    args = sys.argv[1:]
    if args and args[0] in ("--help", "-h"):
        print(__doc__)
        return 0
    if not CHANGELOG.exists():
        print(f"{CHANGELOG.relative_to(ROOT)}: missing", file=sys.stderr)
        return 1

    found = sections()
    if args == ["--list"]:
        for package, version in sorted(found):
            print(f"{package} {version}")
        return 0
    if len(args) != 2:
        print(__doc__)
        return 2

    package, version = args
    body = found.get((package, version))
    if body is None:
        print(
            f"CHANGELOG.md carries no `### {package} — {version}` section. The release notes are "
            "the changelog entry, so write it before tagging -- CONTRIBUTING.md's definition of "
            "done, item 7.", file=sys.stderr)
        return 1

    print(body)
    return 0


if __name__ == "__main__":
    raise SystemExit(main())
