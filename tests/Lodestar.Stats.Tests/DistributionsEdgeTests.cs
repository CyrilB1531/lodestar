using Xunit;

namespace Lodestar.Stats.Tests;

/// <summary>What the corpus does not reach: the arguments a published surface must refuse.</summary>
public sealed class DistributionsEdgeTests
{
    [Theory]
    [InlineData(0.0)]
    [InlineData(-1.0)]
    [InlineData(double.NaN)]
    public void Degrees_of_freedom_that_are_not_positive_are_refused(double df)
    {
        Assert.Throws<ArgumentOutOfRangeException>(() => Distributions.StudentSf(1.0, df));
        Assert.Throws<ArgumentOutOfRangeException>(() => Distributions.StudentQuantile(0.5, df));
        Assert.Throws<ArgumentOutOfRangeException>(() => Distributions.FisherSnedecorSf(1.0, df, 5.0));
        Assert.Throws<ArgumentOutOfRangeException>(() => Distributions.FisherSnedecorSf(1.0, 5.0, df));
        Assert.Throws<ArgumentOutOfRangeException>(() => Distributions.ChiSquaredSf(1.0, df));
    }

    /// <summary>
    /// Chi-squared has no mass below zero, so the upper tail there is the whole of it.
    /// The regularized Q underneath validates its own argument and would throw, which
    /// would leak an internal helper's parameter name out of a public method.
    /// </summary>
    [Theory]
    [InlineData(0.0)]
    [InlineData(-1.0)]
    [InlineData(-1e30)]
    public void Chi_squared_below_its_support_is_one_rather_than_a_throw(double x)
    {
        Assert.Equal(1.0, Distributions.ChiSquaredSf(x, 2.0));
    }

    /// <summary>
    /// The published tail and the internal one the chi-squared tests use are the same
    /// function, so a statistic routed either way gives the same p-value. This is the
    /// agreement decision 0003 asked for rather than a second approximation.
    /// </summary>
    [Fact]
    public void Chi_squared_agrees_with_the_test_that_already_used_it_internally()
    {
        ChiSquaredContingencyResult table = ChiSquared.Contingency(
            [[20.0, 30.0], [30.0, 20.0]], Continuity.None);

        Assert.Equal(
            table.PValue,
            Distributions.ChiSquaredSf(table.Statistic, table.DegreesOfFreedom),
            1e-12);
    }

    /// <summary>
    /// NaN is named in the guard rather than left to the comparison. It fails every
    /// ordering, so a plain <c>&lt;= 0</c> would have let it through into the tail and
    /// returned a NaN probability from a public method.
    /// </summary>
    [Fact]
    public void A_NaN_degree_of_freedom_throws_rather_than_propagating()
    {
        ArgumentOutOfRangeException error =
            Assert.Throws<ArgumentOutOfRangeException>(() => Distributions.StudentSf(1.0, double.NaN));

        Assert.Equal("df", error.ParamName);
    }

    [Theory]
    [InlineData(-0.1)]
    [InlineData(1.1)]
    [InlineData(double.NaN)]
    public void A_probability_outside_the_closed_unit_interval_is_refused(double p)
    {
        Assert.Throws<ArgumentOutOfRangeException>(() => Distributions.StudentQuantile(p, 5.0));
        Assert.Throws<ArgumentOutOfRangeException>(() => Distributions.NormalQuantile(p));
        Assert.Throws<ArgumentOutOfRangeException>(() => Distributions.StudentIsf(p, 5.0));
        Assert.Throws<ArgumentOutOfRangeException>(() => Distributions.NormalIsf(p));
        Assert.Throws<ArgumentOutOfRangeException>(() => Distributions.FisherSnedecorQuantile(p, 2.0, 3.0));
        Assert.Throws<ArgumentOutOfRangeException>(() => Distributions.FisherSnedecorIsf(p, 2.0, 3.0));
        Assert.Throws<ArgumentOutOfRangeException>(() => Distributions.ChiSquaredQuantile(p, 4.0));
        Assert.Throws<ArgumentOutOfRangeException>(() => Distributions.ChiSquaredIsf(p, 4.0));
    }

    /// <summary>
    /// The endpoints answer the support's ends, as scipy's do (#1158): until then 0 and 1 were
    /// refused by the two quantiles published first.
    /// </summary>
    [Fact]
    public void The_endpoints_answer_the_support_ends()
    {
        Assert.Equal(double.NegativeInfinity, Distributions.NormalQuantile(0.0));
        Assert.Equal(double.PositiveInfinity, Distributions.NormalQuantile(1.0));
        Assert.Equal(double.PositiveInfinity, Distributions.NormalIsf(0.0));
        Assert.Equal(double.NegativeInfinity, Distributions.NormalIsf(1.0));
        Assert.Equal(double.NegativeInfinity, Distributions.StudentQuantile(0.0, 5.0));
        Assert.Equal(double.PositiveInfinity, Distributions.StudentQuantile(1.0, 5.0));
        Assert.Equal(double.PositiveInfinity, Distributions.StudentIsf(0.0, 5.0));
        Assert.Equal(double.NegativeInfinity, Distributions.StudentIsf(1.0, 5.0));
        Assert.Equal(0.0, Distributions.FisherSnedecorQuantile(0.0, 2.0, 3.0));
        Assert.Equal(double.PositiveInfinity, Distributions.FisherSnedecorQuantile(1.0, 2.0, 3.0));
        Assert.Equal(double.PositiveInfinity, Distributions.FisherSnedecorIsf(0.0, 2.0, 3.0));
        Assert.Equal(0.0, Distributions.FisherSnedecorIsf(1.0, 2.0, 3.0));
        Assert.Equal(0.0, Distributions.ChiSquaredQuantile(0.0, 4.0));
        Assert.Equal(double.PositiveInfinity, Distributions.ChiSquaredQuantile(1.0, 4.0));
        Assert.Equal(double.PositiveInfinity, Distributions.ChiSquaredIsf(0.0, 4.0));
        Assert.Equal(0.0, Distributions.ChiSquaredIsf(1.0, 4.0));
    }

    [Fact]
    public void A_NaN_statistic_propagates_through_every_density_and_tail()
    {
        double[] answers =
        [
            Distributions.NormalPdf(double.NaN), Distributions.NormalCdf(double.NaN), Distributions.NormalSf(double.NaN),
            Distributions.StudentPdf(double.NaN, 3.0), Distributions.StudentCdf(double.NaN, 3.0), Distributions.StudentSf(double.NaN, 3.0),
            Distributions.FisherSnedecorPdf(double.NaN, 2.0, 3.0), Distributions.FisherSnedecorCdf(double.NaN, 2.0, 3.0), Distributions.FisherSnedecorSf(double.NaN, 2.0, 3.0),
            Distributions.ChiSquaredPdf(double.NaN, 4.0), Distributions.ChiSquaredCdf(double.NaN, 4.0), Distributions.ChiSquaredSf(double.NaN, 4.0),
        ];

        Assert.All(answers, answer => Assert.True(double.IsNaN(answer)));
    }

    [Fact]
    public void Below_the_support_a_density_and_a_lower_tail_are_zero_and_an_upper_tail_is_one()
    {
        Assert.Equal(0.0, Distributions.ChiSquaredPdf(-1.0, 4.0));
        Assert.Equal(0.0, Distributions.ChiSquaredCdf(-1.0, 4.0));
        Assert.Equal(1.0, Distributions.ChiSquaredSf(-1.0, 4.0));
        Assert.Equal(0.0, Distributions.FisherSnedecorPdf(-1.0, 2.0, 3.0));
        Assert.Equal(0.0, Distributions.FisherSnedecorCdf(-1.0, 2.0, 3.0));
        Assert.Equal(1.0, Distributions.FisherSnedecorSf(-1.0, 2.0, 3.0));
    }

    /// <summary>Past the split's exponent a huge df's density keeps its digits: from the rounded point's 106-bit front
    /// it is its inverse chi-squared limit, d/f² times the chi-squared density at d/f, scipy's 0.18150860439621125, where
    /// the double form read 1.4e-13 off (#1745's Review A).</summary>
    [Fact]
    public void A_density_past_the_split_s_exponent_keeps_its_limit() =>
        Assert.Equal(0.18150860439621125, Distributions.FisherSnedecorPdf(2.0, 1e300, 3.7), 1e-16);

    /// <summary>A value whose high part rounds to zero at the tie 2^-1075 rounds up on its positive low part, to the
    /// smallest double, once (#1745's Review A).</summary>
    [Fact]
    public void A_scaled_tie_at_the_last_granule_keeps_its_low_part() =>
        Assert.Equal(double.Epsilon, Lodestar.Stats.Internal.DoubleDouble.ScaledOnce(
            new Lodestar.Stats.Internal.DoubleDouble(1.0, 1e-17), -1075).Rounded);

    /// <summary>Where d₂ is below 2^-106 of d₁f the split's near-one side keeps its low part, so F's upper tail stays on
    /// the 106-bit expansion: at dfs (2001, 20) it read 280 ulps off and rose with f, against
    /// u^10·Π(1000.5 + k)/10!·₂F₁ in 60-digit Decimal (#1745's Review A).</summary>
    [Fact]
    public void A_near_one_split_keeps_the_upper_tail_exact()
    {
        double f = 2.5491802405212984e30;
        double tail = Distributions.FisherSnedecorSf(f, 2001.0, 20.0);
        Assert.Equal(2.4871586185862696e-301, tail, 1e-316);
        double previous = Distributions.FisherSnedecorSf(Math.BitDecrement(f), 2001.0, 20.0);
        for (int step = 0; step < 64; step++)
        {
            f = Math.BitIncrement(f);
            double next = Distributions.FisherSnedecorSf(f, 2001.0, 20.0);
            Assert.True(next <= previous, $"the upper tail rose at f = {f:R}");
            previous = next;
        }
    }

    /// <summary>Where some 2e8 doubles of f share the target's rounding the quantile is the inversion's root, within its
    /// tolerance of the target, rather than that run's edge, whose search took 133 us for scipy's 26 (#1745's Review A).</summary>
    [Theory]
    [InlineData(0.3, 3e-9, 7e-9)]
    [InlineData(0.7, 7e-9, 3e-9)]
    [InlineData(0.49999999999999994, 1e-8, 1e-8)]
    public void An_f_quantile_on_a_run_too_wide_to_search_keeps_its_root(double p, double dfn, double dfd)
    {
        double root = Distributions.FisherSnedecorIsf(p, dfn, dfd);
        Assert.Equal(p, Distributions.FisherSnedecorSf(root, dfn, dfd), p * 1e-9);
    }

    /// <summary>At tiny dfs F's tails hold their order where the fraction changes sides of the mode: log(s B) rounded to a
    /// double there entered the two sides with opposite signs, and at dfs (2e-8, 1e-8) the lower tail fell 2 ulps, against
    /// a 70-digit series' 0.3333333333333333 at both doubles (#1745's Review A).</summary>
    [Fact]
    public void Tiny_dfs_keep_their_tails_in_order_across_the_mode()
    {
        Assert.Equal(0.3333333333333333, Distributions.FisherSnedecorCdf(0.5000000024999999, 2e-8, 1e-8));
        Assert.Equal(0.3333333333333333, Distributions.FisherSnedecorCdf(0.5000000025, 2e-8, 1e-8));
        Assert.Equal(0.5, Distributions.FisherSnedecorSf(1.0000000000000002, 5e-9, 5e-9));
        double root = Distributions.FisherSnedecorIsf(0.49999999999999994, 5e-9, 5e-9);
        Assert.True(ExactQuantileContract.FisherUpperReaches(root, 5e-9, 5e-9, 0.49999999999999994));
        Assert.False(ExactQuantileContract.FisherUpperReaches(Math.BitDecrement(root), 5e-9, 5e-9, 0.49999999999999994));
    }

    /// <summary>A side near one takes its log from the other side, whose low part one less it dropped: b times it read
    /// FisherSnedecorPdf(245, 3.7, 1e300) 4.2e-14 off its 700-digit value (#1745's Review A).</summary>
    [Fact]
    public void A_density_beside_a_huge_df_keeps_its_near_one_side() =>
        Assert.Equal(5.0742557206258821e-195, Distributions.FisherSnedecorPdf(245.0, 3.7, 1e300), 1e-209);

