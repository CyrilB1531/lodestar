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
        // The + 0.0 turns the median's -0 into the +0 scipy's t.ppf and NormalQuantile return (#1690's review).
        return Endpoint(p, double.NegativeInfinity, double.PositiveInfinity)
            ?? Unbounded(-Beta.StudentQuantile(p, df) + 0.0);
    }

    /// <summary>The former name of <see cref="FisherSnedecorSf"/>, which it calls.</summary>
    /// <param name="f">The statistic.</param>
    /// <param name="numeratorDf">Numerator degrees of freedom; must be positive and finite.</param>
    /// <param name="denominatorDf">Denominator degrees of freedom; must be positive and finite.</param>
    /// <returns><c>scipy.stats.f.sf(f, dfn, dfd)</c>.</returns>
    /// <exception cref="ArgumentOutOfRangeException">Either degrees-of-freedom argument is not positive, or is 5e-324, whose half is not a double.</exception>
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
    /// <exception cref="ArgumentOutOfRangeException">Either degrees-of-freedom argument is not positive, or is 5e-324, whose half is not a double.</exception>
    /// <remarks>What a regression's overall significance test reports, and what one-way ANOVA already uses here.</remarks>
    public static double FisherSnedecorSf(double f, double numeratorDf, double denominatorDf)
    {
        RequireShapeDfs(numeratorDf, denominatorDf);
        return Beta.FisherSnedecorSf(f, numeratorDf, denominatorDf);
    }

    /// <summary>The upper tail of the chi-squared distribution: <c>P(X &gt; x)</c>.</summary>
    /// <param name="x">The statistic; negative values are below the support and return one.</param>
    /// <param name="df">Degrees of freedom; must be positive and finite.</param>
    /// <returns><c>scipy.stats.chi2.sf(x, df)</c>.</returns>
    /// <exception cref="ArgumentOutOfRangeException"><paramref name="df"/> is not positive, or is 5e-324, whose half is not a double.</exception>
    /// <remarks>
    /// What a log-rank test reports, and what the chi-squared tests here already use
    /// internally. Published under decision 0003 on the same terms as the three above:
    /// one caller asked, and the layer underneath stays internal.
    /// </remarks>
    public static double ChiSquaredSf(double x, double df)
    {
        RequireShapeDf(df);
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
            ?? Unbounded(Beta.StudentQuantile(p, df));
    }

    /// <summary>The <em>F</em> density.</summary>
    /// <param name="f">The point; below the support the density is zero, and <c>NaN</c> answers <c>NaN</c>.</param>
    /// <param name="numeratorDf">Numerator degrees of freedom; must be positive and finite.</param>
    /// <param name="denominatorDf">Denominator degrees of freedom; must be positive and finite.</param>
    /// <returns><c>scipy.stats.f.pdf(f, dfn, dfd)</c>, and zero at <c>+∞</c>, the density's limit (decision 0010).</returns>
    /// <exception cref="ArgumentOutOfRangeException">Either degrees-of-freedom argument is not positive, or is 5e-324, whose half is not a double.</exception>
    public static double FisherSnedecorPdf(double f, double numeratorDf, double denominatorDf)
    {
        RequireShapeDfs(numeratorDf, denominatorDf);

        // At +∞ the density's limit is zero at every df; scipy's NaN past a numerator of two is its formula forming ∞/∞
        // (decision 0010).
        return Densities.Fisher(f, numeratorDf, denominatorDf);
    }

    /// <summary>The <em>F</em> lower tail: <c>P(F ≤ f)</c>.</summary>
    /// <param name="f">The statistic; <c>NaN</c> answers <c>NaN</c>.</param>
    /// <param name="numeratorDf">Numerator degrees of freedom; must be positive and finite.</param>
    /// <param name="denominatorDf">Denominator degrees of freedom; must be positive and finite.</param>
    /// <returns><c>scipy.stats.f.cdf(f, dfn, dfd)</c>, the incomplete beta on the lower side rather than one minus the tail.</returns>
    /// <exception cref="ArgumentOutOfRangeException">Either degrees-of-freedom argument is not positive, or is 5e-324, whose half is not a double.</exception>
    public static double FisherSnedecorCdf(double f, double numeratorDf, double denominatorDf)
    {
        RequireShapeDfs(numeratorDf, denominatorDf);
        return Beta.FisherSnedecorCdf(f, numeratorDf, denominatorDf);
    }

    /// <summary>The value an <em>F</em> falls below with probability <paramref name="p"/>.</summary>
    /// <param name="p">A probability in <c>[0, 1]</c>; the endpoints answer <c>0</c> and <c>+∞</c>.</param>
    /// <param name="numeratorDf">Numerator degrees of freedom; must be positive and finite.</param>
    /// <param name="denominatorDf">Denominator degrees of freedom; must be positive and finite.</param>
    /// <returns><c>scipy.stats.f.ppf(p, dfn, dfd)</c>.</returns>
    /// <exception cref="ArgumentOutOfRangeException"><paramref name="p"/> is outside <c>[0, 1]</c>, or a degrees-of-freedom argument is not positive, or is 5e-324, whose half is not a double.</exception>
    public static double FisherSnedecorQuantile(double p, double numeratorDf, double denominatorDf)
    {
        RequireProbability(p, nameof(p));
        RequireShapeDfs(numeratorDf, denominatorDf);
        return Endpoint(p, 0.0, double.PositiveInfinity)
            ?? FisherFromTail(p, lowerTail: true, numeratorDf, denominatorDf);
    }

    /// <summary>The <c>f</c> with <c>P(F &gt; f) = p</c>: the inverse of <see cref="FisherSnedecorSf"/>.</summary>
    /// <param name="p">A probability in <c>[0, 1]</c>; the endpoints answer <c>+∞</c> and <c>0</c>.</param>
    /// <param name="numeratorDf">Numerator degrees of freedom; must be positive and finite.</param>
    /// <param name="denominatorDf">Denominator degrees of freedom; must be positive and finite.</param>
    /// <returns><c>scipy.stats.f.isf(p, dfn, dfd)</c>.</returns>
    /// <exception cref="ArgumentOutOfRangeException"><paramref name="p"/> is outside <c>[0, 1]</c>, or a degrees-of-freedom argument is not positive, or is 5e-324, whose half is not a double.</exception>
    public static double FisherSnedecorIsf(double p, double numeratorDf, double denominatorDf)
    {
        RequireProbability(p, nameof(p));
        RequireShapeDfs(numeratorDf, denominatorDf);
        return Endpoint(p, double.PositiveInfinity, 0.0)
            ?? FisherFromTail(p, lowerTail: false, numeratorDf, denominatorDf);
    }

    /// <summary>The chi-squared density.</summary>
    /// <param name="x">The point; below the support the density is zero, and <c>NaN</c> answers <c>NaN</c>.</param>
    /// <param name="df">Degrees of freedom; must be positive and finite.</param>
    /// <returns><c>scipy.stats.chi2.pdf(x, df)</c>, and zero at <c>+∞</c>, the density's limit (decision 0010).</returns>
    /// <exception cref="ArgumentOutOfRangeException"><paramref name="df"/> is not positive, or is 5e-324, whose half is not a double.</exception>
    public static double ChiSquaredPdf(double x, double df)
    {
        RequireShapeDf(df);

        // At +∞ the density's limit is zero at every df; scipy's NaN past two is its formula forming ∞/∞ (decision 0010).
        return Densities.ChiSquared(x, df);
    }

    /// <summary>The chi-squared lower tail: <c>P(X ≤ x)</c>.</summary>
    /// <param name="x">The statistic; below the support the tail is zero, and <c>NaN</c> answers <c>NaN</c>.</param>
    /// <param name="df">Degrees of freedom; must be positive and finite.</param>
    /// <returns><c>scipy.stats.chi2.cdf(x, df)</c>, the regularized lower incomplete gamma.</returns>
    /// <exception cref="ArgumentOutOfRangeException"><paramref name="df"/> is not positive, or is 5e-324, whose half is not a double.</exception>
    public static double ChiSquaredCdf(double x, double df)
    {
        RequireShapeDf(df);
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
    /// <exception cref="ArgumentOutOfRangeException"><paramref name="p"/> is outside <c>[0, 1]</c>, or <paramref name="df"/> is not positive, or is 5e-324, whose half is not a double.</exception>
    public static double ChiSquaredQuantile(double p, double df)
    {
        RequireProbability(p, nameof(p));
        RequireShapeDf(df);
        return 2.0 * GammaQuantile.Invert(p, df / 2.0, upper: false);
    }

    /// <summary>The <c>x</c> with <c>P(X &gt; x) = p</c>: the inverse of <see cref="ChiSquaredSf"/>.</summary>
    /// <param name="p">A probability in <c>[0, 1]</c>; the endpoints answer <c>+∞</c> and <c>0</c>.</param>
    /// <param name="df">Degrees of freedom; must be positive and finite.</param>
    /// <returns><c>scipy.stats.chi2.isf(p, df)</c>.</returns>
    /// <exception cref="ArgumentOutOfRangeException"><paramref name="p"/> is outside <c>[0, 1]</c>, or <paramref name="df"/> is not positive, or is 5e-324, whose half is not a double.</exception>
    public static double ChiSquaredIsf(double p, double df)
    {
        RequireProbability(p, nameof(p));
        RequireShapeDf(df);
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
        var target = new FisherTarget(lowerTail ? p : 1.0 - p, lowerTail ? 1.0 - p : p, numeratorDf, denominatorDf);
        (double y, double complement) = BetaQuantile.InvertBoth(
            target.Lower, target.Upper, numeratorDf / 2.0, denominatorDf / 2.0);
        double f = denominatorDf * y / (numeratorDf * complement);
        f = target.AtCrossing(f);

        // A side below the smallest normal double keeps too few digits, and the half's limit inverts there instead,
        // kept where it holds the target closer: its leading term divides by a shape, ill-conditioned where it is flat.
        double miss = double.NaN;
        if (Beta.TakesLimit(y, complement))
        {
            miss = target.Miss(f);
            double fromLimit = Beta.FisherRootFromLimit(target.Lower, target.Upper, numeratorDf, denominatorDf);
            double limitMiss = double.IsNaN(fromLimit) ? double.PositiveInfinity : target.Miss(fromLimit);
            if (limitMiss <= miss)
            {
                (f, miss) = (fromLimit, limitMiss);
            }
        }

        // Below Beta.SmallShape a root outside the doubles leaves a non-root, and an end strictly past the target is the
        // answer (#1690's review); past it that end's tail is within e^-74 of its limit, so no larger shape needs a look.
        if (numeratorDf / 2.0 >= Beta.SmallShape && denominatorDf / 2.0 >= Beta.SmallShape)
        {
            return f;
        }
        miss = double.IsNaN(miss) ? target.Miss(f) : miss;
        if (miss <= 1e-9)
        {
            return f;
        }
        if (numeratorDf / 2.0 < Beta.SmallShape && target.RootBelow(double.Epsilon))
        {
            return 0.0;
        }
        return denominatorDf / 2.0 < Beta.SmallShape && target.RootAbove(double.MaxValue) ? double.PositiveInfinity : f;
    }

    /// <summary>An F quantile's target, judged on the smaller of its two tails, the one a double keeps whole.</summary>
    /// <remarks>
    /// The ends are f's, not the beta's: at small shapes 1 - y at f = MaxValue is far above the smallest double. Judged
    /// on the tail the caller passed, a target of 1 - 1e-12 hid a miss of 1e-12 and FisherSnedecorIsf(1 - 1e-12, 0.001,
    /// 1e-300) read 0 for +inf (#1690's review).
    /// </remarks>
    private readonly struct FisherTarget(double lower, double upper, double numeratorDf, double denominatorDf)
    {
        // A log-F standard deviation of 1e-8, squared: below it f's last ulps move a deep tail by more than 1e-6, as at
        // (1.15e-271, 1.44e24, 5.39e23), 0.6% off when the walk began at 1e-12 (#1690's review).
        private const double NarrowLogFSquared = 1e-16;

        // The variance per unit of z^2 under which an ulp moves the tail by 1e-10: z eps / sd > 1e-10.
        private const double DeepTailVariance = 4.9e-12;

        // Its bound at the smallest tail, z^2 = 2 * 745: past it no log is taken, as at any ordinary df.
        private const double DeepTailCeiling = 7.3e-9;

        internal double Lower { get; } = lower;

        internal double Upper { get; } = upper;

        /// <summary>The relative distance of the kept tail at <paramref name="f"/> from its target.</summary>
        internal double Miss(double f) => Math.Abs(Kept(f) - KeptTarget) / KeptTarget;

        /// <summary>Whether the root lies strictly below <paramref name="f"/>: its kept tail there already past the target.</summary>
        internal bool RootBelow(double f) => Lower <= Upper ? Kept(f) > KeptTarget : Kept(f) < KeptTarget;

        /// <summary>Whether the root lies strictly above <paramref name="f"/>; a tail equal to the target there is a root,
        /// as at shapes 1e-310, where the tail at the largest double is one half to rounding.</summary>
        internal bool RootAbove(double f) => Lower <= Upper ? Kept(f) < KeptTarget : Kept(f) > KeptTarget;

        /// <summary>The f at which F's own tail crosses the target, walked to by up to eight ulps each way, where the law is narrow.</summary>
        /// <remarks>
        /// f = d₂y/(d₁(1 − y)) rounds three times, so the root in y lands up to four ulps of f from where the tail
        /// crosses, which a law narrower than 1e-12 turns into its whole tail: FisherSnedecorQuantile(0.144, 4.7e118,
        /// 4.5e282) read 0.9999999999999998, whose lower tail is 0 (#1690's review). log F's variance is 2/d1 + 2/d2.
        /// </remarks>
        internal double AtCrossing(double f)
        {
            // An ulp of f moves the kept tail by about z eps / sd, z^2 about -2 log of the tail: deep in it a law of width
            // 1e-6 already moved 3e-9 an ulp, past the 1e-9 Miss accepts, at (2e-55, 1.5e12, 9.6e204) (#1690's review).
            double variance = (2.0 / numeratorDf) + (2.0 / denominatorDf);
            if (variance >= DeepTailCeiling
                || (variance >= NarrowLogFSquared && variance >= -2.0 * Math.Log(KeptTarget) * DeepTailVariance))
            {
                return f;
            }
            for (int i = 0; i < 8 && RootBelow(f); i++)
            {
                f = TailInversion.NextDown(f);
            }
            for (int i = 0; i < 8 && RootAbove(f); i++)
            {
                f = TailInversion.NextUp(f);
            }
            return f;
        }

        private double KeptTarget => Math.Min(Lower, Upper);

        private double Kept(double f) => Lower <= Upper
            ? Beta.FisherSnedecorCdf(f, numeratorDf, denominatorDf)
            : Beta.FisherSnedecorSf(f, numeratorDf, denominatorDf);
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

    /// <summary>A df the chi-squared members take half of as a shape: positive, finite, and halvable.</summary>
    private static void RequireShapeDf(double df)
    {
        RequirePositive(df, nameof(df));
        RequireHalvable(df, nameof(df));
    }

    /// <summary>The F members' two dfs, both checked positive before either is checked halvable, 724c7912's order.</summary>
    private static void RequireShapeDfs(double numeratorDf, double denominatorDf)
    {
        RequirePositive(numeratorDf, nameof(numeratorDf));
        RequirePositive(denominatorDf, nameof(denominatorDf));
        RequireHalvable(numeratorDf, nameof(numeratorDf));
        RequireHalvable(denominatorDf, nameof(denominatorDf));
    }

    /// <summary>A positive df whose half is a double: chi-squared and F take <c>df / 2</c> as a shape.</summary>
    /// <remarks>Half of 5e-324 rounds to zero, which the shapes refused from a private parameter (#1690's review); Student's
    /// tail is one half there in closed form and needs no shape.</remarks>
    private static void RequireHalvable(double value, string name)
    {
        // S1244: the one positive double whose half rounds to zero.
#pragma warning disable S1244
        if (value / 2.0 == 0.0)
#pragma warning restore S1244
        {
            throw new ArgumentOutOfRangeException(name, value, "Degrees of freedom must be at least 1e-323, whose half is a double.");
        }
    }
}
