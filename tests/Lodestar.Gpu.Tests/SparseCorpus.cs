using Lodestar.Abstractions;

namespace Lodestar.Gpu.Tests;

// CA5394/S2245 (insecure randomness): a seeded Random builds a reproducible corpus so a
// failing case replays; there is no security decision here.
#pragma warning disable CA5394, S2245

/// <summary>Seeded sparse and dense operands for the SpMM tests.</summary>
internal static class SparseCorpus
{
    /// <summary>A CSR matrix whose rows hold roughly <paramref name="density"/> of their columns.</summary>
    public static CsrMatrix Matrix(int rows, int columns, double density, int seed)
    {
        var random = new Random(seed);
        var values = new List<double>();
        var indices = new List<int>();
        var pointers = new int[rows + 1];

        for (int row = 0; row < rows; row++)
        {
            pointers[row] = values.Count;
            for (int column = 0; column < columns; column++)
            {
                if (random.NextDouble() < density)
                {
                    indices.Add(column);
                    values.Add((random.NextDouble() * 2.0) - 1.0);
                }
            }
        }

        pointers[rows] = values.Count;
        return new CsrMatrix(rows, columns, [.. values], [.. indices], pointers);
    }

    /// <summary>A row-major dense block.</summary>
    public static double[] Block(int rows, int width, int seed)
    {
        var random = new Random(seed);
        var values = new double[rows * width];
        for (int i = 0; i < values.Length; i++)
        {
            values[i] = (random.NextDouble() * 2.0) - 1.0;
        }

        return values;
    }
}
