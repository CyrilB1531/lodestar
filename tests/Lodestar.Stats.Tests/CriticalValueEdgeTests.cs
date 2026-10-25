using Xunit;

namespace Lodestar.Stats.Tests;

/// <summary>The interval multipliers at a level near one and a one-sided level near zero (#1729's Review A).</summary>
public sealed class CriticalValueEdgeTests
{
    private static readonly double[] X = [1.0, 2.0, 3.0, 4.0, 5.0, 6.0];
    private static readonly double[] Y = [2.0, 1.0, 4.0, 3.0, 6.0, 5.0];

    [Fact]
    public void A_one_sided_correlation_interval_at_a_tiny_level_keeps_its_bound()
    {
        // The level's own quantile, as main took it: 1 − level would round to one below 2^-53, and the bound to tanh(∓∞).
        PearsonResult result = Pearson.Test(X, Y, Alternative.Less);
        double expected = Math.Tanh(Math.Atanh(result.Statistic) + (Distributions.NormalQuantile(1e-20) / Math.Sqrt(3.0)));

        Assert.Equal(expected, result.ConfidenceInterval(1e-20).High, 1e-15);
    }

    [Fact]
    public void A_two_sided_correlation_interval_near_a_level_of_one_keeps_its_bounds()
    {
        // 0.5 + level/2 rounded to one at a level of 1 − 2^-53, whose multiplier read +∞; against 60-digit bounds.
        (double low, double high) = Pearson.Test(X, Y).ConfidenceInterval(1.0 - Math.Pow(2.0, -53.0));

        Assert.Equal(-0.99851992618116308, low, 1e-15);
        Assert.Equal(0.99998698198956556, high, 1e-15);
    }

    [Fact]
    public void A_one_sided_Wilson_interval_at_a_tiny_level_keeps_its_bound()
    {
        // The level's own quantile, as main took it, where 1 − level would round to one; against the 60-digit bound.
        (double low, double high) = Binomial.Test(7, 20, 0.5, Alternative.Less).ProportionConfidenceInterval(1e-20, ProportionInterval.Wilson);

        Assert.Equal(0.0, low);
        Assert.Equal(0.025225934668433872, high, 1e-15);
    }

    [Fact]
    public void A_one_sided_t_interval_at_a_tiny_level_keeps_its_bound()
    {
        // 1 − level rounded to one below 2^-53, and the quantile refused a p the caller never passed.
        TTestResult result = TTest.OneSample(X, 3.0, Alternative.Less);
        double expected = result.Estimate + (Distributions.StudentQuantile(1e-20, result.DegreesOfFreedom) * result.StandardError);

        Assert.Equal(expected, result.ConfidenceInterval(1e-20).High, 1e-12 * Math.Abs(expected));
    }

    [Fact]
    public void An_exact_binomial_interval_at_a_tiny_one_sided_level_is_not_empty()
    {
        // 1 − level rounded to one, and the upper bound read the inverse at zero: the interval (0, 0); against 60 digits.
        (double low, double high) = Binomial.Test(7, 20, 0.5, Alternative.Less).ProportionConfidenceInterval(1e-20);

        Assert.Equal(0.0, low);
        Assert.Equal(0.00072927112473155368, high, 1e-17);
    }

    [Fact]
    public void An_exact_binomial_interval_near_a_level_of_one_keeps_its_upper_bound()
    {
        // 1 − (1 − level)/2 kept four digits of the tail at 1 − 1e-12; against the 60-digit Clopper-Pearson bound.
        (_, double high) = Binomial.Test(7, 20).ProportionConfidenceInterval(1.0 - 1e-12);

        Assert.Equal(0.95119148726475436, high, 1e-15);
    }
}
