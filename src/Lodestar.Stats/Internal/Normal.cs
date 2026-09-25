namespace Lodestar.Stats.Internal;

/// <summary>The complementary error function, and the standard normal's upper tail.</summary>
/// <remarks>
/// Accuracy still comes from erfc(x) = Q(1/2, x^2): <see cref="Gamma"/>'s continued fraction is
/// sampled once, at type initialization, into piecewise Chebyshev interpolants of the slowly varying
/// erfcx(x) = e^(x^2) erfc(x) over y = 4/(4+x), the substitution S. G. Johnson's Faddeeva package
/// (MIT) uses -- nothing else is shared, ADR 0002. A call runs one polynomial and one exponential.
/// Against scipy.special.erfc on 60,001 points over [-6, 27.5] the worst relative gap is 7e-15,
/// where the iteration alone reached 1e-13.
/// </remarks>
internal static class Normal
{
    // Against scipy.special.erfcx, 16 intervals of degree 8 already reached its 2e-15 rounding,
    // 64 of degree 5 stopped at 6e-14, and 32 of degree 8 doubles the smallest that sufficed.
    private const int Intervals = 32;
    private const int Coefficients = 9;

    // erfc(27.3) is 4e-326, under half the smallest subnormal (2.5e-324), so it rounds to zero.
    private const double Cutoff = 27.3;

    private const double LowestY = 4.0 / (4.0 + Cutoff);

    private const double IntervalsPerY = Intervals / (1.0 - LowestY);

    private static readonly double[] Table = SampleScaledErfc();

    // Where the expansion takes over: Sf(30) is 5e-198, still a double, so the two forms overlap
    // over twenty orders of magnitude and the tests compare them there.
    private const double AsymptoticFrom = 30.0;

    // The series is asymptotic, so it diverges eventually; at z = 30 the terms shrink by 1/900
    // each and eight reach 1e-24, far past what the sum's leading 1 can carry.
    private const int AsymptoticTerms = 8;

    internal static double Erfc(double x)
    {
        if (double.IsNaN(x))
        {
            return double.NaN;
        }

        // erfc(-x) = 2 - erfc(x): the table covers the non-negative half only.
        if (x < 0.0)
        {
            return 2.0 - Erfc(-x);
        }

        // Exact sentinel: erfc(0) = 1 is the definition, and it keeps Sf(0) the exact one half
        // Quantile's own sentinel states, rather than the interpolant's value there.
#pragma warning disable S1244
        if (x == 0.0)
#pragma warning restore S1244
        {
            return 1.0;
        }

        return x > Cutoff ? 0.0 : ScaledErfc(x) * Math.Exp(-x * x);
    }

    /// <summary>erfc(sqrt(s)), which is Q(1/2, s): the chi-squared tail at one degree of freedom.</summary>
    /// <remarks>
    /// Taking s rather than its root keeps the exponent exact: squaring sqrt(s) back would
    /// round, and in the far tail e^(-s) turns that rounding into a relative error.
    /// </remarks>
    internal static double ErfcOfSquareRoot(double s) =>
        s > Cutoff * Cutoff ? 0.0 : ScaledErfc(Math.Sqrt(s)) * Math.Exp(-s);

    /// <summary>The standard normal's upper tail: P(Z &gt; z).</summary>
    internal static double Sf(double z) => 0.5 * Erfc(z / Math.Sqrt(2.0));

    /// <summary>The z with <c>P(Z &gt; z) = p</c>: the inverse of <see cref="Sf"/>.</summary>
    /// <remarks>
    /// The root of <see cref="Sf"/> itself, which <see cref="TailInversion"/> reaches from
    /// <see cref="RationalUpperQuantile"/>: the rational approximation only chooses where Newton
    /// starts, so there is still one approximation to keep right instead of two that must agree.
    /// </remarks>
    internal static double Quantile(double p)
    {
        if (double.IsNaN(p) || p <= 0.0 || p >= 1.0)
        {
            throw new ArgumentOutOfRangeException(
                nameof(p), p, "The tail probability must lie strictly inside (0, 1).");
        }

        // S1244: an exact half is the distribution's own symmetric point, not a
        // value that drifted there by rounding -- Sf(0) is exactly 0.5.
#pragma warning disable S1244
        if (p == 0.5)
#pragma warning restore S1244
        {
            return 0.0;
        }

        // Symmetric about zero: p > 1/2 reduces to its mirror 1 - p < 1/2, exact there by
        // Sterbenz, and the side on which the inversion's lower bound of zero holds.
        return p > 0.5
            ? -TailInversion.InvertUpperTail(default(UpperTail), 1.0 - p, RationalUpperQuantile(1.0 - p))
            : TailInversion.InvertUpperTail(default(UpperTail), p, RationalUpperQuantile(p));
    }

