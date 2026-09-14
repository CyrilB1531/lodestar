using System.Text.Json;
using Lodestar.Embeddings.Tokenization;
using Xunit;

namespace Lodestar.Embeddings.Tests.Tokenization;

/// <summary>
/// The piece cache <see cref="BpeTokenizer"/> keeps, as <c>tokenizers</c>' <c>BPE</c> does: a hit
/// must return exactly what the merge loop would, and the cache must stop growing at its capacity.
/// </summary>
public sealed class BpeWordCacheTests
{
    private static readonly string[] Texts = OracleTexts();

    private static string[] OracleTexts()
    {
        using JsonDocument doc = OracleLoader.Load("bytelevel_bpe.json");
        return [.. doc.RootElement.GetProperty("cases").EnumerateArray().Select(c => c.GetProperty("text").GetString()!)];
    }

    [Fact]
    public void A_text_encoded_from_the_cache_matches_the_merge_loop()
    {
        BpeVocabulary vocabulary = ByteLevelBpeTests.Gpt2Vocabulary();
        var uncached = new BpeTokenizer(vocabulary, wordCacheCapacity: 0);
        var cached = new BpeTokenizer(vocabulary);

        // Twice: the first pass fills the cache, the second reads every repeated piece from it.
        for (int pass = 0; pass < 2; pass++)
        {
            foreach (string text in Texts)
            {
                TokenizationResult expected = uncached.Encode(text);
                TokenizationResult actual = cached.Encode(text);
                Assert.Equal(expected.Ids, actual.Ids);
                Assert.Equal(expected.Tokens, actual.Tokens);
            }
        }

        Assert.True(cached.WordCacheCount > 0);
        Assert.Equal(0, uncached.WordCacheCount);
    }

    [Fact]
    public void A_full_cache_stops_growing_and_still_encodes_what_it_does_not_hold()
    {
        BpeVocabulary vocabulary = ByteLevelBpeTests.Gpt2Vocabulary();
        var uncached = new BpeTokenizer(vocabulary, wordCacheCapacity: 0);
        var cached = new BpeTokenizer(vocabulary, wordCacheCapacity: 3);

        foreach (string text in Texts)
        {
            Assert.Equal(uncached.Encode(text).Ids, cached.Encode(text).Ids);
        }

        Assert.Equal(3, cached.WordCacheCount);
    }

    [Fact]
    public void A_piece_of_256_characters_is_encoded_but_not_cached()
    {
        var cached = new BpeTokenizer(ByteLevelBpeTests.Gpt2Vocabulary());

        cached.Encode(new string('a', 256));
        Assert.Equal(0, cached.WordCacheCount);

        cached.Encode(new string('a', 255));
        Assert.Equal(1, cached.WordCacheCount);
    }

    [Fact]
    public void Concurrent_encodes_through_one_cache_agree_with_the_merge_loop()
    {
        BpeVocabulary vocabulary = ByteLevelBpeTests.Gpt2Vocabulary();
        var uncached = new BpeTokenizer(vocabulary, wordCacheCapacity: 0);
        int[][] expected = [.. Texts.Select(text => uncached.Encode(text).Ids.ToArray())];
        var cached = new BpeTokenizer(vocabulary);

        var mismatches = new System.Collections.Concurrent.ConcurrentBag<string>();
        Parallel.For(0, Texts.Length * 16, new ParallelOptions { MaxDegreeOfParallelism = 8 }, i =>
        {
            int at = i % Texts.Length;
            if (!expected[at].SequenceEqual(cached.Encode(Texts[at]).Ids))
            {
                mismatches.Add(Texts[at]);
            }
        });

        Assert.Empty(mismatches);
    }
}
