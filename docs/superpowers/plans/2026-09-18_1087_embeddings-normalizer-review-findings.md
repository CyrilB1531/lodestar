# The normalizer's whole-string decomposition — implementation plan

> **For agentic workers:** REQUIRED SUB-SKILL: Use superpowers:subagent-driven-development
> (recommended) or superpowers:executing-plans to implement this plan task-by-task. Steps use
> checkbox (`- [ ]`) syntax for tracking.

**Goal:** BERT's normalizer returns what `tokenizers` returns for text holding two adjacent code
points .NET calls unassigned, and the three claims #1085 made about itself are corrected to what
its evidence supports.

**Architecture:** `Normalize`'s loop already asks `CharUnicodeInfo` for a category per code point
inside `IsControl`. That lookup is hoisted into the loop, which sets a second `bool` — *the text
held an unassigned code point* — and passes it down through `Lowered` and `StripAccents` to
`Decompose`, which takes `Segmented` when it is true. The `try`/`catch` stays as the runtime's own
answer for a refusal .NET's tables did not predict.

**Tech Stack:** C# on `net10.0;netstandard2.0`, xunit v3 on Microsoft.Testing.Platform,
`tokenizers` 0.23.2 in `.venv-oracles`, BenchmarkDotNet 0.14.0.

**Spec:** [`docs/superpowers/specs/2026-09-18_1087_embeddings-normalizer-review-findings.md`](../specs/2026-09-18_1087_embeddings-normalizer-review-findings.md)

**Branch:** `fix/1087-embeddings-normalizer-review-findings`, in a worktree beside the main
checkout, from `origin/main` at `8a226529`.

## Global Constraints

- One commit; `Closes #1087`, `Closes #1088`, `Closes #1089`, `Closes #1090`, `Closes #1091`.
- Everything in English. No `fix:` prefix on the subject.
- ADR 0146 is **not** amended: the fix restores what it decided, it does not change it.
- Comments: two lines inline, eight of XML prose, `tools/check_comment_length.py` counts
  `<remarks>`'s own tags against that budget.
- No machine path in a tracked file — `tools/check_machine_paths.py` reads the plan too.
- The benchmark holds `./.dotnet-guarded acquire` for its whole window and releases in the command
  that ends it; the CPU must be idle before it starts.
- The PR opens as a **draft** and leaves draft only once the post-fix benchmark is in it.

---

### Task 1: The oracle cases that can tell the two orderings apart

**Files:**

- Modify: `tools/generate_oracles.py` (`BERT_BASIC_TEXTS`, `BERT_BASIC_VOCAB`)
- Modify: `tests/oracles/vocab_txt.json` (regenerated)

**Interfaces:**

- Produces: two `vocab_txt.json` cases per model whose `tokens` are the appended pieces under
  `uncased` — which only an unreordered decomposition can reach.

- [ ] **Step 1: Add the two texts**

At the end of `BERT_BASIC_TEXTS`:

```python
    # Issue #1087: U+1ACF and U+1ADD are unassigned to CharUnicodeInfo and combining marks with a
    # non-zero class to ICU, so a whole-string NFD reorders them where tokenizers does not. The
    # pieces below are in the vocabulary, so a reordered pair is [UNK] and the case can tell.
    "a᫏᫝b", "Á᫏᫝á",
```

- [ ] **Step 2: Add the two pieces**

At the end of `BERT_BASIC_VOCAB`, so no existing id moves:

```python
    "a᫏᫝b", "a᫏᫝a",
```

- [ ] **Step 3: Regenerate the corpus**

From a neutral directory that is not an ancestor of the checkout, writing only this corpus:

```bash
cd /var/tmp && PYTHONSAFEPATH=1 <repo>/.venv-oracles/bin/python - <<'PY'
import json, sys
sys.path.insert(0, "<worktree>/tools")
import generate_oracles as g
payload = g.generate_vocab_txt()
path = g.ORACLE_DIR / "vocab_txt.json"
with path.open("w", encoding="utf-8", newline="\n") as f:
    json.dump(payload, f, ensure_ascii=False, indent=1, allow_nan=False)
    f.write("\n")
print(f"{path}: {payload['metadata']['count']} cases")
PY
echo "exit: $?"
```

