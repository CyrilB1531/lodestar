using Lodestar.Abstractions;
using Lodestar.Text.Distances;
using Lodestar.Text.Indexing;
using Lodestar.Text.Keywords;
using Lodestar.Text.Search;
using Lodestar.Text.Similarity;
using Lodestar.Text.Vectorization;
using Xunit;

namespace Lodestar.Text.Tests;

/// <summary>
/// The refusals #901 grouped: each names the caller's parameter rather than an internal
/// expression, and a value no comparison holds for (NaN) or one that overflows is refused too.
/// </summary>
public sealed class ArgumentValidationTests
{
    [Fact]
    public void Bm25_names_options_for_a_non_finite_setting_epsilon_included()
    {
        CsrMatrix counts = new CountVectorizer().FitTransform(["the cat", "the dog"]);

        var k1 = Assert.Throws<ArgumentOutOfRangeException>(() => new Bm25Index(counts, new Bm25Options(K1: double.NaN)));
        var epsilon = Assert.Throws<ArgumentOutOfRangeException>(() => new Bm25Index(counts, new Bm25Options(Epsilon: double.PositiveInfinity)));

        Assert.Equal("options", k1.ParamName);
        Assert.Equal("options", epsilon.ParamName);
    }

    [Theory]
    [InlineData(0, 0.85, 0.5, 1e-12)]
    [InlineData(2, double.NaN, 0.5, 1e-12)]
    [InlineData(2, 0.85, double.NaN, 1e-12)]
    [InlineData(2, 0.85, 0.5, -1.0)]
    [InlineData(2, 0.85, 0.5, double.NaN)]
    [InlineData(2, 0.85, 0.5, double.PositiveInfinity)]
    public void TextRank_refuses_each_option_out_of_range_naming_options(int window, double damping, double ratio, double tolerance)
    {
        var options = new TextRankOptions { Window = window, Damping = damping, Ratio = ratio, Tolerance = tolerance };

        var error = Assert.Throws<ArgumentOutOfRangeException>(() => new TextRank(options));

        Assert.Equal("options", error.ParamName);
    }

    [Fact]
    public void Rake_names_options_for_a_min_length_below_one()
    {
        var error = Assert.Throws<ArgumentOutOfRangeException>(() => new Rake(new RakeOptions { MinLength = 0 }));

        Assert.Equal("options", error.ParamName);
    }

    [Fact]
    public void LshIndex_names_banding_and_refuses_a_permutation_count_past_int()
    {
        var zero = Assert.Throws<ArgumentOutOfRangeException>(() => new LshIndex(new LshBanding(0, 4)));
        var wrapped = Assert.Throws<ArgumentOutOfRangeException>(() => new LshIndex(new LshBanding(65_536, 65_536)));

        Assert.Equal("banding", zero.ParamName);
        Assert.Equal("banding", wrapped.ParamName);
    }

    [Fact]
    public void BkTree_names_limit()
    {
        BkTree tree = BkTree.OverLevenshtein();
        tree.Add("cat");

        var error = Assert.Throws<ArgumentOutOfRangeException>(() => tree.WithinDistance("cat", 1, limit: -1));

        Assert.Equal("limit", error.ParamName);
    }

    [Fact]
    public void MinHash_and_SimHash_name_the_sequence_holding_a_null_token()
    {
        var minHash = new MinHash(new MinHashPermutations([1UL, 3UL, 5UL, 7UL], [0UL, 1UL, 2UL, 3UL]));

        var signature = Assert.Throws<ArgumentNullException>(() => minHash.Signature(["a", null!]));
        var tokens = Assert.Throws<ArgumentNullException>(() => SimHash.Fingerprint(["a", null!]));
        var weighted = Assert.Throws<ArgumentNullException>(
            () => SimHash.Fingerprint([new KeyValuePair<string, int>(null!, 1)]));

        Assert.Equal("tokens", signature.ParamName);
        Assert.Equal("tokens", tokens.ParamName);
        Assert.Equal("weighted", weighted.ParamName);
    }

    [Fact]
    public void MinHashPermutations_refuses_an_index_outside_the_set_as_documented()
    {
        MinHashPermutations permutations = new MinHashPermutations([1UL, 3UL, 5UL, 7UL], [0UL, 1UL, 2UL, 3UL]);

        Assert.Throws<ArgumentOutOfRangeException>(() => permutations.Multiplier(4));
        Assert.Throws<ArgumentOutOfRangeException>(() => permutations.Addend(-1));
    }

    [Fact]
    public void RankFusion_keeps_the_ranking_at_the_largest_k()
    {
        int[] ranking = [7, 8];
        IReadOnlyList<SearchHit> fused = RankFusion.Rrf([ranking], k: int.MaxValue);

        Assert.Equal([7, 8], fused.Select(hit => hit.Document));
        Assert.True(fused[0].Score > 0);
    }

    [Fact]
    public void DamerauLevenshtein_refuses_a_table_past_the_largest_array()
    {
        string side = new('a', 46_341);

        Assert.Throws<ArgumentException>(() => DamerauLevenshtein.Distance(side, side));
    }
}
