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

    // Newton's next error is about h^2 for a step of h in log x, so 1e-8 leaves ~1e-16; replayed
    // against the bisection this replaced, a 24-df by 36-p grid agrees to Sf's own resolution.
    private const double StepTolerance = 1e-8;

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
            double logStep = Math.Log(s / p)
                * Math.Exp(Math.Log(s) - Math.Log(x) - tail.LogDensity(x));

            if (TryFinish(x, logStep, low, high, out double next))
            {
                return next;
            }

            x = next;
        }

        return x;
    }

    /// <summary>Takes the Newton step or its fallback, and says whether the inversion is done.</summary>
    private static bool TryFinish(double x, double logStep, double low, double high, out double next)
    {
        next = x * Math.Exp(logStep);
        bool inside = next > low && next < high;

        if (Math.Abs(logStep) <= StepTolerance)
        {
            // Within tolerance of the root; a step that rounds onto or past a bound
            // says x itself is as close as the evaluations can tell.
            next = inside ? next : x;
            return true;
        }
        if (inside)
        {
            return false;
        }

        next = Fallback(low, high);

        // S1244: the bracket's own fixed-point test -- the midpoint stops moving
        // once it lands on one of its bounds, and no further bit is available.
#pragma warning disable S1244
        return next == low || next == high;
#pragma warning restore S1244
    }

    // Growth by squaring past 2, and a geometric midpoint (against 1 while the lower bound is
    // still zero) when the bounds are orders apart: both move the exponent, not the mantissa.
    private static double Fallback(double low, double high)
    {
        if (double.IsPositiveInfinity(high))
        {
            return Math.Min(Math.Max(2.0 * low, low * low), double.MaxValue);
        }
        if (high > 4.0 * Math.Max(low, 1.0))
        {
            return Math.Sqrt(Math.Max(low, 1.0)) * Math.Sqrt(high);
        }

        return 0.5 * (low + high);
    }
}
