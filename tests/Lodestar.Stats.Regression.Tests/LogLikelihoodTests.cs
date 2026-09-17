using System.Text.Json;
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
            LogLikelihood.Of(FamilyShape.Of(GlmFamily.Binomial), response, mean, 1.0), 12);
    }

    [Fact]
    public void A_poisson_log_likelihood_carries_the_factorial_term()
    {
        double[] mean = [2.0];
        double[] response = [3.0];

        // 3 log 2 - 2 - log(3!) = 3 log 2 - 2 - log 6
        Assert.Equal(
            (3.0 * Math.Log(2.0)) - 2.0 - Math.Log(6.0),
            LogLikelihood.Of(FamilyShape.Of(GlmFamily.Poisson), response, mean, 1.0), 12);
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

        Assert.Equal(expected, LogLikelihood.Of(FamilyShape.Of(GlmFamily.Poisson), response, mean, 1.0), 12);
    }

    public static TheoryData<int> LogFactorialCases()
    {
        using JsonDocument corpus = OracleLoader.Load("regression_log_factorial.json");
        var data = new TheoryData<int>();
        for (int i = 0; i < corpus.RootElement.GetProperty("cases").GetArrayLength(); i++)
        {
            data.Add(i);
        }

        return data;
    }

    [Theory]
    [MemberData(nameof(LogFactorialCases))]
    public void The_log_factorial_matches_scipy_gammaln_relatively(int index)
    {
        // Relative, as the GLM corpus beside it: the counts reach 2^53, where log(k!) is 3e17 (#665).
        using JsonDocument corpus = OracleLoader.Load("regression_log_factorial.json");
        JsonElement frozen = corpus.RootElement.GetProperty("cases")[index];
        double count = frozen.GetProperty("count").GetDouble();
        double expected = frozen.GetProperty("logFactorial").GetDouble();

        double actual = LogLikelihood.LogFactorial(count);

        double error = expected == 0.0 ? Math.Abs(actual) : Math.Abs(actual - expected) / Math.Abs(expected);
        Assert.True(error <= 1e-9, $"log({count}!) = {actual:R}, gammaln gives {expected:R}, relative error {error:R}");
    }

    public static TheoryData<int> LogGammaCases()
    {
        using JsonDocument corpus = OracleLoader.Load("regression_log_gamma.json");
        var data = new TheoryData<int>();
        for (int i = 0; i < corpus.RootElement.GetProperty("cases").GetArrayLength(); i++)
        {
            data.Add(i);
        }

        return data;
    }

    [Theory]
    [MemberData(nameof(LogGammaCases))]
    public void The_log_gamma_matches_scipy_gammaln(int index)
    {
        // Relative past one and absolute below it, where lnGamma crosses zero between 1 and 2 (#769).
        using JsonDocument corpus = OracleLoader.Load("regression_log_gamma.json");
        JsonElement frozen = corpus.RootElement.GetProperty("cases")[index];
        double x = frozen.GetProperty("x").GetDouble();
        double expected = frozen.GetProperty("logGamma").GetDouble();

        double actual = LogLikelihood.LogGamma(x);

        double error = Math.Abs(actual - expected) / Math.Max(1.0, Math.Abs(expected));
        Assert.True(error <= 1e-9, $"lnGamma({x}) = {actual:R}, gammaln gives {expected:R}, error {error:R}");
    }

    [Fact]
    public void A_negative_binomial_log_likelihood_carries_its_gamma_terms()
    {
        double[] response = [0.0, 3.0];
        double[] mean = [1.5, 2.0];
        const double alpha = 0.5;

        // loglike_obs by hand: theta = 2, so lnGamma(y + 2) - lnGamma(2) - ln(y!) is 0 for y = 0 and ln(4·3·2/6) = ln 4 for y = 3.
        double expected =
            (0.0 - (2.0 * Math.Log(1.75)))
            + ((3.0 * Math.Log(1.0)) - (5.0 * Math.Log(2.0)) + Math.Log(4.0));

        Assert.Equal(expected, LogLikelihood.Of(new FamilyShape(GlmFamily.NegativeBinomial, GlmLink.Log, alpha), response, mean, 1.0), 12);
    }

    [Fact]
    public void The_series_takes_over_from_the_table_without_a_step()
    {
        // log(256!) - log(255!) is log 256; a series and a table disagreeing at the boundary would show here.
        double last = LogLikelihood.LogFactorial(LogLikelihood.TabulatedCounts - 1);
        double first = LogLikelihood.LogFactorial(LogLikelihood.TabulatedCounts);

        Assert.Equal(Math.Log(LogLikelihood.TabulatedCounts), first - last, 10);
    }
    [Fact]
    public void A_negative_binomial_count_read_back_from_its_cache_gives_the_bits_computed_afresh()
    {
        // Repeated counts hit the cache, 300 lies past it and a lone row always misses it: the sum of lone rows is the reference.
        double[] response = [0.0, 3.0, 3.0, 300.0, 0.0, 7.0, 3.0, 300.0];
        double[] mean = [0.4, 2.5, 3.5, 280.0, 1.2, 6.0, 2.9, 310.0];
        var shape = new FamilyShape(GlmFamily.NegativeBinomial, GlmLink.Log, 0.7);

        double expected = 0.0;
        for (int row = 0; row < response.Length; row++)
        {
            expected += LogLikelihood.Of(shape, response.AsSpan(row, 1), [mean[row]], 1.0);
        }

        Assert.Equal(
            BitConverter.DoubleToInt64Bits(expected),
            BitConverter.DoubleToInt64Bits(LogLikelihood.Of(shape, response, mean, 1.0)));
    }
}
