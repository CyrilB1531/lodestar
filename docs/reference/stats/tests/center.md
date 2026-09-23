# Center

Which centre Levene's test measures each group's spread around.

<!-- docs-declaration -->

```csharp
public enum Center { Median, Mean, Trimmed }
```

**Members** — `Median` is the group's median, scipy's `'median'`, and the variant Brown and
Forsythe published; it is the default on both sides. `Mean` is the group's mean, scipy's
`'mean'`, and Levene's original test. `Trimmed` is the mean of what is left after dropping each
end, scipy's `'trimmed'`.

**Example** — the same three groups under two centres, which answer differently.

```csharp
using Lodestar.Stats;

double[] first = [20.1, 19.8, 20.3, 20.0, 19.9, 20.2, 20.1, 19.7];
double[] second = [20.4, 18.9, 21.2, 19.1, 21.0, 18.7, 20.8, 19.4];
double[] third = [20.0, 20.1, 19.9, 20.2, 19.8, 20.1, 20.0, 19.9];

double median = Math.Round(Levene.Test(first, second, third).Statistic, 4);
double trimmed = Math.Round(
    Levene.Test(Center.Trimmed, 0.25, NanPolicy.Propagate, first, second, third).Statistic, 4);

double brownForsythe = median;   // => 45.3383
double quarterTrimmed = trimmed; // => 46.5678
```

**Remarks — the choice is about what an outlier should be allowed to do.** Centring on the mean
lets one stray value pull the centre toward itself and inflate every deviation in that group,
which reads as extra spread. The median does not move, so the group's own outlier shows up as one
large deviation instead of many moderate ones. Trimming sits between the two, dropping
`int(n × proportionToCut)` values from each end of the sorted group before averaging — truncated,
which is `scipy.stats.trim_mean`'s rule, so a small group and a small proportion can trim nothing
at all and leave `Trimmed` identical to `Mean`.

**Applies to** — net10.0, netstandard2.0.

**See also** — [`Levene.Test`](levene-test.md), [`Bartlett.Test`](bartlett-test.md), the
[Python equivalence table](../../../equivalence.md).
