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
        (int[] degree, List<int> from, List<int> to) = Pairs(samples, featureCount, sampleCount, limit);
        return Adjacency(degree, from, to);
    }

    /// <summary>Every pair within the squared radius, each once, with every row's neighbourhood size.</summary>
    /// <remarks>
    /// Sorted along the widest feature, a row's candidates end at the first whose gap on that feature alone
    /// squares past the limit: the full sum is never below one of its terms, so no pair inside is dropped.
    /// </remarks>
    private static (int[] Degree, List<int> From, List<int> To) Pairs(
        ReadOnlySpan<double> samples, int featureCount, int sampleCount, double limit)
    {
        int axis = WidestFeature(samples, featureCount, sampleCount);
        var order = new int[sampleCount];
        var keys = new double[sampleCount];
        for (int i = 0; i < sampleCount; i++)
        {
            order[i] = i;
            keys[i] = samples[(i * featureCount) + axis];
        }

        Array.Sort(keys, order);

        // The rows copied in that order, so a row's candidates are read contiguously rather than scattered.
        var sorted = new double[samples.Length];
        for (int position = 0; position < sampleCount; position++)
        {
            samples.Slice(order[position] * featureCount, featureCount).CopyTo(sorted.AsSpan(position * featureCount));
        }

        var degree = new int[sampleCount];
        var from = new List<int>(sampleCount * 2);
        var to = new List<int>(sampleCount * 2);
        for (int position = 0; position < sampleCount; position++)
        {
            int i = order[position];
            degree[i]++;
            for (int next = position + 1; next < sampleCount; next++)
            {
                double gap = keys[next] - keys[position];
                if (gap * gap > limit)
                {
                    break;
                }

                if (Within(sorted, featureCount, position, next, limit))
                {
                    int j = order[next];
                    from.Add(i);
                    to.Add(j);
                    degree[i]++;
                    degree[j]++;
                }
            }
        }

        return (degree, from, to);
    }

    /// <summary>The pairs as one flat adjacency, each row itself first, then its neighbours as the scan found them.</summary>
    /// <remarks>
    /// Not ascending, and no label depends on it: <see cref="Growth"/> grows each cluster whole before starting the
    /// next. A weighted density sums in this order, as scikit-learn's tree search sums in its own (#1163).
    /// </remarks>
    private static (int[] Offsets, int[] Indices) Adjacency(int[] degree, List<int> from, List<int> to)
    {
        int sampleCount = degree.Length;
        var offsets = new int[sampleCount + 1];
        for (int i = 0; i < sampleCount; i++)
        {
            offsets[i + 1] = offsets[i] + degree[i];
        }

        var fill = (int[])offsets.Clone();
        var unordered = new int[offsets[sampleCount]];
        for (int i = 0; i < sampleCount; i++)
        {
            unordered[fill[i]++] = i;
        }

        for (int pair = 0; pair < from.Count; pair++)
        {
            int i = from[pair];
            int j = to[pair];
            unordered[fill[i]++] = j;
            unordered[fill[j]++] = i;
        }

        return (offsets, unordered);
    }

    /// <summary>The feature whose values spread widest, the one along which sorting prunes the most pairs.</summary>
    private static int WidestFeature(ReadOnlySpan<double> samples, int featureCount, int sampleCount)
    {
        int widest = 0;
        double widestRange = -1.0;
        for (int feature = 0; feature < featureCount; feature++)
        {
            double low = double.PositiveInfinity;
            double high = double.NegativeInfinity;
            for (int i = 0; i < sampleCount; i++)
            {
                double value = samples[(i * featureCount) + feature];
                low = Math.Min(low, value);
                high = Math.Max(high, value);
            }

            if (high - low > widestRange)
            {
                widestRange = high - low;
                widest = feature;
            }
        }

        return widest;
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
