using System.IO;
using Lodestar.Embeddings.Search;
using Xunit;

namespace Lodestar.Embeddings.Tests;

public sealed class EmbeddingIndexBlockTests
{
    /// <summary>Scores are compared to four places; the index stores float, not double.</summary>
    private const int Places = 4;

    [Fact]
    public void FromBlock_scores_exactly_as_an_index_built_by_Add()
    {
        float[] block = [3f, 4f, 0f, 1f, 2f, 2f];

        var added = new EmbeddingIndex(dimension: 2);
        for (int item = 0; item < 3; item++)
        {
            added.Add(block.AsSpan(item * 2, 2));
        }

        EmbeddingIndex bulk = EmbeddingIndex.FromBlock(block, 2, BlockNormalization.Normalize);

        // The index exposes no vector accessor, so equal scores for a query is what "the
        // same bits" is observable as; both paths call NormalizeStored, making this exact.
        Assert.Equal(added.Search([1f, 1f], 3), bulk.Search([1f, 1f], 3));
    }

    [Fact]
    public void AlreadyNormalized_stores_the_block_untouched()
    {
        // A block that is not normalized, taken as though it were. The query is normalized
        // to (0.6, 0.8) and the stored vector is not, so the score is |(3,4)| = 5.
        EmbeddingIndex index = EmbeddingIndex.FromBlock(
            [3f, 4f], 2, BlockNormalization.AlreadyNormalized);

        Assert.Equal(5f, index.Search([3f, 4f], 1)[0].Score, Places);
    }

    [Fact]
    public void Off_normalizes_neither_the_block_nor_the_query()
    {
        EmbeddingIndex index = EmbeddingIndex.FromBlock([3f, 4f], 2, BlockNormalization.Off);

        Assert.Equal(25f, index.Search([3f, 4f], 1)[0].Score, Places);
    }

    [Fact]
    public void FromBlock_copies_so_the_caller_can_reuse_its_buffer()
    {
        float[] block = [1f, 0f];
        EmbeddingIndex index = EmbeddingIndex.FromBlock(
            block, 2, BlockNormalization.AlreadyNormalized);

        block[0] = 0f;
        block[1] = 1f;

        Assert.Equal(1f, index.Search([1f, 0f], 1)[0].Score, Places);
    }

    [Fact]
    public void Ids_travel_with_the_block()
    {
        EmbeddingIndex index = EmbeddingIndex.FromBlock(
            [1f, 0f, 0f, 1f], 2, BlockNormalization.AlreadyNormalized, ["east", null]);

        Assert.True(index.HasIds);
        Assert.Equal("east", index.GetId(0));
        Assert.Null(index.GetId(1));
    }

    [Fact]
    public void An_empty_block_makes_an_empty_index()
    {
        EmbeddingIndex index = EmbeddingIndex.FromBlock([], 2, BlockNormalization.Normalize);

        Assert.Equal(0, index.Count);
        Assert.Equal(2, index.Dimension);
    }

    [Fact]
    public void A_block_that_is_not_a_multiple_of_the_dimension_is_refused()
    {
        ArgumentException e = Assert.Throws<ArgumentException>(
            () => EmbeddingIndex.FromBlock([1f, 2f, 3f], 2, BlockNormalization.Off));

        Assert.Contains("not a multiple", e.Message, StringComparison.Ordinal);
    }

    [Fact]
    public void Ids_of_the_wrong_length_are_refused()
    {
        ArgumentException e = Assert.Throws<ArgumentException>(
            () => EmbeddingIndex.FromBlock([1f, 0f], 2, BlockNormalization.Off, ["a", "b"]));

        Assert.Contains("2 entries for 1", e.Message, StringComparison.Ordinal);
    }

    [Fact]
    public void A_dimension_below_one_is_refused()
    {
        Assert.Throws<ArgumentOutOfRangeException>(
            () => EmbeddingIndex.FromBlock([1f, 0f], 0, BlockNormalization.Off));
    }

    [Fact]
    public void A_normalization_outside_the_enum_is_refused()
    {
        Assert.Throws<ArgumentOutOfRangeException>(
            () => EmbeddingIndex.FromBlock([1f, 0f], 2, (BlockNormalization)99));
    }

