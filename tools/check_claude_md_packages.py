#!/usr/bin/env python3
"""Refuse a CLAUDE.md architecture table that has drifted from src/ (#597).

CLAUDE.md's own "Where a fact belongs" table gives its subject as "what a session needs
to be productive, and the traps that cost time". A package table that under-reports what
src/ holds misleads exactly the reader it is written for, and it did: on 2026-09-10 the
section opened with "Eight independently versioned packages" and listed eight, while src/
held fifteen. The sentence after it named four inter-package edges where
check_nuspec_dependencies.py's EXPECTED carried ten.

Three facts are checked, each against the thing that owns it:

  the row set   against the directories under src/, which is what a package is;
  the count     against that same set, spelled as a word so the prose cannot drift
                from its own table;
  the edge count against EXPECTED in check_nuspec_dependencies.py, which is the
                authority the nuspec gate already asserts against the packed .nuspec.

What is deliberately *not* checked is the "Holds" column. A one-line description of a
package is prose, and a guard that demanded it match anything would either be trivially
satisfied or would stop the sentence being useful.

Usage:  python tools/check_claude_md_packages.py
        python tools/check_claude_md_packages.py --help

Exit:   0 clean, 1 findings printed, 2 bad usage
"""

from __future__ import annotations

import importlib.util
import pathlib
import re
import sys

ROOT = pathlib.Path(__file__).resolve().parent.parent
CLAUDE = ROOT / "CLAUDE.md"
NUSPEC = ROOT / "tools" / "check_nuspec_dependencies.py"

ROW = re.compile(r"^\| `(Lodestar\.[A-Za-z.]+)` \| (core|satellite|interop) \|", re.MULTILINE)
COUNT = re.compile(r"^(\w+) independently versioned packages under `src/`", re.MULTILINE)
EDGES = re.compile(r"The edges: \*\*(\w+)\*\*, all asserted", re.MULTILINE)

# Enough to spell the counts this repository can reach; a sixteenth package adds a word.
WORDS = {
    8: "Eight", 9: "Nine", 10: "Ten", 11: "Eleven", 12: "Twelve", 13: "Thirteen",
    14: "Fourteen", 15: "Fifteen", 16: "Sixteen", 17: "Seventeen", 18: "Eighteen",
    19: "Nineteen", 20: "Twenty",
}
LOWER = {n: w.lower() for n, w in WORDS.items()}


def label(path: pathlib.Path) -> str:
    """A repository-relative path, so a finding is the same on every machine."""
    try:
        return path.relative_to(ROOT).as_posix()
    except ValueError:
        # A path outside the tree, which only the unit tests hand this. Reporting it
        # whole beats raising out of a function whose job is to format a message.
        return path.as_posix()


def source_packages() -> set[str]:
    """A package is a directory under src/ that carries a csproj of its own name."""
    return {
        d.name for d in (ROOT / "src").iterdir()
        if d.is_dir() and (d / f"{d.name}.csproj").exists()
    }


def expected_edges() -> int:
    """Distinct Lodestar-to-Lodestar edges in the nuspec gate's EXPECTED map."""
    spec = importlib.util.spec_from_file_location("_nuspec", NUSPEC)
    module = importlib.util.module_from_spec(spec)
    spec.loader.exec_module(module)

    edges = {
        (package, dependency)
        for package, frameworks in module.EXPECTED.items()
        for dependencies in frameworks.values()
        for dependency in dependencies
        if dependency.startswith("Lodestar.")
    }
    return len(edges)


def findings() -> list[str]:
    if not CLAUDE.exists():
        return [f"{label(CLAUDE)}: missing"]

    text = CLAUDE.read_text(encoding="utf-8")
    result = []

    listed = {name for name, _ in ROW.findall(text)}
    actual = source_packages()
    if missing := sorted(actual - listed):
        result.append(
            f"{label(CLAUDE)}: the architecture table has no row for "
            f"{', '.join(missing)}, which src/ holds.")
    if extra := sorted(listed - actual):
        result.append(
            f"{label(CLAUDE)}: the architecture table lists {', '.join(extra)}, "
            "which src/ does not hold.")

    count = COUNT.search(text)
    if count is None:
        result.append(
            f"{label(CLAUDE)}: no 'N independently versioned packages under `src/`' "
            "sentence to check the table against.")
    elif count.group(1) != WORDS.get(len(actual)):
        result.append(
            f"{label(CLAUDE)}: the prose says {count.group(1)} packages and src/ holds "
            f"{len(actual)} — {WORDS.get(len(actual), len(actual))}.")

    edges = EDGES.search(text)
    real = expected_edges()
    if edges is None:
        result.append(
            f"{label(CLAUDE)}: no 'The edges: **N**, all asserted' sentence to check "
            f"against {label(NUSPEC)}'s EXPECTED.")
    elif edges.group(1) != LOWER.get(real):
        result.append(
            f"{label(CLAUDE)}: the prose says {edges.group(1)} inter-package edges and "
            f"{label(NUSPEC)}'s EXPECTED carries {real} — {LOWER.get(real, real)}.")

    return result


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
        print(f"ok  CLAUDE.md's table names the {len(source_packages())} packages src/ "
              f"holds, and the {expected_edges()} edges EXPECTED carries")
    return 1 if found else 0


if __name__ == "__main__":
    raise SystemExit(main())
