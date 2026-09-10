# RankFusion.Rrf

Fuses two or more rankings by reciprocal rank.

<!-- docs-declaration -->

```csharp
public static IReadOnlyList<SearchHit> Rrf(IEnumerable<IEnumerable<int>> rankings, int k = 60)
```

**Parameters** — `rankings` is one sequence of document identifiers per ranking, each ordered best
first. `k` is the rank offset, defaulting to `RankFusion.DefaultK`, and must be positive.

**Returns** — `IReadOnlyList<SearchHit>` holding every document that appeared in any ranking, best
fused score first.

**Exceptions** — `ArgumentNullException` when `rankings`, or one of them, is null;
`ArgumentOutOfRangeException` when `k` is not positive.

**Example** — the formula, on a document that appears in both rankings.

```csharp
using Lodestar.Text.Search;

IReadOnlyList<SearchHit> fused = RankFusion.Rrf([[1, 2], [3, 1]], k: 60);

// Document 1 is first in one ranking and second in the other: 1/61 + 1/62.
double score = fused[0].Score;  // => 0.03252…
int document = fused[0].Document;  // => 1
int total = fused.Count;  // => 3
```

**Remarks** — the score of a document is `Σ 1 / (k + rank)` over the rankings that hold it.

**Rank counts from 1.** Counting from zero is the common mistake and it changes every number: at
`k = 60` the first position would be worth `1/60` rather than `1/61`.

**A document absent from a ranking contributes nothing from it**, which is what lets rankings of
different lengths be fused without padding — a BM25 top-10 and a vector top-100 need no alignment
step.

**A document repeated inside one ranking is scored at its first position there.** The later
occurrences are ignored rather than summed, because a ranking that lists a document twice still
ranks it once.

**Larger `k` flattens the weight of rank.** At `k = 1` first place is worth `1/2` and second `1/3`;
at `k = 1000` the two are within a thousandth of each other. 60 is the published default and is a
compromise, not a derivation.

Ties break by score alone, and among equal scores the order is the order the documents were first
seen — stable, so fusing the same rankings twice gives one answer.

**Applies to** — net10.0, netstandard2.0.

**See also** — [`RankFusion`](rankfusion.md), [`SearchHit`](searchhit.md).
