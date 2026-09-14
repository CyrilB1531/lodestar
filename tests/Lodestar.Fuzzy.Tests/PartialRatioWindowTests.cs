using System.Text;
using Lodestar.Text.Distances;
using Xunit;

namespace Lodestar.Fuzzy.Tests;

// CA5394/S2245 (insecure randomness): a seeded Random draws a reproducible corpus, so a failing
// pair is the same pair on every run; nothing here is a security decision.
#pragma warning disable CA5394, S2245

/// <summary>
/// <see cref="Fuzz.PartialRatio"/> skips windows and scores the rest from one equality table; this
/// replays every window through <see cref="Indel"/>, the implementation before that change, and
/// requires the same double — not one within tolerance — so a skipped window that would have won
/// fails here rather than in a corpus that happened not to reach it.
/// </summary>
public sealed class PartialRatioWindowTests
{
    /// <summary>Repeats, characters above Latin-1, a surrogate half, and characters no needle holds.</summary>
    private const string Alphabet = "aab éāЖ中\uD83Dxyz";

    [Theory]
    [InlineData(1, 3)]
    [InlineData(2, 4)]
    [InlineData(3, int.MaxValue)]
    public void RandomPairs_MatchEveryWindowScored(int seed, int symbols)
    {
        var random = new Random(seed);
        for (int trial = 0; trial < 3000; trial++)
        {
            int m = random.Next(1, 72);
            int n = random.Next(3) == 0 ? m : m + random.Next(0, 60);
            string needle = Draw(random, m, symbols);
            string text = Draw(random, n, symbols);

            AssertSameAsExhaustive(needle, text);
            AssertSameAsExhaustive(text, needle);
        }
    }

    [Theory]
    [InlineData(11, 3)]
    [InlineData(12, 6)]
    [InlineData(13, int.MaxValue)]
    public void LongNeedles_MatchEveryWindowScored(int seed, int symbols)
    {
        // Past one word, where the table holds a row per word and the carry crosses between them
        // (#720): lengths either side of 128 and 192, and texts that embed a mutated copy.
        var random = new Random(seed);
        for (int trial = 0; trial < 60; trial++)
        {
            // The needle draws from fewer symbols than the text: over the whole alphabet a long needle
            // holds every symbol, so no text character is one it lacks and no skip rule would run.
            int m = trial < 12 ? 64 + (trial % 6) + (64 * (trial % 3)) : random.Next(65, 600);
            string needle = Draw(random, m, Math.Min(symbols, 3));
            string text = random.Next(3) switch
            {
                0 => Mutate(random, needle, symbols),
                1 => Draw(random, random.Next(0, 40), symbols) + Mutate(random, needle, symbols) + Draw(random, random.Next(0, 40), symbols),
                _ => Draw(random, m + random.Next(0, 300), symbols),
            };

            AssertSameAsExhaustive(needle, text);
            AssertSameAsExhaustive(text, needle);
        }
    }

    [Fact]
    public void LongNeedleWhoseBestWindowEndsOnAnAbsentCharacter_MatchesEveryWindowScored()
    {
        // ("abc", "xxxxaxxxx") scaled past one word: the only window that scores ends on characters
        // the needle lacks, which is why a full window is skipped on its first character, never its last.
        string needle = new string('a', 70) + "bc";
        string text = new string('x', 90) + "a" + new string('x', 90);

        AssertSameAsExhaustive(needle, text);
        AssertSameAsExhaustive(needle, new string('x', 80) + new string('a', 30) + new string('x', 80));
    }

    [Fact]
    public void LongWideNeedleWithManyDistinctCharacters_MatchesEveryWindowScored()
    {
        // 300 distinct characters above Latin-1, more than a byte slot or the short path's probe holds.
        var needle = new StringBuilder();
        for (int k = 0; k < 300; k++)
        {
            needle.Append((char)(0x4E00 + (k * 37)));
        }
        string text = "x" + needle.ToString(20, 200) + "中" + needle.ToString(0, 150) + "y";

        AssertSameAsExhaustive(needle.ToString(), text);
        AssertSameAsExhaustive(needle.ToString(0, 180), text);
    }

