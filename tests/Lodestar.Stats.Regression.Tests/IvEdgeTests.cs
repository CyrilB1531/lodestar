using Lodestar.Stats.Regression.Instrumental;
using Xunit;

namespace Lodestar.Stats.Regression.Tests;

/// <summary>What the corpus does not reach: every refusal of the instrumental-variables estimators, and the identities a caller relies on (#1155).</summary>
public sealed class IvEdgeTests
{
    // Twelve rows: one exogenous regressor, one endogenous, two instruments.
    private static readonly double[] Response = [3.1, 4.0, 5.2, 4.4, 6.9, 7.1, 6.0, 8.8, 9.1, 8.2, 10.7, 11.3];
    private static readonly double[] Exogenous = [0.2, -1.0, 0.5, 1.3, -0.4, 0.9, -1.2, 0.1, 1.7, -0.6, 0.8, -0.3];
    private static readonly double[] Endogenous = [1.0, 1.4, 2.1, 1.8, 3.0, 3.3, 2.6, 3.9, 4.2, 3.7, 4.9, 5.4];
    private static readonly double[] Instruments =
        [0.9, 0.1, 1.5, -0.3, 2.2, 0.4, 1.7, 0.8, 3.1, -0.2, 3.3, 0.6, 2.4, 1.1, 3.8, -0.5, 4.1, 0.9, 3.5, 0.2, 4.6, -0.1, 5.2, 0.7];

    private static readonly int[] Clusters = [1, 1, 2, 2, 3, 3, 4, 4, 5, 5, 6, 6];

    private static IvDesign Design(ReadOnlySpan<double> instruments, int instrumentCount) =>
        new(Response, Exogenous, 1, Endogenous, 1, instruments, instrumentCount);

    [Fact]
    public void Fewer_instruments_than_endogenous_regressors_is_refused()
    {
        double[] twoEndogenous = [.. Endogenous.SelectMany(v => new[] { v, v * v })];
        Assert.Throws<ArgumentException>(() => InstrumentalVariables.TwoStageLeastSquares(
            new IvDesign(Response, Exogenous, 1, twoEndogenous, 2, Instruments.Where((_, i) => i % 2 == 0).ToArray(), 1)));
    }

    [Fact]
    public void A_block_of_the_wrong_length_is_refused()
    {
        Assert.Throws<ArgumentException>(() => InstrumentalVariables.TwoStageLeastSquares(Design(Instruments.AsSpan(1), 2)));
        Assert.Throws<ArgumentException>(() => InstrumentalVariables.Liml(
            new IvDesign(Response, Exogenous.AsSpan(1), 1, Endogenous, 1, Instruments, 2)));
        Assert.Throws<ArgumentOutOfRangeException>(() => InstrumentalVariables.Gmm(
            new IvDesign(Response, Exogenous, 1, Endogenous, 0, Instruments, 2)));
    }

    [Fact]
    public void A_cluster_covariance_needs_labels_and_labels_need_a_cluster_covariance()
    {
        var clustered = new IvOptions { CovarianceType = IvCovarianceType.Clustered };
        Assert.Throws<ArgumentException>(() => InstrumentalVariables.TwoStageLeastSquares(Design(Instruments, 2), clustered));
        Assert.Throws<ArgumentException>(() => InstrumentalVariables.TwoStageLeastSquares(Design(Instruments, 2), Clusters, new IvOptions()));
        Assert.Throws<ArgumentException>(() => InstrumentalVariables.Liml(Design(Instruments, 2), Clusters.AsSpan(1), clustered));
        Assert.NotNull(InstrumentalVariables.Gmm(
            Design(Instruments, 2), Clusters, new IvOptions { GmmWeightType = IvCovarianceType.Clustered }));
    }

    [Fact]
    public void Options_an_estimator_does_not_read_are_refused()
    {
        Assert.Throws<ArgumentException>(() =>
            InstrumentalVariables.TwoStageLeastSquares(Design(Instruments, 2), new IvOptions { Fuller = 1.0 }));
        Assert.Throws<ArgumentException>(() =>
            InstrumentalVariables.Gmm(Design(Instruments, 2), new IvOptions { Fuller = 1.0 }));
        Assert.Throws<ArgumentException>(() =>
            InstrumentalVariables.Liml(Design(Instruments, 2), new IvOptions { Fuller = double.NaN }));
        Assert.Throws<ArgumentException>(() =>
            InstrumentalVariables.TwoStageLeastSquares(Design(Instruments, 2), new IvOptions { Bandwidth = -1 }));
        Assert.Throws<ArgumentException>(() =>
            InstrumentalVariables.TwoStageLeastSquares(Design(Instruments, 2), new IvOptions { CovarianceType = (IvCovarianceType)9 }));
        Assert.Throws<ArgumentException>(() =>
            InstrumentalVariables.TwoStageLeastSquares(Design(Instruments, 2), new IvOptions { Kernel = (KernelType)9 }));
        Assert.Throws<ArgumentOutOfRangeException>(() =>
            InstrumentalVariables.TwoStageLeastSquares(Design(Instruments, 2), new IvOptions { ConfidenceLevel = 1.0 }));
        Assert.Throws<ArgumentException>(() =>
            InstrumentalVariables.TwoStageLeastSquares(Design(Instruments, 2), new IvOptions { Bandwidth = 3 }));
        Assert.Throws<ArgumentException>(() =>
            InstrumentalVariables.Liml(Design(Instruments, 2), new IvOptions { Kernel = KernelType.Parzen }));
        Assert.Throws<ArgumentException>(() =>
            InstrumentalVariables.TwoStageLeastSquares(Design(Instruments, 2), new IvOptions { GmmWeightType = IvCovarianceType.Unadjusted }));
        Assert.Throws<ArgumentException>(() =>
            InstrumentalVariables.Gmm(Design(Instruments, 2), new IvOptions { GmmWeightBandwidth = 2 }));
    }

