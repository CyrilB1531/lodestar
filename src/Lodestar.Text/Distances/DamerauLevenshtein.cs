using System.Buffers;
using Lodestar.Text.Internal;

namespace Lodestar.Text.Distances;

// SonarLint S3776: cognitive complexity: a faithful implementation of a published rule-engine; decomposing it would break the 1:1 mapping with the reference that makes divergences auditable.
// SonarLint S4136: the overloads are grouped by concern, with the generic core deliberately last.
#pragma warning disable S3776, S4136

/// <summary>
/// Unrestricted Damerau-Levenshtein (Lowrance-Wagner: insertions, deletions,
/// substitutions and transpositions of adjacent characters, a substring editable
/// more than once, unlike <see cref="Osa"/>). A true metric at unit costs.
/// </summary>
/// <remarks>
/// Reference behavior: <c>rapidfuzz.distance.DamerauLevenshtein</c>; see
/// <c>docs/equivalence.md</c>. See <see cref="TextElement"/> for the UTF-16
/// vs code-point choice. All members are stateless and thread-safe.
/// </remarks>
public static class DamerauLevenshtein
{
    // Cached so the code-point path allocates no delegate per call.
    private static readonly CodePointPair.Measure MeasureCodePoints = Distance<int>;

    /// <summary>Computes the Damerau-Levenshtein distance between <paramref name="a"/> and <paramref name="b"/>.</summary>
    public static int Distance(ReadOnlySpan<char> a, ReadOnlySpan<char> b, TextElement element = TextElement.Utf16Unit)
    {
        return element == TextElement.CodePoint
            ? DistanceCodePoints(a, b, out _, out _)
            : Distance<char>(a, b);
    }

    /// <summary>Length-normalized distance in <c>[0, 1]</c>: <c>distance / max(len(a), len(b))</c>.</summary>
    public static double NormalizedDistance(ReadOnlySpan<char> a, ReadOnlySpan<char> b, TextElement element = TextElement.Utf16Unit)
    {
        int distance;
        int maxLen;
        if (element == TextElement.CodePoint)
        {
            distance = DistanceCodePoints(a, b, out int lenA, out int lenB);
            maxLen = Math.Max(lenA, lenB);
        }
        else
        {
            distance = Distance<char>(a, b);
            maxLen = Math.Max(a.Length, b.Length);
        }

        return maxLen == 0 ? 0.0 : (double)distance / maxLen;
    }

    /// <summary>Length-normalized similarity in <c>[0, 1]</c>: <c>1 - NormalizedDistance</c>.</summary>
    public static double NormalizedSimilarity(ReadOnlySpan<char> a, ReadOnlySpan<char> b, TextElement element = TextElement.Utf16Unit)
    {
        return 1.0 - NormalizedDistance(a, b, element);
    }

    /// <summary>Computes the Damerau-Levenshtein distance over any sequence of equatable elements.</summary>
    /// <remarks>
    /// The common prefix and suffix go first, as rapidfuzz strips them; the rest runs Zhao and
    /// Sahni's linear-space recurrence over the shorter side, three rows instead of the whole
    /// (m + 2) × (n + 2) table (#1199).
    /// </remarks>
    public static int Distance<T>(ReadOnlySpan<T> a, ReadOnlySpan<T> b)
        where T : IEquatable<T>
    {
        Affixes.Trim(ref a, ref b);
        // The distance is symmetric, so the shorter side takes the columns and the rows cost its length.
        if (b.Length > a.Length)
        {
            ReadOnlySpan<T> longer = b;
            b = a;
            a = longer;
        }

        int m = a.Length;
        int n = b.Length;
        if (m == 0)
        {
            return n;
        }
        if (n == 0)
        {
            return m;
        }

        // Each symbol of b gets a dense id once and a reuses them (0 when b lacks it): a transposition
        // only ever asks where b's symbol last occurred in a, so a's other symbols need no slot (#828).
        var ids = new Dictionary<T, int>();
        int[] bId = new int[n];
        for (int j = 0; j < n; j++)
        {
            if (!ids.TryGetValue(b[j], out int id))
            {
                id = ids.Count + 1;
                ids.Add(b[j], id);
            }

            bId[j] = id;
        }

        int[] lastRow = new int[ids.Count + 1]; // symbol id -> last row of a holding it, 0 for none
        int big = m + 1; // past any distance, which is at most the longer length
        int width = n + 2;
        int[] rented = ArrayPool<int>.Shared.Rent(3 * width);
        try
        {
            // Three rows offset by one, so column -1 exists: the row being written, the row
            // before it, and each column's value two rows up at its last match (Zhao's FR).
            Span<int> all = rented.AsSpan(0, 3 * width);
            Span<int> row = all.Slice(0, width);
            Span<int> previous = all.Slice(width, width);
            Span<int> saved = all.Slice(2 * width, width);
            row[0] = big;
            for (int j = 0; j <= n; j++)
            {
                row[j + 1] = j;
            }
            previous.Fill(big);
            saved.Fill(big);

            for (int i = 1; i <= m; i++)
            {
                // row now holds the row two up, which the transpositions read before it is overwritten.
                Span<int> swap = row;
                row = previous;
                previous = swap;

                int aId = ids.TryGetValue(a[i - 1], out int known) ? known : 0;
                int lastMatchCol = 0;   // l: the last column of this row where b matched a[i - 1]
                int twoUpLeft = row[1]; // H[i-2][j-1] as j advances
                int twoUpAtMatch = big; // H[i-2][l-1], saved at the last match of this row
                row[1] = i;

                for (int j = 1; j <= n; j++)
                {
                    int value = Math.Min(previous[j] + (aId == bId[j - 1] ? 0 : 1), Math.Min(row[j] + 1, previous[j + 1] + 1));
                    if (aId == bId[j - 1])
                    {
                        lastMatchCol = j;
                        saved[j + 1] = previous[j - 1];
                        twoUpAtMatch = twoUpLeft;
                    }
                    else
                    {
                        int k = lastRow[bId[j - 1]];
                        if (lastMatchCol != 0 && j - lastMatchCol == 1 && k != 0)
                        {
                            value = Math.Min(value, saved[j + 1] + (i - k));
                        }
                        else if (k != 0 && i - k == 1 && lastMatchCol != 0)
                        {
                            value = Math.Min(value, twoUpAtMatch + (j - lastMatchCol));
                        }
                    }

                    twoUpLeft = row[j + 1];
                    row[j + 1] = value;
                }

                if (aId != 0)
                {
                    lastRow[aId] = i;
                }
            }

            return row[n + 1];
        }
        finally
        {
            ArrayPool<int>.Shared.Return(rented);
        }
    }

    private static int DistanceCodePoints(ReadOnlySpan<char> a, ReadOnlySpan<char> b, out int lenA, out int lenB) =>
        CodePointPair.Distance(a, b, MeasureCodePoints, out lenA, out lenB);
}
