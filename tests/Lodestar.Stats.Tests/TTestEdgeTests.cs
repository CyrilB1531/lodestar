using Xunit;

namespace Lodestar.Stats.Tests;

/// <summary>What the t-test refuses, and the one default that is not scipy's.</summary>
public sealed class TTestEdgeTests
{
    [Fact]
    public void Independent_defaults_to_Welch_where_scipy_defaults_to_Student()
    {
        double[] a = [1.0, 2.0, 3.0, 4.0];
        double[] b = [2.0, 3.0, 8.0, 12.0, 15.0, 20.0];

        TTestResult chosen = TTest.Independent(a, b);
        TTestResult welch = TTest.Independent(a, b, Alternative.TwoSided, Variance.Welch);
        TTestResult student = TTest.Independent(a, b, Alternative.TwoSided, Variance.Equal);

        Assert.Equal(welch.DegreesOfFreedom, chosen.DegreesOfFreedom);
        Assert.NotEqual(student.DegreesOfFreedom, chosen.DegreesOfFreedom);
    }

    [Fact]
    public void Welch_degrees_of_freedom_need_not_be_whole()
    {
        double[] a = [1.0, 2.0, 3.0, 4.0];
        double[] b = [2.0, 3.0, 8.0, 12.0, 15.0, 20.0];

        double df = TTest.Independent(a, b, Alternative.TwoSided, Variance.Welch).DegreesOfFreedom;

        Assert.NotEqual(df, Math.Round(df));
    }

    [Fact]
    public void Independent_refuses_an_empty_sample_and_answers_one_value_as_scipy()
    {
        Assert.Throws<ArgumentException>(() => TTest.Independent([], [1.0, 2.0]));

        // scipy's ttest_ind([1,2,3],[0]): pooled t = 1.7320508, p = 0.2254033, df = 2; Welch nan with df 1 (#1217).
        TTestResult pooled = TTest.Independent([1.0, 2.0, 3.0], [0.0], Alternative.TwoSided, Variance.Equal);
        Assert.Equal(1.7320508075688774, pooled.Statistic, 12);
        Assert.Equal(0.22540333075851665, pooled.PValue, 12);
        Assert.Equal(2.0, pooled.DegreesOfFreedom);
        TTestResult welch = TTest.Independent([1.0, 2.0, 3.0], [0.0]);
        Assert.True(double.IsNaN(welch.Statistic));
        Assert.Equal(1.0, welch.DegreesOfFreedom);
        (double low, double high) = TTest.OneSample([4.0], 0.0).ConfidenceInterval();
        Assert.True(double.IsNaN(low) && double.IsNaN(high));
    }

    [Fact]
    public void Paired_refuses_samples_of_different_length()
    {
        Assert.Throws<ArgumentException>(() => TTest.Paired([1.0, 2.0, 3.0], [1.0, 2.0]));
    }

    [Fact]
    public void OneSample_refuses_a_non_finite_population_mean()
    {
        Assert.Throws<ArgumentOutOfRangeException>(
            () => TTest.OneSample([1.0, 2.0, 3.0], double.NaN));
        Assert.Throws<ArgumentOutOfRangeException>(
            () => TTest.OneSample([1.0, 2.0, 3.0], double.PositiveInfinity));
    }

    [Fact]
    public void A_one_sided_interval_is_half_open_rather_than_narrower()
    {
        double[] a = [1.0, 2.0, 3.0, 4.0, 5.0];

        (double low, double high) = TTest.OneSample(a, 0.0, Alternative.Greater)
            .ConfidenceInterval(0.95);

        Assert.True(double.IsFinite(low));
        Assert.True(double.IsPositiveInfinity(high));
    }

    [Fact]
    public void ConfidenceInterval_refuses_a_level_outside_the_open_unit_interval()
    {
        TTestResult result = TTest.OneSample([1.0, 2.0, 3.0], 0.0);

        Assert.Throws<ArgumentOutOfRangeException>(() => result.ConfidenceInterval(0.0));
        Assert.Throws<ArgumentOutOfRangeException>(() => result.ConfidenceInterval(1.0));
    }

    [Fact]
    public void A_NaN_in_the_sample_propagates_rather_than_being_dropped()
    {
        // The spec's ruling: nan_policy is not a parameter here, and the remarks
        // say a caller who wants scipy's 'omit' filters the array themselves.
        TTestResult result = TTest.OneSample([1.0, double.NaN, 3.0], 0.0);

        Assert.True(double.IsNaN(result.Statistic));
        Assert.True(double.IsNaN(result.PValue));
    }

    /// <summary>A kept NaN makes every figure NaN, the degrees of freedom and the interval included, as scipy's are.</summary>
    [Fact]
    public void A_propagated_NaN_leaves_no_degrees_of_freedom_and_no_interval()
    {
        TTestResult result = TTest.OneSample([1.0, double.NaN, 3.0], 0.0);

        Assert.True(double.IsNaN(result.DegreesOfFreedom));
        (double low, double high) = result.ConfidenceInterval();
        Assert.True(double.IsNaN(low) && double.IsNaN(high));
    }

    /// <summary>∞ − ∞ is a NaN of the differences, not of the samples, and scipy keeps n − 1 there.</summary>
    [Fact]
    public void Infinite_pairs_keep_their_degrees_of_freedom()
    {
        TTestResult result = TTest.Paired([double.PositiveInfinity, 1.0, 2.0], [double.PositiveInfinity, 0.0, 5.0]);

        Assert.Equal(2.0, result.DegreesOfFreedom);
    }
}
