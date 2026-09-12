using Lodestar.Embeddings.Search;
using Xunit;

namespace Lodestar.Extensions.VectorData.Tests;

public sealed class DerivedIndexesTests
{
    private static Document Doc(string id, string text, params float[] vector) =>
        new() { Id = id, Text = text, Embedding = vector };

    [Fact]
    public void The_block_holds_one_row_per_record_in_key_order()
    {
        Document[] records =
        [
            Doc("a", "the cat sat", 1f, 0f, 0f),
            Doc("b", "the dog ran", 0f, 1f, 0f),
        ];

        DerivedIndexes<string> built = DerivedIndexes<string>.Build(
            records, RecordSchema<string, Document>.Create(null), new LodestarVectorStoreOptions());

        Assert.Equal(2, built.Vectors.Count);
        Assert.Equal(3, built.Vectors.Dimension);
        Assert.Equal(["a", "b"], built.Keys);
    }

    [Fact]
    public void The_keyword_half_is_built_over_the_same_documents()
    {
        Document[] records =
        [
            Doc("a", "the cat sat", 1f, 0f, 0f),
            Doc("b", "the dog ran", 0f, 1f, 0f),
        ];

        DerivedIndexes<string> built = DerivedIndexes<string>.Build(
            records, RecordSchema<string, Document>.Create(null), new LodestarVectorStoreOptions());

        Assert.NotNull(built.Keywords);
        Assert.NotNull(built.Vectorizer);
        Assert.Equal(2, built.Keywords.DocumentCount);
    }

    [Fact]
    public void A_record_type_with_no_full_text_property_builds_vectors_alone()
    {
        VectorOnly[] records =
        [
            new() { Id = 1, Embedding = new float[] { 1f, 0f } },
        ];

        DerivedIndexes<int> built = DerivedIndexes<int>.Build(
            records, RecordSchema<int, VectorOnly>.Create(null), new LodestarVectorStoreOptions());

        Assert.Equal(1, built.Vectors.Count);
        Assert.Null(built.Keywords);
        Assert.Null(built.Vectorizer);
    }

    [Fact]
    public void An_empty_collection_builds_an_empty_index_rather_than_throwing()
    {
        DerivedIndexes<string> built = DerivedIndexes<string>.Build(
            [], RecordSchema<string, Document>.Create(null), new LodestarVectorStoreOptions());

        Assert.Equal(0, built.Vectors.Count);
        Assert.Null(built.Keywords);
        Assert.Empty(built.Keys);
    }

    [Fact]
    public void A_record_whose_vector_is_the_wrong_width_is_refused_by_name()
    {
        Document[] records = [Doc("a", "the cat sat", 1f, 0f)];

        ArgumentException error = Assert.Throws<ArgumentException>(() => DerivedIndexes<string>.Build(
            records, RecordSchema<string, Document>.Create(null), new LodestarVectorStoreOptions()));

        Assert.Contains("a", error.Message, StringComparison.Ordinal);
    }
}
