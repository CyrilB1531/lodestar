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
        // The texts overload named "encoder", a parameter it does not have (#1581).
        using var embedder = new OnnxTextEmbedder(Oracle("tiny_embedder_huge_static.onnx"), BatchCorpus.Tokenizer());
        var encoder = new BatchEncoder(BatchCorpus.Tokenizer(), new EncodingOptions { MaxLength = 16 });
        EncodedBatch batch = encoder.EncodeBatch(["the cat sat"], TestContext.Current.CancellationToken);

        Assert.Equal("options", Assert.Throws<ArgumentException>(
            () => embedder.EmbedBatch(["the cat sat"], cancellationToken: TestContext.Current.CancellationToken)).ParamName);
        Assert.Equal("encoder", Assert.Throws<ArgumentException>(
            () => embedder.EmbedBatch(["the cat sat"], encoder, TestContext.Current.CancellationToken)).ParamName);
        Assert.Equal("batch", Assert.Throws<ArgumentException>(
            () => embedder.EmbedBatch(batch, TestContext.Current.CancellationToken)).ParamName);
    }
}
