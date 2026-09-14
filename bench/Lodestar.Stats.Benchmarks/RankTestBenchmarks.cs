using BenchmarkDotNet.Attributes;
using Lodestar.Stats;

namespace Lodestar.Stats.Benchmarks;

// SonarLint S2245: a seeded Random builds a reproducible benchmark corpus; no security use.
#pragma warning disable S2245, CA5394

// CA1822 (mark members static): BenchmarkDotNet rejects static benchmarks, as StatsBenchmarks records.
#pragma warning disable CA1822

/// <summary>The three rank tests at sizes where ranking is the cost, with and without ties (#719).</summary>
/// <remarks>
/// <see cref="StatsBenchmarks"/> stops at 10,000 and races Accord; this class does neither. Each
/// sample holds <see cref="SampleSize"/> values, Kruskal-Wallis three groups of that size. With
/// <see cref="Ties"/> the values are rounded to hundredths, so most of them share a rank.
/// Mann-Whitney is the control: #711 already ranks it by merging sorted samples.
/// </remarks>
[MemoryDiagnoser]
public class RankTestBenchmarks
{
    private double[] _x = [];
    private double[] _y = [];
    private double[] _z = [];

    [Params(10_000, 100_000)]
    public int SampleSize { get; set; }

    [Params(false, true)]
    public bool Ties { get; set; }

    [GlobalSetup]
    public void Setup()
    {
        var random = new Random(719);
        _x = Draw(random, 0.0);
        _y = Draw(random, 0.05);
        _z = Draw(random, 0.1);
    }

    private double[] Draw(Random random, double shift)
    {
        double[] values = new double[SampleSize];
        for (int i = 0; i < values.Length; i++)
        {
            double value = random.NextDouble() + shift;
            values[i] = Ties ? Math.Round(value, 2) : value;
        }
        return values;
    }

    [Benchmark]
    public double KruskalWallisTest() => KruskalWallis.Test(_x, _y, _z).PValue;

    [Benchmark]
    public double WilcoxonPaired() => Wilcoxon.Paired(_x, _y).PValue;

    [Benchmark]
    public double MannWhitneyTest() => MannWhitney.Test(_x, _y).PValue;
}
