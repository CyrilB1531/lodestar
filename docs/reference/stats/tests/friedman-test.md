# Friedman.Test

Compares three or more treatments measured on the same blocks.

<!-- docs-declaration -->

```csharp
public static TestResult Test(double[][] treatments)
```

<!-- docs-declaration -->

```csharp
public static TestResult Test(NanPolicy nanPolicy, double[][] treatments)
```

The policy comes first because the treatments are a `params` array and C# allows no parameter
after one.

**Parameters** — `treatments` is one array per treatment, all the same length; entry `i` of each
is the measurement on block `i`. At least three treatments, as `scipy.stats.friedmanchisquare`
requires. `nanPolicy` says what to do with a `NaN`; scipy's `nan_policy`, defaulting to
[`NanPolicy.Propagate`](../nanpolicy.md).

**Returns** — `TestResult`: the Q statistic, and the upper-tail p-value.

**Exceptions** — `ArgumentException` when there are fewer than three treatments, a treatment is
empty, the treatments have different lengths, or `nanPolicy` is
[`NanPolicy.Raise`](../nanpolicy.md) and a measurement is `NaN`.

**Example — six wines scored by three judges, and what pairing buys.** Each judge is harsh or
generous in their own way, and ranking within a wine removes that before the treatments are
compared. The between-subjects test on the same numbers cannot.

```csharp
using Lodestar.Stats;

double[] strict = [7.0, 5.0, 8.0, 6.0, 9.0, 7.0];
double[] stricter = [6.0, 4.0, 7.0, 5.0, 8.0, 6.0];
double[] generous = [8.0, 7.0, 9.0, 8.0, 9.0, 8.0];

double paired = Math.Round(Friedman.Test(strict, stricter, generous).PValue, 6);      // => 0.003081
double unpaired = Math.Round(KruskalWallis.Test(strict, stricter, generous).PValue, 4); // => 0.0361
```

Both see a difference; the paired test sees it an order of magnitude more clearly, because the
spread between wines is noise to the question "do the judges differ" and ranking inside each wine
throws it away.

**Remarks — omission drops the whole block, not the measurement.** The treatments are paired
across blocks, so a block missing one treatment can no longer be ranked, and ranking the rest
would compare treatments on different subjects. Under [`NanPolicy.Omit`](../nanpolicy.md) the
block leaves entirely; under [`NanPolicy.Propagate`](../nanpolicy.md) the statistic and the
p-value are both `NaN`, because a `NaN` sorted into a ranking would otherwise take a finite rank
like any other value.

**Treatments that agree exactly on every block answer `NaN`.** Every rank is then tied, the tie
correction is exactly zero, and the statistic is `0/0`. scipy answers `NaN` there, where
[`KruskalWallis.Test`](kruskalwallis-test.md) refuses its analogous input with an exception — the
two references disagree, and each is matched rather than reconciled. The
[Python equivalence table](../../../equivalence.md) carries both rows.

**Applies to** — net10.0, netstandard2.0.

**See also** — [`KruskalWallis.Test`](kruskalwallis-test.md) for the unpaired counterpart,
[`Wilcoxon.Paired`](wilcoxon-paired.md) for two treatments rather than three, the
[Python equivalence table](../../../equivalence.md).
