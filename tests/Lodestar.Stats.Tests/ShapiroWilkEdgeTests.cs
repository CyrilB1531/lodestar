using Xunit;

namespace Lodestar.Stats.Tests;

/// <summary>What Shapiro-Wilk refuses, and the range Royston's approximation covers.</summary>
public sealed class ShapiroWilkEdgeTests
{
    [Fact]
    public void Refuses_fewer_than_three_values()
    {
        Assert.Throws<ArgumentException>(() => ShapiroWilk.Test([1.0, 2.0]));
    }

    [Fact]
    public void Refuses_a_sample_with_no_spread_as_Royston_s_algorithm_does()
    {
        // Every value identical: a zero range, AS R94's sixth fault.
        Assert.Throws<ArgumentException>(() => ShapiroWilk.Test([2.0, 2.0, 2.0, 2.0]));
    }

    [Fact]
    public void Computes_above_five_thousand_as_Royston_s_algorithm_does()
    {
        // AS R94 only warns past 5000 (ifault 2) that its fitted transform is extrapolated, and computes; scipy 1.18.1's
        // shapiro of 0 to 5000, which computes the same way.
        double[] many = [.. Enumerable.Range(0, 5001).Select(i => (double)i)];

        TestResult result = ShapiroWilk.Test(many);
        Assert.Equal(0.9549060254127775, result.Statistic, 1e-12);
        Assert.Equal(9.347784518192051e-37, result.PValue, 1e-9 * 9.347784518192051e-37);
    }

    [Fact]
    public void A_normal_looking_sample_is_not_rejected()
    {
        double[] sample =
        [
            -1.62, -1.10, -0.74, -0.47, -0.23, 0.0, 0.23, 0.47, 0.74, 1.10, 1.62,
        ];

        TestResult result = ShapiroWilk.Test(sample);

        Assert.True(result.Statistic is > 0.9 and <= 1.0);
        Assert.True(result.PValue > 0.05);
    }
}
