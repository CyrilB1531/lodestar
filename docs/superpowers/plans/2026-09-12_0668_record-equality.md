# Record equality implementation plan

> **For agentic workers:** REQUIRED SUB-SKILL: Use superpowers:subagent-driven-development
> (recommended) or superpowers:executing-plans to implement this plan task-by-task. Steps use
> checkbox (`- [ ]`) syntax for tracking.

**Goal:** Six public records that compare their collection members by reference compare them by
value instead, under a rule an ADR states once.

**Architecture:** One internal helper in `src/Shared/` carries each comparison and each hash
contribution, so the twelve records that need them hold one spelling rather than twelve. Each
record then writes `public bool Equals(T? other)` — the record's own virtual member, not an
`override` — beside `public override int GetHashCode()`, delegating to that helper.

**Tech Stack:** C#, `net10.0` and `netstandard2.0`, xunit v3, SonarAnalyzer.CSharp at
`AnalysisMode=All`.

**Spec:** [`docs/superpowers/specs/2026-09-12_0668_record-equality.md`](../specs/2026-09-12_0668_record-equality.md)

**Branch:** `fix/668-record-equality`

## Global Constraints

- Both target frameworks, one public API. `netstandard2.0` reaches the same behaviour by
  conditional compilation, never a reduced surface.
- Warnings are errors. `dotnet build Lodestar.slnx -c Release` must stay clean.
- `dotnet test Lodestar.slnx -c Release` must report **32 assemblies** — sixteen suites and their
  sixteen mirrors. Read the count, not the colour.
- A comment block past two lines inline, or eight in XML documentation, carries `long-comment:`
  and a reason. `python3 tools/check_comment_length.py` enforces it.
- A suppression carries a reason a reviewer can disagree with. "Too noisy" is not one.
- Every new public method in a namespace `docs/wiki-map.json` marks `covered` owes a reference
  page. All four namespaces here are covered.
- A `// =>` comment in a reference page's example is an assertion CI executes. The value must be
  bound to a local first.
- No version bumps. No `docs/equivalence.md` rows. Both are out of scope, per the spec.

---

### Task 1: ADR 0112, the rule

**Files:**

- Create: `docs/decisions/0112-a-record-whose-member-compares-by-reference-writes-its-own-equality.md`
- Modify: `docs/decisions/index.yaml` (regenerated, never hand-edited)

**Interfaces:**

- Consumes: nothing.
- Produces: the decision number `0112`, cited by every `<remarks>` in Tasks 2–6.

- [ ] **Step 1: Write the ADR**

````markdown
---
status: accepted
supersedes: []
amends: []
applies: []
---
# 0112 — A record whose member compares by reference writes its own equality

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
````

- [ ] **Step 2: Regenerate the index and check the ADR guards**

```bash
python3 tools/regen_adr_index.py
python3 tools/check_adr_frontmatter.py
python3 tools/check_adr_index_sync.py
```

Expected: all three exit 0, and `git diff --stat docs/decisions/index.yaml` shows it changed.

- [ ] **Step 3: Commit**

```bash
git add docs/decisions/0112-a-record-whose-member-compares-by-reference-writes-its-own-equality.md docs/decisions/index.yaml
git commit -m "State when a record writes its own equality, and why six already did"
```

---

### Task 2: The shared comparisons, and `KMeansOptions` as their first consumer

`src/Shared/` is compiled into every library under `Lodestar.Internal` with a global using, so the
helper is internal: no public surface, no reference page, no sample. Only `Lodestar.Embeddings`
and `Lodestar.Extensions.MathNet` grant `InternalsVisibleTo`, so it is tested through the public
records that use it — which is what the acceptance criteria ask for anyway.

**Files:**

- Create: `src/Shared/ValueEquality.cs`
- Modify: `src/Directory.Build.props` (add the `<Compile Include>` line)
- Modify: `src/Lodestar.Cluster/KMeansOptions.cs`
- Create: `tests/Lodestar.Cluster.Tests/KMeansOptionsEqualityTests.cs`
- Create: `docs/reference/cluster/partitioning/kmeansoptions-equals.md`
- Create: `docs/reference/cluster/partitioning/kmeansoptions-gethashcode.md`
- Modify: `docs/reference/cluster/partitioning/kmeansoptions.md` (add a `## Members` table)
- Modify: `CHANGELOG.md`

**Interfaces:**

- Consumes: decision `0112` from Task 1.
- Verified before relying on it: only `Lodestar.Abstractions` sets
  `LodestarIncludesSharedHelpers` to `false`, so all four packages here receive the helper and
  reach it unqualified through `src/Shared/GlobalUsings.cs`.
- Produces: `Lodestar.Internal.ValueEquality`, with exactly these members, used by Tasks 3–5:
  - `static bool Same(double[]? left, double[]? right)`
  - `static bool Same(double[][]? left, double[][]? right)`
  - `static bool Same<T>(T[]? left, T[]? right)` where `T : IEquatable<T>`
  - `static bool SameSet(IReadOnlyCollection<string>? left, IReadOnlyCollection<string>? right)`
  - `static int CountOf(System.Collections.ICollection? collection)`
  - `static int PresenceOf(object? value)`

- [ ] **Step 1: Write the failing tests**

Create `tests/Lodestar.Cluster.Tests/KMeansOptionsEqualityTests.cs`:

```csharp
using Xunit;

namespace Lodestar.Cluster.Tests;

/// <summary>Decision 0112: the centres compare by value, and the hash agrees.</summary>
public sealed class KMeansOptionsEqualityTests
{
    [Fact]
    public void Separate_arrays_holding_the_same_centres_are_equal()
    {
        KMeansOptions left = new() { InitialCentres = [1.0, 2.0], Seed = 3 };
        KMeansOptions right = new() { InitialCentres = [1.0, 2.0], Seed = 3 };

        Assert.Equal(left, right);
        Assert.True(left == right);
        Assert.Equal(left.GetHashCode(), right.GetHashCode());
    }

    [Fact]
    public void One_differing_centre_is_unequal()
    {
        KMeansOptions left = new() { InitialCentres = [1.0, 2.0] };
        KMeansOptions right = new() { InitialCentres = [1.0, 2.5] };

        Assert.NotEqual(left, right);
    }

    [Fact]
    public void A_differing_length_is_unequal()
    {
        KMeansOptions left = new() { InitialCentres = [1.0, 2.0] };
        KMeansOptions right = new() { InitialCentres = [1.0] };

        Assert.NotEqual(left, right);
    }

    [Fact]
    public void Absent_centres_equal_absent_centres_and_nothing_else()
    {
        KMeansOptions absent = new() { Seed = 7 };
        KMeansOptions alsoAbsent = new() { Seed = 7 };
        KMeansOptions present = new() { Seed = 7, InitialCentres = [] };

        Assert.Equal(absent, alsoAbsent);
        Assert.Equal(absent.GetHashCode(), alsoAbsent.GetHashCode());
        Assert.NotEqual(absent, present);
    }

    [Fact]
    public void A_differing_scalar_is_unequal()
    {
        KMeansOptions left = new() { InitialCentres = [1.0], Tolerance = 1e-4 };
        KMeansOptions right = new() { InitialCentres = [1.0], Tolerance = 1e-5 };

        Assert.NotEqual(left, right);
    }

    [Fact]
    public void A_NaN_tolerance_equals_itself_so_equality_stays_reflexive()
    {
        KMeansOptions left = new() { Tolerance = double.NaN };
        KMeansOptions right = new() { Tolerance = double.NaN };

        Assert.Equal(left, right);
        Assert.Equal(left.GetHashCode(), right.GetHashCode());
    }

    [Fact]
    public void A_NaN_centre_equals_itself_too()
    {
        KMeansOptions left = new() { InitialCentres = [double.NaN, 1.0] };
        KMeansOptions right = new() { InitialCentres = [double.NaN, 1.0] };

        Assert.Equal(left, right);
    }
}
```

