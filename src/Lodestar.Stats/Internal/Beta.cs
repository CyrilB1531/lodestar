namespace Lodestar.Stats.Internal;

/// <summary>The regularized incomplete beta, and the Student and Fisher tails it carries.</summary>
/// <remarks>
/// One continued fraction serves three families: a t-test's p-value is a Student tail, an ANOVA's is a
/// Fisher tail, and both are the incomplete beta under a change of variable. Written from the published
/// description and evaluated by modified Lentz (1976), its 106-bit form by the forward recurrence of convergents rescaled
/// every fourth term, except with one shape large and the other small,
/// where DiDonato and Morris's asymptotic expansion takes over (#841), and with both from 1e5, where
/// Temme's uniform one does (#1690); no reference implementation is transcribed (ADR 0002).
/// </remarks>
internal static class Beta
{
    // A guard, not the stopping rule: a = b = 1e8 at its mean takes 4,600 terms and 1e12 takes
    // 86,000, where the 300 this replaces left a = b = 1e8 wrong by 0.37 (#837).
    private const int MaxIterations = 1_000_000;

    // From here both log Gamma* are Stirling's, as in Gamma.Prefactor.
    internal const double StirlingMinimumShape = Gamma.StirlingMinimumShape;
    private const double Epsilon = 3e-16;
    private const double Tiny = 1e-300;

    // Where the large-shape expansion takes over from the fraction: why is in IsLargeShapeRegion.
    private const double LargeShapeMinimum = 1000.0;

    // Past this the fraction's products of two shapes near 1e154 overflow: ContinuedFraction scales them.
    private const double FractionScaleShape = 1e150;

    // From here the law is the normal one with its first term in 1/df, the next under 1e-36 relative: alone, the normal
    // tail read StudentSf(37, 1e20) 40 ulps off, once the normal tail held 3 ulps (#1756).
    internal const double NormalLimitDf = 1e20;

    // To here the tail is 1/2 - (df/2) asinh(t / sqrt df) to O(df^2 log^2), 1.3e-19 at most (FirstOrderSf has why).
    private const double FirstOrderDf = 1e-12;

    // log Gamma(1/2) = log sqrt(pi), a constant StudentLogScaledBeta took a Lanczos sum for on every call.
    private const double LogSqrtPi = 0.5723649429247001;

    // From here Student's tail at double.MaxValue, about MaxValue^-df, underflows: 744.4 / 709.8 is 1.0488.
    private const double FarRootMaximumDf = 1.05;

    // log(MaxValue^2), with which StudentQuantile bounds the tail at double.MaxValue before evaluating it.
    private const double LogSquaredMaxValue = 1419.565425786768;

    internal const double Ln2 = 0.6931471805599453;

    // The terms FisherTailFromLimit's first form drops are of order z = o y; below this they are below a double's precision.
    private const double LimitBound = 1e-17;

    // Past it in a log x the exponent's rounding passes 1e-14, the most the fraction beside it is off: below, Math.Pow
    // moved tails whose fraction is 50 ulps off by 20 either way.
    private const double LongFrontPower = 100.0;

    // log(2^-1022) and the largest argument Math.Exp keeps finite: SmallFront's two factors stay doubles between them.
    private const double LogSmallestNormal = -708.39;
    private const double MaxExpArgument = 709.78;

    // Below Gamma.SmallShape, where Gamma.LogGammaOnePlus holds, log(s B) is taken whole, by LogScaledBeta: the direct
    // fraction divides by a through it, and the reflected side is SmallShapeLowerTail rather than 1 - I.
    internal const double SmallShape = Gamma.SmallShape;

    // Below it with b ≥ 2a, I_x(a, b) is about two thirds or more, where the fraction lost 6 ulps at FisherSnedecorSf(1e-322,
    // 3.7, 1e-322); nearer, one less I near a half was 2.5 ulps off at FisherSnedecorCdf(1e-10, 1e-10, 1e-10).
    private const double TinyShape = 1e-8;

    // Below it TinyLogScaledBeta's series in s, nothing of order log s cancelling, where double-double's log B and log s,
    // each some −log s, left 1e-29 and read FisherSnedecorCdf(1, 1, 1e-19) 3e-12 off.
    private const double ExactLogScaledShape = 1e-13;

    // Below it in max(a, b)·(1 + |log x| + |log y|), I_x(a, b) is b/(a + b) + ab/(a + b)·log(x/y) to 2^-55, 2.35e-17 at worst
    // of 2,689 points against an 80-digit series; one form across x, where the fraction's sides fell 3 ulps apart (#1743).
    private const double SmallShapesReach = 7.5e-9;

    // SmallShapesReach over 1 + 2 log 2, the least 1 + |log x| + |log y|, at x = ½, rounded down: only below it in both shapes
    // can the first order hold across the mode, SmallShapesHold deciding by the smaller share, as unbalanced shapes fail it.
    private const double SmallShapesAtMode = 3.1e-9;

    // 1 + 2 log 2, 2.386, rounded up: 1 + |log x| + |log y| is at most |log(x/y)| plus it, and the margin covers the logs'
    // rounding at a guessed f.
    private const double LogsPastOdds = 2.39;

    // Twice SmallShapesAtMode: only below it in both dfs can the first order hold across the mode.
    internal const double FirstOrderAtModeDf = 2.0 * SmallShapesAtMode;

    // SmallShapesReach over 1491, rounded down: 1491 bounds 1 + |log x| + |log y| at every F point at dfs of 1e-11. The tail's
    // own test scales the reach by the smaller share, which unbalanced shapes fail, so there the fraction answers.
    internal const double SmallShapesEverywhere = 5.0e-12;

    // A guard, not the stopping rule: no point of the #841 grids took more than 22 terms.
    private const int MaxExpansionTerms = 64;

    // Each order of LogShapeRatio is at most 1/400 of the last inside the region, so ten reach 1e-26.
    private const int ShapeRatioOrders = 10;

    private static readonly double[] BernoulliRatios = BuildBernoulliRatios();
    private static readonly double[] ShapeRatioCoefficients = BuildShapeRatioCoefficients();

    internal static double RegularizedIncomplete(double a, double b, double x) =>
        RegularizedIncomplete(a, b, x, 1.0 - x);

    /// <summary>I_x(a, b), given y = 1 - x as the caller formed it more exactly than 1 - x would be.</summary>
    /// <remarks>
    /// Near x = 1 the rounding of x is a relative error of 1e-16 / y in y, which is what the tail
    /// depends on: a Student argument df / (df + t^2) at df = 2e8 and t = 1 leaves y 1e-8 off.
    /// </remarks>
    internal static double RegularizedIncomplete(double a, double b, double x, double y) =>
        RegularizedIncomplete(a, b, x, y, ShapeLogs.None);

    /// <summary>
    /// <see cref="RegularizedIncomplete(double, double, double, double)"/> with the shapes' logarithms already taken, or NaN
    /// to take them here: an inversion holding the shapes fixed takes them once (#1686, #1690's review).
    /// </summary>
    internal static double RegularizedIncomplete(double a, double b, double x, double y, in ShapeLogs logs) =>
        RegularizedIncomplete(a, b, x, y, logs, double.NaN);

    /// <summary>
    /// The same with <paramref name="deviation"/>, <c>x − a/(a + b)</c>, formed exactly by a caller whose x is rounded through
    /// that same sum, or NaN to take it from x: F's split rounds x as the mean rounds, and past shapes of 1e30 the exact mean
    /// sat thousands of standard deviations from it, FisherSnedecorSf(1, 1e100, 1e60) reading 0 (#1690's review).
    /// </summary>
    /// <remarks>By the double forms, not the 106-bit ones: the inversions' Newton steps need no more than 1e-15, and the 106
    /// bits took F's quantile past scipy's time, 32 us for 26 (#1745). They now include main's lack: the first order's share
    /// and I_½(a, a) = ½, so Binomial.Test(0, 1, 0.5) reads 1 where main read 0.9999999999999982.</remarks>
    internal static double RegularizedIncomplete(double a, double b, double x, double y, in ShapeLogs logs, double deviation) =>
        RegularizedValue(a, b, new DoubleDouble(x, 0.0), new DoubleDouble(y, 0.0), logs, deviation, exact: false).Rounded;

    /// <summary>I_x(a, b) before its last rounding, at a point given to some 106 bits, x and y summing to one: F's and
    /// Student's split, whose rounding to a double is b ε/2 of the tail, hundreds of ulps at shapes in the hundreds (#1745),
    /// and unrounded for the quantiles' Newton steps and tests, which a rounded tail flat over hundreds of ulps of its
    /// statistic near one half told nothing (#1745's Review A).</summary>
    internal static ExactTail ExactRegularizedIncomplete(double a, double b, DoubleDouble x, DoubleDouble y, in ShapeLogs logs, double deviation) =>
        RegularizedValue(a, b, x, y, logs, deviation, exact: true);

    /// <summary>Whether a point is below one to 106 bits, its high part one where its low part is negative.</summary>
    private static bool BelowOne(DoubleDouble point) => point.High < 1.0 || (point.High <= 1.0 && point.Low < 0.0);

    /// <summary><paramref name="tail"/> within [0, 1], an end where it leaves them.</summary>
    /// <remarks>Judged on both parts: on the rounding, a tail rounding to one but below it lost its low part, read then as
    /// a double form's, and StudentIsf(0.49999999999999994, 1e-11) searched 123 tails, 75 µs to scipy's 27 (#1745's Review A).</remarks>
    private static ExactTail Clamped(ExactTail tail)
    {
        DoubleDouble value = tail.Value;
        if (value.High < 0.0 || (value.High <= 0.0 && value.Low <= 0.0))
        {
            return new ExactTail(new DoubleDouble(0.0, 0.0), tail.InDouble);
        }
        return value.High > 1.0 || (value.High >= 1.0 && value.Low >= 0.0) ? new ExactTail(new DoubleDouble(1.0, 0.0), tail.InDouble) : tail;
    }

    /// <summary>I_x(a, b) before its last rounding where <paramref name="exact"/> holds.</summary>
    private static ExactTail RegularizedValue(double a, double b, DoubleDouble x, DoubleDouble y, in ShapeLogs logs, double deviation, bool exact)
    {
        // Checked and its ends read on the rounded point, one set of checks for both forms.
        double rounded = x.High;
        double roundedY = y.High;
        if (double.IsNaN(a) || a <= 0.0)
        {
            throw new ArgumentOutOfRangeException(nameof(a), a, "The first shape must be positive.");
        }
        if (double.IsNaN(b) || b <= 0.0)
        {
            throw new ArgumentOutOfRangeException(nameof(b), b, "The second shape must be positive.");
        }
        if (double.IsNaN(rounded) || rounded < 0.0 || rounded > 1.0)
        {
            throw new ArgumentOutOfRangeException(nameof(x), rounded, "The argument must lie in [0, 1].");
        }

        // Exact sentinels, not a rounding-error-prone comparison: x is validated
        // to [0, 1] above, and 0/1 are the interval's own closed endpoints. An x that
        // rounded to one while y still holds digits is not the endpoint (#1158's review).
#pragma warning disable S1244
        if (rounded == 0.0 || (rounded == 1.0 && roundedY == 0.0))
#pragma warning restore S1244
        {
            return new ExactTail(new DoubleDouble(rounded, 0.0), inDouble: false);
        }

        // A 106-bit point one less a tiny y is no end but a large shape's deep tail: StudentIsf(0.49999999, 2000) lost 2e7 ulps.
        if (exact && !BelowOne(x))
        {
            (x, y) = (ExactSide(rounded, roundedY), ExactSide(roundedY, rounded));
        }
        // Clamped, not refused, where rounding left [0, 1] (#1690's review); one call, Unclamped being inlined: three copies
        // cost F's tail at dfs (3e6, 1e7) 5%. The double forms take the point as given, as main's did.
        return Clamped(Unclamped(a, b, x, y, logs, deviation, exact));
    }

    /// <summary>Whether both shapes lie below SmallShapesAtMode, the one gate of the first order's three readers.</summary>
    private static bool BothBelowMode(double a, double b) => a < SmallShapesAtMode && b < SmallShapesAtMode;

    /// <summary>Whether the first order holds at the point whose logs are <paramref name="logX"/> and <paramref name="logY"/>.</summary>
    private static bool SmallShapesHold(double a, double b, double logX, double logY) =>
        Math.Max(a, b) * (1.0 - logX - logY) <= RelativeReach(a, b);

    /// <summary>SmallShapesReach scaled to the smaller share, <c>√(min(a, b)/(a + b))</c>: its <c>2⁻⁵⁵</c> is absolute, and
    /// against a share of 0.016 it was 8 ulps, F's upper tail rising 2 where the fraction took over at dfs (1e-10, 6e-9)
    /// (#1745's Review A).</summary>
    private static double RelativeReach(double a, double b) => SmallShapesReach * Math.Sqrt(Math.Min(a, b) / (a + b));

    /// <summary>I_x(a, b) to first order in both shapes where <see cref="SmallShapesReach"/> bounds the rest, else NaN: the
    /// two small-shape sides of the fraction take it, so I_x is one form across the mode.</summary>
    private static DoubleDouble SmallShapesTail(double a, double b, double x, double y)
    {
        DoubleDouble none = new(double.NaN, 0.0);
        if (!BothBelowMode(a, b))
        {
            return none;
        }
        // The smaller side's binary exponent bounds its log from below, so most points the first order cannot reach are
        // told without one: the two logs on every such tail cost FisherSnedecorQuantile(0.3, 5e-9, 1e-10) 15%.
        long smallerBits = BitConverter.DoubleToInt64Bits(Math.Min(x, y));
        double belowLog = (1022 - ((smallerBits >> 52) & 0x7FF)) * Ln2;
        if (Math.Max(a, b) * (1.0 + belowLog) > SmallShapesReach)
        {
            return none;
        }
        double logX = LogComplement(y, x);
        double logY = LogComplement(x, y);
        return SmallShapesHold(a, b, logX, logY) ? SmallShapesShare(a, b, logX - logY) : none;
    }

