using Lodestar.Abstractions;
using Lodestar.Embeddings.Search;
using Lodestar.Text.Search;
using Lodestar.Text.Vectorization;

namespace Lodestar.Extensions.VectorData;

/// <summary>The structures a collection's records are searched through, and the records they index.</summary>
/// <remarks>
/// Built whole, never mutated. <c>EmbeddingIndex</c> only appends and <c>Bm25Index</c> takes
/// no document at all, so a store that must honour delete and replace keeps its records and
/// rebuilds these from them. One rebuild produces all of them, because they share an index:
/// position <c>i</c> of the block, row <c>i</c> of the count matrix, <c>Keys[i]</c> and
/// <c>Records[i]</c> are the same record.
/// </remarks>
internal sealed class DerivedIndexes<TKey, TRecord>
    where TKey : notnull
    where TRecord : class
{
    private DerivedIndexes(
        EmbeddingIndex vectors, float[] block, CsrMatrix? counts, Bm25Index? keywords, CountVectorizer? vectorizer, TKey[] keys, TRecord[] records)
    {
        Vectors = vectors;
        Block = block;
        Counts = counts;
        Postings = counts is null ? null : TermPostings.Of(counts);
        Keywords = keywords;
        Vectorizer = vectorizer;
        Keys = keys;
        Records = records;
    }

    /// <summary>The vector half.</summary>
    public EmbeddingIndex Vectors { get; }

    /// <summary>The rows <see cref="Vectors"/> searches, normalized, which a filtered search scores directly.</summary>
    /// <remarks>
    /// The array <see cref="EmbeddingIndex.FromOwnedBlock"/> was handed and normalized in place, so
    /// it is the index's own storage rather than a copy, and nothing writes to it after the build.
    /// </remarks>
    public ReadOnlyMemory<float> Block { get; }

    /// <summary>The term counts <see cref="Keywords"/> was built over, which say what a query matched.</summary>
    /// <remarks>
    /// A score cannot say it: the default IDF is zero for a term in half the records and its floor
    /// is negative when the mean IDF is, so a match can score at or below an unmatched zero.
    /// </remarks>
    public CsrMatrix? Counts { get; }

    /// <summary>Which records hold each term, or <see langword="null"/> without a keyword half.</summary>
    /// <remarks>
    /// Built once with <see cref="Counts"/> rather than per query: a hybrid search asked which records
    /// matched, and answering it by walking every stored count cost the whole corpus on every call (#993).
    /// </remarks>
    public TermPostings? Postings { get; }

    /// <summary>The keyword half, or <see langword="null"/> when no property is full-text indexed.</summary>
    public Bm25Index? Keywords { get; }

    /// <summary>The vocabulary the keyword half was fitted on, so a query transforms against it.</summary>
    public CountVectorizer? Vectorizer { get; }

    /// <summary>The key at each index position, which is how a hit becomes a record.</summary>
    public IReadOnlyList<TKey> Keys { get; }

    /// <summary>The record at each index position, as it stood when these were built.</summary>
    /// <remarks>
    /// A search resolves its hits here rather than through the collection's dictionary, so a
    /// write made while the results are being enumerated cannot pair a score with another record.
    /// </remarks>
    public IReadOnlyList<TRecord> Records { get; }

    /// <summary>Builds every structure from the records as they stand.</summary>
    /// <param name="records">The collection's records, in the order the indexes will report.</param>
    /// <param name="schema">Where each record keeps its key, vector and text.</param>
    /// <param name="options">How the keyword half is tokenized and scored.</param>
    /// <exception cref="ArgumentException">A record's vector is not <see cref="RecordSchema{TKey, TRecord}.Dimension"/> long, which only a record changed in place after its upsert can be.</exception>
    public static DerivedIndexes<TKey, TRecord> Build(
        IReadOnlyCollection<TRecord> records,
        RecordSchema<TKey, TRecord> schema,
        LodestarVectorStoreOptions options)
    {
        int dimension = schema.Dimension;
        float[] block = new float[records.Count * dimension];
        TKey[] keys = new TKey[records.Count];
        TRecord[] held = new TRecord[records.Count];
        List<string> documents = schema.HasFullText ? new List<string>(records.Count) : [];

        int row = 0;
        foreach (TRecord record in records)
        {
            TKey key = schema.KeyOf(record);
            ReadOnlySpan<float> vector = schema.VectorOf(record).Span;
            if (vector.Length != dimension)
            {
                throw new ArgumentException(
                    $"The record keyed {key} carries a vector of {vector.Length} where this "
                    + $"collection is {dimension} wide.", nameof(records));
            }

            vector.CopyTo(block.AsSpan(row * dimension, dimension));
            keys[row] = key;
            held[row] = record;
            if (schema.HasFullText)
            {
                documents.Add(schema.FullTextOf(record));
            }

            row++;
        }

        // FromOwnedBlock rather than Count calls to Add: the whole block is known here, and
        // the array is this method's own, so handing it over costs no second copy.
        EmbeddingIndex vectors = EmbeddingIndex.FromOwnedBlock(
            block, dimension, BlockNormalization.Normalize);

        if (documents.Count == 0)
        {
            return new DerivedIndexes<TKey, TRecord>(vectors, block, null, null, null, keys, held);
        }

        var vectorizer = new CountVectorizer(options.Vectorizer);
        CsrMatrix counts = vectorizer.FitTransform(documents);
        return new DerivedIndexes<TKey, TRecord>(
            vectors, block, counts, new Bm25Index(counts, options.Bm25), vectorizer, keys, held);
    }
}
