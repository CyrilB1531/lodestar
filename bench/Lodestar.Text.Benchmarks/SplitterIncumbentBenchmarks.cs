using BenchmarkDotNet.Attributes;
using Lodestar.Preprocessing;
using Microsoft.ML;
using Microsoft.ML.Data;

namespace Lodestar.Text.Benchmarks;

// CA1822: see LevenshteinIncumbentBenchmarks.
#pragma warning disable CA1822

/// <summary>
/// What a caller pays to cut five cross-validation folds: <see cref="Splitters"/> against ML.NET
/// 5.0.0's <c>CrossValidationSplit</c> and <c>TrainTestSplit</c>, the only splitters in .NET (#762).
/// </summary>
/// <remarks>
/// Not like-for-like: ML.NET hashes a generated sampling key, so its folds are not reproducible
/// and it never stratifies (<see href="https://github.com/dotnet/machinelearning/issues/4396"/>).
/// Its split is lazy too, so the call alone builds ten wrappers and the <c>_Read</c> rows are the
/// ones that decide which rows a fold holds. <c>bench/README.md</c> §44 has the rest.
/// </remarks>
[MemoryDiagnoser]
public class SplitterIncumbentBenchmarks
{
    private const int FoldCount = 5;
    private const double TestFraction = 0.25;

    private MLContext _context = null!;
    private IDataView _data = null!;
    private int[] _labels = null!;

    /// <summary>How many rows to split.</summary>
    [Params(10_000, 100_000)]
    public int SampleCount { get; set; } = 10_000;

    [GlobalSetup]
    public void Setup()
    {
        _labels = new int[SampleCount];
        var rows = new SplitRow[SampleCount];
        for (int row = 0; row < SampleCount; row++)
        {
            // Three classes at 60 / 30 / 10, the cross-language harness's rule.
            int remainder = row % 10;
            int label = 0;
            if (remainder >= 9)
            {
                label = 2;
            }
            else if (remainder >= 6)
            {
                label = 1;
            }

            _labels[row] = label;
            rows[row] = new SplitRow { Feature = row, Label = label };
        }

        _context = new MLContext(seed: 762);
        _data = _context.Data.LoadFromEnumerable(rows);
    }

    [Benchmark(Baseline = true)]
    public IReadOnlyList<FoldSplit> Lodestar_KFold() => Splitters.KFold(SampleCount, FoldCount);

    [Benchmark]
    public IReadOnlyList<FoldSplit> Lodestar_StratifiedKFold() => Splitters.StratifiedKFold(_labels, FoldCount);

    [Benchmark]
    public TrainTestSplit Lodestar_TrainTest() => Splitters.TrainTest(SampleCount, TestFraction);

    [Benchmark]
    public IReadOnlyList<DataOperationsCatalog.TrainTestData> MlNet_CrossValidationSplit() =>
        _context.Data.CrossValidationSplit(_data, FoldCount);

    /// <summary>The same call, with every fold read — which is the point at which the rows are decided.</summary>
    [Benchmark]
    public int MlNet_CrossValidationSplit_Read()
    {
        int seen = 0;
        foreach (DataOperationsCatalog.TrainTestData fold in _context.Data.CrossValidationSplit(_data, FoldCount))
        {
            seen += Count(fold.TestSet) + Count(fold.TrainSet);
        }

        return seen;
    }

    [Benchmark]
    public DataOperationsCatalog.TrainTestData MlNet_TrainTestSplit() =>
        _context.Data.TrainTestSplit(_data, TestFraction);

    [Benchmark]
    public int MlNet_TrainTestSplit_Read()
    {
        DataOperationsCatalog.TrainTestData split = _context.Data.TrainTestSplit(_data, TestFraction);
        return Count(split.TrainSet) + Count(split.TestSet);
    }

    private int Count(IDataView view)
    {
        int rows = 0;
        foreach (SplitRow _ in _context.Data.CreateEnumerable<SplitRow>(view, reuseRowObject: true))
        {
            rows++;
        }

        return rows;
    }
}

/// <summary>One row of the data view ML.NET splits; the shape is the framework's, not this package's.</summary>
public sealed class SplitRow
{
    /// <summary>A value to carry, so the view holds something to filter.</summary>
    public float Feature { get; set; }

    /// <summary>The class, as ML.NET's loader wants it: a number, not an integer label.</summary>
    public float Label { get; set; }
}
