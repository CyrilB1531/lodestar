using BenchmarkDotNet.Attributes;
using Lodestar.Abstractions;
using Lodestar.Preprocessing;

namespace Lodestar.Text.Benchmarks;

// SonarLint S2245, CA5394: a seeded Random builds a reproducible matrix; no security use.
#pragma warning disable S2245, CA5394

/// <summary>A sparse RobustScaler fit, where each column's percentiles need its values.</summary>
[MemoryDiagnoser]
public class RobustScalerSparseBenchmarks
{
    private CsrMatrix _matrix = null!;
    private readonly RobustScalerOptions _options = new() { WithCentring = false };

    /// <summary>Rows, columns and stored values per row.</summary>
    [Params("2000x2000x20", "500x20000x5")]
    public string Shape { get; set; } = "2000x2000x20";

    [GlobalSetup]
    public void Setup()
    {
        string[] parts = Shape.Split('x');
        int rows = int.Parse(parts[0], System.Globalization.CultureInfo.InvariantCulture);
        int columns = int.Parse(parts[1], System.Globalization.CultureInfo.InvariantCulture);
        int perRow = int.Parse(parts[2], System.Globalization.CultureInfo.InvariantCulture);
        var random = new Random(765);
        var values = new double[rows * perRow];
        var indices = new int[rows * perRow];
        var pointers = new int[rows + 1];
        for (int row = 0; row < rows; row++)
        {
            int[] picked = [.. Enumerable.Range(0, columns).OrderBy(_ => random.Next()).Take(perRow).Order()];
            for (int k = 0; k < perRow; k++)
            {
                indices[(row * perRow) + k] = picked[k];
                values[(row * perRow) + k] = random.NextDouble() * 10.0;
            }

            pointers[row + 1] = (row + 1) * perRow;
        }

        _matrix = new CsrMatrix(rows, columns, values, indices, pointers);
    }

    [Benchmark]
    public RobustScaler Fit() => RobustScaler.Fit(_matrix, _options);
}
