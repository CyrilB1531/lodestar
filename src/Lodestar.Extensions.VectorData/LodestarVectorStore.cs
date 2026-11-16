using System.Runtime.CompilerServices;
using Microsoft.Extensions.VectorData;

namespace Lodestar.Extensions.VectorData;

/// <summary>An in-process vector store: named collections, held for the store's lifetime.</summary>
/// <remarks>
/// Nothing here reaches a network or a file. A collection exists once something has ensured it
/// or written to it, which is what <see cref="ListCollectionNamesAsync"/> reports — asking for
/// a collection by name does not create it, the way asking a database for a table does not.
/// The names it holds are read and written under a lock, so the store can be shared by concurrent
/// callers, a dependency-injection singleton's (#1266); a collection's own state is the collection's,
/// which the store changes only by asking it to delete itself or by disposing it.
/// </remarks>
public sealed class LodestarVectorStore : VectorStore
{
    private readonly object _gate = new();
    private readonly Dictionary<string, IExistingCollection> _collections = [];
    private readonly LodestarVectorStoreOptions _options;

    // Read and written under the lock, as the names are.
    private bool _disposed;

    /// <summary>Creates a store whose collections all take the same options.</summary>
    /// <param name="options">How each collection builds its keyword half; <see langword="null"/> takes the defaults.</param>
    public LodestarVectorStore(LodestarVectorStoreOptions? options = null) =>
        _options = options ?? new LodestarVectorStoreOptions();

    /// <inheritdoc />
    /// <remarks>
    /// To successfully request a collection, either <typeparamref name="TRecord"/> must be annotated with attributes that
    /// define the schema of the record type, or <paramref name="definition"/> must be provided, as the base class says. A
    /// different key or record type that gets a new collection under a deleted name disposes the collection the name held,
    /// so a handle kept to it is to be dropped.
    /// </remarks>
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
        IExistingCollection otherwise;
        lock (_gate)
        {
            Guard.NotDisposed(_disposed, this);
            _collections.TryGetValue(name, out IExistingCollection? existing);

            // The same schema gets the object it holds, deleted or not, so a handle kept across a
            // deletion stays the collection that name answers with.
            if (existing is LodestarVectorStoreCollection<TKey, TRecord> same)
            {
                return same;
            }

            // A deleted name is free again, and the collection it held leaves the store, disposed as the store's
            // Dispose would have; its Dispose releases nothing and runs no caller code, so the lock waits on nothing.
            if (existing is null or { Deleted: true })
            {
                var created = new LodestarVectorStoreCollection<TKey, TRecord>(name, _options, definition);
                _collections[name] = created;
                existing?.Dispose();
                return created;
            }

            otherwise = existing;
        }

        // Decided under the lock and named once it is released, from key and record types fixed for the held collection's
        // life, so the refusal reports the store as it stood when decided (#1814); no other caller waits on the naming.
        throw HeldOtherwise(name, otherwise, typeof(TKey), typeof(TRecord));
    }

    /// <summary>What a Task-returning lookup answers for the collection under <paramref name="name"/>.</summary>
    /// <remarks>A disposed store is reported through the task, as a cancelled token is.</remarks>
    private TTask Lookup<TTask>(string name, Func<IExistingCollection?, TTask> answer, Func<Exception, TTask> fault)
        where TTask : Task
    {
        IExistingCollection? held;
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
    private IExistingCollection? Held(string name)
    {
        lock (_gate)
        {
            Guard.NotDisposed(_disposed, this);
            return _collections.TryGetValue(name, out IExistingCollection? held) ? held : null;
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

    // Key and record named one by one, so the message shows which of the two differs.
    private static ArgumentException HeldOtherwise(string name, IExistingCollection held, Type key, Type record)
    {
        RefusalName[] names = TypeNames.Names(held.KeyType, held.RecordType, key, record);
        return new ArgumentException(
            $"The collection {Shown.Value(name)} is already held over {names[0]} keys and {names[1]} records, and this asks for "
            + $"{names[2]} keys and {names[3]} records; one name is one schema.",
            nameof(name));
    }

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
                .Where(entry => entry.Value.Exists)
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

        return Lookup(name, held => Task.FromResult(held is { Exists: true }), Task.FromException<bool>);
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
            held => held?.EnsureDeletedAsync(cancellationToken) ?? Task.CompletedTask,
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

    /// <summary>Disposes every collection the store holds; one whose deleted name went to another schema was disposed then.</summary>
    /// <param name="disposing"><see langword="true"/> when called from <see cref="IDisposable.Dispose"/> rather than a finalizer.</param>
    protected override void Dispose(bool disposing)
    {
        if (disposing)
        {
            IExistingCollection[] held;
            lock (_gate)
            {
                held = [.. _collections.Values];
                _collections.Clear();
                _disposed = true;
            }

            foreach (IExistingCollection collection in held)
            {
                collection.Dispose();
            }
        }

        base.Dispose(disposing);
    }
}

/// <summary>What the store needs of a collection without knowing its type arguments.</summary>
/// <remarks>
/// Each collection closes the generic differently, so the store holds them through this narrow
/// surface: it answers about existence, deletes and disposes them, and names the key and record
/// types a name is held over when another schema asks for it.
/// </remarks>
internal interface IExistingCollection : IDisposable
{
    /// <summary>The key type the collection is held over.</summary>
    Type KeyType { get; }

    /// <summary>The record type the collection is held over.</summary>
    Type RecordType { get; }

    /// <summary>Whether the collection has been created or written to.</summary>
    bool Exists { get; }

    /// <summary>Whether it was deleted and nothing has created or written to it since, which frees its name.</summary>
    bool Deleted { get; }

    /// <summary>Drops the collection and its records.</summary>
    Task EnsureDeletedAsync(CancellationToken cancellationToken);
}
