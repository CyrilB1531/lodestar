#!/usr/bin/env python3
"""Answer whether a pull request can change what `dotnet format` checks (#997).

The lint job runs `dotnet format Lodestar.slnx --verify-no-changes`, which takes about two and a
half minutes on a hosted runner. It can only find something when C# can have changed: a `.cs` or
`.csproj` file, or a file it reads its rules or its project graph from, which is an `.editorconfig`,
a `.globalconfig`, a `.props` or `.targets` file, or the solution. A pull request touching none of
those skips the step.

The answer is ``true`` when any path has one of those names, a rename counting both its names and a
deletion its old one. An empty list also gives ``true``: a pull request whose files could not be
listed runs the check, never skips it.

Usage:  gh api repos/OWNER/REPO/pulls/N/files --paginate \\
            --jq '.[] | .filename, (.previous_filename // empty)' \\
          | python tools/format_needed.py
Prints ``true`` or ``false``; exits 0 either way.
"""

from __future__ import annotations

import sys
from collections.abc import Iterable

FORMAT_INPUTS = (".cs", ".csproj", ".props", ".targets", ".slnx", ".editorconfig", ".globalconfig")


def format_needed(paths: Iterable[str]) -> bool:
    """Whether any non-blank path is C# or configures it, or no path was listed at all."""
    listed = [path.strip() for path in paths if path.strip()]
    return not listed or any(path.endswith(FORMAT_INPUTS) for path in listed)


def main() -> int:
    print("true" if format_needed(sys.stdin) else "false")
    return 0


if __name__ == "__main__":
    raise SystemExit(main())
