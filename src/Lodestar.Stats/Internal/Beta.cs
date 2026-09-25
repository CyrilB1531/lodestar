namespace Lodestar.Stats.Internal;

/// <summary>The regularized incomplete beta, and the Student and Fisher tails it carries.</summary>
/// <remarks>
/// One continued fraction serves three families: a t-test's p-value is a
/// Student tail, an ANOVA's is a Fisher tail, and both are the incomplete beta
/// under a change of variable. Written from the published description and
/// evaluated by modified Lentz (1976), except with one shape large and the other
/// small, where DiDonato and Morris's asymptotic expansion takes over (#841); no
/// reference implementation is transcribed (ADR 0002).
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
    internal static double RegularizedIncomplete(double a, double b, double x, double y)
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

        double front = Front(a, b, x, y);

        // The fraction converges quickly only on the side of the distribution's
        // mode; past it the reflection is the fast branch, not a fallback. The
        // swapped (b, a) below is the reflection identity I_x(a,b) = 1 - I_{1-x}(b,a),
        // not a copy-paste slip.
#pragma warning disable S2234
        return x < (a + 1.0) / (a + b + 2.0)
            ? front / (a * ContinuedFraction(a, b, x))
            : 1.0 - (front / (b * ContinuedFraction(b, a, y)));
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

        bool upper = w >= b;
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
    /// Past both shapes of 10, relative to the mean x0 = a / (a + b): a log(x / x0) + b log(y / y0)
    /// has linear terms a t - b t' that cancel exactly, so only log(1 + t) - t is formed, and
    /// Stirling leaves sqrt(x0 b / 2 pi) Gamma*(a + b) / (Gamma*(a) Gamma*(b)). The Lanczos form
    /// subtracts log-gammas of 1.7e9 at a = b = 1e8. d = x - x0 is taken from the smaller of x and
    /// 1 - x, so the two t share one d instead of two rounded ones.
    /// </remarks>
    internal static double Front(double a, double b, double x, double y)
    {
        if (a < StirlingMinimumShape || b < StirlingMinimumShape)
        {
            // Each logarithm on the side where its argument is small, so a y of 1e-16 is not lost to
            // the x = 1 - y it rounds (#1158's review measured 6e-7 at shapes (5e9, 0.5)).
            double logX = x <= 0.5 ? Math.Log(x) : Gamma.LogOnePlusMinus(-y, x) - y;
            double logY = y <= 0.5 ? Math.Log(y) : Gamma.LogOnePlusMinus(-x, y) - x;
            return Math.Exp((a * logX) + (b * logY) - LogBeta(a, b));
        }

        double c = a + b;
        double x0 = a / c;
        double y0 = b / c;
        double d = x <= 0.5 ? x - x0 : y0 - y;
        double exponent = (a * Gamma.LogOnePlusMinus(d / x0, x / x0))
            + (b * Gamma.LogOnePlusMinus(-d / y0, y / y0))
            + Gamma.LogStirlingCorrection(c) - Gamma.LogStirlingCorrection(a) - Gamma.LogStirlingCorrection(b);
        return Math.Exp(exponent) * Math.Sqrt(x0 * b / (2.0 * Math.PI));
    }

    /// <summary>The upper tail of Student's t distribution: P(T &gt; t).</summary>
    internal static double StudentSf(double t, double df)
    {
        if (double.IsNaN(t))
        {
            return double.NaN;
        }

        // I_{df/(df+t^2)}(df/2, 1/2) is twice the tail beyond |t|, so half of it
        // is the tail on one side and the sign says which side we are on.
        double farTail = FarStudentTail(t, df);
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
        double tail = 0.5 * RegularizedIncomplete(df / 2.0, 0.5, x, complement);
        return t >= 0.0 ? tail : 1.0 - tail;
    }

    /// <summary>Half of <c>I_x(df/2, 1/2)</c> by its leading term, where <c>x = df/(df+t²)</c> is below 1e-100; NaN otherwise.</summary>
    /// <remarks>
    /// Past <c>|t| = 1.3e154</c> the square overflows and the tail read zero, which capped every
    /// quantile there (#1158: the Cauchy's 1e-300 quantile is 3.2e299). Formed in logs from
    /// <c>log x = log df − 2 log|t| − log1p(df/t²)</c>, the leading term <c>x^a / (a B(a, ½))</c>
    /// is exact to the next one's relative size, about <c>x</c>, far below a double's precision.
    /// </remarks>
    private static double FarStudentTail(double t, double df)
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
        double logBeta = Gamma.LogGamma(a) + Gamma.LogGamma(0.5) - Gamma.LogGamma(a + 0.5);
        return 0.5 * Math.Exp((a * logX) - Math.Log(a) - logBeta);
    }

    /// <summary>The upper tail of the F distribution: P(F &gt; f).</summary>
    internal static double FisherSf(double f, double dfn, double dfd)
    {
        if (double.IsNaN(f))
        {
            return double.NaN;
        }
        if (f <= 0.0)
        {
            return 1.0;
        }

        // Both halves by division, as in StudentSf: at f = inf the x = 0 return is taken before y is read.
        (double lower, double upper) = FisherSplit(f, dfn, dfd);
        return RegularizedIncomplete(dfd / 2.0, dfn / 2.0, upper, lower);
    }

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
        double ratio = small / large;
        double increment = ((large - 0.5) * (Gamma.LogOnePlusMinus(ratio, 1.0 + ratio) + ratio))
            + (small * Math.Log(large + small)) - small
            + Gamma.LogStirlingCorrection(large + small) - Gamma.LogStirlingCorrection(large);
        return Gamma.LogGamma(small) - increment;
    }

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

    /// <summary>The t with <c>P(T &gt; t) = p</c>: the inverse of <see cref="StudentSf"/>.</summary>
    /// <remarks>
    /// The root of <see cref="StudentSf"/> itself, reached by <see cref="TailInversion"/>
    /// rather than read off a rational approximation of its own: the seed only decides how
    /// many tail evaluations Newton needs, so there is no second approximation to keep in
    /// agreement with the tail. Bisection took about sixty; this takes one to three.
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

        // Symmetric about zero, so p > 1/2 reduces to its mirror 1 - p < 1/2: the
        // inversion's lower bound of zero holds only on the positive side.
        double tail = p > 0.5 ? 1.0 - p : p;
        var student = new StudentTail(df);
        double t = TailInversion.InvertUpperTail(student, tail, student.Seed(tail));
        return p > 0.5 ? -t : t;
    }

    /// <summary>Student's upper tail and log density, and a seed for inverting it.</summary>
    private readonly struct StudentTail : IUpperTail
    {
        private readonly double _df;

        // log of Gamma((df+1)/2) / (Gamma(df/2) sqrt(df pi)), the density's constant.
        private readonly double _logNormalizer;

        internal StudentTail(double df)
        {
            _df = df;
            _logNormalizer = Gamma.LogGamma((df + 1.0) / 2.0) - Gamma.LogGamma(df / 2.0)
                - (0.5 * Math.Log(df * Math.PI));
        }

        public double Survival(double x) => StudentSf(x, _df);

        public double LogDensity(double x)
        {
            // log(1 + r^2) three ways: a series where 1 + r^2 rounds away r^2, the log of r
            // where squaring would overflow, and the plain form between the two.
            double r = x / Math.Sqrt(_df);
            double r2 = r * r;
            double log1pR2;
            if (r2 < 1e-4)
            {
                log1pR2 = r2 * (1.0 - (r2 * (0.5 - (r2 / 3.0))));
            }
            else if (r > 1e8)
            {
                log1pR2 = 2.0 * Math.Log(r);
            }
            else
            {
                log1pR2 = Math.Log(1.0 + r2);
            }

            return _logNormalizer - (0.5 * (_df + 1.0) * log1pR2);
        }

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

    // CF = 1 + d1/(1 + d2/(1 + ...)) from Abramowitz & Stegun 26.5.8, evaluated
    // directly by modified Lentz (b0 = 1, every later b_i = 1) rather than a reciprocal restatement.
    private static double ContinuedFraction(double a, double b, double x)
    {
        double c = 1.0;
        double d = 0.0;
        double h = 1.0;

        for (int i = 1; i <= MaxIterations; i++)
        {
            int m = i / 2;

            // The numerators alternate between the two forms with the term's
            // parity; folding both into one loop keeps the recurrence's state in one place.
            double numerator = i % 2 == 0
                ? m * (b - m) * x / ((a + (2 * m) - 1.0) * (a + (2 * m)))
                : -(a + m) * (a + b + m) * x / ((a + (2 * m)) * (a + (2 * m) + 1.0));

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
