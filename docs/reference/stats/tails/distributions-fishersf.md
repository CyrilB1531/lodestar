# Distributions.FisherSf

The former name of [`Distributions.FisherSnedecorSf`](distributions-fishersnedecorsf.md), which it calls.

<!-- docs-declaration -->

```csharp
public static double FisherSf(double f, double numeratorDf, double denominatorDf)
```

**Parameters** — `f` is the statistic. `numeratorDf` and `denominatorDf` are the two degrees of
freedom, both of which must be positive and finite.

**Returns** — `scipy.stats.f.sf(f, dfn, dfd)`.

**Exceptions** — `ArgumentOutOfRangeException` when either degrees-of-freedom argument is not
positive, `NaN` included.

**Remarks** — obsolete and hidden from IntelliSense: renamed so that "Fisher" names Fisher's exact
test alone ([#1217](https://github.com/CyrilB1531/lodestar/issues/1217)). It stays for binaries
compiled against 0.5.0, `Lodestar.Stats.Regression` 0.2 among them, until their floors move past
this release.

**Applies to** — net10.0, netstandard2.0.

**See also** — [`Distributions`](distributions.md).
