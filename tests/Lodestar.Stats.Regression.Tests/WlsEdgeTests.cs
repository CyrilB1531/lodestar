using Xunit;

namespace Lodestar.Stats.Regression.Tests;

/// <summary>The weights a corpus of well-posed fits never reaches, and the identities weighting must keep.</summary>
public sealed class WlsEdgeTests
{
    private static readonly double[] Design = [1.0, 2.0, 3.0, 4.0, 5.0, 6.0, 7.0, 8.0];
    private static readonly double[] Response = [2.1, 3.9, 6.2, 7.8, 10.1, 12.2, 13.8, 16.1];
    private static readonly double[] Uneven = [1.0, 2.0, 0.5, 1.0, 3.0, 1.0, 0.25, 2.0];

    [Fact]
    public void A_feature_count_below_one_is_refused()
    {
        Assert.Throws<ArgumentOutOfRangeException>(
            () => WeightedLeastSquares.Fit(Design, Response, Uneven, 0));
    }

    [Fact]
    public void A_response_of_the_wrong_length_is_refused()
    {
        ArgumentException error = Assert.Throws<ArgumentException>(
            () => WeightedLeastSquares.Fit(Design, [1.0, 2.0], Uneven, 1));

        Assert.Equal("response", error.ParamName);
    }

    [Fact]
    public void Weights_of_the_wrong_length_are_refused()
    {
        ArgumentException error = Assert.Throws<ArgumentException>(
            () => WeightedLeastSquares.Fit(Design, Response, [1.0, 2.0], 1));

        Assert.Equal("weights", error.ParamName);
    }

    [Fact]
    public void A_design_with_no_residual_degrees_of_freedom_is_refused()
    {
        ArgumentException error = Assert.Throws<ArgumentException>(
            () => WeightedLeastSquares.Fit([1.0, 2.0], [1.0, 2.0], [1.0, 1.0], 1));

        Assert.Equal("design", error.ParamName);
    }

    /// <summary>The reference takes the square root of a negative weight and fails in its SVD.</summary>
    [Theory]
    [InlineData(-1.0)]
    [InlineData(double.NaN)]
    [InlineData(double.PositiveInfinity)]
    public void A_weight_that_is_negative_or_not_finite_is_refused(double weight)
    {
        double[] weights = [.. Uneven];
        weights[3] = weight;

        ArgumentOutOfRangeException error = Assert.Throws<ArgumentOutOfRangeException>(
            () => WeightedLeastSquares.Fit(Design, Response, weights, 1));

        Assert.Equal("weights", error.ParamName);
    }

    /// <summary>The reference answers all-zero weights through a pseudo-inverse, with no error.</summary>
    [Fact]
    public void All_zero_weights_are_refused()
    {
        ArgumentException error = Assert.Throws<ArgumentException>(
            () => WeightedLeastSquares.Fit(Design, Response, new double[Design.Length], 1));

        Assert.Equal("weights", error.ParamName);
    }

    [Fact]
    public void Fewer_positive_weights_than_parameters_are_refused()
    {
        ArgumentException error = Assert.Throws<ArgumentException>(
            () => WeightedLeastSquares.Fit(Design, Response, [0.0, 0.0, 0.0, 2.0, 0.0, 0.0, 0.0, 0.0], 1));

        Assert.Equal("weights", error.ParamName);
    }

    /// <summary>A zero weight takes the row out of the estimate and leaves it in the count.</summary>
    [Fact]
    public void A_zero_weight_keeps_its_row_in_the_degrees_of_freedom()
    {
        double[] weights = [.. Uneven];
        weights[3] = 0.0;

        OlsSummary summary = WeightedLeastSquares.Fit(Design, Response, weights, 1);

        Assert.Equal(6, summary.ResidualDegreesOfFreedom);
    }

    /// <summary>
    /// Exactly, not within a tolerance: the square root of one is one, the weighted mean of
    /// unit weights is the plain mean, and the two fits share every step after that.
    /// </summary>
    [Fact]
    public void Unit_weights_reproduce_the_ordinary_fit_exactly()
    {
        var options = new OlsOptions { CovarianceType = CovarianceType.Hc3 };
        OlsSummary ordinary = OrdinaryLeastSquares.Fit(Design, Response, 1, options);
        OlsSummary weighted = WeightedLeastSquares.Fit(
            Design, Response, [1.0, 1.0, 1.0, 1.0, 1.0, 1.0, 1.0, 1.0], 1, options);

        Assert.Equal(ordinary.Coefficients, weighted.Coefficients);
        Assert.Equal(ordinary.StandardErrors, weighted.StandardErrors);
        Assert.Equal(ordinary.PValues, weighted.PValues);
        Assert.Equal(ordinary.VarianceInflationFactors, weighted.VarianceInflationFactors);
        Assert.Equal(ordinary.RSquared, weighted.RSquared);
        Assert.Equal(ordinary.FStatistic, weighted.FStatistic);
        Assert.Equal(ordinary.ResidualStandardError, weighted.ResidualStandardError);
    }

    /// <summary>A constant weight rescales the residuals and the design together, so only the scale moves.</summary>
    [Fact]
    public void A_constant_weight_moves_the_residual_scale_and_nothing_else()
    {
        OlsSummary ordinary = OrdinaryLeastSquares.Fit(Design, Response, 1);
        OlsSummary weighted = WeightedLeastSquares.Fit(
            Design, Response, [4.0, 4.0, 4.0, 4.0, 4.0, 4.0, 4.0, 4.0], 1);

        for (int j = 0; j < 2; j++)
        {
            Assert.Equal(ordinary.Coefficients[j], weighted.Coefficients[j], 1e-12);
            Assert.Equal(ordinary.StandardErrors[j], weighted.StandardErrors[j], 1e-12);
        }

        Assert.Equal(ordinary.RSquared, weighted.RSquared, 1e-12);
        Assert.Equal(2.0 * ordinary.ResidualStandardError, weighted.ResidualStandardError, 1e-12);
    }

    [Fact]
    public void The_default_options_fit_an_intercept_at_ninety_five_percent()
    {
        OlsSummary summary = WeightedLeastSquares.Fit(Design, Response, Uneven, 1);

        Assert.True(summary.HasIntercept);
        Assert.Equal(0.95, summary.ConfidenceLevel);
        Assert.Equal(2, summary.Coefficients.Count);
    }
}
