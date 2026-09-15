# GlmFamily

The response distribution a `GeneralizedLinearModel` fits, with the link statsmodels defaults it to.

<!-- docs-declaration -->

```csharp
public enum GlmFamily
```

**Members** — `Binomial` is a response in `{0, 1}`, through the logit link. `Poisson` is a
non-negative count, through the log link. `NegativeBinomial` is a non-negative count whose variance
`μ + αμ²` grows faster than its mean, through the log link, with `α` given by
[`GlmOptions.NegativeBinomialAlpha`](glmoptions.md); the log link is statsmodels' default for it rather
than its canonical one.

**Example** — the same design, read through each family's own link.

```csharp
using Lodestar.Stats.Regression;

double[] design = [0.0, 1.0, 2.0, 3.0, 4.0, 5.0];
double[] binomialResponse = [0.0, 0.0, 1.0, 0.0, 1.0, 1.0];

GlmSummary logit = GeneralizedLinearModel.Fit(design, binomialResponse, 1, GlmFamily.Binomial);

double onLogitScale = logit.Coefficients[1];  // => 1.2140275858506062
```

**Example** — over-dispersed counts: the negative binomial keeps a slope close to Poisson's and
reports how much less sure of it the extra variance leaves the fit.

```csharp
using Lodestar.Stats.Regression;

double[] x = [0.0, 1.0, 2.0, 3.0, 4.0, 5.0, 6.0, 7.0, 8.0, 9.0];
double[] counts = [1.0, 0.0, 2.0, 3.0, 4.0, 3.0, 7.0, 6.0, 9.0, 11.0];

GlmSummary poisson = GeneralizedLinearModel.Fit(x, counts, 1, GlmFamily.Poisson);
GlmSummary negativeBinomial = GeneralizedLinearModel.Fit(
    x, counts, 1, GlmFamily.NegativeBinomial, new GlmOptions { NegativeBinomialAlpha = 0.5 });

double poissonError = Math.Round(poisson.StandardErrors[1], 4);            // => 0.0607
double widerError = Math.Round(negativeBinomial.StandardErrors[1], 4);     // => 0.1054
```

**Remarks** — closed on purpose. IRLS cannot check that a caller-supplied family is internally
consistent, and an incoherent one produces a plausible inference table rather than an error — so a
family is chosen from this set rather than described by an interface (#616). Adding a member is not
a breaking change, which is how `NegativeBinomial` joined (#769) and how Gamma will (#770).

**Applies to** — net10.0, netstandard2.0.

**See also** — [`GeneralizedLinearModel.Fit`](generalizedlinearmodel-fit.md),
[`GlmSummary`](glmsummary.md).
