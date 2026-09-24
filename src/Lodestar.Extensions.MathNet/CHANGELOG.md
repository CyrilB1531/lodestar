# Changelog — Lodestar.Extensions.MathNet

What changed in `Lodestar.Extensions.MathNet`, one release at a time and newest first. Each entry is one
sentence, the issue and the commit, as [`CONTRIBUTING.md`](https://github.com/CyrilB1531/lodestar/blob/main/CONTRIBUTING.md#definition-of-done)'s item 7 sets out.

## [Unreleased]

### Changed

- The `Lodestar.Abstractions` dependency floor rises from 0.1.1 to 0.2.0. ([#1142](https://github.com/CyrilB1531/lodestar/issues/1142))

## [0.1.0] — 2026-09-10

### Added

- **`Lodestar.Extensions.MathNet` 0.1.0 — `CsrMatrix` to and from Math.NET's sparse matrix, in one pass over the stored values.** `MathNetInterop.ToSparseMatrix` and `MathNetInterop.ToCsrMatrix` move the three compressed-row arrays rather than visiting `rows x columns` cells, because both sides store a matrix the same way and both expose it; neither result shares an array with its source, since Math.NET's storage is mutable through `At`. The dense pair is deliberately absent: `CsrMatrix.ToDense()` already returns a `double[,]` and Math.NET builds a `DenseMatrix` from one unaided, so the sparse pair is the one conversion neither side can do for itself. **The conversion sorts.** `CsrMatrix` validates four things and the order of column indices within a row is not among them, while Math.NET reaches a cell by searching that row — so a hand-built matrix handed over unsorted would convert without complaint and then answer zero for values it holds. Each row is sorted and duplicate columns are added together, after a pass that detects the already-sorted case and copies straight through, which is what every vectorizer here produces. [Decision 0089](https://github.com/CyrilB1531/lodestar/blob/53af23c2/docs/decisions/0089-the-interop-tier-may-take-a-dependency-a-core-package-refused.md) records that, and two things beside it: the interop family is now `Lodestar.Extensions.*` — so `Lodestar.Extensions.AI` becomes an instance of a convention rather than a one-off — and an interop satellite **may** take a dependency a core package refused, because converting to a caller's own types is not the same decision as computing with them. `Lodestar.Decomposition`'s refusal of Math.NET stands unchanged, re-measured on 2026-09-09: 5.0.0 is still the only stable release, dated 2022-04-03. ([#571](https://github.com/CyrilB1531/lodestar/issues/571))
