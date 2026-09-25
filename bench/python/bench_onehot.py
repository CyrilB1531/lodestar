#!/usr/bin/env python3
"""Time scikit-learn's sparse OneHotEncoder against Lodestar.Preprocessing's (issue #1161).

Methodology is mirrored by the C# harness (bench/Lodestar.Text.Benchmarks, `compare-onehot` mode):

  * the same integer categories on both sides, built from the row and column index by one formula
    whose counts are skewed, so a few hundred categories are frequent and the tail is not,
  * the same operation: fit, then the sparse transform of the fitted rows, with and without
    min_frequency and max_categories grouping the tail,
  * metric, auto-scaling and best-of-N: `wallcpu.measure`, shared with the harnesses next door.
"""

from __future__ import annotations

import sys
import warnings
from importlib.metadata import version
from pathlib import Path

import numpy as np
from sklearn.preprocessing import OneHotEncoder

sys.path.insert(0, str(Path(__file__).resolve().parent))

from wallcpu import measure, write  # noqa: E402

ROOT = Path(__file__).resolve().parent.parent
OUT = ROOT / "results" / "python-onehot.json"

FEATURES = 3
SIZES = [10_000, 100_000]


def categories(n: int) -> np.ndarray:
    """The C# side's formula: a hashed index folded twice, so small codes are far more frequent."""
    i = np.arange(n, dtype=np.int64)[:, None]
    j = np.arange(FEATURES, dtype=np.int64)[None, :]
    spread = (i * 7919 + j * 104729) % 9973
    return spread % (1 + (i * 31 + j) % 997)


def main() -> None:
    warnings.simplefilter("ignore")
    print("Python one-hot bench")
    results: list[dict] = []
    for n in SIZES:
        x = categories(n)
        results.append(measure(f"sparse_{n}", lambda x=x: OneHotEncoder().fit(x).transform(x)))
        results.append(measure(f"sparse_infrequent_{n}", lambda x=x: OneHotEncoder(
            min_frequency=5, max_categories=200).fit(x).transform(x)))
    write(OUT, results, {"scikit-learn": version("scikit-learn"), "numpy": version("numpy")})


if __name__ == "__main__":
    main()
