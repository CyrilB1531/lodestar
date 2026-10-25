namespace Lodestar.Stats.Internal;

/// <summary>The chi-squared quantiles from the df itself: the gamma inversion at its exact half, then a root below
/// 2⁻¹⁰²¹ taken again from x unhalved.</summary>
internal static class ChiSquaredRoot
{
    /// <summary>The x whose upper tail, or lower when not <paramref name="upper"/>, is <paramref name="p"/>.</summary>
    /// <remarks>
    /// Below ExactHalf.Threshold Q is of degree one in the shape, so the root is Q's at the lifted df and the target
    /// lifted, and P's is Q's at one less the target: no df is halved where its half would round (#1729's Review A).
    /// </remarks>
    internal static double Invert(double p, double df, bool upper)
    {
        if (ExactHalf.Lifts(df))
        {
            double q = upper ? p : 1.0 - p;
            // Past one no tail reaches the lifted target: one, whose root is the support's end.
            double solved = Math.Min(ExactHalf.Lifted(q), 1.0);
            double lifted = ExactHalf.Lifted(df);
            return SmallRoot(2.0 * GammaQuantile.Invert(solved, lifted / 2.0, upper: true), solved, q, df, upper: true);
        }
        // A target past one half is judged on the other side against 1 − p, exact there: a tail near one held its
        // complement to ε, and ChiSquaredIsf(1 − 2^-45, 0.0875) sat 5 granules from ChiSquaredQuantile(2^-45, 0.0875).
        bool flip = p > 0.5;
        double target = flip ? 1.0 - p : p;
        return SmallRoot(2.0 * GammaQuantile.Invert(p, df / 2.0, upper), target, df, upper != flip);
    }

    /// <summary><see cref="SmallRoot(double, double, double, double, bool)"/> at an unlifted df, solved and judged at one
    /// target.</summary>
    private static double SmallRoot(double x, double target, double df, bool upper) => SmallRoot(x, target, target, df, upper);

    /// <summary>A chi-squared root below 2⁻¹⁰²¹, where the inversion solved for x/2 and so answered only even multiples
    /// of the smallest double, taken again from x unhalved.</summary>
    /// <remarks>
    /// From the lead's closed form and a Newton step on the tail, the double nearest the root to the lead's own rounding,
    /// exactly at df 2, zero included: the root lies above a double where the tail at its midpoint with the next lies
    /// short of the target. ChiSquaredQuantile(3.07e-162, 1) read 1e-323 for 1.5e-323 (#1729's Review A). The closed form
    /// solves the law at the df or its lift at <paramref name="solved"/>; the step and the search judge the df's own tail
    /// against the target.
    /// </remarks>
    private static double SmallRoot(double x, double solved, double target, double df, bool upper)
    {
        if (!(x < ExactHalf.Threshold))
        {
            return x;
        }
        // One lead for the closed form and the search, at the df's exact half, lifted where ExactHalf lifts it.
        var lead = new Gamma.UnhalvedLead(df);
        double root = Gamma.UnhalvedRoot(lead, solved, upper);
        if (double.IsNaN(root))
        {
            return x;
        }
        if (root > 0.0)
        {
            double miss = Gamma.ChiSquaredTail(df, root, upper) - target;
            double step = miss / Densities.ChiSquared(root, df);
            double stepped = upper ? root + step : root - step;
            root = stepped > 0.0 && stepped < ExactHalf.Threshold ? stepped : root;
        }
        var rule = new ChiSquaredMidpoint(target, df, upper, lead);
        long start = BitConverter.DoubleToInt64Bits(root);
        if (Steps(start, up: true, ref rule))
        {
            // A walk still stepping at 2^-1021 stops there: the root lies at or past it, nearer than the inversion's x below.
            return Search(start, up: true, ref rule);
        }
        return Steps(start, up: false, ref rule) ? Search(start, up: false, ref rule) : root;
    }

    /// <summary>The double a walk of single ulps from <paramref name="start"/> would stop at, found by steps doubling
    /// until one is not taken and then halving: the walk alone, four ulps each way, returned a guess the rejected Newton
    /// step left further off as is (#1729's Review A). Within a run of doubles whose midpoint tails round alike, the
    /// first reached, as the walk would.</summary>
    private static double Search(long start, bool up, ref ChiSquaredMidpoint rule)
    {
        // The search keeps the last double that steps on and the first that does not, both as bits.
        long end = BitConverter.DoubleToInt64Bits(ExactHalf.Threshold);
        long taken = start;
        long stopped = up ? end : 0;
        for (long step = 1; step < end; step *= 2)
        {
            long probe = up ? start + step : start - step;
            if (probe <= 0 || probe >= end)
            {
                break;
            }
            if (!Steps(probe, up, ref rule))
            {
                stopped = probe;
                break;
            }
            taken = probe;
        }
        while (Math.Abs(stopped - taken) > 1)
        {
            long middle = taken + ((stopped - taken) / 2);
            if (Steps(middle, up, ref rule))
            {
                taken = middle;
            }
            else
            {
                stopped = middle;
            }
        }
        return BitConverter.Int64BitsToDouble(stopped);
    }

    /// <summary>Whether the walk takes the ulp from the double of these bits, up or down; never below zero.</summary>
    private static bool Steps(long bits, bool up, ref ChiSquaredMidpoint rule) =>
        TailInversion.TakesStep(BitConverter.Int64BitsToDouble(bits), down: !up, ref rule, out _);

    /// <summary>The search's rule: a step toward the chi-squared root, taken where the tail at the midpoint lies short of the target.</summary>
    private readonly struct ChiSquaredMidpoint(double target, double df, bool upper, Gamma.UnhalvedLead lead) : TailInversion.IUlpStep
    {
        private readonly Gamma.UnhalvedLead _lead = lead;

        // A subnormal lower target at an unlifted df is met scaled by 2^600 with its tail, both normal there; the df's own
        // lift by 2^200 is the lead's, a separate case.
        private readonly (bool Scaled, double Target) _goal = Goal(target, df, upper);

        public bool Takes(double from, double to)
        {
            // The lower tail rises with x and the upper falls, so the root lies above the midpoint where its tail there is
            // still short of the target on that side.
            double low = Math.Min(from, to);
            double high = Math.Max(from, to);
            // The miss as the end's tail less the target, exact where the two are near, plus the rise to the midpoint.
            double miss;
            if (_goal.Scaled)
            {
                (double atEnd, double rise) = Gamma.ScaledLowerTailBetween(_lead, low, high);
                miss = (atEnd - _goal.Target) + rise;
            }
            else
            {
                miss = Gamma.ChiSquaredTailBetween(_lead, low, high, upper) - _goal.Target;
            }
            if (to > from)
            {
                return upper ? miss > 0.0 : miss < 0.0;
            }
            return upper ? miss < 0.0 : miss > 0.0;
        }

        private static (bool Scaled, double Target) Goal(double target, double df, bool upper) =>
            !upper && target < NearZero.SmallestNormal && !ExactHalf.Lifts(df)
                ? (true, target * Gamma.MidpointLiftFactor)
                : (false, target);
    }
}
