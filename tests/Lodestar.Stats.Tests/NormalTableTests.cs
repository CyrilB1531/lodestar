using Lodestar.Stats.Internal;
using Xunit;

namespace Lodestar.Stats.Tests;

/// <summary>The normal tail's erfcx table, written by tools/generate_normal_table.py, and the tails it carries (#1756).</summary>
public sealed class NormalTableTests
{
    [Fact]
    public void Neighbouring_intervals_meet_where_they_join()
    {
        // A coefficient copied wrong moves its polynomial at both ends: erfcx is continuous, so the two sides of a join
        // agree to its 1.2e-16 fit, and to rounding.
        for (int interval = 0; interval < Normal.IntervalCount - 1; interval++)
        {
            double join = Normal.LowerEnd(interval);
            double above = Normal.ScaledErfcIn(interval, join);
            double below = Normal.ScaledErfcIn(interval + 1, join);
            Assert.True(Math.Abs(above - below) <= 2e-15 * above, $"interval {interval} at {join}: {above} against {below}");
        }
    }

    [Theory]
    [InlineData(12.74, 1.7719741278795751e-37)]
    [InlineData(37.0, 5.725571222524577e-300)]
    public void The_normal_tail_holds_its_last_digits(double z, double expected)
    {
        // Against a 70-digit erfc: the table sampled at type initialization read up to 59 ulps off.
        Assert.Equal(expected, Distributions.NormalSf(z), 4.0 * (Math.BitIncrement(expected) - expected));
    }

    [Fact]
    public void Students_tail_from_its_normal_limit_keeps_its_first_term_in_one_over_df()
    {
        // Φ̄(37) + φ(37)(37³ + 37)/(4 · 1e20) in 70 digits: the normal tail alone is 40 ulps below it.
        const double expected = 5.7255712225246036e-300;
        Assert.Equal(expected, Distributions.StudentSf(37.0, 1e20), 4.0 * (Math.BitIncrement(expected) - expected));
    }

    [Theory]
    [InlineData(25.3, 4.046994415418954e-140, 4.046994415418958e-140)]
    [InlineData(8.3, 4.3816394355093325e-16, 4.381639435509333e-16)]
    public void The_normal_density_and_Student_s_from_df_1e20_hold_their_last_digits(double t, double normal, double student)
    {
        // In 60 digits: e^(−z²/2) dropped z²'s rounding, 19 ulps at 25.3, and Student's density read it from df 1e20.
        Assert.Equal(normal, Distributions.NormalPdf(t), 4.0 * (Math.BitIncrement(normal) - normal));
        Assert.Equal(student, Distributions.StudentPdf(t, 1e20), 4.0 * (Math.BitIncrement(student) - student));
    }

    [Theory]
    [InlineData(3122.857247446631, 2820.7711170896755, true, 4.9001616401350874e-05)]
    [InlineData(4728.428697126431, 6609.813053916451, false, 1.4152798924531643e-74)]
    public void A_chi_squared_tail_through_Temme_s_expansion_holds_its_last_digits(double x, double df, bool upper, double expected)
    {
        // In 90 digits, by the series and the continued fraction alike: through η √(a/2) erfc took 2y² times the
        // argument's roundings, 862 ulps of the first, and σ³ in doubles 99 of the second.
        double actual = upper ? Distributions.ChiSquaredSf(x, df) : Distributions.ChiSquaredCdf(x, df);
        Assert.Equal(expected, actual, 4.0 * (Math.BitIncrement(expected) - expected));
    }

    [Theory]
    [InlineData(3122.857247446631, 2820.7711170896755, true, 4.9001616401350874e-05)]
    [InlineData(4728.428697126431, 6609.813053916451, false, 1.4152798924531643e-74)]
    public void A_chi_squared_tail_s_logarithm_through_Temme_s_expansion_takes_its_exponent_whole(
        double x, double df, bool upper, double expected)
    {
        // The quantile's Newton steps read this logarithm: from a double η it took 862 ulps of the first tail.
        double log = Gamma.LogTail(df / 2.0, x / 2.0, upper).Log;
        Assert.Equal(Math.Log(expected), log, 4.0 * 2.220446049250313e-16 * Math.Abs(log));
    }

    [Theory]
    [InlineData(4140.0, 6000.0, false, 6.472676297195156e-82)]
    [InlineData(9600.0, 6000.0, true, 5.169223140481224e-172)]
    [InlineData(3200.0, 2000.0, true, 7.313139074297604e-59)]
    [InlineData(1400.0, 2000.0, false, 1.0158583345333217e-26)]
    public void A_chi_squared_tail_beside_Temme_s_region_takes_its_exponent_whole(double x, double df, bool upper, double expected)
    {
        // e^−x Σ x^k/k! at integer shapes, in 120 digits: from a double σ the exponent took a σ² ε, 2,794 ulps of the second.
        double actual = upper ? Distributions.ChiSquaredSf(x, df) : Distributions.ChiSquaredCdf(x, df);
        Assert.Equal(expected, actual, 8.0 * (Math.BitIncrement(expected) - expected));
    }

    [Theory]
    [InlineData(2700.0, 2000.0, 1.000483520016733e-24)]
    [InlineData(1300.0, 2000.0, 8.004898532281071e-38)]
    [InlineData(9600.0, 6000.0, 9.70661799662083e-173)]
    public void The_chi_squared_density_at_a_large_df_takes_its_exponent_whole(double x, double df, double expected)
    {
        // x^(a−1) e^(−x/2) / (2^a (a − 1)!) in 80 digits, a = df/2.
        Assert.Equal(expected, Distributions.ChiSquaredPdf(x, df), 8.0 * (Math.BitIncrement(expected) - expected));
    }

    [Theory]
    [InlineData(1e-323, 20.0)]
    [InlineData(1e-310, 1e15)]
    [InlineData(1e-300, 1e290)]
    [InlineData(1e-200, 1e180)]
    public void A_chi_squared_at_an_x_whose_ratio_to_its_shape_underflows_reads_its_limits(double x, double df)
    {
        // x/a rounded to zero, and the exponent's log read its table at −90 (Review A, pass 34).
        Assert.Equal(0.0, Distributions.ChiSquaredPdf(x, df));
        Assert.Equal(0.0, Distributions.ChiSquaredCdf(x, df));
        Assert.Equal(1.0, Distributions.ChiSquaredSf(x, df));
    }

    [Fact]
    public void A_chi_squared_an_ulp_below_its_mean_keeps_its_two_tails()
    {
        // a η²/2 is some 2e-28 there, and its last bits read −4e-28: the root, and both tails, NaN (Review A, pass 35).
        const double df = 58706.7733311098;
        double x = 2.0 * Math.BitDecrement(df / 2.0);
        double sf = Distributions.ChiSquaredSf(x, df);

        Assert.Equal(0.49922382475834, sf, 1e-13);
        Assert.Equal(1.0, sf + Distributions.ChiSquaredCdf(x, df), 1e-15);
    }

    [Theory]
    [InlineData(1.1286223812051971E+17, 20.0)]
    [InlineData(5e298, 1e299)]
    [InlineData(2e299, 1e299)]
    public void A_chi_squared_whose_exponent_underflows_reads_a_positive_zero(double x, double df)
    {
        // Past 2^53 the exponent's low part passed one, and its factor turned the underflowed tail to −0 (Review A, pass 36).
        Assert.False(double.IsNegative(Distributions.ChiSquaredSf(x, df)));
        Assert.False(double.IsNegative(Distributions.ChiSquaredCdf(x, df)));
        Assert.False(double.IsNegative(Distributions.ChiSquaredPdf(x, df)));
    }
}
