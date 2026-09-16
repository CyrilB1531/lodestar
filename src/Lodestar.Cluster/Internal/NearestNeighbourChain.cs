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
        for (int slot = 0; slot < sampleCount; slot++)
        {
            size[slot] = 1;
        }

        var merges = new Merge[sampleCount - 1];
        var chain = new int[sampleCount];
        int chainLength = 0;
        for (int step = 0; step < sampleCount - 1; step++)
        {
            if (chainLength == 0)
            {
                chain[0] = Array.FindIndex(size, count => count > 0);
                chainLength = 1;
            }

            (int x, int y, double height) = MutualNeighbours(distances, size, chain, ref chainLength);
            if (x > y)
            {
                (x, y) = (y, x);
            }

            var joined = new Joined(x, y, height, size[x], size[y]);
            merges[step] = new Merge(x, y, height, step);
            size[x] = 0;
            size[y] = joined.SizeX + joined.SizeY;
            Update(distances, size, joined, linkage);
        }

        return Relabel(merges, sampleCount);
    }

    /// <summary>Walks the chain until its last two links are each other's nearest neighbour.</summary>
    private static (int X, int Y, double Height) MutualNeighbours(
        double[] distances, int[] size, int[] chain, ref int chainLength)
    {
        int count = size.Length;
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
                nearest = distances[Index(count, x, y)];
            }

            for (int candidate = 0; candidate < count; candidate++)
            {
                if (size[candidate] == 0 || candidate == x)
                {
                    continue;
                }

                double distance = distances[Index(count, x, candidate)];
                if (distance < nearest)
                {
                    nearest = distance;
                    y = candidate;
                }
            }

            if (chainLength > 1 && y == chain[chainLength - 2])
            {
                chainLength -= 2;
                return (x, y, nearest);
            }

            chain[chainLength++] = y;
        }
    }

    /// <summary>Lance–Williams: every live cluster's distance to the one now in the merge's second slot.</summary>
    private static void Update(double[] distances, int[] size, in Joined joined, Linkage linkage)
    {
        int count = size.Length;
        for (int other = 0; other < count; other++)
        {
            int sizeOther = size[other];
            if (sizeOther == 0 || other == joined.Y)
            {
                continue;
            }

            double toX = distances[Index(count, other, joined.X)];
            double toY = distances[Index(count, other, joined.Y)];
            distances[Index(count, other, joined.Y)] = linkage switch
            {
                Linkage.Complete => Math.Max(toX, toY),
                Linkage.Average => Average(toX, toY, joined.SizeX, joined.SizeY),
                _ => Ward(toX, toY, joined, sizeOther),
            };
        }
    }

    // Written exactly as scipy's own update, including where the division happens: dividing
    // at the end instead gave a different tree on 1 of 400 integer datasets (#760).
    private static double Average(double toX, double toY, int sizeX, int sizeY) =>
        ((sizeX * toX) + (sizeY * toY)) / (sizeX + sizeY);

    // The reciprocal multiplied through, as scipy does: dividing once at the end gave a different
    // tree on 16 of 400 integer datasets, one ulp turning a tie into an order (#760).
    private static double Ward(double toX, double toY, in Joined joined, int sizeOther)
    {
        double t = 1.0 / (joined.SizeX + joined.SizeY + sizeOther);
        return Math.Sqrt(
            ((sizeOther + joined.SizeX) * t * toX * toX)
            + ((sizeOther + joined.SizeY) * t * toY * toY)
            - (sizeOther * t * joined.Height * joined.Height));
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
        Merge[] sorted = [.. merges.OrderBy(merge => merge.Height).ThenBy(merge => merge.Found)];
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

    /// <summary>Where the pair <c>(i, j)</c> sits in the upper triangle, read row by row.</summary>
    private static long Index(int count, int i, int j)
    {
        if (i > j)
        {
            (i, j) = (j, i);
        }

        return ((long)count * i) - ((long)i * (i + 1) / 2) + j - i - 1;
    }

    private readonly record struct Merge(int X, int Y, double Height, int Found);

    /// <summary>The two slots just merged, with their sizes before the merge.</summary>
    private readonly record struct Joined(int X, int Y, double Height, int SizeX, int SizeY);
}
