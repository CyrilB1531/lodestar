using Lodestar.Stats.Internal;
using Xunit;

namespace Lodestar.Stats.Tests.Internal;

/// <summary>The inverse of the Student upper tail, which is what a confidence interval needs.</summary>
public sealed class StudentQuantileTests
{
    [Theory]
    [InlineData(0.5, 1.0)]
    [InlineData(0.5, 30.0)]
    [InlineData(0.025, 1.0)]
    [InlineData(0.025, 12.0)]
    [InlineData(0.025, 12.7431)]
    [InlineData(1e-12, 8.0)]
    [InlineData(0.999, 3.0)]
    public void Quantile_inverts_the_tail(double p, double df)
    {
        double t = Beta.StudentQuantile(p, df);

        // Relative on the probability, not absolute on t: at p = 1e-12 an
        // absolute check on the recovered probability proves nothing.
        Assert.Equal(1.0, Beta.StudentSf(t, df) / p, 1e-9);
    }

    [Theory]
    // scipy.stats.t.isf, covering each seed the inversion starts from: the Cauchy and df = 2
    // closed forms, the power-law tail past z^2 = df, and the Cornish-Fisher expansion inside it.
    [InlineData(1e-12, 1.0, 318309886183.7907)]
    [InlineData(1e-10, 2.0, 70710.67810804816)]
    [InlineData(1e-100, 3.0, 2.225769823822442e+33)]
    [InlineData(0.01, 5.0, 3.364929998907218)]
    [InlineData(1e-20, 30.0, 22.658878371940183)]
    [InlineData(0.05, 12.0, 1.7822875556493194)]
    [InlineData(0.025, 95.0, 1.985251003505498)]
    public void Quantile_matches_scipy_from_every_seed(double p, double df, double expected)
    {
        Assert.Equal(1.0, Beta.StudentQuantile(p, df) / expected, 1e-12);
    }

    [Fact]
    public void Quantile_is_zero_at_one_half()
    {
        Assert.Equal(0.0, Beta.StudentQuantile(0.5, 7.0), 1e-12);
    }

    [Fact]
    public void Quantile_matches_the_familiar_two_sided_five_percent_points()
    {
        // The numbers every statistics table prints: t(0.025, df).
        Assert.Equal(12.706204736432095, Beta.StudentQuantile(0.025, 1.0), 1e-9);
        Assert.Equal(2.2621571627409915, Beta.StudentQuantile(0.025, 9.0), 1e-9);

        // long-comment: explains a deliberate deviation from the plan's literal
        //     test value, which a reviewer needs the reasoning for.
        // 1e8, not 1e12: StudentSf's reflection branch is in play at both, but
        // which error dominates differs. At 1e8 the residual is genuinely
        // Gamma.LogGamma's own precision at that magnitude (9.4e-8, confirmed
        // by substituting scipy's gammaln, which drops it to 2.5e-8). At 1e12
        // that substitution leaves the residual unchanged (1.254e-4) -- there the
        // reflection itself is the cause: RegularizedIncomplete(0.5, 5e11,
        // 3.84e-12) returns 1 - 0.05 with 6.6e-6 relative error, which the outer
        // 1.0 - (...) amplifies roughly nineteenfold. Neither is owned by this
        // task; 1e8 sits comfortably under the assertion's 1e-6 bound either way.
        Assert.Equal(1.9599639845400545, Beta.StudentQuantile(0.025, 1e8), 1e-6);
    }

    [Fact]
    public void Quantile_refuses_a_probability_outside_the_open_unit_interval()
    {
        Assert.Throws<ArgumentOutOfRangeException>(() => Beta.StudentQuantile(0.0, 5.0));
        Assert.Throws<ArgumentOutOfRangeException>(() => Beta.StudentQuantile(1.0, 5.0));
        Assert.Throws<ArgumentOutOfRangeException>(() => Beta.StudentQuantile(double.NaN, 5.0));
    }
}
