---
status: accepted
supersedes: []
amends: []
applies: ["0008"]
---
# 0115 — `nan_policy` is offered where scipy offers it, and omission is a filter

**Status:** accepted · **Date:** 2026-09-12

## Context

scipy's `nan_policy` is `'propagate'`, `'raise'` or `'omit'`. A caller arriving from scipy with
real data arrives with missing values, and this package had no answer:
[#687](https://github.com/CyrilB1531/lodestar/issues/687).

Two facts shaped the answer, both measured rather than read.

**`'propagate'` is already the shipped behaviour.** `docs/equivalence.md` records that nine of the
ten families follow scipy's default exactly, and that
[`ChiSquare.Contingency`](../reference/stats/tests/chisquare-contingency.md) is the exception,
refusing a `NaN` cell because a contingency table's cells are counts whose marginals are divided
by.

**scipy does not give `nan_policy` to every test.** Read from scipy 1.18.1's signatures,
`chi2_contingency`, `fisher_exact` and `false_discovery_control` do not take it. The issue's
premise that "every test above takes `nan_policy`" is wrong on those three.

## Decision

**A three-valued `NanPolicy` enum, on the eleven entry points whose scipy counterpart takes
`nan_policy`, defaulting to `Propagate`.**

```csharp
public enum NanPolicy { Propagate = 0, Raise, Omit }
```

`Propagate = 0` makes `default(NanPolicy)` the same thing, and no existing call changes behaviour.

[`ChiSquare.Contingency`](../reference/stats/tests/chisquare-contingency.md),
[`FisherExact.Test`](../reference/stats/tests/fisherexact-test.md) and the three
`MultipleComparisons` methods do **not** take it. That is parity, not an omission.

Parity with the library a caller migrates from is this repository's tie-breaker
([`0008`](0008-italian-enza-nltk-divergence.md)). It settles the argument that `'propagate'` and
`'raise'` are each one line at a call site so only `'omit'` carries content: true, and outweighed,
because a caller porting a script wants the parameter the script already passes, and a two-valued
subset of a three-valued parameter is a divergence to explain.

### Omission drops pairs where the inputs are aligned

| entry point | omission |
| --- | --- |
| [`TTest.Paired`](../reference/stats/tests/ttest-paired.md), [`Wilcoxon.Paired`](../reference/stats/tests/wilcoxon-paired.md) | listwise — drop the index when either side is `NaN` |
| [`ChiSquare.GoodnessOfFit`](../reference/stats/tests/chisquare-goodnessoffit.md) with `expected` given | listwise |
| the other eight | per-sample |

Measured, not reasoned: `ttest_rel` on `[1, 2, NaN, 4, 5]` against `[2, NaN, 3, 5, 7]` with
`nan_policy='omit'` returns `statistic = -4.0, df = 2` — three pairs kept, which is listwise and
not what dropping each sample independently gives.

### Omission is a filter, not a second policy

The family's own guards run afterwards, unchanged.
[`ShapiroWilk.Test`](../reference/stats/tests/shapirowilk-test.md) still raises below `n = 3`
and [`KruskalWallis.Test`](../reference/stats/tests/kruskalwallis-test.md) still raises on a
fully tied pool, where scipy returns `(nan, nan)` with a warning — the two divergences
`docs/equivalence.md` already records.

Answering scipy's `(nan, nan)` on a path reached only through omission was rejected: it would make
one degenerate input raise or not according to how it arrived.

### Two consequences worth recording

[`ChiSquare.GoodnessOfFit`](../reference/stats/tests/chisquare-goodnessoffit.md) with an explicit
`expected` will commonly raise under `Omit`, because `chisquare` requires the two to sum alike and
omission breaks that by construction. scipy raises `ValueError`; this package already raises for
the same reason.

[`OneWayAnova.Test`](../reference/stats/tests/onewayanova-test.md) and
[`KruskalWallis.Test`](../reference/stats/tests/kruskalwallis-test.md) are `params double[][]`,
and C# forbids a parameter after a `params` array. They take an overload with the policy
**first** — `Test(NanPolicy nanPolicy, params double[][] groups)` — which is `string.Join`'s
shape and keeps the varargs form a caller already uses.

## Consequences

Eleven entry points gain an optional parameter; no existing call changes behaviour. `Raise` throws
`ArgumentException`, the package's idiom.

No `axis` parameter. scipy's `nan_policy` belongs to an array-API surface that also carries
`axis`; this package takes spans and single samples, where `axis` has no meaning.
