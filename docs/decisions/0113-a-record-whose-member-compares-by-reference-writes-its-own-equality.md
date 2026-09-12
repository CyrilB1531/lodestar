---
status: accepted
supersedes: []
amends: []
applies: []
---
# 0113 — A record whose member compares by reference writes its own equality

**Status:** accepted · **Date:** 2026-09-12

## Context

A record synthesises `Equals` that compares reference-typed members **by reference**. Two
instances holding the same numbers in different arrays compare unequal, and `GetHashCode` agrees
with that rather than with the caller:

```csharp
var a = new KMeansOptions { InitialCentres = [1.0, 2.0] };
var b = new KMeansOptions { InitialCentres = [1.0, 2.0] };
a == b;   // false
```

That is worse than no equality at all, because a record's shape advertises value semantics.

[#668](https://github.com/CyrilB1531/lodestar/issues/668) opened on this as an open question with
three options. The survey that answered it found the question already settled in the tree: of the
thirty public records in `src/`, twelve hold a member that compares by reference, and **six
already write their own equality** — `BpeVocabulary`, `SentencePieceVocabulary`,
`WordPieceVocabulary`, `SpecialTokenTemplate`, `TokenizationResult` and `CountVectorizerOptions`.
All six spell it the same way. `TokenizationResult` gives the reason in its own `<remarks>`.

The issue's grep missed them because it looked for `override bool Equals`. A record's equality
member is `public bool Equals(T? other)` — virtual, not `override`, the record having declared it
already.

## Decision

**A public record implements `public bool Equals(T? other)` and
`public override int GetHashCode()` when any of its members compares by reference.** Anything
else keeps the synthesised equality and owes nothing.

The trigger is the member's equality, not its declaration. A collection member — an array or any
`IReadOnly*<T>` — compares by reference and fires the rule. A member that is itself a record, a
struct or a `string` already compares by value, and the synthesised equality delegating to it is
correct. `SurvivalStep` needs no help; the `SurvivalStep[]` holding it does.

A third case fires the trigger and gets no arm below: a member that compares by reference and is
neither a collection nor value-equal — a mutable class of its own.
[#668](https://github.com/CyrilB1531/lodestar/issues/668) named it, as "a collection or a mutable
object". It is left unarmed deliberately, because what equality means for such a member is that
member's question rather than this rule's. No record in `src/` holds one today; a record that
grows one answers the question in the same commit.

Two arms, by what the member means:

| member | compared | contributes to the hash |
| --- | --- | --- |
| a sequence — `T[]`, `IReadOnlyList<T>` | element by element, in order | its count |
| a set — an `IReadOnlyCollection<T>` whose order is not meaning | `SetEquals` | **whether it is present, never its count** |

And three rules the existing six already fix:

- a `double` member compares with `double.Equals`, not `==`, so `NaN` equals `NaN` as a record's
  equality must. S1244 is suppressed at those lines with that reason;
- a `string` member compares `Ordinal`;
- the hash never walks a collection — scalars, counts and presence only, so it stays O(1).

### Why a set member hashes presence and not its count

Set equality makes `["the", "the"]` equal `["the"]` while their counts differ. Equal objects must
hash alike, so the count is unusable and only presence survives. Hashing the words themselves is
the O(n) the rule exists to avoid. Unequal instances are allowed to collide.

## Consequences

Six records change observably: `KMeansOptions`, `Chi2ContingencyResult`, `KaplanMeierCurve`,
`NelsonAalenCurve`, `RakeOptions` and `TextRankOptions`. `a == b` flips from `false` to `true`
for value-identical instances, which is why each owes a `CHANGELOG.md` entry.

One hazard is kept rather than solved: a hash taken over a member the caller then mutates will
not match afterwards. Hashing counts and presence narrows it — mutating contents leaves the
length alone — and none of the twelve is used as a dictionary key in the tree.

### Rejected

**Document it and leave the code alone.** Leaves a trap that reads like a feature, and makes six
shipped types the exception rather than the rule.

**Make the fitted results plain sealed classes.** A breaking change to three shipped types, and
the precedent does not split the way the argument assumes: `CountVectorizerOptions` is an options
bag and `TokenizationResult` is a result, and both chose equality.