    /// <summary>A Bartlett or Parzen bandwidth past the sample is refused, as <c>cov_kernel</c> refuses it; the quadratic spectral one is not.</summary>
    [Fact]
    public void A_truncating_bandwidth_past_the_sample_is_refused()
    {
        var options = new IvOptions { CovarianceType = IvCovarianceType.Kernel, Bandwidth = Response.Length };

        IvSummary last = InstrumentalVariables.TwoStageLeastSquares(
            Design(Instruments, 2), options with { Bandwidth = Response.Length - 1 });
        IvSummary spectral = InstrumentalVariables.TwoStageLeastSquares(
            Design(Instruments, 2), options with { Kernel = KernelType.QuadraticSpectral, Bandwidth = Response.Length + 5 });

        Assert.Equal(Response.Length - 1, last.Bandwidth);
        Assert.Equal(Response.Length + 5, spectral.Bandwidth);
        Assert.Throws<ArgumentException>(() => InstrumentalVariables.TwoStageLeastSquares(Design(Instruments, 2), options));
        Assert.Throws<ArgumentException>(() =>
            InstrumentalVariables.Gmm(Design(Instruments, 2), options with { Kernel = KernelType.Parzen, Bandwidth = int.MaxValue }));
    }

    /// <summary>A regressor in the tens of thousands beside one in the thousandths is badly scaled, not collinear.</summary>
    [Fact]
    public void A_badly_scaled_design_is_fitted_rather_than_refused()
    {
        double[] large = [.. Exogenous.Select(v => 50_000.0 + (1_000.0 * v))];
        double[] small = [.. Endogenous.Select(v => 1e-3 * v)];
        double[] instruments = [.. Instruments.Select((v, i) => i % 2 == 0 ? 1e-3 * v : v)];
        IvSummary summary = InstrumentalVariables.TwoStageLeastSquares(new IvDesign(Response, large, 1, small, 1, instruments, 2));

        Assert.True(double.IsFinite(summary.Coefficients[2]));
        Assert.NotNull(summary.ModelTest);
    }

    [Fact]
    public void A_null_options_on_the_clustered_overload_is_refused()
    {
        Assert.Throws<ArgumentNullException>(() => InstrumentalVariables.TwoStageLeastSquares(Design(Instruments, 2), Clusters, null!));
    }

    [Fact]
    public void Collinear_instruments_are_refused()
    {
        double[] doubled = [.. Instruments.Where((_, i) => i % 2 == 0).SelectMany(v => new[] { v, 2.0 * v })];
        Assert.Throws<ArgumentException>(() => InstrumentalVariables.TwoStageLeastSquares(Design(doubled, 2)));
    }

    [Fact]
    public void Too_few_rows_for_the_parameters_are_refused()
    {
        Assert.Throws<ArgumentException>(() => InstrumentalVariables.TwoStageLeastSquares(
            new IvDesign(Response.AsSpan(0, 3), Exogenous.AsSpan(0, 3), 1, Endogenous.AsSpan(0, 3), 1, Instruments.AsSpan(0, 6), 2)));
    }

    /// <summary>A just-identified model has no overidentification to test, and LIML there is 2SLS.</summary>
    [Fact]
    public void A_just_identified_model_reports_no_overidentification_and_liml_meets_2sls()
    {
        double[] one = [.. Instruments.Where((_, i) => i % 2 == 0)];
        IvSummary twoStage = InstrumentalVariables.TwoStageLeastSquares(Design(one, 1));
        IvSummary liml = InstrumentalVariables.Liml(Design(one, 1));
        IvSummary gmm = InstrumentalVariables.Gmm(Design(one, 1));

        Assert.Null(twoStage.Overidentification);
        Assert.Null(gmm.Overidentification);
        Assert.Equal(1.0, liml.Kappa!.Value, 12);
        for (int j = 0; j < twoStage.Coefficients.Count; j++)
        {
            Assert.Equal(twoStage.Coefficients[j], liml.Coefficients[j], 10);
            Assert.Equal(twoStage.Coefficients[j], gmm.Coefficients[j], 10);
        }
    }

    [Fact]
    public void The_summary_echoes_the_covariance_and_reports_a_bandwidth_only_for_a_kernel()
    {
        IvSummary robust = InstrumentalVariables.TwoStageLeastSquares(Design(Instruments, 2));
        IvSummary kernel = InstrumentalVariables.TwoStageLeastSquares(
            Design(Instruments, 2), new IvOptions { CovarianceType = IvCovarianceType.Kernel, Bandwidth = 2 });

        Assert.Equal(IvCovarianceType.Robust, robust.CovarianceType);
        Assert.Null(robust.Bandwidth);
        Assert.Equal(2, kernel.Bandwidth);
        Assert.Equal(1.0, robust.Kappa);
        Assert.Null(InstrumentalVariables.Gmm(Design(Instruments, 2)).Kappa);
    }

    [Fact]
    public void Without_an_intercept_the_r_squared_is_uncentred()
    {
        IvSummary summary = InstrumentalVariables.TwoStageLeastSquares(Design(Instruments, 2), new IvOptions { WithIntercept = false });

        Assert.False(summary.HasConstant);
        Assert.Equal(2, summary.Coefficients.Count);
    }
}
