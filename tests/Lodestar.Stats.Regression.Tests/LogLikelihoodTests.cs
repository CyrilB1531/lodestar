using Lodestar.Stats.Regression.Internal;
using Xunit;

namespace Lodestar.Stats.Regression.Tests;

public sealed class LogLikelihoodTests
{
    [Fact]
    public void A_binomial_log_likelihood_is_the_sum_of_log_probabilities()
    {
        double[] mean = [0.25, 0.75];
        double[] response = [0.0, 1.0];

        // log(1 - 0.25) + log(0.75)
        Assert.Equal(
            Math.Log(0.75) + Math.Log(0.75),
            LogLikelihood.Of(GlmFamily.Binomial, response, mean), 12);
    }

    [Fact]
    public void A_poisson_log_likelihood_carries_the_factorial_term()
    {
        double[] mean = [2.0];
        double[] response = [3.0];

        // 3 log 2 - 2 - log(3!) = 3 log 2 - 2 - log 6
        Assert.Equal(
            (3.0 * Math.Log(2.0)) - 2.0 - Math.Log(6.0),
            LogLikelihood.Of(GlmFamily.Poisson, response, mean), 12);
    }
}
