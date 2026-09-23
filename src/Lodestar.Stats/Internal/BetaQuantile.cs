namespace Lodestar.Stats.Internal;

/// <summary>Solves <c>I_x(a, b) = p</c> for <c>x</c>: the inverse regularized incomplete beta.</summary>
/// <remarks>
/// The Clopper-Pearson bounds a binomial proportion's exact interval needs, since a binomial tail
/// is a regularized incomplete beta. <see cref="TailInversion"/> does not serve: it inverts a tail
/// symmetric about zero, where this one lives on <c>(0, 1)</c> with a bracket from the first step.
/// Newton on <c>log I</c> against <c>log x</c>, safeguarded by that bracket. scipy reaches the
/// same numbers from the other side — <c>binomtest</c> root-finds on the binomial tail rather
/// than calling <c>betaincinv</c> — so the corpus compares at 1e-9.
/// </remarks>
internal static class BetaQuantile
{
    // Bisection alone would need about sixty steps; Newton reaches the root in well under ten
    // from the seeds below, and this only bounds a pathological shape rather than the stopping rule.
    private const int MaxIterations = 200;

    // Newton's next error is about h^2 for a step of h, so 1e-9 relative leaves far more than a
    // double carries -- the loop stops on the bracket rather than on this in the hard cases.
    private const double StepTolerance = 1e-13;

    // log(double.Epsilon), the smallest positive double: a seed below this is a root no double
    // can name, and the answer is zero rather than wherever the iteration happened to stop.
    private const double LogSmallestDenormal = -744.44;

    /// <summary>The <c>x</c> in <c>(0, 1)</c> with <c>I_x(a, b) = p</c>.</summary>
    /// <param name="p">The probability, in <c>[0, 1]</c>.</param>
    /// <param name="a">The first shape; positive.</param>
    /// <param name="b">The second shape; positive.</param>
    /// <exception cref="ArgumentOutOfRangeException">A shape is not positive, or <paramref name="p"/> is outside <c>[0, 1]</c>.</exception>
    internal static double Invert(double p, double a, double b)
    {
        if (double.IsNaN(a) || a <= 0.0)
        {
            throw new ArgumentOutOfRangeException(nameof(a), a, "The first shape must be positive.");
        }
        if (double.IsNaN(b) || b <= 0.0)
        {
            throw new ArgumentOutOfRangeException(nameof(b), b, "The second shape must be positive.");
        }
        if (double.IsNaN(p) || p < 0.0 || p > 1.0)
        {
            throw new ArgumentOutOfRangeException(nameof(p), p, "The probability must lie in [0, 1].");
        }

        // S1244: the interval's own closed endpoints, which the validation above admits; only
        // those two literals short-circuit, and every other value is solved for.
#pragma warning disable S1244
        if (p == 0.0)
        {
            return 0.0;
        }
        if (p == 1.0)
        {
            return 1.0;
        }
#pragma warning restore S1244

        // long-comment: which half is solved decides the accuracy of the whole routine, and
        // the reason the test is this short rather than cleverer is a measurement.
        // I_x(a, b) = 1 - I_(1-x)(b, a), so the upper half is solved as its mirror: the
        // probability that is kept is then always the smaller of p and 1 - p, which is the one
        // a double carries to full relative precision. Deciding instead by where the root lies
        // -- one evaluation at the midpoint -- looks better and measures worse: it leaves a
        // target of 1 - 1e-12 unmirrored, and that target is only good to four digits, which
        // cost 2.1e-7 on shapes (1000, 10000). Over a 504-point grid against
        // scipy.special.betaincinv, this rule leaves 8 disagreements and every one of them is
        // a point where scipy's own betainc does not reproduce its own betaincinv (#1121).
        bool mirrored = p > 0.5;
        double solved = Solve(mirrored ? 1.0 - p : p, mirrored ? b : a, mirrored ? a : b);

        return mirrored ? 1.0 - solved : solved;
    }

