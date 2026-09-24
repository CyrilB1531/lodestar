#!/usr/bin/env python3
"""Assert the shape of the performance pages: one comparison per capability.

The guide's subject is what was measured — this library against the library a reader would
otherwise reach for, with that library's version, the machine and the window. A before/after of
this repository's own code is an argument about one commit and belongs in the pull request that
made it, so it must not come back here.

Since #1133 each package's comparisons live in `src/<Package>/performance.md`, and
docs/guides/performance.md is the index. Four rules over each package's page:

1. it opens `# Performance — <Package>`, and a page with no `##` says `NONE_YET` instead;
2. every `##` carries a machine and a window, named rather than inherited from a neighbour;
3. every `##` names an incumbent from `INCUMBENTS`, or is listed in `EXEMPT` with its reason;
4. no table column is named after a branch or a revision — `before`, `after`, `main`, `fix`,
   `origin/main`, `this branch`, `A1 / A2` and the rest of `BRANCH_COLUMNS`.

And over the guide: its `##` are `FIXED_SECTIONS` only, it holds no comparison, it links every
package's page, and rule 4.

Offline and instant. Exit 0 when every page holds, 1 with one line per breach otherwise.
"""

from __future__ import annotations

import re
import sys
from pathlib import Path

GUIDE = Path("docs/guides/performance.md")
SRC = Path("src")

# Sections that are not a package's comparisons. Anything else at `##` is a package id.
FIXED_SECTIONS = ("How to read a row", "Packages")

# What a package with no comparison says instead, word for word, so it reads the same everywhere.
NONE_YET = "Nothing is measured against an incumbent yet."

# The libraries a reader arrives holding. A `###` that names none of them is either exempt or
# is not a comparison, which is what this guide is for.
INCUMBENTS = (
    "rapidfuzz",
    "scikit-learn",
    "scipy",
    "statsmodels",
    "numpy",
    "NumPy",
    "Accord",
    "Math.NET",
    "MathNet",
    "NumFlat",
    "Meta.Numerics",
    "ML.NET",
    "Cortex.TimeSeries",
    "LuceneSharp",
    "Fastenshtein",
    "Quickenshtein",
    "F23.StringSimilarity",
    "FuzzySharp",
    "Microsoft.ML.Tokenizers",
    "TensorPrimitives",
    "Aglomera",
    "`Dbscan`",
    "jellyfish",
    "textdistance",
    "lifelines",
    "sentencepiece",
)

# A capability whose comparison is real and has no third-party side. Each carries the reason,
# which is what a reviewer weighs before a fourth entry is added.
EXEMPT = {
    "Lodestar.Gpu — four kernels against their CPU paths (issue #444)":
        "no .NET library exposes these kernels; the alternative is this repository's own CPU path",
    "Batched embedding — what the number is, and what it is not":
        "the section exists to bound the ratio, not to publish one",
    "BK-tree vs a length-filtered scan (issue #526)":
        "no .NET package publishes a BK-tree; the alternative is the scan a caller would write",
}

# Matched as a word inside the header cell, not as the whole cell: the shapes that reached the
# guide spell it out — `main`, A1 / A2; Allocated before; Lodestar after.
BRANCH_COLUMNS = re.compile(
    r"\b(?:before|after|main|fix|branch|revision|unoptimised|a1 ?/ ?a2)\b", re.IGNORECASE
)

# Deliberately strict: "same machine as above" is what the sections carried before #1106, and it
# made one of them point at the wrong machine once the guide was reordered by package.
MACHINE = re.compile(r"Machine:|AMD Ryzen|Intel (?:Core|Xeon)|NVIDIA")
WINDOW = re.compile(r"\b20\d\d-\d\d-\d\d\b|[Ww]indow:")
HEADING = re.compile(r"^(#{1,3}) (.+)$")


def packages() -> set[str]:
    return {p.name for p in SRC.iterdir() if p.is_dir() and p.name.startswith("Lodestar.")}


def heading_text(raw: str) -> str:
    """The heading with its markdown links flattened, the way an anchor reads it."""
    return re.sub(r"\[([^\]]*)\]\([^)]*\)", r"\1", raw).strip()


def sections(lines: list[str]) -> list[tuple[int, str, str, list[str]]]:
    """Every heading as (line number, level, text, body up to the next heading)."""
    found = []
    for number, line in enumerate(lines, start=1):
        match = HEADING.match(line)
        if match:
            found.append((number, match.group(1), heading_text(match.group(2))))
    out = []
    for index, (number, level, text) in enumerate(found):
        end = found[index + 1][0] - 1 if index + 1 < len(found) else len(lines)
        out.append((number, level, text, lines[number:end]))
    return out


