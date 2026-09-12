using Xunit;

namespace Lodestar.Stats.Tests;

/// <summary>
/// Decision 0115: omission is a filter, so a family's own guards run afterwards. scipy answers
/// (nan, nan) with a warning in both cases below; this package raises, and omission does not
/// change that.
/// </summary>
public sealed class NanPolicyEdgeTests
{
    [Fact]
    public void Omitting_below_shapiro_wilks_floor_still_raises()
    {
        double[] sample = [1.0, double.NaN, double.NaN, double.NaN, 5.0];

        Assert.Throws<ArgumentException>(() => ShapiroWilk.Test(sample, NanPolicy.Omit));
    }

    [Fact]
    public void Omitting_into_a_fully_tied_pool_still_raises()
    {
        double[] a = [2.0, 2.0, double.NaN];
        double[] b = [2.0, 2.0, double.NaN];

        Assert.Throws<ArgumentException>(() => KruskalWallis.Test(NanPolicy.Omit, a, b));
    }

    // CA1030: the leading word reads to the analyzer as the event-pattern verb; here it
    // names the policy value this fact exercises, not a design smell.
#pragma warning disable CA1030
    [Fact]
    public void Raise_refuses_a_nan_and_names_the_parameter()
#pragma warning restore CA1030
    {
        double[] sample = [1.0, 2.0, double.NaN, 4.0];

        ArgumentException error = Assert.Throws<ArgumentException>(
            () => ShapiroWilk.Test(sample, NanPolicy.Raise));
        Assert.Equal("sample", error.ParamName);
    }

    [Fact]
    public void Propagate_carries_the_nan_into_the_result()
    {
        double[] sample = [1.0, 2.0, double.NaN, 4.0, 5.0];

        TestResult result = ShapiroWilk.Test(sample);

        Assert.True(double.IsNaN(result.Statistic));
    }

    [Fact]
    public void Omitting_an_aligned_expectation_breaks_the_sum_agreement()
    {
        double[] observed = [10.0, 20.0, double.NaN, 40.0];
        double[] expected = [15.0, 25.0, 30.0, 40.0];

        Assert.Throws<ArgumentException>(
            () => ChiSquare.GoodnessOfFit(observed, expected, NanPolicy.Omit));
    }
}
