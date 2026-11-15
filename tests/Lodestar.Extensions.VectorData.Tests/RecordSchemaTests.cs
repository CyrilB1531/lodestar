using System.Linq.Expressions;
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

/// <summary>A key of one type, for a collection keyed by another.</summary>
public sealed class StringKeyed
{
    [VectorStoreKey]
    public string Id { get; set; } = string.Empty;

    [VectorStoreVector(2)]
    public ReadOnlyMemory<float> Embedding { get; set; }
}

/// <summary>A full-text mark on a property that holds no text.</summary>
public sealed class NumberMarkedFullText
{
    [VectorStoreKey]
    public string Id { get; set; } = string.Empty;

    [VectorStoreData(IsFullTextIndexed = true)]
    public int Count { get; set; }

    [VectorStoreVector(2)]
    public ReadOnlyMemory<float> Embedding { get; set; }
}

/// <summary>Two vectors, a shape the abstraction allows and this store does not search.</summary>
public sealed class TwoVectors
{
    [VectorStoreKey]
    public string Id { get; set; } = string.Empty;

    [VectorStoreVector(3)]
    public ReadOnlyMemory<float> Title { get; set; }

    [VectorStoreVector(3)]
    public ReadOnlyMemory<float> Body { get; set; }
}

/// <summary>Two full-text indexed texts, of which the keyword half would index one.</summary>
public sealed class TwoTexts
{
    [VectorStoreKey]
    public string Id { get; set; } = string.Empty;

    [VectorStoreData(IsFullTextIndexed = true)]
    public string Title { get; set; } = string.Empty;

    [VectorStoreData(IsFullTextIndexed = true)]
    public string Body { get; set; } = string.Empty;

    [VectorStoreVector(3)]
    public ReadOnlyMemory<float> Embedding { get; set; }
}

/// <summary>Two keys, of which a write would address the record by one.</summary>
public sealed class TwoKeys
{
    [VectorStoreKey]
    public string Id { get; set; } = string.Empty;

    [VectorStoreKey]
    public string OtherId { get; set; } = string.Empty;

    [VectorStoreVector(3)]
    public ReadOnlyMemory<float> Embedding { get; set; }
}

/// <summary>A vector reached through an interface, as generic repository code reads it.</summary>
public interface IEmbedded
{
    ReadOnlyMemory<float> Embedding { get; }
}

/// <summary>A record implementing <see cref="IEmbedded"/>.</summary>
public sealed class EmbeddedNote : IEmbedded
{
    [VectorStoreKey]
    public string Id { get; set; } = string.Empty;

    [VectorStoreVector(3)]
    public ReadOnlyMemory<float> Embedding { get; set; }
}

/// <summary>A key also marked as data, two roles a definition could not list.</summary>
public sealed class KeyAsText
{
    [VectorStoreKey]
    [VectorStoreData]
    public string Id { get; set; } = string.Empty;

    [VectorStoreVector(3)]
    public ReadOnlyMemory<float> Embedding { get; set; }
}

/// <summary>A vector whose getter <see cref="SetterOverride"/> inherits.</summary>
public class VirtualVector
{
    [VectorStoreKey]
    public string Id { get; set; } = string.Empty;

    [VectorStoreVector(3)]
    public virtual ReadOnlyMemory<float> Embedding { get; set; }
}

/// <summary>An override of the setter alone, which reflection lists without a getter.</summary>
public sealed class SetterOverride : VirtualVector
{
    public override ReadOnlyMemory<float> Embedding
    {
        set => base.Embedding = value;
    }
}

/// <summary>A key the store could write but never read back.</summary>
public sealed class SetterOnlyKey
{
    // S2376, CA1044, CA1822: write-only and stateless on purpose, the shape the schema has to refuse.
#pragma warning disable S2376, CA1044, CA1822
    [VectorStoreKey]
    public string Id
    {
        set => _ = value;
    }
#pragma warning restore S2376, CA1044, CA1822

    [VectorStoreVector(3)]
    public ReadOnlyMemory<float> Embedding { get; set; }
}

/// <summary>A sole full-text property the store could write but never read back.</summary>
public sealed class SetterOnlyText
{
    [VectorStoreKey]
    public string Id { get; set; } = string.Empty;

    // S2376, CA1044, CA1822: write-only and stateless on purpose, the shape the schema has to refuse.
#pragma warning disable S2376, CA1044, CA1822
    [VectorStoreData(IsFullTextIndexed = true)]
    public string Text
    {
        set => _ = value;
    }
#pragma warning restore S2376, CA1044, CA1822

