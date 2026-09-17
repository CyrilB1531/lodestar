namespace Lodestar.Metrics.Internal;

/// <summary>
/// The mutual information two labellings would share by chance alone, given their
/// cluster sizes — scikit-learn's <c>expected_mutual_information</c>.
/// </summary>
/// <remarks>
/// A sum over the hypergeometric distribution of every cell the marginals allow,
/// which is what <see cref="AdjustedMutualInformation"/> subtracts to correct for
/// chance. The grouping is the reference's term for term, early return included —
/// the reason <see cref="Contingency.MutualInformation"/> gives about its own.
/// </remarks>
internal static class ExpectedMutualInformation
{
    /// <summary>The expected mutual information of two labellings with these marginals.</summary>
    public static double Compute(int[] rows, int[] columns, int samples)
    {
        // Zero entropy on either side means chance shares nothing: the reference
        // returns exactly 0.0 here rather than reaching it by cancellation.
        if (samples == 0 || rows.Length <= 1 || columns.Length <= 1)
        {
            return 0.0;
        }

        double[] logFactorial = LogFactorials(samples);
        double logSamples = Math.Log(samples);
        double logFactorialSamples = logFactorial[samples];
        double emi = 0.0;

        // What the innermost loop reads per cell count, taken out of it: the same operands in the
        // same order give the same doubles, and a table read replaces a Math.Log per iteration.
        int largest = Math.Min(Max(rows), Max(columns));
        double[] logSamplesPlusLogNij = new double[largest + 1];
        double[] fractionOfSamples = new double[largest + 1];
        double[] logFactorialNijPlusSamples = new double[largest + 1];
        for (int nij = 1; nij <= largest; nij++)
        {
            logSamplesPlusLogNij[nij] = logSamples + Math.Log(nij);
            fractionOfSamples[nij] = nij / (double)samples;
            logFactorialNijPlusSamples[nij] = logFactorial[nij] + logFactorialSamples;
        }

        // Every (class, cluster) pair, not only the non-empty cells: a cell that is
        // empty here still has a chance of being filled, which is the whole quantity.
        for (int i = 0; i < rows.Length; i++)
        {
            int a = rows[i];
            double logA = Math.Log(a);
            for (int j = 0; j < columns.Length; j++)
            {
                int b = columns[j];
                double logAB = logA + Math.Log(b);
                double marginals =
                    logFactorial[a] + logFactorial[b] +
                    logFactorial[samples - a] + logFactorial[samples - b];

                int start = Math.Max(1, a + b - samples);
                int end = Math.Min(a, b);
                for (int nij = start; nij <= end; nij++)
                {
                    double term2 = logSamplesPlusLogNij[nij] - logAB;
                    double gln =
                        marginals -
                        logFactorialNijPlusSamples[nij] -
                        logFactorial[a - nij] - logFactorial[b - nij] -
                        logFactorial[samples - a - b + nij];

                    emi += fractionOfSamples[nij] * term2 * Math.Exp(gln);
                }
            }
        }

        return emi;
    }

    private static int Max(int[] counts)
    {
        int max = 0;
        foreach (int count in counts)
        {
            max = Math.Max(max, count);
        }

        return max;
    }

    /// <summary>
    /// <c>log(k!)</c> for every <c>k</c> up to <paramref name="samples"/>.
    /// </summary>
    /// <remarks>
    /// Every argument the sum above needs is <c>gammaln(k + 1)</c> for an integer <c>k</c>, so a
    /// cumulative table of logarithms answers all of them with no series approximation. It is
    /// not free of error: the prefix sums accumulate, reaching <c>8.4e-10</c> relative at twenty
    /// thousand samples and <c>2.0e-8</c> at two hundred thousand, where a real <c>gammaln</c>
    /// holds ~<c>1e-16</c>. Cheaper here, worse there.
    /// </remarks>
    private static double[] LogFactorials(int samples)
    {
        double[] table = new double[samples + 1];
        double running = 0.0;
        for (int k = 2; k <= samples; k++)
        {
            running += Math.Log(k);
            table[k] = running;
        }

        return table;
    }
}
