using System.Buffers;
using Lodestar.Text.Internal;

namespace Lodestar.Text.Distances;

/// <summary>
/// Ratcliff-Obershelp similarity (Gestalt pattern matching): <c>2·M / T</c> over
/// the longest common substring, recursively paired.
/// </summary>
/// <remarks>
/// Reference behavior: <c>difflib.SequenceMatcher(None, a, b).ratio()</c>;
/// <c>autojunk</c> is not replicated — see
/// <c>docs/decisions/0006-ratcliff-autojunk.md</c>. See <see cref="TextElement"/>
/// for the UTF-16 vs code-point choice.
/// </remarks>
public static class RatcliffObershelp
{
    /// <summary>Computes the Ratcliff-Obershelp similarity of <paramref name="a"/> and <paramref name="b"/>.</summary>
    public static double Similarity(ReadOnlySpan<char> a, ReadOnlySpan<char> b, TextElement element = TextElement.Utf16Unit)
    {
        if (element == TextElement.CodePoint)
        {
            return SimilarityCodePoints(a, b);
        }

        int total = a.Length + b.Length;
        return total == 0 ? 1.0 : 2.0 * MatchLength<char>(a, b) / total;
    }

    /// <summary>Ratcliff-Obershelp distance: <c>1 - Similarity</c>.</summary>
    public static double Distance(ReadOnlySpan<char> a, ReadOnlySpan<char> b, TextElement element = TextElement.Utf16Unit)
    {
        return 1.0 - Similarity(a, b, element);
    }

    /// <summary>The total matched length (M) over any sequence of equatable elements.</summary>
    /// <remarks>
    /// Walks the unmatched ranges from an explicit stack in difflib's
    /// <c>get_matching_blocks</c> order, so the depth is not call stack:
    /// a chain of one-element blocks as long as the input used to overflow the thread (#877).
    /// </remarks>
    internal static int MatchLength<T>(ReadOnlySpan<T> a, ReadOnlySpan<T> b)
        where T : IEquatable<T>
    {
        // Pending ranges are disjoint and non-empty in both strings, so at most min(|a|, |b|) + 1 wait at
        // once; four ints each, pooled, since a Stack<T> per call undid #844's allocation-free kernel (#980).
        int capacity = 4 * (Math.Min(a.Length, b.Length) + 1);
        int[]? rented = capacity <= 256 ? null : ArrayPool<int>.Shared.Rent(capacity);
        Span<int> pending = rented is null ? stackalloc int[256] : rented;
        try
        {
            pending[0] = 0;
            pending[1] = a.Length;
            pending[2] = 0;
            pending[3] = b.Length;
            int top = 4;
            int total = 0;
            while (top > 0)
            {
                top -= 4;
                int aLo = pending[top];
                int aHi = pending[top + 1];
                int bLo = pending[top + 2];
                int bHi = pending[top + 3];
                LongestMatch(a[aLo..aHi], b[bLo..bHi], out int i, out int j, out int size);
                if (size == 0)
                {
                    continue;
                }
                total += size;
                i += aLo;
                j += bLo;
                if (aLo < i && bLo < j)
                {
                    Push(pending, ref top, aLo, i, bLo, j);
                }
                if (i + size < aHi && j + size < bHi)
                {
                    Push(pending, ref top, i + size, aHi, j + size, bHi);
                }
            }
            return total;
        }
        finally
        {
            if (rented is not null)
            {
                ArrayPool<int>.Shared.Return(rented);
            }
        }
    }

    private static void Push(Span<int> pending, ref int top, int aLo, int aHi, int bLo, int bHi)
    {
        pending[top] = aLo;
        pending[top + 1] = aHi;
        pending[top + 2] = bLo;
        pending[top + 3] = bHi;
        top += 4;
    }

    /// <summary>
    /// Finds the longest contiguous matching block, breaking ties like difflib:
    /// earliest start in <paramref name="a"/>, then earliest start in <paramref name="b"/>.
    /// </summary>
    private static void LongestMatch<T>(ReadOnlySpan<T> a, ReadOnlySpan<T> b, out int bestI, out int bestJ, out int bestSize)
        where T : IEquatable<T>
    {
        bestI = 0;
        bestJ = 0;
        bestSize = 0;
        if (a.Length == 0 || b.Length == 0)
        {
            return;
        }

        int width = b.Length;
        int longest = Math.Min(a.Length, width);
        int[] rented = ArrayPool<int>.Shared.Rent(width);
        try
        {
            Span<int> previous = rented.AsSpan(0, width);
            previous.Clear();
            for (int i = 0; i < a.Length; i++)
            {
                T ai = a[i];
                int diagonal = 0; // previous[j-1] value from before this row overwrote it
                for (int j = 0; j < width; j++)
                {
                    int here = previous[j];
                    int run = ai.Equals(b[j]) ? diagonal + 1 : 0;
                    previous[j] = run;
                    // Strictly-greater keeps the earliest end (hence earliest start in a,
                    // then earliest in b) — difflib's tie-break.
                    if (run > bestSize)
                    {
                        bestSize = run;
                        bestI = i - run + 1;
                        bestJ = j - run + 1;
                        if (run == longest)
                        {
                            // No later run can be strictly longer, so the block is final.
                            return;
                        }
                    }
                    diagonal = here;
                }
            }
        }
        finally
        {
            ArrayPool<int>.Shared.Return(rented);
        }
    }

    private static double SimilarityCodePoints(ReadOnlySpan<char> a, ReadOnlySpan<char> b)
    {
        int[] bufA = ArrayPool<int>.Shared.Rent(Math.Max(1, a.Length));
        int[] bufB = ArrayPool<int>.Shared.Rent(Math.Max(1, b.Length));
        try
        {
            int lenA = CodePoints.Decode(a, bufA);
            int lenB = CodePoints.Decode(b, bufB);
            int total = lenA + lenB;
            return total == 0
                ? 1.0
                : 2.0 * MatchLength<int>(bufA.AsSpan(0, lenA), bufB.AsSpan(0, lenB)) / total;
        }
        finally
        {
            ArrayPool<int>.Shared.Return(bufA);
            ArrayPool<int>.Shared.Return(bufB);
        }
    }
}
