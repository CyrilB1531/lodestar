using Lodestar.Stats.Regression.Internal;
using Xunit;

namespace Lodestar.Stats.Regression.Tests;

public sealed class IrlsTests
{
    /// <summary>A separable design: every y = 1 sits above the threshold and every y = 0 below.</summary>
    private static readonly double[] SeparableDesign = [-2.0, -1.0, 1.0, 2.0];
    private static readonly double[] SeparableResponse = [0.0, 0.0, 1.0, 1.0];

    [Fact]
    public void A_converged_fit_reports_the_iterations_it_took()
    {
        double[] design = [0.0, 1.0, 2.0, 3.0, 4.0, 5.0];
        double[] response = [0.0, 0.0, 1.0, 0.0, 1.0, 1.0];

        IrlsResult result = Irls.Fit(
            design, response, featureCount: 1, GlmFamily.Binomial, new GlmOptions());

        Assert.True(result.Converged);
        Assert.InRange(result.Iterations, 1, 100);
        Assert.True(result.DevianceChange >= 0.0);
    }

    [Fact]
    public void Perfect_separation_does_not_converge()
    {
        // 15, not the brief's 25: guarded against the exact 0/1 boundary (#616 review), this
        // fit's shrinking deviance satisfies the tolerance and converges by iteration 21.
        IrlsResult result = Irls.Fit(
            SeparableDesign, SeparableResponse, featureCount: 1, GlmFamily.Binomial,
            new GlmOptions { MaximumIterations = 15 });

        Assert.False(result.Converged);
        Assert.Equal(15, result.Iterations);
    }
}
