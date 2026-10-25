namespace Lodestar.Stats.Internal;

/// <summary>Solves <c>P(a, x) = p</c> or <c>Q(a, x) = p</c> for <c>x</c>: the inverse regularized incomplete gamma.</summary>
/// <remarks>
/// The chi-squared quantiles need it (#1158), and nothing here inverted the gamma before. The
/// same shape as <see cref="BetaQuantile"/>: Newton on the log of the tail against <c>log x</c>,
/// safeguarded by a bracket, solving whichever of <c>p</c> and <c>1 − p</c> is smaller so the
/// target a double carries is always the one it carries to full relative precision. The bracket
/// starts as <c>(0, ∞)</c> and grows until an evaluation closes it from above.
/// </remarks>
internal static class GammaQuantile
{
    // Newton reaches the root in a handful of steps from the seeds below; this bounds a
    // pathological shape rather than the stopping rule.
    private const int MaxIterations = 200;

    // A step in log x is a relative move; 1e-13 leaves far more than the 1e-9 the corpus asks.
    private const double StepTolerance = 1e-13;

    // The midpoint's anchor while the lower bound is zero, as a fraction of the upper: StepRule has why.
    private const double RelativeFloor = 1e-8;

    // Past a = 1e10 the step is held to this fraction of the law's relative width, 1/sqrt(a): with the Stirling slope and a
    // fixed 1e-13, ChiSquaredQuantile(1e-3, 1e30) read 5.9 standard deviations below the mean for 3.1 (#1690's review).
    private const double WidthTolerance = 1e-8;
    private const double WidthShape = 1e10;

    // From here the slope's log-gamma is Stirling's: a log x - x - log Gamma(a) cancels, a nat at a = 1e15 (#1690's
    // review); below, that rounding is under 1e-9 of a nat and the plain form saves the series.
    private const double StirlingSlopeShape = 1e5;

    // log(double.Epsilon): a root below it is no double, and zero says so.
    private const double LogSmallestDenormal = -744.44;

    // 2^-1030: below it a subnormal tail keeps under 44 bits, fewer than its log's ulp of some 708 nats leaves; above, the
    // log read ChiSquaredQuantile(2.225073858507201e-308, 2) 250 ulps off where the tail held it to one.
    private const double LogsBelow = 8.691694759794e-311;

    // log(double.Epsilon / 2): a tail below it rounds to zero, the bracket's end the inversion bisects toward.
    private const double LogUnderflow = -745.13;

    /// <summary>The <c>x</c> with <c>P(a, x) = p</c>, or <c>Q(a, x) = p</c> when <paramref name="upper"/>.</summary>
    /// <param name="p">The probability, in <c>[0, 1]</c>.</param>
    /// <param name="a">The shape; positive.</param>
    /// <param name="upper">Solve the upper tail <c>Q</c> rather than the lower <c>P</c>.</param>
    /// <exception cref="ArgumentOutOfRangeException">The shape is not positive, or <paramref name="p"/> is outside <c>[0, 1]</c>.</exception>
    internal static double Invert(double p, double a, bool upper)
    {
        if (double.IsNaN(a) || a <= 0.0)
        {
            throw new ArgumentOutOfRangeException(nameof(a), a, "The shape must be positive.");
        }
        if (double.IsNaN(p) || p < 0.0 || p > 1.0)
        {
            throw new ArgumentOutOfRangeException(nameof(p), p, "The probability must lie in [0, 1].");
        }

        // S1244: the closed endpoints the validation admits, answered as the support's ends.
#pragma warning disable S1244
        if (p == 0.0)
        {
            return upper ? double.PositiveInfinity : 0.0;
        }
        if (p == 1.0)
        {
            return upper ? 0.0 : double.PositiveInfinity;
        }
#pragma warning restore S1244

        // P(a, x) = 1 - Q(a, x): keep the smaller of the two probabilities, which is the one a
        // double holds to full relative precision, and solve the tail it belongs to.
        bool flip = p > 0.5;
        return Solve(flip ? 1.0 - p : p, a, flip ? !upper : upper);
    }

