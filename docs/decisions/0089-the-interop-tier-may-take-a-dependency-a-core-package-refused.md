---
status: accepted
supersedes: []
amends: []
applies: []
---
# 0089 — The interop tier is `Lodestar.Extensions.*`, and it may take a dependency a core package refused

**Status:** accepted · **Date:** 2026-09-09

## Context

[Decision 0076](0076-a-core-package-carries-no-external-dependency.md) states the tier rule as one
sentence with two clauses: *"A core package carries no external dependency. An external dependency
earns its own satellite package, named for it."* It has been applied twice —
`Lodestar.Onnx` for `Microsoft.ML.OnnxRuntime`, and `Lodestar.Extensions.AI` for
`Microsoft.Extensions.AI.Abstractions` ([#570](https://github.com/CyrilB1531/lodestar/issues/570)).

[#571](https://github.com/CyrilB1531/lodestar/issues/571) is the third, and it is the first where
*which* dependency is taken has already been argued the other way in this repository.

**Phase 2 lot 3 refused Math.NET outright.** V3 of [#437](https://github.com/CyrilB1531/lodestar/issues/437),
recorded in [decision 0059](0059-phase-0-verifications-two-confirmed-voids-do-not-survive-nuget.md),
measured it and concluded that `Lodestar.Decomposition` should write its own thin Householder QR,
partial-pivot LU and one-sided Jacobi SVD rather than reference it. Re-measured on 2026-09-09, that
reading has not aged:

- `MathNet.Numerics` last shipped a stable **5.0.0 on 2022-04-03**. Since then, across 116
  published versions, the only movement is `6.0.0-beta1` (2023-12-17) and `6.0.0-beta2`
  (2025-03-02).
- The repository was last pushed **2025-03-03**. It is not archived, and carries 3 768 stars.
- [mathnet-numerics#117](https://github.com/mathnet/mathnet-numerics/issues/117), the request for a
  sparse SVD, has been **open since 2013-04-24**.

[#443](https://github.com/CyrilB1531/lodestar/issues/443) anticipated the tension and asked for it
to be settled here rather than inherited:

> An interop package takes that same stale dependency by definition. This phase anticipates it —
> *"Satellite tier per #438, since it takes a dependency by definition"* — so it is not a
> contradiction, but **the satellite argument should be made explicitly in that lot's issue** rather
> than inherited, given a sibling package refused the dependency outright on freshness grounds.

## Decision

### 1. A satellite that exists to convert may take a dependency a core package refused

**The two refusals are not the same decision, and freshness weighs differently on each side.**

`Lodestar.Decomposition` was choosing a dependency to *compute with*. A stale library there is a
liability its own users inherit without asking: they wanted a truncated SVD, and a bug or a missing
platform in the numerical dependency underneath is theirs to carry. That is why 0059 concluded the
three dense kernels were cheaper than the dependency.

`Lodestar.Extensions.MathNet` is choosing a dependency to *convert to*. Its entire value is that the
caller **already holds** Math.NET types — that is the only reason to install it. The dependency's
staleness is that caller's problem before it is ours, and this package neither adds to it nor asks
anyone else to take it on: a caller who does not hold those types installs nothing. Refusing the
dependency here would not protect anyone; it would only mean the friction stays where it is.

The rule this states, in a form the next interop lot can apply: **a package whose public surface is
conversion to a third party's types may reference that third party, on any tier, regardless of
whether a core package refused the same reference for its own kernels.** What a satellite may not do
is reach for a stale dependency to *implement* something — that is 0059's question, and it is
unchanged.

### 2. The interop family is `Lodestar.Extensions.<Dependency>`

0076 requires a satellite to be *"named for it"*. Two readings were live, and this settles which:

- `Lodestar.Onnx` took the dependency's distinctive word and one level.
- `Lodestar.Extensions.AI` mirrored `Microsoft.Extensions.AI`, whose distinctive name is two words.

**`Lodestar.Extensions.*` is now the interop family**: `Extensions` marks the tier, and the segment
after it names the dependency. So this package is `Lodestar.Extensions.MathNet` rather than
`Lodestar.MathNet`, and `Lodestar.Extensions.AI` is retroactively an instance of a convention rather
than a one-off. Still within #427's two-level `Lodestar.<Domain>[.<Capability>]` rule.

`Lodestar.Onnx` is **not** renamed. It is published, renaming a package is a break for every
consumer, and it is not an interop package in this sense — it runs models rather than converting to
someone else's types. The family covers packages whose whole surface is a bridge.

### 3. The bridge sorts, because `CsrMatrix` never promised an order

`CsrMatrix`'s constructor validates four things — `rowPointers[0] == 0`, non-decreasing pointers, a
last pointer equal to the stored count, and column indices inside `[0, ColumnCount)`. It says
**nothing** about the order of column indices within a row, about explicit zeros, or about a column
appearing twice in one row. Nothing in this repository has needed it to: the vectorizers build their
rows in ascending column order and reach the matrix through `CreateUnchecked`.

Math.NET's compressed-row storage reaches a cell through `FindItem`, which searches the row's slice.
A matrix handed over with unsorted indices would therefore convert without complaint and answer
lookups wrongly — silently, and only for the caller who built one by hand.

**The conversion to Math.NET sorts each row by column index and adds duplicate entries together**,
after a single pass that detects the already-sorted case and copies straight through. That pass is
what every matrix this repository produces takes, so the cost on our own output is one comparison
per stored value and no allocation.

**Refused: rejecting an unsorted matrix.** It would be stricter, and it would fail on an input
`CsrMatrix` accepts — turning an invariant the type never promised into one a neighbouring package
enforces on its behalf. If sortedness is ever to be an invariant, it belongs in `CsrMatrix`'s own
constructor and in its own decision, not in a bridge.

## What enforces it

`tools/check_nuspec_dependencies.py` gains a `LodestarExtensionsMathNet` row asserting exactly
`Lodestar.Abstractions` and `MathNet.Numerics`, per target framework and per version range, so the
dependency cannot spread to a core package without failing the build. `MathNet.Numerics` appears in
exactly one row, the way `Microsoft.ML.OnnxRuntime` and `Microsoft.Extensions.AI.Abstractions` each
do.

`tools/check_version_floor.py` gains this package as a dependent of `Lodestar.Abstractions`.

`MathNet.Numerics` 5.0.0 ships `netstandard2.0` beside `net461`, `net48`, `net5.0` and `net6.0`.
There is no `net10.0` asset, and none is needed: `netstandard2.0` covers both of this repository's
targets, so #427's `net8.0+` satellite floor is not taken here either — the same position 0076
recorded for `Lodestar.Onnx`, where *"the floor is a permission, not an obligation"*.

## Options considered

**Write the conversion in `Lodestar.Decomposition` instead**, so no new package exists. Refused: it
would put `MathNet.Numerics` inside a core package, which 0076's first clause forbids outright, and
it would hand the dependency to every caller who wanted a truncated SVD — the population 0059
protected by writing the kernels by hand.

**Name it `Lodestar.MathNet`**, on the `Lodestar.Onnx` precedent. Refused in favour of the family
above: two interop packages now exist, and one convention read forward is worth more than each being
locally defensible.

**Offer the dense pair as well** ([`CsrMatrix.ToDense`](../reference/abstractions/sparse/csrmatrix-todense.md)
↔ `DenseMatrix`). Not taken for 0.1.0: it already returns a `double[,]`, and Math.NET builds a `DenseMatrix` from one
unaided, so a wrapper would add a member that saves nobody a line. The sparse pair is the one
conversion neither side can do for itself.