    [VectorStoreVector(3)]
    public ReadOnlyMemory<float> Embedding { get; set; }
}

// S2376, CA1044, S1185: a write-only `new` slot and its override are the shape the schema has to refuse.
#pragma warning disable S2376, CA1044, S1185
/// <summary>A virtual vector a <c>new</c> slot with no getter hides, which <see cref="OverNewSlot"/> overrides.</summary>
public class NewSlot : VirtualVector
{
    public new virtual ReadOnlyMemory<float> Embedding
    {
        set => base.Embedding = value;
    }
}

/// <summary>An override of the <c>new</c> slot's setter: its own code has no getter to read.</summary>
public sealed class OverNewSlot : NewSlot
{
    [VectorStoreVector(3)]
    public override ReadOnlyMemory<float> Embedding
    {
        set => base.Embedding = value;
    }
}
#pragma warning restore S2376, CA1044, S1185

/// <summary>A vector the store could write but never read back.</summary>
public sealed class SetterOnlyVector
{
    [VectorStoreKey]
    public string Id { get; set; } = string.Empty;

    // S2376, CA1044, CA1822: write-only and stateless on purpose, the shape the schema has to refuse.
#pragma warning disable S2376, CA1044, CA1822
    [VectorStoreVector(3)]
    public ReadOnlyMemory<float> Embedding
    {
        set => _ = value;
    }
#pragma warning restore S2376, CA1044, CA1822
}

/// <summary>A full-text property, which <see cref="UnmarkedHider"/> hides with a member of its own.</summary>
public class MarkedText
{
    [VectorStoreData(IsFullTextIndexed = true)]
    public string Text { get; set; } = string.Empty;
}

/// <summary>An unmarked <c>new</c> member over a marked base one: the record's code reads this one.</summary>
public sealed class UnmarkedHider : MarkedText
{
    [VectorStoreKey]
    public string Id { get; set; } = string.Empty;

    public new int Text { get; set; }

    [VectorStoreVector(3)]
    public ReadOnlyMemory<float> Embedding { get; set; }
}

/// <summary>A <c>new</c> member of the same type over a marked base text, which hides it from the full listing.</summary>
public sealed class SameTypeHider : MarkedText
{
    [VectorStoreKey]
    public string Id { get; set; } = string.Empty;

    public new string Text { get; set; } = string.Empty;

    [VectorStoreVector(3)]
    public ReadOnlyMemory<float> Embedding { get; set; }
}

/// <summary>A base text the full-text property of <see cref="HidingNote"/> hides.</summary>
public class PlainText
{
    public string Text { get; set; } = string.Empty;
}

/// <summary>A full-text property declared with <c>new</c> over a base property of the same name.</summary>
public sealed class HidingNote : PlainText
{
    [VectorStoreKey]
    public string Id { get; set; } = string.Empty;

    [VectorStoreData(IsFullTextIndexed = true)]
    public new string Text { get; set; } = string.Empty;

    [VectorStoreVector(3)]
    public ReadOnlyMemory<float> Embedding { get; set; }
}

/// <summary>A vector declared abstract, which an expression names by this declaration.</summary>
public abstract class AbstractNote
{
    public abstract ReadOnlyMemory<float> Embedding { get; set; }
}

/// <summary>The override reflection lists for <see cref="AbstractNote.Embedding"/>.</summary>
public sealed class OverridingNote : AbstractNote
{
    [VectorStoreKey]
    public string Id { get; set; } = string.Empty;

    [VectorStoreVector(3)]
    public override ReadOnlyMemory<float> Embedding { get; set; }
}

/// <summary>A record type that is itself an interface, which the class constraint admits.</summary>
public interface INote
{
    [VectorStoreKey]
    string Id { get; }

    [VectorStoreVector(3)]
    ReadOnlyMemory<float> Embedding { get; }
}

/// <summary>What an <see cref="INote"/> collection holds.</summary>
public sealed class Note : INote
{
    public string Id { get; set; } = string.Empty;

    public ReadOnlyMemory<float> Embedding { get; set; }
}

/// <summary>One generic interface, implemented twice by <see cref="TwiceHeld"/>.</summary>
public interface IHas<out T>
{
    T Value { get; }
}

/// <summary>A vector that implements one instantiation of <see cref="IHas{T}"/>, and a string the other.</summary>
public sealed class TwiceHeld : IHas<ReadOnlyMemory<float>>, IHas<string>
{
    [VectorStoreKey]
    public string Id { get; set; } = string.Empty;

