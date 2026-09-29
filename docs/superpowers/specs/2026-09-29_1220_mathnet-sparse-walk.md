# MathNetInterop.ToCsrMatrix walks what a storage stores

**Issues:** [#1220](https://github.com/CyrilB1531/lodestar/issues/1220).
**Status:** written with the work, 2026-09-29.
**Date:** 2026-09-29.

## The problem

`MathNetInterop.ToCsrMatrix` converted any storage but compressed-row by calling `At` on every
cell: a 100,000-square diagonal matrix cost 1e10 reads for its 1e5 values.

## Decisions

- **A dense matrix is read from its column-major array**: counted per row in one pass, placed in a
  second, columns in order so each row comes out sorted.
- **A diagonal matrix is enumerated over what it stores**, `EnumerateIndexed(Zeros.AllowSkip)`,
  counted then placed the same way. Math.NET 5.0.0 accepts no storage but dense, compressed-row
  and diagonal, and the diagonal one enumerates in row order, so no reordering is needed; Review A's
  suggested test of a caller's storage found `Matrix<double>.Build.OfStorage` refusing one. A first version enumerated the dense storage too, and was 83 % slower on a
  1,000-square one than the per-cell walk.
- **A stored `NaN` is kept and a stored zero dropped**, as before.

## Verification

- Two tests: a 100,000-square diagonal matrix converts, and a dense one matches the cell-by-cell
  reading with a `NaN`, a `-0.0` and an empty row.
- `MathNetInteropBenchmarks`, new, A/B/A against `main` at `bf5cf39b` on an AMD Ryzen 7 8700G:
  diagonal 1,000 634.8–637.0 µs to 7.4 µs, 4,000 9.1–10.0 ms to 28 µs; a dense matrix with 1 %
  non-zeros 1,000 1.15–1.18 ms to 1.01 ms, 4,000 42.0–42.1 ms to 17.5 ms, allocating a quarter.
- Review A found a dense test named for enumeration it no longer takes, and the out-of-order
  branch untested; the test is renamed and the branch, which no storage Math.NET accepts reaches,
  removed.
