namespace Lodestar.Cluster.Internal;

/// <summary>Clusters grown out of an adjacency, ascending by sample index.</summary>
/// <remarks>
/// <c>sklearn.cluster._dbscan_inner</c>. Nothing here looks at a feature: both entry points
/// on <see cref="Dbscan"/> reduce to the same adjacency first, which is why the euclidean
/// and precomputed paths cannot disagree about anything but a distance.
/// </remarks>
internal static class Growth
{
    /// <summary>The label of a sample no core point's expansion ever reached.</summary>
    public const int Noise = -1;

    /// <summary>Labels every sample, and names the core ones.</summary>
    /// <param name="offsets">Where each sample's neighbours start in <paramref name="indices"/>.</param>
    /// <param name="indices">The neighbours themselves, the sample's own index included.</param>
    /// <param name="minimumSamples">How large a neighbourhood makes its centre core.</param>
    /// <remarks>
    /// A sample already labelled is never relabelled, which is the whole rule for a border point
    /// two clusters can reach: it joins whichever is grown first. Measured on four orderings of
    /// one point set (#759) — the label follows the growth order, not the border's own position.
    /// Expansion order inside one cluster cannot change a label, so the stack could be a queue;
    /// the order clusters are <em>started</em> in could not.
    /// </remarks>
    public static (int[] Labels, int[] CoreIndices, int ClusterCount) Label(
        int[] offsets, int[] indices, int minimumSamples)
    {
        int sampleCount = offsets.Length - 1;
        var labels = new int[sampleCount];
        var core = new bool[sampleCount];
        var cores = new List<int>();
        for (int row = 0; row < sampleCount; row++)
        {
            labels[row] = Noise;
            if (offsets[row + 1] - offsets[row] >= minimumSamples)
            {
                core[row] = true;
                cores.Add(row);
            }
        }

        int cluster = 0;
        var stack = new Stack<int>();
        foreach (int seed in cores)
        {
            if (labels[seed] != Noise)
            {
                continue;
            }

            labels[seed] = cluster;
            stack.Push(seed);
            while (stack.Count > 0)
            {
                Expand(stack.Pop(), cluster, offsets, indices, labels, core, stack);
            }

            cluster++;
        }

        return (labels, [.. cores], cluster);
    }

    private static void Expand(
        int current, int cluster, int[] offsets, int[] indices, int[] labels, bool[] core, Stack<int> stack)
    {
        for (int slot = offsets[current]; slot < offsets[current + 1]; slot++)
        {
            int neighbour = indices[slot];
            if (labels[neighbour] != Noise)
            {
                continue;
            }

            labels[neighbour] = cluster;
            if (core[neighbour])
            {
                stack.Push(neighbour);
            }
        }
    }
}
