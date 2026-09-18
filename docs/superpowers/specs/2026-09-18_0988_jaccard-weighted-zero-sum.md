# Weighted `JaccardScore` refuses a zero weight sum — design

**Issue:** [#988](https://github.com/CyrilB1531/lodestar/issues/988) — minor, `Lodestar.Metrics`,
found by the delta review of `main` (`de9f192d..f7fff453`, 2026-09-17).
**Status:** written before the work, 2026-09-18.
**Package:** `Lodestar.Metrics`. No other package is touched, and no edge moves.

## The finding, reproduced

Against scikit-learn 1.9.0 and numpy 2.5.3 in `.venv-oracles`, on
`y_true = [0, 0, 1, 1]`, `y_pred = [0, 1, 1, 0]`, `sample_weight = [1, 1, -1, -1]`,
`average="weighted"`:

| call | scikit-learn 1.9.0 | Lodestar on `af19b0de` |
| --- | --- | --- |
| `jaccard_score` | `ZeroDivisionError: Weights sum to zero, can't be normalized` | `1` |
| `precision_score` | `0.0` | `0.0` |
| `recall_score` | `0.5` | `0.5` |
| `f1_score` | `1.0` | `1.0` |

The supports are `[2, -2]`: they cancel without any of them being zero.

## Why the four disagree in the reference

They reach the mean through two different helpers, and the difference is one line.

`jaccard_score` (`sklearn/metrics/_classification.py:1233`) builds its own weights and
calls `_average`:

```python
weights = MCM[:, 1, 0] + MCM[:, 1, 1]
if not xp.any(weights):
    weights = None
return float(_average(jaccard, weights=weights, xp=xp))
```

`xp.any` is an exact test against zero **per element**, so only supports that are *every one*
zero drop the weights. A set that merely sums to zero keeps them and reaches `_average`,
which raises.

`precision_recall_fscore_support` (`:2213`) calls `_nanaverage`, which
(`sklearn/utils/extmath.py:1374`) *catches* that error:

```python
try:
    return _average(a, weights=weights)
except ZeroDivisionError:
    # this is when all weights are zero, then ignore them
    return _average(a)
```

The comment is wrong about its own guard — the `except` fires on any zero sum, not only on
all-zero weights — and that is exactly the behaviour the three siblings inherit: the
unweighted mean.

`Prf.Average` implements the `_nanaverage` rule and every one of the four metrics goes
through it. That is correct for three of them. The #861 spec recorded Jaccard as "drops
all-zero weights the same way", which holds only when every support is zero.

## What changes

`Prf.Aggregate` takes a Jaccard-specific branch for `Averaging.Weighted` alone:

1. no support is non-zero → the unweighted mean over the defined classes, as today;
2. otherwise the support-weighted mean, with a zero total refused before the division.

The refusal is `Weights.RequireNonZeroSum`, already the package's single wording for the
case — `ArgumentException`, `"Weights sum to zero, can't be normalized."`, `paramName`
`sampleWeight`, which `docs/equivalence.md` records as the shape every `ZeroDivisionError`
out of `numpy.average` takes here.

## What deliberately does not change

- **`Averaging.Macro`, `Micro` and `Binary`.** The reference passes `weights=None` for macro
  and never divides by a weight sum for the other two; nothing to diverge over.
- **`JaccardScore.PerClass`.** It never averages.
- **`ClassificationReport`.** Its rows are precision, recall and F1; it holds no Jaccard row,
  and its two calls to `Prf.Average` keep the `_nanaverage` rule.
- **The three sibling metrics.** They are at parity, measured above, and `Prf.Average` is
  left as it stands.
- **`ZeroDivision.NaN` under Jaccard.** `jaccard_score` refuses `zero_division=np.nan`
  outright (`InvalidParameterError`), so Lodestar's NaN mode is a superset with no reference
  value — `UndefinedAverageTests.Jaccard_under_NaN_averages_only_the_defined_classes`
  pins it, and the new branch keeps skipping NaN classes the same way.

## Reachability

Only a negative `sampleWeight` reaches the refusal. With every weight `>= 0` each support is
`>= 0`, so a zero total means every support is zero, which is case 1. A `sampleWeight` that
is zero throughout is already refused upstream, in `_check_sample_weight`'s words.

## The rejected option

**Record the divergence instead.** `docs/equivalence.md` would say that Lodestar answers the
unweighted mean where `jaccard_score` raises. Rejected by the maintainer on 2026-09-18: the
divergence is not a choice anybody made, it is #861 reading two reference rules as one, and
the package already refuses a zero weight sum in seven other metrics with the sentence this
one would need. A divergence row also costs a reader more than the branch costs the code.

## How it is proven

A new `undefined_averages` fixture, `supports_cancel`, carrying the inputs measured above.
The generator records a raising `jaccard_score` as the string `"ZeroDivisionError"` in the
slot that otherwise holds a number, beside the `null` already written for the NaN mode that
the reference refuses. `UndefinedAverageTests.Scores_match_sklearn` asserts the throw when it
reads that string. The same fixture extends the three siblings' coverage to a cancelling
support, where they stay at the unweighted mean.
