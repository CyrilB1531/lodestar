# The Abstractions findings of the Review B after #1358

**Issues:** [#1363](https://github.com/CyrilB1531/lodestar/issues/1363),
[#1365](https://github.com/CyrilB1531/lodestar/issues/1365) to
[#1369](https://github.com/CyrilB1531/lodestar/issues/1369).
**Status:** written with the work, 2026-09-29.
**Date:** 2026-09-29.

## The problem

The Review B of what #1358 shipped, on `main` at `3088ca29`, found decision 0003(e)'s no-code rule
enforced by no test but the one `KMeansOptions` fact; a constructor page the reference gate could
not recognise, so it dodged the `**Applies to**` rubric; three public constructors undocumented;
five pages still saying every data type moved to `Lodestar.Abstractions`; and a stale count.
[#1364](https://github.com/CyrilB1531/lodestar/issues/1364), whether `IvDesign`, `PanelDesign` and
`UndefinedMetricException` keep their constructors under an amended 0003 or lose them, is the
maintainer's call and is left out.

## Decisions

- **One test reads the assembly's IL**: no method, accessor or non-public member the compiler did
  not write but `Equals` and `GetHashCode`, no nested type such as a closure, and constructors that
  only store — no branch, no throw, no call into Lodestar but a base constructor, a constructor or a
  getter. `CsrMatrix` and the three types #1364 decides are exempt by name. A first version looked
  at non-public members alone; Review A showed a validating constructor, a public method or a
  local function passing it, and a probe type now pins that each is seen.
- **The gate accepts a constructor entry**, titled as its page heads it, `CsrMatrix(rowCount, …)`,
  so the two constructor pages carry `**Applies to**` like every other; it does not yet check their
  parameters or exceptions against the XML.
- **`IvDesign` and `PanelDesign` document their constructors on their type pages**, under a
  `**Constructor**` rubric; both check nothing, their estimator refuses a shape that disagrees.

## Verification

- `DataTypesCarryNoCodeTests` passes on the assembly as compiled on both targets, and refuses its
  probe's validating constructor, public method and local function.
- The reference gate passes with both constructor pages under `**Applies to**`.
