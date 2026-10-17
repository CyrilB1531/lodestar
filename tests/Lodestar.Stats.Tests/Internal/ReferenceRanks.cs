namespace Lodestar.Stats.Tests.Internal;

/// <summary>The tie-correction term and the tie test by a sort of their own, the reference the rankers' shared walk is held to.</summary>
/// <remarks>Production code reads both off its own ranking; these lived in Ranks with no caller but the tests (#1702's Review B).</remarks>
internal static class ReferenceRanks
{
    internal static double TieCorrection(ReadOnlySpan<double> values)
    {
        if (values.Length == 0)
        {
            return 0.0;
        }

        double[] sorted = values.ToArray();
        Array.Sort(sorted);

        double correction = 0.0;
        int start = 0;
        while (start < sorted.Length)
        {
            int end = start;

            // S1244: same reasoning as Ranks.Average -- a tie group is defined by
            // literally equal input values.
#pragma warning disable S1244
            while (end + 1 < sorted.Length && sorted[end + 1] == sorted[start])
#pragma warning restore S1244
            {
                end++;
            }

            double t = end - start + 1;
            correction += (t * t * t) - t;
            start = end + 1;
        }

        return correction;
    }

    internal static bool HasTies(ReadOnlySpan<double> values)
    {
        if (values.Length < 2)
        {
            return false;
        }

        double[] sorted = values.ToArray();
        Array.Sort(sorted);

        // S1244: same reasoning as Ranks.Average -- a tie is defined by literally
        // equal input values.
#pragma warning disable S1244
        for (int i = 1; i < sorted.Length; i++)
        {
            if (sorted[i] == sorted[i - 1])
            {
                return true;
            }
        }
#pragma warning restore S1244

        return false;
    }
}