    /// <summary>I_x(a, b) by one of the two expansions or the fraction, before RegularizedIncomplete's clamp.</summary>
    // Inlined, as ByFraction: as calls the two cost an F tail 5% at (10, 30), 98 against 103 ns (Stopwatch A/B/A/B).
    [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
    private static ExactTail Unclamped(double a, double b, DoubleDouble x, DoubleDouble y, in ShapeLogs logs, double deviation, bool exact)
    {
        // S1244: I_½(a, a) is one half by symmetry, which the double fraction past a harmonic shape of 1000 missed by 1e-14,
        // but not with y off one half, 88 ulps at shapes 1e4 and y an ulp below it, nor with a deviation off the mean, where F's
        // tail at 1 − ε and dfs 1e308 read one half for main's 0 (#1745's Review A).
#pragma warning disable S1244
        if (a == b && x.High == 0.5 && x.Low == 0.0 && y.High == 0.5 && y.Low == 0.0 && !(Math.Abs(deviation) > 0.0))
#pragma warning restore S1244
        {
            return new ExactTail(new DoubleDouble(0.5, 0.0), inDouble: false);
        }
        // Before the large-shape expansion where both hold: against a 60-digit fraction it kept 2e-13 where that one
        // drifted to 8e-12, at shapes (1e7, 1e14) (#1690's review).
        if (UniformBeta.Holds(a, b))
        {
            // Past ExactExponentShapes the point's 1e-32 of r − 1 times the shapes is no longer under an ulp of the exponent:
            // at dfs (3e235, 3e139) it read F's tail at f = 1 as one for one half. The exact deviation's double form there.
            if (!(exact && a + b <= ExactExponentShapes))
            {
                return ExactTail.OfDouble(UniformBeta.Regularized(a, b, x.High, y.High, deviation));
            }
            DoubleDouble uniform = UniformBeta.ExactRegularized(a, b, x.High, y.High, deviation, -ExactShapeExponent(a, b, x, y), out bool uniformInDouble);
            return new ExactTail(uniform, uniformInDouble);
        }
        // S2234: the swapped (b, a, y, x) is the reflection I_x(a, b) = 1 - I_y(b, a), as below.
#pragma warning disable S2234
        if (IsLargeShapeRegion(a, b, x.High))
        {
            return exact && y.High > 0.0 && BelowOne(x)
                ? ExactLargeShapeExpansion(a, b, x, y, reflected: false)
                : ExactTail.OfDouble(LargeShapeExpansion(a, b, x.High, y.High, reflected: false));
        }
        if (IsLargeShapeRegion(b, a, y.High))
        {
            return exact && x.High > 0.0 && BelowOne(y)
                ? ExactLargeShapeExpansion(b, a, y, x, reflected: true)
                : ExactTail.OfDouble(LargeShapeExpansion(b, a, y.High, x.High, reflected: true));
        }
#pragma warning restore S2234
        return ByFraction(a, b, x, y, logs, exact);
    }

    /// <summary>I_x(a, b) by the continued fraction, on whichever side of the mode it converges.</summary>
    /// <remarks>
    /// The fraction converges quickly only on the side of the distribution's mode; past it the reflection is the
    /// fast branch, not a fallback. Below SmallShape the direct side divides by a inside log(a B), and the
    /// reflected side is SmallShapeLowerTail.
    /// </remarks>
    [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
    private static ExactTail ByFraction(double a, double b, DoubleDouble x, DoubleDouble y, in ShapeLogs logs, bool exact)
    {
        // S2234: the swapped (b, a) is the reflection identity I_x(a,b) = 1 - I_{1-x}(b,a), not a copy-paste slip.
#pragma warning disable S2234
        // Where the fraction stays a double its rounding, thousands of ulps past a harmonic shape of 1000, swamps a 106-bit
        // front, which cost 3-5 us a tail: F's quantile at dfs (165557, 8597663) took 64 us, scipy's 29 (#1745's Review A).
        bool exactFraction = exact && x.High > 0.0 && y.High > 0.0 && FractionIsExact(a, b);
        if (x.High >= (a + 1.0) / (a + b + 2.0))
        {
            if (b < SmallShape)
            {
                return SmallShapeLowerTail(a, b, x, y, logs.ScaledSecond, firstOrder: true, exact);
            }
            return exactFraction
                ? new ExactTail(new DoubleDouble(1.0, 0.0) - (ExactFront(a, b, x, y) / (b * ExactFraction(b, a, y))), inDouble: false)
                : ExactTail.OfDouble(1.0 - (SmallFrontOrStirling(a, b, x.High, y.High, logs.OfBeta, exact) / (b * ContinuedFraction(b, a, y.High))));
        }
        if (a < SmallShape)
        {
            return SmallFirstShape(a, b, x, y, logs, exact);
        }
        if (b < SmallShape)
        {
            return SmallSecondShape(a, b, x, y, logs.ScaledSecond, exact);
        }
        return exactFraction
            ? new ExactTail(ExactFront(a, b, x, y, out int exponent) / (a * ExactFraction(a, b, x)), inDouble: false, exponent)
            : ExactTail.OfDouble(SmallFrontOrStirling(a, b, x.High, y.High, logs.OfBeta, exact) / (a * ContinuedFraction(a, b, x.High)));
#pragma warning restore S2234
    }

    /// <summary><see cref="FastFront"/> in double-double, its exponent and exponential to some
    /// 106 bits: below shapes of 10 from log B, past them from Stirling's exponent and its corrections.</summary>
    internal static DoubleDouble ExactFront(double a, double b, DoubleDouble x, DoubleDouble y)
    {
        DoubleDouble front = ExactFront(a, b, x, y, out int exponent);
        return exponent == 0 ? front : DoubleDouble.ScaledOnce(front, exponent);
    }

    /// <summary>The F density from its split to 106 bits, <c>x^a y^b / (B(a, b) f)</c>, one exponential rounded once.</summary>
    /// <remarks>log f and the Stirling scale's log inside the exponent: the front alone, lifted 2^600, underflowed below
    /// e^-1160, and FisherSnedecorPdf(1e-300, 3.7, 1) read 0 for 8.1e-255 (#1746's Review A).</remarks>
    internal static double ExactFisherDensity(double a, double b, DoubleDouble x, DoubleDouble y, double f)
    {
        bool stirling = a >= StirlingMinimumShape && b >= StirlingMinimumShape;
        DoubleDouble scale = default;
        DoubleDouble power;
        if (stirling)
        {
            (DoubleDouble corrections, DoubleDouble constantScale) = StirlingConstants(a, b);
            (power, scale) = (ExactShapeExponent(a, b, x, y) + corrections, constantScale);
        }
        else
        {
            power = ExactSidesLogPower(a, b, x, y) - DoubleDouble.LogBeta(a, b);
        }
        // The two logarithms only where the front leaves the normal doubles: each cost the density a tenth of a microsecond.
        if (Math.Abs(power.High) < FrontLogReach && f > 1.0 / PointReach && f < PointReach)
        {
            DoubleDouble front = DoubleDouble.ExpExact(power);
            return ((stirling ? scale * front : front) / new DoubleDouble(f, 0.0)).Rounded;
        }
        DoubleDouble lifted = stirling ? power + DoubleDouble.Log(scale) : power;
        return DoubleDouble.ExpExact(lifted - DoubleDouble.Log(f)).Rounded;
    }

    // Within them e^power, under 1e196, and its quotient by f stay normal; past them the logarithms take both.
    private const double FrontLogReach = 450.0;
    private const double PointReach = 1e90;

    /// <summary><see cref="ExactFront(double, double, DoubleDouble, DoubleDouble)"/> times 2^-<paramref name="exponent"/>,
    /// lifted where TailExponent lifts its exponential.</summary>
    private static DoubleDouble ExactFront(double a, double b, DoubleDouble x, DoubleDouble y, out int exponent)
    {
        if (a < StirlingMinimumShape || b < StirlingMinimumShape)
        {
            DoubleDouble power = ExactSidesLogPower(a, b, x, y) - DoubleDouble.LogBeta(a, b);
            exponent = TailExponent(power);
            return DoubleDouble.ExpExact(power, -exponent);
        }
        return ExactStirlingFront(a, b, x, y, out exponent);
    }

    /// <summary><see cref="StirlingFront"/> in double-double: <c>a L(x/p) + b L(y/q)</c>, L(r) = log r − (r − 1), against
    /// the exact mean p = a/(a + b).</summary>
    private static DoubleDouble ExactStirlingFront(double a, double b, DoubleDouble x, DoubleDouble y, out int exponent)
    {
        // The corrections and √(a b / (2π (a + b))) to 106 bits too: in double they left I_½(d/2, d/2) an ulp under one
        // half at d = 2000, and the median of equal dfs off one (#1745's Review A).
        (DoubleDouble corrections, DoubleDouble scale) = StirlingConstants(a, b);
        DoubleDouble power = ExactShapeExponent(a, b, x, y) + corrections;
        exponent = TailExponent(power);
        return scale * DoubleDouble.ExpExact(power, -exponent);
    }

    /// <summary>The Stirling front's shape-only parts, the corrections and the scale, the last pair's on this thread, as an
    /// inversion or a search holds its shapes: they took a microsecond of a two-microsecond front.</summary>
    private static (DoubleDouble Corrections, DoubleDouble Scale) StirlingConstants(double a, double b)
    {
        // Symmetric in the shapes, so keyed on the ordered pair, as LogBeta: the two tails' sides swap them.
        (a, b) = (Math.Min(a, b), Math.Max(a, b));
        if (ShapeKey.Matches(a, b, stirlingA, stirlingB))
        {
            return (stirlingCorrections, stirlingScale);
        }
        DoubleDouble sum = DoubleDouble.Sum(a, b);
        DoubleDouble corrections = DoubleDouble.StirlingSeries(sum)
            - DoubleDouble.StirlingSeries(new DoubleDouble(a, 0.0)) - DoubleDouble.StirlingSeries(new DoubleDouble(b, 0.0));
        // a/(a + b) before b, as main's StirlingFront: the product a b overflowed past 1.8e308, and F's tail read NaN.
        DoubleDouble scale = DoubleDouble.Sqrt(b * (new DoubleDouble(a, 0.0) / sum) / TwoPi);
        (stirlingA, stirlingB, stirlingCorrections, stirlingScale) = (a, b, corrections, scale);
        return (corrections, scale);
    }

    [ThreadStatic]
    private static double stirlingA;

    [ThreadStatic]
    private static double stirlingB;

    [ThreadStatic]
    private static DoubleDouble stirlingCorrections;

    [ThreadStatic]
    private static DoubleDouble stirlingScale;

    /// <summary><see cref="ShapeExponent"/> in double-double, from a point given to some 106 bits.</summary>
    private static DoubleDouble ExactShapeExponent(double a, double b, DoubleDouble x, DoubleDouble y)
    {
        DoubleDouble sum = DoubleDouble.Sum(a, b);
        return (a * ExactLogRatioExcess(x * sum / new DoubleDouble(a, 0.0)))
            + (b * ExactLogRatioExcess(y * sum / new DoubleDouble(b, 0.0)));
    }

    /// <summary><c>log r − (r − 1)</c> in double-double.</summary>
    /// <remarks>Within LogExcessSeriesReach of one by its series in d = r − 1, −d²/2 + d³/3 − ⋯, relative to itself: the
    /// log's low part taken to first order left nothing of −d²/2 there, and FisherSnedecorCdf(1 − ε, 2e5, 2e5) read one
    /// half, 180 ulps off (#1745's Review A). Past it the difference loses under three of the log's digits.</remarks>
    private static DoubleDouble ExactLogRatioExcess(DoubleDouble ratio)
    {
        DoubleDouble excess = ratio - new DoubleDouble(1.0, 0.0);
        // A NaN takes the log, not the series, whose test it never met: a million terms, 16 ms (#1745's Review A).
        if (!(Math.Abs(excess.High) <= LogExcessSeriesReach))
        {
            return DoubleDouble.Log(ratio) - excess;
        }
        // Each power (−1)^(k+1) d^k, from −d² at k = 2.
        var terms = new AlternatingLogTerms(excess);
        return SummedSeries(0.5 * terms.Power, 3, ref terms);
    }

    // Below it ExactLogRatioExcess sums its series, under ten terms to ExactEpsilon of itself.
    private const double LogExcessSeriesReach = 1.0 / 1024.0;

    /// <summary>I_x(a, b) on the fraction's direct side at a below SmallShape.</summary>
    // Out of line, as SmallSecondShape, so the fraction at ordinary shapes keeps ByFraction's size.
    [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.NoInlining)]
    private static ExactTail SmallFirstShape(double a, double b, DoubleDouble x, DoubleDouble y, in ShapeLogs logs, bool exact)
    {
        DoubleDouble small = SmallShapesTail(a, b, x.High, y.High);
        if (!double.IsNaN(small.High))
        {
            return new ExactTail(small, inDouble: true);
        }
        // S2234: the reflection I_x(a, b) = 1 - I_y(b, a), and b B(b, a), the shapes swapped on purpose.
#pragma warning disable S2234
        if (a < TinyShape && b >= 2.0 * a)
        {
            return SmallShapeLowerTail(b, a, y, x, logs.ScaledFirst, firstOrder: false, exact).Complement();
        }
        // b below a: a B(a, b) is (a/b) b B(b, a), the ratio outside the exponential, whose log(a/b) of some 200 nats
        // carried 16 ulps into FisherSnedecorCdf(1e-300, 2^-700, 2^-1021) (#1729's Review A).
        if (b < a)
        {
            return SmallSecondShape(a, b, x, y, logs.ScaledSecond, exact);
        }
        return exact
            ? ExactScaledFront(a, b, x, y, ExactLogScaledBeta(a, b))
            : ExactTail.OfDouble(ScaledFront(a, b, x.High, y.High, CachedOrScaled(logs.ScaledFirst, a, b)) / ContinuedFraction(a, b, x.High));
#pragma warning restore S2234
    }

    /// <summary>I_x(a, b) by the fraction at b below SmallShape beside a past it.</summary>
    /// <remarks>
    /// 1/B as b e^(−log b B), and x^a by Math.Pow up to x = 1/2 past a log x of Gamma.LongPower, else in the exponential:
    /// e^(a log x − log B) read FisherSnedecorCdf(5e-324, 0.5, 1e-300) 1.6e-14 off, and (1e-300, 1, 1e-10) 4.4e-14. A
    /// subnormal b times the front, at a b near 2^-1022, is taken as one quotient: it dropped bits the division by a
    /// brought back, FisherSnedecorCdf(5e-324, 0.15, 4.450147717014403e-308) 7 granules off.
    /// </remarks>
    // Out of line, as SmallFirstShape, so the fraction at ordinary shapes keeps ByFraction's size.
    [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.NoInlining)]
    private static ExactTail SmallSecondShape(double a, double b, DoubleDouble exactX, DoubleDouble exactY, double scaledSecond, bool exact)
    {
        if (exact && exactX.High > 0.0 && exactY.High > 0.0)
        {
            // b times the front 2^600 up, exactly, and the tail down once: at a b near 2^-1022 the product fell below the
            // normal doubles where the tail did not, and F's lower tail fell 2,142 times in 20,000 ulps at dfs (3.7, 1e-307).
            DoubleDouble power = ExactSidesLogPower(a, b, exactX, exactY) - ExactLogScaledBeta(b, a);
            int exponent = TailExponent(power);
            DoubleDouble liftedFront = (b * QuotientLift) * DoubleDouble.ExpExact(power, -exponent);
            DoubleDouble liftedTail = liftedFront / (a * FractionFor(a, b, exactX));
            if (liftedTail.High > 0.0 && liftedTail.High < double.PositiveInfinity)
            {
                return new ExactTail(liftedTail, inDouble: false, exponent - QuotientLiftExponent);
            }
        }
        double x = exactX.High;
        double y = exactY.High;
        // Main's form, onto the subnormals once: the exact one's exponential rounded there and then again (#1745's Review A).
        double front = ScaledFront(a, b, x, y, CachedOrScaled(scaledSecond, b, a));
        double fraction = ContinuedFraction(a, b, x);
        double numerator = b * front;
        return ExactTail.OfDouble(numerator >= NearZero.SmallestNormal
            ? numerator / (a * fraction)
            : PowerOfTwo.Quotient(stackalloc double[] { b, front }, stackalloc double[] { a, fraction }, 0));
    }

    /// <summary>Whether <see cref="LargeShapeExpansion"/> holds I_x(a, b), a being the large shape.</summary>
    /// <remarks>
    /// Its terms fall by about b^3 / (24 a^2) near the mean and (log(x) / 2 pi)^2 away from it, so
    /// b^3 &lt;= a^2 and x &gt;= 1/2 bound both. From a = 1000 the fraction's log-gamma prefactor and
    /// cancellation cost 1e-12, growing to 3e-6 at a = 1e8 (#841), where the expansion held 3e-13
    /// against exact sums and scipy and was at most 6% slower than the fraction. Below it the fraction
    /// is kept: its error stays under 5e-12, and the expansion was up to 90% slower at a = 100.
    /// </remarks>
    private static bool IsLargeShapeRegion(double a, double b, double x) =>
        a >= LargeShapeMinimum && b * b * b <= a * a && x >= 0.5;

    /// <summary>The logarithms an inversion holding two shapes fixed takes once, each NaN where it is not taken.</summary>
    /// <remarks>
    /// <c>log(s B)</c> in the form the uncached path takes, LogScaledBeta's, so an inversion solves the tail the forward
    /// functions answer; Student's, whose other shape is always ½, is StudentLogScaledBeta's on both paths. Three fields:
    /// a fourth and fifth, holding both forms, stopped the JIT promoting the struct and cost an F quantile 8% (#1690).
    /// </remarks>
    internal readonly struct ShapeLogs(double logBeta, double scaledFirst, double scaledSecond)
    {
        internal static ShapeLogs None { get; } = new(double.NaN, double.NaN, double.NaN);

        /// <summary><c>log B(a, b)</c>.</summary>
        internal double OfBeta { get; } = logBeta;

        /// <summary><c>log(a B(a, b))</c>, below SmallShape.</summary>
        internal double ScaledFirst { get; } = scaledFirst;

        /// <summary><c>log(b B(a, b))</c>, below SmallShape.</summary>
        internal double ScaledSecond { get; } = scaledSecond;

        /// <summary>The same logarithms for the shapes exchanged, <c>(b, a)</c>.</summary>
        internal ShapeLogs Swapped => new(OfBeta, ScaledSecond, ScaledFirst);

        /// <summary>Every logarithm the shapes call for, taken once; none where both reach UniformBeta, which reads none.</summary>
        internal static ShapeLogs Of(double a, double b)
        {
            if (UniformBeta.Holds(a, b))
            {
                return None;
            }
            return new(
                Beta.LogBeta(a, b),
                a < SmallShape ? LogScaledBeta(a, b) : double.NaN,
                b < SmallShape ? LogScaledBeta(b, a) : double.NaN);
        }
    }

    /// <summary>Whether the large-shape expansions take Q(b, w) rather than P: where P passes one half. Both forms decide
    /// by this one rule, so the Newton steps' double and the search's exact tail subtract the same side.</summary>
    /// <remarks>P(b, w) is w^b / Γ(1 + b) to first order below SmallShape, near one far below w = b: P(4e-52, 4e-94) is
    /// 1 - 9e-50, and 1 - direct read I_x 0 for 9e-50 at shapes (4e106, 4e-52) (#1690's review).</remarks>
    private static bool ExpansionTakesQ(double b, double w) =>
        b < SmallShape ? (b * Math.Log(w)) - Gamma.LogGammaOnePlus(b) > -Ln2 : w >= b;

    /// <summary>The k-th factor (b + 2k − 2)(b + 2k − 1)/N² of the expansions' Pochhammer product.</summary>
    /// <remarks>From b and the integers, not from the running shape, which holds b to an ulp of 2k: the first factor, b
    /// itself, took up to 2.2e-16/b of it and read zero below 1.1e-16, FisherSnedecorSf(2e20, 1e-20, 2000) 1.4e-7 off.</remarks>
    private static double PochhammerStep(double b, int k, double inverseN2)
    {
        double offset = 2.0 * k;
        return (b + (offset - 2.0)) * (b + (offset - 1.0)) * inverseN2;
    }

    /// <summary>I_x(a, b) for a large and b small, or 1 - I_x(a, b) when <paramref name="reflected"/>.</summary>
    /// <remarks>
    /// DiDonato and Morris's BGRAT (1992), in Temme's form (Special Functions, 1996, 11.3.3): with
    /// x = e^-z and N = a + (b - 1)/2, (1 - e^-u)^(b-1) e^-au = e^-Nu u^(b-1) (sinh(u/2)/(u/2))^(b-1),
    /// whose even series integrates term by term into Gamma(a + b)/(Gamma(a) N^b) times the sum of
    /// c_k (b)_2k N^-2k R(b + 2k, Nz). R is Q, for I, or P, for its complement, whichever is under
    /// about a half at k = 0, so that R runs by positive steps or loses only what its term shrinks.
    /// z = -log(1 - y) takes its last digits from y, the more exact of the pair near x = 1.
    /// </remarks>
    private static double LargeShapeExpansion(double a, double b, double x, double y, bool reflected)
    {
        double n = a + (0.5 * (b - 1.0));

        // -log(1 - y) as -log(x) less the gap between the two complements, exact by Sterbenz at x >= 1/2.
        double z = -Math.Log(x) - ((1.0 - x - y) / x);
        double w = n * z;

        // log(Gamma(a + b) / (Gamma(a) N^b)), about -b^3 / (24 N^2): see LogShapeRatio.
        double logRatio = LogShapeRatio(n, 0.5 * b);

        bool upper = ExpansionTakesQ(b, w);
        double r = Gamma.RegularizedWithPrefactor(b, w, upper, out double step);
        step /= b;
        double shape = b;

        // c_k from k c_k = (b - 1)/2 sum_j B_2j/(2j)! c_(k-j), the exponential of the log series.
        Span<double> coefficients = stackalloc double[MaxExpansionTerms];
        coefficients[0] = 1.0;
        double halfExponent = 0.5 * (b - 1.0);
        double inverseN2 = 1.0 / (n * n);
        double pochhammer = 1.0;
        double sum = r;
        for (int k = 1; k < MaxExpansionTerms; k++)
        {
            for (int half = 0; half < 2; half++)
            {
                r = upper ? r + step : r - step;
                shape += 1.0;
                step *= w / shape;
            }

            coefficients[k] = ExpansionCoefficient(coefficients, k, halfExponent);

            pochhammer *= PochhammerStep(b, k, inverseN2);
            double term = coefficients[k] * pochhammer * r;
            sum += term;
            if (Math.Abs(term) <= Epsilon * Math.Abs(sum))
            {
                break;
            }
        }

        double direct = Math.Exp(logRatio) * sum;
        return upper == reflected ? 1.0 - direct : direct;
    }

    /// <summary>The large-shape expansion's c_k from the c_j below it: k c_k = (b − 1)/2 Σ B_2j/(2j)! c_(k−j), the
    /// exponential of the log series; one copy for the double and the double-double expansions.</summary>
    private static double ExpansionCoefficient(Span<double> coefficients, int k, double halfExponent)
    {
        double convolution = 0.0;
        for (int j = 1; j <= k; j++)
        {
            convolution += BernoulliRatios[j - 1] * coefficients[k - j];
        }
        return halfExponent * convolution / k;
    }

    /// <summary>log(Gamma(N + 1/2 + s) / (Gamma(N + 1/2 - s) N^(2s))), for 2s &lt;= N^(2/3).</summary>
    /// <remarks>
    /// With h = 1/2 + s, the two log-gammas' expansions in Bernoulli polynomials (DLMF 5.11.8) cancel
    /// at even orders, since B_n(1 - h) = (-1)^n B_n(h), leaving minus the sum over m of
    /// B_(2m+1)(h) / (m (2m + 1) N^2m). Each term is about (s / N)^2, at most 1/400, of the last,
    /// so no log-gamma of order N log N is formed and none cancels.
    /// </remarks>
    private static double LogShapeRatio(double n, double s)
    {
        double s2 = s * s;
        double inverseN2 = 1.0 / (n * n);
        double power = inverseN2;
        double sum = 0.0;
        int row = 0;
        for (int m = 1; m <= ShapeRatioOrders; m++)
        {
            double polynomial = 0.0;
            for (int i = 0; i <= m; i++)
            {
                polynomial = (polynomial * s2) + ShapeRatioCoefficients[row + i];
            }
            row += m + 1;

            double term = s * polynomial * power;
            sum -= term;
            if (Math.Abs(term) <= Epsilon * Math.Abs(sum))
            {
                break;
            }

            power *= inverseN2;
        }

        return sum;
    }

