using System.Linq.Expressions;
using System.Runtime.CompilerServices;
using Lodestar.Embeddings.Search;
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
    /// <exception cref="NotSupportedException"><paramref name="options"/> sets <c>OrderBy</c>.</exception>
    /// <remarks>
    /// <c>Skip</c> counts over the records the filter admits, the same way
    /// <see cref="SearchAsync{TInput}"/> counts it. <c>OrderBy</c> throws rather than being
    /// silently ignored: a dictionary-backed collection has no order of its own, and ordering
    /// by an arbitrary property expression is not implemented here.
    /// </remarks>
    public override async IAsyncEnumerable<TRecord> GetAsync(
        Expression<Func<TRecord, bool>> filter,
        int top,
        FilteredRecordRetrievalOptions<TRecord>? options = null,
        [EnumeratorCancellation] CancellationToken cancellationToken = default)
    {
        Guard.NotNull(filter);
        Guard.NotLessThan(top, 1);
        if (options?.OrderBy is not null)
        {
            throw new NotSupportedException(
                "OrderBy is not supported: a dictionary-backed collection has no order of its "
                + "own, and ordering by an arbitrary property expression is not implemented here.");
        }

        Func<TRecord, bool> admits = filter.Compile();
        int skip = options?.Skip ?? 0;
        int skipped = 0;
        int taken = 0;

        foreach (TRecord record in _records.Values)
        {
            cancellationToken.ThrowIfCancellationRequested();
            if (!admits(record))
            {
                continue;
            }

            if (skipped < skip)
            {
                skipped++;
                continue;
            }

            yield return record;
            if (++taken == top)
            {
                break;
            }
        }

        await Task.CompletedTask.ConfigureAwait(false);
    }

    /// <inheritdoc />
    public override object? GetService(Type serviceType, object? serviceKey = null) =>
        serviceType == typeof(VectorStoreCollectionMetadata) && serviceKey is null
            ? new VectorStoreCollectionMetadata { VectorStoreSystemName = "lodestar", CollectionName = Name }
            : null;

    /// <inheritdoc />
    /// <exception cref="NotSupportedException"><paramref name="searchValue"/> is not a vector, and this package generates none.</exception>
    /// <remarks>
    /// With a filter, every record is scored and the filter runs before the cut, so
    /// <paramref name="top"/> means <paramref name="top"/>: a caller asking for five matching
    /// records gets five whenever five match. Post-filtering a top-k would return fewer
    /// without saying why.
    /// </remarks>
    public override async IAsyncEnumerable<VectorSearchResult<TRecord>> SearchAsync<TInput>(
        TInput searchValue,
        int top,
        VectorSearchOptions<TRecord>? options = null,
        [EnumeratorCancellation] CancellationToken cancellationToken = default)
    {
        Guard.NotLessThan(top, 1);
        ReadOnlyMemory<float> query = AsVector(searchValue);
        VectorSearchOptions<TRecord> settings = options ?? new VectorSearchOptions<TRecord>();
        Func<TRecord, bool>? admits = RecordFilter.Compile(settings.Filter);
        DerivedIndexes<TKey> indexes = Current();

        // With a filter the whole collection is scored, because the records the filter keeps
        // are not known before it runs and a short list would silently return too few.
        int wanted = admits is null ? top + settings.Skip : indexes.Vectors.Count;
        int skipped = 0;
        int taken = 0;

        foreach (SearchResult hit in Scored(indexes, query, wanted))
        {
            cancellationToken.ThrowIfCancellationRequested();
            TRecord record = _records[indexes.Keys[hit.Index]];
            if (admits is not null && !admits(record))
            {
                continue;
            }

            if (settings.ScoreThreshold is { } threshold && hit.Score < threshold)
            {
                continue;
            }

            if (skipped < settings.Skip)
            {
                skipped++;
                continue;
            }

            yield return new VectorSearchResult<TRecord>(record, hit.Score);
            if (++taken == top)
            {
                break;
            }
        }

        await Task.CompletedTask.ConfigureAwait(false);
    }

    private static IReadOnlyList<SearchResult> Scored(
        DerivedIndexes<TKey> indexes, ReadOnlyMemory<float> query, int wanted) =>
        indexes.Vectors.Count == 0
            ? []
            : indexes.Vectors.Search(query.Span, Math.Min(wanted, indexes.Vectors.Count));

    private static ReadOnlyMemory<float> AsVector<TInput>(TInput searchValue)
        where TInput : notnull => searchValue switch
        {
            ReadOnlyMemory<float> memory => memory,
            float[] array => array,
            _ => throw new NotSupportedException(
                $"{typeof(TInput).Name} is not a vector, and this package generates none: supply a "
                + "ReadOnlyMemory<float> or a float[], or embed the text with Lodestar.Extensions.AI first."),
        };

    /// <summary>Releases resources — none, here: the base class declares the pattern and a consumer's <c>using</c> has to reach something.</summary>
    /// <param name="disposing"><see langword="true"/> when called from <see cref="IDisposable.Dispose"/> rather than a finalizer.</param>
    protected override void Dispose(bool disposing) => base.Dispose(disposing);

    private void Invalidate() => _indexes = null;
}
#pragma warning restore CA1711
