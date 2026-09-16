using NumFlat;
using NumFlat.Clustering;
using LodestarDbscan = Lodestar.Cluster.Dbscan;

namespace Lodestar.Text.Benchmarks;

/// <summary>
/// The blobs, the radius and the three call shapes the two DBSCAN benchmarks share.
/// </summary>
/// <remarks>
/// One block rather than a <c>GlobalSetup</c> in each class: the neighbouring PCA classes were
/// flagged for duplicating theirs (#756), and these two differ only in which libraries they can
/// invite. The radius is measured rather than guessed — see <see cref="RadiusFor"/>.
/// </remarks>
internal sealed class DbscanBlock
{
    /// <summary>How many samples a neighbourhood needs, this package's and both incumbents'.</summary>
    public const int MinimumPoints = 5;

    private DbscanBlock(ClusterBlobs blobs, double radius)
    {
        Blobs = blobs;
        Radius = radius;
        Points = new IndexedPoint[blobs.RowCount];
        if (blobs.FeatureCount == 2)
        {
            for (int row = 0; row < blobs.RowCount; row++)
            {
                Points[row] = new IndexedPoint(
                    row, blobs.Matrix[row * 2], blobs.Matrix[(row * 2) + 1]);
            }
        }
    }

    public ClusterBlobs Blobs { get; }

    /// <summary>The neighbourhood radius this shape is measured at.</summary>
    public double Radius { get; }

    /// <summary>The same rows as <c>Dbscan</c> 3.0.0's planar points, empty above two features.</summary>
    public IndexedPoint[] Points { get; }

    /// <summary>Builds the block for a <c>rows x features x clusters</c> shape.</summary>
    public static DbscanBlock Generate(string shape)
    {
        ClusterBlobs blobs = ClusterBlobs.Generate(shape, spread: 1000.0, seed: 759);
        return new DbscanBlock(blobs, RadiusFor(blobs.FeatureCount));
    }

    /// <summary>
    /// The radius at which this generator's blobs are recovered exactly, measured 2026-09-16.
    /// </summary>
    /// <remarks>
    /// One constant cannot serve: the blobs are unit-variance Gaussian, so the distance to a fifth
    /// neighbour grows with the dimension. The margin over the smallest radius that works is not
    /// cosmetic either — where most of the matrix is noise, NumFlat 1.3.4 loses border samples the
    /// ascending scan reaches first, and disagreeing libraries cannot be timed against each other.
    /// <c>bench/README.md</c> section 48 has the nine-point reproducer.
    /// </remarks>
    public static double RadiusFor(int featureCount) => featureCount switch
    {
        <= 2 => 1.0,
        <= 8 => 3.0,
        _ => 5.0,
    };

    /// <summary>This package's own answer, which both incumbents are checked against.</summary>
    public LodestarDbscan Ours() =>
        LodestarDbscan.Fit(Blobs.Matrix, Blobs.FeatureCount, Radius, MinimumPoints);

    /// <summary>NumFlat's labels for the same rows, in a fresh array.</summary>
    public int[] NumFlatLabels()
    {
        var labels = new int[Blobs.RowCount];
        DbScan.Fit(Blobs.Rows, DistanceMetric.Euclidean, Radius, MinimumPoints, labels);
        return labels;
    }
}

/// <summary>A planar point that remembers which row it came from.</summary>
/// <remarks>
/// <c>Dbscan</c> 3.0.0 returns the objects it clustered rather than a labelling, so the row has
/// to travel with the point for the partitions to be comparable at all. Public only because a
/// benchmark method returning its cluster set has to be, which is BenchmarkDotNet's rule.
/// </remarks>
public sealed class IndexedPoint : global::Dbscan.IPointData
{
    public IndexedPoint(int row, double x, double y)
    {
        Row = row;
        Point = new global::Dbscan.Point(x, y);
    }

    public int Row { get; }

    public global::Dbscan.Point Point { get; }
}
