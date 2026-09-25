namespace Lodestar.Preprocessing.Internal;

/// <summary>scikit-learn's <c>_identify_infrequent</c> and its category mapping, one feature at a time.</summary>
/// <remarks>
/// A category is infrequent below <c>min_frequency</c>, as a count or as a share of the rows; past
/// <c>max_categories</c> the least frequent join them, ranked by a stable sort of the counts so ties keep the
/// sorted category order. The infrequent categories share the last column of their feature's block.
/// </remarks>
internal static class InfrequentGrouping
{
    /// <summary>Each category's grouped position — frequent ones first, in order, infrequent ones last — or null when none is infrequent.</summary>
    public static int[]? Map(int[] counts, int sampleCount, int? minFrequency, double? minFrequencyShare, int? maxCategories)
    {
        int n = counts.Length;
        var infrequent = new bool[n];
        for (int i = 0; i < n; i++)
        {
            infrequent[i] = (minFrequency is { } least && counts[i] < least)
                || (minFrequencyShare is { } share && counts[i] < sampleCount * share);
        }

        int current = n - infrequent.Count(flag => flag) + 1;
        if (maxCategories is { } most && most < current)
        {
            // max_categories counts the infrequent column: keep the most - 1 most frequent, stably.
            int keep = most - 1;
            int[] ascending = [.. Enumerable.Range(0, n).OrderBy(i => counts[i])];
            for (int k = 0; k < n - keep; k++)
            {
                infrequent[ascending[k]] = true;
            }
        }

        if (!infrequent.Contains(true))
        {
            return null;
        }

        int frequent = infrequent.Count(flag => !flag);
        var map = new int[n];
        int next = 0;
        for (int i = 0; i < n; i++)
        {
            map[i] = infrequent[i] ? frequent : next++;
        }

        return map;
    }
}
