#!/usr/bin/env python3
"""Time lifelines' Aalen additive fitter against Lodestar.Survival (#1173).

Methodology is mirrored by the C# harness (bench/Lodestar.Text.Benchmarks, `compare-survival-aalen` mode):

  * the same subjects on both sides, bench_cox_extended's formula: four covariates in [-1, 1), durations from a hashed
    uniform, seven in ten observed, weights one to three,
  * the same operations: a default fit, a weighted fit with both penalties, and the survival of a hundred subjects,
  * metric, auto-scaling and best-of-N: `wallcpu.measure`, shared with the harnesses next door.
"""

from __future__ import annotations

import sys
import warnings
from importlib.metadata import version
from pathlib import Path

from lifelines import AalenAdditiveFitter

sys.path.insert(0, str(Path(__file__).resolve().parent))

from bench_cox_extended import NAMES, subjects  # noqa: E402
from wallcpu import measure, write  # noqa: E402

ROOT = Path(__file__).resolve().parent.parent
OUT = ROOT / "results" / "python-survival-aalen.json"

SIZES = [1_000, 10_000]


def main() -> None:
    warnings.simplefilter("ignore")
    print("Python Aalen additive bench")
    results: list[dict] = []
    for n in SIZES:
        frame = subjects(n)
        plain = frame[NAMES + ["T", "E"]]
        weighted = frame[NAMES + ["T", "E", "w"]]
        fitted = AalenAdditiveFitter().fit(plain, "T", "E")
        rows = frame[NAMES].iloc[:100]
        results.append(measure(f"fit_{n}", lambda p=plain: AalenAdditiveFitter().fit(p, "T", "E")))
        results.append(measure(f"fit_penalised_weighted_{n}", lambda w=weighted: AalenAdditiveFitter(
            coef_penalizer=0.5, smoothing_penalizer=1.0).fit(w, "T", "E", weights_col="w")))
        results.append(measure(f"predict_survival_{n}", lambda m=fitted, r=rows: m.predict_survival_function(r)))
    write(OUT, results, {"lifelines": version("lifelines"), "numpy": version("numpy")})


if __name__ == "__main__":
    main()
