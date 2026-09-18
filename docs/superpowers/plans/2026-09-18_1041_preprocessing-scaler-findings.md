# Five scaler findings in Lodestar.Preprocessing Implementation Plan

> **For agentic workers:** REQUIRED SUB-SKILL: Use superpowers:subagent-driven-development (recommended) or superpowers:executing-plans to implement this plan task-by-task. Steps use checkbox (`- [ ]`) syntax for tracking.

**Goal:** Make `StandardScaler` refuse a non-finite value wherever its three neighbours do, and make the sparse column statistics read a column stored twice in a row the way `CsrMatrix.ToDense` and scikit-learn read it.

**Architecture:** One new internal helper, `SparseColumns.Consolidated`, sums a row's duplicate column entries and is called by the three statistics helpers; six `RequireFinite` calls are added to `StandardScaler`; the six sparse XML `<exception>` tags, four reference pages and two `docs/equivalence.md` rows are corrected to match.

**Tech Stack:** C# on `net10.0;netstandard2.0`, xunit v3 on Microsoft.Testing.Platform, SonarAnalyzer.CSharp at `AnalysisMode=All`, scikit-learn 1.9.0 as the oracle.

**Spec:** [`docs/superpowers/specs/2026-09-18_1041_preprocessing-scaler-findings.md`](../specs/2026-09-18_1041_preprocessing-scaler-findings.md)

**Branch:** `fix/1041-preprocessing-scaler-findings`, off `origin/main` at `04882224`.

## Global Constraints

- One commit for the whole pull request, `Closes #1041`, `Closes #1042`, `Closes #1044`, `Closes #1045`, `Closes #1046`.
- Both target frameworks: everything added compiles and runs on `netstandard2.0` too. No `#if` at a call site; `src/Shared/` carries the polyfills.
- No version number moves: `src/Lodestar.Preprocessing/Version.props` is not touched.
- No new public type or method, so no reference-gate entry and no `samples/Lodestar.Sample` change.
- Warnings are errors and the Sonar analyzers gate the build; a suppression carries a reason a reviewer can disagree with.
- Comments follow the four rules: why not what, two lines inline, eight of prose in XML.
- Every `dotnet` command runs through `./.dotnet-guarded`.
- `docs/equivalence.md` rows land in this commit, not afterwards.
- `CHANGELOG.md` gets one sentence with its issue and commit.

---

### Task 1: Sparse column statistics sum a row's duplicate entries

**Files:**

- Modify: `src/Lodestar.Preprocessing/Internal/SparseColumns.cs` (add `Consolidated`; call it from `Moments`, `MaximumAbsolute`, `ByColumn`)
- Test: `tests/Lodestar.Preprocessing.Tests/PartialFitAndSparseEdgeTests.cs`

**Interfaces:**

- Consumes: `CsrMatrix` from `Lodestar.Abstractions` — `RowCount`, `ColumnCount`, `Values`, `ColumnIndices`, `RowPointers`, `ToDense()`.
- Produces: `internal static CsrMatrix SparseColumns.Consolidated(CsrMatrix matrix)`, returning `matrix` itself when no row stores a column twice.

- [ ] **Step 1: Write the failing tests**

Append to `tests/Lodestar.Preprocessing.Tests/PartialFitAndSparseEdgeTests.cs`, inside the class:

