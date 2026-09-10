# Bm25Index.Score

Scores every document against a query, in document order.

<!-- docs-declaration -->

```csharp
public double[] Score(IEnumerable<int> queryTerms)
```

**Parameters** — `queryTerms` are column indices, one per query *occurrence*. A term given twice
counts twice; an index outside the vocabulary is ignored.

**Returns** — `double[]`, one score per document, in the matrix's row order.

**Exceptions** — `ArgumentNullException` when `queryTerms` is null.

**Example** — a term in every document, where the score is small rather than large.

```csharp
using Lodestar.Abstractions;
using Lodestar.Text.Search;
using Lodestar.Text.Vectorization;

var vectorizer = new CountVectorizer();
CsrMatrix counts = vectorizer.FitTransform(
    ["the cat sat", "the dog sat sat", "a bird flew far away today"]);
int sat = vectorizer.GetFeatureNames().ToList().IndexOf("sat");

double[] scores = new Bm25Index(counts).Score([sat]);

double shorter = scores[0];  // => 0.0799…
double twice = scores[1];  // => 0.1013…
double absent = scores[2];  // => 0
```

**Remarks** — `sat` appears in two of the three documents, so Robertson's IDF for it is negative
and gets floored; the surviving weight is small, which is the point. Compare `cat`, in one document
of three, which scores `0.5755…` — several times more for the same single occurrence.

The second document holds `sat` twice and scores more than the first, but **not twice as much**:
that is `K1` saturating the term frequency. It is also longer, which `B` penalises — the two pull
against each other and the parameters decide by how much.

**A score can be negative**, and `Score` does not clamp. It happens when the floored IDF is itself
negative, which is what [`Bm25Idf.RobertsonFloored`](bm25idf.md) does in a corpus whose mean raw IDF
is below zero. Clamping would hide a genuine property of the weighting.

**Applies to** — net10.0, netstandard2.0.

**See also** — [`Bm25Index`](bm25index.md), [`Bm25Index.Top`](bm25index-top.md).
