# Contributing

## Branching model — GitHub flow

`main` is always releasable. All work happens on a short-lived branch off `main`
and comes back through a pull request. Nothing is committed straight to `main`.

```bash
git switch main && git pull
git switch -c feat/spanish-snowball-stemmer
# … work, commit …
git push -u origin feat/spanish-snowball-stemmer
gh pr create --fill
```

Branch naming — `<type>/<short-kebab-summary>`, optionally prefixed with the
issue number (`feat/2-spanish-snowball-stemmer`):

| Prefix | For |
| --- | --- |
| `feat/` | a new algorithm or public capability |
| `fix/` | a correctness fix |
| `perf/` | a measured optimization (attach before/after numbers) |
| `docs/` | documentation only |
| `chore/` | build, CI, tooling, dependencies |

Keep a branch to one concern. "One algorithm at a time, complete" applies to
branches too: a new stemmer branch carries its implementation, its oracle corpus
and its tests — and nothing else. If you find an unrelated problem while working,
open an issue rather than widening the branch.

Reference the issue from the pull request (`Closes #12`) so it closes on merge. The
`Pull request closes only open issues` check holds three rules on the description:

- **It closes an issue.** A description with no closing keyword (`Closes`, `Fixes`, `Resolves` and
  their variants) fails, and `Refs #12` alone does not count.
- **The issue is open here.** A keyword naming a closed, missing or locked issue, a pull request, or
  an issue of another repository fails. Reopen the issue if the work is unfinished, or write
  `Refs #12`, which it does not read.
- **The issue is assigned to you.** Ask a maintainer to assign it; from a fork, comment on the issue
  first, since GitHub lets a maintainer assign anyone who has. Assigning does not rerun the check,
  so rerun the failed job, or edit the description, afterwards.

A pull request that deliberately closes no issue gets the `no-issue` label from a maintainer, which
waives the first and third rules and leaves a trace in its timeline. Dependabot, Renovate and the
nightly benchmark run's `github-actions[bot]` are waived the same way. Neither waives the second.

### `gh` 2.73.0 or later

The commands above use the GitHub CLI, and an old one **fails silently against
this repository**. Up to early 2025 `gh` asked for `projectCards` in its own
GraphQL queries; GitHub sunset Projects (classic) in 2024 and that field is now
an error, whether or not a repository ever had a classic project — this one
never did.

The part that costs something is not the error text. `gh pr edit --body-file` prints
`GraphQL: Projects (classic) is being deprecated …`, **exits without applying the edit**, and looks
like a warning; `gh issue view --comments` and `gh pr view --comments` fail the same way. The floor
is **2.73.0**, the first release (2025-05-19) after the last of the three upstream fixes merged on
2025-05-08 — measured on 2026-09-11 as broken under `gh 2.46.0` and clean under `gh 2.100.0`.

### The four checks that guard `main`

`main` is protected by a repository ruleset with **no bypass list**. Four checks must pass before
the merge button becomes available:

| Job | What it guards |
| --- | --- |
| `Lint (markdown + C# format)` | markdownlint, `dotnet format --verify-no-changes`, the `tools/tests` suite, that no tracked file holds a machine path, and that the Sonar `.globalconfig` is current |
| `Oracles are reproducible` | that the committed corpora match a fresh generation |
| `Build and analyze` | that `Build, test, analyze`, `Sample consumes the packages` and `Guide snippets compile, reference snippets run` all passed |
| `Pull request closes only open issues` | that the pull request's description closes at least one issue, each an open issue of this repository assigned to its author, rerun whenever the description or the labels change |

There is no approving review to wait for. The project has one maintainer, and GitHub does not let
anyone approve their own pull request, so protection is built on checks instead —
[`.github/workflows/README.md`](.github/workflows/README.md) has that reasoning, and a table of
what each workflow does.

Two things change what you will see on a pull request of your own:

- **A fork or a Dependabot pull request skips the analysis steps**, where `SONAR_TOKEN` is
  unreachable. The build and the tests still run there.