    [VectorStoreVector(3)]
    public ReadOnlyMemory<float> Value { get; set; }

    string IHas<string>.Value => Id;
}

/// <summary>A key declared on a base interface.</summary>
public interface IKeyed
{
    [VectorStoreKey]
    string Id { get; }
}

/// <summary>A record interface whose key it inherits from <see cref="IKeyed"/>.</summary>
public interface IKeyedNote : IKeyed
{
    [VectorStoreVector(3)]
    ReadOnlyMemory<float> Embedding { get; }
}

/// <summary>A key, which <see cref="KeyHider"/> hides with an unmarked member of its own.</summary>
public class MarkedKey
{
    [VectorStoreKey]
    public string Id { get; set; } = string.Empty;
}

/// <summary>An unmarked <c>new</c> member over a marked base key.</summary>
public sealed class KeyHider : MarkedKey
{
    public new string Id { get; set; } = string.Empty;

    [VectorStoreVector(3)]
    public ReadOnlyMemory<float> Embedding { get; set; }
}

/// <summary>A vector, which <see cref="VectorHider"/> hides with an unmarked member of its own.</summary>
public class MarkedVector
{
    [VectorStoreVector(3)]
    public ReadOnlyMemory<float> Embedding { get; set; }
}

/// <summary>An unmarked <c>new</c> member over a marked base vector.</summary>
public sealed class VectorHider : MarkedVector
{
    [VectorStoreKey]
    public string Id { get; set; } = string.Empty;

    public new ReadOnlyMemory<float> Embedding { get; set; }
}

/// <summary>A vector decoded into one buffer every record shares, as a getter avoiding an allocation may.</summary>
public sealed class SharedBuffer(float[] scratch, float[] values)
{
    [VectorStoreKey]
    public string Id { get; set; } = string.Empty;

    [VectorStoreVector(3)]
    public ReadOnlyMemory<float> Embedding
    {
        get
        {
            values.CopyTo(scratch, 0);
            return scratch;
        }
    }
}

/// <summary>A virtual full-text property, which <see cref="UnflaggedOverride"/> overrides as plain data.</summary>
public class VirtualText
{
    [VectorStoreData(IsFullTextIndexed = true)]
    public virtual string Text { get; set; } = string.Empty;
}

/// <summary>An override whose own mark turns the full-text flag off: no <c>new</c> member hides anything.</summary>
public sealed class UnflaggedOverride : VirtualText
{
    [VectorStoreKey]
    public string Id { get; set; } = string.Empty;

    [VectorStoreData]
    public override string Text { get; set; } = string.Empty;

    [VectorStoreVector(3)]
    public ReadOnlyMemory<float> Embedding { get; set; }
}

/// <summary>A vector on a generic base, which <see cref="GenericVectorHider"/> hides.</summary>
/// <typeparam name="T">Unused: the base is generic so that reflection names it with an arity suffix.</typeparam>
#pragma warning disable S2326 // T is unused on purpose: the base only has to be generic.
public class GenericMarkedVector<T>
#pragma warning restore S2326
{
    [VectorStoreVector(3)]
    public ReadOnlyMemory<float> Embedding { get; set; }
}

/// <summary>An unmarked <c>new</c> member over a marked generic base vector.</summary>
public sealed class GenericVectorHider : GenericMarkedVector<int>
{
    [VectorStoreKey]
    public string Id { get; set; } = string.Empty;

    public new ReadOnlyMemory<float> Embedding { get; set; }
}

/// <summary>An override of <see cref="VirtualKey.Id"/> that inherits its mark without repeating it.</summary>
public class InheritedKey : VirtualKey
{
    public override string Id { get; set; } = string.Empty;
}

/// <summary>An unmarked <c>new</c> member over the override <see cref="InheritedKey"/> declares.</summary>
public sealed class InheritedKeyHider : InheritedKey
{
    public new string Id { get; set; } = string.Empty;

    [VectorStoreVector(3)]
    public ReadOnlyMemory<float> Embedding { get; set; }
}

/// <summary>A marked virtual key, which <see cref="InheritedKey"/> overrides.</summary>
public class VirtualKey
{
    [VectorStoreKey]
    public virtual string Id { get; set; } = string.Empty;
}

/// <summary>A record whose key, text or vector getter throws, the one <see cref="Throws"/> names.</summary>
public sealed class ThrowingGetters(string id, string? throws = null)
{
    /// <summary>The property whose getter throws, or <see langword="null"/> for none.</summary>
    public string? Throws { get; } = throws;

