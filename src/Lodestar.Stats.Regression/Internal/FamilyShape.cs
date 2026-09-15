namespace Lodestar.Stats.Regression.Internal;

/// <summary>A family with the link and the parameter it was resolved to, which every IRLS function reads.</summary>
/// <param name="Family">The response distribution.</param>
/// <param name="Link">The resolved link: <see cref="GlmLink.Default"/> only for binomial, where it means logit.</param>
/// <param name="Alpha">The negative binomial's dispersion; the other families ignore it.</param>
internal readonly record struct FamilyShape(GlmFamily Family, GlmLink Link, double Alpha)
{
    /// <summary>A family on its default link, which is how the tests and the shipped families reach IRLS.</summary>
    public static FamilyShape Of(GlmFamily family) => family switch
    {
        GlmFamily.Binomial => new(family, GlmLink.Default, 1.0),
        GlmFamily.Poisson or GlmFamily.NegativeBinomial => new(family, GlmLink.Log, 1.0),
        GlmFamily.Gamma => new(family, GlmLink.Inverse, 1.0),
        _ => throw Families.Undeclared(family),
    };
}
