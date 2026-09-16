using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Engines;
using Lodestar.Preprocessing;
using Microsoft.ML;
using Microsoft.ML.Data;
using Microsoft.ML.Transforms;

namespace Lodestar.Text.Benchmarks;

// CA1822: see LevenshteinIncumbentBenchmarks.
#pragma warning disable CA1822

/// <summary>
/// Fitting and applying a scaler: <see cref="MinMaxScaler"/> and <see cref="RobustScaler"/> against
/// ML.NET 5.0.0's <c>NormalizeMinMax</c> and <c>NormalizeRobustScaling</c> (#763).
/// </summary>
/// <remarks>
/// Its estimator is lazy, so it appears twice: the fit alone, and the fit followed by reading the
/// values back, which is the only one a caller can use. <c>fixZero: false</c> is passed because
/// ML.NET's default scales around zero onto <c>[−1, 1]</c> — a different transform, same name.
/// </remarks>

// long-comment: why this class pins its job where every other one here takes the default.
// The rows span 100 us to 50 ms and every one of them allocates a fresh matrix. Left to size
// itself, the engine put 4,096 invocations in an iteration -- forty seconds a warmup, drifting
// slower each time as the collector fell behind -- and RunStrategy.Monitoring, which skips the
// pilot, reported a standard deviation larger than half the mean. One invocation per iteration with
// a real warmup is what measures this shape: the numbers below hold to a few percent.
[SimpleJob(
    RunStrategy.Throughput,
    launchCount: 1,
    warmupCount: 5,
    iterationCount: 20,
    invocationCount: 1)]
[MemoryDiagnoser]
public class ScalerIncumbentBenchmarks
{
    private const int Features = 10;

    /// <summary>The vector column both sides read; ML.NET names its columns rather than passing values.</summary>
    private const string Column = "Features";

    private MLContext _context = null!;
    private IDataView _data = null!;
    private double[] _matrix = [];

    /// <summary>
    /// How many rows to scale, at ten features each. Twenty thousand rather than a hundred: an
    /// operation that returns a fresh 200,000-value matrix already allocates 1.6 MB, and at a
    /// million values BenchmarkDotNet's warmup never settles — measured, each iteration slower than
    /// the last as the collector falls behind.
    /// </summary>
    [Params(1_000, 20_000)]
    public int RowCount { get; set; } = 1_000;

    // SonarLint S2245, CA5394: a seeded Random builds a reproducible benchmark block; no security use.
#pragma warning disable S2245, CA5394
    [GlobalSetup]
    public void Setup()
    {
        var random = new Random(763);
        _matrix = new double[RowCount * Features];
        var rows = new ScalerRow[RowCount];
        for (int row = 0; row < RowCount; row++)
        {
            var values = new float[Features];
            for (int feature = 0; feature < Features; feature++)
            {
                double value = random.NextDouble() * (1 + feature);
                _matrix[(row * Features) + feature] = value;
                values[feature] = (float)value;
            }

            rows[row] = new ScalerRow { Features = values };
        }

        _context = new MLContext(seed: 763);
        _data = _context.Data.LoadFromEnumerable(rows);
        RequireSameScaling();
    }
#pragma warning restore S2245, CA5394

    [Benchmark(Baseline = true)]
    public double[] Lodestar_MinMax() =>
        MinMaxScaler.Fit(_matrix, Features).Transform(_matrix);

    [Benchmark]
    public double[] Lodestar_MaxAbs() =>
        MaxAbsScaler.Fit(_matrix, Features).Transform(_matrix);

    [Benchmark]
    public double[] Lodestar_Robust() =>
        RobustScaler.Fit(_matrix, Features).Transform(_matrix);

    [Benchmark]
    public NormalizingTransformer MlNet_NormalizeMinMax_Fit() =>
        _context.Transforms.NormalizeMinMax(Column, fixZero: false).Fit(_data);

    /// <summary>The same fit, with the scaled values read — which is where ML.NET does the work.</summary>
    [Benchmark]
    public int MlNet_NormalizeMinMax_Read() =>
        Read(_context.Transforms.NormalizeMinMax(Column, fixZero: false).Fit(_data));

    [Benchmark]
    public int MlNet_NormalizeRobustScaling_Read() =>
        Read(_context.Transforms.NormalizeRobustScaling(Column).Fit(_data));

    private int Read(NormalizingTransformer model)
    {
        int seen = 0;
        foreach (ScalerRow row in _context.Data.CreateEnumerable<ScalerRow>(
            model.Transform(_data), reuseRowObject: true))
        {
            seen += row.Features.Length;
        }

        return seen;
    }

    /// <summary>
    /// The two sides scale the same column to the same values before either is timed — checked in
    /// single precision, which is what ML.NET's pipeline carries.
    /// </summary>
    private void RequireSameScaling()
    {
        double[] ours = MinMaxScaler.Fit(_matrix, Features).Transform(_matrix);
        NormalizingTransformer model = _context.Transforms.NormalizeMinMax(Column, fixZero: false).Fit(_data);
        int row = 0;
        foreach (ScalerRow scaled in _context.Data.CreateEnumerable<ScalerRow>(
            model.Transform(_data), reuseRowObject: false))
        {
            for (int feature = 0; feature < Features; feature++)
            {
                double theirs = scaled.Features[feature];
                double mine = ours[(row * Features) + feature];
                if (Math.Abs(mine - theirs) > 1e-6)
                {
                    throw new InvalidOperationException(
                        $"row {row}, feature {feature}: ML.NET scales to {theirs} where this package scales to "
                        + $"{mine}. Timing two different transforms under one name would say nothing.");
                }
            }

            row++;
        }
    }
}

/// <summary>One row of the data view ML.NET scales; the shape is the framework's, not this package's.</summary>
public sealed class ScalerRow
{
    // CA1819 (properties should not return arrays): ML.NET binds a vector column to a float[]
    // property by reflection. A read-only view is not a shape its loader accepts.
#pragma warning disable CA1819
    /// <summary>The ten features, as ML.NET's normalizers want them: one vector column of <c>float</c>.</summary>
    [VectorType(10)]
    public float[] Features { get; set; } = [];
#pragma warning restore CA1819
}
