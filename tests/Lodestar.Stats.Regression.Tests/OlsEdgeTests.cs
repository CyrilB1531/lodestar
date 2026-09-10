using Xunit;

namespace Lodestar.Stats.Regression.Tests;

/// <summary>The shapes a design can take that a corpus of well-posed fits never reaches.</summary>
public sealed class OlsEdgeTests
{
    private static readonly double[] Design = [1.0, 2.0, 3.0, 4.0, 5.0, 6.0];
    private static readonly double[] Response = [2.0, 4.1, 5.9, 8.2, 9.8, 12.1];

    [Fact]
    public void A_feature_count_below_one_is_refused()
    {
        Assert.Throws<ArgumentOutOfRangeException>(
            () => OrdinaryLeastSquares.Fit(Design, Response, 0));
    }

    [Fact]
    public void A_design_that_is_not_a_whole_number_of_rows_is_refused()
    {
        ArgumentException error = Assert.Throws<ArgumentException>(
            () => OrdinaryLeastSquares.Fit(Design, [1.0, 2.0], 4));

        Assert.Equal("design", error.ParamName);
    }

    [Fact]
    public void A_response_of_the_wrong_length_is_refused()
    {
        ArgumentException error = Assert.Throws<ArgumentException>(
            () => OrdinaryLeastSquares.Fit(Design, [1.0, 2.0], 1));

        Assert.Equal("response", error.ParamName);
    }

    /// <summary>Every standard error here divides by the residual degrees of freedom.</summary>
    [Fact]
    public void A_design_with_no_residual_degrees_of_freedom_is_refused()
    {
        ArgumentException error = Assert.Throws<ArgumentException>(
            () => OrdinaryLeastSquares.Fit([1.0, 2.0], [1.0, 2.0], 1));

        Assert.Equal("design", error.ParamName);
    }

    [Theory]
    [InlineData(0.0)]
    [InlineData(1.0)]
    [InlineData(double.NaN)]
    public void A_confidence_level_outside_the_open_unit_interval_is_refused(double level)
    {
        var options = new OlsOptions();

        Assert.Throws<ArgumentOutOfRangeException>(() => options.ConfidenceLevel = level);
    }

    [Fact]
    public void The_default_options_fit_an_intercept_at_ninety_five_percent()
    {
        OlsSummary summary = OrdinaryLeastSquares.Fit(Design, Response, 1);

        Assert.True(summary.HasIntercept);
        Assert.Equal(0.95, summary.ConfidenceLevel);
        Assert.Equal(2, summary.Coefficients.Count);
        Assert.Single(summary.VarianceInflationFactors);
    }

    /// <summary>
    /// One regressor and no intercept leaves the auxiliary regression with an empty design.
    /// </summary>
    /// <remarks>
    /// A deliberate divergence: statsmodels raises out of numpy here. The rest of the table
    /// is well defined, so the diagnostic that is not says so rather than sinking the fit.
    /// </remarks>
    [Fact]
    public void A_single_regressor_with_no_intercept_reports_an_undefined_vif()
    {
        OlsSummary summary = OrdinaryLeastSquares.Fit(
            Design, Response, 1, new OlsOptions { WithIntercept = false });

        Assert.Single(summary.VarianceInflationFactors);
        Assert.True(double.IsNaN(summary.VarianceInflationFactors[0]));
        Assert.False(double.IsNaN(summary.Coefficients[0]));
    }

    /// <summary>An exact fit leaves no residual variance, and every error under the table is zero.</summary>
    /// <remarks>
    /// Kept out of the corpus because statsmodels reports an infinite F there and JSON has no
    /// way to write one. The behaviour is still worth pinning: it is what a caller sees when a
    /// design happens to span its response.
    /// </remarks>
    [Fact]
    public void An_exact_fit_reports_zero_residual_error_and_an_infinite_f()
    {
        OlsSummary summary = OrdinaryLeastSquares.Fit(
            [1.0, 3.0, 2.0, 1.0, 3.0, 4.0], [5.0, 4.0, 9.0], 2,
            new OlsOptions { WithIntercept = false });

        Assert.Equal(0.0, summary.ResidualStandardError, 12);
        Assert.Equal(1.0, summary.RSquared, 12);
        Assert.True(double.IsPositiveInfinity(summary.FStatistic));
        Assert.Equal(0.0, summary.FPValue);
    }

    /// <summary>A confidence level widens both ends without moving the estimate.</summary>
    [Fact]
    public void A_wider_level_widens_the_interval_around_the_same_estimate()
    {
        OlsSummary ninetyFive = OrdinaryLeastSquares.Fit(Design, Response, 1);
        OlsSummary ninetyNine = OrdinaryLeastSquares.Fit(
            Design, Response, 1, new OlsOptions { ConfidenceLevel = 0.99 });

        Assert.Equal(ninetyFive.Coefficients[1], ninetyNine.Coefficients[1], 12);
        Assert.True(ninetyNine.ConfidenceLower[1] < ninetyFive.ConfidenceLower[1]);
        Assert.True(ninetyNine.ConfidenceUpper[1] > ninetyFive.ConfidenceUpper[1]);
    }
}
