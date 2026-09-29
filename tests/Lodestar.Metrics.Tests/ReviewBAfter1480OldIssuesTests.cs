using Lodestar.Metrics.Internal;
using Xunit;

namespace Lodestar.Metrics.Tests;

/// <summary>
/// Older review findings of <c>Lodestar.Metrics</c> closed after #1480, one fact or theory each; the
/// expected values were measured on scikit-learn 1.9.1 and scipy's <c>gammaln</c>.
/// </summary>
public sealed class ReviewBAfter1480OldIssuesTests
{
    private const string NegativeDistance =
        "Negative values in data passed to `pairwise_distances`. Precomputed distance  need to have non-negative values..";

    [Fact]
    public void A_negative_precomputed_distance_is_refused_with_the_reference_sentence()
    {
        // silhouette_score(D, [0, 0, 1], metric='precomputed') raises; this returned 0.333 (#1275).
        double[] distances = [0, 0, -2, 0, 0, 3, -2, 3, 0];

        ArgumentException score = Assert.Throws<ArgumentException>(
            () => Silhouette.ScoreFromDistances([0, 0, 1], distances));
        ArgumentException perSample = Assert.Throws<ArgumentException>(
            () => Silhouette.PerSampleFromDistances([0, 0, 1], distances));

        Assert.StartsWith(NegativeDistance, score.Message, StringComparison.Ordinal);
        Assert.Equal("distances", perSample.ParamName);
    }

    [Fact]
    public void A_negative_diagonal_within_tolerance_is_refused_as_negative_rather_than_as_diagonal()
    {
        // -1e-15 on the diagonal passes the 100-ulp diagonal test, then check_non_negative refuses it (#1275).
        double[] distances = [-1e-15, 1, 2, 1, 0, 3, 2, 3, 0];

        ArgumentException refused = Assert.Throws<ArgumentException>(
            () => Silhouette.ScoreFromDistances([0, 0, 1], distances));

        Assert.StartsWith(NegativeDistance, refused.Message, StringComparison.Ordinal);
    }

    [Fact]
    public void The_label_count_is_refused_before_a_negative_distance()
    {
        // scikit-learn checks the number of labels before pairwise_distances sees the matrix (#1275).
        double[] distances = [0, 0, -2, 0, 0, 3, -2, 3, 0];

        ArgumentException refused = Assert.Throws<ArgumentException>(
            () => Silhouette.ScoreFromDistances([0, 0, 0], distances));

        Assert.StartsWith("Number of labels is 1.", refused.Message, StringComparison.Ordinal);
    }

    [Fact]
    public void A_negative_zero_distance_is_not_negative()
    {
        // -0.0 < 0 is false in numpy too: the reference scores this matrix 0.38888888888888884 (#1275).
        double score = Silhouette.ScoreFromDistances([0, 0, 1], [0, 1, 2, 1, 0, 3, 2, 3, -0.0]);

        Assert.Equal(0.38888888888888884, score, 1e-15);
    }

    [Theory]
    [InlineData(double.NaN, "Input contains NaN.")]
    [InlineData(double.PositiveInfinity, "Input contains infinity")]
    public void A_non_finite_score_is_refused_before_the_coverage_is_counted(double score, string message)
    {
        // The refusal is what made CoverageError's NaN branch unreachable, which #1278 deleted.
        ArgumentException refused = Assert.Throws<ArgumentException>(
            () => CoverageError.Score([true, false, false, true], [0.5, score, 0.2, 0.1], labelCount: 2));

        Assert.StartsWith(message, refused.Message, StringComparison.Ordinal);
    }

    [Fact]
    public void Coverage_counts_ties_at_the_lowest_relevant_score_as_ranked_above_it()
    {
        // coverage_error([[1,0,1,0]], [[0.5,0.5,0.2,0.2]]) is 4.0 on 1.9.1: the tie takes the worst rank (#1278).
        double coverage = CoverageError.Score([true, false, true, false], [0.5, 0.5, 0.2, 0.2], labelCount: 4);

        Assert.Equal(4.0, coverage);
    }