Expected: `exit: 0`, 82 cases where there were 78, and `git diff --numstat` showing no removed
line other than the two the appended vocabulary entries push down.

- [ ] **Step 4: Check the uncased cases carry the piece, not `[UNK]`**

```bash
python3 -c "
import json
d = json.load(open('tests/oracles/vocab_txt.json'))
for c in d['cases']:
    if '᫏' in c['text']:
        print(c['model'], repr(c['text']), c['tokens'])
"
```

Expected: the two `uncased` rows print `['a᫏᫝b']` and `['a᫏᫝a']`; the two
`cased` rows print `['[UNK]']`, which is what a cased model does with a piece it does not hold.

- [ ] **Step 5: Run it and watch it fail**

```bash
dotnet test tests/Lodestar.Embeddings.Tests -c Release --filter "FullyQualifiedName~VocabTxt"
```

Expected: FAIL on the two uncased cases, which return `[UNK]` because the pair was reordered.
Read the test count, not the colour.

---

### Task 2: The scan comes back, inside the walk that was already happening

**Files:**

- Modify: `src/Lodestar.Embeddings/Tokenization/BertBasicTokenization.cs`

**Interfaces:**

- Produces: `Normalize(string, bool)` unchanged in signature; `Lowered`, `StripAccents` and
  `Decompose` each take one added `bool unassigned`; `IsControl` takes the category instead of
  looking it up.

- [ ] **Step 1: Hoist the category and set the flag**

In `Normalize`, replace the loop body's opening and the call to `IsControl`:

```csharp
        var builder = new StringBuilder(text.Length);
        bool changed = false;
        bool unassigned = false;
        for (int i = 0; i < text.Length; i += Width(text, i))
        {
            int width = Width(text, i);
            int codePoint = width == 2 ? char.ConvertToUtf32(text[i], text[i + 1]) : text[i];
            UnicodeCategory category = CharUnicodeInfo.GetUnicodeCategory(text, i);
            unassigned |= category == UnicodeCategory.OtherNotAssigned;
            if (codePoint == 0 || codePoint == 0xFFFD || IsControl(codePoint, category))
            {
                changed = true;
                continue;
            }
```

and the tail:

```csharp
        // The input itself when nothing was dropped, padded or mapped — ASCII letters, digits and
        // punctuation never are, where tab, newline and the C0 controls are (#992, #1091).
        string cleaned = changed ? builder.ToString() : text;
        return lowercase ? Lowered(cleaned, unassigned) : cleaned;
```

- [ ] **Step 2: Take the category as an argument**

```csharp
    /// <summary><c>tokenizers</c>' <c>is_control</c>: Cc, Cf, Cs and Co but tab, newline and return, and not Cn, which it keeps (#983).</summary>
    private static bool IsControl(int codePoint, UnicodeCategory category)
    {
        if (codePoint is '\t' or '\n' or '\r')
        {
            return false;
        }

        return category is UnicodeCategory.Control
            or UnicodeCategory.Format
            or UnicodeCategory.Surrogate
            or UnicodeCategory.PrivateUse;
    }
```

- [ ] **Step 3: Carry the flag to `Decompose`**

```csharp
    private static string Lowered(string cleaned, bool unassigned) => StripAccents(cleaned, unassigned).ToLowerInvariant();
```

```csharp
    private static string StripAccents(string text, bool unassigned)
    {
        string decomposed = Decompose(text, unassigned);
```

