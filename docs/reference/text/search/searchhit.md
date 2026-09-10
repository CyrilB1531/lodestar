# SearchHit

One document and the score that ranked it.

<!-- docs-declaration -->

```csharp
public sealed record SearchHit(int Document, double Score)
```

**Parameters** — `Document` is the row index in the matrix that was scored. `Score` is higher-is-
better, on the scale of whatever produced it.

**Example** — the shape both [`Bm25Index.Top`](bm25index-top.md) and
[`RankFusion.Rrf`](rankfusion.md) return.

```csharp
using Lodestar.Text.Search;

IReadOnlyList<SearchHit> fused = RankFusion.Rrf([[2, 0, 1], [0, 1, 2]]);

int best = fused[0].Document;  // => 0
int howMany = fused.Count;  // => 3
```

**Remarks** — `Document` is an **index, not an identifier**: it means a row of the matrix that was
scored, and mapping it back to a document of your own is the caller's job.

**The two producers put different things in `Score`.** BM25's is unbounded and can be negative;
RRF's is a sum of reciprocals in `(0, rankings × 1/(k+1)]`. Neither is comparable with the other,
which is exactly why fusing them goes through ranks rather than through scores.

**Applies to** — net10.0, netstandard2.0.

**See also** — [`Bm25Index.Top`](bm25index-top.md), [`RankFusion`](rankfusion.md).
