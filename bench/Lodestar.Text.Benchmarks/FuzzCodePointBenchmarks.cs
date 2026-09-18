using System.Text;
using BenchmarkDotNet.Attributes;
using Lodestar.Fuzzy;

namespace Lodestar.Text.Benchmarks;

// CA1822: see LevenshteinIncumbentBenchmarks.
#pragma warning disable CA1822

/// <summary>
/// The <see cref="TextElement.CodePoint"/> scorers on the texts whose code-point map #987 sized by
/// every code point rather than the distinct ones, and <see cref="Fuzz.WRatio(string, string, TextElement)"/>
/// on an empty operand, which it answered only after building that map (#1056, #1057).
/// </summary>
/// <remarks>
/// The four shapes are the delta review's: repetitive astral text, where the map was oversized, and
/// near-unique astral scalars, where it was not. The allocation column is the point of the class.
/// </remarks>
[MemoryDiagnoser]
public class FuzzCodePointBenchmarks
{
    private const TextElement Unit = TextElement.CodePoint;

    private const string Sentence = "Le cœur a ses raisons que la raison ne connaît point. ";

    private static readonly string[] Emoji = ["\U0001F600", "\U0001F601", "\U0001F602", "\U0001F603"];

    private string _emojiA = "";
    private string _emojiB = "";
    private string _emojiWordsA = "";
    private string _emojiWordsB = "";
    private string _prose = "";
    private string _proseWithEmoji = "";
    private string _distinctA = "";
    private string _distinctB = "";

    [GlobalSetup]
    public void Setup()
    {
        // Four emoji per side, eight distinct across the pair, 10,000 code points each.
        _emojiA = Repeat(i => Emoji[i % 4], 10_000);
        _emojiB = Repeat(i => char.ConvertFromUtf32(0x1F680 + (i % 4)), 10_000);
        _emojiWordsA = Repeat(i => Emoji[i % 4] + Emoji[(i + 1) % 4] + " ", 2_000);
        _emojiWordsB = Repeat(i => Emoji[(i + 2) % 4] + Emoji[(i + 3) % 4] + " ", 2_000);
        _prose = Repeat(i => Sentence[i % Sentence.Length].ToString(), 10_000);
        _proseWithEmoji = _prose + Emoji[0];
        _distinctA = Repeat(i => char.ConvertFromUtf32(0x20000 + i), 10_000);
        _distinctB = Repeat(i => char.ConvertFromUtf32(0x20000 + 5_000 + i), 10_000);
    }

    [Benchmark(Baseline = true)]
    public double Ratio_RepeatedEmoji() => Fuzz.Ratio(_emojiA, _emojiB, Unit);

    [Benchmark]
    public double TokenSortRatio_EmojiWords() => Fuzz.TokenSortRatio(_emojiWordsA, _emojiWordsB, Unit);

    [Benchmark]
    public double WRatio_ProseAndOneEmoji() => Fuzz.WRatio(_prose, _proseWithEmoji, Unit);

    [Benchmark]
    public double Ratio_DistinctAstral() => Fuzz.Ratio(_distinctA, _distinctB, Unit);

    [Benchmark]
    public double WRatio_EmptyOperand() => Fuzz.WRatio("", _prose, Unit);

    private static string Repeat(Func<int, string> piece, int count)
    {
        var text = new StringBuilder();
        for (int i = 0; i < count; i++)
        {
            text.Append(piece(i));
        }

        return text.ToString();
    }
}
