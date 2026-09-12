using Lodestar.Abstractions;
using Lodestar.Embeddings.Search;
using Lodestar.Text.Search;
using Lodestar.Text.Vectorization;

namespace Lodestar.Extensions.VectorData;

/// <summary>The three structures a collection's records are searched through.</summary>
/// <remarks>
/// Built whole, never mutated. <c>EmbeddingIndex</c> only appends and <c>Bm25Index</c> takes
/// no document at all, so a store that must honour delete and replace keeps its records and
/// rebuilds these from them. One rebuild produces all three, because they share an index:
/// position <c>i</c> of the block, row <c>i</c> of the count matrix and <c>Keys[i]</c> are
/// the same record.
/// </remarks>
internal sealed class DerivedIndexes<TKey>
    where TKey : notnull
{
    private DerivedIndexes(EmbeddingIndex vectors, Bm25Index? keywords, CountVectorizer? vectorizer, TKey[] keys)
    {
        Vectors = vectors;
        Keywords = keywords;
        Vectorizer = vectorizer;
        Keys = keys;
    }

    /// <summary>The vector half.</summary>
    public EmbeddingIndex Vectors { get; }

    /// <summary>The keyword half, or <see langword="null"/> when no property is full-text indexed.</summary>
    public Bm25Index? Keywords { get; }

    /// <summary>The vocabulary the keyword half was fitted on, so a query transforms against it.</summary>
    public CountVectorizer? Vectorizer { get; }

    /// <summary>The key at each index position, which is how a hit becomes a record.</summary>
    public IReadOnlyList<TKey> Keys { get; }

    /// <summary>Builds all three from the records as they stand.</summary>
    /// <param name="records">The collection's records, in the order the indexes will report.</param>
    /// <param name="schema">Where each record keeps its key, vector and text.</param>
    /// <param name="options">How the keyword half is tokenized and scored.</param>
    /// <exception cref="ArgumentException">A record's vector is not <see cref="RecordSchema{TKey, TRecord}.Dimension"/> long.</exception>
    public static DerivedIndexes<TKey> Build<TRecord>(
        IReadOnlyCollection<TRecord> records,
        RecordSchema<TKey, TRecord> schema,
        LodestarVectorStoreOptions options)
        where TRecord : class
    {
        int dimension = schema.Dimension;
        float[] block = new float[records.Count * dimension];
        TKey[] keys = new TKey[records.Count];
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
            return new DerivedIndexes<TKey>(vectors, null, null, keys);
        }

        var vectorizer = new CountVectorizer(options.Vectorizer);
        CsrMatrix counts = vectorizer.FitTransform(documents);
        return new DerivedIndexes<TKey>(vectors, new Bm25Index(counts, options.Bm25), vectorizer, keys);
    }
}
