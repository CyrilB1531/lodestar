using BenchmarkDotNet.Attributes;
using Lodestar.Stats;

namespace Lodestar.Stats.Benchmarks;

// SonarLint S2245, CA5394: a seeded Random builds a reproducible corpus; no security use.
#pragma warning disable S2245, CA5394

// CA1822: BenchmarkDotNet rejects static benchmarks — see StatsBenchmarks.
#pragma warning disable CA1822

/// <summary>The exact Kolmogorov-Smirnov table walk, for unequal sizes where no closed form applies.</summary>
[MemoryDiagnoser]
public class KsExactTableBenchmarks
{
    private double[] _a = [];
    private double[] _b = [];

    /// <summary>The two sample sizes, "n x m".</summary>
    [Params("99x101", "999x1001")]
    public string Shape { get; set; } = "99x101";

    [GlobalSetup]
    public void Setup()
    {
        string[] parts = Shape.Split('x');
        int n = int.Parse(parts[0], System.Globalization.CultureInfo.InvariantCulture);
        int m = int.Parse(parts[1], System.Globalization.CultureInfo.InvariantCulture);
        var random = new Random(803);
        _a = [.. Enumerable.Range(0, n).Select(_ => random.NextDouble())];
        _b = [.. Enumerable.Range(0, m).Select(_ => random.NextDouble() + 0.02)];
    }

    [Benchmark]
    public double TwoSidedExact() => KolmogorovSmirnov.TwoSample(_a, _b, method: ExactMethod.Exact).PValue;
}