    private static double Solve(double target, double a, double b)
    {
        double x = Seed(target, a, b);

        // S1244: Seed answers this exact literal when the root underflows, and nothing else
        // reaches it -- the solve below never produces a zero.
#pragma warning disable S1244
        if (x == 0.0)
#pragma warning restore S1244
        {
            return 0.0;
        }

        double low = 0.0;
        double high = 1.0;
        var problem = new Problem(a, b, target);

        for (int i = 0; i < MaxIterations; i++)
        {
            double value = Beta.RegularizedIncomplete(a, b, x);
            if (value < target)
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

    /// <summary>One Newton step, taken on <c>log I</c> against <c>log x</c>.</summary>
    /// <remarks>
    /// In <c>x</c> the step is useless where it is most needed: at shapes (1000, 10000) the tail
    /// crosses a hundred and forty orders of magnitude between <c>x = 0.022</c> and
    /// <c>x = 0.034</c>, so a residual divided by the density there overflows and every step is
    /// discarded for the bracket, which then has a thousand halvings of exponent to walk. In logs
    /// the same curve is close to a straight line, which is why <see cref="TailInversion"/> runs
    /// in logs too. Measured (#1121): 46 of 504 grid points past 1e-9 in <c>x</c>, 0 in logs.
    /// </remarks>
    private static double Advance(
        double x, double value, in Problem problem, double low, double high, out double logStep)
    {
        double a = problem.A;
        double b = problem.B;
        // log(1 - x) through Gamma's series rather than Math.Log(1 - x): the root is often small
        // here, and 1 - x rounds its own leading digits away.
        double logOneMinusX = Gamma.LogOnePlusMinus(-x, 1.0 - x) - x;
        double logDensity = ((a - 1.0) * Math.Log(x)) + ((b - 1.0) * logOneMinusX) - problem.LogBeta;

        // d log I / d log x = x f(x) / I(x), so the step is the log residual over that slope.
        logStep = (Math.Log(problem.Target) - Math.Log(value))
            * Math.Exp(Math.Log(value) - Math.Log(x) - logDensity);
        double next = x * Math.Exp(logStep);

        if (next > low && next < high && !double.IsNaN(next))
        {
            return next;
        }

        logStep = double.NaN;
        return Fallback(low, high);
    }

    /// <summary>Says whether the bracket has nothing left to give, and what to answer if so.</summary>
    private static bool TryFinish(
        double x, double next, double logStep, double low, double high, out double settled)
    {
        settled = x;

        // S1244: the bracket's own fixed point -- the fallback stops moving once it lands on a
        // bound, and no further bit is available from either side.
#pragma warning disable S1244
        if (next == low || next == high)
        {
            return true;
        }
#pragma warning restore S1244

        // A step in log x is a relative move, so the tolerance is one without dividing.
        if (Math.Abs(logStep) <= StepTolerance)
        {
            settled = next;
            return true;
        }

        return false;
    }

    /// <summary>A first guess: the small-x leading term where it applies, the mean otherwise.</summary>
    /// <remarks>
    /// As <c>x</c> approaches zero, <c>I_x(a, b)</c> approaches <c>x^a / (a·B(a, b))</c>, which
    /// inverts in closed form and puts the seed within an order of magnitude of a root that plain
    /// bisection from <c>(0, 1)</c> would need a thousand steps to reach at <c>p = 1e-300</c>.
    /// Past the mean the expansion no longer leads, and the mean is the better seed.
    /// </remarks>
    private static double Seed(double target, double a, double b)
    {
        double mean = a / (a + b);
        double logBeta = Gamma.LogGamma(a) + Gamma.LogGamma(b) - Gamma.LogGamma(a + b);
        double logSmall = (Math.Log(target) + Math.Log(a) + logBeta) / a;

        // Below the smallest denormal the root is not a double at all, and zero says so: at
        // p = 1e-300 with a = 0.5 the true root is near 1e-600 (#1121).
        if (logSmall < LogSmallestDenormal)
        {
            return 0.0;
        }

        double small = Math.Exp(logSmall);

        return double.IsNaN(small) || small <= 0.0 || small >= mean ? mean : small;
    }

    // Geometric while the bounds are orders apart: the root may be at 1e-300, where an
    // arithmetic midpoint moves the mantissa and only the exponent matters.
    private static double Fallback(double low, double high)
    {
        // S1244: zero is the initial lower bound exactly, before any evaluation has replaced it.
#pragma warning disable S1244
        if (low == 0.0)
#pragma warning restore S1244
        {
            return high * 0.125;
        }

        return high > 4.0 * low ? Math.Sqrt(low) * Math.Sqrt(high) : 0.5 * (low + high);
    }

    /// <summary>The shapes, their log beta and the probability, carried together.</summary>
    /// <remarks>One value rather than four parameters: the step needs all of them and S107 caps a signature at seven.</remarks>
    private readonly struct Problem
    {
        internal Problem(double a, double b, double target)
        {
            A = a;
            B = b;
            Target = target;
            LogBeta = Gamma.LogGamma(a) + Gamma.LogGamma(b) - Gamma.LogGamma(a + b);
        }

        internal double A { get; }

        internal double B { get; }

        internal double Target { get; }

        internal double LogBeta { get; }
    }
}
