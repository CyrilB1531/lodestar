using Lodestar.Stats.Internal;
using Xunit;

namespace Lodestar.Stats.Tests.Internal;

/// <summary>
/// The log-gamma and the two regularized incomplete gammas, at values whose
/// closed forms are known exactly, so the test does not need an oracle to say
/// what the answer is.
/// </summary>
public sealed class GammaTests
{
    private const double Tolerance = 1e-13;

    [Theory]
    // Gamma(n) = (n-1)!, so LogGamma(n) = log((n-1)!).
    [InlineData(1.0, 0.0)]
    [InlineData(2.0, 0.0)]
    [InlineData(3.0, 0.6931471805599453)]      // log 2
    [InlineData(6.0, 4.787491742782046)]       // log 120
    // Gamma(1/2) = sqrt(pi), so LogGamma(0.5) = log(pi)/2.
    [InlineData(0.5, 0.5723649429247001)]
    // Gamma(3/2) = sqrt(pi)/2.
    [InlineData(1.5, -0.1207822376352452)]
    // Below 0.5 the reflection formula takes over; Gamma(0.1) = 9.51350769866873.
    [InlineData(0.1, 2.252712651734206)]
    public void LogGamma_matches_the_closed_forms(double x, double expected)
    {
        Assert.Equal(expected, Gamma.LogGamma(x), Tolerance);
    }

    [Fact]
    public void LogGamma_refuses_a_non_positive_argument()
    {
        Assert.Throws<ArgumentOutOfRangeException>(() => Gamma.LogGamma(0.0));
        Assert.Throws<ArgumentOutOfRangeException>(() => Gamma.LogGamma(-1.5));
    }

    [Theory]
    // P(1, x) = 1 - exp(-x): the exponential distribution's CDF, exactly.
    [InlineData(1.0, 0.5, 0.3934693402873666)]
    [InlineData(1.0, 2.0, 0.8646647167633873)]
    [InlineData(1.0, 12.0, 0.9999938557876467)]
    // P(1/2, x) = erf(sqrt(x)); at x = 0.5 that is erf(1/sqrt2) = 0.6826894921370859,
    // which is also the standard normal's mass within one sigma.
    [InlineData(0.5, 0.5, 0.6826894921370859)]
    public void RegularizedP_matches_the_closed_forms(double a, double x, double expected)
    {
        Assert.Equal(expected, Gamma.RegularizedP(a, x), Tolerance);
    }

    [Theory]
    // The series is used below a + 1, the continued fraction above; both must
    // satisfy P + Q = 1, and the crossing is the seam a one-branch implementation gets wrong.
    [InlineData(3.0, 1.0)]
    [InlineData(3.0, 3.9)]
    [InlineData(3.0, 4.0)]
    [InlineData(3.0, 4.1)]
    [InlineData(3.0, 40.0)]
    [InlineData(0.5, 1e-8)]
    [InlineData(200.0, 200.0)]
    public void RegularizedP_and_Q_sum_to_one_across_the_branch_seam(double a, double x)
    {
        Assert.Equal(1.0, Gamma.RegularizedP(a, x) + Gamma.RegularizedQ(a, x), 1e-14);
    }

    [Theory]
    // Both sides of the finite sum's bounds (2a <= 100, x <= 700), so a seam with the iteration
    // shows; relative, as the tail reaches 1e-228. Values: scipy.special.gammaincc.
    [InlineData(0.5, 2.205, 0.03572884112563301)]
    [InlineData(1.0, 3.0, 0.04978706836786395)]
    [InlineData(1.5, 60.0, 7.716790355634162e-26)]
    [InlineData(2.0, 4.744, 0.04999440557799463)]
    [InlineData(2.5, 0.001, 0.9999999904914654)]
    [InlineData(50.0, 300.0, 2.41882858334655e-72)]
    [InlineData(50.0, 700.0, 4.4774296035996127e-228)]
    [InlineData(50.0, 701.0, 1.766309187969092e-228)]
    [InlineData(50.0, 55.0, 0.23220478050085636)]
    [InlineData(50.5, 55.0, 0.25400482476574116)]
    [InlineData(51.0, 55.0, 0.2767563591543263)]
    public void RegularizedQ_matches_scipy_on_both_sides_of_the_closed_form(
        double a, double x, double expected)
    {
        Assert.Equal(1.0, Gamma.RegularizedQ(a, x) / expected, 1e-12);
    }

    [Fact]
    public void RegularizedP_is_zero_at_the_origin_and_one_far_out()
    {
        Assert.Equal(0.0, Gamma.RegularizedP(2.0, 0.0));
        Assert.Equal(1.0, Gamma.RegularizedP(2.0, 400.0), 1e-15);
    }

    [Fact]
    public void RegularizedP_refuses_a_negative_x_or_a_non_positive_a()
    {
        Assert.Throws<ArgumentOutOfRangeException>(() => Gamma.RegularizedP(1.0, -1.0));
        Assert.Throws<ArgumentOutOfRangeException>(() => Gamma.RegularizedP(0.0, 1.0));
    }

    [Fact]
    public void RegularizedP_and_Q_take_the_limit_at_positive_infinity()
    {
        // a * log(x) - x is inf - inf = NaN at x = +inf if computed through the
        // series or continued fraction; the limit must be taken directly instead.
        Assert.Equal(1.0, Gamma.RegularizedP(3.0, double.PositiveInfinity));
        Assert.Equal(0.0, Gamma.RegularizedQ(3.0, double.PositiveInfinity));
    }
}
