#!/usr/bin/env python3
"""Time linearmodels' panel estimators against Lodestar.Stats.Regression's (issue #1156).

Methodology is mirrored by the C# harness (bench/Lodestar.Text.Benchmarks, `compare-panel` mode):

  * the same shapes and the same data, built from each entity, period and column by one formula
    rather than read from a corpus file,
  * the same operations in the same order, each producing the whole table: linearmodels computes
    the R² family, the model tests and the variance decomposition when they are read, so they are
    read here, as the C# summary always carries them,
  * the frame is built outside the timed region, as the C# arrays are; linearmodels' own
    conversion of it into a panel is inside, since it is part of every fit,
  * metric, auto-scaling and best-of-N: `wallcpu.measure`, shared with the harnesses next door.
"""

from __future__ import annotations

import sys
import warnings
from importlib.metadata import version
from pathlib import Path

import numpy as np
import pandas as pd
from linearmodels.panel import BetweenOLS, FirstDifferenceOLS, PanelOLS, RandomEffects

sys.path.insert(0, str(Path(__file__).resolve().parent))

from wallcpu import measure, write  # noqa: E402

ROOT = Path(__file__).resolve().parent.parent
OUT = ROOT / "results" / "python-panel.json"

REGRESSORS = 3
PERIODS = 10
ENTITY_COUNTS = [100, 1_000, 10_000]


def data(entities: int) -> tuple[pd.Series, pd.DataFrame, pd.DataFrame]:
    """The C# side's formula, vectorised: the response, the regressors with a constant, and without."""
    row = np.arange(entities * PERIODS, dtype=float)
    e = np.arange(entities * PERIODS) // PERIODS
    t = np.arange(entities * PERIODS) % PERIODS
    effect = np.sin(0.37 * e)
    x = np.column_stack([np.cos(0.7 * (j + 1) * row + 0.3 * j) + 0.5 * effect for j in range(REGRESSORS)])
    y = effect + 0.2 * np.cos(1.1 * t) + 0.5 * np.sin(2.9 * row) * np.cos(0.13 * row) + x @ (0.4 * np.arange(1, REGRESSORS + 1))
    index = pd.MultiIndex.from_arrays([e, 2000 + t])
    names = [f"x{j}" for j in range(REGRESSORS)]
    bare = pd.DataFrame(x, index=index, columns=names)
    return pd.Series(y, index=index), pd.DataFrame(np.c_[np.ones(len(row)), x], index=index, columns=["const", *names]), bare


def whole_table(result) -> None:
    """Reads what the C# summary carries and linearmodels defers."""
    _ = result.std_errors, result.pvalues, result.conf_int(), result.f_statistic, result.f_statistic_robust
    _ = result.rsquared_within, result.rsquared_between, result.rsquared_overall
    if hasattr(result, "variance_decomposition"):
        _ = result.variance_decomposition
    if hasattr(result, "f_pooled"):
        _ = result.f_pooled


def measure_size(entities: int) -> list[dict]:
    y, x, bare = data(entities)
    suffix = f"n{entities * PERIODS}"
    return [
        measure(f"fe_entity_{suffix}", lambda: whole_table(PanelOLS(y, x, entity_effects=True).fit())),
        measure(f"fe_twoway_clustered_{suffix}", lambda: whole_table(
            PanelOLS(y, x, entity_effects=True, time_effects=True).fit(cov_type="clustered", cluster_entity=True))),
        measure(f"fe_kernel_{suffix}", lambda: whole_table(PanelOLS(y, x, entity_effects=True).fit(cov_type="kernel"))),
        measure(f"between_robust_{suffix}", lambda: whole_table(BetweenOLS(y, x).fit(cov_type="robust"))),
        measure(f"first_difference_robust_{suffix}", lambda: whole_table(FirstDifferenceOLS(y, bare).fit(cov_type="robust"))),
        measure(f"random_effects_{suffix}", lambda: whole_table(RandomEffects(y, x).fit())),
    ]


def main() -> None:
    warnings.simplefilter("ignore")
    print("Python panel bench")
    results: list[dict] = []
    for entities in ENTITY_COUNTS:
        results.extend(measure_size(entities))
    write(OUT, results, {"linearmodels": version("linearmodels"), "numpy": version("numpy"), "pandas": version("pandas")})


if __name__ == "__main__":
    main()
