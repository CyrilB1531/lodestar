# GlmFamily

The response distribution a `GeneralizedLinearModel` fits, with its canonical link.

<!-- docs-declaration -->

```csharp
public enum GlmFamily
```

**Members** — `Binomial` is a response in `{0, 1}`, through the logit link. `Poisson` is a
non-negative count, through the log link.

**Example** — the same design, read through each family's own link.

```csharp
using Lodestar.Stats.Regression;

double[] design = [0.0, 1.0, 2.0, 3.0, 4.0, 5.0];
double[] binomialResponse = [0.0, 0.0, 1.0, 0.0, 1.0, 1.0];

GlmSummary logit = GeneralizedLinearModel.Fit(design, binomialResponse, 1, GlmFamily.Binomial);

double onLogitScale = logit.Coefficients[1];  // => 1.2140275858506053
```

**Remarks** — closed on purpose. IRLS cannot check that a caller-supplied family is internally
consistent, and an incoherent one produces a plausible inference table rather than an error — so a
family is chosen from this set rather than described by an interface (#616). Adding a member is not
a breaking change, which is what a follow-up family (Gamma, negative binomial — both out of scope
here) relies on.

**Applies to** — net10.0, netstandard2.0.

**See also** — [`GeneralizedLinearModel.Fit`](generalizedlinearmodel-fit.md),
[`GlmSummary`](glmsummary.md).
