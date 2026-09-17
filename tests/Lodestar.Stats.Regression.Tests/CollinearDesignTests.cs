using Xunit;

namespace Lodestar.Stats.Regression.Tests;

/// <summary>
/// Designs with a column dependent on the ones before it, which every least-squares fit refuses rather than answering
/// with a table of NaN or of coefficients near 1e14 (#867).
/// </summary>
public sealed class CollinearDesignTests
{
    private static readonly double[] Response = [2.1, 3.9, 6.2, 7.8, 10.1, 12.2, 13.8, 16.1];

    private static readonly double[] Weights = [1.0, 2.0, 1.0, 3.0, 1.0, 2.0, 1.0, 1.0];

    /// <summary>The second regressor three times the first: a pivot of 2e-16 rather than zero, which no NaN check sees.</summary>
    private static readonly double[] Tripled = [1.0, 3.0, 2.0, 6.0, 3.0, 9.0, 4.0, 12.0, 5.0, 15.0, 6.0, 18.0, 7.0, 21.0, 8.0, 24.0];

    /// <summary>The third regressor the sum of the first two.</summary>
    private static readonly double[] Summed =
    [
        1.0, 0.5, 1.5, 2.0, -1.2, 0.8, 3.0, 2.2, 5.2, 4.0, 0.1, 4.1,
        5.0, -0.7, 4.3, 6.0, 1.9, 7.9, 7.0, 3.3, 10.3, 8.0, -2.4, 5.6,
    ];

    public static TheoryData<double[], int> Designs() => new()
    {
        { Tripled, 2 },
        { Summed, 3 },
    };

    private static void AssertRefused(Action fit)
    {
        ArgumentException error = Assert.Throws<ArgumentException>(fit);

        Assert.Equal("design", error.ParamName);
        Assert.Contains("rank-deficient or collinear", error.Message, StringComparison.Ordinal);
    }

    [Theory]
    [MemberData(nameof(Designs))]
    public void Ordinary_least_squares_refuses_a_collinear_design(double[] design, int featureCount)
    {
        AssertRefused(() => OrdinaryLeastSquares.Fit(design, Response, featureCount));
    }

    [Theory]
    [MemberData(nameof(Designs))]
    public void A_robust_fit_refuses_a_collinear_design(double[] design, int featureCount)
    {
        AssertRefused(() => OrdinaryLeastSquares.Fit(design, Response, featureCount, new OlsOptions { CovarianceType = CovarianceType.Hc3 }));
    }

    [Theory]
    [MemberData(nameof(Designs))]
    public void Weighted_least_squares_refuses_a_collinear_design(double[] design, int featureCount)
    {
        AssertRefused(() => WeightedLeastSquares.Fit(design, Response, Weights, featureCount));
    }

    [Theory]
    [MemberData(nameof(Designs))]
    public void Generalized_least_squares_refuses_a_collinear_design(double[] design, int featureCount)
    {
        var covariance = new double[64];
        for (int i = 0; i < 8; i++)
        {
            for (int j = 0; j < 8; j++)
            {
                covariance[(i * 8) + j] = Math.Pow(0.5, Math.Abs(i - j));
            }
        }

        AssertRefused(() => GeneralizedLeastSquares.Fit(design, Response, covariance, featureCount));
    }

    [Theory]
    [MemberData(nameof(Designs))]
    public void The_generalized_linear_model_refuses_a_collinear_design(double[] design, int featureCount)
    {
        // Before #867 the tripled design ran the IRLS budget out instead, and was reported as not converging.
        double[] counts = [2.0, 4.0, 6.0, 8.0, 10.0, 12.0, 14.0, 16.0];

        AssertRefused(() => GeneralizedLinearModel.Fit(design, counts, featureCount, GlmFamily.Poisson));
    }

    [Fact]
    public void A_regressor_that_is_zero_in_every_row_is_refused()
    {
        AssertRefused(() => OrdinaryLeastSquares.Fit([1.0, 0.0, 2.0, 0.0, 3.0, 0.0, 4.0, 0.0, 5.0, 0.0, 6.0, 0.0, 7.0, 0.0, 8.0, 0.0], Response, 2));
    }

    [Fact]
    public void A_nearly_collinear_design_is_still_fitted()
    {
        // A millionth off the tripled column in one row: ill-conditioned, not rank-deficient.
        double[] design = [.. Tripled];
        design[7] += 1e-6;

        OlsSummary summary = OrdinaryLeastSquares.Fit(design, Response, 2);

        Assert.All(summary.Coefficients, coefficient => Assert.True(double.IsFinite(coefficient)));
    }
}
