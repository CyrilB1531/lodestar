# Seven BERT-path findings in Lodestar.Embeddings Implementation Plan

> **For agentic workers:** REQUIRED SUB-SKILL: Use superpowers:subagent-driven-development (recommended) or superpowers:executing-plans to implement this plan task-by-task. Steps use checkbox (`- [ ]`) syntax for tracking.

**Goal:** Make BERT's basic tokenization cost less than it did before #992 on every case #992 measured, with #992's allocations, and make the four documents that describe that pipeline — ADR 0144, the `docs/equivalence.md` row, the `Decompose` summary and the `CA1308` justification — say what the code does.

**Architecture:** `BertBasicTokenization.Normalize` replaces its second pass (`Same`) with a `bool` the appending loop sets, and `Decompose` tries the whole-string `string.Normalize(FormD)` first and walks the text in segments only in the `catch`. `WordPieceTokenizer.TokenizeWord` tests the UTF-16 length before scanning code points. No public API changes and no output changes: the frozen `vocab_txt.json` corpus is the proof, and it gains the one input that reaches the new `catch`.

**Tech Stack:** C# on `net10.0;netstandard2.0`, xunit v3 on Microsoft.Testing.Platform, SonarAnalyzer.CSharp at `AnalysisMode=All`, BenchmarkDotNet, `tokenizers` 0.23.2 as the oracle through `.venv-oracles`.

**Spec:** [`docs/superpowers/specs/2026-09-18_1048_embeddings-normalizer-findings.md`](../specs/2026-09-18_1048_embeddings-normalizer-findings.md)

**Branch:** `fix/1048-embeddings-normalizer-findings`, off `origin/main` at `00774eb1`.

## Global Constraints

- One commit for the whole pull request: `Closes #1048`, `Closes #1049`, `Closes #1050`, `Closes #1051`, `Closes #1052`, `Closes #1053`, `Closes #1064`.
- Both target frameworks. `try`/`catch` and `bool` locals need no polyfill; nothing here may add an `#if` at a call site.
- No version number moves: `src/Lodestar.Embeddings/Version.props` is not touched.
- No new public type or method, so no reference-gate entry and no `samples/Lodestar.Sample` change. `BertBasicTokenization` is `internal`.
- Warnings are errors and the Sonar analyzers gate the build. A `catch (ArgumentException)` that swallows is CA1031-adjacent — it must name why in a comment a reviewer can disagree with.
- Comments follow the four rules: why not what, two lines inline, eight of prose in XML.
- Every `dotnet` command runs through `./.dotnet-guarded`, from the repository root. `$REPO` in the
  commands below is that root, for the two steps that run from somewhere else.
- The oracle generator runs from a neutral directory (`/var/tmp`, not `/tmp`, and never under the checkout) with `PYTHONSAFEPATH=1`, and its own exit code is read — never a pipeline's.
- `docs/equivalence.md` lands in this commit, not afterwards. `CHANGELOG.md` gets one sentence per entry with its issue.
- The perf half carries before/after numbers on a named machine (AMD Ryzen 7 8700G, Ubuntu 26.04.1, .NET SDK 10.0.401), measured with the machine otherwise idle and the lock held.

---

### Task 1: `Normalize` knows what it changed, and the dead suppression goes

Closes the first half of #1048 and all of #1051.

**Files:**

