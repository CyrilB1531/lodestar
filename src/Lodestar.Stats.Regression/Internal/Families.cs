namespace Lodestar.Stats.Regression.Internal;

/// <summary>What each family contributes to IRLS: the link, its derivative, the variance and the deviance.</summary>
/// <remarks>
/// <c>alpha</c> is the negative binomial's dispersion, passed to every function whose value depends on it and
/// ignored by the other families.
/// </remarks>
internal static class Families
{
    /// <summary>The reference's <c>FLOAT_EPS</c>, the floor its variance functions clip a mean to.</summary>
    private const double Epsilon = 2.220446049250313e-16;

    /// <summary>The mean a linear predictor maps to.</summary>
    public static double InverseLink(GlmFamily family, double eta) => family switch
    {
        GlmFamily.Binomial => 1.0 / (1.0 + Math.Exp(-eta)),
        GlmFamily.Poisson or GlmFamily.NegativeBinomial => Math.Exp(eta),
        _ => throw Undeclared(family),
    };

    /// <summary>The derivative of the link at a mean, which IRLS needs for the working response.</summary>
    public static double LinkDerivative(GlmFamily family, double mu) => family switch
    {
        GlmFamily.Binomial => 1.0 / (mu * (1.0 - mu)),
        GlmFamily.Poisson or GlmFamily.NegativeBinomial => 1.0 / mu,
        _ => throw Undeclared(family),
    };

    /// <summary>The variance function.</summary>
    public static double Variance(GlmFamily family, double mu, double alpha) => family switch
    {
        GlmFamily.Binomial => mu * (1.0 - mu),
        GlmFamily.Poisson => mu,
        GlmFamily.NegativeBinomial => mu + (alpha * mu * mu),
        _ => throw Undeclared(family),
    };

    /// <summary>One observation's contribution to the deviance.</summary>
    /// <remarks>
    /// The <c>y == 0</c> and <c>y == 1</c> arms are not an optimisation: the general formula
    /// carries <c>y log(y / mu)</c>, which is <c>0 * -inf</c> at the boundary and NaN in
    /// floating point, where the limit is 0.
    /// </remarks>
    public static double UnitDeviance(GlmFamily family, double y, double mu, double alpha) => family switch
    {
        GlmFamily.Binomial => 2.0 * (Xlogy(y, y / mu) + Xlogy(1.0 - y, (1.0 - y) / (1.0 - mu))),
        GlmFamily.Poisson => 2.0 * (Xlogy(y, y / mu) - (y - mu)),
        GlmFamily.NegativeBinomial => NegativeBinomialUnitDeviance(y, mu, 1.0 / alpha),
        _ => throw Undeclared(family),
    };

    /// <summary>The negative binomial's unit deviance, with <c>theta = 1/alpha</c>.</summary>
    /// <remarks>
    /// <c>y/μ</c> is clipped at <see cref="Epsilon"/> as the reference's <c>_clean</c> clips it, so a zero count
    /// leaves only the second term rather than <c>0 * -inf</c>.
    /// </remarks>
    private static double NegativeBinomialUnitDeviance(double y, double mu, double theta) =>
        2.0 * ((y * Math.Log(Math.Max(y / mu, Epsilon))) - ((y + theta) * Math.Log((y + theta) / (mu + theta))));

    /// <summary><c>x log(y)</c>, and zero where <c>x</c> is, which is the limit rather than NaN.</summary>
    private static double Xlogy(double x, double y) => x == 0.0 ? 0.0 : x * Math.Log(y);

    /// <summary>The refusal every family switch in this package ends on.</summary>
    /// <remarks>
    /// Shared so that no arm throws the parameterless <c>null</c> message: a caller who casts an
    /// undeclared integer to <see cref="GlmFamily"/> reads which value was refused (#616).
    /// </remarks>
    public static ArgumentOutOfRangeException Undeclared(GlmFamily family) =>
        new(nameof(family), family, $"{family} is not a declared {nameof(GlmFamily)}.");
}
