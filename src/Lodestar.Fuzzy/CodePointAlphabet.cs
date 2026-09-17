namespace Lodestar.Fuzzy;

/// <summary>
/// Rewrites two strings so each code point is one UTF-16 unit, keeping equality and code-point order,
/// which is what lets the UTF-16 kernels answer rapidfuzz's code-point scores (#892).
/// </summary>
/// <remarks>
/// Every score here reads only lengths, element equality and, for the token ratios, the order tokens
/// sort in, so a map that is one unit per code point and strictly increasing changes none of them. Units up
/// to the space keep their value, so the space the token ratios join with still sits below every other
/// unit; the rest are ranked onto the non-surrogate units above it. Text with no surrogate is its own map.
/// </remarks>
internal readonly struct CodePointAlphabet
{
    private const int FirstRanked = ' ' + 1;

    private readonly Dictionary<int, char>? _units;

    private CodePointAlphabet(Dictionary<int, char>? units) => _units = units;

    /// <summary>The map for a pair of strings, over every code point either holds, or <see langword="null"/> past what a <see cref="char"/> can rank.</summary>
    /// <remarks>
    /// There are 63,455 non-surrogate units above the space, so two strings holding more distinct code
    /// points than that cannot be rewritten one unit per code point. <c>Fuzz.Ratio</c> answers such a
    /// pair from the code points themselves; the scorers that need the map refuse it (#982).
    /// </remarks>
    public static CodePointAlphabet? Over(string a, string b)
    {
        if (!HasSurrogate(a) && !HasSurrogate(b))
        {
            return new CodePointAlphabet(null);
        }

        var distinct = new List<int>(a.Length + b.Length);
        Collect(a, distinct);
        Collect(b, distinct);
        distinct.Sort();

        var units = new Dictionary<int, char>(distinct.Count);
        int next = FirstRanked;
        int previous = -1;
        foreach (int codePoint in distinct)
        {
            if (codePoint == previous)
            {
                continue;
            }

            previous = codePoint;
            if (codePoint < FirstRanked)
            {
                units[codePoint] = (char)codePoint;
                continue;
            }

            if (next == 0xD800)
            {
                next = 0xE000;
            }

            if (next > char.MaxValue)
            {
                return null;
            }

            units[codePoint] = (char)next++;
        }

        return new CodePointAlphabet(units);
    }

    /// <summary>The text's code points, a lone surrogate keeping its own value (#982).</summary>
    public static int[] Decode(string text)
    {
        var points = new int[CountCodePoints(text)];
        int at = 0;
        for (int i = 0; i < text.Length; i += Width(text, i))
        {
            points[at++] = CodePointAt(text, i);
        }

        return points;
    }

    /// <summary>rapidfuzz's split: runs of its whitespace, which is not <see cref="char.IsWhiteSpace(char)"/>'s.</summary>
    /// <remarks>
    /// U+001C to U+001F always split. U+0085 and U+00A0 split only in a string holding a unit above U+00FF,
    /// which CPython stores wider than Latin-1: measured over every BMP scalar against rapidfuzz 3.14.6 (#974).
    /// </remarks>
    public static string[] Tokenize(string text)
    {
        bool wide = false;
        for (int i = 0; i < text.Length && !wide; i++)
        {
            wide = text[i] > (char)0xFF;
        }

        var tokens = new List<string>();
        int start = -1;
        for (int i = 0; i <= text.Length; i++)
        {
            bool boundary = i == text.Length || IsRapidfuzzWhitespace(text[i], wide);
            if (boundary && start >= 0)
            {
                tokens.Add(text.Substring(start, i - start));
                start = -1;
            }
            else if (!boundary && start < 0)
            {
                start = i;
            }
        }

        return [.. tokens];
    }

    /// <summary><paramref name="text"/> rewritten, one unit per code point.</summary>
    public string Map(string text)
    {
        if (_units is null)
        {
            return text;
        }

        var mapped = new char[CountCodePoints(text)];
        int at = 0;
        for (int i = 0; i < text.Length; i += Width(text, i))
        {
            mapped[at++] = _units[CodePointAt(text, i)];
        }

        return new string(mapped);
    }

    private static bool IsRapidfuzzWhitespace(char unit, bool wide) =>
        unit is (>= (char)0x09 and <= (char)0x0D) or (>= (char)0x1C and <= ' ') or (char)0x1680
            or (>= (char)0x2000 and <= (char)0x200A) or (char)0x2028 or (char)0x2029 or (char)0x202F
            or (char)0x205F or (char)0x3000
        || (wide && unit is (char)0x85 or (char)0xA0);

    private static bool HasSurrogate(string text)
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

    private static void Collect(string text, List<int> distinct)
    {
        for (int i = 0; i < text.Length; i += Width(text, i))
        {
            distinct.Add(CodePointAt(text, i));
        }
    }

    private static int CountCodePoints(string text)
    {
        int count = 0;
        for (int i = 0; i < text.Length; i += Width(text, i))
        {
            count++;
        }

        return count;
    }

    /// <summary>A well-formed pair's scalar, or a lone surrogate's own value, as a Python <c>str</c> holds it.</summary>
    private static int CodePointAt(string text, int index) =>
        Width(text, index) == 2 ? char.ConvertToUtf32(text[index], text[index + 1]) : text[index];

    private static int Width(string text, int index) =>
        char.IsHighSurrogate(text[index]) && index + 1 < text.Length && char.IsLowSurrogate(text[index + 1]) ? 2 : 1;
}
