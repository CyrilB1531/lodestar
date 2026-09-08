using System.Text;
using Lodestar.Embeddings.Persistence;
using Lodestar.Embeddings.Tokenization;
using Xunit;

namespace Lodestar.Embeddings.Tests.Persistence;

/// <summary>
/// What a <c>TemplateProcessing</c> post-processor puts on a BPE vocabulary, and what
/// the other shapes are refused for.
/// </summary>
/// <remarks>
/// Pinned here rather than by an oracle because nothing here changes a token stream:
/// the template names the tokens a caller wraps a sequence in through
/// <see cref="SpecialTokenTemplate"/>, and <c>tokenizers</c> has no counterpart call
/// that would return the two lists on their own to compare against.
/// </remarks>
public sealed class BpePostProcessorLoaderTests
{
    private const string BosPrefixTemplate =
        "{\"type\":\"TemplateProcessing\"," +
        "\"single\":[{\"SpecialToken\":{\"id\":\"<s>\",\"type_id\":0}},{\"Sequence\":{\"id\":\"A\",\"type_id\":0}}]," +
        "\"special_tokens\":{\"<s>\":{\"id\":\"<s>\",\"ids\":[1],\"tokens\":[\"<s>\"]}}}";

    [Fact]
    public void The_single_template_becomes_the_prefix_tokens()
    {
        BpeVocabulary vocabulary = Load(File(BosPrefixTemplate));

        Assert.Equal(["<s>"], vocabulary.PrefixTokens);
        Assert.Empty(vocabulary.SuffixTokens);
    }

    [Fact]
    public void A_special_token_after_the_sequence_becomes_a_suffix()
    {
        BpeVocabulary vocabulary = Load(File(Template(
            "{\"SpecialToken\":{\"id\":\"<s>\",\"type_id\":0}}," +
            "{\"Sequence\":{\"id\":\"A\",\"type_id\":0}}," +
            "{\"SpecialToken\":{\"id\":\"</s>\",\"type_id\":0}}")));

        Assert.Equal(["<s>"], vocabulary.PrefixTokens);
        Assert.Equal(["</s>"], vocabulary.SuffixTokens);
    }

    [Theory]
    [InlineData("null")]
    [InlineData(null)]
    public void A_file_declaring_no_post_processor_leaves_both_lists_empty(string? postProcessor)
    {
        BpeVocabulary vocabulary = Load(File(postProcessor));

        Assert.Empty(vocabulary.PrefixTokens);
        Assert.Empty(vocabulary.SuffixTokens);
    }

    /// <summary>
    /// The two spellings two mirrors of Llama-2 give the same model, which is why
    /// <c>pair</c> is read and discarded rather than reproduced -- decision 0083.
    /// </summary>
    /// <remarks>
    /// <c>daryl149/llama-2-7b-chat-hf</c> writes the first, <c>TheBloke/Llama-2-7B-fp16</c>
    /// the second. Both must load to the same two lists: a loader reading <c>pair</c> would
    /// have to call one of the two mirrors wrong.
    /// </remarks>
    [Theory]
    [InlineData("[{\"SpecialToken\":{\"id\":\"<s>\",\"type_id\":0}},{\"Sequence\":{\"id\":\"A\",\"type_id\":0}},{\"Sequence\":{\"id\":\"B\",\"type_id\":0}}]")]
    [InlineData("[{\"SpecialToken\":{\"id\":\"<s>\",\"type_id\":0}},{\"Sequence\":{\"id\":\"A\",\"type_id\":0}},{\"SpecialToken\":{\"id\":\"<s>\",\"type_id\":1}},{\"Sequence\":{\"id\":\"B\",\"type_id\":1}}]")]
    public void A_pair_template_is_read_and_discarded(string pair)
    {
        BpeVocabulary vocabulary = Load(File(
            "{\"type\":\"TemplateProcessing\"," +
            "\"single\":[{\"SpecialToken\":{\"id\":\"<s>\",\"type_id\":0}},{\"Sequence\":{\"id\":\"A\",\"type_id\":0}}]," +
            $"\"pair\":{pair}," +
            "\"special_tokens\":{\"<s>\":{\"id\":\"<s>\",\"ids\":[1],\"tokens\":[\"<s>\"]}}}"));

        Assert.Equal(["<s>"], vocabulary.PrefixTokens);
        Assert.Empty(vocabulary.SuffixTokens);
    }

