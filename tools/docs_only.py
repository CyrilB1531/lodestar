#!/usr/bin/env python3
"""Answer whether a pull request changes nothing but Markdown (#857).

CI runs a reduced path for such a pull request: the lint, the snippets, the stop-word
check and the documentation tests, which read ``docs/**/*.md`` and so can fail on a
Markdown change alone (#859's missing link was caught there). The build, Sonar, the
sample, the oracles and Windows cannot see a ``.md`` file and are skipped.

The answer is ``true`` only when every path ends in ``.md``, a rename counting both
its names and a deletion its old one. Any other file, an image or a JSON map read by
the tests included, makes it ``false``, and so does an empty list: a pull request whose
files could not be listed takes the full path, never the reduced one.

Usage:  gh api repos/OWNER/REPO/pulls/N/files --paginate \\
            --jq '.[] | .filename, (.previous_filename // empty)' \\
          | python tools/docs_only.py
Prints ``true`` or ``false``; exits 0 either way.
"""

from __future__ import annotations

import sys
from collections.abc import Iterable


def docs_only(paths: Iterable[str]) -> bool:
    """Whether every non-blank path is a Markdown file, and there is at least one."""
    listed = [path.strip() for path in paths if path.strip()]
    return bool(listed) and all(path.endswith(".md") for path in listed)


def main() -> int:
    print("true" if docs_only(sys.stdin) else "false")
    return 0


if __name__ == "__main__":
    raise SystemExit(main())
