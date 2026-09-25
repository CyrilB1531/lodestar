using Lodestar.Stats.Internal;
using Xunit;

namespace Lodestar.Stats.Tests.Internal;

/// <summary>
/// The inverse incomplete gamma behind the chi-squared quantiles (#1158), held to its own tails:
/// whatever it answers, reading P or Q back at that point must give the probability again.
/// </summary>
public sealed class GammaQuantileTests
{
    public static TheoryData<double, double, bool> Corners() => new()
    {
        { 1e-60, 0.25, false }, { 1e-300, 0.25, true },
        { 1e-12, 5e4, false }, { 1e-12, 5e4, true },
        { 0.3, 0.05, false }, { 0.3, 1e-3, true },
        { 1e-100, 1.0, true }, { 0.5, 1e6, false },
        { 0.9999, 2.5, false }, { 0.9999, 2.5, true },
    };

    [Theory]
    [MemberData(nameof(Corners))]
    public void The_answer_reads_back_as_the_probability(double p, double shape, bool upper)
    {
        double x = GammaQuantile.Invert(p, shape, upper);
        double back = upper ? Gamma.RegularizedQ(shape, x) : Gamma.RegularizedP(shape, x);

        Assert.Equal(1.0, back / p, 1e-11);
    }

    [Fact]
    public void A_root_below_the_smallest_double_is_zero()
    {
        // P(0.01, x) ~ x^0.01 / Gamma(1.01): p = 1e-10 puts the root near 1e-1000.
        Assert.Equal(0.0, GammaQuantile.Invert(1e-10, 0.01, upper: false));
    }

    [Fact]
    public void A_shape_that_is_not_positive_is_refused()
    {
        Assert.Throws<ArgumentOutOfRangeException>(() => GammaQuantile.Invert(0.5, 0.0, upper: false));
        Assert.Throws<ArgumentOutOfRangeException>(() => GammaQuantile.Invert(0.5, double.NaN, upper: true));
    }
}
