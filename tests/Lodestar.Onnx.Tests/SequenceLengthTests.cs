using Lodestar.Embeddings.Tokenization;
using Lodestar.Tests.Fixtures;
using Microsoft.ML.OnnxRuntime;
using Xunit;

namespace Lodestar.Onnx.Tests;

/// <summary>
/// The sequence length read off a model with a symbolic sequence axis: the rows of its
/// position-embedding table, net of a RoBERTa-style padding offset (#1214). Each fixture's table
/// takes six tokens (<c>tools/build_tiny_models.py</c>), and a seventh indexes past it.
/// </summary>
public sealed class SequenceLengthTests
{
    private const int Positions = 6;

    /// <summary>Nine tokens with [CLS] and [SEP], past every table here.</summary>
    private static readonly string[] Long = ["the dog runs and the cat plays"];

    private static OnnxTextEmbedder Embedder(string file) =>
        new(Path.Combine(AppContext.BaseDirectory, "oracles", file), BatchCorpus.Tokenizer());

    public static TheoryData<string> Positional() =>
        ["tiny_positional.onnx", "tiny_positional_offset.onnx", "tiny_positional_offset_init.onnx"];

    [Theory]
    [MemberData(nameof(Positional))]
    public void The_position_table_sets_the_length(string file)
    {
        using OnnxTextEmbedder embedder = Embedder(file);

        Assert.Equal(Positions, embedder.MaxSequenceLength);
    }

    /// <summary>
    /// Left to the default, a long text is truncated to the table and embeds as it does with that
    /// length passed explicitly; one token more would index past the table, which is what makes the
    /// length the model's limit and not a guess — for the offset tables, the offset included.
    /// </summary>
    [Theory]
    [MemberData(nameof(Positional))]
    public void A_long_text_is_truncated_to_the_table(string file)
    {
        using OnnxTextEmbedder embedder = Embedder(file);

        float[][] derived = embedder.EmbedBatch(Long, cancellationToken: TestContext.Current.CancellationToken);
        float[][] explicitLength = embedder.EmbedBatch(
            Long, new EncodingOptions { MaxLength = Positions }, TestContext.Current.CancellationToken);

        Assert.Equal(explicitLength[0], derived[0]);
        // One token more indexes past the table: refused by name before the graph runs, not by it (#1423).
        ArgumentException error = Assert.Throws<ArgumentException>(() => embedder.EmbedBatch(
            Long, new EncodingOptions { MaxLength = Positions + 1 }, TestContext.Current.CancellationToken));
        Assert.Equal("options", error.ParamName);
    }

    /// <summary>With no position table to read, nothing is truncated, as before.</summary>
    [Fact]
    public void Without_a_position_table_nothing_is_truncated()
    {
        using OnnxTextEmbedder embedder = Embedder("tiny_embedder.onnx");

        float[][] defaulted = embedder.EmbedBatch(Long, cancellationToken: TestContext.Current.CancellationToken);
        float[][] untruncated = embedder.EmbedBatch(
            Long, new EncodingOptions { Truncation = TruncationStrategy.None }, TestContext.Current.CancellationToken);
        float[][] truncated = embedder.EmbedBatch(
            Long, new EncodingOptions { MaxLength = Positions }, TestContext.Current.CancellationToken);

        Assert.Null(embedder.MaxSequenceLength);
        Assert.Equal(untruncated[0], defaulted[0]);
        Assert.NotEqual(truncated[0], defaulted[0]);
    }

    /// <summary>
    /// Past the table, each entry point refuses under its own parameter, as #1423's page says: the encoder, the
    /// encoded batch and a span of ids alike (#1522).
    /// </summary>
    [Theory]
    [MemberData(nameof(Positional))]
    public void Every_entry_past_the_table_is_refused_under_its_own_name(string file)
    {
        using OnnxTextEmbedder embedder = Embedder(file);
        var encoder = new BatchEncoder(BatchCorpus.Tokenizer(), new EncodingOptions { MaxLength = Positions + 3 });
        EncodedBatch batch = encoder.EncodeBatch(Long, TestContext.Current.CancellationToken);
        long[] ids = [.. Enumerable.Repeat(1L, Positions + 1)];
        long[] mask = [.. Enumerable.Repeat(1L, Positions + 1)];

        Assert.Equal("encoder", Assert.Throws<ArgumentException>(
            () => embedder.EmbedBatch(Long, encoder, TestContext.Current.CancellationToken)).ParamName);
        Assert.Equal("batch", Assert.Throws<ArgumentException>(
            () => embedder.EmbedBatch(batch, TestContext.Current.CancellationToken)).ParamName);
        Assert.Equal("inputIds", Assert.Throws<ArgumentException>(() => embedder.Embed(ids, mask)).ParamName);
    }

    /// <summary>Dimension read a disposed session's metadata where every other member throws (#1521).</summary>
    [Fact]
    public void Dimension_after_dispose_throws_as_the_other_members_do()
    {
        OnnxTextEmbedder embedder = Embedder("tiny_positional.onnx");
        embedder.Dispose();

        Assert.Throws<ObjectDisposedException>(() => embedder.Dimension);
    }
}
