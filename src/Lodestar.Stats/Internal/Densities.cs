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

    // A front or prefactor below it has lost digits to the subnormals; above, front / f and Prefactor(a, x/2) / x are exact,
    // and a gate at 1e-290 read FisherSnedecorPdf(0.1, 1e14, 200) 8e-5 off (#1708's Review A).
    private const double FrontFloor = NearZero.SmallestNormal;

    // A chi-squared prefactor below it has an exponent past -69, whose rounding grows past 1e-14 (#1708).
    private const double LongExponent = 1e-30;

    // Both F shapes past it take the front in logs where it underflows: log B by log-gammas cancels against the powers.
    private const double LargeBothShapes = 1000.0;

    // Below it a halved x loses bits among the subnormals.
    private const double SubnormalHalving = 4.0 * NearZero.SmallestNormal;

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
        double front = double.IsNaN(deviation) ? Beta.Front(a, b, y, oneMinusY) : Beta.StirlingFront(a, b, y, oneMinusY, deviation);
        return front >= FrontFloor ? front / f : UnderflowedFisher(f, dfn, dfd, y, oneMinusY, deviation);
    }

    /// <summary>The F density where its front underflowed before the division by f.</summary>
    /// <remarks>
    /// Reading 0 for 3.3e-150 at f = 1e-300 and dfs 3 and 1, and for 9.2e-292 at 1e-33 and 20 and 20 (#1708): y^(a-1) by
    /// Math.Pow, where logs held the second to 2e-12 only; logs below SmallShape, and past LargeBothShapes, where log B
    /// cancelled against the powers and stepped 230-fold at shapes of 1e15. Out of line, so front / f keeps its time.
    /// </remarks>
    [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.NoInlining)]
    private static double UnderflowedFisher(double f, double dfn, double dfd, double y, double oneMinusY, double deviation)
    {
        double a = dfn / 2.0;
        double b = dfd / 2.0;
        return a >= Gamma.SmallShape && Math.Min(a, b) < LargeBothShapes
            ? SmallSideFisher(f, dfn, dfd, y, oneMinusY, a, b)
            : Math.Exp(Beta.LogFront(a, b, y, oneMinusY, deviation) - Math.Log(f));
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

        // f(x) = (x/2)^(a-1) e^(-x/2) / (2 Gamma(a)) = Prefactor(a, x/2) / x, unless a small x makes its exponent
        // long or loses bits halved; the upper tail, where it is only small, keeps it.
        double prefactor = Gamma.Prefactor(a, x / 2.0);
        return x >= 2.0 || (prefactor >= LongExponent && x >= SubnormalHalving) ? prefactor / x : LongChiSquared(x, a, prefactor);
    }

    /// <summary>The chi-squared density below x = 2 where Prefactor(a, x/2) lost digits or underflowed before the division by x.</summary>
    /// <remarks>
    /// Its exponent's rounding grew, 2e-13 at 1e-62, x^a underflowed, ChiSquaredPdf(1e-70, 10) reading 0 for 1.3e-283, and a
    /// subnormal x halved to 0 (#1708): x^(a-1) by Math.Pow times exp(-x/2 - a ln 2 - log Γ(a)) for shapes in [0.1, 10); below,
    /// logs, log x − ln 2 for a subnormal x; from a shape
    /// of 10, whose Stirling form holds what logs would, logs only where it underflows. Out of line, as UnderflowedFisher.
    /// </remarks>
    [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.NoInlining)]
    private static double LongChiSquared(double x, double a, double prefactor)
    {
        if (a < Gamma.SmallShape)
        {
            // A subnormal a divided by x first, as a e^(...) would round it before: 3.4e-9 off at a of 1e-315 (#1708's Review A).
            double logHalf = x < SubnormalHalving ? Math.Log(x) - Beta.Ln2 : Math.Log(x / 2.0);
            double power = Math.Exp((a * logHalf) - (x / 2.0) - Gamma.LogGammaOnePlus(a));
            return a >= NearZero.SmallestNormal ? a * power / x : power * (a / x);
        }
        if (a < StirlingShape)
        {
            return Math.Pow(x, a - 1.0) * Math.Exp((-x / 2.0) - (a * Beta.Ln2) - Gamma.LogGamma(a));
        }
        return prefactor >= FrontFloor ? prefactor / x : Math.Exp(Gamma.LogStirlingPrefactor(a, x / 2.0) - Math.Log(x));
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

    /// <summary><c>y^(a−1) · (1−y)^b (y/f) / B(a, b)</c> where Front underflowed: the power by Math.Pow, the rest by one exponential.</summary>
    /// <remarks>Where the two overflow and underflow against each other, as at a huge second shape, one exponential takes it all.</remarks>
    private static double SmallSideFisher(double f, double dfn, double dfd, double y, double oneMinusY, double a, double b)
    {
        // y/f is d1/(d2 + d1 f), which a huge denominator df takes subnormal and a tiny one past the largest double, read +inf
        // at (5e-324, 1e10, 1e-300) (#1704's Review A): there log(d1/d2) - log1p(y/(1-y)) instead.
        double ratio = dfn / (dfd + (dfn * f));
        double logRatio = ratio >= NearZero.SmallestNormal && ratio < double.PositiveInfinity
            ? Math.Log(ratio)
            : Math.Log(dfn) - Math.Log(dfd) - NearZero.Log1P(y / oneMinusY);
        double logRest = (b * Beta.LogComplement(y, oneMinusY)) - Beta.LogBeta(a, b) + logRatio;
        // Near y = 1 the power takes log y from 1 - y, which keeps the digits y's rounding would multiply by a - 1.
        if (y > 0.5)
        {
            return Math.Exp(((a - 1.0) * Beta.LogComplement(oneMinusY, y)) + logRest);
        }
        // The power in two halves around the rest, so it may pass below the smallest normal double and still keep its bits:
        // whole, a subnormal power read FisherSnedecorPdf(1e-33, 20, 10000) 3.6e-3 off.
        double half = Math.Pow(y, (a - 1.0) / 2.0);
        double density = half * Math.Exp(logRest) * half;
        return half >= NearZero.SmallestNormal && half < double.PositiveInfinity && density >= NearZero.SmallestNormal && density < double.PositiveInfinity
            ? density
            : Math.Exp(((a - 1.0) * Math.Log(y)) + logRest);
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
