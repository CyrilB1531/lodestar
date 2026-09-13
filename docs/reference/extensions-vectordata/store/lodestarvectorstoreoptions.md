# LodestarVectorStoreOptions

How a collection builds its keyword half, and the offset reciprocal-rank fusion uses.

<!-- docs-declaration -->

```csharp
public sealed class LodestarVectorStoreOptions
```

**Properties** — all three are `init`-only, and each defaults to what the member it configures does
on its own.

- `Vectorizer` is the [`CountVectorizerOptions`](../../text/vectorizers/countvectorizeroptions.md)
  the full-text values are tokenized and counted with, and the query keywords with them. `null`, the
  default, takes `CountVectorizer`'s defaults — scikit-learn's, single-letter words dropped.
- `Bm25` is the [`Bm25Options`](../../text/search/bm25options.md) the keyword index scores with.
  `null`, the default, takes `rank_bm25`'s `k1 = 1.5`, `b = 0.75`.
- `RankFusionK` is the `k` in `1 / (k + rank)` that
  [`LodestarVectorStoreCollection.HybridSearchAsync`](lodestarvectorstorecollection-hybridsearchasync.md)
  fuses with, `60` by default — [`RankFusion.DefaultK`](../../text/search/rankfusion.md). Setting it
  to zero or below throws `ArgumentOutOfRangeException`.

**Example** — the defaults, and a store whose keyword half drops English stop words.

```csharp
using Lodestar.Extensions.VectorData;
using Lodestar.Text.Vectorization;

var defaults = new LodestarVectorStoreOptions();
int k = defaults.RankFusionK;  // => 60

var english = new LodestarVectorStoreOptions
{
    Vectorizer = new CountVectorizerOptions { StopWords = StopWords.English },
    RankFusionK = 20,
};
using var store = new LodestarVectorStore(english);

int tighter = english.RankFusionK;  // => 20
```

**Remarks** — **`RankFusionK` decides how much one ranking's first place is worth against agreement
between the two.** At `k = 1`, a record first in one ranking and absent from the other (`1/2`)
outscores a record fourth in both (`2/5`). At `k = 60` the same record (`1/61`) loses to one ranked
anywhere up to 61st in both, and ties one ranked 62nd in both. Sixty is the value Cormack et al.
(2009) used, and the one `RankFusion.Rrf` takes by default.

The options are **read when used**, at each rebuild and each hybrid search, rather than copied at
construction. Every property is `init`-only, so the difference only shows if a
`CountVectorizerOptions` handed in holds a stop-word collection its caller later mutates.

A store passes one options object to every collection it creates. A collection that needs different
options is constructed directly, with its own.

**Applies to** — net10.0, netstandard2.0.

**See also** — [`LodestarVectorStore`](lodestarvectorstore.md),
[`LodestarVectorStoreCollection`](lodestarvectorstorecollection.md),
[`Bm25Index`](../../text/search/bm25index.md).
