---
status: accepted
supersedes: []
amends: ["0003"]
applies: []
---
# 0010 — A data type's constructor may store what it is given

**Status:** accepted · **Date:** 2026-09-29

## Context

[`0003`](0003-the-package-layout-tiers-boundaries-and-edges.md)(e) keeps code out of
`Lodestar.Abstractions` and states the test by what a type declares: *no hand-written accessor,
setter, constructor body or method*. Read to the letter, three types break it
([#1364](https://github.com/CyrilB1531/lodestar/issues/1364)):

- `IvDesign` and `PanelDesign`, the designs `InstrumentalVariables` and `PanelRegression` take, are
  `readonly ref struct`s holding spans, and C# refuses a record that is a `ref struct`: their
  constructors are written by hand, and each assigns its parameters to the properties of the same
  name — seven assignments and five, nothing else.
- `UndefinedMetricException` is an exception, so a class and not a record, and carries the three
  constructors the .NET conventions require (CA1032): one passes a constant message to
  `InvalidOperationException`, two pass their arguments through.

None of them validates, computes or calls into Lodestar; the shape checks live in the estimators
that receive the designs. What the letter of `0003`(e) refuses is the C# mechanics a type needs
when the compiler cannot write its constructor, not the logic the rule is there to keep out. The
IL test [#1365](https://github.com/CyrilB1531/lodestar/issues/1365) added already reads the rule
that way: it accepts a constructor that only stores, and all three pass it, so their exemption in
that test was dead.

## Decision

**The code `0003`(e) keeps out of `Lodestar.Abstractions` is logic — validation, computation, an
algorithm, a call into Lodestar — not the absence of a hand-written body.** A constructor belongs
there when it only stores: it assigns its parameters, or the type's defaults (a constant, a
collection literal, a constructor of the value it defaults to), to the type's own storage, and
calls its base type's constructor with them. It does not branch, throw, compute from its arguments
or call a Lodestar method. Everything else `0003`(e) states is unchanged: no hand-written accessor,
setter or method but the structural `Equals`/`GetHashCode`, no non-public member, no interface
member with a body, and `CsrMatrix`, `SparseNorm` and `ElementWise` the one exception by name.

## Consequences

- `IvDesign`, `PanelDesign` and `UndefinedMetricException` stay in `Lodestar.Abstractions` as they
  are, and the IL test exempts only the sparse primitive.
- The IL test reads this rule where it can: it refuses a constructor that branches, throws or calls a
  Lodestar method, and it accepts these three. It is looser than this record in places — it lets
  arithmetic and calls into the BCL through, leaves interfaces unchecked, accepts record member
  names on any type and passes non-public auto-properties
  ([#1381](https://github.com/CyrilB1531/lodestar/issues/1381) to
  [#1386](https://github.com/CyrilB1531/lodestar/issues/1386)) — and those gaps are closed in the
  test; where the two disagree, this record is the rule.
- The allowance is for the body, not for writing one: a positional record, or a primary
  constructor, whose storing the compiler writes, is as admissible as the hand-written ones here.
