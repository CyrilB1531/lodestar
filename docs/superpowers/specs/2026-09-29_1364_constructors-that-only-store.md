# A data type's constructor may store what it is given

**Issues:** [#1364](https://github.com/CyrilB1531/lodestar/issues/1364).
**Status:** written with the work, 2026-09-29.
**Date:** 2026-09-29.

## The problem

Decision 0003(e), read to the letter, refuses a hand-written constructor body in
`Lodestar.Abstractions`, and three types carry one: `IvDesign` and `PanelDesign`, `ref struct`s C#
will not make records, and `UndefinedMetricException`, whose three constructors the .NET
conventions require. None validates, computes or calls into Lodestar; each only stores its
parameters or passes them to its base type.

## Decisions

- **Amend 0003 with decision 0010**, the maintainer's reading: the rule keeps out logic —
  validation, computation, an algorithm, a call into Lodestar — and a constructor that only stores
  is not logic. The alternatives were primary constructors on the two `ref struct`s, which the
  record admits as well but which leave the exception as it is, a factory method, which is a
  hand-written method 0003 refuses as much, and moving the types out, which would undo #1142.
- **The IL test exempts only `CsrMatrix`**: the three types already passed its constructor rule,
  so their exemption was dead.

## Verification

- `DataTypesCarryNoCodeTests` passes without the three exemptions on both targets.
- `tools/check_adr_immutable.py`, `tools/check_adr_index_sync.py` and `pytest tools/tests`, which
  guard the decisions' prose counts.
- Review A found the record claiming the IL test reads the rule whole, where it lets arithmetic
  through, and an allowance limited to "no other way" nothing checks; both statements are
  corrected, and the arithmetic gap is on [#1382](https://github.com/CyrilB1531/lodestar/issues/1382).
