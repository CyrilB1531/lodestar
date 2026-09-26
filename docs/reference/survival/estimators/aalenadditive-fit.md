# AalenAdditive.Fit

Fits the model to right-censored durations: lifelines' `fit`.

<!-- docs-declaration -->

```csharp
public static AalenSummary Fit(ReadOnlySpan<double> design, ReadOnlySpan<double> durations, ReadOnlySpan<bool> eventObserved, int featureCount, AalenOptions options = null)
```

<!-- docs-declaration -->

```csharp
public static AalenSummary Fit(ReadOnlySpan<double> design, ReadOnlySpan<double> durations, ReadOnlySpan<bool> eventObserved, ReadOnlySpan<double> weights, int featureCount, AalenOptions options = null)
```

The second overload takes lifelines' `weights_col`.

**Parameters** — `design` holds the covariates row-major, `featureCount` values per subject, with no
intercept column: [`AalenOptions.FitIntercept`](aalenoptions.md) adds it, last. `durations` holds one
non-negative, finite duration per subject, and `eventObserved` is `true` where it ends in the event.
`weights` holds one positive, finite weight per subject, or is empty for ones. `featureCount` is the
number of covariates, zero for a fit on the intercept alone. `options` sets the level, the intercept
and the two penalties; `null` takes the defaults.

**Returns** — an [`AalenSummary`](aalensummary.md): the increments at each event time, their
cumulative sums with variance and bounds, the slopes table, the concordance, and the predictions.

**Exceptions** — `ArgumentOutOfRangeException` when `featureCount` is negative. `ArgumentException`
in any of these cases:

- the spans do not match the subjects, or a value is not finite;
- a duration is negative;
- fewer than two subjects are given, or no subject has the event;
- a covariate does not vary beside the intercept, or there is no column at all;
- the weights are neither empty nor one positive, finite value per subject.

**Example** — a covariate that does not vary has no coefficient beside the intercept, and is refused.

```csharp
using Lodestar.Survival;

double[] months = [3, 10, 5, 4, 8, 18, 12, 6, 9, 20, 7, 15];
bool[] died = [true, true, true, false, true, true, true, true, true, false, true, true];
double[] everyone = [1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1];

string refusal;
try
{
    AalenAdditive.Fit(everyone, months, died, featureCount: 1);
    refusal = "fitted";
}
catch (ArgumentException error)
{
    refusal = error.ParamName!;
}

string parameter = refusal;  // => design
```

**Remarks** — **the subjects are sorted by duration and stepped through the distinct event times.**
At each, the increments solve `(XᵀX + (c₁ + c₂)I) v = Xᵀy + c₂ v₋`, over the rows of the subjects
at risk, with `y` the square root of each weight at that time's deaths and zero elsewhere, `c₁` and
`c₂` the two penalties of
[`AalenOptions`](aalenoptions.md) and `v₋` the previous time's increments. Each column is scaled to
unit sample deviation and each row weighted by the square root of its weight; the answer is rescaled
back. A system that is singular gives zero increments, as lifelines answers its `LinAlgError`:
**a covariate constant among the subjects left at risk** makes it so, and there LAPACK's pivot lands a
few ulps either side of zero by the order of its sums, so lifelines fails or solves by chance. This
answers zeros, without a penalty, wherever the pivot is within 8 ulps of its diagonal per subject at risk, the rounding
those sums leave growing with their length.

**The fit stops once no more than three subjects per covariate remain**, as lifelines does, so
[`EventTimes`](aalensummary.md) may end before the last event.

**lifelines' readings are kept on purpose**, so the numbers are lifelines':

- **a subject leaves the risk set only at an event time.** One censored between two event times is
  never removed, and stays at risk to the end: among the twelve patients above, the one censored at
  month 4 is still in the regression at month 18.
- `CumulativeVariance` is rescaled by the covariate's deviation, not its square;
  [`AalenSummary`](aalensummary.md) has it, with the concordance's own reading.

**Applies to** — net10.0, netstandard2.0.

**See also** — [`AalenAdditive`](aalenadditive.md), [`AalenSummary`](aalensummary.md),
[`AalenOptions`](aalenoptions.md).
