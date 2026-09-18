# 1081 — Six CI findings, a docs claim the docs-only path stopped honouring, two contradictory comments, a count of 21 that is 18, a stager that copies more than MSBuild, and an unwritten version invariant

**Status:** accepted, 2026-09-18. Written with the fix it records.

Issues: [#1081](https://github.com/CyrilB1531/lodestar/issues/1081) (six findings from the
`code-review` run on [#1070](https://github.com/CyrilB1531/lodestar/pull/1070) and
[#1072](https://github.com/CyrilB1531/lodestar/pull/1072)),
[#1059](https://github.com/CyrilB1531/lodestar/issues/1059),
[#1060](https://github.com/CyrilB1531/lodestar/issues/1060),
[#1061](https://github.com/CyrilB1531/lodestar/issues/1061),
[#1062](https://github.com/CyrilB1531/lodestar/issues/1062) and
[#1067](https://github.com/CyrilB1531/lodestar/issues/1067) from delta review #2
(`f7fff453..8047491b`).

One subject: the pull-request pipeline and the tools it calls. Six issues, one pull request,
because every one of them is a sentence or a guard inside `ci.yml`, `tools/` or the documents
that describe them — no library code, no package version, nothing a reviewer would want to
judge separately from its neighbours.

## Reproduced

Measured on `main` at `69dd9866`.

| claim | measured |
| --- | --- |
| `ci.yml:56`, `CONTRIBUTING.md:91`, `tools/README.md:797`: "21 `ReferenceDocumentationTests` classes" | `grep -rl "class ReferenceDocumentationTests" tests/*/Documentation/*.cs \| grep -vc NetStandard` → **18**, one per test project (36 with the mirrors) |
| `CLAUDE.md:34`, `:333-336` and `README.md:268`: `docs/reference/` is "replayed against **both target frameworks' assemblies**" | the docs-only path stages **18 net10.0 assemblies** only — `ci.yml:540-553` skips every `*NetStandard*` project when it builds the artifact, and `ci.yml:86-100` downloads that artifact |
| `tools/stage_doc_inputs.py` copies what MSBuild copies | on the synthetic project below, MSBuild copies **2 files** and `plan()` returns **4** |
| `ci.yml:551`: a real gate failure "still fails, three attempts over" | since [#1063](https://github.com/CyrilB1531/lodestar/issues/1063) it fails on the first attempt; the inline comment twelve lines below says so |
| `release.yml:175` and `bench-nightly.yml:403`: a rerun's second upload is one "the action refuses by default" | `ci.yml:560` records the measurement — the default **keeps both**, two `doc-tests-net10` artifacts on one run of `main` |
| `src/Lodestar.{Cluster,Decomposition,Preprocessing}/Version.props`: the number "never [moves] in the pull request that adds the work" | the commit that wrote that sentence moved **fifteen** versions in a CI pull request (#998) |

### The stager against MSBuild

One project holding all three shapes, built with the .NET 10 SDK and then planned by the script:

```xml
<None Include="../../docs/never.md" CopyToOutputDirectory="Never" LinkBase="docs" />
<None Include="../../docs/false.md" CopyToOutputDirectory="false" LinkBase="docs" />
<None Include="../../docs/**/*.md"
      Exclude="../../docs/never.md;
               ../../docs/private/**"
      CopyToOutputDirectory="PreserveNewest" LinkBase="docs" />
```

MSBuild copied `docs/false.md` and `docs/pub/ok.md`. `plan()` returned those two plus
`docs/never.md` and `docs/private/p.md`:

- `CopyToOutputDirectory` is tested for truthiness (`tools/stage_doc_inputs.py:92`), so `Never`
  and `false` stage the file. MSBuild's copy targets match `Always` and `PreserveNewest` and
  nothing else, which is why `docs/false.md` is in its output at all — the glob item claims it,
  not the `false` one.
- The two accepted values are matched case-insensitively: a second built project spelling them
  `preservenewest` and `ALWAYS` had both files in its output. Every one of the 168 `None` items
  under `tests/` spells it `PreserveNewest` exactly, so this changes nothing today; it is the
  direction the script should fail in, since reading `preservenewest` as "not copied" would leave
  a page judged against `main`'s text.
- `Exclude` is split on `;` with no `strip()` (`:65-68`). XML attribute-value normalisation turns
  the newline into a space, MSBuild trims each entry of a `;`-separated list, and this script does
  not: the second pattern resolves to `<base>/ ../../docs/private/**`, a path that does not exist,
  so it excludes nothing.

Neither shape occurs in the repository today, and the same review compared `plan()` against
MSBuild's own evaluation for all 18 test projects: 17 474 pairs, 0 differences. The failure
direction is a **superset**, so the first excluded page or `Never` item would silently start
being judged by the docs-only path.

### The retry loop has no committed harness

[#1072](https://github.com/CyrilB1531/lodestar/pull/1072) was proved with a fake scanner driven by
hand. The form it replaced —

```bash
if "$scanner" end /d:sonar.token="$SONAR_TOKEN" | tee "$log"; then
```

— is worth `tee`'s status, so a failed analysis (a failed quality gate included) reports as
successful on **every** run. It was caught by the review pass before the push, not by a check, and
nothing in the tree would catch it coming back.

Against that, `ci.yml:563` redirects instead of piping, which costs the live output: this step runs
up to twenty minutes, `cancel-in-progress` is on for pull requests, and a cancelled run now leaves
the log group empty of everything the scanner had printed.

### What the guard's docstring claims

`tools/check_doc_test_counts.py:7` says the check covers "a nested `Documentation.<Sub>`". The
condition is a per-project `count == 0`, so it covers a *whole* suite moving out of the filter's
reach; some classes moving into `Documentation.Pages` while others stay leaves the total non-zero
and passes. That the count is not a floor is deliberate and recorded — a documentation test
deleted on purpose must not need this file edited — so the docstring is the half that is wrong.

`tools/tests/test_check_doc_test_counts.py:67`'s
`test_the_totals_of_several_assemblies_in_one_file_are_summed` asserts only `main(...) == 0`, which
holds for any non-zero count: `totals` reading the first assembly, or `max` instead of `sum`,
leaves it green. It is the only test naming the summing, so the sum is pinned by nothing.

## Decided

| finding | chosen | rejected |
| --- | --- | --- |
| #1059, the netstandard claim | amend `CLAUDE.md` — both frameworks on the full path, net10.0 on the docs-only one — and `README.md`, which carries the same table row | staging the 18 mirrors into the artifact: it doubles a 14-day artefact to test `#if`-conditional public members, of which the tree holds one, identical on both targets |
| #1060, the two comments | make both say what was measured — the default keeps both uploads, so a rerun leaves an ambiguous duplicate | deleting the comments: the flag then reads as a precaution nobody can date |
| #1061, "21" | "18 … one per package", in the three places | "one per package" alone — the number is what a reader checks with one `grep`, and the count moving is how the sentence gets revisited |
| #1062, the stager | accept `Always` and `PreserveNewest` only, `strip()` each `Exclude` pattern, and pin both with the fixture that already holds the shapes | matching MSBuild's full item semantics (`Update`, `Remove`, multi-pattern `Include`): the script reads eighteen known projects, and a general MSBuild evaluator is the thing it exists not to be |
| #1067, the invariant | state it once in `CONTRIBUTING.md` *Releasing*, and make the three `Version.props` comments point at it instead of contradicting it | an ADR: the decision is 0012's, already recorded; what is missing is the process sentence, which belongs in `CONTRIBUTING.md` |
| #1081.1, .3, the messages | rewrite the header comment to match the step, and the refusal to name coverage and duplication, on a push as well as a pull request | — |
| #1081.2, live output | pipe through `tee` under `set -o pipefail`, so the status stays the scanner's | process substitution (`> >(tee "$log")`): the `grep` that follows races the writer |
| #1081.4, the harness | commit the fake scanner as a test that extracts the step's own script from `ci.yml` and runs it | a `grep` for a `tee` pipe in the workflow: it fails the one form it knows and passes every other way of losing the status, including the `pipefail`-less pipe this pull request introduces |
| #1081.5, the docstring | narrow it to what a per-project zero covers, and name the partial move as the limit | widening the guard to a floor per project: a deleted documentation test would then have to edit this file |
| #1081.6, the test | assert `GUARD.totals(path) == 5` on the two-assembly fixture | — |

## Out of scope

- The `-xml` and `-namespace` coexistence still rests on a local measurement rather than a CI run
  of `#1070`'s own pipeline (`docs_only` was false there). The probes since have exercised it, and
  this pull request touches `ci.yml`, so its own run does too.
- `tools/stage_doc_inputs.py` does not read a `;`-separated `Include`, an `Update` or a `Remove`
  item. No test project uses one; if one ever does, `plan()` returns nothing for it, which fails
  towards a subset and shows up as a documentation test reading `main`'s page.