```csharp
    /// <summary>
    /// A column stored twice in one row is the sum of its entries, as scipy's reductions read it
    /// after <c>sum_duplicates</c>: scikit-learn answers <c>scale_ = [8]</c> on this matrix, where
    /// this answered <c>[5]</c> (#1044). <c>CsrMatrix.ToDense</c> agrees from 0.1.2 on (#878), which
    /// is above the floor this package builds against, so only the scalers are asserted here.
    /// </summary>
    [Fact]
    public void A_column_stored_twice_in_a_row_counts_as_the_sum_of_its_entries()
    {
        CsrMatrix duplicated = Duplicated();

        Assert.Equal(8.0, MaxAbsScaler.Fit(duplicated).MaximumAbsolute[0]);
        Assert.Equal(8.0, MaxAbsScaler.Fit(duplicated).Scale[0]);
        Assert.Equal(5.0, StandardScaler.Fit(duplicated).Mean![0]);
    }

    /// <summary>
    /// The buffer a percentile is read over is one slot per row, so a column carrying more values
    /// than there are rows had no percentile to read: it raised <c>destinationArray</c>, an
    /// internal buffer name reaching the caller (#1045). Summing first is what makes it a column.
    /// </summary>
    [Fact]
    public void A_column_stored_twice_in_a_row_no_longer_overruns_the_percentile_buffer()
    {
        RobustScaler scaler = RobustScaler.Fit(Duplicated());

        // The column is 8 and 2, whose quartiles by linear interpolation are 3.5 and 6.5.
        Assert.Equal(3.0, scaler.Scale![0], 12);
    }

    /// <summary>A matrix storing each column once per row is the one the statistics already read.</summary>
    [Fact]
    public void A_matrix_with_no_duplicate_is_read_exactly_as_before()
    {
        var plain = new CsrMatrix(2, 2, [1.0, 2.0, 3.0, 4.0], [0, 1, 0, 1], [0, 2, 4]);

        Assert.Equal(3.0, MaxAbsScaler.Fit(plain).Scale[0]);
        Assert.Equal(4.0, MaxAbsScaler.Fit(plain).Scale[1]);
        Assert.Equal(2.0, StandardScaler.Fit(plain).Mean![0]);
        Assert.Equal(1.0, RobustScaler.Fit(plain).Scale![0], 12);
    }

    /// <summary>Two rows, one column, whose only column is stored twice in the first row.</summary>
    private static CsrMatrix Duplicated() => new(2, 1, [3.0, 5.0, 2.0], [0, 0, 0], [0, 2, 3]);
```

- [ ] **Step 2: Run them to verify they fail**

```bash
./.dotnet-guarded dotnet test tests/Lodestar.Preprocessing.Tests -c Release --filter "FullyQualifiedName~PartialFitAndSparseEdgeTests"
```

Expected: the first fails with `Actual: 5` against an expected `8`, and the second with `ArgumentException: Destination array was not long enough… (Parameter 'destinationArray')`. The third passes already. Read the count, not the colour: three tests must run.

- [ ] **Step 3: Add the consolidation helper**

In `src/Lodestar.Preprocessing/Internal/SparseColumns.cs`, add after `Moments`:

```csharp
    /// <summary>The matrix with each row storing every column once, a row's duplicates summed.</summary>
    /// <remarks>
    /// Returns <paramref name="matrix"/> itself when no row stores a column twice, which is every
    /// matrix this repository builds. The statistics here are scikit-learn's, read through scipy
    /// reductions that call <c>sum_duplicates()</c> first, so they must read what
    /// <see cref="CsrMatrix.ToDense"/> reads rather than each stored entry on its own (#1044).
    /// </remarks>
    public static CsrMatrix Consolidated(CsrMatrix matrix)
    {
        var mark = new int[matrix.ColumnCount];
        ResetMarks(mark);
        if (!HasDuplicate(matrix, mark))
        {
            return matrix;
        }

        ResetMarks(mark);
        var totals = new double[matrix.ColumnCount];
        var values = new List<double>(matrix.Values.Length);
        var columns = new List<int>(matrix.Values.Length);
        var pointers = new int[matrix.RowCount + 1];
        var present = new List<int>();

        for (int row = 0; row < matrix.RowCount; row++)
        {
            present.Clear();
            for (int i = matrix.RowPointers[row]; i < matrix.RowPointers[row + 1]; i++)
            {
                int column = matrix.ColumnIndices[i];
                if (mark[column] != row)
                {
                    mark[column] = row;
                    totals[column] = 0.0;
                    present.Add(column);
                }

                totals[column] += matrix.Values[i];
            }

            present.Sort();
            foreach (int column in present)
            {
                columns.Add(column);
                values.Add(totals[column]);
            }

            pointers[row + 1] = values.Count;
        }

        return new CsrMatrix(matrix.RowCount, matrix.ColumnCount, [.. values], [.. columns], pointers);
    }

    /// <summary>Marks every column as seen in no row, which row 0 would otherwise look like.</summary>
    private static void ResetMarks(int[] mark)
    {
        for (int column = 0; column < mark.Length; column++)
        {
            mark[column] = -1;
        }
    }

    /// <summary>Whether any row stores one column twice, in one pass and with no allocation of its own.</summary>
    private static bool HasDuplicate(CsrMatrix matrix, int[] mark)
    {
        for (int row = 0; row < matrix.RowCount; row++)
        {
            for (int i = matrix.RowPointers[row]; i < matrix.RowPointers[row + 1]; i++)
            {
                int column = matrix.ColumnIndices[i];
                if (mark[column] == row)
                {
                    return true;
                }

                mark[column] = row;
            }
        }

        return false;
    }
```

