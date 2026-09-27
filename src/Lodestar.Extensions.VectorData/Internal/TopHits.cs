using Lodestar.Embeddings.Search;

namespace Lodestar.Extensions.VectorData;

/// <summary>The best hits offered to it, up to a capacity, in <see cref="EmbeddingIndex.Search"/>'s order.</summary>
/// <remarks>
/// A min-heap whose root is the worst hit kept, so an offer that cannot enter costs one
/// comparison. The order is total: score descending by <see cref="float.CompareTo(float)"/>,
/// which puts NaN last, then position ascending, so which hits are kept does not depend on
/// the order they are offered in.
/// </remarks>
internal sealed class TopHits
{
    private readonly Hit[] _heap;
    private int _count;

    /// <summary>Creates an empty set that keeps at most <paramref name="capacity"/> hits.</summary>
    /// <param name="capacity">How many hits to keep; zero keeps none.</param>
    public TopHits(int capacity) => _heap = new Hit[capacity];

    /// <summary>Keeps <paramref name="hit"/> if it ranks among the best offered so far.</summary>
    /// <param name="hit">A scored position.</param>
    public void Offer(SearchResult hit)
    {
        if (_count < _heap.Length)
        {
            _heap[_count] = new Hit(hit);
            MaxHeap.SiftUp(_heap, _count++);
        }
        else if (_count > 0 && Compare(hit, _heap[0].Result) < 0)
        {
            _heap[0] = new Hit(hit);
            MaxHeap.SiftDown(_heap, _count);
        }
    }

    /// <summary>The hits kept, best first.</summary>
    public SearchResult[] Ranked()
    {
        var ranked = new SearchResult[_count];
        for (int i = 0; i < _count; i++)
        {
            ranked[i] = _heap[i].Result;
        }

        Array.Sort(ranked, Compare);
        return ranked;
    }

    /// <summary>Negative when <paramref name="x"/> ranks before <paramref name="y"/>: <see cref="EmbeddingIndex.Search"/>'s own comparison.</summary>
    private static int Compare(SearchResult x, SearchResult y)
    {
        int byScore = y.Score.CompareTo(x.Score);
        return byScore != 0 ? byScore : x.Index.CompareTo(y.Index);
    }

    /// <summary>A hit ordered worst last, so the heap's root is the worst kept.</summary>
    private readonly struct Hit(SearchResult result) : IComparable<Hit>
    {
        public SearchResult Result { get; } = result;

        public int CompareTo(Hit other) => Compare(Result, other.Result);
    }
}
