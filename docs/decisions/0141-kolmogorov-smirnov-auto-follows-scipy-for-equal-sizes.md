---
status: accepted
supersedes: []
amends: []
applies: []
---
# 0141 — Kolmogorov-Smirnov's `Auto` follows scipy for two samples of the same size

**Status:** accepted · **Date:** 2026-09-16

## Context

[`KolmogorovSmirnov.TwoSample`](../reference/stats/tests/kolmogorovsmirnov-twosample.md) with [`ExactMethod.Auto`](../reference/stats/tests/exactmethod.md) took the exact branch while
`a.Length * b.Length` was at most 10,000, because the exact branch walked an `(n+1)×(m+1)` table.
scipy's `ks_2samp(method="auto")` is exact while `max(n, m)` is at most 10,000 (`MAX_AUTO_N`) and
asymptotic above, measured against scipy 1.18.1: at n = m = 1,000 the two defaults returned p-values
4.2% apart on the corpus pair. [#756](https://github.com/CyrilB1531/lodestar/issues/756) gave two
samples of the same size a closed form, Hodges' exceedance probability, O(n) with no table — so for
that case the reason for the lower threshold is gone.

## Decision

**For two samples of the same size and a two-sided alternative, `Auto` is exact while n is at most
10,000, as scipy's is, and [`ExactMethod.Exact`](../reference/stats/tests/exactmethod.md) is no longer refused past the 1,000,000 product
bound**, which exists for the table's allocation and the closed form has none.

**This changes a default p-value**, toward the reference: between n = 101 and n = 10,000 a caller
who passed nothing now gets scipy's exact p-value instead of the asymptotic one. **A caller who wants
the old answer passes [`ExactMethod.Asymptotic`](../reference/stats/tests/exactmethod.md).**

## What was refused

- **Moving the threshold for unequal sizes and one-sided alternatives.** Those still walk the table,
  whose cost is O(n·m); matching scipy there means its path-counting method, a lot of its own worth
  opening when a caller needs it. `docs/equivalence.md` keeps that divergence.
- **Taking the closed form past 10,000 by default.** It would be cheap, but scipy is asymptotic there,
  and parity with the reference is the point of the default.

## Consequences

- `tests/oracles/stats_ks.json` pins n = 1,000 and 10,000 exact under `Auto`, 10,001 asymptotic, and
  10,001 under `Exact` answered rather than refused.
- The equivalence row narrows to unequal sizes and one-sided alternatives.
