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

    // The midpoint's anchor while the lower bound is zero, as a fraction of the upper: StepRule has why.
    private const double RelativeFloor = 1e-8;

    // Past shapes near 1e10 the step is held to this fraction of x's relative width: a fixed 1e-13 outgrew the law, and
    // FisherSnedecorQuantile(0.5, 1e30, 3e30) read a root 50 standard deviations off (#1690's review).
    private const double WidthTolerance = 1e-8;

    // A relative width under this many ulps of the small side has no slope Newton can read between its doubles.
    private const double UlpWidth = 4.0 * NearZero.MachineEpsilon;

    // The small side's mean past which TryNarrow leaves the law to the general route: why is there.
    private const double NarrowMeanLimit = 0.49;

    // A law narrower than an ulp puts every root but its tails' last doubles within this many of its mean.
    private const int MaxWalk = 64;

    // Below this, one minus a number near one leaves fewer than thirteen of its digits (InvertBoth).
    private const double DerivedSideFloor = 1e-3;

    // log(double.Epsilon), the smallest positive double: a seed below this is a root no double
    // can name, and the answer is zero rather than wherever the iteration happened to stop.
    private const double LogSmallestDenormal = -744.44;

    // The largest double below one, 1 - 2^-53.
    private const double LargestBelowOne = 0.9999999999999999;

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
        return Mirrored(p, a, b, Beta.ShapeLogs.Of(a, b));
    }

    /// <summary><see cref="Invert"/>'s solve, the upper half as its mirror, with the shapes' logarithms already taken.</summary>
    private static double Mirrored(double p, double a, double b, Beta.ShapeLogs logs)
    {
        bool mirrored = p > 0.5;
        // S2234: the mirror I_x(a, b) = 1 - I_(1-x)(b, a), whose exchange is the identity rather than a slip.
#pragma warning disable S2234
        double solved = mirrored ? Solve(1.0 - p, b, a, logs.Swapped) : Solve(p, a, b, logs);
#pragma warning restore S2234
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
        var logs = Beta.ShapeLogs.Of(a, b);
        if (TryNarrow(lower, upper, a, b, logs, out (double X, double Complement) narrow))
        {
            return narrow;
        }
        (double x0, double complement0) = SeedBoth(lower, upper, a, b, logs);

        // One minus a number near one is off by about 1e-16 absolute, so above 1e-3 it holds 1e-13 relative and Invert's
        // answer stands; only a smaller one, the case that was 1e-7 off, is refined.
        double derived = lower <= upper ? complement0 : x0;
        if (derived >= DerivedSideFloor)
        {
            return (x0, complement0);
        }

        bool complementSmall = x0 > 0.5;
        var tail = new SmallSideTail(a, b, complementSmall, lower <= upper, Math.Min(lower, upper), logs);
        double s = Refine(tail, complementSmall ? complement0 : x0);
        return complementSmall ? (1.0 - s, s) : (s, 1.0 - s);
    }

    /// <summary><see cref="Invert"/>'s root from the smaller probability's own tail, with its complement.</summary>
    /// <remarks>
    /// Seeded from the smaller probability so the side solved directly is named rather than formed as
    /// one minus a root that rounded to one: at an upper tail of 1e-12 and shapes (0.5, 0.5) the
    /// complement is 1e-24, which 1 - x cannot hold.
    /// </remarks>
    private static (double X, double Complement) SeedBoth(double lower, double upper, double a, double b, Beta.ShapeLogs logs)
    {
        if (lower <= upper)
        {
            double x = Mirrored(lower, a, b, logs);
            return (x, 1.0 - x);
        }
        // S2234: I_x(a, b) = 1 - I_(1-x)(b, a), so the upper tail is the swapped shapes' lower tail at
        // 1 - x, and the exchange is the identity rather than a slip.
#pragma warning disable S2234
        double complement = Mirrored(upper, b, a, logs.Swapped);
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

            double inverseSlope = 1.0 / tail.LogSlope(s, value);
            double logResidual = TailInversion.LogResidual(tail.Target, tail.LogTarget, value, Math.Log(value), inverseSlope);
            double logStep = TailInversion.NewtonStep(logResidual, inverseSlope);
            if (TailInversion.Finish(s, logStep, logResidual, low, high, tail.Rule, out double next))
            {
                return next;
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
        private readonly Beta.ShapeLogs _logs;

        internal SmallSideTail(double a, double b, bool complementSmall, bool lowerTail, double target, Beta.ShapeLogs logs)
        {
            _a = a;
            _b = b;
            _complementSmall = complementSmall;
            _lowerTail = lowerTail;
            _logs = logs;
            Target = target;
            LogTarget = Math.Log(target);
            // I_x(a, b) rises with x; read against 1 - x it falls, and the upper tail is its mirror.
            Increasing = complementSmall != lowerTail;
            (double first, double second) = SmallSide(complementSmall, a, b);
            Rule = new StepRule(ToleranceFor(first, second), double.Epsilon, 0.5, RelativeFloor);
        }

        internal double Target { get; }

        internal double LogTarget { get; }

        internal bool Increasing { get; }

        internal StepRule Rule { get; }

        internal double Value(double s)
        {
            double point = _complementSmall ? 1.0 - s : s;
            double mirror = _complementSmall ? s : 1.0 - s;
            // The upper tail is the incomplete beta with its shapes exchanged, read at the complement.
            return _lowerTail
                ? Beta.RegularizedIncomplete(_a, _b, point, mirror, _logs)
                : Beta.RegularizedIncomplete(_b, _a, mirror, point, _logs.Swapped);
        }

        /// <summary><c>d log T / d log s</c>: <c>± s f / T</c>, with <c>f</c> the beta density at the point.</summary>
        internal double LogSlope(double s, double value)
        {
            double x = _complementSmall ? 1.0 - s : s;
            double y = _complementSmall ? s : 1.0 - s;
            double density = Beta.FastFront(_a, _b, x, y, _logs.OfBeta) / (x * y);
            double slope = s * density / value;
            return Increasing ? slope : -slope;
        }
    }

    private static double Solve(double target, double a, double b, Beta.ShapeLogs logs)
    {
        var problem = new Problem(a, b, target, logs);
        double x = Seed(target, a, b, logs);

        // S1244: Seed's literal when its leading term, x^a / (a B), puts the root below every double. That term holds only
        // where the tail is that small: at shapes 5e-311 it is one half from x = 1e-300 up, so the tail must agree (#1690).
#pragma warning disable S1244
        if (x == 0.0)
#pragma warning restore S1244
        {
            if (Beta.RegularizedIncomplete(a, b, double.Epsilon, 1.0, logs) > target)
            {
                return 0.0;
            }
            x = Math.Min(a / (a + b), LargestBelowOne);
        }

        double low = 0.0;
        double high = 1.0;

        for (int i = 0; i < MaxIterations; i++)
        {
            double value = Beta.RegularizedIncomplete(a, b, x, 1.0 - x, problem.Logs);
            if (value < target)
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

    /// <summary>One Newton step, taken on <c>log I</c> against <c>log x</c>.</summary>
    /// <remarks>
    /// In <c>x</c> the step is useless where it is most needed: at shapes (1000, 10000) the tail
    /// crosses a hundred and forty orders of magnitude between <c>x = 0.022</c> and
    /// <c>x = 0.034</c>, so a residual divided by the density there overflows and every step is
    /// discarded for the bracket, which then has a thousand halvings of exponent to walk. In logs
    /// the same curve is close to a straight line, which is why <see cref="TailInversion"/> runs
    /// in logs too. Measured (#1121): 46 of 504 grid points past 1e-9 in <c>x</c>, 0 in logs.
    /// </remarks>
    private static double NewtonLogStep(double x, double value, in Problem problem, out double logResidual)
    {
        double a = problem.A;
        double b = problem.B;
        // log(1 - x) through Gamma's series where x is small rather than Math.Log(1 - x): the root
        // often is here, and 1 - x rounds its own leading digits away.
        double logOneMinusX = Beta.LogComplement(x, 1.0 - x);
        // Where both shapes reach the uniform expansion, a log x against log B cancels, as LogStirlingFront has why.
        double logDensity = UniformBeta.Holds(a, b)
            ? Beta.LogStirlingFront(a, b, x, 1.0 - x) - Math.Log(x) - logOneMinusX
            : ((a - 1.0) * Math.Log(x)) + ((b - 1.0) * logOneMinusX) - problem.Logs.OfBeta;

        // d log I / d log x = x f(x) / I(x), so the step is the log residual over that slope.
        double logValue = Math.Log(value);
        double inverseSlope = Math.Exp(logValue - Math.Log(x) - logDensity);
        logResidual = TailInversion.LogResidual(problem.Target, problem.LogTarget, value, logValue, inverseSlope);
        return TailInversion.NewtonStep(logResidual, inverseSlope);
    }

    /// <summary>A law whose small side is narrow, solved on that side from its mean, or false to take the general route.</summary>
    /// <remarks>
    /// Where WidthTolerance of the small side's relative width is under StepTolerance, one minus a root of the other side
    /// loses more than the law is wide: at dfs (6e183, 1.4e181) the F quantile came out 114 ulps off, and solving both
    /// sides cost F's quantile at (1e-3, 1e12, 2e12) twice the evaluations (#1690's review). Narrower than an ulp, the
    /// tail steps across the root between two doubles and Newton reads no slope there, so the root is walked to.
    /// </remarks>
    private static bool TryNarrow(
        double lower, double upper, double a, double b, Beta.ShapeLogs logs, out (double X, double Complement) root)
    {
        double mean = a / (a + b);
        bool complementSmall = mean > 0.5;
        double start = complementSmall ? b / (a + b) : mean;
        (double first, double second) = SmallSide(complementSmall, a, b);
        root = default;
        // A narrow law's roots lie within 4e-4 of its mean, relatively, so under NarrowMeanLimit they stay below Refine's
        // half; nearer it 1 - x loses nothing, and the mean alone read F's (0.95, 2e10, 2e10) quantile 1 (#1690's review).
        if (start > NarrowMeanLimit || !IsNarrow(first, second))
        {
            return false;
        }
        var tail = new SmallSideTail(a, b, complementSmall, lower <= upper, Math.Min(lower, upper), logs);
        // The side's relative width squared against UlpWidth's, as IsNarrow takes it.
        double s = second >= UlpWidth * UlpWidth * first * (first + second + 1.0)
            ? Refine(tail, NormalSeed(tail, start, first, second))
            : WalkFromMean(tail, start);
        root = complementSmall ? (1.0 - s, s) : (s, 1.0 - s);
        return true;
    }

    /// <summary>The shapes of the side solved, <c>(b, a)</c> for <c>1 − x</c>, the variable of <c>I_(1−x)(b, a)</c>.</summary>
    private static (double First, double Second) SmallSide(bool complementSmall, double a, double b)
    {
        // S2234: the exchange is the reflection's, not a slip.
#pragma warning disable S2234
        return complementSmall ? (b, a) : (a, b);
#pragma warning restore S2234
    }

    /// <summary>The small side's normal approximation, mean plus z standard deviations, inside (0, ½), below the bracket's end.</summary>
    /// <remarks>From the mean Newton took about eight steps at (1e-3, 1e12, 2e12), 3.9 µs; from this seed 1.5, where 724c7912 took 3.0 (#1690's review).</remarks>
    private static double NormalSeed(in SmallSideTail tail, double mean, double first, double second)
    {
        double sum = first + second;
        double deviation = Math.Sqrt(first / sum * (second / sum) / (sum + 1.0));
        double z = Ndtri.Of(tail.Target);
        double seed = mean + ((tail.Increasing ? z : -z) * deviation);
        return seed > 0.0 && seed < 0.5 ? seed : mean;
    }

    /// <summary>The double nearest the small side's mean at which the tail crosses its target, by steps doubling from it in
    /// ulps, then halving between the last two.</summary>
    /// <remarks>Past MaxWalk ulps, which a law narrower than one only reaches past its tails' last doubles, Refine takes over.
    /// Walked ulp by ulp, FisherSnedecorQuantile(5.4e-194, 1.6e264, 1.2e31) read some 60 tails, 33 us to scipy's 25 (#1745's
    /// Review A).</remarks>
    private static double WalkFromMean(in SmallSideTail tail, double s)
    {
        double value = tail.Value(s);
        bool up = value < tail.Target == tail.Increasing;
        long from = BitConverter.DoubleToInt64Bits(s);
        var side = new StartSide(tail, up, value);
        (long held, long crossed) = TailInversion.SearchBits(from, from + ((up ? 1L : -1L) * (MaxWalk + 1L)), ref side);
        if (double.IsNaN(side.CrossedValue))
        {
            return Refine(tail, BitConverter.Int64BitsToDouble(held));
        }
        bool crossedNearer = Math.Abs(side.CrossedValue - tail.Target) < Math.Abs(side.HeldValue - tail.Target);
        return BitConverter.Int64BitsToDouble(crossedNearer ? crossed : held);
    }

    /// <summary>Whether the small side's tail at a double's bits is still on the start's side of the target, keeping the
    /// values of the last bits on each side, which the search answers.</summary>
    private struct StartSide(SmallSideTail tail, bool up, double value) : TailInversion.IBitTest
    {
        private readonly SmallSideTail _tail = tail;
        private readonly bool _up = up;

        internal double HeldValue { get; private set; } = value;

        internal double CrossedValue { get; private set; } = double.NaN;

        public bool Holds(long bits)
        {
            double atValue = _tail.Value(BitConverter.Int64BitsToDouble(bits));
            bool holds = (atValue < _tail.Target == _tail.Increasing) == _up;
            if (holds)
            {
                HeldValue = atValue;
            }
            else
            {
                CrossedValue = atValue;
            }
            return holds;
        }
    }

    /// <summary>Whether WidthTolerance of x's relative width in I_x(a, b) is below StepTolerance, squared to take no root.</summary>
    private static bool IsNarrow(double a, double b) =>
        WidthTolerance * WidthTolerance * b < StepTolerance * StepTolerance * a * (a + b + 1.0);

    /// <summary>The step in <c>log x</c> below which a root of <c>I_x(a, b)</c> is settled.</summary>
    /// <remarks>
    /// x's relative width is <c>√(b / (a (a + b + 1)))</c>, its standard deviation over its mean; <see cref="IsNarrow"/>
    /// says first, with no root taken, whether WidthTolerance of it falls below StepTolerance.
    /// </remarks>
    private static double ToleranceFor(double a, double b) =>
        IsNarrow(a, b) ? WidthTolerance * Math.Sqrt(b / a) / Math.Sqrt(a + b + 1.0) : StepTolerance;

    /// <summary>A first guess: the small-x leading term where it applies, the mean otherwise.</summary>
    /// <remarks>
    /// As <c>x</c> approaches zero, <c>I_x(a, b)</c> approaches <c>x^a / (a·B(a, b))</c>, which
    /// inverts in closed form and puts the seed within an order of magnitude of a root that plain
    /// bisection from <c>(0, 1)</c> would need a thousand steps to reach at <c>p = 1e-300</c>.
    /// Past the mean the expansion no longer leads, and the mean is the better seed.
    /// </remarks>
    private static double Seed(double target, double a, double b, Beta.ShapeLogs logs)
    {
        // Below one: a mean rounding to 1, at a = 5e199 and b = 0.5, seeded Newton on the bracket's end, and the
        // inversion settled on 2.4e-181 for a root within 1e-200 of one; F's Isf read +inf (#1690's review).
        double mean = Math.Min(a / (a + b), LargestBelowOne);
        // log(a B) whole below SmallShape, where log a + log B cancels; LogBeta's, Stirling past a shape of 10, above it:
        // three log-gammas cancelled at b = 5e199 (#1690). Taken here where the cache holds none, as at uniform shapes.
        double logBeta = double.IsNaN(logs.OfBeta) ? Beta.LogBeta(a, b) : logs.OfBeta;
        double logScaled = a < Beta.SmallShape ? logs.ScaledFirst : Math.Log(a) + logBeta;
        double logSmall = (Math.Log(target) + logScaled) / a;

        // Below the smallest denormal the root is not a double at all, and zero says so: at
        // p = 1e-300 with a = 0.5 the true root is near 1e-600 (#1121).
        if (logSmall < LogSmallestDenormal)
        {
            return 0.0;
        }

        double small = Math.Exp(logSmall);

        return double.IsNaN(small) || small <= 0.0 || small >= mean ? mean : small;
    }

    /// <summary>The shapes and their logarithms, the probability and its log, and the step tolerance, carried together.</summary>
    /// <remarks>
    /// One value rather than six parameters: the step needs them all and S107 caps a signature at seven. The logs are
    /// taken once per solve, where each Newton step took the target's again.
    /// </remarks>
    private readonly struct Problem
    {
        internal Problem(double a, double b, double target, Beta.ShapeLogs logs)
        {
            A = a;
            B = b;
            Target = target;
            LogTarget = Math.Log(target);
            Logs = logs;
            Rule = new StepRule(ToleranceFor(a, b), double.Epsilon, 1.0, RelativeFloor);
        }

        internal double A { get; }

        internal double B { get; }

        internal double Target { get; }

        internal double LogTarget { get; }

        internal Beta.ShapeLogs Logs { get; }

        internal StepRule Rule { get; }
    }
}
