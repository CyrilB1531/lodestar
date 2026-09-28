using Lodestar.Embeddings.Persistence;
using Lodestar.Embeddings.Search;
using Lodestar.Embeddings.Tokenization;
using Xunit;

namespace Lodestar.Embeddings.Tests;

/// <summary>The invariant sweep after #1320: the .npy block and ill-formed text (#1322, #1324); the padded batch is in BatchEncoderTests (#1323).</summary>
public sealed class SweepFindingsTests
{
    private static SentencePieceVocabulary Model(string file) =>
        SentencePieceModelLoader.Load(Path.Combine(AppContext.BaseDirectory, "oracles", file));

    [Fact]
    public void A_block_longer_than_one_slice_round_trips()
    {
        // Three slices and a remainder; the slicing is what keeps a block past 536 million floats writable (#1322).
        float[] values = [.. Enumerable.Range(0, (3 * 60 * 1024) + 17).Select(i => (i * 0.25f) - 1000f)];
        using var written = new MemoryStream();

        NpyFile.Write(written, values, values.Length);

        Assert.Equal(values, NpyFile.Read(written.ToArray()).Values.ToArray());
    }

    [Theory]
    [InlineData("tiny_sp.model")]
    [InlineData("xlmr_fairseq.model")]
    public void A_lone_surrogate_is_refused_with_or_without_a_normalizer(string file)
    {
        // Built at run time: a lone surrogate in a source literal does not survive UTF-8.
        string text = "a" + (char)0xD800 + "b";
        var tokenizer = new SentencePieceTokenizer(Model(file));

        ArgumentException refused = Assert.Throws<ArgumentException>(() => tokenizer.Encode(text));

        Assert.Equal("text", refused.ParamName);
    }

    // 64 vectors of 1,024 dimensions: past one 61,440-float slice, which the writers now step by (#1322).
    private static EmbeddingIndex LargeIndex()
    {
        var index = new EmbeddingIndex(1024);
        for (int v = 0; v < 64; v++)
        {
            index.Add([.. Enumerable.Range(0, 1024).Select(i => (float)Math.Sin((v * 1024) + i))]);
        }

        return index;
    }

    private static float[] Probe() => [.. Enumerable.Range(0, 1024).Select(i => (float)Math.Sin((5 * 1024) + i))];

    [Fact]
    public void An_index_larger_than_one_base64_slice_round_trips()
    {
        using var written = new MemoryStream();
        LargeIndex().Save(written);

        EmbeddingIndex back = EmbeddingIndex.Load(new MemoryStream(written.ToArray()));

        Assert.Equal(5, back.Search(Probe(), 1)[0].Index);
    }

    [Fact]
    public async Task The_asynchronous_save_writes_the_whole_block_past_one_slice()
    {
        // Its byte count once wrapped in int and wrote an empty block, silently.
        using var written = new MemoryStream();
        await LargeIndex().SaveAsync(written, TestContext.Current.CancellationToken);

        EmbeddingIndex back = await EmbeddingIndex.LoadAsync(
            new MemoryStream(written.ToArray()), cancellationToken: TestContext.Current.CancellationToken);

        Assert.Equal(5, back.Search(Probe(), 1)[0].Index);
    }

    [Fact]
    public void A_refused_write_leaves_the_file_it_would_replace()
    {
        // OpenWrite truncates; the shape is now checked before the file is opened.
        string path = Path.GetTempFileName();
        try
        {
            File.WriteAllBytes(path, [1, 2, 3]);

            Assert.Throws<ArgumentException>(() => NpyFile.Write(path, [1f, 2f, 3f], 4));

            Assert.Equal([1, 2, 3], File.ReadAllBytes(path));
        }
        finally
        {
            File.Delete(path);
        }
    }

    [Fact]
    public void A_batch_names_its_own_parameter_when_one_text_is_refused()
    {
        var encoder = new BatchEncoder(
            new SentencePieceTokenizer(Model("xlmr_fairseq.model")), new EncodingOptions { Template = SpecialTokenTemplate.Roberta });

        ArgumentException refused = Assert.Throws<ArgumentException>(
            () => encoder.EncodeAll(["fine", "a" + (char)0xD800 + "b"], TestContext.Current.CancellationToken));

        Assert.Equal("texts", refused.ParamName);
    }
}
