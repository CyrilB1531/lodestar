# PairwiseLogRankResult

One pair of groups from [`LogRank.Pairwise`](logrank-pairwise.md): a row of lifelines'
`pairwise_logrank_test` summary.

<!-- docs-declaration -->

```csharp
public sealed record PairwiseLogRankResult(int GroupA, int GroupB, LogRankResult Result)
```

**Properties** — `GroupA` is the lower of the two labels and `GroupB` the higher. `Result` is the
two-sample [`LogRankResult`](logrankresult.md) between them, on one degree of freedom.

**Example** — two groups make one pair.

```csharp
using Lodestar.Survival;

PairwiseLogRankResult pair = LogRank.Pairwise(
    [1, 2, 3, 4], [7, 7, 2, 2], [true, true, true, true])[0];

int lower = pair.GroupA;   // => 2
int higher = pair.GroupB;  // => 7
```

**Remarks** — value equality compares the labels and the result, the generated record equality.

**Applies to** — net10.0, netstandard2.0.

**See also** — [`LogRank.Pairwise`](logrank-pairwise.md).