    /// <summary>At the origin the density is infinite, finite or zero by the first shape alone, as scipy answers.</summary>
    [Fact]
    public void At_the_origin_a_density_follows_its_shape()
    {
        Assert.Equal(double.PositiveInfinity, Distributions.ChiSquaredPdf(0.0, 1.0));
        Assert.Equal(0.5, Distributions.ChiSquaredPdf(0.0, 2.0));
        Assert.Equal(0.0, Distributions.ChiSquaredPdf(0.0, 4.0));
        Assert.Equal(double.PositiveInfinity, Distributions.FisherSnedecorPdf(0.0, 1.0, 3.0));
        Assert.Equal(1.0, Distributions.FisherSnedecorPdf(0.0, 2.0, 3.0), 1e-14);
        Assert.Equal(0.0, Distributions.FisherSnedecorPdf(0.0, 4.0, 3.0));
    }

    /// <summary>
    /// The Cauchy's quantile at 1e-300 is 1/(π·1e-300) = 3.18e299. Past t = 1.3e154 the tail's
    /// square overflowed and read zero, so the inversion stopped there (#1158).
    /// </summary>
    [Fact]
    public void A_quantile_beyond_the_square_root_of_the_largest_double_is_reached()
    {
        double expected = 1.0 / (Math.PI * 1e-300);

        Assert.Equal(1.0, Distributions.StudentIsf(1e-300, 1.0) / expected, 1e-12);
        Assert.Equal(1.0, Distributions.StudentSf(expected, 1.0) / 1e-300, 1e-12);
    }

    /// <summary>
    /// The published member is the quantile, and the internal helper it delegates to is the
    /// inverse survival function — opposite signs, related by symmetry. A 95% multiplier is
    /// positive, which is what every printed table shows and what an interval adds.
    /// </summary>
    [Fact]
    public void The_quantile_is_the_cdf_inverse_and_not_the_survival_inverse()
    {
        double multiplier = Distributions.StudentQuantile(0.975, 12.0);

        Assert.True(multiplier > 0.0, $"a 95% multiplier is positive, got {multiplier}");
        Assert.Equal(0.025, Distributions.StudentSf(multiplier, 12.0), 1e-12);
    }

    /// <summary>The two-sided p-value a regression table reports is twice the one tail.</summary>
    [Fact]
    public void The_upper_tail_is_one_sided()
    {
        double oneSided = Distributions.StudentSf(2.0, 10.0);
        double symmetric = Distributions.StudentSf(-2.0, 10.0);

        Assert.Equal(1.0, oneSided + symmetric, 1e-12);
    }

    /// <summary>
    /// The median is exactly zero, and not negative zero: the internal helper answers
    /// zero there and a bare negation would publish -0 from a public method.
    /// </summary>
    [Fact]
    public void The_normal_quantile_of_a_half_is_positive_zero()
    {
        double median = Distributions.NormalQuantile(0.5);

        Assert.Equal(0.0, median);
        Assert.False(double.IsNegative(median));
    }

    /// <summary>
    /// The Student quantile approaches this one as its degrees of freedom grow, but it
    /// stops closing at about 1e-8 -- the measurement decisions 0003 and 0121 record, and the
    /// reason the normal one is published rather than approximated by a large df.
    /// </summary>
    [Fact]
    public void The_student_quantile_does_not_reach_the_normal_one()
    {
        double normal = Distributions.NormalQuantile(0.975);
        double student = Distributions.StudentQuantile(0.975, 1.0e8);

        Assert.NotEqual(normal, student, 1e-12);
        Assert.Equal(normal, student, 1e-7);
    }

    /// <summary>
    /// Past shapes of a million scipy's F density is itself off, by 5e-7 at (1e8, 1e8) and 1e-5 at
    /// (1e10, 1), so these points are held to a 60-digit reference instead: <c>log B</c> from
    /// Stirling's series in Python's <c>decimal</c>, the density's logarithm formed exactly (#1158).
    /// </summary>
    [Theory]
    [InlineData(1.0, 1e6, 1e6, 199.47109033293754)]
    [InlineData(1.0001, 1e6, 1e6, 199.202011941266)]
    [InlineData(1.0, 1e8, 1e8, 1994.711397020385)]
    [InlineData(0.9999, 1e8, 1e8, 1760.4806716170015)]
    [InlineData(1.0, 1e10, 1.0, 0.24197072450704482)]
    [InlineData(0.001, 1e10, 1.0, 8.988349475015364e-214)]
    [InlineData(10.0, 1e10, 1.0, 0.012000389483944348)]
    [InlineData(1e6, 1e10, 1.0, 3.989420809203688e-10)]
    public void The_F_density_holds_at_shapes_scipy_loses_digits_on(double f, double dfn, double dfd, double exact)
    {
        Assert.Equal(1.0, Distributions.FisherSnedecorPdf(f, dfn, dfd) / exact, 1e-12);
    }

    /// <summary>At an infinite statistic, or one whose product with the degrees of freedom overflows, the F laws answer their limits rather than NaN or an exception.</summary>
    [Fact]
    public void An_infinite_or_overflowing_F_statistic_answers_the_limits()
    {
        Assert.Equal(1.0, Distributions.FisherSnedecorCdf(double.PositiveInfinity, 2.0, 20.0));
        Assert.Equal(0.0, Distributions.FisherSnedecorSf(double.PositiveInfinity, 2.0, 20.0));
        // At +∞ the densities' limit is zero at every df; scipy's NaN past two is its formula forming ∞/∞ (decision 0010).
        Assert.Equal(0.0, Distributions.FisherSnedecorPdf(double.PositiveInfinity, 2.0, 20.0));
        Assert.Equal(0.0, Distributions.FisherSnedecorPdf(double.PositiveInfinity, 3.0, 20.0));
        Assert.Equal(0.0, Distributions.ChiSquaredPdf(double.PositiveInfinity, 2.0));
        Assert.Equal(0.0, Distributions.ChiSquaredPdf(double.PositiveInfinity, 3.0));
        Assert.Equal(1.0, Distributions.FisherSnedecorCdf(1e308, 10.0, 5.0));
        double density = Distributions.FisherSnedecorPdf(1e308, 10.0, 5.0);
        Assert.True(density >= 0.0 && !double.IsNaN(density), $"a density, got {density}");
    }

    /// <summary>Infinite degrees of freedom are the normal law for Student's t, as scipy's are (#1158's second review).</summary>
    [Fact]
    public void Infinite_degrees_of_freedom_are_the_normal_law_for_Student()
    {
        Assert.Equal(Distributions.NormalPdf(0.3), Distributions.StudentPdf(0.3, double.PositiveInfinity));
        Assert.Equal(Distributions.NormalCdf(-1.2), Distributions.StudentCdf(-1.2, double.PositiveInfinity));
        Assert.Equal(Distributions.NormalSf(1.2), Distributions.StudentSf(1.2, double.PositiveInfinity));
        Assert.Equal(Distributions.NormalQuantile(0.9), Distributions.StudentQuantile(0.9, double.PositiveInfinity));
        Assert.Equal(Distributions.NormalIsf(0.1), Distributions.StudentIsf(0.1, double.PositiveInfinity));
    }

    /// <summary>scipy answers nan for an infinite F or chi-squared count; here that has no meaning and is refused.</summary>
    [Fact]
    public void Infinite_degrees_of_freedom_are_refused_for_F_and_chi_squared()
    {
        Assert.Throws<ArgumentOutOfRangeException>(() => Distributions.FisherSnedecorCdf(1.0, 3.0, double.PositiveInfinity));
        Assert.Throws<ArgumentOutOfRangeException>(() => Distributions.FisherSnedecorPdf(1.0, double.PositiveInfinity, 3.0));
        Assert.Throws<ArgumentOutOfRangeException>(() => Distributions.ChiSquaredPdf(1.0, double.PositiveInfinity));
        Assert.Throws<ArgumentOutOfRangeException>(() => Distributions.ChiSquaredQuantile(0.5, double.PositiveInfinity));
    }

    /// <summary>A t quantile past the largest double is infinite, not the largest double (#1158's second review).</summary>
    [Fact]
    public void A_quantile_past_the_largest_double_is_infinite()
    {
        // On half a degree of freedom the 1e-300 point lies near 1e600.
        Assert.Equal(double.PositiveInfinity, Distributions.StudentIsf(1e-300, 0.5));
        Assert.Equal(double.NegativeInfinity, Distributions.StudentQuantile(1e-300, 0.5));
    }

    [Theory]
    [InlineData(0.75, 0.6744897501960817)]
    [InlineData(0.25, -0.6744897501960817)]
    [InlineData(0.51, 0.02506890825871106)]
    [InlineData(0.5, 0.0)]
    [InlineData(1e-20, -9.262340089798409)]
    [InlineData(0.999999999999, 7.0344869100478356)]
    [InlineData(0.1, -1.2815515655446004)]
    [InlineData(1e-300, -37.0470962993612)]
    [InlineData(0.975, 1.959963984540054)]
    public void The_normal_quantile_is_scipy_s_ndtri_to_the_last_bit(double p, double ppf)
    {
        // scipy 1.18.1's norm.ppf and norm.isf, ndtri(p) and -ndtri(p), measured on glibc: the tails go through
        // Math.Log, glibc's on Linux, so the bit is asserted there and 1e-14 relative where another libm rounds (#1684).
        if (OperatingSystem.IsLinux())
        {
            Assert.Equal(BitConverter.DoubleToInt64Bits(ppf), BitConverter.DoubleToInt64Bits(Distributions.NormalQuantile(p)));
            Assert.Equal(BitConverter.DoubleToInt64Bits(-ppf + 0.0), BitConverter.DoubleToInt64Bits(Distributions.NormalIsf(p)));
            return;
        }

        Assert.Equal(ppf, Distributions.NormalQuantile(p), 1e-14 * Math.Max(1.0, Math.Abs(ppf)));
        Assert.Equal(-ppf, Distributions.NormalIsf(p), 1e-14 * Math.Max(1.0, Math.Abs(ppf)));
    }

    [Theory]
    [InlineData(1e-300, 19, 0xC35502298BE600B8)]
    [InlineData(0.025, 19, 0xC000BE83653B666E)]
    [InlineData(0.975, 20, 0x4000B00D9A954438)]
    [InlineData(1e-20, 20, 0xC043AEA949D929D0)]
    [InlineData(0.025, 1999.9, 0xBFFF60DFC03A83D1)]
    [InlineData(1e-300, 1999.9, 0xC04635CBDBB49B90)]
    [InlineData(0.975, 2000, 0x3FFF60DFB049ED70)]
    [InlineData(1e-300, 2000, 0xC04635BDF09FB7D4)]
    [InlineData(0.4, 2001, 0xBFD037640F98655A)]
    [InlineData(1e-20, 2001, 0xC022BA3720EE0F19)]
    [InlineData(5e-324, 2000, 0xC04768CC558ABEBC)]
    public void Student_quantiles_keep_their_bits_across_the_cached_log_beta(double p, double df, ulong bits)
    {
        // Either side of 20 and of 2000, the smallest double whose lower tail before rounding reaches p against a 100-digit
        // series (#1745), at 5e-324 the inversion's root: #1729 moved (1e-300, 19).
        double expected = BitConverter.Int64BitsToDouble(unchecked((long)bits));
        if (OperatingSystem.IsLinux())
        {
            Assert.Equal(bits, unchecked((ulong)BitConverter.DoubleToInt64Bits(Distributions.StudentQuantile(p, df))));
            return;
        }

        Assert.Equal(expected, Distributions.StudentQuantile(p, df), 1e-12 * Math.Abs(expected));
    }

