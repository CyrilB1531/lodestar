using Lodestar.Cluster;
using Lodestar.Decomposition;
using Lodestar.Embeddings.Search;
using Lodestar.Embeddings.Tokenization;
using Lodestar.Fuzzy;
using Lodestar.Metrics;
using Lodestar.Preprocessing;
using Lodestar.Stats;
using Lodestar.Stats.Regression;
using Lodestar.Stats.Regression.Instrumental;
using Lodestar.Stats.Regression.Panel;
using Lodestar.Survival;
using Lodestar.Text.Keywords;
using Lodestar.Text.Search;
using Lodestar.Text.Vectorization;
using Xunit;

namespace Lodestar.Abstractions.Tests;

/// <summary>The records hash as they compare, every NaN alike and an absent member included (#1284, #1285).</summary>
/// <remarks>
/// net10 hashes every NaN alike on its own, so bit patterns alone cannot fail here; each record is
/// also held to hashing a NaN as <c>0.0</c>, which net10's own hash does not (#1297).
/// </remarks>
public sealed class RecordHashTests
{
    [Fact]
    public void An_anderson_result_with_absent_tables_hashes_and_compares()
    {
        var left = new AndersonResult(1.0, 0.5, null!, null!);
        var right = new AndersonResult(1.0, 0.5, null!, null!);

        Assert.Equal(left, right);
        Assert.Equal(left.GetHashCode(), right.GetHashCode());
    }

    [Fact]
    public void A_tokenization_result_with_absent_lists_hashes_and_compares()
    {
        var left = new TokenizationResult(null!, null!);
        var right = new TokenizationResult(null!, null!);

        Assert.Equal(left, right);
        Assert.Equal(left.GetHashCode(), right.GetHashCode());
        Assert.NotEqual(left, new TokenizationResult([], []));
    }

    [Fact]
    public void A_tokenization_result_compares_its_lists_element_by_element()
    {
        var left = new TokenizationResult(["hel", "##lo"], [7, 8]);

        Assert.Equal(left, new TokenizationResult(["hel", "##lo"], [7, 8]));
        Assert.NotEqual(left, new TokenizationResult(["hel", "##lo"], [7, 9]));
        Assert.NotEqual(left, new TokenizationResult(["hel", "##LO"], [7, 8]));
    }

    [Fact]
    public void An_anderson_result_hashes_every_nan_alike() => AssertNaNStable(n => new AndersonResult(n, n, [1.0], [0.05]));

    [Fact]
    public void A_contingency_result_hashes_every_nan_alike() => AssertNaNStable(n => new ChiSquaredContingencyResult(n, n, 1, [[1.0]]));

    [Fact]
    public void Text_rank_options_hash_every_nan_alike() => AssertNaNStable(n => new TextRankOptions { Damping = n, Tolerance = n, Ratio = n });

    [Fact]
    public void Count_vectorizer_options_hash_every_nan_alike() => AssertNaNStable(n => new CountVectorizerOptions { MinDf = n, MaxDf = n });

    [Fact]
    public void K_means_options_hash_every_nan_alike() => AssertNaNStable(n => new KMeansOptions { Tolerance = n });

    [Fact]
    public void Nmf_options_hash_every_nan_alike() => AssertNaNStable(n => new NmfOptions { Tolerance = n });

    [Fact]
    public void A_survival_curve_hashes_every_nan_alike() => AssertNaNStable(n => new SurvivalCurve([], [], [], [], n));

    [Fact]
    public void A_kaplan_meier_curve_hashes_every_nan_alike() => AssertNaNStable(n => new KaplanMeierCurve([], [], [], [], n));

    [Fact]
    public void A_sentence_piece_hashes_every_nan_alike() => AssertNaNStable(n => new SentencePiece("▁the", n, 3));

    [Fact]
    public void An_extract_result_hashes_every_nan_alike() => AssertNaNStable(n => new ExtractResult("apple", n, 0));

    [Fact]
    public void An_average_row_hashes_every_nan_alike() => AssertNaNStable(n => new AverageRow("macro avg", n, n, n, n));

    [Fact]
    public void A_class_row_hashes_every_nan_alike() => AssertNaNStable(n => new ClassRow(1, "one", n, n, n, n));

    [Fact]
    public void A_ks_result_hashes_every_nan_alike() => AssertNaNStable(n => new KsResult(n, n, n, 1));

    [Fact]
    public void A_wald_test_hashes_every_nan_alike() => AssertNaNStable(n => new WaldTest(n, n, 2, null));

