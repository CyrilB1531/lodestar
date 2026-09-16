using Aglomera;
using Lodestar.Cluster;

namespace Lodestar.Text.Benchmarks;

/// <summary>The agreement the agglomerative benchmark requires before anything is timed.</summary>
/// <remarks>
/// Node ids are arbitrary across libraries, so the tree is compared by what does not depend on them:
/// every merge height in merge order, and the partition the tree gives at the cut, numbered by first
/// appearance on both sides. <c>bench/README.md</c> section 15 is the rule.
/// </remarks>
internal static class AgglomerativeAgreement
{
    private const double Tolerance = 1e-9;

    public static void RequireSameTree(
        AgglomerativeClustering ours, ClusteringResult<AgglomeraPoint> theirs, Linkage linkage, int clusterCount)
    {
        // Level k holds the clustering after k merges, so level 0 is every sample alone.
        for (int merge = 0; merge < ours.Distances.Count; merge++)
        {
            double height = theirs[merge + 1].Dissimilarity;

            // Aglomera's Ward reports the rise in the sum of squares, d²/2 for the reference's d.
            double expected = linkage == Linkage.Ward
                ? ours.Distances[merge] * ours.Distances[merge] / 2.0
                : ours.Distances[merge];
            if (Math.Abs(expected - height) > Tolerance * Math.Max(1.0, Math.Abs(expected)))
            {
                throw new InvalidOperationException(
                    $"Aglomera merges at {height} where this package merges at {expected} (merge {merge}, "
                    + $"{linkage}). Timing two different trees measures nothing.");
            }
        }

        int[] mine = FirstAppearance(ours.Labels);
        int[] other = FirstAppearance(Partition(theirs[ours.Distances.Count + 1 - clusterCount], ours.Labels.Count));
        if (!mine.SequenceEqual(other))
        {
            throw new InvalidOperationException(
                $"Aglomera's {clusterCount} clusters are not this package's ({linkage}).");
        }
    }

    private static int[] Partition(ClusterSet<AgglomeraPoint> level, int rows)
    {
        var labels = new int[rows];
        for (int cluster = 0; cluster < level.Count; cluster++)
        {
            foreach (AgglomeraPoint point in level[cluster])
            {
                labels[point.Row] = cluster;
            }
        }

        return labels;
    }

    private static int[] FirstAppearance(IReadOnlyList<int> labels)
    {
        var seen = new Dictionary<int, int>();
        var renumbered = new int[labels.Count];
        for (int row = 0; row < labels.Count; row++)
        {
            if (!seen.TryGetValue(labels[row], out int label))
            {
                label = seen.Count;
                seen.Add(labels[row], label);
            }

            renumbered[row] = label;
        }

        return renumbered;
    }
}
