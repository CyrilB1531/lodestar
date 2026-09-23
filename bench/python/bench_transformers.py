#!/usr/bin/env python3
"""Time scikit-learn's feature transformers against Lodestar.Preprocessing's (issue #1122).

Methodology is mirrored by the C# harness (bench/Lodestar.Text.Benchmarks,
`compare-transformers` mode) so the two are comparable:

  * same shapes, same operations, in the same order,
  * the same matrix, built by the same rule on both sides rather than read from a
    corpus file -- a committed corpus here would be a file holding `exp(i % 97 / 24)`,
  * metric: milliseconds per operation,
  * auto-scaling: repeat until a measurement lasts >= MIN_TIME,
  * report the best (minimum) of REPEATS measurements,
  * elapsed time (perf_counter) and processor time (process_time) together.

The matrix is strictly positive and right-skewed: skewed because that is the column a
power or a quantile map is called on, strictly positive because Box-Cox refuses anything
else, so one matrix serves every row.

Two parameters are pinned away from the reference's defaults, because leaving them would
compare different work rather than two implementations of the same work:

  * `QuantileTransformer(subsample=None)`, since the default draws 10,000 rows from
    numpy's generator and the C# side has no subsample at all (docs/equivalence.md),
  * `KNNImputer` reads a fixed 2,000-row slice at every size, since it is quadratic in
    the rows where every other operation here is linear.
"""

from __future__ import annotations

import json
import platform
from importlib.metadata import version
from pathlib import Path
from time import perf_counter, process_time

import numpy as np
from sklearn.impute import KNNImputer
from sklearn.preprocessing import (
    KBinsDiscretizer,
    LabelEncoder,
    Normalizer,
    PolynomialFeatures,
    PowerTransformer,
    QuantileTransformer,
)

MIN_TIME = 0.5
REPEATS = 5
FEATURES = 4
IMPUTER_ROWS = 2_000

ROOT = Path(__file__).resolve().parent.parent
OUT = ROOT / "results" / "python-transformers.json"

SIZES = [10_000, 100_000]


def matrix_for(n: int) -> np.ndarray:
    """A strictly positive, right-skewed matrix from the row index. The C# harness's rule."""
    flat = np.exp((np.arange(n * FEATURES, dtype=np.int64) % 97) / 24.0)
    return flat.reshape(n, FEATURES)


def gaps_for(n: int) -> np.ndarray:
    """The same rule with one value in twenty missing, which is what an imputer is called on."""
    flat = np.exp((np.arange(n * FEATURES, dtype=np.int64) % 97) / 24.0)
    flat[::20] = np.nan
    return flat.reshape(n, FEATURES)


def labels_for(n: int) -> np.ndarray:
    """Fifty classes from the row index: a label encoder's cost is the distinct count."""
    return np.array([f"c{row % 50}" for row in range(n)])


def measure(operation: str, action) -> dict:
    """Time one operation, recording both elapsed time and processor time."""
    best_wall, cpu_of_best = float("inf"), float("nan")
    for _ in range(REPEATS):
        iters = 1
        while True:
            c0, w0 = process_time(), perf_counter()
            for _ in range(iters):
                action()
            dt = perf_counter() - w0
            cpu = process_time() - c0
            if dt >= MIN_TIME:
                break
            iters *= 2
        wall_ms = dt / iters * 1e3
        if wall_ms < best_wall:
            best_wall, cpu_of_best = wall_ms, cpu / iters * 1e3
    print(f"  {operation:<28} {best_wall:10.3f} ms/op  cpu {cpu_of_best:8.3f} ms/op")
    return {"operation": operation, "ms_per_op": best_wall, "cpu_ms_per_op": cpu_of_best}


def _fitted(rows: np.ndarray, gaps: np.ndarray) -> tuple:
    """The four fitted objects, built once: their fits are measured by their own rows."""
    # NOSONAR S6709: random_state seeds a subsample, and neither call takes one -- the
    # quantile transformer is pinned to subsample=None so the two sides read the same rows.
    return (
        KBinsDiscretizer(  # NOSONAR S6709
            n_bins=5, encode="onehot-dense", quantile_method="averaged_inverted_cdf").fit(rows),
        QuantileTransformer(subsample=None).fit(rows),  # NOSONAR S6709
        PowerTransformer().fit(rows),
        KNNImputer().fit(gaps),
    )


def measure_size(n: int) -> list[dict]:
    rows = matrix_for(n)
    gaps = gaps_for(IMPUTER_ROWS)
    labels = labels_for(n)
    bins, quantiles, power, imputer = _fitted(rows, gaps)
    suffix = f"n{n}"
    return [
        measure(f"normalize_{suffix}", lambda: Normalizer().transform(rows)),
        # The two hyperparameters spelled out rather than defaulted: the C# side takes the
        # same two values, and a default that moved would silently change the comparison.
        measure(
            f"polynomial_{suffix}",
            lambda: PolynomialFeatures(degree=2, interaction_only=False).fit_transform(rows),
        ),
        measure(
            f"kbins_fit_{suffix}",
            lambda: KBinsDiscretizer(  # NOSONAR S6709: as above, nothing is subsampled
                n_bins=5,
                encode="onehot-dense",
                quantile_method="averaged_inverted_cdf",
            ).fit(rows),
        ),
        measure(f"kbins_{suffix}", lambda: bins.transform(rows)),
        measure(
            f"quantile_fit_{suffix}",
            lambda: QuantileTransformer(subsample=None).fit(rows),  # NOSONAR S6709
        ),
        measure(f"quantile_{suffix}", lambda: quantiles.transform(rows)),
        measure(f"power_fit_{suffix}", lambda: PowerTransformer().fit(rows)),
        measure(f"power_{suffix}", lambda: power.transform(rows)),
        measure(f"knn_impute_{suffix}", lambda: imputer.transform(gaps)),
        measure(f"label_{suffix}", lambda: LabelEncoder().fit_transform(labels)),
    ]


def payload_for(results: list[dict]) -> dict:
    return {
        "metadata": {
            "side": "python",
            "libraries": {
                "scikit-learn": version("scikit-learn"),
                "numpy": version("numpy"),
            },
            "python": platform.python_version(),
            "machine": platform.machine(),
            "min_time_s": MIN_TIME,
            "repeats": REPEATS,
        },
        "results": results,
    }


def main() -> None:
    print("Python transformers bench")
    results: list[dict] = []
    for n in SIZES:
        results.extend(measure_size(n))

    OUT.parent.mkdir(parents=True, exist_ok=True)
    OUT.write_text(json.dumps(payload_for(results), indent=2) + "\n", encoding="utf-8")
    print(f"-> {OUT}")


if __name__ == "__main__":
    main()
