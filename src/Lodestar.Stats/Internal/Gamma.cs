namespace Lodestar.Stats.Internal;

/// <summary>The log-gamma function and the two regularized incomplete gammas.</summary>
/// <remarks>
/// Lanczos (1964) for the log-gamma; the series-below / continued-fraction-above
/// split, evaluated by modified Lentz (1976), for the incomplete pair -- except
/// <c>Q</c> at an integer or half-integer shape, a finite sum (<see cref="HalfIntegerQ"/>).
/// No reference implementation is transcribed (ADR 0003). The upper tail
/// <c>Q</c> is a chi-square p-value: with <c>a = dof/2</c>, <c>x = statistic/2</c>,
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

    private const int MaxIterations = 300;
    private const double Epsilon = 3e-16;

    // A floor well above where the recurrence's denominator would underflow: Lentz
    // divides by it, so a zero one is nudged here instead of an infinity that never recovers.
    private const double Tiny = 1e-300;

    // At 2a = 100 the sum beat the iteration at x = a and x = 2a (default job: 44 vs 177 and 71 ns,
    // a short run had the fraction ahead at 160). Past x = 700 e^-x nears the subnormals, Q need not.
    private const double MaxClosedFormTwiceA = 100.0;
    private const double MaxClosedFormX = 700.0;

    private const double TwoOverSqrtPi = 1.1283791670955126;

    internal static double LogGamma(double x)
    {
        if (double.IsNaN(x) || x <= 0.0)
        {
            throw new ArgumentOutOfRangeException(
                nameof(x), x, "The log-gamma function is defined here for x > 0 only.");
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

        return x < a + 1.0 ? SeriesP(a, x) : 1.0 - ContinuedFractionQ(a, x);
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

        // S1244: 2a landing on an integer exactly is what a chi-squared tail's a = dof/2
        // always does; a shape near one without being one takes the iterative path below.
        double twiceA = 2.0 * a;
#pragma warning disable S1244
        if (twiceA <= MaxClosedFormTwiceA && x <= MaxClosedFormX && twiceA == Math.Floor(twiceA))
#pragma warning restore S1244
        {
            return HalfIntegerQ((int)twiceA, x);
        }

        return x < a + 1.0 ? 1.0 - SeriesP(a, x) : ContinuedFractionQ(a, x);
    }

    /// <summary>e^(x^2) Q(1/2, x^2), which is erfcx(x): what <see cref="Normal"/> samples its table from.</summary>
    /// <remarks>
    /// The prefactor x^a e^-s / Gamma(a) is (x / sqrt(pi)) e^-s at a = 1/2 and s = x^2, so the
    /// scaling cancels its exponential instead of multiplying one back in -- which overflows
    /// past x = 26.6, where the table still has to reach.
    /// </remarks>
    internal static double ScaledUpperHalf(double x)
    {
        double s = x * x;
        double prefactor = x / Math.Sqrt(Math.PI);
        return s < 1.5
            ? Math.Exp(s) - (prefactor * SeriesSum(0.5, s))
            : prefactor * ContinuedFraction(0.5, s);
    }

    // Q(b+1, x) = Q(b, x) + x^b e^-x / Gamma(b+1) by parts, from Q(1, x) = e^-x or Q(1/2, x) =
    // erfc(sqrt x): positive terms only, so the error grows with the term count and not with x.
    private static double HalfIntegerQ(int twiceA, double x)
    {
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
            term = TwoOverSqrtPi * Math.Sqrt(x) * Math.Exp(-x);
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

    // P(a, x) = x^a e^-x / Gamma(a) * sum_{n>=0} x^n / (a(a+1)...(a+n)).
    private static double SeriesP(double a, double x) =>
        SeriesSum(a, x) * Math.Exp((a * Math.Log(x)) - x - LogGamma(a));

    // The sum above without its prefactor, so ScaledUpperHalf can supply its own.
    private static double SeriesSum(double a, double x)
    {
        double term = 1.0 / a;
        double sum = term;
        for (int n = 1; n <= MaxIterations; n++)
        {
            term *= x / (a + n);
            sum += term;
            if (Math.Abs(term) < Math.Abs(sum) * Epsilon)
            {
                break;
            }
        }

        return sum;
    }

    // Q(a, x) = x^a e^-x / Gamma(a) * 1/(x+1-a - 1(1-a)/(x+3-a - 2(2-a)/(x+5-a - ...))),
    // evaluated by modified Lentz.
    private static double ContinuedFractionQ(double a, double x) =>
        Math.Exp((a * Math.Log(x)) - x - LogGamma(a)) * ContinuedFraction(a, x);

    // The fraction above without its prefactor, for the same reason as SeriesSum.
    private static double ContinuedFraction(double a, double x)
    {
        double b = x + 1.0 - a;
        double c = 1.0 / Tiny;
        double d = 1.0 / b;
        double h = d;

        for (int i = 1; i <= MaxIterations; i++)
        {
            double an = -i * (i - a);
            b += 2.0;

            d = (an * d) + b;
            if (Math.Abs(d) < Tiny)
            {
                d = Tiny;
            }

            c = b + (an / c);
            if (Math.Abs(c) < Tiny)
            {
                c = Tiny;
            }

            d = 1.0 / d;
            double delta = d * c;
            h *= delta;

            if (Math.Abs(delta - 1.0) < Epsilon)
            {
                break;
            }
        }

        return h;
    }
}
