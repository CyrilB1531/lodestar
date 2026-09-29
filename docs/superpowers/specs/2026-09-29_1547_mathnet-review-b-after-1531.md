# The MathNet findings of the Review B after #1531

**Issues:** [#1547](https://github.com/CyrilB1531/lodestar/issues/1547),
[#1548](https://github.com/CyrilB1531/lodestar/issues/1548),
[#1549](https://github.com/CyrilB1531/lodestar/issues/1549),
[#1550](https://github.com/CyrilB1531/lodestar/issues/1550),
[#1551](https://github.com/CyrilB1531/lodestar/issues/1551).
**Status:** written with the work, 2026-09-29.
**Date:** 2026-09-29.

## The problem

The Review B of `Lodestar.Extensions.MathNet` after #1531, on `main` at `1c9db24f`, found five
defects:

- `ToCsrMatrix`'s walk over a caller's storage counted in `int` and trusted the indices it was
  given, so an entry outside the matrix escaped as an `IndexOutOfRangeException` or was accepted.
- `ToSparseMatrix` left sorting to Math.NET, whose sort is unstable past 16 entries: a 25-entry row
  holding `1e16`, `1`, `-1e16` in column 0 read `1` where `CsrMatrix.ToDense` reads `0`.
- An inconsistent `CsrMatrix` — from `CreateUnchecked`, or edited afterwards — reached Math.NET,
  which threw a plain `System.Exception` or kept a column outside the matrix.
- The NuGet description still said "one pass over the stored values".
- Nothing tested a caller's own storage, which #1530 said converts.

## Decisions

- **Sorted here, only when needed.** One pass checks the structure and whether every row already
  increases strictly, which is what every vectorizer produces; that matrix is copied as it is. Any
  other is sorted row by row on a key holding the column above the stored position, which makes the
  sort stable, and a repeated column is summed in stored order, as `ToDense` sums it, starting from
  its first value so that a lone `-0.0` keeps its sign as it does on the ordered path. Rejected:
  summing in Math.NET's order and documenting it, because the two readings of one matrix would
  disagree.
- **The constructor's checks, under `matrix`.** Row pointers are checked for their ends and their
  order before any column is read, then every column against the width — the order the
  `CsrMatrix` constructor uses, so a middle pointer past the end cannot index out.
- **A caller's storage is not trusted.** Each entry is checked against the shape, the running total
  is bounded by the largest array, and the second walk must place exactly what the first counted.
- **The tests subclass `Matrix`.** `Matrix.Build.OfStorage` refuses a storage Math.NET does not
  know, so a caller reaches `ToCsrMatrix` with a custom storage only through a `Matrix` subclass;
  the tests do the same.
- **Math.NET's factory is bypassed.** `OfCompressedSparseRowFormat` copies and sorts again what is
  already sorted; the arrays are written into a `SparseMatrix`'s storage instead, through the public
  `RowPointers`, `ColumnIndices` and `Values` fields. The first A/B/A, through the factory, measured
  `ToSparseReversed` at 4,000 rows 2.5 times slower than `main`; the numbers after the change are in
  the pull request.
- **An unstable sort first, a stable one only where a column repeats.** Each row is sorted as
  column-value pairs, which is exact while no column repeats; a row where one does is sorted again
  on the stable key. The two arrays are trimmed only when repeats were merged.
- **A diagonal matrix is read from its array.** The checks a caller's storage now gets cost the
  enumerator walk 70% on a diagonal matrix; Math.NET's own diagonal storage is read from `Data`
  instead, like the dense path, and needs none of them.
