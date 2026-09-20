# One comparison per capability, against the incumbent

**Issue:** [#1106](https://github.com/CyrilB1531/lodestar/issues/1106), section 4 of
[#1071](https://github.com/CyrilB1531/lodestar/issues/1071).
**Status:** written with the work, 2026-09-20.
**Date:** 2026-09-20.

## The problem

`docs/guides/performance.md` is 5,382 lines over 84 `##` sections. `CLAUDE.md`'s *Where a fact
belongs* table gives it one subject — **what was measured, every number with its machine and its
window** — and most of the file is not that. It is the argument a pull request made for its own
commit, kept after the commit merged.

Three shapes, and they are not the same defect.

**A before/after of this repository's own code.** Forty of the eighty-four sections carry a `main`
column and a `fix` column, or a `before` and an `after`, and eleven more are nested inside a
section that does compare against something. They answer "did this commit help", which is a question
about a branch that no longer exists. Whole campaigns are here: the Levenshtein and Indel kernel
saga ([#208](https://github.com/CyrilB1531/lodestar/issues/208),
[#273](https://github.com/CyrilB1531/lodestar/issues/273),
[#301](https://github.com/CyrilB1531/lodestar/issues/301),
[#302](https://github.com/CyrilB1531/lodestar/issues/302),
[#320](https://github.com/CyrilB1531/lodestar/issues/320),
[#357](https://github.com/CyrilB1531/lodestar/issues/357),
[#383](https://github.com/CyrilB1531/lodestar/issues/383),
[#406](https://github.com/CyrilB1531/lodestar/issues/406),
[#407](https://github.com/CyrilB1531/lodestar/issues/407),
[#409](https://github.com/CyrilB1531/lodestar/issues/409),
[#411](https://github.com/CyrilB1531/lodestar/issues/411),
[#413](https://github.com/CyrilB1531/lodestar/issues/413),
[#674](https://github.com/CyrilB1531/lodestar/issues/674),
[#675](https://github.com/CyrilB1531/lodestar/issues/675),
[#717](https://github.com/CyrilB1531/lodestar/issues/717),
[#718](https://github.com/CyrilB1531/lodestar/issues/718)) runs to 1,100 lines; the persistence
investigation ([#323](https://github.com/CyrilB1531/lodestar/issues/323),
[#324](https://github.com/CyrilB1531/lodestar/issues/324),
[#378](https://github.com/CyrilB1531/lodestar/issues/378),
[#432](https://github.com/CyrilB1531/lodestar/issues/432),
[#436](https://github.com/CyrilB1531/lodestar/issues/436),
[#474](https://github.com/CyrilB1531/lodestar/issues/474)) to 950; the allocation wave of
[#811](https://github.com/CyrilB1531/lodestar/issues/811)–[#853](https://github.com/CyrilB1531/lodestar/issues/853)
to 250 across eighteen sections that are a table and one sentence each.

**A number with no counterparty.** `Levenshtein — indicative numbers`, `Vectorizers and fuzzy
matching` and `Clustering agreement from labels` publish absolute milliseconds on a dev machine
against nothing at all. They tell a reader that a call takes 8 ms, which answers no question a
reader arrived with.

**A comparison buried inside one of the other two.** The thing the guide is for — this library
against the library a reader would otherwise use — is there, but it is reached by scrolling past
the argument that surrounds it. `Compared to Python (rapidfuzz) — Indel` is 579 lines, of which
**39 are the comparison** and 540 are ten nested subsections about how the kernel got there.

## What was measured

Sections, by what their tables compare, counted over the 84 `##` headings:

| the section's subject | sections | lines |
| --- | ---: | ---: |
| this library against a named third-party library | 33 | 2,901 |
| a before/after of this repository's own code | 40 | 1,908 |
| an absolute number with no counterparty | 9 | 551 |
| preamble (`Reproduce`, `Principles applied`) | 2 | 18 |

Of the 2,901 lines in the first row, **1,141 are nested before/after subsections** — the comparison
itself is 1,760. So the before/after material is 3,049 lines, 57 % of the file.

Inbound links, from `git grep` over the tree: **51 links across 34 distinct anchors**, from
`bench/README.md` (26), `README.md` (12), `docs/migration/` (6), `docs/guides/` (5),
`docs/reference/` (1) and one spec. **Three are already dead** —
`#against-numpy-on-the-same-format-issue-474`, `#the-same-row-once-the-block-is-adopted-issue-466`
and `#where-the-ingests-time-actually-goes-issue-480`, all from sections earlier edits renamed.

## What was decided

The guide becomes **one comparison per capability**: a `##` per package in the order
[`README.md`'s comparison table](../../../README.md#measured-against-the-net-incumbents) uses, and
a `###` per capability inside it. Each capability section carries the incumbent with its version,
the machine, the window, one table, and a sentence saying how to read it. Nothing else.

**A before/after belongs to the pull request that argued for it**, and goes. Not to an appendix,
not to an index of issue numbers: the closed issue and the merged pull request hold it, and
`git log` holds the text. Keeping a pointer here would re-create the thing being removed, one
indirection further out.

**Two capability sections survive without a third-party incumbent**, because their comparison is
real and no library exists to stand on the other side:

- `Lodestar.Gpu`'s four kernels against this repository's own CPU paths — the comparison
  `bench/README.md`'s GPU gate is written in terms of, and the one `README.md` cites for the
  package.
- Batched embedding, whose subject is that the ratio is an **upper bound** and not a speed-up. It
  exists to stop a number being quoted, which is worth more than the number.

**Headings keep their text.** A surviving section is demoted from `##` to `###` under its package,
and its wording is left alone wherever it already names the incumbent, so its anchor survives and
its inbound links keep resolving. Only three survivors are renamed, each for a reason:

| section | why it is renamed |
| --- | --- |
| `BM25 against LuceneSharp (issue #677)` | absorbs `BM25's top ten without sorting the corpus (issue #751)`, whose run measured both sides after the change |
| `What the index rows against numpy are measuring, and the two paths that did move` | the two paths that moved are the before/after half |
| the five subsections of `The .NET incumbents, on a named machine (issue #679)` | they scatter into five packages, so each inherits the machine block the parent held |

**No number is re-derived.** Every surviving table is transcribed from the run that produced it,
with that run's machine and date, and no cell is computed here. Where a later before/after
re-measured both sides — BM25's query rows, [#751](https://github.com/CyrilB1531/lodestar/issues/751) —
the survivor takes that run's current-code column and drops the `origin/main` one, because both
columns come from the same interleaved window.

**The rule is made mechanical.** `tools/check_performance_sections.py` asserts, over
`docs/guides/performance.md`:

1. every `##` is a package id or one of two fixed headings;
2. every `###` carries a machine and a window;
3. every `###` names an incumbent, or sits on a two-entry exemption list carrying its reason;
4. **no table column is named** `before`, `after`, `main`, `fix`, `origin/main` or `this branch`.

Rule 4 is this issue, in a form a pull request cannot argue with. It is why the BM25 table is
rewritten rather than transcribed: its columns were `origin/main` and `this branch`.

### Options that lost

**An appendix of removed optimisations, one line per issue.** Cheap, and it keeps every inbound
link alive. Rejected: an index of before/afters is a before/after section with extra steps, and the
next contributor reads it as the place to add theirs.

**Keeping the sections and marking them historical.** Rejected for the same reason `docs/decisions/`
does not archive: a document whose sections are sorted by whether they are still true asks the
reader to do the sorting.

**Grouping by incumbent rather than by package.** A reader arrives holding `Fastenshtein`, not
holding `Lodestar.Fuzzy`. Rejected because `README.md`'s table is already the package-first index
and the deep links land inside it; two orders would drift apart.

## What it costs

Ten anchors that other documents cite are deleted with their sections, so ten citations move or
lose their link: `bench/README.md` (seven), `docs/guides/embeddings.md`,
`docs/guides/dictionary-lookup.md` and `docs/reference/text/indexing/bktree.md`. Where the citing
sentence makes a claim that only the deleted numbers supported, the sentence goes with the link
rather than surviving unsupported — `#1071`'s definition of done forbids a number that cannot be
traced, and it forbids the claim as much as the figure.

`tools/README.md` gains an entry, `.github/workflows/ci.yml`'s Lint job a line, and
`tools/tests/` a file.

## Result

`docs/guides/performance.md` goes from **5,382 lines over 84 `##` sections to 1,696 over 12 `##`
and 38 `###`** — one `##` per package, one `###` per capability. Every section is a comparison, and
the three that have no third-party side say so.

| package | comparisons | against |
| --- | ---: | --- |
| `Lodestar.Text` | 5 | rapidfuzz ×2, ML.NET `FeaturizeText`, LuceneSharp, a length-filtered scan |
| `Lodestar.Fuzzy` | 2 | Fastenshtein, Quickenshtein, F23.StringSimilarity; Raffinert.FuzzySharp |
| `Lodestar.Embeddings` | 6 | Microsoft.ML.Tokenizers ×2, `TensorPrimitives`, numpy ×2, an ONNX stub |
| `Lodestar.Metrics` | 2 | scikit-learn, ML.NET's binary evaluator |
| `Lodestar.Decomposition` | 2 | ML.NET `ProjectToPrincipalComponents`, NumFlat |
| `Lodestar.Cluster` | 3 | NumFlat and Meta.Numerics, NumFlat and `Dbscan`, `Aglomera` |
| `Lodestar.Preprocessing` | 3 | ML.NET ×3, scikit-learn |
| `Lodestar.Stats` | 2 | Accord.Statistics, Meta.Numerics |
| `Lodestar.Stats.Regression` | 9 | Math.NET ×2, Accord ×2, statsmodels ×5 |
| `Lodestar.Stats.TimeSeries` | 3 | Cortex.TimeSeries ×2, statsmodels |
| `Lodestar.Gpu` | 1 | this repository's own CPU paths |

Four sections were rebuilt rather than trimmed, because their tables were before/afters that
happened to carry the incumbent in a column:

- **Both rapidfuzz sections.** They published an Intel i7-4770S run whose columns were `before`,
  `+ trimming` and `+ kernel`, and whose prose pointed at ten subsections that no longer exist.
  They now carry the **most recent** runs that measured both sides —
  [#718](https://github.com/CyrilB1531/lodestar/issues/718) for `Levenshtein.Distance` and
  [#717](https://github.com/CyrilB1531/lodestar/issues/717) for `Indel`, both on the Ryzen 8700G
  against rapidfuzz 3.14.6 — which is a newer measurement as well as a cleaner one.
- **BM25.** Merged from two sections, the query rows taken from the run that measured Lucene beside
  the bounded heap.
- **SentencePiece, WordPiece and byte-level BPE.** Their incumbent columns sat beside `Lodestar
  before` and `Lodestar after`; the surviving tables are the current column against
  Microsoft.ML.Tokenizers.

**What the guard caught that a reading did not.** The first version of
`check_performance_sections.py` compared a header cell to a list of names and passed the tokenizer
tables, whose columns are `Lodestar before` and `Allocated after`. Matching the branch word *inside*
the cell found eight more columns across two sections, and the `A1 / A2` shape the regression
sections used would have passed the exact-match version entirely.

Inbound links: **51 across 34 anchors before, 41 across 25 after**, and **none dead** — three were
already dead on `main` and are fixed here. `README.md` keeps both its deep links, repointed;
`bench/README.md` loses nine pointer sentences whose targets were before/afters, and keeps every
figure it states itself, since its subject is how to measure rather than what came out.

Two sections argued for keeping against the first reading, and were kept:

- **Compressing an index (#378)**, because `docs/guides/embeddings.md` prices a decision on it and
  because the table is a cost-of-an-option beside a numpy comparison, not a before/after. It moved
  under `Lodestar.Embeddings` with its anchor intact.
- **BK-tree against a length-filtered scan (#526)**, the most-cited section in the file — four
  links, from two guides and a reference page — and structurally the `Lodestar.Gpu` case: no .NET
  package publishes a BK-tree, so the alternative is the loop a caller would otherwise write. It is
  the guard's third exemption.

### What the review found

`/code-review` over the branch returned six findings, all fixed in place rather than deferred:

| finding | fix |
| --- | --- |
| four shipped XML comments delegated their claim to guide sections this change deletes | the sweep went wider: **twenty** `docs/guides/performance.md` mentions across `src/` and `bench/` were #1103 leftovers pointing at no anchor. Each now carries its number or its issue, or drops the pointer |
| the guard `continue`d on every `##`, so a table placed before the first `###`, or in the preamble, was never read | `branch_breaches` runs over the preamble and over each package's own body too, with a test for each |
| `MACHINE` and `WINDOW` accepted "same machine and window as above" — the phrasing this change removed | both tightened to a named machine and a named window. Eleven sections said "same machine as above", and the reorder by package had already made **one of them point at the wrong machine**: `Meta.Numerics`' "above" landed on the Xeon container rather than the Ryzen |
| three benchmark comments named guide sections or a row (`block_copy_floor`) this change deletes | repointed at `bench/README.md`, which is where the harness's rows belong, or at their issue |
| `README.md`'s `Lodestar.Fuzzy` row claims both the Levenshtein and the `fuzz` ratios, and the repointed anchor reached only the first | points at `#lodestarfuzzy`, the package heading, which covers both |
| three line counts disagreed with `wc -l` | corrected to 5,382 and 1,696 |

CI found two more the local gate cannot see. The reference gate refuses a member named in a guide
without a link to its reference page, and two rewritten paragraphs named one in prose:
`Levenshtein.Distance` in a heading and `ChiSquare.Contingency` in the chi-square paragraph. Both
are linked, and `ReferenceDocumentationTests` reads 2 of 2 and 34 of 34. SonarCloud's gate then
refused the guard itself on two findings — `python:S8786`, a `\s*\|\s*` cell split that can
backtrack, and `python:S3776`, `check` at a cognitive complexity of 17 — so the split is a plain
`str.split` and the two rule groups are `package_breaches` and `capability_breaches`.

### Verified

`tools/check_performance_sections.py` green on the guide, and its fifteen tests among
**`tools/tests`' 669, all passing**. The twenty-one offline guards green, `check_adr_immutable.py`
and `check_repeated_literals.py` against `origin/main` green. markdownlint over the seven globs
(1,053 files, 0 issues). Every `performance.md#` anchor in the tree resolved against the guide's
own headings: 41 links, 25 anchors, none dead. `check_nuspec_dependencies.py` and
`check_doc_test_counts.py` need a packed `./artifacts` and a CI run's `results.xml`, so CI runs
those. No `csharp` fence is touched, so the doc-snippets gate sees no change, and `extract_doc_snippets.py` writes its 638 files unchanged. `dotnet build Lodestar.slnx -c Release` green on both target frameworks, and the two suites that read the docs —
`ReferenceDocumentationTests` in `Lodestar.Stats.Tests` (2) and `Lodestar.Text.Tests` (34) — green:
they are what caught two member names written in prose without a link to their reference page.
