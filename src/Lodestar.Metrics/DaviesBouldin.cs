using Lodestar.Metrics.Internal;

namespace Lodestar.Metrics;

/// <summary>
/// The Davies-Bouldin index — the equivalent of
/// <c>sklearn.metrics.davies_bouldin_score</c>.
/// </summary>
/// <remarks>
/// For each cluster, the worst ratio of "how spread these two are" to "how far apart
/// they sit", averaged over the clusters. **Lower is better**, unlike
/// <see cref="CalinskiHarabasz"/> and <see cref="Silhouette"/>, and <c>0</c> is the
/// floor.
/// </remarks>
public static class DaviesBouldin
{
    /// <summary>Scores a clustering from the samples themselves — <c>davies_bouldin_score(X, labels)</c>.</summary>
    /// <param name="labels">One cluster label per sample.</param>
    /// <param name="features">The samples, row-major: sample <c>i</c> occupies <c>featureCount</c> values from <c>i * featureCount</c>.</param>
    /// <param name="featureCount">How many values each sample holds.</param>
    /// <returns>The mean worst-case similarity between a cluster and any other, <c>0</c> or above. Lower is better.</returns>
    /// <remarks>
    /// No precomputed-distance form, for the reason
    /// <see cref="CalinskiHarabasz.Score"/> gives. Two clusters sharing a centroid
    /// contribute nothing rather than an infinity: the reference replaces a zero
    /// centroid distance with infinity before dividing, so the pair scores zero.
    /// </remarks>
    /// <exception cref="ArgumentException">The inputs disagree in length, a feature is not finite, or the number of distinct labels is outside <c>[2, n - 1]</c> or needs more centroid distances than one array holds (#1468).</exception>
    /// <exception cref="ArgumentOutOfRangeException"><paramref name="featureCount"/> is not positive.</exception>
    public static double Score(ReadOnlySpan<int> labels, ReadOnlySpan<double> features, int featureCount)
    {
        int samples = Partition.Samples(labels, features, featureCount);
        int[] sizes = Partition.Sizes(labels, out int[] ordinals, out int clusters);
        Partition.RequireScorableCount(clusters, samples, nameof(labels));

        double[] centroids = Partition.Centroids(features, featureCount, ordinals, sizes, out _);

        double[] spread = Spreads(features, featureCount, centroids, ordinals, sizes);

        // centroid_distances once, as the reference builds it, its diagonal filled with 0.
        // Bounded rather than wrapped in int past some 46,000 clusters (#1468).
        var apart = new double[TableLength.Of(clusters, clusters, nameof(labels))];
        for (int i = 0; i < clusters; i++)
        {
            for (int j = 0; j < clusters; j++)
            {
                if (i == j)
                {
                    continue;
                }

                apart[(i * clusters) + j] = Partition.Euclidean(centroids, i, centroids, j, featureCount);
            }
        }

        // davies_bouldin_score answers 0 when np.allclose(intra_dists, 0) or
        // np.allclose(centroid_distances, 0), at numpy's atol of 1e-8 (#1205).
        if (AllWithinTolerance(spread) || AllWithinTolerance(apart))
        {
            return 0.0;
        }

        double total = 0.0;
        for (int i = 0; i < clusters; i++)
        {
            total += WorstRatio(apart, spread, i);
        }

        return total / clusters;
    }

    /// <summary>Each cluster's mean distance from its own centroid: the "spread" half of every ratio.</summary>
    /// <remarks>Its own method, so the distance stays inlined in the loop over every sample.</remarks>
    private static double[] Spreads(
        ReadOnlySpan<double> features, int featureCount, double[] centroids, int[] ordinals, int[] sizes)
    {
        var spread = new double[sizes.Length];
        for (int sample = 0; sample < ordinals.Length; sample++)
        {
            int own = ordinals[sample];
            spread[own] += Partition.Euclidean(features, sample, centroids, own, featureCount);
        }

        for (int cluster = 0; cluster < spread.Length; cluster++)
        {
            spread[cluster] /= sizes[cluster];
        }

        return spread;
    }

    /// <summary>Cluster <paramref name="i"/>'s worst ratio of spread to separation against any other.</summary>
    private static double WorstRatio(double[] apart, double[] spread, int i)
    {
        int clusters = spread.Length;
        double worst = 0.0;
        for (int j = 0; j < clusters; j++)
        {
            if (i == j)
            {
                continue;
            }

            double distance = apart[(i * clusters) + j];

            // S1244: whether the two centroids coincide, which is what the reference
            // replaces with infinity before dividing -- the pair then contributes 0.
#pragma warning disable S1244
            double ratio = distance == 0.0 ? 0.0 : (spread[i] + spread[j]) / distance;
#pragma warning restore S1244
            if (ratio > worst)
            {
                worst = ratio;
            }
        }

        return worst;
    }

    // np.allclose's default absolute tolerance; the relative term vanishes against 0.
    private const double AllCloseTolerance = 1e-8;

    private static bool AllWithinTolerance(double[] values)
    {
        foreach (double value in values)
        {
            if (!(Math.Abs(value) <= AllCloseTolerance))
            {
                return false;
            }
        }
        return true;
    }
}