- [ ] **Step 4: Call it from the three statistics helpers**

In the same file, make each of the three read the consolidated matrix first. `Moments` becomes:

```csharp
    public static (double[] Sums, double[] Squares) Moments(CsrMatrix matrix)
    {
        CsrMatrix read = Consolidated(matrix);
        var sums = new double[read.ColumnCount];
        var squares = new double[read.ColumnCount];
        for (int i = 0; i < read.Values.Length; i++)
        {
            int column = read.ColumnIndices[i];
            double value = read.Values[i];
            sums[column] += value;
            squares[column] += value * value;
        }

        return (sums, squares);
    }
```

`MaximumAbsolute` becomes:

```csharp
    public static double[] MaximumAbsolute(CsrMatrix matrix)
    {
        CsrMatrix read = Consolidated(matrix);
        var maxima = new double[read.ColumnCount];
        for (int i = 0; i < read.Values.Length; i++)
        {
            int column = read.ColumnIndices[i];
            double value = Math.Abs(read.Values[i]);
            if (value > maxima[column])
            {
                maxima[column] = value;
            }
        }

        return maxima;
    }
```

`ByColumn` becomes:

```csharp
    public static (double[] Values, int[] Offsets) ByColumn(CsrMatrix matrix)
    {
        CsrMatrix read = Consolidated(matrix);
        var offsets = new int[read.ColumnCount + 1];
        for (int i = 0; i < read.ColumnIndices.Length; i++)
        {
            offsets[read.ColumnIndices[i] + 1]++;
        }

        for (int column = 0; column < read.ColumnCount; column++)
        {
            offsets[column + 1] += offsets[column];
        }

        var values = new double[read.Values.Length];
        var next = (int[])offsets.Clone();
        for (int i = 0; i < read.Values.Length; i++)
        {
            values[next[read.ColumnIndices[i]]++] = read.Values[i];
        }

        return (values, offsets);
    }
```

- [ ] **Step 5: State the invariant `SortedColumn` now relies on**

Replace `SortedColumn`'s `<remarks>` with:

```csharp
    /// <remarks>
    /// The buffer receives the column's stored values in storage order and zeros after them — the
    /// same sequence a scan of the whole matrix built — so the sort returns the same array. It is
    /// one slot per row, which fits because <see cref="ByColumn"/> consolidated the matrix first:
    /// a column is stored at most once per row, so it cannot hold more values than there are rows
    /// (#1045).
    /// </remarks>
```

- [ ] **Step 6: Run the tests to verify they pass**

```bash
./.dotnet-guarded dotnet test tests/Lodestar.Preprocessing.Tests -c Release --filter "FullyQualifiedName~PartialFitAndSparseEdgeTests"
```

Expected: every test in the class passes, and the count is the class's previous count plus three.

---

