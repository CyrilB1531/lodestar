namespace Lodestar.Cluster.Internal;

/// <summary>Ward, complete and average linkage, by the nearest-neighbour chain.</summary>
/// <remarks>
/// <c>scipy.cluster.hierarchy.linkage</c>'s algorithm, which scikit-learn calls for these three
/// when there is no connectivity. Its tie-breaking is the parity target, so every choice below is
/// the reference's rather than an equivalent one: the chain starts at the lowest live slot, prefers
/// the previous link, and takes the first strict minimum in slot order.
/// </remarks>
internal static class NearestNeighbourChain
{
    /// <summary>Builds the whole merge tree.</summary>
    /// <param name="samples">The samples, row-major.</param>
    /// <param name="featureCount">How many values each row carries.</param>
    /// <param name="sampleCount">How many rows <paramref name="samples"/> holds, at least two.</param>
    /// <param name="linkage">Ward, complete or average.</param>
    public static Dendrogram Build(
        ReadOnlySpan<double> samples, int featureCount, int sampleCount, Linkage linkage)
    {
        double[] distances = Condensed(samples, featureCount, sampleCount);
        var size = new int[sampleCount];
        var live = new int[sampleCount];
        var rowBase = new long[sampleCount];
        for (int slot = 0; slot < sampleCount; slot++)
        {
            size[slot] = 1;
            live[slot] = slot;
            rowBase[slot] = RowStart(sampleCount, slot);
        }

        var cells = new Cells(distances, rowBase, live);
        var merges = new Merge[sampleCount - 1];
        var chain = new int[sampleCount];
        int chainLength = 0;
        for (int step = 0; step < sampleCount - 1; step++)
        {
            if (chainLength == 0)
            {
                chain[0] = live[0];
                chainLength = 1;
            }

            (int x, int y, double height) = MutualNeighbours(cells, chain, ref chainLength);
            if (x > y)
            {
                (x, y) = (y, x);
            }

            var joined = new Joined(x, y, height, size[x], size[y]);
            merges[step] = new Merge(x, y, height, step);
            size[x] = 0;
            size[y] = joined.SizeX + joined.SizeY;
            cells.Remove(x);
            switch (linkage)
            {
                case Linkage.Complete:
                    Update<CompleteRule>(cells, size, joined);
                    break;
                case Linkage.Average:
                    Update<AverageRule>(cells, size, joined);
                    break;
                default:
                    Update<WardRule>(cells, size, joined);
                    break;
            }
        }

        return Relabel(merges, sampleCount);
    }

    /// <summary>Walks the chain until its last two links are each other's nearest neighbour.</summary>
    /// <remarks>
    /// Candidates are the live slots in ascending order, the order the scan over every slot met
    /// them in, so the first strict minimum is the same one.
    /// </remarks>
    private static (int X, int Y, double Height) MutualNeighbours(Cells cells, int[] chain, ref int chainLength)
    {
        double[] distances = cells.Distances;
        while (true)
        {
            int x = chain[chainLength - 1];
            int y = 0;
            double nearest = double.PositiveInfinity;

            // The previous link wins a tie against every other candidate, which is what stops
            // the chain cycling between two clusters at equal distance.
            if (chainLength > 1)
            {
                y = chain[chainLength - 2];
                nearest = distances[cells.Index(x, y)];
            }

            Nearest(cells, x, ref y, ref nearest);

            if (chainLength > 1 && y == chain[chainLength - 2])
            {
                chainLength -= 2;
                return (x, y, nearest);
            }

            chain[chainLength++] = y;
        }
    }

    /// <summary>Lowers <paramref name="nearest"/> to the first strictly closer live slot, in ascending order.</summary>
    private static void Nearest(Cells cells, int x, ref int y, ref double nearest)
    {
        double[] distances = cells.Distances;
        long[] rowBase = cells.RowBase;
        int[] live = cells.Live;
        int position = 0;
        for (; position < cells.Count && live[position] < x; position++)
        {
            int candidate = live[position];
            double distance = distances[rowBase[candidate] + x];
            if (distance < nearest)
            {
                nearest = distance;
                y = candidate;
            }
        }

        if (position < cells.Count && live[position] == x)
        {
            position++;
        }

        long xBase = rowBase[x];
        for (; position < cells.Count; position++)
        {
            int candidate = live[position];
            double distance = distances[xBase + candidate];
            if (distance < nearest)
            {
                nearest = distance;
                y = candidate;
            }
        }
    }

    /// <summary>Lance–Williams: every live cluster's distance to the one now in the merge's second slot.</summary>
    /// <remarks>
    /// The rule is a type argument so the linkage is chosen once per merge rather than per cell;
    /// each rule's arithmetic is the one scipy writes, unchanged.
    /// </remarks>
    private static void Update<TRule>(Cells cells, int[] size, in Joined joined)
        where TRule : struct, ILinkageRule
    {
        double[] distances = cells.Distances;
        long[] rowBase = cells.RowBase;
        int[] live = cells.Live;
        int x = joined.X;
        int y = joined.Y;
        long xBase = rowBase[x];
        long yBase = rowBase[y];
        var rule = default(TRule);
        for (int position = 0; position < cells.Count; position++)
        {
            int other = live[position];
            if (other == y)
            {
                continue;
            }

            long toXIndex = other < x ? rowBase[other] + x : xBase + other;
            long toYIndex = other < y ? rowBase[other] + y : yBase + other;
            distances[toYIndex] = rule.Apply(distances[toXIndex], distances[toYIndex], joined, size[other]);
        }
    }

