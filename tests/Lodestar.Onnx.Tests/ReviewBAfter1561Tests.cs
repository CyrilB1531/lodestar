using Lodestar.Embeddings.Tokenization;
using Lodestar.Tests.Fixtures;
using Xunit;

namespace Lodestar.Onnx.Tests;

/// <summary>The Review B findings of <c>Lodestar.Onnx</c> after #1561, one fact each.</summary>
public sealed class ReviewBAfter1561Tests
{
    private static string Oracle(string file) => Path.Combine(AppContext.BaseDirectory, "oracles", file);

    [Fact]
    public void Each_batch_overload_names_its_own_argument_for_a_chunk_past_one_array()
    {
        // The texts overload named "encoder", a parameter it does not have (#1581). The batch is fixed at 65,536 and
        // the sequence is not, so a text past 32,768 tokens, which options let through, makes the chunk too large.
        using var embedder = new OnnxTextEmbedder(Oracle("tiny_embedder_huge_batch.onnx"), BatchCorpus.Tokenizer());
        var options = new EncodingOptions { MaxLength = 40_000 };
        var encoder = new BatchEncoder(BatchCorpus.Tokenizer(), options);
        string[] texts = [string.Join(" ", Enumerable.Repeat("the", 33_000))];
        EncodedBatch batch = encoder.EncodeBatch(texts, TestContext.Current.CancellationToken);

        Assert.Equal("options", Assert.Throws<ArgumentException>(
            () => embedder.EmbedBatch(texts, options, TestContext.Current.CancellationToken)).ParamName);
        Assert.Equal("encoder", Assert.Throws<ArgumentException>(
            () => embedder.EmbedBatch(texts, encoder, TestContext.Current.CancellationToken)).ParamName);
        Assert.Equal("batch", Assert.Throws<ArgumentException>(
            () => embedder.EmbedBatch(batch, TestContext.Current.CancellationToken)).ParamName);
    }
}
