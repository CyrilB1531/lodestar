# AndersonDarling.KSample

Tests whether several samples come from one distribution, `scipy.stats.anderson_ksamp`.

<!-- docs-declaration -->

```csharp
public static AndersonResult KSample(double[][] samples)
```

<!-- docs-declaration -->

```csharp
public static AndersonResult KSample(AndersonKSampleVariant variant, double[][] samples)
```

**Parameters** — `samples` are the samples, at least two, none empty, with at least two distinct
values between them. `variant` is which form of the statistic, scipy's `variant`;
[`AndersonKSampleVariant.Midrank`](andersonksamplevariant.md) by default.

**Returns** — an [`AndersonResult`](andersonresult.md): the normalised statistic, the p-value
interpolated from Scholz and Stephens' table and clamped to `[0.001, 0.25]`, and the critical
values at `25, 10, 5, 2.5, 1, 0.5, 0.1` percent.

**Exceptions** — `ArgumentException` when there are fewer than two samples, an empty one, a `NaN`,
or fewer than two distinct values. `ArgumentOutOfRangeException` when `variant` is not declared.

**Example** — three machines: the same centre, different spreads, so different distributions.

```csharp
using Lodestar.Stats;

double[] first = [20.1, 19.8, 20.3, 20.0, 19.9, 20.2, 20.1, 19.7];
double[] second = [20.4, 18.9, 21.2, 19.1, 21.0, 18.7, 20.8, 19.4];
double[] third = [20.0, 20.1, 19.9, 20.2, 19.8, 20.1, 20.0, 19.9];

AndersonResult result = AndersonDarling.KSample(first, second, third);

double statistic = Math.Round(result.Statistic, 6);   // => 1.560618
double pValue = Math.Round(result.PValue, 6);         // => 0.075272
```

**Remarks** — [`AndersonDarling.Test`](andersondarling-test.md) asks whether one sample is
normal; this asks whether several share any distribution at all. scipy fits a quadratic in the
critical values to the log of the levels and reads it at the statistic; **outside the table it
returns the table's end**, `0.25` or `0.001`, with a warning, which here is simply the clamped
value. scipy's `method=PermutationMethod()` has no counterpart: its draws are random.

**Applies to** — net10.0, netstandard2.0.

**See also** — [`AndersonKSampleVariant`](andersonksamplevariant.md),
[`KolmogorovSmirnov`](kolmogorovsmirnov.md) for two samples.
