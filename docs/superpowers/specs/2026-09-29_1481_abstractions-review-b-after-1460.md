# The Abstractions findings of the Review B after #1460

**Issues:** [#1481](https://github.com/CyrilB1531/lodestar/issues/1481) to
[#1490](https://github.com/CyrilB1531/lodestar/issues/1490).
**Status:** written with the work, 2026-09-29.
**Date:** 2026-09-29.

## The problem

The Review B of `Lodestar.Abstractions` after #1460, on `main` at `1cf61032`, found the no-code
test weaker than #1418 and #1419 meant it to be:

- **Types escaped the scan.** A `Lodestar` type carrying `[GeneratedCode]` was scanned by neither
  fact.
- **The equality check was too loose.** An `Equals` could order through a branch, write a field,
  fold case or sum a list and still pass.
- **The probes did not isolate the checks.** Each was caught by two checks at once, so neither
  check was pinned on its own.

Around that:

- six documentation defects in `CsrMatrix`'s, `NpyBlock`'s and the stop-word options' pages and
  remarks;
- `Guard.NotDisposed` compiled into a package that disposes nothing.

## Decisions

- **Lodestar's namespaces are scanned whole.** Only a compiler-named type is left out. Outside
  them, a type must be compiler-generated, a PolySharp polyfill (`[Microsoft.CodeAnalysis.Embedded]`,
  which is what PolySharp 1.16.0 actually emits, not `GeneratedCode`), or the tracker Microsoft Code
  Coverage injects under its instrumentation namespace.
- **The equality allowlist is by member, not by type.** Ordering branches, stores and float
  conversions are banned beside the earlier opcodes. `string.Equals` with a comparison passes only
  when `Ordinal` is loaded just before it. The scan still reads opcodes and callees, not dataflow;
  its remark says a float product of two fields passes.
- **`NpyBlock.Equals` goes through `ValueEquality.Same`,** the helper every other record uses, rather
  than a loop the tightened scan refuses.
- **`Guard` is split as `ElementWise` was.** `Guard.Disposal.cs` holds `NotDisposed` and is compiled
  by the default helper group only.

## Rejected

- **Scanning the dataflow of an `Equals`.** It would type the evaluation stack, which is a verifier,
  not a test.