- Modify: `src/Lodestar.Embeddings/Tokenization/BertBasicTokenization.cs:17-51` (the `#pragma` pair, the loop, the `Same` call), `:87-105` (delete `Same`, reword `Lowered`'s reason)

**Interfaces:**

- Consumes: nothing new.
- Produces: no signature change. `public static string Normalize(string text, bool lowercase)` keeps its behaviour exactly; `private static bool Same(StringBuilder, string)` ceases to exist.

- [ ] **Step 1: Prove the current output, so the refactor has a baseline**

The frozen corpus is the assertion. Run it first and record the count:

```bash
./.dotnet-guarded dotnet test tests/Lodestar.Embeddings.Tests -c Release --filter "FullyQualifiedName~VocabTxtLoader"
```

Expected: PASS, and the test count is not zero — read the count, not the colour.

- [ ] **Step 2: Replace the second pass with the flag**

In `src/Lodestar.Embeddings/Tokenization/BertBasicTokenization.cs`, delete the
`#pragma warning disable CA1308` line above `Normalize` and the `#pragma warning restore CA1308`
line below it, and rewrite the method body so it reads exactly:

```csharp
    /// <summary><c>BertNormalizer(clean_text, handle_chinese_chars, strip_accents=None, lowercase)</c>.</summary>
    public static string Normalize(string text, bool lowercase)
    {
        var builder = new StringBuilder(text.Length);
        bool changed = false;
        for (int i = 0; i < text.Length; i += Width(text, i))
        {
            int width = Width(text, i);
            int codePoint = width == 2 ? char.ConvertToUtf32(text[i], text[i + 1]) : text[i];
            if (codePoint == 0 || codePoint == 0xFFFD || IsControl(text, i, codePoint))
            {
                changed = true;
                continue;
            }

            if (IsWhitespace(text, i, codePoint))
            {
                builder.Append(' ');
                changed |= text[i] != ' ';
            }
            else if (IsCjk(codePoint))
            {
                builder.Append(' ').Append(text, i, width).Append(' ');
                changed = true;
            }
            else
            {
                builder.Append(text, i, width);
            }
        }

        // The input itself when nothing was dropped, padded or mapped: ASCII text is the common
        // case and copying it three more times was most of what this path cost (#992). The loop
        // says so rather than a second pass comparing the two, which walked all of the text it
        // was about to hand back unchanged (#1048).
        string cleaned = changed ? builder.ToString() : text;
        return lowercase ? Lowered(cleaned) : cleaned;
    }
```

- [ ] **Step 3: Delete `Same` and let `Lowered` say why it lowercases**

Delete the whole `Same` method with its `<summary>`, and replace `Lowered`'s three comment lines
so the block reads:

```csharp
    /// <summary>The accent strip and the lowercasing, which only text holding a mark pays a second pass for.</summary>
    // CA1308: lowercasing is the normalizer's own step, not a comparison key; an upper-cased text
    // would match no uncased vocabulary entry.
#pragma warning disable CA1308
    private static string Lowered(string cleaned) => StripAccents(cleaned).ToLowerInvariant();
#pragma warning restore CA1308
```

- [ ] **Step 4: Run the corpus again, both frameworks**

```bash
./.dotnet-guarded dotnet test tests/Lodestar.Embeddings.Tests tests/Lodestar.Embeddings.NetStandard.Tests -c Release
```

Expected: PASS, the same test count as Step 1's suite plus the mirror's guard facts. Every
`vocab_txt.json` case — tab, `U+3000`, `U+00A0`, the dropped `U+0000`/`U+200B`/`U+00AD`, the CJK
padding, the unassigned code points — is a `changed` branch, so a wrong flag fails here.

- [ ] **Step 5: `git add` and hold** — the commit is one for the whole pull request (see Task 8).

---

### Task 2: `Decompose` tries the whole string, and walks it only when refused

Closes the second half of #1048 and all of #1050.

**Files:**

- Modify: `src/Lodestar.Embeddings/Tokenization/BertBasicTokenization.cs:144-167`
- Modify: `tools/generate_oracles.py:2228-2231` (one text in `BERT_BASIC_TEXTS`)
- Modify: `tests/oracles/vocab_txt.json` (regenerated)
- Test: `tests/Lodestar.Embeddings.Tests/Tokenization/BertNormalizerTests.cs` (create)

**Interfaces:**

- Consumes: `Width(string, int)` and `IsControl(string, int, int)`, unchanged.
- Produces: `private static string Decompose(string text)` keeps its signature; a new
  `private static string Segmented(string text)` holds today's loop.

- [ ] **Step 1: Write the failing test**

Create `tests/Lodestar.Embeddings.Tests/Tokenization/BertNormalizerTests.cs`:

```csharp
using Lodestar.Embeddings.Tokenization;

namespace Lodestar.Embeddings.Tests.Tokenization;

/// <summary>
/// The one input on .NET 10 that <see cref="string.Normalize(NormalizationForm)"/> refuses, which is
/// therefore the only input reaching the segmented walk behind the accent strip (#1050).
/// </summary>
public sealed class BertNormalizerTests
{
    private static WordPieceVocabulary Vocabulary() =>
        new(
            new Dictionary<string, int> { ["[UNK]"] = 0, ["a"] = 1, ["##a"] = 2, ["￾"] = 3 },
            "[UNK]",
            "##",
            Lowercase: true)
        { BasicTokenization = true };

    [Fact]
    public void A_noncharacter_survives_the_normalizer_with_its_neighbours_stripped()
    {
        var tokenizer = new WordPieceTokenizer(Vocabulary());

        // U+FFFE is unassigned, so the normalizer keeps it as tokenizers does, and it sits inside
        // one pre-token with the two accented letters, whose marks the uncased path strips.
        Assert.Equal(["a", "￾", "##a"], tokenizer.Encode("Á￾á").Tokens);
    }
}
```

- [ ] **Step 2: Run it against the current code to see it pass, which is the point**

```bash
./.dotnet-guarded dotnet test tests/Lodestar.Embeddings.Tests -c Release --filter "FullyQualifiedName~BertNormalizerTests"
```

Expected: PASS on the unchanged source — this is a characterization test for the path Step 3
reorders, so it must be green before and after. If it fails, the expectation is wrong and the
tokens it actually returns are what to assert; the encode is the reference's, proven by
`vocab_txt.json`.

- [ ] **Step 3: Reorder `Decompose`**

Replace the `Decompose` block with:

```csharp
    /// <summary>The NFD form, taken around the code points <see cref="string.Normalize(NormalizationForm)"/> refuses.</summary>
    /// <remarks>
    /// On .NET 10 that is `U+FFFE` alone, a noncharacter <c>tokenizers</c> keeps; the `netstandard2.0`
    /// assembly on .NET Framework reaches NLS, which refuses every unassigned code point (#1050).
    /// Lone surrogates, the other refusal, are already gone — <see cref="IsControl"/> drops them.
    /// </remarks>
    private static string Decompose(string text)
    {
        try
        {
            return text.Normalize(NormalizationForm.FormD);
        }
        catch (ArgumentException)
        {
            // The refusal is the only signal either runtime gives, and which code points it names
            // differs between them, so the segmented form is reached by being refused (#1050).
            return Segmented(text);
        }
    }

    /// <summary>The NFD form of each stretch between the unassigned code points, which pass through as they are.</summary>
    /// <remarks>
    /// An unassigned code point has combining class 0, so it starts a new sequence and cutting the text at it
    /// changes no decomposition; it passes through as NFD leaves it (#983).
    /// </remarks>
    private static string Segmented(string text)
    {
        StringBuilder? builder = null;
        int start = 0;
        for (int i = 0; i < text.Length; i += Width(text, i))
        {
            if (CharUnicodeInfo.GetUnicodeCategory(text, i) == UnicodeCategory.OtherNotAssigned)
            {
                builder ??= new StringBuilder(text.Length);
                builder.Append(text.Substring(start, i - start).Normalize(NormalizationForm.FormD));
                builder.Append(text, i, Width(text, i));
                start = i + Width(text, i);
            }
        }

        return builder is null
            ? text.Normalize(NormalizationForm.FormD)
            : builder.Append(text.Substring(start).Normalize(NormalizationForm.FormD)).ToString();
    }
```

- [ ] **Step 4: Run the test and the corpus**

```bash
./.dotnet-guarded dotnet test tests/Lodestar.Embeddings.Tests -c Release --filter "FullyQualifiedName~BertNormalizerTests|FullyQualifiedName~VocabTxtLoader"
```

Expected: PASS, non-zero count. `Segmented` is still reached — by `U+FFFE` on this runtime — so
`x͸y`, `É͸é` and the other #983 cases now take the fast `try` and must give
the same ids they gave before.

- [ ] **Step 5: Add the noncharacter to the oracle corpus**

In `tools/generate_oracles.py`, in `BERT_BASIC_TEXTS`, after the `# Issue #983` block, add:

```python
    # Issue #1050: the one code point .NET 10's Normalize refuses, so the only input that reaches
    # the segmented walk; tokenizers keeps it like any other unassigned one.
    "a￾a", "Á￾á",
```

- [ ] **Step 6: Regenerate and compare, from a neutral directory**

`tools/generate_oracles.py` takes no arguments and writes every corpus, so regenerating one means
calling its generator function. From a neutral directory, with a driver that reuses the module's own
`ORACLE_DIR` and `json.dump` arguments — `ensure_ascii=False, indent=1, allow_nan=False`, `newline="\n"`,
trailing newline — so the file is byte-identical in shape to the other 100:

```bash
cd /var/tmp && PYTHONSAFEPATH=1 "$REPO"/.venv-oracles/bin/python "$SCRATCH"/regen_one.py vocab_txt.json
echo "generator exit: $?"
```

Expected: exit 0, and `tests/oracles/vocab_txt.json`'s `count` rises from 74 to 78 (two texts × cased
and uncased). Read that exit code, not a pipeline's. Then `git diff` the corpus: the only changes must
be the two vocabulary entries, the four cases, and the count.

