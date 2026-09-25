# Development tools — oracle generation, vendored data, packaging checks

Scripts under `tools/`, each responsible for one input the test suite treats as
given:

- `generate_oracles.py` produces the reference values the test suite replays.
- `seeded_random.py` is not a script and has no command line: it is the seeded
  `random.Random` that `generate_oracles.py` and the five
  `bench/corpus/generate_*.py` draw every number through. Each method delegates
  unchanged, which is what keeps every existing seed's sequence — and therefore
  every committed corpus — intact. It exists because Sonar's S2245 cannot know
  that test data protects nothing and fires once per call site, so the waiver
  belongs in one file rather than in thirty pragmas scattered over the
  generators, where they drift away from the code they were meant to cover.
- `fetch_stopwords.py` produces source that is *shipped*, which is why it
  verifies what it downloaded before writing anything.
- `fetch_xlmr_vocab.py` and `build_normalizer_fixtures.py` produce fixtures
  `generate_oracles.py` reads.
- `fetch_gpt2_bpe.py` vendors GPT-2's 50 257-entry vocabulary and its merge
  table into `tests/oracles/` — those two files only, never weights (decision
  0002). The size is the point: a self-trained toy model exercises no merge
  table with 50 000 ranks and proves nothing about reading the `merges.txt`
  layout a real model ships, so `ByteLevelBpeTests`' byte-exact parity claim
  rests on the real vocabulary. `--check` verifies the checked-in fixtures
  against upstream instead of writing them.
