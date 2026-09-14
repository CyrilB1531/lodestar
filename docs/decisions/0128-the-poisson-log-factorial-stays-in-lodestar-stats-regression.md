---
status: accepted
supersedes: []
amends: []
applies: ["0095", "0111"]
---
# 0128 — The Poisson log-factorial stays in `Lodestar.Stats.Regression`, a table below 256 and a series above

**Status:** accepted · **Date:** 2026-09-14

## Context

[`GeneralizedLinearModel.Fit`](../reference/stats-regression/glm/generalizedlinearmodel-fit.md) refused a Poisson count above one million
([#616](https://github.com/CyrilB1531/lodestar/issues/616)). The log-likelihood's `log(y!)` was an
exact prefix-sum table as long as the largest count: 8 MB at the bound, and a wrapped index past
`int.MaxValue`. statsmodels evaluates `gammaln(y + 1)` per observation and has no bound.
[#665](https://github.com/CyrilB1531/lodestar/issues/665) asked for a constant-time log-gamma, held
to `gammaln` at a relative `1e-9` well past a million. It also asked for the table to be kept or
dropped on a measurement.

Two records bear on where that function lives.

- **[0095](0095-the-stats-numerical-layer-publishes-four-members-and-no-more.md)**, applied
  since by 0097, 0098 and others, keeps `Gamma.LogGamma` internal to `Lodestar.Stats`. It also
  refused copying the functions it did publish into the regression package: *"two incomplete betas
  in one repository disagree eventually, and the disagreement surfaces as two p-values for one
  statistic."*
- **[0111](0111-the-generalized-linear-model-does-not-earn-its-own-package.md)** found the GLM
  unable to take a member published by `Lodestar.Stats` on the branch that needed it.
  `Lodestar.Stats.Regression` reaches it through a published floor, so `log(y!)` was computed from
  the response itself, inside the regression package.

## Measured

**Precision**, against `scipy.special.gammaln(k + 1)`, relative error:

| `log(k!)` by | `k = 2` | `k = 3` to `10` | `20` to `1,000,000` | up to `2^53` |
| --- | ---: | ---: | ---: | ---: |
| the prefix-sum table, Kahan-compensated | 0 | 0 | 2.2e-16 | not buildable |
| `Gamma.LogGamma`'s Lanczos sum at `k + 1` | 1.4e-15 | 5.6e-16 | 6.3e-16 | 1.4e-16 |
| Stirling's series, three correction terms | 7.8e-7 | 4.5e-9 to 1.4e-15 | 6.6e-16 | 1.4e-16 |

**Cost**: `GlmPoissonBenchmarks`, one-regressor Poisson fits of 2,000 rows, BenchmarkDotNet default
job on an AMD Ryzen 7 8700G, `origin/main`, this change, then `origin/main` again:

| Mean count | `origin/main` | this change | Allocated before | Allocated after |
| ---: | ---: | ---: | ---: | ---: |
| 5 | 273.7 / 274.7 μs | 275.3 μs | 486.63 KB | 486.48 KB |
| 50,000 | 683.5 / 701.5 μs | **283.1 μs** | 943.73 KB | **486.48 KB** |
| 5,000,000 | refused | 579.7 μs | — | 957.06 KB |

## Decision

**`LogLikelihood.LogFactorial` stays internal to `Lodestar.Stats.Regression`.** It reads a fixed
table of `log(k!)` for `k` below 256, built once exactly as the old table was. Above that it takes
Stirling's series, which is within 6.6e-16 from 20 on. `regression_log_factorial.json` holds it to
`gammaln` at a relative `1e-9` up to `2^53`, the table's boundary included.

**The table sized by the largest count is dropped.** At a mean count of 50,000 it was 457 KB of the
fit's allocation and 2.4× its time, and it bought nothing the series lacks: 6.6e-16 against 2.2e-16,
both far inside `1e-9`. **The 256-entry table stays.** Below 20 the series is not accurate enough.
The table costs nothing measurable at a mean of 5, and every count under 256 keeps the double it
had, so no frozen small-count fit moves.

**The bound goes, and infinity is refused by name.** `+∞` truncates to itself, so the integer check
passed it, and only the bound had stopped it.

## Options

- **Publish `Distributions.LogGamma` from `Lodestar.Stats`, by 0095's rule.** Lost on 0111's
  constraint. The regression package consumes the published floor, so the member ships in one
  release and is consumed in the next. That is two pull requests and a floor rise, for a function
  whose one caller needs integer arguments only.
- **Reuse the Lanczos sum as a copy in the regression package.** Lost on 0095's own objection, since
  it would be a second copy of `Gamma.LogGamma`. It is also not exact on the small counts every
  frozen fit uses: 1.4e-15 at `k = 2` where the table is exact.
- **Stirling everywhere.** Lost: 7.8e-7 at `k = 2`.
- **Keep the table sized by the largest count, beside the series for large counts.** Lost on the
  measurement above: the table was the cost at every count past the fixed one.

**Why this is not the duplicate 0095 refused.** 0095 refused two implementations of one quantity
that two packages both report. `log(y!)` appears only in the Poisson log-likelihood and the AIC,
which only `Lodestar.Stats.Regression` computes, so no quantity is produced twice. `Gamma.LogGamma`
takes real arguments for the incomplete gamma inside `Lodestar.Stats`. This is a log-factorial of
integer counts, and neither package calls the other's.

## Consequences

- `docs/equivalence.md`'s `gammaln(y + 1)` row reads identical at any count, and the reference page
  no longer lists the bound among `Fit`'s refusals.
- **No frozen statsmodels fit has counts past a million.** One was added, with counts from one to six
  million. Its log-likelihood, -108.38, cancels per-row terms near `y log y`, around 1e8. Regenerated
  on the CI runner it read 3.7e-9 away from this machine's: a relative 3e-11, and past the absolute
  `1e-9` that [decision 0073](0073-the-oracle-gate-compares-numbers-not-bytes.md)'s gate reads. So the
  function's own corpus is the oracle for large counts. The small-count fits hold the formula it sits
  in, and a test fits a count of 2e9.
- A second caller of a real-argument log-gamma outside `Lodestar.Stats` would reopen publication
  under 0095. This record does not grant it.
