namespace Lodestar.Stats.Internal;

/// <summary>The regularized incomplete beta, and the Student and Fisher tails it carries.</summary>
/// <remarks>
/// One continued fraction serves three families: a t-test's p-value is a Student tail, an ANOVA's is a
/// Fisher tail, and both are the incomplete beta under a change of variable. Written from the published
/// description and evaluated by modified Lentz (1976), except with one shape large and the other small,
/// where DiDonato and Morris's asymptotic expansion takes over (#841), and with both from 1e5, where
/// Temme's uniform one does (#1690); no reference implementation is transcribed (ADR 0002).
/// </remarks>
internal static class Beta
{
    // A guard, not the stopping rule: a = b = 1e8 at its mean takes 4,600 terms and 1e12 takes
    // 86,000, where the 300 this replaces left a = b = 1e8 wrong by 0.37 (#837).
    private const int MaxIterations = 1_000_000;

    // From here both log Gamma* are Stirling's, as in Gamma.Prefactor.
    private const double StirlingMinimumShape = 10.0;
    private const double Epsilon = 3e-16;
    private const double Tiny = 1e-300;

    // Where the large-shape expansion takes over from the fraction: why is in IsLargeShapeRegion.
    private const double LargeShapeMinimum = 1000.0;

    // Past this the fraction's products of two shapes near 1e154 overflow: ContinuedFraction scales them.
    private const double FractionScaleShape = 1e150;

    // From here the laws' tails differ by (t^4 - 2t^2 - 1)/4df, under 6e-15 below t = 38.6; Normal.Sf's own rounding
    // of z/sqrt 2 costs up to 1.1e-13 near t = 37 (#1690's review).
    internal const double NormalLimitDf = 1e20;

    // To here the tail is 1/2 - (df/2) asinh(t / sqrt df) to O(df^2 log^2), 1.3e-19 at most (FirstOrderSf has why).
    private const double FirstOrderDf = 1e-12;

    // log Gamma(1/2) = log sqrt(pi), a constant StudentLogScaledBeta took a Lanczos sum for on every call.
    private const double LogSqrtPi = 0.5723649429247001;

    // From here Student's tail at double.MaxValue, about MaxValue^-df, underflows: 744.4 / 709.8 is 1.0488.
    private const double FarRootMaximumDf = 1.05;

    // log(MaxValue^2), with which StudentQuantile bounds the tail at double.MaxValue before evaluating it.
    private const double LogSquaredMaxValue = 1419.565425786768;

    private const double Ln2 = 0.6931471805599453;

    // The terms FisherTailFromLimit's first form drops are of order z = o y; below this they are below a double's precision.
    private const double LimitBound = 1e-17;

    // Below Gamma.SmallShape, where Gamma.LogGammaOnePlus holds, log(s B) is taken whole, by LogScaledBeta: the direct
    // fraction divides by a through it, and the reflected side is SmallShapeLowerTail rather than 1 - I.
    internal const double SmallShape = Gamma.SmallShape;

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
    internal static double RegularizedIncomplete(double a, double b, double x, double y, in ShapeLogs logs, double deviation)
    {
        if (double.IsNaN(a) || a <= 0.0)
        {
            throw new ArgumentOutOfRangeException(nameof(a), a, "The first shape must be positive.");
        }
        if (double.IsNaN(b) || b <= 0.0)
        {
            throw new ArgumentOutOfRangeException(nameof(b), b, "The second shape must be positive.");
        }
        if (double.IsNaN(x) || x < 0.0 || x > 1.0)
        {
            throw new ArgumentOutOfRangeException(nameof(x), x, "The argument must lie in [0, 1].");
        }

        // Exact sentinels, not a rounding-error-prone comparison: x is validated
        // to [0, 1] above, and 0/1 are the interval's own closed endpoints. An x that
        // rounded to one while y still holds digits is not the endpoint (#1158's review).
#pragma warning disable S1244
        if (x == 0.0 || (x == 1.0 && y == 0.0))
#pragma warning restore S1244
        {
            return x;
        }

        // A few ulps of rounding left [0, 1]: a Student tail read above one half and an F tail at shape 1e-19 below zero
        // (#1690's review). Clamped, not refused: a test's NaN p-value for 2e-13 of rounding would be worse.
        double value = Unclamped(a, b, x, y, logs, deviation);
        return Math.Min(Math.Max(value, 0.0), 1.0);
    }

