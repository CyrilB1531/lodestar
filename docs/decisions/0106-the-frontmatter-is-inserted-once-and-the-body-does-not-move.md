---
status: accepted
supersedes: []
amends: []
applies: []
---
# 0106 — The frontmatter is inserted once, and the body does not move

**Status:** accepted · **Date:** 2026-09-11

## Context

An ADR is immutable. [`README.md`](README.md) states it — "An ADR's body is never rewritten to
agree with a later one" — and `tools/check_adr_immutable.py` enforces it for every record that
existed before a pull request, addition included: [`0043`](0043-the-equality-table-is-sized-to-the-pattern.md)
pulled three `> **#NNN update:**` blockquotes back out of [`0004`](0004-levenshtein-myers-backlog.md)
and put what they said in a record of its own.

The cost of that rule is a direction. An amendment names what it amends; the decision being
amended cannot be edited to name the amendment, because naming it is an edit. Measured on the 105
records this repository held before this one:

- **17 records** declare a relationship in their header — 16 `**Amends:**`, one `**Supersedes:**`
  — for 18 edges onto **15 distinct decisions**.
- **Not one of those 15 names its own amendment.** The three records that do carry a forward
  pointer — [`0009`](0009-sample-consumes-a-local-feed.md),
  [`0013`](0013-sentencepiece-parity-scope.md), [`0015`](0015-sonar-rules-in-the-build.md) — got it
  by being edited, before anything refused that.
- [`0082`](0082-scipy-joins-the-allowed-permissive-references.md)'s Consequences section already
  wrote down what follows: "The index carries the back-reference instead … for an amended decision
  that cannot name its own amendment."

So the forward direction lives in one hand-written prose column of [`README.md`](README.md).
`tools/tests/test_adr_count_coherence.py` holds that table to one row per file and holds the prose
counts to the directory; it asserts nothing about what a row *says*, and nothing at all about
whether a relationship was recorded.

The failure is concrete. A reader who cites
[`0101`](0101-lodestar-gpu-is-the-one-package-that-does-not-ship-netstandard2-0.md) —
"`Lodestar.Gpu` is the one package that does not ship `netstandard2.0`", accepted, never edited,
correct as written — states the opposite of what this repository decided, because
[`0103`](0103-lodestar-gpu-ships-netstandard2-1-beside-net10.md) amended it to ship
`netstandard2.1` for every .NET 8, Mono and Unity consumer. 0101 cannot say so. Nothing mechanical
makes a reader look.

### Two relations, not one

Conflating *changed* with *used again* is its own error.
[`0097`](0097-the-chi-squared-tail-joins-the-published-four.md) says of
[`0095`](0095-the-stats-numerical-layer-publishes-four-members-and-no-more.md): "This record does
not amend 0095 so much as apply it." 0095 published four members and wrote that publishing later
is always available, so 0097 and [`0098`](0098-the-normal-quantile-is-the-third-member-decision-0095s-rule-publishes.md)
**exercise a sentence 0095 already wrote**. A reader told they amended it goes looking for a clause
that moved.
[`0082`](0082-scipy-joins-the-allowed-permissive-references.md) does the same to
[`0003`](0003-provenance-and-licensing.md) — scipy is one more instance of a class 0003 already
allows — and seven records run [`0074`](0074-the-phase-2-gaps-restated-on-what-the-packages-export.md)'s
`MetadataLoadContext` protocol without changing a word of it.

## Decision

**Every ADR carries a YAML frontmatter block, and `docs/decisions/index.yaml` is generated from
it.** Three relations, declared by the record that makes them:

| relation | what it says |
| --- | --- |
| `supersedes` | the named decision no longer holds, in whole or in the part named |
| `amends` | the named decision still holds, changed where this one says |
| `applies` | the named decision is unchanged and used again, on a rule it already stated |

`tools/regen_adr_index.py` reverses them into `superseded_by`, `amended_by` and `applied_by` —
the direction no record can carry — and `tools/check_adr_index_sync.py` refuses any drift between
the committed file and a fresh generation. `tools/check_adr_frontmatter.py` refuses a record whose
block is missing, unreadable, or names a decision that does not exist.

All four keys are required, empty lists included. An absent key and an empty list read the same to
a generator and not at all the same to a reader: one is a record that declared nothing, the other a
record that declared no relation. Requiring all four is what makes `[]` a statement.

