using System;

namespace Lodestar.Stats.Internal;

/// <summary>Applies a <see cref="NanPolicy"/> to a sample before a test sees it.</summary>
/// <remarks>
/// Decision 0007: omission is a filter, so a family's own guards run afterwards on what
/// survives. Nothing here decides what a degenerate sample means.
/// </remarks>
internal static class NanFilter
{
    /// <summary>One sample, filtered independently of any other; the sample itself where there is nothing to filter.</summary>
    public static ReadOnlySpan<double> Apply(ReadOnlySpan<double> sample, NanPolicy policy, string paramName) =>
        Filtered(sample, policy, paramName) is { } kept ? kept : sample;

    /// <summary>The sample with its NaN values dropped under Omit, or null where it is kept as it is; refused under Raise.</summary>
    /// <remarks>A sample free of NaN is scanned once and never copied; from the first NaN, one pass counts and one fills.</remarks>
    private static double[]? Filtered(ReadOnlySpan<double> sample, NanPolicy policy, string paramName)
    {
        if (policy == NanPolicy.Raise)
        {
            Refuse(sample, paramName);
            return null;
        }
        // An undefined policy omits, as every one but Propagate and Raise always has, so the input meets its own refusals first.
        if (policy == NanPolicy.Propagate)
        {
            return null;
        }

        int first = 0;
        while (first < sample.Length && !double.IsNaN(sample[first]))
        {
            first++;
        }
        if (first == sample.Length)
        {
            return null;
        }

        // Counted, then filled at that size, as ApplyAligned is: a list would copy what it keeps twice.
        int count = first;
        for (int i = first + 1; i < sample.Length; i++)
        {
            count += double.IsNaN(sample[i]) ? 0 : 1;
        }
        double[] kept = new double[count];
        sample.Slice(0, first).CopyTo(kept);
        for (int i = first + 1, at = first; i < sample.Length; i++)
        {
            if (!double.IsNaN(sample[i]))
            {
                kept[at++] = sample[i];
            }
        }
        return kept;
    }

    /// <summary>Two aligned samples, filtered by index so a pair survives or neither does.</summary>
    /// <remarks>
    /// Samples of different lengths come back untouched, so the family's own length guard produces its usual message
    /// rather than this one inventing a second; so do both under <see cref="NanPolicy.Raise"/>, which filters nothing.
    /// </remarks>
    public static void ApplyAligned(
        ReadOnlySpan<double> left,
        ReadOnlySpan<double> right,
        NanPolicy policy,
        string leftName,
        string rightName,
        out ReadOnlySpan<double> keptLeft,
        out ReadOnlySpan<double> keptRight)
    {
        keptLeft = left;
        keptRight = right;
        if (policy == NanPolicy.Raise)
        {
            Refuse(left, leftName);
            Refuse(right, rightName);
            return;
        }
        // Different lengths are the family's to refuse.
        if (left.Length != right.Length)
        {
            return;
        }

        // One pass to count the pairs kept, one to fill arrays of that size.
        int count = 0;
        for (int i = 0; i < left.Length; i++)
        {
            count += double.IsNaN(left[i]) || double.IsNaN(right[i]) ? 0 : 1;
        }
        // Samples free of NaN have nothing to omit, and are handed on as they are.
        if (count == left.Length)
        {
            return;
        }
        double[] filteredLeft = new double[count];
        double[] filteredRight = new double[count];
        for (int i = 0, at = 0; i < left.Length; i++)
        {
            if (!double.IsNaN(left[i]) && !double.IsNaN(right[i]))
            {
                filteredLeft[at] = left[i];
                filteredRight[at++] = right[i];
            }
        }
        keptLeft = filteredLeft;
        keptRight = filteredRight;
    }

    /// <summary>Every group, each filtered independently of the others.</summary>
    public static double[][] ApplyGroups(double[][] groups, NanPolicy policy, string paramName)
    {
        // Propagate hands the groups on as given, null ones included, for the test's own empty-group rule to read.
        if (policy == NanPolicy.Propagate)
        {
            return groups;
        }
        double[][] filtered = new double[groups.Length][];
        for (int i = 0; i < groups.Length; i++)
        {
            // The group itself where omission filters nothing, a null one as empty; Omit's filtered array otherwise.
            double[] group = groups[i] ?? [];
            filtered[i] = Filtered(group, policy, paramName) ?? group;
        }
        return filtered;
    }

    private static void Refuse(ReadOnlySpan<double> sample, string paramName)
    {
        foreach (double value in sample)
        {
            if (double.IsNaN(value))
            {
                throw new ArgumentException(
                    "The sample holds a NaN and NanPolicy.Raise was requested.", paramName);
            }
        }
    }
}
