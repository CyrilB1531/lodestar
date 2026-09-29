# The IL test reads decision 0003(e)'s rule

**Issues:** [#1364](https://github.com/CyrilB1531/lodestar/issues/1364),
[#1381](https://github.com/CyrilB1531/lodestar/issues/1381) to
[#1386](https://github.com/CyrilB1531/lodestar/issues/1386),
[#1392](https://github.com/CyrilB1531/lodestar/issues/1392).
**Status:** written with the work, 2026-09-29.
**Date:** 2026-09-29.

## The problem

`DataTypesCarryNoCodeTests`, added by #1379, was looser than decision 0003(e)'s rule in six places: it
skipped interfaces, let a constructor call any BCL method or do arithmetic, accepted any Lodestar
constructor, getter or base-type method, accepted record member names on any type, passed
non-public auto-properties, and exempted all of `Lodestar.Internal`. It also exempted
`IvDesign`, `PanelDesign` and `UndefinedMetricException` by name, which was dead: 0003(e) keeps out
logic — "no code: no logic, no validation, no numerical layer" — and those three carry none, their
constructors only storing or passing to the base. #1364 asked whether they break the rule; they
do not. 0003(e)'s following sentence, which lists a hand-written constructor body among what a
type must lack, is the rule's means of verification; the maintainer's reading, recorded on #1364,
is that the rule is the absence of logic, and a constructor that only stores carries none. No
amendment is needed (#1394 proposed one and was closed).

## Decisions

- **An interface member has no body.**
- **A constructor is an allowlist of instructions**: loads, stores, locals, array construction,
  `call` and `newobj` — nothing arithmetic, no branch, no throw. Its calls are its base
  constructor, a constructor of a value its defaults name (a type of the assembly, a collection, a
  nullable, a tuple), a compiler-written getter, `Array.Empty` and `RuntimeHelpers.InitializeArray`.
- **A record's members are allowed as its compiler wrote them**: `ToString`, `PrintMembers`,
  `Deconstruct` and the operators must carry `[CompilerGenerated]`, so a hand-written one is
  refused on a record as anywhere else.
- **No non-public property or constructor**, but a record's copy constructor and a static one.
- **The shared helpers are exactly** `ElementWise`, `Guard` and `ValueEquality`: 0003(e) names
  `ValueEquality` for the records and `ElementWise` for the sparse primitive, whose argument checks
  `Guard` is; a fourth compiled in fails the test.
- **Only the sparse primitive is exempt**, `CsrMatrix` and `SparseNorm`, as 0003(e) names it.

## Verification

- The test passes on the assembly as compiled for both targets, and probe types pin that a local
  function, a method, a hand-written `ToString` on a class and on a record, a non-public
  auto-property, a validating, a computing and a BCL-calling constructor, and a default interface
  body are each refused.
- Review A found eight holes and edges, fixed here: record members accepted by name without
  `[CompilerGenerated]`, a record struct detectable by spoofing, no probe for a BCL call, any
  Abstractions or collection constructor callable, an interface's static members unscanned,
  missing storing opcodes, a generic type that would throw, and this spec's and the changelog's
  wording.
- CI collects coverage with Microsoft Code Coverage, which writes a hit counter —
  `ldsfld Tracker::Begin; ldc.i4 n; add; ldc.i4.1; stind.i1` — into every constructor. The first
  push refused them all on CI while passing locally; the scan now skips that exact sequence and
  nothing else, and passes with `--coverage` and without.
