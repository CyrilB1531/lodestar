# ConformalQuantileRule

Which order statistic [`SplitConformal.Quantile`](splitconformal-quantile.md) reads from the
calibration scores.

<!-- docs-declaration -->

```csharp
public enum ConformalQuantileRule { Ceiling, MapieClassification }
```

**Members** — `Ceiling` reads the `k`-th smallest score, `k = ceil((n + 1)(1 − alpha))`, which is
what MAPIE 1.5.0's `SplitConformalRegressor` reads, and the default. `MapieClassification` reads
`numpy.quantile(scores, (n + 1)(1 − alpha)/n, method="higher")`, which is what MAPIE's
`SplitConformalClassifier` reads before `predict_set`.

**Example** — nineteen scores at 10 % miscoverage, where the two rules part by one rank.

```csharp
using Lodestar.Conformal;

double[] scores = [1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17, 18, 19];

double ceiling = SplitConformal.Quantile(scores, 0.1);                                           // => 18
double mapie = SplitConformal.Quantile(scores, 0.1, ConformalQuantileRule.MapieClassification);   // => 19
```

**Remarks** — the two rules agree on most `(n, alpha)` pairs and part by one rank on the rest, so a
prediction set can include or exclude a class differently. Take `MapieClassification` when the sets
must match MAPIE's; `Ceiling` is kept as the default so an existing call keeps its answer.
[Decision 0143](../../../decisions/0143-prediction-sets-can-read-mapies-classification-quantile.md)
has the measurement and why the default did not move.

`Ceiling` is the zero value, so a `default(ConformalQuantileRule)` reads the default rule. A value
outside the two, a cast from an `int` most likely, is refused with `ArgumentOutOfRangeException`.

**Applies to** — net10.0, netstandard2.0.

**See also** — [`SplitConformal.Quantile`](splitconformal-quantile.md),
[`SplitConformal.PredictionSet`](splitconformal-predictionset.md), the
[Python equivalence table](../../../equivalence.md).
