using Lodestar.Stats.Internal;
using Xunit;

namespace Lodestar.Stats.Tests.Internal;

// SonarLint S2245, CA5394: a seeded Random builds a reproducible fixture; no security use.
#pragma warning disable S2245, CA5394

/// <summary>The selected median against the sorted one it replaces, and the trimmed mean's rule.</summary>
public sealed class GroupSpreadTests
{
    [Theory]
    [InlineData(1)]
    [InlineData(2)]
    [InlineData(3)]
    [InlineData(16)]
    [InlineData(17)]
    [InlineData(257)]
    public void The_selected_median_matches_the_sorted_one(int length)
    {
        var random = new System.Random(1121 + length);
        double[] values = new double[length];
        for (int i = 0; i < length; i++)
        {
            // A small range on purpose: repeated values are where a selection goes wrong.
            values[i] = random.Next(0, 6);
        }

        double[] sorted = (double[])values.Clone();
        Array.Sort(sorted);
        double expected = (length % 2) == 1
            ? sorted[length / 2]
            : 0.5 * (sorted[(length / 2) - 1] + sorted[length / 2]);

        Assert.Equal(expected, GroupSpread.Median(values));
    }

    [Fact]
    public void The_median_leaves_the_caller_s_array_alone()
    {
        double[] values = [5.0, 1.0, 4.0, 2.0, 3.0];

        GroupSpread.Median(values);

        Assert.Equal([5.0, 1.0, 4.0, 2.0, 3.0], values);
    }

    /// <summary>An already sorted group is the input a naive pivot turns quadratic.</summary>
    [Fact]
    public void A_sorted_group_is_selected_correctly()
    {
        double[] ascending = [.. Enumerable.Range(0, 1001).Select(i => (double)i)];
        double[] descending = [.. Enumerable.Range(0, 1001).Select(i => 1000.0 - i)];

        Assert.Equal(500.0, GroupSpread.Median(ascending));
        Assert.Equal(500.0, GroupSpread.Median(descending));
    }

    /// <summary>scipy's rule truncates, so a small proportion trims nothing at all.</summary>
    [Fact]
    public void The_trimmed_mean_truncates_the_count_it_drops()
    {
        double[] values = [1.0, 2.0, 3.0, 4.0, 5.0];

        Assert.Equal(3.0, GroupSpread.TrimmedMean(values, 0.05), 12);
        Assert.Equal(3.0, GroupSpread.TrimmedMean(values, 0.25), 12);
    }

    [Fact]
    public void Trimming_everything_away_is_refused()
    {
        Assert.Throws<ArgumentOutOfRangeException>(
            () => GroupSpread.TrimmedMean([1.0, 2.0, 3.0, 4.0], 0.5));
    }
}

#pragma warning restore S2245, CA5394
