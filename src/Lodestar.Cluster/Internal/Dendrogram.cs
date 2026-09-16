namespace Lodestar.Cluster.Internal;

/// <summary>A merge tree in scikit-learn's layout: row <c>k</c> creates node <c>n + k</c>.</summary>
/// <param name="Children">The two node ids each merge joins, row-major.</param>
/// <param name="Distances">The height of each merge, ascending.</param>
internal readonly record struct Dendrogram(int[] Children, double[] Distances);