    [Theory]
    [InlineData(1.0)]
    [InlineData(5.0)]
    [InlineData(37.0)]
    public void At_df_1e20_the_normal_limit_continues_Students_law(double t)
    {
        // Just below 1e20 the general path; above, Normal.Sf, whose rounding of z / sqrt 2 costs 1.1e-13 relative at
        // t = 37, more than the laws' own (t^4 - 2t^2 - 1)/4df. Past 1e305 t^2 / (df + t^2) went subnormal (#1690's review).
        const double below = 9.999999999999998e19;
        double tail = Distributions.StudentSf(t, below);
        Assert.Equal(tail, Distributions.StudentSf(t, 1e20), 2e-13 * tail);
        Assert.Equal(Distributions.StudentIsf(tail, below), Distributions.StudentIsf(tail, 1e20), 1e-14 * t);
        Assert.Equal(Distributions.StudentSf(t, double.PositiveInfinity), Distributions.StudentSf(t, 1e308));
        Assert.Equal(Distributions.StudentIsf(tail, double.PositiveInfinity), Distributions.StudentIsf(tail, 1e308));
    }

    [Theory]
    [InlineData(1e-20)]
    [InlineData(1e-305)]
    [InlineData(1e-310)]
    [InlineData(5e-324)]
    public void To_df_1e_minus_20_the_Student_tail_is_one_half_and_every_quantile_infinite(double df)
    {
        // The tail is one half less (df/4)(|log x| + 2 log 2), which rounds away below df 1e-20 at every finite t; a
        // subnormal df read 0, and 5e-324 threw from a private parameter (#1690's review); scipy reads 0 at 1e300: 0007.
        Assert.Equal(0.5, Distributions.StudentSf(1.0, df));
        Assert.Equal(0.5, Distributions.StudentSf(-1e300, df));
        Assert.Equal(0.5, Distributions.StudentCdf(1e300, df));
        Assert.Equal(0.0, Distributions.StudentSf(double.PositiveInfinity, df));
        Assert.Equal(double.PositiveInfinity, Distributions.StudentIsf(0.4, df));
        Assert.Equal(double.NegativeInfinity, Distributions.StudentIsf(0.6, df));
        Assert.Equal(double.PositiveInfinity, 1.0 / Distributions.StudentQuantile(0.5, df));
    }

    [Theory]
    [InlineData(2e-20)]
    [InlineData(1e-19)]
    [InlineData(1e-17)]
    [InlineData(1e-15)]
    public void Above_df_1e_minus_20_the_Student_tail_stays_under_one_half_on_its_leading_term(double df)
    {
        // 0.5 - (df/4)(|log x| + 2 log 2) from t = 1, whose next terms are below 1e-22. log B, about -log(df/2), rounded
        // by 40 ulps of the tail, which read above one half for df 1e-19 and near t = sqrt(df) (#1690's review).
        for (double t = 1e-15; t < 1e300; t *= 1.3)
        {
            double tail = Distributions.StudentSf(t, df);
            Assert.True(tail <= 0.5, $"t = {t}");
            if (t >= 1.0)
            {
                double logX = Math.Log(df) - (2.0 * Math.Log(t));
                Assert.Equal(0.5 - (df / 4.0 * (-logX + (2.0 * Math.Log(2.0)))), tail, 2.5e-16);
            }
        }
    }

    [Theory]
    [InlineData(2e-20)]
    [InlineData(1.1319478155211762e-18)]
    [InlineData(1e-17)]
    [InlineData(1e-15)]
    [InlineData(1e-12)]
    [InlineData(1e-10)]
    public void Near_one_half_a_tiny_df_quantile_is_a_root_or_past_the_largest_double(double df)
    {
        // Newton settled on non-roots such as 1e-25 at p = 0.49999999999999917, df 1.13e-18 (#1690's review).
        for (int ulps = 1; ulps < 1e7; ulps = (ulps * 5 / 4) + 1)
        {
            double p = 0.5 - (ulps * 5.551115123125783e-17);
            double t = Distributions.StudentIsf(p, df);
            if (double.IsPositiveInfinity(t))
            {
                Assert.True(Distributions.StudentSf(double.MaxValue, df) >= p, $"p = {p}");
            }
            else
            {
                Assert.Equal(p, Distributions.StudentSf(t, df), 4.5e-16);
            }
        }
    }

    [Theory]
    [InlineData("sf", 0.5, 1e-300, 5.0, 3.458988703428305e-298)]
    [InlineData("sf", 0.5, 1e-12, 5.0, 1.4326616951585553e-11)]
    [InlineData("sf", 0.5, 1e-08, 5.0, 9.721446295860417e-08)]
    [InlineData("sf", 10.0, 0.001999, 0.5, 0.006844795660804509)]
    [InlineData("sf", 10.0, 0.199, 5.0, 0.04003121171781387)]
    [InlineData("sf", 10.0, 0.201, 5.0, 0.04001490761240753)]
    [InlineData("sf", 1000000.0, 1e-05, 50.0, 1.0762591106912994e-08)]
    [InlineData("sf", 0.01, 0.01, 1.0, 0.05156554867142635)]
    [InlineData("sf", 0.5, 1e-06, 1e-06, 0.5000001732866716)]
    [InlineData("cdf", 10.0, 5.0, 1e-12, 1.5131335907790648e-11)]
    [InlineData("cdf", 1.0, 1.0, 1e-19, 2.256770556400338e-18)]
    [InlineData("cdf", 0.01, 5.0, 0.001999, 0.002033024785919047)]
    [InlineData("cdf", 10.0, 50.0, 0.199, 0.3375142864476618)]
    [InlineData("cdf", 10.0, 50.0, 0.201, 0.3396406807734091)]
    [InlineData("t", 1.0, 1e-12, 0, 0.4999999999927457)]
    [InlineData("t", 100000.0, 1e-06, 0, 0.4999904431775774)]
    [InlineData("t", 1e+60, 0.199, 0, 4.3199993936142057e-13)]
    [InlineData("t", 1e+60, 0.201, 0, 3.271402342968299e-13)]
    [InlineData("t", 0.001, 0.1, 0, 0.49985190814779157)]
    public void A_shape_below_one_tenth_holds_scipy(string tail, double x, double first, double second, double expected)
    {
        // scipy 1.18.1's f.sf, f.cdf and t.sf, which a 400-digit series matched to 9e-16. Below a shape of 0.1, log B
        // cancelled and the reflection took 1 - I: 1.7e-8 off at (0.5, 1e-8, 5) on main, zero at 1e-300 (#1690's review).
        double actual = tail switch
        {
            "sf" => Distributions.FisherSnedecorSf(x, first, second),
            "cdf" => Distributions.FisherSnedecorCdf(x, first, second),
            _ => Distributions.StudentSf(x, first),
        };
        Assert.Equal(expected, actual, 5e-15 * expected);
    }

    [Theory]
    [InlineData(5e-321, 0.05, 1e-100)]
    [InlineData(1e-310, 0.09, 1e-200)]
    [InlineData(5e-324, 0.09, 1e-50)]
    [InlineData(1e-300, 0.09, 1e-300)]
    public void A_tiny_first_shape_past_the_mode_is_one(double a, double b, double y)
    {
        // delta = log(b B) - b log y passes 745 there, and e^(-delta) times expm1(delta) was 0 times infinity: NaN,
        // which the clamp let through (#1690's review). I_x(a, b) is 1 less about a |log y|, which rounds away.
        Assert.Equal(1.0, Lodestar.Stats.Internal.Beta.RegularizedIncomplete(a, b, 1.0 - y, y));
    }

    [Theory]
    [InlineData(1e140, 1e-30, 5e-171, 1e-13)]
    [InlineData(1e160, 0.5, 1.6035048770711146e-241, 5e-13)]
    [InlineData(1e7, 1e-6, 4.999881406417969e-14, 1e-13)]
    [InlineData(3.0, 0.05, 0.007055910272617936, 1e-14)]
    [InlineData(1.36e154, 1.05, 3.587901e-317, 1e-6)]
    [InlineData(-1.36e154, 1.05, 3.587901e-317, 1e-6)]
    [InlineData(1.35e154, 1.02, 1.478809363563e-312, 1e-9)]
    [InlineData(2e-162, 5e-324, 8.261710209053629e-163, 1e-15)]
    public void A_Student_density_past_an_overflowing_square_is_the_density(double t, double df, double expected, double relative)
    {
        // A 400-digit evaluation. t^2/df overflowed and the density read 0, as scipy's t.pdf does: decision 0007. Or t^2
        // alone, three, whose 106-bit square read (∞, −∞), and a subnormal t^2, which kept a few bits (#1746's Review A).
        Assert.Equal(expected, Distributions.StudentPdf(t, df), relative * expected);
    }

    [Theory]
    [InlineData(2000.0)]
    [InlineData(1e6)]
    [InlineData(1e12)]
    public void A_large_shape_Student_tail_stays_under_one_half(double df)
    {
        // The large-shape expansion's 1 - direct sits outside the clamp ByFraction had alone (#1690's review).
        for (double t = 1e-300; t < 1.0; t *= 1e10)
        {
            Assert.True(Distributions.StudentSf(t, df) <= 0.5, $"t = {t}");
            Assert.True(Distributions.StudentSf(-t, df) >= 0.5, $"t = -{t}");
        }
    }

    [Theory]
    [InlineData(1e-300, 1e-300, 6.901983122333122e-298, 1.0, 1.0)]
    [InlineData(1e-300, 0.5, 5.597735947761608e-301, 1.0, 1.2130613194252669e-300)]
    [InlineData(1e-20, 1.0, 2.1938393439552025e-21, 1.0, 3.678794411714423e-21)]
    [InlineData(1e-08, 1e-10, 2.2448632753657656e-07, 0.9999997755136725, 99.99997754136724)]
    [InlineData(0.001, 1.1, 0.00018619450885552083, 0.9998138054911445, 0.0003028134087025569)]
    [InlineData(0.049, 0.5, 0.02809612816787003, 0.97190387183213, 0.05898939770271939)]
    [InlineData(0.099, 2.0, 0.005609141080636667, 0.9943908589193633, 0.007538643131636822)]
    [InlineData(0.001, 50.0, 3.800356221932517e-27, 1.0, 3.8748526761816144e-27)]
    [InlineData(0.051, 1e-300, 0.9999999999999994, 5.150839529164645e-16, 2.6269281598739684e+283)]
    public void A_chi_squared_shape_below_one_tenth_holds_a_400_digit_series(double a, double x, double upper, double lower, double gammaDensity)
    {
        // Q(a, x), P and the gamma density by a 400-digit series, as chi-squared at df 2a and 2x. Q was 1 - P and the
        // prefactor took log Gamma(a): ChiSquaredSf(1, 2e-300) read -2e-14 for 6e-301 (#1690's review).
        Assert.Equal(upper, Distributions.ChiSquaredSf(2.0 * x, 2.0 * a), 1e-14 * upper);
        Assert.Equal(lower, Distributions.ChiSquaredCdf(2.0 * x, 2.0 * a), 1e-14 * lower);
        Assert.Equal(gammaDensity / 2.0, Distributions.ChiSquaredPdf(2.0 * x, 2.0 * a), 1e-14 * gammaDensity);
    }

    [Theory]
    [InlineData(0.7, 30.0, 1e-8)]
    [InlineData(0.99, 10000.0, 1e-8)]
    [InlineData(0.01, 1e-8, 0.19)]
    [InlineData(0.3, 1e-8, 0.19)]
    [InlineData(0.01, 1e-8, 0.1)]
    [InlineData(0.021647385556131934, 2.5440350370286e-06, 123.8433417464437)]
    [InlineData(0.6549709050497334, 736.2281227264558, 2.3549411408444595e-06)]
    public void An_F_inverse_tail_at_a_tiny_shape_is_a_root_or_an_end(double p, double numerator, double denominator)
    {
        // A step under tolerance that rounded onto the bracket was taken for a root, and near x = 1 the slope makes any
        // step that small: Isf(0.7, 30, 1e-8) read 1.2e307, whose tail is 0.9999964 (#1690's review).
        double f = Distributions.FisherSnedecorIsf(p, numerator, denominator);
        if (double.IsPositiveInfinity(f))
        {
            Assert.True(Distributions.FisherSnedecorSf(double.MaxValue, numerator, denominator) >= p);
        }
        else if (f == 0.0)
        {
            Assert.True(Distributions.FisherSnedecorSf(double.Epsilon, numerator, denominator) <= p);
        }
        else
        {
            Assert.Equal(p, Distributions.FisherSnedecorSf(f, numerator, denominator), 1e-12);
        }
    }

