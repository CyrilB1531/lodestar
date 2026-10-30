namespace Lodestar.Stats.Internal;

/// <summary>The log-gamma function and the two regularized incomplete gammas.</summary>
/// <remarks>
/// Lanczos (1964) for the log-gamma; the series-below / continued-fraction-above split, by
/// Steed's forward sum and its tail read backward, for the incomplete pair -- except <c>Q</c> at an integer or half-integer
/// shape, a finite sum (<see cref="HalfIntegerQ"/>), and a large shape near its mean, Temme's
/// expansion (<see cref="Temme"/>). No reference implementation is transcribed (ADR 0002). The
/// upper tail <c>Q</c> is a chi-square p-value: with <c>a = dof/2</c>, <c>x = statistic/2</c>,
/// <c>Q(a, x)</c> is the probability of a statistic at least this large.
/// </remarks>
internal static class Gamma
{
    // Lanczos g = 7, nine coefficients; the pair is not free -- mixing this
    // table with coefficients tuned for a different g loses eight digits silently.
    private const double LanczosG = 7.0;

    private static readonly double[] LanczosCoefficients =
    [
        0.99999999999980993,
        676.5203681218851,
        -1259.1392167224028,
        771.32342877765313,
        -176.61502916214059,
        12.507343278686905,
        -0.13857109526572012,
        9.9843695780195716e-6,
        1.5056327351493116e-7,
    ];

    // A guard, not the stopping rule: with Temme holding every mean past a = 20 no branch nears it,
    // while the 300 it replaces cut the series at a = 1e8 half a unit short (#837).
    private const int MaxIterations = 100_000;
    private const double Epsilon = 3e-16;

    // 2^-56, an eighth of the ulp of doubles in [1/2, 1): a P term below it cannot move 1 − P where Q is near one.
    private const double OneMinusPFloor = 1.3877787807814457e-17;

    // At 2a = 100 the sum beat the iteration at x = a and x = 2a (default job: 44 vs 177 and 71 ns,
    // a short run had the fraction ahead at 160). Past x = 700 e^-x nears the subnormals, Q need not.
    private const double MaxClosedFormTwiceA = 100.0;
    private const double MaxClosedFormX = 700.0;

    // Past it e^(a log x) carries its exponent's rounding over four times: ChiSquaredCdf(1e-300, 2) read 3.5e-14 off. With
    // x^a by Math.Pow and Γ exact, 282 of 430 random moved values came nearer 1000-digit sums, 56 further (#1729).
    internal const double LongPower = 4.0;

    // 2^512, which lifts a subnormal x into the normal doubles for UnhalvedLowerLead, and the shape it serves up to.
    private static readonly double LeadLift = PowerOfTwo.Of(512);
    private const double MaxLiftedShape = 1.5;

    // cephes igam's region for 20 < a < 200, kept for every larger a: why is in Temme's remarks.
    private const double TemmeMinimumShape = 20.0;
    private const double TemmeMaximumDeviation = 0.3;

    // Below this shape log Gamma(1 + a) is LogGammaOnePlus's series, the prefactor keeps a outside its exponential,
    // and Q is P's series taken whole rather than 1 - P (#1690's review).
    internal const double SmallShape = 0.1;

    // (-1)^k zeta(k) / k from k = 1 (-Euler's gamma), the Taylor series of log Gamma(1 + s): to s^20, 0.1^21 / 21 short.
    private static readonly double[] LogGammaOnePlusCoefficients =
    [
        -0.5772156649015329, 0.8224670334241132, -0.40068563438653143, 0.27058080842778454,
        -0.20738555102867398, 0.1695571769974082, -0.1440498967688461, 0.12550966952474304,
        -0.11133426586956469, 0.1000994575127818, -0.09095401714582904, 0.083353840546109,
        -0.0769325164113522, 0.07143294629536133, -0.06666870588242046, 0.06250095514121304,
        -0.058823978658684585, 0.055555767627403614, -0.05263167937961666, 0.05000004769810169,
    ];

    // Stirling's correction is within 3e-17 of log Gamma*(a) from here, while the Lanczos
    // prefactor a log x - x - log Gamma(a) cancels a digit per decade of a log a.
    internal const double StirlingMinimumShape = 10.0;

    // Between these x PowerPrefactor reads below a shape of 10: x^a stays inside the normal doubles, and past e^750 the tail
    // has underflowed anyway.
    private const double PowerPrefactorLowest = 1e-20;
    private const double PowerPrefactorHighest = 1e20;

    // Past it e^-x nears the subnormals, and is taken in two halves about the rest so a subnormal product rounds once.
    private const double HalvedDecayFrom = 700.0;

    // Inside this reach the series' first dropped term is 3e-21 of the exponent.
    private const double ExcessSeriesReach = 1e-5;

    // From here Q past x = a reads the fraction, read back from its tail, and not 1 − P: Q fell to 0.02 near a + 1.
    private const double FractionFrom = 0.8;

    // Up to this many terms the forward sum holds 4 ulps; the backward pass takes a quarter as many again, and a margin.
    private const int ForwardFractionSteps = 24;
    private const int ForwardFractionMargin = 4;

    // Below this x, a at most x as every caller has it, 90/x + 6 terms read the fraction to 4.3 ulps, and its terms are known
    // without the forward pass, which cost twice its time.
    private const double TailFractionBelow = 4.0;
    private const double TailFractionTerms = 90.0;
    private const int TailFractionMargin = 6;

    // Below it the series for log(1 + t) - t takes at most 16 terms; above it the logarithm's
    // rounding costs under 2e-15 of t^2 / 2.
    private const double LogSeriesMaximum = 0.1;

    // The rows of TemmeSeries' c_k(η) the expansion reads, and the terms of each: twelve rows of sixteen matched
    // twenty-five of twenty-five to 2e-16 over a in (20, 1e8] and |x - a| / a <= 0.3.
    private const int TemmeTerms = TemmeSeries.Rows;
    private const int TemmeDegree = 16;

    internal static double LogGamma(double x)
    {
        if (double.IsNaN(x) || x <= 0.0)
        {
            throw new ArgumentOutOfRangeException(
                nameof(x), x, "The log-gamma function is defined here for x > 0 only.");
        }

        // Gamma(x) = Gamma(1 + x) / x below SmallShape, by LogGammaOnePlus's series: the reflection's pi / sin(pi x)
        // overflowed below 1.75e-308, and a subnormal shape read log B infinite or NaN (#1690's review).
        if (x < SmallShape)
        {
            return LogGammaOnePlus(x) - Math.Log(x);
        }

        // Reflection: Gamma(x)Gamma(1-x) = pi / sin(pi x). Below 0.5 the Lanczos
        // sum loses precision, and above it the reflection would.
        if (x < 0.5)
        {
            return Math.Log(Math.PI / Math.Abs(Math.Sin(Math.PI * x))) - LogGamma(1.0 - x);
        }

        double z = x - 1.0;
        double series = LanczosCoefficients[0];
        for (int i = 1; i < LanczosCoefficients.Length; i++)
        {
            series += LanczosCoefficients[i] / (z + i);
        }

        double t = z + LanczosG + 0.5;
        return (0.5 * Math.Log(2.0 * Math.PI)) + ((z + 0.5) * Math.Log(t)) - t + Math.Log(series);
    }

