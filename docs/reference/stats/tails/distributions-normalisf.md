# Distributions.NormalIsf

The `z` with `P(Z > z) = p`: the inverse of `NormalSf`.

<!-- docs-declaration -->

```csharp
public static double NormalIsf(double p)
```

**Parameters** — `p` is a probability in `[0, 1]`. `0` answers `+∞` and `1` answers `−∞`.

**Returns** — `scipy.stats.norm.isf(p)`.

**Exceptions** — `ArgumentOutOfRangeException` when `p` is outside `[0, 1]`, `NaN` included.

**Example** — the point with 2.5% above it.

```csharp
using Lodestar.Stats;

double upper = Distributions.NormalIsf(0.025);   // => 1.959963…
```

**Remarks** — The mirror of [`NormalQuantile`](distributions-normalquantile.md): `NormalIsf(p)` is `NormalQuantile(1 − p)`, computed without forming `1 − p`, which keeps a tiny `p`'s digits.

**Applies to** — net10.0, netstandard2.0.

**See also** — [`Distributions.NormalQuantile`](distributions-normalquantile.md), [`Distributions.NormalSf`](distributions-normalsf.md).
