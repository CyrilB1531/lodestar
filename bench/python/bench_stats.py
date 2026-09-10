#!/usr/bin/env python3
"""Time the Python counterparts of Lodestar.Stats and Lodestar.Stats.Regression (issue #595).

Methodology is mirrored by the C# harness (bench/Lodestar.Text.Benchmarks,
`compare-stats` and `compare-ols` modes) so the two are comparable:

  * same corpus files (bench/corpus/stats/, from generate_stats.py),
  * same operations, in the same order, over the same sizes,
  * metric: milliseconds per operation,
  * auto-scaling: repeat until a measurement lasts >= MIN_TIME,
  * report the best (minimum) of REPEATS measurements,
  * elapsed time (perf_counter) and processor time (process_time) together.

Two libraries, written as one script because they read one corpus: scipy answers the
three tests and statsmodels the regression. Both are already pinned -- scipy by
decision 0082, statsmodels by decision 0096, which admitted it for the OLS oracle.

`sm.OLS(...).fit()` is timed with the summary quantities the C# side computes in the
same call, so the two are priced on the same work: `Fit` returns the standard errors,
t statistics, p-values, intervals and VIFs whether or not a caller reads them, and a
bare `.fit()` computes the estimate lazily. `variance_inflation_factor` is the one
piece statsmodels leaves outside the fit, so it is called separately and the row is
named for what it adds rather than pretending the fit included it.
"""

from __future__ import annotations

import json
import platform
import sys
from importlib.metadata import version
from pathlib import Path
from time import perf_counter, process_time

# PYTHONSAFEPATH, which this repository sets elsewhere, stops Python putting the
# script's own directory on the path, and the sibling module lives there.
sys.path.insert(0, str(Path(__file__).resolve().parent))

import numpy as np
import statsmodels.api as sm
from scipy import stats as sps
from statsmodels.stats.outliers_influence import variance_inflation_factor

MIN_TIME = 0.5
REPEATS = 5

ROOT = Path(__file__).resolve().parent.parent
CORPUS = ROOT / "corpus" / "stats"
TESTS_OUT = ROOT / "results" / "python-stats.json"
OLS_OUT = ROOT / "results" / "python-ols.json"

SIZES = [1_000, 10_000, 100_000]


def check_corpus() -> None:
    missing = [f"stats_n{n}.json" for n in SIZES if not (CORPUS / f"stats_n{n}.json").exists()]
    if missing:
        raise SystemExit(
            f"benchmark corpus incomplete, missing {missing} in {CORPUS}\n"
            "generate it first: python bench/corpus/generate_stats.py"
        )


def measure(operation: str, action) -> dict:
    """Time one operation, recording both elapsed time and processor time.

    The C# harness (bench/Lodestar.Text.Benchmarks/CrossLang/Harness.cs) records the
    same pair for the same reason: .NET's background collector does its work on other
    threads, so elapsed time understates what an allocating operation actually costs.
    Not a formality on this corpus. scipy's own paths land at cpu/wall 1.00, but
    statsmodels reaches LAPACK through numpy, which is threaded: at 100 000 rows the OLS
    rows measure ~15x more processor time than elapsed. Reporting only the wall clock
    would credit Python with work it spread over cores.
    """
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


def load_size(n: int) -> dict:
    payload = json.loads((CORPUS / f"stats_n{n}.json").read_text(encoding="utf-8"))
    regressors = payload["regressors"]
    design = np.asarray(payload["design"], dtype=np.float64).reshape(n, regressors)
    return {
        "first": np.asarray(payload["first"], dtype=np.float64),
        "second": np.asarray(payload["second"], dtype=np.float64),
        "table": np.asarray(payload["table"], dtype=np.int64),
        "design": design,
        "with_constant": sm.add_constant(design),
        "response": np.asarray(payload["response"], dtype=np.float64),
    }


def ols_summary(exog, endog) -> object:
    """The fit plus the quantities `OlsSummary` carries, so both sides price one table."""
    fitted = sm.OLS(endog, exog).fit()
    return (fitted.params, fitted.bse, fitted.tvalues, fitted.pvalues,
            fitted.conf_int(), fitted.rsquared, fitted.rsquared_adj,
            fitted.fvalue, fitted.f_pvalue)


def measure_size(n: int) -> tuple[list[dict], list[dict]]:
    """The tests rows and the regression rows, kept apart.

    One file per harness, because bench/compare.py's `load(side, bench)` reads
    python-<bench>.json and bench-map.json maps `stats` and `ols` to separate
    harnesses so a change to one package does not re-run the other.
    """
    data = load_size(n)
    first, second, table = data["first"], data["second"], data["table"]
    exog, endog, design = data["with_constant"], data["response"], data["design"]
    suffix = f"n{n}"

    tests = [
        measure(f"welch_t_{suffix}", lambda: sps.ttest_ind(first, second, equal_var=False)),
        measure(f"mann_whitney_{suffix}", lambda: sps.mannwhitneyu(first, second)),
        measure(f"chi_square_{suffix}", lambda: sps.chi2_contingency(table)),
    ]
    regression = [
        measure(f"ols_summary_{suffix}", lambda: ols_summary(exog, endog)),
        measure(f"ols_vif_{suffix}",
                lambda: [variance_inflation_factor(design, j) for j in range(design.shape[1])]),
    ]
    return tests, regression


def payload_for(results: list[dict]) -> dict:
    return {
        "metadata": {
            "side": "python",
            "libraries": {
                "scipy": version("scipy"),
                "statsmodels": version("statsmodels"),
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
    check_corpus()

    print("Python stats bench")
    tests: list[dict] = []
    regression: list[dict] = []
    for n in SIZES:
        size_tests, size_regression = measure_size(n)
        tests.extend(size_tests)
        regression.extend(size_regression)

    for out, results in ((TESTS_OUT, tests), (OLS_OUT, regression)):
        out.parent.mkdir(parents=True, exist_ok=True)
        out.write_text(json.dumps(payload_for(results), indent=2) + "\n", encoding="utf-8")
        print(f"-> {out}")


if __name__ == "__main__":
    main()
