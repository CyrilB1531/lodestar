# The MathNet findings of the Review B after #1409

**Issues:** [#1529](https://github.com/CyrilB1531/lodestar/issues/1529),
[#1530](https://github.com/CyrilB1531/lodestar/issues/1530).
**Status:** written with the work, 2026-09-29.
**Date:** 2026-09-29.

## The problem

The Review B of `Lodestar.Extensions.MathNet` after #1409, on `main` at `1cf61032`, found two
defects:

- `ToCsrMatrix` allocated `new int[rows + 1]` unbounded, so a diagonal matrix of `int.MaxValue`
  rows, one stored value, wrapped the length and threw an undocumented `OverflowException`.
- A remark said Math.NET accepts no storage but its three, though a caller may subclass
  `Matrix<double>` or `MatrixStorage<T>`.

## Decisions

- **Refused at the entry, under `matrix`.** A row count at or past the largest array is refused
  before any storage is read, for the compressed-row copy and the two walks alike: `CsrMatrix`
  could not hold it either.
- **The walk is order-free.** It places each entry by its row, so a caller's storage, in whatever
  order it enumerates, converts correctly; the remark now says that instead.
