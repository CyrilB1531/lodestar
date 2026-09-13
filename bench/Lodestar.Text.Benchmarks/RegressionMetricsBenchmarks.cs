using BenchmarkDotNet.Attributes;
using Lodestar.Metrics;

namespace Lodestar.Text.Benchmarks;

// SonarLint S2245: a seeded Random builds a reproducible benchmark corpus; no security use.
#pragma warning disable S2245, CA5394

/// <summary>
/// The four regression rows of the cross-language metrics table, timed by BenchmarkDotNet
/// on the corpus's own distribution: truth uniform on [0.5, 100), prediction the truth plus
/// Gaussian noise of standard deviation 5 (<c>bench/corpus/generate_metrics.py</c>).
/// </summary>
[MemoryDiagnoser]
public class RegressionMetricsBenchmarks
{
    private double[] _yTrue = [];
    private double[] _yPred = [];

    [Params(100_000, 1_000_000)]
    public int Samples { get; set; }

    [GlobalSetup]
    public void Setup()
    {
        var rng = new Random(20260913);
        _yTrue = new double[Samples];
        _yPred = new double[Samples];
        for (int i = 0; i < Samples; i++)
        {
            _yTrue[i] = 0.5 + (rng.NextDouble() * 99.5);

            // Box-Muller: Random has no Gaussian draw on netstandard2.0, which links this file.
            double gauss = Math.Sqrt(-2.0 * Math.Log(1.0 - rng.NextDouble())) * Math.Cos(2.0 * Math.PI * rng.NextDouble());
            _yPred[i] = _yTrue[i] + (5.0 * gauss);
        }
    }

    [Benchmark]
    public double Mse() => MeanSquaredError.Score(_yTrue, _yPred);

    [Benchmark]
    public double Mae() => MeanAbsoluteError.Score(_yTrue, _yPred);

    [Benchmark]
    public double R2Score() => R2.Score(_yTrue, _yPred);

    [Benchmark]
    public double MedianAe() => MedianAbsoluteError.Score(_yTrue, _yPred);
}
