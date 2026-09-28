# Lodestar.Abstractions review findings after #1283

**Issues:** [#1284](https://github.com/CyrilB1531/lodestar/issues/1284),
[#1285](https://github.com/CyrilB1531/lodestar/issues/1285),
[#1286](https://github.com/CyrilB1531/lodestar/issues/1286),
[#1287](https://github.com/CyrilB1531/lodestar/issues/1287),
[#1288](https://github.com/CyrilB1531/lodestar/issues/1288),
[#1289](https://github.com/CyrilB1531/lodestar/issues/1289),
[#1290](https://github.com/CyrilB1531/lodestar/issues/1290),
[#1291](https://github.com/CyrilB1531/lodestar/issues/1291),
[#1292](https://github.com/CyrilB1531/lodestar/issues/1292).
**Status:** written with the work, 2026-09-28.
**Date:** 2026-09-28.

## The problem

The review of `Lodestar.Abstractions` on `main` at `b6509a4d`, after #1283, found four defects and
five places where the package's documentation or build said something untrue:

1. `AndersonResult.GetHashCode` read `CriticalValues.Length`, and `TokenizationResult` read both
   counts in `Equals` and `GetHashCode`, so a record with an absent member threw where its sibling
   records answer (#1284).
2. Seven records hashed a raw `double` while `Equals` makes every `NaN` equal; .NET Framework hashes
   `NaN` payloads apart, so equal records hashed apart under the netstandard2.0 assembly (#1285).
   Only `NmfOptions` normalised it.
3. `CsrMatrix.NormalizeRows` read any `SparseNorm` other than `L1` as `L2` (#1286).
4. `CsrMatrix.ToDense` allocated `RowCount × ColumnCount` unchecked, and `ProductLength` refused
   only past `int.MaxValue`, not past the largest array (#1287).
5. Remarks cited a deleted record and carried sentences left broken by an ADR sweep (#1288); the
   reference page and NuGet description described one class and one enum (#1289); a test described
   `CreateUnchecked` as internal (#1290); the package compiled dead imports and uncalled helpers
   without saying so (#1291); the README built its example matrix without validation (#1292).

## Decisions

- **Every hash goes through `ValueEquality`.** `HashOf(double)` maps every `NaN` to one hash, the
  rule `NmfOptions` already applied inline; `LengthOf<T>(IReadOnlyList<T>)` and a list overload of
  `Same` give `TokenizationResult` the null-total members its siblings use. `LengthOf` is not a
  `CountOf` overload because an array is both an `ICollection` and an `IReadOnlyList<T>`, which
  would make every existing call ambiguous.
- **The generated equalities too.** A record that keeps its compiler-generated equality hashes a
  `double` through `EqualityComparer<double>.Default`, which is the same raw `GetHashCode`. The
  twenty records of the package that carry a `double` or `double?` and no hash of their own now
  override `GetHashCode` alone, keeping the generated `Equals`: doubles through `HashOf`, every
  other member through `HashOfItem`, the generated equality's own comparer. Added to this pull
  request at the maintainer's request rather than filed apart.
  Each override has its reference page. `Lodestar.Metrics`, `Lodestar.Embeddings` and
  `Lodestar.Fuzzy` compiled against the published `Lodestar.Abstractions` 0.2.0, which has no such
  override, so their reference gate and their netstandard mirror disagreed; like `Lodestar.Stats`
  and `Lodestar.Text` before them, they reach the Abstractions project until its next release.
- **An undefined norm is `ArgumentOutOfRangeException`**, as scikit-learn raises on an unknown norm
  and as the vectorizers refuse an undefined `AnalyzerKind`; the matrix is untouched.
- **The bound is `Array.MaxLength`, `0x7FFFFFC7`**, spelled as a constant because netstandard2.0
  does not declare it. `ToDense` has no argument to blame, so it throws
  `InvalidOperationException`; the products keep their documented `ArgumentOutOfRangeException`.
- **`DoubleMetaphoneCode` cites nothing.** #1288 proposed `0009`, but `0009` covers the encoders
  that follow jellyfish, and jellyfish has no Double Metaphone; no current record states the empty
  secondary, so the remark states the convention on its own.
- **The uncalled helpers are named, not split.** `ElementWise` and `Guard` stay one file each; the
  props comments say which members come along uncalled, as #1283 already did for `NotDisposed`.
  Splitting them would move shared source four other packages compile, for two small methods.

## Verification

- `RecordHashTests` pins, for each of the 28 records carrying a `double`, that instances built from
  four `NaN` bit patterns — quiet, negative, with a payload, and signalling — are equal and hash
  alike, and that `AndersonResult` and `TokenizationResult` with absent
  members compare and hash. net10 normalises `NaN` in `double.GetHashCode` itself, so on the
  runtimes the suites run these pin the contract rather than reproduce the Framework failure.
- `CsrMatrixTests` gains the undefined norm, a `1 × int.MaxValue` matrix densified, and a product
  of `0x7FFFFFC8` cells from a matrix with no rows, which allocates nothing before it is refused.

## Rejected

- **Hashing every array element** to strengthen the hash: the records hash O(1) by design, and the
  length already keeps equal records together.
