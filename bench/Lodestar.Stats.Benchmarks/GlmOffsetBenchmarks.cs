using BenchmarkDotNet.Attributes;
using Lodestar.Stats.Regression;

namespace Lodestar.Stats.Benchmarks;

// SonarLint S2245: a seeded Random builds a reproducible benchmark corpus; no security use.
#pragma warning disable S2245, CA5394

// CA1822 (mark members static): BenchmarkDotNet rejects static benchmarks, as GlmBenchmarks records.
#pragma warning disable CA1822

/// <summary>What an exposure costs a Poisson fit, beside the same fit without one (#787).</summary>
/// <remarks>
/// A class of its own so <see cref="GlmPoissonBenchmarks"/> keeps the source it has on <c>main</c> and an A/B/A of it
/// reads the shared IRLS loop alone. The exposure adds a term per row to two loops and a second, intercept-only IRLS
/// fit for the null deviance; the incumbent is <c>statsmodels</c> through <c>compare-glm</c>, and this prices the
/// allocations that harness does not see.
/// </remarks>
[MemoryDiagnoser]
public class GlmOffsetBenchmarks
{
    private double[] _design = [];
    private double[] _response = [];
    private double[] _exposure = [];

    [Params(200, 20_000)]
    public int SampleSize { get; set; }

    [GlobalSetup]
    public void Setup()
    {
        var random = new Random(787);
        _design = new double[SampleSize];
        _response = new double[SampleSize];
        _exposure = new double[SampleSize];
        for (int row = 0; row < SampleSize; row++)
        {
            double x = random.NextDouble();
            double years = 0.5 + (3.5 * random.NextDouble());
            double mean = years * Math.Exp(0.2 + (0.8 * x));
            _design[row] = x;
            _exposure[row] = years;

            // Knuth's multiplication method: the means stay below 25.
            double limit = Math.Exp(-mean);
            double product = random.NextDouble();
            int count = 0;
            while (product > limit)
            {
                product *= random.NextDouble();
                count++;
            }

            _response[row] = count;
        }

        // An exposure of ones is the fit without one but for the null deviance's refit: a gap in the coefficients
        // would time a model that is not the one this class claims to price.
        double without = GeneralizedLinearModel.Fit(_design, _response, 1, GlmFamily.Poisson).Coefficients[1];
        double ones = GeneralizedLinearModel.Fit(
            _design, _response, [], Enumerable.Repeat(1.0, SampleSize).ToArray(), 1, GlmFamily.Poisson).Coefficients[1];
        if (!(Math.Abs(without - ones) <= 1e-12 * Math.Abs(without)))
        {
            throw new InvalidOperationException($"An exposure of ones gives {ones:R}, none gives {without:R}.");
        }
    }

    [Benchmark(Baseline = true)]
    public double Poisson() => GeneralizedLinearModel.Fit(_design, _response, 1, GlmFamily.Poisson).Akaike;

    [Benchmark]
    public double PoissonWithExposure() =>
        GeneralizedLinearModel.Fit(_design, _response, [], _exposure, 1, GlmFamily.Poisson).Akaike;
}
