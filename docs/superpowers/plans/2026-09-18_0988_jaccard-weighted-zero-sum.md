# Weighted `JaccardScore` zero-sum refusal — implementation plan

> **For agentic workers:** REQUIRED SUB-SKILL: Use superpowers:subagent-driven-development
> (recommended) or superpowers:executing-plans to implement this plan task-by-task. Steps use
> checkbox (`- [ ]`) syntax for tracking.

**Goal:** `JaccardScore.Score(average: Averaging.Weighted)` refuses a support total of zero the
way `jaccard_score` does, instead of answering the unweighted mean the three sibling metrics
correctly answer.

**Architecture:** `Prf.Aggregate` gains one Jaccard-only branch for `Averaging.Weighted`. The
shared accumulation (skip NaN classes, total, weighted total, weight sum, defined count) moves
into a private `Accumulate` so the existing `_nanaverage` rule and the new `jaccard_score` rule
read the same numbers and neither duplicates the loop. `Prf.Average` keeps its behaviour
unchanged for precision, recall and F-beta.

**Tech Stack:** C# on `net10.0;netstandard2.0`, xunit v3 on Microsoft.Testing.Platform,
scikit-learn 1.9.0 / numpy 2.5.3 in `.venv-oracles` for the oracle corpus.

**Spec:** [`docs/superpowers/specs/2026-09-18_0988_jaccard-weighted-zero-sum.md`](../specs/2026-09-18_0988_jaccard-weighted-zero-sum.md)

**Branch:** `fix/988-jaccard-weighted-zero-sum`, in the worktree
a sibling of the main checkout, from `origin/main` at `af19b0de`.

## Global Constraints

- One commit for the pull request; `Closes #988` in the body, no `fix:` prefix on the subject.
- Everything in English — code, comments, oracle keys, CHANGELOG, PR body.
- No `ProjectReference` under `src/`; nothing outside `Lodestar.Metrics` is edited.
- Comments say why, not what: two lines inline, eight of prose in XML documentation.
- The refusal wording is fixed: `ArgumentException`,
  `"Weights sum to zero, can't be normalized."`, `paramName` `"sampleWeight"` — produced by
  `Weights.RequireNonZeroSum`, never written a second time.
- `#pragma warning disable S1244` guards every exact float comparison, with the reason above it.
- The oracle generator runs from a neutral working directory that is not an ancestor of the
  checkout (`/var/tmp`), and its own exit code is read, never a pipeline's.
- `docs/equivalence.md` gets its row in this same commit, not afterwards.

---

### Task 1: The reference values

**Files:**

- Modify: `tools/generate_oracles.py:3031-3090` (`_undefined_average_fixtures`,
  `_undefined_average_case`)
- Modify: `tests/oracles/classification_metrics.json` (regenerated, not hand-edited)

**Interfaces:**

- Produces: a sixth `undefined_averages` case, `"fixture": "supports_cancel"`, whose
  `scores["weighted|0"]["jaccard"]` and `scores["weighted|1"]["jaccard"]` are the string
  `"ZeroDivisionError"` and whose other entries are numbers, as every other case's are.

- [ ] **Step 1: Add the fixture**

In `_undefined_average_fixtures`, after the `zero_total_support` entry:

```python
        {"name": "supports_cancel", "y_true": [0, 0, 1, 1], "y_pred": [0, 1, 1, 0],
         "labels": None, "sample_weight": [1.0, 1.0, -1.0, -1.0]},
```

And extend that function's docstring, which currently ends on the zero-weight sentence, with
the reason this one exists:

```python
    A negative sample weight is what reaches a support total of zero while the
    supports themselves are not zero, which is the one input where jaccard_score
    and precision_recall_fscore_support disagree (#988).
```

- [ ] **Step 2: Record the refusal instead of a value**

In `_undefined_average_case`, replace the single `entry[JACCARD] = …` assignment with the
call that survives the reference raising:

```python
            entry[JACCARD] = None if math.isnan(zd) else _jaccard_or_refusal(
                y_true, y_pred, **kw)
```

and add the helper above `_undefined_average_case`:

