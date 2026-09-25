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

    // Below this, one minus a number near one leaves fewer than thirteen of its digits (InvertBoth).
    private const double DerivedSideFloor = 1e-3;

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

    /// <summary>The <c>x</c> with <c>I_x(a, b) = lower</c> and its complement <c>1 − x</c>, each to full relative precision.</summary>
    /// <param name="lower">The lower-tail probability, in <c>(0, 1)</c>.</param>
    /// <param name="upper">The upper-tail probability <c>1 − lower</c>, given rather than formed so a small one keeps its digits.</param>
    /// <param name="a">The first shape; positive.</param>
    /// <param name="b">The second shape; positive.</param>
    /// <remarks>
    /// <see cref="Invert"/> returns <c>x</c> alone, and a root near one loses <c>1 − x</c>: at shapes
    /// (5e9, 0.5) the root sits 2e-10 below one, and an F quantile formed from <c>1 − x</c> was 1e-7 off
    /// (#1158's review). This refines <see cref="Invert"/>'s answer by Newton on whichever of <c>x</c>
    /// and <c>1 − x</c> is below one half, against the smaller of the two probabilities, reading the
    /// incomplete beta with both of its arguments so neither is one minus the other.
    /// </remarks>
    internal static (double X, double Complement) InvertBoth(double lower, double upper, double a, double b)
    {
        (double x0, double complement0) = SeedBoth(lower, upper, a, b);

        // One minus a number near one is off by about 1e-16 absolute, so above 1e-3 it holds 1e-13
        // relative and Invert's answer stands; only a smaller one, the case that was 1e-7 off, is refined.
        double derived = lower <= upper ? complement0 : x0;
        if (derived >= DerivedSideFloor)
        {
            return (x0, complement0);
        }

        bool complementSmall = x0 > 0.5;
        var tail = new SmallSideTail(a, b, complementSmall, lower <= upper, Math.Min(lower, upper));
        double s = Refine(tail, complementSmall ? complement0 : x0);
        return complementSmall ? (1.0 - s, s) : (s, 1.0 - s);
    }

    /// <summary><see cref="Invert"/>'s root from the smaller probability's own tail, with its complement.</summary>
    /// <remarks>
    /// Seeded from the smaller probability so the side solved directly is named rather than formed as
    /// one minus a root that rounded to one: at an upper tail of 1e-12 and shapes (0.5, 0.5) the
    /// complement is 1e-24, which 1 - x cannot hold.
    /// </remarks>
    private static (double X, double Complement) SeedBoth(double lower, double upper, double a, double b)
    {
        if (lower <= upper)
        {
            double x = Invert(lower, a, b);
            return (x, 1.0 - x);
        }
        // S2234: I_x(a, b) = 1 - I_(1-x)(b, a), so the upper tail is the swapped shapes' lower tail at
        // 1 - x, and the exchange is the identity rather than a slip.
#pragma warning disable S2234
        double complement = Invert(upper, b, a);
#pragma warning restore S2234
        return (1.0 - complement, complement);
    }

    /// <summary>Newton on <c>log s</c> for the small side, safeguarded by the bracket <c>(0, ½]</c>.</summary>
    private static double Refine(in SmallSideTail tail, double s)
    {
        double low = 0.0;
        double high = 0.5;
        for (int i = 0; i < MaxIterations && s > 0.0; i++)
        {
            double value = tail.Value(s);
            if (value < tail.Target == tail.Increasing)
            {
                low = s;
            }
            else
            {
                high = s;
            }

            double logStep = (Math.Log(tail.Target) - Math.Log(value)) / tail.LogSlope(s, value);
            double next = s * Math.Exp(logStep);
            if (!(next > low && next < high) || double.IsNaN(next))
            {
                next = Fallback(low, high);
                logStep = double.NaN;
            }
            if (TryFinish(s, next, logStep, low, high, out double settled))
            {
                return settled;
            }
            s = next;
        }
        return s;
    }

    /// <summary>A beta tail read as a function of whichever of <c>x</c> and <c>1 − x</c> is small.</summary>
    private readonly struct SmallSideTail
    {
        private readonly double _a;
        private readonly double _b;
        private readonly bool _complementSmall;
        private readonly bool _lowerTail;

        internal SmallSideTail(double a, double b, bool complementSmall, bool lowerTail, double target)
        {
            _a = a;
            _b = b;
            _complementSmall = complementSmall;
            _lowerTail = lowerTail;
            Target = target;
            // I_x(a, b) rises with x; read against 1 - x it falls, and the upper tail is its mirror.
            Increasing = complementSmall != lowerTail;
        }

        internal double Target { get; }

        internal bool Increasing { get; }

        internal double Value(double s)
        {
            double point = _complementSmall ? 1.0 - s : s;
            double mirror = _complementSmall ? s : 1.0 - s;
            // The upper tail is the incomplete beta with its shapes exchanged, read at the complement.
            return _lowerTail
                ? Beta.RegularizedIncomplete(_a, _b, point, mirror)
                : Beta.RegularizedIncomplete(_b, _a, mirror, point);
        }

        /// <summary><c>d log T / d log s</c>: <c>± s f / T</c>, with <c>f</c> the beta density at the point.</summary>
        internal double LogSlope(double s, double value)
        {
            double x = _complementSmall ? 1.0 - s : s;
            double y = _complementSmall ? s : 1.0 - s;
            double density = Beta.Front(_a, _b, x, y) / (x * y);
            double slope = s * density / value;
            return Increasing ? slope : -slope;
        }
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
