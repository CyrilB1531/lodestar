using Lodestar.Stats.Internal;

namespace Lodestar.Stats;

/// <summary>
/// The density, the two tails and their inverses of the standard normal, Student's <em>t</em>,
/// <em>F</em> and chi-squared laws, as <c>scipy.stats</c> gives them.
/// </summary>
/// <remarks>
/// <c>Pdf</c>, <c>Cdf</c>, <c>Sf</c>, <c>Quantile</c> (scipy's <c>ppf</c>) and <c>Isf</c> for each
/// law, twenty members (#1158). Each tail is computed on the side that keeps full relative
/// precision, so a cdf of 1e-300 is not <c>1 − sf</c>. The log-gamma, the incomplete beta and
/// gamma and their inverses underneath stay internal.
/// </remarks>
public static class Distributions
{
    /// <summary>The upper tail of Student's <em>t</em>: <c>P(T &gt; t)</c>.</summary>
    /// <param name="t">The statistic.</param>
    /// <param name="df">Degrees of freedom; must be positive, and <c>+∞</c> is the standard normal law.</param>
    /// <returns><c>scipy.stats.t.sf(t, df)</c>.</returns>
    /// <exception cref="ArgumentOutOfRangeException"><paramref name="df"/> is not positive.</exception>
    /// <remarks>
    /// One tail. A two-sided p-value is <c>2 × StudentSf(|t|, df)</c>, which is what a
    /// regression table reports beside each coefficient.
    /// </remarks>
    public static double StudentSf(double t, double df)
    {
        if (double.IsPositiveInfinity(df))
        {
            return NormalSf(t);
        }
        RequirePositive(df, nameof(df));
        return Beta.StudentSf(t, df);
    }

    /// <summary>The value a Student's <em>t</em> falls below with probability <paramref name="p"/>.</summary>
    /// <param name="p">A probability in <c>[0, 1]</c>.</param>
    /// <param name="df">Degrees of freedom; must be positive, and <c>+∞</c> is the standard normal law.</param>
    /// <returns><c>scipy.stats.t.ppf(p, df)</c>.</returns>
    /// <exception cref="ArgumentOutOfRangeException"><paramref name="p"/> is outside <c>[0, 1]</c>, or <paramref name="df"/> is not positive.</exception>
    /// <remarks>
    /// A 95% interval takes <c>StudentQuantile(0.975, df)</c> as its multiplier; the endpoints
    /// answer the support's ends, <c>-∞</c> and <c>+∞</c>, as scipy's do (#1158).
    /// <strong>This is the quantile, not the inverse survival function</strong> — the internal
    /// helper solves <c>P(T &gt; x) = p</c> and carries the opposite sign, so the negation below
    /// is the distribution's symmetry rather than a correction. The reference page has why.
    /// </remarks>
    public static double StudentQuantile(double p, double df)
    {
        if (double.IsPositiveInfinity(df))
        {
            return NormalQuantile(p);
        }
        RequireProbability(p, nameof(p));
        RequirePositive(df, nameof(df));
        return Endpoint(p, double.NegativeInfinity, double.PositiveInfinity)
            ?? Unbounded(-Beta.StudentQuantile(p, df));
    }

    /// <summary>The upper tail of the <em>F</em> distribution: <c>P(F &gt; f)</c>.</summary>
    /// <param name="f">The statistic.</param>
    /// <param name="numeratorDf">Numerator degrees of freedom; must be positive and finite.</param>
    /// <param name="denominatorDf">Denominator degrees of freedom; must be positive and finite.</param>
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
    /// <param name="df">Degrees of freedom; must be positive and finite.</param>
    /// <returns><c>scipy.stats.chi2.sf(x, df)</c>.</returns>
    /// <exception cref="ArgumentOutOfRangeException"><paramref name="df"/> is not positive.</exception>
    /// <remarks>
    /// What a log-rank test reports, and what the chi-squared tests here already use
    /// internally. Published under decision 0003 on the same terms as the three above:
    /// one caller asked, and the layer underneath stays internal.
    /// </remarks>
    public static double ChiSquaredSf(double x, double df)
    {
        RequirePositive(df, nameof(df));
        // Below the support the tail is the whole mass. The regularized Q underneath
        // validates its own argument and would throw on a negative rather than say one.
        if (double.IsNaN(x))
        {
            return double.NaN;
        }
        return x <= 0.0 ? 1.0 : Gamma.RegularizedQ(df / 2.0, x / 2.0);
    }

