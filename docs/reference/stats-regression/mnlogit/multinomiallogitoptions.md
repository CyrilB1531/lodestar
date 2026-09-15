# MultinomialLogitOptions

What a multinomial logit fit should estimate, at what confidence, and how long Newton may run.

<!-- docs-declaration -->

```csharp
public sealed record MultinomialLogitOptions
```

**Properties** — `WithIntercept` prepends a column of ones to every equation; `true` by default. `ConfidenceLevel` is
the two-sided level of the intervals; `0.95` by default, strictly inside (0, 1). `MaximumIterations` is Newton's budget;
`35` by default, the reference's. `Tolerance` is the largest step in any parameter that still counts as converged;
`1e-8` by default. `ThrowOnNonConvergence` says whether a fit that spends its budget throws; `true` by default.

**Example** — a fit allowed one step, inspected rather than refused.

```csharp
using Lodestar.Stats.Regression;

double[] design = [-1.1, -0.73, -0.78, 0.27, -0.25, 0.13, 0.84, 0.86, 0.48, -0.45, -0.75, -0.81, -0.34, -0.05, -0.97];
int[] response = [2, 1, 0, 0, 1, 0, 2, 2, 0, 1, 1, 2, 1, 0, 1];

MultinomialLogitSummary once = MultinomialLogit.Fit(
    design, response, 1, new MultinomialLogitOptions { MaximumIterations = 1, ThrowOnNonConvergence = false });

bool converged = once.Converged;  // => False
```

**Remarks** — `converged` is false exactly when the budget was spent, as in the reference: a fit whose last allowed
step was already small enough still reports `false`.

**Applies to** — net10.0, netstandard2.0.

**See also** — [`MultinomialLogit.Fit`](multinomiallogit-fit.md), [`MultinomialLogitSummary`](multinomiallogitsummary.md).