    [VectorStoreKey]
    public string Id => Fail(nameof(Id), id);

    [VectorStoreData(IsFullTextIndexed = true)]
    public string Text => Fail(nameof(Text), "the cat");

    [VectorStoreVector(3)]
    public ReadOnlyMemory<float> Embedding => Fail(nameof(Embedding), new ReadOnlyMemory<float>([1f, 0f, 0f]));

    private T Fail<T>(string name, T value) =>
        Throws == name ? throw new InvalidOperationException($"{name} cannot be read.") : value;
}

/// <summary>An unmarked <c>new</c> member over <see cref="VirtualText.Text"/>, which does hide it.</summary>
public sealed class NewOverVirtualText : VirtualText
{
    [VectorStoreKey]
    public string Id { get; set; } = string.Empty;

    public new string Text { get; set; } = string.Empty;

    [VectorStoreVector(3)]
    public ReadOnlyMemory<float> Embedding { get; set; }
}

/// <summary>A vector whose getter answers a wider buffer on every read after the first, as a recomputed cache may.</summary>
public sealed class ShiftingVector
{
    [VectorStoreKey]
    public string Id { get; set; } = string.Empty;

    [VectorStoreVector(3)]
    public ReadOnlyMemory<float> Embedding => ++Reads == 1 ? new float[] { 1f, 0f, 0f } : new float[] { 0f, 1f, 0f, 0f };

    /// <summary>How many times <see cref="Embedding"/> was read.</summary>
    public int Reads { get; private set; }
}

public sealed class RecordSchemaTests
{
    private static readonly float[] Query = [1f, 0f, 0f];

    private static async Task<LodestarVectorStoreCollection<string, Document>> OneRecordAsync()
    {
        var collection = new LodestarVectorStoreCollection<string, Document>("documents");
        await collection.UpsertAsync(new Document { Id = "a", Text = "the cat", Embedding = Query });
        return collection;
    }

    private static VectorStoreCollectionDefinition DocumentDefinition(params VectorStoreProperty[] extra) => new()
    {
        Properties =
        [
            new VectorStoreKeyProperty("Id", typeof(string)),
            new VectorStoreVectorProperty("Embedding", typeof(ReadOnlyMemory<float>), 3),
            .. extra,
        ],
    };

    [Fact]
    public void Two_vector_properties_are_refused_at_construction_naming_both()
    {
        NotSupportedException error = Assert.Throws<NotSupportedException>(
            () => new LodestarVectorStoreCollection<string, TwoVectors>("vectors"));

        Assert.Contains("(Body, Title)", error.Message, StringComparison.Ordinal);
    }

    [Fact]
    public async Task Two_full_text_properties_refuse_the_hybrid_search_naming_both_and_not_the_vector_one()
    {
        using var collection = new LodestarVectorStoreCollection<string, TwoTexts>("texts");
        await collection.UpsertAsync(new TwoTexts { Id = "a", Title = "cat", Body = "dog", Embedding = Query });

        Assert.Single(await collection.SearchAsync(Query, 1).ToListAsync());
        NotSupportedException error = await Assert.ThrowsAsync<NotSupportedException>(
            async () => await collection.HybridSearchAsync(Query, ["cat"], 1).ToListAsync());
        Assert.Contains("(Body, Title)", error.Message, StringComparison.Ordinal);
    }

    [Fact]
    public void Two_key_properties_are_refused_at_construction_naming_both()
    {
        ArgumentException error = Assert.Throws<ArgumentException>(
            () => new LodestarVectorStoreCollection<string, TwoKeys>("keys"));

        Assert.Contains("(Id, OtherId)", error.Message, StringComparison.Ordinal);
        Assert.Null(error.ParamName);
    }

    [Fact]
    public void A_null_entry_in_a_definition_is_skipped_as_it_was()
    {
        using var collection = new LodestarVectorStoreCollection<string, Document>(
            "documents", definition: DocumentDefinition((VectorStoreProperty)null!));

        Assert.Equal(3, collection.Schema.Dimension);
    }

    [Fact]
    public async Task A_selector_casting_with_as_is_honoured()
    {
        using var collection = new LodestarVectorStoreCollection<string, OverridingNote>("notes");
        await collection.UpsertAsync(new OverridingNote { Id = "a", Embedding = Query });

        Assert.Single(await collection.SearchAsync(
            Query, 1, new VectorSearchOptions<OverridingNote> { VectorProperty = n => (n as AbstractNote).Embedding }).ToListAsync());
    }

