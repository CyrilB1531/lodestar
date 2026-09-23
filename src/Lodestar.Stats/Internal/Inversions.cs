namespace Lodestar.Stats.Internal;

/// <summary>Counts the inversions of a sequence — the pairs it holds out of order.</summary>
/// <remarks>
/// Kendall's discordant-pair count is an inversion count once the pairs are ordered by the
/// first sample, so this is the difference between the test costing <c>O(n log n)</c> and
/// costing <c>O(n²)</c>: at ten thousand pairs the quadratic count is fifty million
/// comparisons against a merge sort's hundred and thirty thousand. scipy reaches the same
/// order through a Fenwick tree; a merge sort answers the same question with one scratch
/// buffer and no tree.
/// </remarks>
internal static class Inversions
{
    /// <summary>The number of pairs <c>i &lt; j</c> with <c>values[i] &gt; values[j]</c>.</summary>
    /// <remarks>
    /// The first <paramref name="length"/> entries are sorted in place, which every caller here
    /// is done with by the time it asks; a length of its own because the callers hand over
    /// rented buffers, which are longer than they asked for.
    /// <paramref name="scratch"/> is the merge's second buffer and must be at least as long.
    /// </remarks>
    internal static long Count(int[] values, int length, int[] scratch)
    {
        return Sort(values, scratch, 0, length);
    }

    private static long Sort(int[] values, int[] scratch, int start, int length)
    {
        if (length < 2)
        {
            return 0L;
        }

        int half = length / 2;
        long count = Sort(values, scratch, start, half)
            + Sort(values, scratch, start + half, length - half);

        int left = start;
        int leftEnd = start + half;
        int right = leftEnd;
        int rightEnd = start + length;
        int output = start;

        while (left < leftEnd && right < rightEnd)
        {
            if (values[left] <= values[right])
            {
                scratch[output++] = values[left++];
            }
            else
            {
                // Everything still queued on the left is greater than this right-hand value
                // and sits before it, so one comparison settles that whole block of pairs.
                count += leftEnd - left;
                scratch[output++] = values[right++];
            }
        }

        while (left < leftEnd)
        {
            scratch[output++] = values[left++];
        }
        while (right < rightEnd)
        {
            scratch[output++] = values[right++];
        }

        Array.Copy(scratch, start, values, start, length);
        return count;
    }
}