    [Theory]
    [InlineData(1e-6, 1e-10)]
    [InlineData(0.3, 1e-6)]
    public void A_chi_squared_inverse_tail_past_the_smallest_double_is_zero(double p, double df)
    {
        // The tail at twice the smallest double, Q(a, epsilon), is already below p, so the root underflows; the upper
        // solve walked down by eighths until its iterations ran out, at 2e-258 for (1e-6, 1e-10) (#1690's review).
        Assert.True(Distributions.ChiSquaredSf(2.0 * double.Epsilon, df) < p);
        Assert.Equal(0.0, Distributions.ChiSquaredIsf(p, df));
        Assert.Equal(0.0, Distributions.ChiSquaredQuantile(1.0 - p, df));
    }

    [Theory]
    [InlineData(1e-320, 1e-8, 1.0, 3.7831639237564217e-06, 0.9999962168360762, 1e-14)]
    [InlineData(1e-320, 0.5, 2.0, 1.0, 7.071048131556767e-81, 1e-13)]
    [InlineData(1e300, 3.0, 1e-8, 0.9999964516007191, 3.548399280885026e-06, 1e-14)]
    [InlineData(5e-324, 2.0, 0.3, 1.0, 5e-324, 1e-14)]
    [InlineData(3.726465e-318, 0.004192269944532122, 36.34816911177705, 0.7864445782406513, 0.2135554217593487, 1e-14)]
    [InlineData(1e-310, 0.5, 3.0, 1.0, 2.3117518927483304e-78, 1e-13)]
    [InlineData(1e300, 7.0, 0.05, 2.9135022061959573e-08, 0.9999999708649779, 1e-14)]
    public void An_F_tail_past_an_underflowing_split_holds_a_400_digit_series(
        double f, double numerator, double denominator, double upper, double lower, double relative)
    {
        // One half of FisherSplit underflowed to zero and read as the support's end: Sf(1e-320, 1e-8, 1) was 1 (#1690's
        // review). 7e-81 is e^-184, whose exponent's rounding costs about 184 ulps.
        Assert.Equal(upper, Distributions.FisherSnedecorSf(f, numerator, denominator), relative * upper);
        Assert.Equal(lower, Distributions.FisherSnedecorCdf(f, numerator, denominator), relative * lower);
    }

    [Theory]
    [InlineData(1e-320, 0.5, 2.0, 1.767781713280062e+239)]
    [InlineData(1e300, 3.0, 1e-8, 4.999982258003597e-309)]
    public void An_F_density_past_an_underflowing_split_holds_a_400_digit_series(double f, double numerator, double denominator, double expected)
    {
        // The density's y^a read zero there; the second value is subnormal, so held to its own few ulps (#1690's review).
        Assert.Equal(expected, Distributions.FisherSnedecorPdf(f, numerator, denominator), 1e-12 * expected);
    }

    [Fact]
    public void Chi_squared_and_F_take_a_df_of_the_smallest_double_at_its_exact_half()
    {
        // Half of 5e-324 rounds to zero, which the shapes refused (#1690's review); lifted, it is half of 1e-323's law,
        // the densities of degree one in so small a shape. The positivity checks still come first, in main's order.
        double chi = Distributions.ChiSquaredPdf(1e-300, 1e-323) / 2.0;
        Assert.Equal(chi, Distributions.ChiSquaredPdf(1e-300, 5e-324), 1e-15 * chi);
        double fisher = Distributions.FisherSnedecorPdf(1e-300, 1e-323, 1.0) / 2.0;
        Assert.Equal(fisher, Distributions.FisherSnedecorPdf(1e-300, 5e-324, 1.0), 1e-15 * fisher);
        // The small tail of degree one in the lifted shape, 1.8428648589878496e-321 to 40 digits, on the side that shape names.
        Assert.Equal(1.843e-321, Distributions.FisherSnedecorCdf(1.0, 1.0, 5e-324));
        Assert.Equal(1.0, Distributions.FisherSnedecorSf(1.0, 1.0, 5e-324));
        Assert.Equal(1.843e-321, Distributions.FisherSnedecorSf(1.0, 5e-324, 1.0));
        Assert.Equal(1.0, Distributions.FisherSnedecorCdf(1.0, 5e-324, 1.0));
        // The chi-squared upper tail is a E1(x/2) at a = 2.47e-324: 1.705e-321 at 1e-300, and below half a granule at one.
        Assert.Equal(1.705e-321, Distributions.ChiSquaredSf(1e-300, 5e-324));
        Assert.Equal(1.84e-321, Distributions.ChiSquaredSf(5e-324, 5e-324));
        // At 1e-323, an even multiple whose half is a double, the lift still applies, and the tail is twice as large.
        Assert.Equal(3.414e-321, Distributions.ChiSquaredSf(1e-300, 1e-323));
        Assert.Equal(5e-324, Distributions.ChiSquaredSf(1.0, 1e-323));
        Assert.Equal(0.0, Distributions.ChiSquaredSf(1.0, 5e-324));
        Assert.Equal(1.0, Distributions.ChiSquaredCdf(1.0, 5e-324));
        // Every x above zero has a lower tail of one to rounding: the median is the support's end, from either side.
        Assert.Equal(0.0, Distributions.ChiSquaredQuantile(0.5, 5e-324));
        Assert.Equal(0.0, Distributions.ChiSquaredIsf(0.5, 5e-324));
        Assert.Equal("denominatorDf", Assert.Throws<ArgumentOutOfRangeException>(() => Distributions.FisherSnedecorQuantile(0.5, 5e-324, -1.0)).ParamName);
    }

    [Fact]
    public void An_F_tail_at_a_df_near_the_largest_double_keeps_its_correction()
    {
        // scipy 1.18.1, and main: the closed form for an underflowing half dropped (1 - y)^other, about e^-(other y), which
        // is not one when the other df is 1e308: Sf read 0.202 and the density 0.399. scipy's f.pdf is NaN there: 0007.
        Assert.Equal(0.31731050786291415, Distributions.FisherSnedecorSf(1.0, 1.0, 1e308), 1e-14);
        Assert.Equal(0.6826894921370862, Distributions.FisherSnedecorCdf(1.0, 1.0, 1e308), 1e-14);
        Assert.Equal(0.6826894921370862, Distributions.FisherSnedecorSf(1.0, 1e308, 1.0), 1e-14);
        Assert.Equal(0.24197072451913446, Distributions.FisherSnedecorPdf(1.0, 1.0, 1e308), 1e-14);
    }

    [Theory]
    [InlineData(1e200)]
    [InlineData(1e300)]
    [InlineData(1.7e308)]
    public void An_F_inverse_tail_at_a_huge_denominator_df_is_the_chi_squared_one(double denominator)
    {
        // scipy 1.18.1's f.isf(0.3173, 1, 1e200). Three log-gammas cancelled in the seed, which then sat on the bracket's
        // end, and the fallback walked down by eighths: +inf on main, 2675 before the fallback moved the exponent.
        Assert.Equal(1.0000434271174663, Distributions.FisherSnedecorIsf(0.3173, 1.0, denominator), 1e-14);
        Assert.Equal(1.0000434271174663, Distributions.FisherSnedecorQuantile(0.6827, 1.0, denominator), 1e-14);
    }

    [Theory]
    [InlineData(20.0, 4e6, 5.576151949164811e-88)]
    [InlineData(30.0, 9e6, 1.507101347377141e-196)]
    public void A_Student_density_at_a_large_df_keeps_its_digits(double t, double df, double expected)
    {
        // 70-digit log-gamma sums, scipy 1.18.1's t.pdf 2e-14 below the second. A series and log(1 + r^2), shared with the
        // inversion for a while, lost 5e-11 here.
        Assert.Equal(expected, Distributions.StudentPdf(t, df));
    }

    [Fact]
    public void An_F_lower_quantile_judges_its_end_on_the_lower_tail()
    {
        // Past the smallest f the root is zero, judged against the lower tail p keeps whole rather than 1 - p, where the
        // check read a rounded one (#1690's review).
        double atEnd = Distributions.FisherSnedecorCdf(double.Epsilon, 0.1, 5.0);
        Assert.Equal(0.0, Distributions.FisherSnedecorQuantile(atEnd / 2.0, 0.1, 5.0));
        Assert.True(Distributions.FisherSnedecorQuantile(atEnd * 4.0, 0.1, 5.0) > 0.0);
    }

    [Fact]
    public void Equal_tiny_F_shapes_keep_their_symmetry()
    {
        // log(x + s) - log x cancelled at x and s near 1e-300, and 0 times an overflowing slope sent the inversion to the
        // smallest double: Sf read 0.5000000000000293 and the median 0 for 1 (#1690's review).
        Assert.Equal(0.5, Distributions.FisherSnedecorSf(1.0, 1e-300, 1e-300));
        Assert.True(Distributions.FisherSnedecorCdf(double.Epsilon, 1e-310, 1e-310) <= 0.5);
        Assert.Equal(1.0, Distributions.FisherSnedecorQuantile(0.5, 1e-310, 1e-310), 1e-12);
        Assert.Equal(1.0, Distributions.FisherSnedecorIsf(0.5, 1e-310, 1e-310), 1e-12);
    }

    [Fact]
    public void An_F_density_at_a_subnormal_shape_holds_a_400_digit_series()
    {
        // log Gamma's reflection overflowed below 1.75e-308, and log B with it (#1690's review). The origin at a numerator
        // df of 2 is 1 for every denominator df, which log B had left 5e-14 off.
        Assert.Equal(4.999999999749985e-11, Distributions.FisherSnedecorPdf(1e-300, 3.0, 1e-310), 1e-12 * 5e-11);
        Assert.Equal(1.0, Distributions.FisherSnedecorPdf(0.0, 2.0, 1e-310));
        Assert.Equal(2.5e-311, Distributions.FisherSnedecorPdf(1.0, 1e-310, 1e-310), 1e-9 * 2.5e-311);
        Assert.Equal(4.2493681901736886e-308, Distributions.FisherSnedecorSf(1e308, 1e-308, 0.2), 1e-12 * 4.25e-308);
    }

    [Fact]
    public void An_F_tail_whose_half_underflows_beside_a_huge_df_is_the_chi_squared_limit()
    {
        // A 400-digit series. The half underflows while the other df times it is not small, and the first form, dropping
        // terms of that order, was left to the incomplete beta, which read the zero as the support's end (#1690's review).
        Assert.Equal(7.978845608028653e-09, Distributions.FisherSnedecorCdf(1e-16, 1.0, 1e308), 1e-14 * 7.98e-9);
        Assert.Equal(39894228.04014327, Distributions.FisherSnedecorPdf(1e-16, 1.0, 1e308), 1e-14 * 3.99e7);
        Assert.Equal(7.97884560802864e-08, Distributions.FisherSnedecorCdf(1e-14, 1.0, 1e308), 1e-14 * 7.98e-8);
        Assert.Equal(7.978845608027324e-07, Distributions.FisherSnedecorCdf(1e-12, 1.0, 1e307), 1e-14 * 7.98e-7);
        Assert.Equal(1e-16, Distributions.FisherSnedecorQuantile(7.978845608028653e-09, 1.0, 1e308), 1e-13 * 1e-16);
    }

    [Theory]
    [InlineData(1.0, 1e-308)]
    [InlineData(1.0, 1e-310)]
    [InlineData(1.0, 1e-323)]
    [InlineData(1e-300, 1e-308)]
    public void A_chi_squared_lower_tail_at_a_subnormal_shape_is_one(double x, double df)
    {
        // P's series began at 1/a, which overflowed below a = 5.6e-309: ChiSquaredCdf read +inf (#1690's review). The
        // upper tail is about a |log x|, below 1e-305, so the lower one is one to rounding.
        Assert.Equal(1.0, Distributions.ChiSquaredCdf(x, df));
    }

    [Fact]
    public void An_F_tail_on_a_subnormal_half_reads_its_chi_squared_limit_of_one()
    {
        // The half's chi-squared limit runs P's series at a = 5e-311, which began at 1/a and overflowed (#1690's review).
        Assert.Equal(1.0, Distributions.FisherSnedecorCdf(1e300, 1e-310, 1e308));
        Assert.Equal(1.0, Distributions.FisherSnedecorSf(1e-300, 1e308, 1e-310));
    }

