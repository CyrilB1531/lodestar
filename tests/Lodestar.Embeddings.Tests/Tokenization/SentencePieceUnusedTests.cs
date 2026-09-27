using System.Text.Json;
using Lodestar.Embeddings.Persistence;
using Lodestar.Embeddings.Tests.Persistence;
using Lodestar.Embeddings.Tokenization;
using Xunit;

namespace Lodestar.Embeddings.Tests.Tokenization;

/// <summary>
/// Replays <c>sentencepiece_unused.json</c>: <c>tiny_sp.model</c> with three pieces re-typed UNUSED,
/// which sentencepiece keeps an id for and never segments onto (#1213).
/// </summary>
public sealed class SentencePieceUnusedTests
{
    [Fact]
    public void Encode_never_segments_onto_an_unused_piece()
    {
        using JsonDocument doc = OracleLoader.Load("sentencepiece_unused.json");
        var tokenizer = new SentencePieceTokenizer(Vocabulary(doc));

        OracleReplay.AssertEncodings(doc, tokenizer.Encode, "pieces");
    }

    [Fact]
    public void An_unused_piece_keeps_its_id()
    {
        using JsonDocument doc = OracleLoader.Load("sentencepiece_unused.json");
        SentencePieceVocabulary vocabulary = Vocabulary(doc);
        var tokenizer = new SentencePieceTokenizer(vocabulary);

        foreach (JsonElement unused in doc.RootElement.GetProperty("metadata").GetProperty("unused_pieces").EnumerateArray())
        {
            int expected = unused.GetProperty("id").GetInt32();
            Assert.Equal(SentencePieceType.Unused, vocabulary.Types[expected]);
            Assert.False(vocabulary.IsMatchable(expected));
            Assert.True(tokenizer.TryGetId(unused.GetProperty("piece").GetString()!, out int id));
            Assert.Equal(expected, id);
        }
    }

    private static SentencePieceVocabulary Vocabulary(JsonDocument doc)
    {
        string model = doc.RootElement.GetProperty("metadata").GetProperty("model_base64").GetString()!;
        using var stream = new MemoryStream(Convert.FromBase64String(model));
        return SentencePieceModelLoader.Load(stream);
    }
}
