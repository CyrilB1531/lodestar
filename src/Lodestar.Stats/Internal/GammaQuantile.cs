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

            double next = Advance(x, value, problem, low, high, out double logStep);
            if (TryFinish(x, next, logStep, low, high, out double settled))
            {
                return settled;
            }

            x = next;
        }

        return x;
    }

    /// <summary>One Newton step on <c>log T</c> against <c>log x</c>, or the bracket's fallback.</summary>
    /// <remarks>
    /// <c>d log P / d log x = x f(x) / P</c> with <c>f</c> the gamma density, and the negative of
    /// the same over <c>Q</c> for the upper tail; <c>log(x f(x)) = a log x − x − log Γ(a)</c>.
    /// </remarks>
    private static double Advance(
        double x, double value, in Problem problem, double low, double high, out double logStep)
    {
        double logXDensity = (problem.A * Math.Log(x)) - x - problem.LogGammaA;
        double slope = Math.Exp(logXDensity - Math.Log(value)) * (problem.Upper ? -1.0 : 1.0);
        logStep = (problem.LogTarget - Math.Log(value)) / slope;
        double next = x * Math.Exp(logStep);

        if (next > low && next < high && !double.IsNaN(next) && !double.IsInfinity(logStep))
        {
            return next;
        }

        logStep = double.NaN;
        return Fallback(x, low, high);
    }

    /// <summary>Says whether the bracket has nothing left to give, and what to answer if so.</summary>
    private static bool TryFinish(
        double x, double next, double logStep, double low, double high, out double settled)
    {
        settled = x;

        // S1244: the bracket's own fixed point, where the fallback stops moving on a bound.
#pragma warning disable S1244
        if (next == low || next == high)
        {
            return true;
        }
#pragma warning restore S1244

        if (Math.Abs(logStep) <= StepTolerance)
        {
            settled = next;
            return true;
        }

        return false;
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
        if (!problem.Upper)
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

        // The standard-normal quantile of the lower-tail probability, from the upper-tail helper.
        double z = problem.Upper ? Normal.Quantile(problem.Target) : -Normal.Quantile(problem.Target);
        double c = 1.0 / (9.0 * a);
        double cube = 1.0 - c + (z * Math.Sqrt(c));
        double wilsonHilferty = a * cube * cube * cube;

        return wilsonHilferty > 0.0 && !double.IsInfinity(wilsonHilferty) ? wilsonHilferty : a;
    }

    // Growth while no evaluation has bounded the root from above; then geometric while the bounds
    // are orders apart, and arithmetic once they are close.
    private static double Fallback(double x, double low, double high)
    {
        if (double.IsPositiveInfinity(high))
        {
            return Math.Max(x, low) * 4.0;
        }

        // S1244: zero is the initial lower bound exactly, before any evaluation replaced it.
#pragma warning disable S1244
        if (low == 0.0)
#pragma warning restore S1244
        {
            return high * 0.125;
        }

        return high > 4.0 * low ? Math.Sqrt(low) * Math.Sqrt(high) : 0.5 * (low + high);
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
            LogGammaA = Gamma.LogGamma(a);
        }

        internal double A { get; }

        internal double Target { get; }

        internal double LogTarget { get; }

        internal bool Upper { get; }

        internal double LogGammaA { get; }
    }
}
