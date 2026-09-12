---
status: accepted
supersedes: []
amends: []
applies: ["0008", "0095"]
---
# 0115 — The robust covariances come first, and the tail they need was already published

**Status:** accepted · **Date:** 2026-09-12 · **Applies:** [`0008`](0008-italian-enza-nltk-divergence.md), [`0095`](0095-the-stats-numerical-layer-publishes-four-members-and-no-more.md)

## Context

[`#686`](https://github.com/CyrilB1531/lodestar/issues/686) names two families of absence in
`Lodestar.Stats.Regression`, both recorded in `docs/equivalence.md`:

- **The covariances.** `sm.WLS`, `sm.GLS` and `.get_robustcov_results("HC0".."HC3")` are *"out of
  scope for 0.1.0"*.
- **The links.** The GLM wires logit and log; *"the other families `sm.families` publishes
  (Gamma, inverse Gaussian, negative binomial, Tweedie) have no counterpart yet."*

Taken together that is several lots. This record fixes the order and says what each later one
waits for, so the sequence is a decision rather than whatever gets written next.

## Decision

**The heteroskedasticity-consistent covariances come first**, and ship here: `CovarianceType`
with `Nonrobust`, `Hc0`, `Hc1`, `Hc2` and `Hc3`, chosen on `OlsOptions` and echoed on
`OlsSummary`.

They come first because of what this package claims to be. `README.md` sells it as the
**inference table** — *"coefficients everywhere and inference nowhere"* — and a robust covariance
is what an analyst reaches for when the assumption behind the standard errors fails, which is the
ordinary case rather than the exotic one. Shipping the table without HC0–HC3 ships the numbers
that are easiest to compute and hardest to defend. Nothing else on the list has that property:
a missing family stops a fit outright, which is visible, where a standard error computed under an
assumption nobody checked is wrong and silent.

**The order after it**, each opening when the one before has shipped:

1. `WLS`, which is this solve over rows scaled by the square root of a weight — the operation
   `LeastSquares` already performs for IRLS every iteration.
2. Negative binomial, the family a count model reaches for when Poisson's variance assumption
   fails, and the natural sibling of the Poisson that ships.
3. Gamma, with the `.scale` handling `docs/equivalence.md` has already written against.
4. `GLS`, which is not the same solve: it needs a full covariance from the caller and a whitening
   factorization, so it is a lot rather than a parameter.
5. Inverse Gaussian and Tweedie, **deferred under [decision 0095](0095-the-stats-numerical-layer-publishes-four-members-and-no-more.md)'s
   rule** — nothing has asked, and publishing later is always available.

## The distribution moves, and that is parity rather than a choice

Under any `cov_type`, `statsmodels` flips `use_t` from `True` to `False`. Measured rather than
read: the coefficient p-values become `2·Φ̄(|z|)` against the normal, the interval multiplier
becomes the normal quantile, and — asymmetrically — **`f_pvalue` stays on the F distribution**,
`f.sf(fvalue, k, df_resid)` matching to the last bit while `chi2.sf(fvalue·k, k)` does not.

Reproducing that follows [decision 0008](0008-italian-enza-nltk-divergence.md)'s standing
tie-breaker: parity with the library a user migrates from, over what is tidier. A reader who
expected both tests to move together would be wrong about statsmodels, and a package that quietly
corrected the asymmetry would make every p-value disagree with the reference for a reason no
oracle could explain.

The cost is that `OlsSummary.TStatistics` holds a *z* under a robust type, which is what
statsmodels' own `tvalues` does. **`OlsSummary.CovarianceType` exists for that**: a summary that
changes the meaning of two of its properties has to carry the thing that says so, or the shape
advertises a promise it does not keep — which is [`#668`](https://github.com/CyrilB1531/lodestar/issues/668)'s
lesson applied before the fact rather than after it.

## Decision 0095's rule did not have to fire

The normal p-value needs `Φ̄`, and `Distributions` publishes five members of which that is not
one — `Normal.Sf` is on the list [0095](0095-the-stats-numerical-layer-publishes-four-members-and-no-more.md)
explicitly keeps internal. The obvious move was the one 0097 and 0098 already made twice:
publish it, bump `Lodestar.Stats`, release it, then floor on it — a two-package sequence across
two pull requests.

It was not needed. **The square of a standard normal is chi-squared on one degree of freedom**, so
the two-sided normal p-value is `ChiSquaredSf(z², 1)`, and that member has been public since
[0097](0097-the-chi-squared-tail-joins-the-published-four.md). The identity is exact, not an
approximation, and measured against `scipy` it agrees to **1.8e-16 relative at z = 0.5 and 1.1e-13
at z = 37** — the worst case being a p-value of 1e-299, and the tolerance the corpora compare at
being 1e-9.

So the rule this record applies is 0095's, by reading it and finding it did not apply: *publishing
later is always available* has a companion worth writing down — **before asking the layer to
publish, check whether what it already publishes answers the question.** Two of the four published
tails are related by an identity to a third, and that will not be the last time.

## Options that lost

- **Publish `Normal.Sf` anyway, for the name.** `ChiSquaredSf(z * z, 1.0)` needs a comment where
  `NormalSf(z)` would not. Refused because the comment is two lines and the alternative is a
  release of another package to add a member nothing else wants — and an unused published member
  cannot be unpublished, which is the asymmetry 0081 and 0095 are both built on.
- **Keep Student's t under a robust covariance.** Defensible on its own terms, and it would make
  `TStatistics` mean one thing always. Refused on 0008's rule: every p-value would then disagree
  with `statsmodels` for a migrating reader, and the oracle would have to be weakened to let it.
- **Name the properties `Statistics` and drop `TStatistics`.** A breaking rename that would say
  what the numbers are under both modes. Refused as out of scope here, and it is worth recording
  that it was refused *for scope* rather than on merit: `0.x` is where such a rename is cheap, and
  [`#689`](https://github.com/CyrilB1531/lodestar/issues/689) is where that window is tracked.
- **Take WLS in the same lot.** It shares the solve and would have been cheap. Refused because it
  changes what a fit *is* rather than how its covariance is estimated, and a reviewer reading one
  branch should not have to hold both.

## Consequences

- `Lodestar.Stats.Regression` reaches 0.3.0, a minor bump: new members, no behaviour changed for
  a caller who does not ask for one.
- The corpus grows from six cases to twelve, six of them robust, and the four types are fitted on
  **one** heteroskedastic design rather than four — they differ only in a row weight, so a shared
  design tells them apart where separate ones would confound the estimator with the data.
- `LeastSquares.Solve` returns its factorization instead of discarding it, because the leverages
  HC2 and HC3 need are a row of Q against itself. The IRLS caller discards it, so nothing pays for
  what only one mode uses.
- A third tight loop now reads `QrDecomposition.Q` through `IReadOnlyList<double>`, which is
  [`#669`](https://github.com/CyrilB1531/lodestar/issues/669)'s subject. This record does not act
  on it; it adds one more consumer to the case that issue is making.
- A leverage of exactly one leaves HC2 and HC3 dividing by zero. statsmodels returns infinity
  rather than raising, and so does this — the row is what is degenerate, and a summary naming it
  is more use than an exception naming the call.
