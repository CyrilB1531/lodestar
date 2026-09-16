using System.Globalization;

namespace Lodestar.Text.Benchmarks;

/// <summary>The sample block the two principal-component benchmarks share, built the same way for both.</summary>
/// <remarks>
/// One seed and one formula, so the NumFlat table (#701) and the Meta.Numerics one (#756) are read on
/// the same data rather than on two blocks that happen to look alike.
/// </remarks>
internal static class PcaBlock
{
    /// <summary>Reads a <c>rows x columns</c> shape parameter.</summary>
    public static (int Rows, int Columns) Shape(string shape)
    {
        string[] parts = shape.Split('x');
        return (int.Parse(parts[0], CultureInfo.InvariantCulture), int.Parse(parts[1], CultureInfo.InvariantCulture));
    }

    // SonarLint S2245, CA5394: a seeded Random builds a reproducible benchmark block; no security use.
#pragma warning disable S2245, CA5394
    /// <summary>A row-major block whose columns are scaled by their index, so the spectrum is spread rather than flat.</summary>
    public static double[] Matrix(int rowCount, int columnCount)
    {
        var random = new Random(701);
        var matrix = new double[rowCount * columnCount];
        for (int i = 0; i < matrix.Length; i++)
        {
            matrix[i] = random.NextDouble() * (1 + (i % columnCount));
        }

        return matrix;
    }
#pragma warning restore S2245, CA5394

    /// <summary>The same block by column, which is how Meta.Numerics takes it.</summary>
    public static double[][] Columns(double[] matrix, int rowCount, int columnCount)
    {
        var columns = new double[columnCount][];
        for (int column = 0; column < columnCount; column++)
        {
            var values = new double[rowCount];
            for (int row = 0; row < rowCount; row++)
            {
                values[row] = matrix[(row * columnCount) + column];
            }

            columns[column] = values;
        }

        return columns;
    }
}
