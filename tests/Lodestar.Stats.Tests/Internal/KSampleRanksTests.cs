using Lodestar.Stats.Internal;
using Xunit;

// SonarLint S2245 / CA5394: a seeded Random builds reproducible samples large enough to
// reach the radix path; nothing here is a secret, and the corpus must not vary per run.
#pragma warning disable S2245, CA5394

namespace Lodestar.Stats.Tests.Internal;

/// <summary>
/// Kruskal-Wallis' merged rank sums and tie term against the pooled ranking they replaced (#719),
/// compared as bits: group counts from two to the merge's limit, both sides of the radix threshold,
/// tie-heavy groups with signed zeros and infinities.
/// </summary>
public sealed class KSampleRanksTests
{
    private static readonly double[] Specials =
    [
        double.NegativeInfinity, -double.MaxValue, -3.5, -1.0, -double.Epsilon, -0.0,
        0.0, double.Epsilon, 1e-300, 1.0, 2.5, double.MaxValue, double.PositiveInfinity,
    ];

    [Theory]
    [InlineData(2, 5, 0)]
    [InlineData(3, 3_071, 1)]
    [InlineData(3, 3_072, 2)]
    [InlineData(5, 4_000, 3)]
    [InlineData(16, 700, 4)]
    public void Continuous_groups_agree_bit_for_bit_with_the_pooled_ranking(int k, int length, int seed)
    {
        Random random = new(seed);
        double[][] groups = Groups(k, g => length + g, () => (random.NextDouble() * 2e6) - 1e6);

        AssertAgrees(groups);
    }

    [Theory]
    [InlineData(3, 40, 10)]
    [InlineData(4, 5_000, 11)]
    public void Tie_heavy_groups_with_signed_zeros_and_infinities_agree(int k, int length, int seed)
    {
        Random random = new(seed);
        double[][] groups = Groups(k, _ => length, () => Specials[random.Next(Specials.Length)]);

        AssertAgrees(groups);
    }

    [Fact]
    public void Groups_of_unequal_sizes_sharing_rounded_values_agree()
    {
        Random random = new(12);
        double[][] groups = Groups(6, g => 1 + (g * 1_500), () => Math.Round(random.NextDouble() * 10, 1));

        AssertAgrees(groups);
    }

    private static double[][] Groups(int k, Func<int, int> length, Func<double> next)
    {
        double[][] groups = new double[k][];
        for (int g = 0; g < k; g++)
        {
            groups[g] = new double[length(g)];
            for (int i = 0; i < groups[g].Length; i++)
            {
                groups[g][i] = next();
            }
        }

        return groups;
    }

    private static void AssertAgrees(double[][] groups)
    {
        double[] pooled = [.. groups.SelectMany(group => group)];
        double[] ranks = Ranks.Average(pooled);
        double[] expected = new double[groups.Length];
        int offset = 0;
        for (int g = 0; g < groups.Length; g++)
        {
            for (int i = 0; i < groups[g].Length; i++)
            {
                expected[g] += ranks[offset + i];
            }

            offset += groups[g].Length;
        }

        double[] sums = new double[groups.Length];
        double tieCorrection = KSampleRanks.Compute(groups, pooled.Length, sums);

        for (int g = 0; g < groups.Length; g++)
        {
            Assert.Equal(BitConverter.DoubleToInt64Bits(expected[g]), BitConverter.DoubleToInt64Bits(sums[g]));
        }

        Assert.Equal(BitConverter.DoubleToInt64Bits(Ranks.TieCorrection(pooled)), BitConverter.DoubleToInt64Bits(tieCorrection));
    }
}
