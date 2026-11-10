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

    /// <summary>Renumbers the caller's labels, refusing a single cluster; every caller judged their length first.</summary>
    /// <param name="clusters">The caller's labels.</param>
    /// <param name="rowCount">Rows in the design.</param>
    /// <exception cref="ArgumentException">Every row shares one cluster.</exception>
    public static ClusterLabels Dense(ReadOnlySpan<int> clusters, int rowCount)
    {
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

    /// <summary>Refuses a label count that is not the row count, the shape refusal a fit makes before reading values.</summary>
    /// <exception cref="ArgumentException">The lengths differ.</exception>
    public static void RequireLength(int labelCount, int rowCount)
    {
        if (labelCount != rowCount)
        {
            // CA2208, S3928: "clusters" is the public parameter of every fit this check serves.
#pragma warning disable CA2208, S3928
            throw new ArgumentException($"design has {rowCount} rows and clusters holds {labelCount} labels.", "clusters");
#pragma warning restore CA2208, S3928
        }
    }
}
