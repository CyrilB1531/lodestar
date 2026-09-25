#!/usr/bin/env python3
"""Time scikit-learn's splitters against Lodestar.Preprocessing's (issue #762).

Methodology is mirrored by the C# harness (bench/Lodestar.Text.Benchmarks,
`compare-splitters` mode) so the two are comparable:

  * same shapes, same operations, in the same order,
  * the same labels, built by the same rule on both sides rather than read from a
    corpus file -- a splitter's whole input is a row count and a label per row, so a
    corpus here would be a file holding `i % 10`,
  * metric: milliseconds per operation,
  * auto-scaling: repeat until a measurement lasts >= MIN_TIME,
  * report the best (minimum) of REPEATS measurements,
  * elapsed time (perf_counter) and processor time (process_time) together.

`split()` is a generator, so it is drained into a list: yielding a fold and building
one are not the same work, and the C# side returns every fold built. `X` is a dummy
column of zeros because scikit-learn takes the matrix to read `len(X)` from it -- it
is allocated outside the timed region, where the C# side passes an integer and
allocates nothing.
"""

from __future__ import annotations

import json
import platform
from importlib.metadata import version
from pathlib import Path
from time import perf_counter, process_time

import numpy as np
from sklearn.model_selection import (GroupKFold, KFold, RepeatedKFold, StratifiedGroupKFold,
                                     StratifiedKFold, TimeSeriesSplit, train_test_split)

MIN_TIME = 0.5
REPEATS = 5
FOLD_COUNT = 5
TEST_FRACTION = 0.25
SEED = 1157
REPEAT_COUNT = 3
ROWS_PER_GROUP = 100

ROOT = Path(__file__).resolve().parent.parent
OUT = ROOT / "results" / "python-splitters.json"

SIZES = [10_000, 100_000, 1_000_000]


def labels_for(n: int) -> np.ndarray:
    """Three classes at 60 / 30 / 10, from the row index alone.

    The same rule as the C# harness. A skewed shape rather than a balanced one
    because that is where a stratified splitter does work a plain one does not.
    """
    remainder = np.arange(n, dtype=np.int64) % 10
    return np.where(remainder < 6, 0, np.where(remainder < 9, 1, 2))


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


def measure_size(n: int) -> list[dict]:
    rows = np.zeros((n, 1), dtype=np.float64)
    labels = labels_for(n)
    # NOSONAR S6709: both are unshuffled, and the reference refuses a random_state without
    # shuffling -- "Setting a random_state has no effect since shuffle is False" is a ValueError.
    kfold = KFold(n_splits=FOLD_COUNT)  # NOSONAR S6709
    stratified = StratifiedKFold(n_splits=FOLD_COUNT)  # NOSONAR S6709
    indices = np.arange(n, dtype=np.int64)
    suffix = f"n{n}"
    return [
        measure(f"kfold_{suffix}", lambda: list(kfold.split(rows))),
        measure(f"stratified_{suffix}", lambda: list(stratified.split(rows, labels))),
        measure(
            f"traintest_{suffix}",
            # NOSONAR S6709: shuffle=False ignores a random_state
            lambda: train_test_split(  # NOSONAR S6709
                indices, test_size=TEST_FRACTION, shuffle=False),
        ),
    ] + measure_seeded(n, rows, labels, indices)


def measure_seeded(n: int, rows: np.ndarray, labels: np.ndarray, indices: np.ndarray) -> list[dict]:
    """#1157: the seeded forms, and the splitters that arrived with them."""
    groups = np.arange(n, dtype=np.int64) // ROWS_PER_GROUP
    suffix = f"n{n}"
    kfold = KFold(n_splits=FOLD_COUNT, shuffle=True, random_state=SEED)
    stratified = StratifiedKFold(n_splits=FOLD_COUNT, shuffle=True, random_state=SEED)
    group_kfold = GroupKFold(n_splits=FOLD_COUNT)
    stratified_groups = StratifiedGroupKFold(n_splits=FOLD_COUNT)  # NOSONAR S6709: unshuffled, no seed to take
    series = TimeSeriesSplit(n_splits=FOLD_COUNT)
    repeated = RepeatedKFold(n_splits=FOLD_COUNT, n_repeats=REPEAT_COUNT, random_state=SEED)
    return [
        measure(f"kfold_seeded_{suffix}", lambda: list(kfold.split(rows))),
        measure(f"stratified_seeded_{suffix}", lambda: list(stratified.split(rows, labels))),
        measure(f"traintest_seeded_{suffix}",
                lambda: train_test_split(indices, test_size=TEST_FRACTION, random_state=SEED)),
        measure(f"stratified_traintest_{suffix}",
                lambda: train_test_split(indices, test_size=TEST_FRACTION, random_state=SEED, stratify=labels)),
        measure(f"group_kfold_{suffix}", lambda: list(group_kfold.split(rows, groups=groups))),
        measure(f"stratified_group_kfold_{suffix}",
                lambda: list(stratified_groups.split(rows, labels, groups=groups))),
        measure(f"timeseries_{suffix}", lambda: list(series.split(rows))),
        measure(f"repeated_kfold_{suffix}", lambda: list(repeated.split(rows))),
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
    print("Python splitters bench")
    results: list[dict] = []
    for n in SIZES:
        results.extend(measure_size(n))

    OUT.parent.mkdir(parents=True, exist_ok=True)
    OUT.write_text(json.dumps(payload_for(results), indent=2) + "\n", encoding="utf-8")
    print(f"-> {OUT}")


if __name__ == "__main__":
    main()
