using BenchmarkDotNet.Attributes;
using Lodestar.Stats.TimeSeries;

namespace Lodestar.Stats.Benchmarks;

// SonarLint S2245: a seeded Random builds a reproducible benchmark corpus; no security use.
#pragma warning disable S2245, CA5394

// CA1822 (mark members static): BenchmarkDotNet rejects static benchmarks, as GlmBenchmarks records.
#pragma warning disable CA1822

/// <summary>What a vector autoregression costs, by system size and lag order (#786).</summary>
/// <remarks>
/// No .NET library estimates one (decision 0134), so the incumbent is <c>statsmodels</c> through <c>compare-var</c> and
/// this class prices the allocations that harness does not see. The work is one least squares per equation over a
/// design of <c>1 + K·p</c> columns, so both parameters move it.
/// </remarks>
[MemoryDiagnoser]
public class VectorAutoregressionBenchmarks
{
    private double[] _series = [];

    /// <summary>Observations in the series.</summary>
    [Params(500, 5_000)]
    public int SampleSize { get; set; }

    /// <summary>Variables in the system.</summary>
    [Params(2, 5)]
    public int Variables { get; set; }

    /// <summary>Lags each equation carries.</summary>
    [Params(1, 4)]
    public int Lags { get; set; }

    [GlobalSetup]
    public void Setup()
    {
        var random = new Random(786);
        _series = new double[SampleSize * Variables];
        for (int observation = 1; observation < SampleSize; observation++)
        {
            int at = observation * Variables;
            int previous = at - Variables;
            for (int variable = 0; variable < Variables; variable++)
            {
                // A stable system: each variable keeps a third of its own past and a tenth of its neighbour's.
                double carried = (0.33 * _series[previous + variable])
                    + (0.1 * _series[previous + ((variable + 1) % Variables)]);
                _series[at + variable] = carried + ((random.NextDouble() - 0.5) * 0.5);
            }
        }
    }

    [Benchmark]
    public double Fit() => VectorAutoregression.Fit(_series, Variables, Lags).Akaike;
}