### Task 2: StandardScaler refuses a non-finite value on all six paths

**Files:**

- Modify: `src/Lodestar.Preprocessing/StandardScaler.cs` — `Fit(ReadOnlySpan<double>, int, …)` at line 81, `PartialFit` at line 175, `Apply` at line 263, `Transform(CsrMatrix)` at line 215, `InverseTransform(CsrMatrix)` at line 238
- Test: `tests/Lodestar.Preprocessing.Tests/StandardScalerEdgeTests.cs`

**Interfaces:**

- Consumes: `SampleMatrix.RequireFinite(ReadOnlySpan<double> samples, string parameterName)` and `SparseColumns.Divided`/`Multiplied`'s `bool requireFinite` argument, both already in the package.
- Produces: no new signature; six call sites change behaviour from silent propagation to `ArgumentException` naming `samples`.

- [ ] **Step 1: Write the failing tests**

Append to `tests/Lodestar.Preprocessing.Tests/StandardScalerEdgeTests.cs`, inside the class:

```csharp
    /// <summary>
    /// The dense fit was the one entry point of the four scalers that answered a statistic for a
    /// value no statistic can answer for: it returned <c>Scale = [4.04e8, NaN]</c> and poisoned
    /// every later transform of that feature, where scikit-learn nan-skips (#1042).
    /// </summary>
    [Fact]
    public void The_dense_fit_refuses_a_non_finite_value()
    {
        double[] withNan = [-1.2216917e9, 1.0, 2.0, 2.0, 3.0, double.NaN, 4.0, 4.0];
        double[] withInfinity = [-1.2216917e9, 1.0, 2.0, 2.0, 3.0, double.PositiveInfinity, 4.0, 4.0];

        Assert.Equal("samples", Assert.Throws<ArgumentException>(() => StandardScaler.Fit(withNan, 2)).ParamName);
        Assert.Equal("samples", Assert.Throws<ArgumentException>(() => StandardScaler.Fit(withInfinity, 2)).ParamName);
    }

    /// <summary>
    /// <c>partial_fit</c> folds a batch into the same statistics, so it refuses the same batch:
    /// the page already promised "these scalers refuse a non-finite value" (#1042).
    /// </summary>
    [Fact]
    public void Partial_fit_refuses_a_non_finite_batch()
    {
        StandardScaler fitted = StandardScaler.Fit([1.0, 10.0, 2.0, 20.0], 2);

        Assert.Throws<ArgumentException>(() => fitted.PartialFit([3.0, double.NaN]));
        Assert.Throws<ArgumentException>(() => fitted.PartialFit([3.0, double.NegativeInfinity]));
    }

    /// <summary>
    /// Refusing on the way in and not on the way through is the same contradiction one step later:
    /// the dense transform returned <c>-1, NaN, 1, 1</c> where its three neighbours refuse.
    /// </summary>
    [Fact]
    public void The_dense_transform_and_its_inverse_refuse_a_non_finite_value()
    {
        StandardScaler fitted = StandardScaler.Fit([1.0, 10.0, 2.0, 20.0], 2);

        Assert.Throws<ArgumentException>(() => fitted.Transform([1.0, double.NaN]));
        Assert.Throws<ArgumentException>(() => fitted.InverseTransform([1.0, double.PositiveInfinity]));
    }

    /// <summary>
    /// The sparse transform passed both through where <c>MaxAbsScaler</c> and <c>RobustScaler</c>
    /// refuse both, which is the contradiction the equivalence row stated as a fact (#1041).
    /// </summary>
    [Fact]
    public void The_sparse_transform_and_its_inverse_refuse_a_stored_non_finite_value()
    {
        StandardScaler fitted = StandardScaler.Fit(
            new CsrMatrix(2, 2, [1.0, 2.0, 3.0, 4.0], [0, 1, 0, 1], [0, 2, 4]));
        var withNan = new CsrMatrix(2, 2, [1.0, double.NaN], [0, 1], [0, 1, 2]);
        var withInfinity = new CsrMatrix(2, 2, [1.0, double.PositiveInfinity], [0, 1], [0, 1, 2]);

        Assert.Equal("samples", Assert.Throws<ArgumentException>(() => fitted.Transform(withNan)).ParamName);
        Assert.Equal("samples", Assert.Throws<ArgumentException>(() => fitted.Transform(withInfinity)).ParamName);
        Assert.Throws<ArgumentException>(() => fitted.InverseTransform(withNan));
        Assert.Throws<ArgumentException>(() => fitted.InverseTransform(withInfinity));
    }
```

