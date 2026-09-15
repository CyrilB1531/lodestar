# MultinomialLogitSummary

What a multinomial logit fit reports, at `statsmodels` parity.

<!-- docs-declaration -->

```csharp
public sealed class MultinomialLogitSummary
```

**Properties**:

- **Categories:** `Categories` is the distinct labels in ascending order; the first is the reference category.
- **The coefficient table.** `Coefficients`, `StandardErrors`, `ZStatistics`, `PValues`, `ConfidenceLower` and
  `ConfidenceUpper` are indexed by equation first, then by parameter. Equation `j` is `Categories[j + 1]` against
  `Categories[0]`, and its first parameter is the intercept when one was fitted.
- **The whole model:**
  - `LogLikelihood` is the fitted log-likelihood.
  - `NullLogLikelihood` is the constant-only model's, in closed form.
  - `PseudoRSquared` is McFadden's `1 − LogLikelihood / NullLogLikelihood`.
  - `LikelihoodRatio` is `2·(LogLikelihood − NullLogLikelihood)`, and `LikelihoodRatioPValue` its χ² tail on
    `ModelDegreesOfFreedom`.
  - `Akaike` and `Bayesian` count `ModelDegreesOfFreedom + J − 1` parameters.
  - `ModelDegreesOfFreedom` is `(K − 1)·(J − 1)`, and `ResidualDegreesOfFreedom` is `n − ModelDegreesOfFreedom − (J − 1)`.
- **The fit itself:** `HasIntercept`, `ConfidenceLevel`, `Converged` and `Iterations` describe the fit that produced
  the table.

**Example** — the whole-model half of the table.

```csharp
using Lodestar.Stats.Regression;

double[] design = [-0.8, -1.32, -0.25, 0.42, 1.14, 0.11, -0.55, -0.78, 0.75, 1.63, 0.27, -1.23,
                   -0.96, 1.6, 0.2, -1.73, -0.08, -1.16, -0.63, -0.49, -0.71, 0.55, -0.06, -0.59,
                   0.41, 0.83, -1.64, -0.26, -0.98, -0.17, -1.29, 0.02, -0.04, -0.3, -1.05, -0.4];
int[] response = [2, 2, 0, 0, 0, 2, 2, 2, 0, 0, 0, 2, 2, 0, 0, 2, 2, 2, 2, 2, 2, 0, 0, 2,
                  0, 0, 2, 2, 2, 0, 2, 2, 1, 2, 2, 2];

MultinomialLogitSummary fit = MultinomialLogit.Fit(design, response, 1);

int equations = fit.Coefficients.Count;       // => 2
int modelDegrees = fit.ModelDegreesOfFreedom; // => 2
int residualDegrees = fit.ResidualDegreesOfFreedom;  // => 32
```

**Remarks** — **read `Converged` before the rest** when `ThrowOnNonConvergence` was turned off: a table from a fit
that spent its budget is plausible and wrong.

**Applies to** — net10.0, netstandard2.0.

**See also** — [`MultinomialLogit.Fit`](multinomiallogit-fit.md), [`MultinomialLogitOptions`](multinomiallogitoptions.md).
