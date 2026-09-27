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
    public void Trimming_everything_away_leaves_no_mean()
    {
        Assert.True(double.IsNaN(GroupSpread.TrimmedMean([1.0, 2.0, 3.0, 4.0], 0.5)));
    }

    /// <summary>scipy refuses a NaN proportion and a count below zero, and answers NaN for one that trims everything (#1217).</summary>
    [Fact]
    public void A_negative_or_NaN_proportion_is_refused_and_a_full_trim_answers_NaN()
    {
        double[] a = [1, 2, 3, 4];
        double[] b = [2, 3, 4, 9];
        Assert.Throws<ArgumentOutOfRangeException>(() => Levene.Test(Center.Trimmed, -0.5, NanPolicy.Propagate, a, b));
        Assert.Throws<ArgumentOutOfRangeException>(() => Fligner.Test(Center.Trimmed, double.NaN, NanPolicy.Propagate, a, b));
        Assert.True(double.IsNaN(Levene.Test(Center.Trimmed, 0.5, NanPolicy.Propagate, a, b).PValue));
        Assert.True(double.IsNaN(Fligner.Test(Center.Trimmed, 0.5, NanPolicy.Propagate, a, b).PValue));

        // A small negative truncates to no trim at all, as scipy's int() does; past the middle is refused.
        Assert.Equal(Levene.Test(Center.Trimmed, 0.0, NanPolicy.Propagate, a, b), Levene.Test(Center.Trimmed, -0.1, NanPolicy.Propagate, a, b));
        Assert.Throws<ArgumentOutOfRangeException>(
            () => Levene.Test(Center.Trimmed, 0.6, NanPolicy.Propagate, [1, 2, 3, 4, 5], [2, 3, 4, 9, 1]));
        Assert.Throws<ArgumentOutOfRangeException>(
            () => Fligner.Test(Center.Trimmed, 0.6, NanPolicy.Propagate, [1, 2], [1, 2, 3, 4, 5]));
        Assert.Throws<ArgumentOutOfRangeException>(
            () => Levene.Test(Center.Trimmed, double.PositiveInfinity, NanPolicy.Propagate, [1, 2, 3], [1, 2, 3, 4]));
        Assert.Equal(Levene.Test(Center.Mean, 0.05, NanPolicy.Propagate, a, b), Levene.Test(Center.Mean, -1.0, NanPolicy.Propagate, a, b));
    }
}

#pragma warning restore S2245, CA5394
