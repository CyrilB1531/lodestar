namespace Lodestar.Stats.Internal;

/// <summary>A continuous distribution's upper tail and its log density, as an inversion reads them.</summary>
/// <remarks>
/// A struct constraint rather than two delegates: the JIT specializes the inversion per
/// distribution, so the tail it evaluates on every step is not an indirect call.
/// </remarks>
internal interface IUpperTail
{
    double Survival(double x);

    double LogDensity(double x);
}

/// <summary>Solves <c>Sf(x) = p</c> for a tail symmetric about zero, by safeguarded Newton from a seed.</summary>
/// <remarks>
/// The answer is the root of the library's own tail: the seed decides how many steps it takes,
/// and a poor one costs evaluations rather than digits. Newton runs on <c>log Sf</c> against
/// <c>log x</c>, which is exactly linear in a power-law tail and close to it in a Gaussian one.
/// A step that leaves the bracket the evaluations have established is replaced by growth (no
/// upper bound yet) or bisection (both known), so convergence never rests on the step.
/// </remarks>
internal static class TailInversion
{
    // Growth reaches 1e308 in eleven steps and geometric bisection halves the exponent, so
    // even a tail Newton cannot use ends in well under this; bisection alone spent about 60.
    private const int MaxIterations = 200;

    // The log residual a step settling on a bound may leave: about 4,500 ulps of the tail, above its rounding.
    private const double ResidualTolerance = 1e-12;

    // Newton's next error is about h^2 for a step of h in log x, so 1e-8 leaves ~1e-16; replayed
    // against the bisection this replaced, a 24-df by 36-p grid agrees to Sf's own resolution.
    private const double StepTolerance = 1e-8;

    // Growth, and the midpoint against 1 while low is zero, move the exponent: halving toward one from 1 took most of
    // MaxIterations (#1690's review).
    private static readonly StepRule Rule = new(StepTolerance, 1.0, double.PositiveInfinity, 0.0);

    /// <summary>The positive x with <c>Sf(x) = p</c>, for p already reduced into (0, 0.5).</summary>
    internal static double InvertUpperTail<TTail>(TTail tail, double p, double seed)
        where TTail : struct, IUpperTail
    {
        // Sf(0) = 0.5 > p is the symmetry the caller relies on, so zero is a valid lower bound.
        double low = 0.0;
        double high = double.PositiveInfinity;
        double x = seed > 0.0 && seed < double.MaxValue ? seed : 1.0;

        for (int i = 0; i < MaxIterations; i++)
        {
            double s = tail.Survival(x);

            // A NaN tail compares false and lands on the high side, as bisection treated it.
            if (s > p)
            {
                low = x;
            }
            else
            {
                high = x;
            }

            // Step in log x from d log Sf / d log x = -x density / Sf, logs apart: s / x underflows
            // past t = 1e154 at p = 1e-310, and that zero would read as a converged step.
            double logResidual = Math.Log(s / p);
            double logStep = NewtonStep(logResidual, Math.Exp(Math.Log(s) - Math.Log(x) - tail.LogDensity(x)));

            if (Finish(x, logStep, logResidual, low, high, Rule, out double next))
            {
                return next;
            }

            x = next;
        }

        return x;
    }

