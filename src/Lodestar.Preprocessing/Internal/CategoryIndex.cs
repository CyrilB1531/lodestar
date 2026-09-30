namespace Lodestar.Preprocessing.Internal;

/// <summary>Each feature's category positions, looked up the fastest way that finds the same one.</summary>
/// <remarks>
/// A hash for strings and the integral types, whose equality is exactly a comparison of zero, so a
/// hit is the position the binary search found. Any other type keeps the search, with its comparer
/// resolved once: a floating zero's sign or a caller's own type can compare equal without hashing
/// equal.
/// </remarks>
// CS8714: the encoders' T cannot take a notnull constraint without changing their public
// signature, and CategoryMatrix.RequireNoNull refuses a null before any value is fitted or looked up.
#pragma warning disable CS8714
internal sealed class CategoryIndex<T>
{
    private readonly T[][] _categories;
    private readonly IComparer<T> _comparer;
    private readonly Dictionary<T, int>[]? _positions;
    private readonly bool _holdsNaN;

    public CategoryIndex(T[][] categories)
    {
        _categories = categories;
        _holdsNaN = HoldsNaNCategory(categories);

        // Without a NaN category the default order is numpy's, so main's comparer serves; the NaN-last order only where
        // a NaN was fitted (#1229). A double or float takes CategoryTable.IndexOf's typed search either way (#1230).
        _comparer = typeof(T) == typeof(string) || _holdsNaN
            ? CategoryTable.Comparer<T>()
            : System.Collections.Generic.Comparer<T>.Default;
        if (HashesAsItSorts())
        {
            _positions = Positions(categories);
        }
    }

    // A NaN sorts last: one at the end of a column is the only place one can be.
    private static bool HoldsNaNCategory(T[][] categories) =>
        categories.Any(column => column.Length > 0 && CategoryTable.IsNaN(column[^1]));

    private static Dictionary<T, int>[] Positions(T[][] categories)
    {
        IEqualityComparer<T> equality = typeof(T) == typeof(string)
            ? (IEqualityComparer<T>)StringComparer.Ordinal
            : EqualityComparer<T>.Default;
        var positions = new Dictionary<T, int>[categories.Length];
        for (int feature = 0; feature < categories.Length; feature++)
        {
            var held = new Dictionary<T, int>(categories[feature].Length, equality);
            for (int index = 0; index < categories[feature].Length; index++)
            {
                held.Add(categories[feature][index], index);
            }

            positions[feature] = held;
        }

        return positions;
    }

    /// <summary>Whether some feature holds a NaN category, which the ordinal encoder codes NaN (#1229).</summary>
    public bool HoldsNaN => _holdsNaN;

    /// <summary>Where a value sits in a feature's categories, or <c>-1</c> when it is not one of them.</summary>
    public int IndexOf(int feature, T value)
    {
        if (_positions is null)
        {
            return CategoryTable.IndexOf(_categories[feature], value, _comparer);
        }

        return _positions[feature].TryGetValue(value, out int index) ? index : -1;
    }

    private static bool HashesAsItSorts() =>
        typeof(T) == typeof(string) || typeof(T) == typeof(int) || typeof(T) == typeof(long)
        || typeof(T) == typeof(short) || typeof(T) == typeof(sbyte) || typeof(T) == typeof(byte)
        || typeof(T) == typeof(ushort) || typeof(T) == typeof(uint) || typeof(T) == typeof(ulong)
        || typeof(T) == typeof(char) || typeof(T) == typeof(bool);
}
#pragma warning restore CS8714
