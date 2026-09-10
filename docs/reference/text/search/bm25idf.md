# Bm25Idf

Which inverse document frequency a [`Bm25Index`](bm25index.md) weights terms by.

<!-- docs-declaration -->

```csharp
public enum Bm25Idf
```

**Values** — `RobertsonFloored` is `log((N - n + 0.5) / (n + 0.5))` with negatives floored;
`Lucene` is `log(1 + (N - n + 0.5) / (n + 0.5))`.

**Example** — the two are different numbers, not a rescaling of one another.

```csharp
using Lodestar.Abstractions;
using Lodestar.Text.Search;
using Lodestar.Text.Vectorization;

var vectorizer = new CountVectorizer();
CsrMatrix counts = vectorizer.FitTransform(
    ["the cat sat", "the dog sat sat", "a bird flew far away today"]);
int sat = vectorizer.GetFeatureNames().ToList().IndexOf("sat");

double floored = new Bm25Index(counts).Score([sat])[1];  // => 0.1013…
double lucene = new Bm25Index(counts, new Bm25Options(Idf: Bm25Idf.Lucene)).Score([sat])[1];

bool higher = lucene > floored;  // => True
```

**Remarks** — the choice is stated rather than inherited because the three published forms give
different numbers, and the ordering of two documents can differ when a query mixes a common and a
rare term.

**`RobertsonFloored` is what this package is checked against.** Robertson's raw form goes
**negative** once a term appears in more than half the corpus, which would let a match subtract from
a score. `rank_bm25` replaces every negative with `Epsilon × (mean of the raw values)`, and this
reproduces that. Two details matter and are easy to get wrong:

- the mean is taken over the **raw** values, negatives included — flooring first and averaging after
  gives a larger floor;
- the floor is **itself negative** when that mean is below zero, so scores in such a corpus are
  negative and a longer document scores *higher*. That is the reference's behaviour, not a defect.

**`Lucene` is non-negative by construction**, so it needs no floor and ignores `Epsilon`. It is the
variant to pick when negative scores would be awkward downstream — a fusion step, for instance.

`BM25L` and `BM25+`, the other lower-bounding variants, are not here.

**Applies to** — net10.0, netstandard2.0.

**See also** — [`Bm25Options`](bm25options.md), [`Bm25Index.Score`](bm25index-score.md).