    /// <summary>Takes the Newton step or its fallback, and says whether the inversion is done.</summary>
    /// <remarks>
    /// The three inversions share it, each with its own <see cref="StepRule"/>, so a change to the rule is made once: the
    /// gamma and beta ones had grown their own copies, which already differed (#1690's review). A step under an ulp
    /// tries the neighbouring double instead, and a bracket of two neighbours settles: a narrow law's tail moves more
    /// per ulp than ResidualTolerance, and F's quantile at (1e-3, 1e12, 2e12) bisected 30 times, 24 µs where 724c7912 took 3.6.
    /// </remarks>
    internal static bool Finish(
        double x, double logStep, double logResidual, double low, double high, in StepRule rule, out double next)
    {
        next = x * Math.Exp(logStep);
        bool inside = next > low && next < high;

        if (inside && Math.Abs(logStep) <= rule.Tolerance)
        {
            return true;
        }
        if (!inside && SettlesOnBound(logStep, logResidual, rule.Tolerance))
        {
            next = x;
            return true;
        }
        // Two neighbours: the upper one, the smallest x whose evaluation passed the target, unless it is the bracket's
        // unevaluated end; the last one evaluated sat an ulp short of a narrow chi-squared crossing (#1690's review).
        if (!inside && high <= NextUp(low))
        {
            next = high < rule.Ceiling ? high : x;
            return true;
        }
        if (inside)
        {
            return false;
        }

        if (TryNeighbour(x, next, logStep, low, high, out double neighbour))
        {
            next = neighbour;
            return false;
        }

        next = double.IsPositiveInfinity(high) ? Grow(Math.Max(x, low)) : Midpoint(low, high, rule.Floor, rule.RelativeFloor);

        // S1244: the bracket's own fixed-point test -- the midpoint stops moving
        // once it lands on one of its bounds, and no further bit is available.
#pragma warning disable S1244
        return next == low || next == high;
#pragma warning restore S1244
    }

    /// <summary>The log step, the residual times <paramref name="inverseSlope"/>, and zero where the residual is.</summary>
    /// <remarks>
    /// A zero residual is a root whatever the slope: at a density of e^-714, beta shapes 5e-311 and x = 0.5, the inverse
    /// slope overflowed and 0 times infinity sent the fallback down to the smallest double (#1690's review). The gamma and
    /// beta inversions take it too.
    /// </remarks>
    internal static double NewtonStep(double logResidual, double inverseSlope)
    {
        // S1244: an exact zero is the one residual no slope may scale.
#pragma warning disable S1244
        return logResidual == 0.0 ? 0.0 : logResidual * inverseSlope;
#pragma warning restore S1244
    }

    /// <summary><c>log(target / value)</c>, as the difference of the two logs where that costs x no more than a few ulps.</summary>
    /// <remarks>
    /// The difference rounds by an ulp of <c>log target</c>, and x takes that over the slope <c>d log T / d log x</c>:
    /// ChiSquaredIsf(1.3e-204, 4.6e-207), a log of -469 and a slope of 1/577, was 1.3e-11 off (#1690's review). The
    /// quotient holds it to an ulp, but only while it is a positive finite double, and costs a log the common case skips.
    /// </remarks>
    internal static double LogResidual(double target, double logTarget, double value, double logValue, double inverseSlope)
    {
        double residual = logTarget - logValue;
        if (Math.Abs(logTarget * inverseSlope) <= 4.0)
        {
            return residual;
        }
        double ratio = target / value;
        return ratio > 0.0 && ratio < double.PositiveInfinity ? Math.Log(ratio) : residual;
    }

    /// <summary>Whether a step that left the bracket under <paramref name="tolerance"/> settles on its own x.</summary>
    /// <remarks>
    /// Rounding onto a bound says x is as close as the evaluations tell, but only with the residual as small: near a pole
    /// of the density any step is tiny, and an F inverse tail read 1.2e307 for a tail of 0.9999964 (#1690's review).
    /// The gamma and beta inversions take the same rule.
    /// </remarks>
    internal static bool SettlesOnBound(double logStep, double logResidual, double tolerance) =>
        Math.Abs(logStep) <= tolerance && Math.Abs(logResidual) <= ResidualTolerance;

    /// <summary>The next x past a lower bound with no upper one: the largest of √low, 2 low and low², capped at the largest double.</summary>
    /// <remarks>
    /// Each moves the exponent: doubling a tiny bound took most of MaxIterations (#1690's review), and the gamma
    /// inversion's growth by four, from a seed of 2.6e-228 to a root near 250, stopped at 1e-107 after all 200.
    /// </remarks>
    internal static double Grow(double low) =>
        Math.Min(Math.Max(Math.Max(Math.Sqrt(low), 2.0 * low), low * low), double.MaxValue);

    /// <summary>The neighbour of x on the step's side, where the step rounded onto x itself and that neighbour is inside the bracket.</summary>
    private static bool TryNeighbour(double x, double next, double logStep, double low, double high, out double neighbour)
    {
        neighbour = logStep > 0.0 ? NextUp(x) : NextDown(x);
        // S1244: a step that rounds onto x itself, which only the neighbour on its side can test.
#pragma warning disable S1244
        return next == x && logStep != 0.0 && neighbour > low && neighbour < high;
#pragma warning restore S1244
    }

