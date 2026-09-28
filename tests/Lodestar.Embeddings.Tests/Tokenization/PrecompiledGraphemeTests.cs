using System.Text.Json;
using Lodestar.Embeddings.Persistence;
using Lodestar.Embeddings.Tokenization;
using Xunit;

namespace Lodestar.Embeddings.Tests.Tokenization;

/// <summary>A tokenizer.json's Precompiled normalizer reads its charsmap as <c>tokenizers</c> does (#1260).</summary>
/// <remarks>
/// By grapheme, a short cluster replaced whole by its shortest rule; the <c>.model</c> path keeps
/// sentencepiece's longest match, so the same charsmap answers both ways on NFD text.
/// </remarks>
public sealed class PrecompiledGraphemeTests
{
    private static PrecompiledNormalizer Charsmap() =>
        SentencePieceModelLoader.Load(Path.Combine(AppContext.BaseDirectory, "oracles", "xlmr_fairseq.model")).Normalizer!;

    public static TheoryData<int> Indices()
    {
        using JsonDocument doc = OracleLoader.Load("precompiled_grapheme.json");
        var data = new TheoryData<int>();
        for (int i = 0; i < doc.RootElement.GetProperty("cases").GetArrayLength(); i++)
        {
            data.Add(i);
        }

        return data;
    }

    [Theory]
    [MemberData(nameof(Indices))]
    public void The_grapheme_reading_matches_tokenizers(int index)
    {
        using JsonDocument doc = OracleLoader.Load("precompiled_grapheme.json");
        JsonElement c = doc.RootElement.GetProperty("cases")[index];

        Assert.Equal(c.GetProperty("normalized").GetString(), Charsmap().ByGrapheme().Normalize(c.GetProperty("text").GetString()!));
    }

    [Fact]
    public void The_two_readings_part_on_a_decomposed_letter()
    {
        // tokenizers keeps the shortest rule for the whole grapheme, sentencepiece the longest at each position.
        const string Decomposed = "phở";

        Assert.Equal("phơ", Charsmap().ByGrapheme().Normalize(Decomposed));
        Assert.Equal("phở", Charsmap().Normalize(Decomposed));
        Assert.NotEqual(Charsmap(), Charsmap().ByGrapheme());
    }
}