```python
def _jaccard_or_refusal(y_true, y_pred, **kw) -> float | str:
    """The weighted coefficient, or the name of the error the reference raises.

    jaccard_score averages through numpy.average, which refuses a weight total
    of zero; precision_recall_fscore_support reaches the same total through
    _nanaverage, which catches that error and answers the unweighted mean. The
    corpus has to carry the refusal, because it is the divergence under test (#988).
    """
    try:
        return _finite_or_name(skm.jaccard_score(y_true, y_pred, **kw))
    except ZeroDivisionError:
        return "ZeroDivisionError"
```

- [ ] **Step 3: Regenerate the corpus**

From a neutral directory, writing only the corpus this task touches — the full generator
rewrites every file and takes far longer than this change is worth:

```bash
cd /var/tmp && PYTHONSAFEPATH=1 <repo>/.venv-oracles/bin/python - <<'PY'
import json, sys
sys.path.insert(0, "<worktree>/tools")
import generate_oracles as g
payload = g.generate_classification_metrics()
path = g.ORACLE_DIR / "classification_metrics.json"
with path.open("w", encoding="utf-8", newline="\n") as f:
    json.dump(payload, f, ensure_ascii=False, indent=1, allow_nan=False)
    f.write("\n")
print(f"{path}: {payload['metadata']['count']} cases")
PY
echo "exit: $?"
```

Expected: `exit: 0`, and `git diff --stat
tests/oracles/classification_metrics.json` shows added lines only.

- [ ] **Step 4: Check what was written**

```bash
python3 -c "
import json
c = [x for x in json.load(open('tests/oracles/classification_metrics.json'))['undefined_averages'] if x['fixture'] == 'supports_cancel'][0]
for k in ('macro|0', 'weighted|0', 'weighted|1', 'weighted|nan'):
    print(k, c['scores'][k]['jaccard'], c['scores'][k]['precision'], c['scores'][k]['recall'])
"
```

Expected: `weighted|0` and `weighted|1` print `ZeroDivisionError`, `weighted|nan` prints `None`,
`macro|0` prints a number, and the precision and recall columns print numbers throughout.

---

### Task 2: The failing test

**Files:**

- Modify: `tests/Lodestar.Metrics.Tests/UndefinedAverageTests.cs:40-47`

**Interfaces:**

- Consumes: Task 1's `"ZeroDivisionError"` string in the `jaccard` slot.

- [ ] **Step 1: Read the refusal out of the corpus**

Replace the `if (want.GetProperty("jaccard").ValueKind != JsonValueKind.Null)` block with:

```csharp
            // jaccard_score refuses zero_division=nan, so that mode has no reference
            // value; a weighted average whose supports cancel has a refusal instead (#988).
            JsonElement jaccard = want.GetProperty("jaccard");
            if (jaccard.ValueKind == JsonValueKind.String && jaccard.GetString() == Refusal)
            {
                ArgumentException error = Assert.Throws<ArgumentException>(() =>
                    JaccardScore.Score(yTrue, yPred, average, zeroDivision: zero, labels: labels, sampleWeight: sampleWeight));
                Assert.StartsWith("Weights sum to zero", error.Message, StringComparison.Ordinal);
                Assert.Equal("sampleWeight", error.ParamName);
            }
            else if (jaccard.ValueKind != JsonValueKind.Null)
            {
                AssertClose(want, "jaccard",
                    JaccardScore.Score(yTrue, yPred, average, zeroDivision: zero, labels: labels, sampleWeight: sampleWeight),
                    what);
            }
```

and add the constant beside the class's other members, above `Scores_match_sklearn`:

```csharp
    /// <summary>What the corpus holds where the reference raises rather than scoring.</summary>
    private const string Refusal = "ZeroDivisionError";
```

- [ ] **Step 2: Run it and watch it fail**

```bash
dotnet test tests/Lodestar.Metrics.Tests -c Release --filter "FullyQualifiedName~UndefinedAverageTests"
```

Expected: FAIL, `Assert.Throws() Failure: No exception was thrown`, on the `supports_cancel`
case. Read the test count, not the colour: the run must report more than zero tests.

---

### Task 3: The fix

**Files:**

- Modify: `src/Lodestar.Metrics/Internal/Prf.cs:131-196`

**Interfaces:**

- Produces: `Prf.Aggregate` unchanged in signature; `Prf.Average` unchanged in signature and
  behaviour; `Accumulate` and `JaccardWeighted` private to `Prf`.

- [ ] **Step 1: Take the Jaccard branch in `Aggregate`**

