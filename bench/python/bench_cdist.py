#!/usr/bin/env python3
"""Time rapidfuzz's score matrix against Lodestar.Fuzzy's (issue #1123).

Methodology is mirrored by the C# harness (bench/Lodestar.Text.Benchmarks,
`compare-cdist` mode) so the two are comparable:

  * same shapes, same operations, in the same order,
  * the same phrases, built by the same rule on both sides rather than read from a
    corpus file -- a committed corpus here would be a file holding three words and
    an index per row,
  * metric, auto-scaling and best-of-N: `wallcpu.measure`, shared rather than copied
    from the harnesses next door, which is what the duplication gate asked for.

Two parameters are pinned away from the reference's defaults, because leaving them
would compare different work rather than two implementations of the same work:

  * `dtype=np.float64`, since the default is `np.float32` and Lodestar.Fuzzy scores
    in `double` throughout (docs/equivalence.md),
  * `workers=1`, since Lodestar.Fuzzy does not parallelise -- and at these sizes the
    reference's own thread pool is measured slower than one thread anyway.
"""

from __future__ import annotations

import sys
from importlib.metadata import version
from pathlib import Path

import numpy as np
from rapidfuzz import fuzz, process

sys.path.insert(0, str(Path(__file__).resolve().parent))

from wallcpu import measure, write  # noqa: E402

ROOT = Path(__file__).resolve().parent.parent
OUT = ROOT / "results" / "python-cdist.json"

SIZES = [50, 200, 500]

WORDS = [
    "new", "york", "boston", "atlanta", "brooklyn", "angeles", "lakers", "mets", "braves",
    "red", "sox", "knicks",
]


def phrases(count: int, offset: int) -> list[str]:
    """Three words and an index, from the row index alone. The C# harness's rule.

    The trailing index keeps every phrase distinct, so no pair scores 100 by
    accident and the scorer does the same work on both sides.
    """
    out = []
    for i in range(count):
        k = i + offset
        out.append(
            f"{WORDS[k % len(WORDS)]} {WORDS[(k * 5) % len(WORDS)]} "
            f"{WORDS[(k * 7) % len(WORDS)]} {i}"
        )
    return out


def varied(count: int, offset: int) -> list[str]:
    """One to six words and an index, from the row index alone. The C# harness's rule.

    The spread is the point: a length bound rejects a pair only where the lengths
    differ, and the three-word rule above holds every phrase to within a word of
    every other (#1134).
    """
    out = []
    for i in range(count):
        k = i + offset
        words = 1 + (k * 3) % 6
        out.append(
            "".join(f"{WORDS[(k * (w + 1)) % len(WORDS)]} " for w in range(words)) + str(i)
        )
    return out


def measure_size(n: int) -> list[dict]:
    queries = phrases(n, 0)
    choices = phrases(n, 7)
    wide_queries = varied(n, 0)
    wide_choices = varied(n, 7)
    suffix = f"n{n}"
    return [
        measure(
            f"cdist_ratio_{suffix}",
            lambda: process.cdist(queries, choices, dtype=np.float64, workers=1),
        ),
        measure(
            f"cdist_wratio_{suffix}",
            lambda: process.cdist(
                queries, choices, scorer=fuzz.WRatio, dtype=np.float64, workers=1),
        ),
        # A cutoff prunes on both sides, so the pair above is only half the comparison,
        # and three-word phrases give either side little to prune.
        measure(
            f"cdist_ratio_varied_{suffix}",
            lambda: process.cdist(wide_queries, wide_choices, dtype=np.float64, workers=1),
        ),
        measure(
            f"cdist_ratio_varied_cutoff90_{suffix}",
            lambda: process.cdist(
                wide_queries, wide_choices, score_cutoff=90, dtype=np.float64, workers=1),
        ),
    ]


def main() -> None:
    print("Python cdist bench")
    results: list[dict] = []
    for n in SIZES:
        results.extend(measure_size(n))

    write(OUT, results, {"rapidfuzz": version("rapidfuzz"), "numpy": version("numpy")})


if __name__ == "__main__":
    main()