    /// <summary>I_x(a, b) by one of the two expansions or the fraction, before RegularizedIncomplete's clamp.</summary>
    // Inlined, as ByFraction: as calls the two cost an F tail 5% at (10, 30), 98 against 103 ns (Stopwatch A/B/A/B).
    [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
    private static double Unclamped(double a, double b, double x, double y, in ShapeLogs logs, double deviation)
    {
        // Before the large-shape expansion where both hold: against a 60-digit fraction it kept 2e-13 where that one
        // drifted to 8e-12, at shapes (1e7, 1e14) (#1690's review).
        if (UniformBeta.Holds(a, b))
        {
            return UniformBeta.Regularized(a, b, x, y, deviation);
        }
        // S2234: the swapped (b, a, y, x) is the reflection I_x(a, b) = 1 - I_y(b, a), as below.
#pragma warning disable S2234
        if (IsLargeShapeRegion(a, b, x))
        {
            return LargeShapeExpansion(a, b, x, y, reflected: false);
        }
        if (IsLargeShapeRegion(b, a, y))
        {
            return LargeShapeExpansion(b, a, y, x, reflected: true);
        }
#pragma warning restore S2234
        return ByFraction(a, b, x, y, logs);
    }

    /// <summary>I_x(a, b) by the continued fraction, on whichever side of the mode it converges.</summary>
    /// <remarks>
    /// The fraction converges quickly only on the side of the distribution's mode; past it the reflection is the
    /// fast branch, not a fallback. Below SmallShape the direct side divides by a inside log(a B), and the
    /// reflected side is SmallShapeLowerTail.
    /// </remarks>
    [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
    private static double ByFraction(double a, double b, double x, double y, in ShapeLogs logs)
    {
        // S2234: the swapped (b, a) is the reflection identity I_x(a,b) = 1 - I_{1-x}(b,a), not a copy-paste slip.
#pragma warning disable S2234
        if (x >= (a + 1.0) / (a + b + 2.0))
        {
            return b < SmallShape
                ? SmallShapeLowerTail(a, b, y, logs.ScaledSecond)
                : 1.0 - (Front(a, b, x, y, logs.OfBeta) / (b * ContinuedFraction(b, a, y)));
        }
        if (a < SmallShape)
        {
            return ScaledFront(a, b, x, y, CachedOrScaled(logs.ScaledFirst, a, b)) / ContinuedFraction(a, b, x);
        }
        return Front(a, b, x, y, logs.OfBeta) / (a * ContinuedFraction(a, b, x));
#pragma warning restore S2234
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

        // Q where P passes one half. P(b, w) is w^b / Gamma(1 + b) to first order below SmallShape, near one far below w = b:
        // P(4e-52, 4e-94) is 1 - 9e-50, and 1 - direct read I_x 0 for 9e-50 at shapes (4e106, 4e-52) (#1690's review).
        bool upper = b < SmallShape ? (b * Math.Log(w)) - Gamma.LogGammaOnePlus(b) > -Ln2 : w >= b;
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

            double convolution = 0.0;
            for (int j = 1; j <= k; j++)
            {
                convolution += BernoulliRatios[j - 1] * coefficients[k - j];
            }
            coefficients[k] = halfExponent * convolution / k;

            pochhammer *= (shape - 2.0) * (shape - 1.0) * inverseN2;
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

    /// <summary>x^a (1 - x)^b / B(a, b), the factor both branches of the fraction share.</summary>
    /// <remarks>
    /// Past both shapes of 10, ShapeExponent's a log(x / p) + b log(y / q) against the mean p = a / (a + b), whose linear
    /// terms cancel, so only log(1 + t) - t is formed, and
    /// Stirling leaves sqrt(x0 b / 2 pi) Gamma*(a + b) / (Gamma*(a) Gamma*(b)). The Lanczos form
    /// subtracts log-gammas of 1.7e9 at a = b = 1e8. d = x - x0 is taken from the smaller of x and
    /// 1 - x, so the two t share one d instead of two rounded ones.
    /// </remarks>
    internal static double Front(double a, double b, double x, double y) => Front(a, b, x, y, double.NaN);

    /// <summary><c>x^a y^b / B(a, b)</c>, <paramref name="logBeta"/> being an inversion's cached <c>log B</c>, or NaN.</summary>
    /// <remarks>The Stirling branch forms its own <c>B</c>, which the cached one equals; <see cref="ScaledFront"/> takes <c>a B</c>.</remarks>
    internal static double Front(double a, double b, double x, double y, double logBeta) =>
        a < StirlingMinimumShape || b < StirlingMinimumShape
            ? Math.Exp(SmallLogPower(a, b, x, y) - CachedOrLogBeta(logBeta, a, b))
            : StirlingFront(a, b, x, y, double.NaN);

    /// <summary><c>x^a y^b / (a B(a, b))</c> below SmallShape, <paramref name="logScaledBeta"/> being <c>log(a B)</c>.</summary>
    /// <remarks>
    /// Its own method, so the shape inside the divisor cannot meet the Stirling branch, which forms <c>B</c> alone: through
    /// Front's divisor a <c>log(a B)</c> at a shape of 10 would have read <c>a</c> times off (#1690's review).
    /// </remarks>
    private static double ScaledFront(double a, double b, double x, double y, double logScaledBeta) =>
        Math.Exp(SmallLogPower(a, b, x, y) - logScaledBeta);

    /// <summary><c>a log x + b log y</c>, each logarithm on the side where its argument is small.</summary>
    /// <remarks>A y of 1e-16 is not lost to the x = 1 - y it rounds: #1158's review measured 6e-7 at shapes (5e9, 0.5).</remarks>
    [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
    private static double SmallLogPower(double a, double b, double x, double y)
    {
        double logX = x <= 0.5 ? Math.Log(x) : Gamma.LogOnePlusMinus(-y, x) - y;
        double logY = y <= 0.5 ? Math.Log(y) : Gamma.LogOnePlusMinus(-x, y) - x;
        return (a * logX) + (b * logY);
    }

    /// <summary>Front past both shapes of 10, at <paramref name="deviation"/> from the exact mean, or NaN to take it from x.</summary>
    internal static double StirlingFront(double a, double b, double x, double y, double deviation) =>
        Math.Exp(StirlingExponent(a, b, x, y, deviation)) * Math.Sqrt(a / (a + b) * b / (2.0 * Math.PI));

    /// <summary>The logarithm of <see cref="Front(double, double, double, double)"/> past a shape of 10 on both sides.</summary>
    /// <remarks>
    /// <c>a log x + b log y − log B</c> cancels terms of order <c>a log a</c>: at shapes 1e15 it lost 5 nats, and the beta
    /// inversion's Newton steps, 150 times short, stopped 14 standard deviations from the root (#1690's review).
    /// </remarks>
    internal static double LogStirlingFront(double a, double b, double x, double y) =>
        StirlingExponent(a, b, x, y, double.NaN) + (0.5 * Math.Log(a / (a + b) * b / (2.0 * Math.PI)));

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
        // Knuth's two-sum: c + e is a + b exactly.
        double c = a + b;
        double virtualB = c - a;
        double e = (a - (c - virtualB)) + (b - virtualB);
        double x0 = a / c;
        double y0 = b / c;
        double correctionX = (ExactProduct.Residual(a, c, x0) - (e * x0)) / c;
        double correctionY = (ExactProduct.Residual(b, c, y0) - (e * y0)) / c;
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
    /// <remarks>The two limits in closed form (#1690's review): from <c>df = 1e20</c> the normal law, which an
    /// infinite df is and Welch's df overflows to, as Normal.Sf computes it (NormalLimitDf); to <c>df = 1e-12</c>
    /// <see cref="FirstOrderSf"/>.</remarks>
    internal static double StudentSf(double t, double df)
    {
        if (df >= NormalLimitDf)
        {
            return Normal.Sf(t);
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
        // Below SmallShape log(a B) in StudentLogScaledBeta's form, the one the quantile's cache holds.
        return StudentSf(t, df, double.NaN, df / 2.0 < SmallShape ? StudentLogScaledBeta(df / 2.0) : double.NaN);
    }

    /// <summary><see cref="StudentSf(double, double)"/> with <c>log B(df/2, 1/2)</c> and <c>log(df/2 B(df/2, 1/2))</c> given, or NaN to take them.</summary>
    private static double StudentSf(double t, double df, double logBeta, double logScaledBeta)
    {
        if (double.IsNaN(t))
        {
            return double.NaN;
        }

        // I_{df/(df+t^2)}(df/2, 1/2) is twice the tail beyond |t|, so half of it
        // is the tail on one side and the sign says which side we are on.
        double farTail = FarStudentTail(t, df, logBeta, logScaledBeta);
        if (!double.IsNaN(farTail))
        {
            return t >= 0.0 ? farTail : 1.0 - farTail;
        }
        double tSquared = t * t;
        double denominator = df + tSquared;
        double x = df / denominator;

        // Both halves by division, so neither is 1 minus the other: which side to evaluate
        // directly is RegularizedIncomplete's choice, and a far tail stays a small number (#841).
        double complement = tSquared / denominator;
        double tail = 0.5 * RegularizedIncomplete(df / 2.0, 0.5, x, complement, new ShapeLogs(logBeta, logScaledBeta, double.NaN));
        return t >= 0.0 ? tail : 1.0 - tail;
    }

    /// <summary>Half of <c>I_x(df/2, 1/2)</c> by its leading term, where <c>x = df/(df+t²)</c> is below 1e-100; NaN otherwise.</summary>
    /// <remarks>
    /// Past <c>|t| = 1.3e154</c> the square overflows and the tail read zero, which capped every
    /// quantile there (#1158: the Cauchy's 1e-300 quantile is 3.2e299). Formed in logs from
    /// <c>log x = log df − 2 log|t| − log1p(df/t²)</c>, the leading term <c>x^a / (a B(a, ½))</c>
    /// is exact to the next one's relative size, about <c>x</c>, far below a double's precision.
    /// </remarks>
    private static double FarStudentTail(double t, double df, double logBeta, double logScaledBeta)
    {
        double magnitude = Math.Abs(t);
        if (magnitude <= 1e50)
        {
            return double.NaN;
        }
        double ratio = df / magnitude / magnitude;
        double logX = Math.Log(df) - (2.0 * Math.Log(magnitude)) - Math.Log(1.0 + ratio);
        if (logX > -230.0)
        {
            return double.NaN;
        }
        double a = df / 2.0;
        if (a < SmallShape)
        {
            // log(a B) rather than log a + log B, which cancel below SmallShape, in StudentLogScaledBeta's form, as
            // StudentTail's cache takes it, so the inversion solves this tail.
            return 0.5 * Math.Exp((a * logX) - (double.IsNaN(logScaledBeta) ? StudentLogScaledBeta(a) : logScaledBeta));
        }
        // The inversion's cached log B when it has one (#1690), else LogBeta's own, not three log-gammas.
        return 0.5 * Math.Exp((a * logX) - Math.Log(a) - CachedOrLogBeta(logBeta, a, 0.5));
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

        // Both halves by division, as in StudentSf; at f = inf the upper half is 0, and TakesLimit answers there.
        (double lower, double upper) = FisherSplit(f, dfn, dfd);
        return TakesLimit(lower, upper)
            ? FisherTailFromLimit(f, dfn, dfd, lower, upperTail: true)
            : RegularizedIncomplete(dfd / 2.0, dfn / 2.0, upper, lower, ShapeLogs.None, -FisherDeviation(f, dfn, dfd, lower));
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
        // Both halves exact: y can round to one while 1 - y still holds its digits, as at shapes
        // (1e10, 1), and the incomplete beta reads the side that keeps them.
        (double lower, double upper) = FisherSplit(f, dfn, dfd);
        return TakesLimit(lower, upper)
            ? FisherTailFromLimit(f, dfn, dfd, lower, upperTail: false)
            : RegularizedIncomplete(dfn / 2.0, dfd / 2.0, lower, upper, ShapeLogs.None, FisherDeviation(f, dfn, dfd, lower));
    }

    /// <summary>Whether a half of FisherSplit is below the smallest normal double, where it keeps too few digits.</summary>
    internal static bool TakesLimit(double lower, double upper) =>
        lower < NearZero.SmallestNormal || upper < NearZero.SmallestNormal;

    /// <summary>The F tail where <see cref="TakesLimit"/> holds, from the small half's limit.</summary>
    /// <remarks>
    /// With <c>s</c> the small half's shape, <c>o</c> the other's and <c>z = o y</c>, which is <c>d₁f/2</c> on the lower side
    /// and <c>d₂/2f</c> on the upper: <c>y^s / (s B(s, o))</c> while <c>z</c> is below 1e-17, the terms dropped being of order
    /// <c>z</c>; past it <c>o</c> is above 4e290, and the tail is the chi-squared limit <c>P(s, z)</c>, off by order 1/o. A
    /// zero half read as the support's end, a subnormal one kept too few digits, and the first form alone read
    /// <c>FisherSnedecorSf(1, 1, 1e308)</c> 0.202 for 0.317 and <c>FisherSnedecorCdf(1e-16, 1, 1e308)</c> 0 (#1690's review).
    /// </remarks>
    internal static double FisherTailFromLimit(double f, double dfn, double dfd, double lower, bool upperTail)
    {
        (double s, double other, double logY, double z, bool isLower) = SmallHalf(f, dfn, dfd, lower);
        if (z < LimitBound)
        {
            double logHalfTail = (s * logY) - LogShapeBeta(s, other);
            return upperTail == isLower ? -NearZero.Expm1(logHalfTail) : Math.Exp(logHalfTail);
        }
        return upperTail == isLower ? Gamma.RegularizedQ(s, z) : Gamma.RegularizedP(s, z);
    }

    /// <summary>The F density where <see cref="TakesLimit"/> holds, from the same two limits as the tail.</summary>
    internal static double FisherDensityFromLimit(double f, double dfn, double dfd, double lower)
    {
        (double s, double other, double logY, double z, _) = SmallHalf(f, dfn, dfd, lower);
        return z < LimitBound
            ? Math.Exp((s * logY) + Math.Log(s) - LogShapeBeta(s, other) - Math.Log(f))
            : Gamma.Prefactor(s, z) / f;
    }

    /// <summary>The F root whose small half of FisherSplit is below the smallest normal double, by its limit; NaN otherwise.</summary>
    /// <remarks>
    /// The beta inversion read a subnormal <c>y</c> with too few digits, 2e-5 off at f = 1e-318 (#1690's review).
    /// <paramref name="lower"/> and <paramref name="upper"/> are the target's two tails, the smaller of each pair read directly.
    /// </remarks>
    internal static double FisherRootFromLimit(double lower, double upper, double dfn, double dfd)
    {
        double low = HalfRoot(lower, upper, dfn, dfd, isLower: true);
        return double.IsNaN(low) ? HalfRoot(upper, lower, dfn, dfd, isLower: false) : low;
    }

    /// <summary>The small half, as its shape, the other shape, <c>log y</c> and <c>z = o y</c>, from the degrees of freedom and f.</summary>
    private static (double S, double Other, double LogY, double Z, bool IsLower) SmallHalf(
        double f, double dfn, double dfd, double lower) =>
        lower < NearZero.SmallestNormal
            ? (dfn / 2.0, dfd / 2.0, Math.Log(dfn) + Math.Log(f) - Math.Log(dfd), dfn * f / 2.0, true)
            : (dfd / 2.0, dfn / 2.0, Math.Log(dfd) - Math.Log(dfn) - Math.Log(f), dfd / f / 2.0, false);

    /// <summary>The f at which the half <paramref name="isLower"/> names has tail <paramref name="own"/>, if that half is then below the smallest normal; NaN otherwise.</summary>
    private static double HalfRoot(double own, double complement, double dfn, double dfd, bool isLower)
    {
        double s = isLower ? dfn / 2.0 : dfd / 2.0;
        double other = isLower ? dfd / 2.0 : dfn / 2.0;
        double logOwn = own <= complement ? Math.Log(own) : NearZero.Log1P(-complement);
        double logY = (logOwn + LogShapeBeta(s, other)) / s;

        // f from the logs, not from z = o y, which is smaller still and kept fewer digits as a subnormal.
        double logRatio = Math.Log(dfd) - Math.Log(dfn);
        double f = Math.Exp(isLower ? logY + logRatio : logRatio - logY);
        double z = isLower ? dfn * f / 2.0 : dfd / f / 2.0;
        if (z >= LimitBound)
        {
            z = own <= complement ? GammaQuantile.Invert(own, s, upper: false) : GammaQuantile.Invert(complement, s, upper: true);
            f = isLower ? 2.0 * z / dfn : dfd / (2.0 * z);
        }
        // An f that over- or underflowed is no root: at dfs (1e189, 5e-207) the lower half's f underflowed to 0, whose half
        // is 0 too, and the F quantile read 9e-73, a tail of 7e-205, for a root of 1.17e44 (#1690's review).
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
    private static double LogScaledBeta(double s, double other) => Gamma.LogGammaOnePlus(s) - LogGammaIncrement(other, s);

    /// <summary><c>log(s B(s, ½))</c>, Student's, by three log-gammas: its every reader exponentiates it.</summary>
    /// <remarks>
    /// There 1e-15 absolute is 1e-15 relative, and with the other shape ½, where <c>log Γ</c> is 0.57, three log-gammas
    /// hold it and form no <c>log s</c>; LogScaledBeta's ten-step increment cost StudentSf(2, 0.05) 1.8 times 724c7912's
    /// 126 ns (#1690's review). Student's tail, quantile, far tail and density all take this one, so each inverts the
    /// others' function; F keeps LogScaledBeta, which SmallShapeLowerTail needs to <c>s ε</c>.
    /// </remarks>
    internal static double StudentLogScaledBeta(double s) =>
        Gamma.LogGammaOnePlus(s) + LogSqrtPi - Gamma.LogGamma(0.5 + s);

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
    private static double SmallShapeLowerTail(double a, double b, double y, double logScaledSecond)
    {
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
        return -NearZero.Expm1(-delta) - (b * sum * Math.Exp(-delta));
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
        return ((large - 0.5) * Gamma.LogOnePlusMinus(ratio, 1.0 + ratio)) - (0.5 * ratio) + (small * Math.Log(large + small));
    }

    /// <summary><c>d₁f/(d₁f + d₂) − d₁/(d₁ + d₂)</c>, the lower half's distance from its mean, from <c>f − 1</c> exactly.</summary>
    /// <remarks>
    /// It is <c>(f − 1) (lower/f) d₂/(d₁ + d₂)</c>, products alone, where the two fractions each round through their own
    /// sum; read only where both shapes reach the uniform expansion, whose law is narrow enough for that rounding to show.
    /// The last factor is <c>1/(1 + d₁/d₂)</c>: <c>d₁ + d₂</c> overflowed at dfs of 1e308 and every tail read ½ (#1690's review).
    /// </remarks>
    internal static double FisherDeviation(double f, double dfn, double dfd, double lower) =>
        UniformBeta.Holds(dfn / 2.0, dfd / 2.0) ? (f - 1.0) * (lower / f) / (1.0 + (dfn / dfd)) : double.NaN;

    /// <summary><c>y = d₁f / (d₁f + d₂)</c> and <c>1 − y</c>, each by division so neither is one minus the other.</summary>
    /// <remarks>
    /// Where <c>d₁f</c> or the sum overflows, both come from <c>r = (d₂/d₁)/f</c> as <c>1/(1 + r)</c> and
    /// <c>r/(1 + r)</c>, which is <c>(1, 0)</c> at <c>f = +∞</c>: forming <c>∞/∞</c> there made the lower
    /// tail throw and the density NaN (#1158's review).
    /// </remarks>
    internal static (double Lower, double Upper) FisherSplit(double f, double dfn, double dfd)
    {
        double scaled = dfn * f;
        double denominator = dfd + scaled;
        if (!double.IsInfinity(denominator))
        {
            return (scaled / denominator, dfd / denominator);
        }
        double ratio = dfd / dfn / f;
        return (1.0 / (1.0 + ratio), ratio / (1.0 + ratio));
    }

    /// <summary>The t with <c>P(T &gt; t) = p</c>: the inverse of <see cref="StudentSf(double, double)"/>.</summary>
    /// <remarks>
    /// The root of <see cref="StudentSf(double, double)"/> itself, reached by <see cref="TailInversion"/>
    /// rather than read off a rational approximation of its own: the seed only decides how
    /// many tail evaluations Newton needs, so there is no second approximation to keep in
    /// agreement with the tail. Bisection took about sixty; this takes one to three. From df 1e20 it
    /// is the normal quantile, scipy's ndtri, as at an infinite df: not Normal.Sf's root, by up to 1e-13.
    /// </remarks>
    internal static double StudentQuantile(double p, double df)
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

        // S1244: an exact half is the distribution's own symmetric point, not a
        // value that drifted there by rounding -- StudentSf(0, df) is exactly
        // 0.5 for every df, so the shortcut is correct only at that literal.
#pragma warning disable S1244
        if (p == 0.5)
#pragma warning restore S1244
        {
            return 0.0;
        }

        // The two limits StudentSf takes in closed form: the normal law's inverse tail, and FirstOrderSf's inverse,
        // sqrt(df) sinh(2 (1/2 - p) / df), whose sinh overflows to the infinity past every double (#1690's review).
        if (df >= NormalLimitDf)
        {
            return -Ndtri.Of(p);
        }
        if (df <= FirstOrderDf)
        {
            double w = 2.0 * (0.5 - p) / df;
            return FirstOrderRoot(w, df);
        }

        // Symmetric about zero, so p > 1/2 reduces to its mirror 1 - p < 1/2: the
        // inversion's lower bound of zero holds only on the positive side.
        double tail = p > 0.5 ? 1.0 - p : p;
        var student = new StudentTail(df);
        // The tail at double.MaxValue above p puts the root past every double, where Newton settled on non-roots
        // (#1690's review); that tail is at most x^a / 2, x = df / MaxValue^2, so a larger p skips evaluating it.
        bool mayBePast = df < FarRootMaximumDf
            && Math.Log(tail) < (df / 2.0 * (Math.Log(df) - LogSquaredMaxValue)) - Ln2 + 1e-9;
        double t = mayBePast && student.Survival(double.MaxValue) > tail
            ? double.PositiveInfinity
            : TailInversion.InvertUpperTail(student, tail, student.Seed(tail));
        return p > 0.5 ? -t : t;
    }

    /// <summary>Student's density constant, <c>log(1 / (√df B(df/2, ½)))</c>, for <c>df/2</c> below SmallShape.</summary>
    /// <remarks>As <c>½ log df − log 2 − log(a B)</c>: <c>df/2</c> of a subnormal df rounds to zero, and <c>log B</c> of
    /// it threw (#1690's review). Densities.Student keeps its own form above, two log-gammas against LogBeta's three.</remarks>
    internal static double SmallStudentLogNormalizer(double df) =>
        SmallStudentLogNormalizer(0.5 * Math.Log(df), StudentLogScaledBeta(df / 2.0));

    /// <summary><see cref="SmallStudentLogNormalizer(double)"/> from the inversion's cached logarithms.</summary>
    private static double SmallStudentLogNormalizer(double halfLogDf, double logScaledBeta) =>
        halfLogDf - Ln2 - logScaledBeta;

    /// <summary><c>log(1 + t²/df)</c>, which Student's density and its inversion share.</summary>
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
    private readonly struct StudentTail : IUpperTail
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
            // Below SmallShape StudentLogScaledBeta's form, StudentSf's too; log B is read there only on the fraction's
            // reflected side, which takes it itself. Both cost a quantile below df 0.2 1.4 times 724c7912's (#1690's review).
            bool small = df / 2.0 < SmallShape;
            _logScaledBeta = small ? StudentLogScaledBeta(df / 2.0) : double.NaN;
            _logBeta = small ? double.NaN : LogBeta(df / 2.0, 0.5);
            double halfLogDf = 0.5 * Math.Log(df);
            _logNormalizer = double.IsNaN(_logScaledBeta)
                ? -_logBeta - halfLogDf
                : SmallStudentLogNormalizer(halfLogDf, _logScaledBeta);
        }

        public double Survival(double x) => StudentSf(x, _df, _logBeta, _logScaledBeta);

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
        double c = 1.0;
        double d = 0.0;
        double h = 1.0;

        for (int i = 1; i <= MaxIterations; i++)
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
}
