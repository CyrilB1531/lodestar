using Lodestar.Stats.Regression.Internal;
using Xunit;

namespace Lodestar.Stats.Regression.Tests;

public sealed class GlmFamilyTests
{
    [Fact]
    public void Logit_and_its_inverse_are_inverses()
    {
        double mu = Families.InverseLink(FamilyShape.Of(GlmFamily.Binomial), 0.75);

        // logit(mu) = log(mu / (1 - mu)), so the inverse of 0.75 back through the link is 0.75.
        Assert.Equal(0.75, Math.Log(mu / (1.0 - mu)), 12);
    }

    [Fact]
    public void The_log_link_inverts_to_exp()
    {
        Assert.Equal(Math.Exp(1.5), Families.InverseLink(FamilyShape.Of(GlmFamily.Poisson), 1.5), 12);
    }

    [Fact]
    public void A_perfect_binomial_prediction_has_zero_unit_deviance()
    {
        Assert.Equal(0.0, Families.UnitDeviance(FamilyShape.Of(GlmFamily.Binomial), 1.0, 1.0), 12);
        Assert.Equal(0.0, Families.UnitDeviance(FamilyShape.Of(GlmFamily.Binomial), 0.0, 0.0), 12);
    }

    [Fact]
    public void A_perfect_poisson_prediction_has_zero_unit_deviance()
    {
        Assert.Equal(0.0, Families.UnitDeviance(FamilyShape.Of(GlmFamily.Poisson), 3.0, 3.0), 12);
        Assert.Equal(0.0, Families.UnitDeviance(FamilyShape.Of(GlmFamily.Poisson), 0.0, 0.0), 12);
    }

    [Fact]
    public void The_inverse_link_inverts_to_the_reciprocal_and_its_derivative_is_minus_one_over_mu_squared()
    {
        FamilyShape gamma = FamilyShape.Of(GlmFamily.Gamma);

        Assert.Equal(0.25, Families.InverseLink(gamma, 4.0), 12);
        Assert.Equal(-1.0 / 16.0, Families.LinkDerivative(gamma, 4.0), 12);
    }

    [Fact]
    public void A_perfect_gamma_prediction_has_zero_unit_deviance()
    {
        Assert.Equal(0.0, Families.UnitDeviance(FamilyShape.Of(GlmFamily.Gamma), 2.5, 2.5), 12);
    }

    [Fact]
    public void An_undeclared_family_is_refused()
    {
        Assert.Throws<ArgumentOutOfRangeException>(
            () => Families.Variance(new FamilyShape((GlmFamily)7, GlmLink.Log, 1.0), 0.5));
    }
}
