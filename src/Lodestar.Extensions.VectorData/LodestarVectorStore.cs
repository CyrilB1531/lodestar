using System.Runtime.CompilerServices;
using Microsoft.Extensions.VectorData;

namespace Lodestar.Extensions.VectorData;

/// <summary>An in-process vector store: named collections, held for the store's lifetime.</summary>
/// <remarks>
/// Nothing here reaches a network or a file. A collection exists once something has ensured it
/// or written to it, which is what <see cref="ListCollectionNamesAsync"/> reports — asking for
/// a collection by name does not create it, the way asking a database for a table does not.
/// </remarks>
public sealed class LodestarVectorStore : VectorStore
{
    private readonly Dictionary<string, object> _collections = [];
    private readonly LodestarVectorStoreOptions _options;

    /// <summary>Creates a store whose collections all take the same options.</summary>
    /// <param name="options">How each collection builds its keyword half; <see langword="null"/> takes the defaults.</param>
    public LodestarVectorStore(LodestarVectorStoreOptions? options = null) =>
        _options = options ?? new LodestarVectorStoreOptions();

    /// <inheritdoc />
    /// <exception cref="ArgumentException"><paramref name="name"/> is already held under a different key or record type.</exception>
    public override VectorStoreCollection<TKey, TRecord> GetCollection<TKey, TRecord>(
        string name, VectorStoreCollectionDefinition? definition = null)
    {
        Guard.NotNull(name);
        if (_collections.TryGetValue(name, out object? existing))
        {
            return existing as LodestarVectorStoreCollection<TKey, TRecord>
                ?? throw new ArgumentException(
                    $"The collection {name} is already held over a different key or record type; "
                    + "one name is one schema.", nameof(name));
        }

        var created = new LodestarVectorStoreCollection<TKey, TRecord>(name, _options, definition);
        _collections[name] = created;
        return created;
    }

    /// <inheritdoc />
    /// <exception cref="NotSupportedException">Always: this store has no dynamic path.</exception>
    /// <remarks>
    /// A dynamic record is a dictionary, and the filter this store compiles is an expression over
    /// a record's properties — there are none to compile against. Refusing says so; answering
    /// with a collection that then refused every filter would not.
    /// </remarks>
    public override VectorStoreCollection<object, Dictionary<string, object?>> GetDynamicCollection(
        string name, VectorStoreCollectionDefinition definition) =>
        throw new NotSupportedException(
            "This store serves typed records only. Its filters compile an expression over a "
            + "record's properties, which a dictionary does not have.");

    /// <inheritdoc />
    public override async IAsyncEnumerable<string> ListCollectionNamesAsync(
        [EnumeratorCancellation] CancellationToken cancellationToken = default)
    {
        foreach (KeyValuePair<string, object> entry in _collections)
        {
            cancellationToken.ThrowIfCancellationRequested();
            if (entry.Value is IExistingCollection { Exists: true })
            {
                yield return entry.Key;
            }
        }

        await Task.CompletedTask.ConfigureAwait(false);
    }

    /// <inheritdoc />
    public override Task<bool> CollectionExistsAsync(string name, CancellationToken cancellationToken = default) =>
        Task.FromResult(_collections.TryGetValue(name, out object? held)
            && held is IExistingCollection { Exists: true });

    /// <inheritdoc />
    public override Task EnsureCollectionDeletedAsync(string name, CancellationToken cancellationToken = default)
    {
        if (_collections.TryGetValue(name, out object? held) && held is IExistingCollection collection)
        {
            return collection.EnsureDeletedAsync(cancellationToken);
        }

        return Task.CompletedTask;
    }

    /// <inheritdoc />
    public override object? GetService(Type serviceType, object? serviceKey = null) =>
        serviceType == typeof(VectorStoreMetadata) && serviceKey is null
            ? new VectorStoreMetadata { VectorStoreSystemName = "lodestar" }
            : null;

    /// <summary>Disposes every collection the store has ever handed out.</summary>
    /// <param name="disposing"><see langword="true"/> when called from <see cref="IDisposable.Dispose"/> rather than a finalizer.</param>
    protected override void Dispose(bool disposing)
    {
        if (disposing)
        {
            foreach (object collection in _collections.Values)
            {
                (collection as IDisposable)?.Dispose();
            }

            _collections.Clear();
        }

        base.Dispose(disposing);
    }
}

/// <summary>What the store needs of a collection without knowing its type arguments.</summary>
/// <remarks>
/// The store holds collections as <see cref="object"/>, since each closes the generic
/// differently. This is the narrow surface that lets it answer about existence anyway.
/// </remarks>
internal interface IExistingCollection
{
    /// <summary>Whether the collection has been created or written to.</summary>
    bool Exists { get; }

    /// <summary>Drops the collection and its records.</summary>
    Task EnsureDeletedAsync(CancellationToken cancellationToken);
}