    [Fact]
    public void A_zero_standard_error_spans_the_estimate_even_past_every_double()
    {
        // Student's quantile at df 0.001 is past the largest double, and infinity times zero was NaN (#1690's review).
        var result = new TTestResult(1.0, 0.5, 0.001) { Estimate = 2.0 };
        Assert.Equal((2.0, 2.0), result.ConfidenceInterval());
    }

    [Theory]
    [InlineData(5e-324)]
    [InlineData(1e-310)]
    [InlineData(1e-20)]
    public void A_subnormal_df_has_a_Student_density(double df)
    {
        // df/2 rounds to zero at 5e-324, and the density threw from a private parameter (#1690's review).
        double density = Distributions.StudentPdf(0.0, df);
        Assert.True(density > 0.0 && density < 1e-5);
        // Just below half the smallest double, by 7.6e-58 of it: 0 is the one double within half a granule.
        Assert.Equal(0.0, Distributions.StudentPdf(1.0, 5e-324));
    }

    [Fact]
    public void From_df_1e20_the_Student_density_is_the_normal_one()
    {
        // The tail took the normal law from 1e20 and the density did not: 3.7e-14 apart at df 1e308 (#1690's review).
        Assert.Equal(Distributions.NormalPdf(1.0), Distributions.StudentPdf(1.0, 1e308));
        Assert.Equal(Distributions.NormalPdf(1.0), Distributions.StudentPdf(1.0, 1e20));
    }

    [Theory]
    [InlineData(3.0)]
    [InlineData(1e30)]
    [InlineData(double.PositiveInfinity)]
    public void The_Student_median_is_positive_zero(double df)
    {
        // scipy's t.ppf(0.5, df) and NormalQuantile(0.5) are +0; the quantile's negation made it -0 (#1690's review).
        Assert.Equal(double.PositiveInfinity, 1.0 / Distributions.StudentQuantile(0.5, df));
        Assert.Equal(double.PositiveInfinity, 1.0 / Distributions.StudentIsf(0.5, df));
    }

    [Theory]
    [InlineData(1e308, 0.1)]
    [InlineData(1e308, 0.01)]
    [InlineData(1.5e308, 0.5)]
    public void A_root_in_the_top_binade_comes_back_finite(double root, double df)
    {
        // The density's r = x / sqrt(df) overflowed for df < 1 and the bisection's midpoint with it (#1690's review).
        double p = Distributions.StudentSf(root, df);
        Assert.Equal(root, Distributions.StudentIsf(p, df), 1e-12 * root);
    }

    [Theory]
    [InlineData(0.4, 1e-19)]
    [InlineData(0.4, 1e-10)]
    [InlineData(1e-300, 1e-10)]
    [InlineData(1e-3, 1e-3)]
    public void A_Student_quantile_past_the_largest_double_is_infinite(double p, double df)
    {
        // The tail at double.MaxValue still exceeds p, so the root lies beyond it: before #1690's review Newton settled
        // on one finite value for every p at a small df. scipy answers finite values here, roots of no tail: decision 0007.
        Assert.True(Distributions.StudentSf(double.MaxValue, df) > p);
        Assert.Equal(double.PositiveInfinity, Distributions.StudentIsf(p, df));
        Assert.Equal(double.NegativeInfinity, Distributions.StudentQuantile(p, df));
    }

    [Theory]
    [InlineData(0.025, 1e16, 1.9599639845400545)]
    [InlineData(1e-12, 1e16, 7.03448382530114)]
    [InlineData(1e-12, 1e17, 7.034483825301132)]
    [InlineData(1e-12, 1e20, 7.034483825301133)]
    [InlineData(0.4, 1e300, 0.2533471031357998)]
    public void Student_quantiles_at_a_huge_df_match_scipy(double p, double df, double isf)
    {
        // scipy 1.18.1's t.isf. Before #1690 the density's constant, a difference of two log-gammas, lost its
        // digits here, and Newton's last step carried the error: 7e-9 relative at (0.025, 1e16).
        Assert.Equal(isf, Distributions.StudentIsf(p, df), 1e-14 * isf);
        Assert.Equal(-isf, Distributions.StudentQuantile(p, df), 1e-14 * isf);
    }

    [Theory]
    [InlineData(1e7, 1e10, 0.0009993167532229563, 0.15865509257696825, false)]
    [InlineData(1e7, 1e10, 0.001000579769424803, 2.904256303818219e-07, false)]
    [InlineData(1e7, 1e10, 0.0010068948495245422, 1.5676759713545634e-137, false)]
    [InlineData(1e10, 1e7, 0.9990000517391309, 0.0013536346733890132, true)]
    [InlineData(1e10, 1e7, 0.9989978414605503, 8.463138029587699e-24, true)]
    [InlineData(1e6, 1e7, 0.09099576932658238, 0.15865522345530073, false)]
    [InlineData(1e5, 1e5, 0.4888196601123127, 7.525193473332948e-24, true)]
    public void The_incomplete_beta_at_large_shapes_holds_a_sixty_digit_fraction(
        double a, double b, double x, double smallerTail, bool lowerIsSmaller)
    {
        // The continued fraction in 60-digit Decimal; x is on a 2^-40 grid, so 1 - x is exact. The fraction in doubles
        // drifted to 6e-11 at shapes (1e7, 1e10), where Temme's uniform expansion holds 2e-13 (#1690's review).

        // The upper tail is I_(1-x)(b, a), read with the shapes and points exchanged.
        (double first, double second, double point) = lowerIsSmaller ? (a, b, x) : (b, a, 1.0 - x);
        double got = Lodestar.Stats.Internal.Beta.RegularizedIncomplete(first, second, point, 1.0 - point);
        Assert.Equal(smallerTail, got, 1e-12 * smallerTail);
    }

    [Theory]
    [InlineData(1e6)]
    [InlineData(1e18)]
    [InlineData(1e155)]
    [InlineData(1e300)]
    public void An_F_with_equal_huge_dfs_has_its_median_at_one(double df)
    {
        // F and 1/F share a law when the dfs are equal. Past a shape of 1e10 the fraction stopped short of the terms it
        // needs and read 0.70 at 1e18, and 1e155 read NaN (#1690's review).
        Assert.Equal(0.5, Distributions.FisherSnedecorSf(1.0, df, df));
        Assert.Equal(1.0, Distributions.FisherSnedecorQuantile(0.5, df, df), 4e-16);
    }

    [Theory]
    [InlineData(0.3, 2e15)]
    [InlineData(1e-3, 1e15)]
    [InlineData(0.3, 1e18)]
    public void An_F_quantile_at_equal_huge_dfs_sits_on_the_normal_limit(double p, double df)
    {
        // log F is symmetric with variance 4/df + O(1/df^2). Newton's density, a log x against a log B of 1.4e15, lost 5
        // nats, and the median at 2e15 read 0.99999938, 14 standard deviations off (#1690's review).
        double expected = Math.Exp(2.0 * Distributions.NormalQuantile(p) / Math.Sqrt(df));
        double quantile = Distributions.FisherSnedecorQuantile(p, df, df);
        Assert.Equal(expected - 1.0, quantile - 1.0, 1e-6 * Math.Abs(expected - 1.0));
    }

    [Theory]
    [InlineData(0.3, 2e15)]
    [InlineData(1e-30, 1e15)]
    [InlineData(0.3, 1e26)]
    [InlineData(1e-3, 1e28)]
    [InlineData(1e-3, 1e30)]
    public void A_chi_squared_quantile_at_a_huge_df_sits_on_the_normal_limit(double p, double df)
    {
        // k + z sqrt(2k) to O(1). Newton's slope cancelled log Gamma(a) against a log x: (0.3, 2e15) was 1.2e-10 off on main.
        // With that slope exact and a step tolerance of 1e-13, (1e-3, 1e30) read 5.9 deviations for 3.1 (#1690's review).
        double z = Distributions.NormalQuantile(p);
        double sd = Math.Sqrt(2.0 * df);
        double quantile = Distributions.ChiSquaredQuantile(p, df);
        double ulp = Math.BitIncrement(df) - df;
        Assert.Equal(z, (quantile - df) / sd, (1e-6 * Math.Abs(z)) + (2.0 * ulp / sd) + (4.0 * z * z / sd));
    }

    [Fact]
    public void The_large_shape_expansion_keeps_a_tiny_second_shape_s_tail()
    {
        // I_x(a, b) = -b (log y + psi(a) + gamma) to O(b) and O(a y), in 50-digit Decimal. P(b, a y) was 1 - 9e-50 here, and
        // the expansion's 1 - P read 0 (#1690's review).
        const double a = 4.0767483345523385e106;
        const double b = 4.228949954003074e-52;
        const double y = 1e-200;
        Assert.Equal(9.069426513367587e-50, Lodestar.Stats.Internal.Beta.RegularizedIncomplete(a, b, 1.0 - y, y), 1e-14 * 9.07e-50);
    }

    [Fact]
    public void Quantiles_at_a_tiny_df_hold_the_chi_squared_limit()
    {
        // Q(s, z) = 1 - z^s / Gamma(1 + s), so z = exp(-p/s - gamma) in Decimal, and F's lower tail is Q at z = d2/2f. F's
        // limit underflowed to f = 0, and the gamma residual, logs near -469 apart, rounded to zero (#1690's review).

        // s = d2/2, the chi-squared variable 2z, and the F root d2/2z.
        const double dfn = 9.98383125204385e188;
        const double dfd = 4.564566025500491e-207;
        const double p = 1.3162053657560206e-204;
        Assert.Equal(3.8930866907117e-251, Distributions.ChiSquaredIsf(p, dfd), 1e-13 * 3.9e-251);
        // The smallest double whose tail reaches p, 0.4 ulp above the limit's root 1.17247993382496294e44: on log(s B) in
        // double below a shape of 1e-13, 94 ulps above it (#1745's Review A).
        Assert.Equal(1.172479933824963e44, Distributions.FisherSnedecorQuantile(p, dfn, dfd));
    }

    [Fact]
    public void An_F_tail_at_huge_dfs_falls_as_f_rises()
    {
        // The sum a + b rounds by up to 4e16 here, and that rounding times 1 - p - q left the exponent positive near the
        // mean: the tail read 0 at f = 1 + 2^-52 and 0.25 six ulps later (#1690's review).
        const double dfn = 1.0283281608782226e30;
        const double dfd = 6.620009498500643e32;
        double f = 1.0;
        double previous = 1.0;
        for (int i = 0; i < 24; i++)
        {
            double sf = Distributions.FisherSnedecorSf(f, dfn, dfd);
            Assert.True(sf <= previous, $"Sf rose at f = {f:R}");
            Assert.Equal(1.0, sf + Distributions.FisherSnedecorCdf(f, dfn, dfd), 1e-15);
            previous = sf;
            f = Math.BitIncrement(f);
        }
    }

    [Theory]
    [InlineData(1.1277557074706777e-143, 6.038970661339348e183, 1.4321314062996377e181)]
    [InlineData(0.35348411060566276, 1.7511286951227086e105, 1.2038327676983933e105)]
    [InlineData(0.5681692271345151, 3.5631496432313466e108, 9.51108179576824e106)]
    public void An_F_quantile_narrower_than_an_ulp_brackets_its_probability(double p, double dfn, double dfd)
    {
        // log F is narrower than an ulp, so its tail steps from 0 to 1 within a few. One minus the beta's root lost 114 ulps
        // of f on a side of 0.002, and the tail at the quantile read 1 for 1e-143 (#1690's review).
        double quantile = Distributions.FisherSnedecorQuantile(p, dfn, dfd);
        double below = quantile;
        double above = quantile;
        for (int i = 0; i < 8; i++)
        {
            below = Math.BitDecrement(below);
            above = Math.BitIncrement(above);
        }
        Assert.InRange(p, Distributions.FisherSnedecorCdf(below, dfn, dfd), Distributions.FisherSnedecorCdf(above, dfn, dfd));
    }

