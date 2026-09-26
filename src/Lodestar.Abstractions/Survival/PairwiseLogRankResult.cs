namespace Lodestar.Survival;

/// <summary>One pair of groups from <c>LogRank.Pairwise</c>, lifelines' <c>pairwise_logrank_test</c> row.</summary>
/// <param name="GroupA">The lower of the two group labels.</param>
/// <param name="GroupB">The higher of the two group labels.</param>
/// <param name="Result">The two-sample test between them.</param>
public sealed record PairwiseLogRankResult(int GroupA, int GroupB, LogRankResult Result);
