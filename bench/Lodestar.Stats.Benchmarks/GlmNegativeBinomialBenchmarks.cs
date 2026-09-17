using BenchmarkDotNet.Attributes;
using Lodestar.Stats.Regression;

namespace Lodestar.Stats.Benchmarks;

// SonarLint S2245: a seeded Random builds a reproducible benchmark corpus; no security use.
#pragma warning disable S2245, CA5394

// CA1822 (mark members static): BenchmarkDotNet rejects static benchmarks, as GlmBenchmarks records.
#pragma warning disable CA1822

/// <summary>A negative binomial fit at α = 1, whose log-likelihood reads <c>lnΓ(y + θ)</c> once per row.</summary>
/// <remarks>
/// At θ = 1 and small counts each of those raises its argument to 40 one logarithm at a time, so the row count is
/// the axis. Counts are Poisson draws at a gamma-distributed mean, which is the family's own mixture; the incumbent
/// is <c>statsmodels</c> through the cross-language harness, and this class prices the package against itself.
/// </remarks>
[MemoryDiagnoser]
public class GlmNegativeBinomialBenchmarks
{
    private double[] _design = [];
    private double[] _response = [];

    [Params(1_000, 10_000, 100_000)]
    public int SampleSize { get; set; }

    [GlobalSetup]
    public void Setup()
    {
        var random = new Random(781);
        _design = new double[SampleSize];
        _response = new double[SampleSize];
        for (int row = 0; row < SampleSize; row++)
        {
            double x = random.NextDouble();

            // An exponential multiplier is a gamma of shape 1/α = 1, which makes the Poisson draw negative binomial.
            double mean = Math.Exp(0.5 + (0.8 * x)) * -Math.Log(1.0 - random.NextDouble());
            double limit = Math.Exp(-mean);
            double product = random.NextDouble();
            int count = 0;
            while (product > limit)
            {
                product *= random.NextDouble();
                count++;
            }

            _design[row] = x;
            _response[row] = count;
        }
    }

    [Benchmark]
    public double Fit() => GeneralizedLinearModel.Fit(
        _design, _response, 1, GlmFamily.NegativeBinomial, new GlmOptions { NegativeBinomialAlpha = 1.0 }).Akaike;
}
