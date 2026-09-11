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
    public static double Of(GlmFamily family, ReadOnlySpan<double> response, double[] mean)
    {
        double[] logFactorial = family == GlmFamily.Poisson
            ? LogFactorials(response)
            : Array.Empty<double>();

        double total = 0.0;
        for (int row = 0; row < response.Length; row++)
        {
            double y = response[row];
            double mu = mean[row];
            total += family switch
            {
                GlmFamily.Binomial => (y * Math.Log(mu)) + ((1.0 - y) * Math.Log(1.0 - mu)),
                GlmFamily.Poisson => (y * Math.Log(mu)) - mu - logFactorial[(int)y],
                _ => throw new ArgumentOutOfRangeException(nameof(family), family, null),
            };
        }

        return total;
    }

    /// <summary><c>log(k!)</c> for every <c>k</c> up to the largest response, indexed by <c>k</c>.</summary>
    /// <remarks>
    /// The same device as <c>Lodestar.Metrics.Internal.ExpectedMutualInformation.LogFactorials</c>,
    /// whose own remark measured plain prefix sums at <c>8.4e-10</c> relative error at twenty
    /// thousand terms and <c>2.0e-8</c> at two hundred thousand. The oracle here compares at
    /// <c>1e-9</c> absolute, so the running sum below is Kahan-compensated instead, which holds
    /// the error near <c>eps * sum</c> regardless of how far the table runs.
    /// </remarks>
    private static double[] LogFactorials(ReadOnlySpan<double> response)
    {
        int max = 0;
        foreach (double y in response)
        {
            max = Math.Max(max, (int)y);
        }

        var table = new double[max + 1];
        double sum = 0.0;
        double compensation = 0.0;
        for (int k = 2; k <= max; k++)
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
