using System.ComponentModel;
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
    /// <returns><c>scipy.stats.t.sf(t, df)</c>; from <c>df = 1e20</c> the normal law's tail, and to <c>df = 1e-12</c> its first
    /// order, <c>½ − (df/2) asinh(t/√df)</c>, within 1.3e-19 of the law and one half at every finite <c>t</c> to df 1e-20.</returns>
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
    /// <returns><c>scipy.stats.t.ppf(p, df)</c>; from <c>df = 1e20</c> the normal quantile, and to <c>df = 1e-20</c> <c>-∞</c>
    /// below the median and <c>+∞</c> above it, the tail being one half at every double.</returns>
    /// <exception cref="ArgumentOutOfRangeException"><paramref name="p"/> is outside <c>[0, 1]</c>, or <paramref name="df"/> is not positive.</exception>
    /// <remarks>
    /// A 95% interval takes <c>StudentQuantile(0.975, df)</c> as its multiplier; the endpoints
    /// answer the support's ends, <c>-∞</c> and <c>+∞</c>, as scipy's do (#1158).
    /// <strong>This is the quantile, not the inverse survival function</strong>: the smallest
    /// double whose lower tail reaches <paramref name="p"/>, judged on the smaller tail, where
    /// <see cref="StudentIsf"/> is the smallest whose upper tail falls to it, one ulp apart where no
    /// double's tail is p. The reference page has why, and its exceptions.
    /// </remarks>
    public static double StudentQuantile(double p, double df)
    {
        if (double.IsPositiveInfinity(df))
        {
            return NormalQuantile(p);
        }
        RequireProbability(p, nameof(p));
        RequirePositive(df, nameof(df));
        // The + 0.0 turns the median's -0 into the +0 scipy's t.ppf and NormalQuantile return (#1690's review).
        return Endpoint(p, double.NegativeInfinity, double.PositiveInfinity)
            ?? StudentWithinTheDoubles(Beta.StudentLowerRoot(p, df) + 0.0, p, df, upper: false);
    }

    /// <summary>The former name of <see cref="FisherSnedecorSf"/>, which it calls.</summary>
    /// <param name="f">The statistic.</param>
    /// <param name="numeratorDf">Numerator degrees of freedom; must be positive and finite.</param>
    /// <param name="denominatorDf">Denominator degrees of freedom; must be positive and finite.</param>
    /// <returns><c>scipy.stats.f.sf(f, dfn, dfd)</c>.</returns>
    /// <exception cref="ArgumentOutOfRangeException">Either degrees-of-freedom argument is not positive.</exception>
    /// <remarks>
    /// Kept for binaries compiled against 0.5.0 — <c>Lodestar.Stats.Regression</c> 0.2 among them — until their floors
    /// move past this release (#1217).
    /// </remarks>
    // S1133: the removal is scheduled, not forgotten — when Lodestar.Stats.Regression's floor passes this release.
#pragma warning disable S1133
    [Obsolete("Renamed FisherSnedecorSf, so that \"Fisher\" names Fisher's exact test alone (#1217).")]
