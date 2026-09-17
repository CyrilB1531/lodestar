# ZeroOneLoss.Score

The fraction of samples predicted wrongly — `sklearn.metrics.zero_one_loss`.

<!-- docs-declaration -->

```csharp
public static double Score(ReadOnlySpan<int> yTrue, ReadOnlySpan<int> yPred, bool normalize = true, ReadOnlySpan<double> sampleWeight = default)
public static double Score(ReadOnlySpan<bool> yTrue, ReadOnlySpan<bool> yPred, int labelCount, bool normalize = true, ReadOnlySpan<double> sampleWeight = default)
```

**Parameters** — the first overload takes `yTrue` and `yPred` as one label per sample, the second as
a row-major label matrix with `labelCount` values per row. `normalize` divides by the total weight;
`false` returns the weight of the wrong samples instead. `sampleWeight` is one weight per **sample**,
or empty.

**Returns** — `double` in `[0, 1]` when `normalize` holds, and the weight of the wrong samples when
it does not — a count when every weight is `1`.

**Exceptions** — `ArgumentException` when the inputs disagree in length, are empty, when the matrix
is not a whole number of rows of `labelCount`, or when the weights do not match the sample count. A `sampleWeight` holding `NaN` or an infinity is refused with "Input sample_weight contains NaN." or its infinity counterpart, and one that is zero throughout with "Sample weights must contain at least one non-zero number." — both `ArgumentException` naming `sampleWeight`, as scikit-learn's `_check_sample_weight` refuses them. Weights that merely sum to zero are refused too, with numpy's "Weights sum to zero, can't be normalized." — only while `normalize` is true, since the weight of the wrong samples never divides.

**Example** — the count, which is what `normalize: false` is for.

```csharp
using Lodestar.Metrics;

int[] truth = [0, 1, 2, 1];
int[] predicted = [0, 2, 2, 1];

double wrong = ZeroOneLoss.Score(truth, predicted, normalize: false);  // => 1
```

**Remarks** — with weights and `normalize: false` the answer is the **weight** of the wrong samples,
not how many there are: `2` on the example above under weights `[1, 2, 3, 4]`, where the count is
`1`. That is the reference's behaviour and the same widening
[`TopKAccuracy.Score`](../ranking/topkaccuracy-score.md) documents for its own `normalize`.

**Applies to** — net10.0, netstandard2.0.

**See also** — [`HammingLoss.Score`](hammingloss-score.md), [`Accuracy.Score`](accuracy-score.md),
the [Python equivalence table](../../../equivalence.md).