    private interface ILinkageRule
    {
        double Apply(double toX, double toY, in Joined joined, int sizeOther);
    }

    private readonly struct CompleteRule : ILinkageRule
    {
        public double Apply(double toX, double toY, in Joined joined, int sizeOther) => Math.Max(toX, toY);
    }

    private readonly struct AverageRule : ILinkageRule
    {
        // Written exactly as scipy's own update, including where the division happens: dividing
        // at the end instead gave a different tree on 1 of 400 integer datasets (#760).
        public double Apply(double toX, double toY, in Joined joined, int sizeOther) =>
            ((joined.SizeX * toX) + (joined.SizeY * toY)) / (joined.SizeX + joined.SizeY);
    }

    private readonly struct WardRule : ILinkageRule
    {
        // The reciprocal multiplied through, as scipy does: dividing once at the end gave a different
        // tree on 16 of 400 integer datasets, one ulp turning a tie into an order (#760).
        public double Apply(double toX, double toY, in Joined joined, int sizeOther)
        {
            double t = 1.0 / (joined.SizeX + joined.SizeY + sizeOther);
            return Math.Sqrt(
                ((sizeOther + joined.SizeX) * t * toX * toX)
                + ((sizeOther + joined.SizeY) * t * toY * toY)
                - (sizeOther * t * joined.Height * joined.Height));
        }
    }

    /// <summary>Sorts the merges by height, stably, and renames slots to node ids.</summary>
    /// <remarks>
    /// The chain finds merges out of height order, so they are sorted — stably, which keeps the
    /// order the chain found equal heights in and is the reference's <c>mergesort</c>. A slot then
    /// names whichever cluster last occupied it, which a union-find resolves; each pair is written
    /// smaller id first.
    /// </remarks>
    private static Dendrogram Relabel(Merge[] merges, int sampleCount)
    {
        // Found is unique, so the order is total and a plain sort gives what the stable one did.
        var sorted = (Merge[])merges.Clone();
        Array.Sort(sorted, static (left, right) =>
        {
            int order = left.Height.CompareTo(right.Height);
            return order != 0 ? order : left.Found.CompareTo(right.Found);
        });
        var roots = new LinkageRoots(sampleCount);
        var children = new int[2 * (sampleCount - 1)];
        var heights = new double[sampleCount - 1];
        for (int row = 0; row < sorted.Length; row++)
        {
            int left = roots.Find(sorted[row].X);
            int right = roots.Find(sorted[row].Y);
            children[2 * row] = Math.Min(left, right);
            children[(2 * row) + 1] = Math.Max(left, right);
            heights[row] = sorted[row].Height;
            roots.Join(left, right);
        }

        return new Dendrogram(children, heights);
    }

    private static double[] Condensed(ReadOnlySpan<double> samples, int featureCount, int sampleCount)
    {
        var distances = new double[(long)sampleCount * (sampleCount - 1) / 2];
        long slot = 0;
        for (int row = 0; row < sampleCount; row++)
        {
            for (int other = row + 1; other < sampleCount; other++)
            {
                distances[slot++] = EuclideanDistance.Between(samples, featureCount, row, other);
            }
        }

        return distances;
    }

    /// <summary>
    /// The pair <c>(i, j)</c>'s place in the upper triangle read row by row, less <c>j</c>: adding
    /// <c>j &gt; i</c> gives the index, so a scan along one row or column adds instead of multiplying.
    /// </summary>
    private static long RowStart(int count, int i) =>
        ((long)count * i) - ((long)i * (i + 1) / 2) - i - 1;

    /// <summary>The condensed matrix, its row bases, and the live slots in ascending order.</summary>
    private sealed class Cells(double[] distances, long[] rowBase, int[] live)
    {
        public double[] Distances { get; } = distances;

        public long[] RowBase { get; } = rowBase;

        public int[] Live { get; } = live;

        public int Count { get; private set; } = live.Length;

        public long Index(int i, int j) => i < j ? RowBase[i] + j : RowBase[j] + i;

        /// <summary>Drops a slot that merged away, keeping the rest ascending.</summary>
        public void Remove(int slot)
        {
            int position = Array.BinarySearch(Live, 0, Count, slot);
            Array.Copy(Live, position + 1, Live, position, Count - position - 1);
            Count--;
        }
    }

    private readonly record struct Merge(int X, int Y, double Height, int Found);

    /// <summary>The two slots just merged, with their sizes before the merge.</summary>
    private readonly record struct Joined(int X, int Y, double Height, int SizeX, int SizeY);
}
