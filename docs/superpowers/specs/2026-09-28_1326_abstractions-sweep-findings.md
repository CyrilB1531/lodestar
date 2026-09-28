# The Abstractions findings of the sweep after #1325

**Issues:** [#1326](https://github.com/CyrilB1531/lodestar/issues/1326),
[#1327](https://github.com/CyrilB1531/lodestar/issues/1327),
[#1328](https://github.com/CyrilB1531/lodestar/issues/1328),
[#1329](https://github.com/CyrilB1531/lodestar/issues/1329),
[#1330](https://github.com/CyrilB1531/lodestar/issues/1330).
**Status:** written with the work, 2026-09-28.
**Date:** 2026-09-28.

## The problem

The Review B invariant sweep of `Lodestar.Abstractions` on `main` at `eb4d240a` found five gaps
outside #1305's ownership remarks:

1. `CsrMatrix`'s refusal of `values` and `columnIndices` of different lengths named no parameter.
2. The constructor, the boundary a deserialized matrix crosses, documented one exception type of
   three, had no `<param>` tags and no reference entry.
3. `CreateUnchecked`'s page named `IndexOutOfRangeException` for a bad column index, where the block
   products throw `ArgumentOutOfRangeException` from slicing their operand.
4. `KMeansOptions.Equals` called a private helper with a LINQ closure, which decision 0003 does not
   admit in a moved data type.
5. The README, the package description and the sparse page said the package holds the data types
   every package declares, and carries no code but `CsrMatrix` and `SparseNorm`.

## Decisions

- **`nameof(columnIndices)`** for the length refusal, the array the comparison is read against.
- **A constructor page**, `csrmatrix-constructor.md`, on the shape of `BkTree`'s, listing every
  refused shape, and the XML carrying the same three exception types `CreateUnchecked` documents.
- **`ValueEquality.Same(IReadOnlyList<double[]>?, IReadOnlyList<double[]>?)`**, beside the jagged
  overload, replaces `SameSets`; the test holds the type to no static helper and no closure class.
- **The package holds the data types that carry no logic**, forwarded where they moved, and the
  records' structural equality is named as the code it carries beside the sparse primitive.

## Verification

- `CsrMatrixValidationTests` pins the parameter name through both entry points;
  `KMeansOptionsEqualityTests` pins value equality of the sets and the absence of a helper.
- The constructor page's example runs in the doc-snippets gate.