    [Fact]
    public void A_chi_squared_inverse_tail_at_a_tiny_df_grows_to_its_root()
    {
        // Q(a, z) = a E1(z) to O(a), a = df/2, solved for z in 60-digit Decimal. From the seed a = 2.6e-228, growth by four
        // per step stopped at 1.35e-107 after all 200 iterations, a tail of 6e-226 for 1.6e-284 (#1690's review).
        Assert.Equal(249.2172786318603, Distributions.ChiSquaredIsf(1.5953428163057874e-284, 5.24470273781178e-228), 1e-12 * 249.2);
    }

    [Theory]
    [InlineData(1e-300)]
    [InlineData(1e-17)]
    [InlineData(1e-10)]
    public void A_tail_at_a_tiny_positive_argument_stays_at_or_below_one_half(double t)
    {
        // erfc's interpolant read 1 + 1.6e-15 near zero, so NormalSf(1e-17) and StudentSf(1e-17, 1e21), the normal law
        // from df 1e20, read 0.5000000000000008, and Welch's p-value passed one (#1690's review). 0.5 - phi(0) t below.
        double expected = 0.5 - (t * 0.3989422804014327);
        Assert.Equal(expected, Distributions.NormalSf(t), 1e-16);
        Assert.Equal(expected, Distributions.StudentSf(t, 1e21), 1e-16);
        Assert.True(Distributions.StudentSf(t, 1e21) <= 0.5);
    }

    [Theory]
    [InlineData(1e-200, 1e180, 294.0, false)]
    [InlineData(1e-170, 1e160, 1.0, false)]
    [InlineData(1.479e176, 5781.0, 4.21e179, true)]
    public void An_F_tail_with_one_df_past_1e154_reads_zero_rather_than_nan(double f, double dfn, double dfd, bool upper)
    {
        // Each tail is under 1e-300 here: the other df's chi-squared would have to pass 1e170. The fraction's (a + m)(a + b + m)
        // overflowed into inf / inf and the tail read NaN, on main too (#1690's review).
        double tail = upper ? Distributions.FisherSnedecorSf(f, dfn, dfd) : Distributions.FisherSnedecorCdf(f, dfn, dfd);
        Assert.Equal(0.0, tail);
    }

    [Theory]
    [InlineData(0.95, 2e10, 2e10, true)]
    [InlineData(0.4, 2e12, 2e12, false)]
    [InlineData(0.05, 2e12, 2e12, false)]
    [InlineData(0.95, 1.99999e10, 2e10, true)]
    public void An_F_quantile_at_near_equal_huge_dfs_holds_its_tail(double p, double dfn, double dfd, bool lower)
    {
        // Solving the side whose mean is under one half, capped at one half, clamped every root past it: the quantile at
        // (0.95, 2e10, 2e10) read 1, whose tail is 0.5, for scipy's 1.0000232620136307 (#1690's review).
        double quantile = lower ? Distributions.FisherSnedecorQuantile(p, dfn, dfd) : Distributions.FisherSnedecorIsf(p, dfn, dfd);
        double tail = lower ? Distributions.FisherSnedecorCdf(quantile, dfn, dfd) : Distributions.FisherSnedecorSf(quantile, dfn, dfd);
        Assert.Equal(p, tail, 1e-6 * p);
    }

    [Fact]
    public void NormalSf_falls_across_its_series_switch()
    {
        // erfc switches from erf's series to its interpolant at 0.04, z = 0.04 sqrt 2; at 0.01 the interpolant sat 4 ulps
        // above the series and the tail rose there (#1690's review).
        double z = 0.04 * Math.Sqrt(2.0);
        for (int i = 0; i < 40; i++)
        {
            z = Math.BitDecrement(z);
        }
        double previous = Distributions.NormalSf(z);
        for (int i = 0; i < 80; i++)
        {
            z = Math.BitIncrement(z);
            double sf = Distributions.NormalSf(z);
            Assert.True(sf <= previous, $"NormalSf rose at z = {z:R}");
            previous = sf;
        }
    }

    [Theory]
    [InlineData(7.592162500738105e-132, 1.8773808009983036e306, 5.027725952387203e232, 1.0)]
    [InlineData(1.4845305106783903e282, 6.217165131235991e135, 2.1546100837393183e306, 0.0)]
    public void An_F_tail_far_from_a_huge_law_reads_its_limit(double f, double dfn, double dfd, double sf)
    {
        // The uniform expansion's 2 exponent / s overflowed past an exponent of 9e307, and the tail read NaN (#1690's review).
        Assert.Equal(sf, Distributions.FisherSnedecorSf(f, dfn, dfd));
        Assert.Equal(1.0 - sf, Distributions.FisherSnedecorCdf(f, dfn, dfd));
    }

    [Theory]
    [InlineData(0.14382288099444607, 4.700992327819855e118, 4.540995160565704e282)]
    [InlineData(0.9702011686610995, 2.9965212002435175e235, 3.0339052669960465e139)]
    [InlineData(8.246471346778289e-291, 8.9e29, 3.37e27)]
    public void An_F_quantile_at_a_narrow_law_sits_where_its_own_tail_crosses(double p, double dfn, double dfd)
    {
        // f = d2 y / (d1 (1 - y)) rounds three times, up to four ulps, which a law narrower than 1e-12 turns into its
        // tail: the first read 0.9999999999999998, whose lower tail is 0 for 0.144 (#1690's review).
        double quantile = Distributions.FisherSnedecorQuantile(p, dfn, dfd);
        Assert.True(Distributions.FisherSnedecorCdf(quantile, dfn, dfd) >= p * (1 - 1e-9), "the tail at the quantile is short of p");
        Assert.True(Distributions.FisherSnedecorCdf(Math.BitDecrement(quantile), dfn, dfd) <= p * (1 + 1e-9), "the tail an ulp below already reaches p");
    }

    [Theory]
    [InlineData(1e100, 1e60, 0.5)]
    [InlineData(1e60, 1e100, 0.5)]
    [InlineData(1e40, 1e20, 0.5000000000188063)]
    public void An_F_tail_at_one_with_dfs_past_1e30_holds_its_centre(double dfn, double dfd, double sf)
    {
        // F's split rounds x through the same sum as the mean; measured against the exact mean, that rounding sat thousands
        // of standard deviations off and the tail read 0 at (1e100, 1e60) (#1690's review). The last is chi2.cdf(1e20, 1e20).
        Assert.Equal(sf, Distributions.FisherSnedecorSf(1.0, dfn, dfd), 1e-12);
        double deviation = Math.Sqrt((2.0 / dfn) + (2.0 / dfd));
        double density = 1.0 / (deviation * Math.Sqrt(2.0 * Math.PI));
        Assert.Equal(density, Distributions.FisherSnedecorPdf(1.0, dfn, dfd), 1e-9 * density);
    }

    [Theory]
    [InlineData(1.466863520350374e-55, 3.3163652308324555e35, true)]
    [InlineData(4.2089e-20, 6.557955543789673e33, true)]
    [InlineData(1.4227e-158, 3.7505959269482657e36, false)]
    public void A_chi_squared_quantile_narrower_than_an_ulp_sits_at_its_crossing(double p, double df, bool upper)
    {
        // A step tolerance floored at two ulps accepted unevaluated steps four ulps past the crossing (#1690's review).
        double quantile = upper ? Distributions.ChiSquaredIsf(p, df) : Distributions.ChiSquaredQuantile(p, df);
        double below = Math.BitDecrement(quantile);
        if (upper)
        {
            Assert.True(Distributions.ChiSquaredSf(quantile, df) <= p * (1 + 1e-9) && Distributions.ChiSquaredSf(below, df) >= p * (1 - 1e-9));
        }
        else
        {
            Assert.True(Distributions.ChiSquaredCdf(quantile, df) >= p * (1 - 1e-9) && Distributions.ChiSquaredCdf(below, df) <= p * (1 + 1e-9));
        }
    }

    [Theory]
    [InlineData(1e308)]
    [InlineData(double.MaxValue)]
    public void An_F_law_at_dfs_whose_sum_overflows_is_still_a_step_at_one(double df)
    {
        // d1 + d2 overflowed in the deviation from the mean, which read 0, and every tail read one half (#1690's review).
        Assert.Equal(1.0, Distributions.FisherSnedecorSf(0.5, df, df));
        Assert.Equal(0.0, Distributions.FisherSnedecorSf(2.0, df, df));
        Assert.Equal(0.0, Distributions.FisherSnedecorCdf(0.5, df, df));
        Assert.Equal(1.0, Distributions.FisherSnedecorCdf(2.0, df, df));
        Assert.Equal(1.0, Distributions.FisherSnedecorQuantile(0.3, df, df), 4e-16);
    }

    [Theory]
    [InlineData(1.1543e-271, 1.44e24, 5.39e23)]
    [InlineData(2.04e-239, 1.9e17, 3.55e23)]
    [InlineData(2.0920843844945383e-55, 1515294954052.0522, 9.552327752561384e204)]
    public void An_F_inverse_tail_at_a_law_of_width_1e_9_sits_where_its_own_tail_crosses(double p, double dfn, double dfd)
    {
        // The walk to F's own crossing began at a log-F width of 1e-12, and the first read a tail 0.6% past p (#1690's review).
        double quantile = Distributions.FisherSnedecorIsf(p, dfn, dfd);
        Assert.True(Distributions.FisherSnedecorSf(quantile, dfn, dfd) <= p * (1 + 1e-9), "the tail at the quantile is past p");
        Assert.True(Distributions.FisherSnedecorSf(Math.BitDecrement(quantile), dfn, dfd) >= p * (1 - 1e-9), "an ulp below already holds p");
    }

    [Fact]
    public void The_uniform_expansion_answers_nan_for_a_nan_complement()
    {
        // Math.Sign threw on the NaN deviation a NaN y left, where the fraction answered NaN (#1690's review).
        Assert.True(double.IsNaN(Lodestar.Stats.Internal.Beta.RegularizedIncomplete(2e5, 2e5, 0.7, double.NaN)));
    }

    [Theory]
    [InlineData(1e60, 0.05)]
    [InlineData(1e80, 0.15)]
    public void A_Student_far_tail_below_df_0_2_round_trips_through_its_inverse(double t, double df)
    {
        // The far tail took log(a B) by the ten-step increment, the inversion's cache by three log-gammas, so the quantile
        // solved a tail ulps from the one StudentSf answers (#1690's review).
        double p = Distributions.StudentSf(t, df);
        Assert.Equal(p, Distributions.StudentSf(Distributions.StudentIsf(p, df), df), 1e-14 * p);
    }

    [Theory]
    [InlineData(0.3, 0.1, 1.0)]
    [InlineData(1e-3, 0.05, 2.0)]
    public void An_F_quantile_with_a_tiny_numerator_df_round_trips_through_its_tail(double p, double dfn, double dfd)
    {
        // ShapeLogs cached log(a B) in one form and the forward tail took another, ulps apart (#1690's review).
        Assert.Equal(p, Distributions.FisherSnedecorCdf(Distributions.FisherSnedecorQuantile(p, dfn, dfd), dfn, dfd), 1e-13 * p);
    }

    [Theory]
    [InlineData(1.1e-20)]
    [InlineData(1e-18)]
    [InlineData(1e-14)]
    public void Student_s_tail_at_a_tiny_df_falls_with_t(double df)
    {
        // The general path's last ulp sat on either side of one half: at df 1.1e-20 the tail read 0.49999999999999994 at
        // t = 2e-8 and 0.5 at 6e-8 (#1690's review). Below df 1e-12 it is 1/2 - (df/2) asinh(t / sqrt df).
        double previous = 0.5;
        for (double t = 1e-12; t < 1e300; t *= 1.37)
        {
            double sf = Distributions.StudentSf(t, df);
            Assert.True(sf <= previous, $"StudentSf rose at t = {t:R}");
            double z = t / Math.Sqrt(df);
            double asinh = z > 1e150 ? Math.Log(2.0) + Math.Log(t) - (0.5 * Math.Log(df)) : Math.Asinh(z);
            Assert.Equal(0.5 - (0.5 * df * asinh), sf, 1e-16);
            previous = sf;
        }
    }

