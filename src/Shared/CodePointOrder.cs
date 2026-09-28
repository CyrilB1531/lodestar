namespace Lodestar.Internal;

/// <summary>Orders strings by Unicode code point, which is how Python's <c>sorted</c> orders a <c>str</c>.</summary>
/// <remarks>
/// <see cref="StringComparer.Ordinal"/> compares UTF-16 units, and a surrogate pair's leading unit
/// (U+D800 to U+DBFF) sorts below U+E000 to U+FFFF although the character it starts sits above them:
/// <c>"𠮟"</c> precedes <c>"ｶ"</c> ordinally and follows it in Python. A lone surrogate keeps its unit
/// value, as a Python <c>str</c> keeps it (#1264).
/// </remarks>
internal sealed class CodePointOrder : IComparer<string>
{
    /// <summary>The one instance; the comparer holds no state.</summary>
    public static readonly CodePointOrder Instance = new();

    private CodePointOrder()
    {
    }

    /// <inheritdoc/>
    public int Compare(string? x, string? y)
    {
        if (ReferenceEquals(x, y))
        {
            return 0;
        }
        if (x is null)
        {
            return -1;
        }
        if (y is null)
        {
            return 1;
        }
        return Compare(x.AsSpan(), y.AsSpan());
    }

    /// <summary>Compares two spans by code point, the shorter first when one is a prefix of the other.</summary>
    public static int Compare(ReadOnlySpan<char> x, ReadOnlySpan<char> y)
    {
#if NET8_0_OR_GREATER
        int i = x.CommonPrefixLength(y);
#else
        int length = Math.Min(x.Length, y.Length);
        int i = 0;
        while (i < length && x[i] == y[i])
        {
            i++;
        }
#endif
        return i < x.Length && i < y.Length ? FirstDifference(x, y, i) : x.Length - y.Length;
    }

    // Everything before i is equal, so both sides sit at the same character boundary or inside
    // the same pair.
    private static int FirstDifference(ReadOnlySpan<char> x, ReadOnlySpan<char> y, int i)
    {
        char a = x[i];
        char b = y[i];
        // After an equal leading surrogate, the side whose unit completes the pair holds a
        // supplementary character and the other a lone surrogate, which sorts below it.
        if (i > 0 && char.IsHighSurrogate(x[i - 1]))
        {
            bool pairedX = char.IsLowSurrogate(a);
            if (pairedX != char.IsLowSurrogate(b))
            {
                return pairedX ? 1 : -1;
            }
            if (pairedX)
            {
                return a - b;
            }
        }
        return Rank(a, x, i) - Rank(b, y, i);
    }

    // A leading surrogate followed by a trailing one starts a supplementary character, which sorts
    // above every BMP unit; anything else keeps its own value.
    private static int Rank(char c, ReadOnlySpan<char> s, int i)
        => char.IsHighSurrogate(c) && i + 1 < s.Length && char.IsLowSurrogate(s[i + 1]) ? c + 0x10000 : c;
}
