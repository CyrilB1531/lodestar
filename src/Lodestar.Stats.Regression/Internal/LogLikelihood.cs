using Lodestar.Stats;

namespace Lodestar.Stats.Regression.Internal;

/// <summary>The fitted log-likelihood, which is what the AIC is computed from.</summary>
/// <remarks>
/// These are the reference's formulae rather than a rearrangement: an AIC compared across
/// libraries is only comparable if the constant terms agree, and the Poisson's <c>log(y!)</c>
/// is exactly such a term.
/// </remarks>
internal static class LogLikelihood
{
    public static double Of(GlmFamily family, ReadOnlySpan<double> response, double[] mean)
    {
        double total = 0.0;
        for (int row = 0; row < response.Length; row++)
        {
            double y = response[row];
            double mu = mean[row];
            total += family switch
            {
                GlmFamily.Binomial => (y * Math.Log(mu)) + ((1.0 - y) * Math.Log(1.0 - mu)),
                GlmFamily.Poisson => (y * Math.Log(mu)) - mu - Distributions.LogGamma(y + 1.0),
                _ => throw new ArgumentOutOfRangeException(nameof(family), family, null),
            };
        }

        return total;
    }
}
