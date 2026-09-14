using Lodestar.Stats.Internal;
using Xunit;

// SonarLint S2245 / CA5394: a seeded Random builds reproducible samples large enough to
// reach the radix path; nothing here is a secret, and the corpus must not vary per run.
#pragma warning disable S2245, CA5394

namespace Lodestar.Stats.Tests.Internal;

/// <summary>
/// The signed-rank sums from merged magnitudes against Wilcoxon's ranked path, added up in input
/// order as it adds them, compared as bits (#719): with and without ranked zeros, ties across signs.
/// </summary>
public sealed class SignedRanksTests
{
    [Theory]
    [InlineData(1, 0, false)]
    [InlineData(60, 1, true)]
    [InlineData(3_071, 2, false)]
    [InlineData(6_500, 3, true)]
    [InlineData(40_000, 4, false)]
    public void Continuous_differences_agree_bit_for_bit(int n, int seed, bool rankZeros)
    {
        Random random = new(seed);
        double[] values = Draw(n, () => (random.NextDouble() * 2e3) - 1e3);

        AssertAgrees(values, rankZeros);
    }

    [Theory]
    [InlineData(200, 10, true)]
    [InlineData(200, 11, false)]
    [InlineData(9_000, 12, true)]
    [InlineData(9_000, 13, false)]
    public void Rounded_differences_with_zeros_and_ties_across_signs_agree(int n, int seed, bool rankZeros)
    {
        // Rounded to tenths around zero: many zeros, and equal magnitudes on both sides of it.
        Random random = new(seed);
        double[] values = Draw(n, () => Math.Round((random.NextDouble() * 4) - 2, 1));

        AssertAgrees(values, rankZeros);
    }

    [Fact]
    public void A_sample_of_one_tie_group_agrees()
    {
        double[] values = [.. Enumerable.Repeat(-1.5, 5_000), .. Enumerable.Repeat(1.5, 5_000), .. Enumerable.Repeat(0.0, 10)];

        AssertAgrees(values, rankZeros: true);
        AssertAgrees(values, rankZeros: false);
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

    // S1244: a difference of exactly zero is the category Wilcoxon's zero methods decide on.
#pragma warning disable S1244
    private static void AssertAgrees(double[] values, bool rankZeros)
    {
        double[] ranked = rankZeros ? values : [.. values.Where(d => d != 0.0)];
        if (ranked.Length == 0)
        {
            return;
        }

        double[] ranks = Ranks.Average([.. ranked.Select(Math.Abs)]);
        double positive = 0.0;
        double negative = 0.0;
        double zeroRankSum = 0.0;
        double squares = 0.0;
        double nonZeroSquares = 0.0;
        for (int i = 0; i < ranked.Length; i++)
        {
            squares += ranks[i] * ranks[i];
            if (ranked[i] > 0.0)
            {
                positive += ranks[i];
                nonZeroSquares += ranks[i] * ranks[i];
            }
            else if (ranked[i] < 0.0)
            {
                negative += ranks[i];
                nonZeroSquares += ranks[i] * ranks[i];
            }
            else
            {
                zeroRankSum += ranks[i];
            }
        }

        SignedRanks.Sums sums = SignedRanks.Compute(values, rankZeros);

        AssertBits(positive, sums.Positive);
        AssertBits(negative, sums.Negative);
        AssertBits(zeroRankSum, sums.ZeroRankSum);
        AssertBits(squares, sums.Squares);
        AssertBits(nonZeroSquares, sums.NonZeroSquares);
    }
#pragma warning restore S1244

    private static void AssertBits(double expected, double actual) =>
        Assert.Equal(BitConverter.DoubleToInt64Bits(expected), BitConverter.DoubleToInt64Bits(actual));
}
