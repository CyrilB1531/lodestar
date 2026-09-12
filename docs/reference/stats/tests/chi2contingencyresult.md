# Chi2ContingencyResult

A contingency-table chi-square result.

<!-- docs-declaration -->

```csharp
public sealed record Chi2ContingencyResult(double Statistic, double PValue, int Dof, double[][] ExpectedFrequencies)
```

**Properties** — `Statistic` is the chi-square statistic. `PValue` is the upper-tail p-value.
`Dof` is the degrees of freedom, `(rows - 1) * (columns - 1)`. `ExpectedFrequencies` is the table
expected under independence, row-major, the same shape as the input table.

**Example** — the table [`ChiSquare.Contingency`](chisquare-contingency.md) tests for
independence.

```csharp
using Lodestar.Stats;

double[][] table =
[
    [30.0, 20.0],
    [15.0, 35.0],
];

Chi2ContingencyResult result = ChiSquare.Contingency(table);

int dof = result.Dof;                                  // => 1
double expected01 = result.ExpectedFrequencies[0][1];   // => 27.5
```

**Remarks** — `ExpectedFrequencies` is what the table would look like if the two factors were
independent, each cell `rowTotal * columnTotal / grandTotal`; comparing it against `table`
cell by cell is what the statistic itself does. It shares `table`'s exact shape rather than a
flattened form, so `result.ExpectedFrequencies[i][j]` lines up directly with `table[i][j]`.

Being a `record`, equality would otherwise compare `double[][]` by reference; `Equals` and
`GetHashCode` are written by hand instead, so two results holding the same expected frequencies
compare equal. [Decision
0112](https://github.com/CyrilB1531/lodestar/blob/main/docs/decisions/0112-a-record-whose-member-compares-by-reference-writes-its-own-equality.md)
has the rule.

**Applies to** — net10.0, netstandard2.0.

**See also** — [`ChiSquare.Contingency`](chisquare-contingency.md), [`TestResult`](testresult.md),
the [Python equivalence table](../../../equivalence.md).

## Members

| member | what it does |
| --- | --- |
| [`Chi2ContingencyResult.Equals`](chi2contingencyresult-equals.md) | Value equality, the table row by row. |
| [`Chi2ContingencyResult.GetHashCode`](chi2contingencyresult-gethashcode.md) | A hash consistent with it. |
