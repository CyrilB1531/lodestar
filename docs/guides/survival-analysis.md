# Survival analysis

A mean duration is the wrong summary when some durations are not over yet. This guide is about the
three estimators that handle that, using the trial they were published on.

## The problem: a duration that is a lower bound

Freireich's 1963 leukaemia trial followed 21 patients on a treatment and 21 on a placebo, and
recorded how long each stayed in remission. Nine of the treated patients relapsed. The other twelve
were still in remission when the study ended, or left it — so for them the record says *at least*
this long, not *exactly* this long. That is **right censoring**.

Two obvious things to do with those twelve are both wrong. Averaging their durations as if they were
relapse times understates survival. Dropping them throws away the twelve patients the treatment
worked best on, which overstates the relapse rate. The estimators here do neither: a censored patient
counts in the risk set until the moment they leave it, and contributes nothing after.

## The survival curve

```csharp
using Lodestar.Survival;

double[] treatment = [6, 6, 6, 7, 10, 13, 16, 22, 23, 6, 9, 10, 11, 17, 19, 20, 25, 32, 32, 34, 35];
bool[] relapsed = [true, true, true, true, true, true, true, true, true,
                   false, false, false, false, false, false, false, false, false, false, false, false];

KaplanMeierCurve curve = KaplanMeier.Estimate(treatment, relapsed);

double stillInRemissionAtSix = curve.Survival[1];  // => 0.857142…
int atRiskAtSix = curve.Steps[1].AtRisk;  // => 21
```

[`KaplanMeier.Estimate`](../reference/survival/estimators/kaplanmeier-estimate.md) is the call, and
three patients relapsed at week 6 out of 21 at risk, so the estimate steps down to `1 - 3/21`. The
curve is a **step function**, and `Steps` is where the arithmetic is legible: a time, who was at risk
immediately before it, and what happened.

One step in that table has no events at all — week 9, where a patient was censored. The estimate does
not move there. What moves is the denominator of every step after it, which is the whole mechanism.

## Reading the confidence bounds

`Lower` and `Upper` share the index of `Survival`, and they are **not** the estimate plus or minus
its standard error. They are built on the log-log transform of the estimate, which is what `lifelines`
reports by default, and the difference is not cosmetic:

| at week 6 | lower | upper |
| --- | --- | --- |
| log-log transform (what you get) | 0.6197 | 0.9516 |
| plain Greenwood interval | 0.7075 | **1.0067** |

The plain interval leaves `[0, 1]`. The transform cannot, which is the reason for it, and it also
means the bounds are **asymmetric** about the estimate — do not read the half-width as an error bar.

## The cumulative hazard, and why it is not the same reading

```csharp
using Lodestar.Survival;

double[] treatment = [6, 6, 6, 7, 10, 13, 16, 22, 23, 6, 9, 10, 11, 17, 19, 20, 25, 32, 32, 34, 35];
bool[] relapsed = [true, true, true, true, true, true, true, true, true,
                   false, false, false, false, false, false, false, false, false, false, false, false];

NelsonAalenCurve hazard = NelsonAalen.Estimate(treatment, relapsed);

double bySix = hazard.CumulativeHazard[1];  // => 0.150250…
```

[`NelsonAalen.Estimate`](../reference/survival/estimators/nelsonaalen-estimate.md) gives `0.150250…`,
which is `1/21 + 1/20 + 1/19` and **not** `3/21 = 0.142857`. With several events tied at one
time the increment is summed event by event, as though an instant separated them. That is the tie
correction `lifelines` applies, it is a 5% difference at the very first step here, and it is the one
place an implementation written from the plain definition disagrees with the reference.

`Steps` is the same table the survival curve carries, so the two are compared index by index with no
alignment. Where they part is the end of a sample whose last duration is observed: survival reaches
zero and stops, the hazard keeps the size of that last step.

## Comparing two arms

```csharp
using Lodestar.Survival;

double[] treatment = [6, 6, 6, 7, 10, 13, 16, 22, 23, 6, 9, 10, 11, 17, 19, 20, 25, 32, 32, 34, 35];
bool[] relapsed = [true, true, true, true, true, true, true, true, true,
                   false, false, false, false, false, false, false, false, false, false, false, false];
double[] control = [1, 1, 2, 2, 3, 4, 4, 5, 5, 8, 8, 8, 8, 11, 11, 12, 12, 15, 17, 22, 23];
bool[] controlRelapsed = [true, true, true, true, true, true, true, true, true, true, true,
                          true, true, true, true, true, true, true, true, true, true];

LogRankResult test = LogRank.Test(treatment, relapsed, control, controlRelapsed);

double statistic = test.Statistic;  // => 16.79…
int df = test.DegreesOfFreedom;  // => 1
```