    /// <summary>The value a standard normal falls below with probability <paramref name="p"/>.</summary>
    /// <param name="p">A probability in <c>[0, 1]</c>; the endpoints answer <c>-∞</c> and <c>+∞</c>.</param>
    /// <returns><c>scipy.stats.norm.ppf(p)</c>.</returns>
    /// <exception cref="ArgumentOutOfRangeException"><paramref name="p"/> is outside <c>[0, 1]</c>.</exception>
    /// <remarks>
    /// The multiplier a large-sample confidence interval takes, where a Student one would
    /// need degrees of freedom it does not have: <c>NormalQuantile(0.975)</c> is 1.959963…
    /// <strong>This is the quantile, not the inverse survival function</strong> — the
    /// internal helper solves <c>P(Z &gt; z) = p</c> and carries the opposite sign, so the
    /// negation below is the distribution's symmetry rather than a correction, exactly as
    /// <see cref="StudentQuantile"/> does. Published under decision 0003.
    /// </remarks>
    public static double NormalQuantile(double p)
    {
        RequireProbability(p, nameof(p));
        // The + 0.0 is not redundant: the internal helper answers exactly zero at the
        // median, and negating it would hand a caller -0 from a published method.
        return Endpoint(p, double.NegativeInfinity, double.PositiveInfinity) ?? (-Normal.Quantile(p) + 0.0);
    }

    /// <summary>The standard normal density.</summary>
    /// <param name="z">The point; <c>NaN</c> answers <c>NaN</c>.</param>
    /// <returns><c>scipy.stats.norm.pdf(z)</c>.</returns>
    public static double NormalPdf(double z) => Math.Exp(-0.5 * z * z) / Math.Sqrt(2.0 * Math.PI);

    /// <summary>The standard normal's lower tail: <c>P(Z ≤ z)</c>.</summary>
    /// <param name="z">The point; <c>NaN</c> answers <c>NaN</c>.</param>
    /// <returns><c>scipy.stats.norm.cdf(z)</c>, as the upper tail of <c>−z</c>, which keeps the lower tail's digits.</returns>
    public static double NormalCdf(double z) => Normal.Sf(-z);

    /// <summary>The standard normal's upper tail: <c>P(Z &gt; z)</c>.</summary>
    /// <param name="z">The point; <c>NaN</c> answers <c>NaN</c>.</param>
    /// <returns><c>scipy.stats.norm.sf(z)</c>.</returns>
    public static double NormalSf(double z) => Normal.Sf(z);

    /// <summary>The <c>z</c> with <c>P(Z &gt; z) = p</c>: the inverse of <see cref="NormalSf"/>.</summary>
    /// <param name="p">A probability in <c>[0, 1]</c>; the endpoints answer <c>+∞</c> and <c>-∞</c>.</param>
    /// <returns><c>scipy.stats.norm.isf(p)</c>.</returns>
    /// <exception cref="ArgumentOutOfRangeException"><paramref name="p"/> is outside <c>[0, 1]</c>.</exception>
    public static double NormalIsf(double p)
    {
        RequireProbability(p, nameof(p));
        return Endpoint(p, double.PositiveInfinity, double.NegativeInfinity) ?? (Normal.Quantile(p) + 0.0);
    }

    /// <summary>Student's <em>t</em> density.</summary>
    /// <param name="t">The point; <c>NaN</c> answers <c>NaN</c>.</param>
    /// <param name="df">Degrees of freedom; must be positive, and <c>+∞</c> is the standard normal law.</param>
    /// <returns><c>scipy.stats.t.pdf(t, df)</c>.</returns>
    /// <exception cref="ArgumentOutOfRangeException"><paramref name="df"/> is not positive.</exception>
    public static double StudentPdf(double t, double df)
    {
        if (double.IsPositiveInfinity(df))
        {
            return NormalPdf(t);
        }
        RequirePositive(df, nameof(df));
        return Densities.Student(t, df);
    }

