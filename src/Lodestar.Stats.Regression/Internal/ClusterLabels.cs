namespace Lodestar.Stats.Regression.Internal;

/// <summary>Cluster labels renumbered from zero, and how many clusters they name.</summary>
/// <remarks>
/// The reference makes its groups dense through <c>np.unique</c>; only which rows share a label matters, so the
/// first-seen order here gives the same sums.
/// </remarks>
internal sealed class ClusterLabels
{
    private ClusterLabels(int[] labels, int count)
    {
        Labels = labels;
        Count = count;
    }

    /// <summary>One label per row, from zero to <see cref="Count"/> − 1.</summary>
    public int[] Labels { get; }

    /// <summary>How many distinct clusters the rows fall in.</summary>
    public int Count { get; }

    /// <summary>Renumbers the caller's labels, refusing a count that does not match the rows or a single cluster.</summary>
    /// <param name="clusters">The caller's labels.</param>
    /// <param name="rowCount">Rows in the design.</param>
    /// <exception cref="ArgumentException">The lengths differ, or every row shares one cluster.</exception>
    public static ClusterLabels Dense(ReadOnlySpan<int> clusters, int rowCount)
    {
        if (clusters.Length != rowCount)
        {
            throw new ArgumentException(
                $"design has {rowCount} rows and clusters holds {clusters.Length} labels.", nameof(clusters));
        }

        var seen = new Dictionary<int, int>();
        var labels = new int[rowCount];
        for (int row = 0; row < rowCount; row++)
        {
            if (!seen.TryGetValue(clusters[row], out int dense))
            {
                dense = seen.Count;
                seen.Add(clusters[row], dense);
            }

            labels[row] = dense;
        }

        if (seen.Count < 2)
        {
            // The correction divides by G − 1, and one cluster leaves the filling a single outer product of rank one.
            throw new ArgumentException(
                "Every row falls in one cluster; a cluster-robust covariance needs at least two.", nameof(clusters));
        }

        return new ClusterLabels(labels, seen.Count);
    }
}
