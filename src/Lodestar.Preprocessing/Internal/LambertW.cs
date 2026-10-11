namespace Lodestar.Preprocessing.Internal;

/// <summary>The lower real branch of Lambert's W, <c>scipy.special.lambertw(z, k=-1)</c> on <c>[-1/e, 0)</c>.</summary>
/// <remarks>
/// What <c>scipy.stats.boxcox_normmax</c> reads a constrained exponent off when the optimum would overflow
/// the transform (#1227). Seeded by the branch-point series near <c>-1/e</c> and by the asymptotic
/// <c>ln(-z) - ln(-ln(-z))</c> near zero, then refined by Halley's iteration to <c>1e-15</c> relative, a few ulps.
/// It converges in a handful of steps on what <c>boxcox_normmax</c> passes it. Near <c>-1/e</c> it can stop 1e-4
/// relative off, and at <c>z = -5e-324</c> it answers +∞ where the branch is negative; the overflow guard's default
/// ceiling reaches neither.
/// </remarks>
internal static class LambertW
{
    private const int MaxIterations = 64;

    /// <summary><c>W₋₁(z)</c> for <c>z</c> in <c>[-1/e, 0)</c>.</summary>
    public static double LowerBranch(double z)
    {
        double w;
        if (z < -0.25)
        {
            // Near -1/e the product can round a hair below -1, where the root would be NaN: the branch point is 0.
            double p = -Math.Sqrt(Math.Max(0.0, 2.0 * ((Math.E * z) + 1.0)));
            w = -1.0 + p - (p * p / 3.0) + (11.0 / 72.0 * p * p * p);
        }
        else
        {
            double l1 = Math.Log(-z);
            double l2 = Math.Log(-l1);
            w = l1 - l2 + (l2 / l1);
        }

        for (int i = 0; i < MaxIterations; i++)
        {
            double ew = Math.Exp(w);
            double f = (w * ew) - z;
            double denominator = (ew * (w + 1.0)) - ((w + 2.0) * f / ((2.0 * w) + 2.0));
            double next = w - (f / denominator);
            if (double.IsNaN(next) || Math.Abs(next - w) <= 1e-15 * Math.Abs(next))
            {
                return double.IsNaN(next) ? w : next;
            }

            w = next;
        }

        return w;
    }
}