    // x lies in [0, Cutoff]; y = 4/(4+x) then lies in [LowestY, 1], and each interval's
    // polynomial is in u = 2t - 1, t being y's position inside it.
    private static double ScaledErfc(double x)
    {
        double position = ((4.0 / (4.0 + x)) - LowestY) * IntervalsPerY;
        int interval = Math.Min((int)position, Intervals - 1);
        double u = (2.0 * (position - interval)) - 1.0;

        int offset = interval * Coefficients;
        double sum = Table[offset + Coefficients - 1];
        for (int k = Coefficients - 2; k >= 0; k--)
        {
            sum = (sum * u) + Table[offset + k];
        }

        return sum;
    }

    // Chebyshev interpolation at each interval's first-kind nodes, converted to power form in u
    // so a call runs Horner's rule; the sweep the class remarks cite went through the conversion.
    private static double[] SampleScaledErfc()
    {
        double[] table = new double[Intervals * Coefficients];
        for (int interval = 0; interval < Intervals; interval++)
        {
            double[] chebyshev = ChebyshevCoefficients(SampleInterval(interval));
            AddPowerForm(chebyshev, table.AsSpan(interval * Coefficients, Coefficients));
        }

        return table;
    }

    // erfcx at the interval's first-kind Chebyshev nodes, taken in y and mapped back to x.
    private static double[] SampleInterval(int interval)
    {
        double[] values = new double[Coefficients];
        for (int k = 0; k < Coefficients; k++)
        {
            double node = Math.Cos(Math.PI * (k + 0.5) / Coefficients);
            double y = LowestY + ((interval + ((node + 1.0) / 2.0)) / IntervalsPerY);
            values[k] = Gamma.ScaledUpperHalf((4.0 / y) - 4.0);
        }

        return values;
    }

    // The discrete cosine transform of those samples: the interpolant's coefficients in T_m(u).
    private static double[] ChebyshevCoefficients(double[] values)
    {
        double[] chebyshev = new double[Coefficients];
        for (int m = 0; m < Coefficients; m++)
        {
            double sum = 0.0;
            for (int k = 0; k < Coefficients; k++)
            {
                sum += values[k] * Math.Cos(Math.PI * m * (k + 0.5) / Coefficients);
            }

            chebyshev[m] = (m == 0 ? 1.0 : 2.0) * sum / Coefficients;
        }

        return chebyshev;
    }

    // T_0 = 1, T_1 = u, T_(m+1) = 2u T_m - T_(m-1), each kept as power coefficients.
    private static void AddPowerForm(double[] chebyshev, Span<double> power)
    {
        double[] previous = new double[Coefficients];
        double[] current = new double[Coefficients];
        double[] next = new double[Coefficients];
        previous[0] = 1.0;
        current[1] = 1.0;
        power[0] = chebyshev[0];
        power[1] = chebyshev[1];
        for (int m = 2; m < Coefficients; m++)
        {
            next[0] = -previous[0];
            for (int p = 1; p < Coefficients; p++)
            {
                next[p] = (2.0 * current[p - 1]) - previous[p];
            }

            for (int p = 0; p <= m; p++)
            {
                power[p] += chebyshev[m] * next[p];
            }

            (previous, current, next) = (current, next, previous);
        }
    }

    /// <summary>The logarithm of the upper tail, where the tail itself would underflow.</summary>
    /// <remarks>
    /// Anderson-Darling sums the logarithms of both tails, so a value far enough out to
    /// underflow <see cref="Sf"/> would take the whole statistic to negative infinity. Past
    /// <see cref="AsymptoticFrom"/> the tail is expanded rather than evaluated. Below it, a
    /// negative argument reads the tail as one minus the far smaller opposite tail, rather than
    /// as the logarithm of a value that has rounded to one.
    /// </remarks>
    internal static double LogSf(double z)
    {
        if (z >= AsymptoticFrom)
        {
            return Asymptotic(z);
        }
        if (z > -1.0)
        {
            return Math.Log(Sf(z));
        }

        // Sf(z) = 1 - Sf(-z) with Sf(-z) small, so the logarithm is taken of the complement
        // rather than of a sum that has already rounded to one.
        double opposite = Sf(-z);
        return Gamma.LogOnePlusMinus(-opposite, 1.0 - opposite) - opposite;
    }

