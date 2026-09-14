using System.Globalization;
using NumFlat;

namespace Lodestar.Text.Benchmarks;

/// <summary>
/// The seeded Gaussian blocks the two k-means incumbent benchmarks share, in the three layouts
/// their callers take: row-major for this package, a <c>Vec&lt;double&gt;</c> per row for NumFlat, and
/// a column per feature for Meta.Numerics.
/// </summary>
internal sealed class ClusterBlobs
{
    private ClusterBlobs(int rowCount, int featureCount, int clusterCount, double[] matrix)
    {
        RowCount = rowCount;
        FeatureCount = featureCount;
        ClusterCount = clusterCount;
        Matrix = matrix;

        Rows = new Vec<double>[rowCount];
        for (int row = 0; row < rowCount; row++)
        {
            Rows[row] = new Vec<double>(matrix.AsSpan(row * featureCount, featureCount).ToArray());
        }

        Columns = new double[featureCount][];
        for (int feature = 0; feature < featureCount; feature++)
        {
            Columns[feature] = new double[rowCount];
            for (int row = 0; row < rowCount; row++)
            {
                Columns[feature][row] = matrix[(row * featureCount) + feature];
            }
        }
    }

    public int RowCount { get; }

    public int FeatureCount { get; }

    public int ClusterCount { get; }

    public double[] Matrix { get; }

    public Vec<double>[] Rows { get; }

    public double[][] Columns { get; }

    /// <summary>The first <see cref="ClusterCount"/> rows, one from each blob, row-major.</summary>
    public double[] FirstRows => Matrix.AsSpan(0, ClusterCount * FeatureCount).ToArray();

    /// <summary>Parses a <c>rows x features x clusters</c> shape.</summary>
    public static (int Rows, int Features, int Clusters) Parse(string shape)
    {
        string[] parts = shape.Split('x');
        return (
            int.Parse(parts[0], CultureInfo.InvariantCulture),
            int.Parse(parts[1], CultureInfo.InvariantCulture),
            int.Parse(parts[2], CultureInfo.InvariantCulture));
    }

    // SonarLint S2245, CA5394: a seeded Random builds a reproducible benchmark block; no
    // security use.
#pragma warning disable S2245, CA5394

    /// <summary>
    /// Blob centres drawn uniformly from <c>[-spread, spread]</c> per feature, unit-variance
    /// noise around them, and row <c>i</c> drawn from blob <c>i mod k</c> — so the first
    /// <c>k</c> rows are one per blob.
    /// </summary>
    public static ClusterBlobs Generate(string shape, double spread, int seed)
    {
        (int rowCount, int featureCount, int clusterCount) = Parse(shape);
        var random = new Random(seed);
        var centres = new double[clusterCount * featureCount];
        for (int i = 0; i < centres.Length; i++)
        {
            centres[i] = ((2.0 * random.NextDouble()) - 1.0) * spread;
        }

        var matrix = new double[rowCount * featureCount];
        for (int row = 0; row < rowCount; row++)
        {
            int blob = row % clusterCount;
            for (int feature = 0; feature < featureCount; feature++)
            {
                matrix[(row * featureCount) + feature] = centres[(blob * featureCount) + feature] + Gaussian(random);
            }
        }

        return new ClusterBlobs(rowCount, featureCount, clusterCount, matrix);
    }

    private static double Gaussian(Random random)
    {
        // Box-Muller; 1 - NextDouble() keeps the logarithm's argument off zero.
        double u = 1.0 - random.NextDouble();
        double v = random.NextDouble();
        return Math.Sqrt(-2.0 * Math.Log(u)) * Math.Cos(2.0 * Math.PI * v);
    }
#pragma warning restore S2245, CA5394
}
