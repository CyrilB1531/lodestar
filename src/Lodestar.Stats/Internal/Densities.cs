namespace Lodestar.Stats.Internal;

/// <summary>The t, F and chi-squared densities, formed so no large term cancels (#1158).</summary>
/// <remarks>
/// Each density is a ratio of gamma functions times powers, and at a million degrees of freedom
/// those factors are of order 1e6 in logs while their ratio is of order one: subtracting two
/// log-gammas there loses six of the sixteen digits. Where a shape passes
/// <see cref="StirlingShape"/> the ratio is formed from Stirling's series instead, as
/// <c>Gamma.Prefactor</c> already does, so only terms of order one are ever subtracted.
/// </remarks>
internal static class Densities
{
    // Gamma.LogStirlingCorrection holds 3e-17 from here, the same threshold Gamma.Prefactor uses.
    private const double StirlingShape = 10.0;

    /// <summary>Student's t density at <paramref name="t"/> with <paramref name="df"/> degrees of freedom.</summary>
    internal static double Student(double t, double df)
    {
        if (double.IsNaN(t))
        {
            return double.NaN;
        }

        double half = df / 2.0;
        double logRatio = LogGammaHalfStep(half);
        double logDensity = logRatio - (0.5 * Math.Log(df * Math.PI))
            - ((df + 1.0) / 2.0 * Log1P(t / df * t));
        return Math.Exp(logDensity);
    }

    /// <summary>The F density at <paramref name="f"/>, through the beta density of <c>y = d₁f / (d₁f + d₂)</c>.</summary>
    internal static double Fisher(double f, double dfn, double dfd)
    {
        if (double.IsNaN(f))
        {
            return double.NaN;
        }
        if (f < 0.0)
        {
            return 0.0;
        }
        double a = dfn / 2.0;
        double b = dfd / 2.0;
        // S1244: the support's end, where the density is 0, finite or infinite by the first shape alone.
#pragma warning disable S1244
        if (f == 0.0)
        {
            return AtOrigin(a, Math.Exp(-Beta.LogBeta(a, b)) * dfn / dfd);
        }
#pragma warning restore S1244

        if (double.IsPositiveInfinity(f))
        {
            return 0.0;
        }

        // The beta density of y times dy/df reduces to y^a (1-y)^b / (B(a, b) f), and Beta.Front forms
        // that numerator from Stirling past shapes of ten, so no log-gamma of order n log n cancels.
        (double y, double oneMinusY) = Beta.FisherSplit(f, dfn, dfd);
        return Beta.Front(a, b, y, oneMinusY) / f;
    }

    /// <summary>The chi-squared density at <paramref name="x"/>: the gamma prefactor over <c>x</c>.</summary>
    internal static double ChiSquared(double x, double df)
    {
        if (double.IsNaN(x))
        {
            return double.NaN;
        }
        if (x < 0.0)
        {
            return 0.0;
        }
        double a = df / 2.0;
        // S1244: the support's end, where the density is 0, one half or infinite by the shape alone.
#pragma warning disable S1244
        if (x == 0.0)
        {
            return AtOrigin(a, 0.5);
        }
#pragma warning restore S1244
        if (double.IsPositiveInfinity(x))
        {
            return 0.0;
        }

        // f(x) = (x/2)^(a-1) e^(-x/2) / (2 Gamma(a)) = Prefactor(a, x/2) / x.
        return Gamma.Prefactor(a, x / 2.0) / x;
    }

    /// <summary><c>log Γ(h + ½) − log Γ(h)</c>, exact of cancellation for large <c>h</c>.</summary>
    /// <remarks>
    /// With Stirling, <c>log Γ(z) = (z − ½) log z − z + ½ log 2π + c(z)</c>, the difference is
    /// <c>h log1p(1/(2h)) + ½ log h − ½ + c(h + ½) − c(h)</c>: no term of order <c>h log h</c> remains.
    /// </remarks>
    private static double LogGammaHalfStep(double half)
    {
        if (half < StirlingShape)
        {
            return Gamma.LogGamma(half + 0.5) - Gamma.LogGamma(half);
        }
        return (half * Log1P(0.5 / half)) + (0.5 * Math.Log(half)) - 0.5
            + Gamma.LogStirlingCorrection(half + 0.5) - Gamma.LogStirlingCorrection(half);
    }

    /// <summary>A density of shape <paramref name="a"/> at the support's origin: infinite below one, <paramref name="atOne"/> at one, zero above.</summary>
    private static double AtOrigin(double a, double atOne)
    {
        if (a < 1.0)
        {
            return double.PositiveInfinity;
        }
        // S1244: the one shape whose density neither vanishes nor diverges at the origin.
#pragma warning disable S1244
        return a == 1.0 ? atOne : 0.0;
#pragma warning restore S1244
    }

    /// <summary><c>log(1 + x)</c> without losing <c>x</c>'s digits when it is small.</summary>
    /// <remarks>
    /// Gamma's series computes <c>log(1 + t) − t</c> without cancellation for small <c>t</c>, on both
    /// frameworks. For a large <c>x</c> adding <c>x</c> back would drown the logarithm in <c>x</c>'s
    /// rounding, so there the plain logarithm is exact enough and is used instead.
    /// </remarks>
    private static double Log1P(double x) =>
        Math.Abs(x) < 0.5 ? Gamma.LogOnePlusMinus(x, 1.0 + x) + x : Math.Log(1.0 + x);
}
