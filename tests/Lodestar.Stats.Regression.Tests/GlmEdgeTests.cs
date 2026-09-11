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
