# ParametricOptions

What a [`ParametricSurvival`](parametricsurvival.md) fit may be told.

<!-- docs-declaration -->

```csharp
public sealed record ParametricOptions
```

**Properties** — `ConfidenceLevel` is the two-sided level the intervals are reported at; `0.95` by
default. `MaximumIterations` is how many Newton steps the fit may take; `100` by default.
`Breakpoints` are the piecewise exponential's, lifelines' `breakpoints`: positive, finite and
strictly ascending, empty by default, and read by no other model.

**Exceptions** — `ArgumentOutOfRangeException` when `ConfidenceLevel` does not lie strictly inside
`(0, 1)`, or `MaximumIterations` is below one. `ArgumentException` when a breakpoint is not positive
and finite, or the breakpoints do not strictly ascend. Each is thrown where the setting is set, not
where the fit reads it.

**Example** — one breakpoint at eight months: a constant hazard before it, another after.

```csharp
using Lodestar.Survival;

double[] months = [5, 8, 12, 3, 15, 9, 20, 6, 11, 14];
bool[] died = [true, true, false, true, true, true, false, true, true, false];

ParametricFit pieces = ParametricSurvival.Fit(ParametricModel.PiecewiseExponential, months, died,
    new ParametricOptions { Breakpoints = [8.0] });

string second = pieces.ParameterNames[1];   // => lambda_1_
double early = pieces.Parameters[0];        // => 17.5
double late = pieces.Parameters[1];         // => 11
```

**Remarks** — **each piecewise rate is a mean time, not a hazard**: `lambda_0_` is the months at risk
before the breakpoint over the events there, 70 over 4, so its hazard is `1 / 17.5` a month. The
breakpoints split time, not subjects.

**Applies to** — net10.0, netstandard2.0.

**See also** — [`ParametricSurvival.Fit`](parametricsurvival-fit.md), [`ParametricFit`](parametricfit.md).
