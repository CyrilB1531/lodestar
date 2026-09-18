# The segmented decomposition's refusal guard — implementation plan

> **For agentic workers:** REQUIRED SUB-SKILL: Use superpowers:subagent-driven-development
> (recommended) or superpowers:executing-plans to implement this plan task-by-task. Steps use
> checkbox (`- [ ]`) syntax for tracking.

**Goal:** no `ArgumentException` from `string.Normalize` leaves `BertBasicTokenization`, on either
route into `Segmented`, and a code point the runtime refuses passes through as an unassigned one
does.

**Architecture:** `Decompose` gains an internal overload taking the normalization as a
`Func<string, string>`; the private one passes the runtime's `FormD`. `Segmented` hands each
stretch to `AppendDecomposed`, which tries the stretch whole and, on a refusal, cuts out the code
points refused on their own and decomposes what lies between them. The `catch` in `Decompose` no
longer re-normalizes the whole string: it falls through to the same walk.

**Tech Stack:** C# on `net10.0;netstandard2.0`, xunit v3 on Microsoft.Testing.Platform,
`tokenizers` 0.23.2 in `.venv-oracles` for the differential run.

**Spec:** [`docs/superpowers/specs/2026-09-18_1094_segmented-normalize-guard.md`](../specs/2026-09-18_1094_segmented-normalize-guard.md)

**Branch:** `fix/1094-segmented-normalize-guard`, in a worktree beside the main checkout, from
`origin/main` at `791f3f49`.

## Global Constraints

- One commit; `Closes #1094`. Everything in English, no `fix:` prefix on the subject.
- No ADR: the decision restores the rule 0146 already applies to unassigned code points.
- Comments: two lines inline, eight of XML prose (`tools/check_comment_length.py`).
- No public API moves, so no reference page, sample or equivalence row changes.
- Every `dotnet` command runs through the repository root's `.dotnet-guarded`.

---

### Task 1: The seam and the tests that use it

**Files:**

- Modify: `tests/Lodestar.Embeddings.Tests/Tokenization/BertNormalizerTests.cs`

- [x] **Step 1: A normalization that refuses what an old NLS would**

```csharp
private static string RefusesTheEmoji(string text) =>
    text.Contains("\U0001F600", StringComparison.Ordinal)
        ? throw new ArgumentException("Invalid Unicode code point found.", nameof(text))
        : text.Normalize(System.Text.NormalizationForm.FormD);
```

- [x] **Step 2: Three facts, one per route**

```csharp
Assert.Equal("\U0001F600A\u0301\u0378e\u0301",
    BertBasicTokenization.Decompose("\U0001F600\u00C1\u0378\u00E9", unassigned: true, RefusesTheEmoji));
Assert.Equal("A\u0301\U0001F600e\u0301",
    BertBasicTokenization.Decompose("\u00C1\U0001F600\u00E9", unassigned: false, RefusesTheEmoji));
Assert.Equal("\u00C1ab", BertBasicTokenization.Decompose("\u00C1ab", unassigned: false, RefusesThePair));
```

`RefusesThePair` throws on any string holding `"ab"`, so no single code point is refused.

- [x] **Step 3: Run them red**

```bash
./.dotnet-guarded dotnet test tests/Lodestar.Embeddings.Tests -c Release --filter "FullyQualifiedName~BertNormalizer"
```

Expected: a compile failure, since `Decompose` is private and takes no delegate.

### Task 2: The guarded walk

**Files:**

- Modify: `src/Lodestar.Embeddings/Tokenization/BertBasicTokenization.cs`

- [x] **Step 1: The seam**

```csharp
private static readonly Func<string, string> Nfd = static text => text.Normalize(NormalizationForm.FormD);

private static string Decompose(string text, bool unassigned) => Decompose(text, unassigned, Nfd);

internal static string Decompose(string text, bool unassigned, Func<string, string> nfd)
{
    if (!unassigned)
    {
        try
        {
            return nfd(text);
        }
        catch (ArgumentException)
        {
        }
    }

    return Segmented(text, nfd);
}
```

The `catch` keeps its comment; it now falls through to the walk instead of calling it.

- [x] **Step 2: `Segmented` hands each stretch to `AppendDecomposed`**

```csharp
AppendDecomposed(builder, text.Substring(start, i - start), nfd);
builder.Append(text, i, Width(text, i));
```

and once more for the tail, with the builder always allocated: the flag guarantees a cut, and the
`catch` route is the one that re-threw when there was none.

- [x] **Step 3: `AppendDecomposed` and `TryDecompose`**

```csharp
if (TryDecompose(stretch, nfd, out string? decomposed))
{
    builder.Append(decomposed);
    return;
}

int start = 0;
for (int i = 0; i < stretch.Length; i += Width(stretch, i))
{
    if (!TryDecompose(stretch.Substring(i, Width(stretch, i)), nfd, out _))
    {
        string before = stretch.Substring(start, i - start);
        builder.Append(TryDecompose(before, nfd, out decomposed) ? decomposed : before);
        builder.Append(stretch, i, Width(stretch, i));
        start = i + Width(stretch, i);
    }
}
```

and the tail the same way. `TryDecompose` catches `ArgumentException` alone.

- [x] **Step 4: Run them green**

Same command as Task 1 Step 3. Expected: 6 passed.

### Task 3: The differential run

- [x] **Step 1:** 50,000 random texts through `BertNormalizer(lowercase=True)` of `tokenizers`
  0.23.2, from a directory outside the checkout.
- [x] **Step 2:** the same texts through `BertBasicTokenization.Normalize` by reflection, on `main`'s
  build and the branch's. Expected: the two agree on every text, and every text differing from the
  reference holds a code point that differs on its own between two letters.

### Task 4: The gates, then the commit

- [x] **Step 1: Every check script**

```bash
for s in tools/check_*.py; do echo "== $s"; python3 "$s" || echo "FAILED $s"; done
python3 tools/check_repeated_literals.py --base origin/main
python3 -m pytest tools/tests -q
```

- [x] **Step 2: Format and lint**

```bash
./.dotnet-guarded dotnet format Lodestar.slnx --verify-no-changes
npx markdownlint-cli2 "README.md" "CONTRIBUTING.md" "docs/**/*.md" "tools/README.md" "bench/README.md"
```

- [x] **Step 3: The whole suite**

```bash
./.dotnet-guarded dotnet test Lodestar.slnx -c Release
```

Expected: **36 assemblies**, and three more tests than `main` in each of `Lodestar.Embeddings.Tests` and its mirror.

- [x] **Step 4: The review pass**, the code-review skill over the diff; fixes folded in.

- [x] **Step 5: Commit, rebase on `origin/main`, open the PR**

Subject: `Pass a code point the runtime refuses through the segmented decomposition, as an unassigned one`.
Body: the two routes, the decision and its loser, the differential run's counts, `Closes #1094`.
