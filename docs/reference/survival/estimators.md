# Survival estimators — `Lodestar.Survival`

A duration and whether it ended in the event. That pair, one per subject, is what every
estimator here takes, and it is the whole of what survival analysis needs to say something a
mean cannot.

## The problem censoring creates

A subject who leaves the study at month 20 without the event has not survived 20 months — they
have survived *at least* 20 months. Averaging durations throws that away, and dropping the
subject throws away more. **Right censoring** is a subject whose duration is a lower bound, and
the three estimators below are the ones that use it rather than discard it.

Left truncation, interval censoring, Cox regression and the accelerated-failure-time models are
each their own lot; nothing here does them.

## Which estimator?

```mermaid
flowchart TD
    A["What do you want to read off?"] --> B["the probability of surviving past t"]
    A --> C["the accumulated risk by t"]
    A --> D["whether two groups differ"]
    B --> E["KaplanMeier"]
    C --> F["NelsonAalen"]
    D --> G["LogRank"]
```

`KaplanMeier` and `NelsonAalen` are two readings of one table, and they share a timeline by
construction — the same `SurvivalStep[]`, built once. Where they part is at the end of a sample
whose last duration is observed: survival reaches zero and can fall no further, while the hazard
keeps the size of that last step.

## What all three share

- **The pair, as spans.** `durations` and `eventObserved`, the same length, durations
  non-negative. Anything else is refused with `ArgumentException`.
- **Ties are the point, not an edge case.** A time carrying three events is not three times
  carrying one, and the two estimators differ on exactly that — see
  [`NelsonAalen.Estimate`](estimators/nelsonaalen-estimate.md).
- **Time zero is a step**, with everyone at risk and nothing having happened. It is the shape
  lifelines' own event table has, and it makes a curve plottable without a special first point.
- **Each is a static class with no state**, so all three are safe to call from any number of
  threads at once.
- **Each is checked against `lifelines` 0.30.3**, and the corpora are in
  [`tests/oracles`](../../equivalence.md).

## Types

| Type | What it is |
| --- | --- |
| [`KaplanMeier`](estimators/kaplanmeier.md) | The survival function, with Greenwood variance and log-log bounds. |
| [`KaplanMeierCurve`](estimators/kaplanmeiercurve.md) | What it returns: the estimate, its bounds and its steps. |
| [`LogRank`](estimators/logrank.md) | The two-sample test comparing two survival curves. |
| [`LogRankResult`](estimators/logrankresult.md) | What it returns: a statistic, a p-value and the degrees of freedom. |
| [`NelsonAalen`](estimators/nelsonaalen.md) | The cumulative hazard function. |
| [`NelsonAalenCurve`](estimators/nelsonaalencurve.md) | What it returns: the accumulated hazard and its steps. |
| [`SurvivalStep`](estimators/survivalstep.md) | One step of either curve: a time, a risk set, and what happened at it. |

## See also

- [Survival analysis](../../guides/survival-analysis.md) — the guide, with a worked trial.
- [Python → C# equivalence](../../equivalence.md) — the `lifelines` call each of these replaces.
