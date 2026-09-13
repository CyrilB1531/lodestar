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

/// <summary>A vector and no key.</summary>
public sealed class Keyless
{
    [VectorStoreVector(3)]
    public ReadOnlyMemory<float> Embedding { get; set; }
}

/// <summary>A key and no vector.</summary>
public sealed class Vectorless
{
    [VectorStoreKey]
    public string Id { get; set; } = string.Empty;
}

/// <summary>A vector of doubles rather than the single-precision memory the index takes.</summary>
public sealed class DoubleVector
{
    [VectorStoreKey]
    public string Id { get; set; } = string.Empty;

    [VectorStoreVector(3)]
    public ReadOnlyMemory<double> Embedding { get; set; }
}

/// <summary>A vector declaring a distance, which this store does not compute.</summary>
public sealed class EuclideanVector
{
    [VectorStoreKey]
    public string Id { get; set; } = string.Empty;

    [VectorStoreVector(3, DistanceFunction = DistanceFunction.EuclideanDistance)]
    public ReadOnlyMemory<float> Embedding { get; set; }
}

/// <summary>A vector declaring the one distance function this store computes.</summary>
public sealed class CosineVector
{
    [VectorStoreKey]
    public string Id { get; set; } = string.Empty;

    [VectorStoreVector(3, DistanceFunction = DistanceFunction.CosineSimilarity)]
    public ReadOnlyMemory<float> Embedding { get; set; }
}

public sealed class RecordSchemaTests
{
    [Fact]
    public void A_record_type_with_no_key_is_refused_at_construction()
    {
        ArgumentException error = Assert.Throws<ArgumentException>(
            () => new LodestarVectorStoreCollection<string, Keyless>("keyless"));

        Assert.Contains("[VectorStoreKey]", error.Message, StringComparison.Ordinal);
    }

    [Fact]
    public void A_record_type_with_no_vector_is_refused_at_construction()
    {
        ArgumentException error = Assert.Throws<ArgumentException>(
            () => new LodestarVectorStoreCollection<string, Vectorless>("vectorless"));

        Assert.Contains("[VectorStoreVector]", error.Message, StringComparison.Ordinal);
    }

    [Fact]
    public void A_vector_that_is_not_read_only_memory_is_refused_at_construction()
    {
        ArgumentException error = Assert.Throws<ArgumentException>(
            () => new LodestarVectorStoreCollection<string, DoubleVector>("doubles"));

        Assert.Contains("ReadOnlyMemory<float>", error.Message, StringComparison.Ordinal);
    }

    // No collection-side check exists for a width below 1: the abstraction refuses it first, in
    // VectorStoreVectorAttribute's constructor and VectorStoreVectorProperty.Dimensions' setter.
    [Fact]
    public void A_width_below_one_never_reaches_a_collection()
    {
        Assert.Throws<ArgumentOutOfRangeException>(
            () => new VectorStoreVectorProperty("Embedding", typeof(ReadOnlyMemory<float>), 3) { Dimensions = 0 });
    }

    [Fact]
    public void A_definition_naming_a_property_the_type_lacks_is_refused_at_construction()
    {
        var definition = new VectorStoreCollectionDefinition
        {
            Properties =
            [
                new VectorStoreKeyProperty("Id", typeof(string)),
                new VectorStoreVectorProperty("Missing", typeof(ReadOnlyMemory<float>), 3),
            ],
        };

        ArgumentException error = Assert.Throws<ArgumentException>(
            () => new LodestarVectorStoreCollection<string, Document>("documents", definition: definition));

        Assert.Contains("Missing", error.Message, StringComparison.Ordinal);
    }

    [Fact]
    public void A_distance_function_on_the_attribute_is_refused_with_the_reason()
    {
        NotSupportedException error = Assert.Throws<NotSupportedException>(
            () => new LodestarVectorStoreCollection<string, EuclideanVector>("euclidean"));

        Assert.Contains(DistanceFunction.EuclideanDistance, error.Message, StringComparison.Ordinal);
        Assert.Contains("ScoreThreshold", error.Message, StringComparison.Ordinal);
    }

    [Fact]
    public void A_distance_function_on_a_definition_is_refused_with_the_reason()
    {
        var definition = new VectorStoreCollectionDefinition
        {
            Properties =
            [
                new VectorStoreKeyProperty("Id", typeof(string)),
                new VectorStoreVectorProperty("Embedding", typeof(ReadOnlyMemory<float>), 3)
                {
                    DistanceFunction = DistanceFunction.DotProductSimilarity,
                },
            ],
        };

        NotSupportedException error = Assert.Throws<NotSupportedException>(
            () => new LodestarVectorStoreCollection<string, Document>("documents", definition: definition));

        Assert.Contains(DistanceFunction.DotProductSimilarity, error.Message, StringComparison.Ordinal);
    }

    [Fact]
    public void Cosine_similarity_declared_explicitly_is_accepted()
    {
        using var collection = new LodestarVectorStoreCollection<string, CosineVector>("cosine");

        Assert.Equal(3, collection.Schema.Dimension);
    }

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