    [Fact]
    public void A_record_interface_is_read_for_its_own_members_and_not_its_bases()
    {
        ArgumentException error = Assert.Throws<ArgumentException>(
            () => new LodestarVectorStoreCollection<string, IKeyedNote>("notes"));

        Assert.Contains("[VectorStoreKey]", error.Message, StringComparison.Ordinal);
        Assert.Contains("interfaces it extends are not read", error.Message, StringComparison.Ordinal);
    }

    [Fact]
    public void A_definition_with_two_of_a_kind_is_refused_as_the_attributes_are()
    {
        var vectors = new VectorStoreCollectionDefinition
        {
            Properties =
            [
                new VectorStoreKeyProperty("Id", typeof(string)),
                new VectorStoreVectorProperty("Title", typeof(ReadOnlyMemory<float>), 3),
                new VectorStoreVectorProperty("Body", typeof(ReadOnlyMemory<float>), 3),
            ],
        };

        NotSupportedException twoVectors = Assert.Throws<NotSupportedException>(
            () => new LodestarVectorStoreCollection<string, TwoVectors>("vectors", definition: vectors));
        using var twoTexts = new LodestarVectorStoreCollection<string, TwoTexts>(
            "texts",
            definition: DocumentDefinition(
                new VectorStoreDataProperty("Title", typeof(string)) { IsFullTextIndexed = true },
                new VectorStoreDataProperty("Body", typeof(string)) { IsFullTextIndexed = true }));
        ArgumentException twoKeys = Assert.Throws<ArgumentException>(() => new LodestarVectorStoreCollection<string, Document>(
            "documents", definition: DocumentDefinition(new VectorStoreKeyProperty("Text", typeof(string)))));

        Assert.Contains("(Body, Title)", twoVectors.Message, StringComparison.Ordinal);
        Assert.Contains("(Body, Title)", twoTexts.Schema.KeywordRefusal, StringComparison.Ordinal);
        Assert.Contains("(Id, Text)", twoKeys.Message, StringComparison.Ordinal);
    }

    [Fact]
    public async Task A_selector_through_another_instantiation_of_one_generic_interface_is_refused()
    {
        using var collection = new LodestarVectorStoreCollection<string, TwiceHeld>("held");
        await collection.UpsertAsync(new TwiceHeld { Id = "a", Value = Query });

        Assert.Single(await collection.SearchAsync(Query, 1, new VectorSearchOptions<TwiceHeld>
        {
            VectorProperty = r => ((IHas<ReadOnlyMemory<float>>)r).Value,
        }).ToListAsync());
        await Assert.ThrowsAsync<ArgumentException>(async () => await collection.SearchAsync(Query, 1, new VectorSearchOptions<TwiceHeld>
        {
            VectorProperty = r => ((IHas<string>)r).Value,
        }).ToListAsync());
    }

    [Fact]
    public void A_definition_listing_one_property_twice_is_refused_as_malformed()
    {
        ArgumentException error = Assert.Throws<ArgumentException>(() => new LodestarVectorStoreCollection<string, Document>(
            "documents",
            definition: DocumentDefinition(new VectorStoreVectorProperty("Embedding", typeof(ReadOnlyMemory<float>), 3))));

        Assert.Equal("definition", error.ParamName);
        Assert.Contains("Embedding more than once", error.Message, StringComparison.Ordinal);
    }

    [Fact]
    public async Task A_selector_written_against_an_interface_the_record_implements_is_honoured()
    {
        using var collection = new LodestarVectorStoreCollection<string, EmbeddedNote>("notes");
        await collection.UpsertAsync(new EmbeddedNote { Id = "a", Embedding = Query });

        Assert.Single(await collection.SearchAsync(Query, 1, Selecting<EmbeddedNote>()).ToListAsync());
    }

    [Fact]
    public void A_property_marked_for_two_roles_is_refused_as_a_definition_listing_it_twice_is()
    {
        ArgumentException error = Assert.Throws<ArgumentException>(
            () => new LodestarVectorStoreCollection<string, KeyAsText>("roles"));

        Assert.Contains("KeyAsText.Id", error.Message, StringComparison.Ordinal);
    }

    [Fact]
    public async Task A_vector_overriding_only_its_setter_reads_through_the_getter_it_inherits()
    {
        using var collection = new LodestarVectorStoreCollection<string, SetterOverride>("vectors");
        await collection.UpsertAsync(new SetterOverride { Id = "a", Embedding = Query });

        Assert.Single(await collection.SearchAsync(Query, 1).ToListAsync());
        Assert.Single(await collection.SearchAsync(
            Query, 1, new VectorSearchOptions<SetterOverride> { VectorProperty = r => r.Embedding }).ToListAsync());
    }

