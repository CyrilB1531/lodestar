#!/usr/bin/env python3
"""Every documentation suite the docs-only path ran reported at least one test.

The `Lint` job runs the documentation tests by invoking each test assembly directly with
xunit v3's own in-process runner and `-namespace <project>.Documentation` (#997). That runner
**exits 0 when the filter matches nothing**: `-namespace Nope.Documentation` and a one-letter
typo both print `Total: 0` and succeed, so a renamed namespace, a nested `Documentation.<Sub>`
or a renamed test project would leave the only job that tests the docs on a docs-only pull
request green and empty (#1054). CLAUDE.md's "exits 8 and says `Zéro tests exécutés`" is
Microsoft.Testing.Platform's behaviour under `dotnet test`, not this runner's.

So the count is the check, read from the `-xml` result each run writes rather than from its
console summary, which is formatted for a reader.

Usage: check_doc_test_counts.py <directory holding one subdirectory per test project>
"""

from __future__ import annotations

import pathlib
import sys
import xml.etree.ElementTree as ElementTree

RESULTS = "results.xml"


def totals(results: pathlib.Path) -> int:
    """The test count the run recorded, summed over the assemblies in one result file."""
    root = ElementTree.parse(results).getroot()
    return sum(int(assembly.get("total", "0")) for assembly in root.iter("assembly"))


def report(directory: pathlib.Path) -> tuple[list[str], int, int]:
    """One line per project, the failures, and the counts: a missing result file or a suite that ran nothing."""
    problems: list[str] = []
    total = 0
    projects = 0
    for project in sorted(p for p in directory.iterdir() if p.is_dir()):
        projects += 1
        results = project / RESULTS
        if not results.is_file():
            problems.append(f"{project.name}: no {RESULTS}, so the run did not finish")
            continue

        try:
            count = totals(results)
        except ElementTree.ParseError as error:
            # A run killed mid-write leaves a truncated file, which is not a passing suite.
            problems.append(f"{project.name}: {RESULTS} does not parse ({error})")
            continue


        total += count
        print(f"{project.name}: {count} documentation tests")
        if count == 0:
            problems.append(
                f"{project.name}: ran 0 tests. The namespace filter matched nothing, and this "
                "runner reports that as success")

    return problems, total, projects


def main(argv: list[str]) -> int:
    if len(argv) != 2:
        print(__doc__)
        return 2

    directory = pathlib.Path(argv[1])
    if not directory.is_dir():
        print(f"::error::{directory} is not a directory")
        return 1

    problems, total, projects = report(directory)
    print(f"{total} documentation tests over {projects} projects")
    if projects == 0:
        problems.append(f"{directory} holds no test project directory")
    for problem in problems:
        print(f"::error::{problem}")

    return 1 if problems else 0


if __name__ == "__main__":
    sys.exit(main(sys.argv))
