# SimpleImputer

Fills the missing values of each feature with a statistic of the ones present, at
`sklearn.impute.SimpleImputer` parity.

<!-- docs-declaration -->

```csharp
public sealed class SimpleImputer
```

**Properties** — `FeatureCount` and `SampleCount` are the shape it was fitted on, and `Statistics`
is what each feature's missing values are filled with — the reference's `statistics_`.

**Example** — the mean of what is present, per feature.

```csharp
using Lodestar.Preprocessing;

// Two features; each is missing one value.
double[] samples = [1.0, 10.0, 2.0, double.NaN, double.NaN, 30.0];

SimpleImputer imputer = SimpleImputer.Fit(samples, featureCount: 2);

double first = imputer.Statistics[0];   // => 1.5
double second = imputer.Statistics[1];  // => 20
```

**Remarks** — **`NaN` is what marks a value missing**, as it does in the reference: there is no
separate mask, and a matrix with no `NaN` comes back unchanged. An infinity is refused — it marks
nothing and would carry into every statistic.

**Applies to** — net10.0, netstandard2.0.

**See also** — [`SimpleImputerOptions`](simpleimputeroptions.md), [`ImputationStrategy`](imputationstrategy.md).

## Members

| Member | What it does |
| --- | --- |
| [`SimpleImputer.Fit`](simpleimputer-fit.md) | Fits an imputer on a row-major sample matrix. |
| [`SimpleImputer.Transform`](simpleimputer-transform.md) | Fills the missing values of a matrix. |
