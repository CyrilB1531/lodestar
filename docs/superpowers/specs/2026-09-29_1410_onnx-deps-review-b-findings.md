# The Onnx, Embeddings and Abstractions findings of the Review B after #1401

**Issues:** [#1410](https://github.com/CyrilB1531/lodestar/issues/1410) to
[#1459](https://github.com/CyrilB1531/lodestar/issues/1459).
**Status:** written with the work, 2026-09-29.
**Date:** 2026-09-29.

## The problem

The Review B of `Lodestar.Onnx` after #1401, with the two packages it depends on, on `main` at
`14985bd7`, ran the invariant sweep over each package and the open review over Onnx. It found fifty
defects: thirteen in `Lodestar.Abstractions`, ten in `Lodestar.Onnx`, twenty-seven in
`Lodestar.Embeddings`. Most are documentation that says something the code does not do, or cites a
decision record for something it does not state. The rest are code:

- `CsrMatrix.ToDense` bounded a `double[,]` by the one-dimensional limit;
- the no-code test accepted any `Equals` unread and scanned only `Lodestar.*` namespaces;
- the vocabularies' equality was asymmetric across dictionary comparers;
- `EmbeddingIndex.Search` ranked a NaN query;
- `PairRanks` looped forever and two `BpeTokenizer` capacities overflowed on huge input;
- `OnnxTextEmbedder` let a sequence past the position table fail inside ONNX Runtime, and missed a
  transposed output when the batch and the sequence are as long.

## Decisions

- **A `double[,]` is bounded by `uint.MaxValue` cells in all.** Measured on .NET 10: `new
  byte[50000, 50000]` allocates, `new byte[70000, 70000]` fails with "Array dimensions exceeded
  supported range". Each side stays bounded by the one-dimensional limit.
- **`ElementWise` is split, not guarded by a symbol.** `ElementWise.cs` holds `AddScaled`, the one
  update the sparse products call; `ElementWise.Kernels.cs` holds the rotation and the subtraction,
  and `Lodestar.Abstractions` opts out of it.
- **The no-code test reads an equality's IL.** A structural `Equals` or `GetHashCode` is a public
  instance method of the expected signature that does no arithmetic but combining, no ordering, no
  float constant, no throw, no allocation, and calls only named BCL types, the shared equality
  helper or the assembly's own data types. `cgt.un` stays allowed, as C#'s `x is not null`.
- **`MaxArrayLength` stays off `.npy` blocks.** Applying it would refuse a 10,000 × 384 embedding
  matrix under the default. The options' documentation now says a vector block is bounded by
  `MaxTotalBytes`, as an index's already was.
- **The position table bounds `EmbedBatch` as a fixed axis does.** When `MaxSequenceLength` comes from
  the table, a longer sequence is refused under the caller's parameter. The table reader answers
  null whenever it is unsure, so a model it cannot read is not bounded.
- **A declared swap is refused even where the sizes agree.** It applies only when the export names
  its axes, and the output's first two names are the input's two, swapped.
- **`EncodedBatch.Lengths` is a read-only view** rather than documented as live: a cast back to
  `int[]` let a caller move what `Sequence` slices.

## Measured

The two hot paths the fixes touch were measured on an AMD Ryzen 7 8700G, .NET 10.0.12 and
BenchmarkDotNet 0.14.0, with the machine lock held. A is `main` at `14985bd7` in a clean worktree,
B is this branch.

| Benchmark | A | B | A again |
| --- | --- | --- | --- |
| `EmbeddingSearchBenchmarks.SearchTop10`, 10,000 | 356.4 µs | 358.4 µs | 354.1 µs |
| `EmbeddingSearchBenchmarks.SearchTop10`, 100,000 | 3,652.5 µs | 3,613.4 µs | 3,667.7 µs |
| `BpeBenchmarks.Bpe`, first capacity fix | 50.73 ms | 52.63 ms | 50.85 ms |
| `BpeBenchmarks.Bpe`, as committed | — | 48.33 ms | 48.34 ms |

The first capacity fix wrapped `GetByteCount` in a `try` on every piece and cost 3.6 %. The
committed one counts directly up to `int.MaxValue / 3` characters, where the count cannot overflow,
and leaves the `try` to a longer piece: B and A then agree. Allocations are unchanged throughout.

## Rejected

- **Refusing `Embed([], [])`.** ONNX Runtime accepts the zero-width tensor, measured on both tiny
  models, and the zero vector it pools to is what sentence-transformers returns for a fully masked
  sequence.
- **Treating `Guard` in `Lodestar.Abstractions` as a violation of 0003(e).** It is the validation of
  `CsrMatrix`, which 0003(e) exempts.
- **Editing the released CHANGELOG sections** that cite older records through `53af23c2` permalinks:
  they are history.
