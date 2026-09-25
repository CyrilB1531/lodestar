# Spearman.Matrix

Spearman's rho between every pair of variables, `scipy.stats.spearmanr` on a 2-D array.

<!-- docs-declaration -->

```csharp
public static CorrelationMatrix Matrix(ReadOnlySpan<double> data, int variableCount, Alternative alternative = Alternative.TwoSided, NanPolicy nanPolicy = NanPolicy.Propagate)
```

**Parameters** — `data` is the observations, row-major: one row per observation,
`variableCount` values each. `variableCount` is at least two. `alternative` is which tail each
p-value covers. `nanPolicy` is scipy's `nan_policy`.

**Returns** — a [`CorrelationMatrix`](correlationmatrix.md): the correlations and their p-values,
each `variableCount × variableCount` and row-major.

**Exceptions** — `ArgumentOutOfRangeException` when `variableCount` is below two.
`ArgumentException` when `data` is not a whole number of rows, or holds a `NaN` under
[`NanPolicy.Raise`](../nanpolicy.md).

**Example** — three variables, the second rising with the first and the third falling.

```csharp
using Lodestar.Stats;

// Six observations of three variables, row by row.
double[] data =
[
    1.2, 3.4, 10.0,
    2.3, 3.1, 8.0,
    3.1, 4.8, 9.5,
    4.0, 4.2, 6.1,
    5.5, 6.9, 4.0,
    6.1, 6.0, 3.3,
];

CorrelationMatrix matrix = Spearman.Matrix(data, variableCount: 3);

double rising = Math.Round(matrix.Statistics[1], 6);    // => 0.828571
double falling = Math.Round(matrix.Statistics[2], 6);   // => -0.942857
double p = Math.Round(matrix.PValues[2], 6);            // => 0.004805
```

**Remarks** — **each off-diagonal entry is [`Spearman.Test`](spearman-test.md) on its two
columns**. Under [`NanPolicy.Omit`](../nanpolicy.md) rows are dropped pair by pair, so each pair
keeps every row complete for its two variables, as scipy's does; under
[`NanPolicy.Propagate`](../nanpolicy.md) a variable holding a `NaN` has `NaN` in its row and
column and every other entry is computed. The diagonal follows scipy's two paths: under `omit`,
where the data hold a `NaN`, it is `(1, 0)` whatever the alternative; otherwise it is computed as
`numpy.corrcoef` computes it, which can round one ulp below 1. scipy returns a scalar for two variables; this returns the
matrix.

**Applies to** — net10.0, netstandard2.0.

**See also** — [`Spearman.Test`](spearman-test.md), [`CorrelationMatrix`](correlationmatrix.md).
