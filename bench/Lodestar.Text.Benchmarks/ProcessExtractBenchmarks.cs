using BenchmarkDotNet.Attributes;
using Lodestar.Fuzzy;

namespace Lodestar.Text.Benchmarks;

// CA1822: see LevenshteinIncumbentBenchmarks.
#pragma warning disable CA1822

/// <summary>
/// <see cref="Process.Extract"/> and <see cref="Process.ExtractOne"/> over a large candidate list,
/// where keeping every hit and sorting them is the cost a small limit should not pay.
/// </summary>
/// <remarks>
/// <see cref="Fuzz.Ratio"/> is the scorer so the selection is a visible share of the call; under
/// the default <see cref="Fuzz.WRatio"/> the scoring alone hides it. The limit does not apply to
/// <see cref="ExtractOne"/>, which runs once per limit value for the same figure.
/// </remarks>
[MemoryDiagnoser]
public class ProcessExtractBenchmarks
{
    private const int ChoiceCount = 100_000;
    private const string Query = "the quick brown fox";

    private string[] _choices = [];

    /// <summary>How many hits <see cref="Extract"/> keeps.</summary>
    [Params(1, 5)]
    public int Limit { get; set; } = 5;

    // SonarLint S2245, CA5394: a seeded Random builds a reproducible benchmark corpus; no security use.
#pragma warning disable S2245, CA5394
    [GlobalSetup]
    public void Setup()
    {
        const string alphabet = "abcdefghijklmnopqrstuvwxyz ";
        var random = new Random(4040);
        _choices = new string[ChoiceCount];
        for (int i = 0; i < ChoiceCount; i++)
        {
            var characters = new char[random.Next(8, 32)];
            for (int c = 0; c < characters.Length; c++)
            {
                characters[c] = alphabet[random.Next(alphabet.Length)];
            }

            _choices[i] = new string(characters);
        }
    }
#pragma warning restore S2245, CA5394

    [Benchmark(Baseline = true)]
    public IReadOnlyList<ExtractResult> Extract() =>
        Process.Extract(Query, _choices, Fuzz.Ratio, Limit);

    [Benchmark]
    public ExtractResult? ExtractOne() =>
        Process.ExtractOne(Query, _choices, Fuzz.Ratio);
}
