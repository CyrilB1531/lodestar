namespace Lodestar.Stats.Regression.Internal;

/// <summary>What each family contributes to IRLS: the link, its derivative, the variance and the deviance.</summary>
internal static class Families
{
    /// <summary>The mean a linear predictor maps to.</summary>
    public static double InverseLink(GlmFamily family, double eta) => family switch
    {
        GlmFamily.Binomial => 1.0 / (1.0 + Math.Exp(-eta)),
        GlmFamily.Poisson => Math.Exp(eta),
        _ => throw Undeclared(family),
    };

    /// <summary>The derivative of the link at a mean, which IRLS needs for the working response.</summary>
    public static double LinkDerivative(GlmFamily family, double mu) => family switch
    {
        GlmFamily.Binomial => 1.0 / (mu * (1.0 - mu)),
        GlmFamily.Poisson => 1.0 / mu,
        _ => throw Undeclared(family),
    };

    /// <summary>The variance function.</summary>
    public static double Variance(GlmFamily family, double mu) => family switch
    {
        GlmFamily.Binomial => mu * (1.0 - mu),
        GlmFamily.Poisson => mu,
        _ => throw Undeclared(family),
    };

    /// <summary>One observation's contribution to the deviance.</summary>
    /// <remarks>
    /// The <c>y == 0</c> and <c>y == 1</c> arms are not an optimisation: the general formula
    /// carries <c>y log(y / mu)</c>, which is <c>0 * -inf</c> at the boundary and NaN in
    /// floating point, where the limit is 0.
    /// </remarks>
    public static double UnitDeviance(GlmFamily family, double y, double mu) => family switch
    {
        GlmFamily.Binomial => 2.0 * (Xlogy(y, y / mu) + Xlogy(1.0 - y, (1.0 - y) / (1.0 - mu))),
        GlmFamily.Poisson => 2.0 * (Xlogy(y, y / mu) - (y - mu)),
        _ => throw Undeclared(family),
    };

    /// <summary><c>x log(y)</c>, and zero where <c>x</c> is, which is the limit rather than NaN.</summary>
    private static double Xlogy(double x, double y) => x == 0.0 ? 0.0 : x * Math.Log(y);

    private static ArgumentOutOfRangeException Undeclared(GlmFamily family) =>
        new(nameof(family), family, $"{family} is not a declared {nameof(GlmFamily)}.");
}
