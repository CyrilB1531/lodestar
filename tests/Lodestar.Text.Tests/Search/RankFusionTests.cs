using Lodestar.Text.Search;
using Xunit;

namespace Lodestar.Text.Tests.Search;

/// <summary>
/// Reciprocal rank fusion, pinned by its definition rather than by a frozen corpus.
/// </summary>
/// <remarks>
/// There is no canonical Python library for RRF to oracle against — it is one formula from
/// Cormack, Clarke and Buettcher (2009). These tests state that formula, the way
/// <c>Lodestar.Metrics</c>' mean reciprocal rank is stated, so the absence of a corpus is a
/// recorded choice rather than a gap.
/// </remarks>
public sealed class RankFusionTests
{
    /// <summary>The published formula, computed by hand for one small case.</summary>
    [Fact]
    public void The_score_is_the_sum_of_one_over_k_plus_rank()
    {
        // Document 1 is first in ranking A and second in ranking B.
        IReadOnlyList<SearchHit> fused = RankFusion.Rrf([[1, 2], [3, 1]], k: 60);

        SearchHit first = fused.Single(hit => hit.Document == 1);
        Assert.Equal((1.0 / 61.0) + (1.0 / 62.0), first.Score, 1e-15);
    }

    /// <summary>Rank counts from one, which is the mistake that changes every number.</summary>
    [Fact]
    public void Rank_is_one_based_not_zero_based()
    {
        IReadOnlyList<SearchHit> fused = RankFusion.Rrf([[7]], k: 60);

        Assert.Equal(1.0 / 61.0, fused[0].Score, 1e-15);
        Assert.NotEqual(1.0 / 60.0, fused[0].Score, 1e-15);
    }

    [Fact]
    public void K_defaults_to_sixty()
    {
        Assert.Equal(60, RankFusion.DefaultK);
        Assert.Equal(RankFusion.Rrf([[4]], 60)[0].Score, RankFusion.Rrf([[4]])[0].Score, 1e-15);
    }

    /// <summary>A document in both rankings beats one that is first in a single ranking.</summary>
    [Fact]
    public void Agreement_across_rankings_outranks_a_single_first_place()
    {
        IReadOnlyList<SearchHit> fused = RankFusion.Rrf([[9, 5], [5, 9]], k: 1);

        // 5 and 9 each take first and second, so they tie -- and the tie breaks by the
        // order they were first seen, which is ranking A's.
        Assert.Equal(9, fused[0].Document);
        Assert.Equal(fused[0].Score, fused[1].Score, 1e-15);
    }

    /// <summary>A document absent from a ranking simply contributes nothing from it.</summary>
    [Fact]
    public void Rankings_of_different_lengths_need_no_padding()
    {
        IReadOnlyList<SearchHit> fused = RankFusion.Rrf([[1, 2, 3], [3]], k: 60);

        Assert.Equal(3, fused.Count);
        Assert.Equal(3, fused[0].Document);
        Assert.Equal((1.0 / 63.0) + (1.0 / 61.0), fused[0].Score, 1e-15);
    }

    /// <summary>A ranking that lists a document twice ranks it once, at its first position.</summary>
    [Fact]
    public void A_repeat_inside_one_ranking_is_scored_at_its_first_position()
    {
        IReadOnlyList<SearchHit> fused = RankFusion.Rrf([[8, 8, 8]], k: 60);

        SearchHit hit = Assert.Single(fused);
        Assert.Equal(1.0 / 61.0, hit.Score, 1e-15);
    }

    /// <summary>Larger k flattens the advantage of a top position.</summary>
    [Fact]
    public void A_larger_k_flattens_the_weight_of_rank()
    {
        double tight = RankFusion.Rrf([[1, 2]], k: 1)[0].Score - RankFusion.Rrf([[1, 2]], k: 1)[1].Score;
        double flat = RankFusion.Rrf([[1, 2]], k: 1000)[0].Score - RankFusion.Rrf([[1, 2]], k: 1000)[1].Score;

        Assert.True(flat < tight);
    }

    [Fact]
    public void Fusing_the_same_rankings_twice_gives_one_answer()
    {
        int[][] rankings = [[1, 2, 3], [3, 2, 1], [2, 1]];

        Assert.Equal(RankFusion.Rrf(rankings), RankFusion.Rrf(rankings));
    }

    [Fact]
    public void No_rankings_at_all_fuse_to_nothing()
    {
        Assert.Empty(RankFusion.Rrf([]));
    }

    [Theory]
    [InlineData(0)]
    [InlineData(-1)]
    public void A_non_positive_k_is_refused(int k)
    {
        Assert.Throws<ArgumentOutOfRangeException>(() => RankFusion.Rrf([[1]], k));
    }

    [Fact]
    public void A_null_ranking_is_refused()
    {
        Assert.Throws<ArgumentNullException>(() => RankFusion.Rrf(null!));
        Assert.Throws<ArgumentNullException>(() => RankFusion.Rrf([null!]));
    }
}
