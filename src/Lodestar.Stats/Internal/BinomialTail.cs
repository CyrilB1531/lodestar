namespace Lodestar.Stats.Internal;

/// <summary>The binomial mass and its two tails, read off the regularized incomplete beta.</summary>
/// <remarks>
/// A binomial tail is a beta integral — <c>sf(k) = I_p(k+1, n-k)</c> — so nothing here sums a
/// series: the sums would cost O(n) and lose the far tail to cancellation, where
/// <see cref="Beta"/>'s continued fraction holds it. The mass itself is exponentiated from logs
/// for the same reason, since <c>n!</c> leaves a double at 171.
/// </remarks>
internal static class BinomialTail
{
    /// <summary>The probability of exactly <paramref name="k"/> successes in <paramref name="n"/> trials.</summary>
    internal static double Mass(int k, int n, double p)
    {
        if (k < 0 || k > n)
        {
            return 0.0;
        }

        // S1244: the two endpoints of the probability's own range, where the logarithms below
        // are undefined and the mass is exactly one or zero rather than nearly so.
#pragma warning disable S1244
        if (p == 0.0)
        {
            return k == 0 ? 1.0 : 0.0;
        }
        if (p == 1.0)
        {
            return k == n ? 1.0 : 0.0;
        }
#pragma warning restore S1244

        double logChoose = Gamma.LogGamma(n + 1.0)
            - Gamma.LogGamma(k + 1.0) - Gamma.LogGamma(n - k + 1.0);

        return Math.Exp(logChoose + (k * Math.Log(p)) + ((n - k) * (Gamma.LogOnePlusMinus(-p, 1.0 - p) - p)));
    }

    /// <summary>The probability of at most <paramref name="k"/> successes.</summary>
    internal static double Cumulative(int k, int n, double p)
    {
        if (k < 0)
        {
            return 0.0;
        }
        if (k >= n)
        {
            return 1.0;
        }

        return Beta.RegularizedIncomplete(n - k, k + 1.0, 1.0 - p, p);
    }

    /// <summary>The probability of at least <paramref name="k"/> successes.</summary>
    internal static double Survival(int k, int n, double p)
    {
        if (k <= 0)
        {
            return 1.0;
        }
        if (k > n)
        {
            return 0.0;
        }

        return Beta.RegularizedIncomplete(k, n - k + 1.0, p, 1.0 - p);
    }
}
