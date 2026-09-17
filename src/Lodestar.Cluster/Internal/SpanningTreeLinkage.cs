namespace Lodestar.Cluster.Internal;

/// <summary>Single linkage, by a minimum spanning tree.</summary>
/// <remarks>
/// scikit-learn's own path rather than scipy's: <c>mst_linkage_core</c>, which is Prim's algorithm
/// computing each distance when it is needed, so no distance matrix is ever held — linear memory
/// where the other three linkages need a quadratic matrix.
/// </remarks>
internal static class SpanningTreeLinkage
{
    /// <summary>Builds the whole merge tree.</summary>
    /// <param name="samples">The samples, row-major.</param>
    /// <param name="featureCount">How many values each row carries.</param>
    /// <param name="sampleCount">How many rows <paramref name="samples"/> holds, at least two.</param>
    public static Dendrogram Build(ReadOnlySpan<double> samples, int featureCount, int sampleCount)
    {
        Edge[] edges = SpanningTree(samples, featureCount, sampleCount);
        // Found is unique, so the order is total and a plain sort gives what the stable one did.
        Edge[] sorted = edges;
        Array.Sort(sorted, static (left, right) =>
        {
            int order = left.Weight.CompareTo(right.Weight);
            return order != 0 ? order : left.Found.CompareTo(right.Found);
        });

        var roots = new LinkageRoots(sampleCount);
        var children = new int[2 * (sampleCount - 1)];
        var heights = new double[sampleCount - 1];
        for (int row = 0; row < sorted.Length; row++)
        {
            // Left then right, in the edge's own order and not sorted: the MST path writes
            // [7, 4] where scipy's writes [4, 7] for the same merge, measured on 0, 1, 5, 6, 20.
            int left = roots.Find(sorted[row].From);
            int right = roots.Find(sorted[row].To);
            children[2 * row] = left;
            children[(2 * row) + 1] = right;
            heights[row] = sorted[row].Weight;
            roots.Join(left, right);
        }

        return new Dendrogram(children, heights);
    }

    /// <summary>Prim's algorithm from sample 0, taking the first strict minimum at each step.</summary>
    private static Edge[] SpanningTree(ReadOnlySpan<double> samples, int featureCount, int sampleCount)
    {
        var inTree = new bool[sampleCount];
        var nearest = new double[sampleCount];
        for (int sample = 0; sample < sampleCount; sample++)
        {
            nearest[sample] = double.PositiveInfinity;
        }

        var edges = new Edge[sampleCount - 1];
        int current = 0;
        for (int step = 0; step < sampleCount - 1; step++)
        {
            inTree[current] = true;
            double shortest = double.PositiveInfinity;
            int next = 0;
            for (int candidate = 0; candidate < sampleCount; candidate++)
            {
                if (inTree[candidate])
                {
                    continue;
                }

                double distance = EuclideanDistance.Between(samples, featureCount, current, candidate);
                if (distance < nearest[candidate])
                {
                    nearest[candidate] = distance;
                }

                if (nearest[candidate] < shortest)
                {
                    shortest = nearest[candidate];
                    next = candidate;
                }
            }

            edges[step] = new Edge(current, next, shortest, step);
            current = next;
        }

        return edges;
    }

    private readonly record struct Edge(int From, int To, double Weight, int Found);
}