    [Fact]
    public void TextPastTheStackBuffer_MatchesEveryWindowScored()
    {
        var random = new Random(7);
        for (int trial = 0; trial < 40; trial++)
        {
            AssertSameAsExhaustive(Draw(random, random.Next(1, 65), 6), Draw(random, random.Next(257, 400), 6));
        }
    }

    [Fact]
    public void WideNeedleWhoseKeysAllCollide_MatchesEveryWindowScored()
    {
        // 64 distinct characters sharing one bucket of the 128-slot probe, and a text that also
        // holds colliding characters the needle lacks, which must walk the whole run to miss.
        var needle = new StringBuilder();
        for (int k = 0; k < 64; k++)
        {
            needle.Append((char)(0x0100 + (128 * k)));
        }
        string text = "脀" + needle.ToString(10, 30) + "膀" + needle.ToString(0, 20) + "舀";

        AssertSameAsExhaustive(needle.ToString(), text);
        AssertSameAsExhaustive(needle.ToString(0, 40), text);
    }

    [Theory]
    [InlineData("a", "a")]
    [InlineData("a", "b")]
    [InlineData("ab", "xxbxxaxx")]
    [InlineData("abc", "xxxxaxxxx")]
    [InlineData("abcd", "dxxxxxxxa")]
    [InlineData("xyz", "abc")]
    [InlineData("0123456789012345678901234567890123456789012345678901234567890123", "__0123456789012345678901234567890123456789012345678901234567890123__")]
    [InlineData("01234567890123456789012345678901234567890123456789012345678901234", "__01234567890123456789012345678901234567890123456789012345678901234__")]
    public void EdgeShapes_MatchEveryWindowScored(string needle, string text)
    {
        AssertSameAsExhaustive(needle, text);
    }

    private static string Draw(Random random, int length, int symbols)
    {
        var chars = new char[length];
        for (int i = 0; i < length; i++)
        {
            chars[i] = Alphabet[random.Next(Math.Min(symbols, Alphabet.Length))];
        }
        return new string(chars);
    }

    private static string Mutate(Random random, string source, int symbols)
    {
        char[] chars = source.ToCharArray();
        for (int i = 0; i < Math.Max(1, chars.Length / 10); i++)
        {
            chars[random.Next(chars.Length)] = Alphabet[random.Next(Math.Min(symbols, Alphabet.Length))];
        }
        return new string(chars);
    }

    private static void AssertSameAsExhaustive(string a, string b)
    {
        double expected = Exhaustive(a, b);
        double actual = Fuzz.PartialRatio(a, b);
        Assert.True(
            BitConverter.DoubleToInt64Bits(expected) == BitConverter.DoubleToInt64Bits(actual),
            $"PartialRatio(\"{Escape(a)}\", \"{Escape(b)}\") = {actual:R}, every window scored gives {expected:R}");
    }

    private static double Exhaustive(string a, string b)
    {
        if (a.Length == 0 && b.Length == 0)
        {
            return 100.0;
        }
        if (a.Length == 0 || b.Length == 0)
        {
            return 0.0;
        }
        if (a.Length < b.Length)
        {
            return EveryWindow(a, b);
        }
        return b.Length < a.Length ? EveryWindow(b, a) : Math.Max(EveryWindow(a, b), EveryWindow(b, a));
    }

    private static double EveryWindow(string pattern, string text)
    {
        int m = pattern.Length;
        double best = 0;
        for (int offset = -(m - 1); offset < text.Length; offset++)
        {
            int start = Math.Max(0, offset);
            int end = Math.Min(text.Length, offset + m);
            best = Math.Max(best, 100.0 * Indel.NormalizedSimilarity(pattern, text.AsSpan(start, end - start)));
        }
        return best;
    }

    private static string Escape(string s)
    {
        var escaped = new StringBuilder();
        foreach (char c in s)
        {
            escaped.Append(c < 0x7F && c >= 0x20 ? c.ToString() : $"\\u{(int)c:X4}");
        }
        return escaped.ToString();
    }
}
