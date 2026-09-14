using Lodestar.Stats.Internal;
using Xunit;

// SonarLint S2245 / CA5394: a seeded Random builds reproducible samples large enough to
// reach the radix path; nothing here is a secret, and the corpus must not vary per run.
#pragma warning disable S2245, CA5394

namespace Lodestar.Stats.Tests.Internal;

/// <summary>
/// The merged rank sum against the pooled ranking it replaced. The frozen corpus never
/// reaches the radix threshold, so these pin that path, signed zeros and infinities.
/// </summary>
public sealed class TwoSampleRanksTests
{
    private static readonly double[] Specials =
    [
        double.NegativeInfinity, -double.MaxValue, -3.5, -1.0, -double.Epsilon, -0.0,
        0.0, double.Epsilon, 1e-300, 1.0, 2.5, double.MaxValue, double.PositiveInfinity,
    ];

    [Theory]
    [InlineData(1, 1, 0)]
    [InlineData(7, 3, 1)]
    [InlineData(3_071, 1, 2)]
    [InlineData(3_072, 3_072, 3)]
    [InlineData(300, 4_000, 4)]
    [InlineData(20_000, 19_999, 5)]
    public void Continuous_samples_agree_bit_for_bit_with_the_pooled_ranking(int n, int m, int seed)
    {
        Random random = new(seed);
        double[] x = Draw(n, () => (random.NextDouble() * 2e6) - 1e6);
        double[] y = Draw(m, () => (random.NextDouble() * 2e6) - 1e6);

        AssertAgrees(x, y);
    }

    [Theory]
    [InlineData(40, 60, 10)]
    [InlineData(4_000, 6_000, 11)]
    public void Tie_heavy_samples_with_signed_zeros_and_infinities_agree(int n, int m, int seed)
    {
        Random random = new(seed);
        double[] x = Draw(n, () => Specials[random.Next(Specials.Length)]);
        double[] y = Draw(m, () => Specials[random.Next(Specials.Length)]);

        AssertAgrees(x, y);
    }

    [Fact]
    public void Rounded_samples_tie_across_both_groups()
    {
        Random random = new(12);
        double[] x = Draw(5_000, () => Math.Round((random.NextDouble() * 20) - 10, 1));
        double[] y = Draw(5_000, () => Math.Round((random.NextDouble() * 20) - 10, 1));

        AssertAgrees(x, y);
    }

    [Fact]
    public void Keys_order_as_the_doubles_do_and_fold_the_two_zeros()
    {
        for (int i = 1; i < Specials.Length; i++)
        {
            ulong below = RankKeys.OrderKey(Specials[i - 1]);
            ulong above = RankKeys.OrderKey(Specials[i]);
            // S1244: -0.0 and +0.0 are the one pair exact equality is meant to catch.
#pragma warning disable S1244
            if (Specials[i] == 0.0 && Specials[i - 1] == 0.0)
#pragma warning restore S1244
            {
                Assert.Equal(below, above);
            }
            else
            {
                Assert.True(below < above);
            }
        }
    }

    private static double[] Draw(int length, Func<double> next)
    {
        double[] values = new double[length];
        for (int i = 0; i < length; i++)
        {
            values[i] = next();
        }

        return values;
    }

    private static void AssertAgrees(double[] x, double[] y)
    {
        double[] pooled = [.. x, .. y];
        double[] ranks = Ranks.Average(pooled);
        double expectedRankSum = 0.0;
        for (int i = 0; i < x.Length; i++)
        {
            expectedRankSum += ranks[i];
        }

        (double rankSum, double tieCorrection, bool hasTies) = TwoSampleRanks.Compute(x, y);

        Assert.Equal(expectedRankSum, rankSum);
        Assert.Equal(Ranks.TieCorrection(pooled), tieCorrection);
        Assert.Equal(Ranks.HasTies(pooled), hasTies);
    }
}