```csharp
    /// <summary>The NFD form, taken around the code points <see cref="Segmented"/> must not let move.</summary>
    /// <remarks>
    /// The whole-string form is ICU's and <see cref="Segmented"/> cuts at what <c>CharUnicodeInfo</c>
    /// calls unassigned, and the two disagree: a code point .NET's tables do not know and ICU gives a
    /// non-zero combining class is reordered by the first and left alone by the second, where
    /// <c>tokenizers</c> leaves it alone (#1087). So an unassigned code point anywhere sends the whole
    /// text through the segmented walk, on the flag <see cref="Normalize"/> already had the category
    /// for. The catch stays because a runtime may refuse what .NET's tables call assigned — NLS reads
    /// the OS's, not .NET's — and the exception is the only place it says so.
    /// </remarks>
    private static string Decompose(string text, bool unassigned)
    {
        if (unassigned)
        {
            return Segmented(text);
        }

        try
        {
            return text.Normalize(NormalizationForm.FormD);
        }
        catch (ArgumentException)
        {
            return Segmented(text);
        }
    }
```

- [ ] **Step 4: Correct `Segmented`'s own claim**

Its `<remarks>` says an unassigned code point "has combining class 0, so it starts a new sequence
and cutting the text at it changes no decomposition". That is true of .NET's tables and of the
reference's, and false of ICU's — which is the whole of #1087. Replace it with:

```csharp
    /// <summary>The NFD form of each stretch between the unassigned code points, which pass through as they are.</summary>
    /// <remarks>
    /// An unassigned code point has combining class 0 to <c>CharUnicodeInfo</c> and to the reference's
    /// tables, so cutting at it changes no decomposition either side would make. ICU's tables are newer
    /// and give some of them a class, which is why this walk is taken whenever one is present rather
    /// than only when the runtime refuses the string (#983, #1087).
    /// </remarks>
```

- [ ] **Step 5: Run the corpus and watch it pass**

```bash
dotnet test tests/Lodestar.Embeddings.Tests -c Release --filter "FullyQualifiedName~VocabTxt"
```

Expected: PASS, at the count Task 1 Step 5 reported.

- [ ] **Step 6: Run the package, both assemblies**

```bash
dotnet test tests/Lodestar.Embeddings.Tests -c Release
dotnet test tests/Lodestar.Embeddings.NetStandard.Tests -c Release
```

Expected: both green, the mirror one test ahead of its suite.

---

### Task 3: The two comments, and the walk a test can now reach

**Files:**

- Modify: `tests/Lodestar.Embeddings.Tests/Tokenization/BertNormalizerTests.cs`

- [ ] **Step 1: Say what the mirror proves**

Replace the class `<summary>`:

```csharp
/// <summary>
/// The segmented decomposition, which #1087 made reachable from any text holding a code point
/// <c>CharUnicodeInfo</c> calls unassigned rather than only from one the runtime refuses —
/// <c>U+FFFE</c> alone on .NET 10, every unassigned one under NLS. The mirror runs the
/// <c>netstandard2.0</c> assembly on the .NET 10 runtime, so it proves that assembly loads and
/// answers, not that a second Unicode backend was met; no CI job meets NLS (#1089).
/// <c>vocab_txt.json</c> replays the same texts against <c>tokenizers</c>, which keeps the
/// noncharacter and the unassigned pair as it keeps the rest.
/// </summary>
```

- [ ] **Step 2: Drive the multi-cut walk**

Add, beside the existing facts:

```csharp
    [Fact]
    public void Two_unassigned_code_points_keep_their_order_around_a_third()
    {
        // Three cuts, which no input reached before #1087: U+0378, U+1ACF and U+1ADD are all
        // unassigned to CharUnicodeInfo, and ICU reorders the last two (#1089).
        WordPieceTokenizer tokenizer = Uncased();

        string[] tokens = tokenizer.Encode("Á͸᫏᫝é").Tokens;

        Assert.Equal(["a͸᫏᫝e"], tokens);
    }
```

If `Uncased()`'s vocabulary does not hold that piece, assert instead on the one it does: run the
same text through `tokenizers` in `.venv-oracles` and pin what it answers, adding the piece to
`BERT_BASIC_VOCAB` in Task 1 rather than guessing it here.

- [ ] **Step 3: The ASCII comment**

Already replaced in Task 2 Step 1. Confirm no other copy of the claim survives:

```bash
grep -rn "which ASCII text never is" src/ docs/
```

Expected: no output.

- [ ] **Step 4: Run the package again**

```bash
dotnet test tests/Lodestar.Embeddings.Tests -c Release
```

---