    /// <summary><c>log Γ(1 + s)</c> for <c>s</c> below SmallShape, by its Taylor series.</summary>
    internal static double LogGammaOnePlus(double s)
    {
        double sum = 0.0;
        for (int k = LogGammaOnePlusCoefficients.Length - 1; k >= 0; k--)
        {
            sum = (sum + LogGammaOnePlusCoefficients[k]) * s;
        }
        return sum;
    }

    internal static double RegularizedP(double a, double x)
    {
        Validate(a, x);

        // a * log(x) - x is inf - inf = NaN at x = +inf, so the limit is taken
        // here rather than through SeriesP/ContinuedFractionQ.
        if (double.IsPositiveInfinity(x))
        {
            return 1.0;
        }

        // Exact sentinel: x is validated to be non-negative above, and P(a, 0)
        // is 0 by definition, not a limit that rounding could have missed.
#pragma warning disable S1244
        if (x == 0.0)
#pragma warning restore S1244
        {
            return 0.0;
        }

        if (IsTemmeRegion(a, x))
        {
            return Temme(a, x, upper: false);
        }

        if (x >= a + 1.0)
        {
            return OneLess(ContinuedFraction(a, x), a, x);
        }
        return a < SmallShape ? SmallShapeTail(a, x, upper: false) : SeriesP(a, x);
    }

    /// <summary>The chi-squared tail Q(df/2, x/2), or P when not <paramref name="upper"/>, at the exact halves of x and df.</summary>
    /// <remarks>
    /// Below ExactHalf.Threshold halving rounds: an x there takes <see cref="UnhalvedTail"/>, and a df there is lifted,
    /// Q being of degree one in so small a shape. The common path stays inline: through a call ChiSquaredCdf(3.84, 1)
    /// took 4% longer, 220 against 211 ns.
    /// </remarks>
    [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
    internal static double ChiSquaredTail(double df, double x, bool upper)
    {
        if (ExactHalf.Lifts(df))
        {
            return LiftedChiSquaredTail(df, x, upper);
        }
        if (x < ExactHalf.Threshold && x > 0.0)
        {
            return UnhalvedTail(df / 2.0, x, upper);
        }
        return upper ? RegularizedQ(df / 2.0, x / 2.0) : RegularizedP(df / 2.0, x / 2.0);
    }

    /// <summary>The chi-squared tail at a df below ExactHalf.Threshold: Q at the lifted df's exact half, dropped once,
    /// and P one less it.</summary>
    [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.NoInlining)]
    private static double LiftedChiSquaredTail(double df, double x, bool upper)
    {
        // Q(a, 0) is one at any shape, not of degree one: at x = 0 the lifted shape answers it, where df/2 could be zero.
        if (!(x > 0.0))
        {
            double lifted = ExactHalf.Lifted(df) / 2.0;
            return upper ? RegularizedQ(lifted, x / 2.0) : RegularizedP(lifted, x / 2.0);
        }
        return ExactHalf.FromLiftedTail(ChiSquaredTail(ExactHalf.Lifted(df), x, upper: true), upper);
    }

    /// <summary>Q(a, x/2), or P, for an x below ExactHalf.Threshold, from P = (x/2)^a / Γ(1 + a), without halving x.</summary>
    /// <remarks>
    /// There P holds that form to within x/2 of itself, and Q is −expm1 of its log. ChiSquaredCdf(1.5e-323, 1) read 15%
    /// high, and ChiSquaredSf(5e-324, 2e-300) one (#1729's Review A).
    /// </remarks>
    [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.NoInlining)]
    private static double UnhalvedTail(double a, double x, bool upper)
    {
        Validate(a, x);
        if (upper)
        {
            return -NearZero.Expm1(LogUnhalvedLowerLead(a, x));
        }
        // Past one half P is one less Q, rounded once: as the lead's power times its scale it read 1.3 to 3.5 ulps off.
        double logGamma = LogGammaOnePlusAnyShape(a);
        double lead = LiftedLead(a, x, LeadScale(a, logGamma));
        return lead > 0.5 ? 1.0 + NearZero.Expm1(LogUnhalvedLowerLead(a, x, logGamma)) : lead;
    }

    /// <summary>The chi-squared tail midway between <paramref name="low"/> and the next double up, <paramref name="high"/>,
    /// below ExactHalf.Threshold and with <paramref name="low"/> possibly zero: a point no double holds.</summary>
    /// <remarks>UnhalvedTail's form shifted in logs from the nearer end, so the small roots round to the double nearest
    /// the root by the midpoint, as rounding does (#1729's Review A).</remarks>
    internal static double ChiSquaredTailBetween(in UnhalvedLead lead, double low, double high, bool upper)
    {
        if (lead.Lifted)
        {
            return ExactHalf.FromLiftedTail(LeadBetween(lead, low, high, upper: true), upper);
        }
        return LeadBetween(lead, low, high, upper);
    }

    private static double LeadBetween(in UnhalvedLead lead, double low, double high, bool upper)
    {
        double a = lead.Shape;
        double end = low > 0.0 ? low : high;
        double shift = MidpointShift(a, low, high);
        return upper
            ? -NearZero.Expm1(LogUnhalvedLowerLead(a, end, lead.LogGammaOfOnePlusShape) + shift)
            : LiftedLead(a, end, lead.Scale * Math.Exp(shift));
    }

    /// <summary>The lower tail midway between <paramref name="low"/> and <paramref name="high"/> at an unlifted df, times
    /// 2^<see cref="MidpointLift"/>, as the tail at the nearer end and the rise to the midpoint apart: the tail rounded
    /// onto its subnormal grid, or e^shift onto one, a quarter-granule from the target at df 2 read as the target itself,
    /// and ChiSquaredQuantile(1e-308, 2) stopped a granule short of 2e-308 (#1729's Review A).</summary>
    internal static (double Lead, double Rise) ScaledLowerTailBetween(in UnhalvedLead lead, double low, double high)
    {
        double a = lead.Shape;
        double end = low > 0.0 ? low : high;
        // x^a lifted by its own test, before the scale: with 2^600 in the scale a subnormal x^a, some 12 bits, passed.
        double at = LiftedLead(a, end, lead.Scale * MidpointLiftFactor, powerAlone: true);
        return (at, at * NearZero.Expm1(MidpointShift(a, low, high)));
    }

    /// <summary>a log of the midpoint over the nearer end, or over x = 0's next double, its half, where low is zero.</summary>
    private static double MidpointShift(double a, double low, double high) =>
        a * (low > 0.0 ? NearZero.Log1P((high - low) / (2.0 * low)) : -Beta.Ln2);

    /// <summary>The exponent <see cref="ScaledLowerTailBetween"/> lifts by, which takes any subnormal tail to a normal one.</summary>
    internal const int MidpointLift = 600;

    /// <summary>2^<see cref="MidpointLift"/>, the factor the small root's search scales a subnormal target by.</summary>
    internal static readonly double MidpointLiftFactor = PowerOfTwo.Of(MidpointLift);

    /// <summary>A chi-squared df's shape at its exact half, lifted where ExactHalf lifts it, with the lead's two constants
    /// in it: a root's search reads the tail between dozens of pairs of doubles at one df (#1729's Review A).</summary>
    internal readonly struct UnhalvedLead
    {
        internal UnhalvedLead(double df)
        {
            Lifted = ExactHalf.Lifts(df);
            Shape = (Lifted ? ExactHalf.Lifted(df) : df) / 2.0;
            LogGammaOfOnePlusShape = LogGammaOnePlusAnyShape(Shape);
            Scale = LeadScale(Shape, LogGammaOfOnePlusShape);
        }

        internal bool Lifted { get; }

        internal double Shape { get; }

        internal double LogGammaOfOnePlusShape { get; }

        internal double Scale { get; }
    }

    /// <summary><c>(x/2)^a / Γ(1 + a)</c> from x unhalved: the power by Math.Pow.</summary>
    /// <remarks>
    /// e^(a log(x/2)) took its exponent's rounding |a log(x/2)| times, 368 at a = 1/2, 8e-15 off (#1729's Review A). The
    /// chi-squared lower tail's leading term, and with e^(-x/2) its density's at a small shape. A subnormal lead rounds on
    /// the grid once, where x^a and the product each did: x is lifted by 2^512 into the normal doubles and the power
    /// taken there, the lift's 2^(−512a) going into the scale, exact in its exponent.
    /// </remarks>
    internal static double UnhalvedLowerLead(double a, double x)
    {
        return LiftedLead(a, x, LeadScale(a, LogGammaOnePlusAnyShape(a)));
    }

    /// <summary><c>2^-a / Γ(1 + a)</c>: at a chi-squared shape 2^-a and the table, exact at an integer; elsewhere one
    /// exponential of a small exponent.</summary>
    /// <remarks>As 2^-a times 1/Γ at a shape of 5e-6 it read ChiSquaredCdf(1e-310, 1e-5) 2 ulps low, and as one exponential
    /// at a = 1, e^-ln 2 an ulp off one half, ChiSquaredQuantile(1e-308, 2) a granule short of 2e-308 (#1729's Review A).</remarks>
    private static double LeadScale(double a, double logGammaOfOnePlus) =>
        IsHalfIntegerShape(a) ? Math.Pow(0.5, a) * ReciprocalGammaOnePlus(a) : Math.Exp(-(a * Beta.Ln2) - logGammaOfOnePlus);

    /// <summary><c>x^a</c> times <paramref name="scale"/>, x lifted by 2^512 where the product would be subnormal, or where
    /// <paramref name="powerAlone"/>, x^a itself, whose scale may lift it past the subnormals on its own.</summary>
    private static double LiftedLead(double a, double x, double scale, bool powerAlone = false)
    {
        double power = Math.Pow(x, a);
        double lead = power * scale;
        // Past a = 1.5 no x below ExactHalf.Threshold has a lead above zero but subnormal, and 2^(-512a) nears them.
        double tested = powerAlone ? power : lead;
        return tested >= NearZero.SmallestNormal || a > MaxLiftedShape
            ? lead
            : Math.Pow(x * LeadLift, a) * (scale * Math.Pow(2.0, -512.0 * a));
    }

    // S1244: 2a landing on an integer exactly is what a chi-squared shape a = df/2 always does; a shape near one
    // without being one takes the iterative paths and Lanczos.