    /// <summary>A search's test on the bit pattern of a non-negative double, which orders as the double does.</summary>
    internal interface IBitTest
    {
        bool Holds(long bits);
    }

    /// <summary>A quantile's exact tail before its last rounding, and one Newton step on it toward the target from a point
    /// whose tail that is.</summary>
    internal interface INewtonTail
    {
        ExactTail Exact(double statistic);

        double Stepped(double statistic, ExactTail exact);

        /// <summary>Whether a tail before its last rounding reaches the target.</summary>
        bool Reaches(ExactTail exact);
    }

    /// <summary>The last bit pattern still holding <paramref name="test"/> and the first failing it, by steps doubling from
    /// <paramref name="from"/>, where it holds, toward <paramref name="limit"/>, never reached, then halving between them;
    /// <paramref name="limit"/> as the failing one where no probe fails. The chi-squared small root and the F quantile at
    /// tiny dfs both search the doubles so.</summary>
    internal static (long Held, long Failed) SearchBits<TTest>(long from, long limit, ref TTest test)
        where TTest : struct, IBitTest
    {
        (long held, long failed) = DoubleBits(from, limit, ref test);
        return HalveBits(held, failed, ref test);
    }

    /// <summary>SearchBits' doubling steps alone: the last probe holding <paramref name="test"/> and the first failing it,
    /// <paramref name="limit"/>, untested, where none fails.</summary>
    private static (long Held, long Failed) DoubleBits<TTest>(long from, long limit, ref TTest test)
        where TTest : struct, IBitTest
    {
        long held = from;
        long failed = limit;
        long distance = Math.Abs(limit - from);
        // Each probe stays strictly between from and limit, and the step stops before it could pass the largest long.
        for (long step = 1; step < distance; step *= 2)
        {
            long probe = limit > from ? from + step : from - step;
            if (!test.Holds(probe))
            {
                failed = probe;
                break;
            }
            held = probe;
            if (step > distance / 2)
            {
                break;
            }
        }
        return (held, failed);
    }

    /// <summary>The bits of the smallest double whose tail reaches its target, <paramref name="reaches"/> telling which do,
    /// within <paramref name="window"/> of <paramref name="from"/>; −1 where the tail crosses no nearer.</summary>
    /// <remarks>An inversion's root lands a few ulps from its crossing where the tail is exact to its last bit (#1745);
    /// farther, the tail is too flat, or too far from that, for its doubles to place the root better than the inversion.</remarks>
    internal static long NearestReaching<TTest>(long from, long window, ref TTest reaches)
        where TTest : struct, IBitTest =>
        NearestReaching(from, window, reaches.Holds(from), ref reaches);

    /// <summary>The same with the test at <paramref name="from"/> already read, <paramref name="atFrom"/>.</summary>
    private static long NearestReaching<TTest>(long from, long window, bool atFrom, ref TTest reaches)
        where TTest : struct, IBitTest
    {
        long limit = atFrom
            ? Math.Max(from - window, 0)
            : Math.Min(from + window, BitConverter.DoubleToInt64Bits(double.MaxValue));
        if (limit == from)
        {
            // No double beyond: from itself, read already, where it reaches; else none.
            return atFrom ? from : -1;
        }
        var sided = new Sided<TTest>(reaches, atFrom);
        (long held, long failed) = DoubleBits(from, limit, ref sided);
        // The steps never test their limit: 0 reaches nothing, and any other is read here, before the halving, which spent
        // six tails on a window holding no crossing; a crossing at its edge, or the largest double (#1745's Review A).
        if (failed == limit && limit != 0 && reaches.Holds(limit) == atFrom)
        {
            return -1;
        }
        (held, failed) = HalveBits(held, failed, ref sided);
        return atFrom ? held : failed;
    }

    // The ulps either side of an inversion's root within which its crossing is searched, and half of them, within which a
    // Newton step from it is the start; past them up to NewtonSteps steps, each from the last's own exact tail.
    private const long CrossingWindow = 128;
    private const double NewtonReach = 64.0;
    private const int NewtonSteps = 3;

