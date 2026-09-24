#!/usr/bin/env python3
"""Refuse a project that lacks the documents #1133 gave every project, or an index that misses one.

Each publishable package under src/ carries its own README.md (the README its NuGet package
ships), CHANGELOG.md (the source of its release history) and performance.md (the source of its
measured comparisons). The root README.md, CHANGELOG.md and docs/guides/performance.md are the
indexes that link them, and the root CHANGELOG.md holds no entry of its own. Each project under
samples/ and bench/, tests/ itself, and each test suite (not its netstandard mirror, which
compiles the suite's sources) carries a README.md.

A nineteenth package, sample, benchmark or suite then cannot land without its documents, which is
the drift this exists to catch: the eighteen packages had none of the three until #1133.

Standard library only, offline and instant: the pre-commit hook runs it.

Usage:  python tools/check_project_docs.py
Exit:   0 clean, 1 findings printed, 2 bad usage
"""

from __future__ import annotations

import pathlib
import re
import sys

ROOT = pathlib.Path(__file__).resolve().parent.parent

README = "README.md"
CHANGELOG = "CHANGELOG.md"
PERFORMANCE = "performance.md"
ENCODING = "utf-8"
PACKAGE_DOCUMENTS = (README, CHANGELOG, PERFORMANCE)
# The index that links each package's document, and the document it links.
INDEXES = ((README, README), (CHANGELOG, CHANGELOG), (f"docs/guides/{PERFORMANCE}", PERFORMANCE))
MIRROR = ".NetStandard.Tests"
ENTRY = re.compile(r"^- (?!\[`?Lodestar\.[A-Za-z.]+`?\]\(src/Lodestar\.[A-Za-z.]+/CHANGELOG\.md\)$)")


def packages(root: pathlib.Path) -> list[str]:
    """The publishable packages: the projects under src/ that declare a version."""
    return sorted(p.parent.name for p in root.glob("src/Lodestar.*/Version.props"))


def projects(root: pathlib.Path, area: str) -> list[pathlib.Path]:
    """Every directory directly under an area that holds a project file."""
    return sorted(p for p in (root / area).glob("*") if p.is_dir() and any(p.glob("*.csproj")))


def required(root: pathlib.Path) -> list[pathlib.Path]:
    """Every document the tree must hold, in a stable order."""
    paths = [root / "src" / package / name for package in packages(root) for name in PACKAGE_DOCUMENTS]
    paths += [project / README for area in ("samples", "bench") for project in projects(root, area)]
    paths.append(root / "tests" / README)
    paths += [suite / README for suite in projects(root, "tests") if not suite.name.endswith(MIRROR)]
    return paths


def index_findings(root: pathlib.Path) -> list[str]:
    """An index that does not link a package's document."""
    found = []
    for index, document in INDEXES:
        path = root / index
        text = path.read_text(encoding=ENCODING) if path.exists() else ""
        found += [f"{index}: does not link src/{package}/{document}"
                  for package in packages(root) if f"src/{package}/{document})" not in text]
    return found


def changelog_findings(root: pathlib.Path) -> list[str]:
    """The root changelog holds only links; each package's opens with its title and Unreleased."""
    found = []
    root_changelog = root / CHANGELOG
    if root_changelog.exists():
        for number, line in enumerate(root_changelog.read_text(encoding=ENCODING).splitlines(), start=1):
            if ENTRY.match(line):
                found.append(f"CHANGELOG.md:{number}: an entry; it belongs in its package's "
                             "src/<Package>/CHANGELOG.md")
    for package in packages(root):
        path = root / "src" / package / CHANGELOG
        if not path.exists():
            continue
        lines = path.read_text(encoding=ENCODING).splitlines()
        if not lines or lines[0] != f"# Changelog — {package}":
            found.append(f"src/{package}/CHANGELOG.md:1: must open '# Changelog — {package}'")
        if "## [Unreleased]" not in lines:
            found.append(f"src/{package}/CHANGELOG.md: has no '## [Unreleased]' section")
    return found


def findings(root: pathlib.Path) -> list[str]:
    missing = [f"{path.relative_to(root).as_posix()}: is missing"
               for path in required(root) if not path.exists()]
    return missing + index_findings(root) + changelog_findings(root)


def main(argv: list[str]) -> int:
    if argv:
        print(__doc__)
        return 0 if argv[0] in ("--help", "-h") else 2
    lines = findings(ROOT)
    for line in lines:
        print(line)
    if lines:
        return 1
    print(f"ok  {len(packages(ROOT))} packages carry their README, CHANGELOG and performance page, "
          f"{len(required(ROOT)) - 3 * len(packages(ROOT))} other projects their README, "
          "and the three indexes link them all")
    return 0


if __name__ == "__main__":
    raise SystemExit(main(sys.argv[1:]))