In `Prf.Aggregate`, immediately after `double[] perClass = PerClass(cm, metric, beta,
zeroDivision, out double[] support);`, before the `switch (average)`:

```csharp
        // jaccard_score averages through numpy.average and the other three through
        // _nanaverage, which catches its zero-sum error (#988 corrects #861).
        if (average == Averaging.Weighted && metric == PrfMetric.Jaccard)
        {
            return JaccardWeighted(perClass, support);
        }
```

- [ ] **Step 2: Share the accumulation**

Replace the body of `Average` with a call to a new private `Accumulate`, leaving its
`<summary>`, `<remarks>` and signature as they are apart from the remarks correction in Step 4:

```csharp
    public static double Average(double[] perClass, double[] support, Averaging average)
    {
        Accumulate(perClass, support, out double total, out double weighted, out double weightSum, out int defined);

        if (defined == 0)
        {
            return double.NaN;
        }

        // SonarLint S1244: an exact zero is numpy.average's own ZeroDivisionError
        // test, and a tolerance would drop the weights of a small real support.
#pragma warning disable S1244
        return average == Averaging.Macro || weightSum == 0.0 ? total / defined : weighted / weightSum;
#pragma warning restore S1244
    }

    /// <summary>The four running totals both averaging rules read, over the defined classes alone.</summary>
    private static void Accumulate(
        double[] perClass, double[] support, out double total, out double weighted, out double weightSum, out int defined)
    {
        total = 0.0;
        weighted = 0.0;
        weightSum = 0.0;
        defined = 0;
        for (int i = 0; i < perClass.Length; i++)
        {
            double value = perClass[i];
            if (double.IsNaN(value))
            {
                continue;
            }

            defined++;
            total += value;
            weighted += value * support[i];
            weightSum += support[i];
        }
    }
```

- [ ] **Step 3: Write `JaccardWeighted`**

Below `Average`:

```csharp
    /// <summary>
    /// The support-weighted mean for Jaccard — <c>jaccard_score</c>'s own averaging,
    /// which is <c>numpy.average</c> where its three siblings use <c>_nanaverage</c>.
    /// </summary>
    /// <remarks>
    /// The reference drops its weights on <c>not numpy.any(weights)</c>, so only supports that
    /// are every one zero fall back to the unweighted mean; a set that merely sums to zero keeps
    /// them and raises, where <c>_nanaverage</c> catches that error. Only a negative sample weight
    /// reaches the refusal: non-negative weights make every support non-negative (#988).
    /// </remarks>
    /// <exception cref="ArgumentException">The supports sum to zero without all being zero.</exception>
    private static double JaccardWeighted(double[] perClass, double[] support)
    {
        Accumulate(perClass, support, out double total, out double weighted, out double weightSum, out int defined);

        if (defined == 0)
        {
            return double.NaN;
        }

        if (!AnyNonZero(support))
        {
            return total / defined;
        }

        Weights.RequireNonZeroSum(weightSum, "sampleWeight");
        return weighted / weightSum;
    }

    /// <summary><c>numpy.any</c> over the supports: is a single one of them not zero.</summary>
    private static bool AnyNonZero(double[] support) =>
        // SonarLint S1244: numpy.any tests each element against zero exactly, and a
        // tolerance would drop the weights of a support the reference keeps.
#pragma warning disable S1244
        Array.Exists(support, weight => weight != 0.0);
#pragma warning restore S1244
```

- [ ] **Step 4: Correct the sentence #861 got wrong**

In `Average`'s `<remarks>`, replace

```csharp
    /// sum to zero once those are gone fall back to the unweighted mean, as
    /// <c>jaccard_score</c> also does by dropping its weights (#861).
```

with

```csharp
    /// sum to zero once those are gone fall back to the unweighted mean, because
    /// <c>_nanaverage</c> catches <c>numpy.average</c>'s refusal; <see cref="JaccardWeighted"/> does not (#861, #988).
```

- [ ] **Step 5: Run the test and watch it pass**

```bash
dotnet test tests/Lodestar.Metrics.Tests -c Release --filter "FullyQualifiedName~UndefinedAverageTests"
```

Expected: PASS, with the same test count Step 2 of Task 2 reported.

- [ ] **Step 6: Run the whole package, both frameworks**

