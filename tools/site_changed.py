#!/usr/bin/env python3
"""Answer whether a pull request could change the documentation site (#1180).

The `site` job in ci.yml builds the DocFX tree and runs DocFX with warnings as
errors, so a broken link fails the pull request instead of the deployment after
the merge. It costs a .NET tool restore and a full render, which a pull request
touching only C# or tests cannot affect: those skip it.

The site reads every page docs/wiki-map.json publishes (all under docs/, plus
each package's README.md and performance.md), the map itself, the two builders,
tools/site/, the DocFX pin and the icon. ci.yml is listed because the job lives
there. An empty list answers true: a pull request whose files could not be
listed is checked, never waved through.

Usage:  gh api repos/OWNER/REPO/pulls/N/files --paginate \\
            --jq '.[] | .filename, (.previous_filename // empty)' \\
          | python tools/site_changed.py
Prints ``true`` or ``false``; exits 0 either way.
"""

from __future__ import annotations

import re
import sys
from collections.abc import Iterable

PREFIXES = ("docs/", "tools/site/")

EXACT = frozenset({
    "tools/build_site.py",
    "tools/build_wiki.py",
    ".config/dotnet-tools.json",
    "assets/icon.png",
    ".github/workflows/ci.yml",
})

PACKAGE_PAGE = re.compile(r"src/[^/]+/(README|performance)\.md")


def site_changed(paths: Iterable[str]) -> bool:
    """Whether any listed path is one the site reads, or nothing is listed at all."""
    listed = [path.strip() for path in paths if path.strip()]
    if not listed:
        return True
    return any(
        path.startswith(PREFIXES) or path in EXACT or PACKAGE_PAGE.fullmatch(path)
        for path in listed
    )


def main() -> int:
    print("true" if site_changed(sys.stdin) else "false")
    return 0


if __name__ == "__main__":
    raise SystemExit(main())