- [ ] **Step 7: Replay the new cases**

```bash
./.dotnet-guarded dotnet test tests/Lodestar.Embeddings.Tests tests/Lodestar.Embeddings.NetStandard.Tests -c Release --filter "FullyQualifiedName~VocabTxt"
```

Expected: PASS with four more cases than Step 4.

---

### Task 3: The code-point cap short-circuits on the UTF-16 length

Closes #1052.

**Files:**

- Modify: `src/Lodestar.Embeddings/Tokenization/WordPieceTokenizer.cs:342-343` (the remark), `:360` (the test)

**Interfaces:**

- Consumes: `private static int CodePointLength(ReadOnlySpan<char> word)`, unchanged.
- Produces: no signature change.

- [ ] **Step 1: Confirm the existing cases cover both sides of the `&&`**

```bash
./.dotnet-guarded dotnet test tests/Lodestar.Embeddings.Tests -c Release --filter "FullyQualifiedName~VocabTxt"
```

`vocab_txt.json` carries `"\U0001d400" * 51` — 51 code points in 102 units, which passes the cap
only because the second test is the code-point one. That case is the guard for this change.

- [ ] **Step 2: Guard the scan**

In `TokenizeWord`, replace the condition:

```csharp
        if (word.Length > _maxCharsPerWord && CodePointLength(word) > _maxCharsPerWord)
```

