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
/// Encoding categories and filling gaps: <see cref="Encoders"/> and <see cref="SimpleImputer"/>
/// against ML.NET 5.0.0's <c>OneHotEncoding</c> and <c>ReplaceMissingValues</c> (#764).
/// </summary>
/// <remarks>
/// Its estimator is lazy, so it appears as a fit and as a fit whose values are read back — the only
/// one a caller can use. The <c>IDataView</c> is built in <c>GlobalSetup</c> and excluded, so ML.NET
/// is not charged for its own entry cost.
/// </remarks>

// long-comment: why this class pins its job, as ScalerIncumbentBenchmarks does.
// The rows span tens of microseconds to tens of milliseconds and each allocates a fresh matrix.
// Left to size itself the engine puts thousands of invocations in an iteration, and the warmup
// never settles as the collector falls behind. One invocation per iteration with a real warmup is
// what measures this shape.
[SimpleJob(
    RunStrategy.Throughput,
    launchCount: 1,
    warmupCount: 5,
    iterationCount: 20,
    invocationCount: 1)]
[MemoryDiagnoser]
public class EncoderIncumbentBenchmarks
{
    // long-comment: why the categorical side is one feature and the numeric side four.
    // ML.NET's OneHotEncoding takes one named column, so encoding four features here against one
    // there would have charged this package four times the work for the same row -- measured, that
    // alone turned a win into a 2.8x loss at twenty thousand rows. Its ReplaceMissingValues takes a
    // vector column, so the imputer rows do compare four against four.
    private const int CategoricalFeatures = 1;
    private const int NumericFeatures = 4;
    private const int Categories = 20;
    private const string Column = "Category";
    private const string NumbersColumn = "Numbers";

    private MLContext _context = null!;
    private IDataView _categorical = null!;
    private IDataView _numeric = null!;
    private string[] _values = [];
    private double[] _samples = [];

    /// <summary>How many rows to encode, at four features each.</summary>
    [Params(1_000, 20_000)]
    public int RowCount { get; set; } = 1_000;

    // SonarLint S2245, CA5394: a seeded Random builds a reproducible benchmark corpus; no security use.
#pragma warning disable S2245, CA5394
    [GlobalSetup]
    public void Setup()
    {
        var random = new Random(764);
        _values = new string[RowCount * CategoricalFeatures];
        _samples = new double[RowCount * NumericFeatures];
        var categorical = new CategoryRow[RowCount];
        var numeric = new NumericRow[RowCount];
        for (int row = 0; row < RowCount; row++)
        {
            _values[row] = $"c{random.Next(Categories)}";
            categorical[row] = new CategoryRow { Category = _values[row] };

            var values = new float[NumericFeatures];
            for (int feature = 0; feature < NumericFeatures; feature++)
            {
                // One value in twenty is missing, which is what the imputer is for.
                int position = (row * NumericFeatures) + feature;
                _samples[position] = random.Next(20) == 0 ? double.NaN : random.NextDouble();
                values[feature] = (float)(double.IsNaN(_samples[position]) ? float.NaN : _samples[position]);
            }

            numeric[row] = new NumericRow { Numbers = values };
        }

        _context = new MLContext(seed: 764);
        _categorical = _context.Data.LoadFromEnumerable(categorical);
        _numeric = _context.Data.LoadFromEnumerable(numeric);
    }
#pragma warning restore S2245, CA5394

    [Benchmark(Baseline = true)]
    public double[] Lodestar_OneHot() =>
        Encoders.OneHot(_values, CategoricalFeatures).Transform(_values);

    [Benchmark]
    public double[] Lodestar_Ordinal() =>
        Encoders.Ordinal(_values, CategoricalFeatures).Transform(_values);

    [Benchmark]
    public double[] Lodestar_Impute() =>
        SimpleImputer.Fit(_samples, NumericFeatures).Transform(_samples);

    [Benchmark]
    public ITransformer MlNet_OneHotEncoding_Fit() =>
        _context.Transforms.Categorical.OneHotEncoding(Column).Fit(_categorical);

    /// <summary>The same fit, with the encoded values read — which is where ML.NET does the work.</summary>
    [Benchmark]
    public int MlNet_OneHotEncoding_Read()
    {
        OneHotEncodingTransformer model = _context.Transforms.Categorical.OneHotEncoding(Column).Fit(_categorical);
        return MlNetCursor.Drain(model.Transform(_categorical));
    }

    [Benchmark]
    public int MlNet_ReplaceMissingValues_Read()
    {
        MissingValueReplacingTransformer model = _context.Transforms.ReplaceMissingValues(NumbersColumn).Fit(_numeric);
        return MlNetCursor.Drain(model.Transform(_numeric));
    }

}

/// <summary>One categorical row, as ML.NET's encoder wants it: a named text column.</summary>
public sealed class CategoryRow
{
    /// <summary>The category, which ML.NET addresses by name rather than by position.</summary>
    public string Category { get; set; } = string.Empty;
}

/// <summary>One numeric row, as ML.NET's imputer wants it: a named vector column.</summary>
public sealed class NumericRow
{
    // CA1819 (properties should not return arrays): ML.NET binds a vector column to a float[]
    // property by reflection, and a read-only view is not a shape its loader accepts.
#pragma warning disable CA1819
    /// <summary>The four values, with a NaN where one is missing.</summary>
    /// <remarks>Four spelled out: an attribute argument is a constant, and the benchmark's own is out of scope here.</remarks>
    [VectorType(4)]
    public float[] Numbers { get; set; } = [];
#pragma warning restore CA1819
}
