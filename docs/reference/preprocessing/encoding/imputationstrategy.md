# ImputationStrategy

What [`SimpleImputer`](simpleimputer.md) fills a missing value with.

<!-- docs-declaration -->

```csharp
public enum ImputationStrategy
```

**Fields** — `Mean` (the default, as the reference's is), `Median`, `MostFrequent` and `Constant` —
`strategy="mean"`, `"median"`, `"most_frequent"` and `"constant"`.

**Example** — the tie rule, which is the one worth knowing.

```csharp
using Lodestar.Preprocessing;

// 1 and 2 both appear twice.
double[] tied = [1.0, 1.0, 2.0, 2.0, double.NaN];

SimpleImputer imputer = SimpleImputer.Fit(
    tied, 1, new SimpleImputerOptions { Strategy = ImputationStrategy.MostFrequent });

double fill = imputer.Statistics[0];  // => 1
```

**Remarks** — **a tie goes to the smaller value**, measured against the reference rather than
assumed. An implementation that keeps the last of an equally long run fills with 2 instead and
passes every untied case, which is why the corpus freezes this one.

`Median` is `np.ma.median`, as the reference's is: the two middle values added and halved, the
middle one added to itself where the count is odd — so three values of `1e308` give infinity
([#1230](https://github.com/CyrilB1531/lodestar/issues/1230)).

**Applies to** — net10.0, netstandard2.0.

**See also** — [`SimpleImputerOptions`](simpleimputeroptions.md), [`SimpleImputer.Fit`](simpleimputer-fit.md).
