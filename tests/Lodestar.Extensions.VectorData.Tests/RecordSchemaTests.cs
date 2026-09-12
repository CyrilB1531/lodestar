using Microsoft.Extensions.VectorData;
using Xunit;

namespace Lodestar.Extensions.VectorData.Tests;

/// <summary>A record carrying all three kinds of property, as a consumer writes one.</summary>
public sealed class Document
{
    [VectorStoreKey]
    public string Id { get; set; } = string.Empty;

    [VectorStoreData(IsFullTextIndexed = true)]
    public string Text { get; set; } = string.Empty;

    [VectorStoreVector(3)]
    public ReadOnlyMemory<float> Embedding { get; set; }
}

/// <summary>The same, with nothing marked for full-text search.</summary>
public sealed class VectorOnly
{
    [VectorStoreKey]
    public int Id { get; set; }

    [VectorStoreVector(2)]
    public ReadOnlyMemory<float> Embedding { get; set; }
}

public sealed class RecordSchemaTests
{
    [Fact]
    public void Attributes_name_the_key_the_vector_and_the_text()
    {
        RecordSchema<string, Document> schema = RecordSchema<string, Document>.Create(null);
        var record = new Document { Id = "a", Text = "the cat sat", Embedding = new float[] { 1f, 0f, 0f } };

        Assert.Equal("a", schema.KeyOf(record));
        Assert.Equal("the cat sat", schema.FullTextOf(record));
        Assert.Equal(3, schema.Dimension);
        Assert.True(schema.HasFullText);
        Assert.Equal(1f, schema.VectorOf(record).Span[0]);
    }

    [Fact]
    public void A_record_with_no_full_text_property_says_so_rather_than_throwing()
    {
        RecordSchema<int, VectorOnly> schema = RecordSchema<int, VectorOnly>.Create(null);

        Assert.False(schema.HasFullText);
        Assert.Equal(2, schema.Dimension);
    }

    [Fact]
    public void A_definition_overrides_the_attributes()
    {
        var definition = new VectorStoreCollectionDefinition
        {
            Properties =
            [
                new VectorStoreKeyProperty("Id", typeof(string)),
                new VectorStoreVectorProperty("Embedding", typeof(ReadOnlyMemory<float>), 3),
            ],
        };

        RecordSchema<string, Document> schema = RecordSchema<string, Document>.Create(definition);

        // The definition names no full-text property, so the collection has no keyword half
        // even though the attribute on Text says otherwise.
        Assert.False(schema.HasFullText);
    }

    [Fact]
    public void A_record_with_no_key_is_refused_by_name()
    {
        ArgumentException error = Assert.Throws<ArgumentException>(
            () => RecordSchema<string, string>.Create(null));

        Assert.Contains("VectorStoreKey", error.Message, StringComparison.Ordinal);
    }
}