    [Fact]
    public void No_expression_tree_reads_the_override_reflection_lists_without_a_getter()
    {
        // So no selector can name it, and the selector check needs no chain of its own (#1798).
        ParameterExpression record = Expression.Parameter(typeof(SetterOverride), "record");
        MemberExpression unreadable = Expression.Property(
            record, typeof(SetterOverride).GetProperty(nameof(SetterOverride.Embedding))!);

        Assert.Throws<ArgumentException>(() => Expression.Convert(unreadable, typeof(object)));
        Assert.Throws<ArgumentException>(() => Expression.Lambda<Func<SetterOverride, ReadOnlyMemory<float>>>(unreadable, record));
    }

    [Theory]
    [InlineData(nameof(ThrowingGetters.Text))]
    [InlineData(nameof(ThrowingGetters.Embedding))]
    public async Task A_getter_that_throws_fails_the_batch_before_any_record_is_written(string failing)
    {
        using var collection = new LodestarVectorStoreCollection<string, ThrowingGetters>("records");

        await Assert.ThrowsAsync<InvalidOperationException>(
            () => collection.UpsertAsync([new ThrowingGetters("a"), new ThrowingGetters("b", failing)]));

        Assert.Null(await collection.GetAsync("a"));
    }

    [Theory]
    [InlineData(nameof(ThrowingGetters.Id))]
    [InlineData(nameof(ThrowingGetters.Text))]
    [InlineData(nameof(ThrowingGetters.Embedding))]
    public async Task A_getter_that_throws_fails_the_upsert_unwrapped_before_the_record_is_held(string failing)
    {
        using var collection = new LodestarVectorStoreCollection<string, ThrowingGetters>("records");

        InvalidOperationException error = await Assert.ThrowsAsync<InvalidOperationException>(
            () => collection.UpsertAsync(new ThrowingGetters("a", failing)));

        Assert.StartsWith(failing, error.Message, StringComparison.Ordinal);
        Assert.Null(await collection.GetAsync("a"));
    }

    [Fact]
    public async Task A_batch_holds_each_record_s_vector_though_the_getter_reuses_one_buffer()
    {
        using var collection = new LodestarVectorStoreCollection<string, SharedBuffer>("vectors");
        float[] scratch = new float[3];

        await collection.UpsertAsync([
            new SharedBuffer(scratch, [1f, 0f, 0f]) { Id = "a" },
            new SharedBuffer(scratch, [0f, 1f, 0f]) { Id = "b" },
        ]);

        VectorSearchResult<SharedBuffer> hit = Assert.Single(await collection.SearchAsync(Query, 1).ToListAsync());
        Assert.Equal("a", hit.Record.Id);
        Assert.Equal(1d, hit.Score!.Value, 6);
    }

    [Fact]
    public void A_hidden_mark_on_a_generic_base_or_an_override_names_the_declaration_hidden()
    {
        Assert.Contains("GenericMarkedVector<Int32>.Embedding is marked", Assert.Throws<ArgumentException>(
            () => new LodestarVectorStoreCollection<string, GenericVectorHider>("vectors")).Message, StringComparison.Ordinal);
        Assert.Contains("InheritedKey.Id is marked", Assert.Throws<ArgumentException>(
            () => new LodestarVectorStoreCollection<string, InheritedKeyHider>("keys")).Message, StringComparison.Ordinal);
    }

    [Fact]
    public void An_override_that_drops_the_full_text_mark_is_not_called_a_hidden_member()
    {
        using var collection = new LodestarVectorStoreCollection<string, UnflaggedOverride>("texts");

        Assert.False(collection.Schema.HasFullText);
        Assert.DoesNotContain("hides", collection.Schema.KeywordRefusal, StringComparison.Ordinal);

        using var hidden = new LodestarVectorStoreCollection<string, NewOverVirtualText>("texts");
        Assert.Contains("VirtualText.Text is marked, and a `new` member hides it", hidden.Schema.KeywordRefusal, StringComparison.Ordinal);
    }

    [Fact]
    public void An_override_whose_chain_has_no_getter_is_refused_rather_than_read_through_a_hidden_base()
    {
        Assert.Contains("has no getter", Assert.Throws<ArgumentException>(
            () => new LodestarVectorStoreCollection<string, OverNewSlot>("vectors")).Message, StringComparison.Ordinal);
    }

