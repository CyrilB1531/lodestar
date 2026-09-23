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
    /// <summary>Held once, so a scorer a caller passed can be told from the one <see cref="Cdist"/> defaults to.</summary>
    private static readonly Func<string, string, double> IndelRatio = Fuzz.Ratio;

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

    /// <summary>Scores every query against every choice, reproducing <c>rapidfuzz.process.cdist</c>.</summary>
    /// <param name="queries">The queries, one per row of the result.</param>
    /// <param name="choices">The choices, one per column.</param>
    /// <param name="scorer">
    /// Similarity scorer in <c>[0, 100]</c>. <see langword="null"/> takes
    /// <see cref="Fuzz.Ratio(string, string)"/> — <c>cdist</c>'s default, <strong>not</strong>
    /// <see cref="Extract"/>'s <see cref="Fuzz.WRatio(string, string)"/>.
    /// </param>
    /// <param name="scoreCutoff">Minimum score to report; a cell below it reads <c>0</c> rather than being dropped.</param>
    /// <returns>A <see cref="ScoreMatrix"/> of <c>queries.Count</c> rows by <c>choices.Count</c> columns.</returns>
    /// <exception cref="ArgumentNullException"><paramref name="queries"/> or <paramref name="choices"/> is null.</exception>
    /// <exception cref="ArgumentOutOfRangeException">The matrix would hold more than <see cref="int.MaxValue"/> scores.</exception>
    /// <remarks>
    /// The cutoff zeroes where <see cref="Extract"/>'s filters, the reference's meaning and all a
    /// matrix can do; empty inputs give an empty matrix, not a refusal. On the default scorer it
    /// also skips pairs a length bound already put under it — <c>BelowLengthCeiling</c> has why.
    /// </remarks>
    public static ScoreMatrix Cdist(
        IReadOnlyList<string> queries,
        IReadOnlyList<string> choices,
        Func<string, string, double>? scorer = null,
        double scoreCutoff = 0.0)
    {
        Guard.NotNull(queries);
        Guard.NotNull(choices);
        scorer ??= IndelRatio;

        // A cell under the cutoff reads 0 whatever it scores, so a pair the lengths alone put
        // there needs no scan. Only Ratio may take it: see BelowLengthCeiling (#1134).
        bool bounded = scoreCutoff > 0.0 && IndelRatio.Equals(scorer);

        int rows = queries.Count;
        int columns = choices.Count;
        long cells = (long)rows * columns;
        if (cells > int.MaxValue)
        {
            throw new ArgumentOutOfRangeException(
                nameof(queries), cells,
                $"Scoring {rows} queries against {columns} choices needs {cells} scores, past int.MaxValue.");
        }

        var scores = new double[cells];
        for (int row = 0; row < rows; row++)
        {
            string query = queries[row];
            int start = row * columns;
            for (int column = 0; column < columns; column++)
            {
                string choice = choices[column];
                if (bounded && BelowLengthCeiling(query, choice, scoreCutoff))
                {
                    continue;
                }

                double score = scorer(query, choice);
                scores[start + column] = score >= scoreCutoff ? score : 0.0;
            }
        }

        return new ScoreMatrix(rows, columns, scores);
    }

    /// <summary>Whether the two lengths alone put a pair under the cutoff, with no scan able to lift it.</summary>
    /// <remarks>
    /// An Indel edit moves one character, so the distance is at least the difference in lengths.
    /// Evaluating what <see cref="Fuzz.Ratio(string, string)"/> evaluates with that floor in the
    /// distance's place makes the ceiling exact where it binds, so a pair scoring the cutoff to
    /// the last bit survives it. It is <em>false</em> for a length-blind scorer:
    /// <c>partial_ratio("cat", "the cat sat on the mat")</c> is 100 against a ceiling of 24.
    /// </remarks>
    private static bool BelowLengthCeiling(string? a, string? b, double scoreCutoff)
    {
        // A null element is left to the scorer, which is what names it in the exception.
        if (a is null || b is null)
        {
            return false;
        }

        int total = a.Length + b.Length;
        return total != 0 &&
            100.0 * (1.0 - ((double)Math.Abs(a.Length - b.Length) / total)) < scoreCutoff;
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
