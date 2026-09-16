# SimpleImputerOptions

How [`SimpleImputer`](simpleimputer.md) fills a missing value, and what to do with an empty feature.

<!-- docs-declaration -->

```csharp
public sealed record SimpleImputerOptions
```

**Properties** — `Strategy` chooses the statistic, `FillValue` is what
[`ImputationStrategy.Constant`](imputationstrategy.md) fills with (zero by default), and
`KeepEmptyFeatures` decides what happens to a feature with no value at all —
`sklearn.impute.SimpleImputer`'s `strategy`, `fill_value` and `keep_empty_features`.

**Example** — a constant the caller chose, and a feature that has nothing to average.

```csharp
using Lodestar.Preprocessing;

double[] samples = [1.0, double.NaN, 3.0];

SimpleImputer constant = SimpleImputer.Fit(
    samples,
    1,
    new SimpleImputerOptions { Strategy = ImputationStrategy.Constant, FillValue = -1.0 });

string filled = string.Join(",", constant.Transform(samples));  // => 1,-1,3
```

**Remarks** — **`KeepEmptyFeatures` is where this package and the reference differ.** Its default
drops an empty feature from the output; here that is refused, and this option fills the feature with
zero instead — the reference's `keep_empty_features=True`. Either way the output has as many columns
as the input, which the reference's default does not guarantee.

**Applies to** — net10.0, netstandard2.0.

**See also** — [`SimpleImputer.Fit`](simpleimputer-fit.md), [`ImputationStrategy`](imputationstrategy.md).