- **A pull request that changes only Markdown, or only a workflow the pull-request pipeline does
  not read, skips the build.** When it moved a Markdown file, `Lint` runs the documentation tests
  in the build's place, against the binaries `main` published for your base commit. The rules are
  [`tools/skip_build.py`](tools/README.md#skip_buildpy)'s; a skip is accepted only for a pull
  request classified that way, never a bare `skipped`.

## Specs and plans

A design spec goes in `docs/superpowers/specs/`, named
`<date>_<issue id padded to 4>_<slug>.md` — a second spec on one issue takes a letter (`0122b`).
The skills that write it are vendored under [`.claude/skills/`](.claude/skills/README.md); read
`brainstorming` and `writing-plans` rather than copying a neighbouring file.

**The spec is committed. The plan is not.** A spec records what was decided, what was measured and
what was rejected, so it stays worth reading after the work merges — and it is still a record when
it is written late. A plan is an instrument for work that has not started, with checkbox steps and
a `Branch:` line; once the pull request merges, the branch is gone and the boxes are checkboxes
nobody may tick. So write the plan beside the work — the session's scratch directory, or a path
`.git/info/exclude` keeps out, as the `.superpowers/` workspace already is — and leave it there.

**A spec opens its `**Status:**` with one of three clauses, and nothing else:**

| clause | what it says |
| --- | --- |
| `written before the work` | the spec led; the work followed it |
| `written with the work` | spec and work moved together, in one commit |
| `**retrospective**` | the work merged first; the spec caught up |

Anything after the clause is free text: the date it was written, an amendment, a note that the work
was never done. A retrospective spec is **dated by the work, not by the day it was written**, so the
status line is the only place that says the file arrived late — put the writing date there.
[`tools/check_spec_status.py`](tools/README.md#check_spec_statuspy) reads the clause, the file name
and the empty `plans/`, and runs in the pre-commit hook.

## Definition of done

A change is not finished until all of these hold:

1. **`dotnet build` is clean.** Warnings are errors repository-wide
   (`TreatWarningsAsErrors` in the root `Directory.Build.props`), covering `src`,
   `tests`, `bench` and `samples` alike — so a warning fails the build.
2. **`dotnet test` passes**, and any new algorithm replays a frozen oracle
   corpus. Conformance is *proven*, never assumed — see below.
3. **Lint is clean**: `dotnet format --verify-no-changes` and markdownlint.
4. **Public API carries XML documentation**, naming the Python function whose
   behavior it matches.
5. **The C# in the documentation still compiles.** Every ```` ```csharp ````
   fence in `README.md`, `docs/guides/` and `docs/reference/<package>/` is
   extracted from the Markdown and built against the packed packages — there is
   no second copy, so a snippet cannot drift from the API. The reference pages'
   fences are then **executed**, so a result a page promises is checked rather
   than trusted; item 6 has the `// =>` marker that states one. A fence that
   genuinely cannot compile opts out with
   `<!-- docs-compile: skip - reason -->` on the line above it, and one that
   compiles but cannot be run with `<!-- docs-run: skip - reason -->`; the
   reason has to be one a reviewer can disagree with.
6. **A new public type or method carries a reference entry.** The pages under
   `docs/reference/<package>/` follow the layout of the .NET API reference: a `###` entry per
   exported type, a `####` entry per public method with all overloads sharing it, and inside an
   entry, in order — a one-sentence summary, the declaration under a `<!-- docs-declaration -->`
   marker, **Parameters**, **Returns**, **Exceptions**, **Example**, **Remarks**, **Applies to**,
   **See also**. Empty rubrics are left out rather than filled with "none".

   The prose a reader came for lives in **Remarks**: what the member is for, when to prefer it to
   its neighbour, and the trap. The Python counterpart is not repeated — link
   [`docs/equivalence.md`](docs/equivalence.md) under **See also**.

   In an **Example**, a `// =>` comment is an assertion the CI executes; a plain `//` stays a
   comment. The value must be bound to a local first, and a trailing `…` means prefix match. A
   fence that cannot be executed carries `<!-- docs-run: skip - reason -->` on the line above.

   A Mermaid diagram is welcome where it shows a mechanism prose cannot hand a reader in one
   glance, and is removed in review when it only restates the sentence above it.

   Which namespaces are enforced is declared in [`docs/wiki-map.json`](docs/wiki-map.json), and
   `ReferenceDocumentationTests` fails the build when a page and the assembly disagree.

   **Exceptions** is checked against the member's own `<exception cref>` tags: the two must name
   the same set of types, in either order, so a `throw` added to a member owes both edits in the
   same commit. The sentence around each type is not compared — *when* it is thrown stays a
   review question. A namespace still owing that parity is named in
   [`docs/wiki-map.json`](docs/wiki-map.json)'s `exceptionsUnchecked` list, which only ever
   shrinks, and `tests/Shared/ReferenceDocumentation.cs` is the gate that reads it.

   A member that has a reference entry is linked to it wherever it is named in prose or in a
   table. Using it obliges the page as well: a member named anywhere on a page — inside a
   ```` ```csharp ```` fence included, where Markdown cannot carry a link — has to be linked to
   its entry at least once somewhere on that page, so a reader who meets it has a way to find
   out what it does. `ReferenceDocumentationTests` fails the build on either. A reference page
   is exempt for its own members: its headings are the entries.

   The page's opening table is navigation, not a summary: every exported type gets a row, its
   name linked to its own `###` entry — `` [`Levenshtein`](#levenshtein) ``. The anchor is
   GitHub's slug rule, lower-cased with dots dropped, and `ReferenceDocumentationTests` fails
   the build on a row with no link.
7. **A change to shipped behaviour carries its changelog entry**, in the package's own
   `src/<Package>/CHANGELOG.md` under `## [Unreleased]` and its `### Added`, `### Changed` or
   `### Fixed`: **one sentence, the issue and the commit — nothing else**. The root
   `CHANGELOG.md` only lists the eighteen files. The why
   lives in the issue and the how in the commit, so an entry carries no rationale, no measurement
   and no caveat:

   ```markdown
   - The byte-level decode substitutes U+FFFD instead of throwing. ([#149](https://github.com/CyrilB1531/lodestar/issues/149), [`5948a59`](https://github.com/CyrilB1531/lodestar/commit/5948a59))
   ```

   An entry whose commit closed no issue keeps the sentence and the commit link alone, rather than
   a fabricated issue link. New public surface, a fixed defect and a measured performance change
   all qualify; a refusal does not, because it changed nothing a caller can observe — the decision
   record is where that lives.

   This is item 7 rather than a line beside the release procedure because it was one, and four lots shipped
   without it — including [#450](https://github.com/CyrilB1531/lodestar/issues/450), a whole
   public API. Every other item here is checked by a gate that fails the build; this one is not,
   which is exactly why it needs to be read alongside them rather than at release time.

```bash
dotnet build Lodestar.slnx -c Release
dotnet test Lodestar.slnx -c Release
dotnet format Lodestar.slnx --verify-no-changes
npx markdownlint-cli2 "README.md" "CHANGELOG.md" "CONTRIBUTING.md" "docs/**/*.md" "tools/README.md" "tools/sonarqube-local/README.md" "bench/README.md" "bench/*/README.md" "samples/*/README.md" "tests/README.md" "tests/*/README.md" "src/*/*.md" ".github/workflows/README.md"
```

Neither `python` nor `python3` is safe to assume on both platforms: Ubuntu 24.04 ships
`/usr/bin/python3` and no `python` by default, while python.org's Windows installer (and
`winget install Python.Python.3.12`, which wraps it) ships `python.exe` and no `python3.exe` —
a `python3` that does resolve there is often the Microsoft Store app-execution alias, which
opens the Store instead of running anything.

```bash
# POSIX (bash/zsh)
python3 tools/check_version_floor.py
python3 tools/extract_doc_snippets.py && dotnet build samples/Lodestar.DocSnippets -c Release
python3 tools/check_machine_paths.py
```

```powershell
# PowerShell — split, not chained with `&&`, which needs PowerShell 7+
python tools/check_version_floor.py
python tools/extract_doc_snippets.py
dotnet build samples/Lodestar.DocSnippets -c Release
python tools/check_machine_paths.py
```

`check_version_floor.py` is offline and instant. It catches the version numbers
that must agree drifting apart, which MSBuild is perfectly happy to let happen. CI runs it
with `--check-feed`, which additionally proves the dependency floor is published
— see [`tools/README.md`](tools/README.md). If you touched packaging, packing and
running `python3 tools/check_nuspec_dependencies.py ./artifacts --require-all` (`python …` on
Windows, per the split above) closes the loop.

`check_machine_paths.py` refuses a tracked file that holds a path under someone's
home directory. `/tmp` is deliberately allowed, other than the session
scratch-directory shape `/tmp/claude-<digits>/`, which is the one that carried
eight of the ten paths this guard exists because of. `/usr`, `/etc`, `~/.nuget`
and other system paths are allowed too. An ordinary account name can still
collide with the probes derived from `$HOME`; `--no-environment` skips those
and keeps the named shapes enforcing.

## Before committing: the guards, one command earlier

The guards above are CI steps, so by default the first thing that tells you a
machine path reached a tracked file is a red job on a pull request. A tracked
hook removes that round trip, and installing it is one command with no
dependency:

```bash
git config core.hooksPath .githooks
```

```powershell
git config core.hooksPath .githooks
```

`.githooks/pre-commit` then runs the twenty-two offline guards —
`check_machine_paths.py`, `check_sdd_citations.py`, `check_comment_length.py`, `check_version_floor.py`,
`check_sample_culture.py`, `check_bench_map.py`, `check_sample_coverage.py`,
`check_netstandard_guards.py`, `check_no_console_writeline.py`,
`check_readme_pack_loop.py`, `check_claude_md_packages.py`,
`check_readme_packages.py`, `check_release_workflow_packages.py`, `check_unreleased.py`,
`check_requirements_lock_sync.py`, `check_gpu_tests_force_cpu.py`,
`check_adr_frontmatter.py`, `check_adr_index_sync.py`, `check_adr_index_is_cited.py`,
`check_spec_status.py`, `check_performance_sections.py` and `check_project_docs.py` — before
every commit, reports every one
that failed rather than the first, and refuses the commit if any did. It
resolves `python3` then `python` — neither name is safe to assume on both
platforms — and, on a machine with neither, says so and lets the commit
through rather than blocking work over a development dependency.

**It is skippable, on purpose.** `git commit --no-verify` bypasses it for one
commit, and its failure message says so. A hook that presents itself as
mandatory is a hook that gets deleted rather than skipped, and a deleted hook is
silent next time.

Three things are worth knowing before relying on it.

- **It is not a rehearsal of CI.** It runs the guards and nothing else: not the
  build, not the tests, not the packaging, doc-snippets or reference gates, not
  Sonar. A green commit is not a green pull request.
- **It runs in about a second, and that is the whole budget.** Anything that reached
  `dotnet build` would be uninstalled within a week.
  [`tools/README.md`](tools/README.md) has the per-guard figures behind that claim.
- **It reads the worktree, not the commit.** `git ls-files` reports the index,
  so a newly `git add`ed file *is* checked — which running the scripts by hand
  does not do. Their contents are then read from disk, so a file staged in one
  state and edited in another is judged in its worktree state.

Five guards CI runs stay out of it: `check_nuspec_dependencies.py` reads the
`.nuspec` files inside a packed `./artifacts`, `check_adr_immutable.py` and
`check_repeated_literals.py` both take `--base`, the pull request's own base
commit, which a commit made before a pull request exists has none to name,
`check_doc_test_counts.py` reads the `results.xml` files a CI run just wrote, and
`check_pr_closes.py` reads a pull request's description, which a commit does not
have.

`check_version_floor.py` needs no exclusion. CI passes it `--check-feed`, which reaches
nuget.org, and the hook does not — but that is a flag rather than a guard, and its two offline
rules run in both places. [`tools/README.md`](tools/README.md) carries each exclusion beside the
guard it excludes, with the reason.

## Before pushing: the half the build cannot see

`dotnet build` enforces the Sonar rules that live in `.globalconfig` (see
[Analyzers](#analyzers) below), but it has no view of three things the quality gate on the pull
request still judges: the Python rules over `tools/`, duplication, and coverage.

[`tools/sonarqube-local/`](tools/sonarqube-local/README.md) runs a disposable SonarQube Community
server that covers all three, for whoever wants that answer before pushing rather than after — with
the commands, the Elasticsearch trap, what one run cost, and the four reasons it is not a rehearsal
of the CI analysis. It is optional: nothing in CI uses it, and **a finding it reports is real while
a clean run promises nothing**.

## Working across two packages

The four libraries version and release independently, and `Lodestar.Fuzzy`
reaches `Lodestar.Text` through a `PackageReference` on the published package
rather than a project reference — the reasoning is in
[`docs/decisions/0001`](docs/decisions/0001-the-foundations-target-frameworks-comparison-unit-persistence-and-versioning.md).

A plain clone builds with no extra step: the version `Lodestar.Fuzzy` depends on
is a floor pinned in `src/Directory.Packages.props`, and it always names a
release that is already on nuget.org.

**When a branch edits `Lodestar.Text` and `Lodestar.Fuzzy` together**, that floor
points at a `Lodestar.Text` older than the one in your working tree, so
`Lodestar.Fuzzy` would compile against the published assembly and not see your
change. Flip the reference back for the duration:

```bash
# POSIX (bash/zsh)
export LodestarUseProjectRefs=true
dotnet build Lodestar.slnx -c Release   # prints a reminder that this is on
```

```powershell
# PowerShell
$env:LodestarUseProjectRefs = 'true'
dotnet build Lodestar.slnx -c Release   # prints a reminder that this is on
```

MSBuild reads environment variables as properties, so one export covers `build`,
`test` and the IDE. Nothing to pass per command, and nothing to pack and restore
between edits.

Two things to keep straight:

- **It is a local loop, not a merge strategy.** CI never sets the property and
  asserts the default path, so a branch whose `Lodestar.Fuzzy` needs new
  `Lodestar.Text` API cannot go green. Release `Lodestar.Text` first, raise the
  floor in `src/Directory.Packages.props`, then land the `Lodestar.Fuzzy` side.
  Two packages that release independently cannot also be merged as one.
- **Unset it before measuring anything.** With the property on you are building a
  graph that will never ship; benchmark numbers and packaging checks taken there
  describe nothing real.

**A feature pull request never touches `Version.props`.** `main` carries the next revision rather
than the published one, so the number your branch packs is already ahead of the feed. Moving it is
the release's act, not the feature's — [`README.md`](README.md#publishing) has how one is cut.

## Oracle validation

New algorithms are validated by replaying reference outputs captured from the
canonical Python library — not by trusting that the C# passes tests someone wrote
alongside the implementation.

1. Add a generator section to [`tools/generate_oracles.py`](tools/generate_oracles.py).
2. Build `.venv-oracles` once, on **Python 3.12 or later** — the interpreter every workflow pins,
   and the floor `tools/python_floor.py` holds and the generators refuse below
   (`tools/python_floor.py` has
   why it is the CI interpreter rather than the oldest one that parses). `python3` is 3.10 on
   Ubuntu 22.04 and 3.11 on this project's hosted session image, so name the version rather than
   taking the default:

   ```bash
   # POSIX (bash/zsh)
   python3.12 -m venv .venv-oracles
   .venv-oracles/bin/pip install --only-binary :all: --require-hashes -r tools/requirements.lock.txt
   ```

   ```powershell
   # PowerShell
   py -3.12 -m venv .venv-oracles
   .venv-oracles\Scripts\pip.exe install --only-binary :all: --require-hashes -r tools\requirements.lock.txt
   ```

   Install from the generated lock, not the human-edited `tools/requirements.txt`, and with the
   same two flags CI passes: `--require-hashes` pins the transitive graph as well as the direct
   dependencies, and `--only-binary :all:` means no source distribution runs a `setup.py`.
3. Regenerate, and commit the resulting `tests/oracles/*.json`:

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

   Run it from a neutral working directory. `nltk` refuses to import its own
   dependencies when they appear to live *under* the current directory, on POSIX and
   Windows alike, so the run fails — even with `PYTHONSAFEPATH` set — whenever the
   working directory is an ancestor of the virtualenv:

   ```text
   ImportError: Blocked import of regex from current working directory for security reasons
   ```

   Running from the repository root, or from `~`, does not satisfy this. Neither does `/tmp`
   when the checkout is itself under `/tmp` — which is where a hosted or sandboxed session puts
   it, and the reason this recommendation used to name `/tmp` unconditionally and mislead. Read
   the rule rather than the example: pick any directory the virtualenv does not live under.
   `/tmp` (POSIX) or `$env:TEMP` (PowerShell) serves for an ordinary checkout under `~`;
   `/var/tmp` serves when `/tmp` does not.

   Check the generator's own exit code, not a pipeline's. `python … | tail` reports
   `tail`'s status, so a failed generation looks successful — and the drift check
   that follows then proves nothing, because nothing was regenerated.
4. Add a test that replays the corpus, with a `1e-9` tolerance for floating-point
   results and exact comparison for strings.

Generation must be deterministic: a fixed seed, no wall-clock timestamps, no
unordered iteration. The `Oracles are reproducible` CI job keeps the committed
corpora, regenerates, and compares the two with
[`tools/compare_oracles.py`](tools/compare_oracles.py) — floats at the same `1e-9`
step 4 asks a test for, and everything else (integers, strings, key sets and their
order, array lengths and their order, the set of files) exactly. So a corpus whose
*values* move, or that gains, loses or reorders anything, blocks the pull request;
one whose last digits follow the CPU that generated it does not.
`tools/compare_oracles.py`
has why the gate stopped asking for byte-identity, which no machine could hold.

### Dependencies

`tools/requirements.txt` is the human-edited input; `tools/requirements.lock.txt`
is generated and pins the whole resolved graph with hashes. CI installs from the
lock, so a transitive bump cannot change the corpora behind your back. After
editing the input, regenerate:

```bash
pip install pip-tools
pip-compile --generate-hashes --strip-extras   --output-file tools/requirements.lock.txt tools/requirements.txt
```

Then regenerate the corpora and confirm they are unchanged. If they move, the
dependency bump changed reference output — resolve that deliberately, in the same
commit, rather than letting it land on someone else's pull request.

`tools/requirements-nodeps.txt` holds two packages that cannot live in the lock, for two different
reasons.

keybert (the MMR oracle's reference) declares `sentence-transformers`, and through it torch and
transformers, as its own dependencies, even though `keybert._mmr.mmr` — the only call the generator
makes — imports nothing but numpy and scikit-learn, both already pinned in the lock. `pip-compile`
has no per-package `--no-deps`, so pulling keybert into `requirements.txt` would pin that whole
stack into the lock that five other CI jobs install, two of them benchmark workflows that need none
of it. keybert is instead hash-pinned in its own file and installed with
`pip install --no-deps --require-hashes`, which skips dependency resolution entirely — only the
*Oracles are reproducible* job installs it, immediately after the lock.

summa (the TextRank oracle's reference) has a different problem: it publishes no wheel
at all, only an sdist, so `--only-binary :all:` — the flag every one of the five install sites
passes, lock and no-deps file alike — refuses it outright. It is hash-pinned here too, and the
*Oracles are reproducible* job's install of this file adds `--no-binary summa` alongside its
existing `--only-binary :all:`, so pip's per-package override lets that one setup.py run while
every other package, at every install site, stays wheels-only.

Anything added to this file must import nothing outside `requirements.lock.txt`, or the same
problem `--no-deps` exists to avoid reappears one entry later — the test summa passes on `scipy`
(via scikit-learn) and `numpy` (pinned directly) exactly as keybert passes it on numpy and
scikit-learn.
`tools/requirements-nodeps.txt`
records that boundary, both packages' reasons for needing it, and the options each beat.

**A decision record is never edited, and may only be deleted.** An amendment is therefore a new
record, which is why `0008` amends `0004` rather than `0004` changing, and why
[`docs/decisions/index.yaml`](docs/decisions/index.yaml) lists `0008` under `0004`'s `amended_by`
while every `applied_by` list stays empty: `tools/check_adr_immutable.py` refuses a diff that rewrites an accepted record, and allows
one that removes it. Should a record ever amend another, it declares `supersedes`, `amends` or
`applies` in its own frontmatter and the index is regenerated with
`python tools/regen_adr_index.py`; `tools/check_adr_frontmatter.py` and
`tools/check_adr_index_sync.py` refuse the commit otherwise. **The numbering restarted at `0001` on
2026-09-20** ([#1103](https://github.com/CyrilB1531/lodestar/issues/1103)), so read a citation older
than that against the tree that carried it: `git show 53af23c2:docs/decisions/<file>`.

Where behavior deliberately diverges from the Python reference, record it in
[`docs/decisions/`](docs/decisions/README.md) rather than in a code comment alone — see
[`0007`](docs/decisions/0007-the-deliberate-divergences.md) for the shape of
one.

## Analyzers

### Where the rules run

`SonarAnalyzer.CSharp` is referenced by every project under `src/`, `tests/`,
`bench/` and `samples/`, and the .NET code-quality rules are on at
`AnalysisMode=All` repository-wide, so **the rules that gate the pull request also
gate `dotnet build`**. Warnings are errors here, which means a Sonar or `CAxxxx`
finding is a compile error on your machine rather than a comment on your pull
request:

```bash
dotnet build Lodestar.slnx -c Release
```

That claim needs two mechanisms, not one. `AnalysisMode=All` covers the .NET
code-quality rules on its own. SonarAnalyzer's own rules do not: the package
ships a large share of them **disabled**, the SonarCloud quality profile enables
some of those, and nothing in `AnalysisMode` closes that gap — a finding there
used to surface only at the quality gate, three minutes after a push (issue #109).
The root **`.globalconfig`** closes it: a generated file, not a hand-written one,
that raises exactly the rules the profile activates and the package ships
disabled, to `warning`. Regenerate it with
[`tools/generate_sonar_globalconfig.py`](tools/generate_sonar_globalconfig.py) —
see [`tools/README.md`](tools/README.md#generate_sonar_globalconfigpy) for the
full command, including where it reads the SARIF error log from. `dotnet build` picks up
`.globalconfig` at the repository root with no wiring. The SDK's
`Microsoft.Managed.Core.targets` already globs every ancestor directory of every
compiled file for a file with exactly that name, so nothing declares it, and
nothing should.

A third case reaches neither mechanism: an analyzer diagnostic that ships **below
warning**. `TreatWarningsAsErrors` acts on warnings, so an `Info` rule is invisible
to the build, and SonarCloud imports it as an INFO code smell that does not move the
new-code gate either. Ten `xUnit2033` findings reached `main` that way (issue #690).
Those are raised in **`tests/analyzers.globalconfig`**, which is hand-written, named
explicitly in `tests/Directory.Build.props`, and cannot take the root file's name
because that one is generated. One rule is raised per measured escape rather than the
whole `Info` category: raising the category instead would turn every future informational
diagnostic into a build error nobody chose.

CI's `Lint` job runs the same generator with `--check` on every pull request,
comparing against the committed file without writing it. A red **`Sonar
globalconfig is current`** step means the SonarCloud profile has moved since the
file was last generated: regenerate with the command above and commit the
result, the same as any other generated file here.

Regenerating is required, not optional, whenever `AnalysisLevel` is raised past
`10.0` or `$(LodestarSonarAnalyzerVersion)` is bumped. Either can change which
rules the package ships disabled, which changes the delta the file encodes. This
applies whether the bump is a deliberate edit or an automated dependency update
(a Dependabot pull request, should one ever be wired for this pin): skip the
regeneration and the `Sonar globalconfig is current` job goes red with a diff and
no explanation of why, on a pull request that touched no C#.

It is an analyzer-only reference (`PrivateAssets="all"`), so it reaches no
published package — `tools/check_nuspec_dependencies.py` asserts that. The
version is pinned once, as `$(LodestarSonarAnalyzerVersion)` in the root
`Directory.Build.props`; raising it will usually surface new rules and therefore
a cleanup, so treat it as its own change. `AnalysisLevel` is pinned to `10.0`
for the same reason.

The command above does not reach `samples/`. The samples are outside
`Lodestar.slnx` and consume the packages from a local feed, so the analysers read
them only when the samples themselves are built. That needs a `pack` first, and
happens in the two CI jobs that pack: `Sample consumes the packages` and `Guide
snippets compile, reference snippets run`. Expect a finding there from CI rather
than from `dotnet build Lodestar.slnx`. Those two builds ran inside the
SonarQube Cloud analysis window until [#1028](https://github.com/CyrilB1531/lodestar/issues/1028)
moved packing out of the build job, so `samples/` now reaches the build's
analysers and not the quality gate.

One thing still only SonarCloud sees, so a green local build is not a green
quality gate: **duplication and coverage**.

### Suppressions

Deliberate suppressions live in the source, as a `#pragma warning disable` with a
comment giving the reason:

```csharp
// SonarLint S3776: cognitive complexity: faithful port of a published
// rule-engine; decomposing it would break the 1:1 mapping with the reference.
#pragma warning disable S3776
```

Do not reach for `.editorconfig` or `.vscode/settings.json` **to change what
SonarLint reports**. SonarLint reads neither: it ignores `.editorconfig`
entirely, and `sonarlint.rules` is declared application-scope in the extension
manifest, so VS Code silently drops it from a workspace file. The pragma works
because SonarLint's C# analysis is SonarAnalyzer running through Roslyn.

The **build** is a different tool: its Roslyn pass does read analyzer
configuration files, which is how `.globalconfig` raises the rules
SonarAnalyzer ships disabled (see [Analyzers](#analyzers) above). That file is
generated from the server's profile — change the profile, or the generator,
never the file.

A rule that a whole area trips *by being that area* — xunit's underscored test
names, BenchmarkDotNet's reflection-instantiated types, a sample printing to the
console — goes in that area's `Directory.Build.props` as a `NoWarn` entry, with a
comment naming each rule and why it does not apply there. A rule that one call
site disagrees with stays a `#pragma warning disable` in the source, with its
reason above it. Never add either without the reason.

A suppression needs a justification a reviewer can disagree with. "Too noisy" is
not one.

**When suppressed code moves, the suppression does not follow it.** Extracting a
method into a new file leaves the `#pragma` behind in the file the code left, and
the rule reappears against the new one. This has already happened twice while
extracting the shared Snowball framework — `CA1845`, then `S3267`. Both times the
build stayed green, because nothing in it ran the analyzer. That is no longer
true for `src/`, `tests/` and `bench/`, where the rule now reappears as a build
error at the moment of the extraction. Nor is it true for `samples/`, where it
reappears when the samples are built.

## Licensing and provenance

The project is Apache-2.0. Two hard rules, expanded in
[`0002`](docs/decisions/0002-provenance-and-the-allowed-references.md):

- **Never transcribe GPL-licensed code.** Implement from the *published algorithm
  description*. This is why the stemmers and phonetic encoders are original
  implementations rather than ports of an existing codebase — and it is not a
  formality: an oracle proves the behavior matches without the source needing to.
- **Never commit model weights.** Test fixtures are small and synthetic.

New third-party attributions go in [`THIRD-PARTY-NOTICES.md`](THIRD-PARTY-NOTICES.md).

## Performance claims

Performance is a stated selling point, so numbers are measured, not asserted.
Benchmarks live in [`bench/`](bench/README.md). Attach before/after figures to any
`perf/` pull request, and say which machine produced them.

Verify what you are actually measuring before quoting a result. A benchmark that
silently exercises the wrong build or the wrong code path will still produce
confident-looking numbers.

## Claims in comments

A comment here often carries the reason a divergence from the Python reference exists, which is what makes
that divergence reviewable. That is what makes them load-bearing, and it is also what makes them dangerous:
nothing checks them, and they go stale when the code beside them moves. Four rules, and they bind every
tracked file — `src/`, `tests/`, `tools/`, `bench/`, `samples/`, `docs/` and `docs/superpowers/` alike. A
spec that overclaims what its corpus proves is the same defect as a comment that overclaims what the
reference does.

**A comment says why, never what.** Restating the line below it is noise, and it goes stale faster than the
code does — the code at least gets compiled.

**A claim carries what would check it.** Where it is executable — a measurement, a reference library's
output, a count — run it and cite the corpus case, the file and line, or the command. "Measured" with no
pointer is an assertion wearing a measurement's clothes.

**Two budgets, because the two kinds of prose sit in different places.** An inline comment stands between
a reader and the code, so it gets **two lines** — a sentence, not a paragraph. XML documentation is the
member's own interface, read by a caller who does not have the source and required on every public member,
so it gets **eight**, counted over prose. A `<param>` or an `<exception>` that a well-formed member must
carry does not spend the budget. **The reason above a `#pragma warning disable` is not counted at all**:
[Suppressions](#suppressions) below already demands a reason a reviewer can disagree with, which is a
stricter requirement than brevity and rarely met in two lines. Past either budget, the reasoning belongs in
[`docs/decisions/`](docs/decisions/README.md),
cited from one line — or it needs cutting. `tools/check_comment_length.py` counts them, and CI runs
it in the `Lint` job: a block past its budget with no marker fails the build.

**A longer block carries a marker naming its reason**, as its first line:

```csharp
// long-comment: <why this one needs the room>
```

Longer is allowed where it is necessary; the marker is what stops it becoming the norm. It is held to the
bar a `#pragma warning disable` is held to — a reason a reviewer can disagree with, and "it felt useful" is
not one. A code review judges whether the marker was deserved, because the guard can only see that one
exists.