    [Fact]
    public void A_key_or_vector_a_new_member_hides_is_named_in_the_refusal()
    {
        Assert.Contains("MarkedKey.Id is marked, and a `new` member hides it", Assert.Throws<ArgumentException>(
            () => new LodestarVectorStoreCollection<string, KeyHider>("keys")).Message, StringComparison.Ordinal);
        Assert.Contains("MarkedVector.Embedding is marked, and a `new` member hides it", Assert.Throws<ArgumentException>(
            () => new LodestarVectorStoreCollection<string, VectorHider>("vectors")).Message, StringComparison.Ordinal);
    }

    [Theory]
    [InlineData(false)]
    [InlineData(true)]
    public async Task A_write_stores_the_vector_its_width_was_checked_on(bool batch)
    {
        using var collection = new LodestarVectorStoreCollection<string, ShiftingVector>("vectors");
        var record = new ShiftingVector { Id = "a" };

        // The getter's second read is four wide, which the slot's copy would refuse after the slot was taken (#1801).
        await (batch ? collection.UpsertAsync([record]) : collection.UpsertAsync(record));

        Assert.Equal(1, record.Reads);
        VectorSearchResult<ShiftingVector> hit = Assert.Single(await collection.SearchAsync(Query, 1).ToListAsync());
        Assert.Equal(1d, hit.Score!.Value, 6);
    }

    [Fact]
    public void A_vector_without_a_getter_is_refused_at_construction()
    {
        ArgumentException error = Assert.Throws<ArgumentException>(
            () => new LodestarVectorStoreCollection<string, SetterOnlyVector>("vectors"));

        Assert.Contains("has no getter", error.Message, StringComparison.Ordinal);
    }

    [Fact]
    public void A_marked_base_property_a_new_member_hides_is_not_read()
    {
        using var collection = new LodestarVectorStoreCollection<string, UnmarkedHider>("hidden");

        Assert.False(collection.Schema.HasFullText);
        Assert.Contains("MarkedText.Text is marked, and a `new` member hides it", collection.Schema.KeywordRefusal, StringComparison.Ordinal);

        using var sameType = new LodestarVectorStoreCollection<string, SameTypeHider>("hidden");
        Assert.Contains("MarkedText.Text is marked, and a `new` member hides it", sameType.Schema.KeywordRefusal, StringComparison.Ordinal);
    }

    [Fact]
    public async Task A_selector_reaching_a_hidden_base_property_of_the_same_name_is_refused()
    {
        using var collection = new LodestarVectorStoreCollection<string, HidingNote>("notes");
        await collection.UpsertAsync(new HidingNote { Id = "a", Text = "the cat", Embedding = Query });

        await Assert.ThrowsAsync<ArgumentException>(async () => await collection.HybridSearchAsync(
            Query, ["cat"], 1, new HybridSearchOptions<HidingNote> { AdditionalProperty = n => ((PlainText)n).Text }).ToListAsync());
        Assert.Single(await collection.HybridSearchAsync(
            Query, ["cat"], 1, new HybridSearchOptions<HidingNote> { AdditionalProperty = n => n.Text }).ToListAsync());
    }

    [Fact]
    public async Task A_selector_naming_the_abstract_declaration_an_override_implements_is_honoured()
    {
        using var collection = new LodestarVectorStoreCollection<string, OverridingNote>("notes");
        await collection.UpsertAsync(new OverridingNote { Id = "a", Embedding = Query });

        Assert.Single(await collection.SearchAsync(
            Query, 1, new VectorSearchOptions<OverridingNote> { VectorProperty = n => ((AbstractNote)n).Embedding }).ToListAsync());
    }

    [Fact]
    public async Task A_selector_on_a_record_type_that_is_an_interface_is_honoured()
    {
        using var collection = new LodestarVectorStoreCollection<string, INote>("notes");
        await collection.UpsertAsync(new Note { Id = "a", Embedding = Query });

        Assert.Single(await collection.SearchAsync(
            Query, 1, new VectorSearchOptions<INote> { VectorProperty = n => n.Embedding }).ToListAsync());
    }

    [Fact]
    public async Task A_selector_converted_twice_is_honoured()
    {
        using LodestarVectorStoreCollection<string, Document> collection = await OneRecordAsync();

        Assert.Single(await collection.SearchAsync(
            Query, 1, new VectorSearchOptions<Document> { VectorProperty = d => (object?)(ReadOnlyMemory<float>?)d.Embedding }).ToListAsync());
    }

