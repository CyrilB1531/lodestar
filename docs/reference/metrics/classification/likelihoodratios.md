# LikelihoodRatios

How much a prediction should move a belief — and, unlike every other classification metric here,
**independently of how common the class is**.

A positive prediction multiplies the prior odds by [`Positive`](likelihoodratios-compute.md) and a
negative one by [`Negative`](likelihoodratios-compute.md). That is what makes the pair worth
reporting on a rare class: [`Precision`](precision.md) falls as the class gets rarer even though the
classifier has not changed, and these do not. A test asserts exactly that — holding sensitivity and
specificity fixed while adding negatives leaves both ratios where they were and moves precision.

## Two numbers, so a small type of its own

`class_likelihood_ratios` returns a pair, and the two are not interchangeable: `LR+` above `1` says a
positive prediction is evidence *for* the class, `LR-` below `1` says a negative prediction is
evidence *against* it. A tuple would have carried no names and no documentation, so this is a sealed
class with two named properties — the shape
[`RocCurve`](roccurve.md) and the other curves take, applied to two scalars instead of three arrays.

## Four ways a ratio has no value, and they do not answer alike

| what is missing | `Positive` | `Negative` |
| --- | --- | --- |
| nothing false-positive — specificity is `1` | undefined | a value |
| nothing true-negative — specificity is `0` | a value | undefined |
| no negative sample in the truth | undefined | undefined |
| **no positive sample in the truth** | `NaN`, replaced only if nothing is false-positive | `NaN`, replaced only if nothing is true-negative |

The last row is the one worth knowing. A ratio is replaced only where its own count vanishes, as the
reference replaces it; with no positive sample, the one that is not replaced is `0/0`, `NaN`.
Measured, with the replacement set to `1`: `([0, 0], [0, 1])` gives `(NaN, NaN)`, `([0, 0], [1, 1])`
gives `(NaN, 1)`, and a truth of all positives gives `(1, 1)` (#1250).

## Members

| Member | What it does |
| --- | --- |
| [`LikelihoodRatios.Compute`](likelihoodratios-compute.md) | Both ratios, from labels. |
