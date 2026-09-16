using BenchmarkDotNet.Attributes;
using Lodestar.Stats;

namespace Lodestar.Stats.Benchmarks;

// SonarLint S2245, CA5394: a seeded Random builds a reproducible corpus; no security use.
#pragma warning disable S2245, CA5394

// CA1822: BenchmarkDotNet rejects static benchmarks — see StatsBenchmarks.
#pragma warning disable CA1822

/// <summary>The exact Mann-Whitney distribution at a balanced and a lopsided pair of sizes.</summary>
[MemoryDiagnoser]
public class MannWhitneyExactBenchmarks
{
    private double[] _x = [];
    private double[] _y = [];

    /// <summary>The two sample sizes, "n x m"; both products sit at the exact bound.</summary>
    [Params("8x2500", "141x141")]
    public string Shape { get; set; } = "8x2500";

    [GlobalSetup]
    public void Setup()
    {
        string[] parts = Shape.Split('x');
        int n = int.Parse(parts[0], System.Globalization.CultureInfo.InvariantCulture);
        int m = int.Parse(parts[1], System.Globalization.CultureInfo.InvariantCulture);
        var random = new Random(711);
        _x = [.. Enumerable.Range(0, n).Select(_ => random.NextDouble())];
        _y = [.. Enumerable.Range(0, m).Select(_ => random.NextDouble() + 0.05)];
    }

    [Benchmark]
    public double Exact() => MannWhitney.Test(_x, _y, method: ExactMethod.Exact).PValue;
}
