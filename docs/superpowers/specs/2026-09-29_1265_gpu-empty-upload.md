# DeviceSparseMatrix.Upload takes a matrix storing nothing

**Issues:** [#1265](https://github.com/CyrilB1531/lodestar/issues/1265).
**Status:** written with the work, 2026-09-29.
**Date:** 2026-09-29.

## The problem

`DeviceSparseMatrix.Upload(ctx, [0, 0, 0], [], [], 2, 3)` — a TF-IDF block of stop-word-only
documents, which `CsrMatrix.Multiply` accepts — passed validation and threw
`NullReferenceException`: ILGPU's array overload of `Allocate1D` refuses a zero-length array, which
`DeviceTokenHashes` and `DeviceTextBlock` already guard. The buffer allocated first leaked on that
throw, or on any later failure.

## Decisions

- **An empty array allocates through the length overload**, as the two siblings do.
- **Every type holding several resident buffers releases the ones already allocated when a later
  allocation or the constructor fails**: `DeviceSparseMatrix`, and the two siblings, whose second
  allocation had the same leak. So does the result `TiledSparseDenseProduct.Multiply` allocates for
  a resident block, released if a launch fails; the other transient buffers are under `using`.

## Verification

- `EmptySparseMatrixTests`: the empty matrix uploads, reports no stored value, and multiplies to
  the zeros the CPU path gives; on `main` the same test throws `NullReferenceException`.
- Review A found the resident product's result buffer leaking on a failed launch; fixed here.
- The release is one helper, `DeviceOwnership.ReleaseOnFailure`, tested directly: the first push
  wrote a `catch` at each of the four sites, which no test can reach without a device failing, and
  SonarCloud's new-code coverage fell to 41.9 %.