```bash
dotnet test tests/Lodestar.Metrics.Tests tests/Lodestar.Metrics.NetStandard.Tests -c Release
```

Expected: both assemblies green, and the netstandard mirror reports more tests than its suite
by its `NetStandardAssemblyGuardTests` count.

---

### Task 4: The documentation the gates read

**Files:**

- Modify: `src/Lodestar.Metrics/JaccardScore.cs:25` (the `Score` overload's `<exception>`)
- Modify: `docs/reference/metrics/classification/jaccardscore-score.md` (**Exceptions**)
- Modify: `docs/equivalence.md:216`
- Modify: `CHANGELOG.md`

- [ ] **Step 1: The XML documentation**

On `JaccardScore.Score` only — `PerClass` never averages and keeps its line — replace the
`<exception cref="ArgumentException">` text with:

```csharp
    /// <exception cref="ArgumentException">The inputs disagree in length; the weights do not match, hold a non-finite value or are zero throughout; or under <see cref="Averaging.Weighted"/> the class supports sum to zero without all being zero.</exception>
```

- [ ] **Step 2: The reference page**

In `jaccardscore-score.md`, under **Exceptions**, extend the first sentence so the signature
and the page agree:

```markdown
**Exceptions** — `ArgumentException` when the inputs disagree in length or the weights do not match, hold a non-finite value or are zero throughout; when `average` is `Averaging.Binary` and
`posLabel` occurs in neither input, which is the refusal `Precision.Score` already makes; and
when `average` is `Averaging.Weighted` and the class supports sum to zero without all being
zero, which only a negative weight reaches and which `jaccard_score` refuses in the same words.
```

- [ ] **Step 3: The equivalence row**

Append to the `jaccard_score` row's notes, before the final `|`:

```markdown
 **A weighted average whose supports cancel is refused**, where the three sibling metrics answer the unweighted mean: `jaccard_score` drops its weights only when *every* support is zero (`numpy.any`) and otherwise reaches `numpy.average`, which raises `ZeroDivisionError`, while `precision_recall_fscore_support` catches that same error in `_nanaverage`. `ArgumentException` in numpy's own sentence here, as everywhere else that call refuses. Only a negative `sample_weight` reaches it — measured, `jaccard_score([0,0,1,1], [0,1,1,0], sample_weight=[1,1,-1,-1], average='weighted')` (#988).
```

- [ ] **Step 4: The changelog**

Under the unreleased `Lodestar.Metrics` heading, in the shape the file already uses — one
sentence, the issue, the commit:

```markdown

- Weighted `JaccardScore.Score` refuses class supports that sum to zero without all being zero, as `jaccard_score` does, instead of answering the unweighted mean its three sibling metrics correctly answer (#988).
```

- [ ] **Step 5: Compile and run the documentation fences**

```bash
python3 tools/extract_doc_snippets.py && dotnet build samples/Lodestar.DocSnippets -c Release
```

Expected: both exit 0. The page's `// =>` assertion on `0.6666…` is unchanged by this work and
must still hold.

---

### Task 5: The gates, then the commit

- [ ] **Step 1: Every check script, not a subset**

```bash
for s in tools/check_*.py; do echo "== $s"; python3 "$s" || echo "FAILED $s"; done
```

Expected: no `FAILED` line. `check_repeated_literals.py` needs `--base origin/main`:

```bash
python3 tools/check_repeated_literals.py --base origin/main
```

- [ ] **Step 2: Format and lint**

```bash
dotnet format Lodestar.slnx --verify-no-changes && npx markdownlint-cli2 "docs/**/*.md" "CONTRIBUTING.md" "README.md"
```

- [ ] **Step 3: The whole suite, from outside the checkout**

Nested worktrees silence Sonar rules, so build the solution from its own root, which
a sibling of the main checkout is:

```bash
dotnet test Lodestar.slnx -c Release 2>&1 | tail -40
```

Expected: **36 assemblies**, eighteen suites and their eighteen mirrors. A count below that is
a suite that went missing, which has no exit code.

- [ ] **Step 4: Commit**

```bash
git add -A && git commit
```

Subject, no type prefix: `Refuse a weighted Jaccard whose supports cancel, as jaccard_score does`.
Body: the two averaging helpers, the one input that separates them, `Closes #988`, and the
`Co-Authored-By` trailer.
