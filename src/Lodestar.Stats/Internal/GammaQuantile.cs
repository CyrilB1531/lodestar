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

        double low = 0.0;
        double high = double.PositiveInfinity;

        for (int i = 0; i < MaxIterations; i++)
        {
            double value = upper ? Gamma.RegularizedQ(a, x) : Gamma.RegularizedP(a, x);

            // P rises with x and Q falls, so the side of the root a value puts x on depends on
            // which tail is solved.
            if ((value < target) != upper)
            {
                low = x;
            }
            else
            {
                high = x;
            }

            double logStep = NewtonLogStep(x, value, problem, out double logResidual);
            if (TailInversion.Finish(x, logStep, logResidual, low, high, problem.Rule, out double next))
            {
                return next;
            }

            x = next;
        }

        return x;
    }

    /// <summary>The Newton step on <c>log T</c> against <c>log x</c>, with the log residual it divides.</summary>
    /// <remarks>
    /// <c>d log P / d log x = x f(x) / P</c> with <c>f</c> the gamma density, and the negative of
    /// the same over <c>Q</c> for the upper tail; <c>log(x f(x)) = a log x − x − log Γ(a)</c>.
    /// </remarks>
    private static double NewtonLogStep(double x, double value, in Problem problem, out double logResidual)
    {
        double logXDensity = problem.A >= StirlingSlopeShape
            ? Gamma.LogStirlingPrefactor(problem.A, x)
            : (problem.A * Math.Log(x)) - x - problem.LogGammaA;
        double logValue = Math.Log(value);
        double inverseSlope = 1.0 / (Math.Exp(logXDensity - logValue) * (problem.Upper ? -1.0 : 1.0));
        logResidual = TailInversion.LogResidual(problem.Target, problem.LogTarget, value, logValue, inverseSlope);
        return TailInversion.NewtonStep(logResidual, inverseSlope);
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

        internal bool Upper { get; }

        internal double LogGammaA { get; }

        /// <summary>The step in <c>log x</c> below which the root is settled, and the midpoint's anchor.</summary>
        internal StepRule Rule { get; }
    }
}
