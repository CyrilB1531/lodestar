namespace Lodestar.Stats.Internal;

/// <summary>The log-gamma function and the two regularized incomplete gammas.</summary>
/// <remarks>
/// Lanczos (1964) for the log-gamma; the series-below / continued-fraction-above split, by
/// modified Lentz (1976), for the incomplete pair -- except <c>Q</c> at an integer or half-integer
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

    // A floor well above where the recurrence's denominator would underflow: Lentz
    // divides by it, so a zero one is nudged here instead of an infinity that never recovers.
    private const double Tiny = 1e-300;

    // At 2a = 100 the sum beat the iteration at x = a and x = 2a (default job: 44 vs 177 and 71 ns,
    // a short run had the fraction ahead at 160). Past x = 700 e^-x nears the subnormals, Q need not.
    private const double MaxClosedFormTwiceA = 100.0;
    private const double MaxClosedFormX = 700.0;

    private const double TwoOverSqrtPi = 1.1283791670955126;

    // cephes igam's region for 20 < a < 200, kept for every larger a: why is in Temme's remarks.
    private const double TemmeMinimumShape = 20.0;
    private const double TemmeMaximumDeviation = 0.3;

    // Stirling's correction is within 3e-17 of log Gamma*(a) from here, while the Lanczos
    // prefactor a log x - x - log Gamma(a) cancels a digit per decade of a log a.
    private const double StirlingMinimumShape = 10.0;

    // Below it the series for log(1 + t) - t takes at most 16 terms; above it the logarithm's
    // rounding costs under 2e-15 of t^2 / 2.
    private const double LogSeriesMaximum = 0.1;

    private const int TemmeTerms = 12;
    private const int TemmeDegree = 16;

    /// <summary>The coefficients d(k, n) of c_k(eta) = sum over n of d(k, n) eta^n, row by row.</summary>
    /// <remarks>
    /// Temme (1979), as DLMF 8.12.8 and 8.12.9 state it: c_0 = 1/(lambda - 1) - 1/eta and
    /// c_k = c_(k-1)'/eta + (-1)^k g_k/(lambda - 1), g_k Stirling's coefficients for Gamma*.
    /// Expanded here by exact rational arithmetic, lambda - 1 reverted from eta^2 / 2 =
    /// lambda - 1 - log lambda, then rounded: c_0 starts -1/3, 1/12, -2/135 and c_1 at -1/540,
    /// DLMF 8.12.10's values. Twelve rows of sixteen matched twenty-five of twenty-five to
    /// 2e-16 over a in (20, 1e8] and |x - a| / a &lt;= 0.3.
    /// </remarks>
    private static readonly double[] TemmeCoefficients =
    [
        // c_0
        -0.3333333333333333, 0.08333333333333333, -0.014814814814814815, 0.0011574074074074073,
        0.0003527336860670194, -0.0001787551440329218, 3.919263178522438e-05, -2.185448510679992e-06,
        -1.85406221071516e-06, 8.296711340953087e-07, -1.7665952736826078e-07, 6.707853543401498e-09,
        1.0261809784240309e-08, -4.382036018453353e-09, 9.14769958223679e-10, -2.5514193994946248e-11,
        // c_1
        -0.001851851851851852, -0.003472222222222222, 0.0026455026455026454, -0.0009902263374485596,
        0.00020576131687242798, -4.018775720164609e-07, -1.8098550334489977e-05, 7.64916091608111e-06,
        -1.6120900894563446e-06, 4.647127802807434e-09, 1.378633446915721e-07, -5.752545603517705e-08,
        1.1951628599778148e-08, -1.7543241719747647e-11, -1.0091543710600413e-09, 4.162792991842583e-10,
        // c_2
        0.004133597883597883, -0.0026813271604938273, 0.0007716049382716049, 2.0093878600823047e-06,
        -0.0001073665322636516, 5.2923448829120125e-05, -1.2760635188618728e-05, 3.423578734096138e-08,
        1.3721957309062934e-06, -6.298992138380055e-07, 1.4280614206064242e-07, -2.0477098421990866e-10,
        -1.409252991086752e-08, 6.228974084922022e-09, -1.3670488396617114e-09, 9.428356159014678e-13,
        // c_3
        0.0006494341563786008, 0.00022947209362139917, -0.0004691894943952557, 0.00026772063206283885,
        -7.561801671883977e-05, -2.396505113867297e-07, 1.1082654115347302e-05, -5.6749528269915965e-06,
        1.4230900732435883e-06, -2.7861080291528143e-11, -1.6958404091930278e-07, 8.099464905388083e-08,
        -1.9111168485973655e-08, 2.3928620439808118e-12, 2.0620131815488797e-09, -9.460496661855133e-10,
        // c_4
        -0.0008618882909167117, 0.0007840392217200666, -0.0002990724803031902, -1.4638452578843418e-06,
        6.641498215465122e-05, -3.968365047179435e-05, 1.1375726970678419e-05, 2.507497226237533e-10,
        -1.6954149536558305e-06, 8.907507532205309e-07, -2.292934834000805e-07, 2.956794137544049e-11,
        2.8865829742708783e-08, -1.4189739437803219e-08, 3.4463580499464896e-09, -2.3024517174528067e-13,
        // c_5
        -0.00033679855336635813, -6.972813758365857e-05, 0.0002772753244959392, -0.00019932570516188847,
        6.797780477937208e-05, 1.419062920643967e-07, -1.3594048189768693e-05, 8.018470256334202e-06,
        -2.291481176508095e-06, -3.252473551298454e-10, 3.4652846491085265e-07, -1.8447187191171344e-07,
        4.8240967037894184e-08, -1.7989466721743514e-14, -6.306194500013523e-09, 3.162417628774568e-09,
        // c_6
        0.0005313079364639922, -0.0005921664373536939, 0.0002708782096718045, 7.902353232660328e-07,
        -8.153969367561969e-05, 5.61168275310625e-05, -1.8329116582843375e-05, -3.0796134506033047e-09,
        3.465155368803609e-06, -2.0291327396058603e-06, 5.788792863149004e-07, 2.338630673826657e-13,
        -8.828600746330484e-08, 4.7435958880408125e-08, -1.2545415020710383e-08, 8.649648858010293e-14,
        // c_7
        0.00034436760689237765, 5.171790908260592e-05, -0.00033493161081142234, 0.0002812695154763237,
        -0.00010976582244684731, -1.2741009095484485e-07, 2.7744451511563645e-05, -1.8263488805711332e-05,
        5.7876949497350525e-06, 4.93875893393627e-10, -1.0595367014026043e-06, 6.166714376110408e-07,
        -1.7562973359060463e-07, -1.297447328701544e-12, 2.695423606288966e-08, -1.4578352908731272e-08,
        // c_8
        -0.0006526239185953094, 0.0008394987206720873, -0.000438297098541721, -6.969091458420552e-07,
        0.00016644846642067547, -0.00012783517679769218, 4.629953263691304e-05, 4.557909867922708e-09,
        -1.0595271125805195e-05, 6.783342904865167e-06, -2.1075476666258803e-06, -1.7213731432817144e-11,
        3.773587741611098e-07, -2.1867506700122867e-07, 6.220228804018927e-08, 6.597703826733e-16,
        // c_9
        -0.0005967612901927463, -7.204895416020011e-05, 0.0006782308837667328, -0.0006401475260262758,
        0.00027750107634328704, 1.819700838046515e-07, -8.479507117068503e-05, 6.105192082501531e-05,
        -2.1073920183404862e-05, -8.858589014125599e-10, 4.5284535953805374e-06, -2.8427815022504407e-06,
        8.708234177864641e-07, 3.6886101871706966e-12, -1.534469519070206e-07, 8.862466778790695e-08,
        // c_10
        0.0013324454494800656, -0.0019144384985654776, 0.0011089369134596636, 9.9324041226423e-07,
        -0.0005087450129309319, 0.00042735056665392886, -0.00016858853767910798, -8.1301893922785e-09,
        4.5284402370562144e-05, -3.127053674781734e-05, 1.044986828530338e-05, 4.8435226265680926e-11,
        -2.148256587345626e-06, 1.329369701097492e-06, -4.029569309210103e-07, -1.756787766632329e-13,
        // c_11
        0.001579727660730835, 0.00016251626278391583, -0.0020633421035543276, 0.00213896861856891,
        -0.0010108559391263003, -3.99127055299192e-07, 0.0003623502508476469, -0.00028143901463712157,
        0.00010449513336495887, 2.12114184918303e-09, -2.5779417251947842e-05, 1.7281818956040464e-05,
        -5.641377387290428e-06, -1.1024320105776174e-11, 1.1223224418895174e-06, -6.869339637952674e-07,
    ];

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

        if (IsTemmeRegion(a, x))
        {
            return Temme(a, x, upper: false);
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

        if (IsClosedFormQ(a, x))
        {
            return HalfIntegerQ((int)(2.0 * a), x);
        }

        if (IsTemmeRegion(a, x))
        {
            return Temme(a, x, upper: true);
        }

        return x < a + 1.0 ? 1.0 - SeriesP(a, x) : ContinuedFractionQ(a, x);
    }

    // S1244: 2a landing on an integer exactly is what a chi-squared tail's a = dof/2
    // always does; a shape near one without being one takes the iterative path.
