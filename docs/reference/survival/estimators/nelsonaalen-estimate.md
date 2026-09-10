# NelsonAalen.Estimate

The cumulative hazard of a right-censored sample.

<!-- docs-declaration -->

```csharp
public static NelsonAalenCurve Estimate(ReadOnlySpan<double> durations, ReadOnlySpan<bool> eventObserved)
```

**Parameters** — `durations` holds one non-negative time per subject; `eventObserved` is `true`
where that time ends in the event. The two spans must be the same length.

**Returns** — a `NelsonAalenCurve` whose `Steps` and `CumulativeHazard` share one index.

**Exceptions** — `ArgumentException` when the spans differ in length, the sample is empty, or a
duration is negative or `NaN`.

**Example** — the increment at a time with three tied events.

```csharp
using Lodestar.Survival;

NelsonAalenCurve curve = NelsonAalen.Estimate([6, 6, 6, 7], [true, true, true, true]);

double atSix = curve.CumulativeHazard[1];  // => 1.083…
```

**Remarks** — **the increment is not `d/n`.** With `d` events tied at one time it is the sum of
`1 / (n - i)` over those events, as though they had been separated by an instant each. Three events
among 21 at risk give

```text
1/21 + 1/20 + 1/19 = 0.150251     and not     3/21 = 0.142857
```

which is a 5% difference at the very first step of the Freireich arm. This is the tie correction
`lifelines` applies by default, and it is the single place an implementation written from the plain
definition will disagree with it. The frozen corpus is what caught it here — four of the eight
samples carry ties on purpose for that reason.

Censorings contribute nothing to the sum and everything to the denominators after them, which is
the same mechanism Kaplan-Meier uses and why the two share a step table.

**Applies to** — net10.0, netstandard2.0.

**See also** — [`NelsonAalen`](nelsonaalen.md), [`KaplanMeier.Estimate`](kaplanmeier-estimate.md).
