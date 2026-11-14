using System.Runtime.CompilerServices;
using Microsoft.Extensions.VectorData;

namespace Lodestar.Extensions.VectorData;

/// <summary>An in-process vector store: named collections, held for the store's lifetime.</summary>
/// <remarks>
/// Nothing here reaches a network or a file. A collection exists once something has ensured it
/// or written to it, which is what <see cref="ListCollectionNamesAsync"/> reports — asking for
/// a collection by name does not create it, the way asking a database for a table does not.
/// The names it holds are read and written under a lock, so the store can be shared by concurrent
/// callers, a dependency-injection singleton's (#1266); a collection's own state is the collection's.
/// </remarks>
public sealed class LodestarVectorStore : VectorStore
{
    private readonly object _gate = new();
    private readonly Dictionary<string, object> _collections = [];
    private readonly LodestarVectorStoreOptions _options;

    // Read and written under the lock, as the names are.
    private bool _disposed;

    /// <summary>Creates a store whose collections all take the same options.</summary>
    /// <param name="options">How each collection builds its keyword half; <see langword="null"/> takes the defaults.</param>
    public LodestarVectorStore(LodestarVectorStoreOptions? options = null) =>
        _options = options ?? new LodestarVectorStoreOptions();

    /// <inheritdoc />
    /// <exception cref="ArgumentNullException"><paramref name="name"/> is null.</exception>
    /// <exception cref="ObjectDisposedException">The store has been disposed.</exception>
    /// <exception cref="ArgumentException"><paramref name="name"/> is already held under a different key or record type and not deleted since, or the schema is unusable.</exception>
    /// <exception cref="NotSupportedException">The schema has more than one vector property, or the vector declares a distance function other than cosine similarity.</exception>
    public override VectorStoreCollection<TKey, TRecord> GetCollection<TKey, TRecord>(
        string name, VectorStoreCollectionDefinition? definition = null)
    {
        Guard.NotNull(name);

        // A read that may insert, whole under the lock, the schema's reflection included: callers racing
        // for one new name get one collection, and each name pays that reflection once.
        lock (_gate)
        {
            Guard.NotDisposed(_disposed, this);
            if (_collections.TryGetValue(name, out object? existing)
                && existing is not IExistingCollection { Deleted: true })
            {
                return existing as LodestarVectorStoreCollection<TKey, TRecord>
                    ?? throw new ArgumentException(
                        $"The collection {name} is already held over a different key or record type; "
                        + "one name is one schema.", nameof(name));
            }

            // A deleted name is free again. The same schema still gets the object it held, so a
            // handle kept across the deletion stays the collection that name answers with.
            if (existing is LodestarVectorStoreCollection<TKey, TRecord> kept)
            {
                return kept;
            }

            var created = new LodestarVectorStoreCollection<TKey, TRecord>(name, _options, definition);
            _collections[name] = created;
            return created;
        }
    }

    /// <summary>What a Task-returning lookup answers for the collection under <paramref name="name"/>.</summary>
    /// <remarks>A disposed store is reported through the task, as a cancelled token is.</remarks>
    private TTask Lookup<TTask>(string name, Func<object?, TTask> answer, Func<Exception, TTask> fault)
        where TTask : Task
    {
        object? held;
        try
        {
            held = Held(name);
        }
        catch (ObjectDisposedException disposed)
        {
            return fault(disposed);
        }

        return answer(held);
    }

    /// <summary>The collection held under <paramref name="name"/>, if any, read under the lock.</summary>
    /// <exception cref="ObjectDisposedException">The store has been disposed.</exception>
    private object? Held(string name)
    {
        lock (_gate)
        {
            Guard.NotDisposed(_disposed, this);
            return _collections.TryGetValue(name, out object? held) ? held : null;
        }
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
    /// <exception cref="ObjectDisposedException">The store has been disposed.</exception>
    /// <exception cref="OperationCanceledException"><paramref name="cancellationToken"/> is cancelled while the names are read.</exception>
    public override async IAsyncEnumerable<string> ListCollectionNamesAsync(
        [EnumeratorCancellation] CancellationToken cancellationToken = default)
    {
        // A cancelled token is answered before a disposed store, as the other lookups answer it.
        cancellationToken.ThrowIfCancellationRequested();

        // Materialised under the lock before the first yield: a collection requested while the
        // caller enumerates would otherwise invalidate the dictionary enumerator mid-walk.
        List<string> names;
        lock (_gate)
        {
            Guard.NotDisposed(_disposed, this);
            names = [.. _collections
                .Where(entry => entry.Value is IExistingCollection { Exists: true })
                .Select(entry => entry.Key)];
        }

        foreach (string name in names)
        {
            cancellationToken.ThrowIfCancellationRequested();
            yield return name;
        }

        await Task.CompletedTask.ConfigureAwait(false);
    }

    /// <inheritdoc />
    /// <exception cref="ArgumentNullException"><paramref name="name"/> is null.</exception>
    /// <exception cref="ObjectDisposedException">The store has been disposed.</exception>
    public override Task<bool> CollectionExistsAsync(string name, CancellationToken cancellationToken = default)
    {
        // Named, as GetCollection names it, rather than surfacing as Dictionary's "key" (#1353).
        Guard.NotNull(name);
        if (cancellationToken.IsCancellationRequested)
        {
            return Task.FromCanceled<bool>(cancellationToken);
        }

        return Lookup(name, held => Task.FromResult(held is IExistingCollection { Exists: true }), Task.FromException<bool>);
    }

    /// <inheritdoc />
    /// <exception cref="ArgumentNullException"><paramref name="name"/> is null.</exception>
    /// <exception cref="ObjectDisposedException">The store has been disposed.</exception>
    public override Task EnsureCollectionDeletedAsync(string name, CancellationToken cancellationToken = default)
    {
        Guard.NotNull(name);
        if (cancellationToken.IsCancellationRequested)
        {
            return Task.FromCanceled(cancellationToken);
        }

        // Deleting empties the collection, a write to it, so it runs outside the store's lock.
        return Lookup(
            name,
            held => held is IExistingCollection collection ? collection.EnsureDeletedAsync(cancellationToken) : Task.CompletedTask,
            Task.FromException);
    }

    /// <inheritdoc />
    /// <exception cref="ArgumentNullException"><paramref name="serviceType"/> is null.</exception>
    public override object? GetService(Type serviceType, object? serviceKey = null)
    {
        Guard.NotNull(serviceType);
        return serviceType == typeof(VectorStoreMetadata) && serviceKey is null ? Metadata() : null;
    }

    // Its own method so the guard above runs even where the init-only setter below fails to bind:
    // the netstandard2.0 build loaded beside a newer target's abstractions throws MissingMethodException.
    private static VectorStoreMetadata Metadata() => new() { VectorStoreSystemName = "lodestar" };

    /// <summary>Disposes every collection the store holds; a collection whose name went to another schema is no longer held.</summary>
    /// <param name="disposing"><see langword="true"/> when called from <see cref="IDisposable.Dispose"/> rather than a finalizer.</param>
    protected override void Dispose(bool disposing)
    {
        if (disposing)
        {
            object[] held;
            lock (_gate)
            {
                held = [.. _collections.Values];
                _collections.Clear();
                _disposed = true;
            }

            foreach (object collection in held)
            {
                (collection as IDisposable)?.Dispose();
            }
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

    /// <summary>Whether it was deleted and nothing has created or written to it since, which frees its name.</summary>
    bool Deleted { get; }

    /// <summary>Drops the collection and its records.</summary>
    Task EnsureDeletedAsync(CancellationToken cancellationToken);
}
