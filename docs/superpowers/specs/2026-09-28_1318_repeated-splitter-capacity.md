# The repeated splitters' fold-list capacity

**Issues:** [#1318](https://github.com/CyrilB1531/lodestar/issues/1318).
**Status:** written with the work, 2026-09-28.
**Date:** 2026-09-28.

## The problem

The final open review of the Review B after #1316 found `RepeatedKFold` and
`RepeatedStratifiedKFold` sizing their result list as `new List<FoldSplit>(repeatCount * foldCount)`
in `int`: `2 × 2³⁰` wrapped negative and threw `ArgumentOutOfRangeException` under `capacity`,
which neither method has. The invariant sweep searched `new T[a * b]` and missed the capacity form.

## Decisions

- **The capacity goes through `TableLength.Of`**, #1314's helper, naming `repeatCount`, and the
  refusal is documented on both methods and their pages.
- **The sweep's search is widened** to `checked(...)` products and collection capacities; a grep of
  `Lodestar.Decomposition`, `Lodestar.Preprocessing` and `Lodestar.Abstractions` for the capacity
  form finds these two and `OneHotEncoder`'s rows by features, which the input's own length bounds.

## Verification

- `TableBoundTests` refuses `RepeatedKFold(4, 2, 2³⁰, 0)` naming `repeatCount`, before a fold is
  computed.