#pragma warning disable S1244
    private static bool IsClosedFormQ(double a, double x) =>
        2.0 * a <= MaxClosedFormTwiceA && x <= MaxClosedFormX && 2.0 * a == Math.Floor(2.0 * a);
#pragma warning restore S1244

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
        double series = (1.0 / 12.0) - (w * ((1.0 / 360.0) - (w * ((1.0 / 1260.0) - (w * ((1.0 / 1680.0)
            - (w * ((1.0 / 1188.0) - (w * ((691.0 / 360360.0) - (w / 156.0)))))))))));
        return series / z;
    }

    private static bool IsTemmeRegion(double a, double x) =>
        a > TemmeMinimumShape && Math.Abs(x - a) < TemmeMaximumDeviation * a;

    /// <summary>P(a, x) or Q(a, x) by Temme's uniform asymptotic expansion.</summary>
    /// <remarks>
    /// Q = erfc(eta sqrt(a/2)) / 2 + R and P = erfc(-eta sqrt(a/2)) / 2 - R, with R = e^(-a eta^2/2)
    /// / sqrt(2 pi a) times the sum of c_k(eta) a^-k, eta^2 / 2 = lambda - 1 - log lambda, lambda =
    /// x / a, eta signed as x - a (DLMF 8.12); asymptotic, so the sum also stops at a growing term.
    /// The series needs about 7 sqrt(a) terms at x = a. This held 1e-13 of the full series over
    /// |x - a| / a &lt;= 0.3 up to a = 1e10; scipy's region past a = 200 is 4.5 standard deviations
    /// wide, and its capped series is 60% wrong just past that at a = 1e8 (docs/equivalence.md).
    /// </remarks>
    private static double Temme(double a, double x, bool upper)
    {
        double sigma = (x - a) / a;
        double eta = Math.Sqrt(-2.0 * LogOnePlusMinus(sigma, x / a));
        if (x < a)
        {
            eta = -eta;
        }

        double sum = 0.0;
        double inversePower = 1.0;
        double previous = double.PositiveInfinity;
        for (int k = 0; k < TemmeTerms; k++)
        {
            int row = k * TemmeDegree;
            double coefficient = TemmeCoefficients[row + TemmeDegree - 1];
            for (int n = TemmeDegree - 2; n >= 0; n--)
            {
                coefficient = (coefficient * eta) + TemmeCoefficients[row + n];
            }

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

        double remainder = Math.Exp(-0.5 * a * eta * eta) / Math.Sqrt(2.0 * Math.PI * a) * sum;
        double argument = eta * Math.Sqrt(a / 2.0);
        return upper
            ? (0.5 * Normal.Erfc(argument)) + remainder
            : (0.5 * Normal.Erfc(-argument)) - remainder;
    }

    // x^a e^-x / Gamma(a), from a = 10 through Stirling so no term of order a log a cancels; the
    // chi-squared density is this over x (#1158).
    internal static double Prefactor(double a, double x)
    {
        if (a < StirlingMinimumShape)
        {
            return Math.Exp((a * Math.Log(x)) - x - LogGamma(a));
        }

        double exponent = (a * LogOnePlusMinus((x - a) / a, x / a)) - LogStirlingCorrection(a);
        return Math.Exp(exponent) * Math.Sqrt(a / (2.0 * Math.PI));
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

        if (x < a + 1.0)
        {
            double lower = prefactor * SeriesSum(a, x);
            return upper ? 1.0 - lower : lower;
        }

        double tail = prefactor * ContinuedFraction(a, x);
        return upper ? tail : 1.0 - tail;
    }

    // P(a, x) = x^a e^-x / Gamma(a) * sum_{n>=0} x^n / (a(a+1)...(a+n)).
    private static double SeriesP(double a, double x) => SeriesSum(a, x) * Prefactor(a, x);

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
        Prefactor(a, x) * ContinuedFraction(a, x);

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