and extend `CodePointLength`'s remark to say which half protects which:

```csharp
    /// <summary>The word's length in code points, which is what <c>tokenizers</c> caps (#992).</summary>
    /// <remarks>
    /// A surrogate pair counted twice made a 51-character astral word exceed a limit of 100. The scan
    /// runs only once the UTF-16 length is already over the cap, which it cannot be for a word under
    /// it — a code point is one or two units, never none (#1052).
    /// </remarks>
```

- [ ] **Step 3: Run the suites**

```bash
./.dotnet-guarded dotnet test tests/Lodestar.Embeddings.Tests tests/Lodestar.Embeddings.NetStandard.Tests -c Release
```

Expected: PASS. The 51-`U+1D400` case still encodes to pieces rather than `[UNK]`, and a
201-unit ASCII word still becomes `[UNK]`.

---

### Task 4: The equivalence row says what the sweep measured, in both directions

Closes #1053 and #1064.

**Files:**

- Modify: `docs/equivalence.md:129`

- [ ] **Step 1: Replace the divergence sentence**

In the `BertTokenizer(vocab_file=…)` row, the text currently reads
"… and about 140 punctuation characters split a word here and not there; the same residue #887
records for the `Whitespace` pre-tokenizer." Replace from "about 600" through "pre-tokenizer" with:

