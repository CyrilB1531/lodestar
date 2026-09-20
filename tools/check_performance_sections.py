#!/usr/bin/env python3
"""Assert the shape of docs/guides/performance.md: one comparison per capability.

The guide's subject is what was measured — this library against the library a reader would
otherwise reach for, with that library's version, the machine and the window. A before/after of
this repository's own code is an argument about one commit and belongs in the pull request that
made it, so it must not come back here.

Four rules, over the guide alone:

1. every `##` names a package under `src/`, or is one of the headings in `FIXED_SECTIONS`;
2. every `###` carries a machine and a window, named rather than inherited from a neighbour;
3. every `###` names an incumbent from `INCUMBENTS`, or is listed in `EXEMPT` with its reason;
4. no table column is named after a branch or a revision — `before`, `after`, `main`, `fix`,
   `origin/main`, `this branch`, `A1 / A2` and the rest of `BRANCH_COLUMNS`.

Offline and instant. Exit 0 when the guide holds, 1 with one line per breach otherwise.
"""

from __future__ import annotations

import re
import sys
from pathlib import Path

GUIDE = Path("docs/guides/performance.md")
SRC = Path("src")

# Sections that are not a package's comparisons. Anything else at `##` is a package id.
FIXED_SECTIONS = ("How to read a row",)

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
HEADING = re.compile(r"^(#{2,3}) (.+)$")


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


def branch_breaches(number: int, body: list[str]) -> list[str]:
    return [
        f"{GUIDE}:{number + 1 + offset}: column '{cell}' is a branch or a revision; a "
        "before/after belongs in the pull request that made the change"
        for offset, cell in branch_columns(body)
    ]


def package_breaches(number: int, text: str, known: set[str]) -> list[str]:
    if text in known or text in FIXED_SECTIONS:
        return []
    return [
        f"{GUIDE}:{number}: '## {text}' is neither a package under src/ nor one of "
        f"{', '.join(FIXED_SECTIONS)}"
    ]


def capability_breaches(number: int, text: str, body: list[str]) -> list[str]:
    """Rules 2 and 3, over one `###`: a named machine, a named window, an incumbent."""
    joined = "\n".join(body)
    breaches = []
    if not MACHINE.search(joined):
        breaches.append(f"{GUIDE}:{number}: '{text}' names no machine")
    if not WINDOW.search(joined):
        breaches.append(f"{GUIDE}:{number}: '{text}' names no window")
    if text not in EXEMPT and not any(name in joined for name in INCUMBENTS):
        breaches.append(
            f"{GUIDE}:{number}: '{text}' names no incumbent and is not in EXEMPT — "
            "a section here compares this library to the one a reader would otherwise use"
        )
    return breaches


def check(lines: list[str], known: set[str]) -> list[str]:
    first = next((number for number, _, _, _ in sections(lines)), len(lines) + 1)
    breaches = branch_breaches(0, lines[:first - 1])  # the preamble, which has no heading
    package = None
    for number, level, text, body in sections(lines):
        breaches += branch_breaches(number, body)  # rule 4, wherever the table sits
        if level == "##":
            package = text
            breaches += package_breaches(number, text, known)
        elif package is None or package in FIXED_SECTIONS:
            breaches.append(f"{GUIDE}:{number}: '### {text}' does not sit under a package")
        else:
            breaches += capability_breaches(number, text, body)
    return breaches


def main() -> int:
    if not GUIDE.exists():
        print(f"{GUIDE}: not found — run from the repository root", file=sys.stderr)
        return 1
    breaches = check(GUIDE.read_text(encoding="utf-8").split("\n"), packages())
    for breach in breaches:
        print(breach)
    if breaches:
        print(f"\n{len(breaches)} breach(es) of the performance guide's shape.", file=sys.stderr)
        return 1
    print(f"{GUIDE}: every comparison names an incumbent, a machine and a window.")
    return 0


if __name__ == "__main__":
    sys.exit(main())
