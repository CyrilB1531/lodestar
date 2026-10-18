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

    /// <summary>A walk's rule: whether to take the ulp from one double to its neighbour.</summary>
    internal interface IUlpStep
    {
        bool Takes(double from, double to);
    }

    /// <summary><paramref name="value"/> walked an ulp at a time, down or up, while <paramref name="step"/> takes each
    /// ulp, at most <paramref name="maxSteps"/>, never below zero: the F quantile walks its crossing in f with it, eight ulps
    /// each way, and the chi-squared small root's search reads <see cref="TakesStep"/> for each of its probes.</summary>
    internal static double Walk<TStep>(double value, bool down, int maxSteps, ref TStep step)
        where TStep : struct, IUlpStep
    {
        for (int i = 0; i < maxSteps && TakesStep(value, down, ref step, out double next); i++)
        {
            value = next;
        }
        return value;
    }

    /// <summary>Whether <paramref name="step"/> takes the ulp from <paramref name="value"/>, down or up, to <paramref name="next"/>.</summary>
    internal static bool TakesStep<TStep>(double value, bool down, ref TStep step, out double next)
        where TStep : struct, IUlpStep
    {
        next = down ? NextDown(value) : NextUp(value);
        // S1244: NextDown(0) is 0 itself, the end of a walk down, which the step must not read as a step up.
#pragma warning disable S1244
        return next != value && step.Takes(value, next);
#pragma warning restore S1244
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
