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

    public CategoryIndex(T[][] categories)
    {
        _categories = categories;
        _comparer = CategoryTable.Comparer<T>();
        if (!HashesAsItSorts())
        {
            return;
        }

        IEqualityComparer<T> equality = typeof(T) == typeof(string)
            ? (IEqualityComparer<T>)StringComparer.Ordinal
            : EqualityComparer<T>.Default;
        _positions = new Dictionary<T, int>[categories.Length];
        for (int feature = 0; feature < categories.Length; feature++)
        {
            var positions = new Dictionary<T, int>(categories[feature].Length, equality);
            for (int index = 0; index < categories[feature].Length; index++)
            {
                positions.Add(categories[feature][index], index);
            }

            _positions[feature] = positions;
        }
    }

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
