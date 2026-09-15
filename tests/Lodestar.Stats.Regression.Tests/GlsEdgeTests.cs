using Xunit;

namespace Lodestar.Stats.Regression.Tests;

/// <summary>The covariances a corpus of well-posed fits never reaches, and the identities whitening must keep.</summary>
public sealed class GlsEdgeTests
{
    private static readonly double[] Design = [1.0, 2.0, 3.0, 4.0, 5.0, 6.0];
    private static readonly double[] Response = [2.2, 3.9, 6.4, 7.7, 10.3, 11.8];

    private static double[] Diagonal(params double[] variances)
    {
        int n = variances.Length;
        var matrix = new double[n * n];
        for (int i = 0; i < n; i++)
        {
            matrix[(i * n) + i] = variances[i];
        }

        return matrix;
    }

    [Fact]
    public void A_covariance_of_the_wrong_length_is_refused()
    {
        ArgumentException error = Assert.Throws<ArgumentException>(
            () => GeneralizedLeastSquares.Fit(Design, Response, [1.0, 1.0, 1.0, 1.0, 1.0, 1.0], 1));

        Assert.Equal("covariance", error.ParamName);
    }

    [Fact]
    public void A_non_finite_covariance_entry_is_refused()
    {
        double[] covariance = Diagonal(1.0, 1.0, 1.0, 1.0, 1.0, 1.0);
        covariance[7] = double.NaN;

        ArgumentOutOfRangeException error = Assert.Throws<ArgumentOutOfRangeException>(
            () => GeneralizedLeastSquares.Fit(Design, Response, covariance, 1));

        Assert.Equal("covariance", error.ParamName);
    }

    /// <summary>The reference's Cholesky reads the lower triangle and never looks at the upper one.</summary>
    [Fact]
    public void An_asymmetric_covariance_is_refused()
    {
        double[] covariance = Diagonal(1.0, 1.0, 1.0, 1.0, 1.0, 1.0);
        covariance[1] = 0.3;

        ArgumentException error = Assert.Throws<ArgumentException>(
            () => GeneralizedLeastSquares.Fit(Design, Response, covariance, 1));

        Assert.Equal("covariance", error.ParamName);
    }

    [Fact]
    public void A_covariance_that_is_not_positive_definite_is_refused()
    {
        double[] covariance = Diagonal(1.0, 1.0, -1.0, 1.0, 1.0, 1.0);

        ArgumentException error = Assert.Throws<ArgumentException>(
            () => GeneralizedLeastSquares.Fit(Design, Response, covariance, 1));

        Assert.Equal("covariance", error.ParamName);
    }

    [Fact]
    public void A_design_with_no_residual_degrees_of_freedom_is_refused()
    {
        ArgumentException error = Assert.Throws<ArgumentException>(
            () => GeneralizedLeastSquares.Fit([1.0, 2.0], [1.0, 2.0], Diagonal(1.0, 1.0), 1));

        Assert.Equal("design", error.ParamName);
    }

    /// <summary>Exactly: L is the identity, and dividing by one and subtracting zero move no bit.</summary>
    [Fact]
    public void The_identity_covariance_reproduces_the_ordinary_fit_exactly()
    {
        var options = new OlsOptions { CovarianceType = CovarianceType.Hc2 };
        OlsSummary ordinary = OrdinaryLeastSquares.Fit(Design, Response, 1, options);
        OlsSummary generalized = GeneralizedLeastSquares.Fit(
            Design, Response, Diagonal(1.0, 1.0, 1.0, 1.0, 1.0, 1.0), 1, options);

        Assert.Equal(ordinary.Coefficients, generalized.Coefficients);
        Assert.Equal(ordinary.StandardErrors, generalized.StandardErrors);
        Assert.Equal(ordinary.RSquared, generalized.RSquared);
        Assert.Equal(ordinary.FStatistic, generalized.FStatistic);
    }

    /// <summary>A diagonal covariance is the weighted fit with weights 1/σ, which is how a statsmodels vector sigma maps here.</summary>
    [Theory]
    [InlineData(true)]
    [InlineData(false)]
    public void A_diagonal_covariance_is_the_weighted_fit_with_inverse_weights(bool withIntercept)
    {
        double[] variances = [1.0, 0.5, 2.0, 1.0, 0.25, 4.0];
        var options = new OlsOptions { WithIntercept = withIntercept };

        OlsSummary generalized = GeneralizedLeastSquares.Fit(Design, Response, Diagonal(variances), 1, options);
        OlsSummary weighted = WeightedLeastSquares.Fit(
            Design, Response, [.. variances.Select(v => 1.0 / v)], 1, options);

        for (int j = 0; j < weighted.Coefficients.Count; j++)
        {
            Assert.Equal(weighted.Coefficients[j], generalized.Coefficients[j], 1e-12);
            Assert.Equal(weighted.StandardErrors[j], generalized.StandardErrors[j], 1e-12);
        }

        Assert.Equal(weighted.RSquared, generalized.RSquared, 1e-12);
        Assert.Equal(weighted.FStatistic, generalized.FStatistic, 1e-9);
    }
}
