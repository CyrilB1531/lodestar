using Microsoft.Extensions.VectorData;

namespace Lodestar.Sample;

/// <summary>The record type the store and collection samples both index.</summary>
/// <remarks>
/// A note carries text a keyword search can match and a hand-picked three-wide vector a
/// nearest-neighbour search can rank, which is enough to exercise both halves of a hybrid
/// search without pulling in an embedder.
/// </remarks>
internal static class VectorDataCorpus
{
    /// <summary>One row of the store: a key, searchable text, and a vector.</summary>
    internal sealed class Note
    {
        [VectorStoreKey]
        public string Id { get; set; } = string.Empty;

        [VectorStoreData(IsFullTextIndexed = true)]
        public string Text { get; set; } = string.Empty;

        [VectorStoreVector(3)]
        public ReadOnlyMemory<float> Embedding { get; set; }
    }

    /// <summary>Three notes, each nearest to a different axis.</summary>
    public static Note[] Notes { get; } =
    [
        new Note { Id = "a", Text = "the cat sat on the mat", Embedding = new float[] { 1f, 0f, 0f } },
        new Note { Id = "b", Text = "the dog ran in the park", Embedding = new float[] { 0f, 1f, 0f } },
        new Note { Id = "c", Text = "an elephant crossed the river", Embedding = new float[] { 0f, 0f, 1f } },
    ];

    /// <summary>A key and a vector, and nothing marked <c>IsFullTextIndexed</c> — HybridSearchAsync's refusal case.</summary>
    internal sealed class PlainNote
    {
        [VectorStoreKey]
        public string Id { get; set; } = string.Empty;

        [VectorStoreVector(3)]
        public ReadOnlyMemory<float> Embedding { get; set; }
    }
}
