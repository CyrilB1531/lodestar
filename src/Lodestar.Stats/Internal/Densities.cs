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

    // log(2 pi) / 2.
    private const double HalfLogTwoPi = 0.9189385332046728;

    /// <summary>Student's t density at <paramref name="t"/> with <paramref name="df"/> degrees of freedom.</summary>
    internal static double Student(double t, double df)
    {
        if (double.IsNaN(t))
        {
            return double.NaN;
        }

        // The two limits StudentSf takes (#1690's review): the normal law, and below Beta.SmallShape the constant
        // from log(a B), since df/2 of a subnormal df rounds to zero and LogGammaHalfStep threw on it.
        if (df >= Beta.NormalLimitDf)
        {
            return Distributions.NormalPdf(t);
        }
        double half = df / 2.0;
        // Two log-gammas, not the inversion's -log B - log(df)/2: through Beta.LogBeta StudentPdf took 1.3 to 2 times as
        // long, 54 to 71 ns at df 5.5 (Stopwatch; the #1690 spec has the run).
        double logNormalizer = half < Beta.SmallShape
            ? Beta.SmallStudentLogNormalizer(df)
            : LogNormalizer(half, df);
        // t^2/df overflowed past 1e308 and the density read 0, as scipy's t.pdf does: StudentPdf(1e140, 1e-30) for 5e-171.
        double logDensity = logNormalizer - ((df + 1.0) / 2.0 * Beta.StudentLogOnePlusSquare(t, df));
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
            // AtOrigin reads its second value at a = 1 only, where B(1, b) = 1/b makes it b dfn/dfd = 1 exactly: through
            // log B it was 5e-14 off at dfd 1e-310 (#1690's review).
            return AtOrigin(a, 1.0);
        }
#pragma warning restore S1244

        if (double.IsPositiveInfinity(f))
        {
            return 0.0;
        }

        // The beta density of y times dy/df reduces to y^a (1-y)^b / (B(a, b) f), and Beta.Front forms
        // that numerator from Stirling past shapes of ten, so no log-gamma of order n log n cancels.
        (double y, double oneMinusY) = Beta.FisherSplit(f, dfn, dfd);
        if (Beta.TakesLimit(y, oneMinusY))
        {
            return Beta.FisherDensityFromLimit(f, dfn, dfd, y);
        }
        // Against the exact mean from f - 1, as the tail takes it: at shapes past 1e30 y's rounding is the law's width.
        double deviation = Beta.FisherDeviation(f, dfn, dfd, y);
        return (double.IsNaN(deviation) ? Beta.Front(a, b, y, oneMinusY) : Beta.StirlingFront(a, b, y, oneMinusY, deviation)) / f;
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

    /// <summary><c>log Γ(h + ½) − log Γ(h) − ½ log(2h π)</c>, h = df/2, with the two <c>½ log h</c> cancelled by hand from a shape of 10.</summary>
    /// <remarks>
    /// Formed as a difference they were 23 each at df 1e19, and the density stepped 1.4e-15 crossing df 1e20, where it
    /// becomes the normal one (#1690's review).
    /// </remarks>
    private static double LogNormalizer(double half, double df)
    {
        if (half < StirlingShape)
        {
            return Gamma.LogGamma(half + 0.5) - Gamma.LogGamma(half) - (0.5 * Math.Log(df * Math.PI));
        }
        return (half * NearZero.Log1P(0.5 / half)) - 0.5 - HalfLogTwoPi
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
}