[`LogRank.Test`](../reference/survival/estimators/logrank-test.md) does the comparison. At every time
an event occurs in either arm, the treated arm's observed relapses are compared against
what the pooled risk sets would hand it. The statistic accumulates the difference and its variance,
and its p-value is a chi-squared upper tail on one degree of freedom.

**The control arm being entirely uncensored does not make the comparison unfair**, and a fully
censored arm would not be excluded either: censored subjects sit in the pooled risk sets and raise
the other arm's expected counts, which is the opposite of dropping them.

Two sanity properties worth knowing: the statistic is a square, so it is never negative and the order
of the two arms cannot change it; and identical arms give exactly zero.

## When covariates matter: the Cox model

**Read the assumption before the table.** A Cox model says each covariate multiplies the hazard by
a fixed factor, the same factor at every time. That is the "proportional hazards" in its name. A
hazard ratio of 4 for a treatment means four times the hazard in the first month and in the
twentieth. If the effect fades or reverses, the one number the table reports is an average of
something that changed, and its p-value tests that average.

**This release does not test the assumption for you.** Before trusting a table, look for its
failure:

- **Plot the curves first.** Fit [`KaplanMeier`](../reference/survival/estimators/kaplanmeier.md) on
  each level of a binary covariate. Curves that cross are the clearest sign that its hazards are not
  proportional.
- **Fit an early window and a late one.** Censor everyone at a midpoint for the first fit, then fit
  only those still at risk after it. A coefficient that moves materially between the two is not
  constant in time.

Schoenfeld residuals, the formal test, are a later lot.

```csharp
using Lodestar.Survival;

// One row per patient: dose in mg, then 1 if on the new treatment.
double[] design = [1.0, 0.0, 2.0, 1.0, 1.5, 0.0, 3.0, 1.0, 2.5, 0.0,
                   0.5, 1.0, 2.0, 0.0, 1.0, 1.0, 3.5, 0.0, 0.5, 1.0];
double[] months = [12, 5, 20, 3, 15, 9, 8, 14, 2, 18];
bool[] died = [true, true, false, true, true, true, true, false, true, true];

CoxSummary summary = CoxProportionalHazards.Fit(design, months, died, featureCount: 2);

double perMilligram = summary.HazardRatios[0];  // 4.07: each mg multiplies the hazard by about four
double dosePValue = summary.PValues[0];         // 0.027
double treatment = summary.HazardRatios[1];     // 4.29, and its interval runs from 0.61 to 30.3
double concordance = summary.ConcordanceIndex;  // 0.774
```

[`CoxProportionalHazards.Fit`](../reference/survival/estimators/coxproportionalhazards-fit.md) takes
the same durations and event flags as the curves, plus a row-major design with one row per subject.
It reports each coefficient's hazard ratio with its interval, the likelihood-ratio test of the model
against no covariates, and Harrell's concordance: the share of comparable pairs whose ordering the
model gets right. A concordance of 0.5 is chance.

**Ten subjects cannot carry two covariates**, and the interval on the treatment above says so: it
covers both a protective and a harmful effect. The table is honest about that. It is the reader who
has to look at the interval before the ratio.

**Two designs are refused rather than fitted.**

- **A covariate the others determine**, such as a duplicated column or a column of ones, has no
  identifiable coefficient.
- **A covariate that separates the events**, where every higher value fails before every lower one,
  has an infinite coefficient.

Both throw `ArgumentException` naming the cause. `lifelines` would return a table behind a warning
there.

## What is not here

Right censoring only. **Left truncation** (subjects who enter late), **interval censoring** (an event
known only to fall between two visits) and the **accelerated-failure-time** models are each their own
lot — and each changes the risk table rather than adding a step on top of it, which is why none of
them is a flag on these calls. The Cox model has no strata, time-varying covariates, weights or
penalty, no prediction for a new subject, and no test of its own assumption.

## See also

- [Survival estimators](../reference/survival/estimators.md) — the reference pages.
- [Python → C# equivalence](../equivalence.md) — the `lifelines` call each of these replaces.
- [`decisions/0099`](../decisions/0099-survival-has-no-incumbent-and-scikit-survival-is-refused-on-its-licence.md)
  — why `lifelines` is the oracle and `scikit-survival` is refused.
- [`decisions/0124`](../decisions/0124-the-cox-model-stays-in-lodestar-survival-and-refuses-what-it-cannot-estimate.md)
  — where the Cox model lives, and the three places it parts from `lifelines`.
