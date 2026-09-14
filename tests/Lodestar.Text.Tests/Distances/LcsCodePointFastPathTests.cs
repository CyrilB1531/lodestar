using System.Text;
using Lodestar.Text;
using Lodestar.Text.Distances;
using Xunit;

namespace Lodestar.Text.Tests.Distances;

// SonarLint S2245 / CA5394: a seeded Random builds a reproducible corpus for this
// test; the sequence is fixed by the seed and nothing here is a security decision.
#pragma warning disable S2245, CA5394

/// <summary>
/// The code-point LCS and Indel paths against the dynamic program they replaced (#675).
/// </summary>
/// <remarks>
/// The oracles prove the answer matches rapidfuzz. These prove what random corpora reach only by
/// luck: lone surrogates beside pairs, the same emoji in both operands, and more distinct astral
/// code points than the 2,048 names the renaming can hand out.
/// </remarks>
public sealed class LcsCodePointFastPathTests
{
    private const int Seed = 20260914;

    private static int[] ToCodePoints(string text)
    {
        var points = new List<int>(text.Length);
        int i = 0;
        while (i < text.Length)
        {
            bool pair = char.IsHighSurrogate(text[i]) && i + 1 < text.Length && char.IsLowSurrogate(text[i + 1]);
            points.Add(pair ? char.ConvertToUtf32(text[i], text[i + 1]) : text[i]);
            i += pair ? 2 : 1;
        }
        return [.. points];
    }

    /// <summary>The LCS without the fast path: the generic overload over <c>int</c> is the dynamic program by construction.</summary>
    private static int ByDynamicProgram(string a, string b) =>
        Lcs.SubsequenceLength<int>(ToCodePoints(a), ToCodePoints(b));

    private static string Draw(Random rng, int length)
    {
        // Latin, CJK, a few emoji pairs, and both halves of a surrogate pair on their own.
        string[] pieces = ["a", "b", "c", "e", "é", "中", "文", "\U0001F600", "\U0001F680", "\U0001F9E0", "\uD83D", "\uDE00"];
        var text = new StringBuilder();
        for (int i = 0; i < length; i++)
        {
            text.Append(pieces[rng.Next(pieces.Length)]);
        }
        return text.ToString();
    }

    [Fact]
    public void Random_mixed_operands_agree_with_the_dynamic_program()
    {
        var rng = new Random(Seed);
        for (int trial = 0; trial < 3000; trial++)
        {
            string a = Draw(rng, rng.Next(0, 90));
            string b = Draw(rng, rng.Next(0, 90));
            int expected = ByDynamicProgram(a, b);
            int lengths = ToCodePoints(a).Length + ToCodePoints(b).Length;

            Assert.Equal(expected, Lcs.SubsequenceLength(a, b, TextElement.CodePoint));
            Assert.Equal(lengths - (2 * expected), Indel.Distance(a, b, TextElement.CodePoint));
        }
    }

    [Fact]
    public void A_lone_surrogate_is_not_equal_to_the_pair_it_would_open()
    {
        // "\uD83D" alone and U+1F600, whose high half it is, are different code points.
        Assert.Equal(0, Lcs.SubsequenceLength("\uD83D", "\U0001F600", TextElement.CodePoint));
        Assert.Equal(1, Lcs.SubsequenceLength("\uD83Dx", "\U0001F600\uD83D", TextElement.CodePoint));
        Assert.Equal(3, Indel.Distance("\uD83D", "\U0001F600\U0001F600", TextElement.CodePoint));
    }

    [Fact]
    public void A_renamed_astral_code_point_never_matches_a_lone_surrogate_holding_its_name()
    {
        // U+D800 to U+DAFF are all taken by lone surrogates in both operands, so the emoji's name
        // is handed out past them, and a name that collided with one would add a false match.
        var loneHighs = new StringBuilder();
        for (char c = '\uD800'; c < '\uDB00'; c++)
        {
            loneHighs.Append(c).Append('x');
        }
        string a = loneHighs + "\U0001F600";
        string b = "\U0001F600" + loneHighs;

        Assert.Equal(ByDynamicProgram(a, b), Lcs.SubsequenceLength(a, b, TextElement.CodePoint));
    }

    [Fact]
    public void More_distinct_astral_code_points_than_names_still_agree()
    {
        var rng = new Random(Seed);
        var a = new StringBuilder();
        var b = new StringBuilder();
        for (int i = 0; i < 2100; i++)
        {
            a.Append(char.ConvertFromUtf32(0x20000 + i));
            b.Append(char.ConvertFromUtf32(0x20000 + rng.Next(2100)));
        }

        Assert.Equal(ByDynamicProgram(a.ToString(), b.ToString()),
            Lcs.SubsequenceLength(a.ToString(), b.ToString(), TextElement.CodePoint));
    }

    [Fact]
    public void Normalized_distance_divides_by_code_points()
    {
        // Two emoji against one: three code points, six UTF-16 units.
        Assert.Equal(1.0 / 3.0, Indel.NormalizedDistance("\U0001F600\U0001F680", "\U0001F600", TextElement.CodePoint), 12);
    }
}
