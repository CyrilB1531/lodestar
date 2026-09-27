namespace Lodestar.Stats.Internal;

/// <summary>The binomial mass and its two tails, read off the regularized incomplete beta.</summary>
/// <remarks>
/// A binomial tail is a beta integral — <c>sf(k) = I_p(k+1, n-k)</c> — so nothing here sums a
/// series: the sums would cost O(n) and lose the far tail to cancellation, where
/// <see cref="Beta"/>'s continued fraction holds it. The mass itself is exponentiated from logs
/// for the same reason, since <c>n!</c> leaves a double at 171, in the saddle-point form the
/// comment in <see cref="Mass"/> explains.
/// </remarks>
internal static class BinomialTail
{
    private const int StirlingSeriesFloor = 16;

    // |v| < 0.1 on the series branch, so each term shrinks a hundredfold: 1e-16 by the eighth.
    private const int DevianceTerms = 64;

    private static readonly double HalfLogTwoPi = 0.5 * Math.Log(2.0 * Math.PI);

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

        if (k == 0)
        {
            return Math.Exp(n * LogComplement(p));
        }
        if (k == n)
        {
            return Math.Exp(n * Math.Log(p));
        }

        // long-comment: why the mass is not three log-gammas. Past 1e8 trials each log-gamma
        // is near 2e9, so its last bits are 1e-7 of the mass, and the two-sided search compares
        // masses at 1 + 1e-7: Test(49_999_999, 100_000_000) answered 0.99992 where scipy has
        // 1.0 (#1247). Loader's saddle-point form (Fast and Accurate Computation of Binomial
        // Probabilities, 2000) cancels the large terms analytically, leaving Stirling's error
        // terms and two deviances, all small: 5.7e-11 from scipy's binom.pmf over 20,000
        // random cases up to 2e9 trials, where the log-gammas reached 1.5e-5.
        double logMass = StirlingError(n) - StirlingError(k) - StirlingError(n - k)
            - Deviance(k, n * p) - Deviance(n - k, n * (1.0 - p));

        return Math.Exp(logMass) * Math.Sqrt(n / (2.0 * Math.PI * k * (double)(n - k)));
    }

    // log(1 - p) through the same series Gamma keeps for log(1 + t) - t, exact near p = 0.
    private static double LogComplement(double p) => Gamma.LogOnePlusMinus(-p, 1.0 - p) - p;

    /// <summary><c>log(n!) - log(sqrt(2 pi n) (n/e)^n)</c>, what Stirling's formula leaves out.</summary>
    /// <remarks>
    /// Below 16 the factorial is an exact double, so the difference is taken directly; above,
    /// the asymptotic series, whose next term (691/360360 n^-11) is under 1e-16 there.
    /// </remarks>
    private static double StirlingError(int n)
    {
        if (n < StirlingSeriesFloor)
        {
            double factorial = 1.0;
            for (int i = 2; i <= n; i++)
            {
                factorial *= i;
            }

            return Math.Log(factorial) - ((n + 0.5) * Math.Log(n)) + n - HalfLogTwoPi;
        }

        double inverseSquare = 1.0 / ((double)n * n);
        return (1.0 / 12.0
            - ((1.0 / 360.0
                - ((1.0 / 1260.0
                    - ((1.0 / 1680.0 - (inverseSquare / 1188.0)) * inverseSquare)) * inverseSquare))
               * inverseSquare)) / n;
    }

    /// <summary><c>x log(x / mean) + mean - x</c>, without the cancellation when x is near the mean.</summary>
    /// <remarks>
    /// Near the mean the two terms nearly cancel, so the difference is expanded in
    /// <c>v = (x - mean) / (x + mean)</c>: <c>(x - mean) v + 2x sum v^(2j+1) / (2j+1)</c>.
    /// </remarks>
    private static double Deviance(double x, double mean)
    {
        if (Math.Abs(x - mean) >= 0.1 * (x + mean))
        {
            return (x * Math.Log(x / mean)) + mean - x;
        }

        double v = (x - mean) / (x + mean);
        double vSquared = v * v;
        double sum = (x - mean) * v;
        double term = 2.0 * x * v;
        for (int j = 1; j < DevianceTerms; j++)
        {
            term *= vSquared;
            double next = sum + (term / ((2 * j) + 1));
            // S1244: the series has converged when a term no longer moves the sum at all.
#pragma warning disable S1244
            if (next == sum)
#pragma warning restore S1244
            {
                return next;
            }

            sum = next;
        }

        return sum;
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