#pragma warning disable S1244
    internal static bool IsHalfIntegerShape(double a) => a > 0.0 && 2.0 * a <= MaxClosedFormTwiceA && 2.0 * a == Math.Floor(2.0 * a);
#pragma warning restore S1244

    /// <summary>1/Γ(1 + a) at a half-integer shape, from <see cref="ReciprocalGammaOnePlusHalves"/>.</summary>
    /// <remarks>Lanczos' log Γ holds about 1e-15 only, and moved ChiSquaredQuantile(1e-308, 2) four ulps off the 2e-308
    /// HalfIntegerQ's inversion answers (#1729's Review A).</remarks>
    private static double ReciprocalGammaOnePlusHalfInteger(double a) => ReciprocalGammaOnePlusHalves[(int)(2.0 * a)];

    // 1/Γ(1 + k/2) for k = 0 to MaxClosedFormTwiceA, each correctly rounded from an 80-digit product of the halves and √π:
    // a product in doubles rounded at each step past 2^53, a few ulps by k = 100 (#1729's Review A).
    private static readonly double[] ReciprocalGammaOnePlusHalves =
    [
        1.0, 1.1283791670955126, 1.0, 0.7522527780636751,
        0.5, 0.30090111122547003, 0.16666666666666666, 0.08597174606442,
        0.041666666666666664, 0.01910483245876, 0.008333333333333333, 0.0034736059015927274,
        0.001388888888888889, 0.0005344009079373427, 0.0001984126984126984, 7.125345439164569e-05,
        2.48015873015873e-05, 8.38275934019361e-06, 2.7557319223985893e-06, 8.823957200203801e-07,
        2.755731922398589e-07, 8.403768762098858e-08, 2.505210838544172e-08, 7.307625010520746e-09,
        2.08767569878681e-09, 5.846100008416597e-10, 1.6059043836821613e-10, 4.330444450678961e-11,
        1.1470745597729725e-11, 2.986513414261352e-12, 7.647163731819816e-13, 1.9267828479105497e-13,
        4.779477332387385e-14, 1.1677471805518484e-14, 2.8114572543455206e-15, 6.672841031724848e-16,
        1.5619206968586225e-16, 3.606941098229647e-17, 8.22063524662433e-18, 1.8497133837075115e-18,
        4.110317623312165e-19, 9.022992115646397e-20, 1.9572941063391263e-20, 4.196740518905301e-21,
        8.896791392450574e-22, 1.8652180084023562e-22, 3.868170170630684e-23, 7.937097908095133e-24,
        1.6117375710961184e-24, 3.239631799222503e-25, 6.446950284384474e-26, 1.2704438428323542e-26,
        2.4795962632247976e-27, 4.794127708801336e-28, 9.183689863795546e-29, 1.7433191668368497e-29,
        3.279889237069838e-30, 6.116909357322279e-31, 1.1309962886447716e-31, 2.0735285957024674e-32,
        3.7699876288159054e-33, 6.798454412139238e-34, 1.216125041553518e-34, 2.1582394959172183e-35,
        3.8003907548547434e-36, 6.640736910514518e-37, 1.151633562077195e-37, 1.9823095255267216e-38,
        3.387157535521162e-39, 5.745824711671657e-40, 9.67759295863189e-41, 1.6185421723018752e-41,
        2.6882202662866363e-42, 4.434362115895549e-43, 7.265460179153071e-44, 1.182496564238813e-44,
        1.911963205040282e-45, 3.0714196473735403e-46, 4.902469756513544e-47, 7.775745942717823e-48,
        1.2256174391283858e-48, 1.91993726980687e-49, 2.9893108271424046e-50, 4.626354867004506e-51,
        7.117406731291439e-52, 1.0885540863540014e-52, 1.6552108677421951e-53, 2.5024231870206928e-54,
        3.7618428812322616e-55, 5.623422892181332e-56, 8.359650847182804e-57, 1.2359171191607324e-57,
        1.817315401561479e-58, 2.657886277765016e-59, 3.866628513960594e-60, 5.595550058452665e-61,
        8.055476070751236e-62, 1.153721661536632e-62, 1.643974708316579e-63, 2.3307508313871353e-64,
        3.287949416633158e-65,
    ];

    /// <summary>1/Γ(1 + a): the table at a half-integer shape, else e^(−log Γ(1 + a)).</summary>
    internal static double ReciprocalGammaOnePlus(double a) =>
        IsHalfIntegerShape(a) ? ReciprocalGammaOnePlusHalfInteger(a) : Math.Exp(-LogGammaOnePlusAnyShape(a));

    /// <summary><c>e^rest / Γ(a)</c> from a shape of SmallShape: the table at a half-integer shape, else in one exponent,
    /// as the prefactor and the density at a small statistic take it.</summary>
    // Inlined: as a call it cost ChiSquaredCdf(1e-6, 3) 40%, 9.3 to 10.6 against 6.7 ns (Stopwatch A/B/A/B).
    [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
    internal static double ExpOverGamma(double a, double rest) =>
        IsHalfIntegerShape(a) ? a * ReciprocalGammaOnePlusHalfInteger(a) * Math.Exp(rest) : Math.Exp(rest - LogGamma(a));

    /// <summary>The log of <see cref="UnhalvedLowerLead"/>, for the upper tail's expm1 and the root's closed form.</summary>
    private static double LogUnhalvedLowerLead(double a, double x) => LogUnhalvedLowerLead(a, x, LogGammaOnePlusAnyShape(a));

    /// <summary><c>a log(x/2) − log Γ(1 + a)</c>, the last given: the tail, the root's midpoint rule and its closed form read
    /// one lead.</summary>
    private static double LogUnhalvedLowerLead(double a, double x, double logGammaOfOnePlus) =>
        (a * (Math.Log(x) - Beta.Ln2)) - logGammaOfOnePlus;

    /// <summary>A first guess at the x below ExactHalf.Threshold whose <see cref="UnhalvedTail"/> is <paramref name="target"/>,
    /// or NaN where that x is not below it.</summary>
    /// <remarks>
    /// The lead's inverse in closed form, <c>x^a = 2^a P Γ(1 + a)</c>: by Math.Pow on x^a for the lower tail, in logs
    /// where x^a is subnormal and for the upper, whose P is one less a small target. Never through x/2, whose rounding
    /// answered only even multiples of the smallest double, ChiSquaredQuantile(3.07e-162, 1) 1e-323 for 1.5e-323, and 0
    /// at 1.6e-162 (#1729's Review A). The caller refines it.
    /// </remarks>
    internal static double UnhalvedRoot(in UnhalvedLead lead, double target, bool upper)
    {
        double a = lead.Shape;
        double x;
        if (upper)
        {
            x = Math.Exp(((NearZero.Log1P(-target) + lead.LogGammaOfOnePlusShape) / a) + Beta.Ln2);
        }
        else
        {
            // x^a itself, never x/2: a halved subnormal root rounds, to zero below the smallest double.
            // The lead's own scale, 2^-a / Γ(1 + a), which the search judges the root by.
            double power = target / lead.Scale;
            x = power >= NearZero.SmallestNormal
                ? Math.Pow(power, 1.0 / a)
                : Math.Exp(((Math.Log(target) + lead.LogGammaOfOnePlusShape) / a) + Beta.Ln2);
        }
        return x < ExactHalf.Threshold ? x : double.NaN;
    }

    /// <summary><c>log Γ(1 + a)</c> at any positive shape: <see cref="LogGammaOnePlus"/>'s series below SmallShape, where
    /// alone it holds, the table at a half-integer shape, and log Γ(a) + log a elsewhere.</summary>
    internal static double LogGammaOnePlusAnyShape(double a)
    {
        if (a < SmallShape)
        {
            return LogGammaOnePlus(a);
        }
        return IsHalfIntegerShape(a) ? -Math.Log(ReciprocalGammaOnePlusHalfInteger(a)) : LogGamma(a) + Math.Log(a);
    }

    internal static double RegularizedQ(double a, double x)
    {
        Validate(a, x);

        // Same reasoning as RegularizedP: the limit is taken directly rather
        // than through a computation that hits inf - inf at x = +inf.
        if (double.IsPositiveInfinity(x))
        {
            return 0.0;
        }

        // Exact sentinel: same reasoning as RegularizedP, Q(a, 0) = 1 by definition.
#pragma warning disable S1244
        if (x == 0.0)
#pragma warning restore S1244
        {
            return 1.0;
        }

        if (IsClosedFormQ(a, x))
        {
            return HalfIntegerQ((int)(2.0 * a), x);
        }

        if (IsTemmeRegion(a, x))
        {
            return Temme(a, x, upper: true);
        }

        if (x >= a + 1.0 || FractionReads(a, x))
        {
            return ContinuedFractionQ(a, x);
        }
        return a < SmallShape || TailSeriesHolds(a, x) ? SmallShapeTail(a, x, upper: true) : OneLess(SeriesSum(a, x), a, x);
    }

    private static bool FractionReads(double a, double x) => x >= FractionFrom && x >= a;

    // Below FractionFrom, where the fraction needs too many terms: a P within 0.67 of its x^a / Γ(1 + a) leaves little of 1 − P.
    private static bool TailSeriesHolds(double a, double x) =>
        x < FractionFrom && (x > 0.5 ? a <= 1.1 * x : a * Math.Log(x) >= -0.4);

    /// <summary>Q(a, x), or P when not <paramref name="upper"/>, below <c>x = a + 1</c> where <c>1 − P</c> cancels.</summary>
    /// <remarks>
    /// <c>P = x^a / Γ(1 + a) · (1 + a T)</c>, <c>T = Σ (−x)^n / ((a + n) n!)</c> from <c>n = 1</c>, so with
    /// <c>u = a log x − log Γ(1 + a)</c>, <c>Q = −expm1(u) − a T e^u</c>. Below 1.1 <c>T</c>'s alternating terms cancel
    /// by at most a factor of four. <c>ChiSquaredSf(1, 2e-300)</c> read <c>−2e-14</c> for <c>6e-301</c> (#1690's review).
    /// </remarks>
    private static double SmallShapeTail(double a, double x, bool upper)
    {
        double logPower = a * Math.Log(x);
        // Past SmallShape the Taylor series no longer holds, and log Γ(a) + log a cancelled to 40 ulps of a Q of 0.03.
        double logGamma = a < SmallShape ? LogGammaOnePlus(a) : DoubleDouble.LogGammaOnePlus(a).High;
        double u = logPower - logGamma;
        // The first two terms in double-double: rounded, their few ulps each took 13 ulps of a Q of 0.004 at x = 0.75,
        // where T is half its first term (Review A, pass 40).
        DoubleDouble square = DoubleDouble.Product(x, x);
        DoubleDouble half = new(0.5 * square.High, 0.5 * square.Low);
        DoubleDouble head = (new DoubleDouble(-x, 0.0) / DoubleDouble.Sum(a, 1.0)) + (half / DoubleDouble.Sum(a, 2.0));
        double power = half.High;
        double rest = 0.0;
        for (int n = 3; n <= MaxIterations; n++)
        {
            power *= -x / n;
            double term = power / (a + n);
            rest += term;
            if (Math.Abs(term) <= Epsilon * Math.Abs(head.High))
            {
                break;
            }
        }
        double sum = head.High + (head.Low + rest);
        if (upper)
        {
            return -NearZero.Expm1(u) - (a * sum * Math.Exp(u));
        }
        // P = x^a / Γ(1 + a) (1 + a T), where SeriesSum began at 1/a, which overflowed below a = 5.6e-309 and read
        // ChiSquaredCdf(1, 1e-310) +inf (#1690's review); past LongPower x^a by Math.Pow, as Prefactor takes it.
        double lead = PowerTimesExp(x, a, logPower, -logGamma);
        return lead * (1.0 + (a * sum));
    }

    /// <summary><c>x^a e^rest</c>, <paramref name="logPower"/> being <c>a log x</c>: past LongPower in it x^a by Math.Pow,
    /// within it one exponential, which rounds less there.</summary>
    internal static double PowerTimesExp(double x, double a, double logPower, double rest) =>
        logPower < -LongPower ? Math.Pow(x, a) * Math.Exp(rest) : Math.Exp(logPower + rest);

    private static bool IsClosedFormQ(double a, double x) => x <= MaxClosedFormX && IsHalfIntegerShape(a);

    // Q of a shape plus one is Q plus x^b e^-x / Gamma(b+1), by parts: positive terms, whose error grows with their count
    // and not x. Below x = a/2 past a shape of one, one less HalfIntegerP, whose series stops at 2^-56 of one.
    private static double HalfIntegerQ(int twiceA, double x)
    {
        if (twiceA > 2 && x < twiceA / 4.0)
        {
            return 1.0 - HalfIntegerP(twiceA, x);
        }
        double sum;
        double term;
        double b;
        int increments;
        if ((twiceA & 1) == 1)
        {
            sum = Normal.ErfcOfSquareRoot(x);
            if (twiceA == 1)
            {
                return sum;
            }

            // x^(1/2) e^-x / Gamma(3/2), taking Q(1/2, x) to Q(3/2, x).
            term = Normal.TwoOverSqrtPi * Math.Sqrt(x) * Math.Exp(-x);
            sum += term;
            b = 1.5;
            increments = ((twiceA - 1) / 2) - 1;
        }
        else
        {
            term = Math.Exp(-x);
            sum = term;
            b = 1.0;
            increments = (twiceA / 2) - 1;
        }

        for (; increments > 0; increments--)
        {
            term *= x / b;
            sum += term;
            b += 1.0;
        }

        return sum;
    }

    /// <summary>P(a, x) below x = a/2 past a shape of one, below the median, so Q is past one half and the sum's roundings were its own:
    /// ChiSquaredSf(0.1106, 19) read 6.5 ulps below one, and (0.0341, 19) above it (#1729's Review A).</summary>
    /// <remarks>
    /// P's lead <c>x^a e^−x / Γ(1 + a)</c> by Math.Pow and the table, times <c>Σ x^n / ((a + 1)…(a + n))</c>: as fast as
    /// the sum there, where 1 − SeriesP, through Stirling, took four times as long, and the sum's recurrence took a ε into
    /// the lead. Up to x = 0.9a it held 6-ulp tails at df 99 to one, at four times the sum's time near that cut.
    /// </remarks>
    private static double HalfIntegerP(int twiceA, double x)
    {
        double a = twiceA / 2.0;
        // Only 1 − P is read, so P's terms stop below OneMinusPFloor, absolutely.
        double lead = Math.Pow(x, a) * Math.Exp(-x) * ReciprocalGammaOnePlusHalfInteger(a);
        double sum = lead;
        double n = a;
        while (lead > OneMinusPFloor)
        {
            n += 1.0;
            lead *= x / n;
            sum += lead;
        }
        return sum;
    }

    private static void Validate(double a, double x)
    {
        if (double.IsNaN(a) || a <= 0.0)
        {
            throw new ArgumentOutOfRangeException(nameof(a), a, "The shape must be positive.");
        }
        if (double.IsNaN(x) || x < 0.0)
        {
            throw new ArgumentOutOfRangeException(nameof(x), x, "The argument must not be negative.");
        }
    }

    /// <summary>log(1 + t) - t, given 1 + t as the caller formed it more exactly than 1 + t would.</summary>
    internal static double LogOnePlusMinus(double t, double onePlusT)
    {
        if (Math.Abs(t) >= LogSeriesMaximum)
        {
            return Math.Log(onePlusT) - t;
        }

        // log(1 + t) - t = sum over j >= 2 of (-1)^(j+1) t^j / j, without the cancellation.
        double power = -t * t;
        double sum = 0.0;
        for (int j = 2; j < 64; j++)
        {
            double term = power / j;
            sum += term;
            if (Math.Abs(term) <= Epsilon * Math.Abs(sum))
            {
                break;
            }

            power *= -t;
        }

        return sum;
    }

    /// <summary>log Gamma*(z), where Gamma(z) = sqrt(2 pi) z^(z - 1/2) e^-z Gamma*(z).</summary>
    /// <remarks>
    /// Stirling's series, B_2n / (2n (2n - 1) z^(2n - 1)), through 1/(156 z^13): at z = 10 the
    /// first term left out is 3e-17. Only for z at or above <see cref="StirlingMinimumShape"/>.
    /// </remarks>
    internal static double LogStirlingCorrection(double z)
    {
        double w = 1.0 / (z * z);
        double series = Stirling1 + (w * (Stirling2 + (w * (Stirling3 + (w * (Stirling4 + (w * (Stirling5
            + (w * (Stirling6 + (w * Stirling7)))))))))));
        return series / z;
    }

    // B_2n / (2n (2n - 1)) for n = 1 to 7, the series above; Beta.LogGammaIncrement takes it term by term.
    internal const double Stirling1 = 1.0 / 12.0;
    internal const double Stirling2 = -1.0 / 360.0;
    internal const double Stirling3 = 1.0 / 1260.0;
    internal const double Stirling4 = -1.0 / 1680.0;
    internal const double Stirling5 = 1.0 / 1188.0;
    internal const double Stirling6 = -691.0 / 360360.0;
    internal const double Stirling7 = 1.0 / 156.0;

    internal static bool IsTemmeRegion(double a, double x) =>
        a > TemmeMinimumShape && Math.Abs(x - a) < TemmeMaximumDeviation * a;

    /// <summary>P(a, x) or Q(a, x) by Temme's uniform asymptotic expansion.</summary>
    /// <remarks>
    /// Q = erfc(eta sqrt(a/2)) / 2 + R and P = erfc(-eta sqrt(a/2)) / 2 - R, with R = e^(-a eta^2/2)
    /// / sqrt(2 pi a) times the sum of c_k(eta) a^-k, eta^2 / 2 = lambda - 1 - log lambda, lambda =
    /// x / a, eta signed as x - a (DLMF 8.12); asymptotic, so the sum also stops at a growing term.
    /// The series needs about 7 sqrt(a) terms at x = a, and with the exponent taken whole the tail held 2 ulps of a
    /// 90-digit one over 104 points to a = 6,000 (#1756); scipy's region past a = 200 is 4.5 standard deviations wide,
    /// and its capped series is 60% wrong just past that at a = 1e8 (docs/equivalence.md).
    /// </remarks>
    private static double Temme(double a, double x, bool upper)
    {
        // The exponent a η²/2 = −a (log(1 + σ) − σ) to some 106 bits, its erfc from it, its low part a first-order
        // factor: through η √(a/2) erfc took 2y² times the argument's roundings, 862 ulps of Q(1410, 1561) (#1756).
        DoubleDouble whole = TemmeExponent(a, x);
        double exponent = whole.High;
        double root = Math.Sqrt(exponent);
        double eta = Math.Sqrt(2.0 / a) * (x < a ? -root : root);
        double sum = TemmeSeriesSum(a, eta);
        double decay = Math.Exp(-exponent);
        double remainder = decay * Math.Max(0.0, 1.0 - whole.Low) / Math.Sqrt(2.0 * Math.PI * a) * sum;
        double tail = Normal.ErfcOfSquareRoot(exponent);
        // d erfc(√s)/ds = −e^−s / √(π s): the low part moves the tail by that much, nothing at x = a, where it is zero.
        if (exponent > 0.0)
        {
            tail -= whole.Low * decay / (Math.Sqrt(Math.PI) * root);
        }
        if (upper != (x > a))
        {
            tail = 2.0 - tail;
        }

        return upper ? (0.5 * tail) + remainder : (0.5 * tail) - remainder;
    }

    /// <summary>The exponent <c>−a (log(1 + σ) − σ)</c>, σ = (x − a)/a, which is a η²/2, to some 106 bits.</summary>
    /// <remarks>Never below zero, as a η²/2 is not: within ulps of a its last bits read −4e-28, and the root NaN.</remarks>
    private static DoubleDouble TemmeExponent(double a, double x)
    {
        DoubleDouble exponent = -LogExcess(a, x);
        return exponent.High > 0.0 ? exponent : new DoubleDouble(0.0, 0.0);
    }

    /// <summary>The sum of <c>c_k(η) a^-k</c>, stopped at a growing term.</summary>
    private static double TemmeSeriesSum(double a, double eta)
    {
        double sum = 0.0;
        double inversePower = 1.0;
        double previous = double.PositiveInfinity;
        for (int k = 0; k < TemmeTerms; k++)
        {
            double coefficient = TemmeSeries.Value(k, TemmeDegree, eta);

            double term = inversePower * coefficient;
            if (Math.Abs(term) > previous)
            {
                break;
            }

            sum += term;
            if (Math.Abs(term) <= Epsilon * Math.Abs(sum))
            {
                break;
            }

            previous = Math.Abs(term);
            inversePower /= a;
        }
        return sum;
    }

    /// <summary>log Q(a, x), or log P when not <paramref name="upper"/>, over a below SmallShape, where the tail lies below
    /// the normal doubles and kept only their few bits, and <c>x f(x) / T</c>, the slope of its log in log x: the prefactor
    /// in logs beside the fraction, the series or Temme's sum, whose own quotient the slope reads. Validated arguments.</summary>
    /// <remarks>The slope as two logs apart, each about −x, kept only their rounding past a Newton overshoot, and
    /// ChiSquaredIsf(1e-310, 2e-5) read 6.9e17 for 1391.48; log a, some 570 nats at 6.5e-249 against an x of 139, took
    /// 4 ulps of x where a times the rest is exact (#1729's Review A).</remarks>
    internal static (double Log, double Ratio) LogTail(double a, double x, bool upper)
    {
        if (IsTemmeRegion(a, x))
        {
            // Only the small side: the other is one less it, which Temme's direct sum reads whole.
            if (upper ? x > a : x < a)
            {
                return TemmeLogTail(a, x, upper);
            }
        }
        else if (upper ? x >= a + 1.0 : (x < a + 1.0 && a >= SmallShape))
        {
            double rest = upper ? ContinuedFraction(a, x) : SeriesSum(a, x);
            return (LogPrefactor(a, x) + Math.Log(rest), 1.0 / rest);
        }
        double log = Math.Log(upper ? RegularizedQ(a, x) : RegularizedP(a, x));
        double scaledLog = log - (a < SmallShape ? Math.Log(a) : 0.0);
        return (scaledLog, Math.Exp(LogPrefactor(a, x) - scaledLog));
    }

    /// <summary><see cref="LogTail"/> in Temme's region on the small side, where x lies past a for the upper tail.</summary>
    /// <remarks>e^(−a η²/2) taken out of both terms, erfc(y) = erfcx(y) e^(−y²): the two logs apart, some 5.5e267 each at
    /// a = 5e299, differed by their rounding, and ChiSquaredQuantile(5e-324, 1e300) read 7e299. The exponent is
    /// <see cref="Temme"/>'s, whole: from a double η it took 862 ulps of Q(1410, 1561) (#1756).</remarks>
    private static (double Log, double Ratio) TemmeLogTail(double a, double x, bool upper)
    {
        DoubleDouble whole = TemmeExponent(a, x);
        double y = Math.Sqrt(whole.High);
        double eta = Math.Sqrt(2.0 / a) * (x < a ? -y : y);
        double sum = TemmeSeriesSum(a, eta);
        double scaled = (0.5 * Normal.ScaledComplement(y)) + ((upper ? sum : -sum) / Math.Sqrt(2.0 * Math.PI * a));
        // x f(x) is e^(-a η²/2 - Stirling's correction) √(a/2π), so the exponential cancels in the slope.
        double ratio = Math.Exp(-LogStirlingCorrection(a)) * Math.Sqrt(a / (2.0 * Math.PI)) / scaled;
        return (-whole.High + (Math.Log(scaled) - whole.Low), ratio);
    }

    /// <summary>The log of <see cref="Prefactor(double, double)"/>, which underflows where the tail it scales does, over a below SmallShape.</summary>
    private static double LogPrefactor(double a, double x)
    {
        if (a < SmallShape)
        {
            return (a * Math.Log(x)) - x - LogGammaOnePlus(a);
        }
        return a < StirlingMinimumShape ? (a * Math.Log(x)) - x - LogGamma(a) : LogStirlingPrefactor(a, x);
    }

    // x^a e^-x / Gamma(a), below a shape of 10 as PowerPrefactor's three factors, from it through Stirling so no term of
    // order a log a cancels; the chi-squared density is this over x (#1158).
    internal static double Prefactor(double a, double x) => Prefactor(a, x, whole: true);

    /// <summary><see cref="Prefactor(double, double)"/>, its exponent from a shape of 10 in doubles unless
    /// <paramref name="whole"/>: one less a tail needs it only to an ulp of one.</summary>
    private static double Prefactor(double a, double x, bool whole)
    {
        // a x^a e^-x / Gamma(1 + a) below SmallShape: log Gamma(a), about -log a, rounded by |log a| ulps.
        if (a < SmallShape)
        {
            // e^-x apart, in two halves about the rest so a subnormal product rounds once: as one exponent x's size took 255 ulps of it.
            double halfDecay = Math.Exp(-0.5 * x);
            return a * PowerTimesExp(x, a, a * Math.Log(x), -LogGammaOnePlus(a)) * halfDecay * halfDecay;
        }
        if (a < StirlingMinimumShape)
        {
            if (ReadsPowerPrefactor(a, x))
            {
                return PowerPrefactor(a, x);
            }

            double power = a * Math.Log(x);
            // At a chi-squared shape 1/Γ(a) from the table on both sides of LongPower, not Lanczos' log Γ, good to 1e-15:
            // one side alone stepped ChiSquaredCdf(0.40379303598931077, 5) down 10 ulps as x rose (#1729's Review A).
            return power < -LongPower ? LongPowerPrefactor(x, a) : ExpOverGamma(a, power - x);
        }

        if (!whole)
        {
            return Math.Exp(PlainStirlingExponent(a, x)) * Math.Sqrt(a / (2.0 * Math.PI));
        }
        DoubleDouble exponent = StirlingExponent(a, x);
        // Past 2^53 the low part can pass one, and an underflowed exponential read −0 (Review A, pass 36).
        return Math.Exp(exponent.High) * Math.Max(0.0, 1.0 + exponent.Low) * Math.Sqrt(a / (2.0 * Math.PI));
    }

    /// <summary>One less the other tail, <paramref name="rest"/> times the prefactor: its exponent in doubles where that tail
    /// is under a thousandth, whose error barely reaches one, and whole past it, where 1 − P took 57 ulps of a Q of 0.4 (#1756).</summary>
    private static double OneLess(double rest, double a, double x)
    {
        if (a < StirlingMinimumShape)
        {
            return 1.0 - (rest * Prefactor(a, x));
        }

        double plain = rest * Prefactor(a, x, whole: false);
        return 1.0 - (plain < 1e-3 ? plain : rest * Prefactor(a, x, whole: true));
    }

    /// <summary>Whether <see cref="Prefactor(double, double)"/> reads <see cref="PowerPrefactor"/>, whose factors stay normal.</summary>
    internal static bool ReadsPowerPrefactor(double a, double x) =>
        a >= SmallShape && a < StirlingMinimumShape && x > PowerPrefactorLowest && x < PowerPrefactorHighest;

    /// <summary>x^a e^-x / Γ(a) below a shape of 10 as three factors, each within an ulp.</summary>
    /// <remarks>Through a log x − x − log Γ(a) each term's rounding reached the tail, 141 ulps of ChiSquaredSf(20.09, 18.64),
    /// and 12.7 of ChiSquaredCdf(6.79, 13) at a half-integer shape (#1756).</remarks>
    private static double PowerPrefactor(double a, double x)
    {
        double rest = Math.Pow(x, a) * ReciprocalGamma(a);
        if (x < HalvedDecayFrom)
        {
            return rest * Math.Exp(-x);
        }
        double halfDecay = Math.Exp(-0.5 * x);
        return halfDecay * rest * halfDecay;
    }

    /// <summary>1/Γ(a) below a shape of 10: the table at a half-integer shape, else 1/Γ(1 + f) over Π (f + k), f the
    /// fraction of a, each f + k exact and the product kept with its residuals.</summary>
    internal static double ReciprocalGamma(double a)
    {
        if (IsHalfIntegerShape(a))
        {
            return a * ReciprocalGammaOnePlusHalfInteger(a);
        }

        int whole = (int)a;
        double fraction = a - whole;
        double reciprocal = ReciprocalGammaOnePlusFraction(fraction);
        if (whole == 0)
        {
            return a * reciprocal;
        }

        double high = 1.0;
        double low = 0.0;
        for (int k = 1; k < whole; k++)
        {
            DoubleDouble step = DoubleDouble.Product(high, fraction + k);
            low = step.Low + (low * (fraction + k));
            high = step.High;
        }
        return reciprocal / high * (1.0 - (low / high));
    }

    /// <summary>1/Γ(1 + f) over [0, 1], within 3.7e-20 of mpmath's: its degree-16 Chebyshev fit, unrolled, where a loop over
    /// an array of its coefficients took 81 ns to 2.3.</summary>
    private static double ReciprocalGammaOnePlusFraction(double f)
    {
        double sum = -8.521820446681207e-10;
        sum = (sum * f) + 2.3630858794920432e-08;
        sum = (sum * f) - 2.4083612503619526e-07;
        sum = (sum * f) + 1.1832658730843587e-06;
        sum = (sum * f) - 1.3030643874115745e-06;
        sum = (sum * f) - 2.009377474090673e-05;
        sum = (sum * f) + 0.00012802610331145715;
        sum = (sum * f) - 0.00021523094537767175;
        sum = (sum * f) - 0.0011651711582134274;
        sum = (sum * f) + 0.007218944122549672;
        sum = (sum * f) - 0.009621971683209187;
        sum = (sum * f) - 0.04219773453633884;
        sum = (sum * f) + 0.1665386113807218;
        sum = (sum * f) - 0.04200263503401723;
        sum = (sum * f) - 0.6558780715202559;
        sum = (sum * f) + 0.5772156649015329;
        sum = (sum * f) + 1.0;
        return sum;
    }

    /// <summary>x^a e^-x / Γ(a) past LongPower in a log x: x^a by Math.Pow, or where that is subnormal as mantissa and
    /// exponent apart, the product rounding onto the subnormals once, where x^a and the product each did.</summary>
    private static double LongPowerPrefactor(double x, double a)
    {
        double power = Math.Pow(x, a);
        // At x = 0 the power is the prefactor's zero, which PowerOfTwo.Split would read as NaN.
        // 1/Γ(a) as PowerPrefactor takes it: e^(−log Γ(a)) held 57 ulps of ChiSquaredCdf(1.07e-23, 16.98) (Review A, pass 44).
        double rest = ReciprocalGamma(a) * Math.Exp(-x);
        if (power >= NearZero.SmallestNormal || !(x > 0.0))
        {
            return power * rest;
        }
        (double mantissa, int exponent) = PowerOfTwo.Split(x);
        (double value, int powerExponent) = PowerOfTwo.Power(mantissa, exponent, a);
        return PowerOfTwo.Scale(value * rest, powerExponent);
    }

    /// <summary>The logarithm of <see cref="Prefactor(double, double)"/> from a shape of 10, with no term of order <c>a log a</c> formed.</summary>
    /// <remarks>
    /// <c>a log x − x − log Γ(a)</c> cancels terms of order <c>a log a</c>: at a = 1e15 its rounding reaches a nat, and the
    /// chi-squared quantile's Newton steps came out short by that factor (#1690's review).
    /// </remarks>
    internal static double LogStirlingPrefactor(double a, double x)
    {
        DoubleDouble exponent = StirlingExponent(a, x);
        return exponent.High + (exponent.Low + (0.5 * Math.Log(a / (2.0 * Math.PI))));
    }

    /// <summary><see cref="Prefactor(double, double)"/> over <paramref name="divisor"/> from a shape of 10, where the prefactor is
    /// subnormal and the quotient need not be: e^high in two halves about the division, so it rounds once (#1756).</summary>
    internal static double StirlingPrefactorOver(double a, double x, double divisor)
    {
        DoubleDouble exponent = StirlingExponent(a, x);
        double half = Math.Exp(0.5 * exponent.High);
        return half * Math.Max(0.0, 1.0 + exponent.Low) * Math.Sqrt(a / (2.0 * Math.PI)) / divisor * half;
    }

    /// <summary><c>a (log(1 + σ) − σ)</c> less Stirling's correction, σ = (x − a)/a, to some 106 bits: from a double σ
    /// its rounding took a σ² ε, 1,529 ulps of P(3000, 2070) beside Temme's region (#1756).</summary>
    [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.NoInlining)]
    private static DoubleDouble StirlingExponent(double a, double x)
    {
        // At x of zero or ∞, a shape whose a log(x/a) can overflow, or an x/a that underflows, where the tail does too, the
        // double form's limits: double-double reads ∞ − ∞, and QuickLog no zero.
        if (!(x > 1e-300 * a && x < double.PositiveInfinity && a < 1e300))
        {
            return new DoubleDouble(PlainStirlingExponent(a, x), 0.0);
        }

        (double high, double low) = LogExcess(a, x);
        DoubleDouble corrected = DoubleDouble.Sum(high, -LogStirlingCorrection(a));
        return DoubleDouble.Sum(corrected.High, corrected.Low + low);
    }

    private static double PlainStirlingExponent(double a, double x) =>
        (a * LogOnePlusMinus((x - a) / a, x / a)) - LogStirlingCorrection(a);

    /// <summary><c>a (log(1 + σ) − σ)</c>, σ = (x − a)/a, to some 106 bits, for x/a a positive double and a under 1e300.</summary>
    /// <remarks>log(x/a) from a quotient and its residual, σ from x − a's two-sum and its own: each residual is exact, so
    /// neither quotient need be rounded correctly, and a reciprocal serves. Their difference cancels at most the bits σ²/2
    /// leaves of σ, from a log within 5.2e-24 (#1756).</remarks>
    private static DoubleDouble LogExcess(double a, double x)
    {
        double inverse = 1.0 / a;
        DoubleDouble difference = DoubleDouble.Sum(x, -a);
        double sigma = difference.High * inverse;
        double sigmaLow = (ExactProduct.Residual(difference.High, sigma, a) + difference.Low) * inverse;
        if (Math.Abs(sigma) < ExcessSeriesReach)
        {
            return LogExcessSeries(a, sigma, sigmaLow);
        }

        double ratio = x * inverse;
        (double logHigh, double logLow) = DoubleDouble.QuickLog(ratio);
        DoubleDouble inner = DoubleDouble.Sum(logHigh, -sigma);
        DoubleDouble product = DoubleDouble.Product(a, inner.High);
        // log(1 + d) to its second order, d the quotient's relative residual: at x an ulp below a, d² is all of σ²/2.
        double residual = ExactProduct.Residual(x, ratio, a) / x;
        double innerLow = inner.Low + logLow + residual + (0.5 * residual * residual) - sigmaLow;
        return DoubleDouble.Sum(product.High, product.Low + (a * innerLow));
    }

    /// <summary><c>a (−σ²/2 + σ³/3 − σ⁴/4 + σ⁵/5)</c> for σ + <paramref name="sigmaLow"/>: the log's absolute 1e-32
    /// times a was the tail's whole error near x = a, 188 ulps of ChiSquaredCdf at a = 1e18 and 123 at a = 11,300 (#1756).</summary>
    [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.NoInlining)]
    private static DoubleDouble LogExcessSeries(double a, double sigma, double sigmaLow)
    {
        DoubleDouble square = DoubleDouble.Product(sigma, sigma);
        DoubleDouble leading = DoubleDouble.Product(a, -0.5 * square.High);
        double cubic = sigma * square.High * ((1.0 / 3.0) - (sigma * (0.25 - (0.2 * sigma))));
        return DoubleDouble.Sum(leading.High, leading.Low + (a * (cubic - (0.5 * square.Low) - (sigma * sigmaLow))));
    }

    /// <summary>P(a, x), or Q(a, x) when <paramref name="upper"/>, with the prefactor x^a e^-x / Gamma(a).</summary>
    /// <remarks>
    /// For a caller that needs both, as the incomplete beta's large-shape expansion does to run the
    /// recurrence Q(a + 1, x) = Q(a, x) + x^a e^-x / Gamma(a + 1): below Temme's region the series
    /// or fraction shares the prefactor instead of forming it twice. Validated arguments only.
    /// </remarks>
    internal static double RegularizedWithPrefactor(double a, double x, bool upper, out double prefactor)
    {
        prefactor = Prefactor(a, x);
        if (upper && IsClosedFormQ(a, x))
        {
            return HalfIntegerQ((int)(2.0 * a), x);
        }
        if (a > TemmeMinimumShape)
        {
            return upper ? RegularizedQ(a, x) : RegularizedP(a, x);
        }

        if (x < a + 1.0 && !(upper && FractionReads(a, x)))
        {
            if (a < SmallShape || (upper && TailSeriesHolds(a, x)))
            {
                return SmallShapeTail(a, x, upper);
            }
            double lower = prefactor * SeriesSum(a, x);
            return upper ? 1.0 - lower : lower;
        }

        double tail = prefactor * ContinuedFraction(a, x);
        return upper ? tail : 1.0 - tail;
    }

    // P(a, x) = x^a e^-x / Gamma(a) * sum_{n>=0} x^n / (a(a+1)...(a+n)).
    private static double SeriesP(double a, double x)
    {
        // The prefactor first: after the sum, dynamic PGO took ChiSquaredCdf(1300, 2000) to 206 ns against 118 (#1756).
        double prefactor = Prefactor(a, x);
        return SeriesSum(a, x) * prefactor;
    }

    // The sum above without its prefactor, so a caller can supply its own.
    private static double SeriesSum(double a, double x)
    {
        double term = 1.0 / a;
        double sum = term;
        // Each addition's rounding, by Dekker's fast two-sum as the terms are positive and under the sum: near x = a, where
        // some 3√a terms count, the sum's roundings took 5.5 ulps of it, and 11 of a Q of 0.48 read as one less P (#1756).
        double carry = 0.0;
        for (int n = 1; n <= MaxIterations; n++)
        {
            term *= x / (a + n);
            double next = sum + term;
            carry += term - (next - sum);
            sum = next;
            if (Math.Abs(term) < Math.Abs(sum) * Epsilon)
            {
                break;
            }
        }

        return sum + carry;
    }

    // Q(a, x) = x^a e^-x / Gamma(a) * 1/(x+1-a - 1(1-a)/(x+3-a - 2(2-a)/(x+5-a - ...))).
    private static double ContinuedFractionQ(double a, double x) =>
        Prefactor(a, x) * ContinuedFraction(a, x);

    /// <summary>The fraction above without its prefactor, for the same reason as SeriesSum.</summary>
    /// <remarks>Steed's forward sum, its denominators positive past x = a, finds the terms it needs; past ForwardFractionSteps
    /// its errors have grown, and modified Lentz's forward product held 100 ulps of a Q of 0.03 at some 80 terms, so the fraction
    /// is read back from its tail (#1756). Called only at a up to x.</remarks>
    private static double ContinuedFraction(double a, double x)
    {
        if (x < TailFractionBelow)
        {
            return FractionFromTail(a, x, (int)(TailFractionTerms / x) + TailFractionMargin);
        }

        double b = x + 1.0 - a;
        double d = 1.0 / b;
        double h = d;
        double delta = d;
        int taken = MaxIterations;

        for (int i = 1; i <= MaxIterations; i++)
        {
            double an = -i * (i - a);
            b += 2.0;
            double next = 1.0 / (b + (an * d));
            delta *= -an * d * next;
            d = next;
            h += delta;

            if (Math.Abs(delta) < Epsilon * Math.Abs(h))
            {
                taken = i;
                break;
            }
        }

        return taken <= ForwardFractionSteps ? h : FractionFromTail(a, x, taken + (taken / 4) + ForwardFractionMargin);
    }

    /// <summary>The fraction from its last term back, its error a few ulps however many terms.</summary>
    private static double FractionFromTail(double a, double x, int terms)
    {
        double start = x + 1.0 - a;
        double tail = 0.0;
        for (int i = terms; i > 0; i--)
        {
            tail = -i * (i - a) / (start + (2.0 * i) + tail);
        }

        return 1.0 / (start + tail);
    }
}
