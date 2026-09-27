using System.Buffers;
using System.Runtime.CompilerServices;
using Lodestar.Text.Internal;

namespace Lodestar.Text.Similarity;

/// <summary>
/// Shared q-gram multiset machinery for the token-set similarity measures.
/// </summary>
/// <remarks>
/// Matches <c>textdistance</c> semantics: q-grams are counted as a <em>multiset</em>
/// (a bag), so repeated grams count with multiplicity. The default <c>qval = 1</c>
/// makes the grams individual characters / code points.
/// </remarks>
internal static class QgramCounts
{
    /// <summary>Computes the multiset intersection size and each side's total gram count.</summary>
    public static (int Intersection, int SizeA, int SizeB) Compute(
        ReadOnlySpan<char> a, ReadOnlySpan<char> b, int qval, TextElement element)
    {
        if (qval < 1)
        {
            throw new ArgumentOutOfRangeException(nameof(qval), qval, "qval must be >= 1.");
        }

        if (element == TextElement.Utf16Unit)
        {
            char[] unitsA = ArrayPool<char>.Shared.Rent(Math.Max(1, a.Length));
            char[] unitsB = ArrayPool<char>.Shared.Rent(Math.Max(1, b.Length));
            try
            {
                a.CopyTo(unitsA);
                b.CopyTo(unitsB);
                return CountSorted(unitsA, a.Length, unitsB, b.Length, qval);
            }
            finally
            {
                ArrayPool<char>.Shared.Return(unitsA);
                ArrayPool<char>.Shared.Return(unitsB);
            }
        }

        // A code point is an int, so a lone surrogate is a gram element like any other rather than
        // a string char.ConvertFromUtf32 refuses to build (#1261).
        int[] pointsA = ArrayPool<int>.Shared.Rent(Math.Max(1, a.Length));
        int[] pointsB = ArrayPool<int>.Shared.Rent(Math.Max(1, b.Length));
        try
        {
            int countA = CodePoints.Decode(a, pointsA);
            int countB = CodePoints.Decode(b, pointsB);
            return CountSorted(pointsA, countA, pointsB, countB, qval);
        }
        finally
        {
            ArrayPool<int>.Shared.Return(pointsA);
            ArrayPool<int>.Shared.Return(pointsB);
        }
    }

    /// <summary>
    /// The score when neither input holds a whole gram: 1 for equal inputs, 0 otherwise.
    /// </summary>
    /// <remarks>
    /// Two empty bags say nothing about the inputs, so <c>"a"</c> against <c>"b"</c> at
    /// <c>qval = 2</c> must not score as identical. textdistance divides by zero there,
    /// and its <c>quick_answer</c> gives equal inputs 1 before counting (#882).
    /// </remarks>
    public static double NoGrams(ReadOnlySpan<char> a, ReadOnlySpan<char> b)
    {
        return a.SequenceEqual(b) ? 1.0 : 0.0;
    }

    /// <summary>The same three counts from each side's grams sorted by content, without a string per gram.</summary>
    /// <remarks>
    /// A gram is its start index; sorting the starts by the gram's characters puts equal grams in runs,
    /// and walking both sorted lists together takes the smaller run of every gram both hold. Ordinal
    /// element order is the equality the string keys used, and the counts are integers. The elements
    /// are UTF-16 units or code points, in the first <paramref name="lengthA"/> and
    /// <paramref name="lengthB"/> items of the caller's buffers.
    /// </remarks>
    // Kept out of Compute: inlined there, a trigram count over UTF-16 units ran 20% slower.
    [MethodImpl(MethodImplOptions.NoInlining)]
    private static (int Intersection, int SizeA, int SizeB) CountSorted<T>(T[] textA, int lengthA, T[] textB, int lengthB, int qval)
        where T : IComparable<T>, IEquatable<T>
    {
        int sizeA = Math.Max(0, lengthA - qval + 1);
        int sizeB = Math.Max(0, lengthB - qval + 1);
        if (sizeA == 0 || sizeB == 0)
        {
            return (0, sizeA, sizeB);
        }

        int[] startsA = ArrayPool<int>.Shared.Rent(sizeA);
        int[] startsB = ArrayPool<int>.Shared.Rent(sizeB);
        try
        {
            SortStarts(textA, startsA, sizeA, qval);
            SortStarts(textB, startsB, sizeB, qval);

            int intersection = 0;
            int i = 0;
            int j = 0;
            while (i < sizeA && j < sizeB)
            {
                int order = textA.AsSpan(startsA[i], qval).SequenceCompareTo(textB.AsSpan(startsB[j], qval));
                if (order < 0)
                {
                    i++;
                }
                else if (order > 0)
                {
                    j++;
                }
                else
                {
                    int runA = RunLength(textA, startsA, i, sizeA, qval);
                    int runB = RunLength(textB, startsB, j, sizeB, qval);
                    intersection += Math.Min(runA, runB);
                    i += runA;
                    j += runB;
                }
            }

            return (intersection, sizeA, sizeB);
        }
        finally
        {
            ArrayPool<int>.Shared.Return(startsA);
            ArrayPool<int>.Shared.Return(startsB);
        }
    }

