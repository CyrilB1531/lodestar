using Lodestar.Stats.Internal;

namespace Lodestar.Stats;

/// <summary>
/// The four tail probabilities a caller holding its own statistic needs, and the two
/// quantiles that turn a confidence level into a bound.
/// </summary>
/// <remarks>
/// Published narrowly under decision 0095, which exercises the condition decision 0081 wrote
/// for itself. The incomplete beta and gamma and the normal tail underneath these stay
/// internal; log-gamma joined the published members for a Poisson log-likelihood (#616).
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
        RequireProbability(p, nameof(p));
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

    /// <summary>The upper tail of the chi-squared distribution: <c>P(X &gt; x)</c>.</summary>
    /// <param name="x">The statistic; negative values are below the support and return one.</param>
    /// <param name="df">Degrees of freedom; must be positive.</param>
    /// <returns><c>scipy.stats.chi2.sf(x, df)</c>.</returns>
    /// <exception cref="ArgumentOutOfRangeException"><paramref name="df"/> is not positive.</exception>
    /// <remarks>
    /// What a log-rank test reports, and what the chi-squared tests here already use
    /// internally. Published under decision 0097 on the same terms as the three above:
    /// one caller asked, and the layer underneath stays internal.
    /// </remarks>
    public static double ChiSquaredSf(double x, double df)
    {
        RequirePositive(df, nameof(df));
        // Below the support the tail is the whole mass. The regularized Q underneath
        // validates its own argument and would throw on a negative rather than say one.
        return x <= 0.0 ? 1.0 : Gamma.RegularizedQ(df / 2.0, x / 2.0);
    }

    /// <summary>The value a standard normal falls below with probability <paramref name="p"/>.</summary>
    /// <param name="p">A probability strictly inside <c>(0, 1)</c>.</param>
    /// <returns><c>scipy.stats.norm.ppf(p)</c>.</returns>
    /// <exception cref="ArgumentOutOfRangeException"><paramref name="p"/> is not strictly inside <c>(0, 1)</c>.</exception>
    /// <remarks>
    /// The multiplier a large-sample confidence interval takes, where a Student one would
    /// need degrees of freedom it does not have: <c>NormalQuantile(0.975)</c> is 1.959963…
    /// <strong>This is the quantile, not the inverse survival function</strong> — the
    /// internal helper solves <c>P(Z &gt; z) = p</c> and carries the opposite sign, so the
    /// negation below is the distribution's symmetry rather than a correction, exactly as
    /// <see cref="StudentQuantile"/> does. Published under decision 0098.
    /// </remarks>
    public static double NormalQuantile(double p)
    {
        RequireProbability(p, nameof(p));
        // The + 0.0 is not redundant: the internal helper answers exactly zero at the
        // median, and negating it would hand a caller -0 from a published method.
        return -Normal.Quantile(p) + 0.0;
    }

    /// <summary>The natural log of the gamma function.</summary>
    /// <param name="x">A positive argument.</param>
    /// <returns><c>log Γ(x)</c>.</returns>
    /// <exception cref="ArgumentOutOfRangeException"><paramref name="x"/> is not positive.</exception>
    /// <remarks>
    /// Published for <c>Lodestar.Stats.Regression</c>'s Poisson log-likelihood, whose AIC needs
    /// <c>log Γ(y + 1)</c> and therefore this member (#616). Decision 0095 listed it among the
    /// internals that stay so while nothing had asked; 0081's asymmetry is why asking is what
    /// changes it.
    /// </remarks>
    public static double LogGamma(double x) => Internal.Gamma.LogGamma(x);

    /// <summary>A probability is strictly inside the unit interval; the endpoints are refused.</summary>
    /// <remarks>Shared by the two quantiles so the two cannot drift apart on what they accept.</remarks>
    private static void RequireProbability(double value, string name)
    {
        if (double.IsNaN(value) || value <= 0.0 || value >= 1.0)
        {
            throw new ArgumentOutOfRangeException(
                name, value, "A probability lies strictly inside (0, 1).");
        }
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
