using System;
using System.Collections;
using System.Collections.Generic;

namespace Lodestar.Internal;

/// <summary>
/// The comparisons a record needs when one of its members compares by reference, and the
/// O(1) hash contributions that stay consistent with them (decision 0113).
/// </summary>
/// <remarks>
/// Twelve records need these, and six wrote them independently before the rule was stated;
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

    /// <summary>Whether a member is present, as the only hash contribution a set may make.</summary>
    /// <remarks>
    /// A set member's count is unusable: <c>SameSet</c> makes <c>["the", "the"]</c> equal
    /// <c>["the"]</c> while the counts differ, and equal objects must hash alike.
    /// </remarks>
    public static int PresenceOf(object? value) => value is null ? -1 : 0;
}