    // Row m holds B_(2m+1)(1/2 + s) / (m (2m + 1)) in s^2, highest power first, from
    // B_n(1/2 + s) = sum over even k of C(n, k) (2^(1-k) - 1) B_k s^(n-k).
    private static double[] BuildShapeRatioCoefficients()
    {
        double[] coefficients = new double[(ShapeRatioOrders * (ShapeRatioOrders + 3)) / 2];
        int row = 0;
        for (int m = 1; m <= ShapeRatioOrders; m++)
        {
            int order = (2 * m) + 1;
            double binomial = 1.0;
            double factorial = 1.0;
            for (int k = 0; k <= order; k++)
            {
                if (k > 0)
                {
                    binomial = binomial * (order - k + 1) / k;
                    factorial *= k;
                }
                if (k % 2 == 1)
                {
                    continue;
                }

                double bernoulli = k == 0 ? 1.0 : BernoulliRatios[(k / 2) - 1] * factorial;
                coefficients[row + (k / 2)] =
                    binomial * (Math.Pow(2.0, 1 - k) - 1.0) * bernoulli / (m * order);
            }
            row += m + 1;
        }

        return coefficients;
    }

    // B_2j / (2j)!, exact to j = 10. Past it zeta(2j) is 1 within 1e-6, so each is the last over
    // -(2 pi)^2, on coefficients already below 1e-16.
    private static double[] BuildBernoulliRatios()
    {
        double[] exact =
        [
            1.0 / 12.0,
            -1.0 / 720.0,
            1.0 / 30240.0,
            -1.0 / 1209600.0,
            1.0 / 47900160.0,
            -691.0 / 1307674368000.0,
            1.0 / 74724249600.0,
            -3617.0 / 10670622842880000.0,
            43867.0 / 5109094217170944000.0,
            -174611.0 / 802857662698291200000.0,
        ];
        double[] ratios = new double[MaxExpansionTerms];
        exact.CopyTo(ratios, 0);
        for (int j = exact.Length; j < ratios.Length; j++)
        {
            ratios[j] = -ratios[j - 1] / (4.0 * Math.PI * Math.PI);
        }

        return ratios;
    }

    /// <summary><c>x^a y^b / B(a, b)</c>, the factor both branches of the fraction share, by the double forms alone,
    /// <paramref name="logBeta"/> being the cached <c>log B</c>, or NaN.</summary>
    /// <remarks>
    /// Past both shapes of 10, ShapeExponent's a log(x / p) + b log(y / q) against the mean p = a / (a + b), whose linear
    /// terms cancel, so only log(1 + t) - t is formed, and Stirling leaves sqrt(x0 b / 2 pi) Gamma*(a + b) / (Gamma*(a)
    /// Gamma*(b)), its own B, which the cached one equals. The Lanczos form subtracts log-gammas of 1.7e9 at a = b = 1e8.
    /// d = x - x0 is taken from the smaller of x and 1 - x, so the two t share one d instead of two rounded ones;
    /// <see cref="ScaledFront"/> takes <c>a B</c>.
    /// </remarks>
    internal static double FastFront(double a, double b, double x, double y, double logBeta) =>
        SmallFrontOrStirling(a, b, x, y, logBeta, exact: false);

    private static double SmallFrontOrStirling(double a, double b, double x, double y, double logBeta, bool exact) =>
        a < StirlingMinimumShape || b < StirlingMinimumShape
            ? SmallFront(a, b, x, y, logBeta, exact)
            : StirlingFront(a, b, x, y, double.NaN);

    /// <summary><see cref="FastFront"/> below a shape of 10: the smaller side's power by
    /// Math.Pow past a log of LongFrontPower, where e^(a log x) took some 700 nats' rounding and FisherSnedecorCdf(2.5e-305,
    /// 2, 5) read 5.5e-14 off (#1729's Review A).</summary>
    private static double SmallFront(double a, double b, double x, double y, double cachedLogBeta, bool exact)
    {
        // Exact here only past FractionScaleShape, ByFraction's one exact caller whose fraction stays a double: the
        // front from the rounded point in double-double, rounded before that fraction's division.
        if (exact && x > 0.0 && y > 0.0)
        {
            return DoubleDouble.ExpExact(ExactSidesLogPower(a, b, ExactSide(x, y), ExactSide(y, x)) - DoubleDouble.LogBeta(a, b)).Rounded;
        }
        double logBeta = CachedOrLogBeta(cachedLogBeta, a, b);
        double lower = LogPower(a, x, y);
        double upper = LogPower(b, y, x);
        double rest = upper - logBeta;
        // Only where x^a is normal and e^rest finite: past them 0 times +∞ read FisherSnedecorSf(1e300, 3.7, 1e300) NaN.
        if (x <= y && lower < -LongFrontPower && lower > LogSmallestNormal && rest < MaxExpArgument)
        {
            return Math.Pow(x, a) * Math.Exp(rest);
        }
        return Math.Exp((lower + upper) - logBeta);
    }

    /// <summary><c>x^a y^b / (a B(a, b)) / CF</c> in double-double where x and y are positive, else by ScaledFront.</summary>
    private static ExactTail ExactScaledFront(double a, double b, DoubleDouble x, DoubleDouble y, DoubleDouble logScaledBeta)
    {
        if (x.High > 0.0 && y.High > 0.0)
        {
            DoubleDouble power = ExactSidesLogPower(a, b, x, y) - logScaledBeta;
            int exponent = TailExponent(power);
            return new ExactTail(DoubleDouble.ExpExact(power, -exponent) / FractionFor(a, b, x), inDouble: false, exponent);
        }
        return ExactTail.OfDouble(ScaledFront(a, b, x.High, y.High, logScaledBeta.High) / ContinuedFraction(a, b, x.High));
    }

    /// <summary><c>log(s B(s, other))</c> in double-double from ExactLogScaledShape up, its parts' some 106 bits of about
    /// −log s each cancelling to under 1e-29; below it TinyLogScaledBeta. Rounded to a double up to 1e-8, its s ε entered
    /// the fraction's two sides with opposite signs, and F's tail at dfs (2e-8, 1e-8) fell 2 ulps where they meet (#1745's
    /// Review A).</summary>
    private static DoubleDouble ExactLogScaledBeta(double s, double other) =>
        s >= ExactLogScaledShape
            ? DoubleDouble.LogBeta(s, other) + DoubleDouble.Log(s)
            : TinyLogScaledBeta(s, other);

    /// <summary><c>log(s B(s, other))</c> below ExactLogScaledShape: log Γ(1 + s) less <c>log Γ(other + s) − log Γ(other)</c>,
    /// <c>s ψ(z) + s² ψ′(z)/2</c> at z = other, or below one at 1 + other less <c>log(1 + s/other)</c>, the next term under
    /// s³. LogScaledBeta's double held s ε, and FisherSnedecorQuantile(5e-298, 1e200, 1e-300) read 584 ulps (#1745's Review A).</summary>
    private static DoubleDouble TinyLogScaledBeta(double s, double other)
    {
        DoubleDouble z = new(other, 0.0);
        DoubleDouble increment = new(0.0, 0.0);
        if (other < 1.0)
        {
            z = DoubleDouble.Sum(1.0, other);
            increment = -LogOnePlusRatio(s, other);
        }
        increment += (s * Digamma(z)) + new DoubleDouble(0.5 * s * s * Trigamma.Of(z.High), 0.0);
        return DoubleDouble.LogGammaOnePlus(s) - increment;
    }

    /// <summary><c>log(1 + s/other)</c> in double-double: Log1P of the ratio, the two logarithms past a ratio of 1e-3,
    /// where the ratio can pass the doubles at a subnormal other.</summary>
    private static DoubleDouble LogOnePlusRatio(double s, double other) =>
        s > 1e-3 * other
            ? DoubleDouble.Log(DoubleDouble.Sum(other, s)) - DoubleDouble.Log(other)
            : DoubleDouble.Log1P(new DoubleDouble(s, 0.0) / new DoubleDouble(other, 0.0));

    /// <summary>ψ(z) for z from one, raised past 20 by <c>ψ(z) = ψ(z + 1) − 1/z</c>, then <c>log z − 1/(2z) − Σ B_2k/(2k z^2k)</c>
    /// in double, its first term under 2e-4 and so its rounding under 1e-20 of ψ; to 1e-21 at eight terms.</summary>
    private static DoubleDouble Digamma(DoubleDouble z)
    {
        DoubleDouble one = new(1.0, 0.0);
        DoubleDouble steps = new(0.0, 0.0);
        while (z.High < DigammaFloor)
        {
            steps += one / z;
            z += one;
        }
        DoubleDouble inverse = one / z;
        double w = inverse.High * inverse.High;
        double series = w * (DigammaTerms[0] + (w * (DigammaTerms[1] + (w * (DigammaTerms[2] + (w * (DigammaTerms[3]
            + (w * (DigammaTerms[4] + (w * (DigammaTerms[5] + (w * (DigammaTerms[6] + (w * DigammaTerms[7]))))))))))))));
        return DoubleDouble.Log(z) - (0.5 * inverse) - new DoubleDouble(series, 0.0) - steps;
    }

    private const double DigammaFloor = 20.0;

    // B_2k / 2k, k from 1 to 8.
    private static readonly double[] DigammaTerms =
        [1.0 / 12.0, -1.0 / 120.0, 1.0 / 252.0, -1.0 / 240.0, 1.0 / 132.0, -691.0 / 32760.0, 1.0 / 12.0, -3617.0 / 8160.0];

    /// <summary><c>x^a y^b / (a B(a, b))</c> below SmallShape, <paramref name="logScaledBeta"/> being <c>log(a B)</c>.</summary>
    /// <remarks>
    /// Its own method, so the shape inside the divisor cannot meet the Stirling branch, which forms <c>B</c> alone: through
    /// Front's divisor a <c>log(a B)</c> at a shape of 10 would have read <c>a</c> times off (#1690's review). x^a by
    /// Math.Pow up to x = 1/2 past a log x of Gamma.LongPower, where e^(a log x) took some 40 nats' rounding: FisherSnedecorSf(
    /// 1.14e305, 0.0254, 0.118) read 2.4e-15 off (#1729's Review A); within it one exponential rounds less.
    /// </remarks>
    private static double ScaledFront(double a, double b, double x, double y, double logScaledBeta)
    {
        double power = LogPower(a, x, y);
        double rest = LogPower(b, y, x) - logScaledBeta;
        return x <= 0.5 ? Gamma.PowerTimesExp(x, a, power, rest) : Math.Exp(power + rest);
    }

    /// <summary><c>a log x + b log y</c> in double-double, each logarithm on the side where its argument is small.</summary>
    /// <remarks>Past one half a side's log is −log of one less the other: as one less a tiny side, its double-double
    /// dropped that side's low part, b times it 4.2e-14 of FisherSnedecorPdf(245, 3.7, 1e300) (#1745's Review A).</remarks>
    private static DoubleDouble ExactSidesLogPower(double a, double b, DoubleDouble x, DoubleDouble y) =>
        (a * SideLog(x, y)) + (b * SideLog(y, x));

    /// <summary><c>log side</c>, from its complement past one half.</summary>
    private static DoubleDouble SideLog(DoubleDouble side, DoubleDouble complement) =>
        side.High > 0.5 ? -MinusLogOneLess(side, complement) : DoubleDouble.Log(side);

