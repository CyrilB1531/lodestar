# RobustScalerOptions

Which steps [`RobustScaler`](robustscaler.md) applies, and between which percentiles it scales.

<!-- docs-declaration -->

```csharp
public sealed record RobustScalerOptions
```

**Properties** — `WithCentring` subtracts each feature's median and `WithScaling` divides by its
interpercentile range, both on by default. `LowerPercentile` and `UpperPercentile` are that range,
defaulting to the quartiles. `UnitVariance` divides that range again, by the normal quantiles of the
two percentiles, so a normal column comes out with a standard deviation of 1 — the reference's
`with_centering`, `with_scaling`, `quantile_range` and `unit_variance`, same defaults.

**Example** — the deciles instead of the quartiles, centring turned off, and unit variance.

```csharp
using Lodestar.Preprocessing;

double[] samples = [0.0, 1.0, 2.0, 3.0, 4.0, 5.0, 6.0, 7.0, 8.0, 9.0, 10.0];

RobustScaler deciles = RobustScaler.Fit(
    samples, 1, new RobustScalerOptions { LowerPercentile = 10.0, UpperPercentile = 90.0 });

double wider = deciles.Scale![0];  // => 8

RobustScaler uncentred = RobustScaler.Fit(
    samples, 1, new RobustScalerOptions { WithCentring = false });

bool noMedian = uncentred.Centre is null;  // => True

// 1.3489795 is the normal quantile gap at the quartiles, so a range of 8 becomes 5.93.
RobustScaler unit = RobustScaler.Fit(
    samples, 1, new RobustScalerOptions { UnitVariance = true });

double standardised = unit.Scale![0];  // => 3.706505546264003
```

**Remarks** — **each switch decides exactly one statistic**, which is worth stating because
[`StandardScalerOptions`](standardscaleroptions.md)'s pair does not: there, turning centring off
still fits a mean. Here, `WithCentring = false` leaves `Centre` null and nothing else moves.

A wider percentile range makes the scale larger and the scaled values smaller; the reference offers
it for the same reason it offers the quartiles, and refuses anything outside `0 ≤ lower ≤ upper ≤ 100`.

The last digit is worth a word: `scipy` reports `3.706505546264005` for the same column, two units in
the last place away, because its normal quantile and this one are different implementations of the
same function. The corpus compares at `1e-9` relative, which is four orders of magnitude wider.

**`UnitVariance` divides the range after the near-constant floor, not before**, which is the
reference's order and is visible only on a constant feature: it lands on `1/1.3489795`, not on 1.
The quantile is `Lodestar.Stats`' published one — the edge
[decision 0138](../../../decisions/0138-lodestar-preprocessing-takes-an-edge-on-lodestar-stats-for-the-normal-quantile.md)
took rather than carry a second copy. **A percentile of 0 or 100 is refused with it**, having no
finite quantile, where the reference divides by an infinity and reports a scale of zero.

**Applies to** — net10.0, netstandard2.0.

**See also** — [`RobustScaler`](robustscaler.md), [`RobustScaler.Fit`](robustscaler-fit.md).
