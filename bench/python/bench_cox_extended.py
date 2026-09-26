#!/usr/bin/env python3
"""Time lifelines' extended Cox fits, proportional hazards test, predictions and time-varying fit against Lodestar.Survival (#1171).

Methodology is mirrored by the C# harness (bench/Lodestar.Text.Benchmarks, `compare-cox-extended` mode):

  * the same subjects on both sides, built from the index by one formula: four covariates in [-1, 1) each hashed by its own multiplier, durations
    from a hashed uniform, seven in ten observed, three strata, weights one to three,
  * the same operations, each a fit at lifelines' defaults as a caller would run it: stratified and weighted,
    ridge with the robust variance, lasso, the proportional hazards test of a fitted model, the survival of a
    hundred subjects at ten times, and a time-varying fit over two intervals per subject,
  * metric, auto-scaling and best-of-N: `wallcpu.measure`, shared with the harnesses next door.
"""

from __future__ import annotations

import sys
import warnings
from importlib.metadata import version
from pathlib import Path

import numpy as np
import pandas as pd
from lifelines import CoxPHFitter, CoxTimeVaryingFitter
from lifelines.statistics import proportional_hazard_test

sys.path.insert(0, str(Path(__file__).resolve().parent))

from wallcpu import measure, write  # noqa: E402

ROOT = Path(__file__).resolve().parent.parent
OUT = ROOT / "results" / "python-cox-extended.json"

SIZES = [1_000, 10_000]
FEATURES = 4
NAMES = [f"x{j}" for j in range(FEATURES)]


def subjects(n: int) -> pd.DataFrame:
    """The C# side's formula, from the index alone."""
    i = np.arange(n, dtype=np.int64)[:, None]
    j = np.arange(FEATURES, dtype=np.int64)[None, :]
    x = ((i * (2654435761 + j * 104729)) % 10007) / 10007.0 * 2.0 - 1.0
    k = np.arange(n, dtype=np.int64)
    u = ((k * 7919) % 10007 + 1) / 10008.0
    duration = np.round(-np.log(u) / np.exp(0.5 * x[:, 0] - 0.3 * x[:, 1]) * 10.0, 2) + 0.01
    frame = pd.DataFrame(x, columns=NAMES)
    frame["T"], frame["E"] = duration, ((k * 37) % 10 < 7).astype(int)
    frame["s"], frame["w"] = k % 3, (1 + k % 3).astype(float)
    return frame


def intervals(frame: pd.DataFrame) -> pd.DataFrame:
    """Two intervals per subject, split at half its duration; the second carries x1 moved by a quarter."""
    first = frame[NAMES].copy()
    first["start"], first["stop"], first["E"], first["id"] = 0.0, frame["T"] / 2.0, 0, frame.index
    second = frame[NAMES].copy()
    second["x1"] = second["x1"] + 0.25
    second["start"], second["stop"], second["E"], second["id"] = frame["T"] / 2.0, frame["T"], frame["E"], frame.index
    return pd.concat([first, second], ignore_index=True)


def main() -> None:
    warnings.simplefilter("ignore")
    print("Python extended Cox bench")
    results: list[dict] = []
    for n in SIZES:
        frame = subjects(n)
        plain = frame[NAMES + ["T", "E"]]
        fitted = CoxPHFitter().fit(plain, "T", "E")
        rows = frame[NAMES].iloc[:100]
        times = np.quantile(frame["T"], np.linspace(0.05, 0.95, 10))
        varying = intervals(frame)
        results.append(measure(f"stratified_weighted_{n}", lambda f=frame: CoxPHFitter().fit(
            f[NAMES + ["T", "E", "s", "w"]], "T", "E", strata=["s"], weights_col="w")))
        results.append(measure(f"ridge_robust_{n}", lambda p=plain: CoxPHFitter(penalizer=0.1).fit(p, "T", "E", robust=True)))
        results.append(measure(f"lasso_{n}", lambda p=plain: CoxPHFitter(penalizer=0.05, l1_ratio=1.0).fit(p, "T", "E")))
        results.append(measure(f"ph_test_{n}", lambda m=fitted, p=plain: proportional_hazard_test(m, p, time_transform="rank")))
        results.append(measure(f"predict_survival_{n}", lambda m=fitted, r=rows, t=times: m.predict_survival_function(r, times=t)))
        results.append(measure(f"time_varying_{n}", lambda v=varying: CoxTimeVaryingFitter().fit(
            v, id_col="id", event_col="E", start_col="start", stop_col="stop")))
    write(OUT, results, {"lifelines": version("lifelines"), "numpy": version("numpy")})


if __name__ == "__main__":
    main()
