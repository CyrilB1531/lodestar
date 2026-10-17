# Wilcoxon.OneSample

Compares a sample of differences against a median of zero.

<!-- docs-declaration -->

```csharp
public static TestResult OneSample(ReadOnlySpan<double> differences, ZeroMethod zeroMethod = ZeroMethod.Wilcox, Alternative alternative = Alternative.TwoSided, Continuity continuity = Continuity.None, ExactMethod method = ExactMethod.Auto, NanPolicy nanPolicy = NanPolicy.Propagate)
```

**Parameters** — `differences` is the sample, which answers `NaN` for both when empty, given or left
by omission, as scipy's does; the span is read, never
modified. `zeroMethod` says what to do with differences that are exactly zero. `alternative` says
which tail the p-value covers. `continuity` says whether the normal approximation gets the
half-unit correction. `method` chooses the exact null distribution, the exhaustive permutation
test, its normal approximation, or a choice between them by the number of non-zero differences.
`nanPolicy` says what to do with a `NaN`; scipy's `nan_policy`, defaulting to
[`NanPolicy.Propagate`](../nanpolicy.md).

**Returns** — `TestResult`: the signed-rank statistic and the p-value. Two-sided, the statistic is the
smaller of the two rank sums; under [`Alternative.Less`](alternative.md) or [`Alternative.Greater`](alternative.md) it is the sum of the
positive ranks, which can be the larger one — scipy's `wilcoxon` reports the same.

**Exceptions** — `ArgumentException` when `nanPolicy` is `NanPolicy.Raise` and the sample holds a
`NaN`. `ArgumentOutOfRangeException` when `method` is
`ExactMethod.Exact` and the zero-method-processed sample exceeds 500 values. `ArgumentOutOfRangeException` when an option is outside its enumeration.

**Example** — seven differences, two of them exactly zero.

```csharp
using Lodestar.Stats;

double[] differences = [2.0, 0.0, 3.0, 0.0, 2.0, 3.0, 3.0];

TestResult result = Wilcoxon.OneSample(differences);

double w = result.Statistic;   // => 0
double p = result.PValue;      // => 0.0625
```

**Remarks — every difference tied at zero is a defined answer, not an error.** When
`zeroMethod` leaves nothing to rank — `ZeroMethod.Wilcox` drops every value — the statistic is
`0.0`. The exact and permutation nulls put all their mass there, a p-value of `1.0`; the normal
approximation, asked for or chosen by `Auto` past 13 values, has no variance and answers `NaN`, as
scipy's does. Pratt's zeros answer `NaN` from the normal approximation too, their exact null's own
p-value under `Exact`, and `1` from the permutation null `Auto` takes up to 13 values; ZSplit ranks the zeros and answers as scipy does.

**Under `NanPolicy.Propagate`, a NaN reaches the statistic and the p-value.** The check runs
before ranking — a NaN difference is neither greater than, less than nor equal to zero, so
unguarded it would fall into the zero group along with every genuine tie at zero.

**The exact route has a size bound this package added.** scipy's own signed-rank table is exact
for any `n`, but its total, `2^n`, overflows a `double` to `+Infinity` past `n = 1023`, and every
p-value it then divides would silently come out as exactly `0.0`. `ExactMethod.Exact` above 500
ranked values is refused with `ArgumentOutOfRangeException` instead, comfortably inside that
margin; `ExactMethod.Auto` never reaches the bound on its own, since it turns unconditionally
asymptotic above 50 values.

**Applies to** — net10.0, netstandard2.0.

**See also** — [`Wilcoxon.Paired`](wilcoxon-paired.md), [`TTest.OneSample`](ttest-onesample.md)
for the parametric counterpart, [`ZeroMethod`](zeromethod.md), [`ExactMethod`](exactmethod.md),
the [Python equivalence table](../../../equivalence.md).