    [Fact]
    public void A_test_result_hashes_every_nan_alike() => AssertNaNStable(n => new Lodestar.Stats.TestResult(n, n));

    [Fact]
    public void A_log_rank_result_hashes_every_nan_alike() => AssertNaNStable(n => new LogRankResult(n, n, 1));

    [Fact]
    public void A_restricted_mean_result_hashes_every_nan_alike() => AssertNaNStable(n => new RestrictedMeanResult(n, n));

    [Fact]
    public void A_survival_step_hashes_every_nan_alike() => AssertNaNStable(n => new SurvivalStep(n, 3, 1, 0));

    [Fact]
    public void A_keyword_match_hashes_every_nan_alike() => AssertNaNStable(n => new KeywordMatch("sparse matrix", n));

    [Fact]
    public void A_search_hit_hashes_every_nan_alike() => AssertNaNStable(n => new SearchHit(4, n));

    [Fact]
    public void Bm25_options_hash_every_nan_alike() => AssertNaNStable(n => new Bm25Options(n, n, Bm25Idf.RobertsonFloored, n));

    [Fact]
    public void Min_max_scaler_options_hash_every_nan_alike() => AssertNaNStable(n => new MinMaxScalerOptions { Low = n, High = n });

    [Fact]
    public void One_hot_encoder_options_hash_every_nan_alike() => AssertNaNStable(n => new OneHotEncoderOptions { MinFrequencyShare = n });

    [Fact]
    public void Robust_scaler_options_hash_every_nan_alike() => AssertNaNStable(n => new RobustScalerOptions { LowerPercentile = n, UpperPercentile = n });

    [Fact]
    public void Simple_imputer_options_hash_every_nan_alike() => AssertNaNStable(n => new SimpleImputerOptions { FillValue = n });

    [Fact]
    public void Iv_options_hash_every_nan_alike() => AssertNaNStable(n => new IvOptions { ConfidenceLevel = n, Fuller = n });

    [Fact]
    public void Panel_options_hash_every_nan_alike() => AssertNaNStable(n => new PanelOptions { ConfidenceLevel = n });

    [Fact]
    public void A_search_result_hashes_every_nan_alike()
    {
        // A float Score, which #1285's double overload did not reach (#1294).
        Assert.Equal(new SearchResult(2, 0f).GetHashCode(), new SearchResult(2, float.NaN).GetHashCode());
        Assert.Equal(
            new SearchResult(2, float.NaN).GetHashCode(),
            new SearchResult(2, BitConverter.Int32BitsToSingle(unchecked((int)0xFFC0_0001))).GetHashCode());
    }

    [Fact]
    public void Options_with_an_absent_token_pattern_hash_and_compare()
    {
        // Equals compared a null pattern; the hash threw on it (#1296).
        Assert.Equal(new RakeOptions { TokenPattern = null! }.GetHashCode(), new RakeOptions { TokenPattern = null! }.GetHashCode());
        Assert.Equal(new TextRankOptions { TokenPattern = null! }.GetHashCode(), new TextRankOptions { TokenPattern = null! }.GetHashCode());
        Assert.Equal(
            new CountVectorizerOptions { TokenPattern = null! }.GetHashCode(),
            new CountVectorizerOptions { TokenPattern = null! }.GetHashCode());
    }

    [Fact]
    public void Log_rank_options_hash_every_nan_alike() => AssertNaNStable(n => new LogRankOptions { P = n, Q = n, Truncation = n });

    // Every NaN double.Equals makes equal, whatever its sign, payload or signalling bit.
    private static readonly long[] NaNPatterns =
        [0x7FF8_0000_0000_0000, unchecked((long)0xFFF8_0000_0000_0000), 0x7FF8_0000_0000_0001, 0x7FF0_0000_0000_0001];

    private static void AssertNaNStable<T>(Func<double, T> make)
        where T : IEquatable<T>
    {
        // net10 hashes NaN to 0x7FF00000 and 0.0 to 0; ValueEquality.HashOf sends NaN to 0 (#1297).
        Assert.Equal(make(0.0).GetHashCode(), make(double.NaN).GetHashCode());

        T first = make(BitConverter.Int64BitsToDouble(NaNPatterns[0]));
        foreach (long bits in NaNPatterns)
        {
            T other = make(BitConverter.Int64BitsToDouble(bits));
            Assert.True(first.Equals(other));
            Assert.Equal(first.GetHashCode(), other.GetHashCode());
        }
    }
}
