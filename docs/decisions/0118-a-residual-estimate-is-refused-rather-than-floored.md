---
status: accepted
supersedes: []
amends: []
applies: ["0070", "0095"]
---
# 0118 — A residual estimate is refused rather than floored, and the interval width varies

**Status:** accepted · **Date:** 2026-09-12 · **Applies:** [`0070`](0070-k-greater-than-n-returns-an-infinite-interval.md), [`0095`](0095-the-stats-numerical-layer-publishes-four-members-and-no-more.md)

## Context

`docs/guides/conformal.md` has carried the same admission since the package shipped:

> That is a real limitation, not a simplification … **this package does not ship one yet.**

Every interval `SplitConformal` produced had the **same width for every input**, because
`AbsoluteResiduals` scores each calibration point by `|y − ŷ|` and the quantile of those is one
number. On data whose error varies with the input — most data — that is too wide where the model is
confident and too narrow where it is not, while still carrying the marginal coverage guarantee.

That combination is the trap rather than the inconvenience. The guarantee is what a reader comes
for, and it is exactly what makes a constant width easy to mistake for an adequate one.
[`#683`](https://github.com/CyrilB1531/lodestar/issues/683) is that gap, and it matters more here
than the equivalent gap elsewhere because the survey behind
[`#441`](https://github.com/CyrilB1531/lodestar/issues/441) found **no C# implementation of
conformal prediction at all** — a sole implementation sets what .NET thinks the technique is.

## What MAPIE does, measured

`ResidualNormalisedScore`'s signed score is `(y − ŷ) / r̂` and its interval is `ŷ + score · r̂`,
where `r̂` is a second model's prediction of `|y − ŷ|`. Reproduced end to end against MAPIE 1.5.0
with both estimators prefit, the bounds agree to **0.0 — not a tolerance, exactly**, and the widths
on that fixture ran from 4.01 to 10.11.

One contract is worth writing down because it cost an hour: under `prefit=True`, MAPIE calls the
residual estimator's `predict` and uses the result **as `r̂`**. Its own non-prefit path trains on
`log |y − ŷ|` and exponentiates, and a prefit model that returns the log instead of the residual is
silently thresholded at `eps` — the warning MAPIE raises says so, and the intervals it then
produces are off by eight orders of magnitude rather than wrong by a little.

## Decision

**`NormalisedResiduals` and `NormalisedInterval`, taking `r̂` and not the model that produced it.**

That follows the shape every member of this package already has: the caller owns the models, and
what is written down is the arithmetic that carries the guarantee. A `residual_estimator` parameter
would mean an estimator interface, a fit, and a package that suddenly has opinions about where a
model comes from — which is what `prefit` exists to avoid on the other side.

**A non-positive or `NaN` estimate is refused.** MAPIE floors at `1e-8` instead, and the divergence
is deliberate:

- MAPIE floors because **its own** residual model may predict a negative, and it has nowhere to
  send the complaint mid-pipeline.
- Here the estimate is the caller's own argument. Flooring would turn their bug into an interval of
  width `q · 1e-8`, which is not an error and not a wide interval — it **reads as certainty**, and
  the reader most likely to hit it is the one who fitted the residual model on the residual rather
  than on its log.

This is [decision 0070](0070-k-greater-than-n-returns-an-infinite-interval.md)'s reasoning applied
and its direction reversed, which is worth being explicit about. There, MAPIE raised and this
package returned an infinite interval, because a trivial-but-honest answer beats an exception. Here
MAPIE answers and this package raises — for the same reason read the other way: `q · 1e-8` is not
an honest answer, it is a confident one produced from an input that carries no information.

**`GammaConformityScore` waits**, under [decision 0095](0095-the-stats-numerical-layer-publishes-four-members-and-no-more.md)'s
rule. Nothing has asked; publishing later is always available and unpublishing never is. It is also
a different kind of thing — a multiplicative score for a strictly positive target — rather than the
adaptive-width fix this issue was about.

## Options that lost

- **Floor at `1e-8`, for parity.** [Decision 0008](0008-italian-enza-nltk-divergence.md)'s rule is
  parity with the library a user migrates from, and this is a real cost against it: a migrating
  caller whose pipeline relied on the floor gets an exception here. Refused because the behaviour
  being matched is a defence against MAPIE's own internals, not a promise to its users — and
  `docs/equivalence.md` records the divergence where a migrating reader will meet it.
- **Take the residual model instead of its predictions.** Closer to MAPIE's surface, and it would
  let this package apply the `log`/`exp` convention itself rather than documenting it. Refused: it
  needs an estimator abstraction that nothing else here has, and the first thing it would have to
  do is offer a `prefit` escape hatch.
- **Return `PositiveInfinity` for a non-positive estimate**, the way `Quantile` does for `k > n`.
  Consistent-looking and wrong: an infinite quantile is a *correct* statement about a calibration
  set too small to support the level, where a non-positive `r̂` is an input that cannot be
  interpreted at all.
- **One `Interval` overload taking an optional estimate**, rather than a second member. Fewer names,
  and it would hide the decision: an interval whose width varies is a different promise, and the
  call site should say which one it made.

## Consequences

- `Lodestar.Conformal` reaches 0.2.0 — new members, nothing existing moved.
- The corpus grows by three cases, and every one of them has estimates that **vary**: a constant
  estimate reduces the score to the absolute residual divided by a number, so a fixture built that
  way would pass while testing nothing. A test asserts the widths differ by more than a factor of
  two, which the corpus alone cannot catch.
- `docs/guides/conformal.md`'s limitation paragraph becomes a section about choosing between the
  two scores. The **exchangeability** caveat keeps leading the guide: a normalised score does not
  weaken it, and a reader should not be able to infer that it does.
- This record is `0117`, and [`#703`](https://github.com/CyrilB1531/lodestar/pull/703) is open
  holding a `0116` that collides with the one already on `main`. If that branch renumbers into
  `0117` the two collide in turn; `0118` is free.
