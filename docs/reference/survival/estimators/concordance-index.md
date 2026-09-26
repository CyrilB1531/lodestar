# Concordance.Index

The share of comparable pairs a set of scores orders correctly, a tie counted one half.

<!-- docs-declaration -->

```csharp
public static double Index(ReadOnlySpan<double> eventTimes, ReadOnlySpan<double> predictedScores)
```

<!-- docs-declaration -->

```csharp
public static double Index(ReadOnlySpan<double> eventTimes, ReadOnlySpan<double> predictedScores, ReadOnlySpan<bool> eventObserved)
```

The first overload reads every time as an observed event, lifelines' `event_observed=None`.

**Parameters** — `eventTimes` holds one duration per subject. `predictedScores` holds one score per
subject, **higher for a longer predicted survival**. `eventObserved` holds one flag per subject,
`true` where its duration ends in the event.

**Returns** — the concordance index, between zero and one, a half for scores that carry no
information.

**Exceptions** — `ArgumentException` when the spans differ in length, a time or a score is `NaN`, or
no pair is comparable, where lifelines raises `ZeroDivisionError`.

**Example** — a censored subject is compared only against the events before it.

```csharp
using Lodestar.Survival;

double c = Concordance.Index([1, 2, 3, 4, 5], [1.5, 1.0, 3.5, 4.0, 6.0], [true, true, false, true, true]);

double index = c;   // => 0.875
```

**Remarks** — **a pair is comparable when the shorter duration ends in an event**, against any later
duration and against a censoring at the same duration; two events at one duration are not
comparable. The pair is concordant when the shorter duration carries the lower score, and a tie in
the scores counts one half. A hazard or a risk score orders the other way, so it is passed negated,
as lifelines asks; [`CoxSummary.ConcordanceIndex`](coxsummary.md) is this index on the negated linear
predictor. The walk sorts once and counts through a Fenwick tree, `n log n` in the subjects.

**Applies to** — net10.0, netstandard2.0.

**See also** — [`Concordance`](concordance.md), [`CoxSummary`](coxsummary.md).