    private static void SortStarts<T>(T[] text, int[] starts, int count, int qval)
        where T : IComparable<T>
    {
        for (int i = 0; i < count; i++)
        {
            starts[i] = i;
        }

        Array.Sort(starts, 0, count, new GramOrder<T>(text, qval));
    }

    private static int RunLength<T>(T[] text, int[] starts, int at, int count, int qval)
        where T : IEquatable<T>
    {
        int end = at + 1;
        ReadOnlySpan<T> gram = text.AsSpan(starts[at], qval);
        while (end < count && text.AsSpan(starts[end], qval).SequenceEqual(gram))
        {
            end++;
        }

        return end - at;
    }

    /// <summary>Orders gram start positions by the grams' elements.</summary>
    private sealed class GramOrder<T>(T[] text, int qval) : IComparer<int>
        where T : IComparable<T>
    {
        public int Compare(int x, int y) => text.AsSpan(x, qval).SequenceCompareTo(text.AsSpan(y, qval));
    }
}

/// <summary>Jaccard similarity on character q-gram multisets: <c>|A∩B| / |A∪B|</c>.</summary>
/// <remarks>Reference: <c>textdistance.Jaccard</c> (default <c>qval = 1</c>). Inputs too short for one gram score 1 when equal, 0 otherwise.</remarks>
public static class Jaccard
{
    /// <summary>Computes the Jaccard similarity of <paramref name="a"/> and <paramref name="b"/>.</summary>
    /// <exception cref="ArgumentOutOfRangeException"><paramref name="qval"/> is below 1.</exception>
    public static double Similarity(ReadOnlySpan<char> a, ReadOnlySpan<char> b, int qval = 1, TextElement element = TextElement.Utf16Unit)
    {
        (int inter, int sizeA, int sizeB) = QgramCounts.Compute(a, b, qval, element);
        int union = sizeA + sizeB - inter;
        return union == 0 ? QgramCounts.NoGrams(a, b) : (double)inter / union;
    }
}

/// <summary>Sørensen-Dice similarity: <c>2·|A∩B| / (|A| + |B|)</c>.</summary>
/// <remarks>Reference: <c>textdistance.Sorensen</c> (default <c>qval = 1</c>). Inputs too short for one gram score 1 when equal, 0 otherwise.</remarks>
public static class SorensenDice
{
    /// <summary>Computes the Sørensen-Dice similarity of <paramref name="a"/> and <paramref name="b"/>.</summary>
    /// <exception cref="ArgumentOutOfRangeException"><paramref name="qval"/> is below 1.</exception>
    public static double Similarity(ReadOnlySpan<char> a, ReadOnlySpan<char> b, int qval = 1, TextElement element = TextElement.Utf16Unit)
    {
        (int inter, int sizeA, int sizeB) = QgramCounts.Compute(a, b, qval, element);
        int denom = sizeA + sizeB;
        return denom == 0 ? QgramCounts.NoGrams(a, b) : 2.0 * inter / denom;
    }
}