- `fetch_llama2_mistral_tokenizers.py` vendors the Llama-2 and Mistral v0.1
  `tokenizer.json` — the two files [#175](https://github.com/CyrilB1531/lodestar/issues/175)
  opens on, which spell the same SentencePiece-BPE pipeline two different ways,
  which is why the corpus needs both rather than one file twice.
  `meta-llama/Llama-2-7b-hf` is gated and answers 401 without credentials this
  project does not have, so decision 0005 section 5's method applies: two
  ungated mirrors, held to agreeing on the sections that decide what a text
  encodes to, stand in for reading the original — and that agreement is
  re-asserted on every fetch rather than recorded once. Decision 0002 admits the
  Llama 2 Community License as a named exception for this artifact alone, which
  is why the tool also writes its three licence files into
  `docs/vendored/llama2/`, from the same mirror as the artifact. `--check`
  verifies rather than writes. Its docstring has why
  `NousResearch/Llama-2-7b-hf` is the right mirror for Llama-3 and the wrong one
  here — 119 merges in a different order, and merge order is rank in BPE.
- `build_tiny_models.py` builds fixtures too small for that pipeline to bother
  with — two ONNX graphs, a trained BPE, and a hand-constructed BPE — and
  commits them directly.
- `check_nuspec_dependencies.py` verifies what the packages *declare*.
- `compare_oracles.py` compares two directories of corpora the way the suites
  do — floats at `1e-9`, relative where the suite compares relatively, everything else exactly — which is what the
  `Oracles are reproducible` gate asks instead of byte-identity
  (tools/compare_oracles.py).
- `check_version_floor.py` verifies that the version numbers the source tree
  keeps in three places still agree.
- `check_pr_closes.py` refuses a pull request that closes no issue, or whose
  closing keyword names an issue that is not open here or not assigned to its
  author; `pr-closes.yml` runs it on every pull request.
- `check_requirements_lock_sync.py` refuses a `requirements.txt` pin that
  `requirements.lock.txt` has never heard of, which is what CI installs from.
- `check_gpu_tests_force_cpu.py` refuses a `Lodestar.Gpu` test that uses whatever
  accelerator the machine happens to have.
- `check_netstandard_guards.py` refuses a netstandard2.0 mirror that carries no
  assembly guard, or that leaves one of its library's Lodestar dependencies, direct
  or transitive, unpinned and therefore loaded from net10.0.
- `check_machine_paths.py` refuses a tracked file that holds a path under
  someone's home directory.
- `check_sdd_citations.py` refuses a file that cites a task's report or brief from a
  plan's workspace, which git ignores and the plan deletes when it finishes.
  `docs/superpowers/`, the vendored `.claude/skills/` and the one accepted ADR
  that already carries such a citation are exempt.
- `check_spec_status.py` refuses a spec that does not say when it was written, and
  a plan that is committed at all.
- `check_performance_sections.py` refuses a comparison in a package's
  `src/<Package>/performance.md` that names no incumbent, no machine or no window, any table
  column named after a branch, and a `docs/guides/performance.md` index that holds a comparison
  or misses a package.
- `check_project_docs.py` refuses a package without its own `README.md`, `CHANGELOG.md` and
  `performance.md`, a sample, benchmark or test suite without its `README.md`, a root index
  (`README.md`, `CHANGELOG.md`, `docs/guides/performance.md`) that misses a package, and an
  entry left in the root `CHANGELOG.md` (#1133).
- `check_comment_length.py` refuses a comment block that runs past its budget
  without saying why.
- `check_no_console_writeline.py` refuses a `Console` call in a shipped package,
  and an unexplained one in a benchmark.
- `check_sample_culture.py` refuses a sample that can print a number in the
  contributor's culture rather than the same way everywhere.
- `check_sample_coverage.py` refuses a public class with no `<Class>Sample.cs`,
  package by package as each is split (CONTRIBUTING.md's Definition of done). The packaging gate already
  asks that a type be *referenced*; this asks which file references it, so an
  example stays where its name says it is.
- `check_readme_pack_loop.py` refuses a README whose pack loop cannot restore the
  sample it is followed by. Six hard-coded pack lists exist in this repository
  (seven until #1028); every one that something reads stayed current, where this
  one packed nine of fifteen until #597. `samples/Lodestar.Sample.csproj`'s own references are the source of
  truth, and order is not compared.
- `check_claude_md_packages.py` refuses a CLAUDE.md architecture table that has
  drifted from `src/`. It checks three things against what owns them: the row set
  against the directories under `src/`, the count in the prose against that same
  set, and the inter-package edge count against
  `check_nuspec_dependencies.py`'s `EXPECTED`. The *Holds* column is prose and is
  deliberately not checked.
- `check_readme_packages.py` refuses the prose beside those two lists when it has
  drifted instead. `README.md`'s Publishing paragraph, its Structure tree and
  `CLAUDE.md`'s quick-commands pack loop each understated the repository by seven
  packages until #688 — every one of them next to a list a guard already held
  current, which is why none was noticed. `src/` is the source of truth for which
  packages exist and CLAUDE.md's architecture table for which tier each is in, so the
  core-tier count is checked too. The counts are spelled out, and that is the half a
  contributor forgets.
- `check_bench_map.py` refuses a `[Benchmark]` class that `bench/bench-map.json`
  does not name. The nightly run (#11) measures only the classes a night's diff
  selected and reads that map to decide which, so a class missing from it is
  never selected and stops being measured with nothing going red. The map cannot
  be derived — `FuzzBenchmarks` names only `Fuzz`, which reaches `Indel`, then
  `Lcs`, then `Affixes` — so it is hand-written, and this guard holds it to what
  it can check: completeness. Whether a class's globs are *right* is not, and
  being too narrow is invisible here; `bench/README.md` has why they are written
  at directory granularity. Since
  [#649](https://github.com/CyrilB1531/lodestar/issues/649) it also refuses a
  `bench/*/*.csproj` that `Lodestar.slnx` does not list, which is the same
  silence one layer down: `bench/Lodestar.Gpu.Benchmarks` was outside the
  solution, so no build reached it and raising the analyser broke it unseen.
- `classify_change.py` answers two questions from a change's file paths, and keeps them
  apart: **ships** is `src/<Package>/` only, which decides the milestone because a milestone
  names a release; **about** also counts `tests/`, `bench/` and the documentation pages
  `docs/wiki-map.json` attributes, which decides the boards and the labels. It resolves
  `DataNet.*` to its post-rename name, refuses a bare prefix like `DataNet.NetStandard` that
  names no package, and reports a path it cannot attribute rather than folding it into the
  cross-cutting bucket, and reports a shared build file under `src/` — `Directory.*.props`,
  `Shared/` — as shipping without naming a package, since those reach every `.nupkg` and
  the milestone asks whether a change ships rather than which packages (#638).
  #628 has the three defects each of the others closes.
  `.github/workflows/classify-pull-request.yml` calls it when a pull request opens and
  on every push to it, so the attribution follows the diff rather than a first guess.
  Its `unattributed` count is for a reader, not a label: a pull request always has its
  file list, so touching no package is an answer rather than a gap.
- `check_unreleased.py` reads what each package has merged and not published, from the tags,
  `main` and `src/<Package>/Version.props` — none of which can drift. Unpublished work is the
  normal state between a merge and a release, so it is reported; only a version declared past
  its own tag fails, which is a release prepared and never cut. A missing entry under the
  package's own `## [Unreleased]` is a note rather than a failure, because a commit touching only XML comments owes the
  changelog nothing and nothing here can tell the two apart.
- `changelog_section.py` prints the `## [<Version>]` section of `src/<Package>/CHANGELOG.md`
  for one package release (a `DataNet.*` release is found in its Lodestar file), which is
  what `release.yml` hands to `gh release create` as the Release body. A missing section is an
  error rather than an empty body: CONTRIBUTING.md makes the entry item 7 of the definition of
  done and says nothing gates it, so this is that gate. `--list` prints every release the
  package changelogs carry, which is what the #626 backfill read.
- `check_release_workflow_packages.py` refuses a release workflow whose hard-coded
  package list has drifted from `src/`. `release-nuget-org.yml`'s `options:` and
  `release.yml`'s `case` allow-list must each name every `src/Lodestar.*` holding a
  `Version.props`, and may name nothing else; four packages were missing from both
  until #610, so a third of the 0.6.0 milestone could not be published. It also
  refuses a `case` pattern wrapped with a backslash, which keeps the next line's
  indentation instead of joining the alternatives. Order is not compared.
- All of those but `check_nuspec_dependencies.py`, which needs a packed
  `./artifacts`, also run before a commit for whoever installs
  `.githooks/pre-commit` with `git config core.hooksPath .githooks` —
  CONTRIBUTING.md's [*Before
  committing*](../CONTRIBUTING.md#before-committing-the-guards-one-command-earlier)
  has what it does and does not cover. About a second is the whole budget, and it
  is already spent: measured over the whole tree on one machine, 0.97 s for
  `check_machine_paths.py`, 0.12 s for `check_comment_length.py`, 0.06 s for
  `check_version_floor.py` and 0.03 s for `check_sample_culture.py` — 1.18 s for
  those four in sequence. A guard that reached `dotnet build` would be
  uninstalled within a week.
- `check_adr_immutable.py` refuses a pull request that touches a
  `docs/decisions/` ADR that already existed at its base commit, addition
  included — an accepted decision is never edited, only amended by a new one.
  One exception, tools/regen_adr_index.py: a YAML frontmatter block inserted above the title
  with the body below it byte-identical. Not part of the pre-commit set above: it
  needs the pull request's own base commit, not something a commit made before one
  exists can name.
- `check_adr_frontmatter.py`, `check_adr_index_sync.py` and
  `check_adr_index_is_cited.py` hold `docs/decisions/index.yaml` to the records it
  is generated from, and hold the two process documents to sending a reader there.
  All three are offline and run before a commit; `## The ADR index` below has what
  each one refuses.
- `check_repeated_literals.py` refuses a pull request that pushes a Python string
  literal in `tools/` past SonarCloud's S1192 threshold — measured on
  [#488](https://github.com/CyrilB1531/lodestar/pull/488) as more than three
  occurrences, with the issue anchored on the literal's first one, so only a
  literal the change both pushes over and introduces is reported. Like the ADR
  guard it needs the pull request's base commit, and for the same reason it is not
  in the pre-commit set above: `tools/generate_oracles.py` already holds
  some 108 literals over the threshold, so the only useful question is what a
  change *adds*. `--report` prints that standing backlog without failing.
- `generate_sonar_globalconfig.py` writes the `.globalconfig` that raises the
  Sonar rules `SonarAnalyzer.CSharp` ships disabled, from the SonarCloud
  quality profile that gates the pull request.
- `count_cited_claims.py` counts the comment blocks that name a reference
  library and how many of them point at something a reader can open — a corpus
  file, an oracle case, an ADR, an issue, a test class, a stated measurement. It
  reports and exits 0 whatever it finds; the guard that blocks is
  `check_comment_length.py`, whose block parser this borrows rather than
  re-implementing. It exists because the rule needs a home: #151's sweep
  reported "162 claims, 0 cited" counting *lines* and looking for the citation
  on the same line, which a block almost never satisfies, and the corrected
  figure was then quoted as prose in a commit message where it did not reproduce
  for a reviewer, because nothing said what counted as a citation. A
  `<see cref>` does not — it points at another member of this library, not at
  what checks the claim, and counting it reads 45% where the honest figure is
  12%. Takes path prefixes to narrow the sweep.
- `extract_doc_snippets.py` turns the ` ```csharp ` fences in `README.md` and
  `docs/` into a project the compiler — and, for the reference pages, the
  runtime — can judge.
- `build_wiki.py` produces what the GitHub wiki publishes: `docs/` turned into
  a flat page per package channel and per released version.
- `select_benchmarks.py` names the benchmark classes a range of commits makes
  worth re-running (#11): it reads `bench/bench-map.json`, asks git what moved
  since the baseline, and prints one class per line — empty when nothing
  relevant moved. Both of its biases run toward measuring too much. An entry
  under `always` selects every class, and the globs are directory-wide, because
  a benchmark run for nothing costs minutes while one not run hides a regression
  and nothing goes red when it does. An absent baseline selects everything, so a
  run that never happened cannot silently narrow the next one. Takes
  `--since <commit>` with an optional `--head`, or `--all`, and `--harnesses`
  for the Python comparison harnesses the same diff selects.
- `render_nightly.py` renders the page the nightly run publishes to the wiki,
  from the BenchmarkDotNet reports and `bench/compare.py` output the job left on
  disk. It carries the baseline commit the *next* run reads, which is why the
  result is a file under `docs/guides/` rather than an artefact: the series has
  to remember where it stopped. It is deliberately not `docs/guides/performance.md`, whose
  numbers are comparable to each other because the machine is named — a hosted
  runner is a shared VM whose hardware differs between runs, so what survives
  here is the ratio inside one run, never the absolute figure. Takes `--commit`
  and `--baseline` with the run's two shas, `--runner` for the label, and
  `--selected`/`--harnesses` for what the night measured; `--branch` writes the
  copy under `docs/guides/branch/` instead, which is what a
  `workflow_dispatch` off another ref uses so its numbers never touch the page a
  merge into `main` could carry along
  ([#379](https://github.com/CyrilB1531/lodestar/issues/379)); `--stdout` is
  what a local check uses.
- `render_benchmark_latest.py` answers what that page cannot: when each method
  was last measured at all. A class quiet for weeks does not reappear on
  `nightly_run.md` until something near it moves, so this walks the wiki clone's
  own git log for that page and keeps the newest occurrence per section, with
  tonight's fresh reports taking priority over the history. Run it *alongside*
  `render_nightly.py` rather than after its wiki push, or a class measured
  tonight waits for tomorrow's aggregate to notice it. Numbers from different
  nights ran on different VMs and rank nothing against each other; the page says
  "as of when". Takes `--wiki <path to a lodestar.wiki.git clone>`, with
  `--commit` and `--max-commits` to stamp and bound the walk, and the same
  `--branch` and `--stdout` as its neighbour.
- `nightly_series.py` is the nightly's memory
  ([#672](https://github.com/CyrilB1531/lodestar/issues/672)). It keeps every BenchmarkDotNet `Ratio`
  the nightly page publishes in `bench/nightly/ratios.csv`, with the runner's CPU, and reports the
  ratios that moved. A movement is a step past the larger of 30% and four times the key's own noise,
  or a drift past 20% over ten days, as
  `docs/guides/nightly_run.md`
  measured. It has three subcommands:
  - `compare` appends the "Ratios that moved" section to `docs/guides/nightly_run.md`, or to the
    branch copy with `--branch`, and prints one `::warning::` per new movement;
  - `record --date <YYYY-MM-DD>` appends main's page to the series;
  - `backfill` rebuilds the series from the git history of `docs/guides/nightly_run.md`.

  Every path it writes is a constant chosen by a flag, never an argument, as its neighbours'
  are.

  `compare` and `record` take `--wiki <clone>`, and `backfill` accepts it too. With it, a night
  whose pull request never merged still reaches the series from the wiki's history. Runs of
  commits that are not in this checkout's history are skipped.
  The nightly runs `compare` before `record`, and records only on `main`.
- `sonarqube-local/` holds the compose file for a disposable local SonarQube
  server, covering the Python rules, duplication and coverage that no local
  `dotnet build` reaches. [`sonarqube-local/README.md`](sonarqube-local/README.md)
  has the commands, what one run cost, and why it is not a rehearsal of CI.
- `tests/` holds the pytest suite CI runs over these scripts, and one of its
  files holds *this page* to them: `test_readme_covers_the_tools.py` fails when
  a `tools/*.py` or `tools/*.cs` is named nowhere here. The list above is the
  document's contract rather than a courtesy, so a new script arrives with its row
  ([#652](https://github.com/CyrilB1531/lodestar/issues/652),
  [#619](https://github.com/CyrilB1531/lodestar/issues/619)). A few of those tests
  read a workflow instead of a script: `test_end_analysis_retry.py` runs `ci.yml`'s
  own `End analysis` script against a fake scanner, so the retry that is only for
  an outage, and the pipeline that keeps the scanner's exit status, are asserted
  rather than reviewed ([#1081](https://github.com/CyrilB1531/lodestar/issues/1081)).

## `survey.cs`

The one tool here that is not Python, because `MetadataLoadContext` is a .NET API.
It reads a NuGet package's **exported surface**, which is what
[decision 0004](../docs/decisions/0004-what-is-written-here-and-what-is-delegated.md)
requires before a gap claim may be written down — against the assembly, never against
the README.

```bash
dotnet run tools/survey.cs -- <package> <version> [regex] [--assembly <name>]
dotnet run tools/survey.cs -- Microsoft.ML.TimeSeries 5.0.0 'Arima|Acf|Stationar'
dotnet run tools/survey.cs -- Microsoft.ML 5.0.0 'Pca' --assembly Microsoft.ML.PCA
```

**`--assembly` because a package id is not an assembly name.** `Microsoft.ML` 5.0.0 installs no
`Microsoft.ML.dll` at all: its surface is spread over `Microsoft.ML.Data`, `Microsoft.ML.PCA` and
six more, and `Microsoft.ML.PCA` is not a package id anyone can install. Deriving the target from
the package id made that surface unreadable, which was found on the tool's second use (#685). The
error lists the closure, so the name to pass is one it printed.

A **file-based app**, so there is no `.csproj`, no entry in `Lodestar.slnx` and nothing
for CI to build. The `#:package` and `#:property` directives at the top are the whole
project file; the trim and single-file analysers are off because reading an arbitrary
assembly's surface cannot satisfy them by construction.

**It publishes the package before reading it**, and that is the point rather than an
implementation detail. A bare `lib/*.dll` cannot be opened — `GetExportedTypes` needs
every assembly its signatures mention — and that trap has now been paid twice, on
`Mosaik.Core` (0074) and on `Microsoft.ML.TimeSeries`, which drags `Microsoft.ML`, an
MKL redistributable and `Newtonsoft.Json` behind it.

**Three member counts, one of them named.** Type counts reproduced exactly across five
decisions; member counts did not, and the difference was what each ad-hoc run happened
to include. Measured while writing this:

| package | types | quote this | with accessors and operators | without constructors |
| --- | --- | --- | --- | --- |
| `Microsoft.ML.TimeSeries` 5.0.0 | 34 | **124** | 155 | 111 |
| `MathNet.Numerics` 5.0.0 | 336 | 5 707 | 6 938 | **5 335** |

Decision 0004's "124 members" is the third column and reproduces on the nose; decision
0003's "5 333 members" is the fifth, two apart on a different SDK. Neither was wrong and
neither said which it was, so all three are printed and **a new record quotes the third
column** — declared public members, accessors and operators excluded, constructors kept,
because a caller calls those.

A package that installs no assembly is reported with the closure's actual contents rather
than as a blank, which is the finding 0104 recorded for `cs-glm` 1.0.1.

## `generate_oracles.py`

`generate_oracles.py` produces the frozen reference corpora under
`tests/oracles/`, from the canonical Python libraries. **These libraries are
development dependencies only** — never runtime ones: the C# deliverable depends
only on the committed JSON.

## Regenerate

[`../CONTRIBUTING.md`](../CONTRIBUTING.md#oracle-validation) has the command, because the
virtualenv, the interpreter floor and the neutral working directory it needs are all part of one
procedure. Two of the three fail silently when guessed: an interpreter below **3.12** stops the
generator with a sentence (`tools/python_floor.py`, and
`tools/python_floor.py`),
and a working directory above the virtualenv makes `nltk` refuse its own imports.

The script is **deterministic** (fixed seed, no timestamps): regenerating on another machine
produces the same corpus — the same cases, in the same order, with the same values to the
precision anything asserts on. It does *not* produce the same bytes, and it cannot: the last
digits of a BLAS-reduced value follow the CPU the generator ran on. That is why the gate compares
with `compare_oracles.py` below rather than with `git diff` (`tools/compare_oracles.py`).
Committing the regenerated JSON is part of the change.

Two generators need a package the lock doesn't install, each pinned in
`tools/requirements-nodeps.txt` instead, for a different reason: `mmr.json` needs keybert, whose
own dependencies (sentence-transformers, and through it torch) never enter `requirements.lock.txt`
because `generate_mmr` calls only `keybert._mmr.mmr`, which imports nothing but numpy and
scikit-learn — installed with `pip install --no-deps --require-hashes` so those dependencies are
never resolved at all. `keywords_textrank.json` needs summa, which publishes no wheel and so fails
the `--only-binary :all:` every install site otherwise passes; installing this file adds
`--no-binary summa` to let its sdist build, hash-checked like everything else here.
[`../CONTRIBUTING.md`](../CONTRIBUTING.md#dependencies) has the full reasoning.

## `compare_oracles.py`

Compares two directories of corpora and reports only what the test suites would notice:

```bash
python tools/compare_oracles.py <expected-dir> <actual-dir>
```

Floats agree within `1e-9` absolute, which is the tolerance the suites replay these corpora at;
integers, strings, booleans, nulls, an object's key set *and* key order, an array's length and
order, and the set of files must all match exactly, and so must a non-finite value. Exit `0`
clean, `1` with the differences printed as `::error::` lines naming each JSON path, `2` on bad
usage.

The `Oracles are reproducible` job copies `tests/oracles` aside, regenerates, and runs this over
the two. To ask the same question locally, do the same: copy the committed corpora somewhere,
regenerate, and compare — the generator writes in place, so there is nothing to compare against
otherwise.

## `draw_icon.py`

Draws `assets/icon.png`, the icon every published package embeds. Run by hand and
rarely — the output is committed, so a fresh clone needs nothing:

```bash
python tools/draw_icon.py
```

**Pillow is its only dependency, and it is deliberately not in
`requirements.txt`.** That file feeds a hash-pinned lock every CI job installs,
and no job draws an icon; adding it there would make three workflows carry an
image library for nothing. Install it ad hoc when you need to redraw:
`pip install Pillow`.

The star is drawn at eight times the final size and downsampled, because its
points are thin and a 128-pixel canvas has no room for a jagged edge. Check the
result at **32 pixels** as well as at 128 — that is the size the package list
shows, and a mark that dissolves there is the wrong mark.

## `fetch_stopwords.py`

Regenerates `src/Lodestar.Text/Vectorization/StopWords.Snowball.cs` from the
Snowball stop-word lists (BSD-3-Clause). No third-party package needed — the
standard library is enough:

```bash
python tools/fetch_stopwords.py            # regenerate
python tools/fetch_stopwords.py --check    # verify the checked-in file is current
```

Each file is checked against a pinned SHA-256 before use. A mismatch means
Snowball edited the list upstream: read the diff, update the pin, adjust the
counts in `StopWordsTests`, and record it — do not regenerate quietly. The nltk
stop-word corpus is **not** a permitted source here, whatever its convenience:
see [`../docs/decisions/0002-provenance-and-the-allowed-references.md`](../docs/decisions/0002-provenance-and-the-allowed-references.md).

## `fetch_xlmr_vocab.py`

Rebuilds `tests/oracles/xlmr_fairseq.model`, the fixture behind the XLM-R oracle:

```bash
python tools/fetch_xlmr_vocab.py            # rebuild
python tools/fetch_xlmr_vocab.py --check    # verify the checked-in fixture
```

It downloads `xlm-roberta-base`'s SentencePiece vocabulary (MIT — vocabulary
only, never weights), checks it against a pinned SHA-256, and **re-emits** it:
same 250 000 pieces, scores and types, at the ids HuggingFace gives them
(`<s>`=0, `<pad>`=1, `</s>`=2, `<unk>`=3, `<mask>`=250001). The relabelling is
the point — the stock file is laid out `<unk>`=0/`<s>`=1/`</s>`=2, which is the
one layout the old id-based control filter got right. The `normalizer_spec` is
copied across untouched, `nmt_nfkc` and its character map included; it was
overwritten with `identity` until #75 made the map readable. See
[`../docs/decisions/0005-the-proof-standard-and-the-oracle-each-family-is-frozen-from.md`](../docs/decisions/0005-the-proof-standard-and-the-oracle-each-family-is-frozen-from.md).

Like `tiny_sp.model`, the result is an *input* to `generate_oracles.py`, not one
of its outputs: it is committed, and the `Oracles are reproducible` job replays
it without touching the network. Rebuilding it is a deliberate act, and a
changed pin means the ids in `xlmr_fairseq.json` move with it.

## `build_normalizer_fixtures.py`

Trains the two small models the normalizer oracle needs beyond the XLM-R one:

```bash
python tools/build_normalizer_fixtures.py            # rebuild
python tools/build_normalizer_fixtures.py --check    # verify the checked-in fixtures
```

`nmt_nfkc_cf.model` carries the case-folding map; `custom_norm.model` carries a
map compiled from three hand-written rules and calls its normalizer nothing more
than `user_defined`. The second is the one that keeps the claim honest: what
`PrecompiledNormalizer` interprets is *a* character map, not the one map every
stock model happens to share. Both are committed inputs to
`generate_oracles.py`, like `tiny_sp.model` — CI never retrains them, and
training is not guaranteed reproducible across `sentencepiece` versions.

## `build_tiny_models.py`

Builds fixtures too small to fit the pipeline the other scripts share: two
ONNX graphs (`tiny_encoder.onnx`, `tiny_embedder.onnx`), a trained
character-level BPE (`tiny_bpe.json`), a hand-constructed BPE holding one
orphaned vocabulary entry (`orphan_bpe_model.json`), and a hand-constructed
BPE carrying `roberta-base`'s own `added_tokens` table
(`roberta_shaped_model.json`) — five fixtures, all committed rather than
rebuilt by CI. See the module docstring for what each one proves.

`tiny_bpe.json`'s trainer is not byte-reproducible across runs: tokens and
merges that tie in frequency land at different ids each time, because the
Rust `HashMap` behind the frequency counts seeds its hash randomly per
process. So, like `tiny_sp.model`, the committed file is authoritative and
rebuilding it is a deliberate act — a diff there is expected, and must not be
committed without regenerating `bpe.json` in the same commit.

`orphan_bpe_model.json` has no such caveat: its vocabulary and merge table are
stated directly rather than learned, so rebuilding it is byte-reproducible —
running it again and diffing is a legitimate way to confirm nothing changed.

## `check_nuspec_dependencies.py`

Asserts that the `<dependencies>` of every packed `.nupkg` match an expected
table, exactly — an unexpected dependency fails as loudly as a missing one:

```bash
python tools/check_nuspec_dependencies.py artifacts
```

A package's dependency graph is a *build output*, derived from whatever restore
resolved, so nobody writes it down and nothing notices when it drifts. This
script is where it is written down. It matters more since the four packages
version independently: `Lodestar.Fuzzy` reaches `Lodestar.Text` through a
`PackageReference`, and that edge is now the one thing holding the two together.
See [`../docs/decisions/0001-the-foundations-target-frameworks-comparison-unit-persistence-and-versioning.md`](../docs/decisions/0001-the-foundations-target-frameworks-comparison-unit-persistence-and-versioning.md).

Dependency **ids and version ranges** are both asserted. The range matters as
much as the id here: a `PackageReference` emits the floor from
`src/Directory.Packages.props`, while the `LodestarUseProjectRefs` developer loop
emits `Lodestar.Text`'s own current version. Same id, different number — which is
what lets this check catch a package accidentally built with the escape hatch
left on.

## `check_netstandard_guards.py`

Refuses a `tests/*.NetStandard.Tests` mirror that cannot prove it mirrors.

```bash
python tools/check_netstandard_guards.py
```

Two rules, both of which failed silently before [#529](https://github.com/CyrilB1531/lodestar/issues/529).

**Every `Lodestar.*` package the library depends on, directly or through another one,
needs its own pinned `ProjectReference` in the mirror.** `SetTargetFramework` does not travel across a
`PackageReference`: NuGet resolves package assets against the *consuming* project's
framework, which for a mirror is `net10.0`. So a mirror that pins only its own library
still loads its dependencies' `net10.0` build. Measured on 2026-09-02: `Lodestar.Text`
and `Lodestar.Decomposition` were running 832 tests against the `net10.0`
`Lodestar.Abstractions`, all green. The check follows each dependency's own
`src/` project to the end of the chain, because a leak one hop down is the same leak:
the `Lodestar.Fuzzy`, `Lodestar.Extensions.VectorData` and `Lodestar.Stats.TimeSeries`
mirrors pinned `Lodestar.Text` or `Lodestar.Decomposition` and loaded the `net10.0`
`Lodestar.Abstractions` beneath them
([#888](https://github.com/CyrilB1531/lodestar/issues/888)).

**Every mirror carries `NetStandardAssemblyGuardTests.cs`**, which reads the loaded
assembly's `TargetFrameworkAttribute` at run time. Three of seven mirrors had no such
file, which is how the first rule's breakage survived unnoticed.

A mirror therefore reports more tests than its sibling suite, by exactly the number of
guard facts it carries — one per assembly it must prove it loaded.

## `check_version_floor.py`

Checks the three places a `Lodestar.Text` version number lives, each for a
different reason, none of which MSBuild relates to the others:

```bash
python tools/check_version_floor.py               # offline: the two rules below
python tools/check_version_floor.py --check-feed  # also: the floor is published
```

- `src/Lodestar.Text/Version.props` — what `Lodestar.Text` *is*.
- `src/Directory.Packages.props` — the *floor* `Lodestar.Fuzzy` requires of it.
- `check_nuspec_dependencies.py` — the floor that check asserts actually shipped.

The floor must not exceed the declared version, and must already be on nuget.org
— that is what makes `git clone && dotnet build` work with no pack step. A floor
naming an unpublished version still builds for whoever raised it, whose cache is
warm, and fails for everyone else. `--check-feed` is what turns that into a CI
failure rather than a contributor's bug report.

## `check_pr_closes.py`

Reads a pull request's description and holds three rules on its closing keywords —
`close`, `fix` and `resolve` in any tense and case, with an optional colon:

- there is at least one, since `Refs #N` alone does not count;
- each names an open issue of this repository, not a closed, missing or locked one,
  a pull request, or an issue elsewhere;
- each issue is assigned to the pull request's author.

A bot author in `BOTS` (Dependabot, Renovate, the nightly run's
`github-actions[bot]`) and the `no-issue` label waive the first and third rules,
never the second. Nothing inside code or an HTML comment is read. `pr-closes.yml`
runs it as the required check `Pull request closes only open issues`
([#1152](https://github.com/CyrilB1531/lodestar/issues/1152)).

```bash
gh pr view <number> --json body -q .body \
  | python3 tools/check_pr_closes.py --repo CyrilB1531/lodestar --author <login> --labels <a,b>
```

`GH_TOKEN` or `GITHUB_TOKEN` authenticates the lookups when set.

## `check_requirements_lock_sync.py`

`requirements.txt` is written by hand and `requirements.lock.txt` is `pip-compile`
output, so they drift in exactly one direction: a pin added to the first and never
compiled into the second.

```bash
python tools/check_requirements_lock_sync.py   # offline, instant
```

CI installs from the **lock**, so that drift is invisible locally to anyone whose
virtual environment already holds the new package. Measured on 2026-09-10: #604 added
`datasketch` and `simhash` to `requirements.txt` alone, every local check passed, and
the `Oracles are reproducible` job stopped on `ModuleNotFoundError`.

Only the **direct** pins are compared, and names are folded per PEP 503 — `pip-compile`
writes `rake_nltk` where `requirements.txt` says `rake-nltk`, and those are one name.
Three things it deliberately leaves alone:

- **Transitive pins.** The lock holds far more than `requirements.txt` names, which is
  the whole point of compiling it.
- **Hashes.** `pip install --require-hashes` verifies those and already runs.
- **Whether the lock is a *current* compile.** Asserting that means resolving the index,
  which an offline guard cannot do, and the install step catches a stale resolution.

A dependency `requirements.txt` declares without pinning is reported rather than
skipped: it cannot be compared against a lock, and silence would make the guard weaker
than it looks.

## `check_gpu_tests_force_cpu.py`

`Lodestar.Gpu`'s suites run in CI on a runner with no graphics hardware, and
`GpuContext.Create()` falls back to the CPU accelerator rather than throwing.

```bash
python tools/check_gpu_tests_force_cpu.py   # offline, instant
```

**That fallback is deliberate and it is also the trap.** A test written with a bare
`Create()` passes on a developer machine *using the GPU* and passes on the runner *using
the processor*, so the suite silently asserts different things in different places.
Nothing else catches it: both outcomes are green, the difference is invisible in a log,
and what it hides is the class of bug that only appears on a card — a group size, a
memory limit, a synchronisation the CPU accelerator happens to serialise.

So a correctness test passes `preferCpu: true`, and the few whose subject *is* the
preferred device are named in the script's `EXEMPT` set with the reason. Benchmarks are
deliberately out of scope: their whole purpose is the device a machine actually has, and
`bench/README.md`'s GPU gate
requires them to report which one produced a figure rather than to force one.

## `generate_sonar_globalconfig.py`

Writes the root `.globalconfig`: the rules SonarCloud's quality profile activates
for this project and that `SonarAnalyzer.CSharp` ships disabled, raised to
`warning` so `dotnet build` enforces them instead of the quality gate three
minutes after a push. This is the file [`../CONTRIBUTING.md`](../CONTRIBUTING.md#analyzers)
points at, and where a contributor lands after CI prints a diff at them on the
**`Sonar globalconfig is current`** job.

It needs two inputs: one anonymous SonarCloud call it makes itself, and a SARIF
v2 error log from a local build, which only the build can produce. Both the
error log's path and the `.globalconfig` it writes are fixed constants in the
script, not command-line arguments — the tool takes no path or URL of any kind
(issue #131), so there is nothing to pass beyond `--check`. Run from the
repository root, so `$(pwd)` is `ROOT`:

```bash
mkdir -p obj
dotnet build src/Lodestar.Fuzzy -c Release --no-incremental -f net10.0 \
  -p:ErrorLog=$(pwd)/obj/sonar-rules.sarif%2Cversion=2
python tools/generate_sonar_globalconfig.py
```

The path handed to `-p:ErrorLog` must be absolute. MSBuild resolves a relative one
against the project directory (`src/Lodestar.Fuzzy`), not the shell's current
directory, so a relative `obj/sonar-rules.sarif` here would write to
`src/Lodestar.Fuzzy/obj/` and the script — which always looks under the
repository's own `obj/` — would report the input missing.

The `%2C` is load-bearing, not decorative — it is the URL-encoded comma MSBuild
needs between the SARIF path and `version=2`. A bare comma there is parsed as two
separate properties, and `ErrorLog` falls back to its default SARIF **v1**, which
carries no `defaultConfiguration.enabled` flag at all. `disabled_rules()` would
then read an empty rule table and the generated file would enforce nothing, with
no error to say why. `obj/sonar-rules.sarif` is where the script always looks —
`obj/` is already git-ignored, so the error log never becomes something to commit
or clean up by hand.

`--check` compares the regenerated file against the committed one instead of
writing it — no `.globalconfig` in the tree is touched. This is what the `Lint`
job runs on every pull request:

```bash
python tools/generate_sonar_globalconfig.py --check
```

A regenerated file that differs means the SonarCloud profile moved; an
unreachable API is reported separately and never as drift, so a network hiccup
cannot make the check pass on a stale file.

## `extract_doc_snippets.py`

Turns every ` ```csharp ` fence in `README.md`, `docs/guides/*.md` and
`docs/reference/*/*.md` into one method in a generated, git-ignored project
under `samples/Lodestar.DocSnippets/Generated/` — the guides stay the single
source of truth, so a snippet and its compiled counterpart cannot drift:

```bash
python3 tools/extract_doc_snippets.py            # regenerate
python3 tools/extract_doc_snippets.py --check    # report without writing
```

A fence that cannot compile opts out with `<!-- docs-compile: skip - reason
a reviewer can disagree with -->` on the line above it — CONTRIBUTING.md's
[*Definition of done*](../CONTRIBUTING.md#definition-of-done), item 5, has
the exact syntax. A marker with no fence after it is an error, not silence:
an opt-out that stopped applying must not go unnoticed.

Pages under `docs/reference/` carry three more markers, and land in the
`Lodestar.DocSnippets.Reference` namespace instead of `Lodestar.DocSnippets`:

- `<!-- docs-declaration -->` marks a signature shown above a fence — the
  declaration itself, excluded from compilation entirely.
- `<!-- docs-run: skip - reason -->` compiles the fence but never runs it,
  under the same "a reviewer can disagree with the reason" rule as
  `docs-compile: skip`.
- a trailing `// =>` comment on a local-variable declaration becomes an
  assertion on the value the declaration promises, so a reference example is
  not only compiled but *executed* — a promised result that stops being true
  fails CI instead of a reader.

## `build_wiki.py`

Turns a checkout into the tree the GitHub wiki publishes: `docs/` becomes a
flat page per package channel and, once a package is tagged, per released
version — flat because a wiki addresses a page by file name alone, with no
directory context of its own, and two pages sharing a base name would
otherwise collide silently.

```bash
python3 tools/build_wiki.py --repo <dir> --out <dir> \
  --released Lodestar.Text=0.3.0 [--released ...]     # every live channel
python3 tools/build_wiki.py --repo <dir> --out <dir> \
  --archive Lodestar.Text=0.4.0                       # one frozen version
```

`docs/wiki-map.json` is the only place that says which page belongs to which
package; a page it declares that the tree does not hold is an error, not
silence — that is how a renamed guide stops being published without anyone
noticing. Without `--archive` the run refreshes every live channel plus the
root pages (`Home`, `_Sidebar`, and the pages whose subject is the project
rather than a package); with it, the run freezes that one package's snapshot
first and then refreshes the live channels on top of it — only that package is
frozen, which is what lets a per-package release tag publish one version, but
the sidebar and the live pages' banner are read off the tree, so they are
rewritten too rather than left naming the previous release.
`.github/workflows/wiki.yml` is what calls it, on pushes to `main` and on
per-package release tags.

Each live channel also gets a generated entry page, `<channel>.md`: its guides
under *Start here*, then one row per namespace it covers, labelled with that
namespace page's H1 and described by its first sentence, so the hub cannot drift
from the pages it lists. A namespace declared as a directory must be documented
by a page whose H1 names it in backticks, or the run fails. A package that covers
no namespace and ships no guide gets no entry page, and `Home` names it "no
pages yet". The entry page is not archived; the module docstring says why.

Every package page opens on a breadcrumb — `Home › Text › Distances` on a
member page, the namespace page being the one beside the member's directory —
since a wiki has no directories to say where a page sits. An archived page
drops the hub from that line, because an archive has none. `Home` is
`docs/wiki-map.json`'s `home` document, whose links must all land on published
pages, followed by the generated package table. The sidebar lists each root page
once, and a globbed directory by its `README.md` alone.

## `check_no_console_writeline.py`

Refuses a `Console` call under `src/`, always, and one under `bench/` that does
not say why it is there.

```bash
python3 tools/check_no_console_writeline.py           # findings, exit 1 if any
python3 tools/check_no_console_writeline.py --report  # the marked ones, always exit 0
python3 tools/check_no_console_writeline.py --help
```

**`src/` has no marker and will not get one.** A library that writes to a console
its caller did not open is deciding for an application it cannot see. The packages
here have never done it; this is what keeps that true once nobody is watching.

**`bench/` needs a reason, not permission.** Ten calls there narrated a run —
banners, one line per measured row, `-> path` after a write — and accumulated
precisely because each was harmless on its own. None was in a timed region and
none moved a published number. Four remain, each carrying what no file does: that
the wrong build was measured, which build it was, why a cell is missing from a
table, and the group sizes a diagnostic class's own rows are read against. Each
says so on its own line:

```csharp
// console-print: the wrong build was measured, so every number below is a lie.
Console.Error.WriteLine(…);
```

The marker goes above the call or trails it, and an empty one is refused. Whether
the reason is good is a review's call, not this guard's — the same division
`check_comment_length.py` draws.

There is deliberately **no exemption list in the script**.
[`check_machine_paths.py`](#check_machine_pathspy) says why they rot: switched off
one file at a time, by someone who is not the reviewer. A marker rots in the diff
that adds it, in front of the person who can refuse it —
This file's `check_no_console_writeline.py` section records that choice and what the four
marked calls carry.

## `check_comment_length.py`

Refuses a comment block that runs past its budget without saying why —
**two lines** for an inline comment, **eight** for XML documentation, counted
over prose. `CONTRIBUTING.md`'s
[*Claims in comments*](../CONTRIBUTING.md#claims-in-comments) has why the two
budgets differ.

```bash
python3 tools/check_comment_length.py           # findings, exit 1 if any
python3 tools/check_comment_length.py --report  # counts only, always exit 0
python3 tools/check_comment_length.py --help
```

CI runs it in the `Lint` job and on Windows, so a finding fails the build. The `--report` line prints beside it, which is how the marker count stays visible without failing on its own growth.

Longer stays possible where it is necessary. A block past its budget carries
`long-comment:` and a reason as its first line; an empty marker is refused. The
guard sees only that a marker exists — whether the block deserved one is a
code review's call, per `CONTRIBUTING.md`'s *Claims in comments*.

A docstring is not a comment block. Python prose belongs in one, and the tools
in this directory open with thirty-line docstrings on purpose.

## `skip_build.py`

Answers whether a pull request changes nothing the build, the tests or the Windows checks can
judge ([#1073](https://github.com/CyrilB1531/lodestar/issues/1073)). Two classes qualify: Markdown
only, which is `docs_only.py`'s answer, and **workflow only** — every path a workflow-ish file
other than `.github/workflows/ci.yml`: `release.yml`, `release-nuget-org.yml`, `bench-nightly.yml`,
`bench-ondemand.yml`, `classify-pull-request.yml`, `wiki.yml` and `dependabot.yml`, enumerated in
`UNREAD` and asserted against the directory. Nothing in the pull-request pipeline reads them, so a
build and a platform check say nothing about a change to one. What that cost was measured on
[#1072](https://github.com/CyrilB1531/lodestar/pull/1072) — six minutes of build and test, and a
Windows job still running after the required checks had gone green — and a one-line change to
`bench-nightly.yml` paid exactly the same until this rule; it now costs about 100 s, all of it
`Lint`.

`ci.yml` is excluded by the rule rather than by a convention: that file *is* the gate, so running
the pipeline is the only way to know a change to it works. Every path must qualify, so a pull
request mixing `ci.yml` with anything else takes the full path too.

`--workflows-only` answers the narrower question — every path such a workflow and **no Markdown at
all** — which is what lets `Guide snippets compile, reference snippets run` be skipped as well. Any
Markdown runs it, because that is the job that compiles and runs the guides' fences.

```bash
# true for a pull request that changes only bench-nightly.yml; false for #1072, which changed
# ci.yml and is the one workflow the rule excludes.
gh api repos/CyrilB1531/lodestar/pulls/1076/files --paginate \
    --jq '.[] | .filename, (.previous_filename // empty)' | python3 tools/skip_build.py
```

## `docs_only.py`

Answers whether a pull request changes nothing but Markdown
([#857](https://github.com/CyrilB1531/lodestar/issues/857)). The `changes` job in `ci.yml` feeds it
the pull request's files from the API. Since [#1073](https://github.com/CyrilB1531/lodestar/issues/1073)
the jobs themselves key on [`skip_build.py`](#skip_buildpy), which is the broader question; this
one names the class `Build and analyze` prints when it accepts a skip, and separates a
Markdown-only pull request from one that also touches a workflow. `Lint`'s documentation tests
([#997](https://github.com/CyrilB1531/lodestar/issues/997)) run on either. Those tests stay because 18
`ReferenceDocumentationTests` classes, one per package, read `docs/**/*.md`: one caught a missing
reference link in
the pull request that became #859.

```bash
gh api repos/CyrilB1531/lodestar/pulls/857/files --paginate \
  --jq '.[] | .filename, (.previous_filename // empty)' | python3 tools/docs_only.py
```

It prints `true` only when every path ends in `.md`, a rename counting both its names. An image, a
JSON map the tests read, or an empty list gives `false`: a pull request whose files could not be
listed takes the full path, never the reduced one.

## `format_needed.py`

Answers whether a pull request can change what `dotnet format --verify-no-changes` checks, which
decides whether `Lint` spends about two and a half minutes on it
([#997](https://github.com/CyrilB1531/lodestar/issues/997)). The `changes` job feeds it the same file
list as `docs_only.py`.

```bash
gh api repos/CyrilB1531/lodestar/pulls/997/files --paginate \
  --jq '.[] | .filename, (.previous_filename // empty)' | python3 tools/format_needed.py
```

It prints `true` when any path ends in `.cs`, `.csproj`, `.props`, `.targets`, `.slnx`,
`.editorconfig` or `.globalconfig`, a rename counting both its names. An empty list also gives
`true`: a pull request whose files could not be listed runs the check.

## `stage_doc_inputs.py`

Copies the Markdown and `wiki-map.json` a test project copies to its output, from the checkout,
into an already built output ([#997](https://github.com/CyrilB1531/lodestar/issues/997)). On a
docs-only pull request, `Lint` runs the documentation tests on the net10.0 binaries main's CI run
published for the pull request's base commit, so nothing is compiled; this puts the pull request's
own docs beside those binaries, where MSBuild would have put them.

```bash
python3 tools/stage_doc_inputs.py tests/Lodestar.Stats.Tests/Lodestar.Stats.Tests.csproj doc-tests/Lodestar.Stats.Tests
```

It reads each `<None Include=...>` item reaching `docs/` whose `CopyToOutputDirectory` is
`Always` or `PreserveNewest` — the two values MSBuild's copy targets match, so `Never` and `false`
stage nothing — and honours `Link`, `LinkBase` and a stripped `Exclude`, rather than assuming a
layout: `Lodestar.Stats.Tests` links `docs/reference/stats.md` beside its own folder's pages
([#1062](https://github.com/CyrilB1531/lodestar/issues/1062)).

## `check_doc_test_counts.py`

Refuses a documentation-test run in which any suite reported **zero** tests
([#1054](https://github.com/CyrilB1531/lodestar/issues/1054)). The docs-only path invokes each
test assembly directly, and xunit v3's in-process runner exits 0 when its `-namespace` filter
matches nothing — `-namespace Nope.Documentation` and a one-letter typo both print `Total: 0` and
succeed. On such a pull request that loop is the only thing testing the docs, so a renamed
namespace or test project would have left it green and empty.

```bash
python3 tools/check_doc_test_counts.py "$RUNNER_TEMP/doc-tests"
```

The count comes from the `-xml` result each run writes, not from the console summary, which is
formatted for a reader; a missing result file fails too, since it means the run did not finish.

## `check_sample_culture.py`

Refuses a sample that can print a number in whoever ran it's culture. The sample
is the packaging gate: CI runs it on every pull request and a contributor reads
its output to see that a package works. String interpolation formats through
`CurrentCulture`, so the same commit printed `0.807` on CI and `0,807` on a
French console for two releases, and nothing failed — the gate checks that every
public type is reachable, not what the run said.

`CA1305` cannot catch it. The rule fires on an explicit `ToString(string)` and
never on an interpolated hole, at any `AnalysisMode`, so the gap is in the rule
rather than in the configuration and raising `AnalysisLevel` would not surface
one of them. `CLAUDE.md`'s analyzer section
recorded that and left it open; [#205](https://github.com/CyrilB1531/lodestar/issues/205)
closed it.

```bash
python3 tools/check_sample_culture.py
```

Two checks, because neither covers the other:

1. **No interpolated hole carries a standard format specifier.** `{value:F3}` and
   friends are the rewritable ones, and `Inv.F3(value)` —
   `samples/Lodestar.Sample/Inv.cs` — is what they become. An aligned hole
   (`{value,10:F3}`) is reported as such, because a rewrite has to keep the
   alignment rather than swallow it.

   A hole is found by its closing `:F3}` and then walked *backwards* to its
   opening brace, because the expression can hold braces of its own — an object
   initializer in an argument list does, and three such holes survived the first
   sweep of this issue precisely because a single regular expression cannot span
   them. The specifier must be a letter followed by digits: the sample embeds
   vocabularies as JSON, where `:10}` is data rather than a format. A *custom*
   format (`{value:0.###}`) is therefore not matched, and is left to check 2.
2. **`Program.cs` still pins the thread culture.** That covers what no syntactic
   scan can: a bare `{value}` hole whose expression is a `double` reads exactly
   like a bare `{count}` hole whose expression is an `int`. Matched on the
   assignment rather than the comment above it, so rewording the comment does not
   fail the build.

Sources come from `git ls-files`, never a glob: `bin/` and `obj/` hold copies of
every sample source, and editing those would turn the guard green while the files
that ship still printed in the contributor's culture.

Exit codes:

- `0` — clean.
- `1` — findings printed, each naming its file, line and hole.
- `2` — bad usage, or no tracked sample sources to scan.

## `check_machine_paths.py`

Refuses a tracked file that holds a path under someone's home directory. Ten of
them reached this public repository across six documents before anything looked
for them, and both sweeps that removed them started from a reader noticing a
line rather than from a check. They arrive by being pasted from a terminal,
which is exactly when nobody is thinking about what the string contains.

```bash
python3 tools/check_machine_paths.py                # named shapes, plus this machine's $HOME
python3 tools/check_machine_paths.py --no-environment  # named shapes only
python3 tools/check_machine_paths.py --help
```

Two probe sets. **Named shapes** run everywhere: a home directory under `/home`
or `/Users`, its Windows equivalent, the root user's own home, and the session
scratch-directory prefix. **Environment-derived probes** are computed at run
time from `$HOME` — the path itself, the account name bounded by a separator or
a dash, and the dashed form a session scratch directory is named after — which
catch shapes no fixed list enumerates, on the machine where a path is actually
created.

## `check_sdd_citations.py`

Refuses a file that cites a task's report or brief — a `task-` number followed by
`-report.md` or `-brief.md` — or any path into the subagent-driven-development
workspace under `.superpowers/`. That workspace is kept out of the repository by
`.git/info/exclude` and deleted when a plan finishes, so a citation of it is
dangling from the commit that writes it. Seventeen comments and one ADR carried
one before [#730](https://github.com/CyrilB1531/lodestar/issues/730).

```bash
python3 tools/check_sdd_citations.py
python3 tools/check_sdd_citations.py --help
```

The fix for a finding keeps the claim and points at something tracked instead —
the test that pins it, the oracle case, the commit whose message holds the
measurement, or the issue — per CONTRIBUTING.md's
[*Claims in comments*](../CONTRIBUTING.md#claims-in-comments). Exempt:
`docs/superpowers/`, where a spec may describe the workspace a plan ran in; the
vendored `.claude/skills/` that create it; decision 0002, which cites a report and
cannot be edited; and the guard and its test.

## `check_spec_status.py`

Refuses a spec that does not open its `**Status:**` with one of three clauses —
`written before the work`, `written with the work`, `**retrospective**` — and
refuses a plan that is committed at all. A spec records what was decided and what
was measured, so it is still a record when written late; a plan is an instrument
for work that has not started, and the branch it names does not outlive the pull
request that closes its issue. 116 plans, 88,745 lines, were tracked before
[#1104](https://github.com/CyrilB1531/lodestar/issues/1104), cited by no file
outside `docs/superpowers/`.

```bash
python3 tools/check_spec_status.py
python3 tools/check_spec_status.py --help
```

It reads the opening clause and nothing else: anything after it — a date, an
amendment, a note that the work was never done — is free text. That is the point
of the rule rather than an omission. `accepted, 2026-09-16. Written before the
work.` was the shape most of the tree carried, and it buries the one fact a reader
is after behind a lifecycle word every spec would carry alike. The name is checked
on the same pass, against CLAUDE.md's
[*Workflow*](../CLAUDE.md#workflow): `<date>_<issue padded to four>_<kebab
slug>.md`, with an optional letter for a second spec on one issue.

## `check_performance_sections.py`

Refuses a comparison in a package's `src/<Package>/performance.md` that is not one
(#1133 moved each package's comparisons there). Four rules per page: it opens
`# Performance — <Package>`, or says `Nothing is measured against an incumbent yet.`;
every `##` carries a machine and a window; every `##` names an incumbent from
`INCUMBENTS` or sits in `EXEMPT` with its reason; and **no table column is named after a
branch or a revision** — `before`, `after`, `main`, `fix`, `origin/main`,
`this branch`, `A1 / A2`. `docs/guides/performance.md` is the index: it keeps only
*How to read a row* and *Packages*, and links every package's page.

```bash
python3 tools/check_performance_sections.py
```

The fourth rule is the one the guard exists for. A before/after is an argument
about one commit and belongs in the pull request that made it
([#1106](https://github.com/CyrilB1531/lodestar/issues/1106) took the guide from 5,382
lines to 1,696), but it is not recognisable from its prose — every
optimisation writes "faster". It is recognisable from its columns, and the match
is a word inside the header cell rather than the whole cell, because the shapes
that got there spell it out: `main`, A1 / A2; Allocated before; Lodestar after.
Three sections are exempt, each because its comparison is real and has no
third-party side: `Lodestar.Gpu`'s kernels against this repository's own CPU paths,
the batched-embedding section, whose subject is that the ratio is an upper bound,
and the BK-tree against the length-filtered scan a caller would otherwise write.

## `check_adr_immutable.py`

Refuses a pull request that touches a `docs/decisions/` ADR already present at
its base commit — addition included, not just removal or rewording. "Amend
0004 in a decision of its own instead of editing it" is why: even the
`> **#NNN update:**` blockquote a few earlier ADRs still carry is no longer the
convention, and nothing enforced either version of the rule before this.

```bash
python3 tools/check_adr_immutable.py --base <commit>
python3 tools/check_adr_immutable.py --help
```

`--base` is the pull request's own base commit (`github.event.pull_request.base.sha`
in CI); there is no default; comparing against the wrong thing silently on a
rebased or force-pushed branch is worse than requiring the argument. A file
absent at `--base` is a new ADR and is unrestricted, and so is
`docs/decisions/README.md`, the index rather than a decision, which gains a row
on every one added.

One change to a record that already existed is allowed, and only one: a YAML
frontmatter block added above the title, with the body below it byte-identical.
That is tools/regen_adr_index.py's exception, and it is self-limiting — the rule tests that
there was no block before, so a record that carries one can never take the path
again, and `check_adr_frontmatter.py` refuses a new ADR without one. Appending a
line to an accepted body still fails, with a message saying the exception exists
and that this change is not it.

Exit codes:

- `0` — clean.
- `1` — findings printed, each naming the file and how many lines it added or
  removed relative to `--base`.
- `2` — bad usage.

An ordinary account name (`src`, `build`, `net` and the like) can turn a derived
probe into noise on an otherwise unrelated line. `--no-environment` drops that
set and leaves the named shapes enforcing, which have no such escape by design:
a path under a home directory is never wanted in a committed file. The report
names which probe matched each finding, and mentions `--no-environment` only
when a derived probe is the one that fired.

Exit codes:

- `0` — clean.
- `1` — findings printed (with a suggestion — `$SCRATCH`, `$(mktemp -d)`, or a
  description of what the path held).
- `2` — bad usage.

It exempts only its own source and its own test module, which have to contain
the patterns they search for to exist; nothing else is, because an exemption
list that grows is a guard being switched off one file at a time. See
[`../docs/superpowers/specs/2026-08-12_0133_machine-path-guard.md`](../docs/superpowers/specs/2026-08-12_0133_machine-path-guard.md)
for the measurement that shaped the two-probe-set design.

## The ADR index

`docs/decisions/index.yaml` is generated, never hand-edited. Each record declares
`supersedes`, `amends` and `applies` in its own YAML frontmatter;
`regen_adr_index.py` crosses those into `superseded_by`, `amended_by` and
`applied_by` — the direction an immutable record cannot carry, because naming the
decision that amended you is an edit. tools/regen_adr_index.py has the whole reasoning, and
`adr_index.py` is the reader and emitter the generator and both guards share.

```bash
python3 tools/regen_adr_index.py           # rewrite docs/decisions/index.yaml
python3 tools/regen_adr_index.py --check   # print whether it would change, exit 1 if so
python3 tools/check_adr_frontmatter.py
python3 tools/check_adr_index_sync.py
python3 tools/check_adr_index_is_cited.py
```

- `check_adr_frontmatter.py` refuses a record whose block is missing, unreadable,
  missing one of `status`, `supersedes`, `amends` or `applies`, carrying a key that
  is none of those, disagreeing with the first word of its own `**Status:**` line,
  or naming a decision that does not exist or is itself.
- `check_adr_index_sync.py` regenerates in memory and compares the **whole text**
  against what is committed, printing the first lines that differ. The emitter is
  deterministic, so any difference at all means the file was hand-edited or a new
  record was added without regenerating.
- `check_adr_index_is_cited.py` refuses `CLAUDE.md` or `CONTRIBUTING.md` that no
  longer has one paragraph naming `docs/decisions/index.yaml` together with both
  `amended_by` and `applied_by`. Not an exact sentence — rewording prose is normal,
  and a guard that forbids it gets deleted — but a reader told to follow one edge
  and not the other is told half of it.

Standard library only, like every guard here, because `.githooks/pre-commit` runs
them through whichever of `python3` or `python` a contributor's machine resolves
rather than through `.venv-oracles`. Nothing parses arbitrary YAML: the block is a
fixed four-key shape read by a strict reader, and the index is emitted and compared
as bytes. Every four-digit reference is quoted because PyYAML reads YAML 1.1, where
a bare `0010` is octal and resolves to `8`.

Exit codes, all three: `0` clean, `1` findings printed, `2` bad usage.

## Rules

- **Code-point semantics.** rapidfuzz/jellyfish iterate over code points; the C#
  suite therefore replays with `TextElement.CodePoint`. No lone surrogate is
  emitted (it would not survive the JSON round-trip).
- **Provenance.** We *run* these libraries to generate data — which creates no
  right over the outputs — but we do not **transcribe** any code. `python-
  Levenshtein` (GPL) is excluded even from generation, for hygiene. See
  [`../docs/decisions/0002-provenance-and-the-allowed-references.md`](../docs/decisions/0002-provenance-and-the-allowed-references.md).
