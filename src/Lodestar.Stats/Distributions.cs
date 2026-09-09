using Lodestar.Stats.Internal;

namespace Lodestar.Stats;

/// <summary>
/// The three tail probabilities a caller holding its own statistic needs, and the one
/// quantile that turns a confidence level into a bound.
/// </summary>
/// <remarks>
/// Published narrowly under decision 0095, which exercises the condition decision 0081 wrote
/// for itself. The log-gamma, the incomplete beta and gamma and the normal tail underneath
/// these stay internal.
/// </remarks>
public static class Distributions
{
    /// <summary>The upper tail of Student's <em>t</em>: <c>P(T &gt; t)</c>.</summary>
    /// <param name="t">The statistic.</param>
    /// <param name="df">Degrees of freedom; must be positive.</param>
    /// <returns><c>scipy.stats.t.sf(t, df)</c>.</returns>
    /// <exception cref="ArgumentOutOfRangeException"><paramref name="df"/> is not positive.</exception>
    /// <remarks>
    /// One tail. A two-sided p-value is <c>2 × StudentSf(|t|, df)</c>, which is what a
    /// regression table reports beside each coefficient.
    /// </remarks>
    public static double StudentSf(double t, double df)
    {
        RequirePositive(df, nameof(df));
        return Beta.StudentSf(t, df);
    }

    /// <summary>The value a Student's <em>t</em> falls below with probability <paramref name="p"/>.</summary>
    /// <param name="p">A probability strictly inside <c>(0, 1)</c>.</param>
    /// <param name="df">Degrees of freedom; must be positive.</param>
    /// <returns><c>scipy.stats.t.ppf(p, df)</c>.</returns>
    /// <exception cref="ArgumentOutOfRangeException"><paramref name="p"/> is not strictly inside <c>(0, 1)</c>, or <paramref name="df"/> is not positive.</exception>
    /// <remarks>
    /// A 95% interval takes <c>StudentQuantile(0.975, df)</c> as its multiplier; the endpoints
    /// are refused rather than answered with the two infinities.
    /// <strong>This is the quantile, not the inverse survival function</strong> — the internal
    /// helper solves <c>P(T &gt; x) = p</c> and carries the opposite sign, so the negation below
    /// is the distribution's symmetry rather than a correction. The reference page has why.
    /// </remarks>
    public static double StudentQuantile(double p, double df)
    {
        if (double.IsNaN(p) || p <= 0.0 || p >= 1.0)
        {
            throw new ArgumentOutOfRangeException(
                nameof(p), p, "A probability lies strictly inside (0, 1).");
        }

        RequirePositive(df, nameof(df));
        return -Beta.StudentQuantile(p, df);
    }

    /// <summary>The upper tail of the <em>F</em> distribution: <c>P(F &gt; f)</c>.</summary>
    /// <param name="f">The statistic.</param>
    /// <param name="numeratorDf">Numerator degrees of freedom; must be positive.</param>
    /// <param name="denominatorDf">Denominator degrees of freedom; must be positive.</param>
    /// <returns><c>scipy.stats.f.sf(f, dfn, dfd)</c>.</returns>
    /// <exception cref="ArgumentOutOfRangeException">Either degrees-of-freedom argument is not positive.</exception>
    /// <remarks>What a regression's overall significance test reports, and what one-way ANOVA already uses here.</remarks>
    public static double FisherSf(double f, double numeratorDf, double denominatorDf)
    {
        RequirePositive(numeratorDf, nameof(numeratorDf));
        RequirePositive(denominatorDf, nameof(denominatorDf));
        return Beta.FisherSf(f, numeratorDf, denominatorDf);
    }

    /// <summary>Degrees of freedom are counts of freedom, so zero and below are not values.</summary>
    /// <remarks>NaN is named rather than left to the comparison: it fails every ordering,
    /// so a plain <c>&lt;= 0</c> would let it through into the tail.</remarks>
    private static void RequirePositive(double value, string name)
    {
        if (double.IsNaN(value) || value <= 0.0)
        {
            throw new ArgumentOutOfRangeException(name, value, "Degrees of freedom must be positive.");
        }
    }
}
