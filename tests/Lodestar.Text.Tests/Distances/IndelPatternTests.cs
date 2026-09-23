using Lodestar.Text.Distances;
using Lodestar.Text.Tests.Oracles;
using Xunit;

namespace Lodestar.Text.Tests.Distances;

// CA5394/S2245 (insecure randomness): a seeded Random draws a reproducible corpus, so a failing
// pair can be reached again from the seed alone. No security use.
#pragma warning disable CA5394, S2245

/// <summary>
/// The handle answers what the pairwise call answers, over every pair the package freezes and
/// over a corpus that crosses each width band and the guard at either end (#1130, #1138).
/// </summary>
/// <remarks>
/// Compared against <see cref="Indel"/> rather than against the corpus's own numbers: those are
/// <c>TextElement.CodePoint</c>, and this handle compares UTF-16 units. The pairwise path is
/// proven against rapidfuzz by <see cref="IndelOracleTests"/>, so agreeing with it is the proof.
/// </remarks>
public sealed class IndelPatternTests
{
    private static readonly OracleFile<EditDistanceCase> Corpus =
        OracleCorpus.Load<EditDistanceCase>("indel.json");

    [Fact]
    public void Every_frozen_pair_agrees_with_the_pairwise_call()
    {
        int replayed = 0;
        foreach (EditDistanceCase c in Corpus.Cases)
        {
            AssertAgrees(c.A, c.B);
            AssertAgrees(c.B, c.A);
            replayed++;
        }

        Assert.True(replayed >= 1_500, $"only {replayed} pairs replayed");
    }

    /// <summary>The bands: one word, two words, past the table, and a pattern above Latin-1.</summary>
    [Theory]
    [InlineData(0)]
    [InlineData(1)]
    [InlineData(15)]
    [InlineData(16)]
    [InlineData(17)]
    [InlineData(63)]
    [InlineData(64)]
    [InlineData(65)]
    [InlineData(127)]
    [InlineData(128)]
    [InlineData(129)]
    [InlineData(400)]
    public void A_random_corpus_agrees_at_every_width(int patternLength)
    {
        var random = new Random(1130 + patternLength);
        foreach (bool wide in new[] { false, true })
        {
            string pattern = Draw(random, patternLength, wide);
            for (int i = 0; i < 40; i++)
            {
                // A third share the pattern's first sixteen units and a third its last sixteen:
                // either end decides whether the guard sends a two-word pair to the pairwise path.
                string text = Draw(random, random.Next(0, 80), wide);
                if (i % 3 == 0 && pattern.Length >= 16)
                {
                    text = string.Concat(pattern.AsSpan(0, 16), text);
                }
                else if (i % 3 == 1 && pattern.Length >= 16)
                {
                    text = string.Concat(text, pattern.AsSpan(pattern.Length - 16));
                }

                AssertAgrees(pattern, text);
            }
        }
    }

    [Fact]
    public void A_disposed_handle_refuses_to_scan()
    {
        IndelPattern handle = IndelPattern.For("abcdefgh");
        handle.Dispose();

        Assert.Throws<ObjectDisposedException>(() => handle.Distance("abc".AsSpan()));
        Assert.Throws<ObjectDisposedException>(() => handle.NormalizedSimilarity("abc".AsSpan()));
        Assert.Throws<ObjectDisposedException>(() => handle.NormalizedDistance("abc".AsSpan()));
    }

    /// <summary>The empty pair has an answer without scanning, and still refuses once disposed.</summary>
    [Fact]
    public void An_empty_pair_refuses_on_a_disposed_handle()
    {
        IndelPattern handle = IndelPattern.For(string.Empty);
        Assert.Equal(0.0, handle.NormalizedDistance(default));
        Assert.Equal(1.0, handle.NormalizedSimilarity(default));

        handle.Dispose();
        Assert.Throws<ObjectDisposedException>(() => handle.NormalizedDistance(default));
    }

    /// <summary>Disposing twice returns one array once, which is why this is a class.</summary>
    [Fact]
    public void Disposing_twice_is_harmless()
    {
        IndelPattern handle = IndelPattern.For("abcdefgh");
        handle.Dispose();
        handle.Dispose();

        Assert.Throws<ObjectDisposedException>(() => handle.Distance("abc".AsSpan()));
    }

    [Fact]
    public void A_null_pattern_is_refused()
    {
        Assert.Throws<ArgumentNullException>(() => IndelPattern.For((string)null!));
    }

    /// <summary>The span overload copies, so the caller's buffer may be reused or go out of scope.</summary>
    [Fact]
    public void The_span_overload_copies_its_pattern()
    {
        Span<char> buffer = ['a', 'b', 'c', 'd'];
        using IndelPattern handle = IndelPattern.For((ReadOnlySpan<char>)buffer);
        buffer.Fill('z');

        Assert.Equal(Indel.Distance("abcd".AsSpan(), "abcx".AsSpan()), handle.Distance("abcx".AsSpan()));
    }

    private static void AssertAgrees(string pattern, string text)
    {
        using IndelPattern handle = IndelPattern.For(pattern);

        int expected = Indel.Distance(pattern.AsSpan(), text.AsSpan());
        Assert.Equal(expected, handle.Distance(text.AsSpan()));
        Assert.Equal(
            Indel.NormalizedDistance(pattern.AsSpan(), text.AsSpan()),
            handle.NormalizedDistance(text.AsSpan()));
        Assert.Equal(
            Indel.NormalizedSimilarity(pattern.AsSpan(), text.AsSpan()),
            handle.NormalizedSimilarity(text.AsSpan()));
    }

    private static string Draw(Random random, int length, bool wide)
    {
        var text = new char[length];
        for (int i = 0; i < length; i++)
        {
            text[i] = wide && i % 5 == 0 ? (char)('一' + random.Next(64)) : (char)('a' + random.Next(6));
        }

        return new string(text);
    }
}
