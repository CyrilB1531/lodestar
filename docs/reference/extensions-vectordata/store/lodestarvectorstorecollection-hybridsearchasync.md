# LodestarVectorStoreCollection.HybridSearchAsync

Fuses the vector ranking with a BM25 ranking over keywords.

<!-- docs-declaration -->

```csharp
public IAsyncEnumerable<VectorSearchResult<TRecord>> HybridSearchAsync<TInput>(TInput searchValue, ICollection<string> keywords, int top, HybridSearchOptions<TRecord> options = null, CancellationToken cancellationToken = default) where TInput : notnull
```

**Parameters** — `searchValue` is the query vector, as a `ReadOnlyMemory<float>` or a `float[]`.
`keywords` are the terms the keyword half scores, joined into one query document. `top` is the most
results returned. `options` carries `Filter` and `Skip`, both applied to the fused ranking;
`ScoreThreshold` is refused, and `VectorProperty`, `AdditionalProperty` and `IncludeVectors` are not
read.
`cancellationToken` is checked between results.

**Returns** — `IAsyncEnumerable<VectorSearchResult<TRecord>>`, best first. Each score is the record's
**reciprocal-rank fusion score**, `Σ 1 / (k + rank)` over the rankings it appears in, not a
similarity. An empty collection returns no results.

**Exceptions** — `ArgumentNullException` when `keywords` is null. `ArgumentOutOfRangeException` when
`top` is less than 1, or when the collection's `Vectorizer` or `Bm25` options are outside their range.
`ArgumentException` when the query is not the collection's vector width, an empty collection
included, or when the `Vectorizer` options' `NgramRange` is not an ascending range from 1.
`InvalidOperationException` when the `Vectorizer` options' `MaxDf` corresponds to fewer
records than their `MinDf`, as a `CountVectorizer` fit refuses it. Options are checked by the first
hybrid search over a non-empty collection; a vector search never reads them.
`NotSupportedException` when `TRecord` marks no `IsFullTextIndexed` property, when `searchValue` is
not a vector, or when `options` sets `ScoreThreshold`. `OperationCanceledException`
when `cancellationToken` is cancelled between results. All of them are raised when enumeration
begins, not when the method is called.

**Example** — a record the vector ranks last and the keyword ranks first comes out on top.

```csharp
using Lodestar.Extensions.VectorData;
using Microsoft.Extensions.VectorData;

static async Task<string> FuseAsync()
{
    using var notes = new LodestarVectorStoreCollection<string, Note>("notes");
    await notes.UpsertAsync([
        new Note { Id = "a", Text = "the cat sat on the mat", Embedding = new float[] { 1f, 0f, 0f } },
        new Note { Id = "b", Text = "the dog ran in the park", Embedding = new float[] { 0f, 1f, 0f } },
        new Note { Id = "c", Text = "an elephant crossed the river", Embedding = new float[] { 0f, 0f, 1f } },
    ]);

    List<VectorSearchResult<Note>> hits = await notes
        .HybridSearchAsync(new float[] { 1f, 0f, 0f }, ["elephant", "zebra"], 2)
        .ToListAsync();

    return string.Join(",", hits.Select(hit => hit.Record.Id));
}

string fused = FuseAsync().GetAwaiter().GetResult();  // => c,a
```

The vector ranking is `a, b, c`; the keyword ranking is `c` alone, since `zebra` is in no note. At
`k = 60`, `c` scores `1/63 + 1/61`, `a` scores `1/61` and `b` scores `1/62`, so the fused order is
`c, a` — a result neither ranking gives on its own.

**Remarks** — three published pieces, joined and nothing added, which the collection keeps
up to date one written record at a time rather than rebuilding:

1. the vector ranking of **every** record, as [`EmbeddingIndex.Search`](../../embeddings/search/embeddingindex-search.md)
   ranks it;
2. the keyword ranking, as [`Bm25Index.Score`](../../text/search/bm25index-score.md) scores it over
   the vocabulary a [`CountVectorizer`](../../text/vectorizers/countvectorizer.md) would fit on the
   full-text values held now — each text is tokenized by that vectorizer once, and a query exactly as
   the records were;
3. the fusion, [`RankFusion.Rrf`](../../text/search/rankfusion-rrf.md), at
   [`LodestarVectorStoreOptions`](lodestarvectorstoreoptions.md)' `RankFusionK`, 60 by default.

Every result, score and tie is the one those three would give over a from-scratch build; the suite
replays random writes against exactly that build to hold it so.

**What a query costs.** Every record's vector is scored, as an exact search must, and every
matched record's BM25 score. Neither ranking is then sorted in full: both are read to a depth, each
record read has its rank in the other counted exactly, and a record read in neither can fuse to no
more than `2 / (k + depth + 1)` — so once the last result wanted fuses above that, the answer is
settled, and otherwise the depth doubles. The first depth is `2k`, 120 at the default, or `top +
Skip` when that is more. The BM25 figures that are whole-corpus by definition — the average length, and the floor
on a negative IDF, a share of the mean IDF over the vocabulary — are counts kept as records are
written, and the floor, when a keyword needs it, costs one pass over the vocabulary after each write.

**The keyword ranking holds only the records the keywords matched**, ordered by score descending and
then by index, which is [`Bm25Index.Top`](../../text/search/bm25index-top.md)'s own order over that
subset. `RankFusion.Rrf` reads a ranking's positions rather than its scores, so passing every
document through would hand an unmatched record credit for the order it was inserted in. Only the
matched records are scored and sorted, read from the queried terms' postings. A record enters the keyword ranking when its full-text
value holds
at least one of the keywords, **whatever the sign of its BM25 score**: the default IDF is zero for a
term in exactly half the records, and its floor for a commoner term is negative whenever the mean IDF
is, so a matched record can score zero or less — in a one-record collection it always does.

**A keyword outside the vocabulary contributes nothing** and does not fail, which is what BM25 means
by an unseen term. A collection whose full-text values yield no tokens at all — every word a stop
word, say — has a vocabulary of no terms and does not throw either: every search on it
degrades to the vector ranking alone, scored through the fusion.

**`Filter` and `Skip` apply after the fusion**, and since the vector ranking holds every record, the
fused ranking does too — so a filter never shortens the results below `top` for want of candidates.
`ScoreThreshold` is **refused, not ignored**: setting it throws `NotSupportedException`. A fused score
is a sum of `1 / (k + rank)` over the rankings, not a similarity — it depends on `k` and on a record's
rank in each list — so no threshold written for similarities means anything against it, and ignoring
one would hand the caller every result with nothing to say so. Cut the fused results by count with
`top` instead.

**Only vectors are accepted**, as in
[`LodestarVectorStoreCollection.SearchAsync`](lodestarvectorstorecollection-searchasync.md). The
method is reached on the concrete type or through `IKeywordHybridSearchable<TRecord>`, which is how a
consumer holding the abstraction's `VectorStoreCollection<TKey, TRecord>` asks for it.

**Applies to** — net10.0, netstandard2.0.

**See also** — [`LodestarVectorStoreCollection.SearchAsync`](lodestarvectorstorecollection-searchasync.md),
[`LodestarVectorStoreOptions`](lodestarvectorstoreoptions.md),
[`LodestarVectorStoreCollection`](lodestarvectorstorecollection.md).
