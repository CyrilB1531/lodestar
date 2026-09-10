# Bm25Index.Top

The best documents for a query, best first.

<!-- docs-declaration -->

```csharp
public IReadOnlyList<SearchHit> Top(IEnumerable<int> queryTerms, int count)
```

**Parameters** — `queryTerms` are column indices, as [`Score`](bm25index-score.md) takes them.
`count` is how many hits to return; fewer come back when the corpus is smaller.

**Returns** — `IReadOnlyList<SearchHit>`, descending by score.

**Exceptions** — `ArgumentNullException` when `queryTerms` is null;
`ArgumentOutOfRangeException` when `count` is negative.

**Example** — the two best documents for a rare term.

```csharp
using Lodestar.Abstractions;
using Lodestar.Text.Search;
using Lodestar.Text.Vectorization;

var vectorizer = new CountVectorizer();
CsrMatrix counts = vectorizer.FitTransform(
    ["the cat sat", "the dog sat sat", "a bird flew far away today"]);
int cat = vectorizer.GetFeatureNames().ToList().IndexOf("cat");

IReadOnlyList<SearchHit> hits = new Bm25Index(counts).Top([cat], 2);

int best = hits[0].Document;  // => 0
double score = hits[0].Score;  // => 0.5755…
int howMany = hits.Count;  // => 2
```

**Remarks** — the same scores [`Score`](bm25index-score.md) returns, ordered; it is not a second
scoring path.

**Ties break by document index, ascending.** A query that separates nothing therefore returns the
corpus in its own order rather than in whatever order the sort happened to produce, which is what
makes two runs of the same query comparable.

**Documents scoring zero come back like any other.** Asking for ten hits from a corpus of ten gives
ten, however few actually matched — filter on the score if only matches are wanted. That is a
deliberate choice: a cutoff belongs to the caller, who knows what the scores mean in their corpus,
and this type does not.

**Applies to** — net10.0, netstandard2.0.

**See also** — [`Bm25Index`](bm25index.md), [`SearchHit`](searchhit.md).