    /// <summary><c>a log x + b log y</c>, each logarithm on the side where its argument is small: Front's and LogFront's one
    /// numerator.</summary>
    [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
    private static double SidesLogPower(double a, double b, double x, double y) => LogPower(a, x, y) + LogPower(b, y, x);

    /// <summary><c>exponent · log side</c>, the logarithm taken on whichever of <c>side + other = 1</c> is small.</summary>
    /// <remarks>A y of 1e-16 is not lost to the x = 1 - y it rounds: #1158's review measured 6e-7 at shapes (5e9, 0.5).</remarks>
    [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
    private static double LogPower(double exponent, double side, double other) => exponent * LogComplement(other, side);

    /// <summary>Front past both shapes of 10, at <paramref name="deviation"/> from the exact mean, or NaN to take it from x.</summary>
    internal static double StirlingFront(double a, double b, double x, double y, double deviation) =>
        Math.Exp(StirlingExponent(a, b, x, y, deviation)) * Math.Sqrt(a / (a + b) * b / (2.0 * Math.PI));

    /// <summary>The logarithm of <see cref="FastFront"/> past a shape of 10 on both sides.</summary>
    /// <remarks>
    /// <c>a log x + b log y − log B</c> cancels terms of order <c>a log a</c>: at shapes 1e15 it lost 5 nats, and the beta
    /// inversion's Newton steps, 150 times short, stopped 14 standard deviations from the root (#1690's review).
    /// </remarks>
    internal static double LogStirlingFront(double a, double b, double x, double y) => LogStirlingFront(a, b, x, y, double.NaN);

    /// <summary>The same at <paramref name="deviation"/> from the exact mean, or NaN to take it from x.</summary>
    internal static double LogStirlingFront(double a, double b, double x, double y, double deviation) =>
        StirlingExponent(a, b, x, y, deviation) + (0.5 * Math.Log(a / (a + b) * b / (2.0 * Math.PI)));

    /// <summary>The logarithm of Front or of StirlingFront at <paramref name="deviation"/>, as the shapes choose between them.</summary>
    /// <remarks>For a density whose front underflows before its division by the argument, as F's did at 1e-33 and shapes 20 (#1708).</remarks>
    internal static double LogFront(double a, double b, double x, double y, double deviation) =>
        a < StirlingMinimumShape || b < StirlingMinimumShape
            ? SidesLogPower(a, b, x, y) - LogBeta(a, b)
            : LogStirlingFront(a, b, x, y, deviation);

    /// <summary><c>log(1 − value)</c> given both <paramref name="value"/> and its <paramref name="complement"/>, on the side where it keeps its digits.</summary>
    [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
    internal static double LogComplement(double value, double complement) =>
        complement <= 0.5 ? Math.Log(complement) : Gamma.LogOnePlusMinus(-value, complement) - value;

    /// <summary>Front's exponent past a shape of 10: <see cref="ShapeExponent"/> and the Stirling corrections.</summary>
    private static double StirlingExponent(double a, double b, double x, double y, double deviation) =>
        ShapeExponent(a, b, x, y, deviation, out _)
        + Gamma.LogStirlingCorrection(a + b) - Gamma.LogStirlingCorrection(a) - Gamma.LogStirlingCorrection(b);

    /// <summary><c>a log(x/p) + b log(y/q)</c>, <c>p = a/(a+b)</c> and <c>q = b/(a+b)</c> exact, to its last digits.</summary>
    /// <remarks>
    /// With <c>D = x − p</c>, it is <c>a L(D/p) + b L(−D/q)</c>, L(t) = log(1 + t) − t: the linear terms cancel since
    /// <c>a/p = b/q = a + b</c>. D comes from the smaller of x and y, against the rounded p corrected by
    /// <c>(a − c p − e p)/c</c>, c = a + b rounded with error e, both exact: without the correction a tail at shapes
    /// (3e6, 1e7) was 1e-12 off, and without e, worth <c>e (1 − p − q)</c>, the exponent at (5e29, 3e32) came out positive
    /// and F's tail fell from 0.25 to 0 as f grew (#1690's review). <paramref name="deviation"/> is D, which exchanging
    /// the shapes and points only negates.
    /// </remarks>
    internal static double ShapeExponent(double a, double b, double x, double y, double givenDeviation, out double deviation)
    {
        // c + e is a + b exactly.
        (double c, double e) = DoubleDouble.Sum(a, b);
        (double x0, double correctionX) = CorrectedShare(a, c, e);
        (double y0, double correctionY) = CorrectedShare(b, c, e);
        double fromX = (x - x0) - correctionX;
        double fromY = correctionY - (y - y0);
        deviation = x < y ? fromX : fromY;
        // S1244: x = y = 1/2 is the one point both sides claim, and their mean keeps the sign antisymmetric there.
#pragma warning disable S1244
        if (x == y)
#pragma warning restore S1244
        {
            deviation = 0.5 * (fromX + fromY);
        }
        if (!double.IsNaN(givenDeviation))
        {
            deviation = givenDeviation;
        }
        // 1 + t against the same exact mean as t, x / (x0 + correction), as a factor: subtracting the correction from
        // x / x0 cost 6e-6 of F's tail at (1e-10, 30, 100), where x / x0 is 1e-10.
        double towardX = x / x0 * (1.0 - (correctionX / x0));
        double towardY = y / y0 * (1.0 - (correctionY / y0));
        return (a * Gamma.LogOnePlusMinus(deviation / x0, towardX)) + (b * Gamma.LogOnePlusMinus(-deviation / y0, towardY));
    }

    /// <summary>Student's tail to first order in df, <c>½ − (df/2) asinh(t/√df)</c>, for df to FirstOrderDf.</summary>
    /// <remarks>
    /// <c>I_x(a, ½) = 1 + a (log x − 2 log(1 + √(1 − x))) + O(a²)</c>, which is <c>1 − 2a asinh(t/√df)</c> at
    /// <c>x = df/(df + t²)</c>: equal to a 400-digit series to the last bit at df 1e-12, 1e-16 and 5e-324. The general
    /// path's last ulp sat either side of one half there, and the tail rose with t at df 1.1e-20 (#1690's review).
    /// </remarks>
    private static double FirstOrderSf(double t, double df) => 0.5 - (0.5 * df * ScaledAsinh(t, df));

    /// <summary><c>√df sinh(w)</c>, FirstOrderSf's root, through logs where sinh alone overflows.</summary>
    /// <remarks>Past w = 700 <c>e^w</c> overflowed before √df scaled it, and roots near 1e302 at df 1e-12 read +∞ (#1690's review).</remarks>
    private static double FirstOrderRoot(double w, double df)
    {
        if (Math.Abs(w) > 700.0)
        {
            double magnitude = Math.Exp(Math.Abs(w) - Ln2 + (0.5 * Math.Log(df)));
            return w < 0.0 ? -magnitude : magnitude;
        }
        return Math.Sqrt(df) * 0.5 * (NearZero.Expm1(w) - NearZero.Expm1(-w));
    }

    /// <summary><c>asinh(t/√df)</c>, through logs where <c>t/√df</c> overflows, as at a subnormal df.</summary>
    private static double ScaledAsinh(double t, double df)
    {
        double z = t / Math.Sqrt(df);
        double magnitude = Math.Abs(z);
        double asinh = magnitude > 1e150
            ? Ln2 + Math.Log(Math.Abs(t)) - (0.5 * Math.Log(df))
            : NearZero.Log1P(magnitude + (magnitude * magnitude / (1.0 + Math.Sqrt(1.0 + (magnitude * magnitude)))));
        return t < 0.0 ? -asinh : asinh;
    }

    /// <summary>The upper tail of Student's t distribution: P(T &gt; t).</summary>
    /// <remarks>The two limits in closed form (#1690's review): from <c>df = 1e20</c> the normal law with its first term
    /// in 1/df, the normal law itself at the infinite df Welch's df overflows to (NormalLimitDf); to <c>df = 1e-12</c>
    /// <see cref="FirstOrderSf"/>.</remarks>
    internal static double StudentSf(double t, double df) => StudentSf(t, df, exact: true);

    /// <summary><see cref="StudentSf(double, double)"/> by the double forms, as Pearson's p-value takes its tail: Spearman's
    /// matrix reads one a pair, and the 106-bit tail took it from 92 ms to 488 ms at 1,000 variables, scipy's 160 (#1746's
    /// Review A).</summary>
    internal static double DoubleFormStudentSf(double t, double df) => StudentSf(t, df, exact: false);

    private static double StudentSf(double t, double df, bool exact)
    {
        if (df >= NormalLimitDf)
        {
            return NormalLimitSf(t, df);
        }
        if (df > 0.0 && df <= FirstOrderDf)
        {
            // The support's ends, and NaN, answer for themselves rather than through the formula's asinh.
            if (double.IsNaN(t))
            {
                return t;
            }
            if (double.IsInfinity(t))
            {
                return t > 0.0 ? 0.0 : 1.0;
            }
            return FirstOrderSf(t, df);
        }
        // No cached log(a B): past FirstOrderDf the exact tail takes it in double-double, which discarded the cached one,
        // 141 of StudentSf(2, 1e-9)'s 698 ns (#1745's Review A).
        return StudentSf(t, df, double.NaN, double.NaN, exact);
    }

    /// <summary>The same by the double forms alone where <paramref name="exact"/> is false, for the inversion's steps.</summary>
    private static double StudentSf(double t, double df, double logBeta, double logScaledBeta, bool exact)
    {
        if (double.IsNaN(t))
        {
            return double.NaN;
        }
        // I_{df/(df+t^2)}(df/2, 1/2) is twice the tail beyond |t|, so half of it
        // is the tail on one side and the sign says which side we are on.
        ExactTail tail = TailBeyond(t, df, logBeta, logScaledBeta, exact);
        // The larger side one less the smaller before its rounding: rounded twice, StudentCdf(1.306883524082081, 0.7) read
        // an ulp off, 3,319 of 18,000 points (#1745's Review A).
        return t >= 0.0 ? tail.Rounded : tail.Complement().Rounded;
    }

    /// <summary>The tail beyond |t|, half of <c>I_x(df/2, ½)</c>, before its last rounding where <paramref name="exact"/>.</summary>
    private static ExactTail TailBeyond(double t, double df, double logBeta, double logScaledBeta, bool exact)
    {
        DoubleDouble farTail = FarStudentTail(t, df, logBeta, logScaledBeta, exact, out int farExponent);
        if (!double.IsNaN(farTail.High))
        {
            return new ExactTail(farTail, inDouble: !exact, farExponent);
        }
        var logs = new ShapeLogs(logBeta, logScaledBeta, double.NaN);
        if (exact)
        {
            // Both halves by division to 106 bits, so neither is 1 minus the other and a far tail stays small (#841).
            // Rounded, they lost b ε/2 of the tail at df 2000, 1.8e-14 (#1745).
            DoubleDouble tSquared = DoubleDouble.Product(t, t);
            DoubleDouble denominator = new DoubleDouble(df, 0.0) + tSquared;
            ExactTail whole = ExactRegularizedIncomplete(df / 2.0, 0.5, new DoubleDouble(df, 0.0) / denominator, tSquared / denominator, logs, double.NaN);
            return whole.Halved();
        }
        // The inversion's steps need no 106 bits: the split in double, as main had it.
        double square = t * t;
        double sum = df + square;
        return ExactTail.OfDouble(0.5 * RegularizedIncomplete(df / 2.0, 0.5, df / sum, square / sum, logs));
    }

    /// <summary>Half of <c>I_x(df/2, 1/2)</c> by its leading term, where <c>x = df/(df+t²)</c> is below 1e-100; NaN otherwise.</summary>
    /// <remarks>
    /// Past <c>|t| = 1.3e154</c> the square overflows and the tail read zero, which capped every
    /// quantile there (#1158: the Cauchy's 1e-300 quantile is 3.2e299). Formed in logs from
    /// <c>log x = log df − 2 log|t| − log1p(df/t²)</c>, the leading term <c>x^a / (a B(a, ½))</c>
    /// is exact to the next one's relative size, about <c>x</c>, far below a double's precision.
    /// </remarks>
    private static DoubleDouble FarStudentTail(double t, double df, double logBeta, double logScaledBeta, bool exact, out int exponent)
    {
        exponent = 0;
        double magnitude = Math.Abs(t);
        if (magnitude <= 1e50)
        {
            return new DoubleDouble(double.NaN, 0.0);
        }
        double ratio = df / magnitude / magnitude;
        double logX = Math.Log(df) - (2.0 * Math.Log(magnitude)) - Math.Log(1.0 + ratio);
        if (logX > -230.0)
        {
            return new DoubleDouble(double.NaN, 0.0);
        }
        double a = df / 2.0;
        if (exact)
        {
            // To 106 bits, as the nearer tails: one double exponential of a rounded log was 25 ulps off at StudentSf(1e100, 1)
            // and stepped every 150 ulps of t (#1745's Review A). log1p(ratio) is ratio below 1e-100.
            DoubleDouble exactLogX = DoubleDouble.Log(df) - (2.0 * DoubleDouble.Log(magnitude)) - new DoubleDouble(ratio, 0.0);
            DoubleDouble power = (a * exactLogX) - ExactLogScaledBeta(a, 0.5);
            exponent = TailExponent(power);
            return 0.5 * DoubleDouble.ExpExact(power, -exponent);
        }
        if (a < SmallShape)
        {
            // log(a B) rather than log a + log B, which cancel below SmallShape, in StudentLogScaledBeta's form, as
            // StudentTail's cache takes it, so the inversion solves this tail.
            return new DoubleDouble(0.5 * Math.Exp((a * logX) - (double.IsNaN(logScaledBeta) ? StudentLogScaledBeta(a) : logScaledBeta)), 0.0);
        }
        // The inversion's cached log B when it has one (#1690), else LogBeta's own, not three log-gammas.
        return new DoubleDouble(0.5 * Math.Exp((a * logX) - Math.Log(a) - CachedOrLogBeta(logBeta, a, 0.5)), 0.0);
    }

    /// <summary>The upper tail of the F distribution: P(F &gt; f).</summary>
    /// <remarks>FisherSnedecorCdf's mirror, kept apart: one body taking the tail as a flag cost the lower tail 1.5% (#1690).</remarks>
    internal static double FisherSnedecorSf(double f, double dfn, double dfd)
    {
        if (double.IsNaN(f))
        {
            return double.NaN;
        }
        if (f <= 0.0)
        {
            return 1.0;
        }
        // The support's end: through the small half's limit, shapes of 8.5e307 made log B NaN there.
        if (double.IsPositiveInfinity(f))
        {
            return 0.0;
        }

        return FisherTailAt(f, dfn, dfd, upperTail: true).Rounded;
    }

    /// <summary>The lower tail of the F distribution: P(F &lt;= f).</summary>
    internal static double FisherSnedecorCdf(double f, double dfn, double dfd)
    {
        if (double.IsNaN(f))
        {
            return double.NaN;
        }
        if (f <= 0.0)
        {
            return 0.0;
        }
        if (double.IsPositiveInfinity(f))
        {
            return 1.0;
        }
        return FisherTailAt(f, dfn, dfd, upperTail: false).Rounded;
    }

    /// <summary>FisherSnedecorSf, or FisherSnedecorCdf, before its last rounding, which the quantiles step on and judge.</summary>
    internal static ExactTail ExactFisherTail(double f, double dfn, double dfd, bool upperTail)
    {
        if (double.IsNaN(f) || f <= 0.0 || double.IsPositiveInfinity(f))
        {
            return ExactTail.OfDouble(upperTail ? FisherSnedecorSf(f, dfn, dfd) : FisherSnedecorCdf(f, dfn, dfd));
        }
        return FisherTailAt(f, dfn, dfd, upperTail);
    }

    /// <summary>An F tail at a positive finite f: lifted where a df's half is not a double, at the halves otherwise.</summary>
    [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
    private static ExactTail FisherTailAt(double f, double dfn, double dfd, bool upperTail) =>
        ExactHalf.EitherBelow(dfn, dfd)
            ? LiftedFisherTail(f, dfn, dfd, upperTail)
            : FisherTail(f, dfn, dfd, dfn / 2.0, dfd / 2.0, upperTail);

    /// <summary>An F tail at shapes <paramref name="a"/> and <paramref name="b"/>, its point <c>y</c> from the dfs.</summary>
    /// <remarks>
    /// Both halves by division, as in StudentSf: y can round to one while 1 - y still holds its digits, as at shapes
    /// (1e10, 1), and the incomplete beta reads the side that keeps them. The shapes are the dfs' halves but where
    /// <see cref="LiftedFisherTail"/> lifts them.
    /// </remarks>
    [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
    private static ExactTail FisherTail(double f, double dfn, double dfd, double a, double b, bool upperTail)
    {
        (DoubleDouble exactLower, DoubleDouble exactUpper, double lower, double upper) = ExactFisherSplit(f, dfn, dfd);
        bool belowFloor = lower < ProductFloor || upper < ProductFloor;
        if (belowFloor && SmallSideTail(f, dfn, dfd, (a, b), lower < upper, upperTail, out ExactTail small))
        {
            return small;
        }
        // A half below ProductFloor SmallSideTail declines takes the limit too, the other shape past 1e275: the 106-bit forms
        // read its point to a double's bits, and FisherSnedecorCdf(0.0102, 367.69, 1.2e308) 70 ulps off (#1746's Review A).
        if (belowFloor || TakesLimit(lower, upper))
        {
            return FisherTailFromLimit(f, dfn, dfd, a, b, lower, upperTail);
        }
        // From the dfs y is formed from, NaN where a df is lifted: UniformBeta, the deviation's one reader, holds no such shape.
        double deviation = FisherDeviation(f, dfn, dfd, lower);
        // S2234: the upper tail is the reflection I_(1-y)(b, a), its arguments swapped on purpose.
#pragma warning disable S2234
        return upperTail
            ? ExactRegularizedIncomplete(b, a, exactUpper, exactLower, ShapeLogs.None, -deviation)
            : ExactRegularizedIncomplete(a, b, exactLower, exactUpper, ShapeLogs.None, deviation);
#pragma warning restore S2234
    }

    /// <summary>An F tail at a df below ExactHalf.Threshold, from the lifted shapes, dropped once where of degree one.</summary>
    [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.NoInlining)]
    private static ExactTail LiftedFisherTail(double f, double dfn, double dfd, bool upperTail)
    {
        (double a, double b, bool lone, bool smallUpper) = ExactHalf.FisherShapes(dfn, dfd);
        // One lifted: its small tail scales with it.
        return lone
            ? ExactTail.OfDouble(ExactHalf.FromLiftedTail(FisherTail(f, dfn, dfd, a, b, upperTail: smallUpper).Rounded, upperTail == smallUpper))
            : FisherTail(f, dfn, dfd, a, b, upperTail);
    }

    /// <summary>The log-odds <c>log(x/y)</c> at which the first order's lower tail, or its upper, before its last rounding,
    /// is <paramref name="target"/>: the share's parts as SmallShapesShare forms them, the
    /// target less the share exact within a factor of two (Sterbenz), so a start for the F quantile's search. At twice the
    /// shapes it reads half the log-odds, exactly while other·share is normal: every step scales by a power of two.</summary>
    internal static double SmallShapesLogOdds(double a, double b, double target, bool upperTail)
    {
        double other = upperTail ? b : a;
        (double share, double correction) = ShareParts(other, upperTail ? a : b);
        double ownLogOdds = ((target - share) - correction) / (other * share);
        return upperTail ? -ownLogOdds : ownLogOdds;
    }

    /// <summary>Whether the first order's guess holds at log-odds <paramref name="logOdds"/>, 1 + |log x| + |log y| being
    /// at most |log(x/y)| + 1 + 2 log 2: absolute, as a guess, where the tail's own reach is relative to the smaller share,
    /// the search reading the exact tail; relative here, a lone lifted df's guess failed and its root fell short.</summary>
    internal static bool SmallShapesHoldAt(double a, double b, double logOdds) =>
        Math.Max(a, b) * (LogsPastOdds + Math.Abs(logOdds)) <= SmallShapesReach;

    /// <summary><c>own/(other + own) + other·own/(other + own)·logRatio</c>, I_x(other, own) to first order at
    /// <c>logRatio = log(x/y)</c>: the share with the sum's and the quotient's roundings carried, lifted first so the
    /// residual is a normal double, and the first-order term added to them before the one last rounding.</summary>
    /// <remarks>Its low part kept: rounded, a plateau a/(a + b) 8e-18 above its rounding read as reaching it, and
    /// FisherSnedecorIsf(0.9999999999, 1e-300, 1e-310) answered ε for main's +∞ (#1745's Review A).</remarks>
    private static DoubleDouble SmallShapesShare(double other, double own, double logRatio)
    {
        (double share, double correction) = ShareParts(other, own);
        return DoubleDouble.Sum(share, correction + (other * share * logRatio));
    }

    /// <summary><c>own/(other + own)</c> rounded and what it lacks, the shapes lifted so the residual is a normal double.</summary>
    private static (double Share, double Correction) ShareParts(double other, double own)
    {
        double liftedOther = other * ExactHalf.Lift;
        double liftedOwn = own * ExactHalf.Lift;
        // sum + error is the lifted shapes' sum exactly.
        (double sum, double error) = DoubleDouble.Sum(liftedOther, liftedOwn);
        return CorrectedShare(liftedOwn, sum, error);
    }

    /// <summary><c>part/(sum + error)</c> rounded and what it lacks, <c>sum + error</c> being an exact sum: the residual
    /// exact, the sum's error carried. ShapeExponent's mean and the first order's share take it alike.</summary>
    [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
    private static (double Share, double Correction) CorrectedShare(double part, double sum, double error)
    {
        double share = part / sum;
        return (share, (ExactProduct.Residual(part, sum, share) - (error * share)) / sum);
    }

    /// <summary>Whether a half of FisherSplit is below the smallest normal double, where it keeps too few digits.</summary>
    internal static bool TakesLimit(double lower, double upper) =>
        lower < NearZero.SmallestNormal || upper < NearZero.SmallestNormal;

    /// <summary>The F tail where <see cref="TakesLimit"/> holds, or a half below ProductFloor SmallSideTail declines, from
    /// the small half's limit.</summary>
    /// <remarks>
    /// With <c>s</c> the small half's shape, <c>o</c> the other's and <c>z = o y</c>, which is <c>d₁f/2</c> on the lower side
    /// and <c>d₂/2f</c> on the upper: <c>y^s / (s B(s, o))</c> while <c>z</c> is below 1e-17, the terms dropped being of order
    /// <c>z</c>; past it <c>o</c> is above 4e290, and the tail is the chi-squared limit <c>P(s, z)</c>, off by order 1/o. A
    /// zero half read as the support's end, a subnormal one kept too few digits, and the first form alone read
    /// <c>FisherSnedecorSf(1, 1, 1e308)</c> 0.202 for 0.317 and <c>FisherSnedecorCdf(1e-16, 1, 1e308)</c> 0 (#1690's review).
    /// </remarks>
    internal static ExactTail FisherTailFromLimit(double f, double dfn, double dfd, double a, double b, double lower, bool upperTail)
    {
        (double s, double other, double logY, double z, bool isLower) = SmallHalf(f, dfn, dfd, a, b, lower);
        if (FirstOrderHoldsInLimit(a, b, logY))
        {
            double logRatio = isLower ? logY : -logY;
            return new ExactTail(upperTail ? SmallShapesShare(b, a, -logRatio) : SmallShapesShare(a, b, logRatio), inDouble: true);
        }
        (double mantissa, int oddsExponent) = OddsParts(f, dfn, dfd, isLower);
        if (BelowLimitBound(other, mantissa, oddsExponent))
        {
            if (upperTail == isLower)
            {
                // One less the half's tail, which is near one only where s log y is small, so its log loses nothing.
                (double logScaled, double ratio) = HalfBeta(s, other);
                return ExactTail.OfDouble(-NearZero.Expm1((s * logY) - logScaled + Math.Log(ratio)));
            }
            (double value, int exponent) = HalfTailParts(f, dfn, dfd, (s, other), logY, isLower);
            return ExactTail.OfDouble(PowerOfTwo.Scale(value, exponent));
        }
        // The limit to 106 bits, from z = o y so formed: as a double it fell 34 ulps between neighbouring f at dfs (30, 1e307),
        // and 2 where the 106-bit forms above 2^-1022 met it at dfs (0.3, 1e300) (#1746's Review A).
        DoubleDouble exactZ = isLower
            ? 0.5 * (2.0 * other / dfd) * DoubleDouble.Product(dfn, f)
            : 0.5 * (2.0 * other / dfn) * (new DoubleDouble(dfd, 0.0) / new DoubleDouble(f, 0.0));
        DoubleDouble limit = ExactRegularizedGamma(s, exactZ, upperTail == isLower, out _, out bool inDouble, out int limitExponent);
        // Past a shape near 1e307 s log z and log Γ(s) both overflow, ∞ − ∞: the double limit's 0 or 1, as main read them,
        // where FisherSnedecorSf(1e-300, 1e308, 1e300) read NaN (#1746's Review A).
        if (!double.IsNaN(limit.High))
        {
            return new ExactTail(limit, inDouble, limitExponent);
        }
        return ExactTail.OfDouble(upperTail == isLower ? Gamma.RegularizedQ(s, z) : Gamma.RegularizedP(s, z));
    }

    /// <summary>The tails at a side y below 2^-969 to 106 bits: its own, <c>y^s / (s B(s, o))</c> times its first-order
    /// terms, and one less that by −expm1, where the other shape times the odds is below LimitBound and the dfs are their halves;
    /// false otherwise. Below 2^-969 the side's double-double kept a double's digits, and FisherSnedecorQuantile(4.44e-308,
    /// 2, 5) answered a double short; the other side's limit double, FisherSnedecorIsf(0.5776, 0.2, 0.0015) 1,200 ulps
    /// short (#1745's Review A).</summary>
    /// <remarks>
    /// <c>log I = s log r − (s + o − (s + o)/(s + 1)) r − log(s B(s, o))</c> in the odds r, subnormal or near it: the terms
    /// dropped are of order r² and <c>(o r)²</c>, under 1e-34.
    /// </remarks>
    [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.NoInlining)]
    private static bool SmallSideTail(
        double f, double dfn, double dfd, (double A, double B) shapes, bool isLower, bool upperTail, out ExactTail tail)
    {
        tail = default;
        // Lifted dfs' shapes are not their halves, and their tails take LiftedFisherTail's forms.
        if (ExactHalf.EitherBelow(dfn, dfd))
        {
            return false;
        }
        (double s, double other) = isLower ? shapes : (shapes.B, shapes.A);
        // Where the limit's first order holds, its one form across the mode stands, both sides of it: the own side alone in
        // 106 bits jumped at ProductFloor, and FisherSnedecorSf(4.9896007738368e291, d, d) rose an ulp (#1746's Review A).
        if (FirstOrderHoldsInLimit(shapes.A, shapes.B, HalfLogY(f, dfn, dfd, isLower)))
        {
            return false;
        }
        (double mantissa, int oddsExponent) = OddsParts(f, dfn, dfd, isLower);
        if (!BelowLimitBound(other, mantissa, oddsExponent))
        {
            return false;
        }
        double odds = PowerOfTwo.Scale(mantissa, oddsExponent);
        DoubleDouble logProduct = DoubleDouble.Log(dfn) + DoubleDouble.Log(f);
        DoubleDouble logOdds = isLower ? logProduct - DoubleDouble.Log(dfd) : DoubleDouble.Log(dfd) - logProduct;
        double firstOrder = (s + other - ((s + other) / (s + 1.0))) * odds;
        DoubleDouble power = (s * logOdds) - new DoubleDouble(firstOrder, 0.0) - ExactLogScaledBeta(s, other);
        if (upperTail == isLower)
        {
            // −expm1 of the own tail's log: one less a tail near one, 1 − 7e-298 at dfs (1e-300, 2), kept nothing of it.
            tail = new ExactTail(-DoubleDouble.Expm1Exact(power), inDouble: false);
            return true;
        }
        int exponent = TailExponent(power);
        tail = new ExactTail(DoubleDouble.ExpExact(power, -exponent), inDouble: false, exponent);
        return true;
    }

    /// <summary><c>y^s / (s B(s, o))</c>, the small half's tail where FisherTailFromLimit's first form holds, as <c>v 2^k</c>.</summary>
    /// <remarks>
    /// Past an s log y of Gamma.LongPower, y^s from y's mantissa and exponent, not <c>e^(s log y)</c>, which took that exponent's
    /// rounding: FisherSnedecorCdf(1e-310, 1, 3) read 3.4e-14 off (#1729's Review A). Past a shape of 10 the other side's
    /// <c>o^s</c> goes with it, <c>(o y)^s</c>, and <c>log(s B o^s)</c> is formed with no <c>s log o</c> to cancel. The
    /// other shape below s and SmallShape: <c>s B(s, o)</c> is <c>(s/o) o B(o, s)</c>, the ratio out of the exponential.
    /// </remarks>
    private static (double Value, int Exponent) HalfTailParts(
        double f, double dfn, double dfd, (double S, double Other) shapes, double logY, bool isLower)
    {
        (double s, double other) = shapes;
        // Within LongPower one exponential of s log y rounds less than the power's parts.
        if (s * logY >= -Gamma.LongPower)
        {
            (double logScaled, double ratio) = HalfBeta(s, other);
            return (ratio * Math.Exp((s * logY) - logScaled), 0);
        }
        (double mantissa, int exponent) = OddsParts(f, dfn, dfd, isLower);
        double factor;
        if (OtherScales(s, other))
        {
            (double logScaled, double ratio) = HalfBeta(s, other);
            factor = ratio * Math.Exp(-logScaled);
        }
        else if (other >= StirlingMinimumShape)
        {
            (double otherMantissa, int otherExponent) = PowerOfTwo.Split(other);
            mantissa *= otherMantissa;
            exponent += otherExponent;
            factor = Math.Exp(-LogShapeBetaTimesPower(s, other));
        }
        else
        {
            factor = ReciprocalShapeBeta(s, other);
        }
        (double value, int powerExponent) = PowerOfTwo.Power(mantissa, exponent, s);
        return (value * factor, powerExponent);
    }

    /// <summary>Whether the other shape takes the scaling of <c>s B(s, o)</c>: below SmallShape and s, where
    /// <c>log(o B(o, s))</c> holds to <c>o ε</c>; HalfBeta and HalfTailParts read the one rule.</summary>
    private static bool OtherScales(double s, double other) => other < SmallShape && other < s;

    /// <summary><c>log(s B(s, o))</c> and the ratio kept outside it: below SmallShape and s, the other shape takes the
    /// scaling, <c>s B(s, o) = (s/o) o B(o, s)</c>, <c>log(o B(o, s))</c> holding to <c>o ε</c>; else the ratio is one.</summary>
    /// <remarks>The limit's complement, its short tail and its root read it; its long tail reads the forms of HalfTailParts.</remarks>
    private static (double LogScaled, double Ratio) HalfBeta(double s, double other)
    {
        // S2234: log(o B(o, s)), the smaller shape first on purpose.
#pragma warning disable S2234
        return OtherScales(s, other) ? (LogScaledBeta(other, s), other / s) : (LogShapeBeta(s, other), 1.0);
#pragma warning restore S2234
    }

    /// <summary><c>1/(s B(s, o))</c>: from the table of <c>1/Γ(1 + k/2)</c> where both shapes are half-integers, else
    /// <c>e^(−log s B)</c>, whose Lanczos log-gammas hold 1e-15: FisherSnedecorPdf(5e-324, 2, 1) read 1.0000000000000018.</summary>
    private static double ReciprocalShapeBeta(double s, double other) =>
        Gamma.IsHalfIntegerShape(s) && Gamma.IsHalfIntegerShape(other) && Gamma.IsHalfIntegerShape(s + other)
            ? other * Gamma.ReciprocalGammaOnePlus(s) * Gamma.ReciprocalGammaOnePlus(other)
                / ((s + other) * Gamma.ReciprocalGammaOnePlus(s + other))
            : Math.Exp(-LogShapeBeta(s, other));

    /// <summary>The F odds <c>d₁f/d₂</c>, or their inverse where <paramref name="isLower"/> is false, as <c>m 2^e</c>.</summary>
    /// <remarks>Two roundings in the mantissa, none in the exponent: the odds themselves are below the doubles here.</remarks>
    internal static (double Mantissa, int Exponent) OddsParts(double f, double dfn, double dfd, bool isLower)
    {
        (double mn, int en) = PowerOfTwo.Split(dfn);
        (double mf, int ef) = PowerOfTwo.Split(f);
        (double md, int ed) = PowerOfTwo.Split(dfd);
        return isLower ? (mn * mf / md, en + ef - ed) : (md / (mn * mf), ed - en - ef);
    }

    /// <summary><c>log(s B(s, o) o^s)</c> for <c>o</c> from a shape of 10, with no <c>s log o</c> formed.</summary>
    /// <remarks>
    /// <c>log Γ(1 + s)</c> less <c>log Γ(o + s) − log Γ(o) − s log o</c>, which Stirling gives as
    /// <c>(o − ½)(log1p(r) − r) − r/2 + s log1p(r)</c> plus the corrections' difference, <c>r = s/o</c>.
    /// </remarks>
    private static double LogShapeBetaTimesPower(double s, double other)
    {
        double ratio = s / other;
        double increment = StirlingShift(other, ratio)
            + (s * NearZero.Log1P(ratio))
            + Gamma.LogStirlingCorrection(other + s) - Gamma.LogStirlingCorrection(other);
        return Gamma.LogGammaOnePlusAnyShape(s) - increment;
    }

    /// <summary>The F density where <see cref="TakesLimit"/> holds, from the same two limits as the tail.</summary>
    internal static double FisherDensityFromLimit(double f, double dfn, double dfd, double a, double b, double lower)
    {
        (double s, double other, double logY, double z, bool isLower) = SmallHalf(f, dfn, dfd, a, b, lower);
        if (z < LimitBound && !ExactHalf.EitherBelow(dfn, dfd))
        {
            return ExactDensityFromOdds(f, dfn, dfd, (s, other), isLower);
        }
        if (z < LimitBound)
        {
            // s times the half's tail over f, s's and f's exponents taken with the power's: nothing leaves the doubles before the end.
            (double value, int exponent) = HalfTailParts(f, dfn, dfd, (s, other), logY, isLower);
            return PowerOfTwo.Quotient(stackalloc double[] { s, value }, stackalloc double[] { f }, exponent);
        }
        return Gamma.Prefactor(s, z) / f;
    }

    /// <summary><c>r^s / ((1 + r)^(s + o) B(s, o) f)</c> in the odds r to 106 bits, rounded once: the half's tail in doubles
    /// took |s log y| ε, and FisherSnedecorPdf(3e-308, 2.589, 15.2) read 52 ulps low (#1746's Review A).</summary>
    /// <remarks>log1p(r) is r here, its next term of order r², under 1e-300.</remarks>
    private static double ExactDensityFromOdds(double f, double dfn, double dfd, (double S, double Other) shapes, bool isLower)
    {
        (double s, double other) = shapes;
        (double mantissa, int oddsExponent) = OddsParts(f, dfn, dfd, isLower);
        double odds = PowerOfTwo.Scale(mantissa, oddsExponent);
        DoubleDouble logProduct = DoubleDouble.Log(dfn) + DoubleDouble.Log(f);
        DoubleDouble logOdds = isLower ? logProduct - DoubleDouble.Log(dfd) : DoubleDouble.Log(dfd) - logProduct;
        DoubleDouble power = (s * logOdds) - new DoubleDouble((s + other) * odds, 0.0) - ExactLogScaledBeta(s, other)
            + DoubleDouble.Log(s) - DoubleDouble.Log(f);
        return DoubleDouble.ExpExact(power).Rounded;
    }

    /// <summary>f times <see cref="FisherDensityFromLimit"/>, the quantiles' Newton front, formed without the division by f:
    /// past it the density underflowed at FisherSnedecorQuantile(5e-298, 1e200, 1e-300), whose front is 5e-301, and with no
    /// step the search read 14 tails, 35 us to scipy's 25 (#1745's Review A).</summary>
    internal static double FisherFrontFromLimit(double f, double dfn, double dfd, double a, double b, double lower)
    {
        (double s, double other, double logY, double z, bool isLower) = SmallHalf(f, dfn, dfd, a, b, lower);
        if (z < LimitBound)
        {
            (double value, int exponent) = HalfTailParts(f, dfn, dfd, (s, other), logY, isLower);
            return PowerOfTwo.Quotient(stackalloc double[] { s, value }, stackalloc double[] { 1.0 }, exponent);
        }
        return Gamma.Prefactor(s, z);
    }

    /// <summary>The F root whose small half of FisherSplit is below the smallest normal double, by its limit; NaN otherwise.</summary>
    /// <remarks>
    /// The beta inversion read a subnormal <c>y</c> with too few digits, 2e-5 off at f = 1e-318 (#1690's review).
    /// <paramref name="lower"/> and <paramref name="upper"/> are the target's two tails, the smaller of each pair read directly.
    /// </remarks>
    internal static double FisherRootFromLimit(double lower, double upper, double dfn, double dfd, double a, double b)
    {
        double low = HalfRoot(lower, upper, dfn, dfd, a, b, isLower: true);
        return double.IsNaN(low) ? HalfRoot(upper, lower, dfn, dfd, a, b, isLower: false) : low;
    }

    /// <summary>The small half, as its shape, the other shape, <c>log y</c> and <c>z = o y</c>, from the degrees of freedom and f.</summary>
    /// <remarks>z takes the other shape as given, lifted or not: <c>2o / df</c> is one or 2^200, exact.</remarks>
    private static (double S, double Other, double LogY, double Z, bool IsLower) SmallHalf(
        double f, double dfn, double dfd, double a, double b, double lower) =>
        lower < 0.5
            ? (a, b, HalfLogY(f, dfn, dfd, true), dfn * f / 2.0 * (2.0 * b / dfd), true)
            : (b, a, HalfLogY(f, dfn, dfd, false), dfd / f / 2.0 * (2.0 * a / dfn), false);

    /// <summary>The log of the split's half below the doubles, from the dfs and f: SmallHalf's and SmallSideTail's one form.</summary>
    private static double HalfLogY(double f, double dfn, double dfd, bool isLower) =>
        isLower ? Math.Log(dfn) + Math.Log(f) - Math.Log(dfd) : Math.Log(dfd) - Math.Log(dfn) - Math.Log(f);

    /// <summary>Whether RegularizedIncomplete's first order holds in the limit, the other half's log within the doubles' last
    /// bit of zero: FisherTailFromLimit's and SmallSideTail's one test, so the two sides take one form (#1746's Review A).</summary>
    private static bool FirstOrderHoldsInLimit(double a, double b, double logY) =>
        BothBelowMode(a, b) && SmallShapesHold(a, b, logY, 0.0);

    /// <summary>Whether the other shape times the odds, <c>z = o y</c>, is below LimitBound, from the odds' mantissa and
    /// exponent before they round onto the subnormals: rounded to 0 at f = ε, dfs (1e307, 1e308), they passed. SmallSideTail's
    /// and FisherTailFromLimit's one test: decided twice, FisherSnedecorCdf(5e9, 1e307, 1e-7) read 16 ulps high (#1746's
    /// Review A).</summary>
    private static bool BelowLimitBound(double other, double mantissa, int oddsExponent)
    {
        (double otherMantissa, int otherExponent) = PowerOfTwo.Split(other);
        return PowerOfTwo.Scale(otherMantissa * mantissa, otherExponent + oddsExponent) < LimitBound;
    }

    /// <summary>The f at which the half <paramref name="isLower"/> names has tail <paramref name="own"/>, if that half is then below the smallest normal; NaN otherwise.</summary>
    private static double HalfRoot(double own, double complement, double dfn, double dfd, double a, double b, bool isLower)
    {
        double s = isLower ? a : b;
        double other = isLower ? b : a;
        double logOwn = own <= complement ? Math.Log(own) : NearZero.Log1P(-complement);
        // HalfBeta, the tail's normalizer within LongPower; past it the tail's other forms agree with it to Lanczos' 1e-15.
        (double logScaled, double ratio) = HalfBeta(s, other);
        double logY = (logOwn - Math.Log(ratio) + logScaled) / s;

        // f from the logs, not from z = o y, which is smaller still and kept fewer digits as a subnormal.
        double logRatio = Math.Log(dfd) - Math.Log(dfn);
        double f = Math.Exp(isLower ? logY + logRatio : logRatio - logY);
        // z = o y as SmallHalf forms it, the other shape lifted or not: 2o/df is one or 2^200, exact.
        double lift = isLower ? 2.0 * other / dfd : 2.0 * other / dfn;
        double z = (isLower ? dfn * f / 2.0 : dfd / f / 2.0) * lift;
        if (z >= LimitBound)
        {
            z = own <= complement ? GammaQuantile.Invert(own, s, upper: false) : GammaQuantile.Invert(complement, s, upper: true);
            f = isLower ? 2.0 * z / lift / dfn : dfd * lift / (2.0 * z);
        }
        // An f that over- or underflowed is no root: at dfs (9.98e188, 4.56e-207) the lower half's f underflowed to 0, whose
        // half is 0 too, and the F quantile read 9e-73, a tail of 7e-205, for a root of 1.17e44 (#1690's review).
        (double lower, double upper) = FisherSplit(f, dfn, dfd);
        double half = isLower ? lower : upper;
        return f > 0.0 && f < double.PositiveInfinity && half < NearZero.SmallestNormal ? f : double.NaN;
    }

    /// <summary><c>log(s B(s, other))</c>: LogScaledBeta below SmallShape, where <c>log s + log B</c> cancels.</summary>
    private static double LogShapeBeta(double s, double other) =>
        s < SmallShape ? LogScaledBeta(s, other) : Math.Log(s) + LogBeta(s, other);

    /// <summary><c>log B(a, b)</c>, through a Stirling increment when one shape is large and the other is not.</summary>
    /// <remarks>
    /// <c>log Γ(a + b) − log Γ(a)</c> subtracts two terms of order <c>a log a</c> when only <c>a</c> is
    /// large; from Stirling it is <c>(a − ½) log1p(b/a) + b log(a + b) − b</c> plus the corrections'
    /// difference, with nothing of that order formed.
    /// </remarks>
    internal static double LogBeta(double a, double b)
    {
        double large = Math.Max(a, b);
        double small = Math.Min(a, b);
        if (large < StirlingMinimumShape)
        {
            return Gamma.LogGamma(a) + Gamma.LogGamma(b) - Gamma.LogGamma(a + b);
        }
        return Gamma.LogGamma(small) - StirlingIncrement(large, small);
    }

    /// <summary><c>log(s B(s, other))</c> for <c>s</c> below SmallShape, to <c>s ε</c> absolute while <c>other</c> is not below <c>s</c>.</summary>
    /// <remarks>
    /// <c>log s + log B</c> cancels, about <c>−log s</c> each, and the log-gammas behind it hold only 1e-15
    /// absolute near one, more than a tail of order <c>s</c> can lose (#1690's review).
    /// </remarks>
    internal static double LogScaledBeta(double s, double other) => Gamma.LogGammaOnePlus(s) - LogGammaIncrement(other, s);

    /// <summary><c>log(s B(s, ½))</c>, Student's, by three log-gammas: its every reader exponentiates it.</summary>
    /// <remarks>
    /// There 1e-15 absolute is 1e-15 relative, and with the other shape ½, where <c>log Γ</c> is 0.57, three log-gammas
    /// hold it and form no <c>log s</c>; LogScaledBeta's ten-step increment cost StudentSf(2, 0.05) 1.8 times 724c7912's
    /// 126 ns (#1690's review). The inversion's double-form tail takes this one; the density and the 106-bit tails take
    /// log B and log s in double-double from ExactLogScaledShape up, TinyLogScaledBeta below. Below TinyShape it is LogScaledBeta, to <c>s ε</c>: the
    /// three log-gammas read StudentSf(0.00486, 7.5e-12) 12 ulps off one half, and past |t| = 1e50 the far tail as far.
    /// </remarks>
    internal static double StudentLogScaledBeta(double s) =>
        s < TinyShape ? LogScaledBeta(s, 0.5) : Gamma.LogGammaOnePlus(s) + LogSqrtPi - Gamma.LogGamma(0.5 + s);

    /// <summary>An inversion's cached <c>log(s B(s, other))</c>, or NaN to take it here.</summary>
    private static double CachedOrScaled(double cached, double s, double other) =>
        double.IsNaN(cached) ? LogScaledBeta(s, other) : cached;

    /// <summary>An inversion's cached <c>log B(a, b)</c>, or NaN to take it here.</summary>
    private static double CachedOrLogBeta(double cached, double a, double b) =>
        double.IsNaN(cached) ? LogBeta(a, b) : cached;

    /// <summary><c>log Γ(x + s) − log Γ(x)</c> for <c>s</c> below SmallShape: to <c>s ε</c> absolute from <c>x = s</c>, to <c>ε</c> relative below.</summary>
    /// <remarks>
    /// Shifted past 10 by <c>log Γ(x + 1) = log Γ(x) + log x</c>, each step a <c>log1p(s / x)</c>, then Stirling as
    /// StirlingIncrementBody forms it, with the corrections' difference taken term by term as
    /// <c>c_k x^(1−2k) expm1((1 − 2k) log1p(s / x))</c>: no two terms of order <c>x</c> are subtracted.
    /// </remarks>
    private static double LogGammaIncrement(double x, double s)
    {
        double shifts = 0.0;
        while (x < StirlingMinimumShape)
        {
            // log((x + s) / x) as log(s / x) + log1p(x / s) once s passes x: s / x can overflow at a subnormal x.
            shifts += s < x ? NearZero.Log1P(s / x) : LogRatio(s, x) + NearZero.Log1P(x / s);
            x += 1.0;
        }

        double logRatio = NearZero.Log1P(s / x);
        double w = 1.0 / (x * x);
        double corrections = Gamma.Stirling7 * NearZero.Expm1(-13.0 * logRatio);
        corrections = (Gamma.Stirling6 * NearZero.Expm1(-11.0 * logRatio)) + (w * corrections);
        corrections = (Gamma.Stirling5 * NearZero.Expm1(-9.0 * logRatio)) + (w * corrections);
        corrections = (Gamma.Stirling4 * NearZero.Expm1(-7.0 * logRatio)) + (w * corrections);
        corrections = (Gamma.Stirling3 * NearZero.Expm1(-5.0 * logRatio)) + (w * corrections);
        corrections = (Gamma.Stirling2 * NearZero.Expm1(-3.0 * logRatio)) + (w * corrections);
        corrections = (Gamma.Stirling1 * NearZero.Expm1(-logRatio)) + (w * corrections);
        return StirlingIncrementBody(x, s) + (corrections / x) - shifts;
    }

    /// <summary><c>I_x(a, b)</c> for <c>b</c> below SmallShape past the mode, where <c>1 − I_y(b, a)</c> cancels.</summary>
    /// <remarks>
    /// From <c>B_y(b, a) = Σ (1 − a)_n / n! · y^(n+b) / (n + b)</c>: with <c>δ = log(b B(b, a)) − b log y</c> and
    /// <c>S</c> the sum from <c>n = 1</c> over <c>y^b</c>, <c>I_x(a, b) = −expm1(−δ) − b S e^(−δ)</c>. Past the mode
    /// <c>y</c> is at most about one half, so <c>S</c> converges geometrically. <c>FisherSnedecorSf(0.5, 1e-8, 5)</c>
    /// was 1.7e-8 off at 724c7912 and a shape of 1e-300 read zero (#1690's review).
    /// </remarks>
    private static ExactTail SmallShapeLowerTail(double a, double b, DoubleDouble exactX, DoubleDouble exactY, double logScaledSecond, bool firstOrder, bool exact)
    {
        // The first order where it holds, as on the direct side, so I_x is one form across the mode; SmallFirstShape, reflecting
        // here, has tried it. Tested in RegularizedIncomplete it cost FisherSnedecorCdf(1.1, 3e6, 1e7) 3%, 68 against 66 ns.
        double x = exactX.High;
        double y = exactY.High;
        if (firstOrder)
        {
            DoubleDouble small = SmallShapesTail(a, b, x, y);
            if (!double.IsNaN(small.High))
            {
                return new ExactTail(small, inDouble: true);
            }
        }
        if (exact && y > 0.0)
        {
            return new ExactTail(ExactSmallShapeLowerTail(a, b, exactY, ExactLogScaledBeta(b, a)), inDouble: false);
        }
        double delta = CachedOrScaled(logScaledSecond, b, a) - (b * Math.Log(y));
        double head = NearZero.Expm1(delta);
        double coefficient = 1.0;
        double power = 1.0;
        double sum = 0.0;
        for (int n = 1; n <= MaxIterations; n++)
        {
            coefficient *= (n - a) / n;
            power *= y;
            double term = coefficient * power / (n + b);
            sum += term;
            if (Math.Abs(b * term) <= Epsilon * Math.Abs(head - (b * sum)))
            {
                break;
            }
        }
        // In this form, not e^(-delta) (expm1(delta) - b S), whose expm1 overflows past delta 709.8.
        return ExactTail.OfDouble(-NearZero.Expm1(-delta) - (b * sum * Math.Exp(-delta)));
    }

    /// <summary><c>log Γ(large + small) − log Γ(large)</c> from Stirling, for <c>large</c> of at least 10.</summary>
    private static double StirlingIncrement(double large, double small) =>
        StirlingIncrementBody(large, small)
            + Gamma.LogStirlingCorrection(large + small) - Gamma.LogStirlingCorrection(large);

    /// <summary><c>log(s / x)</c>, in logs where the ratio overflows at a subnormal x.</summary>
    /// <remarks><c>log(x + s) − log x</c> cancelled when both are tiny: F(1, 1e-300, 1e-300) read 0.5000000000000293 (#1690's review).</remarks>
    private static double LogRatio(double s, double x)
    {
        double ratio = s / x;
        return double.IsInfinity(ratio) ? Math.Log(s) - Math.Log(x) : Math.Log(ratio);
    }

    /// <summary>Stirling's <c>log Γ(large + small) − log Γ(large)</c> without the corrections, which each caller takes its way.</summary>
    /// <remarks>As <c>(large − ½)(log1p(r) − r) − r/2 + small log(large + small)</c>, <c>r = small / large</c>, so no term of
    /// order <c>small</c> is subtracted from another: LogGammaIncrement needs it to <c>small ε</c> (#1690's review).</remarks>
    private static double StirlingIncrementBody(double large, double small)
    {
        double ratio = small / large;
        return StirlingShift(large, ratio) + (small * Math.Log(large + small));
    }

    /// <summary><c>(large − ½)(log(1 + r) − r) − r/2</c>, r the small shape over the large: the part of Stirling's
    /// increment StirlingIncrementBody and LogShapeBetaTimesPower share, before each one's own last log.</summary>
    private static double StirlingShift(double large, double ratio) =>
        ((large - 0.5) * Gamma.LogOnePlusMinus(ratio, 1.0 + ratio)) - (0.5 * ratio);

    /// <summary><c>d₁f/(d₁f + d₂) − d₁/(d₁ + d₂)</c>, the lower half's distance from its mean, from <c>f − 1</c> exactly.</summary>
    /// <remarks>
    /// It is <c>(f − 1) (lower/f) d₂/(d₁ + d₂)</c>, products alone, where the two fractions each round through their own
    /// sum; read only where both shapes reach the uniform expansion, whose law is narrow enough for that rounding to show.
    /// The last factor is <c>1/(1 + d₁/d₂)</c>: <c>d₁ + d₂</c> overflowed at dfs of 1e308 and every tail read ½ (#1690's review).
    /// </remarks>
    internal static double FisherDeviation(double f, double dfn, double dfd, double lower) =>
        UniformBeta.Holds(dfn / 2.0, dfd / 2.0) ? (f - 1.0) * (lower / f) / (1.0 + (dfn / dfd)) : double.NaN;

    /// <summary>FisherSplit's halves to some 106 bits where d₁f is normal and the sum finite, and rounded; elsewhere its
    /// doubles, each on the side LogComplement reads. Formed once: twice it cost every F tail its split again.</summary>
    internal static (DoubleDouble ExactLower, DoubleDouble ExactUpper, double Lower, double Upper) ExactFisherSplit(double f, double dfn, double dfd)
    {
        double scaled = dfn * f;
        // A subnormal d₁f too, which ExactQuotients lifts by 2^600: the odds rounded apart moved x by ulps as f rose.
        if ((scaled >= NearZero.SmallestNormal || SplitLifts(scaled, dfd)) && !double.IsInfinity(dfd + scaled))
        {
            (DoubleDouble exactLower, DoubleDouble exactUpper) = ExactQuotients(dfn, f, dfd);
            return (exactLower, exactUpper, exactLower.Rounded, exactUpper.Rounded);
        }
        (double lower, double upper) = SplitByDoubles(f, dfn, dfd);
        return (ExactSide(lower, upper), ExactSide(upper, lower), lower, upper);
    }

    // 2^-969: from it a product's residual, 2^-106 of it, is a normal double. And 2^600, the lift below it.
    private const double ProductFloor = 2.0041683600089728e-292;
    private const double QuotientLift = 4.149515568880993e+180;
    private const int QuotientLiftExponent = 600;

    /// <summary>Whether d₂ lifted by 2^600 stays finite, so ExactQuotients can lift a small d₁f: one test for the split's
    /// gate and the quotients' lift.</summary>
    private static bool QuotientLifts(double dfd) => !double.IsInfinity(dfd * QuotientLift);

    /// <summary>Whether a subnormal d₁f takes ExactQuotients' lifted split.</summary>
    private static bool SplitLifts(double scaled, double dfd) =>
        scaled > 0.0 && scaled < NearZero.SmallestNormal && QuotientLifts(dfd);

    // e^-500 is 2^-721: a tail from a smaller exponential is held 2^600 up, so its low part stays normal through the
    // divisions that follow, 2^248 of them; from a larger one it ends above 2^-969 unlifted.
    private const double TailLiftPower = -500.0;

    /// <summary>The exponent an ExactTail holds an exponential of <paramref name="power"/> at: −600 below TailLiftPower.</summary>
    private static int TailExponent(DoubleDouble power) => power.High < TailLiftPower ? -QuotientLiftExponent : 0;

    /// <summary><c>d₁f / (d₂ + d₁f)</c> and <c>d₂ / (d₂ + d₁f)</c> to some 106 bits.</summary>
    private static (DoubleDouble Lower, DoubleDouble Upper) ExactQuotients(double dfn, double f, double dfd)
    {
        // Below ProductFloor the product's residual rounds to the subnormals, the point some 53 bits, and F's lower tail fell
        // 552 times in 20,000 ulps at dfs (0.3, 1e-307): both sums 2^600 up there, exactly (#1745's Review A).
        if (dfn * f < ProductFloor && QuotientLifts(dfd))
        {
            dfd *= QuotientLift;
            f *= QuotientLift;
        }
        DoubleDouble exactScaled = DoubleDouble.Product(dfn, f);
        DoubleDouble exactDenominator = new DoubleDouble(dfd, 0.0) + exactScaled;
        // The near-one side as one less the other: as its own quotient its low part lost the smaller term past 2^-106 of the
        // sum, (1, 0) at f = 2.549e30, dfs (2001, 20), and F's upper tail fell back to the double form (#1745's Review A).
        if (exactScaled.High <= dfd)
        {
            DoubleDouble lower = exactScaled / exactDenominator;
            return (lower, new DoubleDouble(1.0, 0.0) - lower);
        }
        DoubleDouble upper = new DoubleDouble(dfd, 0.0) / exactDenominator;
        return (new DoubleDouble(1.0, 0.0) - upper, upper);
    }

    /// <summary><c>y = d₁f / (d₁f + d₂)</c> and <c>1 − y</c>, each by division so neither is one minus the other.</summary>
    /// <remarks>
    /// Where <c>d₁f</c> or the sum overflows, both come from <c>r = (d₂/d₁)/f</c> as <c>1/(1 + r)</c> and
    /// <c>r/(1 + r)</c>, which is <c>(1, 0)</c> at <c>f = +∞</c>: forming <c>∞/∞</c> there made the lower
    /// tail throw and the density NaN (#1158's review). Where <c>d₁f</c> is subnormal, both come from the odds
    /// <c>d₁f/d₂</c> when those are normal, since <c>d₁f</c> itself keeps too few digits (#1725).
    /// </remarks>
    internal static (double Lower, double Upper) FisherSplit(double f, double dfn, double dfd)
    {
        // ExactFisherSplit's, rounded: one gate decides the 106-bit split, so every reader takes one split (#1745's Review A).
        (_, _, double lower, double upper) = ExactFisherSplit(f, dfn, dfd);
        return (lower, upper);
    }

    /// <summary>The split ExactQuotients does not form: from the odds where a subnormal d₁f is not lifted, by the
    /// quotients where its sum is finite, else from the ratio of the dfs.</summary>
    private static (double Lower, double Upper) SplitByDoubles(double f, double dfn, double dfd)
    {
        double scaled = dfn * f;
        // A subnormal d1 f quantises y, 6.9% off at (5e-324, 3.7, 1e-300) (#1725): y/(1 − y) = d1 f/d2 where it is
        // normal (#1729).
        if (scaled < NearZero.SmallestNormal)
        {
            // f/d2 can itself be subnormal and lose digits, or overflow at a tiny d2; d1/d2 is a number in both (#1725, #1729).
            double quotient = f / dfd;
            double odds = quotient >= NearZero.SmallestNormal && quotient < double.PositiveInfinity ? dfn * quotient : dfn / dfd * f;
            if (odds >= NearZero.SmallestNormal && odds < double.PositiveInfinity)
            {
                return (odds / (1.0 + odds), 1.0 / (1.0 + odds));
            }
        }
        if (!double.IsInfinity(dfd + scaled))
        {
            // Each rounded once from 106 bits, so each moves with f: rounding d1 f and d2 + d1 f apart, x stepped back an
            // ulp as f rose, and F's tail with it at dfs (30, 0.001) (#1745).
            (DoubleDouble exactLower, DoubleDouble exactUpper) = ExactQuotients(dfn, f, dfd);
            return (exactLower.Rounded, exactUpper.Rounded);
        }
        // In the other order where dfd / dfn alone overflows, to inf or, at f = +inf, inf / inf: (1e308, 0.5, 1.5e308) read
        // (0, NaN). The sum overflowing, the ratio is at most about 1e308 / (dfn f), so one of the two orders is a number.
        double ratio = dfd / dfn / f;
        if (!(ratio < double.PositiveInfinity))
        {
            ratio = dfd / f / dfn;
        }
        return (1.0 / (1.0 + ratio), ratio / (1.0 + ratio));
    }

    /// <summary>The t with <c>P(T &gt; t) = p</c>: the inverse of <see cref="StudentSf(double, double)"/>.</summary>
    /// <remarks>
    /// The root of <see cref="StudentSf(double, double)"/> itself, reached by <see cref="TailInversion"/>
    /// rather than read off a rational approximation of its own: the seed only decides how
    /// many tail evaluations Newton needs, so there is no second approximation to keep in
    /// agreement with the tail. Bisection took about sixty tails. From df 1e20 it
    /// is the normal quantile, scipy's ndtri, as at an infinite df: not Normal.Sf's root, by up to 1e-13.
    /// </remarks>
    internal static double StudentUpperRoot(double p, double df) => StudentRoot(p, df, upper: true);

    /// <summary>The t with <c>P(T ≤ t) = p</c>, the smallest double whose lower tail reaches p: the mirror of
    /// <see cref="StudentUpperRoot(double, double)"/>'s smallest double whose upper tail falls to it, one ulp apart where no
    /// double's tail is p, as F's quantile and inverse tail are (#1745's Review A).</summary>
    internal static double StudentLowerRoot(double p, double df) => StudentRoot(p, df, upper: false);

    /// <summary>Whether an upper tail <paramref name="excess"/> past its target has fallen to it, or strictly past it: the
    /// Newton start's and the bit search's one tie rule.</summary>
    private static bool FallenTo(double excess, bool strictly) => strictly ? excess < 0.0 : excess <= 0.0;

    private static double StudentRoot(double p, double df, bool upper)
    {
        RequireStudentArguments(p, df);
        // S1244: an exact half is the distribution's own symmetric point, not a
        // value that drifted there by rounding -- StudentSf(0, df) is exactly
        // 0.5 for every df, so the shortcut is correct only at that literal.
#pragma warning disable S1244
        if (p == 0.5)
#pragma warning restore S1244
        {
            return 0.0;
        }
        double closed = StudentClosedRoot(p, df);
        if (!double.IsNaN(closed))
        {
            return upper ? closed : -closed;
        }

        // Symmetric about zero, so p > 1/2 reduces to its mirror 1 - p < 1/2: the
        // inversion's lower bound of zero holds only on the positive side.
        double tail = p > 0.5 ? 1.0 - p : p;
        var student = new StudentTail(df);
        // The tail at double.MaxValue above p puts the root past every double, where Newton settled on non-roots
        // (#1690's review); that tail is at most x^a / 2, x = df / MaxValue^2, so a larger p skips evaluating it.
        bool mayBePast = df < FarRootMaximumDf
            && Math.Log(tail) < (df / 2.0 * (Math.Log(df) - LogSquaredMaxValue)) - Ln2 + 1e-9;
        // The exact tail there, as the search judges it: the double forms', above it, put a root the largest double reaches
        // past every double (#1745's Review A).
        ExactTail? atLargest = mayBePast ? student.ExactUpperTail(double.MaxValue) : null;
        double t = atLargest?.Less(tail) > 0.0
            ? double.PositiveInfinity
            : TailInversion.InvertUpperTail(student, tail, student.Seed(tail));
        // The side reading the smaller target is the smallest double whose upper tail falls to it; the mirrored side the
        // largest whose tail is still at it, the double below the first strictly past it, or the inversion's own run.
        bool direct = (p < 0.5) == upper;
        t = SettledStudentRoot(t, tail, student, strictly: !direct, atLargest, out bool inRun);
        if (direct)
        {
            return t;
        }
        return inRun ? -t : -TailInversion.NextDown(t);
    }

    /// <summary>StudentRoot's refusals: a probability outside (0, 1), or a df not positive.</summary>
    private static void RequireStudentArguments(double p, double df)
    {
        if (double.IsNaN(p) || p <= 0.0 || p >= 1.0)
        {
            throw new ArgumentOutOfRangeException(
                nameof(p), p, "The tail probability must lie strictly inside (0, 1).");
        }
        if (double.IsNaN(df) || df <= 0.0)
        {
            throw new ArgumentOutOfRangeException(
                nameof(df), df, "The degrees of freedom must be positive.");
        }
    }

    /// <summary>Student's upper tail from <see cref="NormalLimitDf"/>: <c>Φ̄(t) + φ(t)(t³ + t)/(4 df)</c>.</summary>
    private static double NormalLimitSf(double t, double df)
    {
        double correction = Distributions.NormalPdf(t) * ((t * t * t) + t) / (4.0 * df);
        // At an infinite t the density's zero times t³ is NaN; the correction is zero there, as at an infinite df.
        return double.IsNaN(correction) ? Normal.Sf(t) : Normal.Sf(t) + correction;
    }

    /// <summary>The upper root in the two limits StudentSf takes in closed form, NaN between them: the normal law's inverse
    /// tail, and FirstOrderSf's inverse, sqrt(df) sinh(2 (1/2 - p) / df), whose sinh overflows to the infinity past every
    /// double (#1690's review).</summary>
    private static double StudentClosedRoot(double p, double df)
    {
        if (df >= NormalLimitDf)
        {
            return -Ndtri.Of(p);
        }
        return df <= FirstOrderDf ? FirstOrderRoot(2.0 * (0.5 - p) / df, df) : double.NaN;
    }

    /// <summary>The smallest t whose upper tail reaches <paramref name="tail"/>, searched near the inversion's
    /// <paramref name="t"/>, which is kept where its tail is the target or no crossing lies near (#1745).</summary>
    private static double SettledStudentRoot(double t, double tail, in StudentTail student, bool strictly, ExactTail? atLargest, out bool inRun)
    {
        inRun = true;
        if (!(t > 0.0 && t <= double.MaxValue))
        {
            return t;
        }
        ExactTail exact = student.ExactUpperTail(t);
        double kept = exact.Rounded;
        // A subnormal target keeps a root whose tail is subnormal too, as FisherQuantile.Invert does and for its reason.
        if (tail < NearZero.SmallestNormal && kept < NearZero.SmallestNormal)
        {
            return t;
        }
        // Newton steps on that tail unrounded, the upper tail falling at the density, land within an ulp or two of the edge.
        var newton = new StudentNewton(student, tail, upward: !strictly);
        var reaches = new StudentReaches(student, tail, strictly);
        long nearest = TailInversion.Settled(t, exact, newton.Reaches(exact), ref newton, ref reaches);
        if (nearest < 0)
        {
            // None reaches, or none strictly passes: the largest, its tail (StudentRoot's where read) still the target, as
            // F's OrBelowTheLargest has it; else the root lies past every double (#1745's Review A).
            return strictly && (atLargest ?? student.ExactUpperTail(double.MaxValue)).Less(tail) <= 0.0 ? double.MaxValue : double.PositiveInfinity;
        }
        inRun = false;
        return BitConverter.Int64BitsToDouble(nearest);
    }

    /// <summary>Student's exact upper tail at a positive t, and a Newton step on it toward <c>tail</c>, reached at it
    /// where <c>upward</c> holds, strictly past it otherwise.</summary>
    private readonly struct StudentNewton(StudentTail student, double tail, bool upward) : TailInversion.INewtonTail
    {
        private readonly StudentTail _student = student;
        private readonly double _tail = tail;
        private readonly bool _upward = upward;

        public ExactTail Exact(double statistic) => _student.ExactUpperTail(statistic);

        // Over t times the density, which stays a double where the density underflows near the largest double, and the
        // step never moved: StudentIsf(StudentSf(MaxValue, 0.05), 0.05) kept a root short of the target (#1745's Review A).
        public double Stepped(double statistic, ExactTail exact) => TailInversion.EdgeStep(
            statistic, exact.Less(_tail), Math.Exp(_student.LogDensity(statistic) + Math.Log(statistic)), risingTail: false);

        public bool Reaches(ExactTail exact) => FallenTo(exact.Less(_tail), strictly: !_upward);
    }

    /// <summary>Whether Student's upper tail at a double's bits has fallen to the target, or strictly past it.</summary>
    private readonly struct StudentReaches(StudentTail student, double tail, bool strictly) : TailInversion.IBitTest
    {
        private readonly StudentTail _student = student;
        private readonly double _tail = tail;
        private readonly bool _strictly = strictly;

        public bool Holds(long bits)
        {
            double excess = _student.ExactUpperTail(BitConverter.Int64BitsToDouble(bits)).Less(_tail);
            return FallenTo(excess, _strictly);
        }
    }

    /// <summary>The log of Student's density constant, <c>−log(√df B(½, df/2))</c>, in double-double: below SmallShape
    /// from <c>log(s B(s, ½))</c>, as StudentTail's.</summary>
    internal static DoubleDouble ExactStudentLogNormalizer(double df)
    {
        double half = df / 2.0;
        DoubleDouble halfLogDf = 0.5 * DoubleDouble.Log(df);
        // A subnormal half keeps too few digits to log, and log(s B(s, ½)), of order s, is under 1e-300 there.
        if (half < NearZero.SmallestNormal)
        {
            return halfLogDf - DoubleDouble.Log(2.0);
        }
        return half < SmallShape
            ? DoubleDouble.Log(half) - halfLogDf - ExactLogScaledBeta(half, 0.5)
            : -(DoubleDouble.LogBeta(0.5, half) + halfLogDf);
    }

    /// <summary><see cref="StudentLogOnePlusSquare"/> in double-double, the ratio's logarithms apart where it overflows.</summary>
    internal static DoubleDouble ExactStudentLogOnePlusSquare(double t, double df)
    {
        double ratio = t / df * t;
        if (double.IsInfinity(ratio))
        {
            DoubleDouble logSquare = (2.0 * DoubleDouble.Log(Math.Abs(t))) - DoubleDouble.Log(df);
            return logSquare + new DoubleDouble(NearZero.Log1P(1.0 / ratio), 0.0);
        }
        // t² past the doubles, or below the normal ones, with t²/df within them: the product read (∞, −∞) at StudentPdf(1.36e154,
        // 1.05), 0, and kept a few bits at StudentPdf(2e-162, 5e-324), 4.9% off (#1746's Review A).
        double square = t * t;
        if (double.IsInfinity(square) || square < NearZero.SmallestNormal)
        {
            return DoubleDouble.Log1P(t * (new DoubleDouble(t, 0.0) / new DoubleDouble(df, 0.0)));
        }
        return DoubleDouble.Log1P(DoubleDouble.Product(t, t) / new DoubleDouble(df, 0.0));
    }

    /// <summary><c>log(1 + t²/df)</c>, which Student's inversion reads; the density and the public tail read ExactStudentLogOnePlusSquare.</summary>
    /// <remarks>
    /// <c>log1p(t/df · t)</c> as 724c7912's density took it, and <c>2 log(|t|/√df)</c> where that ratio overflows, which read
    /// the density 0 (#1690's review). A series and <c>log(1 + r²)</c> the inversion had lost 5e-11 at df 1e7.
    /// </remarks>
    internal static double StudentLogOnePlusSquare(double t, double df)
    {
        double ratio = t / df * t;
        return double.IsInfinity(ratio)
            ? 2.0 * (Math.Log(Math.Abs(t)) - (0.5 * Math.Log(df)))
            : NearZero.Log1P(ratio);
    }

    /// <summary>Student's upper tail and log density, and a seed for inverting it.</summary>
    internal readonly struct StudentTail : IUpperTail
    {
        private readonly double _df;

        // The density's constant, from the log B below rather than two log-gammas of its own (#1690). Four fields at
        // most: a fifth, caching half log df, stopped the JIT promoting the struct and cost 6% at df 3 (Stopwatch A/B/A/B).
        private readonly double _logNormalizer;

        // log B(df/2, 1/2), which every step's tail evaluation took afresh (#1686), and log(df/2 B) below SmallShape.
        private readonly double _logBeta;
        private readonly double _logScaledBeta;

        internal StudentTail(double df)
        {
            _df = df;
            // Below SmallShape StudentLogScaledBeta's form for the double-form steps; log B is read there only on the fraction's
            // reflected side, which takes it itself. Both cost a quantile below df 0.2 1.4 times 724c7912's (#1690's review).
            bool small = df / 2.0 < SmallShape;
            _logScaledBeta = small ? StudentLogScaledBeta(df / 2.0) : double.NaN;
            _logBeta = small ? double.NaN : LogBeta(df / 2.0, 0.5);
            double halfLogDf = 0.5 * Math.Log(df);
            // Below SmallShape as ½ log df − log 2 − log(a B): df/2 of a subnormal df rounds to zero, and log B of it threw
            // (#1690's review).
            _logNormalizer = double.IsNaN(_logScaledBeta)
                ? -_logBeta - halfLogDf
                : halfLogDf - Ln2 - _logScaledBeta;
        }

        public double Survival(double x) => StudentSf(x, _df, _logBeta, _logScaledBeta, exact: false);

        /// <summary>The upper tail at a positive <paramref name="x"/> before its last rounding, from the cached logarithms.</summary>
        internal ExactTail ExactUpperTail(double x) => TailBeyond(x, _df, _logBeta, _logScaledBeta, exact: true);

        public double LogDensity(double x) => _logNormalizer - (0.5 * (_df + 1.0) * StudentLogOnePlusSquare(x, _df));

        /// <summary>Where Newton starts for p in (0, 0.5); its accuracy costs steps, not digits.</summary>
        /// <remarks>
        /// df = 1 and df = 2 have closed forms. Otherwise the Cornish-Fisher expansion in 1/df
        /// about the normal quantile (Abramowitz and Stegun 26.7.5) while z^2 &lt;= df, and past
        /// that the power-law tail Sf(t) ~ exp(logNormalizer) df^((df-1)/2) t^-df solved for t,
        /// which bounds the root from above and so also caps an overshooting expansion. Over 24
        /// df in [0.01, 1e12] by 36 p in [5e-324, 1 - 1e-16] this averages 1.84 tail evaluations
        /// for 1 &lt;= df &lt;= 1e6 and 1e-20 &lt;= p &lt;= 0.49, four at most.
        /// </remarks>
        internal double Seed(double p)
        {
            // S1244: the two closed forms hold at these exact degrees of freedom only.
#pragma warning disable S1244
            if (_df == 1.0)
            {
                return 1.0 / Math.Tan(Math.PI * p);
            }
            if (_df == 2.0)
            {
                return (1.0 - (2.0 * p)) / Math.Sqrt(2.0 * p * (1.0 - p));
            }
#pragma warning restore S1244

            double z = Normal.RationalUpperQuantile(p);
            double z2 = z * z;
            double g1 = z * (z2 + 1.0) / 4.0;
            double g2 = z * ((((5.0 * z2) + 16.0) * z2) + 3.0) / 96.0;
            double g3 = z * ((((((3.0 * z2) + 19.0) * z2) + 17.0) * z2) - 15.0) / 384.0;
            double g4 = z * ((((((((79.0 * z2) + 776.0) * z2) + 1482.0) * z2) - 1920.0) * z2) - 945.0)
                / 92160.0;
            double inverse = 1.0 / _df;
            double expansion = z + (inverse * (g1 + (inverse * (g2 + (inverse * (g3 + (inverse * g4)))))));

            double powerLaw = Math.Exp(
                (_logNormalizer + (0.5 * (_df - 1.0) * Math.Log(_df)) - Math.Log(p)) / _df);

            return z2 <= _df && expansion > 0.0 && expansion < powerLaw ? expansion : powerLaw;
        }
    }

    // Every length over a + b past FractionScaleShape, where (a + m)(a + b + m) overflowed into inf / inf and F's tail
    // read NaN at shapes (5e179, 147) (#1690's review). Two inlined copies, so the common one folds its scale of 1 away.
    private static double ContinuedFraction(double a, double b, double x)
    {
        if (a + b > FractionScaleShape)
        {
            double scale = 1.0 / (a + b);
            return Lentz(a * scale, b * scale, x, scale);
        }
        return Lentz(a, b, x, 1.0);
    }

    /// <summary>The fraction by modified Lentz, every length in units of <paramref name="scale"/>.</summary>
    /// <remarks>
    /// CF = 1 + d1/(1 + d2/(1 + ...)) from Abramowitz &amp; Stegun 26.5.8, evaluated directly (b0 = 1, every later
    /// b_i = 1) rather than a reciprocal restatement.
    /// </remarks>
    [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
    private static double Lentz(double scaledA, double scaledB, double x, double scale)
    {
        // The first iteration from c = 1, d = 0. Its term at m = 0 cancels a where a (a + b) x is subnormal: it quantised,
        // and FisherSnedecorSf(1e-318, 20, 1e-320) read 5.7e-5 off (#1729). a (a + 1) is the larger below the mode.
        double head = -scaledA * (scaledA + scaledB) * x;
        // Elsewhere as the loop forms it: cancelling everywhere moved 811 of 139,000 random F and t values by up to 1e-13,
        // and of 60 such F tails 38 came nearer a 1000-digit sum and 22 went further, rounding rather than digits.
        double c = 1.0 + (head <= -NearZero.SmallestNormal
            ? head / (scaledA * (scaledA + scale))
            : -(scaledA + scaledB) * x / (scaledA + scale));
        if (Math.Abs(c) < Tiny)
        {
            c = Tiny;
        }
        double d = 1.0;
        double h = c;
        if (Math.Abs(c - 1.0) < Epsilon)
        {
            return h;
        }

        for (int i = 2; i <= MaxIterations; i++)
        {
            double m = (i / 2) * scale;

            // The numerators alternate between the two forms with the term's
            // parity; folding both into one loop keeps the recurrence's state in one place.
            double numerator = i % 2 == 0
                ? m * (scaledB - m) * x / ((scaledA + (2.0 * m) - scale) * (scaledA + (2.0 * m)))
                : -(scaledA + m) * (scaledA + scaledB + m) * x / ((scaledA + (2.0 * m)) * (scaledA + (2.0 * m) + scale));

            d = 1.0 + (numerator * d);
            if (Math.Abs(d) < Tiny)
            {
                d = Tiny;
            }
            d = 1.0 / d;

            c = 1.0 + (numerator / c);
            if (Math.Abs(c) < Tiny)
            {
                c = Tiny;
            }

            double delta = c * d;
            h *= delta;

            if (Math.Abs(delta - 1.0) < Epsilon)
            {
                break;
            }
        }

        return h;
    }

    // 2π as two doubles.
    private static readonly DoubleDouble TwoPi = new(6.283185307179586, 2.4492935982947064e-16);

    // The harmonic shape up to which the fraction is taken in double-double, its cost still under scipy's.
    private const double ExactFractionShapes = 1000.0;

    // The shapes' sum up to which Temme's exponent is taken from the point in double-double.
    internal const double ExactExponentShapes = 1e15;

    // The double-double fraction stops where a step moves it by less than this: under the a·ε of 1e-24 a shape of 1e-8
    // moves its tail between neighbouring f, so the count of terms changes without a visible step; 1e-30 cost 13% more.
    private const double ExactEpsilon = 1e-26;

    /// <summary>A point as a double-double: itself up to one half, past it one less its complement, exactly, as
    /// LogComplement reads them.</summary>
    private static DoubleDouble ExactSide(double value, double complement) =>
        value <= complement ? new DoubleDouble(value, 0.0) : DoubleDouble.Sum(1.0, -complement);

    /// <summary>The fraction in double-double up to a harmonic shape <c>ab/(a + b)</c> of ExactFractionShapes, past it in
    /// double.</summary>
    /// <remarks>Past FractionScaleShape in the sum only the double fraction rescales its lengths: the double-double one's
    /// products overflowed to NaN at a df of 1e180 and ran a million terms, 37 ms. Its terms grow as the square root of
    /// that shape near the mode: in double-double F's tail at dfs (1.9e5, 1.9e5) took 30 us, past scipy's 20 (#1745's
    /// Review A). There ByFraction takes main's double forms whole, front included. The harmonic shape formed alike from
    /// either side of the mode, so both sides take one precision.</remarks>
    private static DoubleDouble FractionFor(double a, double b, DoubleDouble x) =>
        FractionIsExact(a, b)
            ? ExactFraction(a, b, x)
            : new DoubleDouble(ContinuedFraction(a, b, x.High), 0.0);

    /// <summary>Whether F's tail at these dfs, by the fraction, keeps its double: past a harmonic shape of 1000, below
    /// Temme's shapes (Cyril, 2026-10-27, #1745's Review A).</summary>
    internal static bool FisherFractionIsDouble(double dfn, double dfd)
    {
        double a = dfn / 2.0;
        double b = dfd / 2.0;
        return HarmonicShape(a, b) > ExactFractionShapes && !UniformBeta.Holds(a, b);
    }

    /// <summary>Whether FractionFor takes the fraction to 106 bits at these shapes.</summary>
    private static bool FractionIsExact(double a, double b) =>
        HarmonicShape(a, b) <= ExactFractionShapes && a + b <= FractionScaleShape;

    /// <summary><c>ab/(a + b)</c>, one form for both deciders above, so they agree at ExactFractionShapes.</summary>
    private static double HarmonicShape(double a, double b) => Math.Min(a, b) * (Math.Max(a, b) / (a + b));

    /// <summary><see cref="ContinuedFraction"/> in double-double.</summary>
    private static DoubleDouble ExactFraction(double a, double b, DoubleDouble x)
    {
        // The convergents A/B of 1 + a₁/(1 + a₂/(1 + ⋯)) by their forward recurrence, rescaled by powers of two and compared
        // every fourth term: modified Lentz's three divisions a term took F's tail to 2.3 us, past scipy's 20 under load.
        DoubleDouble one = new(1.0, 0.0);
        FractionTerms terms = FractionTerms.For(a, b);
        // A₀ = B₀ = 1, and A₁ = 1 + a₁ over B₁ = 1.
        DoubleDouble olderA = one;
        DoubleDouble olderB = one;
        DoubleDouble newerA = one - (x * terms.First);
        DoubleDouble newerB = one;
        DoubleDouble value = newerA;
        DoubleDouble previousR = one;
        for (int i = 2; i <= MaxIterations; i++)
        {
            (DoubleDouble r, DoubleDouble coefficient) = terms.At(i, previousR);
            previousR = r;
            DoubleDouble older = x * coefficient;
            (olderA, newerA) = (newerA, (r * newerA) + (older * olderA));
            (olderB, newerB) = (newerB, (r * newerB) + (older * olderB));
            if (i % 4 == 3 && Math.Abs(newerB.High) > 0.0)
            {
                DoubleDouble next = newerA / newerB;
                bool settled = Math.Abs((next - value).High) <= ExactEpsilon * Math.Abs(next.High);
                value = next;
                if (settled)
                {
                    break;
                }
                // Back toward one by a power of two, exactly, so neither convergent leaves the doubles; at most 2^1000 a
                // check, so a subnormal denominator's scale stays finite, where past 2^1023 it read +∞.
                double scale = PowerOfTwo.Of(Math.Min(-PowerOfTwo.Split(Math.Abs(newerB.High)).Exponent, 1000));
                olderA = olderA.Scaled(scale);
                newerA = newerA.Scaled(scale);
                olderB = olderB.Scaled(scale);
                newerB = newerB.Scaled(scale);
            }
        }
        return value;
    }

    /// <summary>ExactFraction's terms at shapes a and b, apart from the point: each term's denominator multiplies its step
    /// instead of dividing its numerator, <c>A_i = r_i A_(i−1) + r_(i−1) r_i a_i A_(i−2)</c>, each r_i scaled by a power of
    /// two.</summary>
    /// <remarks>Kept per thread for the last shapes, which a quantile's two or three exact tails share: built per tail,
    /// the terms took most of F's quantile at dfs (4000, 4000), 30 us, past scipy's 26 (#1745).</remarks>
    private sealed class FractionTerms
    {
        // The terms kept, 128 KB a thread at most; past them each is built per tail, as a fraction that long is.
        private const int Capacity = 4096;

        [ThreadStatic]
        private static FractionTerms? perThread;

        private double _a = double.NaN;
        private double _b = double.NaN;
        private DoubleDouble _sum;
        private int _count;
        private DoubleDouble[] _r = new DoubleDouble[64];
        private DoubleDouble[] _coefficients = new DoubleDouble[64];

        /// <summary><c>(a + b)/(a + 1)</c>, the first numerator over the point.</summary>
        internal DoubleDouble First { get; private set; }

        /// <summary>The thread's terms, emptied where the shapes are not those it holds.</summary>
        internal static FractionTerms For(double a, double b)
        {
            FractionTerms terms = perThread ??= new FractionTerms();
            if (!ShapeKey.Matches(a, b, terms._a, terms._b))
            {
                terms._a = a;
                terms._b = b;
                terms._sum = DoubleDouble.Sum(a, b);
                terms.First = terms._sum / DoubleDouble.Sum(a, 1.0);
                terms._count = 0;
            }
            return terms;
        }

        /// <summary>Term <paramref name="i"/>, from 2: r_i and <c>r_(i−1) r_i a_i</c> over the point, r_(i−1) being
        /// <paramref name="previousR"/>.</summary>
        internal (DoubleDouble R, DoubleDouble Coefficient) At(int i, DoubleDouble previousR)
        {
            int index = i - 2;
            if (index < _count)
            {
                return (_r[index], _coefficients[index]);
            }
            (DoubleDouble r, DoubleDouble coefficient) = Build(i, previousR);
            if (index < Capacity)
            {
                if (index == _r.Length)
                {
                    Array.Resize(ref _r, 2 * index);
                    Array.Resize(ref _coefficients, 2 * index);
                }
                _r[index] = r;
                _coefficients[index] = coefficient;
                _count = index + 1;
            }
            return (r, coefficient);
        }

        private (DoubleDouble R, DoubleDouble Coefficient) Build(int i, DoubleDouble previousR)
        {
            DoubleDouble one = new(1.0, 0.0);
            double m = (double)(i / 2);
            DoubleDouble twoM = DoubleDouble.Sum(_a, 2.0 * m);
            (DoubleDouble numerator, DoubleDouble denominator) = i % 2 == 0
                ? (m * DoubleDouble.Sum(_b, -m), (twoM - one) * twoM)
                : (-(DoubleDouble.Sum(_a, m) * (_sum + new DoubleDouble(m, 0.0))), twoM * (twoM + one));
            double unit = PowerOfTwo.Of(-PowerOfTwo.Split(denominator.High).Exponent);
            return (denominator.Scaled(unit), previousR * numerator.Scaled(unit));
        }
    }

    /// <summary>SmallShapeLowerTail's series in double-double, <paramref name="logScaled"/> being <c>log(b B(b, a))</c>.</summary>
    private static DoubleDouble ExactSmallShapeLowerTail(double a, double b, DoubleDouble y, DoubleDouble logScaled)
    {
        DoubleDouble one = new(1.0, 0.0);
        DoubleDouble delta = logScaled - (b * DoubleDouble.Log(y));
        // −expm1(−δ) itself, not one less e^(−δ): a δ of 1e-200 kept one double's digits through the difference.
        DoubleDouble head = -DoubleDouble.Expm1Exact(-delta);
        DoubleDouble decay = one - head;
        DoubleDouble coefficient = one;
        DoubleDouble power = one;
        // The sum times b as it goes, so the stopping test takes no product of its own each term.
        DoubleDouble scaledSum = new(0.0, 0.0);
        for (int n = 1; n <= MaxIterations; n++)
        {
            coefficient *= DoubleDouble.Sum(n, -a) / new DoubleDouble(n, 0.0);
            power *= y;
            DoubleDouble scaledTerm = b * (coefficient * power / DoubleDouble.Sum(n, b));
            scaledSum += scaledTerm;
            // Against the value returned, the sum scaled by the decay: against head less the sum it ran to underflow, or
            // stopped early, where the two part by δ² (#1746's Review A).
            DoubleDouble value = head - (scaledSum * decay);
            if (Math.Abs(scaledTerm.High) * decay.High <= ExactEpsilon * Math.Abs(value.High))
            {
                return value;
            }
        }
        return head - (scaledSum * decay);
    }

    /// <summary><see cref="LargeShapeExpansion"/> in double-double: −log x, the incomplete gamma at <c>N(−log x)</c> and its
    /// steps, and the sum to some 106 bits, the coefficients, of order <c>(b/N)²</c> of it, in double.</summary>
    private static ExactTail ExactLargeShapeExpansion(double a, double b, DoubleDouble x, DoubleDouble complement, bool reflected)
    {
        DoubleDouble one = new(1.0, 0.0);
        double n = a + (0.5 * (b - 1.0));
        DoubleDouble z = MinusLogOneLess(x, complement);
        // N's rounding times z is w's, which Q(b, w) reads at its slope: N in double-double.
        DoubleDouble w = (new DoubleDouble(a, 0.0) + (0.5 * DoubleDouble.Sum(b, -1.0))) * z;
        double logRatio = LogShapeRatio(n, 0.5 * b);
        bool upper = ExpansionTakesQ(b, w.High);
        DoubleDouble r = ExactRegularizedGamma(b, w, upper, out DoubleDouble step, out bool inDouble, out int exponent);
        step /= new DoubleDouble(b, 0.0);
        // b + 2k to 106 bits: as a double each step carried its ε, and the sum with it.
        DoubleDouble shape = new(b, 0.0);

        Span<double> coefficients = stackalloc double[MaxExpansionTerms];
        coefficients[0] = 1.0;
        double halfExponent = 0.5 * (b - 1.0);
        double inverseN2 = 1.0 / (n * n);
        double pochhammer = 1.0;
        DoubleDouble sum = r;
        for (int k = 1; k < MaxExpansionTerms; k++)
        {
            for (int half = 0; half < 2; half++)
            {
                r = upper ? r + step : r - step;
                shape += new DoubleDouble(1.0, 0.0);
                step *= w / shape;
            }
            coefficients[k] = ExpansionCoefficient(coefficients, k, halfExponent);
            pochhammer *= PochhammerStep(b, k, inverseN2);
            DoubleDouble term = (coefficients[k] * pochhammer) * r;
            sum += term;
            if (Math.Abs(term.High) <= ExactEpsilon * Math.Abs(sum.High))
            {
                break;
            }
        }
        DoubleDouble direct = DoubleDouble.ExpExact(new DoubleDouble(logRatio, 0.0)) * sum;
        if (upper == reflected)
        {
            return new ExactTail(one - (exponent == 0 ? direct : DoubleDouble.ScaledOnce(direct, exponent)), inDouble);
        }
        return new ExactTail(direct, inDouble, exponent);
    }

    /// <summary><c>−log x</c>, x being one less <paramref name="complement"/>: below LogExcessSeriesReach its series
    /// <c>y + y²/2 + ⋯</c>, whose y keeps the bits a point near one leaves in its low part alone: FisherSnedecorSf(10, 1, dfd)
    /// drifted 4 ulps either side of erfc(√5) as dfd went from 1e20 to 1e300 (#1745's Review A).</summary>
    private static DoubleDouble MinusLogOneLess(DoubleDouble x, DoubleDouble complement)
    {
        if (!(complement.High <= LogExcessSeriesReach))
        {
            return -DoubleDouble.Log(x);
        }
        var terms = new LogOneLessTerms(complement);
        return SummedSeries(complement, 2, ref terms);
    }

    // Below it a shape under SmallShape takes the series past a + 1 too: in double-double Legendre's fraction near x = 1
    // took 8.4 us of FisherSnedecorSf(2.1e9, 1e-9, 1e300)'s 8.7, the series 3, the same tail to its last bit up to x = 12.
    private const double SmallShapeSeriesReach = 3.0;

    // Below it a shape takes that series: from SmallShape the fraction past a + 1, slow there, took FisherSnedecorIsf(0.975,
    // 1e300, 0.25) to 36 us, scipy's 30 (#1745's Review A). Below 3 Q is over e^-3, so its −expm1 form cancels nothing.
    private const double SmallShapeSeriesShape = 1.0;

    /// <summary>log Γ(½) as DoubleDouble.LogGamma has it, so a tail reads the same bits either way; in its own class, so
    /// Beta's initializer, which the double forms' callers run, does not build DoubleDouble's tables.</summary>
    private static class HalfLogGamma
    {
        internal static readonly DoubleDouble Value = DoubleDouble.LogGamma(new DoubleDouble(0.5, 0.0));
    }

    /// <summary>log Γ(a) in double-double, the last shape's on this thread, as LogBeta holds its pair: a quantile's tails
    /// at one shape each formed it again (#1745's Review A).</summary>
    private static DoubleDouble CachedLogGamma(double a)
    {
        if (ShapeKey.Matches(a, a, gammaShape, gammaShape))
        {
            return cachedLogGamma;
        }
        DoubleDouble value = DoubleDouble.LogGamma(new DoubleDouble(a, 0.0));
        (gammaShape, cachedLogGamma) = (a, value);
        return value;
    }

    [ThreadStatic]
    private static double gammaShape;

    [ThreadStatic]
    private static DoubleDouble cachedLogGamma;

    /// <summary>P(a, x), or Q where <paramref name="upper"/> holds, in double-double, with x^a e^-x / Γ(a).</summary>
    /// <remarks>The series below x = a + 1, Legendre's fraction past it by modified Lentz, as Gamma takes them.</remarks>
    private static DoubleDouble ExactRegularizedGamma(
        double a, DoubleDouble x, bool upper, out DoubleDouble prefactor, out bool inDouble, out int exponent)
    {
        inDouble = false;
        DoubleDouble one = new(1.0, 0.0);
        // S1244: Student's shape is one half exactly, whose log Γ is taken once rather than on every tail.
#pragma warning disable S1244
        DoubleDouble logGamma = a == 0.5 ? HalfLogGamma.Value : CachedLogGamma(a);
#pragma warning restore S1244
        DoubleDouble power = (a * DoubleDouble.Log(x)) - x - logGamma;
        bool smallShape = x.High < Math.Max(a + 1.0, SmallShapeSeriesReach) && a < SmallShapeSeriesShape;
        bool temme = !smallShape && a >= ExactTemmeShape && Gamma.IsTemmeRegion(a, x.High);
        // A side the prefactor scales, lifted with it, its result and the prefactor at one exponent: the series' and the small
        // shape's lower side, the fraction's upper. Not Temme's: at its region's edge its tail, e^(−0.057 a), is normal to 1.2e4.
        bool scaled = !temme && (upper == !(smallShape || x.High < a + 1.0));
        exponent = scaled ? TailExponent(power) : 0;
        prefactor = DoubleDouble.ExpExact(power, -exponent);
        if (smallShape)
        {
            return ExactSmallShapeGamma(a, x, upper, -exponent);
        }
        if (temme)
        {
            return ExactTemme(a, x, upper);
        }
        if (x.High < a + 1.0)
        {
            DoubleDouble lower = prefactor * ExactGammaSeries(a, x);
            return upper ? one - lower : lower;
        }
        // A prefactor below the doubles leaves no tail, before the fraction, whose test past an x of 1e292 the subnormal low
        // parts never met: a million terms, 70 ms at FisherSnedecorSf(1e300, 1, 1e300) (#1745's Review A).
        if (!(prefactor.High > 0.0))
        {
            inDouble = true;
            return new DoubleDouble(upper ? prefactor.High : 1.0 - prefactor.High, 0.0);
        }
        DoubleDouble tail = prefactor * ExactGammaFraction(a, x);
        return upper ? tail : one - tail;
    }

    /// <summary>Gamma's Temme form in double-double, its remainder <c>R = e^(−aη²/2)/√(2πa) Σ c_k(η) a^-k</c> with it: in
    /// double R's exponent, from a double η, kept 46 ulps of F's tail at dfs (2000, 1e6) (#1742's Review A). The series' √a
    /// terms near the mean, which took 77 us at a = 9.5e4, are not summed (#1745's Review A).</summary>
    private static DoubleDouble ExactTemme(double a, DoubleDouble x, bool upper)
    {
        DoubleDouble one = new(1.0, 0.0);
        // η²/2 = λ − 1 − log λ, λ = x/a, from the point to 106 bits.
        DoubleDouble ratio = x / new DoubleDouble(a, 0.0);
        // Not ExactLogRatioExcess's series: against its exact square the double η of the remainder read F's tail at (100, 1e20,
        // 1) 2 ulps off P(50, 50), and this form 0.16 (#1745's Review A).
        DoubleDouble halfSquare = (ratio - one) - DoubleDouble.Log(ratio);
        if (halfSquare.High < 0.0)
        {
            halfSquare = new DoubleDouble(0.0, 0.0);
        }
        DoubleDouble square = a * halfSquare;
        DoubleDouble half = 0.5 * DoubleDouble.ErfcOfSquareRoot(square);
        // The side from the point to 106 bits: x.High at a beside a negative low part lies below it.
        bool above = x.High > a || (x.High >= a && x.Low >= 0.0);
        DoubleDouble eta = DoubleDouble.Sqrt(2.0 * halfSquare);
        DoubleDouble scale = DoubleDouble.ExpExact(-square) / DoubleDouble.Sqrt(TwoPi * new DoubleDouble(a, 0.0));
        DoubleDouble remainder = scale * ExactTemmeSum(a, above ? eta : -eta);
        // Q = ½ erfc(η √(a/2)) + R, erfc of a negative argument two less that of its magnitude; P = 1 − Q, formed apart.
        DoubleDouble upperHalf = above ? half : one - half;
        DoubleDouble lowerHalf = above ? one - half : half;
        return upper ? upperHalf + remainder : lowerHalf - remainder;
    }

    /// <summary><c>Σ c_k(η) a^-k</c> to some 1e-26 from a shape of ExactTemmeShape, where TemmeSeries' twelve rows reach it:
    /// row k reads 27 − 2k terms, past which they fall under 1e-28 at the region's |η| of 0.34, and up to c_3 the first
    /// 10 − 2k in double-double, whose double rounding passes 1e-27 of the sum.</summary>
    private static DoubleDouble ExactTemmeSum(double a, DoubleDouble eta)
    {
        DoubleDouble sum = new(0.0, 0.0);
        DoubleDouble inversePower = new(1.0, 0.0);
        DoubleDouble shape = new(a, 0.0);
        double previous = double.PositiveInfinity;
        for (int k = 0; k < TemmeSeries.Rows; k++)
        {
            DoubleDouble term = inversePower * ExactTemmeRow(k, eta);
            if (Math.Abs(term.High) > previous)
            {
                break;
            }
            sum += term;
            if (Math.Abs(term.High) <= ExactEpsilon * Math.Abs(sum.High))
            {
                break;
            }
            previous = Math.Abs(term.High);
            inversePower /= shape;
        }
        return sum;
    }

    /// <summary>c_k(η): the terms past the head in double, then the head's in double-double, by Horner.</summary>
    private static DoubleDouble ExactTemmeRow(int k, DoubleDouble eta)
    {
        int terms = ExactTemmeTerms - (2 * k);
        int head = k < TemmeSeries.LowRows ? TemmeSeries.LowTerms - (2 * k) : 0;
        double tail = 0.0;
        for (int j = terms - 1; j >= head; j--)
        {
            tail = (tail * eta.High) + TemmeSeries.Coefficient(k, j);
        }
        DoubleDouble value = new(tail, 0.0);
        for (int j = head - 1; j >= 0; j--)
        {
            value = (value * eta) + new DoubleDouble(TemmeSeries.Coefficient(k, j), TemmeSeries.CoefficientLow(k, j));
        }
        return value;
    }

    private const int ExactTemmeTerms = 27;

    // From a shape of 100 the last of TemmeSeries' rows, c_11(η) a^-11, is under 2e-25 of the sum, and the next under 3e-27.
    private const double ExactTemmeShape = 100.0;


    /// <summary>Gamma.SmallShapeTail in double-double: with <c>u = a log x − log Γ(1 + a)</c> and <c>T = Σ (−x)^n / ((a + n) n!)</c>,
    /// <c>Q = −expm1(u) − a T e^u</c> and <c>P = e^u (1 + a T)</c>, so a Q of 1e-19 is not one less a P of 1 − 1e-19.</summary>
    private static DoubleDouble ExactSmallShapeGamma(double a, DoubleDouble x, bool upper, int lift)
    {
        DoubleDouble u = (a * DoubleDouble.Log(x)) - DoubleDouble.LogGammaOnePlus(a);
        var terms = new SmallShapeGammaTerms(a, x);
        DoubleDouble sum = SummedSeries(new DoubleDouble(0.0, 0.0), 1, ref terms);
        DoubleDouble lead = DoubleDouble.ExpExact(u, lift);
        return upper
            ? -DoubleDouble.Expm1Exact(u) - (a * (sum * lead))
            : lead * (new DoubleDouble(1.0, 0.0) + (a * sum));
    }

    /// <summary><c>Σ x^n / (a (a + 1) ⋯ (a + n))</c> in double-double.</summary>
    private static DoubleDouble ExactGammaSeries(double a, DoubleDouble x)
    {
        var terms = new GammaSeriesTerms(a, x);
        return SummedSeries(terms.Term, 1, ref terms);
    }

    /// <summary>A double-double series' terms, each from the last, the <c>k</c>-th by <see cref="Next"/>.</summary>
    private interface ISeriesTerms
    {
        DoubleDouble Next(int k);
    }

    /// <summary><paramref name="sum"/> plus the terms from the <paramref name="first"/>-th until one falls to ExactEpsilon
    /// of the sum, at most MaxIterations: one stopping rule for the four series that each carried their own.</summary>
    private static DoubleDouble SummedSeries<TTerms>(DoubleDouble sum, int first, ref TTerms terms)
        where TTerms : struct, ISeriesTerms
    {
        for (int k = first; k <= MaxIterations; k++)
        {
            DoubleDouble term = terms.Next(k);
            sum += term;
            if (Math.Abs(term.High) <= ExactEpsilon * Math.Abs(sum.High))
            {
                break;
            }
        }
        return sum;
    }

    /// <summary>(−1)^(k+1) d^k / k, the power from −d² at k = 2.</summary>
    private struct AlternatingLogTerms(DoubleDouble excess) : ISeriesTerms
    {
        private readonly DoubleDouble _excess = excess;

        internal DoubleDouble Power { get; private set; } = -(excess * excess);

        public DoubleDouble Next(int k)
        {
            Power = -(Power * _excess);
            return Power / new DoubleDouble(k, 0.0);
        }
    }

    /// <summary>y^k / k, the power from y at k = 1.</summary>
    private struct LogOneLessTerms(DoubleDouble complement) : ISeriesTerms
    {
        private readonly DoubleDouble _complement = complement;
        private DoubleDouble _power = complement;

        public DoubleDouble Next(int k)
        {
            _power *= _complement;
            return _power / new DoubleDouble(k, 0.0);
        }
    }

    /// <summary>(−x)^n / ((a + n) n!).</summary>
    private struct SmallShapeGammaTerms(double a, DoubleDouble x) : ISeriesTerms
    {
        private DoubleDouble _power = new(1.0, 0.0);

        public DoubleDouble Next(int k)
        {
            _power = -(_power * x) / new DoubleDouble(k, 0.0);
            return _power / DoubleDouble.Sum(a, k);
        }
    }

    /// <summary>x^n / (a (a + 1) ⋯ (a + n)), the term from 1/a.</summary>
    private struct GammaSeriesTerms(double a, DoubleDouble x) : ISeriesTerms
    {
        internal DoubleDouble Term { get; private set; } = new DoubleDouble(1.0, 0.0) / new DoubleDouble(a, 0.0);

        public DoubleDouble Next(int k)
        {
            Term = Term * x / DoubleDouble.Sum(a, k);
            return Term;
        }
    }

    /// <summary>Legendre's fraction for Q(a, x) over its prefactor in double-double, by modified Lentz.</summary>
    private static DoubleDouble ExactGammaFraction(double a, DoubleDouble x)
    {
        DoubleDouble one = new(1.0, 0.0);
        DoubleDouble b = x + DoubleDouble.Sum(1.0, -a);
        DoubleDouble c = new(1.0 / Tiny, 0.0);
        DoubleDouble d = one / b;
        DoubleDouble h = d;
        for (int i = 1; i <= MaxIterations; i++)
        {
            DoubleDouble an = -(i * DoubleDouble.Sum(i, -a));
            b += new DoubleDouble(2.0, 0.0);
            d = (an * d) + b;
            if (Math.Abs(d.High) < Tiny)
            {
                d = new DoubleDouble(Tiny, 0.0);
            }
            c = b + (an / c);
            if (Math.Abs(c.High) < Tiny)
            {
                c = new DoubleDouble(Tiny, 0.0);
            }
            d = one / d;
            DoubleDouble delta = d * c;
            h *= delta;
            if (Math.Abs((delta - one).High) < ExactEpsilon)
            {
                break;
            }
        }
        return h;
    }
}
