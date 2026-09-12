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

    [Fact]
    public void The_factorial_table_accumulates_across_more_than_one_response()
    {
        double[] mean = [1.0, 3.0];
        double[] response = [2.0, 4.0];

        // long-comment: hand-computed expectations for each row; third line shows why accumulation matters.
        // Row 0: 2 log 1 - 1 - log(2!) = -1 - log 2.
        // Row 1: 4 log 3 - 3 - log(4!) = 4 log 3 - 3 - log 24.
        // The second row's table entry is reached only by accumulating past the first.
        double expected =
            ((2.0 * Math.Log(1.0)) - 1.0 - Math.Log(2.0)) +
            ((4.0 * Math.Log(3.0)) - 3.0 - Math.Log(24.0));

        Assert.Equal(expected, LogLikelihood.Of(GlmFamily.Poisson, response, mean), 12);
    }
}
