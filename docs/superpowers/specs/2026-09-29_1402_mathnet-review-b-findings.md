# The MathNet findings of the Review B after #1399

**Issues:** [#1402](https://github.com/CyrilB1531/lodestar/issues/1402) to
[#1408](https://github.com/CyrilB1531/lodestar/issues/1408).
**Status:** written with the work, 2026-09-29.
**Date:** 2026-09-29.

## The problem

The Review B of `Lodestar.Extensions.MathNet` after #1399, on `main` at `14985bd7`, found
`ToSparseMatrix` sorting each row and adding duplicate columns before handing the arrays to
Math.NET 5.0.0's `SparseCompressedRowMatrixStorage<double>.OfCompressedSparseRowFormat`, which
copies them and then calls its own `NormalizeOrdering` and `NormalizeDuplicates` on every call. The
documentation built on the local pass was wrong with it. Around that: the `ToCsrMatrix` page
said a stored zero is dropped on every path, three places cited decision 0003 for what it does not
record, the README and pages called every conversion one pass over the stored values,
`docs/equivalence.md` named a compressed-column storage Math.NET does not have, three interop
READMEs read "A interop package", and `Version.props` said the `Lodestar.Abstractions` floor never
moved.

## Decisions

- **Math.NET normalises; this package does not repeat it.** The out-of-order and duplicate-column
  tests stay, so a Math.NET release that stopped normalising fails them.
- **The compressed-row path keeps an explicit zero.** A `CsrMatrix` with sorted rows and no
  repeated column round-trips unchanged, zeros included, which `ToSparseMatrix`'s page already promised; the page for `ToCsrMatrix` now says
  only the dense and diagonal walks drop one, and a test pins the round trip.
- **The count handed to Math.NET is `NonZeroCount`**, the length of `Values`, as before.

## Measured

`MathNetInteropBenchmarks.ToSparse*` on an AMD Ryzen 7 8700G, .NET 10.0.12, BenchmarkDotNet
0.14.0, A/B/A with the machine lock held: a 1 %-dense square matrix, its rows sorted or reversed.

| Case | Size | A | B | A again | Allocated A → B |
| --- | --- | --- | --- | --- | --- |
| sorted | 1,000 | 39.7 µs | 39.8 µs | 37.9 µs | 123 KB → 123 KB |
| reversed | 1,000 | 131.9 µs | 143.0 µs | 125.7 µs | 546 KB → 123 KB |
| sorted | 4,000 | 795.8 µs | 673.4 µs | 755.4 µs | 1,894 KB → 1,894 KB |
| reversed | 4,000 | 2,450.6 µs | 812.2 µs | 2,391.9 µs | 8,264 KB → 1,894 KB |

Sorted rows, what every vectorizer here produces, lose the detection pass. Reversed rows of forty
entries run three times faster; reversed rows of ten run about 10 % slower on the clock, Math.NET's
own sort of a short reversed row costing more than `Array.Sort` followed by Math.NET's sort of the
row already sorted, while allocating a quarter as much. The regression is on hand-built unsorted input only, and kept.

## Rejected

- **Passing the last row pointer instead of `Values.Length`**, for a matrix built through
  `CsrMatrix.CreateUnchecked` whose last pointer disagrees with its values: such a matrix breaks
  `CreateUnchecked`'s own contract, arrays valid by construction.
