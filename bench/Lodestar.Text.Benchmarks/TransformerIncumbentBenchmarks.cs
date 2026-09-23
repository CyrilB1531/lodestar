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
/// Reshaping a feature: the #1122 transformers against the two ML.NET 5.0.0 estimators that
/// answer the same question — <c>NormalizeLpNorm</c> and <c>NormalizeBinning</c> (#1122).
/// </summary>
/// <remarks>
/// The other five have no ML.NET counterpart at all, so they are measured against themselves here
/// and against scikit-learn through <c>compare-transformers</c>; <c>bench/README.md</c> says which
/// row answers which question. ML.NET's estimators are lazy, so each appears as a fit whose rows
/// are read — the only shape a caller can use.
/// </remarks>

// long-comment: why this class pins its job, as EncoderIncumbentBenchmarks does.
// PowerTransformer fits an exponent by iterated likelihood and KnnImputer is quadratic in the
// rows, so one row of this class costs milliseconds where another costs microseconds. Left to
// size itself the engine puts thousands of invocations of the cheap rows in one iteration and a
// single invocation of the expensive ones, and the warmup never settles.
[SimpleJob(
    RunStrategy.Throughput,
    launchCount: 1,
    warmupCount: 5,
    iterationCount: 20,
    invocationCount: 1)]
[MemoryDiagnoser]
public class TransformerIncumbentBenchmarks
{
    private const int Features = 4;
    private const string VectorColumn = "Numbers";

    // long-comment: why the imputer row count is pinned and the others are not.
    // KnnImputer compares every receiving row with every fitted row, so its cost is quadratic
    // where every other row here is linear. At 20,000 rows that is 1.6 billion distance terms --
    // past the ceiling the type itself refuses at -- so the imputer reads a fixed slice and the
    // RowCount parameter scales the rest.
    private const int ImputerRows = 2_000;

    private MLContext _context = null!;
    private IDataView _view = null!;
    private double[] _samples = [];
    private double[] _withGaps = [];
    private KBinsDiscretizer _bins = null!;
    private QuantileTransformer _quantiles = null!;
    private PowerTransformer _power = null!;
    private KnnImputer _imputer = null!;

    /// <summary>How many rows to transform, at four features each.</summary>
    [Params(1_000, 20_000)]
    public int RowCount { get; set; } = 1_000;

    // SonarLint S2245, CA5394: a seeded Random builds a reproducible benchmark corpus; no security use.
#pragma warning disable S2245, CA5394
    [GlobalSetup]
    public void Prepare()
    {
        var random = new Random(1122);
        _samples = new double[RowCount * Features];
        _withGaps = new double[ImputerRows * Features];
        var rows = new NumericRow[RowCount];
        for (int row = 0; row < RowCount; row++)
        {
            var values = new float[Features];
            for (int feature = 0; feature < Features; feature++)
            {
                // Strictly positive and right-skewed: what PowerTransformer is for, and what
                // both families accept.
                double value = Math.Exp(random.NextDouble() * 4.0);
                _samples[(row * Features) + feature] = value;
                values[feature] = (float)value;
            }

            rows[row] = new NumericRow { Numbers = values };
        }

        // Its own values rather than a slice of the matrix above: the imputer's row count is
        // fixed where RowCount is not, so a slice would read past the end at the smaller size.
        for (int row = 0; row < ImputerRows * Features; row++)
        {
            // One value in twenty missing, which is the shape an imputer is called on.
            _withGaps[row] = random.Next(20) == 0 ? double.NaN : Math.Exp(random.NextDouble() * 4.0);
        }

        _context = new MLContext(seed: 1122);
        _view = _context.Data.LoadFromEnumerable(rows);

        // The fitted objects are built once: this class times the transform, and the fits are
        // measured by their own rows below.
        _bins = KBinsDiscretizer.Fit(_samples, Features);
        _quantiles = QuantileTransformer.Fit(_samples, Features);
        _power = PowerTransformer.Fit(_samples, Features);
        _imputer = KnnImputer.Fit(_withGaps, Features);
    }
#pragma warning restore S2245, CA5394

    [Benchmark(Baseline = true)]
    public double[] Lodestar_Normalize() => Normalizer.Transform(_samples, Features);

    [Benchmark]
    public double[] Lodestar_Polynomial() => PolynomialFeatures.Transform(_samples, Features);

    [Benchmark]
    public double[] Lodestar_Discretize() => _bins.Transform(_samples);

    [Benchmark]
    public KBinsDiscretizer Lodestar_Discretize_Fit() => KBinsDiscretizer.Fit(_samples, Features);

    [Benchmark]
    public double[] Lodestar_Quantile() => _quantiles.Transform(_samples);

    [Benchmark]
    public QuantileTransformer Lodestar_Quantile_Fit() => QuantileTransformer.Fit(_samples, Features);

    [Benchmark]
    public double[] Lodestar_Power() => _power.Transform(_samples);

    /// <summary>The exponent search, which is the expensive half of this family.</summary>
    [Benchmark]
    public PowerTransformer Lodestar_Power_Fit() => PowerTransformer.Fit(_samples, Features);

    /// <summary>Quadratic in the rows, so pinned at <see cref="ImputerRows"/> on every parameter.</summary>
    [Benchmark]
    public double[] Lodestar_KnnImpute() => _imputer.Transform(_withGaps);

    [Benchmark]
    public int MlNet_NormalizeLpNorm_Read()
    {
        LpNormNormalizingTransformer model = _context.Transforms.NormalizeLpNorm(VectorColumn).Fit(_view);
        return MlNetCursor.Drain(model.Transform(_view));
    }

    [Benchmark]
    public int MlNet_NormalizeBinning_Read()
    {
        NormalizingTransformer model =
            _context.Transforms.NormalizeBinning(VectorColumn, maximumBinCount: 5).Fit(_view);
        return MlNetCursor.Drain(model.Transform(_view));
    }
}
