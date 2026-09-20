# Five scaler findings in Lodestar.Preprocessing — design

**Status:** written before the work.
**Issues:** [#1041](https://github.com/CyrilB1531/lodestar/issues/1041),
[#1042](https://github.com/CyrilB1531/lodestar/issues/1042),
[#1044](https://github.com/CyrilB1531/lodestar/issues/1044),
[#1045](https://github.com/CyrilB1531/lodestar/issues/1045),
[#1046](https://github.com/CyrilB1531/lodestar/issues/1046).
**Date:** 2026-09-18.

## Why the five travel together

They are five readings of two sentences. `Lodestar.Preprocessing` tells a caller two things
about its scalers — *a non-finite value is refused* and *a sparse fit matches the reference* —
and neither is true of every entry point. #1041 and #1042 are the first sentence failing on
`StandardScaler`; #1044 and #1045 are the second failing on a `CsrMatrix` that stores one column
twice in a row; #1046 is the first sentence being true in the reference pages and absent from
the XML that ships. They touch the same six methods, the same two rows of
`docs/equivalence.md`, and the same internal helper, so splitting them would mean writing the
same row three times and reverting it twice.

## What is measured

Reproduced on `main` at `04882224`, against scikit-learn 1.9.0 / scipy 1.18.1 / numpy 2.5.3.

| Finding | Input | This, today | scikit-learn |
| --- | --- | --- | --- |
| #1042 | 8×2 dense, one `NaN` in column 1 | `Scale = [4.0403655377756065e8, NaN]`, `Mean = [-1.52711458125e8, NaN]`, no exception | `scale_ = [4.04036554e8, 1.95583898e1]` — nan-skipped |
| #1042 | the same with `+∞` | accepted, `Scale = [4.0403655377756065e8, NaN]` | `ValueError: Input X contains infinity…` |
| #1041 | `StandardScaler.Transform(CsrMatrix)` storing `NaN` | returns `1, NaN` | passes the `NaN` through |
| #1041 | the same storing `+∞` | returns `1, ∞` | `ValueError: Input X contains infinity…` |
| #1041 | `MaxAbsScaler.Transform` on either | `ArgumentException` naming `samples` | as above |
| #1044 | `new CsrMatrix(2, 1, [3, 5, 2], [0, 0, 0], [0, 2, 3])`, `MaxAbsScaler.Fit` | `Scale = [5]` | `scale_ = [8]` |
| #1044 | the same matrix, `CsrMatrix.ToDense` | `[[8], [2]]` | `[[8], [2]]` |
| #1045 | the same matrix, `RobustScaler.Fit` | `ArgumentException: Destination array was not long enough… (Parameter 'destinationArray')` | raises too, `could not broadcast input array from shape (3,) into shape (2,)` |
| #1046 | the six sparse `Transform`/`InverseTransform` XML tags | no `<exception>` mentions the no-row refusal #989 added | — |

`StandardScaler`'s dense `Transform` is in the same state as its dense `Fit` — `Transform` of
`[1, NaN, 3, 4]` returns `-1, NaN, 1, 1` — which the issues did not name but the fix cannot
leave behind: refusing on the way in and not on the way through is the same contradiction one
step later.

## Decisions

Three were Cyril's, taken 2026-09-18.

### 1. `StandardScaler` refuses a non-finite value wherever its neighbours do

`MaxAbsScaler`, `MinMaxScaler` and `RobustScaler` call `RequireFinite` on every public entry
point. `StandardScaler` calls it on exactly one, the sparse `Fit`. The six that do not are the
dense `Fit`, the dense `PartialFit`, the dense `Transform` and `InverseTransform` (both through
`Apply`), and the two `CsrMatrix` overloads that pass `requireFinite: false`.

**Chosen:** add the check to all six, so one sentence covers the four scalers.

**Rejected — document the propagation.** A `Scale` of `NaN` is not a result: it poisons every
later `Transform` of that feature with no exception between the input and the wrong answer, and
the caller who reads `Scale` is not the caller who passed the `NaN`. Recording that in prose
buys consistency of documentation at the price of consistency of behaviour.

**Rejected — fix only the sparse side.** It makes #1041's equivalence row true and leaves
`StandardScaler` disagreeing with itself between dense and sparse, which is the shape of the bug
rather than its fix.

This is a behaviour change on shipped overloads: a `NaN` or an infinity that returned a poisoned
answer now raises `ArgumentException` naming `samples`. It is the divergence from scikit-learn
the other three scalers already carry, so no new row is needed — only a true one.

### 2. Sparse column statistics consolidate duplicates first

`docs/reference/abstractions/sparse/csrmatrix.md` already states the rule: each operation follows
its own reference. `ToDense`, `Multiply` and `TransposeMultiply` sum a column stored twice in a
row because `scipy.sparse.csr_matrix` does; `RowL1Norm`, `RowL2Norm` and `NormalizeRows` read each
entry on its own because `sklearn.preprocessing.normalize` does. The scalers' reference is
`sklearn.utils.sparsefuncs`, which reduces through scipy and therefore sums — measured above,
`scale_ = 8` against this package's `5`.

**Chosen:** sum the duplicates before reading the statistics, in
`SparseColumns.Moments`, `SparseColumns.MaximumAbsolute` and `SparseColumns.ByColumn`.

**Rejected — document per-entry semantics.** It would put the scalers on the opposite side of
the rule from their own reference, and leave 14 of 300 random cases quietly disagreeing with
scikit-learn.

**Rejected — refuse a duplicate.** `CsrMatrix` accepts one and `ToDense` reads it; refusing at
the scaler would make a matrix that transforms fine one that cannot be fitted.

### 3. One pull request, five `Closes`

The five change the same six methods and the same two equivalence rows. One branch,
`fix/1041-preprocessing-scaler-findings`, one commit, five `Closes`.

## Design

### `SparseColumns.Consolidated`

One new internal helper, taking a `CsrMatrix` and returning either the same instance — when no
row stores a column twice, which is every matrix this repository builds — or a new one whose rows
carry each column once, the duplicates summed and the entries in ascending column order. Detection
is a single pass per row over a `int[]` of last-seen marks sized to the column count, so the common
case costs one scan and no allocation.

`Moments`, `MaximumAbsolute` and `ByColumn` call it first. `Divided`, `Multiplied` and
`MultiplyColumns` do not: a transform multiplies each stored value by its column's factor and
returns a matrix with the same stored positions, which is what `inplace_column_scale` does and
what the reference page promises. Consolidating there would change the caller's matrix shape
behind their back.

### `SortedColumn`'s buffer

The buffer's length is not an implementation detail to be enlarged: a percentile is read over the
whole column, the absent zeros included, so the buffer is exactly `RowCount` long and a column
holding more values than there are rows has no percentile to read. Consolidation is therefore the
whole fix — a column is stored once per row afterwards, so `count <= RowCount` holds by
construction and `Array.Copy` can no longer be the one to discover otherwise. `ByColumn`
consolidates at the source and `SortedColumn`'s remarks state the invariant it relies on; a test
proves `RobustScaler.Fit` returns on the matrix that raised.

### The six XML tags

`<exception cref="ArgumentException">` on `StandardScaler.Transform(CsrMatrix)`,
`StandardScaler.InverseTransform(CsrMatrix)` and the four `MaxAbsScaler`/`RobustScaler`
counterparts gains the no-row refusal #989 added, and — for `StandardScaler` — the non-finite one
decision 1 adds. The reference pages already say both; this is the `.xml` catching up.

## Testing

Every change is a refusal or a number, so each gets a test naming the measured value above.

- Dense `StandardScaler.Fit`, `PartialFit`, `Transform` and `InverseTransform` refuse a `NaN` and
  an infinity, naming `samples`.
- Both `CsrMatrix` overloads of `StandardScaler` refuse both, matching what `MaxAbsScaler` and
  `RobustScaler` already raise.
- `MaxAbsScaler.Fit`, `StandardScaler.Fit` and `RobustScaler.Fit` on the duplicate matrix answer
  the consolidated column: `MaxAbsScaler.Scale = [8]`, and `RobustScaler.Fit` returns rather than
  throwing.
- A matrix with no duplicate is returned unchanged by the consolidation, so the existing corpus
  replays bit for bit.
- The mirrored `*.NetStandard.Tests` project picks all of this up by linking the same sources.

No oracle corpus moves: the duplicate matrix is not in one, and the finite corpora consolidate to
themselves.

## Out of scope

- `MinMaxScaler` has no `CsrMatrix` overload, by the refusal `docs/equivalence.md` already records.
- `SimpleImputer`, the encoders and the splitters are untouched.
- scikit-learn's nan-skipping (`nanmean`, `nanpercentile`) stays un-implemented: this package
  refuses the `NaN` instead, which is the existing divergence and not this work's to revisit.
