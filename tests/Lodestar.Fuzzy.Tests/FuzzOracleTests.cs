using System.Text.Json;
using Lodestar.Fuzzy;
using Lodestar.Text;
using Xunit;

namespace Lodestar.Fuzzy.Tests;

public sealed class FuzzOracleTests
{
    private const double Tolerance = 1e-4;

    private static JsonDocument Load()
    {
        string path = Path.Combine(AppContext.BaseDirectory, "oracles", "fuzz.json");
        return JsonDocument.Parse(File.ReadAllText(path));
    }

    [Fact]
    public void All_ratios_match_rapidfuzz()
    {
        using JsonDocument doc = Load();
        var failures = new List<string>();

        foreach (JsonElement c in doc.RootElement.GetProperty("cases").EnumerateArray())
        {
            if (c.GetProperty("codePointOnly").GetBoolean())
            {
                continue;
            }

            string a = c.GetProperty("a").GetString()!;
            string b = c.GetProperty("b").GetString()!;

            Check(failures, c, "ratio", c.GetProperty("ratio").GetDouble(), Fuzz.Ratio(a, b));
            Check(failures, c, "partial_ratio", c.GetProperty("partial_ratio").GetDouble(), Fuzz.PartialRatio(a, b));
            Check(failures, c, "token_sort_ratio", c.GetProperty("token_sort_ratio").GetDouble(), Fuzz.TokenSortRatio(a, b));
            Check(failures, c, "token_set_ratio", c.GetProperty("token_set_ratio").GetDouble(), Fuzz.TokenSetRatio(a, b));
            Check(failures, c, "wratio", c.GetProperty("wratio").GetDouble(), Fuzz.WRatio(a, b));
            Check(failures, c, "partial_token_sort_ratio", c.GetProperty("partial_token_sort_ratio").GetDouble(), Fuzz.PartialTokenSortRatio(a, b));
            Check(failures, c, "partial_token_set_ratio", c.GetProperty("partial_token_set_ratio").GetDouble(), Fuzz.PartialTokenSetRatio(a, b));
        }

        Assert.True(failures.Count == 0, string.Join("\n", failures));
    }

    /// <summary>Every case, the non-BMP and whitespace ones included, at <see cref="TextElement.CodePoint"/> (#892).</summary>
    [Fact]
    public void All_ratios_match_rapidfuzz_over_code_points()
    {
        using JsonDocument doc = Load();
        var failures = new List<string>();
        const TextElement unit = TextElement.CodePoint;

        foreach (JsonElement c in doc.RootElement.GetProperty("cases").EnumerateArray())
        {
            string a = c.GetProperty("a").GetString()!;
            string b = c.GetProperty("b").GetString()!;

            Check(failures, c, "ratio", c.GetProperty("ratio").GetDouble(), Fuzz.Ratio(a, b, unit));
            Check(failures, c, "partial_ratio", c.GetProperty("partial_ratio").GetDouble(), Fuzz.PartialRatio(a, b, unit));
            Check(failures, c, "token_sort_ratio", c.GetProperty("token_sort_ratio").GetDouble(), Fuzz.TokenSortRatio(a, b, unit));
            Check(failures, c, "token_set_ratio", c.GetProperty("token_set_ratio").GetDouble(), Fuzz.TokenSetRatio(a, b, unit));
            Check(failures, c, "wratio", c.GetProperty("wratio").GetDouble(), Fuzz.WRatio(a, b, unit));
            Check(failures, c, "partial_token_sort_ratio", c.GetProperty("partial_token_sort_ratio").GetDouble(), Fuzz.PartialTokenSortRatio(a, b, unit));
            Check(failures, c, "partial_token_set_ratio", c.GetProperty("partial_token_set_ratio").GetDouble(), Fuzz.PartialTokenSetRatio(a, b, unit));
        }

        Assert.True(failures.Count == 0, string.Join("\n", failures));
    }

    [Fact]
    public void The_default_unit_still_counts_a_surrogate_pair_as_two()
    {
        // Two emoji sharing a high surrogate: half the units match, and no code point does.
        Assert.Equal(50.0, Fuzz.Ratio("\U0001F600", "\U0001F601"), 4);
        Assert.Equal(0.0, Fuzz.Ratio("\U0001F600", "\U0001F601", TextElement.CodePoint), 4);
    }

    /// <summary>Past what a char can rank, Ratio answers over the code points and the others say why not (#982).</summary>
    [Fact]
    public void An_alphabet_past_what_a_char_can_rank_is_answered_by_Ratio_alone()
    {
        var builder = new System.Text.StringBuilder();
        for (int codePoint = 0x21; codePoint <= 0xFFFF; codePoint++)
        {
            if (codePoint is >= 0xD800 and <= 0xDFFF)
            {
                continue;
            }

            builder.Append((char)codePoint);
        }

        string wide = builder.Append(char.ConvertFromUtf32(0x1F600)).ToString();

        // rapidfuzz 3.14.6: fuzz.ratio(wide, "abc") == 0.009454923651486258.
        Assert.Equal(0.009454923651486258, Fuzz.Ratio(wide, "abc", TextElement.CodePoint), 15);
        Assert.Throws<ArgumentException>(() => Fuzz.PartialRatio(wide, "abc", TextElement.CodePoint));
        Assert.Throws<ArgumentException>(() => Fuzz.TokenSortRatio(wide, "abc", TextElement.CodePoint));
        Assert.Throws<ArgumentException>(() => Fuzz.WRatio(wide, "abc", TextElement.CodePoint));
    }

    [Fact]
    public void An_undeclared_unit_is_refused()
    {
        Assert.Throws<ArgumentOutOfRangeException>(() => Fuzz.Ratio("a", "b", (TextElement)2));
        Assert.Throws<ArgumentOutOfRangeException>(() => Fuzz.WRatio("a", "b", (TextElement)2));
    }

    [Fact]
    public void Process_takes_the_code_point_scorer()
    {
        string[] choices = ["\U0001F601", "\U0001F600 ok"];

        IReadOnlyList<ExtractResult> hits = Process.Extract(
            "\U0001F600", choices, (q, c) => Fuzz.WRatio(q, c, TextElement.CodePoint), limit: 1);

        Assert.Equal(1, hits[0].Index);
    }

    [Theory]
    [InlineData("hello world", "world hello", 100.0)]   // token_sort ignores order
    [InlineData("new york mets", "new york mets", 100.0)]
    public void TokenSort_known(string a, string b, double expected)
    {
        Assert.Equal(expected, Fuzz.TokenSortRatio(a, b), 4);
    }

    [Fact]
    public void Ratio_is_indel_not_levenshtein()
    {
        // fuzz.ratio uses Indel similarity: "abc"/"abcd" -> 2*3/(3+4)*100.
        Assert.Equal(200.0 * 3 / 7, Fuzz.Ratio("abc", "abcd"), 4);
    }

    private static void Check(List<string> failures, JsonElement c, string name, double expected, double actual)
    {
        if (Math.Abs(expected - actual) > Tolerance)
        {
            string a = c.GetProperty("a").GetString()!;
            string b = c.GetProperty("b").GetString()!;
            failures.Add($"[#{c.GetProperty("id").GetInt32()}] {name}(\"{a}\", \"{b}\"): expected {expected:R}, got {actual:R}");
        }
    }
}
