#!/usr/bin/env python3
"""Time lifelines' log-rank family, restricted mean, fixed-point test and concordance against Lodestar.Survival (#1170).

Methodology is mirrored by the C# harness (bench/Lodestar.Text.Benchmarks, `compare-survival-family` mode):

  * the same subjects on both sides, built from the index by one formula: durations in hundredths with
    ties, seven in ten observed, five groups, a score loosely tied to the duration,
  * the same operations: a weighted two-group test, the multi-group and pairwise tests over five groups,
    a Kaplan-Meier fit with its restricted mean and variance, two fits compared at one time, the
    concordance index,
  * metric, auto-scaling and best-of-N: `wallcpu.measure`, shared with the harnesses next door.
"""

from __future__ import annotations

import sys
import warnings
from importlib.metadata import version
from pathlib import Path

import numpy as np
from lifelines import KaplanMeierFitter
from lifelines.statistics import (logrank_test, multivariate_logrank_test, pairwise_logrank_test,
                                  survival_difference_at_fixed_point_in_time_test)
from lifelines.utils import concordance_index, restricted_mean_survival_time

sys.path.insert(0, str(Path(__file__).resolve().parent))

from wallcpu import measure, write  # noqa: E402

ROOT = Path(__file__).resolve().parent.parent
OUT = ROOT / "results" / "python-survival-family.json"

SIZES = [1_000, 10_000, 100_000]
GROUPS = 5
HORIZON = 50.0


def subjects(n: int) -> tuple[np.ndarray, np.ndarray, np.ndarray, np.ndarray]:
    """The C# side's formula: durations, events, groups and scores from the index alone."""
    i = np.arange(n, dtype=np.int64)
    durations = ((i * 2654435761) % 10007) / 100.0 + 0.01
    events = (i * 37) % 10 < 7
    groups = i % GROUPS
    scores = ((i * 40503) % 1009) / 10.0 + durations
    return durations, events, groups, scores


def rmst(durations: np.ndarray, events: np.ndarray) -> tuple:
    return restricted_mean_survival_time(KaplanMeierFitter().fit(durations, events), t=HORIZON, return_variance=True)


def fixed(durations: np.ndarray, events: np.ndarray, groups: np.ndarray) -> object:
    a, b = groups == 0, groups == 1
    return survival_difference_at_fixed_point_in_time_test(
        HORIZON, KaplanMeierFitter().fit(durations[a], events[a]), KaplanMeierFitter().fit(durations[b], events[b]))


def main() -> None:
    warnings.simplefilter("ignore")
    print("Python survival family bench")
    results: list[dict] = []
    for n in SIZES:
        d, e, g, s = subjects(n)
        a, b = g == 0, g == 1
        w = 1.0 + (np.arange(n) % 3)
        results.append(measure(f"logrank_wilcoxon_weighted_{n}", lambda d=d, e=e, a=a, b=b, w=w: logrank_test(
            d[a], d[b], e[a], e[b], weights_A=w[a], weights_B=w[b], weightings="wilcoxon")))
        results.append(measure(f"logrank_fleming_harrington_{n}", lambda d=d, e=e, a=a, b=b: logrank_test(
            d[a], d[b], e[a], e[b], weightings="fleming-harrington", p=1.0, q=1.0)))
        results.append(measure(f"multigroup_{n}", lambda d=d, e=e, g=g: multivariate_logrank_test(d, g, e)))
        results.append(measure(f"pairwise_{n}", lambda d=d, e=e, g=g: pairwise_logrank_test(d, g, e)))
        results.append(measure(f"rmst_{n}", lambda d=d, e=e: rmst(d, e)))
        results.append(measure(f"fixed_point_{n}", lambda d=d, e=e, g=g: fixed(d, e, g)))
        results.append(measure(f"concordance_{n}", lambda d=d, e=e, s=s: concordance_index(d, s, e)))
    write(OUT, results, {"lifelines": version("lifelines"), "numpy": version("numpy")})


if __name__ == "__main__":
    main()