    [Fact]
    public void Two_sparse_labels_build_no_direct_table()
    {
        // Range 4,000,001 over two labels: the table cost 15.6 MB and 4 ms a call (#1280).
        LabelIndex implicitSet = LabelIndex.Create([0, 4_000_000], [0, 4_000_000], []);
        LabelIndex explicitSet = LabelIndex.Create([0, 4_000_000], [0, 4_000_000], [4_000_000, 0]);

        Assert.False(implicitSet.TryGetDirect(out _, out _));
        Assert.False(explicitSet.TryGetDirect(out _, out _));
        Assert.Equal(1, explicitSet.IndexOf(0));
        Assert.Equal(0, explicitSet.IndexOf(4_000_000));
        Assert.Equal(-1, explicitSet.IndexOf(1));
    }

    [Fact]
    public void A_dense_label_set_keeps_its_direct_table()
    {
        // Four labels over two samples are allowed 4 * (4 + 2) + 1024 slots; one more is refused (#1280).
        LabelIndex index = LabelIndex.Create([0, 1047], [5, 9], []);
        LabelIndex wider = LabelIndex.Create([0, 1048], [5, 9], []);

        Assert.True(index.TryGetDirect(out int[] table, out int min));
        Assert.False(wider.TryGetDirect(out _, out _));
        Assert.Equal(1048, table.Length);
        Assert.Equal(0, min);
    }

    [Fact]
    public void Spaced_labels_over_many_samples_keep_the_direct_table()
    {
        // Ten labels 1,000 apart over 100,000 samples: a binary search per sample cost 10x the table (#1280).
        int[] yTrue = [.. Enumerable.Range(0, 100_000).Select(i => (i % 10) * 1_000)];

        Assert.True(LabelIndex.Create(yTrue, yTrue, []).TryGetDirect(out _, out _));
    }

    [Fact]
    public void Sparse_labels_count_the_matrix_a_dense_relabelling_counts()
    {
        // Through the binary-search path, the cells are the ones the direct table gave (#1280).
        int[] yTrue = [.. Enumerable.Range(0, 2_000).Select(i => (i * 7) % 6)];
        int[] yPred = [.. Enumerable.Range(0, 2_000).Select(i => ((i * i) + (i / 3)) % 6)];
        int[] sparseTrue = [.. yTrue.Select(label => label * 1_000_000)];
        int[] sparsePred = [.. yPred.Select(label => label * 1_000_000)];

        Assert.Equal(
            ConfusionMatrix.Compute(yTrue, yPred).ToArray(),
            ConfusionMatrix.Compute(sparseTrue, sparsePred).ToArray());
    }

    [Fact]
    public void An_explicit_label_set_longer_than_an_array_is_refused_rather_than_wrapped()
    {
        // requested plus extra summed in int wrapped negative past the largest array (#1539).
        ArgumentException refused = Assert.Throws<ArgumentException>(
            () => LabelIndex.UnionLength(int.MaxValue, 2, "labels"));

        Assert.Equal("labels", refused.ParamName);
        Assert.Equal(7, LabelIndex.UnionLength(3, 4, "labels"));
    }

    [Theory]
    [InlineData(2, 0.6931471805599453)]
    [InlineData(1000, 5912.128178488164)]
    [InlineData(20000, 178075.6217371987)]
    [InlineData(79999, 823177.8271410995)]
    [InlineData(80000, 823189.1169230132)]
    [InlineData(123456, 1323904.4924837977)]
    [InlineData(159999, 1757263.5825490612)]
    [InlineData(160000, 1757275.5654781554)]
    public void The_log_factorial_table_holds_gammaln_at_a_large_sample_count(int k, double gammaln)
    {
        // scipy.special.gammaln(k + 1); the plain prefix sum was 8.2e-9 off at 160,000 and 1.2e-8 at worst (#1281).
        double[] table = ExpectedMutualInformation.LogFactorials(160_000);

        Assert.Equal(gammaln, table[k], (Math.BitIncrement(gammaln) - gammaln) * 4);
    }

    [Fact]
    public void The_expected_mutual_information_of_a_large_clustering_matches_the_reference()
    {
        // expected_mutual_information over marginals [40000, 50000, 70000] x [60000, 100000] (#1281).
        double emi = ExpectedMutualInformation.Compute([40_000, 50_000, 70_000], [60_000, 100_000], 160_000);

        Assert.Equal(6.250090235130628e-06, emi, 6.250090235130628e-06 * 1e-9);
    }
}
