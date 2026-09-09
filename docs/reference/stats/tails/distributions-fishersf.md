# Distributions.FisherSf

The upper tail of the *F* distribution: `P(F > f)`.

<!-- docs-declaration -->

```csharp
public static double FisherSf(double f, double numeratorDf, double denominatorDf)
```

**Parameters** — `f` is the statistic. `numeratorDf` and `denominatorDf` are the two degrees of
freedom, both of which must be positive.

**Returns** — `scipy.stats.f.sf(f, dfn, dfd)`.

**Exceptions** — `ArgumentOutOfRangeException` when either degrees-of-freedom argument is not
positive, `NaN` included.

**Example** — the overall significance of a model with two regressors on twenty residual degrees
of freedom.

```csharp
using Lodestar.Stats;

double overall = Distributions.FisherSf(4.0, 2.0, 20.0);  // => 0.0345716…

// Far into the tail, where an absolute tolerance would accept a zero.
double extreme = Distributions.FisherSf(500.0, 3.0, 100.0);  // => 4.8466962308084166E-60
```

**Remarks** — the same tail one-way ANOVA already reports here, exposed for a caller that computed
its own *F* — a regression's overall test, or a nested-model comparison — rather than handing this
package its groups.

The two degrees of freedom are **not** interchangeable: `FisherSf(f, a, b)` and `FisherSf(f, b, a)`
are different numbers, and the numerator's is the one that counts the constraints being tested.

**Applies to** — net10.0, netstandard2.0.

**See also** — [`OneWayAnova`](../tests/onewayanova.md),
[`Distributions.StudentSf`](distributions-studentsf.md).
