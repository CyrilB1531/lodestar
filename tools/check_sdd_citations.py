#!/usr/bin/env python3
"""Refuse a file that cites a report from a plan's workspace, which no reader can open (#730).

The subagent-driven-development skill writes each task's brief and report into a
per-plan directory under .superpowers/, which .git/info/exclude keeps out of the
repository and which is deleted when the plan finishes. A comment citing
`task-8-report.md` as its evidence is therefore dangling from the commit that
writes it: seventeen comments and one ADR carried such a citation before this
looked for them, every one of them pasted while the report still existed.

A comment carries what would check its claim (CONTRIBUTING.md, *Claims in
comments*), so the fix is never to delete the claim -- it is to point at
something tracked: the test that pins it, the oracle case, the commit whose
message holds the measurement, or the issue.

Exempt, and nothing else:

- docs/superpowers/, where plans describe that workspace while it exists.
- .claude/skills/, the vendored skills that create it.
- ADR 0082, which cites a report and cannot be edited (check_adr_immutable.py);
  a new ADR is not exempt. docs/decisions/README.md's row for 0082 names the
  commit holding the same finding.
- This module and its test, which contain the pattern they search for.

Usage:  python tools/check_sdd_citations.py
        python tools/check_sdd_citations.py --help

Exit:   0 clean, 1 findings printed, 2 bad usage
"""

from __future__ import annotations

import pathlib
import re
import subprocess
import sys

ROOT = pathlib.Path(__file__).resolve().parent.parent

# A report or brief named by its task number, or any path into the workspace. Digits are
# required, so a placeholder like task-N-report.md in prose describing the layout is not one.
CITATION = re.compile(r"task-\d+-(?:report|brief)\.md|\.superpowers[/\\]sdd")

EXEMPT_PREFIXES = ("docs/superpowers/", ".claude/skills/")

EXEMPT_FILES = frozenset({
    "tools/check_sdd_citations.py",
    "tools/tests/test_check_sdd_citations.py",
    "docs/decisions/0082-scipy-joins-the-allowed-permissive-references.md",
})


def is_exempt(path: str) -> bool:
    """Whether a repository-relative, forward-slashed path is outside this guard's scope."""
    return path in EXEMPT_FILES or path.startswith(EXEMPT_PREFIXES)


def scan_text(text: str) -> list[tuple[int, str]]:
    """Every (1-based line number, matched text) citation in `text`."""
    return [(text.count("\n", 0, match.start()) + 1, match.group(0))
            for match in CITATION.finditer(text)]


def tracked_files() -> list[str]:
    """Every tracked or untracked-but-not-ignored path, relative to ROOT.

    Untracked too, so the hook catches a citation before it is committed; the
    workspace itself is ignored, so its own reports are never listed here.
    """
    listing = subprocess.run(
        ["git", "ls-files", "--cached", "--others", "--exclude-standard"],
        capture_output=True, text=True, check=True, cwd=ROOT)
    paths = listing.stdout.split("\n")[:-1] if listing.stdout else []

    # A tracked path deleted from the working tree is listed and cannot be read.
    return [p for p in paths if (ROOT / p).is_file()]


def findings(root: pathlib.Path, paths: list[str]) -> list[str]:
    """One `path:line: citation` per match across `paths`, exempt paths skipped."""
    found = []
    for path in paths:
        if is_exempt(path):
            continue
        try:
            text = (root / path).read_text(encoding="utf-8")
        except (OSError, UnicodeDecodeError):
            continue
        found.extend(f"{path}:{line}: {matched}" for line, matched in scan_text(text))
    return found


def main(argv: list[str]) -> int:
    arguments = argv[1:]
    if "--help" in arguments or "-h" in arguments:
        print(__doc__)
        return 0
    if arguments:
        print(__doc__, file=sys.stderr)
        return 2

    found = findings(ROOT, tracked_files())
    for finding in found:
        print(finding)

    if found:
        print(
            f"\n{len(found)} citation(s) of a plan workspace no reader can open. Keep the "
            "claim and point at something tracked instead: the test, the oracle case, "
            "the commit or the issue.",
            file=sys.stderr)
        return 1
    return 0


if __name__ == "__main__":
    sys.exit(main(sys.argv))
