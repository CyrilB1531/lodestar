using Lodestar.Stats.Internal;
using Xunit;

namespace Lodestar.Stats.Tests.Internal;

/// <summary>The complementary error function and the standard normal's upper tail.</summary>
public sealed class NormalTests
{
    [Theory]
    [InlineData(0.0, 1.0)]
    [InlineData(0.5, 0.4795001221869535)]
    [InlineData(1.0, 0.15729920705028516)]
    [InlineData(2.0, 0.004677734981047266)]
    // Negative arguments come back through erfc(-x) = 2 - erfc(x).
    [InlineData(-1.0, 1.8427007929497148)]
    public void Erfc_matches_the_published_values(double x, double expected)
    {
        Assert.Equal(expected, Normal.Erfc(x), 1e-14);
    }

    [Theory]
    // The three sigma landmarks, to fifteen digits.
    [InlineData(0.0, 0.5)]
    [InlineData(1.0, 0.15865525393145707)]
    [InlineData(1.959963984540054, 0.025)]
    [InlineData(-1.0, 0.8413447460685429)]
    public void Sf_matches_the_normal_landmarks(double z, double expected)
    {
        Assert.Equal(expected, Normal.Sf(z), 1e-14);
    }

    [Fact]
    public void Sf_stays_accurate_in_the_far_tail()
    {
        // Relative: P(Z > 10) is 7.6e-24, and an absolute check at 1e-9 would
        // accept a hard zero here.
        Assert.Equal(1.0, Normal.Sf(10.0) / 7.61985302416047e-24, 1e-9);
    }

    [Theory]
    // Relative, for Sf's reason; 1.105665 is near the sweep's worst point and 26.5 the last half
    // step before erfc leaves the normal doubles. Values: scipy.special.erfc.
    [InlineData(1.105665, 0.11790062272966388)]
    [InlineData(5.0, 1.5374597944280347e-12)]
    [InlineData(10.0, 2.0884875837625446e-45)]
    [InlineData(26.0, 5.663192408856145e-296)]
    [InlineData(26.5, 2.2109076642637343e-307)]
    public void Erfc_stays_accurate_to_the_last_normal_double(double x, double expected)
    {
        Assert.Equal(1.0, Normal.Erfc(x) / expected, 1e-12);
    }

    [Theory]
    // erfc(27.3) is 4e-326, under half the smallest subnormal: zero is the rounded value.
    [InlineData(27.3)]
    [InlineData(40.0)]
    public void Erfc_rounds_to_zero_past_the_subnormals(double x)
    {
        Assert.Equal(0.0, Normal.Erfc(x));
    }

    [Fact]
    public void Erfc_and_Sf_are_zero_at_positive_infinity()
    {
        Assert.Equal(0.0, Normal.Erfc(double.PositiveInfinity));
        Assert.Equal(0.0, Normal.Sf(double.PositiveInfinity));
    }

    [Theory]
    [InlineData(0.5, 0.0)]
    [InlineData(0.025, 1.959963984540054)]
    [InlineData(0.05, 1.6448536269514722)]
    [InlineData(0.975, -1.959963984540054)]
    [InlineData(1e-10, 6.361340902404056)]
    public void Quantile_inverts_the_upper_tail(double p, double expected)
    {
        Assert.Equal(expected, Normal.Quantile(p), 1e-9);
    }

    [Theory]
    // scipy.stats.norm.isf at each p. Relative, because an absolute 1e-9 on z = 37 says
    // nothing about the eleventh digit the far tail is resolved to.
    [InlineData(1e-20, 9.262340089798409)]
    [InlineData(1e-100, 21.273453560965322)]
    [InlineData(1e-300, 37.0470962993612)]
    public void Quantile_reaches_the_far_tail(double p, double expected)
    {
        Assert.Equal(1.0, Normal.Quantile(p) / expected, 1e-13);
    }

    [Fact]
    public void The_rational_seed_already_agrees_with_the_inverted_tail()
    {
        // A mistyped AS 241 coefficient would still converge after Newton; this is what
        // catches it, in each of the approximation's three regions.
        foreach (double p in new[] { 0.3, 0.075, 0.025, 1e-8, 1e-12, 1e-50, 1e-300 })
        {
            Assert.Equal(1.0, Normal.RationalUpperQuantile(p) / Normal.Quantile(p), 1e-14);
        }
    }

    [Fact]
    public void Quantile_refuses_a_probability_outside_the_open_unit_interval()
    {
        Assert.Throws<ArgumentOutOfRangeException>(() => Normal.Quantile(0.0));
        Assert.Throws<ArgumentOutOfRangeException>(() => Normal.Quantile(1.0));
    }
}