- [ ] **Step 2: Run the tests to verify they fail**

```bash
dotnet test tests/Lodestar.Cluster.Tests -c Release --filter "FullyQualifiedName~KMeansOptionsEquality"
```

Expected: FAIL. `Separate_arrays_holding_the_same_centres_are_equal` and both NaN tests fail —
the synthesised equality compares `InitialCentres` by reference and `Tolerance` with `==`.

- [ ] **Step 3: Write the shared helper**

Create `src/Shared/ValueEquality.cs`:

```csharp
using System;
using System.Collections;
using System.Collections.Generic;

namespace Lodestar.Internal;

/// <summary>
/// The comparisons a record needs when one of its members compares by reference, and the
/// O(1) hash contributions that stay consistent with them (decision 0112).
/// </summary>
/// <remarks>
/// Twelve records need these, and six wrote them independently before the rule was stated;
/// every method is total on null, since a record's equality must answer for an absent member.
/// </remarks>
internal static class ValueEquality
{
    /// <summary>Whether two blocks hold the same values, with <c>NaN</c> equal to <c>NaN</c>.</summary>
    /// <remarks>
    /// <c>SequenceEqual</c> compares with <c>==</c>, under which <c>NaN</c> equals nothing — and
    /// an equality that is not reflexive is not one a record may have. S1244 fires on the element
    /// comparison and is wrong here for the usual reason: this is value equality between two
    /// stored numbers, where "the same" means the same bits.
    /// </remarks>
    public static bool Same(double[]? left, double[]? right)
    {
        if (ReferenceEquals(left, right))
        {
            return true;
        }
        if (left is null || right is null || left.Length != right.Length)
        {
            return false;
        }
        for (int i = 0; i < left.Length; i++)
        {
#pragma warning disable S1244
            if (!left[i].Equals(right[i]))
#pragma warning restore S1244
            {
                return false;
            }
        }
        return true;
    }

    /// <summary>Whether two jagged tables hold the same values, row by row.</summary>
    /// <remarks>
    /// Comparing the outer length alone would pass two tables that agree on their row count and
    /// differ inside a row, which is the shape a contingency table takes.
    /// </remarks>
    public static bool Same(double[][]? left, double[][]? right)
    {
        if (ReferenceEquals(left, right))
        {
            return true;
        }
        if (left is null || right is null || left.Length != right.Length)
        {
            return false;
        }
        for (int i = 0; i < left.Length; i++)
        {
            if (!Same(left[i], right[i]))
            {
                return false;
            }
        }
        return true;
    }

    /// <summary>Whether two arrays hold equal elements, in order.</summary>
    /// <remarks>
    /// The constraint is what keeps this off the <c>double[]</c> overload above, which has its
    /// own reason to exist, and off any element type whose own equality is by reference.
    /// </remarks>
    public static bool Same<T>(T[]? left, T[]? right)
        where T : IEquatable<T>
    {
        if (ReferenceEquals(left, right))
        {
            return true;
        }
        if (left is null || right is null || left.Length != right.Length)
        {
            return false;
        }
        for (int i = 0; i < left.Length; i++)
        {
            if (!EqualityComparer<T>.Default.Equals(left[i], right[i]))
            {
                return false;
            }
        }
        return true;
    }

    /// <summary>Whether two collections hold the same words, order and repetition aside.</summary>
    public static bool SameSet(IReadOnlyCollection<string>? left, IReadOnlyCollection<string>? right)
    {
        if (ReferenceEquals(left, right))
        {
            return true;
        }
        if (left is null || right is null)
        {
            return false;
        }
        HashSet<string> mine = new(left, StringComparer.Ordinal);
        return mine.SetEquals(right);
    }

    /// <summary>A sequence's length, or <c>-1</c> for an absent one, as a hash contribution.</summary>
    /// <remarks>
    /// <c>-1</c> rather than <c>0</c> so an absent member and an empty one do not collide, which
    /// they must not: <c>Same</c> reports them unequal.
    /// </remarks>
    public static int CountOf(ICollection? collection) => collection?.Count ?? -1;

    /// <summary>Whether a member is present, as the only hash contribution a set may make.</summary>
    /// <remarks>
    /// A set member's count is unusable: <c>SameSet</c> makes <c>["the", "the"]</c> equal
    /// <c>["the"]</c> while the counts differ, and equal objects must hash alike.
    /// </remarks>
    public static int PresenceOf(object? value) => value is null ? -1 : 0;
}
```

- [ ] **Step 4: Compile the helper into every library**

In `src/Directory.Build.props`, inside the `ItemGroup` guarded by
`'$(LodestarIncludesSharedHelpers)' != 'false'`, add the line after the `StringCompat.cs` one:

```xml
    <Compile Include="$(MSBuildThisFileDirectory)Shared/ValueEquality.cs" Link="Internal/ValueEquality.cs" />
```

- [ ] **Step 5: Give `KMeansOptions` its equality**

In `src/Lodestar.Cluster/KMeansOptions.cs`, after the `InitialCentres` property and its
`#pragma warning restore CA1819`, before the closing brace of the record:

```csharp

    /// <summary>Compares every option, the centres element by element.</summary>
    /// <param name="other">The options to compare against.</param>
    /// <remarks>
    /// The generated equality would compare <see cref="InitialCentres"/> by reference, so two
    /// option sets built from separate arrays holding the same centres would be unequal.
    /// Decision 0112 has the rule and the six records that reached it first.
    /// </remarks>
    public bool Equals(KMeansOptions? other)
    {
        if (ReferenceEquals(this, other))
        {
            return true;
        }
        if (other is null
            || MaxIterations != other.MaxIterations
            || Seed != other.Seed
            // S1244 warns against exact floating-point comparison, which is right for
            // arithmetic and wrong here: this is value equality between two configurations,
            // where "the same tolerance" means the same bits. double.Equals also makes NaN
            // equal to NaN, which a record's equality needs and == gets wrong.
#pragma warning disable S1244
            || !Tolerance.Equals(other.Tolerance))
#pragma warning restore S1244
        {
            return false;
        }
        return ValueEquality.Same(InitialCentres, other.InitialCentres);
    }

    /// <summary>Hashes the scalars and the centre count, which is O(1).</summary>
    /// <remarks>
    /// Equal options necessarily agree on the count; unequal ones are allowed to collide.
    /// Hashing the centres themselves would make the cheap operation the expensive one.
    /// </remarks>
    public override int GetHashCode()
    {
        unchecked
        {
            int hash = (17 * 31) + MaxIterations;
            hash = (hash * 31) + Seed;
            hash = (hash * 31) + Tolerance.GetHashCode();
            return (hash * 31) + ValueEquality.CountOf(InitialCentres);
        }
    }
```

- [ ] **Step 6: Run the tests to verify they pass**

```bash
dotnet test tests/Lodestar.Cluster.Tests -c Release --filter "FullyQualifiedName~KMeansOptionsEquality"
```

Expected: PASS, 7 tests.

- [ ] **Step 7: Write the two reference pages**

Create `docs/reference/cluster/partitioning/kmeansoptions-equals.md`:

````markdown
# KMeansOptions.Equals

Compares every option, the centres element by element.

<!-- docs-declaration -->

```csharp
public bool Equals(KMeansOptions? other)
```

**Parameters** — `other` is the options to compare against, or `null`.

**Returns** — `true` when every option matches and both centre blocks hold the same values.

**Example** — two option sets built from separate arrays.

```csharp
using Lodestar.Cluster;

KMeansOptions left = new() { InitialCentres = [1.0, 2.0], Seed = 3 };
KMeansOptions right = new() { InitialCentres = [1.0, 2.0], Seed = 3 };

bool same = left == right;  // => True
```

