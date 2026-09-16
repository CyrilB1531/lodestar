using BenchmarkDotNet.Attributes;
using Lodestar.Abstractions;
using Lodestar.Preprocessing;

namespace Lodestar.Text.Benchmarks;

// CA1822: see LevenshteinIncumbentBenchmarks.
#pragma warning disable CA1822

/// <summary>
/// What a caller pays for not holding the matrix: a scaler fitted over batches against one whole
/// fit, and the sparse path against the dense one on the same data (#765).
/// </summary>
/// <remarks>
/// No foreign incumbent. ML.NET's normalizers have no incremental entry point and its sparse support
/// is inside the pipeline, so these rows compare this package against itself — which is what the
/// question is: whether the batched and sparse paths cost what they should.
/// </remarks>
[MemoryDiagnoser]
public class PartialFitBenchmarks
{
    private const int Features = 10;

    private double[] _matrix = [];
    private double[][] _batches = [];
    private CsrMatrix _sparse = null!;
    private double[] _dense = [];

    /// <summary>How many rows to fit, at ten features each.</summary>
    [Params(10_000, 100_000)]
    public int RowCount { get; set; } = 10_000;

    /// <summary>How many batches to cut those rows into.</summary>
    [Params(10, 100)]
    public int BatchCount { get; set; } = 10;

    // SonarLint S2245, CA5394: a seeded Random builds a reproducible benchmark block; no security use.
#pragma warning disable S2245, CA5394
    [GlobalSetup]
    public void Setup()
    {
        var random = new Random(765);
        _matrix = new double[RowCount * Features];
        for (int i = 0; i < _matrix.Length; i++)
        {
            _matrix[i] = random.NextDouble();
        }

        int rowsPerBatch = RowCount / BatchCount;
        _batches = new double[BatchCount][];
        for (int batch = 0; batch < BatchCount; batch++)
        {
            int start = batch * rowsPerBatch * Features;
            int length = batch == BatchCount - 1 ? _matrix.Length - start : rowsPerBatch * Features;
            _batches[batch] = _matrix.AsSpan(start, length).ToArray();
        }

        // One value in ten stored: the shape a CsrMatrix is for, and the same data densely.
        var values = new List<double>();
        var columns = new List<int>();
        var pointers = new List<int> { 0 };
        _dense = new double[RowCount * Features];
        for (int row = 0; row < RowCount; row++)
        {
            for (int feature = 0; feature < Features; feature++)
            {
                if (random.Next(10) != 0)
                {
                    continue;
                }

                double value = random.NextDouble() + 0.5;
                values.Add(value);
                columns.Add(feature);
                _dense[(row * Features) + feature] = value;
            }

            pointers.Add(values.Count);
        }

        _sparse = new CsrMatrix(RowCount, Features, [.. values], [.. columns], [.. pointers]);
    }
#pragma warning restore S2245, CA5394

    [Benchmark(Baseline = true)]
    public StandardScaler WholeFit() => StandardScaler.Fit(_matrix, Features);

    [Benchmark]
    public StandardScaler BatchedFit()
    {
        StandardScaler scaler = StandardScaler.Fit(_batches[0], Features);
        for (int batch = 1; batch < _batches.Length; batch++)
        {
            scaler = scaler.PartialFit(_batches[batch]);
        }

        return scaler;
    }

    [Benchmark]
    public StandardScaler SparseFit() => StandardScaler.Fit(_sparse);

    [Benchmark]
    public StandardScaler DenseFitOfTheSameData() =>
        StandardScaler.Fit(_dense, Features, new StandardScalerOptions { WithMean = false });
}
