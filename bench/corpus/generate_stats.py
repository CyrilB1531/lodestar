#!/usr/bin/env python3
"""Generate the benchmark corpus for Lodestar.Stats and Lodestar.Stats.Regression (issue #595).

Written rather than committed, like bench/corpus/metrics: both language sides read
these same files, which is what makes the comparison mean anything, and the bytes do
not need to be reproducible across machines.

    python bench/corpus/generate_stats.py

One file per sample size, carrying both halves of the comparison. The tests half is
two independent samples plus a contingency table; the regression half is a design of
four regressors and a response built from them plus noise, which is the shape
OlsBenchmarks already measures against Accord so the two tables can be read together.
"""

from __future__ import annotations

import json
import sys
from pathlib import Path

# Standalone script, not a package: puts the repository root on sys.path so the
# import below resolves the way every static analyser expects it to.
sys.path.append(str(Path(__file__).resolve().parents[2]))

from tools.seeded_random import SeededRandom  # noqa: E402

SEED = 595
SIZES = [1_000, 10_000, 100_000]
REGRESSORS = 4

# long-comment: a corrected measurement error, kept so it is not reintroduced.
# Chi-square costs what the table's SHAPE costs, not what its counts hold: the test
# reads r x c cells whatever numbers are in them. A fixed 4x5 across every sample size
# would therefore measure the same work three times and report it as three rows --
# which is what the first run of this harness did, at an implausible 680x.
TABLE_SHAPES = {1_000: (4, 5), 10_000: (12, 15), 100_000: (40, 50)}

OUT = Path(__file__).resolve().parent / "stats"


def two_samples(rng: SeededRandom, n: int) -> tuple[list[float], list[float]]:
    """Two normal samples that differ in both mean and spread.

    Unequal variances on purpose: Welch's t-test is the one being measured, and a
    sample pair it would answer the same way as Student's would not exercise it.
    """
    first = [rng.gauss(0.0, 1.0) for _ in range(n)]
    second = [rng.gauss(0.3, 1.6) for _ in range(n)]
    return first, second


def contingency(rng: SeededRandom, n: int) -> list[list[int]]:
    """A table with no zero cell, so the expected frequencies stay defined."""
    rows, columns = TABLE_SHAPES[n]
    return [[rng.randint(20, 400) for _ in range(columns)] for _ in range(rows)]


def design(rng: SeededRandom, n: int) -> tuple[list[float], list[float]]:
    """A row-major design and the response it explains, with noise on top.

    Row-major with no constant column, which is the shape `OrdinaryLeastSquares.Fit`
    documents and the shape statsmodels needs `add_constant` for. The generator writes
    one array and each side adds its own intercept, rather than writing two.
    """
    rows: list[float] = []
    response: list[float] = []
    for _ in range(n):
        signal = 0.0
        for column in range(REGRESSORS):
            value = rng.random()
            rows.append(value)
            signal += (column + 1) * value
        response.append(signal + (rng.random() - 0.5))
    return rows, response


def write_size(n: int) -> Path:
    # long-comment: names the invariant a later size addition would otherwise break.
    # One generator per size rather than one for the file: a size added later must not
    # move the draws of the sizes already measured, the way generate_metrics.py keeps
    # its regression pair on a stream of its own.
    rng = SeededRandom(SEED + n)
    first, second = two_samples(rng, n)
    rows, response = design(rng, n)
    payload = {
        "sample_size": n,
        "regressors": REGRESSORS,
        "first": first,
        "second": second,
        "table": contingency(rng, n),
        "design": rows,
        "response": response,
    }
    path = OUT / f"stats_n{n}.json"
    path.write_text(json.dumps(payload) + "\n", encoding="utf-8")
    return path


def main() -> None:
    OUT.mkdir(parents=True, exist_ok=True)
    for n in SIZES:
        path = write_size(n)
        size_mb = path.stat().st_size / (1024 * 1024)
        print(f"  {path.name:<22} {size_mb:6.1f} MB")
    print(f"-> {OUT}")


if __name__ == "__main__":
    main()
