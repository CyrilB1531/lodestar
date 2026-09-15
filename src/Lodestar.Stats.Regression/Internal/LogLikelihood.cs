namespace Lodestar.Stats.Regression.Internal;

/// <summary>The fitted log-likelihood, which is what the AIC is computed from.</summary>
/// <remarks>
/// These are the reference's formulae rather than a rearrangement: an AIC compared across
/// libraries is only comparable if the constant terms agree, and the Poisson's <c>log(y!)</c>
/// is exactly such a term.
/// </remarks>
internal static class LogLikelihood
{
    /// <summary>The log-likelihood of one family's fitted mean against its response.</summary>
    /// <param name="family">The response distribution.</param>
    /// <param name="response">
    /// One value per row: 0 or 1 for <see cref="GlmFamily.Binomial"/>, a non-negative integer
    /// count for <see cref="GlmFamily.Poisson"/> (<see cref="GlmFamily.Poisson"/>'s own summary
    /// says so). The entry point is what refuses a response outside its family; this method
    /// trusts that refusal rather than repeating it.
    /// </param>
    /// <param name="mean">The fitted mean, one per row.</param>
    /// <param name="alpha">The negative binomial's dispersion; the other families ignore it.</param>
    public static double Of(GlmFamily family, ReadOnlySpan<double> response, double[] mean, double alpha)
    {
        // lnΓ(1/α) is the same in every row and costs a few dozen logarithms: read once, not per row (#781).
        double logGammaTheta = family == GlmFamily.NegativeBinomial ? LogGamma(1.0 / alpha) : 0.0;

        double total = 0.0;
        for (int row = 0; row < response.Length; row++)
        {
            double y = response[row];
            double mu = mean[row];
            total += family switch
            {
                GlmFamily.Binomial => (y * Math.Log(mu)) + ((1.0 - y) * Math.Log(1.0 - mu)),
                GlmFamily.Poisson => (y * Math.Log(mu)) - mu - LogFactorial(y),
                GlmFamily.NegativeBinomial => NegativeBinomialTerm(y, mu, alpha, logGammaTheta),
                _ => throw Families.Undeclared(family),
            };
        }

        return total;
    }

    /// <summary>One row of the negative binomial log-likelihood, term for term as <c>loglike_obs</c> writes it.</summary>
    private static double NegativeBinomialTerm(double y, double mu, double alpha, double logGammaTheta)
    {
        double theta = 1.0 / alpha;
        return (y * Math.Log(alpha * mu))
            - ((y + theta) * Math.Log(1.0 + (alpha * mu)))
            + LogGamma(y + theta)
            - logGammaTheta
            - LogFactorial(y);
    }

    /// <summary><c>lnΓ(x)</c> for <c>x &gt; 0</c>, which the negative binomial reads at non-integer arguments.</summary>
    /// <remarks>
    /// The argument is raised to 40 or more by <c>lnΓ(x) = lnΓ(x + n) − Σ log(x + k)</c>, then read off the
    /// Stirling series <see cref="LogFactorial"/> uses. Measured against <c>scipy.special.gammaln</c>, a shift
    /// to 20 leaves 4.6e-13 — the omitted <c>1/(1680x⁷)</c> term — and a shift to 40 under 4e-15;
    /// <c>regression_log_gamma.json</c> holds it (#769).
    /// </remarks>
    internal static double LogGamma(double x)
    {
        double shift = 0.0;
        while (x < 40.0)
        {
            shift += Math.Log(x);
            x += 1.0;
        }

        double inverse = 1.0 / x;
        double inverseSquared = inverse * inverse;
        double series = inverse * ((1.0 / 12.0) - (inverseSquared * ((1.0 / 360.0) - (inverseSquared / 1260.0))));
        return ((x - 0.5) * Math.Log(x)) - x + HalfLogTwoPi + series - shift;
    }

    /// <summary>Counts below this read <c>log(k!)</c> from <see cref="SmallLogFactorials"/>; the rest take Stirling's series.</summary>
    /// <remarks>
    /// Measured against <c>scipy.special.gammaln(k + 1)</c>, the series with three correction terms is
    /// off by 7.8e-7 at 2 and within 6.6e-16 from 20 on. 256 keeps every count a small-count fit sees
    /// on the table, so the doubles those fits returned do not move (#665).
    /// </remarks>
    internal const int TabulatedCounts = 256;

    /// <summary><c>log(k!)</c> for <c>k</c> below <see cref="TabulatedCounts"/>, built once.</summary>
    /// <remarks>
    /// The same device as <c>Lodestar.Metrics.Internal.ExpectedMutualInformation.LogFactorials</c>,
    /// whose own remark measured plain prefix sums at <c>8.4e-10</c> relative error at twenty
    /// thousand terms. The running sum below is Kahan-compensated instead, which holds the error near
    /// <c>eps * sum</c>; it was sized by the largest response until #665, which made it 8 MB at a million.
    /// </remarks>
    private static readonly double[] SmallLogFactorials = BuildSmallLogFactorials();

    private const double HalfLogTwoPi = 0.91893853320467274178;

    /// <summary><c>log(count!)</c> for a non-negative integer <paramref name="count"/>, in constant time and space.</summary>
    /// <remarks>
    /// <c>statsmodels</c> evaluates <c>gammaln(y + 1)</c>. Past the table, Stirling's series
    /// <c>n log n - n + log(2πn)/2 + 1/(12n) - 1/(360n³) + 1/(1260n⁵)</c>, whose next term is under 1e-20
    /// at 256; <c>regression_log_factorial.json</c> holds it to <c>gammaln</c> up to 2^53.
    /// </remarks>
    internal static double LogFactorial(double count)
    {
        if (count < TabulatedCounts)
        {
            return SmallLogFactorials[(int)count];
        }

        double inverse = 1.0 / count;
        double inverseSquared = inverse * inverse;
        double series = inverse * ((1.0 / 12.0) - (inverseSquared * ((1.0 / 360.0) - (inverseSquared / 1260.0))));
        return (count * Math.Log(count)) - count + HalfLogTwoPi + (0.5 * Math.Log(count)) + series;
    }

    private static double[] BuildSmallLogFactorials()
    {
        var table = new double[TabulatedCounts];
        double sum = 0.0;
        double compensation = 0.0;
        for (int k = 2; k < TabulatedCounts; k++)
        {
            double term = Math.Log(k) - compensation;
            double next = sum + term;
            compensation = (next - sum) - term;
            sum = next;
            table[k] = sum;
        }

        return table;
    }
}
