# RankFusion

Combining several rankings of the same documents into one.

<!-- docs-declaration -->

```csharp
public static class RankFusion
```

**Example** — two rankings that disagree, fused.

```csharp
using Lodestar.Text.Search;

IReadOnlyList<SearchHit> fused = RankFusion.Rrf([[2, 0, 1], [0, 1, 2]]);

int best = fused[0].Document;  // => 0
int worst = fused[2].Document;  // => 1
double k = RankFusion.DefaultK;  // => 60
```

**Remarks** — reciprocal rank fusion, from Cormack, Clarke and Buettcher (2009). It is what makes a
hybrid search: fuse a BM25 ranking with a vector one and **neither side's scale has to be
reconciled**, because only positions are read. That is the whole reason to prefer it over
normalising two score distributions and adding them.

**A last place costs more than a first place gains.** Document 2 is ranked first and then last;
document 0 is ranked second and then first, and wins — `1/61 + 1/63` is less than `1/62 + 1/61`.
That asymmetry is the whole behaviour of `k`: at 60 the gap between consecutive ranks is small and
consistency beats a single strong opinion, and lowering `k` reverses it.

**There is no canonical Python library to check this against.** It is one formula, so it is pinned
by tests that state it — the same way `Lodestar.Metrics`' mean reciprocal rank is pinned — rather
than by a frozen corpus. `docs/equivalence.md` records that exception instead of leaving the row
looking skipped.

Thread-safe.

**Applies to** — net10.0, netstandard2.0.

**See also** — [the search index](../search.md), [`Bm25Index.Top`](bm25index-top.md).

## Members

| Member | What it does |
| --- | --- |
| [`RankFusion.Rrf`](rankfusion-rrf.md) | Fuses two or more rankings by reciprocal rank. |
