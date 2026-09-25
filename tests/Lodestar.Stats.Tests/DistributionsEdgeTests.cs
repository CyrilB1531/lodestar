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
        Assert.Throws<ArgumentOutOfRangeException>(() => Distributions.FisherSf(1.0, df, 5.0));
        Assert.Throws<ArgumentOutOfRangeException>(() => Distributions.FisherSf(1.0, 5.0, df));
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
        Chi2ContingencyResult table = ChiSquare.Contingency(
            [[20.0, 30.0], [30.0, 20.0]], Continuity.None);

        Assert.Equal(
            table.PValue,
            Distributions.ChiSquaredSf(table.Statistic, table.Dof),
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
        Assert.Throws<ArgumentOutOfRangeException>(() => Distributions.FisherQuantile(p, 2.0, 3.0));
        Assert.Throws<ArgumentOutOfRangeException>(() => Distributions.FisherIsf(p, 2.0, 3.0));
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
        Assert.Equal(0.0, Distributions.FisherQuantile(0.0, 2.0, 3.0));
        Assert.Equal(double.PositiveInfinity, Distributions.FisherQuantile(1.0, 2.0, 3.0));
        Assert.Equal(double.PositiveInfinity, Distributions.FisherIsf(0.0, 2.0, 3.0));
        Assert.Equal(0.0, Distributions.FisherIsf(1.0, 2.0, 3.0));
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
            Distributions.FisherPdf(double.NaN, 2.0, 3.0), Distributions.FisherCdf(double.NaN, 2.0, 3.0), Distributions.FisherSf(double.NaN, 2.0, 3.0),
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
        Assert.Equal(0.0, Distributions.FisherPdf(-1.0, 2.0, 3.0));
        Assert.Equal(0.0, Distributions.FisherCdf(-1.0, 2.0, 3.0));
        Assert.Equal(1.0, Distributions.FisherSf(-1.0, 2.0, 3.0));
    }

    /// <summary>At the origin the density is infinite, finite or zero by the first shape alone, as scipy answers.</summary>
    [Fact]
    public void At_the_origin_a_density_follows_its_shape()
    {
        Assert.Equal(double.PositiveInfinity, Distributions.ChiSquaredPdf(0.0, 1.0));
        Assert.Equal(0.5, Distributions.ChiSquaredPdf(0.0, 2.0));
        Assert.Equal(0.0, Distributions.ChiSquaredPdf(0.0, 4.0));
        Assert.Equal(double.PositiveInfinity, Distributions.FisherPdf(0.0, 1.0, 3.0));
        Assert.Equal(1.0, Distributions.FisherPdf(0.0, 2.0, 3.0), 1e-14);
        Assert.Equal(0.0, Distributions.FisherPdf(0.0, 4.0, 3.0));
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
        Assert.Equal(1.0, Distributions.FisherPdf(f, dfn, dfd) / exact, 1e-12);
    }

    /// <summary>At an infinite statistic, or one whose product with the degrees of freedom overflows, the F laws answer their limits rather than NaN or an exception.</summary>
    [Fact]
    public void An_infinite_or_overflowing_F_statistic_answers_the_limits()
    {
        Assert.Equal(1.0, Distributions.FisherCdf(double.PositiveInfinity, 2.0, 20.0));
        Assert.Equal(0.0, Distributions.FisherSf(double.PositiveInfinity, 2.0, 20.0));
        Assert.Equal(0.0, Distributions.FisherPdf(double.PositiveInfinity, 2.0, 20.0));
        Assert.Equal(1.0, Distributions.FisherCdf(1e308, 10.0, 5.0));
        double density = Distributions.FisherPdf(1e308, 10.0, 5.0);
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
        Assert.Throws<ArgumentOutOfRangeException>(() => Distributions.FisherCdf(1.0, 3.0, double.PositiveInfinity));
        Assert.Throws<ArgumentOutOfRangeException>(() => Distributions.FisherPdf(1.0, double.PositiveInfinity, 3.0));
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
}