```text
about 600 assigned code points are classified differently by .NET 10 and by `tokenizers`' own tables, so 455 nonspacing marks are stripped here and kept there, 20 format characters dropped here and kept there, and about 140 punctuation characters split a word here and not there. Four go the other way: `U+1734` and `U+1171E` are stripped by `tokenizers` and kept here, and `U+166D` and `U+111C9` split a word there and not here — the same table skew read backwards, all four moved category in a recent Unicode version. Measured by the code-point sweeps of [#992](https://github.com/CyrilB1531/lodestar/issues/992) and [#983](https://github.com/CyrilB1531/lodestar/issues/983) on this pipeline; #887's sweep of the `Whitespace` pre-tokenizer found no residue at all, which is what rows above and below still claim for it.
```

- [ ] **Step 2: Check the two rows the sentence now points at are still true**

```bash
grep -n "0 differences\|Exact parity, swept over all 1,112,064" docs/equivalence.md
```

Expected: rows 105 and 136 still carry #887's exact-parity claim, unedited.

- [ ] **Step 3: Lint the file**

```bash
npx markdownlint-cli2 "docs/**/*.md"
```

Expected: zero errors.

---

### Task 5: ADR 0146 amends 0144's dropped-Cn clause

Closes #1049.

**Files:**

- Create: `docs/decisions/0146-berts-normalizer-keeps-the-unassigned-code-points.md`
- Modify: `docs/decisions/index.yaml`, `docs/decisions/README.md` (both regenerated)

- [ ] **Step 1: Take the number from the repository, not from the directory listing**

```bash
./.next-adr
```

Expected: `0146`. If it prints anything else, use what it prints — another branch has taken 0146.

- [ ] **Step 2: Write the record**

Create the file with the frontmatter `status: accepted`, `supersedes: []`, `amends: ["0144"]`,
`applies: []`, and a body that states: 0144's Decision listed Cn among the categories dropped;
`tokenizers`' `is_control` tests `char::is_control` plus the Cf/Cs/Co categories and never Cn, so
an unassigned code point is text to it; keeping them is what #983 measured against
`tokenizers` 0.23.2; the consequence is that a recent emoji .NET 10's tables do not know yet
becomes `[UNK]` instead of vanishing. Refused options: patching .NET's table (the skew is a
moving target and `docs/equivalence.md` records it), and leaving 0144 to be read as written.

- [ ] **Step 3: Regenerate the index and the prose table**

```bash
python3 tools/regen_adr_index.py
```

Expected: `docs/decisions/index.yaml` shows `0144` with `amended_by: ["0146"]` and `0146` with
`amends: ["0144"]`; `docs/decisions/README.md` gains the 0146 row. If the script does not write
the README, add the row by hand in the same shape as its neighbours.

- [ ] **Step 4: Prove 0144's body did not move**

```bash
python3 tools/check_adr_immutable.py
```

Expected: exit 0.

---

### Task 6: The benchmark class, and the numbers

The perf half of #1048 and #1052 cannot ship without before/after on a named machine.

**Files:**

- Create: `bench/Lodestar.Text.Benchmarks/BertNormalizerBenchmarks.cs`
- Modify: `bench/bench-map.json` (register the class and its sources), `bench/README.md` (a new
  numbered section), `docs/guides/performance.md` (the numbers)

**Interfaces:**

- Consumes: `BenchCorpus.Path("vocab_30k.txt")` and `BenchCorpus.Path("documents.json")`,
  `VocabTxtLoader.Load(string, bool)`, `WordPieceTokenizer.Encode(string)`.
- Produces: `public class BertNormalizerBenchmarks` with `[Params]` `Text` of
  `enum NormalizerText { Ascii, Accented, Cjk }`, `[Params(false, true)] public bool Lowercase`,
  and one `[Benchmark] public int Encode()`.

- [ ] **Step 1: Write the benchmark**

