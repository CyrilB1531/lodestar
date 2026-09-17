using Lodestar.Cluster.Internal;

namespace Lodestar.Cluster;

/// <summary>
/// Builds a merge tree bottom-up and cuts it, at
/// <c>sklearn.cluster.AgglomerativeClustering(metric="euclidean")</c> parity.
/// </summary>
/// <remarks>
/// Spans in, arrays out, row-major. The whole tree is always built — the reference builds it too
/// when there is no connectivity — so <see cref="Children"/> and <see cref="Distances"/> hold every
/// merge whatever the cut. Labels, children and heights match the reference exactly, ties included.
/// </remarks>
public sealed class AgglomerativeClustering
{
    private readonly int[] _labels;
    private readonly int[] _children;
    private readonly double[] _distances;

    private AgglomerativeClustering(
        int featureCount, int clusterCount, Linkage linkage, int[] labels, Dendrogram tree)
    {
        FeatureCount = featureCount;
        ClusterCount = clusterCount;
        Linkage = linkage;
        _labels = labels;
        _children = tree.Children;
        _distances = tree.Distances;
    }

    /// <summary>How many values each row of the fitted matrix carried.</summary>
    public int FeatureCount { get; }

    /// <summary>How many clusters the tree was cut into — scikit-learn's <c>n_clusters_</c>.</summary>
    public int ClusterCount { get; }

    /// <summary>How the distance between two clusters was measured.</summary>
    public Linkage Linkage { get; }

    /// <summary>The cluster each sample belongs to — scikit-learn's <c>labels_</c>.</summary>
    /// <remarks>
    /// Numbered as the reference numbers them, which is <strong>not</strong> by first appearance:
    /// a cluster's label is its position in the heap the cut walks, so <c>0, 1, 5, 6, 20</c> cut
    /// into three under complete linkage labels as <c>2, 2, 0, 0, 1</c>.
    /// </remarks>
    public IReadOnlyList<int> Labels => _labels;

    /// <summary>The two node ids each merge joins, row-major — scikit-learn's <c>children_</c>.</summary>
    /// <remarks>
    /// Row <c>k</c>, at indices <c>2k</c> and <c>2k + 1</c>, creates node <c>n + k</c>; an id below
    /// <c>n</c> is a sample. Ward, complete and average write each pair smaller id first; single
    /// linkage writes them in the order its spanning tree found them, as the reference does.
    /// </remarks>
    public IReadOnlyList<int> Children => _children;

    /// <summary>The height of each merge, in merge order — scikit-learn's <c>distances_</c>.</summary>
    public IReadOnlyList<double> Distances => _distances;

    /// <summary>Clusters a row-major sample matrix into a given number of clusters.</summary>
    /// <param name="samples">The samples, row-major: <paramref name="featureCount"/> values per row.</param>
    /// <param name="featureCount">How many values each row carries.</param>
    /// <param name="clusterCount">How many clusters to cut the tree into.</param>
    /// <param name="linkage">How the distance between two clusters is measured.</param>
    /// <returns>A fitted clustering.</returns>
    /// <exception cref="ArgumentOutOfRangeException"><paramref name="featureCount"/> or <paramref name="clusterCount"/> is not positive, <paramref name="clusterCount"/> exceeds the sample count, or <paramref name="linkage"/> is not a defined value.</exception>
    /// <exception cref="ArgumentException"><paramref name="samples"/> holds fewer than two rows, a partial one, or a value that is not finite.</exception>
    public static AgglomerativeClustering Fit(
        ReadOnlySpan<double> samples, int featureCount, int clusterCount, Linkage linkage = Linkage.Ward)
    {
        Guard.NotLessThan(featureCount, 1);
        Guard.NotLessThan(clusterCount, 1);
        Defined(linkage, nameof(linkage));

        int sampleCount = Rows(samples, featureCount);
        Finite.Require(samples, nameof(samples));
        if (clusterCount > sampleCount)
        {
            throw new ArgumentOutOfRangeException(
                nameof(clusterCount), clusterCount,
                $"Cannot cut {sampleCount} samples into more clusters than there are samples.");
        }

        return Cut(samples, featureCount, sampleCount, linkage, _ => clusterCount);
    }

