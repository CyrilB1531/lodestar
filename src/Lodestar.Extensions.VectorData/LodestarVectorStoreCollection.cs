using System.Linq.Expressions;
using System.Runtime.CompilerServices;
using Lodestar.Abstractions;
using Lodestar.Embeddings.Search;
using Lodestar.Text.Search;
using Lodestar.Text.Vectorization;
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
public sealed class LodestarVectorStoreCollection<TKey, TRecord>
    : VectorStoreCollection<TKey, TRecord>, IKeywordHybridSearchable<TRecord>, IExistingCollection
    where TKey : notnull
    where TRecord : class
{
    private readonly Dictionary<TKey, TRecord> _records = [];
    private readonly RecordSchema<TKey, TRecord> _schema;
    private readonly LodestarVectorStoreOptions _options;
    private DerivedIndexes<TKey, TRecord>? _indexes;
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
    }

    /// <inheritdoc />
    public override string Name { get; }

    /// <summary>How many times the caches have been rebuilt, which the suite asserts on.</summary>
    internal int RebuildCount { get; private set; }

    /// <summary>The caches, rebuilt first when a write has happened since the last read.</summary>
    internal DerivedIndexes<TKey, TRecord> Current()
    {
        if (_indexes is null)
        {
            _indexes = DerivedIndexes<TKey, TRecord>.Build(_records.Values, _schema, _options);
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
        _deleted = false;
        return Task.CompletedTask;
    }

    /// <inheritdoc />
    public override Task EnsureCollectionDeletedAsync(CancellationToken cancellationToken = default)
    {
        _exists = false;
        _deleted = true;
        Invalidate();
        _records.Clear();
        return Task.CompletedTask;
    }

    /// <inheritdoc />
    /// <exception cref="ArgumentNullException"><paramref name="record"/> is null.</exception>
    /// <exception cref="ArgumentException">The record's key is null, or its vector is not the collection's width.</exception>
    public override Task UpsertAsync(TRecord record, CancellationToken cancellationToken = default)
    {
        TKey key = _schema.Admit(record, nameof(record));
        Invalidate();
        _records[key] = record;
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

        Invalidate();
        foreach (KeyValuePair<TKey, TRecord> entry in admitted)
        {
            _records[entry.Key] = entry.Value;
        }

        _exists = true;
        _deleted = false;
        return Task.CompletedTask;
    }

    /// <inheritdoc />
    /// <exception cref="ArgumentNullException"><paramref name="key"/> is null.</exception>
    public override Task DeleteAsync(TKey key, CancellationToken cancellationToken = default)
    {
        if (_records.ContainsKey(key))
        {
            Invalidate();
            _records.Remove(key);
        }

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

        if (Array.Exists(batch, _records.ContainsKey))
        {
            Invalidate();
            foreach (TKey key in batch)
            {
                _records.Remove(key);
            }
        }

        return Task.CompletedTask;
    }

    /// <inheritdoc />
    public override Task<TRecord?> GetAsync(
        TKey key, RecordRetrievalOptions? options = null, CancellationToken cancellationToken = default) =>
        Task.FromResult(_records.TryGetValue(key, out TRecord? record) ? record : null);

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
            if (_records.TryGetValue(key, out TRecord? record))
            {
                found.Add(record);
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
        // cannot invalidate the dictionary enumerator this walk would otherwise still hold.
        List<TRecord> admitted = [.. _records.Values.Where(filter.Compile()).Skip(options?.Skip ?? 0).Take(top)];
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
    /// <exception cref="ArgumentException">The query, or a held record changed in place since its upsert, is not the collection's vector width.</exception>
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
        List<VectorSearchResult<TRecord>> results = Nearest(Current(), query, top, settings);

        foreach (VectorSearchResult<TRecord> result in results)
        {
            cancellationToken.ThrowIfCancellationRequested();
            yield return result;
        }

        await Task.CompletedTask.ConfigureAwait(false);
    }

    /// <summary>The whole of a vector search, computed before anything is yielded.</summary>
    /// <remarks>
    /// Hits resolve through the records the indexes were built from, never the live dictionary,
    /// so every score belongs to the record it is returned with.
    /// </remarks>
    private static List<VectorSearchResult<TRecord>> Nearest(
        DerivedIndexes<TKey, TRecord> indexes, ReadOnlyMemory<float> query, int top, VectorSearchOptions<TRecord> settings)
    {
        Func<TRecord, bool>? admits = RecordFilter.Compile(settings.Filter);
        IEnumerable<VectorSearchResult<TRecord>> ranked = admits is null
            ? Scored(indexes, query, (int)Math.Min((long)top + settings.Skip, indexes.Vectors.Count))
                .Select(hit => new VectorSearchResult<TRecord>(indexes.Records[hit.Index], hit.Score))
            : Admitted(indexes, query, top, settings.Skip, admits);

        // A huge Skip overflows int, so top + Skip is summed in long, here and in Admitted.
        return [.. ranked
            .Where(result => settings.ScoreThreshold is not { } threshold || result.Score >= threshold)
            .Skip(settings.Skip)
            .Take(top)];
    }

    /// <summary>The best <paramref name="top"/> + <paramref name="skip"/> records the filter admits, best first.</summary>
    /// <remarks>
    /// The filter runs before any scoring, so a rejected record costs no dot product, and only
    /// the survivors wanted are kept in a bounded heap rather than the whole collection sorted.
    /// Scores are <see cref="EmbeddingIndex.Search"/>'s own: the same normalized query dotted with
    /// the same stored row, ordered by score descending then position ascending. The threshold is
    /// safe to apply after the cut: it drops a suffix of that order, never a record ahead of one it keeps.
    /// </remarks>
    private static IEnumerable<VectorSearchResult<TRecord>> Admitted(
        DerivedIndexes<TKey, TRecord> indexes, ReadOnlyMemory<float> query, int top, int skip, Func<TRecord, bool> admits)
    {
        EmbeddingIndex vectors = indexes.Vectors;
        if (vectors.Count == 0)
        {
            return [];
        }
        float[] normalized = query.ToArray();
        float norm = VectorMath.L2Norm(normalized);
        if (norm > 0)
        {
            for (int i = 0; i < normalized.Length; i++)
            {
                normalized[i] /= norm;
            }
        }

        int dimension = vectors.Dimension;
        ReadOnlySpan<float> block = indexes.Block.Span;
        var best = new TopHits((int)Math.Min((long)top + skip, vectors.Count));
        for (int row = 0; row < vectors.Count; row++)
        {
            if (admits(indexes.Records[row]))
            {
                best.Offer(new SearchResult(row, VectorMath.Dot(normalized, block.Slice(row * dimension, dimension))));
            }
        }

        return best.Ranked().Select(hit => new VectorSearchResult<TRecord>(indexes.Records[hit.Index], hit.Score));
    }

    private static IReadOnlyList<SearchResult> Scored(
        DerivedIndexes<TKey, TRecord> indexes, ReadOnlyMemory<float> query, int wanted) =>
        indexes.Vectors.Count == 0
            ? []
            : indexes.Vectors.Search(query.Span, Math.Min(wanted, indexes.Vectors.Count));

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
    /// <exception cref="ArgumentOutOfRangeException"><paramref name="top"/> is less than 1.</exception>
    /// <exception cref="ArgumentException">The query, or a held record changed in place since its upsert, is not the collection's vector width.</exception>
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

        List<VectorSearchResult<TRecord>> results = Fused(Current(), query, keywords, top, settings);
        foreach (VectorSearchResult<TRecord> result in results)
        {
            cancellationToken.ThrowIfCancellationRequested();
            yield return result;
        }

        await Task.CompletedTask.ConfigureAwait(false);
    }

    /// <summary>The whole of a hybrid search, computed before anything is yielded.</summary>
    private List<VectorSearchResult<TRecord>> Fused(
        DerivedIndexes<TKey, TRecord> indexes,
        ReadOnlyMemory<float> query,
        ICollection<string> keywords,
        int top,
        HybridSearchOptions<TRecord> settings)
    {
        Func<TRecord, bool>? admits = RecordFilter.Compile(settings.Filter);
        int[] byVector = [.. Scored(indexes, query, indexes.Vectors.Count).Select(hit => hit.Index)];
        int[] byKeyword = KeywordRanking(indexes, keywords);

        IEnumerable<VectorSearchResult<TRecord>> fused = RankFusion.Rrf([byVector, byKeyword], Options.RankFusionK)
            .Select(hit => new VectorSearchResult<TRecord>(indexes.Records[hit.Document], hit.Score))
            .Where(result => admits is null || admits(result.Record));

        return [.. fused.Skip(settings.Skip).Take(top)];
    }

    /// <summary>The documents the keywords matched, best first; none when nothing is held.</summary>
    /// <remarks>
    /// A marked schema over no records builds no keyword index, so the empty ranking is the answer
    /// rather than a refusal. Only matched documents are ranked, since <see cref="RankFusion.Rrf"/>
    /// reads rank position and not score; matching is read from the term postings
    /// and not from the score, whose sign Robertson's IDF leaves open. The order is <c>Top</c>'s:
    /// score descending, then document index (#993).
    /// </remarks>
    private static int[] KeywordRanking(DerivedIndexes<TKey, TRecord> indexes, ICollection<string> keywords)
    {
        if (indexes.Keywords is null || indexes.Vectorizer is null || indexes.Postings is null)
        {
            return [];
        }

        return indexes.Postings.Ranked(QueryTerms(indexes.Vectorizer, keywords), indexes.Keywords);
    }

    /// <summary>The keywords as column indices of the fitted vocabulary, unseen terms dropped.</summary>
    /// <remarks>
    /// <see cref="CsrMatrix"/> publishes no per-row column enumerator, so this walks
    /// <see cref="CsrMatrix.RowPointers"/> and <see cref="CsrMatrix.ColumnIndices"/> directly —
    /// the same pattern <c>Bm25Index</c> itself uses to enumerate a row's non-zeros.
    /// </remarks>
    private static List<int> QueryTerms(CountVectorizer vectorizer, ICollection<string> keywords)
    {
        CsrMatrix row = vectorizer.Transform([string.Join(" ", keywords)]);
        var terms = new List<int>();
        for (int k = row.RowPointers[0]; k < row.RowPointers[1]; k++)
        {
            terms.Add(row.ColumnIndices[k]);
        }

        return terms;
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

    private void Invalidate() => _indexes = null;
}
#pragma warning restore CA1711