    private static double Solve(double target, double a, bool upper)
    {
        // The upper tail at the smallest double already under the target puts the root below every double, as Seed
        // finds for the lower tail (#1690's review). Past Gamma.SmallShape that tail is within e^-74 of one.
        if (upper && a < Gamma.SmallShape && Gamma.RegularizedQ(a, double.Epsilon) < target)
        {
            return 0.0;
        }

        var problem = new Problem(a, target, upper);
        double x = Seed(problem);

        // S1244: Seed answers this exact literal when the root underflows, and nothing else does.
#pragma warning disable S1244
        if (x == 0.0)
#pragma warning restore S1244
        {
            return 0.0;
        }

        // A target past LogsBelow is judged on the tail's log: the tail itself kept a few bits, and ChiSquaredIsf(5e-324, 1)
        // read 1481.4857 for 1481.1267 (#1729's Review A).
        return problem.InLogs ? Iterate<LogTailStep>(x, problem) : Iterate<TailStep>(x, problem);
    }

    /// <summary>Newton within the bracket, each step and side from <typeparamref name="TStep"/>: one loop for the tail and
    /// its log, a struct so the step is inlined, where a branch on the two cost ChiSquaredIsf(1e-10, 100) 2.7%.</summary>
    private static double Iterate<TStep>(double x, in Problem problem)
        where TStep : struct, IStep
    {
        TStep step = default;
        double low = 0.0;
        double high = double.PositiveInfinity;

        for (int i = 0; i < MaxIterations; i++)
        {
            double logStep = step.Take(x, problem, out bool below, out double logResidual);

            // P rises with x and Q falls, so the side of the root a value puts x on depends on
            // which tail is solved.
            if (below != problem.Upper)
            {
                low = x;
            }
            else
            {
                high = x;
            }

            if (TailInversion.Finish(x, logStep, logResidual, low, high, problem.Rule, out double next))
            {
                return next;
            }

            x = next;
        }

        return x;
    }

    /// <summary>A Newton step on <c>log T</c> against <c>log x</c>, with the log residual it divides and whether the tail
    /// lies below the target.</summary>
    private interface IStep
    {
        double Take(double x, in Problem problem, out bool below, out double logResidual);
    }

    /// <summary>The step on the tail itself.</summary>
    /// <remarks>
    /// <c>d log P / d log x = x f(x) / P</c> with <c>f</c> the gamma density, and the negative of
    /// the same over <c>Q</c> for the upper tail; <c>log(x f(x)) = a log x − x − log Γ(a)</c>.
    /// </remarks>
    private readonly struct TailStep : IStep
    {
        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public double Take(double x, in Problem problem, out bool below, out double logResidual)
        {
            double value = problem.Upper ? Gamma.RegularizedQ(problem.A, x) : Gamma.RegularizedP(problem.A, x);
            below = value < problem.Target;
            double logValue = Math.Log(value);
            double inverseSlope = InverseSlope(x, logValue, problem);
            logResidual = TailInversion.LogResidual(problem.Target, problem.LogTarget, value, logValue, inverseSlope);
            return TailInversion.NewtonStep(logResidual, inverseSlope);
        }

        /// <summary><c>d log x / d log T</c>, from <c>log(x f(x)) = a log x − x − log Γ(a)</c> and the tail's log.</summary>
        private static double InverseSlope(double x, double logValue, in Problem problem)
        {
            double logXDensity = problem.A >= StirlingSlopeShape
                ? Gamma.LogStirlingPrefactor(problem.A, x)
                : (problem.A * Math.Log(x)) - x - problem.LogGammaA;
            return 1.0 / (Math.Exp(logXDensity - logValue) * (problem.Upper ? -1.0 : 1.0));
        }
    }

    /// <summary>The step on the tail's log and the slope its parts give, for a subnormal target.</summary>
    private readonly struct LogTailStep : IStep
    {
        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public double Take(double x, in Problem problem, out bool below, out double logResidual)
        {
            (double logValue, double ratio) = Gamma.LogTail(problem.A, x, problem.Upper);
            below = logValue < problem.LogScaledTarget;
            logResidual = problem.LogScaledTarget - logValue;
            // A tail a double holds as zero bisects, as on the tail itself: Newton in log x from x = 2e285 gains a nat a
            // step, and ChiSquaredIsf(1e-310, 2e-5) ran out at 6.9e17 for 1391.48 (#1729's Review A).
            double slope = problem.Upper ? -ratio : ratio;
            return logValue + problem.LogScale < LogUnderflow ? double.NaN : TailInversion.NewtonStep(logResidual, 1.0 / slope);
        }
    }

