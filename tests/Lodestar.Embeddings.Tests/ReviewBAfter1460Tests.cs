using System.Text;
using Lodestar.Embeddings.Persistence;
using Lodestar.Embeddings.Tokenization;
using Xunit;

namespace Lodestar.Embeddings.Tests;

/// <summary>The Review B findings of <c>Lodestar.Embeddings</c> after #1397 and #1460, one fact or theory each.</summary>
public sealed class ReviewBAfter1460Tests
{
    private static MemoryStream Utf8(string text) => new(Encoding.UTF8.GetBytes(text));

    [Fact]
    public void The_loaders_refuse_a_null_stream_under_their_own_names()
    {
        // ReadAllBytes named it "stream" (#1500).
        Assert.Equal("source", Assert.Throws<ArgumentNullException>(() => TokenizerJsonLoader.LoadWordPiece((Stream)null!)).ParamName);
        Assert.Equal("source", Assert.Throws<ArgumentNullException>(() => SentencePieceModelLoader.Load((Stream)null!)).ParamName);
        Assert.Equal("vocabJson", Assert.Throws<ArgumentNullException>(() => BpeFilesLoader.Load(null!, Utf8("a b"))).ParamName);
        Assert.Equal("mergesPath", Assert.Throws<ArgumentNullException>(() => BpeFilesLoader.Load("vocab.json", (string)null!)).ParamName);
    }

    [Fact]
    public void BpeFilesLoader_refuses_a_null_merges_stream_before_reading_the_vocabulary()
    {
        using var vocab = new ThrowingStream();

        // It read the whole vocabulary first (#1500).
        Assert.Equal("merges", Assert.Throws<ArgumentNullException>(() => BpeFilesLoader.Load(vocab, null!)).ParamName);
    }

    [Fact]
    public void A_negative_maxCharsPerWord_is_refused()
    {
        var vocab = new Dictionary<string, int>(StringComparer.Ordinal) { ["[UNK]"] = 0, ["a"] = 1 };

        // tokenizers' max_input_chars_per_word is a usize (#1505).
        Assert.Throws<ArgumentOutOfRangeException>(() => new WordPieceTokenizer(vocab, maxCharsPerWord: -1));
    }

    [Fact]
    public void An_undefined_truncation_strategy_is_refused()
    {
        var vocab = new Dictionary<string, int>(StringComparer.Ordinal)
        {
            ["[PAD]"] = 0,
            ["[UNK]"] = 1,
            ["[CLS]"] = 2,
            ["[SEP]"] = 3,
        };
        var options = new EncodingOptions { Truncation = (TruncationStrategy)99 };

        // It truncated as LongestFirst would (#1505).
        Assert.Equal("options", Assert.Throws<ArgumentException>(() => new BatchEncoder(new WordPieceTokenizer(vocab), options)).ParamName);
    }

    [Fact]
    public void A_model_defining_a_piece_twice_is_refused_as_sentencepiece_refuses_it()
    {
        // sentencepiece 0.2 raises "▁a is already defined"; this kept the last (#1501).
        using var repeated = new MemoryStream(Model(("<unk>", 2), ("\u2581a", 1), ("\u2581a", 1)));
        InvalidDataException error = Assert.Throws<InvalidDataException>(() => SentencePieceModelLoader.Load(repeated));
        Assert.Contains("already defined", error.Message, StringComparison.Ordinal);

        // A normal piece and a control one of the same string live in its two maps, and load.
        using var apart = new MemoryStream(Model(("<unk>", 2), ("\u2581a", 1), ("\u2581a", 3)));
        Assert.Equal(3, SentencePieceModelLoader.Load(apart).Count);
    }

    [Fact]
    public void A_string_both_reserved_and_normal_resolves_to_the_reserved_piece()
    {
        // piece_to_id looks in sentencepiece's reserved map first: a normal ▁a at 1 and a control one at 2 give 2 (#1501).
        using var both = new MemoryStream(Model(("<unk>", 2), ("\u2581a", 1), ("\u2581a", 3)));
        var tokenizer = new SentencePieceTokenizer(SentencePieceModelLoader.Load(both));

        Assert.True(tokenizer.TryGetId("\u2581a", out int id));
        Assert.Equal(2, id);
    }

    [Fact]
    public void EncodeBatch_names_texts_for_a_batch_past_one_array()
    {
        var vocab = new Dictionary<string, int>(StringComparer.Ordinal)
        {
            ["[PAD]"] = 0,
            ["[UNK]"] = 1,
            ["[CLS]"] = 2,
            ["[SEP]"] = 3,
            ["a"] = 4,
        };
        var encoder = new BatchEncoder(new WordPieceTokenizer(vocab));
        string[] texts = [.. Enumerable.Repeat("a", 50_000), string.Join(' ', Enumerable.Repeat("a", 43_000))];

        // 50,001 rows of 43,002 is past the largest array, refused before allocating; it named Pad's "count" (#1505).
        ArgumentException error = Assert.Throws<ArgumentException>(
            () => encoder.EncodeBatch(texts, TestContext.Current.CancellationToken));

        Assert.Equal("texts", error.ParamName);
        Assert.DoesNotContain("count", error.Message, StringComparison.Ordinal);
    }

    /// <summary>A <c>ModelProto</c> of pieces with a zero score and the given type, and the identity normalizer.</summary>
    private static byte[] Model(params (string Piece, int Type)[] pieces)
    {
        var model = new List<byte>();
        foreach ((string piece, int type) in pieces)
        {
            byte[] text = Encoding.UTF8.GetBytes(piece);
            List<byte> message = [0x0A, (byte)text.Length, .. text, 0x15, 0, 0, 0, 0, 0x18, (byte)type];
            model.AddRange([0x0A, (byte)message.Count, .. message]);
        }

        // normalizer_spec { name: "identity" }, which the loader requires be declared.
        byte[] identity = Encoding.UTF8.GetBytes("identity");
        model.AddRange([0x1A, (byte)(identity.Length + 2), 0x0A, (byte)identity.Length, .. identity]);
        return [.. model];
    }

    /// <summary>A stream whose every read fails, so a refusal made before reading can be told from one made after.</summary>
    private sealed class ThrowingStream : MemoryStream
    {
        public override int Read(byte[] buffer, int offset, int count) =>
            throw new InvalidOperationException("The loader read before refusing its arguments.");

        public override int Read(Span<byte> buffer) =>
            throw new InvalidOperationException("The loader read before refusing its arguments.");

        public override int ReadByte() =>
            throw new InvalidOperationException("The loader read before refusing its arguments.");
    }
}
