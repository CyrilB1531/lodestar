# 0668 — A record with a reference-typed member implements its own equality

**Status:** accepted, 2026-09-12. Written before the work.

**Issue:** [#668](https://github.com/CyrilB1531/lodestar/issues/668), surfaced while reviewing
[#666](https://github.com/CyrilB1531/lodestar/pull/666), where "could this options type be a
record?" was answered with a condition rather than a yes.

## Problem

A record synthesises `Equals` that compares reference-typed members **by reference**. Two
instances holding the same numbers in different arrays therefore compare unequal, and
`GetHashCode` agrees with that rather than with the caller's intent:

```csharp
var a = new KMeansOptions { InitialCentres = [1.0, 2.0] };
var b = new KMeansOptions { InitialCentres = [1.0, 2.0] };
a == b;   // false
```

This is worse than having no equality at all, because a record's shape advertises value
semantics. A caller who reaches for `==` gets an answer, and the answer is wrong.

## What the tree already does, which the issue did not know

The issue reports "None of these overrides `Equals` or `GetHashCode` — verified by grep, not
assumed." That is wrong on five of its eleven rows, and the correction is what shapes this
decision: **the repository already has a house pattern, applied to six public records.**

Surveyed 2026-09-12 over every `public sealed record` in `src/` — of the thirty-two, twelve hold at
least one member that compares by reference. `string` is excluded throughout, being a reference
type with value equality already:

| record | package | own equality today |
| --- | --- | --- |
| `BpeVocabulary` | `Lodestar.Embeddings` | yes |
| `SentencePieceVocabulary` | `Lodestar.Embeddings` | yes |
| `WordPieceVocabulary` | `Lodestar.Embeddings` | yes |
| `SpecialTokenTemplate` | `Lodestar.Embeddings` | yes |
| `TokenizationResult` | `Lodestar.Embeddings` | yes |
| `CountVectorizerOptions` | `Lodestar.Text` | yes — the issue does not list it |
| `KMeansOptions` | `Lodestar.Cluster` | no |
| `Chi2ContingencyResult` | `Lodestar.Stats` | no |
| `KaplanMeierCurve` | `Lodestar.Survival` | no |
| `NelsonAalenCurve` | `Lodestar.Survival` | no |
| `RakeOptions` | `Lodestar.Text` | no |
| `TextRankOptions` | `Lodestar.Text` | no |

All six existing ones are written the same way: `public bool Equals(T? other)` — the record's
own virtual `Equals(T)`, which is why it is not `override` — beside
`public override int GetHashCode()`. `TokenizationResult` states the reasoning in its
`<remarks>`, in the tree, since before this issue was opened:

> The generated equality would compare `Tokens` and `Ids` by reference, so two results holding
> the same tokens would be unequal — in the one place a caller has every reason to compare.

`OlsOptions` and `GlmOptions` are records as of #666 and are confirmed unaffected: every member
of both is a value type.

## Decision

A public record in `src/` implements `public bool Equals(T? other)` and
`public override int GetHashCode()` when any of its members **compares by reference**. Anything
else keeps the synthesised equality and owes nothing.

The trigger is the member's equality, not its declaration. A collection member — an array or any
`IReadOnly*<T>` — compares by reference and fires the rule. A member that is itself a record, or
a struct, or a `string`, already compares by value, and the synthesised equality delegating to it
is correct; such a member fires nothing. This is the distinction that keeps the rule from
over-reaching: `SurvivalStep` is a reference type and needs no help, while the `SurvivalStep[]`
holding it does.

Two arms, by what the member means rather than by what it is declared as:

| member | compared | contributes to the hash |
| --- | --- | --- |
| a sequence — `T[]`, `IReadOnlyList<T>` | element by element, in order | its count |
| a set — an `IReadOnlyCollection<T>` whose order is not meaning, as `StopWords` is | `HashSet.SetEquals` | **whether it is present, never its count** |

Three rules the same precedent already fixes, carried unchanged:

- a `double` member compares with `double.Equals`, not `==`, so that `NaN` equals `NaN` as a
  record's equality requires. SonarLint's S1244 is suppressed at those lines with that reason,
  which is the wording `CountVectorizerOptions` already carries;
- a `string` member compares `Ordinal`;
- the hash never walks a collection. Scalars, counts and presence only, so it stays O(1).

### Why a set member hashes presence and not count

This is the one rule that cannot be derived from "hash the lengths", and
`CountVectorizerOptions` found it first. Set equality makes `["the", "the"]` equal `["the"]`
while their counts differ. Equal objects are required to hash alike, so the count is unusable
and only presence survives. Hashing the words themselves would be the O(n) the rule exists to
avoid. Unequal options are allowed to collide.

## Options rejected

**Document it and leave the code alone.** One `<remarks>` per type saying the collection members
compare by reference. Cheapest, and honest about what the code does, but it leaves a trap that
reads like a feature — and it would make six shipped types the exception rather than the rule,
since they already compare structurally.

**Make the ones that never wanted equality plain sealed classes.** Attractive for a fitted curve,
which a caller has little reason to compare. Rejected on two grounds. It is a breaking change to
three shipped types, removing `with` and value semantics from a published surface. And the
precedent does not split the way the argument assumes: `CountVectorizerOptions` is an options bag
and `TokenizationResult` is a result, and both chose equality — the second because asserting an
encoding against one written out by hand is exactly what its callers do.

**Per-type hash strategies.** Rejected because the ADR would then set no rule a future record
could follow without re-deciding, which is most of what the ADR is for.

## The six types

| type | members needing care | arm |
| --- | --- | --- |
| `KMeansOptions` | `double[]? InitialCentres`, plus `Tolerance` | sequence, nullable |
| `Chi2ContingencyResult` | `double[][] ExpectedFrequencies` | sequence, jagged: outer count, then each row element by element |
| `KaplanMeierCurve` | `SurvivalStep[]`, three `double[]`, `ConfidenceLevel` | sequence ×4 |
| `NelsonAalenCurve` | `SurvivalStep[]`, `double[]` | sequence ×2 |
| `RakeOptions` | `IReadOnlyCollection<string>? StopWords` | set |
| `TextRankOptions` | `IReadOnlyCollection<string>? StopWords` | set |

`SurvivalStep` is a record of four value types, so its own equality is already correct and the
two curves compare their steps element by element for free.

The two options types copy `CountVectorizerOptions` rather than paraphrase it: the member is the
same type, used the same way, and a second spelling of one rule is how the two drift apart.

## Testing

The issue's acceptance criterion is a test proving two value-identical instances compare equal
**and** that their hash codes match. Each of the six gets, in its package's existing suite and so
mirrored onto `netstandard2.0` without further work:

- two instances built from separate arrays holding the same values: equal, and hash-equal;
- one differing in a single element: unequal;
- the null-member case, both sides null and one side null;
- for `RakeOptions` and `TextRankOptions`, `["the", "the"]` against `["the"]`: equal, and
  hash-equal — the case that proves the count is not in the hash;
- for `Chi2ContingencyResult`, two tables agreeing on the outer count and differing inside one
  row: unequal, which is what a jagged comparison that stops at the outer length would miss.

## Documentation owed, in the same commit as the code

- A reference page per new member. Methods have their own page in this layout
  (`kmeans-fit.md`), and `TokenizationResult` already pays it as `tokenizationresult-equals.md`
  and `tokenizationresult-gethashcode.md`, so this is twelve pages, plus a `## Members` table on
  the six type pages that do not have one yet. Their `// =>` fences execute in CI, so each page's
  promise is checked rather than trusted.
- One `CHANGELOG.md` entry per affected package under `[Unreleased]` — four, for `Lodestar.Cluster`,
  `Lodestar.Stats`, `Lodestar.Survival` and `Lodestar.Text`. Flipping `a == b` from `false` to
  `true` is observable to a caller, which is what item 7 of the definition of done asks about.
- ADR **0113**, the next free number, carrying this decision. `tools/regen_adr_index.py` regenerates
  `docs/decisions/index.yaml` in the same commit.

## Not in scope

- **No version bumps.** All four packages sit at their tags but for `Lodestar.Text`, which is
  already ahead of its own. `check_unreleased.py` treats unpublished work as the normal state
  between a merge and a release and fails only a version declared past its tag, so releasing is a
  separate act.
- **No `docs/equivalence.md` rows.** Equality here is a .NET concern with no Python call to map;
  scikit-learn's options objects are not compared for equality by their callers either.
- **No new samples.** The six types are public already and reachable from `samples/` as they
  stand; no new public *type* is introduced, only members on existing ones.

## Risk

Measured, not estimated: **no test and no sample in the tree compares any of the six**, so nothing
here depends on the reference semantics being replaced. The change is observable to a caller
outside the repository, which is why it is an ADR and a changelog entry rather than a quiet fix.

The one hazard the issue names and this decision keeps is real: a hash taken over a member the
caller then mutates will not match afterwards. Hashing counts and presence rather than contents
narrows it — mutating an array's contents leaves its length alone, so the hash survives the
common case and only a resize disturbs it. Records of this kind are configuration and results,
not dictionary keys, and none of the six is used as one in the tree.