    /// <summary>A first guess: the small-x leading term in the lower tail, Wilson-Hilferty otherwise.</summary>
    /// <remarks>
    /// As <c>x</c> approaches zero, <c>P(a, x)</c> approaches <c>x^a / Γ(a + 1)</c>, which inverts in
    /// closed form and reaches a root near <c>1e-300</c> that growth from the mean would need a
    /// thousand halvings to find. Elsewhere the Wilson-Hilferty cube, exact enough in the body that
    /// Newton finishes in a few steps.
    /// </remarks>
    private static double Seed(in Problem problem)
    {
        double a = problem.A;
        // The leading term drops e^-x, so from StirlingSlopeShape it seeds near a / e, where the tail underflows, and growth
        // squared past the root: ChiSquaredQuantile(0.3, 1e100) took 30 evaluations (#1690's review). Wilson-Hilferty holds there.
        if (!problem.Upper && a < StirlingSlopeShape)
        {
            double logSmall = (problem.LogTarget + Gamma.LogGamma(a + 1.0)) / a;
            if (logSmall < LogSmallestDenormal)
            {
                return 0.0;
            }
            double small = Math.Exp(logSmall);
            if (small < a)
            {
                return small;
            }
        }

        // The standard-normal quantile of the lower-tail probability: scipy's ndtri, negated from an upper tail.
        double z = problem.Upper ? -Ndtri.Of(problem.Target) : Ndtri.Of(problem.Target);
        double c = 1.0 / (9.0 * a);
        double cube = 1.0 - c + (z * Math.Sqrt(c));
        double wilsonHilferty = a * cube * cube * cube;

        return wilsonHilferty > 0.0 && !double.IsInfinity(wilsonHilferty) ? wilsonHilferty : a;
    }

    /// <summary>The shape, the tail solved and the target, carried together through the step.</summary>
    private readonly struct Problem
    {
        internal Problem(double a, double target, bool upper)
        {
            A = a;
            Target = target;
            LogTarget = Math.Log(target);
            // Below Gamma.SmallShape the tail and the target over a, as Gamma.LogTail takes the tail; read only in logs.
            InLogs = MeetsInLogs(target, a);
            bool scaled = a < Gamma.SmallShape && InLogs;
            LogScale = scaled ? Math.Log(a) : 0.0;
            LogScaledTarget = scaled ? LogOver(target, a) : LogTarget;
            Upper = upper;
            // Read only below StirlingSlopeShape, by the slope; past it the slope is Stirling's.
            LogGammaA = a < StirlingSlopeShape ? Gamma.LogGamma(a) : double.NaN;
            // WidthTolerance / sqrt(a) passes StepTolerance at a = 1e10, and below it the root is not taken.
            // The midpoint anchors at the smallest double while the lower bound is zero.
            Rule = new StepRule(a > WidthShape ? WidthTolerance / Math.Sqrt(a) : StepTolerance, double.Epsilon, double.PositiveInfinity, RelativeFloor);
        }

        internal double A { get; }

        internal double Target { get; }

        internal double LogTarget { get; }

        /// <summary>Whether the target is met on the tail's log, the one decision the step and the scaled target read.</summary>
        internal bool InLogs { get; }

        internal double LogScale { get; }

        internal double LogScaledTarget { get; }

        internal bool Upper { get; }

        internal double LogGammaA { get; }

        /// <summary>The step in <c>log x</c> below which the root is settled, and the midpoint's anchor.</summary>
        internal StepRule Rule { get; }

        /// <summary>Whether a target is met on the tail's log: past LogsBelow, or anywhere subnormal below Gamma.SmallShape,
        /// where the log over a is no longer some 708 nats and the subnormal tail's slope in log x is small: ChiSquaredIsf(1e-310,
        /// 2^-1021) sat on a plateau 43 ulps from its root.</summary>
        private static bool MeetsInLogs(double target, double a) =>
            target < LogsBelow || (target < NearZero.SmallestNormal && a < Gamma.SmallShape);

        /// <summary><c>log(target / a)</c>: the quotient's log where it is normal, past its 570 nats of log a, else the two logs
        /// apart, a then within 36 nats of one; a subnormal quotient kept a few bits, ChiSquaredIsf(1e-320, 4.4e-7) 7e-14 off.</summary>
        private static double LogOver(double target, double a)
        {
            double quotient = target / a;
            return quotient >= NearZero.SmallestNormal ? Math.Log(quotient) : Math.Log(target) - Math.Log(a);
        }
    }
}
