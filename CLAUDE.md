# CLAUDE.md

This file provides guidance to Claude Code (claude.ai/code) when working with code in this repository.

Lodestar is a data-science toolkit for C#/.NET with a deliberately narrow thesis: don't rewrite
Python's ecosystem, write native code only where .NET has no maintained equivalent at the
reference's parity — with **no Python at runtime**. Measured package by package, that gap is the
**apparatus around a computation** rather than the computation: the tokenizer loader and not its
encoder, the regression's inference table and not its coefficients, the time-series diagnostics and
not the forecast, sparse decomposition and not dense
([decision 0004](docs/decisions/0004-what-is-written-here-and-what-is-delegated.md) decides each
case). Everything else is delegated to an existing .NET library, and `docs/migration/` records
which.

`CONTRIBUTING.md` is the authoritative process document. This file covers what a session needs to
be productive quickly, and the traps that cost time.

## Where a fact belongs

Each document below has one subject; content belonging to another document goes there, with a link
left behind. The source column tells you whether to correct the document or something upstream of
it.

| document | its source | its subject |
| --- | --- | --- |
| `bench/README.md` | the `bench/` harness projects and scripts, hand-maintained | **how to measure** — the harness, the corpus, the commands |
| `docs/guides/performance.md` | a benchmark run on a named machine | **what was measured** — one comparison per capability against the incumbent, each number with its machine and its window; a before/after belongs in the pull request that made it |
| `tools/README.md` | the scripts under `tools/`, hand-maintained | what each tool does and how to run it |
| `tools/sonarqube-local/README.md` | one run of the disposable local server, on a named machine | how to run the half of the quality gate no `dotnet build` reaches, and what that run cost |
| `.github/workflows/README.md` | the workflows in that directory and the repository ruleset, hand-maintained | what the pipeline runs, and what has to be green before `main` accepts a merge |
| `CONTRIBUTING.md` | the project's own process, hand-maintained | the process a contributor follows |
| `CLAUDE.md` | what a session has found, hand-maintained | what a session needs to be productive, and the traps that cost time |
| `docs/equivalence.md` | the oracle corpora in `tests/oracles/*.json`, replayed against the C# they compare | the Python call to C# counterpart mapping, with each divergence |
| `docs/migration/` | the .NET package chosen for each need | what is delegated to another .NET library, and why |
| `docs/reference/` | the exported types and public methods of the namespaces `docs/wiki-map.json` covers, replayed against both target frameworks' assemblies — net10.0's alone on a pull request that skips the build, which runs on the binaries `main` staged for its base commit ([#1059](https://github.com/CyrilB1531/lodestar/issues/1059)) | what each function is for: declaration, parameters, returns, example, remarks |
| `docs/wiki-map.json` | the packages and the pages that ship with each, hand-maintained | which page belongs to which package, and which namespaces the reference gate enforces |
| `docs/wiki/home.md` | the guides and namespace pages it links, hand-maintained | the published wiki's front page: what Lodestar is, and where each task starts |
| `CHANGELOG.md` | the merged pull requests, per release | what changed, per release |
| `docs/decisions/` | each record's frontmatter and `**Status:**` line, crossed into [`index.yaml`](docs/decisions/index.yaml) and read in prose in [`README.md`](docs/decisions/README.md) | one axis of the project, with its options and its loser |
| root `README.md` | the project as it stands, hand-maintained | what the project is, and where to go next |
| `.claude/skills/` | [obra/superpowers](https://github.com/obra/superpowers), vendored at a pinned commit | how a spec and a plan are written; its README says what was taken |

## Commands

```bash
dotnet build Lodestar.slnx -c Release      # both target frameworks; warnings are errors
dotnet test Lodestar.slnx -c Release       # runs the suite twice: net10 and netstandard2.0 assemblies
dotnet format Lodestar.slnx --verify-no-changes
npx markdownlint-cli2 "README.md" "CONTRIBUTING.md" "docs/**/*.md" "tools/README.md" "tools/sonarqube-local/README.md" "bench/README.md" ".github/workflows/README.md"
```

Neither `python` nor `python3` is safe to assume on both platforms: Ubuntu 24.04 ships
`/usr/bin/python3` and no `python`, while python.org's Windows installer (and `winget install
Python.Python.3.12`, which wraps it) ships `python.exe` and no `python3.exe` — and a `python3` that
does resolve there is often the Microsoft Store alias, which opens the Store instead of running.

```bash
# POSIX (bash/zsh)
python3 tools/check_version_floor.py      # offline, instant; catches the three version numbers drifting apart
python3 tools/check_machine_paths.py      # catches a tracked file holding a path under someone's home directory
python3 tools/check_sample_culture.py     # catches a sample number printed in the contributor's culture
python3 tools/check_repeated_literals.py --base origin/main   # catches a literal this branch pushed past S1192
```

```powershell
# PowerShell
python tools/check_version_floor.py
python tools/check_machine_paths.py
python tools/check_sample_culture.py
python tools/check_repeated_literals.py --base origin/main
```

A single test, or one area:

```bash
dotnet test Lodestar.slnx -c Release --filter "FullyQualifiedName~SpanishSnowball"
dotnet test tests/Lodestar.Text.Tests -c Release --filter "FullyQualifiedName~Levenshtein"
```

**Read the test count, not the colour.** A `--filter` matching nothing exited zero under VSTest;
under Microsoft.Testing.Platform, which xunit v3 runs on since
[#623](https://github.com/CyrilB1531/lodestar/issues/623), it exits **8** and says `Zéro tests
exécutés`. That is `dotnet test`. **A test assembly invoked directly** (`dotnet
Lodestar.X.Tests.dll -namespace …`, which CI's docs-only path does) runs xunit v3's in-process
runner, which prints `Total: 0` and **exits 0** — hence `tools/check_doc_test_counts.py` reading the
count out of the `-xml` result ([#1054](https://github.com/CyrilB1531/lodestar/issues/1054)). The
count is also what catches a whole *suite* going missing, which has no exit code at all:
`dotnet test Lodestar.slnx -c Release` must report **36 assemblies**, eighteen suites and their
eighteen mirrors.

Oracle corpora (see *Oracle validation* below), run from outside the repository:

```bash
# POSIX (bash/zsh)
cd /tmp && PYTHONSAFEPATH=1 <repo>/.venv-oracles/bin/python <repo>/tools/generate_oracles.py
```

```powershell
# PowerShell
cd $env:TEMP
$env:PYTHONSAFEPATH = '1'
<repo>\.venv-oracles\Scripts\python.exe <repo>\tools\generate_oracles.py
Remove-Item Env:PYTHONSAFEPATH   # POSIX sets it only for this one command; PowerShell must clear it back out
```

Both need a neutral directory because `nltk` refuses to import under the repository (see *Oracle
validation* below). Neutral means **not an ancestor of the checkout**, which is why `/tmp` is the
wrong answer in a hosted session: those put the worktree under `/tmp`, so the guard fires anyway.
Check where the repository is first — `/var/tmp` serves when `/tmp` cannot.

**`.venv-oracles` is built on 3.12 or later, never on the platform `python3`** — 3.10 on Ubuntu
22.04, 3.11 on the hosted session image. Below the floor the generators stop with a sentence naming
both versions; before #486 they died with a `SyntaxError` in `seeded_random.py`, which a `| tail`
reported as success. CONTRIBUTING.md's [*Oracle validation*](CONTRIBUTING.md#oracle-validation)
step 2 has the creation command, and `tools/python_floor.py` holds the floor — the CI interpreter.

Guide snippets, benchmarks, packaging (see the `python`/`python3` split above):

```bash
# POSIX (bash/zsh)
python3 tools/extract_doc_snippets.py && dotnet build samples/Lodestar.DocSnippets -c Release
```

```powershell
# PowerShell — split, not chained with `&&`, which needs PowerShell 7+
python tools/extract_doc_snippets.py
dotnet build samples/Lodestar.DocSnippets -c Release
```

```bash
dotnet run -c Release --project bench/Lodestar.Text.Benchmarks -- --filter '*Levenshtein*'
for p in src/Lodestar.Abstractions src/Lodestar.Text src/Lodestar.Embeddings src/Lodestar.Fuzzy \
         src/Lodestar.Metrics src/Lodestar.Conformal src/Lodestar.Decomposition src/Lodestar.Cluster \
         src/Lodestar.Preprocessing src/Lodestar.Stats src/Lodestar.Stats.Regression \
         src/Lodestar.Stats.TimeSeries \
         src/Lodestar.Survival src/Lodestar.Onnx src/Lodestar.Gpu src/Lodestar.Extensions.AI \
         src/Lodestar.Extensions.MathNet src/Lodestar.Extensions.VectorData; do
  dotnet pack "$p" -c Release -o ./artifacts
done
```

## Architecture

Eighteen independently versioned packages under `src/`, in three tiers, all settled by
[decision 0003](docs/decisions/0003-the-package-layout-tiers-boundaries-and-edges.md). **Core**
carries no external dependency at all. `Lodestar.Onnx` and `Lodestar.Gpu` are the **satellites**,
each carrying the one dependency that is its reason to be a package. `Lodestar.Extensions.*` is the
**interop** tier, allowed a dependency a core package refused, because converting to a foreign type
is not computing with it. An external dependency added to a core package fails
`tools/check_nuspec_dependencies.py`, not a review.

The table below is asserted, not maintained by hand:
`python3 tools/check_claude_md_packages.py` fails when it drifts from `src/` or from that
script's `EXPECTED` edge map.

| Package | Tier | Holds |
| --- | --- | --- |
| `Lodestar.Abstractions` | core | `CsrMatrix`, `SparseNorm` and the dense-block products — the sparse primitive the others share — and, under decision 0003, the public data types the packages declare, with no code ([#1142](https://github.com/CyrilB1531/lodestar/issues/1142) moves them). |
| `Lodestar.Text` | core | distances, phonetics, set similarity, stemmers, tokenizers, sparse vectorizers, persistence, `BkTree`, keyword extraction. |
| `Lodestar.Embeddings` | core | sub-word tokenizers (WordPiece, SentencePiece, BPE/byte-level BPE), batch encoding pipeline, pooling, SIMD kNN `EmbeddingIndex`, `.npy` interop. |
| `Lodestar.Fuzzy` | core | `fuzz.*`, `process.extract`, blocking deduplication. |
| `Lodestar.Metrics` | core | classification, regression, clustering and ranking metrics at scikit-learn parity. |
| `Lodestar.Conformal` | core | split conformal intervals and prediction sets, at MAPIE parity. |
| `Lodestar.Decomposition` | core | truncated SVD and NMF over a `CsrMatrix`, the Householder QR, and the variance principal components explain, with the dense kernels written here. |
| `Lodestar.Cluster` | core | k-means by Lloyd's algorithm over a row-major span, at scikit-learn parity. |
| `Lodestar.Preprocessing` | core | feature scaling, encoding, imputation and the cross-validation splitters, fitted on arrays or a `CsrMatrix` and applied to spans, at scikit-learn parity. |
| `Lodestar.Stats` | core | classical hypothesis tests at scipy parity, plus the four tail members decision 0003 publishes for its neighbours. |
| `Lodestar.Stats.Regression` | core | ordinary, weighted and generalized least squares with the whole inference table, at statsmodels parity. |
| `Lodestar.Stats.TimeSeries` | core | the autocorrelation functions, Ljung-Box, the augmented Dickey-Fuller test, KPSS and seasonal decomposition, at statsmodels parity. |
| `Lodestar.Survival` | core | Kaplan-Meier, Nelson-Aalen and the log-rank test at lifelines parity, right-censored. |
| `Lodestar.Onnx` | satellite | `OnnxTextEmbedder`, and the reason the tier exists: `Microsoft.ML.OnnxRuntime`. |
| `Lodestar.Extensions.AI` | interop | the ONNX embedding path behind `IEmbeddingGenerator`; carries `Microsoft.Extensions.AI.Abstractions`. |
| `Lodestar.Extensions.MathNet` | interop | `CsrMatrix` to and from Math.NET's sparse matrix; carries `MathNet.Numerics`. |
| `Lodestar.Extensions.VectorData` | interop | an in-process `VectorStore` with hybrid keyword and vector search over `EmbeddingIndex` and `Bm25Index`; carries `Microsoft.Extensions.VectorData.Abstractions`. |
| `Lodestar.Gpu` | satellite | ILGPU kernels over device-resident matrices and text. **The one package on `net10.0;netstandard2.1`** — ILGPU publishes no `netstandard2.0` asset and does publish a 2.1 one. Nothing under `src/` may depend on it, so the SIMD path stays complete. |

The edges: **twenty-seven**, all asserted per target framework and per version range —
`Text`, `Decomposition`, `Extensions.MathNet`, `Stats`, `Cluster`, `Conformal`, `Embeddings`,
`Fuzzy`, `Gpu`, `Metrics`, `Preprocessing`, `Stats.Regression`, `Stats.TimeSeries` and `Survival`
→ `Abstractions`, all but the third for the data types they forward there (#1142); `Fuzzy` →
`Text`; `Onnx` → `Embeddings`; `Extensions.AI` → `Embeddings` and `Onnx`;
`Extensions.VectorData` → `Embeddings` and `Text`; `Stats.Regression` → `Stats` and
`Decomposition`; `Stats.TimeSeries` → `Stats` and `Stats.Regression`; `Survival` → `Stats`;
`Preprocessing` → `Stats` and `Cluster`, for the normal quantile `RobustScaler`'s `unit_variance`
divides by and the Lloyd's algorithm `KBinsDiscretizer`'s `kmeans` strategy runs.
`tools/check_nuspec_dependencies.py`'s `EXPECTED` is the authority, and the count above is checked
against it.

Four cross-cutting facts explain most of the layout, and none of them is visible
from a single file.

### 1. Two target frameworks, one public API

Everything ships `net10.0;netstandard2.0` in a single package. `netstandard2.0` reaches equivalent
behaviour through conditional compilation, **never a reduced API**. Gaps close in a fixed order:
PolySharp polyfills, then `System.Memory` / `System.Numerics.Vectors` / `System.Text.Json`
referenced only on that target, then a hand-written fallback. `src/Shared/` holds `Guard`,
`StringCompat` and friends, compiled into every library under `Lodestar.Internal` with a global
using, so no call site carries an `#if`.

The `*.NetStandard.Tests` projects **link the same test sources** and pin
`SetTargetFramework=netstandard2.0` on the project reference. That is why `dotnet test` runs
everything twice: without it the assemblies shipped to .NET Framework, Mono and Unity would be
compile-verified and never executed. A new test file is picked up by both automatically.

**A mirror reports more tests than its suite, as expected**: each carries
`NetStandardAssemblyGuardTests`, one fact per assembly it must prove it loaded. **Pin every
`Lodestar.*` package the library depends on, transitive ones included, with its own
`ProjectReference`** — a `PackageReference` leaks the pin, because `SetTargetFramework` does not
travel across one and NuGet resolves package assets against the *mirror's* framework, net10.0.
`Lodestar.Text` and `Lodestar.Decomposition` ran 832 tests against the net10.0
`Lodestar.Abstractions` that way, all green
([#529](https://github.com/CyrilB1531/lodestar/issues/529)).
`python3 tools/check_netstandard_guards.py` enforces both halves.

The one deliberate behavioural split is `VectorMath.Dot` — `Vector<T>` SIMD on
net10, scalar loop on netstandard2.0.

### 2. Versions are per package, and `src/` references packages, not projects

Each publishable project declares its version in a sibling `src/<Package>/Version.props` and
nowhere else. `Lodestar.Fuzzy` reaches `Lodestar.Text` through a `PackageReference` on a
**published floor** pinned in `src/Directory.Packages.props`, which is what makes `git clone &&
dotnet build` work with no pack step. A CI job asserts through evaluated MSBuild that no `src/`
project carries a `ProjectReference`.

When a branch edits two packages together, the floor points at an older
`Lodestar.Text` than your working tree:

```bash
# POSIX (bash/zsh)
export LodestarUseProjectRefs=true   # local developer loop only; CI never sets it
```

```powershell
# PowerShell
$env:LodestarUseProjectRefs = 'true'   # local developer loop only; CI never sets it
```

Unset it before measuring anything: with it on you are building a graph that will never ship.
CONTRIBUTING.md's [*Working across two packages*](CONTRIBUTING.md#working-across-two-packages) has
the release order that gets such a branch to green. Release tags are `<PackageId>/v<Version>`.

### 3. Conformance is proven by frozen oracles, not by hand-written expectations

Every algorithm replays reference values captured from the canonical Python library (rapidfuzz,
jellyfish, textdistance, difflib, scikit-learn, nltk, HuggingFace `tokenizers`, sentencepiece,
numpy, ONNX Runtime) into `tests/oracles/*.json`, compared at `1e-9` for floats and exactly for
strings. Python is a **development dependency only**.

Three traps, each already worth a session:

- **Run the generator from a neutral working directory** — CONTRIBUTING.md's
  [*Oracle validation*](CONTRIBUTING.md#oracle-validation) has why and the exact
  error `nltk` raises otherwise.
- **Read the generator's own exit code**, never a pipeline's. `python … | tail`
  reports `tail`'s status, so a failed generation looks successful — and the
  comparison that follows then proves nothing, because nothing was regenerated.
- **The `Oracles are reproducible` job compares numbers, not bytes** — it copies the committed
  corpora aside, regenerates, and runs `tools/compare_oracles.py` over the two: floats at the same
  `1e-9` the suites use, everything else exactly. It used to `git diff` them, which failed on the
  last digits of a BLAS-reduced value and read as flaky. A red here means a corpus moved by more
  than any assertion tolerates, so believe it. On failure the job uploads the regenerated corpora
  as an artefact, so the comparison can be made off the runner.

**Seven records, one per axis, and a number names a different record than it did before
2026-09-20** — [#1103](https://github.com/CyrilB1531/lodestar/issues/1103) restarted the numbering
after merging the 75 records that stated an axis and deleting the 72 that stated a mechanism. Read
an older citation against the tree it was written in: `git show 53af23c2:docs/decisions/<file>`.
**`0003` changed text on 2026-09-24**, when the numbering entered epoch 3: its rule for
`Lodestar.Abstractions` went from the types packages exchange to the public data types, with no
code. Its epoch-2 text reads at `9f9406c5`.
[`docs/decisions/index.yaml`](docs/decisions/index.yaml) carries each record's `supersedes`,
`amends` and `applies` together with the reverses a record cannot state for itself — `amended_by`,
which says the decision changed, and `applied_by`, which says it was used again unchanged —
generated from the frontmatter by `tools/regen_adr_index.py`. Every one of those lists is empty
today, because **a record is never edited and may only be deleted**: an amendment is a new record,
and `tools/check_adr_immutable.py` enforces exactly that — the one exception being a diff that raises
`docs/decisions/.numbering-epoch`, which is how `0003` was rewritten. Follow both edges before citing
a record that has them.

Where behaviour deliberately diverges from the Python reference, it goes in
[`docs/decisions/`](docs/decisions/README.md), the fastest way to understand why
something looks wrong. `docs/equivalence.md` maps each Python call to its C#
counterpart; **a row lands in the same commit as the function**, not afterwards.

### 4. The analyzers gate the build, not the pull request

`SonarAnalyzer.CSharp` is referenced by every project under `src/`, `tests/`, `bench/` and
`samples/`, and the .NET code-quality rules run at `AnalysisMode=All` with `AnalysisLevel` pinned to
`10.0` — CONTRIBUTING.md's [*Analyzers*](CONTRIBUTING.md#analyzers) has what that costs a finding.
The analyzer version is pinned once as `$(LodestarSonarAnalyzerVersion)` in the root
`Directory.Build.props`; raising it or `AnalysisLevel` surfaces new rules, and is its own change.

- A rule an *area* trips by being that area (xunit's underscored names, BenchmarkDotNet's
  reflection-instantiated types, a sample printing to the console) belongs in that area's
  `Directory.Build.props` as `NoWarn`, with a comment naming each rule.
- A rule one *call site* disagrees with takes a `#pragma warning disable` in the source, with the
  reason above it. Never either without a reason a reviewer can disagree with; "too noisy" is not one.
- **Do not reach for `.editorconfig` or `.vscode/settings.json`** for SonarLint rules. It ignores
  the first, and `sonarlint.rules` is application-scope, so VS Code drops it from a workspace file.
- **When suppressed code moves, the suppression stays behind.** Extracting a method into a new file
  leaves the `#pragma` in the old one and the rule reappears. Twice here.

`dotnet build Lodestar.slnx` does **not** reach `samples/` — they are outside the
solution. Duplication and coverage are visible only to SonarCloud, so a green
local build is not a green quality gate.

## Three gates that constrain how code is written

- **The packaging gate.** `samples/Lodestar.Sample` consumes the packages from `./artifacts`
  through `samples/NuGet.config`, and every new public type must be reachable from it by a member
  reference: new public API means a new use in `Lot*.cs`. Both sample builds need a fresh `pack`
  **and** an isolated `NUGET_PACKAGES`, or they judge the published packages instead of the working
  tree (CONTRIBUTING.md's Definition of done).
- **The doc-snippets gate.** Every ` ```csharp ` fence in `README.md`, `docs/guides/` and
  `docs/reference/*/*.md` compiles against the packed packages, so a renamed method fails CI. The
  reference fences are **executed** on top of that, and a trailing `// =>` is an assertion on the
  value the page promises — CONTRIBUTING.md's
  [*Definition of done*](CONTRIBUTING.md#definition-of-done), items 5 and 6, have the two opt-out
  markers.
- **The reference gate.** A new public type or method in a namespace `docs/wiki-map.json`'s
  `covered` table lists needs an entry in its package's reference page under `docs/reference/`,
  checked against both target frameworks' assemblies — a signature drifting from its documentation
  fails CI rather than a reader. A pull request that skips the build runs against net10.0's
  assemblies alone, the ones `main` staged for it
  ([#1059](https://github.com/CyrilB1531/lodestar/issues/1059)), and the mirrors judge the page on
  the next push to `main`. Only the `covered` namespaces are enforced; the rest waits on a reference
  page nobody has written.

## Provenance — two hard rules

- **Never transcribe GPL-licensed code** — CONTRIBUTING.md's
  [*Licensing and provenance*](CONTRIBUTING.md#licensing-and-provenance) has the rule, decision 0002
  the reasoning. Reading a reference implementation to diagnose one failing case is fine; deriving
  the implementation from it is not.
- **Never commit model weights.** Test fixtures are small and synthetic; vocabularies
  are fetched against a pinned SHA-256 by `tools/fetch_*.py`.

## Workflow

GitHub flow, one concern per branch, `<type>/<issue>-<kebab-summary>` (`feat/`, `fix/`, `perf/`,
`docs/`, `chore/`). Reference the issue with `Closes #n`. Everything in English — code, comments,
ADRs, commit messages, PR bodies. Comments follow four rules: say why not what, carry what would
check the claim, two lines inline or eight of prose in XML documentation, and a marker with its
reason past that. `CONTRIBUTING.md`'s *Claims in comments* states them,
`tools/check_comment_length.py` counts the lines, and
`.github/instructions/comment_claims.instructions.md` carries what a review asks. Commit messages
carry no `feat:`/`fix:` prefix.

`main` is protected by three required checks with no bypass list. "Require approvals" is off because
a single maintainer cannot approve their own PR. Do not commit, merge or tag unless asked. A `perf/`
PR carries before/after numbers and names the machine.

**Clear Sonar findings before committing, not after.** A green build is not a
clean Sonar, and a finding introduced by a pull request blocks its merge.

Design specs live in `docs/superpowers/specs/`, named
`<date>_<issue id padded to 4>_<slug>.md` — the repository's naming, which overrides the skills'
default; a second spec on one issue takes a letter (`0122b`). A plan is written to the same shape
and **is not committed**: it lives beside the work, in the session's scratch directory or under a
path `.git/info/exclude` keeps out, the way the `.superpowers/` workspace already does. The skills that write them are vendored under
[`.claude/skills/`](.claude/skills/README.md) rather than installed, so a hosted session reaches
them; read `writing-plans` and `brainstorming` first. **Do not reproduce the format by copying a
neighbouring file.** Two plans written that way failed `writing-plans`' own self-review
([#454](https://github.com/CyrilB1531/lodestar/issues/454)), most visibly its **No Placeholders**
rule: a step that says what to do without showing how is a plan failure, and code steps carry code.

**A spec may be written after the fact; a plan may not.** A spec records measured facts and rejected
options, and is still a record when written late — issues #202 to #446 were backfilled from the
commits that closed them. A plan is an instrument for work that has not started, with checkbox steps
and a `Branch:` line, so one written for merged work is checkboxes nobody may tick on a branch that
no longer exists. That is why the plan is not tracked: 116 of them, 88,745 lines, were, and no file
outside `docs/superpowers/` cited one — against 88 citations into `specs/`
([#1104](https://github.com/CyrilB1531/lodestar/issues/1104) deleted them).

**Every spec opens its `**Status:**` with one of three clauses, and nothing else:**

| clause | what it says |
| --- | --- |
| `written before the work` | the spec led; the work followed it |
| `written with the work` | spec and work moved together, in one commit |
| `**retrospective**` | the work merged first; the spec caught up |

Anything after that clause is free text — the date it was written, an amendment, a note that the
work was never done. **Date a backfilled spec by the work, not by the day it was written**, or the
directory loses its ordering; the status line is the only place saying the file arrived late, and
carries the writing date instead. `tools/check_spec_status.py` enforces the clause, the file name
and the empty `plans/`. A citation inside a retrospective spec uses the numbering of its own date,
which for a decision record before 2026-09-20 is not today's.

### SonarQube MCP server

`.github/instructions/sonarqube_mcp.instructions.md` applies here: disable automatic analysis with
`toggle_automatic_analysis` when starting a task, call `analyze_file_list` on the files you changed
at the end, then re-enable it. Look project keys up with `search_my_sonarqube_projects` rather than
guessing, and do not confirm a fix through `search_sonar_issues_in_projects` — the server will not
reflect the change yet.
