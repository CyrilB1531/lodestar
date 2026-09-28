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

**Example** — one breakpoint at ten months: a constant hazard before it, another after.

```csharp
using Lodestar.Survival;

double[] months = [5, 8, 12, 3, 15, 9, 20, 6, 11, 14];
bool[] died = [true, true, false, true, true, true, false, true, true, false];

ParametricFit pieces = ParametricSurvival.Fit(ParametricModel.PiecewiseExponential, months, died,
    new ParametricOptions { Breakpoints = [10.0] });

string second = pieces.ParameterNames[1];   // => lambda_1_
double early = pieces.Parameters[0];        // => 16.2
double late = pieces.Parameters[1];         // => 11
```

**Remarks** — **each piecewise rate is a mean time, not a hazard**: `lambda_0_` is the months at risk
before the breakpoint over the events there, 81 over 5, so its hazard is `1 / 16.2` a month. The
breakpoints split time, not subjects. An event exactly on a breakpoint takes the mean of the two
pieces' hazards, as lifelines does, so it has no such closed form
([#1309](https://github.com/CyrilB1531/lodestar/issues/1309)).

Reading `Breakpoints` returns a copy: the array the `init` validated cannot be rewritten after it
([#1307](https://github.com/CyrilB1531/lodestar/issues/1307)).

**Applies to** — net10.0, netstandard2.0.

**See also** — [`ParametricSurvival.Fit`](parametricsurvival-fit.md), [`ParametricFit`](parametricfit.md).

## Members

| Member | What it does |
| --- | --- |
| [`ParametricOptions.Equals`](parametricoptions-equals.md) | Value equality, the breakpoints element by element. |
| [`ParametricOptions.GetHashCode`](parametricoptions-gethashcode.md) | A hash consistent with it. |
