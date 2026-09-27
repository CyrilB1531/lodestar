using System.Linq.Expressions;
using System.Runtime.CompilerServices;
using Lodestar.Embeddings.Search;
using Lodestar.Text.Search;
using Microsoft.Extensions.VectorData;

namespace Lodestar.Extensions.VectorData;

/// <summary>An in-process collection: the records, and the two indexes kept beside them.</summary>
/// <typeparam name="TKey">The key type the record's key property carries.</typeparam>
/// <typeparam name="TRecord">The record type, whose schema is read once on construction.</typeparam>
/// <remarks>
/// A write costs its own record, never the collection's (#1214): its vector is normalized into
/// its slot, and its text staged for the keyword half, which tokenizes what was staged when a
/// hybrid search next needs it. An updated record's superseded vector is overwritten rather than
/// masked. Searches may run concurrently with each other; a write may not run beside anything:
/// an in-memory collection built for one process is not a database.
/// </remarks>
// Every provider of this abstraction names its collection type after the base class it
// extends; CA1711 flags the suffix, but matching Microsoft.Extensions.VectorData is the point.
#pragma warning disable CA1711
public sealed class LodestarVectorStoreCollection<TKey, TRecord>
    : VectorStoreCollection<TKey, TRecord>, IKeywordHybridSearchable<TRecord>, IExistingCollection
    where TKey : notnull
    where TRecord : class
{
    private readonly HeldRecords<TKey, TRecord> _held;
    private readonly KeywordIndex? _keywords;
    private readonly RecordSchema<TKey, TRecord> _schema;
    private readonly LodestarVectorStoreOptions _options;
    private bool _exists;
    private bool _deleted;

    /// <summary>Creates a collection over a record type the schema is read from.</summary>
    /// <param name="name">The collection's name, which <see cref="Name"/> reports.</param>
    /// <param name="options">How the keyword half is built and fused; <see langword="null"/> takes the defaults.</param>
    /// <param name="definition">An explicit schema, or <see langword="null"/> to read <typeparamref name="TRecord"/>'s attributes.</param>
    /// <exception cref="ArgumentNullException"><paramref name="name"/> is null.</exception>
    /// <exception cref="ArgumentException">The schema has no key property, no vector property, a vector that is not <c>ReadOnlyMemory&lt;float&gt;</c>, or a definition naming a property <typeparamref name="TRecord"/> lacks.</exception>
    /// <exception cref="NotSupportedException">The vector declares a distance function other than <c>DistanceFunction.CosineSimilarity</c>.</exception>
    public LodestarVectorStoreCollection(
        string name,
        LodestarVectorStoreOptions? options = null,
        VectorStoreCollectionDefinition? definition = null)
    {
        Guard.NotNull(name);
        Name = name;
        _options = options ?? new LodestarVectorStoreOptions();
        _schema = RecordSchema<TKey, TRecord>.Create(definition);
        _held = new HeldRecords<TKey, TRecord>(_schema.Dimension);
        _keywords = _schema.HasFullText ? new KeywordIndex(_options) : null;
    }

    /// <inheritdoc />
    public override string Name { get; }

    /// <summary>How many texts the keyword half has tokenized, which the suite asserts a write does not multiply.</summary>
    internal int TokenizedTexts => _keywords?.TokenizedTexts ?? 0;

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
        _deleted = false;
        return Task.CompletedTask;
    }

    /// <inheritdoc />
    public override Task EnsureCollectionDeletedAsync(CancellationToken cancellationToken = default)
    {
        _exists = false;
        _deleted = true;
        _held.Clear();
        _keywords?.Clear();
        return Task.CompletedTask;
    }

    /// <inheritdoc />
    /// <exception cref="ArgumentNullException"><paramref name="record"/> is null.</exception>
    /// <exception cref="ArgumentException">The record's key is null, or its vector is not the collection's width.</exception>
    public override Task UpsertAsync(TRecord record, CancellationToken cancellationToken = default)
    {
        TKey key = _schema.Admit(record, nameof(record));
        Write(key, record);
        _exists = true;
        _deleted = false;
        return Task.CompletedTask;
    }

    /// <inheritdoc />
    /// <exception cref="ArgumentNullException"><paramref name="records"/> is null, or holds a null record.</exception>
    /// <exception cref="ArgumentException">A record's key is null, or its vector is not the collection's width.</exception>
    /// <remarks>Every record is checked before any is written, so a refused batch writes nothing.</remarks>
    public override Task UpsertAsync(IEnumerable<TRecord> records, CancellationToken cancellationToken = default)
    {
        Guard.NotNull(records);
        var admitted = new List<KeyValuePair<TKey, TRecord>>();
        foreach (TRecord record in records)
        {
            admitted.Add(new KeyValuePair<TKey, TRecord>(_schema.Admit(record, nameof(records)), record));
        }

        foreach (KeyValuePair<TKey, TRecord> entry in admitted)
        {
            Write(entry.Key, entry.Value);
        }

        _exists = true;
        _deleted = false;
        return Task.CompletedTask;
    }

    /// <inheritdoc />
    /// <exception cref="ArgumentNullException"><paramref name="key"/> is null.</exception>
    public override Task DeleteAsync(TKey key, CancellationToken cancellationToken = default)
    {
        Erase(key);
        return Task.CompletedTask;
    }

    /// <inheritdoc />
    /// <exception cref="ArgumentNullException"><paramref name="keys"/> is null, or holds a null key.</exception>
    /// <remarks>Every key is read and checked before any record is removed, so a refused batch removes nothing.</remarks>
    public override Task DeleteAsync(IEnumerable<TKey> keys, CancellationToken cancellationToken = default)
    {
        Guard.NotNull(keys);
        TKey[] batch = [.. keys];
        if (Array.Exists(batch, key => key is null))
        {
            throw new ArgumentNullException(nameof(keys), "A null key cannot address a record.");
        }

        foreach (TKey key in batch)
        {
            Erase(key);
        }

        return Task.CompletedTask;
    }

    /// <summary>Holds an admitted record: its vector into its slot, its text staged.</summary>
    private void Write(TKey key, TRecord record)
    {
        int slot = _held.Put(key, record, _schema.VectorOf(record).Span);
        _keywords?.Stage(slot, _schema.FullTextOf(record));
    }

    private void Erase(TKey key)
    {
        int slot = _held.Remove(key);
        if (slot >= 0)
        {
            _keywords?.Remove(slot);
        }
    }

    /// <inheritdoc />
    public override Task<TRecord?> GetAsync(
        TKey key, RecordRetrievalOptions? options = null, CancellationToken cancellationToken = default) =>
        Task.FromResult(_held.TryGet(key, out TRecord? record) ? record : null);

    /// <inheritdoc />
    /// <exception cref="ArgumentNullException"><paramref name="keys"/> is null.</exception>
    /// <exception cref="OperationCanceledException"><paramref name="cancellationToken"/> is cancelled between records.</exception>
    public override async IAsyncEnumerable<TRecord> GetAsync(
        IEnumerable<TKey> keys,
        RecordRetrievalOptions? options = null,
        [EnumeratorCancellation] CancellationToken cancellationToken = default)
    {
        Guard.NotNull(keys);
        var found = new List<TRecord>();
        foreach (TKey key in keys)
        {
            if (_held.TryGet(key, out TRecord? record))
            {
                found.Add(record!);
            }
        }

        foreach (TRecord record in found)
        {
            cancellationToken.ThrowIfCancellationRequested();
            yield return record;
        }

        await Task.CompletedTask.ConfigureAwait(false);
    }

    /// <inheritdoc />
    /// <exception cref="ArgumentNullException"><paramref name="filter"/> is null.</exception>
    /// <exception cref="ArgumentOutOfRangeException"><paramref name="top"/> is less than 1.</exception>
    /// <exception cref="NotSupportedException"><paramref name="options"/> sets <c>OrderBy</c>.</exception>
    /// <exception cref="OperationCanceledException"><paramref name="cancellationToken"/> is cancelled between records.</exception>
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

        // Materialised before the first yield, so a write made while the caller enumerates
        // changes nothing already answered.
        List<TRecord> admitted = [.. _held.Records().Where(filter.Compile()).Skip(options?.Skip ?? 0).Take(top)];
        foreach (TRecord record in admitted)
        {
            cancellationToken.ThrowIfCancellationRequested();
            yield return record;
        }

        await Task.CompletedTask.ConfigureAwait(false);
    }

    /// <inheritdoc />
    /// <exception cref="ArgumentNullException"><paramref name="serviceType"/> is null.</exception>
    public override object? GetService(Type serviceType, object? serviceKey = null)
    {
        Guard.NotNull(serviceType);
        return serviceType == typeof(VectorStoreCollectionMetadata) && serviceKey is null ? Metadata() : null;
    }

    // Its own method so the guard above runs even where the init-only setters below fail to bind:
    // the netstandard2.0 build loaded beside a newer target's abstractions throws MissingMethodException.
    private VectorStoreCollectionMetadata Metadata() =>
        new() { VectorStoreSystemName = "lodestar", CollectionName = Name };

    /// <inheritdoc />
    /// <exception cref="ArgumentOutOfRangeException"><paramref name="top"/> is less than 1.</exception>
    /// <exception cref="ArgumentException">The query is not the collection's vector width.</exception>
    /// <exception cref="NotSupportedException"><paramref name="searchValue"/> is not a vector, and this package generates none.</exception>
    /// <exception cref="OperationCanceledException"><paramref name="cancellationToken"/> is cancelled between results.</exception>
    /// <remarks>
    /// With a filter, the filter runs once on every record, in the order they are held, before
    /// the cut, so <paramref name="top"/> means <paramref name="top"/>: a caller asking for five
    /// matching records gets five whenever five match. Post-filtering a top-k would return fewer
    /// without saying why. Only the records it admits are scored.
    /// </remarks>
    public override async IAsyncEnumerable<VectorSearchResult<TRecord>> SearchAsync<TInput>(
        TInput searchValue,
        int top,
        VectorSearchOptions<TRecord>? options = null,
        [EnumeratorCancellation] CancellationToken cancellationToken = default)
    {
        Guard.NotLessThan(top, 1);
        ReadOnlyMemory<float> query = QueryOf(searchValue);
        VectorSearchOptions<TRecord> settings = options ?? new VectorSearchOptions<TRecord>();
        List<VectorSearchResult<TRecord>> results = Nearest(query, top, settings);

        foreach (VectorSearchResult<TRecord> result in results)
        {
            cancellationToken.ThrowIfCancellationRequested();
            yield return result;
        }

        await Task.CompletedTask.ConfigureAwait(false);
    }

    /// <summary>The whole of a vector search, computed before anything is yielded.</summary>
    /// <remarks>
    /// The filter, when there is one, runs before any scoring, so a rejected record costs no dot
    /// product, and only the survivors wanted are kept in a bounded heap. Hits resolve to their
    /// records here, so a write made while the results are enumerated cannot pair a score with
    /// another record. The threshold is safe to apply after the cut: it drops a suffix of the
    /// order, never a record ahead of one it keeps.
    /// </remarks>
    private List<VectorSearchResult<TRecord>> Nearest(
        ReadOnlyMemory<float> query, int top, VectorSearchOptions<TRecord> settings)
    {
        // A huge Skip overflows int, so top + Skip is summed in long.
        int wanted = (int)Math.Min((long)top + settings.Skip, _held.Count);
        SearchResult[] ranked = _held.Nearest(query.Span, wanted, RecordFilter.Compile(settings.Filter));
        return [.. ranked
            .Select(hit => new VectorSearchResult<TRecord>(_held.At(hit.Index)!, hit.Score))
            .Where(result => settings.ScoreThreshold is not { } threshold || result.Score >= threshold)
            .Skip(settings.Skip)
            .Take(top)];
    }

    /// <summary>The search value as a vector of the collection's width.</summary>
    /// <remarks>Checked against the schema, not the index, so an empty collection refuses a wrong width too.</remarks>
    private ReadOnlyMemory<float> QueryOf<TInput>(TInput searchValue)
        where TInput : notnull
    {
        ReadOnlyMemory<float> query = AsVector(searchValue);
        if (query.Length != Schema.Dimension)
        {
            throw new ArgumentException(
                $"query length {query.Length} != dimension {Schema.Dimension}.", nameof(searchValue));
        }

        return query;
    }

    private static ReadOnlyMemory<float> AsVector<TInput>(TInput searchValue)
        where TInput : notnull => searchValue switch
        {
            ReadOnlyMemory<float> memory => memory,
            float[] array => array,
            _ => throw new NotSupportedException(
                $"{typeof(TInput).Name} is not a vector, and this package generates none: supply a "
                + "ReadOnlyMemory<float> or a float[], or embed the text with Lodestar.Extensions.AI first."),
        };

    /// <summary>Fuses the vector ranking with a BM25 ranking over the keywords.</summary>
    /// <typeparam name="TInput">The search value's type; a vector, since this package generates none.</typeparam>
    /// <param name="searchValue">The query vector.</param>
    /// <param name="keywords">The terms the keyword half scores, taken as one query document.</param>
    /// <param name="top">How many fused results to return.</param>
    /// <param name="options">A filter and a skip, applied to the fused ranking.</param>
    /// <param name="cancellationToken">Checked between results.</param>
    /// <exception cref="ArgumentNullException"><paramref name="keywords"/> is null.</exception>
    /// <exception cref="ArgumentOutOfRangeException"><paramref name="top"/> is less than 1, or the options' vectorizer or BM25 settings are outside their range.</exception>
    /// <exception cref="ArgumentException">The query is not the collection's vector width, or the options' <c>NgramRange</c> is not an ascending range from 1.</exception>
    /// <exception cref="InvalidOperationException">The options' <c>MaxDf</c> corresponds to fewer records than their <c>MinDf</c>.</exception>
    /// <exception cref="NotSupportedException">The record type marks no <c>IsFullTextIndexed</c> property, the search value is not a vector, or <paramref name="options"/> sets <c>ScoreThreshold</c>.</exception>
    /// <exception cref="OperationCanceledException"><paramref name="cancellationToken"/> is cancelled between results.</exception>
    /// <remarks>
    /// A term the collection never saw scores nothing rather than failing. A document the
    /// keywords do not match is dropped from the keyword ranking, rather than kept at score
    /// zero in index order, because <see cref="RankFusion.Rrf"/> reads rank position, not
    /// score. Fusion is reciprocal rank at <see cref="LodestarVectorStoreOptions.RankFusionK"/>,
    /// and a threshold is refused because a fused score is a sum of reciprocal ranks, not a similarity.
    /// </remarks>
    public async IAsyncEnumerable<VectorSearchResult<TRecord>> HybridSearchAsync<TInput>(
        TInput searchValue,
        ICollection<string> keywords,
        int top,
        HybridSearchOptions<TRecord>? options = null,
        [EnumeratorCancellation] CancellationToken cancellationToken = default)
        where TInput : notnull
    {
        Guard.NotNull(keywords);
        Guard.NotLessThan(top, 1);
        ReadOnlyMemory<float> query = QueryOf(searchValue);
        if (!Schema.HasFullText)
        {
            throw new NotSupportedException(
                $"{typeof(TRecord).Name} marks no property [VectorStoreData(IsFullTextIndexed = true)], "
                + "so this collection has no keyword half to fuse with.");
        }

        HybridSearchOptions<TRecord> settings = options ?? new HybridSearchOptions<TRecord>();
        if (settings.ScoreThreshold is not null)
        {
            throw new NotSupportedException(
                "ScoreThreshold is not supported on a hybrid search: a fused score is a sum of "
                + "1 / (k + rank) over the rankings, not a similarity, so no threshold written for "
                + "similarities means anything against it.");
        }

        List<VectorSearchResult<TRecord>> results = Fused(query, keywords, top, settings);
        foreach (VectorSearchResult<TRecord> result in results)
        {
            cancellationToken.ThrowIfCancellationRequested();
            yield return result;
        }

        await Task.CompletedTask.ConfigureAwait(false);
    }

    /// <summary>The whole of a hybrid search, computed before anything is yielded.</summary>
    /// <remarks>
    /// Only matched records enter the keyword ranking, since <see cref="RankFusion.Rrf"/> reads rank
    /// position and not score; <see cref="FusedTop"/> then fuses it with the vector ranking without
    /// ranking either in full, and the filter and <c>Skip</c> apply to the fused order.
    /// </remarks>
    private List<VectorSearchResult<TRecord>> Fused(
        ReadOnlyMemory<float> query,
        ICollection<string> keywords,
        int top,
        HybridSearchOptions<TRecord> settings)
    {
        if (_held.Count == 0)
        {
            return [];
        }

        KeywordMatches matched = _keywords!.Matched(keywords);
        int wanted = (int)Math.Min((long)top + settings.Skip, _held.Count);
        List<(int Slot, double Score)> fused = FusedTop.Select(
            _held, query.Span, matched, Options.RankFusionK, wanted, RecordFilter.Compile(settings.Filter));

        return [.. fused
            .Select(hit => new VectorSearchResult<TRecord>(_held.At(hit.Slot)!, hit.Score))
            .Skip(settings.Skip)
            .Take(top)];
    }

    /// <summary>Releases resources — none, here: the base class declares the pattern and a consumer's <c>using</c> has to reach something.</summary>
    /// <param name="disposing"><see langword="true"/> when called from <see cref="IDisposable.Dispose"/> rather than a finalizer.</param>
    protected override void Dispose(bool disposing) => base.Dispose(disposing);

    /// <inheritdoc />
    bool IExistingCollection.Exists => _exists;

    /// <inheritdoc />
    bool IExistingCollection.Deleted => _deleted;

    /// <inheritdoc />
    Task IExistingCollection.EnsureDeletedAsync(CancellationToken cancellationToken) =>
        EnsureCollectionDeletedAsync(cancellationToken);
}
#pragma warning restore CA1711
