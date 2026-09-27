using BenchmarkDotNet.Attributes;
using Lodestar.Text.Distances;

namespace Lodestar.Text.Benchmarks;

/// <summary>
/// Damerau-Levenshtein on a short, a longer and a long pair, against F23.StringSimilarity's
/// <c>Damerau</c>, the one maintained .NET implementation of the unrestricted distance.
/// </summary>
/// <remarks>
/// The 1,000-character pair is #1199's: the full table was 1,002 × 1,002 cells, where three rows now
/// hold the distance. Both arms are checked to agree before anything is timed.
/// </remarks>
[MemoryDiagnoser]
public class DamerauLevenshteinBenchmarks
{
    private readonly F23.StringSimilarity.Damerau _f23 = new();
    private string _a = "";
    private string _b = "";

    /// <summary>Characters in each string.</summary>
    [Params(12, 120, 1_000)]
    public int Length { get; set; }

    [GlobalSetup]
    public void Setup()
    {
        const string text = "the quick brown fox jumps over the lazy dog while the cat sleeps on a warm mat ";
        string long_ = string.Concat(Enumerable.Repeat(text, 1 + ((Length + 3) / text.Length)));
        _a = long_[..Length];
        _b = long_[3..(3 + Length)];
        if (Lodestar() != (int)F23StringSimilarity())
        {
            throw new InvalidOperationException($"The two arms disagree at length {Length}.");
        }
    }

    [Benchmark(Baseline = true)]
    public int Lodestar() => DamerauLevenshtein.Distance(_a, _b);

    [Benchmark]
    public double F23StringSimilarity() => _f23.Distance(_a, _b);
}