    [Fact]
    public void A_post_processor_of_another_kind_is_refused_by_name()
    {
        InvalidDataException thrown = Assert.Throws<InvalidDataException>(
            () => Load(File("{\"type\":\"RobertaProcessing\",\"sep\":[\"</s>\",2],\"cls\":[\"<s>\",0]}")));

        Assert.Contains("its post_processor is 'RobertaProcessing'", thrown.Message, StringComparison.Ordinal);
    }

    [Fact]
    public void A_template_naming_no_sequence_is_refused()
    {
        InvalidDataException thrown = Assert.Throws<InvalidDataException>(
            () => Load(File(Template("{\"SpecialToken\":{\"id\":\"<s>\",\"type_id\":0}}"))));

        Assert.Contains("never names the sequence it wraps", thrown.Message, StringComparison.Ordinal);
    }

    [Fact]
    public void A_template_naming_two_sequences_is_refused()
    {
        InvalidDataException thrown = Assert.Throws<InvalidDataException>(
            () => Load(File(Template(
                "{\"Sequence\":{\"id\":\"A\",\"type_id\":0}},{\"Sequence\":{\"id\":\"A\",\"type_id\":0}}"))));

        Assert.Contains("more than one Sequence", thrown.Message, StringComparison.Ordinal);
    }

    [Fact]
    public void A_template_naming_the_second_sequence_is_refused()
    {
        InvalidDataException thrown = Assert.Throws<InvalidDataException>(
            () => Load(File(Template("{\"Sequence\":{\"id\":\"B\",\"type_id\":0}}"))));

        Assert.Contains("names sequence 'B' rather than 'A'", thrown.Message, StringComparison.Ordinal);
    }

    [Fact]
    public void A_template_step_of_a_third_kind_is_refused()
    {
        InvalidDataException thrown = Assert.Throws<InvalidDataException>(
            () => Load(File(Template("{\"Sequence\":{\"id\":\"A\",\"type_id\":0}},{\"Whatever\":{\"id\":\"x\"}}"))));

        Assert.Contains("neither a SpecialToken nor a Sequence", thrown.Message, StringComparison.Ordinal);
    }

    [Fact]
    public void A_template_processing_with_no_single_template_is_refused()
    {
        InvalidDataException thrown = Assert.Throws<InvalidDataException>(
            () => Load(File("{\"type\":\"TemplateProcessing\",\"special_tokens\":{}}")));

        Assert.Contains("declares no 'single' template", thrown.Message, StringComparison.Ordinal);
    }

    /// <summary>The other two loaders keep refusing any post-processor at all.</summary>
    /// <remarks>
    /// Only the BPE path reads one: this lot exists for the SentencePiece-BPE lineage, and
    /// widening the WordPiece and Unigram paths on the same commit would accept files
    /// nothing has measured.
    /// </remarks>
    [Fact]
    public void The_wordpiece_loader_still_refuses_a_template_processing()
    {
        string json = "{\"version\":\"1.0\",\"truncation\":null,\"padding\":null,\"added_tokens\":[]," +
            "\"normalizer\":null,\"pre_tokenizer\":{\"type\":\"Whitespace\"}," +
            $"\"post_processor\":{BosPrefixTemplate},\"decoder\":null," +
            "\"model\":{\"type\":\"WordPiece\",\"unk_token\":\"[UNK]\",\"continuing_subword_prefix\":\"##\"," +
            "\"max_input_chars_per_word\":100,\"vocab\":{\"[UNK]\":0,\"a\":1}}}";

        InvalidDataException thrown = Assert.Throws<InvalidDataException>(
            () => TokenizerJsonLoader.LoadWordPiece(new MemoryStream(Encoding.UTF8.GetBytes(json))));

        Assert.Contains("post_processor", thrown.Message, StringComparison.Ordinal);
    }

    private static string Template(string steps) =>
        $"{{\"type\":\"TemplateProcessing\",\"single\":[{steps}],\"special_tokens\":{{}}}}";

    /// <summary>A minimal BPE file whose only variable is its <c>post_processor</c>.</summary>
    private static string File(string? postProcessor) =>
        "{\"version\":\"1.0\",\"truncation\":null,\"padding\":null,\"added_tokens\":[]," +
        "\"normalizer\":null,\"pre_tokenizer\":{\"type\":\"Whitespace\"}," +
        (postProcessor is null ? string.Empty : $"\"post_processor\":{postProcessor},") +
        "\"decoder\":null," +
        "\"model\":{\"type\":\"BPE\",\"unk_token\":null,\"vocab\":{\"a\":0,\"b\":1},\"merges\":[]}}";

    private static BpeVocabulary Load(string json) =>
        TokenizerJsonLoader.LoadBpe(new MemoryStream(Encoding.UTF8.GetBytes(json)));
}
