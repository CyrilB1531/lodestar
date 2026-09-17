namespace Lodestar.Fuzzy;

/// <summary>A single extraction hit: the matched choice, its score and its index in the input.</summary>
public readonly record struct ExtractResult(string Choice, double Score, int Index);

/// <summary>
/// Finds the best matches for a query within a collection of choices, reproducing
/// <c>rapidfuzz.process</c>.
/// </summary>
/// <remarks>
/// The default scorer is <see cref="Fuzz.WRatio(string, string)"/> (like rapidfuzz). Results are
/// sorted by score descending, ties broken by original index, filtered by a score
/// cutoff and capped at a limit.
/// </remarks>
public static class Process
{
    /// <summary>
    /// Returns the best matches for <paramref name="query"/> among <paramref name="choices"/>.
    /// </summary>
    /// <param name="query">The query string.</param>
    /// <param name="choices">The candidate strings.</param>
    /// <param name="scorer">Similarity scorer (default <see cref="Fuzz.WRatio(string, string)"/>), returning a value in [0, 100].</param>
    /// <param name="limit">Maximum number of results (default 5); <c>null</c> returns all above the cutoff.</param>
    /// <param name="scoreCutoff">Minimum score to keep (inclusive). Default 0.</param>
    /// <exception cref="ArgumentNullException"><paramref name="query"/> or <paramref name="choices"/> is null.</exception>
    /// <exception cref="ArgumentOutOfRangeException"><paramref name="limit"/> is negative.</exception>
    public static IReadOnlyList<ExtractResult> Extract(
        string query,
        IEnumerable<string> choices,
        Func<string, string, double>? scorer = null,
        int? limit = 5,
        double scoreCutoff = 0.0)
    {
        Guard.NotNull(query);
        Guard.NotNull(choices);
        if (limit is < 0)
        {
            // rapidfuzz's compiled extract fails on a negative limit too, with no parameter named.
            throw new ArgumentOutOfRangeException(nameof(limit), limit, "The limit cannot be negative.");
        }
        scorer ??= Fuzz.WRatio;

        // A bounded limit keeps only a heap of that many hits: the final order is total, so the
        // kept set and its sorted order are the ones a full sort and truncation gave.
        if (limit is { } bound)
        {
            return TopHits(query, choices, scorer, bound, scoreCutoff);
        }

        var hits = new List<ExtractResult>();
        int index = 0;
        foreach (string choice in choices)
        {
            double score = scorer(query, choice);
            if (score >= scoreCutoff)
            {
                hits.Add(new ExtractResult(choice, score, index));
            }
            index++;
        }

        hits.Sort(Compare);
        return hits;
    }

    /// <summary>Stable order: score descending, ties by original index.</summary>
    private static int Compare(ExtractResult x, ExtractResult y)
    {
        int c = y.Score.CompareTo(x.Score);
        return c != 0 ? c : x.Index.CompareTo(y.Index);
    }

    private static List<ExtractResult> TopHits(
        string query,
        IEnumerable<string> choices,
        Func<string, string, double> scorer,
        int limit,
        double scoreCutoff)
    {
        // A heap ordered by Compare with the worst kept hit at its root, the one a better hit evicts.
        var heap = new List<ExtractResult>(Math.Min(limit, 64));
        int index = 0;
        foreach (string choice in choices)
        {
            double score = scorer(query, choice);
            if (score >= scoreCutoff && limit > 0)
            {
                var hit = new ExtractResult(choice, score, index);
                if (heap.Count < limit)
                {
                    heap.Add(hit);
                    SiftUp(heap, heap.Count - 1);
                }

                // Every kept hit has a lower index, so only a strictly higher score ranks above the root.
                else if (score.CompareTo(heap[0].Score) > 0)
                {
                    heap[0] = hit;
                    SiftDown(heap, 0);
                }
            }
            index++;
        }

        heap.Sort(Compare);
        return heap;
    }

    private static void SiftUp(List<ExtractResult> heap, int child)
    {
        ExtractResult item = heap[child];
        while (child > 0)
        {
            int parent = (child - 1) >> 1;
            if (Compare(heap[parent], item) >= 0)
            {
                break;
            }
            heap[child] = heap[parent];
            child = parent;
        }
        heap[child] = item;
    }

    private static void SiftDown(List<ExtractResult> heap, int parent)
    {
        ExtractResult item = heap[parent];
        int count = heap.Count;
        while (true)
        {
            int child = (2 * parent) + 1;
            if (child >= count)
            {
                break;
            }
            if (child + 1 < count && Compare(heap[child + 1], heap[child]) > 0)
            {
                child++;
            }
            if (Compare(heap[child], item) <= 0)
            {
                break;
            }
            heap[parent] = heap[child];
            parent = child;
        }
        heap[parent] = item;
    }

    /// <summary>Returns the single best match, or <c>null</c> if none clears the cutoff.</summary>
    public static ExtractResult? ExtractOne(
        string query,
        IEnumerable<string> choices,
        Func<string, string, double>? scorer = null,
        double scoreCutoff = 0.0)
    {
        Guard.NotNull(query);
        Guard.NotNull(choices);
        scorer ??= Fuzz.WRatio;

        // Strictly higher only, so the first of equal scores stays: the head of the sorted list.
        ExtractResult? best = null;
        int index = 0;
        foreach (string choice in choices)
        {
            double score = scorer(query, choice);
            if (score >= scoreCutoff && (best is not { } kept || score.CompareTo(kept.Score) > 0))
            {
                best = new ExtractResult(choice, score, index);
            }
            index++;
        }
        return best;
    }
}
