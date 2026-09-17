using BenchmarkDotNet.Attributes;
using Lodestar.Metrics;

namespace Lodestar.Text.Benchmarks;

// SonarLint S2245, CA5394: a seeded Random builds reproducible rows; no security use.
#pragma warning disable S2245, CA5394

// CA1822: BenchmarkDotNet rejects static benchmarks.
#pragma warning disable CA1822

/// <summary>The ranked-row metrics over many short rows, where per-row allocation and sorting dominate.</summary>
/// <remarks>
/// Scores are rounded to two places so rows hold ties, which is the case the tie handling pays for.
/// </remarks>
[MemoryDiagnoser]
public class RankingMetricsBenchmarks
{
    private const int Labels = 10;
    private double[] _relevance = [];
    private double[] _scores = [];
    private bool[] _relevant = [];

    /// <summary>How many rows.</summary>
    [Params(100_000)]
    public int Rows { get; set; }

    [GlobalSetup]
    public void Setup()
    {
        var random = new Random(7);
        int size = Rows * Labels;
        _relevance = [.. Enumerable.Range(0, size).Select(_ => (double)random.Next(4))];
        _scores = [.. Enumerable.Range(0, size).Select(_ => Math.Round(random.NextDouble(), 2))];
        _relevant = [.. Enumerable.Range(0, size).Select(_ => random.Next(3) == 0)];
    }

    [Benchmark]
    public double NdcgTieAveraged() => Ndcg.Score(_relevance, _scores, Labels);

    [Benchmark]
    public double NdcgIgnoringTies() => Ndcg.Score(_relevance, _scores, Labels, ignoreTies: true);

    [Benchmark]
    public double DcgTieAveraged() => Dcg.Score(_relevance, _scores, Labels);

    [Benchmark]
    public double ReciprocalRankScore() => ReciprocalRank.Score(_relevance, _scores, Labels);

    [Benchmark]
    public double CoverageErrorScore() => CoverageError.Score(_relevant, _scores, Labels);

    [Benchmark]
    public double LabelRankingAveragePrecisionScore() =>
        LabelRankingAveragePrecision.Score(_relevant, _scores, Labels);
}
