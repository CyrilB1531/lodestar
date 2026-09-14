using BenchmarkDotNet.Attributes;
using Lodestar.Stats.Regression;

namespace Lodestar.Stats.Benchmarks;

// SonarLint S2245: a seeded Random builds a reproducible benchmark corpus; no security use.
#pragma warning disable S2245, CA5394

// CA1822 (mark members static): BenchmarkDotNet rejects static benchmarks, as GlmBenchmarks records.
#pragma warning disable CA1822

/// <summary>A Poisson fit at three count magnitudes, the one axis the log-likelihood's <c>log(y!)</c> sees (#665).</summary>
/// <remarks>
/// <see cref="GlmBenchmarks"/> is logistic and races Accord. Here the response's mean decides what
/// <c>LogLikelihood.LogFactorial</c> does: 5 stays on its table, 50,000 takes the series, and
/// 5,000,000 is past the million the fit refused until #665. Counts are drawn as rounded normals
/// above 30, which is a benchmark corpus and not a Poisson sampler anyone should reuse.
/// </remarks>
[MemoryDiagnoser]
public class GlmPoissonBenchmarks
{
    private const int SampleSize = 2_000;
    private double[] _design = [];
    private double[] _response = [];

    [Params(5.0, 50_000.0, 5_000_000.0)]
    public double MeanCount { get; set; }

    [GlobalSetup]
    public void Setup()
    {
        var random = new Random(665);
        _design = new double[SampleSize];
        _response = new double[SampleSize];
        for (int row = 0; row < SampleSize; row++)
        {
            double x = random.NextDouble();
            double mean = MeanCount * Math.Exp(0.3 * (x - 0.5));
            _design[row] = x;
            _response[row] = Draw(random, mean);
        }
    }

    private static double Draw(Random random, double mean)
    {
        if (mean > 30.0)
        {
            double normal = Math.Sqrt(-2.0 * Math.Log(1.0 - random.NextDouble())) * Math.Cos(2.0 * Math.PI * random.NextDouble());
            return Math.Max(0.0, Math.Round(mean + (Math.Sqrt(mean) * normal)));
        }

        // Knuth's multiplication method, for the small mean.
        double limit = Math.Exp(-mean);
        double product = random.NextDouble();
        int count = 0;
        while (product > limit)
        {
            product *= random.NextDouble();
            count++;
        }

        return count;
    }

    [Benchmark]
    public double Fit() => GeneralizedLinearModel.Fit(_design, _response, 1, GlmFamily.Poisson).Akaike;
}
