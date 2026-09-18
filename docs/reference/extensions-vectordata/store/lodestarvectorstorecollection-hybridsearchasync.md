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
`top` is less than 1. `ArgumentException` when the query is not the collection's vector width, an
empty collection included, or when a held record's vector was changed in place to another width
since it was written.
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

**Remarks** — three published pieces, joined and nothing added:

1. the vector ranking of **every** record, from [`EmbeddingIndex.Search`](../../embeddings/search/embeddingindex-search.md);
2. the keyword ranking, from [`Bm25Index.Score`](../../text/search/bm25index-score.md), over the
   vocabulary a [`CountVectorizer`](../../text/vectorizers/countvectorizer.md) fitted on the
   full-text values during the same rebuild — so a query is tokenized exactly as the records were;
3. the fusion, [`RankFusion.Rrf`](../../text/search/rankfusion-rrf.md), at
   [`LodestarVectorStoreOptions`](lodestarvectorstoreoptions.md)' `RankFusionK`, 60 by default.

**The keyword ranking holds only the records the keywords matched**, ordered by score descending and
then by index, which is [`Bm25Index.Top`](../../text/search/bm25index-top.md)'s own order over that
subset. `RankFusion.Rrf` reads a ranking's positions rather than its scores, so passing every
document through would hand an unmatched record credit for the order it was inserted in. Only the
matched records are sorted, read from the queried terms' postings rather than from a pass over every
stored count; scoring them still costs one `double` per record of the collection per query, since
`Bm25Index.Score` returns every record's score. A record enters the keyword ranking when its full-text
value holds
at least one of the keywords, **whatever the sign of its BM25 score**: the default IDF is zero for a
term in exactly half the records, and its floor for a commoner term is negative whenever the mean IDF
is, so a matched record can score zero or less — in a one-record collection it always does.

**A keyword outside the vocabulary contributes nothing** and does not fail, which is what BM25 means
by an unseen term. A collection whose full-text values yield no tokens at all — every word a stop
word, say — builds a keyword index over no terms and does not throw either: every search on it
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
