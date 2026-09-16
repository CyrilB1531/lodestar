using BenchmarkDotNet.Attributes;
using Lodestar.Metrics;

namespace Lodestar.Text.Benchmarks;

// SonarLint S2245, CA5394: a seeded Random builds reproducible labellings; no security use.
#pragma warning disable S2245, CA5394

// CA1822: BenchmarkDotNet rejects static benchmarks.
#pragma warning disable CA1822

/// <summary>The clustering agreement scores over one contingency table, at two cluster counts.</summary>
/// <remarks>
/// Every score here builds the same sparse table, so its cost is the table's: one lookup per sample
/// into a map keyed by (true, predicted) pair, whose spread depends on how the key hashes.
/// </remarks>
[MemoryDiagnoser]
public class ClusteringAgreementBenchmarks
{
    private int[] _true = [];
    private int[] _predicted = [];

    /// <summary>How many samples.</summary>
    [Params(100_000)]
    public int Samples { get; set; }

    /// <summary>How many clusters on each side, so up to its square in cells.</summary>
    [Params(10, 100)]
    public int Clusters { get; set; }

    [GlobalSetup]
    public void Setup()
    {
        var random = new Random(2026);
        _true = [.. Enumerable.Range(0, Samples).Select(_ => random.Next(Clusters))];
        _predicted = [.. Enumerable.Range(0, Samples).Select(_ => random.Next(Clusters))];
    }

    [Benchmark]
    public double AdjustedRandScore() => AdjustedRand.Score(_true, _predicted);

    [Benchmark]
    public double MutualInformationScore() => MutualInformation.Score(_true, _predicted);
}
