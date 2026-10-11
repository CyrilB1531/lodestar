namespace Lodestar.Preprocessing.Internal;

/// <summary>The distinct values of each feature, in the order the reference puts them.</summary>
/// <remarks>
/// Sorted, because that is what decides the column layout of a one-hot encoding and the code of an
/// ordinal one. <strong>Strings sort by code point</strong>, which is numpy's order and not .NET's
/// default: measured, `['B', 'a', 'b', 'A']` gives `A, B, a, b` there and `a, A, b, B` under a
/// culture-sensitive comparison; and by code point rather than by UTF-16 unit, or a character past
/// the BMP sorts below U+E000 to U+FFFF (#1254). <strong>A floating NaN sorts last</strong>, as
/// <c>np.unique</c> puts it, where .NET's default comparer puts it first (#1229).
/// </remarks>
internal static class CategoryTable
{
    /// <summary>The comparer the reference's order needs: code points for strings, NaN last for every floating type, the default otherwise.</summary>
    public static IComparer<T> Comparer<T>()
    {
        if (typeof(T) == typeof(string))
        {
            return (IComparer<T>)(object)CodePointOrder.Instance;
        }

        // double and float, the types an encoder meets most, compare directly; Half through its IsNaN.
        if (typeof(T) == typeof(double))
        {
            return (IComparer<T>)(object)NanLastDouble;
        }

        if (typeof(T) == typeof(float))
        {
            return (IComparer<T>)(object)NanLastSingle;
        }

        return NanTest<T>.Order;
    }

    private static readonly IComparer<double> NanLastDouble = System.Collections.Generic.Comparer<double>.Create(CompareNanLast);

    private static readonly IComparer<float> NanLastSingle = System.Collections.Generic.Comparer<float>.Create(
        (x, y) => CompareNanLast(x, y));

    /// <summary>numpy's floating order: ascending, every NaN equal to every other and above every number.</summary>
    private static int CompareNanLast(double x, double y)
    {
        if (double.IsNaN(x))
        {
            return double.IsNaN(y) ? 0 : 1;
        }

        return double.IsNaN(y) ? -1 : x.CompareTo(y);
    }

    /// <summary>Whether a category is a floating NaN, which the ordinal encoder codes as NaN (#1229).</summary>
    public static bool IsNaN<T>(T value)
    {
        if (typeof(T) == typeof(double))
        {
            return double.IsNaN((double)(object)value!);
        }

        return typeof(T) == typeof(float) ? float.IsNaN((float)(object)value!) : NanTest<T>.Predicate?.Invoke(value) ?? false;
    }

    /// <summary>Whether a floating category is infinite, double, float and Half alike.</summary>
    public static bool IsInfinity<T>(T value)
    {
        if (typeof(T) == typeof(double))
        {
            return double.IsInfinity((double)(object)value!);
        }

        return typeof(T) == typeof(float) ? float.IsInfinity((float)(object)value!) : NanTest<T>.Infinite?.Invoke(value) ?? false;
    }

    /// <summary>numpy's dtype name for a floating category type, or null for any other.</summary>
    public static string? FloatingDtype<T>()
    {
        if (typeof(T) == typeof(double))
        {
            return "float64";
        }

        if (typeof(T) == typeof(float))
        {
            return "float32";
        }

        return typeof(T).FullName == HalfName ? "float16" : null;
    }

    private const string HalfName = "System.Half";

    // Code-point order differs from ordinal only past a surrogate: a column of strings with none sorts ordinally, the
    // runtime's own comparison, where code points for every string made a string encoder's fit 40% slower (#1254).
    private static IComparer<T> ColumnOrder<T>(T[] column, IComparer<T> comparer)
    {
        if (typeof(T) != typeof(string))
        {
            return comparer;
        }

        return ((string[])(object)column).Any(HoldsSurrogate) ? comparer : (IComparer<T>)StringComparer.Ordinal;
    }

    private static bool HoldsSurrogate(string text)
    {
        for (int i = 0; i < text.Length; i++)
        {
            if (char.IsSurrogate(text[i]))
            {
                return true;
            }
        }

        return false;
    }

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

