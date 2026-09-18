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

    [Theory]
    [MemberData(nameof(Designs))]
    public void An_estimate_refuses_a_collinear_design(double[] design, int featureCount)
    {
        // The equivalence row promises the numbers Fit keeps, and answered [0.036, 9.4e13, -3.1e13] here (#979).
        AssertRefused(() => OrdinaryLeastSquares.Estimate(design, Response, featureCount));
    }

    [Theory]
    [MemberData(nameof(Designs))]
    public void An_estimate_without_an_intercept_refuses_a_collinear_design(double[] design, int featureCount)
    {
        AssertRefused(() => OrdinaryLeastSquares.Estimate(design, Response, featureCount, withIntercept: false));
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
        OlsEstimate estimate = OrdinaryLeastSquares.Estimate(design, Response, 2);

        Assert.All(summary.Coefficients, coefficient => Assert.True(double.IsFinite(coefficient)));
        Assert.All(estimate.Coefficients, coefficient => Assert.True(double.IsFinite(coefficient)));
    }

    [Theory]
    [InlineData(1e-2)]
    [InlineData(1e-4)]
    [InlineData(1e-6)]
    [InlineData(1e-8)]
    [InlineData(1e-10)]
    public void Fit_RefusesADependentColumnFarSmallerThanTheColumnsItDependsOn(double delta)
    {
        // x3 = x1 - x2 is exact in double, so the design has rank 3 whatever delta is, as
        // statsmodels 0.15.0 reports; the per-column pivot saw 1e-13 and answered 1e14 (#978).
        double[] r = [0.3, -1.1, 0.7, 2.2, -0.4, 1.5, -2.0, 0.9];
        double[] response = [2.1, 3.9, 6.2, 7.8, 10.1, 12.2, 13.8, 16.1];
        var design = new double[8 * 3];
        for (int i = 0; i < 8; i++)
        {
            double x1 = i + 1;
            double x2 = x1 + (delta * r[i]);
            design[(i * 3) + 0] = x1;
            design[(i * 3) + 1] = x2;
            design[(i * 3) + 2] = x1 - x2;
        }

        ArgumentException failure = Assert.Throws<ArgumentException>(
            () => OrdinaryLeastSquares.Fit(design, response, 3));

        Assert.Equal("design", failure.ParamName);
        Assert.Contains("rank-deficient or collinear", failure.Message, StringComparison.Ordinal);
    }

    // SonarLint S2245, CA5394: a seeded Random builds a reproducible design; no security use.
#pragma warning disable S2245, CA5394
    [Fact]
    public void Fit_AcceptsAFullRankDesignWhoseColumnsDifferInScale()
    {
        // numpy.linalg.matrix_rank calls this one full rank, so the spectral test must accept it
        // rather than merely being stricter than the pivot it replaces.
        var seeded = new Random(978);
        var design = new double[200 * 3];
        var response = new double[200];
        for (int i = 0; i < 200; i++)
        {
            design[(i * 3) + 0] = 1e7 + (seeded.NextDouble() * 9.9e8);
            design[(i * 3) + 1] = 1e-6 + (seeded.NextDouble() * 9.99e-4);
            design[(i * 3) + 2] = seeded.NextDouble() - 0.5;
            response[i] = seeded.NextDouble();
        }

        OlsSummary summary = OrdinaryLeastSquares.Fit(design, response, 3);

        Assert.Equal(196, summary.ResidualDegreesOfFreedom);
    }

    [Fact]
    public void Fit_AndEstimate_AgreeOnADesignOnlyTheUnscaledSpectrumRefuses()
    {
        // The refusal is scale-sensitive and both gates on the normal-equations path are scale
        // invariant, so Fit answered this with a coefficient near 1e11 while Estimate threw.
        var seeded = new Random(1095);
        var design = new double[200 * 2];
        var response = new double[200];
        for (int i = 0; i < 200; i++)
        {
            design[(i * 2) + 0] = seeded.NextDouble();
            design[(i * 2) + 1] = seeded.NextDouble() * 1e-14;
            response[i] = seeded.NextDouble();
        }

        ArgumentException fromFit = Assert.Throws<ArgumentException>(
            () => OrdinaryLeastSquares.Fit(design, response, 2));
        ArgumentException fromEstimate = Assert.Throws<ArgumentException>(
            () => OrdinaryLeastSquares.Estimate(design, response, 2, withIntercept: true));

        Assert.Equal("design", fromFit.ParamName);
        Assert.Equal(fromEstimate.Message, fromFit.Message);
    }
#pragma warning restore S2245, CA5394
}