**Remarks** — a record's generated equality compares `InitialCentres` by reference, so the two
above would be unequal without this, in the one place a caller has reason to compare: asserting
that a configuration built twice is the same configuration. `Tolerance` compares by bits, which
makes `NaN` equal `NaN` and keeps equality reflexive. [Decision
0112](https://github.com/CyrilB1531/lodestar/blob/main/docs/decisions/0112-a-record-whose-member-compares-by-reference-writes-its-own-equality.md)
has the rule.

**Applies to** — net10.0, netstandard2.0.

**See also** — [`KMeansOptions.GetHashCode`](kmeansoptions-gethashcode.md),
[`KMeansOptions`](kmeansoptions.md).
````

Create `docs/reference/cluster/partitioning/kmeansoptions-gethashcode.md`:

````markdown
# KMeansOptions.GetHashCode

A hash consistent with the equality, in constant time.

<!-- docs-declaration -->

```csharp
public override int GetHashCode()
```

**Returns** — a hash over the scalars and the number of centres.

**Example** — equal options hash alike.

```csharp
using Lodestar.Cluster;

KMeansOptions left = new() { InitialCentres = [1.0, 2.0], Seed = 3 };
KMeansOptions right = new() { InitialCentres = [1.0, 2.0], Seed = 3 };

bool alike = left.GetHashCode() == right.GetHashCode();  // => True
```

**Remarks** — the centres contribute their count, never their values: equal options necessarily
agree on the count, and hashing the block itself would make the cheap operation the expensive one
on a large start. Unequal options are allowed to collide. An absent block contributes `-1` rather
than `0`, so it does not collide with an empty one — which it must not, since the two are
unequal.

**Applies to** — net10.0, netstandard2.0.

**See also** — [`KMeansOptions.Equals`](kmeansoptions-equals.md),
[`KMeansOptions`](kmeansoptions.md).
````

- [ ] **Step 8: Link them from the type page**

At the end of `docs/reference/cluster/partitioning/kmeansoptions.md`, add:

```markdown

## Members

| member | what it does |
| --- | --- |
| [`KMeansOptions.Equals`](kmeansoptions-equals.md) | Value equality, the centres element by element. |
| [`KMeansOptions.GetHashCode`](kmeansoptions-gethashcode.md) | A hash consistent with it. |
```

- [ ] **Step 9: Add the changelog entry**

In `CHANGELOG.md`, under `## [Unreleased]`, add a `### Lodestar.Cluster` section with a
`#### Changed` heading (keeping the existing package sections in place):

```markdown
### Lodestar.Cluster

#### Changed

- **`KMeansOptions` compares its centres by value.** Two option sets built from separate arrays
  holding the same centres were unequal and now are equal, with `GetHashCode` agreeing; decision
  0112 has the rule for every record whose member compares by reference.
  ([#668](https://github.com/CyrilB1531/lodestar/issues/668))
```

- [ ] **Step 10: Run the gates and commit**

```bash
dotnet build Lodestar.slnx -c Release
python3 tools/check_comment_length.py
npx markdownlint-cli2 "docs/**/*.md"
git add src/Shared/ValueEquality.cs src/Directory.Build.props src/Lodestar.Cluster/KMeansOptions.cs tests/Lodestar.Cluster.Tests/KMeansOptionsEqualityTests.cs docs/reference/cluster CHANGELOG.md
git commit -m "Compare KMeansOptions by value, on comparisons the other five will share"
```

Expected: build clean, both guards exit 0.

---

### Task 3: `Chi2ContingencyResult`

**Files:**

- Modify: `src/Lodestar.Stats/TestResult.cs`
- Create: `tests/Lodestar.Stats.Tests/Chi2ContingencyResultEqualityTests.cs`
- Create: `docs/reference/stats/tests/chi2contingencyresult-equals.md`
- Create: `docs/reference/stats/tests/chi2contingencyresult-gethashcode.md`
- Modify: `docs/reference/stats/tests/chi2contingencyresult.md`
- Modify: `CHANGELOG.md`

**Interfaces:**

- Consumes: `ValueEquality.Same(double[][]?, double[][]?)` and `ValueEquality.CountOf` from Task 2.
- Produces: nothing later tasks rely on.

- [ ] **Step 1: Write the failing tests**

Create `tests/Lodestar.Stats.Tests/Chi2ContingencyResultEqualityTests.cs`:

```csharp
using Xunit;

namespace Lodestar.Stats.Tests;

/// <summary>Decision 0112: the expected table compares by value, row by row.</summary>
public sealed class Chi2ContingencyResultEqualityTests
{
    private static Chi2ContingencyResult Result(double[][] expected) =>
        new(Statistic: 1.5, PValue: 0.2, Dof: 1, ExpectedFrequencies: expected);

    [Fact]
    public void Separate_tables_holding_the_same_frequencies_are_equal()
    {
        Chi2ContingencyResult left = Result([[1.0, 2.0], [3.0, 4.0]]);
        Chi2ContingencyResult right = Result([[1.0, 2.0], [3.0, 4.0]]);

        Assert.Equal(left, right);
        Assert.Equal(left.GetHashCode(), right.GetHashCode());
    }

    [Fact]
    public void A_table_differing_inside_a_row_is_unequal()
    {
        Chi2ContingencyResult left = Result([[1.0, 2.0], [3.0, 4.0]]);
        Chi2ContingencyResult right = Result([[1.0, 2.0], [3.0, 4.5]]);

        Assert.NotEqual(left, right);
    }

    [Fact]
    public void A_table_with_a_differing_row_count_is_unequal()
    {
        Chi2ContingencyResult left = Result([[1.0, 2.0], [3.0, 4.0]]);
        Chi2ContingencyResult right = Result([[1.0, 2.0]]);

        Assert.NotEqual(left, right);
    }

    [Fact]
    public void A_row_of_a_differing_width_is_unequal()
    {
        Chi2ContingencyResult left = Result([[1.0, 2.0]]);
        Chi2ContingencyResult right = Result([[1.0, 2.0, 3.0]]);

        Assert.NotEqual(left, right);
    }

    [Fact]
    public void A_differing_statistic_is_unequal()
    {
        Chi2ContingencyResult left = Result([[1.0]]);
        Chi2ContingencyResult right = new(2.5, 0.2, 1, [[1.0]]);

        Assert.NotEqual(left, right);
    }
}
```

- [ ] **Step 2: Run the tests to verify they fail**

```bash
dotnet test tests/Lodestar.Stats.Tests -c Release --filter "FullyQualifiedName~Chi2ContingencyResultEquality"
```

Expected: FAIL on `Separate_tables_holding_the_same_frequencies_are_equal` — the table compares
by reference.

- [ ] **Step 3: Write the implementation**

In `src/Lodestar.Stats/TestResult.cs`, change the `Chi2ContingencyResult` declaration from its
one-line body to a braced one. Replace:

```csharp
public sealed record Chi2ContingencyResult(
    double Statistic, double PValue, int Dof, double[][] ExpectedFrequencies);
```

with:

```csharp
public sealed record Chi2ContingencyResult(
    double Statistic, double PValue, int Dof, double[][] ExpectedFrequencies)
{
    /// <summary>Compares the scalars and the expected table, row by row.</summary>
    /// <param name="other">The result to compare against.</param>
    /// <remarks>
    /// The generated equality would compare <see cref="ExpectedFrequencies"/> by reference, so
    /// two results holding the same table would be unequal. Decision 0112 has the rule.
    /// </remarks>
    public bool Equals(Chi2ContingencyResult? other)
    {
        if (ReferenceEquals(this, other))
        {
            return true;
        }
        if (other is null || Dof != other.Dof)
        {
            return false;
        }
        // S1244: value equality between two stored results, where "the same statistic" means
        // the same bits. double.Equals also makes NaN equal NaN, which equality must.
#pragma warning disable S1244
        if (!Statistic.Equals(other.Statistic) || !PValue.Equals(other.PValue))
#pragma warning restore S1244
        {
            return false;
        }
        return ValueEquality.Same(ExpectedFrequencies, other.ExpectedFrequencies);
    }

    /// <summary>Hashes the scalars and the row count, which is O(1).</summary>
    /// <remarks>
    /// Equal results necessarily agree on the row count; unequal ones may collide. Walking the
    /// table would make the cheap operation cost what the test itself cost.
    /// </remarks>
    public override int GetHashCode()
    {
        unchecked
        {
            int hash = (17 * 31) + Statistic.GetHashCode();
            hash = (hash * 31) + PValue.GetHashCode();
            hash = (hash * 31) + Dof;
            return (hash * 31) + ValueEquality.CountOf(ExpectedFrequencies);
        }
    }
}
```

Keep the `#pragma warning disable CA1819, S2368` and its `restore` exactly where they are, around
the declaration.

- [ ] **Step 4: Run the tests to verify they pass**

```bash
dotnet test tests/Lodestar.Stats.Tests -c Release --filter "FullyQualifiedName~Chi2ContingencyResultEquality"
```

Expected: PASS, 5 tests.

- [ ] **Step 5: Write the two reference pages**

Create `docs/reference/stats/tests/chi2contingencyresult-equals.md`:

````markdown
# Chi2ContingencyResult.Equals

Compares the scalars and the expected table, row by row.

<!-- docs-declaration -->

```csharp
public bool Equals(Chi2ContingencyResult? other)
```

**Parameters** — `other` is the result to compare against, or `null`.

**Returns** — `true` when the statistic, p-value and degrees of freedom match and both expected
tables hold the same frequencies.

**Example** — the same result, reached twice.

```csharp
using Lodestar.Stats;

Chi2ContingencyResult left = new(1.5, 0.2, 1, [[1.0, 2.0], [3.0, 4.0]]);
Chi2ContingencyResult right = new(1.5, 0.2, 1, [[1.0, 2.0], [3.0, 4.0]]);

bool same = left == right;  // => True
```

**Remarks** — the comparison descends into each row rather than stopping at the row count, which
is what a table differing inside one row needs. [Decision
0112](https://github.com/CyrilB1531/lodestar/blob/main/docs/decisions/0112-a-record-whose-member-compares-by-reference-writes-its-own-equality.md)
has the rule.

**Applies to** — net10.0, netstandard2.0.

**See also** — [`Chi2ContingencyResult.GetHashCode`](chi2contingencyresult-gethashcode.md),
[`Chi2ContingencyResult`](chi2contingencyresult.md).
````

Create `docs/reference/stats/tests/chi2contingencyresult-gethashcode.md`:

````markdown
# Chi2ContingencyResult.GetHashCode

A hash consistent with the equality, in constant time.

<!-- docs-declaration -->

```csharp
public override int GetHashCode()
```

**Returns** — a hash over the scalars and the number of rows.

**Example** — equal results hash alike.

```csharp
using Lodestar.Stats;

Chi2ContingencyResult left = new(1.5, 0.2, 1, [[1.0, 2.0], [3.0, 4.0]]);
Chi2ContingencyResult right = new(1.5, 0.2, 1, [[1.0, 2.0], [3.0, 4.0]]);

bool alike = left.GetHashCode() == right.GetHashCode();  // => True
```

**Remarks** — the table contributes its row count, never its frequencies. Equal results agree on
that count, and walking the table would make hashing cost what the test itself cost.

**Applies to** — net10.0, netstandard2.0.

**See also** — [`Chi2ContingencyResult.Equals`](chi2contingencyresult-equals.md),
[`Chi2ContingencyResult`](chi2contingencyresult.md).
````

- [ ] **Step 6: Link them from the type page**

At the end of `docs/reference/stats/tests/chi2contingencyresult.md`, add:

```markdown

## Members

| member | what it does |
| --- | --- |
| [`Chi2ContingencyResult.Equals`](chi2contingencyresult-equals.md) | Value equality, the table row by row. |
| [`Chi2ContingencyResult.GetHashCode`](chi2contingencyresult-gethashcode.md) | A hash consistent with it. |
```

- [ ] **Step 7: Add the changelog entry and commit**

In `CHANGELOG.md`, under `## [Unreleased]`, add to the `### Lodestar.Stats` section (creating it
with a `#### Changed` heading if absent):

```markdown
- **`Chi2ContingencyResult` compares its expected table by value.** Two results holding the same
  table were unequal and now are equal, with `GetHashCode` agreeing; decision 0112 has the rule.
  ([#668](https://github.com/CyrilB1531/lodestar/issues/668))
```

```bash
dotnet build Lodestar.slnx -c Release
python3 tools/check_comment_length.py
git add src/Lodestar.Stats/TestResult.cs tests/Lodestar.Stats.Tests/Chi2ContingencyResultEqualityTests.cs docs/reference/stats CHANGELOG.md
git commit -m "Compare a chi-squared result's expected table by value, row by row"
```

---

### Task 4: `KaplanMeierCurve` and `NelsonAalenCurve`

Both live in one file and share one shape, so they share a task: a reviewer rejecting one would
reject the other.

**Files:**

- Modify: `src/Lodestar.Survival/SurvivalResults.cs`
- Create: `tests/Lodestar.Survival.Tests/SurvivalCurveEqualityTests.cs`
- Create: `docs/reference/survival/estimators/kaplanmeiercurve-equals.md`
- Create: `docs/reference/survival/estimators/kaplanmeiercurve-gethashcode.md`
- Create: `docs/reference/survival/estimators/nelsonaalencurve-equals.md`
- Create: `docs/reference/survival/estimators/nelsonaalencurve-gethashcode.md`
- Modify: `docs/reference/survival/estimators/kaplanmeiercurve.md`
- Modify: `docs/reference/survival/estimators/nelsonaalencurve.md`
- Modify: `CHANGELOG.md`

**Interfaces:**

- Consumes: `ValueEquality.Same(double[]?, double[]?)`, `ValueEquality.Same<T>(T[]?, T[]?)` and
  `ValueEquality.CountOf` from Task 2. `SurvivalStep` is a record of four value types, so it
  satisfies `where T : IEquatable<T>` through the record's generated `IEquatable<SurvivalStep>`.
- Produces: nothing later tasks rely on.

- [ ] **Step 1: Write the failing tests**

Create `tests/Lodestar.Survival.Tests/SurvivalCurveEqualityTests.cs`:

```csharp
using Xunit;

namespace Lodestar.Survival.Tests;

/// <summary>Decision 0112: the curves compare their arrays by value.</summary>
public sealed class SurvivalCurveEqualityTests
{
    private static SurvivalStep[] Steps() =>
        [new(0.0, 4, 0, 0), new(1.0, 4, 1, 0)];

    private static KaplanMeierCurve Kaplan(double[] survival) =>
        new(Steps(), survival, [0.4, 0.3], [0.9, 0.8], 0.95);

    [Fact]
    public void Two_kaplan_meier_curves_holding_the_same_values_are_equal()
    {
        KaplanMeierCurve left = Kaplan([1.0, 0.75]);
        KaplanMeierCurve right = Kaplan([1.0, 0.75]);

        Assert.Equal(left, right);
        Assert.Equal(left.GetHashCode(), right.GetHashCode());
    }

    [Fact]
    public void A_kaplan_meier_curve_differing_in_one_estimate_is_unequal()
    {
        Assert.NotEqual(Kaplan([1.0, 0.75]), Kaplan([1.0, 0.5]));
    }

    [Fact]
    public void A_kaplan_meier_curve_differing_in_a_step_is_unequal()
    {
        KaplanMeierCurve left = Kaplan([1.0, 0.75]);
        KaplanMeierCurve right = new(
            [new(0.0, 4, 0, 0), new(2.0, 4, 1, 0)], [1.0, 0.75], [0.4, 0.3], [0.9, 0.8], 0.95);

        Assert.NotEqual(left, right);
    }

    [Fact]
    public void A_kaplan_meier_curve_differing_in_its_level_is_unequal()
    {
        KaplanMeierCurve left = Kaplan([1.0, 0.75]);
        KaplanMeierCurve right = new(Steps(), [1.0, 0.75], [0.4, 0.3], [0.9, 0.8], 0.90);

        Assert.NotEqual(left, right);
    }

    [Fact]
    public void Two_nelson_aalen_curves_holding_the_same_values_are_equal()
    {
        NelsonAalenCurve left = new(Steps(), [0.0, 0.25]);
        NelsonAalenCurve right = new(Steps(), [0.0, 0.25]);

        Assert.Equal(left, right);
        Assert.Equal(left.GetHashCode(), right.GetHashCode());
    }

    [Fact]
    public void A_nelson_aalen_curve_differing_in_one_hazard_is_unequal()
    {
        Assert.NotEqual(
            new NelsonAalenCurve(Steps(), [0.0, 0.25]),
            new NelsonAalenCurve(Steps(), [0.0, 0.50]));
    }

    [Fact]
    public void A_nelson_aalen_curve_of_a_differing_length_is_unequal()
    {
        Assert.NotEqual(
            new NelsonAalenCurve(Steps(), [0.0, 0.25]),
            new NelsonAalenCurve([new(0.0, 4, 0, 0)], [0.0]));
    }
}
```

- [ ] **Step 2: Run the tests to verify they fail**

```bash
dotnet test tests/Lodestar.Survival.Tests -c Release --filter "FullyQualifiedName~SurvivalCurveEquality"
```

Expected: FAIL on both `..._holding_the_same_values_are_equal` tests.

- [ ] **Step 3: Write the implementation**

In `src/Lodestar.Survival/SurvivalResults.cs`, replace the `KaplanMeierCurve` declaration's
semicolon body:

```csharp
public sealed record KaplanMeierCurve(
    SurvivalStep[] Steps,
    double[] Survival,
    double[] Lower,
    double[] Upper,
    double ConfidenceLevel);
```

with:

```csharp
public sealed record KaplanMeierCurve(
    SurvivalStep[] Steps,
    double[] Survival,
    double[] Lower,
    double[] Upper,
    double ConfidenceLevel)
{
    /// <summary>Compares the steps and all three curves, element by element.</summary>
    /// <param name="other">The curve to compare against.</param>
    /// <remarks>
    /// The generated equality would compare the four arrays by reference, so two curves fitted
    /// from the same data would be unequal. Decision 0112 has the rule.
    /// </remarks>
    public bool Equals(KaplanMeierCurve? other)
    {
        if (ReferenceEquals(this, other))
        {
            return true;
        }
        // S1244: the level is a stored configuration, compared by bits so NaN stays reflexive.
#pragma warning disable S1244
        if (other is null || !ConfidenceLevel.Equals(other.ConfidenceLevel))
#pragma warning restore S1244
        {
            return false;
        }
        return ValueEquality.Same(Steps, other.Steps)
            && ValueEquality.Same(Survival, other.Survival)
            && ValueEquality.Same(Lower, other.Lower)
            && ValueEquality.Same(Upper, other.Upper);
    }

    /// <summary>Hashes the level and the step count, which is O(1).</summary>
    /// <remarks>
    /// The four arrays share one index, so the step count stands for all of them. A curve can
    /// hold thousands of steps, which is the walk this avoids.
    /// </remarks>
    public override int GetHashCode()
    {
        unchecked
        {
            int hash = (17 * 31) + ConfidenceLevel.GetHashCode();
            return (hash * 31) + ValueEquality.CountOf(Steps);
        }
    }
}
```

And replace:

```csharp
public sealed record NelsonAalenCurve(SurvivalStep[] Steps, double[] CumulativeHazard);
```

with:

```csharp
public sealed record NelsonAalenCurve(SurvivalStep[] Steps, double[] CumulativeHazard)
{
    /// <summary>Compares the steps and the hazard, element by element.</summary>
    /// <param name="other">The curve to compare against.</param>
    /// <remarks>
    /// The generated equality would compare both arrays by reference, so two curves fitted from
    /// the same data would be unequal. Decision 0112 has the rule.
    /// </remarks>
    public bool Equals(NelsonAalenCurve? other)
    {
        if (ReferenceEquals(this, other))
        {
            return true;
        }
        return other is not null
            && ValueEquality.Same(Steps, other.Steps)
            && ValueEquality.Same(CumulativeHazard, other.CumulativeHazard);
    }

    /// <summary>Hashes the step count, which is O(1).</summary>
    /// <remarks>
    /// Both arrays share one index, so the step count stands for both. Equal curves agree on it;
    /// unequal ones are allowed to collide.
    /// </remarks>
    public override int GetHashCode() => ValueEquality.CountOf(Steps);
}
```

Keep the `#pragma warning disable CA1819` above `KaplanMeierCurve` and the matching `restore`
below `NelsonAalenCurve` exactly where they are.

- [ ] **Step 4: Run the tests to verify they pass**

```bash
dotnet test tests/Lodestar.Survival.Tests -c Release --filter "FullyQualifiedName~SurvivalCurveEquality"
```

Expected: PASS, 7 tests.

- [ ] **Step 5: Write the four reference pages**

Create `docs/reference/survival/estimators/kaplanmeiercurve-equals.md`:

````markdown
# KaplanMeierCurve.Equals

Compares the steps and all three curves, element by element.

<!-- docs-declaration -->

```csharp
public bool Equals(KaplanMeierCurve? other)
```

**Parameters** — `other` is the curve to compare against, or `null`.

**Returns** — `true` when the confidence level matches and the steps, estimates and both bounds
hold the same values.

**Example** — the same curve, built twice.

```csharp
using Lodestar.Survival;

SurvivalStep[] steps = [new(0.0, 4, 0, 0), new(1.0, 4, 1, 0)];
KaplanMeierCurve left = new(steps, [1.0, 0.75], [0.4, 0.3], [0.9, 0.8], 0.95);
KaplanMeierCurve right = new(
    [new(0.0, 4, 0, 0), new(1.0, 4, 1, 0)], [1.0, 0.75], [0.4, 0.3], [0.9, 0.8], 0.95);

bool same = left == right;  // => True
```

**Remarks** — `SurvivalStep` is a record of value types, so its own equality is already correct;
what needed writing is the comparison of the arrays holding it. [Decision
0112](https://github.com/CyrilB1531/lodestar/blob/main/docs/decisions/0112-a-record-whose-member-compares-by-reference-writes-its-own-equality.md)
has the rule.

**Applies to** — net10.0, netstandard2.0.

**See also** — [`KaplanMeierCurve.GetHashCode`](kaplanmeiercurve-gethashcode.md),
[`KaplanMeierCurve`](kaplanmeiercurve.md).
````

Create `docs/reference/survival/estimators/kaplanmeiercurve-gethashcode.md`:

````markdown
# KaplanMeierCurve.GetHashCode

A hash consistent with the equality, in constant time.

<!-- docs-declaration -->

```csharp
public override int GetHashCode()
```

**Returns** — a hash over the confidence level and the number of steps.

**Example** — equal curves hash alike.

```csharp
using Lodestar.Survival;

SurvivalStep[] steps = [new(0.0, 4, 0, 0), new(1.0, 4, 1, 0)];
KaplanMeierCurve left = new(steps, [1.0, 0.75], [0.4, 0.3], [0.9, 0.8], 0.95);
KaplanMeierCurve right = new(
    [new(0.0, 4, 0, 0), new(1.0, 4, 1, 0)], [1.0, 0.75], [0.4, 0.3], [0.9, 0.8], 0.95);

bool alike = left.GetHashCode() == right.GetHashCode();  // => True
```

**Remarks** — the four arrays share one index, so the step count stands for all of them. A fitted
curve can hold thousands of steps, which is the walk this avoids; unequal curves may collide.

**Applies to** — net10.0, netstandard2.0.

**See also** — [`KaplanMeierCurve.Equals`](kaplanmeiercurve-equals.md),
[`KaplanMeierCurve`](kaplanmeiercurve.md).
````

Create `docs/reference/survival/estimators/nelsonaalencurve-equals.md`:

````markdown
# NelsonAalenCurve.Equals

Compares the steps and the cumulative hazard, element by element.

<!-- docs-declaration -->

```csharp
public bool Equals(NelsonAalenCurve? other)
```

**Parameters** — `other` is the curve to compare against, or `null`.

**Returns** — `true` when the steps and the hazard hold the same values.

**Example** — the same curve, built twice.

```csharp
using Lodestar.Survival;

NelsonAalenCurve left = new([new(0.0, 4, 0, 0), new(1.0, 4, 1, 0)], [0.0, 0.25]);
NelsonAalenCurve right = new([new(0.0, 4, 0, 0), new(1.0, 4, 1, 0)], [0.0, 0.25]);

bool same = left == right;  // => True
```

**Remarks** — a record's generated equality compares both arrays by reference, so the two above
would be unequal without this. [Decision
0112](https://github.com/CyrilB1531/lodestar/blob/main/docs/decisions/0112-a-record-whose-member-compares-by-reference-writes-its-own-equality.md)
has the rule.

**Applies to** — net10.0, netstandard2.0.

**See also** — [`NelsonAalenCurve.GetHashCode`](nelsonaalencurve-gethashcode.md),
[`NelsonAalenCurve`](nelsonaalencurve.md).
````

Create `docs/reference/survival/estimators/nelsonaalencurve-gethashcode.md`:

````markdown
# NelsonAalenCurve.GetHashCode

A hash consistent with the equality, in constant time.

<!-- docs-declaration -->

```csharp
public override int GetHashCode()
```

**Returns** — a hash over the number of steps.

**Example** — equal curves hash alike.

```csharp
using Lodestar.Survival;

NelsonAalenCurve left = new([new(0.0, 4, 0, 0), new(1.0, 4, 1, 0)], [0.0, 0.25]);
NelsonAalenCurve right = new([new(0.0, 4, 0, 0), new(1.0, 4, 1, 0)], [0.0, 0.25]);

bool alike = left.GetHashCode() == right.GetHashCode();  // => True
```

**Remarks** — both arrays share one index, so the step count stands for both. Equal curves agree
on it; unequal ones are allowed to collide rather than pay for a walk.

**Applies to** — net10.0, netstandard2.0.

**See also** — [`NelsonAalenCurve.Equals`](nelsonaalencurve-equals.md),
[`NelsonAalenCurve`](nelsonaalencurve.md).
````

- [ ] **Step 6: Link them from the two type pages**

At the end of `docs/reference/survival/estimators/kaplanmeiercurve.md`:

```markdown

## Members

| member | what it does |
| --- | --- |
| [`KaplanMeierCurve.Equals`](kaplanmeiercurve-equals.md) | Value equality over the steps and the three curves. |
| [`KaplanMeierCurve.GetHashCode`](kaplanmeiercurve-gethashcode.md) | A hash consistent with it. |
```

At the end of `docs/reference/survival/estimators/nelsonaalencurve.md`:

```markdown

## Members

| member | what it does |
| --- | --- |
| [`NelsonAalenCurve.Equals`](nelsonaalencurve-equals.md) | Value equality over the steps and the hazard. |
| [`NelsonAalenCurve.GetHashCode`](nelsonaalencurve-gethashcode.md) | A hash consistent with it. |
```

- [ ] **Step 7: Add the changelog entry and commit**

In `CHANGELOG.md`, under `## [Unreleased]`, in a `### Lodestar.Survival` section with a
`#### Changed` heading:

```markdown
- **`KaplanMeierCurve` and `NelsonAalenCurve` compare their arrays by value.** Two curves fitted
  from the same data were unequal and now are equal, with `GetHashCode` agreeing; decision 0112
  has the rule. ([#668](https://github.com/CyrilB1531/lodestar/issues/668))
```

```bash
dotnet build Lodestar.slnx -c Release
python3 tools/check_comment_length.py
git add src/Lodestar.Survival/SurvivalResults.cs tests/Lodestar.Survival.Tests/SurvivalCurveEqualityTests.cs docs/reference/survival CHANGELOG.md
git commit -m "Compare the two survival curves by value, over the index their arrays share"
```

---

### Task 5: `RakeOptions` and `TextRankOptions`

Both take the set arm, and both copy `CountVectorizerOptions` rather than paraphrase it: the
member is the same type used the same way, and a second spelling is how two rules drift apart.

**Files:**

- Modify: `src/Lodestar.Text/Keywords/RakeOptions.cs`
- Modify: `src/Lodestar.Text/Keywords/TextRankOptions.cs`
- Create: `tests/Lodestar.Text.Tests/Keywords/KeywordOptionsEqualityTests.cs`
- Create: `docs/reference/text/keywords/rakeoptions-equals.md`
- Create: `docs/reference/text/keywords/rakeoptions-gethashcode.md`
- Create: `docs/reference/text/keywords/textrankoptions-equals.md`
- Create: `docs/reference/text/keywords/textrankoptions-gethashcode.md`
- Modify: `docs/reference/text/keywords/rakeoptions.md`, `docs/reference/text/keywords/textrankoptions.md`
- Modify: `CHANGELOG.md`

**Interfaces:**

- Consumes: `ValueEquality.SameSet` and `ValueEquality.PresenceOf` from Task 2.
- Produces: nothing later tasks rely on.

- [ ] **Step 1: Write the failing tests**

Create `tests/Lodestar.Text.Tests/Keywords/KeywordOptionsEqualityTests.cs`:

```csharp
using Xunit;
using Lodestar.Text.Keywords;

namespace Lodestar.Text.Tests.Keywords;

/// <summary>Decision 0112: StopWords compares as a set, and the hash carries presence only.</summary>
public sealed class KeywordOptionsEqualityTests
{
    [Fact]
    public void Rake_options_with_the_same_stop_words_in_separate_lists_are_equal()
    {
        RakeOptions left = new() { StopWords = ["the", "a"] };
        RakeOptions right = new() { StopWords = ["the", "a"] };

        Assert.Equal(left, right);
        Assert.Equal(left.GetHashCode(), right.GetHashCode());
    }

    [Fact]
    public void Rake_stop_words_compare_as_a_set_so_order_and_repetition_do_not_count()
    {
        RakeOptions left = new() { StopWords = ["the", "the", "a"] };
        RakeOptions right = new() { StopWords = ["a", "the"] };

        Assert.Equal(left, right);
        Assert.Equal(left.GetHashCode(), right.GetHashCode());
    }

    [Fact]
    public void Rake_options_differing_in_a_stop_word_are_unequal()
    {
        RakeOptions left = new() { StopWords = ["the"] };
        RakeOptions right = new() { StopWords = ["an"] };

        Assert.NotEqual(left, right);
    }

    [Fact]
    public void Absent_rake_stop_words_equal_absent_ones_and_not_an_empty_set()
    {
        RakeOptions absent = new();
        RakeOptions alsoAbsent = new();
        RakeOptions empty = new() { StopWords = [] };

        Assert.Equal(absent, alsoAbsent);
        Assert.Equal(absent.GetHashCode(), alsoAbsent.GetHashCode());
        Assert.NotEqual(absent, empty);
    }

    [Fact]
    public void Rake_options_differing_in_a_scalar_are_unequal()
    {
        RakeOptions left = new() { MinLength = 1 };
        RakeOptions right = new() { MinLength = 2 };

        Assert.NotEqual(left, right);
    }

    [Fact]
    public void TextRank_options_with_the_same_stop_words_in_separate_lists_are_equal()
    {
        TextRankOptions left = new() { StopWords = ["the", "a"] };
        TextRankOptions right = new() { StopWords = ["the", "a"] };

        Assert.Equal(left, right);
        Assert.Equal(left.GetHashCode(), right.GetHashCode());
    }

    [Fact]
    public void TextRank_stop_words_compare_as_a_set()
    {
        TextRankOptions left = new() { StopWords = ["the", "the"] };
        TextRankOptions right = new() { StopWords = ["the"] };

        Assert.Equal(left, right);
        Assert.Equal(left.GetHashCode(), right.GetHashCode());
    }

    [Fact]
    public void TextRank_options_differing_in_a_scalar_are_unequal()
    {
        TextRankOptions left = new() { Damping = 0.85 };
        TextRankOptions right = new() { Damping = 0.80 };

        Assert.NotEqual(left, right);
    }

    [Fact]
    public void TextRank_options_differing_in_an_optional_word_count_are_unequal()
    {
        TextRankOptions left = new() { Words = 5 };
        TextRankOptions right = new();

        Assert.NotEqual(left, right);
    }
}
```

- [ ] **Step 2: Run the tests to verify they fail**

```bash
dotnet test tests/Lodestar.Text.Tests -c Release --filter "FullyQualifiedName~KeywordOptionsEquality"
```

Expected: FAIL on every `..._are_equal` test — `StopWords` compares by reference.

- [ ] **Step 3: Give `RakeOptions` its equality**

In `src/Lodestar.Text/Keywords/RakeOptions.cs`, before the record's closing brace:

```csharp

    /// <summary>Compares every option, treating <see cref="StopWords"/> as a set.</summary>
    /// <param name="other">The options to compare against.</param>
    /// <remarks>
    /// <see cref="StopWords"/> compares as a set, not by reference or sequence — the generated
    /// equality would otherwise treat two lists of the same words as unequal. Decision 0112 has
    /// the rule, and <see cref="Lodestar.Text.Vectorization.CountVectorizerOptions"/> the
    /// same member.
    /// </remarks>
    public bool Equals(RakeOptions? other)
    {
        if (ReferenceEquals(this, other))
        {
            return true;
        }
        if (other is null
            || Metric != other.Metric
            || MinLength != other.MinLength
            || MaxLength != other.MaxLength
            || IncludeRepeatedPhrases != other.IncludeRepeatedPhrases
            || !string.Equals(TokenPattern, other.TokenPattern, StringComparison.Ordinal))
        {
            return false;
        }
        return ValueEquality.SameSet(StopWords, other.StopWords);
    }

    /// <summary>Hashes the scalars, which is O(1).</summary>
    /// <remarks>
    /// <see cref="StopWords"/> contributes only whether it is present. Its <em>count</em> cannot
    /// be used: equality compares as a set, so <c>["the", "the"]</c> equals <c>["the"]</c> while
    /// the counts differ, and equal objects must hash alike.
    /// </remarks>
    public override int GetHashCode()
    {
        unchecked
        {
            int hash = (17 * 31) + (int)Metric;
            hash = (hash * 31) + MinLength;
            hash = (hash * 31) + MaxLength;
            hash = (hash * 31) + (IncludeRepeatedPhrases ? 1 : 0);
            hash = (hash * 31) + StringComparer.Ordinal.GetHashCode(TokenPattern);
            return (hash * 31) + ValueEquality.PresenceOf(StopWords);
        }
    }
```

- [ ] **Step 4: Give `TextRankOptions` its equality**

In `src/Lodestar.Text/Keywords/TextRankOptions.cs`, before the record's closing brace:

```csharp

    /// <summary>Compares every option, treating <see cref="StopWords"/> as a set.</summary>
    /// <param name="other">The options to compare against.</param>
    /// <remarks>
    /// <see cref="StopWords"/> compares as a set, not by reference or sequence — the generated
    /// equality would otherwise treat two lists of the same words as unequal. Decision 0112 has
    /// the rule.
    /// </remarks>
    public bool Equals(TextRankOptions? other)
    {
        if (ReferenceEquals(this, other))
        {
            return true;
        }
        if (other is null
            || Window != other.Window
            || MaxIterations != other.MaxIterations
            || Words != other.Words
            || !string.Equals(TokenPattern, other.TokenPattern, StringComparison.Ordinal))
        {
            return false;
        }
        // S1244: value equality between two configurations, where "the same damping" means the
        // same bits. double.Equals also makes NaN equal NaN, which equality must.
#pragma warning disable S1244
        if (!Damping.Equals(other.Damping)
            || !Tolerance.Equals(other.Tolerance)
            || !Ratio.Equals(other.Ratio))
#pragma warning restore S1244
        {
            return false;
        }
        return ValueEquality.SameSet(StopWords, other.StopWords);
    }

    /// <summary>Hashes the scalars, which is O(1).</summary>
    /// <remarks>
    /// <see cref="StopWords"/> contributes only whether it is present, for the reason
    /// <see cref="Equals(TextRankOptions)"/> gives: a set's count is not preserved by its own
    /// equality, and equal objects must hash alike.
    /// </remarks>
    public override int GetHashCode()
    {
        unchecked
        {
            int hash = (17 * 31) + Window;
            hash = (hash * 31) + MaxIterations;
            hash = (hash * 31) + (Words ?? -1);
            hash = (hash * 31) + Damping.GetHashCode();
            hash = (hash * 31) + Tolerance.GetHashCode();
            hash = (hash * 31) + Ratio.GetHashCode();
            hash = (hash * 31) + StringComparer.Ordinal.GetHashCode(TokenPattern);
            return (hash * 31) + ValueEquality.PresenceOf(StopWords);
        }
    }
```

- [ ] **Step 5: Run the tests to verify they pass**

```bash
dotnet test tests/Lodestar.Text.Tests -c Release --filter "FullyQualifiedName~KeywordOptionsEquality"
```

Expected: PASS, 9 tests.

- [ ] **Step 6: Write the four reference pages**

Create `docs/reference/text/keywords/rakeoptions-equals.md`:

````markdown
# RakeOptions.Equals

Compares every option, treating the stop words as a set.

<!-- docs-declaration -->

```csharp
public bool Equals(RakeOptions? other)
```

**Parameters** — `other` is the options to compare against, or `null`.

**Returns** — `true` when every scalar matches and both stop-word collections hold the same
words.

**Example** — the same configuration, written two ways.

```csharp
using Lodestar.Text.Keywords;

RakeOptions left = new() { StopWords = ["the", "the", "a"] };
RakeOptions right = new() { StopWords = ["a", "the"] };

bool same = left == right;  // => True
```

**Remarks** — the stop words are a set, so order and repetition do not count, and an absent
collection is not an empty one. [Decision
0112](https://github.com/CyrilB1531/lodestar/blob/main/docs/decisions/0112-a-record-whose-member-compares-by-reference-writes-its-own-equality.md)
has the rule.

**Applies to** — net10.0, netstandard2.0.

**See also** — [`RakeOptions.GetHashCode`](rakeoptions-gethashcode.md),
[`RakeOptions`](rakeoptions.md).
````

Create `docs/reference/text/keywords/rakeoptions-gethashcode.md`:

````markdown
# RakeOptions.GetHashCode

A hash consistent with the equality, in constant time.

<!-- docs-declaration -->

```csharp
public override int GetHashCode()
```

**Returns** — a hash over the scalars and whether stop words are present.

**Example** — options equal as sets hash alike.

```csharp
using Lodestar.Text.Keywords;

RakeOptions left = new() { StopWords = ["the", "the", "a"] };
RakeOptions right = new() { StopWords = ["a", "the"] };

bool alike = left.GetHashCode() == right.GetHashCode();  // => True
```

**Remarks** — the stop words contribute only their presence, never their count. Set equality
makes `["the", "the"]` equal `["the"]` while the counts differ, and equal objects must hash
alike; hashing the words themselves would be the walk this avoids.

**Applies to** — net10.0, netstandard2.0.

**See also** — [`RakeOptions.Equals`](rakeoptions-equals.md), [`RakeOptions`](rakeoptions.md).
````

Create `docs/reference/text/keywords/textrankoptions-equals.md`:

````markdown
# TextRankOptions.Equals

Compares every option, treating the stop words as a set.

<!-- docs-declaration -->

```csharp
public bool Equals(TextRankOptions? other)
```

**Parameters** — `other` is the options to compare against, or `null`.

**Returns** — `true` when every scalar matches and both stop-word collections hold the same
words.

**Example** — the same configuration, written two ways.

```csharp
using Lodestar.Text.Keywords;

TextRankOptions left = new() { StopWords = ["the", "the"] };
TextRankOptions right = new() { StopWords = ["the"] };

bool same = left == right;  // => True
```

**Remarks** — `Damping`, `Tolerance` and `Ratio` compare by bits, which makes `NaN` equal `NaN`
and keeps equality reflexive. [Decision
0112](https://github.com/CyrilB1531/lodestar/blob/main/docs/decisions/0112-a-record-whose-member-compares-by-reference-writes-its-own-equality.md)
has the rule.

**Applies to** — net10.0, netstandard2.0.

**See also** — [`TextRankOptions.GetHashCode`](textrankoptions-gethashcode.md),
[`TextRankOptions`](textrankoptions.md).
````

Create `docs/reference/text/keywords/textrankoptions-gethashcode.md`:

````markdown
# TextRankOptions.GetHashCode

A hash consistent with the equality, in constant time.

<!-- docs-declaration -->

```csharp
public override int GetHashCode()
```

**Returns** — a hash over the scalars and whether stop words are present.

**Example** — options equal as sets hash alike.

```csharp
using Lodestar.Text.Keywords;

TextRankOptions left = new() { StopWords = ["the", "the"] };
TextRankOptions right = new() { StopWords = ["the"] };

bool alike = left.GetHashCode() == right.GetHashCode();  // => True
```

**Remarks** — the stop words contribute only their presence, never their count, because set
equality does not preserve a count and equal objects must hash alike. An absent `Words` limit
contributes `-1`, so it does not collide with a limit of zero.

**Applies to** — net10.0, netstandard2.0.

**See also** — [`TextRankOptions.Equals`](textrankoptions-equals.md),
[`TextRankOptions`](textrankoptions.md).
````

- [ ] **Step 7: Link them from the two type pages**

At the end of `docs/reference/text/keywords/rakeoptions.md`:

```markdown

## Members

| member | what it does |
| --- | --- |
| [`RakeOptions.Equals`](rakeoptions-equals.md) | Value equality, the stop words as a set. |
| [`RakeOptions.GetHashCode`](rakeoptions-gethashcode.md) | A hash consistent with it. |
```

At the end of `docs/reference/text/keywords/textrankoptions.md`:

```markdown

## Members

| member | what it does |
| --- | --- |
| [`TextRankOptions.Equals`](textrankoptions-equals.md) | Value equality, the stop words as a set. |
| [`TextRankOptions.GetHashCode`](textrankoptions-gethashcode.md) | A hash consistent with it. |
```

- [ ] **Step 8: Add the changelog entry and commit**

In `CHANGELOG.md`, under `## [Unreleased]`, in the `### Lodestar.Text` section's `#### Changed`
heading:

```markdown
- **`RakeOptions` and `TextRankOptions` compare their stop words as a set.** Two option sets
  holding the same words were unequal and now are equal, with `GetHashCode` agreeing —
  `CountVectorizerOptions` already behaved this way, and decision 0112 makes it the rule.
  ([#668](https://github.com/CyrilB1531/lodestar/issues/668))
```

```bash
dotnet build Lodestar.slnx -c Release
python3 tools/check_comment_length.py
git add src/Lodestar.Text/Keywords tests/Lodestar.Text.Tests/Keywords/KeywordOptionsEqualityTests.cs docs/reference/text CHANGELOG.md
git commit -m "Compare the two keyword options' stop words as a set, as the vectorizer does"
```

---

### Task 6: The gates, end to end

Nothing new is written here. This task exists because the three gates that judge this change do
not run in the per-package loops above, and a green `dotnet build` is not a green quality gate.

**Files:** none created or modified, unless a gate reports a finding.

**Interfaces:**

- Consumes: every change from Tasks 1–5.
- Produces: the branch, ready for a pull request.

- [ ] **Step 1: Both frameworks, both suites**

```bash
dotnet build Lodestar.slnx -c Release
dotnet test Lodestar.slnx -c Release
```

Expected: build clean; the test run reports **32 assemblies**. Read the count — a suite that goes
missing has no exit code. The six new test classes are picked up by the `netstandard2.0` mirrors
automatically, because those projects link the same sources.

- [ ] **Step 2: The doc-snippets gate, which executes the new pages' assertions**

The list is the sixteen `ci.yml` packs, not the eight CLAUDE.md's example shows — that example
predates six packages, and three of them (`Lodestar.Cluster`, `Lodestar.Stats`,
`Lodestar.Survival`) own reference pages this change adds.

```bash
dotnet build Lodestar.slnx -c Release
for proj in src/Lodestar.Abstractions src/Lodestar.Text src/Lodestar.Embeddings \
            src/Lodestar.Fuzzy src/Lodestar.Metrics src/Lodestar.Conformal \
            src/Lodestar.Decomposition src/Lodestar.Onnx src/Lodestar.Extensions.AI \
            src/Lodestar.Extensions.MathNet src/Lodestar.Cluster src/Lodestar.Preprocessing \
            src/Lodestar.Stats src/Lodestar.Stats.Regression src/Lodestar.Survival \
            src/Lodestar.Gpu; do
  dotnet pack "$proj" --configuration Release --no-build --output ./artifacts
done
python3 tools/extract_doc_snippets.py
dotnet build samples/Lodestar.DocSnippets -c Release
```

Expected: clean. Every `// =>` in the twelve new reference pages is an assertion that runs, so a
`True` promised by a page and not delivered by the code fails here.

- [ ] **Step 3: The reference gate**

```bash
dotnet test tests/Lodestar.Cluster.Tests -c Release --filter "FullyQualifiedName~ReferenceDocumentation"
dotnet test tests/Lodestar.Stats.Tests -c Release --filter "FullyQualifiedName~ReferenceDocumentation"
dotnet test tests/Lodestar.Survival.Tests -c Release --filter "FullyQualifiedName~ReferenceDocumentation"
dotnet test tests/Lodestar.Text.Tests -c Release --filter "FullyQualifiedName~ReferenceDocumentation"
```

Expected: PASS. A hand-written `Equals` carries no `CompilerGeneratedAttribute`, so it owes an
entry and this is what checks the twelve pages exist and match the assemblies.

- [ ] **Step 4: The remaining guards and the lint**

```bash
sh .githooks/pre-commit
npx markdownlint-cli2 "README.md" "CONTRIBUTING.md" "docs/**/*.md" "tools/README.md" "bench/README.md"
dotnet format Lodestar.slnx --verify-no-changes
python3 tools/check_repeated_literals.py --base origin/main
```

Expected: all exit 0.

- [ ] **Step 5: Clear Sonar before the pull request, not after**

Run the SonarQube MCP server's `analyze_file_list` over every file Tasks 1–5 created or modified.
Duplication is the finding to expect: six `Equals` implementations share a shape, which is why
the comparisons live in `ValueEquality` rather than being written out six times. If the server
reports duplication anyway, the answer is a further extraction, never a suppression.

- [ ] **Step 6: Open the pull request**

```bash
git push -u origin fix/668-record-equality
```

The body closes the issue with `Closes #668`, names decision 0112, and states the correction the
spec records: six records already did this, so the change ratifies a house pattern rather than
inventing one. No version bumps — releasing is a separate act.
