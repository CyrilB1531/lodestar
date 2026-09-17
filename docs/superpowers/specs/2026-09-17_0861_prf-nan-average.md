# 0861 — Macro and weighted averages follow scikit-learn's `_nanaverage`

**Status:** accepted, 2026-09-17. Written with the fix it records.

Issue: [#861](https://github.com/CyrilB1531/lodestar/issues/861), found by the systematic review of `main`.

## Reproduced

| call | scikit-learn 1.9 | `main` |
| --- | ---: | ---: |
| `precision_score([0,0,1], [0,0,0], average="macro", zero_division=np.nan)` | `0.6667` | `NaN` |
| `precision_score([0,2,1,1], [1,2,0,1], labels=[0,2], sample_weight=[0,0,1,1], average="weighted", zero_division=1)` | `0.5` | `0.0` |

`precision_recall_fscore_support` averages through `_nanaverage` (`sklearn/utils/extmath.py`): an empty or all-`NaN` input is `NaN`; otherwise the `NaN` entries leave with their weights, and weights that then sum to zero give the unweighted mean. `Prf.Average` summed every class, `NaN` included, and returned `0.0` on zero total support under a comment claiming scikit-learn does. The second row reaches zero total support through zero weights because `ConfusionMatrix.Compute` refuses a label set absent from `y_true`, where scikit-learn scores `recall_score([0,0], [1,1], labels=[1], average="weighted", zero_division=1)` as `1.0`. `classification_report` computes its macro and weighted rows through the same function, and `ClassificationReport` through `Prf.Average`, so both move together.

## Change

`Prf.Average` skips `NaN` classes, returns `NaN` when none is left, and falls back to the unweighted mean when the remaining support sums to zero. `jaccard_score` does not call `_nanaverage` but drops all-zero weights the same way, and refuses `zero_division=np.nan`; under `ZeroDivision.NaN`, this package's extension there, `JaccardScore` skips the undefined classes like the other scores.

## Oracles

`classification_metrics.json` gains an `undefined_averages` section: five fixtures (one class never predicted, unweighted and weighted; a requested label absent from both sides; every class undefined; zero total support through zero sample weights) under `zero_division` 0, 1 and `nan`, macro and weighted, for precision, recall, F1, F-beta at 0.5 and 2, Jaccard, and the report's two average rows. The existing cases are unchanged: `tools/compare_oracles.py` reports only the new section.

## Rejected

- **Adding `np.nan` to `ZERO_DIVISIONS`.** Every existing case would gain keys, and the classification tests' parsers would move with them, for a mode the fixtures there rarely make undefined.
