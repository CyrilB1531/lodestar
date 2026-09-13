namespace Lodestar.Stats.Internal;

/// <summary>The complementary error function, and the standard normal's upper tail.</summary>
/// <remarks>
/// Built on the regularized incomplete gamma rather than on a rational
/// approximation of its own: erfc(x) = Q(1/2, x^2) for x >= 0 is an identity,
/// not a fit, so the accuracy this reaches in the far tail is the accuracy
/// <see cref="Gamma"/> already has to have for the chi-square tests.
/// </remarks>
internal static class Normal
{
    internal static double Erfc(double x)
    {
        if (double.IsNaN(x))
        {
            return double.NaN;
        }

        // erfc(-x) = 2 - erfc(x). Reflecting rather than evaluating at a negative
        // argument keeps the identity above valid, since Q takes x^2 either way.
        if (x < 0.0)
        {
            return 2.0 - Erfc(-x);
        }

        // Exact sentinel: erfc(0) = 1 is the definition, not an approximation
        // that could have accumulated rounding error to compare against.
#pragma warning disable S1244
        return x == 0.0 ? 1.0 : Gamma.RegularizedQ(0.5, x * x);
#pragma warning restore S1244
    }

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

    /// <summary>Wichura's AS 241 (PPND16) on the upper tail, for p in (0, 0.5).</summary>
    /// <remarks>
    /// Wichura, M. J. (1988), "Algorithm AS 241: The percentage points of the normal
    /// distribution", <em>Applied Statistics</em> 37(3), 477-484: the paper's three rational
    /// functions and coefficients, about 1e-16 relative. A seed for both quantiles here and
    /// never an answer on its own, so a coefficient off by a digit costs a Newton step.
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
