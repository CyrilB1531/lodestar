# Bm25Options

How a [`Bm25Index`](bm25index.md) saturates term frequency and normalizes length.

<!-- docs-declaration -->

```csharp
public sealed record Bm25Options(double K1 = 1.5, double B = 0.75, Bm25Idf Idf = Bm25Idf.RobertsonFloored, double Epsilon = 0.25)
```

**Parameters** — `K1` saturates term frequency and must be non-negative. `B` interpolates length
normalization and lies in `[0, 1]`. `Idf` picks which inverse document frequency to weight by.
`Epsilon` is the floor multiplier [`Bm25Idf.RobertsonFloored`](bm25idf.md) applies, ignored by the
Lucene variant.

**Example** — switching length normalization off, and switching the IDF variant.

```csharp
using Lodestar.Text.Search;

var defaults = new Bm25Options();
double k1 = defaults.K1;  // => 1.5

var flat = defaults with { B = 0.0 };
var lucene = defaults with { Idf = Bm25Idf.Lucene };

double noLengthPenalty = flat.B;  // => 0
```

**Remarks** — the defaults are `rank_bm25`'s, so the documented default is the tested one.

**`K1` is 1.5, not 1.2.** Lucene defaults to 1.2, and a caller porting numbers from a Lucene index
has to say so explicitly. The two are not close enough to ignore: `K1` is what decides how much a
second occurrence of a term is worth relative to the first.

**`B = 0` removes length normalization entirely**, which makes a term in a one-word document score
exactly what it scores in a thousand-word one. `B = 1` divides fully by relative length. The default
0.75 is the value the literature settled on and neither endpoint is usually what you want.

**Applies to** — net10.0, netstandard2.0.

**See also** — [`Bm25Index`](bm25index.md), [`Bm25Idf`](bm25idf.md).
