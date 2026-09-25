#!/usr/bin/env python3
"""Time linearmodels' IV2SLS, IVLIML and IVGMM against Lodestar.Stats.Regression's (issue #1155).

Methodology is mirrored by the C# harness (bench/Lodestar.Text.Benchmarks, `compare-iv` mode):

  * the same shapes and the same data, built from each row and column index by one formula
    rather than read from a corpus file,
  * the same operations in the same order, each producing the whole table: linearmodels computes
    the first-stage diagnostics and the overidentification test lazily, so they are read here, as
    the C# summary always carries them,
  * metric: milliseconds per operation, best of REPEATS, each repeated until it lasts MIN_TIME,
  * elapsed time (perf_counter) and processor time (process_time) together.
"""

from __future__ import annotations

import json
import platform
import warnings
from importlib.metadata import version
from pathlib import Path
from time import perf_counter, process_time

import numpy as np
from linearmodels.iv import IV2SLS, IVGMM, IVLIML

MIN_TIME = 0.5
REPEATS = 5
EXOGENOUS = 3
ENDOGENOUS = 2
INSTRUMENTS = 4
ROWS_PER_CLUSTER = 20

ROOT = Path(__file__).resolve().parent.parent
OUT = ROOT / "results" / "python-iv.json"

SIZES = [1_000, 10_000, 100_000]


def data(n: int) -> tuple:
    """The C# side's formula, vectorised."""
    i = np.arange(n, dtype=float)
    u = np.sin(1.7 * i) * np.cos(0.3 * i)
    z = np.column_stack([np.sin(0.7 * (j + 1) * i + 1.3 * j) + 0.5 * np.cos(0.11 * i * (j + 1)) for j in range(INSTRUMENTS)])
    w = np.column_stack([np.cos(0.9 * (j + 1) * i + 0.4 * j) for j in range(EXOGENOUS)])
    x = np.column_stack([
        0.6 * u + 0.3 * np.sin(2.3 * i + k) + z @ np.array([0.5 + 0.1 * (j + k) for j in range(INSTRUMENTS)])
        for k in range(ENDOGENOUS)])
    y = 0.5 + u + w @ np.array([0.2 * (j + 1) for j in range(EXOGENOUS)]) + 1.5 * x[:, 0] - 0.7 * x[:, 1]
    clusters = (np.arange(n) // ROWS_PER_CLUSTER).astype(int)
    return y, np.c_[np.ones(n), w], x, z, clusters


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


def whole_table(result, overidentification: str) -> None:
    """Reads what the C# summary carries and linearmodels defers."""
    _ = result.std_errors, result.pvalues, result.conf_int(), result.f_statistic
    _ = result.first_stage.diagnostics
    _ = getattr(result, overidentification)


def measure_size(n: int) -> list[dict]:
    y, exog, endog, instr, clusters = data(n)
    suffix = f"n{n}"
    return [
        measure(f"2sls_robust_{suffix}", lambda: whole_table(IV2SLS(y, exog, endog, instr).fit(), "sargan")),
        measure(f"liml_robust_{suffix}", lambda: whole_table(IVLIML(y, exog, endog, instr).fit(), "sargan")),
        measure(f"gmm_robust_{suffix}", lambda: whole_table(IVGMM(y, exog, endog, instr).fit(), "j_stat")),
        measure(f"2sls_kernel_{suffix}",
                lambda: whole_table(IV2SLS(y, exog, endog, instr).fit(cov_type="kernel"), "sargan")),
        measure(f"2sls_clustered_{suffix}",
                lambda: whole_table(IV2SLS(y, exog, endog, instr).fit(cov_type="clustered", clusters=clusters),
                                    "sargan")),
    ]


def main() -> None:
    warnings.simplefilter("ignore")
    print("Python instrumental-variables bench")
    results: list[dict] = []
    for n in SIZES:
        results.extend(measure_size(n))
    payload = {
        "metadata": {
            "side": "python",
            "libraries": {"linearmodels": version("linearmodels"), "numpy": version("numpy")},
            "python": platform.python_version(),
            "machine": platform.machine(),
            "min_time_s": MIN_TIME,
            "repeats": REPEATS,
        },
        "results": results,
    }
    OUT.parent.mkdir(parents=True, exist_ok=True)
    OUT.write_text(json.dumps(payload, indent=2) + "\n", encoding="utf-8")
    print(f"-> {OUT}")


if __name__ == "__main__":
    main()