    /// <summary>Clusters by cutting the tree at a height rather than at a count — <c>distance_threshold</c>.</summary>
    /// <param name="samples">The samples, row-major: <paramref name="featureCount"/> values per row.</param>
    /// <param name="featureCount">How many values each row carries.</param>
    /// <param name="distanceThreshold">The height at and above which a merge is not made.</param>
    /// <param name="linkage">How the distance between two clusters is measured.</param>
    /// <returns>A fitted clustering, whose <see cref="ClusterCount"/> says how many clusters the height left.</returns>
    /// <exception cref="ArgumentOutOfRangeException"><paramref name="featureCount"/> is not positive, <paramref name="distanceThreshold"/> is negative, infinite or not a number, or <paramref name="linkage"/> is not a defined value.</exception>
    /// <exception cref="ArgumentException"><paramref name="samples"/> holds fewer than two rows, a partial one, or a value that is not finite.</exception>
    /// <remarks>
    /// <strong>The threshold is exclusive</strong>: a merge exactly at it is not made, so the cluster
    /// count is one more than the number of merges at or above it. Zero is allowed and leaves every
    /// sample its own cluster.
    /// </remarks>
    public static AgglomerativeClustering FitToThreshold(
        ReadOnlySpan<double> samples, int featureCount, double distanceThreshold, Linkage linkage = Linkage.Ward)
    {
        Guard.NotLessThan(featureCount, 1);
        Defined(linkage, nameof(linkage));

        // `!(>= 0)` rather than `< 0`, so a NaN is refused rather than counting no merge at all.
        // Infinity is refused as the reference refuses it: its range is [0, inf), open at the top.
        if (!(distanceThreshold >= 0.0) || double.IsPositiveInfinity(distanceThreshold))
        {
            throw new ArgumentOutOfRangeException(
                nameof(distanceThreshold), distanceThreshold, "Must be finite and zero or greater.");
        }

        int sampleCount = Rows(samples, featureCount);
        Finite.Require(samples, nameof(samples));
        return Cut(samples, featureCount, sampleCount, linkage, tree => AtOrAbove(tree.Distances, distanceThreshold) + 1);
    }

    private static AgglomerativeClustering Cut(
        ReadOnlySpan<double> samples, int featureCount, int sampleCount, Linkage linkage, Func<Dendrogram, int> count)
    {
        Dendrogram tree = linkage == Linkage.Single
            ? SpanningTreeLinkage.Build(samples, featureCount, sampleCount)
            : NearestNeighbourChain.Build(samples, featureCount, sampleCount, linkage);
        int clusters = count(tree);
        return new AgglomerativeClustering(
            featureCount, clusters, linkage, TreeCut.Labels(tree.Children, sampleCount, clusters), tree);
    }

    private static int AtOrAbove(double[] distances, double threshold) =>
        distances.Count(distance => distance >= threshold);

    private static void Defined(Linkage linkage, string paramName)
    {
        if (linkage is < Linkage.Ward or > Linkage.Single)
        {
            throw new ArgumentOutOfRangeException(paramName, linkage, "Not a defined linkage.");
        }
    }

    private static int Rows(ReadOnlySpan<double> samples, int featureCount)
    {
        if (samples.Length == 0 || samples.Length % featureCount != 0)
        {
            throw new ArgumentException(
                $"samples holds {samples.Length} values, which is not a positive whole number of "
                + $"rows of {featureCount}.",
                nameof(samples));
        }

        // Two, as the reference requires: one sample has no merge to make and no tree to cut.
        int rows = samples.Length / featureCount;
        if (rows < 2)
        {
            throw new ArgumentException("samples holds one row; a merge tree needs at least two.", nameof(samples));
        }

        return rows;
    }
}
