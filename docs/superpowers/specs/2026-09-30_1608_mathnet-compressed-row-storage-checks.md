# ToCsrMatrix checks a compressed-row storage before trusting it

**Issues:** [#1608](https://github.com/CyrilB1531/lodestar/issues/1608).
**Status:** written with the work, 2026-09-30.
**Date:** 2026-09-30.

## The problem

The Review B of `Lodestar.Extensions.MathNet` after #1607, on `main` at `d9d61e1f`, found the
compressed-row branch of `ToCsrMatrix` copying Math.NET's `Values`, `ColumnIndices` and
`RowPointers` as they were. Math.NET 5.0.0 declares the first two public writable fields and the
third a public array, so a caller can leave them inconsistent: a last pointer of `-1` sized a copy
at `-1` and threw `OverflowException`, and a decreasing pointer, a column past the width or a short
`Values` threw `ArgumentException` under `rowPointers`, `columnIndices` or `sourceArray`, where
`ToSparseMatrix` names `matrix` for the same defects (#1549).

## Decisions

- **The pointers are checked before they size a copy**: the first is 0 and none decreases, in the
  `CsrMatrix` constructor's order and in `ToSparseMatrix`'s sentences, the check the two share; then
  the last must not pass the length of `Values`, then of `ColumnIndices`, each named when short —
  Math.NET may leave either longer than what it stores. Each field is read once, so a caller
  swapping an array between the check and the copy cannot reach `Array.Copy`'s refusal.
- **The columns are checked once copied**, by the column check `ToSparseMatrix` runs, so every
  refusal names `matrix`; the last pointer needs no comparison with the count it defines.
- **A null `Values` or `ColumnIndices` reads as empty**: a storage holding nothing converts, one
  whose pointers claim values is refused as ending past them.
- Rejected: **building through the `CsrMatrix` constructor** and translating its refusals, which
  would still size the copies from an unchecked pointer.
