using Accord.Statistics.Analysis;
using BenchmarkDotNet.Attributes;
using Lodestar.Stats.Regression;

namespace Lodestar.Stats.Benchmarks;

// SonarLint S2245: a seeded Random builds a reproducible benchmark corpus; no security use.
#pragma warning disable S2245, CA5394

// CA1822 (mark members static): BenchmarkDotNet rejects static benchmarks, and the
// build succeeds either way -- so following the rule breaks the run, not the compile.
#pragma warning disable CA1822

/// <summary>
/// The OLS summary table against Accord.Statistics, the one .NET library that carried it.
/// </summary>
/// <remarks>
/// Decision 0096's reading found this incumbent where the issue expected none, so the
/// comparison is a measurement rather than an explanation of its absence. Accord is
/// archived (last published 2017) and LGPL-2.1, which bars it from src/ and not from a
/// benchmark project that ships nothing -- the same footing as StatsBenchmarks beside it.
/// The shapes differ on purpose: a row-major span against the jagged array Accord takes.
/// </remarks>
[MemoryDiagnoser]
public class OlsBenchmarks
{
    private double[] _design = [];
    private double[] _response = [];
    private double[][] _jagged = [];

    /// <summary>Rows. Four regressors throughout, which is where a table is read.</summary>
    [Params(100, 10_000)]
    public int SampleSize { get; set; }

    private const int Regressors = 4;

    [GlobalSetup]
    public void Setup()
    {
        Random random = new(566);
        _design = new double[SampleSize * Regressors];
        _response = new double[SampleSize];
        _jagged = new double[SampleSize][];

        for (int row = 0; row < SampleSize; row++)
        {
            var line = new double[Regressors];
            double signal = 0.0;
            for (int column = 0; column < Regressors; column++)
            {
                double value = random.NextDouble();
                line[column] = value;
                _design[(row * Regressors) + column] = value;
                signal += (column + 1) * value;
            }

            _jagged[row] = line;
            _response[row] = signal + (random.NextDouble() - 0.5);
        }
    }

    [Benchmark(Baseline = true)]
    public double Lodestar_Ols() =>
        OrdinaryLeastSquares.Fit(_design, _response, Regressors).PValues[1];

    [Benchmark]
    public double Accord_Ols()
    {
        var analysis = new MultipleLinearRegressionAnalysis(intercept: true);
        analysis.Learn(_jagged, _response);
        return analysis.Coefficients[1].TTest.PValue;
    }

    // Both rows price the whole table, which is the only shape either library offers:
    // Fit computes the covariance and the VIFs, and Accord's Learn computes the analysis.
}