    [Fact]
    public void An_exact_tail_rounding_to_one_keeps_its_low_part()
    {
        // Clamped on its rounding, I_x(df/2, 1/2) at df 1e-11 lost the low part read below one, and the quantile's Newton
        // steps took it for a double form's: StudentIsf(0.49999999999999994, 1e-11) searched 123 tails (#1745's Review A).
        const double df = 1e-11;
        const double t = 1e-11;
        var denominator = new Lodestar.Stats.Internal.DoubleDouble(df, 0.0) + Lodestar.Stats.Internal.DoubleDouble.Product(t, t);
        Lodestar.Stats.Internal.ExactTail tail = Lodestar.Stats.Internal.Beta.ExactRegularizedIncomplete(
            df / 2.0, 0.5, new Lodestar.Stats.Internal.DoubleDouble(df, 0.0) / denominator,
            Lodestar.Stats.Internal.DoubleDouble.Product(t, t) / denominator, Lodestar.Stats.Internal.Beta.ShapeLogs.None, double.NaN);
        Assert.Equal(1.0, tail.Rounded);
        Assert.True(tail.Value.Low < 0.0);
        Assert.False(tail.InDouble);
    }

    [Fact]
    public void A_first_order_Student_root_near_1e302_is_finite()
    {
        // sinh(w) overflowed before sqrt(df) scaled it, past w = 709.8, and the root read +inf (#1690's review). An ulp of p
        // near one half is 1e-4 of t here, so the root is judged by its tail.
        const double df = 1e-12;
        double p = Distributions.StudentSf(1.117e302, df);
        double root = Distributions.StudentIsf(p, df);
        Assert.True(double.IsFinite(root));
        Assert.Equal(p, Distributions.StudentSf(root, df), 1.2e-16);
    }

    [Theory]
    [InlineData(30.0, 4.906713927148187e-198)]
    [InlineData(37.0, 5.72557122252457682e-300)]
    public void The_normal_far_tail_holds_its_exponent(double z, double sf)
    {
        // Q(z) to 20 digits. Through z / sqrt 2 the exponent took that quotient's rounding twice, 1.1e-13 of Q(37), and
        // Student's tail jumped by that much crossing df 1e20, where it becomes this one (#1690's review).
        Assert.Equal(sf, Distributions.NormalSf(z), 2e-15 * sf);
        Assert.Equal(sf, Distributions.StudentSf(z, 1e20), 2e-15 * sf);
    }

    [Fact]
    public void Student_s_density_is_continuous_where_it_becomes_the_normal_one()
    {
        // Two half log h of 23 each cancelled in the constant, and the density stepped 1.4e-15 crossing df 1e20 (#1690's review).
        double normal = 1.0 / Math.Sqrt(2.0 * Math.PI);
        Assert.Equal(normal, Distributions.StudentPdf(0.0, 9.999999999999998e19), 2e-16);
        Assert.Equal(normal, Distributions.StudentPdf(0.0, 1e20), 2e-16);
    }

    [Theory]
    [InlineData(1e-300, 1e9)]
    [InlineData(1e-12, 1e300)]
    [InlineData(1e-323, 0.5)]
    [InlineData(1.7e308, 1.7e308)]
    public void The_F_tails_at_an_infinite_f_are_the_support_s_end_where_dfd_over_dfn_overflows(double dfn, double dfd)
    {
        // dfd / dfn / f was inf / inf and the tails threw (#1705), as scipy's f.sf and f.cdf do not.
        Assert.Equal(0.0, Distributions.FisherSnedecorSf(double.PositiveInfinity, dfn, dfd));
        Assert.Equal(1.0, Distributions.FisherSnedecorCdf(double.PositiveInfinity, dfn, dfd));
    }

    [Theory]
    [InlineData(5e-324, 1e-323, 0.5)]
    [InlineData(1e-310, 1e-310, 1e300)]
    public void An_F_inverse_tail_past_the_largest_double_is_infinite_at_a_subnormal_numerator_df(double p, double dfn, double dfd)
    {
        // The tail at the largest double still exceeds p, so the root lies past it; the check of the infinite answer threw (#1704).
        Assert.True(Distributions.FisherSnedecorSf(double.MaxValue, dfn, dfd) > p);
        Assert.Equal(double.PositiveInfinity, Distributions.FisherSnedecorIsf(p, dfn, dfd));
    }

    [Theory]
    [InlineData(1e-70, 10.0, 1.3020833333333333e-283)]
    [InlineData(1e-62, 10.0, 1.3020833333333335e-251)]
    [InlineData(1e-210, 3.0, 3.9894228040143267e-106)]
    [InlineData(1e-300, 2.0, 0.5)]
    [InlineData(5e-324, 1.0, 1.7948069285245253e161)]
    [InlineData(1e-20, 4.0, 2.4999999999999999e-21)]
    [InlineData(1e-19, 4.0, 2.4999999999999999e-20)]
    [InlineData(5e-324, 0.1, 6.862545259797682e305)]
    [InlineData(1.5e-323, 0.1, 2.4166849068059488e305)]
    public void The_chi_squared_density_holds_its_digits_at_a_tiny_x(double x, double df, double density)
    {
        // x^a underflowed before the division by x, 0 at 1e-70, and its exponent cost 2e-13 at 1e-62 (#1708); 50-digit sums.
        // Left: Gamma(a) through its log, 1.6e-15 at a shape of 5, and below a shape of 0.1 an exponent of -74, 1.2e-14.
        Assert.Equal(density, Distributions.ChiSquaredPdf(x, df), (df < 0.2 ? 1.5e-14 : 2e-15) * density);
    }

    [Theory]
    [InlineData(1e-300, 3.0, 1.0, 3.3079733725307523e-150)]
    [InlineData(1e-70, 10.0, 5.0, 1.876875e-277)]
    [InlineData(1e-300, 3.0, 20.0, 2.149585981371381e-150)]
    [InlineData(1e-300, 3.0, 0.19, 6.2993316913547444e-150)]
    [InlineData(1e-300, 3.0, 100.0, 2.0884669337873489e-150)]
    [InlineData(1e-33, 20.0, 20.0, 9.237800000000005e-292)]
    public void The_F_density_holds_its_digits_at_a_tiny_f(double f, double dfn, double dfd, double density)
    {
        // log B(10, 10), -14, takes the last of them: 5.4e-15 at shapes of 20.
        double tolerance = dfn >= 20.0 ? 6e-15 : 2e-15;
        // y^a underflowed before the division by f, and FisherSnedecorPdf(1e-300, 3, 1) read 0 (#1708), as did the last three
        // under a first fix; 80-digit sums, against which scipy's f.pdf is 4e-14 off there.
        Assert.Equal(density, Distributions.FisherSnedecorPdf(f, dfn, dfd), tolerance * density);
    }

    [Theory]
    [InlineData(1.0, 1.0, 1e14, 0.24197072451914214)]
    [InlineData(1.0, 19.9, 1e14, 1.247916480195731)]
    [InlineData(2.0, 3.0, 1e12, 0.14595651998903386)]
    [InlineData(0.5, 1.0, 1e7, 0.4393912702443541)]
    public void The_F_density_at_a_huge_denominator_df_keeps_one_minus_y_on_its_log1p_side(double f, double dfn, double dfd, double density)
    {
        // A first fix for #1708 took log(1 - y) after 1 - y had rounded, and read the first 4e-4 off; 90-digit sums, and what
        // main's own front holds there, 2.4e-14 at a first shape of 10 against a second of 5e13.
        Assert.Equal(density, Distributions.FisherSnedecorPdf(f, dfn, dfd), 3e-14 * density);
    }

    [Theory]
    [InlineData(1.9999, 300.0, 4.7935748211820564e-262)]
    [InlineData(2.0, 300.0, 4.8291797467471066e-262)]
    public void The_chi_squared_density_is_continuous_at_x_two_from_a_shape_of_ten(double x, double df, double density)
    {
        // A first fix for #1708 took a branch of its own below x = 2 at every shape; 90-digit sums, and the 2.2e-14 main's
        // own Stirling prefactor holds at 1.9999, an exponent of -600.
        Assert.Equal(density, Distributions.ChiSquaredPdf(x, df), 2.5e-14 * density);
    }

    [Theory]
    [InlineData(1e-33, 20.0, 10000.0, 2.78062961778363e-293, 1e-13)]
    [InlineData(1e-33, 21.0, 200.0, 2.387125026815725e-309, 1e-12)]
    [InlineData(1e-33, 21.0, 20.0, 6.93878708310438e-308, 1e-13)]
    [InlineData(1.0533, 2e6, 2e6, 4.887097733893733e-291, 1e-9)]
    [InlineData(1.00052, 2e10, 2e10, 1.0463893054737687e-289, 1e-9)]
    [InlineData(0.1, 1e14, 200.0, 5.43894222487651e-290, 1e-13)]
    [InlineData(0.1, 1e12, 200.0, 5.438946585827567e-290, 1e-13)]
    [InlineData(0.1, 1e10, 200.0, 5.439382698530586e-290, 2e-13)]
    [InlineData(0.1, 1e8, 200.0, 5.483170414762349e-290, 2e-13)]
    [InlineData(0.01445, 1e6, 21.0, 2.984536606183596e-290, 1e-13)]
    public void The_F_density_keeps_its_digits_where_its_power_or_its_front_leaves_the_normal_doubles(
        double f, double dfn, double dfd, double density, double tolerance)
    {
        // A subnormal power read the first 3.6e-3 off, and a fallback from fronts of 1e-290 the sixth 8e-5, in the Review A
        // of the fix for #1708. Against these 90-digit sums main's own front, kept where normal, holds 1.6e-13 at the ninth.
        Assert.Equal(density, Distributions.FisherSnedecorPdf(f, dfn, dfd), tolerance * density);
    }

    [Theory]
    [InlineData(1500.0, 19.0, 2.185326408261971e-307)]
    [InlineData(1490.0, 9.0, 1.36925296e-315)]
    [InlineData(1450.0, 10.0, 7.881545898871309e-306)]
    [InlineData(1e100, 10.0, 0.0)]
    [InlineData(1e200, 6.0, 0.0)]
    public void The_chi_squared_upper_tail_density_keeps_main_s_form(double x, double df, double density)
    {
        // A small-prefactor fallback read NaN at 1e100 and 0 at 1500 (#1708's Review A). scipy's chi2.pdf, the subnormal
        // to its granularity; at 1500 a 60-digit product, which scipy, through Lanczos' Γ(9.5), reads 1.4e-13 low.
        Assert.Equal(density, Distributions.ChiSquaredPdf(x, df), Math.Max(1e-13 * density, 1e-322));
    }

    [Fact]
    public void A_split_whose_f_over_dfd_overflows_keeps_its_odds()
    {
        // f/d2 is +inf here, and y fell back to the subnormal d1 f, 5.4e-5 off (#1727's Review B). The split from exact
        // rationals of the three doubles.
        (double lower, double upper) = Lodestar.Stats.Internal.Beta.FisherSplit(1234.567, 1e-323, 2.3e-308);
        Assert.Equal(5.303975149469276e-13, lower, 1e-15 * lower);
        Assert.Equal(0.9999999999994696, upper, 3.3e-16);
    }

    [Fact]
    public void A_huge_finite_f_splits_its_halves_where_dfd_over_dfn_overflows()
    {
        // dfd / dfn / f was inf / 1e308: the split read (0, NaN), where y = d1 f / (d1 f + d2) is a quarter.
        (double lower, double upper) = Lodestar.Stats.Internal.Beta.FisherSplit(1e308, 0.5, 1.5e308);
        Assert.Equal(0.25, lower, 1e-15);
        Assert.Equal(0.75, upper, 1e-15);
    }

    [Fact]
    public void The_chi_squared_density_at_a_subnormal_shape_divides_before_it_rounds()
    {
        // a e^(...) rounded to a subnormal before the division by x, 3.4e-9 off (#1708's Review A); e^(...) is one here.
        // a is the df's exact half, 1.000000000952012e-315 in 50-digit Decimal, which df / 2.0 rounds (#1729's Review A).
        Assert.Equal(1.000000000952012e-15, Distributions.ChiSquaredPdf(1e-300, 2.0000000000123e-315), 1e-15 * 1e-15);
    }