    /// <summary>Student's <em>t</em> lower tail: <c>P(T ≤ t)</c>.</summary>
    /// <param name="t">The statistic; <c>NaN</c> answers <c>NaN</c>.</param>
    /// <param name="df">Degrees of freedom; must be positive, and <c>+∞</c> is the standard normal law.</param>
    /// <returns><c>scipy.stats.t.cdf(t, df)</c>, as the upper tail of <c>−t</c>.</returns>
    /// <exception cref="ArgumentOutOfRangeException"><paramref name="df"/> is not positive.</exception>
    public static double StudentCdf(double t, double df)
    {
        if (double.IsPositiveInfinity(df))
        {
            return NormalCdf(t);
        }
        RequirePositive(df, nameof(df));
        return Beta.StudentSf(-t, df);
    }

    /// <summary>The <c>t</c> with <c>P(T &gt; t) = p</c>: the inverse of <see cref="StudentSf"/>.</summary>
    /// <param name="p">A probability in <c>[0, 1]</c>; the endpoints answer <c>+∞</c> and <c>-∞</c>.</param>
    /// <param name="df">Degrees of freedom; must be positive, and <c>+∞</c> is the standard normal law.</param>
    /// <returns><c>scipy.stats.t.isf(p, df)</c>.</returns>
    /// <exception cref="ArgumentOutOfRangeException"><paramref name="p"/> is outside <c>[0, 1]</c>, or <paramref name="df"/> is not positive.</exception>
    public static double StudentIsf(double p, double df)
    {
        if (double.IsPositiveInfinity(df))
        {
            return NormalIsf(p);
        }
        RequireProbability(p, nameof(p));
        RequirePositive(df, nameof(df));
        return Endpoint(p, double.PositiveInfinity, double.NegativeInfinity)
            ?? Unbounded(Beta.StudentQuantile(p, df));
    }

    /// <summary>The <em>F</em> density.</summary>
    /// <param name="f">The point; below the support the density is zero, and <c>NaN</c> answers <c>NaN</c>.</param>
    /// <param name="numeratorDf">Numerator degrees of freedom; must be positive and finite.</param>
    /// <param name="denominatorDf">Denominator degrees of freedom; must be positive and finite.</param>
    /// <returns><c>scipy.stats.f.pdf(f, dfn, dfd)</c>.</returns>
    /// <exception cref="ArgumentOutOfRangeException">Either degrees-of-freedom argument is not positive.</exception>
    public static double FisherPdf(double f, double numeratorDf, double denominatorDf)
    {
        RequirePositive(numeratorDf, nameof(numeratorDf));
        RequirePositive(denominatorDf, nameof(denominatorDf));
        return Densities.Fisher(f, numeratorDf, denominatorDf);
    }

    /// <summary>The <em>F</em> lower tail: <c>P(F ≤ f)</c>.</summary>
    /// <param name="f">The statistic; <c>NaN</c> answers <c>NaN</c>.</param>
    /// <param name="numeratorDf">Numerator degrees of freedom; must be positive and finite.</param>
    /// <param name="denominatorDf">Denominator degrees of freedom; must be positive and finite.</param>
    /// <returns><c>scipy.stats.f.cdf(f, dfn, dfd)</c>, the incomplete beta on the lower side rather than one minus the tail.</returns>
    /// <exception cref="ArgumentOutOfRangeException">Either degrees-of-freedom argument is not positive.</exception>
    public static double FisherCdf(double f, double numeratorDf, double denominatorDf)
    {
        RequirePositive(numeratorDf, nameof(numeratorDf));
        RequirePositive(denominatorDf, nameof(denominatorDf));
        if (double.IsNaN(f))
        {
            return double.NaN;
        }
        if (f <= 0.0)
        {
            return 0.0;
        }
        // Both halves exact: y can round to one while 1 - y still holds its digits, as at shapes
        // (1e10, 1), and the incomplete beta reads the side that keeps them.
        (double lower, double upper) = Beta.FisherSplit(f, numeratorDf, denominatorDf);
        return Beta.RegularizedIncomplete(numeratorDf / 2.0, denominatorDf / 2.0, lower, upper);
    }

