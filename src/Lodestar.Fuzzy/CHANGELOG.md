# Changelog — Lodestar.Fuzzy

What changed in `Lodestar.Fuzzy`, one release at a time and newest first. Each entry is one
sentence, the issue and the commit, as [`CONTRIBUTING.md`](https://github.com/CyrilB1531/lodestar/blob/main/CONTRIBUTING.md#definition-of-done)'s item 7 sets out.

## [Unreleased]

## [0.5.0] — 2026-09-24

### Added

- `Process.Cdist` scores every query against every choice into a `ScoreMatrix`, at `rapidfuzz.process.cdist` parity — its default scorer is `Fuzz.Ratio`, as the reference's is, and not `Process.Extract`'s `WRatio`. ([#1123](https://github.com/CyrilB1531/lodestar/issues/1123))
- Every `Fuzz` scorer takes a `TextElement`, whose `CodePoint` compares code points, splits on rapidfuzz's whitespace and sorts tokens by code point, so rapidfuzz's scores hold past the BMP. ([#892](https://github.com/CyrilB1531/lodestar/issues/892))

### Fixed

- `Fuzz.TokenSetRatio`, `Fuzz.PartialTokenSetRatio` and `Fuzz.WRatio` score `0` rather than up to `100` when one side has no words, as rapidfuzz does. ([#860](https://github.com/CyrilB1531/lodestar/issues/860))
- `Fuzz.TokenSetRatio` and `Fuzz.PartialTokenSetRatio` throw `ArgumentNullException` on a null string rather than `NullReferenceException`. ([#891](https://github.com/CyrilB1531/lodestar/issues/891))
- `Process.Extract` refuses a negative `limit` with an `ArgumentOutOfRangeException` naming it, where it failed inside `List.RemoveRange`. ([#910](https://github.com/CyrilB1531/lodestar/issues/910))
- `Fuzz`'s `TextElement.CodePoint` overloads split tokens on U+00A0 and U+0085 in a string holding a character above U+00FF, as rapidfuzz does, where French text with a no-break space scored below rapidfuzz. ([#974](https://github.com/CyrilB1531/lodestar/issues/974))
- `Fuzz.Ratio` and `Fuzz.PartialRatio` at `TextElement.CodePoint` allocate nothing on text inside the BMP, and `Fuzz.WRatio` answers an empty operand without tokenizing the other. ([#987](https://github.com/CyrilB1531/lodestar/issues/987))
- `Fuzz.Ratio` at `TextElement.CodePoint` answers a pair holding more than 63,455 distinct code points, where it threw, and the scorers that still cannot, say so on their pages. ([#982](https://github.com/CyrilB1531/lodestar/issues/982))
- `Fuzz`'s `TextElement.CodePoint` overloads size their code-point map by the distinct code points, where #987 sized it by every one and quadrupled the allocation on repetitive astral text, and `Fuzz.WRatio` answers an empty operand there before building it. ([#1056](https://github.com/CyrilB1531/lodestar/issues/1056), [#1057](https://github.com/CyrilB1531/lodestar/issues/1057))

### Changed

- `ExtractResult` is compiled into `Lodestar.Abstractions` under the same name and forwarded from here. ([#1142](https://github.com/CyrilB1531/lodestar/issues/1142))
- `Process.Cdist` rejects a pair on its lengths alone rather than scoring it, where the cutoff is above zero and the scorer is the default `Fuzz.Ratio` the bound holds for. ([#1134](https://github.com/CyrilB1531/lodestar/issues/1134))
- `Fuzz.TokenSetRatio` reads two of its three scores from lengths, `Fuzz.WRatio` tokenizes each side once, and `Process.Extract` keeps a bounded heap where it sorted every hit. ([#853](https://github.com/CyrilB1531/lodestar/issues/853))
- `Fuzz.PartialRatio` scores a needle of up to 64 characters from one equality table and skips windows that cannot win. ([#714](https://github.com/CyrilB1531/lodestar/issues/714), [`9ec3595f`](https://github.com/CyrilB1531/lodestar/commit/9ec3595f))
- `Fuzz.PartialRatio` does the same for a needle past 64 characters. ([#720](https://github.com/CyrilB1531/lodestar/issues/720), [`96856e70`](https://github.com/CyrilB1531/lodestar/commit/96856e70))
- The `Lodestar.Text` dependency floor rises from 0.4.0 to 0.6.0. ([#682](https://github.com/CyrilB1531/lodestar/issues/682), [`afc1909d`](https://github.com/CyrilB1531/lodestar/commit/afc1909d))

## [0.4.0] — 2026-08-21

### Changed

- **`fuzz.ratio` and `process.extract` now require the kernels they were made faster by.** The floor on `Lodestar.Text` moves from `0.3.1` to `0.4.0`, so a caller who references only `Lodestar.Fuzzy` stops resolving a `Lodestar.Text` that predates #208, #320, #357 and #302. No source file changes; `Lodestar.Text 0.4.0` also refuses a `null` word in the phonetic encoders, which a consumer of both packages meets here. ([#415](https://github.com/CyrilB1531/lodestar/issues/415), [`8a1573c`](https://github.com/CyrilB1531/lodestar/commit/8a1573c))

## [0.3.1] — 2026-08-16

### Changed

- The package is `Lodestar.Fuzzy`, and its namespaces are `Lodestar.Fuzzy.*`. `Lodestar.Fuzzy 0.3.1` holds the same code as `DataNet.Fuzzy 0.3.0`, and its floor names `Lodestar.Text 0.3.1`. ([#194](https://github.com/CyrilB1531/data.net/issues/194), [`b2911a5`](https://github.com/CyrilB1531/lodestar/commit/b2911a5))

## [0.3.0] — 2026-08-14 (published as DataNet.Fuzzy)

### Changed

- `DataNet.Fuzzy` depends on `DataNet.Text` as a published NuGet package rather than a project reference, so a package can ship without dragging the other two with it. ([#64](https://github.com/CyrilB1531/data.net/issues/64), [`96286ac`](https://github.com/CyrilB1531/data.net/commit/96286ac))

## [0.2.0] — 2026-08-05

Reach, correctness and honesty about performance. Nothing in the public API was
removed or renamed, so upgrading from `0.1.0` is a version bump.

### Added

- `netstandard2.0` becomes a second target framework, reaching .NET Framework 4.6.1+, Mono, Xamarin and Unity through conditional compilation rather than a reduced API. ([#8](https://github.com/CyrilB1531/data.net/issues/8), [`0a321f1`](https://github.com/CyrilB1531/data.net/commit/0a321f1))
- Four Snowball stemmers join English and French: `SpanishSnowballStemmer`, `PortugueseSnowballStemmer`, `ItalianSnowballStemmer` and `GermanSnowballStemmer`. ([#8](https://github.com/CyrilB1531/data.net/issues/8), [`0a321f1`](https://github.com/CyrilB1531/data.net/commit/0a321f1))
- Blocked (multi-word) Myers removes the 64-character cap on `Levenshtein.Distance`'s bit-parallel path. ([#8](https://github.com/CyrilB1531/data.net/issues/8), [`0a321f1`](https://github.com/CyrilB1531/data.net/commit/0a321f1))
- A benchmark suite compares the `net10.0` and `netstandard2.0` builds of the same library. ([#8](https://github.com/CyrilB1531/data.net/issues/8), [`0a321f1`](https://github.com/CyrilB1531/data.net/commit/0a321f1))
- Mirror test projects replay the entire suite against the `netstandard2.0` assemblies, 339 tests across both builds. ([#17](https://github.com/CyrilB1531/data.net/issues/17), [`48b7d05`](https://github.com/CyrilB1531/data.net/commit/48b7d05))
- A sample under `samples/DataNet.Sample` consumes the packages by `PackageReference` from a locally packed feed, and runs in CI. ([#50](https://github.com/CyrilB1531/data.net/issues/50), [`391a71c`](https://github.com/CyrilB1531/data.net/commit/391a71c))
- `CONTRIBUTING.md` and this changelog are added. ([#8](https://github.com/CyrilB1531/data.net/issues/8), [`0a321f1`](https://github.com/CyrilB1531/data.net/commit/0a321f1))
- SonarQube Cloud analysis, a `lint` CI job (markdownlint and `dotnet format`), and Dependabot for GitHub Actions are added. ([#8](https://github.com/CyrilB1531/data.net/issues/8), [`0a321f1`](https://github.com/CyrilB1531/data.net/commit/0a321f1))

### Changed

- Long-string `Levenshtein.Distance` is 20–33× faster: 684 µs to 21 µs at 512 characters. ([#8](https://github.com/CyrilB1531/data.net/issues/8), [`0a321f1`](https://github.com/CyrilB1531/data.net/commit/0a321f1))
- Regular expressions are bounded by a match timeout: a pathological pattern now raises `RegexMatchTimeoutException` instead of hanging the calling thread. ([#8](https://github.com/CyrilB1531/data.net/issues/8), [`0a321f1`](https://github.com/CyrilB1531/data.net/commit/0a321f1))
- Warnings are errors across the whole repository, covering `src`, `tests` and `bench` rather than the libraries alone. ([#8](https://github.com/CyrilB1531/data.net/issues/8), [`0a321f1`](https://github.com/CyrilB1531/data.net/commit/0a321f1))

### Fixed

- Static-analysis defects fixed and verified against the oracle corpora: an `int` division widened to `double` in `Jaro`, nested classes shadowing their outer type in the Snowball stemmers, unread step-method return values, and nested ternaries in three files. ([#8](https://github.com/CyrilB1531/data.net/issues/8), [`0a321f1`](https://github.com/CyrilB1531/data.net/commit/0a321f1))
- Code coverage was never collected: CI referenced `coverlet.collector` without depending on it, so the collection step silently did nothing. ([#8](https://github.com/CyrilB1531/data.net/issues/8), [`0a321f1`](https://github.com/CyrilB1531/data.net/commit/0a321f1))

### Security

- A `workflow_dispatch` input was interpolated directly into a shell command in a job holding `id-token: write`, letting it mint a nuget.org publishing key; values now reach the shell through the environment. ([#8](https://github.com/CyrilB1531/data.net/issues/8), [`0a321f1`](https://github.com/CyrilB1531/data.net/commit/0a321f1))
- GitHub Actions are pinned to full commit SHAs, so a moved tag cannot change what runs in CI. ([#8](https://github.com/CyrilB1531/data.net/issues/8), [`0a321f1`](https://github.com/CyrilB1531/data.net/commit/0a321f1))
- CI dependency installation is hardened: markdownlint pinned with lifecycle scripts disabled, and `pip install --require-hashes` against a generated lock file pinning all 29 packages. ([#8](https://github.com/CyrilB1531/data.net/issues/8), [`0a321f1`](https://github.com/CyrilB1531/data.net/commit/0a321f1))

### Documentation

- Package metadata now attributes the project to Cyril BRUNET (`Authors`, `Company`, `Copyright`). ([#8](https://github.com/CyrilB1531/data.net/issues/8), [`7523f34`](https://github.com/CyrilB1531/data.net/commit/7523f34))
- `THIRD-PARTY-NOTICES.md` now records the shipped dependencies instead of saying "None yet". ([#8](https://github.com/CyrilB1531/data.net/issues/8), [`7523f34`](https://github.com/CyrilB1531/data.net/commit/7523f34))

### Notes

- Deliberate analyzer suppressions live in the source as `#pragma warning disable` with their justification, since SonarLint reads neither `.editorconfig` nor workspace settings. ([#8](https://github.com/CyrilB1531/data.net/issues/8), [`0a321f1`](https://github.com/CyrilB1531/data.net/commit/0a321f1))
- The `netstandard2.0` build is behavior-verified: the whole suite is replayed against those assemblies, not only compiled. ([#17](https://github.com/CyrilB1531/data.net/issues/17), [`48b7d05`](https://github.com/CyrilB1531/data.net/commit/48b7d05))

## [0.1.0] — 2026-08-01

First release. All four lots of the project brief are delivered, and every
building block is validated by replaying frozen reference outputs captured from
the canonical Python libraries — see [`docs/equivalence.md`](../../docs/equivalence.md).

### Added

- Lot 4 — applied fuzzy matching (`DataNet.Fuzzy`): `fuzz.*` (ratio / partial / token_sort / token_set / WRatio), `process.extract` and `extractOne`, blocking deduplication. ([#8](https://github.com/CyrilB1531/data.net/issues/8), [`0a321f1`](https://github.com/CyrilB1531/data.net/commit/0a321f1))
- Migration guides for NumPy, pandas, scikit-learn, statsmodels, PyTorch, matplotlib and seaborn, plus a three-column inventory mapping each need to use / build / decide. ([#8](https://github.com/CyrilB1531/data.net/issues/8), [`0a321f1`](https://github.com/CyrilB1531/data.net/commit/0a321f1))
- A decision log records the deliberate divergences from the Python references. ([#8](https://github.com/CyrilB1531/data.net/issues/8), [`0a321f1`](https://github.com/CyrilB1531/data.net/commit/0a321f1))
- Publishing to nuget.org via Trusted Publishing (keyless, OIDC) and to GitHub Packages. ([#8](https://github.com/CyrilB1531/data.net/issues/8), [`0a321f1`](https://github.com/CyrilB1531/data.net/commit/0a321f1))
