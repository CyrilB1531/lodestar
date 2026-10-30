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

    [Theory]
    [InlineData(20.08848583018674, 18.641436304853226, 0.36696995284402706, 0.633030047155973, 0.0585068686549086)]
    [InlineData(13.729803494036245, 18.155148448716147, 0.7552192294005173, 0.2447807705994827, 0.06277185611822893)]
    [InlineData(6.793788149148612, 13.0, 0.9124632868509178, 0.08753671314908217, 0.048467343948910974)]
    [InlineData(5.269460987549943, 19.0, 0.9991680364805987, 0.0008319635194013346, 0.0011333544911156072)]
    [InlineData(6.284229515503238, 14.0, 0.9588109400866277, 0.04118905991337234, 0.02886467434381428)]
    [InlineData(1.5916415368477606, 0.02314310525444686, 0.0036570005471619925, 0.996342999452838, 0.0032933000067921023)]
    [InlineData(1.5811806933156474, 0.18221136044027492, 0.03091824972496042, 0.9690817502750396, 0.026786469573052866)]
    [InlineData(12.718429259219405, 13.132563250294217, 0.48038381880484676, 0.5196161811951533, 0.07910071878722569)]
    [InlineData(0.0001801140974382492, 17.302608278534656, 1.0, 6.021309756383449e-41, 2.8921498474824567e-36)]
    [InlineData(1.0708155002437939e-23, 16.98267403867735, 1.0, 2.1235975370279155e-203, 1.68396725451174e-179)]
    [InlineData(6.214243569358266e-24, 14.09958924081893, 1.0, 3.387368619407038e-170, 3.8428254064887973e-146)]
    [InlineData(2.0000000028284273e+18, 2e+18, 0.07864959335543396, 0.921350406644566, 7.338132432661527e-11)]
    [InlineData(1.9999999936754447e+18, 2e+18, 0.9992172988033449, 0.0007827011966551886, 1.3440260803232585e-12)]
    [InlineData(2.0000000000089444e+22, 2e+22, 0.32735895437104867, 0.6726410456289513, 1.804886230723793e-12)]
    public void A_chi_squared_holds_eight_ulps_of_each_tail_and_its_density(double x, double df, double upper, double lower, double density)
    {
        // Against 80 digits: the double exponent held 141 ulps of the first Q, the small shapes' T 12.3, the series' sum 11
        // (Review A, pass 40); past 1e15 the log's absolute 1e-32 times the shape 188 (pass 38).
        Assert.Equal(upper, Distributions.ChiSquaredSf(x, df), 8.0 * (Math.BitIncrement(upper) - upper));
        Assert.Equal(lower, Distributions.ChiSquaredCdf(x, df), 8.0 * (Math.BitIncrement(lower) - lower));
        Assert.Equal(density, Distributions.ChiSquaredPdf(x, df), 8.0 * (Math.BitIncrement(density) - density));
    }

    [Theory]
    [InlineData(22601.65145783713, 22601.651457837128, 0.49874906732770147, 0.0018763842494890202)]
    [InlineData(2.107432938431407, 0.201755843827637, 0.022364387436144453, 0.017641361362769285)]
    [InlineData(1.964676919938341, 0.33285524666896016, 0.04364521144030941, 0.034085607053608524)]
    [InlineData(2.872886813338203, 1.0652769006763974, 0.09824397314144531, 0.06022490320476642)]
    [InlineData(1000.0, 0.08781584325561224, 8.401225822802801e-222, 4.208629286341451e-222)]
    [InlineData(1e-5, 94.86279235655199, 1.0, 1.2704906026319878e-305)]
    public void A_chi_squared_tail_near_its_small_shapes_and_its_mean_holds_eight_ulps(double x, double df, double upper, double density)
    {
        // In 80 digits: 123 ulps at x just past df, 196 at a Q of 0.02, 255 of the density at x = 1000, 414 where its front is subnormal (Review A, pass 39).
        Assert.Equal(upper, Distributions.ChiSquaredSf(x, df), 8.0 * (Math.BitIncrement(upper) - upper));
        Assert.Equal(density, Distributions.ChiSquaredPdf(x, df), 8.0 * (Math.BitIncrement(density) - density));
    }
}