    [Fact]
    public void FromOwnedBlock_takes_the_array_rather_than_copying_it()
    {
        float[] block = [1f, 0f];
        EmbeddingIndex index = EmbeddingIndex.FromOwnedBlock(
            block, 2, BlockNormalization.AlreadyNormalized);

        Assert.Equal(1f, index.Search([1f, 0f], 1)[0].Score, Places);

        // Ownership transferred, so writing to the array afterwards moves the score.
        // Asserted rather than only documented: breaking it raises nothing at run time.
        block[0] = 0f;
        block[1] = 1f;

        Assert.Equal(0f, index.Search([1f, 0f], 1)[0].Score, Places);
    }

    [Fact]
    public void FromOwnedBlock_normalizes_the_callers_array_in_place()
    {
        float[] block = [3f, 4f];
        EmbeddingIndex index = EmbeddingIndex.FromOwnedBlock(
            block, 2, BlockNormalization.Normalize);

        Assert.Equal(0.6f, block[0], Places);
        Assert.Equal(0.8f, block[1], Places);
        Assert.Equal(1f, index.Search([3f, 4f], 1)[0].Score, Places);
    }

    [Fact]
    public void FromOwnedBlock_scores_exactly_as_FromBlock()
    {
        float[] copied = [3f, 4f, 0f, 1f];
        float[] owned = [3f, 4f, 0f, 1f];

        EmbeddingIndex a = EmbeddingIndex.FromBlock(copied, 2, BlockNormalization.Normalize);
        EmbeddingIndex b = EmbeddingIndex.FromOwnedBlock(owned, 2, BlockNormalization.Normalize);

        Assert.Equal(a.Search([1f, 1f], 2), b.Search([1f, 1f], 2));
    }

    [Fact]
    public void FromOwnedBlock_refuses_a_null_block()
    {
        Assert.Throws<ArgumentNullException>(
            () => EmbeddingIndex.FromOwnedBlock(null!, 2, BlockNormalization.Off));
    }

    [Fact]
    public void FromOwnedBlock_refuses_a_block_that_is_not_a_multiple_of_the_dimension()
    {
        ArgumentException e = Assert.Throws<ArgumentException>(
            () => EmbeddingIndex.FromOwnedBlock([1f, 2f, 3f], 2, BlockNormalization.Off));

        Assert.Contains("not a multiple", e.Message, StringComparison.Ordinal);
    }

    [Fact]
    public void A_reloaded_AlreadyNormalized_block_is_not_normalized_a_second_time()
    {
        // The stored vector (3, 4) is not unit length; the query normalizes to (0.6, 0.8).
        // Score is |(3,4)| = 5, unless reload normalizes it again and drops it to 1.
        EmbeddingIndex index = EmbeddingIndex.FromBlock(
            [3f, 4f], 2, BlockNormalization.AlreadyNormalized);

        using var stream = new MemoryStream();
        index.Save(stream);
        stream.Position = 0;
        EmbeddingIndex reloaded = EmbeddingIndex.Load(stream);

        Assert.Equal(5f, reloaded.Search([3f, 4f], 1)[0].Score, Places);
    }

    [Fact]
    public void DivideRow_rounds_every_element_as_the_scalar_division_does()
    {
        float[] specials = [0f, -0f, float.Epsilon, -float.Epsilon, 1e-40f, float.MaxValue, -float.MaxValue, float.NaN, float.PositiveInfinity, 1e-30f];
        double[] norms = [1.0, 3.0, 7.1e-3, 1e-300, 1e300, 0.3, double.Epsilon, 5e-324 * 3];

        // S2245 / CA5394: seeded, so a failure reproduces; nothing here is security-sensitive.
#pragma warning disable S2245, CA5394
        var random = new Random(474);
#pragma warning restore S2245, CA5394
        for (int length = 0; length <= 70; length++)
        {
            foreach (double norm in norms)
            {
                var row = new float[length];
                for (int i = 0; i < length; i++)
                {
#pragma warning disable CA5394
                    row[i] = random.Next(6) == 0
                        ? specials[random.Next(specials.Length)]
                        : (float)((random.NextDouble() - 0.5) * Math.Pow(10, random.Next(-45, 39)));
#pragma warning restore CA5394
                }

                float[] expected = [.. row.Select(v => (float)(v / norm))];
                EmbeddingIndex.DivideRow(row, norm);
                Assert.Equal(
                    expected.Select(BitConverter.SingleToInt32Bits),
                    row.Select(BitConverter.SingleToInt32Bits));
            }
        }
    }
}
