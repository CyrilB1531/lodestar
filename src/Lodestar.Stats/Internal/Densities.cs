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

    // A chi-squared prefactor or an F front below it has an exponent past -69, whose rounding grows past 1e-14 (#1708).
    private const double LongExponent = 1e-30;

    // Both F shapes past it take the front in logs where it underflows: log B by log-gammas cancels against the powers.
    private const double LargeBothShapes = 1000.0;

    // The most parts a power is split into, each at most MaxLogPart in log: e^-600 is normal, so three hold any underflow
    // the product can bring back, the smallest double over the largest being e^-1453; four, so the count is a power of two.
    private const int MaxPowerParts = 4;
    private const double MaxLogPart = 600.0;

    // Below it the F density takes 1/B as s e^(−log s B), log B being past 7 nats.
    private const double ProductShape = 1e-3;

    // e^-8: from ProductShape to Beta.SmallShape a front below it takes the product, its exponent's rounding past 1e-15:
    // FisherSnedecorPdf(1e10, 0.002, 5) was 6e-15 off as one exponential, 5e-16 at a df an ulp under 0.002.
    private const double SmallShapeFront = 3.3546262790251185e-4;

    // The most factors SmallShapeFisher and LongFrontFisher form: three, and each power in up to MaxPowerParts parts.
    private const int MaxFactors = 3 + (2 * MaxPowerParts);

    // Twice ExactHalf.Threshold, conservatively: below it the density takes x unhalved, as halving x rounds under the threshold.
    private const double SubnormalHalving = 2.0 * ExactHalf.Threshold;

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
        // S1244: the support's end, where the density is 0, finite or infinite by the first shape alone.
#pragma warning disable S1244
        if (f == 0.0)
        {
            // AtOrigin reads its second value at a = 1 only, where B(1, b) = 1/b makes it 1 exactly, through log B 5e-14
            // off at dfd 1e-310 (#1690's review); it reads the shape's side of one, which a rounded half keeps.
            return AtOrigin(dfn / 2.0, 1.0);
        }
