using Lodestar.Stats;
using Xunit;

namespace Lodestar.Stats.Regression.Tests;

/// <summary>The identities HAC and cluster reduce to at their boundaries, and the options each refuses (#775).</summary>
/// <remarks>
/// The corpora hold both at 1e-9 against statsmodels. These hold what a corpus does not reach: that zero lags is HC0,
/// that one cluster per row is HC0 or HC1 depending on the correction, and that a label is only a name.
/// </remarks>
public sealed class HacClusterEdgeTests
{
    private static readonly double[] Design =
        [1.0, 0.5, 2.0, 1.5, 3.0, 0.7, 4.0, 2.2, 5.0, 0.9, 6.0, 1.1, 7.0, 2.6, 8.0, 0.4, 9.0, 1.8, 10.0, 2.0];

    private static readonly double[] Response = [2.9, 3.1, 7.4, 6.2, 11.8, 9.9, 16.1, 12.7, 19.5, 18.3];

    private static readonly double[] Weights = [1.0, 2.0, 0.5, 1.5, 3.0, 1.0, 0.25, 2.0, 0.75, 1.25];

    private static readonly int[] Clusters = [4, 4, 9, 9, 9, 2, 2, 7, 7, 7];

    private static readonly OlsOptions ClusterOptions = new() { CovarianceType = CovarianceType.Cluster };

    private static void AssertSameErrors(OlsSummary expected, OlsSummary actual)
    {
        for (int j = 0; j < expected.StandardErrors.Count; j++)
        {
            Assert.Equal(expected.StandardErrors[j], actual.StandardErrors[j], 1e-12);
        }
    }

    [Fact]
    public void Hac_with_no_lags_is_hc0()
    {
        OlsSummary white = OrdinaryLeastSquares.Fit(
            Design, Response, 2, new OlsOptions { CovarianceType = CovarianceType.Hc0 });
        OlsSummary hac = OrdinaryLeastSquares.Fit(
            Design, Response, 2, new OlsOptions { CovarianceType = CovarianceType.Hac, HacLags = 0 });

        AssertSameErrors(white, hac);
    }

    [Fact]
    public void Corrected_hac_with_no_lags_is_hc1()
    {
        OlsSummary hc1 = WeightedLeastSquares.Fit(
            Design, Response, Weights, 2, new OlsOptions { CovarianceType = CovarianceType.Hc1 });
        OlsSummary hac = WeightedLeastSquares.Fit(
            Design,
            Response,
            Weights,
            2,
            new OlsOptions { CovarianceType = CovarianceType.Hac, HacLags = 0, SmallSampleCorrection = true });

        AssertSameErrors(hc1, hac);
    }

    [Fact]
    public void Lags_past_the_last_row_still_move_the_weights()
    {
        // Both reach every pair of rows; the Bartlett weights divide by L + 1, so the two differ, as in statsmodels.
        OlsSummary nine = OrdinaryLeastSquares.Fit(
            Design, Response, 2, new OlsOptions { CovarianceType = CovarianceType.Hac, HacLags = 9 });
        OlsSummary twelve = OrdinaryLeastSquares.Fit(
            Design, Response, 2, new OlsOptions { CovarianceType = CovarianceType.Hac, HacLags = 12 });

        Assert.NotEqual(nine.StandardErrors[1], twelve.StandardErrors[1]);
    }

    [Fact]
    public void One_cluster_per_row_is_hc1_corrected_and_hc0_uncorrected()
    {
        // G = n turns G/(G−1)·(n−1)/(n−k) into n/(n−k), HC1's own factor.
        int[] own = [.. Enumerable.Range(0, 10)];
        OlsSummary hc0 = OrdinaryLeastSquares.Fit(
            Design, Response, 2, new OlsOptions { CovarianceType = CovarianceType.Hc0 });
        OlsSummary hc1 = OrdinaryLeastSquares.Fit(
            Design, Response, 2, new OlsOptions { CovarianceType = CovarianceType.Hc1 });

        AssertSameErrors(hc1, OrdinaryLeastSquares.Fit(Design, Response, own, 2, ClusterOptions));
        AssertSameErrors(
            hc0,
            OrdinaryLeastSquares.Fit(Design, Response, own, 2, ClusterOptions with { SmallSampleCorrection = false }));
    }

