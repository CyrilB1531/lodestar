using Xunit;

namespace Lodestar.Stats.Regression.Tests;

/// <summary>The interval multipliers near a level of one, against 50-digit upper quantiles of the double
/// (1 − level)/2, 4.999889391399392e-13 (#1729's Review A).</summary>
public sealed class CriticalValueEdgeTests
{
    private static readonly double[] Design = [1.0, 2.0, 3.0, 4.0, 5.0, 6.0];
    private static readonly double[] Response = [2.2, 3.9, 6.4, 7.7, 10.3, 11.8];
    private const double Level = 1.0 - 1e-12;

    [Fact]
    public void The_least_squares_multipliers_keep_their_digits_near_a_level_of_one()
    {
        // 1 − (1 − level)/2 rounded: at 1 − 1e-12 the multipliers kept about four digits.
        OlsSummary student = OrdinaryLeastSquares.Fit(Design, Response, 1, new OlsOptions { ConfidenceLevel = Level });
        OlsSummary robust = OrdinaryLeastSquares.Fit(
            Design, Response, 1, new OlsOptions { ConfidenceLevel = Level, CovarianceType = CovarianceType.Hc2 });

        Assert.Equal(1565.0921708841860, HalfWidth(student.ConfidenceLower[1], student.ConfidenceUpper[1]) / student.StandardErrors[1], 1e-12 * 1565.09);
        Assert.Equal(7.1305098928792724, HalfWidth(robust.ConfidenceLower[1], robust.ConfidenceUpper[1]) / robust.StandardErrors[1], 1e-13 * 7.13);
    }

    [Fact]
    public void The_generalized_linear_multiplier_keeps_its_digits_near_a_level_of_one()
    {
        GlmSummary fit = GeneralizedLinearModel.Fit(Design, [2.0, 4.0, 6.0, 8.0, 10.0, 12.0], 1, GlmFamily.Poisson, new GlmOptions { ConfidenceLevel = Level });

        Assert.Equal(7.1305098928792724, HalfWidth(fit.ConfidenceLower[1], fit.ConfidenceUpper[1]) / fit.StandardErrors[1], 1e-13 * 7.13);
    }

    private static double HalfWidth(double lower, double upper) => (upper - lower) / 2.0;
}
