using System.Text;
using BenchmarkDotNet.Attributes;
using Lodestar.Text.Keywords;

namespace Lodestar.Text.Benchmarks;

// SonarLint S2245, CA5394: a seeded Random builds a reproducible document; no security use.
#pragma warning disable S2245, CA5394

/// <summary>TextRank over a long document whose stop words isolate many of its content words.</summary>
[MemoryDiagnoser]
public class TextRankBenchmarks
{
    private static readonly string[] StopWords = ["the", "of", "and", "a", "to", "in", "is", "it", "that", "for"];
    private readonly TextRank _textRank = new();
    private string _document = "";

    /// <summary>How many words the document holds.</summary>
    [Params(2_000, 8_000)]
    public int Words { get; set; }

    [GlobalSetup]
    public void Setup()
    {
        var random = new Random(2026);
        var text = new StringBuilder();
        for (int i = 0; i < Words; i++)
        {
            // Two stop words in three positions, so many content words have no content neighbour.
            text.Append(random.Next(3) < 2
                ? StopWords[random.Next(StopWords.Length)]
                : "term" + random.Next(Words / 4).ToString(System.Globalization.CultureInfo.InvariantCulture));
            text.Append(i % 17 == 16 ? ". " : " ");
        }

        _document = text.ToString();
    }

    [Benchmark]
    public int Extract() => _textRank.Extract(_document).Count;
}
