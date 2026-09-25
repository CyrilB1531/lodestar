# IvDesign

An instrumental-variables problem's data: the response and its three row-major blocks with their
widths.

<!-- docs-declaration -->

```csharp
public readonly ref struct IvDesign
```

**Properties** — `Response` is one value per row. `Exogenous`, `Endogenous` and `Instruments` are
row-major blocks, `ExogenousCount`, `EndogenousCount` and `InstrumentCount` values per row: the
regressors assumed uncorrelated with the error, the ones that are not, and the excluded instruments.
`Exogenous` is empty and `ExogenousCount` zero when there are none. None of the blocks carries a
constant column of its own: [`IvOptions.WithIntercept`](ivoptions.md) adds it.

**Example** — the same data described once and fitted twice.

```csharp
using Lodestar.Stats.Regression;
using Lodestar.Stats.Regression.Instrumental;

double[] response = [3.1, 4.0, 5.2, 4.4, 6.9, 7.1, 6.0, 8.8, 9.1, 8.2, 10.7, 11.3];
double[] exogenous = [0.2, -1.0, 0.5, 1.3, -0.4, 0.9, -1.2, 0.1, 1.7, -0.6, 0.8, -0.3];
double[] endogenous = [1.0, 1.4, 2.1, 1.8, 3.0, 3.3, 2.6, 3.9, 4.2, 3.7, 4.9, 5.4];
double[] instruments =
    [0.9, 0.1, 1.5, -0.3, 2.2, 0.4, 1.7, 0.8, 3.1, -0.2, 3.3, 0.6,
     2.4, 1.1, 3.8, -0.5, 4.1, 0.9, 3.5, 0.2, 4.6, -0.1, 5.2, 0.7];

var design = new IvDesign(response, exogenous, 1, endogenous, 1, instruments, 2);

int rows = design.Response.Length;                  // => 12
double twoStage = InstrumentalVariables.TwoStageLeastSquares(design).Coefficients[2];  // => 1.9054825408…
double liml = InstrumentalVariables.Liml(design).Coefficients[2];                     // => 1.9021995…
```

**Remarks** — a `ref struct`, so the spans are read in place and a design lives no longer than the
call it is passed to. The constructor checks nothing; the estimators refuse a block whose length is
not its width times the rows, and fewer instruments than endogenous regressors.

**Applies to** — net10.0, netstandard2.0.

**See also** — [`InstrumentalVariables`](../iv/instrumentalvariables.md).