```csharp
using System.Text.Json;
using BenchmarkDotNet.Attributes;
using Lodestar.Embeddings.Persistence;
using Lodestar.Embeddings.Tokenization;

namespace Lodestar.Text.Benchmarks;

/// <summary>Which flavour of text a row of the BERT normalizer table measures.</summary>
public enum NormalizerText
{
    /// <summary>The corpus documents as generated, which are lowercase ASCII.</summary>
    Ascii,

    /// <summary>The same documents with every <c>e</c> replaced by <c>é</c>, so the accent strip runs.</summary>
    Accented,

    /// <summary>The same documents with every <c>a</c> replaced by a CJK ideograph, so the padding runs.</summary>
    Cjk,
}

/// <summary>
/// The <c>vocab.txt</c> route's normalizer, the half of an encode BERT's BasicTokenizer adds:
/// `BertNormalizer` then `BertPreTokenizer` over the corpus documents, cased and uncased.
/// </summary>
/// <remarks>
/// The flavours are the cases #992 changed and #1048 measured: ASCII text the normalizer does not
/// touch, accented text an uncased model decomposes, and ideographs it pads.
/// </remarks>
[MemoryDiagnoser]
public class BertNormalizerBenchmarks
{
    private readonly Dictionary<NormalizerText, string[]> _texts = [];
    private WordPieceTokenizer _cased = null!;
    private WordPieceTokenizer _uncased = null!;

    /// <summary>The flavour of text this row encodes.</summary>
    [Params(NormalizerText.Ascii, NormalizerText.Accented, NormalizerText.Cjk)]
    public NormalizerText Text { get; set; }

    /// <summary>Whether the vocabulary is the uncased one, which is what runs the accent strip.</summary>
    [Params(false, true)]
    public bool Lowercase { get; set; }

    [GlobalSetup]
    public void Setup()
    {
        string[] documents = JsonSerializer.Deserialize<string[]>(
            File.ReadAllBytes(BenchCorpus.Path("documents.json")))!;
        _texts[NormalizerText.Ascii] = documents;
        _texts[NormalizerText.Accented] =
            [.. documents.Select(d => d.Replace('e', 'é'))];
        _texts[NormalizerText.Cjk] = [.. documents.Select(d => d.Replace('a', '中'))];

        _cased = new WordPieceTokenizer(VocabTxtLoader.Load(BenchCorpus.Path("vocab_30k.txt"), false));
        _uncased = new WordPieceTokenizer(VocabTxtLoader.Load(BenchCorpus.Path("vocab_30k.txt"), true));
    }

    [Benchmark]
    public int Encode()
    {
        WordPieceTokenizer tokenizer = Lowercase ? _uncased : _cased;
        int total = 0;
        foreach (string text in _texts[Text])
        {
            total += tokenizer.Encode(text).Ids.Count;
        }
        return total;
    }
}
```

If `VocabTxtLoader.Load`'s second positional parameter is not `lowercase`, read its signature and
pass it by name.

- [ ] **Step 2: Register it, or the gate refuses it**

In `bench/bench-map.json`, add to `benchmarks`:

```json
  "BertNormalizerBenchmarks": [
   "src/Lodestar.Embeddings/Tokenization/**",
   "src/Lodestar.Embeddings/Persistence/**"
  ],
```

then:

```bash
python3 tools/check_bench_map.py
```

Expected: exit 0.

- [ ] **Step 3: Build it**

```bash
./.dotnet-guarded dotnet build bench/Lodestar.Text.Benchmarks -c Release
```

Expected: zero warnings. `CA1822` and the other bench-wide rules are already `NoWarn`ed in
`bench/Directory.Build.props`; a new rule surfacing here gets a reason, not a blanket suppression.

- [ ] **Step 4: Prepare both states outside the checkout**

**Neither side runs from the checkout.** BenchmarkDotNet resolves the benchmark project by name
under the working directory and refuses to build when it finds more than one — and a session's
`.claude/worktrees/` sit inside this one, each holding its own copy. It fails as
`NotSupportedException: Found more than one matching project file`, after printing a table of `NA`.
So both states get a worktree in `/var/tmp`, which also keeps the nested-worktree Sonar problem out
of the picture.

