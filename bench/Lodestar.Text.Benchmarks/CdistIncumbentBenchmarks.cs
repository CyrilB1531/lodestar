using BenchmarkDotNet.Attributes;
using Lodestar.Fuzzy;
using Raffinert.FuzzySharp.SimilarityRatio.Scorer.StrategySensitive;

namespace Lodestar.Text.Benchmarks;

// CA1822: see LevenshteinIncumbentBenchmarks.
#pragma warning disable CA1822

/// <summary>
/// <see cref="Process.Cdist"/> against the loop .NET forces today: Raffinert.FuzzySharp
/// publishes no matrix call at all, so its counterpart is `ExtractAll` once per query (#1123).
/// </summary>
/// <remarks>
/// Checked by reflection before this class was written: `Raffinert.FuzzySharp.Process` exports
/// `ExtractAll`, `ExtractTop`, `ExtractSorted` and `ExtractOne`, and nothing that takes two
/// collections. That absence is the finding the issue names, so the incumbent row here is the
/// hand-written loop rather than a rival bulk call.
/// </remarks>
[MemoryDiagnoser]
public class CdistIncumbentBenchmarks
{
    private readonly DefaultRatioScorer _scorer = new();
    private string[] _queries = [];
    private string[] _choices = [];

    /// <summary>How many queries and how many choices; the work is the square of it.</summary>
    [Params(50, 200)]
    public int Size { get; set; } = 50;

    [GlobalSetup]
    public void Prepare()
    {
        _queries = CdistCorpus.Phrases(Size, 1123);
        _choices = CdistCorpus.Phrases(Size, 1124);
    }

    [Benchmark(Baseline = true)]
    public double Lodestar_Cdist()
    {
        ScoreMatrix matrix = Process.Cdist(_queries, _choices);
        return matrix[0, 0];
    }

    /// <summary>The same matrix written by hand against this package, which is what a caller does today.</summary>
    [Benchmark]
    public double Lodestar_HandWrittenLoop()
    {
        var scores = new double[_queries.Length * _choices.Length];
        for (int row = 0; row < _queries.Length; row++)
        {
            for (int column = 0; column < _choices.Length; column++)
            {
                scores[(row * _choices.Length) + column] = Fuzz.Ratio(_queries[row], _choices[column]);
            }
        }

        return scores[0];
    }

    [Benchmark]
    public double FuzzySharp_ExtractAllPerQuery()
    {
        double total = 0.0;
        foreach (string query in _queries)
        {
            foreach (var hit in Raffinert.FuzzySharp.Process.ExtractAll(query, _choices, s => s, _scorer, 0))
            {
                total += hit.Score;
            }
        }

        return total;
    }
}
