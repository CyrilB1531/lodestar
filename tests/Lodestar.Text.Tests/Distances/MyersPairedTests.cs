using Lodestar.Text.Distances;
using Xunit;

namespace Lodestar.Text.Tests.Distances;

// SonarLint S2245 / CA5394: a seeded Random builds a reproducible corpus for this
// test; the sequence is fixed by the seed and nothing here is a security decision.
#pragma warning disable S2245, CA5394

/// <summary>
/// The paired, column-major Myers kernel for a Latin-1 pattern past one word, against the dynamic
/// program (#718).
/// </summary>
/// <remarks>
/// <see cref="Myers.TryDistance(ReadOnlySpan{char}, ReadOnlySpan{char}, out int)"/> is called
/// directly, so no affix trim can shorten a pattern back under one word and the route is the one
/// asserted. The lengths sit on either side of a pair's boundary, where a padded word and the masked
/// counts would go wrong first.
/// </remarks>
public sealed class MyersPairedTests
{
    private const string Latin = "abcdefghijklmnopqrstuvwxyz éà";

    private static string Draw(Random rng, int length, bool wideText)
    {
        char[] text = new char[length];
        for (int i = 0; i < length; i++)
        {
            text[i] = wideText && rng.Next(8) == 0 ? '中' : Latin[rng.Next(Latin.Length)];
        }
        return new string(text);
    }

    public static TheoryData<int> PatternLengths => [65, 127, 128, 129, 191, 192, 193, 256, 257, 400];

    [Theory]
    [MemberData(nameof(PatternLengths))]
    public void Agrees_with_the_dynamic_program_either_side_of_a_pair(int length)
    {
        var rng = new Random(718 + length);
        for (int trial = 0; trial < 40; trial++)
        {
            string pattern = Draw(rng, length, wideText: false);
            string text = trial % 2 == 0
                ? Mutate(rng, pattern)
                : Draw(rng, rng.Next(0, 2 * length), wideText: trial % 3 == 0);

            Assert.True(Myers.TryDistance(pattern.AsSpan(), text.AsSpan(), out int distance));
            Assert.Equal(Levenshtein.Distance<char>(pattern.AsSpan(), text.AsSpan()), distance);
        }
    }

    [Fact]
    public void A_text_of_characters_the_pattern_never_holds_costs_every_position()
    {
        string pattern = new('a', 200);
        string text = new('中', 150);

        Assert.True(Myers.TryDistance(pattern.AsSpan(), text.AsSpan(), out int distance));
        Assert.Equal(200, distance);
    }

    [Fact]
    public void An_empty_text_costs_the_whole_pattern()
    {
        string pattern = new('q', 300);

        Assert.True(Myers.TryDistance(pattern.AsSpan(), ReadOnlySpan<char>.Empty, out int distance));
        Assert.Equal(300, distance);
    }

    private static string Mutate(Random rng, string pattern)
    {
        char[] text = pattern.ToCharArray();
        for (int i = 0; i < Math.Max(1, pattern.Length / 10); i++)
        {
            text[rng.Next(text.Length)] = Latin[rng.Next(Latin.Length)];
        }
        return new string(text) + new string('z', rng.Next(0, 20));
    }
}