If `StandardScalerEdgeTests.cs` does not already have `using Lodestar.Abstractions;`, add it at the top of the file.

- [ ] **Step 2: Run them to verify they fail**

```bash
./.dotnet-guarded dotnet test tests/Lodestar.Preprocessing.Tests -c Release --filter "FullyQualifiedName~StandardScalerEdgeTests"
```

Expected: all four fail — the three dense ones with `Assert.Throws() Failure: No exception was thrown`, the sparse one likewise.

- [ ] **Step 3: Add the check to the dense fit**

In `src/Lodestar.Preprocessing/StandardScaler.cs`, in `Fit(ReadOnlySpan<double> samples, int featureCount, StandardScalerOptions? options = null)`, after `int sampleCount = SampleMatrix.Rows(samples, featureCount);`:

```csharp
        SampleMatrix.RequireFinite(samples, nameof(samples));
```

- [ ] **Step 4: Add the check to `PartialFit` and to `Apply`**

In `PartialFit`, after `int rows = SampleMatrix.Rows(samples, FeatureCount);`:

```csharp
        SampleMatrix.RequireFinite(samples, nameof(samples));
```

In `Apply`, after `SampleMatrix.Rows(samples, FeatureCount);`:

```csharp
        SampleMatrix.RequireFinite(samples, nameof(samples));
```

- [ ] **Step 5: Turn the two sparse flags on**

In `Transform(CsrMatrix samples)` replace the argument:

```csharp
        return SparseColumns.Divided(samples, FeatureCount, _scale, requireFinite: true);
```

In `InverseTransform(CsrMatrix samples)`:

```csharp
        return SparseColumns.Multiplied(samples, FeatureCount, _scale, requireFinite: true);
```

- [ ] **Step 6: Run the tests to verify they pass**

```bash
./.dotnet-guarded dotnet test tests/Lodestar.Preprocessing.Tests -c Release
```

Expected: every test in the project passes. A frozen-corpus failure here would mean a corpus carries a non-finite value; there is none, so investigate rather than relax the check.

---

### Task 3: The six XML exception tags, four reference pages and two equivalence rows

**Files:**

- Modify: `src/Lodestar.Preprocessing/StandardScaler.cs`, `src/Lodestar.Preprocessing/MaxAbsScaler.cs`, `src/Lodestar.Preprocessing/RobustScaler.cs` — the six sparse `Transform`/`InverseTransform` `<exception>` tags
- Modify: `docs/reference/preprocessing/scaling/standardscaler-fit.md`, `standardscaler-partialfit.md`, `standardscaler-transform.md`, `standardscaler-inversetransform.md`
- Modify: `docs/reference/abstractions/sparse/csrmatrix.md`
- Modify: `docs/equivalence.md`

**Interfaces:**

- Consumes: the behaviour Tasks 1 and 2 landed.
- Produces: nothing code depends on; the reference gate and the doc-snippets gate read these files.

- [ ] **Step 1: Add the no-row refusal to `MaxAbsScaler`'s and `RobustScaler`'s four tags**

In `MaxAbsScaler.cs` (lines 154 and 200) and `RobustScaler.cs` (lines 226 and 252), replace each of the four with:

```csharp
    /// <exception cref="ArgumentException"><paramref name="samples"/> holds no row, has another column count, or stores a non-finite value.</exception>
```

