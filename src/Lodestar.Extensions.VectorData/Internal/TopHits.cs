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
    private readonly SearchResult[] _heap;
    private int _count;

    /// <summary>Creates an empty set that keeps at most <paramref name="capacity"/> hits.</summary>
    /// <param name="capacity">How many hits to keep; zero keeps none.</param>
    public TopHits(int capacity) => _heap = new SearchResult[capacity];

    /// <summary>Keeps <paramref name="hit"/> if it ranks among the best offered so far.</summary>
    /// <param name="hit">A scored position.</param>
    public void Offer(SearchResult hit)
    {
        if (_count < _heap.Length)
        {
            _heap[_count] = hit;
            SiftUp(_count++);
        }
        else if (_count > 0 && Ahead(hit, _heap[0]))
        {
            _heap[0] = hit;
            SiftDown(0);
        }
    }

    /// <summary>The hits kept, best first.</summary>
    public SearchResult[] Ranked()
    {
        var ranked = new SearchResult[_count];
        Array.Copy(_heap, ranked, _count);
        Array.Sort(ranked, Compare);
        return ranked;
    }

    /// <summary>Negative when <paramref name="x"/> ranks before <paramref name="y"/>: <see cref="EmbeddingIndex.Search"/>'s own comparison.</summary>
    private static int Compare(SearchResult x, SearchResult y)
    {
        int byScore = y.Score.CompareTo(x.Score);
        return byScore != 0 ? byScore : x.Index.CompareTo(y.Index);
    }

    /// <summary>Whether <paramref name="x"/> ranks before <paramref name="y"/>.</summary>
    private static bool Ahead(SearchResult x, SearchResult y) => Compare(x, y) < 0;

    private void SiftUp(int at)
    {
        while (at > 0)
        {
            int parent = (at - 1) / 2;
            if (!Ahead(_heap[parent], _heap[at]))
            {
                return;
            }
            (_heap[parent], _heap[at]) = (_heap[at], _heap[parent]);
            at = parent;
        }
    }

    private void SiftDown(int at)
    {
        while (true)
        {
            int worst = at;
            int left = (2 * at) + 1;
            int right = left + 1;
            if (left < _count && Ahead(_heap[worst], _heap[left]))
            {
                worst = left;
            }
            if (right < _count && Ahead(_heap[worst], _heap[right]))
            {
                worst = right;
            }
            if (worst == at)
            {
                return;
            }
            (_heap[worst], _heap[at]) = (_heap[at], _heap[worst]);
            at = worst;
        }
    }
}