```bash
for side in before after; do
  git worktree add /var/tmp/lodestar-1048-$side 00774eb1
  ln -sfn "$REPO"/bench/corpus/vocabs /var/tmp/lodestar-1048-$side/bench/corpus/vocabs
  cp bench/Lodestar.Text.Benchmarks/BertNormalizerBenchmarks.cs /var/tmp/lodestar-1048-$side/bench/Lodestar.Text.Benchmarks/
done
cp src/Lodestar.Embeddings/Tokenization/BertBasicTokenization.cs \
   src/Lodestar.Embeddings/Tokenization/WordPieceTokenizer.cs \
   /var/tmp/lodestar-1048-after/src/Lodestar.Embeddings/Tokenization/
```

Both worktrees start at the same commit and differ in exactly the two files under measurement; the
corpus is a symlink because it is untracked, and the benchmark class is copied because it does not
exist on `main`.

- [ ] **Step 5: Alternate the runs, each under the machine lock**

Three rounds, before → after, after → before, before → after, each run wrapped so it holds the lock
for its own duration and nothing else can take the machine mid-run:

```bash
./.dotnet-guarded bash -c "cd /var/tmp/lodestar-1048-before && dotnet run -c Release --project bench/Lodestar.Text.Benchmarks -- --filter '*BertNormalizerBenchmarks*'"
./.dotnet-guarded bash -c "cd /var/tmp/lodestar-1048-after  && dotnet run -c Release --project bench/Lodestar.Text.Benchmarks -- --filter '*BertNormalizerBenchmarks*'"
```

Record the mean and the allocation of all six rows per run, with the one-minute load average at the
start of each. `acquire`/`release` around the whole campaign is the other legal shape, but it forces
every run inside it to call `dotnet` bare, which is worth avoiding.

- [ ] **Step 6: Write the numbers where numbers live**

Add a section to `docs/guides/performance.md` titled
`## BERT's basic tokenization without its second pass (issue #1048)`, in the shape of its
neighbours: the machine, the runtime, the window, the one-minute load average, all runs shown, and
a `before | after | change` table over the six rows. Add the how-to-measure half — the corpus, the
flavours, the command — as the next numbered section of `bench/README.md`.

- [ ] **Step 7: Judge the result before writing the claim**

The target from the spec is at or under #992's before-numbers on every case, with #992's
allocations. If a case is slower than #992's before-number, the pull request stays draft and the
finding goes back to the issue rather than into the prose.

- [ ] **Step 8: Remove the worktrees**

```bash
git worktree remove /var/tmp/lodestar-1048-before --force
git worktree remove /var/tmp/lodestar-1048-after --force
```

---

### Task 7: The CHANGELOG entries

**Files:**

- Modify: `CHANGELOG.md`, under `## [Unreleased]` → `### Lodestar.Embeddings`

- [ ] **Step 1: One sentence under `#### Changed`**

```text
- BERT's basic tokenization tells from its own loop whether it changed the text rather than comparing the result to the input, and reaches `string.Normalize` directly instead of scanning for the unassigned code points only .NET Framework refuses, which takes accented uncased text back under its pre-#992 time at #992's allocations; a word's code points are counted only once its UTF-16 length is over the cap. ([#1048](https://github.com/CyrilB1531/lodestar/issues/1048), [#1050](https://github.com/CyrilB1531/lodestar/issues/1050), [#1052](https://github.com/CyrilB1531/lodestar/issues/1052))
```

- [ ] **Step 2: One sentence under `#### Fixed`**

```text
- The `vocab.txt` route's record matches its behaviour: [decision 0146](docs/decisions/0146-berts-normalizer-keeps-the-unassigned-code-points.md) amends 0144's dropped-Cn clause, `docs/equivalence.md` credits the residue to the sweep that measured it and lists the four code points that diverge the other way, and the dead `CA1308` suppression on `Normalize` is gone. ([#1049](https://github.com/CyrilB1531/lodestar/issues/1049), [#1051](https://github.com/CyrilB1531/lodestar/issues/1051), [#1053](https://github.com/CyrilB1531/lodestar/issues/1053), [#1064](https://github.com/CyrilB1531/lodestar/issues/1064))
```

