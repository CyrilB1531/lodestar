namespace Lodestar.Cluster.Internal;

/// <summary>Every sample's neighbours within a radius, as one flat adjacency.</summary>
/// <remarks>
/// Offsets and indices rather than an <c>int[][]</c>: the growth loop reads a row's
/// neighbours and never holds two, so the jagged array would be one allocation per sample
/// for no gain. It is the shape <c>CsrMatrix</c> already uses in this repository.
/// </remarks>
internal static class Neighbourhoods
{
    /// <summary>The neighbours of every row within <paramref name="epsilon"/>, by euclidean distance.</summary>
    /// <param name="samples">The samples, row-major.</param>
    /// <param name="featureCount">How many values each row carries.</param>
    /// <param name="sampleCount">How many rows <paramref name="samples"/> holds.</param>
    /// <param name="epsilon">The inclusive radius.</param>
    /// <remarks>
    /// Squared distances against a squared radius, so no square root is taken. That is
    /// exactness rather than speed: a sample at exactly <paramref name="epsilon"/> has to
    /// stay inside, and comparing <c>Math.Sqrt(d)</c> with the radius can round it out.
    /// </remarks>
    public static (int[] Offsets, int[] Indices) Euclidean(
        ReadOnlySpan<double> samples, int featureCount, int sampleCount, double epsilon)
    {
        double limit = epsilon * epsilon;

        // Each pair once: d(i, j) and d(j, i) are the same sum, so the upper triangle decides both, kept
        // in (i, j) order so the rows below come out ascending without a sort (#818).
        var degree = new int[sampleCount];
        var from = new List<int>(sampleCount * 2);
        var to = new List<int>(sampleCount * 2);
        for (int i = 0; i < sampleCount; i++)
        {
            degree[i]++;
            for (int j = i + 1; j < sampleCount; j++)
            {
                if (Within(samples, featureCount, i, j, limit))
                {
                    from.Add(i);
                    to.Add(j);
                    degree[i]++;
                    degree[j]++;
                }
            }
        }

        var offsets = new int[sampleCount + 1];
        for (int i = 0; i < sampleCount; i++)
        {
            offsets[i + 1] = offsets[i] + degree[i];
        }

        // Row i receives its smaller neighbours from earlier rows' pairs, then itself, then its
        // larger neighbours from its own pairs: ascending, as the full scan wrote it.
        var fill = (int[])offsets.Clone();
        var indices = new int[offsets[sampleCount]];
        int pair = 0;
        for (int i = 0; i < sampleCount; i++)
        {
            indices[fill[i]++] = i;
            for (; pair < from.Count && from[pair] == i; pair++)
            {
                int j = to[pair];
                indices[fill[i]++] = j;
                indices[fill[j]++] = i;
            }
        }

        return (offsets, indices);
    }

    /// <summary>The same adjacency, read off a square distance matrix instead of computed.</summary>
    /// <param name="distances">The pairwise distances, row-major and square.</param>
    /// <param name="sampleCount">The side of that matrix.</param>
    /// <param name="epsilon">The inclusive radius.</param>
    /// <remarks>
    /// The diagonal is a zero distance, so a sample is its own neighbour here exactly as it
    /// is under <see cref="Euclidean"/> — which is what keeps one meaning of
    /// <c>MinimumSamples</c> across both entry points.
    /// </remarks>
    public static (int[] Offsets, int[] Indices) Precomputed(
        ReadOnlySpan<double> distances, int sampleCount, double epsilon)
    {
        var offsets = new int[sampleCount + 1];
        var indices = new List<int>(sampleCount * 4);
        for (int row = 0; row < sampleCount; row++)
        {
            offsets[row] = indices.Count;
            int start = row * sampleCount;
            for (int other = 0; other < sampleCount; other++)
            {
                if (distances[start + other] <= epsilon)
                {
                    indices.Add(other);
                }
            }
        }

        offsets[sampleCount] = indices.Count;
        return (offsets, [.. indices]);
    }

    /// <summary>Whether two rows are within the squared radius, stopping once the sum is past it.</summary>
    /// <remarks>
    /// Every term is a square, so the partial sums never fall: once one exceeds the limit the
    /// whole sum would too, and the answer is the one the full sum gives.
    /// </remarks>
    private static bool Within(
        ReadOnlySpan<double> samples, int featureCount, int left, int right, double limit)
    {
        double total = 0.0;
        int a = left * featureCount;
        int b = right * featureCount;
        for (int feature = 0; feature < featureCount; feature++)
        {
            double gap = samples[a + feature] - samples[b + feature];
            total += gap * gap;
            if (total > limit)
            {
                return false;
            }
        }

        return total <= limit;
    }
}
