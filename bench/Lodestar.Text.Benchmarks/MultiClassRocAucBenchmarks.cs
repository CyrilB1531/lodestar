using BenchmarkDotNet.Attributes;
using Lodestar.Metrics;

namespace Lodestar.Text.Benchmarks;

// SonarLint S2245, CA5394: a seeded Random builds reproducible probabilities; no security use.
#pragma warning disable S2245, CA5394

/// <summary>Multiclass ROC AUC on one thread, both reductions, so label handling and pair scans show beside the sorts.</summary>
[MemoryDiagnoser]
public class MultiClassRocAucBenchmarks
{
    private const int Classes = 10;
    private int[] _true = [];
    private double[] _scores = [];

    /// <summary>How many samples.</summary>
    [Params(100_000, 1_000_000)]
    public int Samples { get; set; }

    [GlobalSetup]
    public void Setup()
    {
        var random = new Random(23);
        _true = new int[Samples];
        _scores = new double[Samples * Classes];
        for (int i = 0; i < Samples; i++)
        {
            _true[i] = random.Next(Classes);
            double sum = 0.0;
            for (int c = 0; c < Classes; c++)
            {
                double value = random.NextDouble() + (c == _true[i] ? 0.5 : 0.0);
                _scores[(i * Classes) + c] = value;
                sum += value;
            }

            for (int c = 0; c < Classes; c++)
            {
                _scores[(i * Classes) + c] /= sum;
            }
        }
    }

    [Benchmark]
    public double OneVsRest() => RocAuc.MultiClass(_true, _scores, Classes);

    [Benchmark]
    public double OneVsOne() =>
        RocAuc.MultiClass(_true, _scores, Classes, new MultiClassRocOptions { Strategy = MultiClassStrategy.OneVsOne });
}