#pragma warning restore S1133
    [EditorBrowsable(EditorBrowsableState.Never)]
    public static double FisherSf(double f, double numeratorDf, double denominatorDf) =>
        FisherSnedecorSf(f, numeratorDf, denominatorDf);

    /// <summary>The upper tail of the <em>F</em> distribution: <c>P(F &gt; f)</c>.</summary>
    /// <param name="f">The statistic.</param>
    /// <param name="numeratorDf">Numerator degrees of freedom; must be positive and finite.</param>
    /// <param name="denominatorDf">Denominator degrees of freedom; must be positive and finite.</param>
    /// <returns><c>scipy.stats.f.sf(f, dfn, dfd)</c>.</returns>
    /// <exception cref="ArgumentOutOfRangeException">Either degrees-of-freedom argument is not positive.</exception>
    /// <remarks>What a regression's overall significance test reports, and what one-way ANOVA already uses here.</remarks>
    public static double FisherSnedecorSf(double f, double numeratorDf, double denominatorDf)
    {
        RequirePositiveDfs(numeratorDf, denominatorDf);
        return Beta.FisherSnedecorSf(f, numeratorDf, denominatorDf);
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
        if (x <= 0.0)
        {
            return 1.0;
        }
        return Gamma.ChiSquaredTail(df, x, upper: true);
    }

    /// <summary>The value a standard normal falls below with probability <paramref name="p"/>.</summary>
    /// <param name="p">A probability in <c>[0, 1]</c>; the endpoints answer <c>-∞</c> and <c>+∞</c>.</param>
    /// <returns><c>scipy.stats.norm.ppf(p)</c>.</returns>
    /// <exception cref="ArgumentOutOfRangeException"><paramref name="p"/> is outside <c>[0, 1]</c>.</exception>
    /// <remarks>
    /// The multiplier a large-sample confidence interval takes, where a Student one would
    /// need degrees of freedom it does not have: <c>NormalQuantile(0.975)</c> is 1.959963…
    /// It is <c>scipy.special.ndtri</c>'s Cephes approximation ported statement for statement, so the
    /// value is scipy's to the last bit where .NET's logarithm is glibc's, as on Linux, and to 1e-14
    /// relative elsewhere (#1684). Published under decision 0003.
    /// </remarks>
    public static double NormalQuantile(double p)
    {
        RequireProbability(p, nameof(p));
        return Endpoint(p, double.NegativeInfinity, double.PositiveInfinity) ?? Ndtri.Of(p);
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
        // scipy's norm.isf is -ndtri(p); the + 0.0 turns the median's -0 into the +0 scipy returns.
        return Endpoint(p, double.PositiveInfinity, double.NegativeInfinity) ?? (-Ndtri.Of(p) + 0.0);
    }

    /// <summary>Student's <em>t</em> density.</summary>
    /// <param name="t">The point; <c>NaN</c> answers <c>NaN</c>.</param>
    /// <param name="df">Degrees of freedom; must be positive, and <c>+∞</c> is the standard normal law.</param>
    /// <returns><c>scipy.stats.t.pdf(t, df)</c>; from <c>df = 1e20</c> the normal density. Where <c>t²/df</c> overflows scipy
    /// reads zero and this the density itself: <c>StudentPdf(1e140, 1e-30)</c> is 5e-171.</returns>
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
    /// <returns><c>scipy.stats.t.cdf(t, df)</c>, as the upper tail of <c>−t</c>, with <see cref="StudentSf"/>'s two limits.</returns>
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
    /// <returns><c>scipy.stats.t.isf(p, df)</c>; from <c>df = 1e20</c> the normal law's, and to <c>df = 1e-20</c> <c>+∞</c>
    /// below one half and <c>-∞</c> above it, the tail being one half at every double.</returns>
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
            ?? StudentWithinTheDoubles(Beta.StudentUpperRoot(p, df), p, df, upper: true);
    }

    /// <summary>The <em>F</em> density.</summary>
    /// <param name="f">The point; below the support the density is zero, and <c>NaN</c> answers <c>NaN</c>.</param>
    /// <param name="numeratorDf">Numerator degrees of freedom; must be positive and finite.</param>
    /// <param name="denominatorDf">Denominator degrees of freedom; must be positive and finite.</param>
    /// <returns><c>scipy.stats.f.pdf(f, dfn, dfd)</c>, and zero at <c>+∞</c>, the density's limit (decision 0010).</returns>
    /// <exception cref="ArgumentOutOfRangeException">Either degrees-of-freedom argument is not positive.</exception>
    public static double FisherSnedecorPdf(double f, double numeratorDf, double denominatorDf)
    {
        RequirePositiveDfs(numeratorDf, denominatorDf);

        // At +∞ the density's limit is zero at every df; scipy's NaN past a numerator of two is its formula forming ∞/∞
        // (decision 0010).
        return Densities.Fisher(f, numeratorDf, denominatorDf);
    }

    /// <summary>The <em>F</em> lower tail: <c>P(F ≤ f)</c>.</summary>
    /// <param name="f">The statistic; <c>NaN</c> answers <c>NaN</c>.</param>
    /// <param name="numeratorDf">Numerator degrees of freedom; must be positive and finite.</param>
    /// <param name="denominatorDf">Denominator degrees of freedom; must be positive and finite.</param>
    /// <returns><c>scipy.stats.f.cdf(f, dfn, dfd)</c>, the incomplete beta on the lower side rather than one minus the tail.</returns>
    /// <exception cref="ArgumentOutOfRangeException">Either degrees-of-freedom argument is not positive.</exception>
    public static double FisherSnedecorCdf(double f, double numeratorDf, double denominatorDf)
    {
        RequirePositiveDfs(numeratorDf, denominatorDf);
        return Beta.FisherSnedecorCdf(f, numeratorDf, denominatorDf);
    }

    /// <summary>The value an <em>F</em> falls below with probability <paramref name="p"/>.</summary>
    /// <param name="p">A probability in <c>[0, 1]</c>; the endpoints answer <c>0</c> and <c>+∞</c>.</param>
    /// <param name="numeratorDf">Numerator degrees of freedom; must be positive and finite.</param>
    /// <param name="denominatorDf">Denominator degrees of freedom; must be positive and finite.</param>
    /// <returns><c>scipy.stats.f.ppf(p, dfn, dfd)</c>.</returns>
    /// <exception cref="ArgumentOutOfRangeException"><paramref name="p"/> is outside <c>[0, 1]</c>, or a degrees-of-freedom argument is not positive.</exception>
    public static double FisherSnedecorQuantile(double p, double numeratorDf, double denominatorDf)
    {
        RequireProbability(p, nameof(p));
        RequirePositiveDfs(numeratorDf, denominatorDf);
        return Endpoint(p, 0.0, double.PositiveInfinity)
            ?? FisherWithinTheDoubles(FisherQuantile.Invert(p, givenLower: true, numeratorDf, denominatorDf), p, givenLower: true, numeratorDf, denominatorDf);
    }

    /// <summary>The <c>f</c> with <c>P(F &gt; f) = p</c>: the inverse of <see cref="FisherSnedecorSf"/>.</summary>
    /// <param name="p">A probability in <c>[0, 1]</c>; the endpoints answer <c>+∞</c> and <c>0</c>.</param>
    /// <param name="numeratorDf">Numerator degrees of freedom; must be positive and finite.</param>
    /// <param name="denominatorDf">Denominator degrees of freedom; must be positive and finite.</param>
    /// <returns><c>scipy.stats.f.isf(p, dfn, dfd)</c>.</returns>
    /// <exception cref="ArgumentOutOfRangeException"><paramref name="p"/> is outside <c>[0, 1]</c>, or a degrees-of-freedom argument is not positive.</exception>
    public static double FisherSnedecorIsf(double p, double numeratorDf, double denominatorDf)
    {
        RequireProbability(p, nameof(p));
        RequirePositiveDfs(numeratorDf, denominatorDf);
        return Endpoint(p, double.PositiveInfinity, 0.0)
            ?? FisherWithinTheDoubles(FisherQuantile.Invert(p, givenLower: false, numeratorDf, denominatorDf), p, givenLower: false, numeratorDf, denominatorDf);
    }

    /// <summary>The chi-squared density.</summary>
    /// <param name="x">The point; below the support the density is zero, and <c>NaN</c> answers <c>NaN</c>.</param>
    /// <param name="df">Degrees of freedom; must be positive and finite.</param>
    /// <returns><c>scipy.stats.chi2.pdf(x, df)</c>, and zero at <c>+∞</c>, the density's limit (decision 0010).</returns>
    /// <exception cref="ArgumentOutOfRangeException"><paramref name="df"/> is not positive.</exception>
    public static double ChiSquaredPdf(double x, double df)
    {
        RequirePositive(df, nameof(df));

        // At +∞ the density's limit is zero at every df; scipy's NaN past two is its formula forming ∞/∞ (decision 0010).
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
        if (x <= 0.0)
        {
            return 0.0;
        }
        return Gamma.ChiSquaredTail(df, x, upper: false);
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
        return ChiSquaredRoot.Invert(p, df, upper: false);
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
        return ChiSquaredRoot.Invert(p, df, upper: true);
    }

    /// <summary>A Student quantile <paramref name="t"/>, or AtTheEnd's double where it is infinite: by symmetry the smaller
    /// tail at either end is the upper tail at the largest double; the caller's, Sf or Cdf at that end.</summary>
    private static double StudentWithinTheDoubles(double t, double p, double df, bool upper)
    {
        if (!double.IsInfinity(t))
        {
            return t;
        }
        double smaller = StudentSf(double.MaxValue, df);
        // The caller's own tail is that one, by symmetry, unless it is the far side's, Isf at −∞ or the quantile at +∞, which
        // is the lower tail at the largest double either way.
        double callers = upper == t < 0.0 ? StudentCdf(double.MaxValue, df) : smaller;
        return AtTheEnd(t, p, smaller, callers);
    }

    /// <summary>An F quantile <paramref name="f"/>, or AtTheEnd's double where it is +∞, from the tail FisherQuantile keeps,
    /// the smaller target's, the caller's at a tie, and the caller's own.</summary>
    private static double FisherWithinTheDoubles(double f, double p, bool givenLower, double numeratorDf, double denominatorDf)
    {
        if (!double.IsPositiveInfinity(f))
        {
            return f;
        }
        bool keepsLower = FisherQuantile.KeepsLower(givenLower ? p : 1.0 - p, givenLower ? 1.0 - p : p, givenLower);
        double kept = keepsLower
            ? FisherSnedecorCdf(double.MaxValue, numeratorDf, denominatorDf)
            : FisherSnedecorSf(double.MaxValue, numeratorDf, denominatorDf);
        // The caller's own tail read only where it is the other.
        double callers = kept;
        if (keepsLower != givenLower)
        {
            callers = givenLower
                ? FisherSnedecorCdf(double.MaxValue, numeratorDf, denominatorDf)
                : FisherSnedecorSf(double.MaxValue, numeratorDf, denominatorDf);
        }
        return AtTheEnd(f, p, kept, callers);
    }

    /// <summary>The largest double of <paramref name="root"/>'s sign where a tail there, as the public tails round it, is
    /// its target: the smaller tail the smaller of <paramref name="p"/> and one less it, or the caller's own p; else the
    /// infinite root (Cyril, 2026-10-29, #1746).</summary>
    /// <remarks>
    /// That tail rounds onto the target from short of it: judged before rounding, Isf(Sf(MaxValue, 0.5, 0.5)) answered +∞
    /// where main answered the largest double. Equality, not reaching: -MaxValue's tail reaches every target below it,
    /// whose root lies past the doubles (#1690). Either tail, so every round trip holds (Cyril, 2026-10-29): on the smaller
    /// alone Isf(Sf(MaxValue)) read +∞ where Sf there passes one half; near one the caller's rounds 1e-16 wide, so
    /// Q(1 − p) = −Q(p) can part there. Read only past the doubles, so no finite quantile pays for it.
    /// </remarks>
    private static double AtTheEnd(double root, double p, double smallerTail, double callersTail)
    {
        // S1244: the round trip's own doubles, compared bit for bit; a tail one ulp off is a root past the doubles.
#pragma warning disable S1244
        bool isTarget = smallerTail == Math.Min(p, 1.0 - p) || callersTail == p;
#pragma warning restore S1244
        if (!isTarget)
        {
            return root;
        }
        return root > 0.0 ? double.MaxValue : -double.MaxValue;
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

    /// <summary>The F members' two dfs, each positive, the numerator's refused first.</summary>
    private static void RequirePositiveDfs(double numeratorDf, double denominatorDf)
    {
        RequirePositive(numeratorDf, nameof(numeratorDf));
        RequirePositive(denominatorDf, nameof(denominatorDf));
    }
}