### Task 4: The sweep that includes the case, and the claims it settles

**Files:**

- Modify: `docs/guides/performance.md` (the *BERT's basic tokenization* section)
- Modify: `CHANGELOG.md` (the `Lodestar.Embeddings` entry #1048 left)

- [ ] **Step 1: Find the diverging set**

A scratch console project outside the repository, named `Lodestar.Embeddings.Tests` so
`InternalsVisibleTo` admits it, referencing `src/Lodestar.Embeddings`:

```csharp
var moving = new List<int>();
for (int cp = 0; cp <= 0x10FFFF; cp++)
{
    if (cp is >= 0xD800 and <= 0xDFFF) { continue; }
    string one = char.ConvertFromUtf32(cp);
    if (CharUnicodeInfo.GetUnicodeCategory(one, 0) != UnicodeCategory.OtherNotAssigned) { continue; }
    // A code point ICU gives a combining class moves left past a starter; .NET's tables, which
    // call it unassigned, give it class 0 and would not.
    if (("a" + one).Normalize(NormalizationForm.FormD) != "a" + one) { moving.Add(cp); }
}
Console.WriteLine($"unassigned: …, of which ICU reorders: {moving.Count}");
```

Record both counts; they are what the paragraph will state.

- [ ] **Step 2: Sweep, on the fix**

In the same program, comparing `BertBasicTokenization.Normalize(text, lowercase: true)` against
the segmented-always form:

- every unassigned code point between the nine fixed neighbour pairs the old sweep used;
- every ordered pair drawn from the diverging set;
- every unassigned code point against each member of the diverging set, in both orders.

Print the total number of inputs and the number of differences.

- [ ] **Step 3: Rewrite the paragraph**

Replace `docs/guides/performance.md`'s "**The output is unchanged, and that was measured rather
than argued.**" paragraph with what Step 2 actually found, saying plainly that the sweep #1085
published covered nine fixed neighbours and not a pair of unassigned code points, that the pair is
the case that diverges, and what the corrected sweep covers and finds. The document's subject is
what was measured, so the count and its window belong in that sentence.

- [ ] **Step 4: Correct the changelog**

The #1048 entry carries the same claim. Replace it with one sentence that is true of the merged
state plus this fix, and add the fix's own entry naming the five issues.

---

### Task 5: The benchmark #1090 asks for

- [ ] **Step 1: Check the machine is free**

```bash
uptime && ./.dotnet-guarded status
```

Expected: a one-minute load average under 1.0 and `free`. If not, wait; a benchmark taken on a busy
machine is worse than none.

- [ ] **Step 2: Hold the lock and measure A/B/A**

`main` at `8a226529`, then the fix, then `main` again, from worktrees outside the checkout,
`--filter '*BertNormalizer*'`. Release the lock in the same command that ends the campaign.

- [ ] **Step 3: Publish the six rows**

Add the A/B/A table to `docs/guides/performance.md` beside the #1048 one, with the machine, the
window, the load average and the lock, in the shape the section already uses.

---

### Task 6: The gates, then the commit

- [ ] **Step 1: Every check script**

```bash
for s in tools/check_*.py; do echo "== $s"; python3 "$s" || echo "FAILED $s"; done
python3 tools/check_repeated_literals.py --base origin/main
python3 tools/check_adr_immutable.py --base origin/main
```

- [ ] **Step 2: Format, lint, snippets**

```bash
dotnet format Lodestar.slnx --verify-no-changes
npx markdownlint-cli2 "README.md" "CONTRIBUTING.md" "docs/**/*.md" "tools/README.md" "bench/README.md"
```

- [ ] **Step 3: The whole suite**

```bash
dotnet test Lodestar.slnx -c Release 2>&1 | tail -40
```

Expected: **36 assemblies**, and a count no lower than `main`'s.

- [ ] **Step 4: Commit, then open the PR as a draft**

Subject: `Take the segmented decomposition whenever a code point is unassigned, and say what the sweep covered`.
Body: the reference measurement, the five `Closes` lines, and the `Co-Authored-By` trailer. The PR
leaves draft only once Task 5's table is in it.