    /// <summary>The value an <em>F</em> falls below with probability <paramref name="p"/>.</summary>
    /// <param name="p">A probability in <c>[0, 1]</c>; the endpoints answer <c>0</c> and <c>+∞</c>.</param>
    /// <param name="numeratorDf">Numerator degrees of freedom; must be positive and finite.</param>
    /// <param name="denominatorDf">Denominator degrees of freedom; must be positive and finite.</param>
    /// <returns><c>scipy.stats.f.ppf(p, dfn, dfd)</c>.</returns>
    /// <exception cref="ArgumentOutOfRangeException"><paramref name="p"/> is outside <c>[0, 1]</c>, or a degrees-of-freedom argument is not positive.</exception>
    public static double FisherQuantile(double p, double numeratorDf, double denominatorDf)
    {
        RequireProbability(p, nameof(p));
        RequirePositive(numeratorDf, nameof(numeratorDf));
        RequirePositive(denominatorDf, nameof(denominatorDf));
        return Endpoint(p, 0.0, double.PositiveInfinity)
            ?? FisherFromTail(p, lowerTail: true, numeratorDf, denominatorDf);
    }

    /// <summary>The <c>f</c> with <c>P(F &gt; f) = p</c>: the inverse of <see cref="FisherSf"/>.</summary>
    /// <param name="p">A probability in <c>[0, 1]</c>; the endpoints answer <c>+∞</c> and <c>0</c>.</param>
    /// <param name="numeratorDf">Numerator degrees of freedom; must be positive and finite.</param>
    /// <param name="denominatorDf">Denominator degrees of freedom; must be positive and finite.</param>
    /// <returns><c>scipy.stats.f.isf(p, dfn, dfd)</c>.</returns>
    /// <exception cref="ArgumentOutOfRangeException"><paramref name="p"/> is outside <c>[0, 1]</c>, or a degrees-of-freedom argument is not positive.</exception>
    public static double FisherIsf(double p, double numeratorDf, double denominatorDf)
    {
        RequireProbability(p, nameof(p));
        RequirePositive(numeratorDf, nameof(numeratorDf));
        RequirePositive(denominatorDf, nameof(denominatorDf));
        return Endpoint(p, double.PositiveInfinity, 0.0)
            ?? FisherFromTail(p, lowerTail: false, numeratorDf, denominatorDf);
    }

    /// <summary>The chi-squared density.</summary>
    /// <param name="x">The point; below the support the density is zero, and <c>NaN</c> answers <c>NaN</c>.</param>
    /// <param name="df">Degrees of freedom; must be positive and finite.</param>
    /// <returns><c>scipy.stats.chi2.pdf(x, df)</c>.</returns>
    /// <exception cref="ArgumentOutOfRangeException"><paramref name="df"/> is not positive.</exception>
    public static double ChiSquaredPdf(double x, double df)
    {
        RequirePositive(df, nameof(df));
        return Densities.ChiSquared(x, df);
    }

    /// <summary>The chi-squared lower tail: <c>P(X ≤ x)</c>.</summary>
    /// <param name="x">The statistic; below the support the tail is zero, and <c>NaN</c> answers <c>NaN</c>.</param>
    /// <param name="df">Degrees of freedom; must be positive and finite.</param>
    /// <returns><c>scipy.stats.chi2.cdf(x, df)</c>, the regularized lower incomplete gamma.</returns>
    /// <exception cref="ArgumentOutOfRangeException"><paramref name="df"/> is not positive.</exception>
    public static double ChiSquaredCdf(double x, double df)
    {
        RequirePositive(df, nameof(df));
        if (double.IsNaN(x))
        {
            return double.NaN;
        }
        return x <= 0.0 ? 0.0 : Gamma.RegularizedP(df / 2.0, x / 2.0);
    }

    /// <summary>The value a chi-squared falls below with probability <paramref name="p"/>.</summary>
    /// <param name="p">A probability in <c>[0, 1]</c>; the endpoints answer <c>0</c> and <c>+∞</c>.</param>
    /// <param name="df">Degrees of freedom; must be positive and finite.</param>
    /// <returns><c>scipy.stats.chi2.ppf(p, df)</c>.</returns>
    /// <exception cref="ArgumentOutOfRangeException"><paramref name="p"/> is outside <c>[0, 1]</c>, or <paramref name="df"/> is not positive.</exception>
    public static double ChiSquaredQuantile(double p, double df)
    {
        RequireProbability(p, nameof(p));
        RequirePositive(df, nameof(df));
        return 2.0 * GammaQuantile.Invert(p, df / 2.0, upper: false);
    }

