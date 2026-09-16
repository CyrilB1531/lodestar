using LodestarDbscan = Lodestar.Cluster.Dbscan;

namespace Lodestar.Text.Benchmarks;

/// <summary>The agreement the DBSCAN benchmark requires before anything is timed.</summary>
/// <remarks>
/// Cluster numbers are arbitrary across libraries and the partition is not, so every side is
/// reduced to a canonical labelling first: clusters renumbered by the row each is first seen
/// on, noise kept as <c>-1</c>. Two libraries agree when those arrays are equal, whatever they
/// called their clusters. <c>bench/README.md</c> section 15 is the rule.
/// </remarks>
internal static class DbscanAgreement
{
    /// <summary>Renumbers a labelling by first appearance, noise left alone.</summary>
    public static int[] Canonical(IReadOnlyList<int> labels)
    {
        var seen = new Dictionary<int, int>();
        var canonical = new int[labels.Count];
        for (int row = 0; row < labels.Count; row++)
        {
            int label = labels[row];
            if (label < 0)
            {
                canonical[row] = -1;
                continue;
            }

            if (!seen.TryGetValue(label, out int renumbered))
            {
                renumbered = seen.Count;
                seen.Add(label, renumbered);
            }

            canonical[row] = renumbered;
        }

        return canonical;
    }

    /// <summary>Throws unless the two labellings describe the same partition.</summary>
    public static void RequireSamePartition(LodestarDbscan ours, IReadOnlyList<int> theirs, string name)
    {
        int[] mine = Canonical(ours.Labels);
        int[] other = Canonical(theirs);
        if (mine.Length != other.Length)
        {
            throw new InvalidOperationException(
                $"{name} labelled {other.Length} rows where this package labelled {mine.Length}.");
        }

        int disagreements = 0;
        int firstRow = -1;
        for (int row = 0; row < mine.Length; row++)
        {
            if (mine[row] != other[row])
            {
                disagreements++;
                if (firstRow < 0)
                {
                    firstRow = row;
                }
            }
        }

        if (disagreements > 0)
        {
            throw new InvalidOperationException(
                $"{name} disagrees with this package on {disagreements} of {mine.Length} rows, "
                + $"first at row {firstRow}: {mine[firstRow]} here, {other[firstRow]} there. "
                + "Timing two different answers measures nothing.");
        }
    }
}
