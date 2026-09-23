using System.Text.Json;
using Lodestar.Stats.Internal;
using Lodestar.Stats.Tests.Oracles;
using Xunit;

namespace Lodestar.Stats.Tests.Internal;

/// <summary>Replays <c>tests/oracles/stats_beta_quantile.json</c>, and the round trip it rests on.</summary>
/// <remarks>
/// A corpus of its own because the inversion has no test family: proving it only through the
/// binomial intervals that use it would let a systematic bias hide inside a wider tolerance.
/// </remarks>
public sealed class BetaQuantileTests
{
    private const double Tolerance = 1e-9;

    [Fact]
    public void Every_case_matches_scipy()
    {
        using JsonDocument document = StatsCorpus.Load("stats_beta_quantile.json");
        int replayed = 0;

        foreach (JsonElement c in document.RootElement.GetProperty("cases").EnumerateArray())
        {
            string name = c.GetProperty("name").GetString()!;
            double a = c.GetProperty("a").GetDouble();
            double b = c.GetProperty("b").GetDouble();
            double p = c.GetProperty("p").GetDouble();
            double expected = StatsCorpus.Number(c.GetProperty("x"));

            double actual = BetaQuantile.Invert(p, a, b);

            double relative = Math.Abs(actual - expected) / Math.Abs(expected);
            Assert.True(
                relative <= Tolerance,
                $"{name}: {actual} differs from {expected} by {relative} relative.");
            replayed++;
        }

        Assert.True(replayed >= 100, $"only {replayed} cases replayed");
    }

    /// <summary>
    /// The inverse against this package's own forward function, which is the check the corpus
    /// cannot make: scipy's own pair disagrees past <c>p = 1e-300</c>, so agreement with
    /// <c>betaincinv</c> proves nothing there and self-consistency is what is left.
    /// </summary>
    [Theory]
    [InlineData(1e-300, 1000.0, 20.0)]
    [InlineData(1e-200, 1000.0, 10000.0)]
    [InlineData(1e-30, 50.0, 50.0)]
    [InlineData(0.5, 2.0, 5.0)]
    [InlineData(0.975, 5.0, 2.0)]
    public void The_inverse_round_trips_through_the_forward_function(double p, double a, double b)
    {
        double x = BetaQuantile.Invert(p, a, b);
        double back = Beta.RegularizedIncomplete(a, b, x);

        double relative = Math.Abs(back - p) / p;
        Assert.True(relative <= 1e-9, $"I_{x}({a}, {b}) = {back}, asked for {p} ({relative} relative).");
    }

    [Fact]
    public void The_closed_endpoints_answer_themselves()
    {
        Assert.Equal(0.0, BetaQuantile.Invert(0.0, 2.0, 3.0));
        Assert.Equal(1.0, BetaQuantile.Invert(1.0, 2.0, 3.0));
    }

    /// <summary>A root no double can name is zero, not wherever the iteration stopped.</summary>
    [Fact]
    public void A_root_below_the_smallest_double_is_zero()
    {
        Assert.Equal(0.0, BetaQuantile.Invert(1e-300, 0.5, 3.0));
    }

    [Theory]
    [InlineData(-0.1, 2.0, 3.0)]
    [InlineData(1.1, 2.0, 3.0)]
    [InlineData(double.NaN, 2.0, 3.0)]
    [InlineData(0.5, 0.0, 3.0)]
    [InlineData(0.5, 2.0, -1.0)]
    public void An_argument_outside_its_range_is_refused(double p, double a, double b)
    {
        Assert.Throws<ArgumentOutOfRangeException>(() => BetaQuantile.Invert(p, a, b));
    }
}
