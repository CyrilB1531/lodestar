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

    /// <summary>With no position table to read, the table's six positions cut nothing: the default is 256 (#1257).</summary>
    [Fact]
    public void Without_a_position_table_a_text_under_256_is_not_truncated()
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

    /// <summary>
    /// An encoder left open takes the model's limit as the options overload does and sentence-transformers does (#1257):
    /// a sequence of the limit is untouched, one token more and far more are cut to it with the template kept.
    /// </summary>
    [Theory]
    [MemberData(nameof(Positional))]
    public void An_encoder_left_open_is_truncated_to_the_table(string file)
    {
        using OnnxTextEmbedder embedder = Embedder(file);
        var open = new BatchEncoder(BatchCorpus.Tokenizer());
        string atLimit = string.Join(" ", Enumerable.Repeat("the", Positions - 2));
        string onePast = atLimit + " dog";
        string farPast = string.Join(" ", Enumerable.Repeat("the", 600));
        string[] texts = ["", atLimit, onePast, farPast];

        float[][] actual = embedder.EmbedBatch(texts, open, TestContext.Current.CancellationToken);
        float[][] expected = embedder.EmbedBatch(
            texts, new EncodingOptions { MaxLength = Positions }, TestContext.Current.CancellationToken);

        Assert.Equal(expected, actual);
        Assert.Equal(actual[1], actual[2]);
        Assert.Null(open.Options.MaxLength);
    }

    /// <summary>A cap the encoder sets below the limit is the encoder's own (#1257).</summary>
    [Theory]
    [MemberData(nameof(Positional))]
    public void A_cap_the_encoder_sets_is_kept(string file)
    {
        using OnnxTextEmbedder embedder = Embedder(file);
        var options = new EncodingOptions { MaxLength = Positions - 2 };

        Assert.Equal(
            embedder.EmbedBatch(Long, options, TestContext.Current.CancellationToken),
            embedder.EmbedBatch(Long, new BatchEncoder(BatchCorpus.Tokenizer(), options), TestContext.Current.CancellationToken));
    }

    /// <summary>A static export's axis is a limit as the table is (#1257).</summary>
    [Fact]
    public void An_encoder_left_open_is_truncated_to_a_fixed_axis()
    {
        using OnnxTextEmbedder embedder = Embedder("tiny_embedder_static.onnx");
        string[] farPast = [string.Join(" ", Enumerable.Repeat("the dog", 40))];

        Assert.Equal(
            embedder.EmbedBatch(farPast, new EncodingOptions { MaxLength = 16 }, TestContext.Current.CancellationToken),
            embedder.EmbedBatch(farPast, new BatchEncoder(BatchCorpus.Tokenizer()), TestContext.Current.CancellationToken));
    }

    /// <summary>No stated maximum and no configuration beside the model: cut at 256, the maintainer's default (#1257).</summary>
    [Fact]
    public void An_encoder_left_open_on_a_model_with_no_limit_cuts_at_256()
    {
        using OnnxTextEmbedder embedder = Embedder("tiny_embedder.onnx");
        string[] farPast = [string.Join(" ", Enumerable.Repeat("the dog", 300))];

        Assert.Equal(
            embedder.EmbedBatch(farPast, new EncodingOptions { MaxLength = 256 }, TestContext.Current.CancellationToken),
            embedder.EmbedBatch(farPast, new BatchEncoder(BatchCorpus.Tokenizer()), TestContext.Current.CancellationToken));
    }

    /// <summary><c>None</c> never cuts: past the limit the text is what is refused, naming it, as under the options overload.</summary>
    [Theory]
    [MemberData(nameof(Positional))]
    public void An_encoder_that_never_cuts_refuses_the_text_past_the_table(string file)
    {
        using OnnxTextEmbedder embedder = Embedder(file);
        var refusing = new BatchEncoder(
            BatchCorpus.Tokenizer(), new EncodingOptions { Truncation = TruncationStrategy.None });

        Assert.Equal("texts", Assert.Throws<ArgumentException>(
            () => embedder.EmbedBatch(Long, refusing, TestContext.Current.CancellationToken)).ParamName);
    }

    /// <summary>A template longer than the limit leaves nothing to cut: the row stays refused, an empty batch stays empty.</summary>
    [Fact]
    public void A_template_past_the_limit_is_left_to_the_length_refusal()
    {
        using OnnxTextEmbedder embedder = Embedder("tiny_positional.onnx");
        var template = new SpecialTokenTemplate(
            ["[CLS]", "[CLS]", "[CLS]", "[CLS]"], ["[SEP]", "[SEP]", "[SEP]", "[SEP]"], "[PAD]");
        var encoder = new BatchEncoder(BatchCorpus.Tokenizer(), new EncodingOptions { Template = template });

        Assert.Empty(embedder.EmbedBatch([], encoder, TestContext.Current.CancellationToken));
        Assert.Equal("encoder", Assert.Throws<ArgumentException>(
            () => embedder.EmbedBatch([""], encoder, TestContext.Current.CancellationToken)).ParamName);
        // The options overload goes through the same default, so it agrees (#1257).
        Assert.Empty(embedder.EmbedBatch([], encoder.Options, TestContext.Current.CancellationToken));
        Assert.Equal("options", Assert.Throws<ArgumentException>(
            () => embedder.EmbedBatch([""], encoder.Options, TestContext.Current.CancellationToken)).ParamName);
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