**The 105 records that predate the index were given their block in one pass, and that is the one
exception to immutability this repository has.** `tools/check_adr_immutable.py` now allows exactly
this and nothing else: a frontmatter block inserted above the title, with the body below it
**byte-identical**. Changing a block that is already there is refused like any other edit.

The exception is self-limiting by construction. Once a record carries a block it can never take
this path again — the rule tests that there was no block before — and `check_adr_frontmatter.py`
refuses a new ADR without one, so no future record can arrive in the state the exception covers.

**What immutability is for survives intact**: the historical reasoning is not rewritten, and the
verification is mechanical rather than promised. The retrofit's 105 diffs are six added lines and
zero removed, each one, and every body hashes to what it hashed to before.

### `applies` is seeded, not complete

The retrofit records what a record's own text states. `amends` and `supersedes` come from the
`**Amends:**` / `**Supersedes:**` header — mechanical, with two exceptions where the *older* record
is the one that states the edge, and the retrofit reads it there:
[`0012`](0012-per-package-versioning.md) `amends` 0009, which says "Amended by 0012" in its own
body, and [`0014`](0014-precompiled-normalizer.md) `supersedes` 0013, whose `**Status:**` line says
"superseded in part by `0014`".

`applies` is seeded from records that say they run, require or follow another decision's rule or
protocol — fifteen records, each justified by a sentence in its own text. **It is not claimed
exhaustive**, and `index.yaml`'s own header says so: an empty list means nothing was declared,
never that nothing exists. [`README.md`](README.md)'s relationships column stays what it has
always been — a reading, in prose, of what a `**Status:**` line does not say. The index is the
mechanical layer beneath it, not a replacement for it.

Going forward the set is complete by construction: a new ADR declares its own relations, and the
guard refuses one that does not.

## Options that lost

- **The index alone carries the retro-catalogue, and no record is touched.** Tempting, because it
  needs no exception at all. Refused because it makes `index.yaml` the source of truth for half the
  corpus and a derived file for the other half — so regenerating it would *delete* the older half,
  and the generator would need a hand-maintained table of 105 entries living next to the guard that
  exists to stop hand-maintained tables drifting (#586, #597, #610).
- **A one-shot list of 105 paths in `check_adr_immutable.py`.** Works exactly once, leaves 105
  paths in a guard forever, and says nothing about why. The rule adopted instead says what is
  allowed rather than where, which is checkable, and it stops applying on its own.
- **Frontmatter in the `**Status:**` line, with no block at all.** The line already carries
  `**Amends:**` for 17 records, so the information is partly there. Refused: parsing prose is what
  produced the 43 candidate sentences this lot had to read by hand, and the line cannot hold the
  `applies` relation without becoming a paragraph.
- **PyYAML, for both the block and the index.** Refused for the reason every guard here is standard
  library only: `.githooks/pre-commit` runs them through whichever of `python3` or `python` a
  contributor's machine resolves, not through `.venv-oracles`, so a third-party import turns a
  missing package into a failed commit. The block is a fixed four-key shape read by a strict reader
  that refuses anything else, and the index is **emitted** and compared as bytes — nothing parses
  arbitrary YAML. Quoting is not cosmetic either: PyYAML reads YAML 1.1, where a bare `0010` is
  octal and resolves to `8`, so every reference is quoted for the benefit of any reader that does
  bring a YAML library.

## Consequences

- `tools/check_adr_immutable.py` grows one rule and keeps its reason in its own docstring. The
  negative case is asserted: appending a line to an accepted body still fails, with a message that
  names the exception and says these changes are not it.
- **`tools/build_wiki.py` strips the frontmatter.** `docs/wiki-map.json` publishes
  `docs/decisions/*.md` to the GitHub wiki, which renders a `---` block as a rule and a paragraph
  of keys. 105 wiki pages would have opened with their own metadata.
- `.githooks/pre-commit` runs three more guards, which makes sixteen; `check_adr_immutable.py`
  stays out for [`0046`](0046-check-adr-immutable-runs-in-ci-only.md)'s reason, unchanged by this.
- **The rule this narrows is not in an ADR.** Immutability is stated in
  [`README.md`](README.md) and enforced by a script; no decision record ever decided it, which is
  why this record amends nothing. That is worth writing down: the convention older than the guard
  was never itself a decision, so the first exception to it is also the first place it is stated
  with its boundary.
- A reader citing a decision now has one mechanical instruction, asserted by
  `tools/check_adr_index_is_cited.py` in both `CLAUDE.md` and `CONTRIBUTING.md`: read
  `docs/decisions/index.yaml` first and follow `amended_by` and `applied_by`.