    /// <summary>The first Newton step from <paramref name="root"/>, whose exact tail is <paramref name="exact"/>, to move
    /// within NewtonReach ulps, the steps before it farther and each taken from its own tail; else the last point reached.
    /// <paramref name="atStart"/> is the exact tail at the start where a step took it, else NaN.</summary>
    /// <remarks>A root farther than NewtonReach kept the inversion's start: Student's at df 1999 searched 14 exact tails, 47 us
    /// to scipy's 24, and at dfs near 0.05 crossings 196 ulps off lay past the window (#1745's Review A).</remarks>
    private static double NewtonStart<TTail>(double root, ExactTail exact, double? firstStep, ref TTail tail, out ExactTail atStart)
        where TTail : struct, INewtonTail
    {
        double start = root;
        double next = root;
        atStart = exact;
        for (int step = 0; step < NewtonSteps; step++)
        {
            next = step == 0 && firstStep.HasValue ? firstStep.Value : tail.Stepped(start, exact);
            if (!(next > 0.0 && next <= double.MaxValue))
            {
                return start;
            }
            if (step == NewtonSteps - 1 || Math.Abs(next - start) <= NewtonReach * (NextUp(start) - start))
            {
                break;
            }
            start = next;
            exact = tail.Exact(start);
            atStart = exact;
        }
        // A step that moved nothing lands on the tail just taken, which Settle would otherwise take again.
        atStart = BitConverter.DoubleToInt64Bits(next) == BitConverter.DoubleToInt64Bits(start)
            ? exact
            : new ExactTail(new DoubleDouble(double.NaN, 0.0), inDouble: false);
        return next;
    }

    /// <summary>The bits of the smallest double reaching the target near the Newton steps' start from <paramref name="root"/>,
    /// whose exact tail is <paramref name="exact"/> and test <paramref name="rootReaches"/>; −1 as Settle has it.</summary>
    internal static long Settled<TTail, TTest>(double root, ExactTail exact, bool rootReaches, ref TTail tail, ref TTest reaches)
        where TTail : struct, INewtonTail
        where TTest : struct, IBitTest =>
        Settled(root, exact, rootReaches, ref tail, ref reaches, searchesFar: true, firstStep: null);

    /// <summary><see cref="Settled{TTail, TTest}(double, ExactTail, bool, ref TTail, ref TTest)"/>, or
    /// <see cref="NotSearched"/> where <paramref name="searchesFar"/> does not hold and the crossing is not at the Newton
    /// steps' start, the root or a neighbour of either; <paramref name="firstStep"/> the first step where the caller took it, NaN included.</summary>
    internal static long Settled<TTail, TTest>(
        double root, ExactTail exact, bool rootReaches, ref TTail tail, ref TTest reaches, bool searchesFar, double? firstStep)
        where TTail : struct, INewtonTail
        where TTest : struct, IBitTest
    {
        double start = NewtonStart(root, exact, firstStep, ref tail, out ExactTail atStart);
        bool? startReaches = double.IsNaN(atStart.Rounded) ? null : tail.Reaches(atStart);
        return Settle(root, start, rootReaches, startReaches, ref reaches, searchesFar);
    }

    /// <summary>What Settled answers where it was told not to search past the windows.</summary>
    internal const long NotSearched = -2;

