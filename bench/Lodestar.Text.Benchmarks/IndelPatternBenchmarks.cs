using BenchmarkDotNet.Attributes;
using Lodestar.Text.Distances;
using Raffinert.FuzzySharp.SimilarityRatio.Scorer.StrategySensitive;

namespace Lodestar.Text.Benchmarks;

// CA1822: see IndelBenchmarks.
#pragma warning disable CA1822

/// <summary>
/// One query against many texts: <see cref="IndelPattern"/> against the pairwise loop it
/// replaces, and against the .NET incumbent's scorer called once per text (#1130).
/// </summary>
/// <remarks>
/// Shared affixes are the whole reason this has six corpora rather than one. The handle's table
/// spans the pattern, so it cannot drop a shared prefix and suffix the way the pairwise call
/// does; the long-affix corpora are the ones a flattering table would leave out, and they are
/// the case fuzzy matching is most often pointed at.
/// </remarks>
[MemoryDiagnoser]
public class IndelPatternBenchmarks
{
    private const int Texts = 64;

    private readonly DefaultRatioScorer _incumbent = new();
    private string _query = string.Empty;
    private string[] _texts = [];

    /// <summary>How much the texts share with the query, which is what decides the guard.</summary>
    /// <remarks>
    /// <c>longAffix</c> is the two-word shape the guard exists for, <c>oneWordAffix</c> the shape
    /// that would refute leaving one word unguarded, and <c>suffixOnly</c> the one #1138 asked
    /// about. All but <c>beyondTable</c> keep the query inside <c>MaxInterleavedPattern</c>, or
    /// the row measures the fallback instead of the kernel — which is what #1137 caught.
    /// </remarks>
    [Params("unrelated", "sharing24", "oneWordAffix", "longAffix", "suffixOnly", "beyondTable")]
    public string Corpus { get; set; } = "unrelated";

    /// <summary>Units in the part that differs; the shared affixes are on top of it.</summary>
    /// <remarks>
    /// 4 is not decoration. What the pairwise trim buys grows as the middle shrinks, so a short
    /// middle under a long affix is the shape that costs the handle most — and a sweep that
    /// started at 16 would have missed it.
    /// </remarks>
    [Params(4, 16, 32)]
    public int Length { get; set; } = 16;

    [GlobalSetup]
    public void Setup()
    {
        // 45 + 32 + 45 is 122, inside the table's 128 units; 70 + 4 + 70 is 144, outside it at
        // every Length, where 60 put the smallest middle back inside and misnamed the row.
        (int prefix, int suffix) = Corpus switch
        {
            "sharing24" => (12, 12),
            "oneWordAffix" => (28, 28),
            "longAffix" => (45, 45),
            "suffixOnly" => (0, 45),
            "beyondTable" => (70, 70),
            _ => (0, 0),
        };

        _query = Phrase(0, prefix, Length, suffix);
        _texts = new string[Texts];
        for (int i = 0; i < Texts; i++)
        {
            _texts[i] = Phrase(i + 1, prefix, Length, suffix);
        }

        // A row that computes something else is not a comparison. The incumbent scores on the
        // 0-100 scale and rounds, so it is checked against the same ratio rather than equated.
        // S1244: exact equality is the claim. The handle reaches the same LCS by the same
        // recurrence and divides it by the same two lengths, so a tolerance would hide a defect.
#pragma warning disable S1244
        if (Lodestar_HeldPattern() != Lodestar_Pairwise())
#pragma warning restore S1244
        {
            throw new InvalidOperationException("the held pattern disagrees with the pairwise call");
        }

        if (Math.Abs(FuzzySharp_PerText() - (100.0 * Lodestar_Pairwise())) > Texts)
        {
            throw new InvalidOperationException("the incumbent is not scoring the same measure");
        }
    }

    /// <summary>Shared affixes around a middle that differs, from the row index alone.</summary>
    /// <remarks>No corpus file and no <c>Random</c>: the rule is the corpus, and it is here.</remarks>
    private static string Phrase(int index, int prefix, int middle, int suffix)
    {
        var text = new char[prefix + middle + suffix];
        for (int i = 0; i < text.Length; i++)
        {
            bool shared = i < prefix || i >= prefix + middle;
            text[i] = shared ? (char)('a' + (i % 23)) : (char)('a' + ((index * 7) + i) % 23);
        }

        return new string(text);
    }

    [Benchmark(Baseline = true)]
    public double Lodestar_Pairwise()
    {
        double total = 0.0;
        foreach (string text in _texts)
        {
            total += Indel.NormalizedSimilarity(_query.AsSpan(), text.AsSpan());
        }

        return total;
    }

    [Benchmark]
    public double Lodestar_HeldPattern()
    {
        using IndelPattern query = IndelPattern.For(_query);
        double total = 0.0;
        foreach (string text in _texts)
        {
            total += query.NormalizedSimilarity(text.AsSpan());
        }

        return total;
    }

    /// <summary>The maintained .NET incumbent, which publishes no held form: one call per text.</summary>
    [Benchmark]
    public double FuzzySharp_PerText()
    {
        double total = 0.0;
        foreach (string text in _texts)
        {
            total += _incumbent.Score(_query, text);
        }

        return total;
    }
}
