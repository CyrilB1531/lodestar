using System;
using System.Collections.Generic;

namespace Lodestar.Stats.Internal;

/// <summary>Applies a <see cref="NanPolicy"/> to a sample before a test sees it.</summary>
/// <remarks>
/// Decision 0117: omission is a filter, so a family's own guards run afterwards on what
/// survives. Nothing here decides what a degenerate sample means.
/// </remarks>
internal static class NanFilter
{
    /// <summary>One sample, filtered independently of any other.</summary>
    public static double[] Apply(ReadOnlySpan<double> sample, NanPolicy policy, string paramName)
    {
        if (policy == NanPolicy.Raise)
        {
            Refuse(sample, paramName);
            return sample.ToArray();
        }

        List<double> kept = new(sample.Length);
        foreach (double value in sample)
        {
            if (!double.IsNaN(value))
            {
                kept.Add(value);
            }
        }
        return kept.ToArray();
    }

    /// <summary>Two aligned samples, filtered by index so a pair survives or neither does.</summary>
    /// <remarks>
    /// Samples of different lengths are returned untouched, so the family's own length guard
    /// produces its usual message rather than this one inventing a second.
    /// </remarks>
    public static (double[] Left, double[] Right) ApplyAligned(
        ReadOnlySpan<double> left,
        ReadOnlySpan<double> right,
        NanPolicy policy,
        string leftName,
        string rightName)
    {
        if (policy == NanPolicy.Raise)
        {
            Refuse(left, leftName);
            Refuse(right, rightName);
            return (left.ToArray(), right.ToArray());
        }
        if (left.Length != right.Length)
        {
            return (left.ToArray(), right.ToArray());
        }

        List<double> keptLeft = new(left.Length);
        List<double> keptRight = new(right.Length);
        for (int i = 0; i < left.Length; i++)
        {
            if (!double.IsNaN(left[i]) && !double.IsNaN(right[i]))
            {
                keptLeft.Add(left[i]);
                keptRight.Add(right[i]);
            }
        }
        return (keptLeft.ToArray(), keptRight.ToArray());
    }

    /// <summary>Every group, each filtered independently of the others.</summary>
    public static double[][] ApplyGroups(double[][] groups, NanPolicy policy, string paramName)
    {
        double[][] filtered = new double[groups.Length][];
        for (int i = 0; i < groups.Length; i++)
        {
            filtered[i] = Apply(groups[i], policy, paramName);
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
