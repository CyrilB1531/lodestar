using Accord.Statistics.Links;
using Accord.Statistics.Models.Regression;
using Accord.Statistics.Models.Regression.Fitting;
using BenchmarkDotNet.Attributes;
using Lodestar.Stats.Regression;

namespace Lodestar.Stats.Benchmarks;

// SonarLint S2245: a seeded Random builds a reproducible benchmark corpus; no security use.
#pragma warning disable S2245, CA5394

// CA1822 (mark members static): BenchmarkDotNet rejects static benchmarks, and the
// build succeeds either way -- so following the rule breaks the run, not the compile.
#pragma warning disable CA1822

// CS0618: Accord.Statistics 3.8.0's own recommended replacement, Learn(x, y), loops past any
// bound this project could measure on the corpus below -- observed directly, not assumed --
// where the constructor-and-Run pair this benchmark uses accepts the same MaximumIterations
// and Tolerance budget GeneralizedLinearModel.Fit is given, which is what makes the two rows
// comparable at all.
#pragma warning disable CS0618

/// <summary>
/// The GLM inference table against Accord.Statistics, the one .NET library that carried it.
/// </summary>
/// <remarks>
/// Decision 0111 kept the generalized linear model inside <c>Lodestar.Stats.Regression</c>
/// rather than a new package, on the same reading decision 0096 did for OLS: Accord is
/// archived (last published 2017) and LGPL-2.1, which bars it from src/ and not from a
/// benchmark project that ships nothing -- the same footing as OlsBenchmarks beside it. The
/// shapes differ on purpose: a row-major span against the jagged array Accord takes.
/// </remarks>
[MemoryDiagnoser]
public class GlmBenchmarks
{
    private double[] _design = [];
    private double[] _response = [];
    private double[][] _jagged = [];

    /// <summary>Rows.</summary>
    [Params(200, 2_000)]
    public int SampleSize { get; set; }

    /// <summary>Regressors, excluding the intercept both sides fit.</summary>
    [Params(1, 3)]
    public int Regressors { get; set; }

    private const int MaximumIterations = 100;
    private const double Tolerance = 1e-8;

    [GlobalSetup]
    public void Setup()
    {
        var random = new Random(616);
        _design = new double[SampleSize * Regressors];
        _response = new double[SampleSize];
        _jagged = new double[SampleSize][];

        for (int row = 0; row < SampleSize; row++)
        {
            var line = new double[Regressors];
            double logit = -0.5;
            for (int column = 0; column < Regressors; column++)
            {
                double value = random.NextDouble();
                line[column] = value;
                _design[(row * Regressors) + column] = value;
                logit += value - 0.5;
            }

            _jagged[row] = line;
            double probability = 1.0 / (1.0 + Math.Exp(-logit));
            _response[row] = random.NextDouble() < probability ? 1.0 : 0.0;
        }
    }

    [Benchmark(Baseline = true)]
    public double Lodestar_Glm() =>
        GeneralizedLinearModel.Fit(_design, _response, Regressors, GlmFamily.Binomial).PValues[1];

    [Benchmark]
    public double Accord_Glm()
    {
        var regression = new GeneralizedLinearRegression(new LogitLinkFunction())
        {
            NumberOfInputs = Regressors,
        };
        var irls = new IterativeReweightedLeastSquares(regression);
        double delta = double.PositiveInfinity;
        int iteration = 0;
        while (delta > Tolerance && iteration < MaximumIterations)
        {
            delta = irls.Run(_jagged, _response);
            iteration++;
        }

        return regression.GetWaldTest(1).PValue;
    }

    // Both rows price the whole IRLS loop to convergence: Fit builds its table alongside it,
    // and Accord's Run/GetWaldTest pair does the same, one coefficient at a time.
}