    /// <summary>Mills' ratio asymptotically: the density's logarithm, corrected by an alternating series.</summary>
    private static double Asymptotic(double z)
    {
        double inverseSquare = 1.0 / (z * z);
        double term = 1.0;
        double sum = 1.0;
        for (int k = 1; k <= AsymptoticTerms; k++)
        {
            term *= -(2.0 * k - 1.0) * inverseSquare;
            sum += term;
        }

        return (-0.5 * z * z) - Math.Log(z) - (0.5 * Math.Log(2.0 * Math.PI)) + Math.Log(sum);
    }

    /// <summary>Wichura's AS 241 (PPND16) on the upper tail, for p in (0, 0.5).</summary>
    /// <remarks>
    /// Wichura, M. J. (1988), "Algorithm AS 241: The percentage points of the normal
    /// distribution", <em>Applied Statistics</em> 37(3), 477-484: the paper's three rational
    /// functions and coefficients, about 1e-16 relative. The seed for both quantiles here, and
    /// Fligner-Killeen's normal scores on its own, where a statistic compared at 1e-9 needs no Newton step.
    /// </remarks>
    internal static double RationalUpperQuantile(double p)
    {
        double q = 0.5 - p;
        if (q <= 0.425)
        {
            double r = 0.180625 - (q * q);
            return q * Rational(r, CentralNumerator, CentralDenominator);
        }

        double s = Math.Sqrt(-Math.Log(p));
        return s <= 5.0
            ? Rational(s - 1.6, IntermediateNumerator, IntermediateDenominator)
            : Rational(s - 5.0, FarNumerator, FarDenominator);
    }

    // Both polynomials are degree seven, lowest coefficient first; the denominator's
    // constant term is 1 in all three regions, so it is implied rather than stored.
    private static double Rational(double x, double[] numerator, double[] denominator)
    {
        double top = numerator[7];
        double bottom = denominator[6];
        for (int i = 6; i >= 0; i--)
        {
            top = (top * x) + numerator[i];
        }
        for (int i = 5; i >= 0; i--)
        {
            bottom = (bottom * x) + denominator[i];
        }

        return top / ((bottom * x) + 1.0);
    }

    private static readonly double[] CentralNumerator =
    [
        3.3871328727963666080, 133.14166789178437745, 1971.5909503065514427,
        13731.693765509461125, 45921.953931549871457, 67265.770927008700853,
        33430.575583588128105, 2509.0809287301226727,
    ];

    private static readonly double[] CentralDenominator =
    [
        42.313330701600911252, 687.18700749205790830, 5394.1960214247511077,
        21213.794301586595867, 39307.895800092710610, 28729.085735721942674,
        5226.4952788528545610,
    ];

    private static readonly double[] IntermediateNumerator =
    [
        1.42343711074968357734, 4.63033784615654529590, 5.76949722146069140550,
        3.64784832476320460504, 1.27045825245236838258, 0.241780725177450611770,
        0.0227238449892691845833, 7.74545014278341407640e-4,
    ];

    private static readonly double[] IntermediateDenominator =
    [
        2.05319162663775882187, 1.67638483018380384940, 0.689767334985100004550,
        0.148103976427480074590, 0.0151986665636164571966, 5.47593808499534494600e-4,
        1.05075007164441684324e-9,
    ];

    private static readonly double[] FarNumerator =
    [
        6.65790464350110377720, 5.46378491116411436990, 1.78482653991729133580,
        0.296560571828504891230, 0.0265321895265761230930, 1.24266094738807843860e-3,
        2.71155556874348757815e-5, 2.01033439929228813265e-7,
    ];

    private static readonly double[] FarDenominator =
    [
        0.599832206555887937690, 0.136929880922735805310, 0.0148753612908506148525,
        7.86869131145613259100e-4, 1.84631831751005468180e-5, 1.42151175831644588870e-7,
        2.04426310338993978564e-15,
    ];

    /// <summary>The standard normal tail as <see cref="TailInversion"/> reads it.</summary>
    private readonly struct UpperTail : IUpperTail
    {
        // log(sqrt(2 pi)), the standard normal density's normalizing constant.
        private const double LogSqrtTwoPi = 0.91893853320467274178;

        public double Survival(double x) => Sf(x);

        public double LogDensity(double x) => (-0.5 * x * x) - LogSqrtTwoPi;
    }
}
