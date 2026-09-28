using System;
using System.Collections;
using System.Collections.Generic;

namespace Lodestar.Internal;

/// <summary>
/// The comparisons a record needs when one of its members compares by reference, and the
/// O(1) hash contributions that stay consistent with them.
/// </summary>
/// <remarks>
/// Fourteen records need these, and six wrote them independently before the rule was stated;
/// every method is total on null, since a record's equality must answer for an absent member.
/// </remarks>
internal static class ValueEquality
{
    /// <summary>Whether two jagged tables hold the same values, row by row.</summary>
    /// <remarks>
    /// Comparing the outer length alone would pass two tables that agree on their row count and
    /// differ inside a row, which is the shape a contingency table takes.
    /// </remarks>
    public static bool Same(double[][]? left, double[][]? right)
    {
        if (ReferenceEquals(left, right))
        {
            return true;
        }
        if (left is null || right is null || left.Length != right.Length)
        {
            return false;
        }
        for (int i = 0; i < left.Length; i++)
        {
            if (!Same(left[i], right[i]))
            {
                return false;
            }
        }
        return true;
    }

    /// <summary>Whether two lists of blocks hold the same blocks, in order and value by value.</summary>
    public static bool Same(IReadOnlyList<double[]>? left, IReadOnlyList<double[]>? right)
    {
        if (ReferenceEquals(left, right))
        {
            return true;
        }
        if (left is null || right is null || left.Count != right.Count)
        {
            return false;
        }
        for (int i = 0; i < left.Count; i++)
        {
            if (!Same(left[i], right[i]))
            {
                return false;
            }
        }
        return true;
    }

    /// <summary>Whether two arrays hold equal elements, in order.</summary>
    /// <remarks>
    /// <c>EqualityComparer&lt;T&gt;.Default</c> reaches <c>IEquatable&lt;T&gt;.Equals</c>, which for
    /// <c>double</c> makes <c>NaN</c> equal <c>NaN</c> and <c>+0.0</c> equal <c>-0.0</c> — the
    /// first is what keeps a record's equality reflexive. The constraint keeps this off any
    /// element type whose own equality is by reference.
    /// </remarks>
    public static bool Same<T>(T[]? left, T[]? right)
        where T : IEquatable<T>
    {
        if (ReferenceEquals(left, right))
        {
            return true;
        }
        if (left is null || right is null || left.Length != right.Length)
        {
            return false;
        }
        for (int i = 0; i < left.Length; i++)
        {
            if (!EqualityComparer<T>.Default.Equals(left[i], right[i]))
            {
                return false;
            }
        }
        return true;
    }

    /// <summary>Whether two lists hold equal elements, in order.</summary>
    /// <remarks>The list form of <see cref="Same{T}(T[], T[])"/>, for a member typed as an interface (#1284).</remarks>
    public static bool Same<T>(IReadOnlyList<T>? left, IReadOnlyList<T>? right)
        where T : IEquatable<T>
    {
        if (ReferenceEquals(left, right))
        {
            return true;
        }
        if (left is null || right is null || left.Count != right.Count)
        {
            return false;
        }
        for (int i = 0; i < left.Count; i++)
        {
            if (!EqualityComparer<T>.Default.Equals(left[i], right[i]))
            {
                return false;
            }
        }
        return true;
    }

    /// <summary>Whether two collections hold the same words, order and repetition aside.</summary>
    public static bool SameSet(IReadOnlyCollection<string>? left, IReadOnlyCollection<string>? right)
    {
        if (ReferenceEquals(left, right))
        {
            return true;
        }
        if (left is null || right is null)
        {
            return false;
        }
        HashSet<string> mine = new(left, StringComparer.Ordinal);
        return mine.SetEquals(right);
    }

    /// <summary>A sequence's length, or <c>-1</c> for an absent one, as a hash contribution.</summary>
    /// <remarks>
    /// <c>-1</c> rather than <c>0</c> so an absent member and an empty one do not collide, which
    /// they must not: <c>Same</c> reports them unequal.
    /// </remarks>
    public static int CountOf(ICollection? collection) => collection?.Count ?? -1;

    /// <summary>A list's length, or <c>-1</c> for an absent one, as <see cref="CountOf(ICollection)"/> gives an array's.</summary>
    public static int LengthOf<T>(IReadOnlyList<T>? list) => list?.Count ?? -1;

    /// <summary>A double's hash, with every <c>NaN</c> hashed alike.</summary>
    /// <remarks>
    /// <c>double.Equals</c> makes every <c>NaN</c> equal, and .NET Framework's <c>GetHashCode</c>
    /// hashes their payloads apart, so equal records would hash apart there (#1285).
    /// </remarks>
    public static int HashOf(double value) => double.IsNaN(value) ? 0 : value.GetHashCode();

    /// <summary>A float's hash, with every <c>NaN</c> hashed alike, as <see cref="HashOf(double)"/> (#1294).</summary>
    public static int HashOf(float value) => float.IsNaN(value) ? 0 : value.GetHashCode();

    /// <summary>An optional double's hash, <c>-1</c> when absent and every <c>NaN</c> alike.</summary>
    public static int HashOf(double? value) => value is { } present ? HashOf(present) : -1;

    /// <summary>Any other member's hash, as the generated record equality's comparer gives it.</summary>
    /// <remarks>For a record that keeps its generated equality and hashes its doubles through <see cref="HashOf(double)"/> (#1285).</remarks>
    public static int HashOfItem<T>(T value) => value is null ? 0 : EqualityComparer<T>.Default.GetHashCode(value);

    /// <summary>Whether a member is present, as the only hash contribution a set may make.</summary>
    /// <remarks>
    /// A set member's count is unusable: <c>SameSet</c> makes <c>["the", "the"]</c> equal
    /// <c>["the"]</c> while the counts differ, and equal objects must hash alike.
    /// </remarks>
    public static int PresenceOf(object? value) => value is null ? -1 : 0;
}
