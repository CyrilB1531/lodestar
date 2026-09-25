# Distributions.StudentPdf

Student's *t* density.

<!-- docs-declaration -->

```csharp
public static double StudentPdf(double t, double df)
```

**Parameters** — `t` is the point; `NaN` answers `NaN`. `df` is the degrees of freedom, which must be positive; `+∞` is the standard normal law, as scipy's is.

**Returns** — `scipy.stats.t.pdf(t, df)`.

**Exceptions** — `ArgumentOutOfRangeException` when `df` is not positive, `NaN` included.

**Example** — the density at the centre and two units out, on five degrees of freedom.

```csharp
using Lodestar.Stats;

double centre = Distributions.StudentPdf(0.0, 5.0);   // => 0.379606…
double twoOut = Distributions.StudentPdf(2.0, 5.0);   // => 0.065090…
```

**Remarks** — Formed in logs. Past ten degrees of freedom the ratio `Γ((ν+1)/2) / Γ(ν/2)` comes from Stirling's series rather than from two log-gammas of order `ν log ν` subtracted, which at a million degrees of freedom would lose six digits.

**Applies to** — net10.0, netstandard2.0.

**See also** — [`Distributions.StudentCdf`](distributions-studentcdf.md), [`Distributions.StudentSf`](distributions-studentsf.md).
