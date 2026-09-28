# CsrMatrix takes the shared Guard, and says what it shares

**Issue:** [#1282](https://github.com/CyrilB1531/lodestar/issues/1282).
**Status:** written with the work, 2026-09-28.
**Date:** 2026-09-28.

## The problem

The post-merge review of `Lodestar.Metrics` and `Lodestar.Abstractions` (`main` at `1c09576a`) found
three things in `CsrMatrix`:

1. `RequireNotNull` and `RequireNotNegative` re-implemented `Guard.NotNull` and `Guard.NotLessThan`
   from `src/Shared/Guard.cs`, `#if` branches included, because the package opts out of the shared
   helpers and had no way to take one alone.
2. The csproj said the package compiles only `ValueEquality` of the shared helpers, while
   `LodestarIncludesElementWise` was on as well.
3. The class remark called instances immutable except for `NormalizeRows`, while `Values`,
   `ColumnIndices` and `RowPointers` are taken and exposed without a copy.

## Decisions

1. **`Guard` gets an opt-in, `LodestarIncludesGuard`**, for a library that opts out of the shared
   helpers — the pattern `ElementWise` and `RankBracket` already follow. Abstractions turns it on and
   `CsrMatrix` calls `Guard.NotNull` and `Guard.NotLessThan(value, 0)`.
2. **The comments say what is true.** The csproj names the three shared helpers and what each is
   for; the class remark says the arrays are shared, not copied, and that a holder can change them.
3. **The arrays stay writable.** Exposing them read-only would break every package that reads them
   through the published floor, which is a pre-1.0 API decision of its own, not this issue's.

## Review before merge

The `code-review` tool raised eight points, seven handled here: the props file's own comment still said
"ValueEquality alone"; the new opt-in reached `Guard` only through `ValueEquality`'s global using, so
`GlobalUsings.cs` now comes once for any opted-out helper; `GuardBlock` still hand-rolled a column
check, now `Guard.NotLessThan(columnCount, 1)`; the comment claimed no uncalled shared code while
`NotDisposed` comes with the file; the `ElementWise` opt-in sat apart from the comment naming it; the
`ParamName`s had no assertion; and the changelog did not say the net10.0 message changes.

The eighth is rejected: dropping the opt-out altogether, now that #1142 removed the
`InternalsVisibleTo` behind #440's CS0436. It would compile `RegexDefaults` and `StringCompat` into
a package whose rule (decision 0003) is no code but what its types call.

## Consequences

On .NET 10 a negative dimension now reaches `ArgumentOutOfRangeException.ThrowIfLessThan` rather than
`ThrowIfNegative`: the same exception and `ParamName`, a different message. No test asserts the text.

## Verification

The Abstractions suite and its mirror, `CsrMatrixValidationTests` among them; the full solution, since
every package builds against this project.
