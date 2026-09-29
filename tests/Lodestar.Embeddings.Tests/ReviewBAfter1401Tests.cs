using Lodestar.Embeddings.Persistence;
using Lodestar.Embeddings.Search;
using Lodestar.Embeddings.Tokenization;
using Xunit;

namespace Lodestar.Embeddings.Tests;

/// <summary>The Review B findings of <c>Lodestar.Embeddings</c> after #1401, one fact or theory each.</summary>
public sealed class ReviewBAfter1401Tests
{
    private static Dictionary<string, int> Map(StringComparer comparer, string key) => new(comparer) { [key] = 0 };

    [Fact]
    public void WordPiece_vocabulary_equality_is_symmetric_across_comparers()
    {
        var folding = new WordPieceVocabulary(Map(StringComparer.OrdinalIgnoreCase, "A"), "A", "##", Lowercase: false);
        var exact = folding with { Vocab = Map(StringComparer.Ordinal, "a") };

        // Each side finds the other's key under its own comparer only one way round (#1433).
        Assert.Equal(folding.Equals(exact), exact.Equals(folding));
        Assert.False(exact.Equals(folding));
    }

    [Fact]
    public void Bpe_vocabulary_equality_is_symmetric_across_comparers()
    {
        var folding = new BpeVocabulary(Map(StringComparer.OrdinalIgnoreCase, "A"), []);
        var exact = folding with { Vocab = Map(StringComparer.Ordinal, "a") };

        Assert.Equal(folding.Equals(exact), exact.Equals(folding));
        Assert.False(exact.Equals(folding));
    }

    [Theory]
    [InlineData(float.NaN)]
    [InlineData(float.PositiveInfinity)]
    [InlineData(float.NegativeInfinity)]
    public void Search_refuses_a_query_holding_a_non_finite_value(float value)
    {
        var index = new EmbeddingIndex(2, normalize: false);
        index.Add([1f, 0f]);
        index.Add([0f, 1f]);

        // It scored NaN against every item and returned them in insertion order, as if ranked (#1434).
        ArgumentException error = Assert.Throws<ArgumentException>(() => index.Search([value, 0f], k: 2));

        Assert.Equal("query", error.ParamName);
    }

    [Fact]
    public async Task VocabTxtLoader_refuses_a_null_name_before_reading_the_stream()
    {
        using var stream = new UnreadableStream();

        // The stream would throw if read: the refusal must come first, not after the whole file (#1439).
        Assert.Throws<ArgumentNullException>(() => VocabTxtLoader.Load(stream, unkToken: null!));
        Assert.Throws<ArgumentNullException>(() => VocabTxtLoader.Load(stream, continuationPrefix: null!));
        await Assert.ThrowsAsync<ArgumentNullException>(
            () => VocabTxtLoader.LoadAsync(stream, unkToken: null!, cancellationToken: TestContext.Current.CancellationToken));
    }

    [Fact]
    public void IsMatchable_on_an_absent_type_list_is_out_of_range_rather_than_null()
    {
        var vocabulary = new SentencePieceVocabulary([new("<unk>", 0, 0)], null!, UnkId: 0, BosId: -1, EosId: -1, PadId: -1);

        // The record's own equality reads an absent list as legal; this dereferenced it (#1440).
        Assert.Throws<ArgumentOutOfRangeException>(() => vocabulary.IsMatchable(0));
    }

    [Fact]
    public void EncodedBatch_lengths_cannot_be_written_through_a_cast()
    {
        var encoder = new BatchEncoder(WordPiece());
        EncodedBatch batch = encoder.EncodeBatch(["a b", "a"], TestContext.Current.CancellationToken);

        // Cast back to int[], the list moved what Sequence slices (#1452).
        Assert.False(batch.Lengths is int[]);
        Assert.Equal(batch.Lengths[0], batch.Sequence(0).Length);
    }

    [Fact]
    public void BatchEncoder_counts_the_template_once_at_construction()
    {
        List<string> prefix = ["[CLS]"];
        var options = new EncodingOptions
        {
            Template = new SpecialTokenTemplate(prefix, ["[SEP]"], "[PAD]"),
            MaxLength = 4,
            Truncation = TruncationStrategy.LongestFirst,
        };
        var encoder = new BatchEncoder(WordPiece(), options);
        long[] before = encoder.Encode("a b a b");

        // The ids were resolved once; a list the caller grows afterwards must not shrink the budget (#1453).
        prefix.Add("[CLS]");

        Assert.Equal(before, encoder.Encode("a b a b"));
    }

    private static WordPieceTokenizer WordPiece() =>
        new(new WordPieceVocabulary(
            new Dictionary<string, int>(StringComparer.Ordinal)
            {
                ["[PAD]"] = 0,
                ["[UNK]"] = 1,
                ["[CLS]"] = 2,
                ["[SEP]"] = 3,
                ["a"] = 4,
                ["b"] = 5,
            },
            "[UNK]",
            "##",
            Lowercase: false));

    /// <summary>A stream whose every read fails, so a test can tell a refusal made before reading from one made after.</summary>
    private sealed class UnreadableStream : MemoryStream
    {
        public override int Read(byte[] buffer, int offset, int count) =>
            throw new InvalidOperationException("The loader read before refusing its arguments.");

        public override int Read(Span<byte> buffer) =>
            throw new InvalidOperationException("The loader read before refusing its arguments.");

        public override Task<int> ReadAsync(byte[] buffer, int offset, int count, CancellationToken cancellationToken) =>
            throw new InvalidOperationException("The loader read before refusing its arguments.");

        public override ValueTask<int> ReadAsync(Memory<byte> buffer, CancellationToken cancellationToken = default) =>
            throw new InvalidOperationException("The loader read before refusing its arguments.");
    }
}
