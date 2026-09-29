namespace Lodestar.Metrics.Internal;

/// <summary>
/// <c>np.sum</c> and <c>np.average</c> over float64 as numpy takes them, pairwise, so an average scikit-learn
/// reports comes out with its own bits: the classification averages that end in <c>np.average</c> use these (#1586).
/// </summary>
internal static class NumpyAverage
{
    /// <summary><c>np.average(values)</c>: the pairwise sum over the count, as numpy takes it (#1586).</summary>
    public static double Mean(double[] values) => Sum(values) / values.Length;

    /// <summary>
    /// numpy's <c>pairwise_sum</c> over float64: in order below eight, eight running sums up to 128,
    /// halves past it. Totals of one <c>1e16</c>, six <c>1</c> and one <c>-1e16</c> sum to 0 in order and to 4 here.
    /// </summary>
    public static double Sum(ReadOnlySpan<double> values)
    {
        int count = values.Length;
        if (count < 8)
        {
            double plain = 0.0;
            foreach (double value in values)
            {
                plain += value;
            }

            return plain;
        }

        if (count <= 128)
        {
            Span<double> lanes = stackalloc double[8];
            values.Slice(0, 8).CopyTo(lanes);
            int i = 8;
            for (; i < count - (count % 8); i += 8)
            {
                for (int lane = 0; lane < 8; lane++)
                {
                    lanes[lane] += values[i + lane];
                }
            }

            double result = ((lanes[0] + lanes[1]) + (lanes[2] + lanes[3])) + ((lanes[4] + lanes[5]) + (lanes[6] + lanes[7]));
            for (; i < count; i++)
            {
                result += values[i];
            }

            return result;
        }

        int half = count / 2;
        half -= half % 8;
        return Sum(values.Slice(0, half)) + Sum(values.Slice(half));
    }

    /// <summary>
    /// <c>np.average(values, weights=weights)</c>, both sums pairwise as numpy takes them (#1586). Overwrites
    /// <paramref name="values"/> with the products: every caller reads it for the last time here.
    /// </summary>
    public static double Weighted(double[] values, double[] weights, string paramName)
    {
        for (int i = 0; i < values.Length; i++)
        {
            // _average_binary_score forces a zero-weighted score to 0, so its NaN never reaches the average (#1277).
#pragma warning disable S1244
            values[i] = weights[i] == 0.0 ? 0.0 : values[i] * weights[i];
#pragma warning restore S1244
        }

        double weightSum = Sum(weights);

        // One class alone pairs with nothing, and np.average refuses the empty weighting (#1566).
#pragma warning disable S1244
        if (weightSum == 0.0)
#pragma warning restore S1244
        {
            throw new ArgumentException("Weights sum to zero, can't be normalized.", paramName);
        }

        return Sum(values) / weightSum;
    }
}
