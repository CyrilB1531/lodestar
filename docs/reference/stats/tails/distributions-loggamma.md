# Distributions.LogGamma

The natural log of the gamma function.

<!-- docs-declaration -->

```csharp
public static double LogGamma(double x)
```

**Parameters** — `x` is a positive argument.

**Returns** — `scipy.special.gammaln(x)`.

**Exceptions** — `ArgumentOutOfRangeException` when `x` is not positive, `NaN` included.

**Example** — the factorial term a Poisson log-likelihood carries.

```csharp
using Lodestar.Stats;

// A Poisson response of 3 carries log(3!) in its log-likelihood: LogGamma(y + 1).
double logFactorial = Distributions.LogGamma(4.0);   // => 1.791759…

// Gamma(1/2) = sqrt(pi), so this is log(pi)/2.
double halfInteger = Distributions.LogGamma(0.5);     // => 0.572364…
```

**Remarks** — published for `Lodestar.Stats.Regression`'s Poisson log-likelihood, whose AIC
needs `log Γ(y + 1)` to agree with the reference's constant term
([decision 0095](../../../decisions/0095-the-stats-numerical-layer-publishes-four-members-and-no-more.md)
listed it among the internals that stay so while nothing had asked; #616 is what asked).

Unlike the four members decision 0095 first published, this one carries no separate corpus: its
internal implementation is already checked against closed forms — `LogGamma(n)` against the
factorial identity `log((n - 1)!)`, and the reflection formula's own identity — which is what a
log-gamma at a handful of digits calls for, rather than the far-tail precision a p-value needs.

**Applies to** — net10.0, netstandard2.0.

**See also** — [`Distributions.ChiSquaredSf`](distributions-chisquaredsf.md),
[`Distributions.NormalQuantile`](distributions-normalquantile.md).