    /// <summary>The bits of the smallest double reaching the target near <paramref name="start"/>, else near
    /// <paramref name="root"/>, a step the double forms misjudged, as at the narrow F law of dfs (3e235, 3e139), else the
    /// crossing searched from the root however far; −1 where no double above the root reaches the target.</summary>
    /// <remarks><paramref name="rootReaches"/> is the test at the root, which the caller has read, and
    /// <paramref name="startReaches"/> the start's where read: read again, each cost a whole tail, 5 us of
    /// FisherSnedecorIsf(0.9999999999, 1999, 3e-9)'s 29, scipy's 28.5 (#1745's Review A).</remarks>
    private static long Settle<TTest>(double root, double start, bool rootReaches, bool? startReaches, ref TTest reaches, bool searchesFar)
        where TTest : struct, IBitTest
    {
        long startBits = BitConverter.DoubleToInt64Bits(start);
        long rootBits = BitConverter.DoubleToInt64Bits(root);
        // Not searching far, the start and its neighbour alone: a window's eight tails cost more than the root they replace.
        long window = searchesFar ? CrossingWindow : 1;
        long nearest;
        if (startBits == rootBits)
        {
            nearest = NearestReaching(rootBits, window, rootReaches, ref reaches);
        }
        else
        {
            // −1 is no double's bits, so an unread start is never taken as known.
            var known = new KnownAt<TTest>(reaches, rootBits, rootReaches, startReaches.HasValue ? startBits : -1L, startReaches ?? false);
            nearest = NearestReaching(startBits, window, ref known);
            // The root's window through the start's known answers, searching far or not: unread, a law narrower than an ulp
            // left its crossing at the root's neighbour, FisherSnedecorIsf(0.5000001175890465, 1.33e35, 1.41e61) an ulp short.
            nearest = nearest < 0 ? NearestReaching(rootBits, window, rootReaches, ref known) : nearest;
        }
        if (nearest >= 0)
        {
            return nearest;
        }
        return searchesFar ? FarCrossing(rootBits, rootReaches, ref reaches) : NotSearched;
    }

    /// <summary>The crossing past the window about <paramref name="from"/>, whose test is <paramref name="atFrom"/> and the
    /// window's throughout, by steps doubling from its edge toward it then halving; −1 where no double above reaches the target.</summary>
    /// <remarks>Past the window where a tail is a double form's, whose Newton steps aim at the target itself: kept, the
    /// root read 1,522 ulps past the crossing at F dfs (0.001, 1e6), the tail a subnormal half's limit (#1745's Review A).</remarks>
    private static long FarCrossing<TTest>(long from, bool atFrom, ref TTest reaches)
        where TTest : struct, IBitTest
    {
        // The window's doubles were read: from its edge the steps test none again, eight tails (#1745's Review A).
        long largest = BitConverter.DoubleToInt64Bits(double.MaxValue);
        from = atFrom ? from - CrossingWindow : Math.Min(from + CrossingWindow, largest);
        if (atFrom)
        {
            return SearchBits(from, 0L, ref reaches).Held;
        }
        if (from == largest)
        {
            return -1;
        }
        // The largest double tested before halving toward it, as NearestReaching tests its limit: some 60 tails otherwise
        // where no double above the root reaches the target (#1745's Review A).
        var shortOf = new Sided<TTest>(reaches, false);
        (long held, long failed) = DoubleBits(from, largest, ref shortOf);
        if (failed == largest && !reaches.Holds(largest))
        {
            return -1;
        }
        return HalveBits(held, failed, ref shortOf).Failed;
    }

    /// <summary>One Newton step from <paramref name="statistic"/>, the tail <paramref name="excess"/> past its aim and its
    /// slope in log of the statistic <paramref name="front"/>, against a tail rising with the statistic where
    /// <paramref name="risingTail"/> holds: F's and Student's steps, one form.</summary>
    internal static double EdgeStep(double statistic, double excess, double front, bool risingTail)
    {
        double step = statistic * (excess / front);
        return risingTail ? statistic - step : statistic + step;
    }

    /// <summary>A test whose answer at <c>known</c> is already known, as the root's is to Settle: a search from a step one
    /// ulp off it read that whole tail again.</summary>
    private struct KnownAt<TTest>(TTest inner, long known, bool holds, long other, bool otherHolds) : IBitTest
        where TTest : struct, IBitTest
    {
        private TTest _inner = inner;

        public bool Holds(long bits)
        {
            if (bits == known)
            {
                return holds;
            }
            return bits == other ? otherHolds : _inner.Holds(bits);
        }
    }

    /// <summary>A test, or its negation, as the bit search's test from a start where it reads <c>side</c>.</summary>
    internal struct Sided<TTest>(TTest inner, bool side) : IBitTest
        where TTest : struct, IBitTest
    {
        private TTest _inner = inner;
        private readonly bool _side = side;

        public bool Holds(long bits) => _inner.Holds(bits) == _side;
    }

