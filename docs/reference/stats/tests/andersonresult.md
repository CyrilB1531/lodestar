# AndersonResult

An Anderson-Darling result: the statistic, and the table it is read against.

<!-- docs-declaration -->

```csharp
public sealed record AndersonResult(double Statistic, double PValue, double[] CriticalValues, double[] SignificanceLevels)
```

**Properties** — `Statistic` is the A² statistic; larger means further from normal. `PValue` is
the p-value interpolated from the table, clamped to its ends. `CriticalValues` are the statistic's
critical values for this sample size, one per level. `SignificanceLevels` are those levels, in
percent, as scipy reports them: `15, 10, 5, 2.5, 1`.

**Example** — the statistic beside the level it fails first, if any.

```csharp
using Lodestar.Stats;

double[] heights = [172.0, 168.0, 175.0, 180.0, 169.0, 171.0, 177.0, 165.0, 173.0, 179.0];

AndersonResult result = AndersonDarling.Test(heights);

double fivePercent = result.SignificanceLevels[2];    // => 5
double criticalThere = result.CriticalValues[2];      // => 0.685
double observed = Math.Round(result.Statistic, 4);    // => 0.1373
```

**Remarks — two shapes in one record, because scipy is in the middle of replacing one with the
other.** Through 1.18, `scipy.stats.anderson` returns critical values at fixed significance levels
and no p-value at all. Since 1.17 that shape emits a `FutureWarning`, and from 1.19 it is removed:
`critical_values`, `significance_level` and `fit_result` go, and a `pvalue` interpolated from the
same table takes their place.

Both are carried here. The p-value is what a reader of the other twelve families expects to find,
and the critical values are what actually carries information, since the interpolation clamps and
cannot leave `[0.01, 0.15]`. The frozen corpus is built the same way for the same reason: the
statistic and the p-value are replayed from the shape that survives, and the critical values are
stated from Stephens' constants — then checked against the shape being removed, while it is still
there to check against. The [Python equivalence table](../../../equivalence.md) has the whole of
it.

Being a `record`, two results with the same numbers are equal, and both tables are compared value
by value rather than by reference — the generated equality would call two results holding the same
table unequal.

**Applies to** — net10.0, netstandard2.0.

**See also** — [`AndersonDarling.Test`](andersondarling-test.md),
[`ShapiroWilk.Test`](shapirowilk-test.md),
[`Chi2ContingencyResult`](chi2contingencyresult.md) for the other result carrying a table, the
[Python equivalence table](../../../equivalence.md).

## Members

| Member | What it does |
| --- | --- |
| [`AndersonResult.Equals`](andersonresult-equals.md) | Compares the two scalars and both tables, value by value. |
| [`AndersonResult.GetHashCode`](andersonresult-gethashcode.md) | Hashes the scalars and the table length, which is O(1). |