    [Theory]
    [InlineData(5e-324, 1.0, true, 1481.1266547553563)]
    [InlineData(5e-324, 800.0, true, 3447.2257792597159)]
    [InlineData(5e-324, 1e9, true, 1001721300.6458117)]
    [InlineData(5e-324, 800.0, false, 49.134364251159558)]
    [InlineData(1e-320, 5e4, false, 38853.679807961526)]
    [InlineData(5e-324, 1e10, false, 9994560873.1097090)]
    [InlineData(5e-324, 1e300, false, 1e300)]
    [InlineData(1e-310, 2e-5, true, 1391.4842226185543)]
    [InlineData(2.225073858507201e-308, 2.0, false, 4.450147717014402e-308)]
    [InlineData(1e-320, 4.39780802753384e-7, true, 1429.8470689058026)]
    public void A_chi_squared_quantile_at_a_subnormal_probability_keeps_its_digits(double p, double df, bool upper, double x)
    {
        // Judged on the subnormal tail itself, which kept a few bits, ChiSquaredIsf(5e-324, 1) read 1481.4857 (#1729's
        // Review A); against 50-digit roots of the tail's log.
        double root = upper ? Distributions.ChiSquaredIsf(p, df) : Distributions.ChiSquaredQuantile(p, df);
        Assert.Equal(x, root, 1e-15 * x);
    }

    [Fact]
    public void The_F_density_at_a_small_shape_keeps_its_digits_either_side_of_the_product_cut()
    {
        // Front's exponent of 45 nats read 6e-15 off at 0.002, where a df an ulp under took products (#1729's Review A).
        Assert.Equal(3.1289999644237285e-30, Distributions.FisherSnedecorPdf(1e10, 0.002, 5), 1e-15 * 3.1289999644237285e-30);
        Assert.Equal(3.1289999644237285e-30, Distributions.FisherSnedecorPdf(1e10, 2.0 * Math.BitDecrement(1e-3), 5), 1e-15 * 3.1289999644237285e-30);
    }

    [Fact]
    public void A_chi_squared_inverse_tail_near_one_at_a_subnormal_root_is_the_quantile_of_its_complement()
    {
        // Q near one held P to ε, and the root sat 5 granules from the lower quantile's (#1729's Review A).
        double p = Math.Pow(2.0, -45.0);
        Assert.Equal(Distributions.ChiSquaredQuantile(p, 0.0875), Distributions.ChiSquaredIsf(1.0 - p, 0.0875));
    }

    [Fact]
    public void A_chi_squared_lower_quantile_near_one_at_a_subnormal_root_is_the_inverse_tail_of_its_complement()
    {
        // P near one held Q to ε, and the root sat 11,541 granules from the inverse tail's (#1729's Review A).
        Assert.Equal(Distributions.ChiSquaredIsf(1.0 - 0.99645, 1e-5), Distributions.ChiSquaredQuantile(0.99645, 1e-5));
    }

    [Theory]
    [InlineData(1e300, 3.7, 1e300)]
    [InlineData(1e300, 3.0, 1e250)]
    public void An_F_tail_past_a_huge_denominator_df_is_zero_not_NaN(double f, double dfn, double dfd)
    {
        // x^a by Math.Pow underflowed beside an e^rest past the doubles, and 0 times +∞ read NaN (#1729's Review A).
        Assert.Equal(0.0, Distributions.FisherSnedecorSf(f, dfn, dfd));
        Assert.Equal(1.0, Distributions.FisherSnedecorCdf(f, dfn, dfd));
    }

    [Theory]
    [InlineData(2.5e-305, 2.0, 5.0, 2.5e-305)]
    [InlineData(1e-200, 2.0, 5.0, 1e-200)]
    public void An_F_lower_tail_far_below_one_keeps_its_digits(double f, double dfn, double dfd, double expected)
    {
        // e^(a log x) took some 700 nats' rounding and read 2.5e-305's tail 500 ulps off (#1729's Review A); the 60-digit
        // tail at dfs 2 and 5 is f itself to 3e-17.
        Assert.Equal(expected, Distributions.FisherSnedecorCdf(f, dfn, dfd), 2e-15 * expected);
    }

    [Theory]
    [InlineData(1e-308)]
    [InlineData(4.619638887548376e-309)]
    [InlineData(3e-320)]
    public void The_chi_squared_quantile_at_df_two_and_a_subnormal_root_is_twice_the_probability(double p)
    {
        // The midpoint's tail rounded onto its grid read a quarter-granule as the target, a granule short (#1729's Review A).
        Assert.Equal(2.0 * p, Distributions.ChiSquaredQuantile(p, 2.0));
    }

    [Fact]
    public void An_F_lower_tail_beside_a_second_df_near_the_smallest_normal_keeps_its_bits()
    {
        // b times the front, subnormal there, dropped bits the division by a brought back; against the 80-digit tail.
        Assert.Equal(1.6364237716358404e-308, Distributions.FisherSnedecorCdf(5e-324, 0.15, 4.450147717014403e-308), 2e-15 * 1.64e-308);
    }

    [Fact]
    public void A_chi_squared_lower_tail_with_a_subnormal_power_rounds_once()
    {
        // x^a rounded onto the subnormals and the product with 1/Γ again, a granule off; against the 80-digit tail.
        Assert.Equal(7.52252778063675e-310, Distributions.ChiSquaredCdf(2e-206, 3.0));
    }

    [Theory]
    [InlineData(0.5)]
    [InlineData(3.0)]
    [InlineData(9.5)]
    public void The_gamma_prefactor_at_zero_is_zero(double a)
    {
        // Past LongPower the power went to PowerOfTwo.Split, which reads zero as NaN (#1729's Review A).
        Assert.Equal(0.0, Lodestar.Stats.Internal.Gamma.Prefactor(a, 0.0));
    }

    [Fact]
    public void Tails_below_2_to_the_minus_969_round_once_and_their_quantiles_reach_the_target()
    {
        // Their double-doubles' low parts went subnormal, and the tails and quantiles stopped an ulp short; the values are
        // the closed forms' to 1,300 digits (#1745's Review A).
        Assert.Equal(1.4483392809312908e-307, Distributions.FisherSnedecorCdf(2.2743439249683246e-154, 4, 5));
        Assert.Equal(3.3011239034249844e-307, Distributions.FisherSnedecorSf(3.140388479691043e+204, 2, 3));
        Assert.Equal(2.6058053776716e-309, Distributions.StudentSf(1.2215412897344406e+308, 1));
        Assert.Equal(3.550070150994942e-154, Distributions.FisherSnedecorQuantile(3.528839461555869e-307, 4, 5));
        Assert.Equal(3.9863179480871846e+153, Distributions.StudentIsf(3.1464883951547906e-308, 2));
    }

    [Theory]
    [InlineData(9.169652348961944e-16, 3.614251328859572e35, 1.335281399455076e36, true)]
    [InlineData(0.5000001175890465, 1.333081543353448e35, 1.4119150561657234e61, false)]
    [InlineData(1.1400231467844526e-142, 2.891206902605065e269, 1.834510027769961e33, true)]
    [InlineData(0.49999999999977596, 7.486478238040092e34, 1.1652515917252837e35, true)]
    [InlineData(0.7031317514847646, 1.102748314407507e35, 1.231131747554599e201, false)]
    [InlineData(0.25, 4.57928097506806e34, 6.9159370405322e63, true)]
    public void An_f_quantile_whose_crossing_is_beside_an_unsearched_root_takes_it(double p, double dfn, double dfd, bool lower)
    {
        // A law narrower than an ulp, its first Newton step far off: the root was answered an ulp short (#1745's Review A).
        double f = lower ? Distributions.FisherSnedecorQuantile(p, dfn, dfd) : Distributions.FisherSnedecorIsf(p, dfn, dfd);
        Func<double, bool> reaches = x => lower
            ? ExactQuantileContract.FisherLowerReaches(x, dfn, dfd, p)
            : ExactQuantileContract.FisherUpperReaches(x, dfn, dfd, p);
        Assert.True(reaches(f), $"root {f}");
        Assert.False(reaches(Math.BitDecrement(f)), $"root {f}");
    }

    [Fact]
    public void An_f_inverse_tail_on_a_first_order_plateau_above_its_rounding_is_the_largest_double()
    {
        // a/(a + b) lies 8e-18 above 0.9999999999 at every f; rounded, the plateau read as reaching it and answered ε
        // (#1745's Review A). No double reaches it before rounding; the largest, rounding to it, answers (#1746).
        Assert.Equal(double.MaxValue, Distributions.FisherSnedecorIsf(0.9999999999, 1e-300, 1e-310));
        double p = Distributions.FisherSnedecorSf(1.0, 1e-15, 1e-20);
        Assert.Equal(Distributions.FisherSnedecorQuantile(1.0 - p, 1e-15, 1e-20), Distributions.FisherSnedecorIsf(p, 1e-15, 1e-20));
    }

    [Theory]
    [InlineData(0.9, 3e5, 3e5, 2.3934649418911482e-179)]
    [InlineData(1.05, 2e5, 1e6, 3.1295036931690254e-42)]
    [InlineData(1.02, 4e5, 1e9, 1.276757848447452e-15)]
    [InlineData(3.548517320745774e-08, 3877.0285089526355, 2.218659653366229e-05, 4.2021564014224245e-124)]
    [InlineData(2e-6, 1e5, 1e-3, 1.2478871700343557e-106)]
    [InlineData(1e-33, 20.0, 20.0, 9.237800000000005e-292)]
    [InlineData(3e-308, 2.589, 15.2, 4.3323273861572293383e-91)]
    public void An_f_density_far_down_keeps_its_last_ulp(double f, double dfn, double dfd, double expected)
    {
        // Stirling's front against the deviation read the first 626 ulps off; a shape below 1e-3, an underflowed front and a
        // subnormal y kept a double's exponential, the next four 376, 84, 29 and 52 ulps off; 80-digit sums (#1746's Review A).
        Assert.Equal(expected, Distributions.FisherSnedecorPdf(f, dfn, dfd), 1.5 * Math.BitIncrement(expected) - 1.5 * expected);
    }

    [Fact]
    public void An_f_tail_in_temmes_region_of_the_large_shape_expansion_rounds_once()
    {
        // Temme's remainder took its exponential from a double η: 46 ulps off at dfs (2000, 1e6), and the quantile an ulp
        // past 1.2; the values are the binomial sums' to 50 digits (#1742's Review A).
        Assert.Equal(1.182170615600484e-07, Distributions.FisherSnedecorSf(1.1722903302693688, 2000, 1e6));
        Assert.Equal(1.3424153826423973e-09, Distributions.FisherSnedecorSf(1.2, 2000, 1e6));
        Assert.Equal(1.2, Distributions.FisherSnedecorIsf(1.3424153826423986e-9, 2000, 1e6));
    }

    [Fact]
    public void A_tail_below_a_shape_of_1e_minus_13_takes_its_log_scaled_beta_to_106_bits()
    {
        // log(s B(s, o)) in double held s ε, and the quantile read 584 ulps past the chi-squared limit's root; the values
        // are −expm1(s log(s/f) + γ s) to 80 digits (#1745's Review A).
        Assert.Equal(1.2442191766489454e-198, Distributions.FisherSnedecorCdf(1.049919789998326e-92, 1e200, 1e-200));
        Assert.Equal(1.7544196613275181e134, Distributions.FisherSnedecorQuantile(5e-298, 1e200, 1e-300));
    }

    [Fact]
    public void An_f_quantile_on_a_law_narrower_than_an_ulp_keeps_its_root()
    {
        // The walk from the mean now doubles its steps; the root is the one it walked to (#1745's Review A).
        Assert.Equal(
            0.9999999999999879,
            Distributions.FisherSnedecorQuantile(5.435177775019613e-194, 1.6221897293382113e264, 1.2032842379396074e31));
    }

    [Theory]
    [InlineData(204.20039947624326, 240.0875741080169, 2.0546148018221642e-271)]
    [InlineData(63.297634853080034, 194.78511696749328, 1.0660036090931271e-131)]
    public void A_student_density_hundreds_of_nats_down_rounds_once(double t, double df, double expected)
    {
        // One double exponential of some 620 nats read 1.5e-13 off; the values are 70-digit log-gamma sums' (#1745's
        // Review A).
        Assert.Equal(expected, Distributions.StudentPdf(t, df));
    }
}
