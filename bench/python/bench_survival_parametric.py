#!/usr/bin/env python3
"""Time lifelines' parametric fitters, AFT regressions and non-parametric curves against Lodestar.Survival (#1172).

Methodology is mirrored by the C# harness (bench/Lodestar.Text.Benchmarks, `compare-survival-parametric` mode):

  * the same subjects on both sides, bench_cox_extended's formula: four covariates in [-1, 1), durations from a hashed
    uniform, seven in ten observed; an interval runs from the duration to half as far again, one in five open,
  * the same operations, each a fit at lifelines' defaults as a caller would run it: a Weibull, a left-censored
    log-logistic, an interval-censored log-normal and a generalized gamma univariate fit, a Weibull AFT regression, a
    log-normal one penalised with the robust variance, an interval-censored log-logistic one, the
    Breslow-Fleming-Harrington curve and the left-censored Kaplan-Meier curve,
  * metric, auto-scaling and best-of-N: `wallcpu.measure`, shared with the harnesses next door.
"""

from __future__ import annotations

import sys
import warnings
from importlib.metadata import version
from pathlib import Path

import numpy as np
from lifelines import (BreslowFlemingHarringtonFitter, GeneralizedGammaFitter, KaplanMeierFitter,
                       LogLogisticAFTFitter, LogLogisticFitter, LogNormalAFTFitter, LogNormalFitter,
                       WeibullAFTFitter, WeibullFitter)

sys.path.insert(0, str(Path(__file__).resolve().parent))

from bench_cox_extended import NAMES, subjects  # noqa: E402
from wallcpu import measure, write  # noqa: E402

ROOT = Path(__file__).resolve().parent.parent
OUT = ROOT / "results" / "python-survival-parametric.json"

SIZES = [1_000, 10_000]


def main() -> None:
    warnings.simplefilter("ignore")
    print("Python parametric survival bench")
    results: list[dict] = []
    for n in SIZES:
        frame = subjects(n)
        k = np.arange(n)
        t, e = frame["T"].to_numpy(), frame["E"].to_numpy()
        lower = t.copy()
        upper = np.where(e == 1, t, np.where(k % 5 == 0, np.inf, t * 1.5))
        design = frame[NAMES + ["T", "E"]]
        bounded = frame[NAMES].copy()
        bounded["L"], bounded["U"] = lower, upper
        results.append(measure(f"weibull_{n}", lambda t=t, e=e: WeibullFitter().fit(t, e)))
        results.append(measure(f"loglogistic_left_{n}", lambda t=t, e=e: LogLogisticFitter().fit_left_censoring(t, e)))
        results.append(measure(f"lognormal_interval_{n}", lambda lo=lower, up=upper: LogNormalFitter().fit_interval_censoring(lo, up)))
        results.append(measure(f"generalized_gamma_{n}", lambda t=t, e=e: GeneralizedGammaFitter().fit(t, e)))
        results.append(measure(f"weibull_aft_{n}", lambda d=design: WeibullAFTFitter().fit(d, "T", "E")))
        results.append(measure(f"lognormal_aft_ridge_robust_{n}", lambda d=design: LogNormalAFTFitter(penalizer=0.1).fit(
            d, "T", "E", robust=True)))
        results.append(measure(f"loglogistic_aft_interval_{n}", lambda b=bounded: LogLogisticAFTFitter().fit_interval_censoring(
            b, "L", "U")))
        results.append(measure(f"breslow_fleming_harrington_{n}", lambda t=t, e=e: BreslowFlemingHarringtonFitter().fit(t, e)))
        results.append(measure(f"kaplan_meier_left_{n}", lambda t=t, e=e: KaplanMeierFitter().fit_left_censoring(t, e)))
    write(OUT, results, {"lifelines": version("lifelines"), "numpy": version("numpy")})


if __name__ == "__main__":
    main()
