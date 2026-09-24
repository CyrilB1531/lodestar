# A README, a CHANGELOG and a performance page per project

**Issue:** [#1133](https://github.com/CyrilB1531/lodestar/issues/1133).
**Status:** written before the work, 2026-09-24.
**Date:** 2026-09-24.

## The problem

The eighteen publishable projects under `src/` carry no `README.md`, no `CHANGELOG.md` and no
`performance.md` of their own. What exists is repository-wide, and a consumer of one package
reads about all of them:

- **One README for every package.** `Directory.Build.props` sets
  `<PackageReadmeFile>README.md</PackageReadmeFile>` and packs
  `$(MSBuildThisFileDirectory)README.md`, the repository root. Somebody installing
  `Lodestar.Survival` opens its nuget.org page and reads about eighteen packages, a Python parity
  thesis and a solution layout, rather than about Kaplan-Meier, Nelson-Aalen and the log-rank
  test.
- **One CHANGELOG for every package.** `CHANGELOG.md`, 1,011 lines, holds one `### Lodestar.X`
  section per package under each `## Released — <date>`. No project sets `PackageReleaseNotes`.
  A consumer of `Lodestar.Fuzzy` who wants to know what changed between two versions reads a file
  covering seventeen packages they do not have.
- **One performance page for every package.** `docs/guides/performance.md`, 2,076 lines, has a
  `## Lodestar.X` section for eleven packages. The seven with no measured number
  (`Abstractions`, `Conformal`, `Survival`, `Onnx`, `Extensions.AI`, `Extensions.MathNet`,
  `Extensions.VectorData`) cannot be told apart from those whose numbers were never written up.

The non-publishable projects have no README either: neither `samples/` (2 projects) nor `tests/`
(36: eighteen suites and their eighteen netstandard mirrors) has one, and `bench/` (5 projects)
has a single `bench/README.md` at its root.

## Decisions

Made by the maintainer on 2026-09-24.

1. **The per-project `CHANGELOG.md` is the source.** Each package's changes are written in
   `src/<Package>/CHANGELOG.md` and nowhere else. The root `CHANGELOG.md` keeps only a link to
   each of the eighteen. Rejected: the root file stays the source and the per-project files are
   generated from it. That keeps every package's history in a file about seventeen others.
2. **The per-project `performance.md` is the source.** Each package's measured comparisons live
   in `src/<Package>/performance.md`. `docs/guides/performance.md` keeps only what applies to
   all of them (*How to read a row*) and a link to each of the eighteen. Rejected: the guide
   stays the source, or is assembled from the per-project files by a script. Both keep one
   subject in two places.
3. **The root `README.md` explains the project and hands over to each package.** It says what
   Lodestar is for, what each package does in a sentence or two, and links to that package's
   README. Each `src/<Package>/README.md` is written for that package and is the README its
   NuGet package ships.
4. **`samples/`, `bench/` and `tests/` carry READMEs too, under option (b):** one README per
   project in `samples/` (2) and `bench/` (5), each project having a distinct purpose. In `tests/`,
   a `tests/README.md` explaining how a suite and its mirror pair up, plus one README per suite
   (18). The mirrors compile their suite's sources, so the root README covers them rather than
   eighteen near-copies. Rejected: option (a), a README in every one of the 43 projects, 18 of
   them near-identical.

## The design

### Per-package `README.md`

Written by hand for each of the eighteen, never templated from a neighbour. In this order:

1. what the package does, in the terms of its `<Description>`;
2. install, with the `dotnet add package` line;
3. **one call that shows it**, as a `csharp` fence, compiled by the doc-snippets gate;
4. the reference it is measured against, for example rapidfuzz or scikit-learn, and a link to
   `docs/equivalence.md`;
5. what it depends on, from `tools/check_nuspec_dependencies.py`'s `EXPECTED`;
6. links to its reference pages, its `CHANGELOG.md` and its `performance.md`.

**Links in a package README are absolute GitHub URLs**, because nuget.org renders the file
outside the repository and resolves no relative link.

`Directory.Build.props` packs `$(MSBuildProjectDirectory)/README.md` instead of the root one. A
publishable project without a README fails the pack with an MSBuild error naming the missing
file, rather than silently shipping the root README.

### The root `README.md`

The project's purpose and thesis stay: no Python at runtime, native code only where .NET has no
maintained equivalent. The *Packages* part becomes one row or short paragraph per package, with a
link to `src/<Package>/README.md`. Anything that describes a single package moves to that
package's README. `tools/check_readme_packages.py` keeps asserting that every package is
counted and listed.

### Per-package `CHANGELOG.md`

Keep a Changelog, one file per package:

```markdown
# Changelog — Lodestar.Fuzzy

## [Unreleased]

## [0.5.0] — 2026-09-24

### Changed

- `ExtractResult` is compiled into `Lodestar.Abstractions` under the same name and forwarded from here. ([#1142](https://github.com/CyrilB1531/lodestar/issues/1142))
```

**The history moves in full.** Every entry under the root file's `### Lodestar.X — <version>`
heading goes to `src/Lodestar.X/CHANGELOG.md` under `## [<version>] — <date>`, with its wording
unchanged. The older releases move under three rules:

- a `DataNet.X` release goes to the package it was renamed to, headed
  `## [0.3.0] — 2026-08-14 (published as DataNet.Text)`;
- the joint releases `0.1.0` and `0.2.0` predate the split. An entry naming a package goes to
  that package; an entry naming several goes to each of them. An entry naming none, such as the
  sample or CI, goes to each of the three packages the release covered, `Text`, `Embeddings`
  and `Fuzzy`, since it was part of all three releases. The release's introduction describes
  that release, so it heads the version in each of the three;
- a release block's introduction describes several packages at once. A paragraph explaining one
  package's version, such as why `Lodestar.Embeddings` went to 0.7.0 rather than 0.6.1, becomes
  a short paragraph under that version in that package's file. A paragraph only counting or
  listing packages is dropped, and stays in git history.

The move is done by a one-off script that is not committed. Two checks then prove it lost and
invented nothing, and the pull request records both results: every entry line of the root file
at the base commit appears in at least one package's file, and every entry line in a package's
file appears in the root file at the base commit.

The root `CHANGELOG.md` keeps its introduction (the per-package versioning, the one-sentence
entry rule) and a list of eighteen links. It has no entries.

**Tooling moves with the source.**

- `tools/changelog_section.py <Package> <Version>` reads `src/<Package>/CHANGELOG.md` and
  prints the body under `## [<Version>]`, so `release.yml`'s Release notes keep working. A missing
  section is still an error. `--list` walks the eighteen files.
- `tools/check_unreleased.py` reads each package's `## [Unreleased]`.
- **`PackageReleaseNotes`** is set in `Directory.Build.props` to the package's changelog at its
  own tag:
  `https://github.com/CyrilB1531/lodestar/blob/$(PackageId)/v$(Version)/src/$(PackageId)/CHANGELOG.md`.
  The link resolves once the tag exists, which is when the package reaches nuget.org. It is a
  link and not the section's text because MSBuild cannot parse the file without a custom task.
- `CONTRIBUTING.md` item 7 and `README.md`'s *Publishing* point at the per-package file.

### Per-package `performance.md`

```markdown
# Performance — Lodestar.Cluster

How to read a row: [docs/guides/performance.md](../../docs/guides/performance.md#how-to-read-a-row).

## k-means (Lloyd) against scikit-learn

(the existing ### sections under ## Lodestar.Cluster, each one heading level up)
```

Each `### <capability>` under `## Lodestar.X` in the guide becomes a `## <capability>` in
`src/Lodestar.X/performance.md`, word for word, one heading level up. A package with no measured
comparison says so in one line, **"Nothing is measured against an incumbent yet."**, and says
why when a reason exists (for example `Lodestar.Abstractions` has no incumbent of its own).

`docs/guides/performance.md` keeps its introduction, *How to read a row*, and a table of the
eighteen packages, each linking to its page and marked either as having comparisons or as having
none.

`tools/check_performance_sections.py` applies its four rules per file. `##` headings are now
comparisons, each carrying a machine, a window and an incumbent or an `EXEMPT` reason, and no
column may be named after a branch. It also asserts that the guide links each of the eighteen
and holds no comparison of its own.

**The wiki** publishes each package's `README.md` and `performance.md` in that package's section
(`docs/wiki-map.json` `pages`). `tools/build_wiki.py` already rewrites repository-relative links
to wiki names, so the guide's links keep working there. `docs/guides/performance.md` stays in the
wiki root as the index.

### READMEs for samples, benchmarks and tests

- **`samples/Lodestar.Sample/README.md`, `samples/Lodestar.DocSnippets/README.md`**: what the
  project proves (the packaging gate; the doc-snippets gate), how to run it, and why it consumes
  packed packages rather than projects.
- **`bench/<Project>/README.md`**, five of them: what the project measures and how to run one
  benchmark from it. `bench/README.md` keeps its subject, how to measure, and links to the five.
- **`tests/README.md`**: the suite and mirror pairing, why `dotnet test` runs everything twice,
  the 36-assembly count, and the oracle corpora. **`tests/<Package>.Tests/README.md`**, eighteen of
  them: what the suite covers, which oracle corpora it replays, and its mirror.

These carry no `csharp` fence, so the doc-snippets gate does not read them.

### The guard

A new **`tools/check_project_docs.py`** refuses a tree where:

- a publishable project under `src/` lacks `README.md`, `CHANGELOG.md` or `performance.md`;
- a project under `samples/` or `bench/`, or a suite under `tests/`, lacks `README.md`;
- `tests/README.md` is missing;
- the root `README.md`, the root `CHANGELOG.md` or `docs/guides/performance.md` does not link
  each package's file;
- a package's `CHANGELOG.md` does not open with `# Changelog — <Package>` and an
  `## [Unreleased]` section;
- the root `CHANGELOG.md` holds a list item other than a link to a package's changelog.

It runs in CI's `Lint` job and in the pre-commit hook, since it is offline and instant.
A nineteenth package then cannot ship without its three files.

### Knock-on changes

- **Doc-snippets gate:** `tools/extract_doc_snippets.py`'s `SOURCES` gains `src/*/README.md`,
  so each package README's call is compiled against the packed packages.
- **markdownlint:** the glob in `ci.yml`'s `Lint`, `CONTRIBUTING.md` and `CLAUDE.md` gains
  `CHANGELOG.md`, `src/*/*.md`, `samples/*/README.md`, `bench/*/README.md`, `tests/README.md` and
  `tests/*/README.md`. They are narrow on purpose: a `tests/**/*.md` glob walks every `bin/` and
  `obj/` of a built tree, and locally it did not finish.
- **`CLAUDE.md`'s *Where a fact belongs*:**
  - new rows for `src/<Package>/README.md`, `CHANGELOG.md` and `performance.md`, and for the
    `samples/`, `bench/` and `tests/` READMEs;
  - the rows for the root `CHANGELOG.md` and `docs/guides/performance.md` become *an index of
    the per-package files*.
- **The one-sentence entry rule** (`CONTRIBUTING.md` item 7) is unchanged; only the file an
  entry goes in moves.

## What stays out

- **The root `README.md`'s thesis, and `docs/equivalence.md`**, which stay repository-wide.
- **Reference pages under `docs/reference/`**, already per namespace.
- **New measurements.** The seven packages with none get the one-line statement, not a benchmark.
- **A README per netstandard mirror.**

## Done

- `src/<Package>/README.md`, `CHANGELOG.md` and `performance.md` exist for all eighteen, each
  written for its package; each NuGet package ships its own README and a `PackageReleaseNotes`
  link to its own changelog, checked in the packed `.nuspec`.
- The root `CHANGELOG.md` and `docs/guides/performance.md` are indexes; the root `README.md`
  describes each package and links its README.
- The history check over the changelog move passes, and its result is in the pull request.
- `changelog_section.py` prints the right section for every `<Package> <Version>` the old root
  file listed, which `release.yml` depends on.
- READMEs exist for the 2 samples, the 5 benchmark projects, `tests/` and the 18 suites.
- `tools/check_project_docs.py` passes, runs in `Lint` and the pre-commit hook, and has tests.
- The doc snippets, including the eighteen package README calls, compile and run; markdownlint
  covers the new paths; the wiki build publishes the new pages.
