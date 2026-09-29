using Lodestar.Embeddings.Tokenization;
using Lodestar.Tests.Fixtures;
using Xunit;

namespace Lodestar.Onnx.Tests;

/// <summary>A static export, its batch and sequence axes fixed, embeds what the dynamic one embeds (#1258).</summary>
/// <remarks>
/// <c>tiny_embedder_static.onnx</c> is <c>tiny_embedder.onnx</c> with its axes fixed at <c>[2, 16]</c>: every row is
/// padded to 16 with masked positions the mean leaves out, and three texts run as two chunks of two, the second
/// filled with a masked row. The vectors must equal the dynamic export's exactly: the lookup is the same, and a
/// masked position never enters the mean.
/// </remarks>
public sealed class FixedAxesTests
{
    private static string Oracle(string file) => Path.Combine(AppContext.BaseDirectory, "oracles", file);

    [Fact]
    public void A_static_export_reads_its_fixed_axes_and_embeds_as_the_dynamic_one()
    {
        string[] texts = ["the cat sat", "a dog ran far", "the"];
        var options = new EncodingOptions { MaxLength = 16 };
        using var dynamicEmbedder = new OnnxTextEmbedder(Oracle("tiny_embedder.onnx"), BatchCorpus.Tokenizer());
        using var staticEmbedder = new OnnxTextEmbedder(Oracle("tiny_embedder_static.onnx"), BatchCorpus.Tokenizer());

        float[][] expected = dynamicEmbedder.EmbedBatch(texts, options, TestContext.Current.CancellationToken);
        float[][] actual = staticEmbedder.EmbedBatch(texts, cancellationToken: TestContext.Current.CancellationToken);

        Assert.Equal(16, staticEmbedder.MaxSequenceLength);
        Assert.Equal(expected, actual);
    }

    [Fact]
    public void A_single_sequence_fills_the_fixed_batch_with_masked_rows()
    {
        long[] ids = [2, 7, 9, 3];
        long[] mask = [1, 1, 1, 1];
        using var dynamicEmbedder = new OnnxTextEmbedder(Oracle("tiny_embedder.onnx"));
        using var staticEmbedder = new OnnxTextEmbedder(Oracle("tiny_embedder_static.onnx"));

        Assert.Equal(dynamicEmbedder.Embed(ids, mask), staticEmbedder.Embed(ids, mask));
    }

    [Fact]
    public void A_sequence_past_the_fixed_axis_is_refused_under_the_callers_parameter()
    {
        long[] seventeen = [.. Enumerable.Repeat(2L, 17)];
        long[] mask = [.. Enumerable.Repeat(1L, 17)];
        using var staticEmbedder = new OnnxTextEmbedder(Oracle("tiny_embedder_static.onnx"), BatchCorpus.Tokenizer());

        Assert.Equal("inputIds", Assert.Throws<ArgumentException>(() => staticEmbedder.Embed(seventeen, mask)).ParamName);
        Assert.Equal(
            "options",
            Assert.Throws<ArgumentException>(() => staticEmbedder.EmbedBatch(
                ["the cat"], new EncodingOptions { MaxLength = 17 }, TestContext.Current.CancellationToken)).ParamName);
    }
}
