namespace Lodestar.Preprocessing.Internal;

/// <summary>The distinct values of each feature, in the order the reference puts them.</summary>
/// <remarks>
/// Sorted, because that is what decides the column layout of a one-hot encoding and the code of an
/// ordinal one. <strong>Strings sort by code point</strong>, which is numpy's order and not .NET's
/// default: measured, `['B', 'a', 'b', 'A']` gives `A, B, a, b` there and `a, A, b, B` under a
/// culture-sensitive comparison, and the two produce different columns for the same data.
/// </remarks>
internal static class CategoryTable
{
    /// <summary>The comparer the reference's order needs: ordinal for strings, the default otherwise.</summary>
    public static IComparer<T> Comparer<T>() =>
        typeof(T) == typeof(string) ? (IComparer<T>)StringComparer.Ordinal : System.Collections.Generic.Comparer<T>.Default;

    /// <summary>Each feature's distinct values, sorted.</summary>
    public static T[][] Build<T>(ReadOnlySpan<T> values, int featureCount, int sampleCount)
    {
        IComparer<T> comparer = Comparer<T>();
        var categories = new T[featureCount][];
        for (int feature = 0; feature < featureCount; feature++)
        {
            var column = new T[sampleCount];
            for (int row = 0; row < sampleCount; row++)
            {
                column[row] = values[(row * featureCount) + feature];
            }

            Array.Sort(column, comparer);
            categories[feature] = Distinct(column, comparer);
        }

        return categories;
    }

    /// <summary>Where a value sits in its feature's categories, or <c>-1</c> when it is not one of them.</summary>
    public static int IndexOf<T>(T[] categories, T value, IComparer<T> comparer) =>
        Array.BinarySearch(categories, value, comparer) is var found && found >= 0 ? found : -1;

    /// <summary>The runs of a sorted column, collapsed to one value each.</summary>
    private static T[] Distinct<T>(T[] sorted, IComparer<T> comparer)
    {
        if (sorted.Length == 0)
        {
            return [];
        }

        int distinct = 1;
        for (int i = 1; i < sorted.Length; i++)
        {
            if (comparer.Compare(sorted[i], sorted[i - 1]) != 0)
            {
                distinct++;
            }
        }

        var result = new T[distinct];
        result[0] = sorted[0];
        int next = 1;
        for (int i = 1; i < sorted.Length; i++)
        {
            if (comparer.Compare(sorted[i], sorted[i - 1]) != 0)
            {
                result[next++] = sorted[i];
            }
        }

        return result;
    }
}