def branch_columns(body: list[str]) -> list[tuple[int, str]]:
    """Header cells naming a branch or a revision, with their offset inside the body."""
    hits = []
    for offset, line in enumerate(body):
        if not line.lstrip().startswith("|") or offset + 1 >= len(body):
            continue
        delimiter = body[offset + 1].strip()
        if "-" not in delimiter or set(delimiter) - set("|-: "):
            continue  # not a header row: the next line is not the delimiter
        for cell in line.strip().strip("|").split("|"):
            if BRANCH_COLUMNS.search(cell.replace("`", "")):
                hits.append((offset, cell.strip()))
    return hits


def branch_breaches(where: str, number: int, body: list[str]) -> list[str]:
    return [
        f"{where}:{number + 1 + offset}: column '{cell}' is a branch or a revision; a "
        "before/after belongs in the pull request that made the change"
        for offset, cell in branch_columns(body)
    ]


def capability_breaches(where: str, number: int, text: str, body: list[str]) -> list[str]:
    """Rules 2 and 3, over one comparison: a named machine, a named window, an incumbent."""
    joined = "\n".join(body)
    breaches = []
    if not MACHINE.search(joined):
        breaches.append(f"{where}:{number}: '{text}' names no machine")
    if not WINDOW.search(joined):
        breaches.append(f"{where}:{number}: '{text}' names no window")
    if text not in EXEMPT and not any(name in joined for name in INCUMBENTS):
        breaches.append(
            f"{where}:{number}: '{text}' names no incumbent and is not in EXEMPT — "
            "a section here compares this library to the one a reader would otherwise use"
        )
    return breaches


def check_package(lines: list[str], package: str) -> list[str]:
    """The four rules over one package's page; a `###` belongs to the comparison above it."""
    where = f"{SRC}/{package}/performance.md"
    breaches = []
    if not lines or lines[0] != f"# Performance — {package}":
        breaches.append(f"{where}:1: the page must open '# Performance — {package}'")
    breaches += branch_breaches(where, 0, lines)
    comparisons = [number for number, level, _, _ in sections(lines) if level == "##"]
    if not comparisons and NONE_YET not in "\n".join(lines):
        breaches.append(f"{where}: no comparison, and not the line '{NONE_YET}'")
    for index, number in enumerate(comparisons):
        end = comparisons[index + 1] - 1 if index + 1 < len(comparisons) else len(lines)
        text = heading_text(lines[number - 1][3:])
        breaches += capability_breaches(where, number, text, lines[number:end])
    return breaches


def check_guide(lines: list[str], known: set[str]) -> list[str]:
    """The index: its fixed sections only, no comparison, and a link to every package's page."""
    where = str(GUIDE)
    breaches = branch_breaches(where, 0, lines)
    for number, level, text, _ in sections(lines):
        if level == "###":
            breaches.append(f"{where}:{number}: '### {text}' is a comparison; it belongs in "
                            "its package's src/<Package>/performance.md")
        elif level == "##" and text not in FIXED_SECTIONS:
            breaches.append(
                f"{where}:{number}: '## {text}' is not one of {', '.join(FIXED_SECTIONS)}")
    joined = "\n".join(lines)
    breaches += [f"{where}: no link to src/{package}/performance.md"
                 for package in sorted(known) if f"src/{package}/performance.md)" not in joined]
    return breaches


def main() -> int:
    if not GUIDE.exists():
        print(f"{GUIDE}: not found — run from the repository root", file=sys.stderr)
        return 1
    known = packages()
    breaches = check_guide(GUIDE.read_text(encoding="utf-8").split("\n"), known)
    for package in sorted(known):
        page = SRC / package / "performance.md"
        if page.exists():
            breaches += check_package(page.read_text(encoding="utf-8").split("\n"), package)
        else:
            breaches.append(f"{page}: missing")
    for breach in breaches:
        print(breach)
    if breaches:
        print(f"\n{len(breaches)} breach(es) of the performance pages' shape.", file=sys.stderr)
        return 1
    print(f"{len(known)} performance pages and their index: every comparison names an "
          "incumbent, a machine and a window.")
    return 0


if __name__ == "__main__":
    sys.exit(main())