/// <summary>Overlap (Szymkiewicz-Simpson) coefficient: <c>|A∩B| / min(|A|, |B|)</c>.</summary>
/// <remarks>Reference: <c>textdistance.Overlap</c> (default <c>qval = 1</c>). Inputs too short for one gram score 1 when equal, 0 otherwise; one side without a gram gives 0.</remarks>
public static class Overlap
{
    /// <summary>Computes the overlap coefficient of <paramref name="a"/> and <paramref name="b"/>.</summary>
    /// <exception cref="ArgumentOutOfRangeException"><paramref name="qval"/> is below 1.</exception>
    public static double Similarity(ReadOnlySpan<char> a, ReadOnlySpan<char> b, int qval = 1, TextElement element = TextElement.Utf16Unit)
    {
        (int inter, int sizeA, int sizeB) = QgramCounts.Compute(a, b, qval, element);
        int denom = Math.Min(sizeA, sizeB);
        if (denom == 0)
        {
            return sizeA == 0 && sizeB == 0 ? QgramCounts.NoGrams(a, b) : 0.0;
        }
        return (double)inter / denom;
    }
}

/// <summary>Tversky index: <c>|A∩B| / (|A∩B| + α·|A\B| + β·|B\A|)</c>.</summary>
/// <remarks>Reference: <c>textdistance.Tversky</c> (default <c>qval = 1</c>, <c>α = β = 1</c>, which equals Jaccard).</remarks>
public static class Tversky
{
    /// <summary>Computes the Tversky index of <paramref name="a"/> and <paramref name="b"/>.</summary>
    /// <exception cref="ArgumentOutOfRangeException"><paramref name="qval"/> is below 1.</exception>
    public static double Similarity(
        ReadOnlySpan<char> a,
        ReadOnlySpan<char> b,
        double alpha = 1.0,
        double beta = 1.0,
        int qval = 1,
        TextElement element = TextElement.Utf16Unit)
    {
        // textdistance answers before weighing anything: equal inputs score 1, and an empty one 0.
        if (qval >= 1 && a.SequenceEqual(b))
        {
            return 1.0;
        }
        if (qval >= 1 && (a.IsEmpty || b.IsEmpty))
        {
            return 0.0;
        }

        (int inter, int sizeA, int sizeB) = QgramCounts.Compute(a, b, qval, element);
        double denom = inter + alpha * (sizeA - inter) + beta * (sizeB - inter);

        // SonarLint S1244: the guard is against dividing by zero, not against a
        // denominator that is merely small. Exact zero means no shared gram and no
        // weighted surplus — two differing inputs under zero weights, or too short for
        // a gram — where textdistance divides by zero; they share nothing, so 0.
#pragma warning disable S1244
        return denom == 0.0 ? 0.0 : inter / denom;
#pragma warning restore S1244
    }
}

/// <summary>Cosine (Ochiai) similarity on q-gram multisets: <c>|A∩B| / √(|A|·|B|)</c>.</summary>
/// <remarks>Reference: <c>textdistance.Cosine</c> (default <c>qval = 1</c>). Inputs too short for one gram score 1 when equal, 0 otherwise; one side without a gram gives 0.</remarks>
public static class Cosine
{
    /// <summary>Computes the cosine similarity of <paramref name="a"/> and <paramref name="b"/> over q-gram multisets.</summary>
    /// <exception cref="ArgumentOutOfRangeException"><paramref name="qval"/> is below 1.</exception>
    public static double Similarity(ReadOnlySpan<char> a, ReadOnlySpan<char> b, int qval = 1, TextElement element = TextElement.Utf16Unit)
    {
        (int inter, int sizeA, int sizeB) = QgramCounts.Compute(a, b, qval, element);
        if (sizeA == 0 || sizeB == 0)
        {
            return sizeA == 0 && sizeB == 0 ? QgramCounts.NoGrams(a, b) : 0.0;
        }
        return inter / Math.Sqrt((double)sizeA * sizeB);
    }
}