    /// <summary>The <c>x</c> with <c>P(X &gt; x) = p</c>: the inverse of <see cref="ChiSquaredSf"/>.</summary>
    /// <param name="p">A probability in <c>[0, 1]</c>; the endpoints answer <c>+∞</c> and <c>0</c>.</param>
    /// <param name="df">Degrees of freedom; must be positive and finite.</param>
    /// <returns><c>scipy.stats.chi2.isf(p, df)</c>.</returns>
    /// <exception cref="ArgumentOutOfRangeException"><paramref name="p"/> is outside <c>[0, 1]</c>, or <paramref name="df"/> is not positive.</exception>
    public static double ChiSquaredIsf(double p, double df)
    {
        RequireProbability(p, nameof(p));
        RequirePositive(df, nameof(df));
        return 2.0 * GammaQuantile.Invert(p, df / 2.0, upper: true);
    }

    /// <summary>An <em>F</em> quantile from the incomplete beta's inverse, with <c>y</c> and <c>1 − y</c> both exact.</summary>
    /// <remarks>
    /// With <c>y = d₁f / (d₁f + d₂)</c>, <c>P(F ≤ f) = I_y(d₁/2, d₂/2)</c> and <c>f = d₂y / (d₁(1 − y))</c>.
    /// A root near one would leave <c>1 − y</c> as one minus a number near one, so both come from
    /// <see cref="BetaQuantile.InvertBoth"/>, which solves on whichever side is small.
    /// </remarks>
    private static double FisherFromTail(double p, bool lowerTail, double numeratorDf, double denominatorDf)
    {
        double lower = lowerTail ? p : 1.0 - p;
        double upper = lowerTail ? 1.0 - p : p;
        (double y, double complement) = BetaQuantile.InvertBoth(
            lower, upper, numeratorDf / 2.0, denominatorDf / 2.0);
        return denominatorDf * y / (numeratorDf * complement);
    }

    /// <summary>A quantile the inversion capped at the largest double, answered as the infinity it stands for.</summary>
    /// <remarks>
    /// The inversion's growth stops at <see cref="double.MaxValue"/>, so a root past it, such as the
    /// t quantile at <c>1e-300</c> on half a degree of freedom, near <c>1e600</c>, came back finite.
    /// </remarks>
    private static double Unbounded(double quantile)
    {
        if (Math.Abs(quantile) < double.MaxValue)
        {
            return quantile;
        }
        return quantile > 0.0 ? double.PositiveInfinity : double.NegativeInfinity;
    }

    /// <summary>The answer at a closed endpoint of <c>[0, 1]</c>, or <see langword="null"/> inside it.</summary>
    private static double? Endpoint(double p, double atZero, double atOne)
    {
        // S1244: the two closed endpoints the validation admits, answered as the support's ends.
#pragma warning disable S1244
        if (p == 0.0)
        {
            return atZero;
        }
        return p == 1.0 ? atOne : null;
#pragma warning restore S1244
    }

    /// <summary>A probability lies in the closed unit interval; the endpoints are answered, not refused.</summary>
    /// <remarks>Shared by every quantile so none can drift from the others on what it accepts.</remarks>
    private static void RequireProbability(double value, string name)
    {
        if (double.IsNaN(value) || value < 0.0 || value > 1.0)
        {
            throw new ArgumentOutOfRangeException(name, value, "A probability lies in [0, 1].");
        }
    }

    /// <summary>Degrees of freedom are counts of freedom, so zero and below are not values, nor is infinity.</summary>
    /// <remarks>NaN is named rather than left to the comparison: it fails every ordering,
    /// so a plain <c>&lt;= 0</c> would let it through into the tail. An infinite count is the
    /// normal law for Student's t, which each Student member answers before reaching this, and
    /// has no meaning scipy gives a value to for F and chi-squared.</remarks>
    private static void RequirePositive(double value, string name)
    {
        if (double.IsNaN(value) || value <= 0.0 || double.IsPositiveInfinity(value))
        {
            throw new ArgumentOutOfRangeException(name, value, "Degrees of freedom must be positive and finite.");
        }
    }
}
