using BenchmarkDotNet.Attributes;
using Lodestar.Fuzzy;

namespace Lodestar.Text.Benchmarks;

// CA1822: see LevenshteinIncumbentBenchmarks.
#pragma warning disable CA1822

/// <summary>
/// What a cutoff buys <see cref="Process.Cdist"/> once a length bound may reject a pair before
/// scoring it: the before and after of the same run, on one corpus and then the other (#1134).
/// </summary>
/// <remarks>
/// The bound is taken only on the default scorer, so the lambda row is the unbounded path — the
/// same arithmetic through a delegate the gate cannot recognise. `Widths` is the second half of
/// the finding: the narrow corpus still varies by a word's length, so it rejects at a high cutoff
/// and at no other, which is what prices the test where it cannot fire.
/// </remarks>
[MemoryDiagnoser]
public class CdistCutoffBenchmarks
{
    private string[] _queries = [];
    private string[] _choices = [];

    /// <summary>The cutoff cells are reported above; <c>0</c> is the call before this change.</summary>
    [Params(0, 60, 80, 90)]
    public int Cutoff { get; set; }

    /// <summary>Phrases of one to six words, or of three: how much spread the lengths give the bound.</summary>
    [Params(true, false)]
    public bool Widths { get; set; } = true;

    [GlobalSetup]
    public void Prepare()
    {
        (int min, int max) = Widths ? (1, 6) : (3, 3);
        _queries = CdistCorpus.Phrases(64, 1134, min, max);
        _choices = CdistCorpus.Phrases(64, 1135, min, max);
    }

    [Benchmark(Baseline = true)]
    public double Lodestar_Cdist_Bounded()
    {
        ScoreMatrix matrix = Process.Cdist(_queries, _choices, scorer: null, Cutoff);
        return matrix[0, 0];
    }

    /// <summary>The same scores through a delegate the gate refuses to recognise, so every pair is scored.</summary>
    [Benchmark]
    public double Lodestar_Cdist_EveryPair()
    {
        ScoreMatrix matrix = Process.Cdist(_queries, _choices, (a, b) => Fuzz.Ratio(a, b), Cutoff);
        return matrix[0, 0];
    }
}
