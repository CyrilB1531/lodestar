# AndersonDarling.Test

Tests a sample against the normal distribution, fitting its mean and spread.

<!-- docs-declaration -->

```csharp
public static AndersonResult Test(ReadOnlySpan<double> x)
```

**Parameters** — `x` is the sample, at least two values, since the spread is estimated from it;
the span is read, never modified.

**Returns** — `AndersonResult`: the A² statistic, the p-value interpolated from Stephens' table,
and the table itself — the critical values for this sample size, and the significance levels they
belong to.

**Exceptions** — `ArgumentException` when the sample holds fewer than two values, or every value
is the same and there is no spread to standardise by.

**Example** — ten heights, and the table they are read against.

```csharp
using Lodestar.Stats;

double[] heights = [172.0, 168.0, 175.0, 180.0, 169.0, 171.0, 177.0, 165.0, 173.0, 179.0];

AndersonResult result = AndersonDarling.Test(heights);

double squared = Math.Round(result.Statistic, 4);   // => 0.1373
double smallest = result.CriticalValues[0];         // => 0.511
double atFivePercent = result.CriticalValues[2];    // => 0.685
```

`0.1373` is below every critical value in the table, so the sample is consistent with a normal one
at every level the table covers.

**Remarks — read the critical values, not the p-value.** The interpolation runs over a table from
15% down to 1%, and clamps at both ends, so the p-value cannot be reported outside `[0.01, 0.15]`
however far from normal the sample is. A sample that is wildly non-normal and one that is merely
suspicious both come back at `0.01`.

```csharp
using Lodestar.Stats;

double[] skewed = [1.0, 1.1, 1.2, 1.3, 1.4, 1.5, 1.6, 1.7, 1.8, 40.0];

double clamped = Math.Round(AndersonDarling.Test(skewed).PValue, 4);   // => 0.01
double shapiro = Math.Round(ShapiroWilk.Test(skewed).PValue, 8);       // => 1.7E-07
```

Both reject. Only [`ShapiroWilk.Test`](shapirowilk-test.md) says how emphatically, which is why
that one is the member to reach for when the number matters beyond "reject or not". This family's
own answer is the statistic against the table, and that is what
[`AndersonResult.CriticalValues`](andersonresult.md) is for.

**The critical values depend on the sample size**, because the distribution is fitted rather than
given: they are Stephens' constants divided by `1 + 0.75/n + 2.25/n²` and rounded to three
decimals, exactly as scipy scales them. A table read from a textbook without that scaling is the
asymptotic one and is wrong for a short sample.

**Applies to** — net10.0, netstandard2.0.

**See also** — [`ShapiroWilk.Test`](shapirowilk-test.md), [`AndersonResult`](andersonresult.md),
[`Bartlett.Test`](bartlett-test.md), the [Python equivalence table](../../../equivalence.md).
