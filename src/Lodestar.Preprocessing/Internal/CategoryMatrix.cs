namespace Lodestar.Preprocessing.Internal;

/// <summary>The shape checks a categorical matrix needs, which <see cref="SampleMatrix"/> does for doubles.</summary>
internal static class CategoryMatrix
{
    /// <summary>The row count, with the two shapes that are not a matrix refused.</summary>
    public static int Rows<T>(ReadOnlySpan<T> values, int featureCount)
    {
        if (values.Length == 0 || values.Length % featureCount != 0)
        {
            throw new ArgumentException(
                $"values holds {values.Length} entries, which is not a positive whole number of "
                + $"rows of {featureCount}.",
                nameof(values));
        }

        return values.Length / featureCount;
    }

    /// <summary>Refuses a null category, which has no place in a sort and no code of its own.</summary>
    public static void RequireNoNull<T>(ReadOnlySpan<T> values)
    {
        for (int i = 0; i < values.Length; i++)
        {
            if (values[i] is null)
            {
                throw new ArgumentException(
                    $"values[{i}] is null. A category is a value; the reference has no null level either.",
                    nameof(values));
            }
        }
    }

    /// <summary>
    /// Refuses an infinite floating category, which the reference's <c>allow-nan</c> check refuses where it lets a
    /// NaN through; <c>LabelEncoder</c>, which checks nothing, does not call it (#1229).
    /// </summary>
    public static void RejectInfinity<T>(ReadOnlySpan<T> values)
    {
        if (CategoryTable.FloatingDtype<T>() is { } dtype)
        {
            foreach (T value in values)
            {
                if (CategoryTable.IsInfinity(value))
                {
                    throw new ArgumentException(
                        $"Input contains infinity or a value too large for dtype('{dtype}').", nameof(values));
                }
            }
        }
    }
}
