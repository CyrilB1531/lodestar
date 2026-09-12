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
    public void A_poisson_count_above_the_bound_is_refused()
    {
        // 2e6 is past the million this fit bounds the response at; the refusal is what stops
        // Internal/LogLikelihood allocating a log-factorial table indexed by it.
        ArgumentException refusal = Assert.Throws<ArgumentException>(() => GeneralizedLinearModel.Fit(
            Design, [0.0, 1.0, 2.0, 0.0, 1.0, 2_000_000.0], featureCount: 1, GlmFamily.Poisson));

        Assert.Contains("log-factorial", refusal.Message, StringComparison.Ordinal);
    }

    [Fact]
    public void A_count_at_the_bound_is_still_fitted()
    {
        GlmSummary summary = GeneralizedLinearModel.Fit(
            Design, [0.0, 1.0, 2.0, 0.0, 1.0, 1_000_000.0], featureCount: 1, GlmFamily.Poisson);

        Assert.True(summary.Converged);
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
    public void An_all_zero_poisson_response_is_fitted_rather_than_looping()
    {
        // The starting mean is mean(y) = 0 here, where the log link is -Infinity: clamped, the
        // first weight is a number and the fit converges instead of exhausting its budget.
        GlmSummary summary = GeneralizedLinearModel.Fit(
            Design, [0.0, 0.0, 0.0, 0.0, 0.0, 0.0], featureCount: 1, GlmFamily.Poisson);

        Assert.True(summary.Converged);
        Assert.Equal(0.0, summary.Deviance, 1e-9);
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
