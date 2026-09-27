using Lodestar.Embeddings.Tokenization;
using Lodestar.Tests.Fixtures;
using Xunit;

namespace Lodestar.Onnx.Tests;

/// <summary>
/// What the embedder accepts from a model's output, and what it refuses by name — against the
/// four variants of <c>tiny_embedder.onnx</c> that <c>tools/build_tiny_models.py</c> writes (#1214).
/// </summary>
public sealed class OutputContractTests
{
    private static string Oracle(string file) => Path.Combine(AppContext.BaseDirectory, "oracles", file);

    private static readonly string[] Texts = ["the cat sat on the mat", "the", "hello world!", ""];

    /// <summary>A bound comfortably above every text here, so nothing is truncated.</summary>
    private static readonly EncodingOptions Options = new() { MaxLength = 64, BatchSize = 2 };

    /// <summary>
    /// The table is k/64 with |k| at most 32, exact in float16 and bfloat16, so a half-precision
    /// output converted to float must pool to the float32 model's vectors bit for bit.
    /// </summary>
    [Theory]
    [InlineData("tiny_embedder_fp16.onnx")]
    [InlineData("tiny_embedder_bf16.onnx")]
    public void A_half_precision_output_embeds_as_the_float32_model_does(string file)
    {
        using var reference = new OnnxTextEmbedder(Oracle("tiny_embedder.onnx"), BatchCorpus.Tokenizer());
        using var half = new OnnxTextEmbedder(Oracle(file), BatchCorpus.Tokenizer());

        float[][] expected = reference.EmbedBatch(Texts, Options, TestContext.Current.CancellationToken);
        float[][] actual = half.EmbedBatch(Texts, Options, TestContext.Current.CancellationToken);

        Assert.Equal(expected.Length, actual.Length);
        for (int i = 0; i < expected.Length; i++)
        {
            Assert.Equal(expected[i], actual[i]);
        }
        Assert.Equal(reference.Embed([5, 9, 12], [1, 1, 0]), half.Embed([5, 9, 12], [1, 1, 0]));
    }

    [Fact]
    public void An_output_of_another_element_type_is_refused_by_name()
    {
        using var embedder = new OnnxTextEmbedder(Oracle("tiny_embedder_fp64.onnx"));

        NotSupportedException error = Assert.Throws<NotSupportedException>(() => embedder.Embed([5, 9], [1, 1]));

        Assert.Contains("last_hidden_state", error.Message, StringComparison.Ordinal);
        Assert.Contains("Double", error.Message, StringComparison.Ordinal);
    }

    /// <summary>
    /// <c>[seq, batch, dim]</c> holds as many elements as <c>[batch, seq, dim]</c>, so only the
    /// shape tells them apart; unchecked, the pooler averaged the wrong rows without a word.
    /// </summary>
    [Fact]
    public void A_rank_3_output_that_is_not_batch_by_sequence_is_refused_by_name()
    {
        using var embedder = new OnnxTextEmbedder(Oracle("tiny_transposed.onnx"));

        InvalidOperationException error = Assert.Throws<InvalidOperationException>(
            () => embedder.Embed([5, 9, 12], [1, 1, 1]));

        Assert.Contains("last_hidden_state", error.Message, StringComparison.Ordinal);
        Assert.Contains("[3, 1, 4]", error.Message, StringComparison.Ordinal);
        Assert.Contains("[1, 3, dim]", error.Message, StringComparison.Ordinal);
    }

    /// <summary>A transposed output of a one-by-one batch has the expected shape, so it runs.</summary>
    [Fact]
    public void A_shape_that_matches_is_not_refused()
    {
        using var embedder = new OnnxTextEmbedder(Oracle("tiny_transposed.onnx"));
        using var reference = new OnnxTextEmbedder(Oracle("tiny_embedder.onnx"));

        Assert.Equal(reference.Embed([7], [1]), embedder.Embed([7], [1]));
    }
}