    [Fact]
    public void Relabelling_the_clusters_changes_nothing()
    {
        int[] renamed = [.. Clusters.Select(label => -1000 + (label * 37))];

        OlsSummary original = WeightedLeastSquares.Fit(Design, Response, Weights, Clusters, 2, ClusterOptions);
        OlsSummary relabelled = WeightedLeastSquares.Fit(Design, Response, Weights, renamed, 2, ClusterOptions);

        Assert.Equal(original.StandardErrors, relabelled.StandardErrors);
        Assert.Equal(original.FPValue, relabelled.FPValue);
    }

    [Fact]
    public void A_cluster_fit_reads_its_f_test_on_one_less_than_the_cluster_count()
    {
        OlsSummary summary = OrdinaryLeastSquares.Fit(Design, Response, Clusters, 2, ClusterOptions);

        Assert.Equal(Distributions.FisherSf(summary.FStatistic, 2, 3), summary.FPValue, 1e-15);
        Assert.Equal(7, summary.ResidualDegreesOfFreedom);
    }

    [Fact]
    public void Hac_without_lags_is_refused()
    {
        ArgumentException error = Assert.Throws<ArgumentException>(
            () => OrdinaryLeastSquares.Fit(Design, Response, 2, new OlsOptions { CovarianceType = CovarianceType.Hac }));

        Assert.Equal("options", error.ParamName);
    }

    [Fact]
    public void Lags_on_another_type_are_refused()
    {
        ArgumentException error = Assert.Throws<ArgumentException>(
            () => WeightedLeastSquares.Fit(
                Design, Response, Weights, 2, new OlsOptions { CovarianceType = CovarianceType.Hc3, HacLags = 2 }));

        Assert.Equal("options", error.ParamName);
    }

    [Fact]
    public void A_negative_lag_count_is_refused()
    {
        Assert.Throws<ArgumentOutOfRangeException>(() => new OlsOptions { HacLags = -1 });
    }

    [Theory]
    [InlineData(CovarianceType.Nonrobust)]
    [InlineData(CovarianceType.Hc1)]
    public void A_correction_on_a_type_without_one_is_refused(CovarianceType type)
    {
        ArgumentException error = Assert.Throws<ArgumentException>(
            () => OrdinaryLeastSquares.Fit(
                Design, Response, 2, new OlsOptions { CovarianceType = type, SmallSampleCorrection = true }));

        Assert.Equal("options", error.ParamName);
    }

    [Fact]
    public void Cluster_without_labels_is_refused()
    {
        ArgumentException error = Assert.Throws<ArgumentException>(
            () => OrdinaryLeastSquares.Fit(Design, Response, 2, ClusterOptions));

        Assert.Equal("options", error.ParamName);
    }

    [Fact]
    public void Labels_with_another_type_are_refused()
    {
        ArgumentException error = Assert.Throws<ArgumentException>(
            () => OrdinaryLeastSquares.Fit(
                Design, Response, Clusters, 2, new OlsOptions { CovarianceType = CovarianceType.Hc0 }));

        Assert.Equal("options", error.ParamName);
    }

    [Fact]
    public void Labels_of_the_wrong_length_are_refused()
    {
        ArgumentException error = Assert.Throws<ArgumentException>(
            () => WeightedLeastSquares.Fit(Design, Response, Weights, [1, 2, 3], 2, ClusterOptions));

        Assert.Equal("clusters", error.ParamName);
    }

    [Fact]
    public void A_single_cluster_is_refused()
    {
        ArgumentException error = Assert.Throws<ArgumentException>(
            () => OrdinaryLeastSquares.Fit(Design, Response, new int[10], 2, ClusterOptions));

        Assert.Equal("clusters", error.ParamName);
    }

    [Theory]
    [InlineData(CovarianceType.Hac)]
    [InlineData(CovarianceType.Cluster)]
    public void Generalized_least_squares_refuses_both(CovarianceType type)
    {
        double[] identity = new double[100];
        for (int i = 0; i < 10; i++)
        {
            identity[(i * 10) + i] = 1.0;
        }

        ArgumentException error = Assert.Throws<ArgumentException>(
            () => GeneralizedLeastSquares.Fit(
                Design, Response, identity, 2, new OlsOptions { CovarianceType = type, HacLags = type == CovarianceType.Hac ? 1 : null }));

        Assert.Equal("options", error.ParamName);
    }
}