            IComparer<T> order = ColumnOrder(column, comparer);
            Sort(column, order);
            categories[feature] = Distinct(column, order);
        }

        return categories;
    }

    /// <summary>
    /// Sorts a column in the reference's order. A double or float column takes the default sort, which puts NaN
    /// first, then moves its NaNs last: the delegate comparer doubled the sort time, measured at 2M values.
    /// </summary>
    private static void Sort<T>(T[] column, IComparer<T> comparer)
    {
        if (column is double[] doubles)
        {
            Array.Sort(doubles);
            MoveNaNsLast(doubles, double.IsNaN);
        }
        else if (column is float[] singles)
        {
            Array.Sort(singles);
            MoveNaNsLast(singles, float.IsNaN);
        }
        else
        {
            Array.Sort(column, comparer);
        }
    }

    /// <summary>Rotates the NaNs a default sort put first to the end, keeping the numbers' order.</summary>
    private static void MoveNaNsLast<T>(T[] sorted, Func<T, bool> isNaN)
    {
        int nans = 0;
        while (nans < sorted.Length && isNaN(sorted[nans]))
        {
            nans++;
        }

        if (nans == 0 || nans == sorted.Length)
        {
            return;
        }

        var leading = new T[nans];
        Array.Copy(sorted, leading, nans);
        Array.Copy(sorted, nans, sorted, 0, sorted.Length - nans);
        Array.Copy(leading, 0, sorted, sorted.Length - nans, nans);
    }

    /// <summary>Where a value sits in its feature's categories, or <c>-1</c> when it is not one of them.</summary>
    public static int IndexOf<T>(T[] categories, T value, IComparer<T> comparer)
    {
        // A double or float through the default comparer, the runtime's own search, past a trailing NaN: a comparer
        // delegate per probe made the encoders' transform half as slow again as main's (#1230).
        if (typeof(T) == typeof(double))
        {
            double[] numbers = (double[])(object)categories;
            return IndexOfNumber(numbers, NumberCount(numbers), (double)(object)value!);
        }

        if (typeof(T) == typeof(float))
        {
            return IndexOfNumber((float[])(object)categories, (float)(object)value!);
        }

        return Array.BinarySearch(categories, value, comparer) is var found && found >= 0 ? found : -1;
    }

    /// <summary>How many of a column sorted NaN last are numbers: all but a NaN at its end, held at most once.</summary>
    internal static int NumberCount(double[] categories) =>
        categories.Length > 0 && double.IsNaN(categories[^1]) ? categories.Length - 1 : categories.Length;

    /// <summary>A value's position in a column sorted NaN last, its numbers in the default order before the NaN.</summary>
    internal static int IndexOfNumber(double[] categories, int numbers, double value)
    {
        if (double.IsNaN(value))
        {
            return numbers < categories.Length ? numbers : -1;
        }

        return Array.BinarySearch(categories, 0, numbers, value) is var found && found >= 0 ? found : -1;
    }

    /// <summary><see cref="NumberCount(double[])"/> for a <see langword="float"/> column.</summary>
    private static int NumberCount(float[] categories) =>
        categories.Length > 0 && float.IsNaN(categories[^1]) ? categories.Length - 1 : categories.Length;

    private static int IndexOfNumber(float[] categories, float value)
    {
        int numbers = NumberCount(categories);
        if (float.IsNaN(value))
        {
            return numbers < categories.Length ? numbers : -1;
        }

        return Array.BinarySearch(categories, 0, numbers, value) is var found && found >= 0 ? found : -1;
    }

    /// <summary>
    /// Whether two neighbours of a sorted column are one category. A double or float compares by its own equality,
    /// every NaN equal and -0 equal to 0 as the comparer has them, without a delegate call per pair.
    /// </summary>
    private static bool Same<T>(T a, T b, IComparer<T> comparer) =>
        typeof(T) == typeof(double) || typeof(T) == typeof(float)
            ? EqualityComparer<T>.Default.Equals(a, b)
            : comparer.Compare(a, b) == 0;

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
            if (!Same(sorted[i], sorted[i - 1], comparer))
            {
                distinct++;
            }
        }

        var result = new T[distinct];
        result[0] = sorted[0];
        int next = 1;
        for (int i = 1; i < sorted.Length; i++)
        {
            if (!Same(sorted[i], sorted[i - 1], comparer))
            {
                result[next++] = sorted[i];
            }
        }

        return result;
    }

    /// <summary>
    /// A type's own static <c>IsNaN</c>, found once: <see cref="double"/>, <see cref="float"/> and <c>Half</c> alike,
    /// on either target — a netstandard2.0 build run on .NET 5 or later still meets <c>Half</c> — or none.
    /// </summary>
    private static class NanTest<T>
    {
        public static readonly Func<T, bool>? Predicate = Floating("IsNaN");

        /// <summary>The type's own <c>IsInfinity</c>, for the floating types alone.</summary>
        public static readonly Func<T, bool>? Infinite = Floating("IsInfinity");

        /// <summary>The default order with NaN last where <see cref="Predicate"/> finds one, built once per type.</summary>
        public static readonly System.Collections.Generic.Comparer<T> Order = OrderWithNanLast();

        private static System.Collections.Generic.Comparer<T> OrderWithNanLast()
        {
            Func<T, bool>? isNaN = Predicate;
            System.Collections.Generic.Comparer<T> numeric = System.Collections.Generic.Comparer<T>.Default;
            if (isNaN is null)
            {
                return numeric;
            }

            return System.Collections.Generic.Comparer<T>.Create((x, y) =>
            {
                bool xNaN = isNaN(x);
                bool yNaN = isNaN(y);
                return xNaN || yNaN ? xNaN.CompareTo(yNaN) : numeric.Compare(x, y);
            });
        }

        /// <summary>Each floating type's own test, one delegate per type, so no value is boxed to be tested.</summary>
        private static Func<T, bool>? Floating(string name)
        {
            // The floating types numpy has a NaN for, and no caller's own type that happens to declare the test.
            if (typeof(T) != typeof(double) && typeof(T) != typeof(float) && typeof(T).FullName != HalfName)
            {
                return null;
            }

            System.Reflection.MethodInfo? method = typeof(T).GetMethod(
                name, System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Static, null, [typeof(T)], null);
            return method is null || method.ReturnType != typeof(bool)
                ? null
                : (Func<T, bool>)Delegate.CreateDelegate(typeof(Func<T, bool>), method);
        }
    }
}
