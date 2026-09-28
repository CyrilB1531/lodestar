# ChiSquaredContingencyResult

A contingency-table chi-square result.

<!-- docs-declaration -->

```csharp
public sealed record ChiSquaredContingencyResult(double Statistic, double PValue, int DegreesOfFreedom, double[][] ExpectedFrequencies)
```

**Properties** — `Statistic` is the chi-square statistic. `PValue` is the upper-tail p-value.
`DegreesOfFreedom` is the degrees of freedom, `(rows - 1) * (columns - 1)`. `ExpectedFrequencies` is the table
expected under independence, row-major, the same shape as the input table.

**Example** — the table [`ChiSquared.Contingency`](chisquared-contingency.md) tests for
independence.

```csharp
using Lodestar.Stats;

double[][] table =
[
    [30.0, 20.0],
    [15.0, 35.0],
];

ChiSquaredContingencyResult result = ChiSquared.Contingency(table);

int dof = result.DegreesOfFreedom;                                  // => 1
double expected01 = result.ExpectedFrequencies[0][1];   // => 27.5
```

**Remarks** — `ExpectedFrequencies` is what the table would look like if the two factors were
independent, each cell `rowTotal * columnTotal / grandTotal`; comparing it against `table`
cell by cell is what the statistic itself does. It shares `table`'s exact shape rather than a
flattened form, so `result.ExpectedFrequencies[i][j]` lines up directly with `table[i][j]`.

Being a `record`, equality would otherwise compare `double[][]` by reference; `Equals` and
`GetHashCode` are written by hand instead, so two results holding the same expected frequencies
compare equal. the equality rule
has the rule.

`ExpectedFrequencies` and its rows are taken and exposed as they are, not copied:
writing to one changes this record and what it equals, and every holder of the same array, a `with`
copy included ([#1305](https://github.com/CyrilB1531/lodestar/issues/1305)).

**Applies to** — net10.0, netstandard2.0.

**See also** — [`ChiSquared.Contingency`](chisquared-contingency.md), [`TestResult`](testresult.md),
the [Python equivalence table](../../../equivalence.md).

## Members

| member | what it does |
| --- | --- |
| [`ChiSquaredContingencyResult.Equals`](chisquaredcontingencyresult-equals.md) | Value equality, the table row by row. |
| [`ChiSquaredContingencyResult.GetHashCode`](chisquaredcontingencyresult-gethashcode.md) | A hash consistent with it. |
