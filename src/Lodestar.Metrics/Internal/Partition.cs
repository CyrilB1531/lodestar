namespace Lodestar.Metrics.Internal;

/// <summary>
/// What the three internal-validity metrics share: reading a label vector as a
/// partition, and agreeing that a feature block really is <c>n × featureCount</c>.
/// </summary>
/// <remarks>
/// Extracted rather than copied a third time when Calinski-Harabasz and
/// Davies-Bouldin arrived (#192): all three refuse the same label counts, with the
/// same sentence, and a second copy of that bound is a second place for it to drift.
/// </remarks>
internal static class Partition
{
    /// <summary>Cluster sizes, plus a dense ordinal per sample in first-seen order.</summary>
    /// <param name="labels">One cluster label per sample; any integers, not necessarily contiguous.</param>
    /// <param name="ordinals">Filled with each sample's index into the returned sizes.</param>
    /// <param name="clusters">How many distinct labels occur.</param>
    public static int[] Sizes(ReadOnlySpan<int> labels, out int[] ordinals, out int clusters)
    {
        ordinals = new int[labels.Length];
        clusters = 0;
        if (labels.Length > 0 && TryDirectRange(labels, out int min, out int range))
        {
            // Ordinal + 1 per label value, 0 while unseen: the same first-seen ordinals the
            // dictionary below assigns, with an array read where it hashes.
            int[] slots = new int[range];
            for (int i = 0; i < labels.Length; i++)
            {
                int slot = labels[i] - min;
                int ordinal = slots[slot] - 1;
                if (ordinal < 0)
                {
                    ordinal = clusters++;
                    slots[slot] = ordinal + 1;
                }

                ordinals[i] = ordinal;
            }
        }
        else
        {
            Dictionary<int, int> known = [];
            for (int i = 0; i < labels.Length; i++)
            {
                if (!known.TryGetValue(labels[i], out int ordinal))
                {
                    ordinal = clusters++;
                    known[labels[i]] = ordinal;
                }

                ordinals[i] = ordinal;
            }
        }

        int[] counts = new int[clusters];
        foreach (int ordinal in ordinals)
        {
            counts[ordinal]++;
        }

        return counts;
    }

    /// <summary>Whether the labels span few enough values for a table indexed by value.</summary>
    /// <remarks>The bound is <see cref="LabelIndex"/>'s: at most four slots per sample, plus a fixed allowance.</remarks>
    private static bool TryDirectRange(ReadOnlySpan<int> labels, out int min, out int range)
    {
        min = labels[0];
        int max = labels[0];
        foreach (int label in labels)
        {
            min = Math.Min(min, label);
            max = Math.Max(max, label);
        }

        long span = (long)max - min + 1;
        range = (int)Math.Min(span, int.MaxValue);
        return span <= (4L * labels.Length) + 1024;
    }

    /// <summary>scikit-learn's own bound on how many clusters a validity score can read.</summary>
    /// <remarks>
    /// One cluster leaves nothing to compare against, and one cluster per sample
    /// leaves nothing inside one. Measured on 1.9.1: silhouette, Calinski-Harabasz
    /// and Davies-Bouldin refuse both, with the sentence reproduced here.
    /// </remarks>
    /// <exception cref="ArgumentException">The count is outside <c>[2, samples - 1]</c>.</exception>
    public static void RequireScorableCount(int clusters, int samples, string parameterName)
    {
        if (clusters < 2 || clusters > samples - 1)
        {
            throw new ArgumentException(
                $"Number of labels is {clusters}. Valid values are 2 to n_samples - 1 (inclusive)",
                parameterName);
        }
    }

    /// <summary>Checks a feature block against its labels and returns the sample count.</summary>
    /// <exception cref="ArgumentOutOfRangeException"><paramref name="featureCount"/> is not positive.</exception>
    /// <exception cref="ArgumentException">The block is not <c>labels.Length × featureCount</c>.</exception>
    public static int Samples(ReadOnlySpan<int> labels, ReadOnlySpan<double> features, int featureCount)
    {
        if (featureCount <= 0)
        {
            throw new ArgumentOutOfRangeException(
                nameof(featureCount), featureCount, "featureCount must be positive.");
        }

        if (features.Length != labels.Length * featureCount)
        {
            throw new ArgumentException(
                $"features holds {features.Length} values, which is not {labels.Length} samples " +
                $"of {featureCount}.",
                nameof(features));
        }

        return labels.Length;
    }

    /// <summary>Each cluster's centroid, and the centroid of everything, row-major.</summary>
    /// <param name="features">The samples, row-major.</param>
    /// <param name="featureCount">How many values each sample holds.</param>
    /// <param name="ordinals">Each sample's cluster ordinal.</param>
    /// <param name="sizes">How many samples each cluster holds.</param>
    /// <param name="overall">Filled with the mean of every sample.</param>
    public static double[] Centroids(
        ReadOnlySpan<double> features, int featureCount, int[] ordinals, int[] sizes, out double[] overall)
    {
        int clusters = sizes.Length;
        var centroids = new double[clusters * featureCount];
        overall = new double[featureCount];

        for (int sample = 0; sample < ordinals.Length; sample++)
        {
            int at = ordinals[sample] * featureCount;
            int from = sample * featureCount;
            for (int f = 0; f < featureCount; f++)
            {
                centroids[at + f] += features[from + f];
                overall[f] += features[from + f];
            }
        }

        for (int cluster = 0; cluster < clusters; cluster++)
        {
            int at = cluster * featureCount;
            for (int f = 0; f < featureCount; f++)
            {
                centroids[at + f] /= sizes[cluster];
            }
        }

        for (int f = 0; f < featureCount; f++)
        {
            overall[f] /= ordinals.Length;
        }

        return centroids;
    }

    /// <summary>The euclidean distance between two rows of two row-major blocks.</summary>
    public static double Euclidean(
        ReadOnlySpan<double> left, int leftRow, ReadOnlySpan<double> right, int rightRow, int featureCount)
    {
        double total = 0.0;
        int a = leftRow * featureCount;
        int b = rightRow * featureCount;
        for (int f = 0; f < featureCount; f++)
        {
            double delta = left[a + f] - right[b + f];
            total += delta * delta;
        }

        return Math.Sqrt(total);
    }
}
