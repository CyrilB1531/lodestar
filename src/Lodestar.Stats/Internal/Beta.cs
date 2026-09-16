namespace Lodestar.Stats.Internal;

/// <summary>The regularized incomplete beta, and the Student and Fisher tails it carries.</summary>
/// <remarks>
/// One continued fraction serves three families: a t-test's p-value is a
/// Student tail, an ANOVA's is a Fisher tail, and both are the incomplete beta
/// under a change of variable. Written from the published description and
/// evaluated by modified Lentz (1976); no reference implementation is
/// transcribed (ADR 0003).
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

    internal static double RegularizedIncomplete(double a, double b, double x)
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
        // to [0, 1] above, and 0/1 are the interval's own closed endpoints.
#pragma warning disable S1244
        if (x == 0.0 || x == 1.0)
#pragma warning restore S1244
        {
            return x;
        }

        double front = Front(a, b, x);

        // The fraction converges quickly only on the side of the distribution's
        // mode; past it the reflection is the fast branch, not a fallback. The
        // swapped (b, a) below is the reflection identity I_x(a,b) = 1 - I_{1-x}(b,a),
        // not a copy-paste slip.
#pragma warning disable S2234
        return x < (a + 1.0) / (a + b + 2.0)
            ? front / (a * ContinuedFraction(a, b, x))
            : 1.0 - (front / (b * ContinuedFraction(b, a, 1.0 - x)));
#pragma warning restore S2234
    }

    /// <summary>x^a (1 - x)^b / B(a, b), the factor both branches of the fraction share.</summary>
    /// <remarks>
    /// Past both shapes of 10, relative to the mean x0 = a / (a + b): a log(x / x0) + b log(y / y0)
    /// has linear terms a t - b t' that cancel exactly, so only log(1 + t) - t is formed, and
    /// Stirling leaves sqrt(x0 b / 2 pi) Gamma*(a + b) / (Gamma*(a) Gamma*(b)). The Lanczos form
    /// subtracts log-gammas of 1.7e9 at a = b = 1e8. d = x - x0 is taken from the smaller of x and
    /// 1 - x, so the two t share one d instead of two rounded ones.
    /// </remarks>
    private static double Front(double a, double b, double x)
    {
        double y = 1.0 - x;
        if (a < StirlingMinimumShape || b < StirlingMinimumShape)
        {
            return Math.Exp(
                Gamma.LogGamma(a + b) - Gamma.LogGamma(a) - Gamma.LogGamma(b) +
                (a * Math.Log(x)) + (b * Math.Log(y)));
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
        double tSquared = t * t;
        double denominator = df + tSquared;
        double x = df / denominator;

        // long-comment: this is the fix for a measured far-tail collapse, not
        //     routine commentary -- reflecting unconditionally returned exactly
        //     0.0 at ordinary df once t was large enough (df = 200, t = 10 is
        //     BetaTests's non-regression case). A residual remains above
        //     roughly df = 1.5e9 -- about 1.5 billion observations through
        //     TTest's n1 + n2 - 2 -- where RegularizedIncomplete itself
        //     saturates to exactly 1.0, which no branch choice here repairs.
        // Direct evaluation of I_x(df/2, 1/2) is what the far-tail case needs:
        // reflecting through I_x(a,b) = 1 - I_{1-x}(b,a) trades one cancellation
        // for another one downstream -- 1.0 minus a value that is itself within
        // a few ULPs of 1 collapses to exactly 0.0 once the true tail is below
        // roughly 1e-16, which is reachable at ordinary df once t is large
        // enough (df = 200, t = 10 already gets there). Reflection earns its
        // keep only in the opposite regime, where x itself has already lost the
        // precision direct evaluation needs: IsDirectlyAccurate below detects
        // that by comparing the naive 1.0 - x against the complement computed
        // by division, rather than by guessing a df or magnitude cutoff.
        double complement = tSquared / denominator;
        double tail = 0.5 * (IsDirectlyAccurate(x, complement)
            ? RegularizedIncomplete(df / 2.0, 0.5, x)
            : 1.0 - RegularizedIncomplete(0.5, df / 2.0, complement));
        return t >= 0.0 ? tail : 1.0 - tail;
    }

    // x <= 0.5 (t^2 >= df) is always safe. Past that, direct evaluation is
    // accurate exactly when 1 - x still recovers the true (divided) complement.
    private static bool IsDirectlyAccurate(double x, double complement)
    {
        if (x <= 0.5)
        {
            return true;
        }

        double naiveComplement = 1.0 - x;
        return Math.Abs(naiveComplement - complement) <= complement * 1e-9;
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

        return RegularizedIncomplete(dfd / 2.0, dfn / 2.0, dfd / (dfd + (dfn * f)));
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
