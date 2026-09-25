# AndersonKSampleVariant

Which form of the k-sample Anderson-Darling statistic
[`AndersonDarling.KSample`](andersondarling-ksample.md) computes: scipy 1.18's `variant`.

<!-- docs-declaration -->

```csharp
public enum AndersonKSampleVariant { Midrank, Right, Continuous }
```

**Members** — `Midrank`, the default, is Scholz and Stephens' equation 7, with mid-ranks for ties,
for continuous and discrete samples alike: scipy's `midrank=True`. `Right` is their equation 6,
for discrete samples: `midrank=False`. `Continuous` is equation 3, which assumes no ties.

**Example** — the same two samples under two variants.

```csharp
using Lodestar.Stats;

double[] before = [2.1, 3.4, 1.9, 4.2, 3.3, 2.8, 3.9, 2.2, 3.1, 2.7];
double[] after = [2.5, 3.1, 2.8, 4.6, 3.9, 3.0, 4.4, 2.9, 3.5, 3.6];

double midrank = Math.Round(AndersonDarling.KSample(before, after).Statistic, 6);
double continuous = Math.Round(
    AndersonDarling.KSample(AndersonKSampleVariant.Continuous, before, after).Statistic, 6);

double withTies = midrank;          // => 0.385197
double withoutTies = continuous;    // => 0.342788
```

**Remarks** — scipy 1.18 is replacing `midrank` with `variant`, and will drop the critical values
from the result it returns; these are kept, since they are what a clamped p-value leaves to read.

**Applies to** — net10.0, netstandard2.0.

**See also** — [`AndersonDarling.KSample`](andersondarling-ksample.md).
