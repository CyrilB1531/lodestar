#!/usr/bin/env python3
"""Print the CHANGELOG.md section for one package release, for a GitHub Release body (#626).

Measured on 2026-09-11: 35 release tags, 4 GitHub Releases. `release.yml` parses the tag,
checks it against the declared version, tests, packs and pushes -- and never creates a
Release. The four that exist were made by hand on one day in September. So the Releases page
shows nothing of the 0.6.0 milestone, and the only way to see what a version contains is to
read CHANGELOG.md and match headings by hand.

The notes are already written. Each package keeps its own `src/<Package>/CHANGELOG.md` since
#1133, with a `## [<Version>] -- <date>` heading per release, and this prints the body under one
so the workflow can hand it to `gh release create`. A release published under the pre-rename name
is headed `(published as DataNet.X)` in its Lodestar file, and is found under that name too, since
its tag is `DataNet.X/v<Version>`.

A section runs from its own heading to the next `## ` or `# ` heading. Anything deeper
(`### Added`, `### Fixed`) belongs to the section and is kept.

**A missing section is an error, not an empty body.** CONTRIBUTING.md's definition of done makes
the changelog entry item 7 and says in as many words that nothing gates it, "precisely because
four lots shipped without it". A Release published with a blank body is that same silence, one
step further downstream, so this refuses instead and the caller decides.

Usage:  python tools/changelog_section.py <Package> <Version>
        python tools/changelog_section.py --list
        python tools/changelog_section.py --help

  --list   Print every `<Package> <Version>` the changelogs carry, one per line, and exit 0.
           What the backfill reads to find what it can write.

Exit:   0 printed, 1 no such section, 2 bad usage
"""

from __future__ import annotations

import pathlib
import re
import sys

ROOT = pathlib.Path(__file__).resolve().parent.parent
SRC = ROOT / "src"

# `## [0.6.0] — 2026-09-10`, with the em dash the files use, and the pre-rename suffix
# `(published as DataNet.Text)` that four tags need to be found under their own name.
RELEASE = re.compile(
    r"^## \[(?P<version>\d[^\]]*)\](?: — \S+)?"
    r"(?: \(published as (?P<as>(?:Lodestar|DataNet)\.[A-Za-z.]+)\))?\s*$", re.MULTILINE)
# A `##` or `#` heading ends a section; `###` and deeper are part of it.
BOUNDARY = re.compile(r"^#{1,2} ", re.MULTILINE)


def sections() -> dict[tuple[str, str], str]:
    """Every release section the package changelogs carry, keyed by published id and version."""
    found = {}
    for path in sorted(SRC.glob("*/CHANGELOG.md")):
        text = path.read_text(encoding="utf-8")
        for match in RELEASE.finditer(text):
            start = match.end()
            nxt = BOUNDARY.search(text, start)
            body = text[start:nxt.start() if nxt else len(text)].strip()
            found[(match.group("as") or path.parent.name, match.group("version"))] = body
    return found


def main() -> int:
    args = sys.argv[1:]
    if args and args[0] in ("--help", "-h"):
        print(__doc__)
        return 0
    if not SRC.is_dir():
        print(f"{SRC}: missing", file=sys.stderr)
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
        home = package.replace("DataNet.", "Lodestar.", 1)
        print(
            f"src/{home}/CHANGELOG.md carries no `## [{version}]` section. The release notes are "
            "the changelog entry, so write it before tagging -- CONTRIBUTING.md's definition of "
            "done, item 7.", file=sys.stderr)
        return 1

    print(body)
    return 0


if __name__ == "__main__":
    raise SystemExit(main())
