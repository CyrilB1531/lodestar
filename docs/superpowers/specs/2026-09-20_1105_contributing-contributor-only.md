# Keep in CONTRIBUTING only what a contributor does, on any machine

**Issue:** [#1105](https://github.com/CyrilB1531/lodestar/issues/1105), section 6 of
[#1071](https://github.com/CyrilB1531/lodestar/issues/1071).
**Status:** written with the work, 2026-09-20.
**Date:** 2026-09-20.

## The problem

`CONTRIBUTING.md` is 793 lines and 6 635 words across seventeen sections. It is the document a
first-time contributor is pointed at, and a measurable part of it describes something they will
never do, or something that happened on one desk.

Three defects, and they are not the same defect.

**Some of it is a maintainer's procedure.** Cutting a release, configuring the branch ruleset and
deciding that approvals stay off are the acts of the one person who merges. A contributor reads
them to find out that they do not apply.

**Some of it is one machine's measurement.** The local SonarQube section quotes an image size, a
pull time, a start-up time, three scanner timings, a line count and a duplication percentage, all
taken on the maintainer's desk with Podman. None of them is a fact a reader can act on; all of them
go stale the next time the repository grows.

**Some of it is a second copy.** The paragraph on the reduced path a documentation-only pull
request takes restates `tools/README.md`'s entries for `skip_build.py`, `docs_only.py`,
`format_needed.py` and `stage_doc_inputs.py`, at the length of all four put together, and adds the
`Guide snippets` job's own 81 s of 105 s — a number whose subject is the job, not the process.

A fourth thing was found on the way.
[#1103](https://github.com/CyrilB1531/lodestar/issues/1103) deleted 147 decision records and
rewrote 431 files' citations; in `CONTRIBUTING.md` it replaced 22 lines, and four of them lost
their link and kept their sentence:

| line | what it says now |
| --- | --- |
| 193 | `see the exception-tag gate.` — nothing to open |
| 323 | `each its own record rather than an edit to 0037.` — `0037` names a different record since the restart, and the two "later exclusions" both point at `tools/README.md` with no anchor |
| 653 | `` `CLAUDE.md`'s analyzer section `` — was `docs/decisions/0110` |
| 676–677 | `` `CLAUDE.md`'s analyzer section and `CLAUDE.md`'s analyzer section. `` — the same phrase twice, standing for two different deleted records |

## What was measured

Section lengths, from `awk` over the headings:

| section | lines | whose |
| --- | --- | --- |
| `Branching model — GitHub flow` | 31 | contributor |
| `` `gh` 2.73.0 or later `` | 21 | the floor is a contributor's; 14 lines derive it from `gh 2.46.0` on one desk |
| `Review, with a single maintainer` | 62 | 45 maintainer, 17 a second copy of `tools/README.md` |
| `Specs and plans` | 28 | contributor |
| `Definition of done` | 115 | contributor |
| `Before committing` | 65 | contributor; 6 lines are per-guard timings from one machine |
| `Before pushing` | 81 | optional, and every number in it is one desk's |
| `Working across two packages` | 43 | contributor |
| `Releasing` | 31 | 22 maintainer, 9 the changelog entry's shape |
| `Oracle validation` | 77 | contributor |
| `Dependencies` | 58 | contributor |
| `Analyzers` / `Where the rules run` / `Suppressions` | 119 | contributor |
| `Licensing and provenance` | 13 | contributor |
| `Performance claims` | 10 | contributor |
| `Claims in comments` | 37 | contributor |

Inbound links, from `git grep` for `CONTRIBUTING.md#` over `origin/main`: sixteen, over ten
distinct anchors — `tools/README.md` (eight), `CLAUDE.md` (six), `CHANGELOG.md` (one, at
`#releasing`) and one spec. Two of the anchors this change touches are cited — `#releasing` and
`#before-pushing-the-half-the-build-cannot-see` — so both need a destination, not a deletion.

## What was decided

Each displaced fact goes next to the thing it describes, rather than into one maintainer's
document. A document whose subject is *who reads it* is the grab-bag that
[`README.md`'s "Where a fact belongs"](../../../README.md#where-a-fact-belongs) table exists to
prevent; a document whose subject is *what it describes* is not.

| what moves | from | to |
| --- | --- | --- |
| the ruleset, its empty bypass list, why approvals are off, why the required check is this repository's own job | `Review, with a single maintainer` | **new** `.github/workflows/README.md` |
| the reduced path a classified pull request takes | `Review, with a single maintainer` | deleted; `tools/README.md` already carries it |
| the compose file, the `vm.max_map_count` trap, the scanner commands, every measured number, the four reasons it is not a rehearsal | `Before pushing` | **new** `tools/sonarqube-local/README.md` |
| setting `Version.props`, tagging, closing the release issue by bumping again | `Releasing` | `README.md`'s `## Publishing`, which already carries the tag shape |
| the four per-guard timings | `Before committing` | `tools/README.md`, beside the hook's own bullet |
| the `gh 2.46.0` derivation | `` `gh` 2.73.0 or later `` | deleted; the floor and the symptom stay |

`.github/workflows/README.md` gets a subject of its own rather than being a home for leftovers:
**what the pipeline is and what protects `main`** — a table of the seven workflows, which exists
nowhere today, and the protection rules that decide when a pull request may merge.

Three things stay in `CONTRIBUTING.md` that a first reading would have moved.

- **The three required checks, as a table.** A contributor whose pull request is red needs to know
  which check blocked it and what that check guards. Only the reasoning behind the ruleset leaves.
- **The changelog entry's shape.** Writing the entry is item 7 of the definition of done, which is
  a contributor's obligation; cutting the release is not. The shape folds into item 7 and
  `CHANGELOG.md`'s link follows it there.
- **The pre-commit hook's policy** — that it is skippable, that it is not a rehearsal of CI, that
  it reads the worktree rather than the commit, and that it runs in about a second. The claim is
  what a contributor acts on; only the four figures behind it are one desk's.

### Options that lost

**A single `MAINTAINING.md`.** One place to look, one new row in the table. Rejected: its subject
would be the reader rather than the content, so the next maintainer-shaped fact lands there by
default instead of beside what it describes, and the file becomes the thing `CONTRIBUTING.md` is
today.

**Leaving the ruleset in `CONTRIBUTING.md` and moving only the rest.** Rejected because the
reasoning is the bulk of it: 45 of its 62 lines argue why protection is built on checks, which is a
choice a contributor cannot make and does not need to weigh.

**Summarising the local SonarQube run rather than moving it.** Rejected: `tools/sonarqube-local/`
holds a `compose.yaml` and no README at all, so the instructions for running it have no other home,
and `tools/README.md` cites the anchor.

## What it costs

Two new files enter the markdownlint glob, which is declared in three places —
`.github/workflows/ci.yml`, `CLAUDE.md` and `CONTRIBUTING.md`'s definition of done — and all three
move together. `README.md`'s "Where a fact belongs" table gains a row per new document.

## Result

`CONTRIBUTING.md` goes from **793 lines and 6 635 words to 653 and 5 325** — 140 lines out, 21 of
them replaced by the changelog entry's shape and example arriving in item 7 and by the
`Version.props` rule staying behind. Section by section:

| section | before | after |
| --- | --- | --- |
| `` `gh` 2.73.0 or later `` | 21 | 14 |
| `Review, with a single maintainer` → `The three checks that guard main` | 62 | 26 |
| `Definition of done` | 115 | 123 |
| `Before committing` | 65 | 58 |
| `Before pushing` | 81 | 12 |
| `Working across two packages` | 43 | 47 |
| `Releasing` | 31 | 0 |
| `Where the rules run` | 75 | 73 |

Every section left describes something a contributor does. One measurement remains, in the `gh`
floor: it names two versions of the GitHub CLI, not a machine, and it is what makes the floor
checkable rather than asserted — the rule CLAUDE.md's *Claims in comments* states.

Both cited anchors still resolve. `#releasing` becomes `#definition-of-done`, because the shape
`CHANGELOG.md` wanted is what stayed behind — only the release procedure went to
`README.md#publishing`. `#before-pushing-the-half-the-build-cannot-see` keeps its heading, three
paragraphs shorter, pointing at `tools/sonarqube-local/README.md`. Every
`CONTRIBUTING.md#`-anchored link in the tree was checked against the headings that exist: sixteen
links, eight distinct anchors after the change, none dead.

Two `#1103` leftovers outside `CONTRIBUTING.md` were fixed on the way, because they are the same
defect one file over: `tests/analyzers.globalconfig` cited `docs/decisions/0112`, and
`tools/README.md`'s `sonarqube-local/` bullet pointed at the section that moved.

### Verified

markdownlint over the seven globs (1 052 files, 0 issues); the twenty-two offline check scripts,
`check_adr_immutable.py` and `check_repeated_literals.py` against `origin/main`, all green;
`tools/tests`' 653 tests green. `check_nuspec_dependencies.py` and `check_doc_test_counts.py` need
a packed `./artifacts` and a CI run's `results.xml`, so CI runs those.
