using System.Text;
using System.Text.Json;
using Lodestar.Embeddings.Persistence;
using Lodestar.Embeddings.Tokenization;
using Xunit;

namespace Lodestar.Embeddings.Tests.Persistence;

/// <summary>
/// Replays <c>tokenizer_json_bert.json</c>: stock BERT <c>tokenizer.json</c> files, shaped as
/// all-MiniLM-L6-v2's is — the default <c>BertNormalizer</c>, a <c>BertPreTokenizer</c>, a
/// post-processor, truncation and padding — which <see cref="TokenizerJsonLoader.LoadWordPiece(Stream, ArtifactLoadOptions?)"/>
/// refused before #1210.
/// </summary>
public sealed class TokenizerJsonBertTests
{
    [Theory]
    [InlineData("uncased_template", true)]
    [InlineData("cased_bert_processing", false)]
    public void A_stock_bert_file_loads_as_the_basic_tokenization_pipeline(string model, bool lowercase)
    {
        using JsonDocument doc = OracleLoader.Load("tokenizer_json_bert.json");
        WordPieceVocabulary vocabulary = Vocabulary(doc, model);

        Assert.True(vocabulary.BasicTokenization);
        Assert.Equal(lowercase, vocabulary.Lowercase);
        Assert.Equal(["[CLS]"], vocabulary.PrefixTokens);
        Assert.Equal(["[SEP]"], vocabulary.SuffixTokens);
    }

    [Theory]
    [InlineData("uncased_template")]
    [InlineData("cased_bert_processing")]
    public void Encode_matches_tokenizers_without_the_special_tokens(string model)
    {
        using JsonDocument doc = OracleLoader.Load("tokenizer_json_bert.json");
        var tokenizer = new WordPieceTokenizer(Vocabulary(doc, model));

        OracleReplay.AssertEncodings(doc, tokenizer.Encode, "tokens", modelFilter: model);
    }

    /// <summary>The post-processed encoding is the prefix, the model's tokens and the suffix, in that order.</summary>
    [Theory]
    [InlineData("uncased_template")]
    [InlineData("cased_bert_processing")]
    public void The_prefix_and_suffix_are_what_the_post_processor_wraps_the_text_in(string model)
    {
        using JsonDocument doc = OracleLoader.Load("tokenizer_json_bert.json");
        WordPieceVocabulary vocabulary = Vocabulary(doc, model);
        var tokenizer = new WordPieceTokenizer(vocabulary);

        int replayed = 0;
        foreach (JsonElement c in doc.RootElement.GetProperty("cases").EnumerateArray())
        {
            if (!string.Equals(c.GetProperty("model").GetString(), model, StringComparison.Ordinal))
            {
                continue;
            }
            string[] wrapped = [.. c.GetProperty("wrapped").EnumerateArray().Select(e => e.GetString()!)];
            string[] actual = [.. vocabulary.PrefixTokens, .. tokenizer.Encode(c.GetProperty("text").GetString()!).Tokens, .. vocabulary.SuffixTokens];
            Assert.Equal(wrapped, actual);
            replayed++;
        }
        Assert.True(replayed > 0);
    }

    private static WordPieceVocabulary Vocabulary(JsonDocument doc, string model)
    {
        string json = doc.RootElement.GetProperty("metadata").GetProperty("tokenizer_json").GetProperty(model).GetRawText();
        using var stream = new MemoryStream(Encoding.UTF8.GetBytes(json));
        return TokenizerJsonLoader.LoadWordPiece(stream);
    }
}
