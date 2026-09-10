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
    /// agreement decision 0081 asked for rather than a second approximation.
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
    [InlineData(0.0)]
    [InlineData(1.0)]
    [InlineData(double.NaN)]
    public void A_probability_outside_the_open_unit_interval_is_refused(double p)
    {
        Assert.Throws<ArgumentOutOfRangeException>(() => Distributions.StudentQuantile(p, 5.0));
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
}
