---
status: accepted
supersedes: []
amends: []
applies: ["0095"]
---
# 0125 — The factorization types keep `IReadOnlyList<double>`, and a hot consumer reads the property into a local

**Status:** accepted · **Date:** 2026-09-14 · **Applies:** [`0095`](0095-the-stats-numerical-layer-publishes-four-members-and-no-more.md)

## Context

[`QrDecomposition`](../reference/decomposition/factorization/qrdecomposition.md) publishes `Q` and
`R` as `IReadOnlyList<double>` over `double[]` backing fields. It was published under
[decision 0095](0095-the-stats-numerical-layer-publishes-four-members-and-no-more.md) because
`Lodestar.Stats.Regression` needed it. [#667](https://github.com/CyrilB1531/lodestar/issues/667)
noticed that each read through the interface is a call the JIT may not inline, in loops that run once
per IRLS iteration. [#669](https://github.com/CyrilB1531/lodestar/issues/669) asked for the cost to be
measured before anything moved, with three options:

1. add `ReadOnlySpan<double>` accessors beside the lists;
2. change the property types outright, which is breaking;
3. change nothing here.

The decision sets the rule for every factorization type. `Lodestar.Decomposition` now has four:

- [`QrDecomposition`](../reference/decomposition/factorization/qrdecomposition.md): `Q`, `R`;
- [`TruncatedSvd`](../reference/decomposition/factorization/truncatedsvd.md): `Components`,
  `SingularValues`, `ExplainedVariance`, `ExplainedVarianceRatio`;
- [`Nmf`](../reference/decomposition/factorization/nmf.md): `Weights`, `Components`;
- [`PrincipalComponentVariance`](../reference/decomposition/factorization/principalcomponentvariance.md):
  three lists.

Eleven properties in all, each `IReadOnlyList<double>`.

## The measurement

AMD Ryzen 7 8700G, .NET 10.0.12, BenchmarkDotNet default job, 2026-09-14. **A** is `origin/main`.
**B** is a prototype with `Q` and `R` typed `ReadOnlySpan<double>`, and the three consumers in
`Lodestar.Stats.Regression` reading them as spans. Both were built with `LodestarUseProjectRefs=true`,
so that the read path is the only difference, and measured A, B, A to bound the drift.

**With dynamic PGO, the default on .NET 8 and later, the change buys nothing.** Across the 16 fits of
`OlsBenchmarks`, `GlmBenchmarks` and `RobustCovarianceBenchmarks`, B over the mean of the two A passes
lies between 0.984 and 1.012, inside an A-to-A drift of 0.994 to 1.020.

**Isolated, the interface read is 4× slower once PGO is off.** Take the `Qᵀy` loop alone, with the
list passed into a method that is not inlined: 18.8 µs against 4.7 µs at 2,000 × 4, and 118 µs
against 29 µs at 10,000 × 5, with `DOTNET_TieredPGO=0` or `DOTNET_TieredCompilation=0`. With PGO on,
the two are within 1%.

**In whole fits without PGO, that 4× appears in one place only.** With `DOTNET_TieredPGO=0`, A, B, A:

| fit | A (mean) | B | B / A |
| --- | ---: | ---: | ---: |
| OLS, 100 and 10,000 rows | 14.95 µs, 1,588.2 µs | 14.93 µs, 1,586.8 µs | 0.999, 0.999 |
| GLM, 200 × 1, 200 × 3, 2,000 × 3 | 33.0, 63.0, 616.0 µs | 33.2, 63.2, 617.3 µs | 1.005, 1.004, 1.002 |
| HC3, 100 and 10,000 rows | 17.09 µs, 1,933.3 µs | 16.34 µs, 1,855.7 µs | **0.956, 0.960** |

GLM at 2,000 × 1 read 1.047, B slower, which is the wrong direction for any read-path gain and is
counted as noise.

**The difference is where the list crosses a method boundary.** `LeastSquares.Solve` reads `qr.Q`
into a local in the method that holds the `QrDecomposition`, so the JIT sees the concrete array
through the inlined getter and devirtualizes without PGO. `RobustCovariance.Leverages` took the list
as a parameter, where the concrete type is invisible. A third build, **C**, kept `IReadOnlyList` and
changed `Leverages` to take the `QrDecomposition` and read `Q` into a local. Without PGO it measured
16.41 µs and 1,855.96 µs, the same as the span prototype, against 17.08 µs and 1,924.70 µs for A on
the same run.

## Decision

**Option 3: the eleven properties stay `IReadOnlyList<double>`.** A consumer in this repository that
reads a factor in a loop reads the property into a local in the method that holds the factorization,
rather than passing the list across a method boundary. That is where the only measured cost was, and
it is removed without changing a public type.

## Options that lost

- **Changing the property types to `ReadOnlySpan<double>`.** It measured nothing on the default
  runtime, and nothing on OLS or GLM without PGO. The one gain it showed, C reproduces without it. It
  would cost every caller of eleven properties what a span cannot do: LINQ, `foreach` into an async
  method, capture in a lambda, `Count` in place of `Length`. Paid in a 0.x release it is cheap in
  version numbers and not in what callers can write.
- **Adding span accessors beside the lists.** It would give two ways to read one array on eleven
  properties, for a gain only C's local read needed.

## Consequences

- `Lodestar.Decomposition` ships no surface change, and its unreleased 0.3.0 carries nothing for
  this decision.
- [#670](https://github.com/CyrilB1531/lodestar/issues/670) no longer waits on a release. Its work is
  the local read in `RobustCovariance.Leverages` and the check that no other consumer passes a factor
  across a boundary, inside `Lodestar.Stats.Regression` against the published floor.
- **Not measured, and the condition for reopening this:** a runtime that does not devirtualize at all.
  .NET Framework's JIT, Mono and Unity's are what a `netstandard2.0` caller runs on, and none is
  installed on the machine above. There the local read may not help and the 4× may reach every loop.
  A whole-fit measurement on one of them, showing a cost the span removes and the local read does
  not, reopens this decision.