- [ ] **Step 2b: Check the shape**

Entries are one sentence, the issue, and the commit — nothing else. The commit hash is unknown
until Task 8, and the neighbours show both shapes; leave it off rather than invent one.

---

### Task 8: Gate, review, commit, push

- [ ] **Step 1: The whole build and the whole suite**

```bash
./.dotnet-guarded dotnet build Lodestar.slnx -c Release
./.dotnet-guarded dotnet test Lodestar.slnx -c Release
```

Expected: zero warnings; **36 assemblies** reported, and a test count at or above `main`'s 10 642
plus the cases added here. A count that dropped means a suite went missing, which has no exit code.

- [ ] **Step 2: Format and lint**

```bash
./.dotnet-guarded dotnet format Lodestar.slnx --verify-no-changes
npx markdownlint-cli2 "README.md" "CONTRIBUTING.md" "docs/**/*.md" "tools/README.md" "bench/README.md"
```

- [ ] **Step 3: Every check script, not a subset**

```bash
for t in tools/check_*.py; do echo "== $t"; python3 "$t" || echo "FAILED $t"; done
python3 tools/check_repeated_literals.py --base origin/main
```

Expected: every one exit 0. `check_doc_test_counts.py`, `check_claude_md_packages.py`,
`check_bench_map.py` and `check_adr_immutable.py` are the four this change can break.

- [ ] **Step 4: The doc snippets**

```bash
python3 tools/extract_doc_snippets.py
```

Expected: exit 0. No fence changed here, so no `pack` is needed unless the extractor reports one.

- [ ] **Step 5: The six-angle review pass over the touched files**

Read `src/Lodestar.Embeddings/Tokenization/BertBasicTokenization.cs`,
`src/Lodestar.Embeddings/Tokenization/WordPieceTokenizer.cs` and the three documents again for:
correctness against the reference, the `netstandard2.0` path, analyzer findings, comment claims
that are no longer true, the four documents agreeing with each other, and the numbers matching what
was measured. Fix findings in place; they belong to this pull request, not to new issues.

- [ ] **Step 6: One commit**

Add the files by name. This checkout is shared with other sessions — `git status` shows their
in-flight edits to `.github/workflows/`, `CLAUDE.md`, `CONTRIBUTING.md`, `tools/` and three
`Version.props` — and `git add -A` would sweep them into this commit:

```bash
git add src/Lodestar.Embeddings/Tokenization/BertBasicTokenization.cs \
        src/Lodestar.Embeddings/Tokenization/WordPieceTokenizer.cs \
        tests/Lodestar.Embeddings.Tests/Tokenization/BertNormalizerTests.cs \
        tests/oracles/vocab_txt.json tools/generate_oracles.py \
        bench/Lodestar.Text.Benchmarks/BertNormalizerBenchmarks.cs bench/bench-map.json bench/README.md \
        docs/equivalence.md docs/guides/performance.md CHANGELOG.md \
        docs/decisions/0146-berts-normalizer-keeps-the-unassigned-code-points.md \
        docs/decisions/index.yaml docs/decisions/README.md \
        docs/superpowers/specs/2026-09-18_1048_embeddings-normalizer-findings.md \
        docs/superpowers/plans/2026-09-18_1048_embeddings-normalizer-findings.md
git commit
```

Message: a subject line in the imperative with no `feat:`/`fix:` prefix, the seven `Closes #…`
lines, and the `Co-Authored-By` trailer.

- [ ] **Step 7: Sync and open the pull request**

```bash
git fetch origin && git rebase origin/main
./.dotnet-guarded dotnet test tests/Lodestar.Embeddings.Tests -c Release
git push -u origin fix/1048-embeddings-normalizer-findings
gh pr create --fill --milestone "Next release"
```

A clean rebase can still break a JSON file — re-run `tools/check_bench_map.py` and the oracle
replay after it. The pull request opens as a draft if any number is still missing, and carries the
before/after table and the machine in its body.