#pragma warning restore S1244

        if (double.IsPositiveInfinity(f))
        {
            return 0.0;
        }
        // The halves are formed only where they are exact; LiftedFisher takes its shapes lifted.
        return ExactHalf.EitherBelow(dfn, dfd) ? LiftedFisher(f, dfn, dfd) : FisherAt(f, dfn, dfd, dfn / 2.0, dfd / 2.0);
    }

    /// <summary>The F density at a df below ExactHalf.Threshold: at the lifted shapes, of degree one in them, dropped once.</summary>
    [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.NoInlining)]
    private static double LiftedFisher(double f, double dfn, double dfd)
    {
        (double a, double b, _, _) = ExactHalf.FisherShapes(dfn, dfd);
        (double y, double oneMinusY) = Beta.FisherSplit(f, dfn, dfd);
        return SmallShapeFisher(f, dfn, dfd, (a, b), y, oneMinusY, ExactHalf.Drop);
    }

    /// <summary>The F density at a shape below ProductShape, times <paramref name="scale"/>.</summary>
    /// <remarks>
    /// <c>s y^a (1 − y)^b / (s B(a, b) f)</c>, s the smaller shape, as factors each a double, multiplied so every partial
    /// product stays normal and only the last rounds: through Front's <c>e^(−log B)</c> of some 600 it took 600 ulps, and
    /// as one exponential before s/f, FisherSnedecorPdf(1e-320, 60, 4.4e-308) read 0 for 2.4e-314 (#1729's Review A).
    /// </remarks>
    [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.NoInlining)]
    private static double SmallShapeFisher(
        double f, double dfn, double dfd, (double A, double B) shapes, double y, double oneMinusY, double scale)
    {
        (double a, double b) = shapes;
        double small = Math.Min(a, b);
        Span<double> factors = stackalloc double[MaxFactors];
        factors[0] = small;
        factors[1] = scale;
        factors[2] = Math.Exp(-Beta.LogScaledBeta(small, Math.Max(a, b)));
        return SidesProduct(factors, 3, (f, dfn, dfd), shapes, (y, oneMinusY));
    }

    /// <summary>The F density past a front of LongExponent as SmallShapeFisher's product, <paramref name="logBeta"/> being
    /// Front's <c>log B</c>: below SmallShape <c>log s + log B</c> cancels and <c>s B</c> takes LogScaledBeta.</summary>
    /// <remarks>Its own method: a log B passed through SmallShapeFisher cost that density 24 of its 170 ns.</remarks>
    [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.NoInlining)]
    private static double LongFrontFisher(
        double f, double dfn, double dfd, (double A, double B) shapes, double y, double oneMinusY, double logBeta)
    {
        if (Math.Min(shapes.A, shapes.B) < Beta.SmallShape)
        {
            return SmallShapeFisher(f, dfn, dfd, shapes, y, oneMinusY, 1.0);
        }
        Span<double> factors = stackalloc double[MaxFactors];
        factors[0] = Math.Exp(-logBeta);
        return SidesProduct(factors, 1, (f, dfn, dfd), shapes, (y, oneMinusY));
    }

    /// <summary>The leading factors times <c>y^a (1 − y)^b</c>, over f.</summary>
    [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
    private static double SidesProduct(
        Span<double> factors, int count, (double F, double Dfn, double Dfd) law, (double A, double B) shapes, (double Y, double OneMinusY) split)
    {
        int scale = 0;
        count = AddSide(factors, count, law, (split.Y, split.OneMinusY), shapes.A, isLower: true, ref scale);
        count = AddSide(factors, count, law, (split.OneMinusY, split.Y), shapes.B, isLower: false, ref scale);
        return BalancedProduct(factors.Slice(0, count), law.F, scale);
    }

    /// <summary>Appends <c>side^exponent</c>: by Math.Pow on a normal side at most one half, from the other side's log1p
    /// above one half, and below the normal doubles from the odds' mantissa and exponent, which a subnormal side lacks:
    /// its power's 2^k is added into <paramref name="scale"/>.</summary>
    private static int AddSide(
        Span<double> factors, int count, (double F, double Dfn, double Dfd) law, (double Side, double Other) split, double exponent, bool isLower,
        ref int scale)
    {
        (double side, double other) = split;
        if (TryPower(factors, ref count, side, exponent))
        {
            return count;
        }
        if (side >= NearZero.SmallestNormal)
        {
            return AddPower(factors, count, side, exponent, Beta.LogComplement(other, side));
        }
        (double mantissa, int binary) = Beta.OddsParts(law.F, law.Dfn, law.Dfd, isLower);
        (double value, int binaryPower) = PowerOfTwo.Power(mantissa, binary, exponent);
        factors[count++] = value;
        scale += binaryPower;
        return count;
    }

    /// <summary>Appends <c>value^exponent</c> as one factor where the base is a normal double at most one half and the
    /// power by Math.Pow is normal too; false, appending nothing, otherwise.</summary>
    private static bool TryPower(Span<double> factors, ref int count, double value, double exponent)
    {
        if (value < NearZero.SmallestNormal || value > 0.5)
        {
            return false;
        }
        double power = Math.Pow(value, exponent);
        if (!(power >= NearZero.SmallestNormal))
        {
            return false;
        }
        factors[count++] = power;
        return true;
    }

    /// <summary>Appends <c>base^exponent</c> as parts each a normal double: by Math.Pow from a base of at most one half,
    /// and above it from its log, taken on the complement's side, where the base's own rounding would grow with the power.</summary>
    private static int AddPower(Span<double> factors, int count, double value, double exponent, double logValue)
    {
        double logPower = exponent * logValue;
        // A power of two, which divides the exponent exactly: a third rounded, and |log value| times that,
        // FisherSnedecorPdf(5e-324, 3.6, 1.8e-19) 1.2e-13 off (#1729's Review A).
        double needed = Math.Ceiling(-logPower / MaxLogPart);
        int parts = needed <= 2.0 ? (int)Math.Max(1.0, needed) : MaxPowerParts;
        double part = value >= NearZero.SmallestNormal && value <= 0.5
            ? Math.Pow(value, exponent / parts)
            : Math.Exp(logPower / parts);
        for (int i = 0; i < parts; i++)
        {
            factors[count++] = part;
        }
        return count;
    }

    /// <summary>The product of the factors times <c>2^scale</c> over <paramref name="divisor"/>, rounding once where it
    /// leaves the normal doubles.</summary>
    private static double BalancedProduct(Span<double> factors, double divisor, int scale)
    {
        // In order while every partial product stays normal, each step then rounding once as any order would; a factor
        // of 0, +∞ or NaN leaves the normal doubles at once.
        bool normal = scale == 0;
        double plain = 1.0;
        foreach (double factor in factors)
        {
            plain *= factor;
            normal &= plain >= NearZero.SmallestNormal && plain <= double.MaxValue;
        }
        // Else as mantissas and one exponent: ordering the factors by their logs overflowed, FisherSnedecorPdf(1e-310, 1,
        // 4.450147717014403e-308) reading +∞ for 10.5 (#1729's Review A).
        return normal ? plain / divisor : PowerOfTwo.Quotient(factors, stackalloc double[] { divisor }, scale);
    }

    /// <summary>The F density at a positive finite f and shapes <paramref name="a"/> and <paramref name="b"/>, y from the dfs.</summary>
    [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
    private static double FisherAt(double f, double dfn, double dfd, double a, double b)
    {
        // The beta density of y times dy/df reduces to y^a (1-y)^b / (B(a, b) f), and Beta.Front forms
        // that numerator from Stirling past shapes of ten, so no log-gamma of order n log n cancels.
        (double y, double oneMinusY) = Beta.FisherSplit(f, dfn, dfd);
        if (Beta.TakesLimit(y, oneMinusY))
        {
            return Beta.FisherDensityFromLimit(f, dfn, dfd, a, b, y);
        }
        // Front's e^(−log B), log B some 100 nats at shapes 1e-25 and 1e-47, took 4.2e-14 (#1729's Review A). Past
        // ProductShape it is under 8 nats, and LogScaledBeta's ten log1p steps took 2.4 times as long at dfs 0.1 and 3.
        if (a < ProductShape || b < ProductShape)
        {
            return SmallShapeFisher(f, dfn, dfd, (a, b), y, oneMinusY, 1.0);
        }
        // Against the exact mean from f - 1, as the tail takes it: at shapes past 1e30 y's rounding is the law's width.
        double deviation = Beta.FisherDeviation(f, dfn, dfd, y);
        bool small = Math.Min(a, b) < Beta.StirlingMinimumShape;
        double logBeta = small ? Beta.LogBeta(a, b) : double.NaN;
        double front = double.IsNaN(deviation) ? Beta.Front(a, b, y, oneMinusY, logBeta) : Beta.StirlingFront(a, b, y, oneMinusY, deviation);
        // Front's one exponential of 200 nats took 4.2e-14 at FisherSnedecorPdf(1e10, 0.002, 30), where 0.0019 took products.
        // Below LargeBothShapes 1/B is a double whatever the smaller shape below 10: Γ(10) 1000^-10 is 3.6e-25.
        bool longFront = front < LongExponent || (front < SmallShapeFront && Math.Min(a, b) < Beta.SmallShape);
        if (longFront && front >= FrontFloor && small && Math.Max(a, b) < LargeBothShapes)
        {
            return LongFrontFisher(f, dfn, dfd, (a, b), y, oneMinusY, logBeta);
        }
        return front >= FrontFloor ? front / f : UnderflowedFisher(f, dfn, dfd, (a, b), y, oneMinusY, deviation);
    }

    /// <summary>The F density where its front underflowed before the division by f.</summary>
    /// <remarks>
    /// Reading 0 for 3.3e-150 at f = 1e-300 and dfs 3 and 1, and for 9.2e-292 at 1e-33 and 20 and 20 (#1708): y^(a-1) by
    /// Math.Pow, where logs held the second to 2e-12 only; logs below SmallShape, and past LargeBothShapes, where log B
    /// cancelled against the powers and stepped 230-fold at shapes of 1e15. Out of line, so front / f keeps its time.
    /// </remarks>
    [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.NoInlining)]
    private static double UnderflowedFisher(
        double f, double dfn, double dfd, (double A, double B) shapes, double y, double oneMinusY, double deviation)
    {
        (double a, double b) = shapes;
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
        if (ExactHalf.Lifts(df))
        {
            // Of degree one in a shape this small, at the lifted df's exact half, dropped once: see ExactHalf.
            return ChiSquared(x, ExactHalf.Lifted(df)) * ExactHalf.Drop;
        }
        double prefactor = Gamma.Prefactor(a, x / 2.0);
        return x >= 2.0 || (prefactor >= LongExponent && x >= SubnormalHalving)
            ? prefactor / x
            : LongChiSquared(x, a, prefactor);
    }

    /// <summary>The chi-squared density below x = 2 where Prefactor(a, x/2) lost digits or underflowed before the division by x.</summary>
    /// <remarks>
    /// Its exponent's rounding grew, 2e-13 at 1e-62, x^a underflowed, ChiSquaredPdf(1e-70, 10) reading 0 for 1.3e-283, and a
    /// subnormal x halved to 0 (#1708): x^(a-1) by Math.Pow times exp(-x/2 - a ln 2 - log Γ(a)) for shapes in [0.1, 10); below,
    /// Gamma.UnhalvedLowerLead, x^a by Math.Pow from x unhalved; from a shape
    /// of 10, whose Stirling form holds what logs would, logs only where it underflows. Out of line, as UnderflowedFisher.
    /// </remarks>
    [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.NoInlining)]
    private static double LongChiSquared(double x, double a, double prefactor)
    {
        if (a < Gamma.SmallShape)
        {
            // a times the power can be subnormal just above the lift, where x brings it back: a/x first there, one rounding.
            double power = Gamma.UnhalvedLowerLead(a, x) * Math.Exp(-x / 2.0);
            double scaled = a * power;
            return scaled >= NearZero.SmallestNormal ? scaled / x : power * (a / x);
        }
        if (a < StirlingShape)
        {
            // At a chi-squared shape 1/Γ(a) as the tails take it, a product, not Lanczos' log Γ, good to about 1e-15.
            // a − 1 is exact from a shape of one half; below, it rounded and |log x| times that, 1.9e-14 at (1e-300, 0.3).
            double power = a < 0.5 ? Math.Pow(x, a) / x : Math.Pow(x, a - 1.0);
            return power * Gamma.ExpOverGamma(a, (-x / 2.0) - (a * Beta.Ln2));
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