- [ ] **Step 2: Rewrite `StandardScaler`'s two tags**

In `StandardScaler.cs`, replace line 207 and line 234 — each currently reading `<paramref name="samples"/> has another column count.` — with:

```csharp
    /// <exception cref="ArgumentException"><paramref name="samples"/> holds no row, has another column count, or stores a non-finite value.</exception>
```

- [ ] **Step 3: Add the dense refusal to `StandardScaler`'s four dense tags**

In `StandardScaler.cs`, the dense `Fit`, `PartialFit`, `Transform(ReadOnlySpan<double>)` and `InverseTransform(ReadOnlySpan<double>)` each carry
`<exception cref="ArgumentException"><paramref name="samples"/> holds no row, or a partial one.</exception>`. Replace each with:

```csharp
    /// <exception cref="ArgumentException"><paramref name="samples"/> holds no row, a partial one, or a non-finite value.</exception>
```

- [ ] **Step 4: Correct the four StandardScaler reference pages**

In `docs/reference/preprocessing/scaling/standardscaler-fit.md`, replace the three lines of the
**Exceptions** paragraph that begin "ArgumentException when samples holds no row" with:

```markdown
`ArgumentException` when `samples` holds no row, a partial one, or a non-finite value — a length
that is not a positive whole number of rows is not a matrix, and guessing which values were meant
would be worse than refusing. A `NaN` or an infinity is refused for the same reason and is
[one divergence](../../../equivalence.md): scikit-learn skips the `NaN` and reports a per-feature
count, where this and the three other scalers refuse it rather than answer a mean for it.
```

In `standardscaler-partialfit.md`, `standardscaler-transform.md` and
`standardscaler-inversetransform.md`, the **Exceptions** line ends "holds no row, a partial one."
or "holds no row, or a partial one." Replace each with:

```markdown
**Exceptions** — `ArgumentException` when `samples` holds no row, a partial one, or a non-finite value.
```

`standardscaler-partialfit.md` line 41 already says these scalers refuse a non-finite value; leave
it, it is now true.

In `standardscaler-transform.md` and `standardscaler-inversetransform.md`, the sparse paragraph
says "ArgumentException when it holds no row or its column count is not FeatureCount". Replace that
clause with:

```markdown
`ArgumentException` when it holds no row, stores a non-finite value, or its column count is not `FeatureCount`,
```

- [ ] **Step 5: Add the sparse column statistics to `csrmatrix.md`'s per-operation list**

In `docs/reference/abstractions/sparse/csrmatrix.md`, the paragraph on a column stored twice in a
row ends "which is how scipy.sparse.csr_matrix reads it." Append to it:

```markdown
`Lodestar.Preprocessing`'s three sparse fits sum them too,
since `sklearn.utils.sparsefuncs` reduces through scipy.
```

- [ ] **Step 6: Rewrite the equivalence rows**

In `docs/equivalence.md`, in the `scaler.transform(sparse)` row, replace the sentence beginning
"Two divergences on a value no statistic can answer for" with:

```markdown
**One divergence on a value no statistic can answer for**: a stored `NaN` is refused here for all three scalers where the reference passes it through (`ensure_all_finite="allow-nan"`). A stored infinity is refused on both sides.
```

In the `StandardScaler().fit(X)` row, replace "Identical, the near-constant rule included:" with:

```markdown
Identical, the near-constant rule included, with **one divergence**: a `NaN` or an infinity is refused here naming `samples`, where the reference skips the `NaN` and refuses only the infinity — the rule the four scalers share, and the one `PartialFit`, `Transform` and `InverseTransform` apply too.
```

In the dense `scaler.transform(X)` and `scaler.inverse_transform(X)` rows, replace the leading
"Identical." with "Identical, the refusal of a non-finite value included."

In the `StandardScaler(with_mean=False).fit(sparse)` row, append to the cell, after the decision
0139 link:

