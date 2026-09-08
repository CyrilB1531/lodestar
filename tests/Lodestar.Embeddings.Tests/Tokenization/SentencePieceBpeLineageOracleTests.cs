using System.Text.Json;
using Lodestar.Embeddings.Persistence;
using Lodestar.Embeddings.Tests.Persistence;
using Lodestar.Embeddings.Tokenization;
using Xunit;

namespace Lodestar.Embeddings.Tests.Tokenization;

/// <summary>
/// Replays <c>llama2_mistral.json</c>: the two files #175 names, encoding their own
/// texts, against <c>tokenizers</c> 0.23.1 reading the same bytes.
/// </summary>
/// <remarks>
/// Not a synthetic pair. Llama-2 writes the whitespace escape as a <c>Prepend</c> plus
/// <c>Replace</c> normalizer with a null pre-tokenizer, Mistral v0.1 as a
/// <c>Metaspace</c> pre-tokenizer with <c>split</c> off — decision 0050 §2's two
/// writings of one value, here against two real files rather than one model varied.
/// </remarks>
public sealed class SentencePieceBpeLineageOracleTests
{
    private const string Corpus = "sentencepiece_bpe_lineage.json";

    private static readonly Dictionary<string, string> Fixtures = new(StringComparer.Ordinal)
    {
        ["llama2"] = "llama2_tokenizer.json",
        ["mistral_v01"] = "mistral_v01_tokenizer.json",
    };

    [Fact]
    public void Encode_and_decode_reproduce_both_models()
    {
        using JsonDocument doc = OracleLoader.Load(Corpus);

        var tokenizers = Fixtures.ToDictionary(
            pair => pair.Key,
            pair => new BpeTokenizer(Load(pair.Value)),
            StringComparer.Ordinal);

        var failures = new List<string>();
        int replayed = 0;
        foreach (JsonElement c in doc.RootElement.GetProperty("cases").EnumerateArray())
        {
            string model = c.GetProperty("model").GetString()!;
            string text = c.GetProperty("text").GetString()!;
            string[] expectedTokens = [.. c.GetProperty("tokens").EnumerateArray().Select(e => e.GetString()!)];
            int[] expectedIds = [.. c.GetProperty("ids").EnumerateArray().Select(e => e.GetInt32())];
            string expectedDecoded = c.GetProperty("decoded").GetString()!;

            TokenizationResult result = tokenizers[model].Encode(text);
            replayed++;

            if (!expectedTokens.SequenceEqual(result.Tokens))
            {
                failures.Add($"[{model} {text}] tokens exp [{string.Join(" ", expectedTokens)}] "
                    + $"got [{string.Join(" ", result.Tokens)}]");
            }
            if (!expectedIds.SequenceEqual(result.Ids))
            {
                failures.Add($"[{model} {text}] ids exp [{string.Join(", ", expectedIds)}] "
                    + $"got [{string.Join(", ", result.Ids)}]");
            }

            string decoded = tokenizers[model].Decode(result.Ids) ?? string.Empty;
            if (!string.Equals(expectedDecoded, decoded, StringComparison.Ordinal))
            {
                failures.Add($"[{model} {text}] decoded exp '{expectedDecoded}' got '{decoded}'");
            }
        }

        Assert.Equal(16, replayed);
        Assert.Empty(failures);
    }

    /// <summary>Both files load at all, with the flags that make them this lineage.</summary>
    [Theory]
    [InlineData("llama2")]
    [InlineData("mistral_v01")]
    public void Each_file_loads_as_the_third_pipeline(string model)
    {
        BpeVocabulary vocabulary = Load(Fixtures[model]);

        Assert.True(vocabulary.ByteFallback);
        Assert.True(vocabulary.FuseUnk);
        Assert.False(vocabulary.ByteLevel);
        Assert.Equal(["<s>"], vocabulary.PrefixTokens);
        Assert.Empty(vocabulary.SuffixTokens);
    }

    /// <summary>
    /// The corpus reaches the two behaviours it exists to prove, rather than passing
    /// on rows that never leave the ordinary merge loop.
    /// </summary>
    /// <remarks>
    /// #208's constraint, against ADR 0004's precedent — the blocked Myers path shipped
    /// at zero coverage while 168 tests passed, because every long case fell back
    /// earlier. Asserted here rather than assumed, so a future edit that stops reaching
    /// byte_fallback fails instead of going quietly green.
    /// </remarks>
    [Fact]
    public void The_corpus_reaches_both_the_escape_and_the_byte_fallback()
    {
        using JsonDocument doc = OracleLoader.Load(Corpus);

        var escaped = new HashSet<string>(StringComparer.Ordinal);
        var byteResolved = new HashSet<string>(StringComparer.Ordinal);
        foreach (JsonElement c in doc.RootElement.GetProperty("cases").EnumerateArray())
        {
            string model = c.GetProperty("model").GetString()!;
            foreach (string token in c.GetProperty("tokens").EnumerateArray().Select(e => e.GetString()!))
            {
                if (token.StartsWith('▁'))
                {
                    escaped.Add(model);
                }
                if (token.Length == 6 && token.StartsWith("<0x", StringComparison.Ordinal))
                {
                    byteResolved.Add(model);
                }
            }
        }

        Assert.Equal(Fixtures.Keys.Order(), escaped.Order());
        Assert.Equal(Fixtures.Keys.Order(), byteResolved.Order());
    }

    /// <summary>
    /// The one row where the two models answer differently, so the corpus cannot pass
    /// while measuring the same thing twice.
    /// </summary>
    [Fact]
    public void The_two_models_part_on_a_symbol_only_one_of_them_covers()
    {
        using JsonDocument doc = OracleLoader.Load(Corpus);

        var byModel = doc.RootElement.GetProperty("cases").EnumerateArray()
            .Where(c => c.GetProperty("text").GetString() == "\U0001F600ok")
            .ToDictionary(
                c => c.GetProperty("model").GetString()!,
                c => c.GetProperty("tokens").EnumerateArray().Select(e => e.GetString()!).ToArray(),
                StringComparer.Ordinal);

        Assert.Equal(["▁", "<0xF0>", "<0x9F>", "<0x98>", "<0x80>", "ok"], byModel["llama2"]);
        Assert.Equal(["▁", "\U0001F600", "ok"], byModel["mistral_v01"]);
    }

    private static BpeVocabulary Load(string fixture) =>
        TokenizerJsonLoader.LoadBpe(
            Path.Combine(AppContext.BaseDirectory, "oracles", fixture),
            OracleReplay.BpeBounds());
}
