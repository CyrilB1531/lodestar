using Xunit;

namespace Lodestar.Stats.Regression.Tests;

public sealed class GlmEdgeTests
{
    private static readonly double[] Design = [0.0, 1.0, 2.0, 3.0, 4.0, 5.0];
    private static readonly double[] Response = [0.0, 0.0, 1.0, 0.0, 1.0, 1.0];

    [Fact]
    public void A_design_and_response_of_mismatched_length_are_refused()
    {
        Assert.Throws<ArgumentException>(() => GeneralizedLinearModel.Fit(
            Design, [0.0, 1.0], featureCount: 1, GlmFamily.Binomial));
    }

    [Fact]
    public void An_empty_design_is_refused_as_an_empty_design()
    {
        // Not as a missing residual degree of freedom, which is what the length comparison
        // alone left it as: 0 values and 0 responses agree on every shape (#616).
        ArgumentException refusal = Assert.Throws<ArgumentException>(() => GeneralizedLinearModel.Fit(
            [], [], featureCount: 1, GlmFamily.Binomial));

        Assert.Equal("design", refusal.ParamName);
        Assert.Contains("not a positive whole number of rows", refusal.Message, StringComparison.Ordinal);
    }

    [Fact]
    public void A_feature_count_below_one_is_refused()
    {
        Assert.Throws<ArgumentOutOfRangeException>(() => GeneralizedLinearModel.Fit(
            Design, Response, featureCount: 0, GlmFamily.Binomial));
    }

    [Fact]
    public void A_binomial_response_outside_zero_and_one_is_refused()
    {
        // statsmodels reads a proportion here and means a grouped fit by it. Accepting it
        // silently would answer a question the caller did not ask (#616).
        Assert.Throws<ArgumentException>(() => GeneralizedLinearModel.Fit(
            Design, [0.0, 0.5, 1.0, 0.0, 1.0, 1.0], featureCount: 1, GlmFamily.Binomial));
    }

    [Theory]
    [InlineData(0.0)]
    [InlineData(-1.0)]
    [InlineData(double.NaN)]
    [InlineData(double.PositiveInfinity)]
    public void A_negative_binomial_alpha_that_is_not_finite_and_positive_is_refused(double alpha)
    {
        // statsmodels divides by zero at 0 and fails its first deviance below it (#769).
        Assert.Throws<ArgumentOutOfRangeException>(() => new GlmOptions { NegativeBinomialAlpha = alpha });
    }

    [Theory]
    [InlineData(GlmFamily.Binomial)]
    [InlineData(GlmFamily.Poisson)]
    public void An_alpha_given_to_a_family_without_one_is_refused(GlmFamily family)
    {
        ArgumentException error = Assert.Throws<ArgumentException>(() => GeneralizedLinearModel.Fit(
            Design, Response, featureCount: 1, family, new GlmOptions { NegativeBinomialAlpha = 0.5 }));

        Assert.Equal("options", error.ParamName);
    }

    [Fact]
    public void An_unset_alpha_is_the_reference_default_of_one()
    {
        double[] counts = [1.0, 0.0, 2.0, 3.0, 5.0, 4.0];

        GlmSummary unset = GeneralizedLinearModel.Fit(Design, counts, featureCount: 1, GlmFamily.NegativeBinomial);
        GlmSummary one = GeneralizedLinearModel.Fit(
            Design, counts, featureCount: 1, GlmFamily.NegativeBinomial, new GlmOptions { NegativeBinomialAlpha = 1.0 });

        Assert.Equal(one.Coefficients, unset.Coefficients);
        Assert.Equal(one.LogLikelihood, unset.LogLikelihood);
    }

    [Fact]
    public void A_fractional_negative_binomial_response_is_refused()
    {
        // The reference's gammaln would take 1.5; the two count families keep one rule (#769).
        ArgumentException error = Assert.Throws<ArgumentException>(() => GeneralizedLinearModel.Fit(
            Design, [0.0, 1.5, 2.0, 0.0, 1.0, 1.0], featureCount: 1, GlmFamily.NegativeBinomial));

        Assert.Equal("response", error.ParamName);
    }

    [Fact]
    public void A_vanishing_alpha_approaches_the_poisson_fit()
    {
        double[] counts = [1.0, 0.0, 2.0, 3.0, 5.0, 4.0];

        GlmSummary poisson = GeneralizedLinearModel.Fit(Design, counts, featureCount: 1, GlmFamily.Poisson);
        GlmSummary nearly = GeneralizedLinearModel.Fit(
            Design, counts, featureCount: 1, GlmFamily.NegativeBinomial, new GlmOptions { NegativeBinomialAlpha = 1e-8 });

        Assert.Equal(poisson.Coefficients[0], nearly.Coefficients[0], 1e-6);
        Assert.Equal(poisson.Coefficients[1], nearly.Coefficients[1], 1e-6);
    }

    [Fact]
    public void A_negative_poisson_response_is_refused()
    {
        Assert.Throws<ArgumentException>(() => GeneralizedLinearModel.Fit(
            Design, [0.0, -1.0, 2.0, 0.0, 1.0, 1.0], featureCount: 1, GlmFamily.Poisson));
    }