    /// <summary>The adjacent bit patterns halving leaves of <paramref name="held"/>, holding <paramref name="test"/>, and
    /// <paramref name="failed"/>, failing it; neither end is tested.</summary>
    internal static (long Held, long Failed) HalveBits<TTest>(long held, long failed, ref TTest test)
        where TTest : struct, IBitTest
    {
        while (Math.Abs(failed - held) > 1)
        {
            long middle = held + ((failed - held) / 2);
            if (test.Holds(middle))
            {
                held = middle;
            }
            else
            {
                failed = middle;
            }
        }
        return (held, failed);
    }

    /// <summary>The next double above a non-negative <paramref name="value"/>, either zero included; NaN, +∞ and negatives answer themselves.</summary>
    /// <remarks>Math.BitIncrement on net10.0, the bits on netstandard2.0, which has none; both only where they agree.</remarks>
    internal static double NextUp(double value)
    {
        // Zero of either sign, whose -0 bits plus one are the smallest negative double on netstandard2.0.
#pragma warning disable S1244
        if (value == 0.0)
#pragma warning restore S1244
        {
            return double.Epsilon;
        }
        if (!(value > 0.0 && value < double.PositiveInfinity))
        {
            return value;
        }
#if NET
        return Math.BitIncrement(value);
#else
        return BitConverter.Int64BitsToDouble(BitConverter.DoubleToInt64Bits(value) + 1);
#endif
    }

    /// <summary>The next double below a positive <paramref name="value"/>; NaN, zero, +∞ and negatives answer themselves.</summary>
    internal static double NextDown(double value)
    {
        if (!(value > 0.0 && value < double.PositiveInfinity))
        {
            return value;
        }
#if NET
        return Math.BitDecrement(value);
#else
        return BitConverter.Int64BitsToDouble(BitConverter.DoubleToInt64Bits(value) - 1);
#endif
    }

    /// <summary>A bracket's next point: √(low·high), against the larger of <paramref name="floor"/> and <paramref name="relativeFloor"/> of high while low is zero, while they are orders apart; then the midpoint.</summary>
    /// <remarks>
    /// The three inversions share it, so the exponent halves wherever a root is far: the beta one, by eighths from 1e-16,
    /// spent all 200 iterations on a root of 1e-200, and the gamma one still stepped down by eighths (#1690's review).
    /// </remarks>
    internal static double Midpoint(double low, double high, double floor, double relativeFloor)
    {
        double anchor = low > 0.0 ? low : Math.Max(floor, relativeFloor * high);
        if (high > 4.0 * anchor)
        {
            return Math.Sqrt(anchor) * Math.Sqrt(high);
        }

        // Halved first where the sum would overflow, so two bounds in the top binade keep a finite midpoint.
        double midpoint = 0.5 * (low + high);
        return double.IsInfinity(midpoint) ? low + (0.5 * (high - low)) : midpoint;
    }
}

/// <summary>How an inversion settles: its step tolerance in <c>log x</c>, its midpoint's anchor while the lower bound is zero, and its bracket's end.</summary>
/// <remarks>
/// A tolerance under an ulp of <c>log x</c> is left as it is: a step rounding onto x tries its neighbour in Finish, so the
/// root is held to an ulp. A floor of two ulps, tried first, accepted unevaluated steps and left a chi-squared quantile
/// narrower than an ulp four ulps past its crossing (#1690's review).
/// </remarks>
internal readonly struct StepRule(double tolerance, double floor, double ceiling, double relativeFloor)
{
    internal double Tolerance { get; } = tolerance;

    internal double Floor { get; } = floor;

    /// <summary>The bracket's initial upper end, which no evaluation stands behind.</summary>
    internal double Ceiling { get; } = ceiling;

    /// <summary>A fraction of the upper bound the anchor is held to while the lower one is zero, or 0 for none.</summary>
    /// <remarks>
    /// Against the smallest double alone, one failed step from a root near 0.1 jumped to 1e-162 and climbed back by halving
    /// exponents; at 1e-8 of the bound the descent still moves four decades a step (#1690's review).
    /// </remarks>
    internal double RelativeFloor { get; } = relativeFloor;
}
