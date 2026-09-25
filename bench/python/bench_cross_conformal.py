#!/usr/bin/env python3
"""Time MAPIE's cross-conformal regressors against Lodestar.Conformal's CrossConformal (issue #1159).

Methodology is mirrored by the C# harness (bench/Lodestar.Text.Benchmarks, `compare-cross-conformal` mode):

  * the same shapes and the same predictions: every model is a formula of the row index, shifted by
    the mean index of the rows it was fitted on, and K-fold runs unshuffled, so both sides know each
    fold's model without sharing a file,
  * what is timed is the interval at every test point from fitted models: `predict_interval` here,
    which also runs each model's `predict` on the test block -- timed on its own as `predict_only`
    so it can be read off -- and the loop over `CrossConformal.Interval` on the C# side, which is
    handed those predictions,
  * the jackknife-after-bootstrap draws its bags from MAPIE's generator here and from a formula on
    the C# side, at the same model count and a similar out-of-bag share,
  * metric, auto-scaling and best-of-N: `wallcpu.measure`, shared with the harnesses next door.
"""

from __future__ import annotations

import sys
import warnings
from importlib.metadata import version
from pathlib import Path

import numpy as np
from sklearn.base import BaseEstimator, RegressorMixin
from sklearn.model_selection import KFold

sys.path.insert(0, str(Path(__file__).resolve().parent))

from wallcpu import measure, write  # noqa: E402

ROOT = Path(__file__).resolve().parent.parent
OUT = ROOT / "results" / "python-cross-conformal.json"

FOLDS = 10
BOOTSTRAPS = 30
TESTS = 500
ALPHA = 0.1
SIZES = [1_000, 10_000]


def base(index: np.ndarray) -> np.ndarray:
    """The C# side's formula for a prediction at a row index, positive so the gamma score applies."""
    return 50.0 + 10.0 * np.sin(0.01 * index)


class FormulaRegressor(RegressorMixin, BaseEstimator):
    """A model that is a formula of the row index, shifted by the mean index it was fitted on."""

    def fit(self, x, _y):
        self.shift_ = float(np.mean(x[:, 0])) / len(x)
        self.n_features_in_ = 1
        self.is_fitted_ = True
        return self

    def predict(self, x):
        return base(x[:, 0]) + self.shift_


def measure_size(n: int) -> list[dict]:
    from mapie.regression import CrossConformalRegressor, JackknifeAfterBootstrapRegressor

    index = np.arange(n, dtype=float)
    x = index[:, None]
    y = base(index) + 2.0 * np.sin(1.3 * index)
    x_test = (n + np.arange(TESTS, dtype=float))[:, None]
    suffix = f"n{n}"

    def cross(method: str, score: str):
        # NOSONAR S6709: unshuffled on purpose, so the C# side knows each fold; KFold refuses a
        # random_state without shuffle=True.
        folds = KFold(FOLDS)  # NOSONAR S6709
        return CrossConformalRegressor(FormulaRegressor(), confidence_level=1 - ALPHA, conformity_score=score,
                                       method=method, cv=folds, random_state=0).fit_conformalize(x, y)

    plus, minmax, gamma = cross("plus", "absolute"), cross("minmax", "absolute"), cross("plus", "gamma")
    bag = JackknifeAfterBootstrapRegressor(FormulaRegressor(), confidence_level=1 - ALPHA, resampling=BOOTSTRAPS,
                                           random_state=0).fit_conformalize(x, y)
    models = plus._mapie_regressor.estimator_.estimators_
    return [
        measure(f"predict_only_{suffix}", lambda: [m.predict(x_test) for m in models]),
        measure(f"cv_plus_{suffix}", lambda: plus.predict_interval(x_test)),
        measure(f"cv_minmax_{suffix}", lambda: minmax.predict_interval(x_test)),
        measure(f"cv_plus_gamma_{suffix}", lambda: gamma.predict_interval(x_test)),
        measure(f"bootstrap_plus_{suffix}", lambda: bag.predict_interval(x_test)),
    ]


def main() -> None:
    warnings.simplefilter("ignore")
    print("Python cross-conformal bench")
    results: list[dict] = []
    for n in SIZES:
        results.extend(measure_size(n))
    write(OUT, results, {"mapie": version("mapie"), "numpy": version("numpy")})


if __name__ == "__main__":
    main()