    [Fact]
    public void A_fractional_poisson_response_is_refused()
    {
        // The log-likelihood computes log(y!) from the response, which a count has and a
        // proportion does not; statsmodels reaches for a gamma function here instead.
        Assert.Throws<ArgumentException>(() => GeneralizedLinearModel.Fit(
            Design, [0.0, 1.5, 2.0, 0.0, 1.0, 1.0], featureCount: 1, GlmFamily.Poisson));
    }

    [Fact]
    public void A_poisson_count_past_the_old_bound_is_fitted()
    {
        // 2e9 is past the million the fit refused until #665 and past int.MaxValue, where the table
        // it indexed would have wrapped; log(y!) is constant-time now, so the fit simply runs.
        GlmSummary summary = GeneralizedLinearModel.Fit(
            Design, [0.0, 1.0, 2.0, 0.0, 1.0, 2_000_000_000.0], featureCount: 1, GlmFamily.Poisson);

        Assert.True(summary.Converged);
        Assert.True(double.IsFinite(summary.LogLikelihood));
    }

    [Fact]
    public void An_infinite_poisson_count_is_refused()
    {
        // Infinity truncates to itself, so only its own check stops it now the bound is gone.
        ArgumentException refusal = Assert.Throws<ArgumentException>(() => GeneralizedLinearModel.Fit(
            Design, [0.0, 1.0, 2.0, 0.0, 1.0, double.PositiveInfinity], featureCount: 1, GlmFamily.Poisson));

        Assert.Contains("finite non-negative integer", refusal.Message, StringComparison.Ordinal);
    }

    [Fact]
    public void A_rank_deficient_design_is_refused_as_one()
    {
        // The second regressor is twice the first. statsmodels takes a pseudo-inverse and reports
        // df_resid = nobs - rank; the QR here divides by a zero pivot, so it refuses instead.
        ArgumentException refusal = Assert.Throws<ArgumentException>(() => GeneralizedLinearModel.Fit(
            [1.0, 2.0, 2.0, 4.0, 3.0, 6.0, 4.0, 8.0, 5.0, 10.0, 6.0, 12.0],
            [0.0, 0.0, 1.0, 0.0, 1.0, 1.0],
            featureCount: 2,
            GlmFamily.Binomial));

        Assert.Equal("design", refusal.ParamName);
        Assert.Contains("rank-deficient or collinear", refusal.Message, StringComparison.Ordinal);
    }

    [Fact]
    public void An_all_zero_poisson_response_is_refused_rather_than_answered()
    {
        // Clamping the start would make this converge, and on a number set by the tolerance
        // rather than by the data: the maximum is at minus infinity. The reference refuses too.
        ArgumentException refusal = Assert.Throws<ArgumentException>(
            () => GeneralizedLinearModel.Fit(
                Design, [0.0, 0.0, 0.0, 0.0, 0.0, 0.0], featureCount: 1, GlmFamily.Poisson));

        Assert.Contains("every count is zero", refusal.Message, StringComparison.Ordinal);
    }

    [Fact]
    public void A_poisson_response_with_one_positive_count_is_fitted()
    {
        GlmSummary summary = GeneralizedLinearModel.Fit(
            Design, [0.0, 0.0, 0.0, 0.0, 0.0, 1.0], featureCount: 1, GlmFamily.Poisson);

        Assert.True(summary.Converged);
    }

    [Theory]
    [InlineData(0.0)]
    [InlineData(1.0)]
    [InlineData(double.NaN)]
    public void A_confidence_level_outside_the_open_unit_interval_is_refused(double level)
    {
        Assert.Throws<ArgumentOutOfRangeException>(() => new GlmOptions { ConfidenceLevel = level });
    }

    [Fact]
    public void An_iteration_budget_below_one_is_refused()
    {
        // A budget of zero skips the loop and reports zero coefficients over a zero inverse R,
        // which reaches a caller with ThrowOnNonConvergence off as a table of 0/0 (#616).
        Assert.Throws<ArgumentOutOfRangeException>(() => new GlmOptions { MaximumIterations = 0 });
    }

    [Theory]
    [InlineData(0.0)]
    [InlineData(-1e-8)]
    [InlineData(double.NaN)]
    public void A_tolerance_that_is_not_above_zero_is_refused(double tolerance)
    {
        Assert.Throws<ArgumentOutOfRangeException>(() => new GlmOptions { Tolerance = tolerance });
    }

    [Fact]
    public void Perfect_separation_throws_by_default()
    {
        // 15, not the default 100: IrlsTests documents this design converging by iteration 21
        // under boundary clamping, so a lower budget is what actually exercises non-convergence.
        Assert.Throws<InvalidOperationException>(() => GeneralizedLinearModel.Fit(
            [-2.0, -1.0, 1.0, 2.0], [0.0, 0.0, 1.0, 1.0],
            featureCount: 1, GlmFamily.Binomial,
            new GlmOptions { MaximumIterations = 15 }));
    }

    [Fact]
    public void Perfect_separation_returns_when_the_throw_is_turned_off()
    {
        GlmSummary summary = GeneralizedLinearModel.Fit(
            [-2.0, -1.0, 1.0, 2.0], [0.0, 0.0, 1.0, 1.0],
            featureCount: 1, GlmFamily.Binomial,
            new GlmOptions { MaximumIterations = 15, ThrowOnNonConvergence = false });

        Assert.False(summary.Converged);
    }
}