```markdown
**One divergence on a column stored twice in one row**: all three sum its entries first, as `scipy.sparse`'s reductions do after `sum_duplicates()`, so `MaxAbsScaler` agrees with the reference — which reaches scipy — while the reference's own `StandardScaler` reads each entry on its own and its `RobustScaler` raises.
```

- [ ] **Step 7: Verify the reference gate and the snippets still hold**

```bash
python3 tools/extract_doc_snippets.py
```

Expected: exits 0. Then, from Task 4, the full build compiles the extracted snippets.

---

### Task 4: The gates, the changelog and the commit

**Files:**

- Modify: `CHANGELOG.md`
- Test: the whole solution

- [ ] **Step 1: Add the changelog sentence**

Under the unreleased `Lodestar.Preprocessing` heading in `CHANGELOG.md`, add one line:

```markdown
- `StandardScaler` refuses a non-finite value on every overload, and the three sparse fits sum a column stored twice in one row, as scipy's reductions do ([#1041](https://github.com/CyrilB1531/lodestar/issues/1041), [#1042](https://github.com/CyrilB1531/lodestar/issues/1042), [#1044](https://github.com/CyrilB1531/lodestar/issues/1044), [#1045](https://github.com/CyrilB1531/lodestar/issues/1045), [#1046](https://github.com/CyrilB1531/lodestar/issues/1046)).
```

- [ ] **Step 2: Run every check script, not a subset**

```bash
for s in tools/check_*.py; do echo "== $s"; python3 "$s" || echo "FAILED $s"; done
```

Expected: each exits 0. `tools/check_repeated_literals.py` needs `--base origin/main`; run it separately with that argument.

- [ ] **Step 3: Build and test both target frameworks**

```bash
./.dotnet-guarded dotnet build Lodestar.slnx -c Release
```

Expected: zero warnings, zero errors.

```bash
./.dotnet-guarded dotnet test Lodestar.slnx -c Release
```

Expected: **36 assemblies**, eighteen suites and their eighteen mirrors, all green. Read the count, not the colour.

- [ ] **Step 4: Format and lint**

```bash
./.dotnet-guarded dotnet format Lodestar.slnx --verify-no-changes
npx markdownlint-cli2 "README.md" "CONTRIBUTING.md" "docs/**/*.md" "tools/README.md" "bench/README.md"
```

Expected: both exit 0.

- [ ] **Step 5: Re-run the differential test against scikit-learn**

Run 300 random CSR matrices and 300 random dense matrices through `MaxAbsScaler.Fit`, `StandardScaler.Fit` and `RobustScaler.Fit` on both sides, comparing at `1e-9`, and confirm the 14 duplicate cases now agree and no finite case moved.

- [ ] **Step 6: Commit**

```bash
git add -A
git commit
```

The message names the behaviour, carries the five `Closes` lines and the `Co-Authored-By` trailer. One commit for the whole pull request.

## Self-review

**Spec coverage.** Decision 1 → Task 2 steps 3–5 and Task 3 steps 2–4. Decision 2 → Task 1 steps 3–4 and Task 3 steps 5–6. `SortedColumn`'s buffer → Task 1 step 5 and its test. The six XML tags (#1046) → Task 3 steps 1–2. The testing section → Task 1 step 1, Task 2 step 1, Task 4 steps 3 and 5. Out-of-scope items appear in no task.

**Placeholder scan.** Every code step carries the code. The doc steps quote the replacement text. No "TBD", no "handle edge cases", no "similar to Task N".

**Type consistency.** `Consolidated(CsrMatrix)` is defined in Task 1 step 3 and called under that name in step 4 and nowhere else. `HasDuplicate(CsrMatrix, int[])` is private to the same file. `SampleMatrix.RequireFinite(ReadOnlySpan<double>, string)` and `SparseColumns.Divided/Multiplied(..., bool requireFinite)` already exist with those signatures — verified at `src/Lodestar.Preprocessing/Internal/SampleMatrix.cs:27` and `Internal/SparseColumns.cs:118`.
