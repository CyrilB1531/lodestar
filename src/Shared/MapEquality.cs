using System.Collections.Generic;

namespace Lodestar.Internal;

/// <summary>Structural equality over two maps, for the records that hold one.</summary>
/// <remarks>
/// Apart from <see cref="ValueEquality"/> because <c>Lodestar.Abstractions</c> compiles that file in and holds no map
/// to compare: shared source it never calls would be code a data-type package carries for nothing (#1417, #1433).
/// </remarks>
internal static class MapEquality
{
    /// <summary>Whether two maps hold the same entries, each side looked up under the other's own comparer.</summary>
    /// <remarks>
    /// One direction answers under one comparer only: a case-insensitive map holding <c>"A"</c> finds <c>"a"</c> in an
    /// ordinal one and not the reverse, so a vocabulary's equality stopped being symmetric (#1433).
    /// </remarks>
    public static bool SameEntries<TKey, TValue>(IReadOnlyDictionary<TKey, TValue>? left, IReadOnlyDictionary<TKey, TValue>? right)
        where TKey : notnull
    {
        if (ReferenceEquals(left, right))
        {
            return true;
        }

        return left is not null && right is not null && left.Count == right.Count
            && EntriesFoundIn(left, right) && EntriesFoundIn(right, left);
    }

    private static bool EntriesFoundIn<TKey, TValue>(IReadOnlyDictionary<TKey, TValue> from, IReadOnlyDictionary<TKey, TValue> into)
        where TKey : notnull
    {
        EqualityComparer<TValue> values = EqualityComparer<TValue>.Default;
        foreach (KeyValuePair<TKey, TValue> entry in from)
        {
            if (!into.TryGetValue(entry.Key, out TValue? found) || !values.Equals(found, entry.Value))
            {
                return false;
            }
        }

        return true;
    }
}