    // Generic code constrained to an interface: the compiler casts the parameter before reading the member.
    private static VectorSearchOptions<T> Selecting<T>()
        where T : class, IEmbedded => new() { VectorProperty = record => record.Embedding };

    [Fact]
    public async Task A_vector_selector_naming_the_vector_is_honoured_and_any_other_refused()
    {
        using LodestarVectorStoreCollection<string, Document> collection = await OneRecordAsync();

        Assert.Single(await collection.SearchAsync(
            Query, 1, new VectorSearchOptions<Document> { VectorProperty = d => d.Embedding }).ToListAsync());

        ArgumentException error = await Assert.ThrowsAsync<ArgumentException>(async () => await collection.SearchAsync(
            Query, 1, new VectorSearchOptions<Document> { VectorProperty = d => d.Text }).ToListAsync());
        Assert.Equal("options", error.ParamName);
        Assert.Contains("Embedding", error.Message, StringComparison.Ordinal);
    }

    [Fact]
    public async Task Hybrid_selectors_naming_the_vector_and_the_text_are_honoured_and_any_other_refused()
    {
        using LodestarVectorStoreCollection<string, Document> collection = await OneRecordAsync();

        Assert.Single(await collection.HybridSearchAsync(Query, ["cat"], 1, new HybridSearchOptions<Document>
        {
            VectorProperty = d => d.Embedding,
            AdditionalProperty = d => d.Text,
        }).ToListAsync());

        await Assert.ThrowsAsync<ArgumentException>(async () => await collection.HybridSearchAsync(
            Query, ["cat"], 1, new HybridSearchOptions<Document> { AdditionalProperty = d => d.Id }).ToListAsync());
        await Assert.ThrowsAsync<ArgumentException>(async () => await collection.HybridSearchAsync(
            Query, ["cat"], 1, new HybridSearchOptions<Document> { VectorProperty = d => d.Text }).ToListAsync());
    }

    [Fact]
    public void A_key_property_of_another_type_than_the_collection_key_is_refused_at_construction()
    {
        ArgumentException error = Assert.Throws<ArgumentException>(
            () => new LodestarVectorStoreCollection<int, StringKeyed>("keys"));

        Assert.Contains("StringKeyed.Id is String", error.Message, StringComparison.Ordinal);
        Assert.Contains("Int32", error.Message, StringComparison.Ordinal);
    }

    [Fact]
    public void A_key_property_of_the_collection_key_type_or_its_nullable_is_accepted()
    {
        using var exact = new LodestarVectorStoreCollection<int, VectorOnly>("exact");
        using var widened = new LodestarVectorStoreCollection<object, StringKeyed>("widened");

        Assert.Equal(2, exact.Schema.Dimension);
        Assert.Equal(2, widened.Schema.Dimension);
    }

    [Fact]
    public void A_full_text_property_that_is_not_a_string_is_refused_at_construction()
    {
        ArgumentException error = Assert.Throws<ArgumentException>(
            () => new LodestarVectorStoreCollection<string, NumberMarkedFullText>("numbers"));

        Assert.Contains("NumberMarkedFullText.Count is Int32", error.Message, StringComparison.Ordinal);
    }

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
    public void A_definition_naming_a_plain_data_property_the_type_lacks_is_refused_too()
    {
        ArgumentException error = Assert.Throws<ArgumentException>(() => new LodestarVectorStoreCollection<string, Document>(
            "documents", definition: DocumentDefinition(new VectorStoreDataProperty("Tag", typeof(string)))));

        Assert.Equal("definition", error.ParamName);
        Assert.Contains("Tag", error.Message, StringComparison.Ordinal);
    }

    [Fact]
    public void A_key_or_a_sole_text_without_a_getter_is_refused_at_construction()
    {
        Assert.Contains("has no getter", Assert.Throws<ArgumentException>(
            () => new LodestarVectorStoreCollection<string, SetterOnlyKey>("keys")).Message, StringComparison.Ordinal);
        Assert.Contains("has no getter", Assert.Throws<ArgumentException>(
            () => new LodestarVectorStoreCollection<string, SetterOnlyText>("texts")).Message, StringComparison.Ordinal);
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

        RecordSchema<string, Document>.Admitted admitted = schema.Admit(record, "record");
        Assert.Equal("a", admitted.Key);
        Assert.Equal("the cat sat", admitted.Text);
        Assert.Equal(3, schema.Dimension);
        Assert.True(schema.HasFullText);
        Assert.Equal(1f, admitted.Vector.Span[0]);
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
