using System.Runtime.CompilerServices;
using Microsoft.Extensions.VectorData;

namespace Lodestar.Extensions.VectorData;

/// <summary>An in-process collection: the records are the state, the indexes are caches.</summary>
/// <typeparam name="TKey">The key type the record's key property carries.</typeparam>
/// <typeparam name="TRecord">The record type, whose schema is read once on construction.</typeparam>
/// <remarks>
/// A write updates the dictionary and marks the caches stale; the first read after it
/// rebuilds them, so a batch of writes costs one rebuild rather than one per record. An
/// updated record's superseded vector is gone rather than masked — the case an
/// append-with-tombstones store gets wrong. Not thread-safe for concurrent writes: an
/// in-memory collection built for one process is not a database.
/// </remarks>
// Every provider of this abstraction names its collection type after the base class it
// extends; CA1711 flags the suffix, but matching Microsoft.Extensions.VectorData is the point.
#pragma warning disable CA1711
public sealed class LodestarVectorStoreCollection<TKey, TRecord> : VectorStoreCollection<TKey, TRecord>
    where TKey : notnull
    where TRecord : class
{
    private readonly Dictionary<TKey, TRecord> _records = [];
    private readonly RecordSchema<TKey, TRecord> _schema;
    private readonly LodestarVectorStoreOptions _options;
    private DerivedIndexes<TKey>? _indexes;
    private bool _exists;

    /// <summary>Creates a collection over a record type the schema is read from.</summary>
    /// <param name="name">The collection's name, which <see cref="Name"/> reports.</param>
    /// <param name="options">How the keyword half is built and fused; <see langword="null"/> takes the defaults.</param>
    /// <param name="definition">An explicit schema, or <see langword="null"/> to read <typeparamref name="TRecord"/>'s attributes.</param>
    /// <exception cref="ArgumentNullException"><paramref name="name"/> is null.</exception>
    /// <exception cref="ArgumentException"><typeparamref name="TRecord"/> declares no key or no vector property.</exception>
    public LodestarVectorStoreCollection(
        string name,
        LodestarVectorStoreOptions? options = null,
        VectorStoreCollectionDefinition? definition = null)
    {
        Guard.NotNull(name);
        Name = name;
        _options = options ?? new LodestarVectorStoreOptions();
        _schema = RecordSchema<TKey, TRecord>.Create(definition);
    }

    /// <inheritdoc />
    public override string Name { get; }

    /// <summary>How many times the caches have been rebuilt, which the suite asserts on.</summary>
    internal int RebuildCount { get; private set; }

    /// <summary>The caches, rebuilt first when a write has happened since the last read.</summary>
    internal DerivedIndexes<TKey> Current()
    {
        if (_indexes is null)
        {
            _indexes = DerivedIndexes<TKey>.Build(_records.Values, _schema, _options);
            RebuildCount++;
        }

        return _indexes;
    }

    /// <summary>The schema this collection reads its records through.</summary>
    internal RecordSchema<TKey, TRecord> Schema => _schema;

    /// <summary>The options the keyword half and the fusion were configured with.</summary>
    internal LodestarVectorStoreOptions Options => _options;

    /// <inheritdoc />
    public override Task<bool> CollectionExistsAsync(CancellationToken cancellationToken = default) =>
        Task.FromResult(_exists);

    /// <inheritdoc />
    public override Task EnsureCollectionExistsAsync(CancellationToken cancellationToken = default)
    {
        _exists = true;
        return Task.CompletedTask;
    }

    /// <inheritdoc />
    public override Task EnsureCollectionDeletedAsync(CancellationToken cancellationToken = default)
    {
        _exists = false;
        _records.Clear();
        Invalidate();
        return Task.CompletedTask;
    }

    /// <inheritdoc />
    public override Task UpsertAsync(TRecord record, CancellationToken cancellationToken = default)
    {
        Guard.NotNull(record);
        _records[_schema.KeyOf(record)] = record;
        _exists = true;
        Invalidate();
        return Task.CompletedTask;
    }

    /// <inheritdoc />
    public override Task UpsertAsync(IEnumerable<TRecord> records, CancellationToken cancellationToken = default)
    {
        Guard.NotNull(records);
        foreach (TRecord record in records)
        {
            _records[_schema.KeyOf(record)] = record;
        }

        _exists = true;
        Invalidate();
        return Task.CompletedTask;
    }

    /// <inheritdoc />
    public override Task DeleteAsync(TKey key, CancellationToken cancellationToken = default)
    {
        if (_records.Remove(key))
        {
            Invalidate();
        }

        return Task.CompletedTask;
    }

    /// <inheritdoc />
    public override Task DeleteAsync(IEnumerable<TKey> keys, CancellationToken cancellationToken = default)
    {
        Guard.NotNull(keys);
        bool removed = false;
        foreach (TKey key in keys)
        {
            removed |= _records.Remove(key);
        }

        if (removed)
        {
            Invalidate();
        }

        return Task.CompletedTask;
    }

    /// <inheritdoc />
    public override Task<TRecord?> GetAsync(
        TKey key, RecordRetrievalOptions? options = null, CancellationToken cancellationToken = default) =>
        Task.FromResult(_records.TryGetValue(key, out TRecord? record) ? record : null);

    /// <inheritdoc />
    public override async IAsyncEnumerable<TRecord> GetAsync(
        IEnumerable<TKey> keys,
        RecordRetrievalOptions? options = null,
        [EnumeratorCancellation] CancellationToken cancellationToken = default)
    {
        Guard.NotNull(keys);
        foreach (TKey key in keys)
        {
            cancellationToken.ThrowIfCancellationRequested();
            if (_records.TryGetValue(key, out TRecord? record))
            {
                yield return record;
            }
        }

        await Task.CompletedTask.ConfigureAwait(false);
    }

    /// <inheritdoc />
    public override IAsyncEnumerable<TRecord> GetAsync(
        System.Linq.Expressions.Expression<Func<TRecord, bool>> filter,
        int top,
        FilteredRecordRetrievalOptions<TRecord>? options = null,
        CancellationToken cancellationToken = default) =>
        throw new NotSupportedException("Filtered retrieval arrives with the filter, in the next task.");

    /// <inheritdoc />
    public override object? GetService(Type serviceType, object? serviceKey = null) =>
        serviceType == typeof(VectorStoreCollectionMetadata) && serviceKey is null
            ? new VectorStoreCollectionMetadata { VectorStoreSystemName = "lodestar", CollectionName = Name }
            : null;

    /// <inheritdoc />
    public override IAsyncEnumerable<VectorSearchResult<TRecord>> SearchAsync<TInput>(
        TInput searchValue,
        int top,
        VectorSearchOptions<TRecord>? options = null,
        CancellationToken cancellationToken = default) =>
        throw new NotSupportedException("Vector search arrives in the next task.");

    /// <summary>Releases resources — none, here: the base class declares the pattern and a consumer's <c>using</c> has to reach something.</summary>
    /// <param name="disposing"><see langword="true"/> when called from <see cref="IDisposable.Dispose"/> rather than a finalizer.</param>
    protected override void Dispose(bool disposing) => base.Dispose(disposing);

    private void Invalidate() => _indexes = null;
}
#pragma warning restore CA1711
