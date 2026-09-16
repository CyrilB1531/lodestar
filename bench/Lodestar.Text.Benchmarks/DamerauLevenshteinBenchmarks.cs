using BenchmarkDotNet.Attributes;
using Lodestar.Text.Distances;

namespace Lodestar.Text.Benchmarks;

/// <summary>Damerau-Levenshtein on a short and a longer pair, where every cell reads the last-row table.</summary>
[MemoryDiagnoser]
public class DamerauLevenshteinBenchmarks
{
    private string _a = "";
    private string _b = "";

    /// <summary>Characters in each string.</summary>
    [Params(12, 120)]
    public int Length { get; set; }

    [GlobalSetup]
    public void Setup()
    {
        const string text = "the quick brown fox jumps over the lazy dog while the cat sleeps on a warm mat ";
        _a = string.Concat(Enumerable.Repeat(text, 4))[..Length];
        _b = string.Concat(Enumerable.Repeat(text, 4))[3..(3 + Length)];
    }

    [Benchmark]
    public int Distance() => DamerauLevenshtein.Distance(_a, _b);
}
