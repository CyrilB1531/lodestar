# Bm25Index

BM25 (Okapi) scored over a document-term matrix of raw counts.

<!-- docs-declaration -->

```csharp
public sealed class Bm25Index
```

**Example** — three documents, and the rare term that separates them.

```csharp
using Lodestar.Abstractions;
using Lodestar.Text.Search;
using Lodestar.Text.Vectorization;

var vectorizer = new CountVectorizer();
CsrMatrix counts = vectorizer.FitTransform(
    ["the cat sat", "the dog sat sat", "a bird flew far away today"]);
int cat = vectorizer.GetFeatureNames().ToList().IndexOf("cat");

var index = new Bm25Index(counts);
double[] scores = index.Score([cat]);

double first = scores[0];  // => 0.5755…
double second = scores[1];  // => 0
int documents = index.DocumentCount;  // => 3
```

**Remarks** — the matrix is the one `CountVectorizer` produced, so everything that shaped it —
the analyzer, the n-gram range, the stop words, `MinDf` and `MaxDf` — has already happened and is
not re-decided here. `Bm25Index` adds the weighting and nothing else.

**Document length comes free.** BM25's `|D|` is the L1 norm of a raw-count row, which
[`CsrMatrix.RowL1Norm`](../../abstractions/sparse/csrmatrix-rowl1norm.md) already exposes; document
frequency is one pass over the column indices. There is no new numerical machinery in this type.

**Building transposes, so a query does not scan.** CSR is row-major, and reaching the documents
that hold one term through it means reading every row. The constructor sorts the non-zeros by
column once — one extra copy of them — after which a query term reads its own postings and a rare
term costs what it matches rather than what the corpus holds.

**Queries are column indices, not strings.** One entry per query *occurrence*, so a term given
twice counts twice — which is how the reference counts it, and a set-based query would halve the
contribution. An index outside the vocabulary is ignored, which is how a term the corpus never saw
behaves. `GetFeatureNames()` is the map from term to column.

Immutable once built, and safe to score from any number of threads at once.

Reference behaviour is `rank_bm25` 0.2.2's `BM25Okapi`, matched over 8 corpora.

**Applies to** — net10.0, netstandard2.0.

**See also** — [the search index](../search.md), [`Bm25Options`](bm25options.md),
[`RankFusion`](rankfusion.md).

## Members

| Member | What it does |
| --- | --- |
| [`Bm25Index.Score`](bm25index-score.md) | Scores every document against a query. |
| [`Bm25Index.Top`](bm25index-top.md) | The best documents for a query, best first. |
