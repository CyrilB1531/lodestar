using Lodestar.Text;
using Lodestar.Text.Distances;

namespace Lodestar.Fuzzy;

/// <summary>
/// Applied fuzzy-matching ratios, reproducing <c>rapidfuzz.fuzz</c>.
/// </summary>
/// <remarks>
/// Scores are in <c>[0, 100]</c>, case-sensitive with punctuation kept, as in rapidfuzz. <see cref="Ratio(string, string)"/>
/// is the Indel similarity ×100, <em>not</em> Levenshtein. By default each scorer compares UTF-16 units and splits
/// on <see cref="char.IsWhiteSpace(char)"/>; its <see cref="TextElement.CodePoint"/> overload compares code points,
/// splits on rapidfuzz's whitespace and sorts tokens by code point, as rapidfuzz does (decision 0002, #892). Thread-safe.
/// </remarks>
public static class Fuzz
{
    /// <summary>Indel similarity ×100 — the base ratio.</summary>
    public static double Ratio(string a, string b)
    {
        Guard.NotNull(a);
        Guard.NotNull(b);
        return 100.0 * Indel.NormalizedSimilarity(a, b);
    }

    /// <summary><see cref="Ratio(string, string)"/>, over the unit <paramref name="element"/> names.</summary>
    /// <param name="a">The first string.</param>
    /// <param name="b">The second string.</param>
    /// <param name="element"><see cref="TextElement.CodePoint"/> for rapidfuzz's score on any string, the BMP or past it.</param>
    /// <exception cref="ArgumentOutOfRangeException"><paramref name="element"/> is not a declared value.</exception>
    public static double Ratio(string a, string b, TextElement element)
    {
        if (element == TextElement.Utf16Unit)
        {
            return Ratio(a, b);
        }

        CodePointPair? pair = CodePointPair.Of(a, b, element, tokens: false);
        return pair is null ? RatioOverCodePoints(a, b) : Ratio(pair.Value.A, pair.Value.B);
    }

    /// <summary>Best <see cref="Ratio(string, string)"/> between the shorter string and any substring of the longer.</summary>
    public static double PartialRatio(string a, string b)
    {
        Guard.NotNull(a);
        Guard.NotNull(b);
        if (a.Length == 0 && b.Length == 0)
        {
            return 100.0;
        }
        if (a.Length == 0 || b.Length == 0)
        {
            return 0.0;
        }

        // Slide the shorter string over the longer. When lengths are equal, neither
        // is strictly shorter, so try both orientations (matches rapidfuzz).
        if (a.Length < b.Length)
        {
            return SlideMax(a, b);
        }
        if (b.Length < a.Length)
        {
            return SlideMax(b, a);
        }
        return Math.Max(SlideMax(a, b), SlideMax(b, a));
    }

    /// <summary><see cref="PartialRatio(string, string)"/>, over the unit <paramref name="element"/> names.</summary>
    /// <param name="a">The first string.</param>
    /// <param name="b">The second string.</param>
    /// <param name="element"><see cref="TextElement.CodePoint"/> for rapidfuzz's score on any string, the BMP or past it.</param>
    /// <exception cref="ArgumentOutOfRangeException"><paramref name="element"/> is not a declared value.</exception>
    /// <exception cref="ArgumentException">The two strings hold more distinct code points than a <see cref="char"/> can rank, which only <see cref="Ratio(string, string, TextElement)"/> answers.</exception>
    public static double PartialRatio(string a, string b, TextElement element)
    {
        if (element == TextElement.Utf16Unit)
        {
            return PartialRatio(a, b);
        }

        CodePointPair pair = Required(CodePointPair.Of(a, b, element, tokens: false));
        return PartialRatio(pair.A, pair.B);
    }

    /// <summary>
    /// Slides <paramref name="pattern"/> across <paramref name="text"/> and returns the
    /// best ratio over the aligned windows (full length in the interior, truncated at edges).
    /// </summary>
    private static double SlideMax(string pattern, string text)
    {
        int m = pattern.Length;
        int n = text.Length;
        if (m == 0 || n == 0)
        {
            return 0.0;
        }
        if (m <= ShortNeedleWindows.MaxNeedle)
        {
            return ShortNeedleWindows.SlideMax(pattern, text);
        }

        return LongNeedleWindows.SlideMax(pattern, text);
    }

    /// <summary><see cref="Ratio(string, string)"/> after splitting, sorting and rejoining the tokens of each string.</summary>
    public static double TokenSortRatio(string a, string b)
    {
        Guard.NotNull(a);
        Guard.NotNull(b);
        return Ratio(string.Join(" ", SortedTokens(a)), string.Join(" ", SortedTokens(b)));
    }

    /// <summary><see cref="TokenSortRatio(string, string)"/>, over the unit <paramref name="element"/> names.</summary>
    /// <param name="a">The first string.</param>
    /// <param name="b">The second string.</param>
    /// <param name="element"><see cref="TextElement.CodePoint"/> for rapidfuzz's score on any string, the BMP or past it.</param>
    /// <exception cref="ArgumentOutOfRangeException"><paramref name="element"/> is not a declared value.</exception>
    /// <exception cref="ArgumentException">The two strings hold more distinct code points than a <see cref="char"/> can rank, which only <see cref="Ratio(string, string, TextElement)"/> answers.</exception>
    public static double TokenSortRatio(string a, string b, TextElement element)
    {
        if (element == TextElement.Utf16Unit)
        {
            return TokenSortRatio(a, b);
        }

        CodePointPair pair = Required(CodePointPair.Of(a, b, element, tokens: true));
        return Ratio(string.Join(" ", pair.TokensA), string.Join(" ", pair.TokensB));
    }

    /// <summary>Token-set ratio: compares the shared tokens against each string's full sorted token set.</summary>
    public static double TokenSetRatio(string a, string b)
    {
        Guard.NotNull(a);
        Guard.NotNull(b);
        return TokenSet(a, b, partial: false);
    }

    /// <summary><see cref="TokenSetRatio(string, string)"/>, over the unit <paramref name="element"/> names.</summary>
    /// <param name="a">The first string.</param>
    /// <param name="b">The second string.</param>
    /// <param name="element"><see cref="TextElement.CodePoint"/> for rapidfuzz's score on any string, the BMP or past it.</param>
    /// <exception cref="ArgumentOutOfRangeException"><paramref name="element"/> is not a declared value.</exception>
    /// <exception cref="ArgumentException">The two strings hold more distinct code points than a <see cref="char"/> can rank, which only <see cref="Ratio(string, string, TextElement)"/> answers.</exception>
    public static double TokenSetRatio(string a, string b, TextElement element)
    {
        if (element == TextElement.Utf16Unit)
        {
            return TokenSetRatio(a, b);
        }

        CodePointPair pair = Required(CodePointPair.Of(a, b, element, tokens: true));
        return TokenSet(pair.TokensA, Distinct(pair.TokensA), pair.TokensB, Distinct(pair.TokensB), partial: false);
    }

    /// <summary><see cref="PartialRatio(string, string)"/> on sorted-token strings.</summary>
    public static double PartialTokenSortRatio(string a, string b)
    {
        Guard.NotNull(a);
        Guard.NotNull(b);
        return PartialRatio(string.Join(" ", SortedTokens(a)), string.Join(" ", SortedTokens(b)));
    }

    /// <summary><see cref="PartialTokenSortRatio(string, string)"/>, over the unit <paramref name="element"/> names.</summary>
    /// <param name="a">The first string.</param>
    /// <param name="b">The second string.</param>
    /// <param name="element"><see cref="TextElement.CodePoint"/> for rapidfuzz's score on any string, the BMP or past it.</param>
    /// <exception cref="ArgumentOutOfRangeException"><paramref name="element"/> is not a declared value.</exception>
    /// <exception cref="ArgumentException">The two strings hold more distinct code points than a <see cref="char"/> can rank, which only <see cref="Ratio(string, string, TextElement)"/> answers.</exception>
    public static double PartialTokenSortRatio(string a, string b, TextElement element)
    {
        if (element == TextElement.Utf16Unit)
        {
            return PartialTokenSortRatio(a, b);
        }

        CodePointPair pair = Required(CodePointPair.Of(a, b, element, tokens: true));
        return PartialRatio(string.Join(" ", pair.TokensA), string.Join(" ", pair.TokensB));
    }

    /// <summary>Token-set ratio using <see cref="PartialRatio(string, string)"/> for the comparisons.</summary>
    public static double PartialTokenSetRatio(string a, string b)
    {
        Guard.NotNull(a);
        Guard.NotNull(b);
        return TokenSet(a, b, partial: true);
    }

    /// <summary><see cref="PartialTokenSetRatio(string, string)"/>, over the unit <paramref name="element"/> names.</summary>
    /// <param name="a">The first string.</param>
    /// <param name="b">The second string.</param>
    /// <param name="element"><see cref="TextElement.CodePoint"/> for rapidfuzz's score on any string, the BMP or past it.</param>
    /// <exception cref="ArgumentOutOfRangeException"><paramref name="element"/> is not a declared value.</exception>
    /// <exception cref="ArgumentException">The two strings hold more distinct code points than a <see cref="char"/> can rank, which only <see cref="Ratio(string, string, TextElement)"/> answers.</exception>
    public static double PartialTokenSetRatio(string a, string b, TextElement element)
    {
        if (element == TextElement.Utf16Unit)
        {
            return PartialTokenSetRatio(a, b);
        }

        CodePointPair pair = Required(CodePointPair.Of(a, b, element, tokens: true));
        return TokenSet(pair.TokensA, Distinct(pair.TokensA), pair.TokensB, Distinct(pair.TokensB), partial: true);
    }

    /// <summary>
    /// Weighted ratio: combines the base, partial and token ratios with rapidfuzz's
    /// length-dependent weighting and returns the maximum.
    /// </summary>
    public static double WRatio(string a, string b)
    {
        Guard.NotNull(a);
        Guard.NotNull(b);
        // Before the tokenization, which #970 moved behind it: an empty operand scores zero whatever
        // the other holds, and Process.Extract pays this per empty choice (#987).
        if (a.Length == 0 || b.Length == 0)
        {
            return 0.0;
        }

        return WRatio(a, b, SortedTokens(a), SortedTokens(b));
    }

    /// <summary><see cref="WRatio(string, string)"/>, over the unit <paramref name="element"/> names.</summary>
    /// <param name="a">The first string.</param>
    /// <param name="b">The second string.</param>
    /// <param name="element"><see cref="TextElement.CodePoint"/> for rapidfuzz's score on any string, the BMP or past it.</param>
    /// <exception cref="ArgumentOutOfRangeException"><paramref name="element"/> is not a declared value.</exception>
    /// <exception cref="ArgumentException">The two strings hold more distinct code points than a <see cref="char"/> can rank, which only <see cref="Ratio(string, string, TextElement)"/> answers.</exception>
    public static double WRatio(string a, string b, TextElement element)
    {
        if (element == TextElement.Utf16Unit)
        {
            return WRatio(a, b);
        }

        CodePointPair pair = Required(CodePointPair.Of(a, b, element, tokens: true));
        return WRatio(pair.A, pair.B, pair.TokensA, pair.TokensB);
    }

    /// <summary>The weighted ratio over two strings and their sorted tokens, which each mode supplies its own way.</summary>
    private static double WRatio(string a, string b, string[] tokensA, string[] tokensB)
    {
        if (a.Length == 0 || b.Length == 0)
        {
            return 0.0;
        }

        const double unbaseScale = 0.95;
        double lenRatio = (double)Math.Max(a.Length, b.Length) / Math.Min(a.Length, b.Length);

        double best = Ratio(a, b);

        // The sort and set ratios share one tokenization and one sort per side: the set ratio
        // only deduplicates what the sort ratio already ordered.
        string sortedA = string.Join(" ", tokensA);
        string sortedB = string.Join(" ", tokensB);
        if (lenRatio < 1.5)
        {
            best = Math.Max(best, Ratio(sortedA, sortedB) * unbaseScale);
            best = Math.Max(best, TokenSet(tokensA, Distinct(tokensA), tokensB, Distinct(tokensB), partial: false) * unbaseScale);
            return best;
        }

        double partialScale = lenRatio > 8.0 ? 0.6 : 0.9;
        best = Math.Max(best, PartialRatio(a, b) * partialScale);
        best = Math.Max(best, PartialRatio(sortedA, sortedB) * unbaseScale * partialScale);
        best = Math.Max(best, TokenSet(tokensA, Distinct(tokensA), tokensB, Distinct(tokensB), partial: true) * unbaseScale * partialScale);
        return best;
    }

    /// <summary>Two strings and their tokens rewritten one unit per code point, the tokens sorted by code point.</summary>
    private readonly struct CodePointPair
    {
        private CodePointPair(string a, string b, string[] tokensA, string[] tokensB)
        {
            A = a;
            B = b;
            TokensA = tokensA;
            TokensB = tokensB;
        }

        public string A { get; }

        public string B { get; }

        public string[] TokensA { get; }

        public string[] TokensB { get; }

        /// <param name="a">The first string.</param>
        /// <param name="b">The second string.</param>
        /// <param name="element">The unit, refused unless it is the code point.</param>
        /// <param name="tokens">Whether the caller reads <see cref="TokensA"/>, which only the token scorers do (#987).</param>
        public static CodePointPair? Of(string a, string b, TextElement element, bool tokens)
        {
            Guard.NotNull(a);
            Guard.NotNull(b);
            if (element != TextElement.CodePoint)
            {
                throw new ArgumentOutOfRangeException(nameof(element), element, "The unit must be Utf16Unit or CodePoint.");
            }

            CodePointAlphabet? map = CodePointAlphabet.Over(a, b);
            if (map is null)
            {
                return null;
            }

            CodePointAlphabet alphabet = map.Value;
            return new CodePointPair(
                alphabet.Map(a),
                alphabet.Map(b),
                tokens ? Sorted(alphabet, a) : [],
                tokens ? Sorted(alphabet, b) : []);
        }

        /// <summary>Mapped before sorting: the map keeps code-point order, so an ordinal sort of its units is that order.</summary>
        private static string[] Sorted(CodePointAlphabet alphabet, string text)
        {
            string[] tokens = CodePointAlphabet.Tokenize(text);
            for (int i = 0; i < tokens.Length; i++)
            {
                tokens[i] = alphabet.Map(tokens[i]);
            }

            Array.Sort(tokens, StringComparer.Ordinal);
            return tokens;
        }
    }

    /// <summary>The Indel similarity over code points, which no <see cref="char"/> map bounds (#982).</summary>
    /// <remarks>
    /// <see cref="Ratio(string, string)"/>'s own expression, over the decoded code points: the kernel
    /// <see cref="Indel.Distance{T}(ReadOnlySpan{T}, ReadOnlySpan{T})"/> takes any equatable element.
    /// </remarks>
    private static double RatioOverCodePoints(string a, string b)
    {
        int[] first = CodePointAlphabet.Decode(a);
        int[] second = CodePointAlphabet.Decode(b);
        int total = first.Length + second.Length;
        return total == 0 ? 100.0 : 100.0 * (1.0 - ((double)Indel.Distance<int>(first, second) / total));
    }

    /// <summary>The pair, or the refusal the scorers that need the map owe the caller.</summary>
    private static CodePointPair Required(CodePointPair? pair) =>
        pair ?? throw new ArgumentException(
            "The two strings hold more distinct code points than the 63,455 non-surrogate UTF-16 units this "
            + "scorer ranks them on. Ratio answers such a pair; the others need a wider kernel (#982).");

    private static string[] Tokenize(string s) =>
        s.Split((char[]?)null, StringSplitOptions.RemoveEmptyEntries);

    private static string[] SortedTokens(string s)
    {
        string[] tokens = Tokenize(s);
        Array.Sort(tokens, StringComparer.Ordinal);
        return tokens;
    }

    private static double TokenSet(string a, string b, bool partial)
    {
        // Sorted, deduplicated arrays rather than five SortedSet<string>: the sets hold a
        // handful of tokens each, and a red-black node apiece was most of what #494 measured.
        string[] setA = SortedTokens(a);
        string[] setB = SortedTokens(b);
        return TokenSet(setA, Distinct(setA), setB, Distinct(setB), partial);
    }

    private static double TokenSet(string[] setA, int countA, string[] setB, int countB, bool partial)
    {
        // rapidfuzz scores 0 when either side has no words; the prefix reading below would give 100.
        if (countA == 0 || countB == 0)
        {
            return 0.0;
        }

        var shape = default(SetShape);
        Measure(setA, countA, setB, countB, ref shape);
        int lengthA = shape.CombinedLength(shape.OnlyAChars, shape.OnlyACount);
        int lengthB = shape.CombinedLength(shape.OnlyBChars, shape.OnlyBCount);

        // Both combined strings are written side by side into one buffer of spaces, so the
        // tokens are copied once and no separator is written.
        int total = lengthA + lengthB;
        Span<char> buffer = total <= 512 ? stackalloc char[total] : new char[total];
        buffer.Fill(' ');
        Write(setA, countA, setB, countB, shape, buffer.Slice(0, lengthA), buffer.Slice(lengthA));
        ReadOnlySpan<char> combinedA = buffer.Slice(0, lengthA);
        ReadOnlySpan<char> combinedB = buffer.Slice(lengthA);

        if (partial)
        {
            string sect = combinedA.Slice(0, shape.SharedLength).ToString();
            string first = combinedA.ToString();
            string second = combinedB.ToString();
            double p1 = PartialRatio(sect, first);
            double p2 = PartialRatio(sect, second);
            return Math.Max(p1, Math.Max(p2, PartialRatio(first, second)));
        }

        double r1 = PrefixRatio(shape.SharedLength, lengthA);
        double r2 = PrefixRatio(shape.SharedLength, lengthB);
        return Math.Max(r1, Math.Max(r2, 100.0 * Indel.NormalizedSimilarity(combinedA, combinedB)));
    }

    /// <summary><see cref="Ratio(string, string)"/> of a string against one it is a prefix of, from the two lengths.</summary>
    /// <remarks>
    /// The intersection opens each combined string, so their longest common subsequence is the
    /// intersection itself and the Indel distance is the length difference — rapidfuzz reads it the
    /// same way. The expression is Indel.NormalizedDistance's, so the score is the same bits.
    /// </remarks>
    private static double PrefixRatio(int prefixLength, int length)
    {
        int total = prefixLength + length;
        double distance = total == 0 ? 0.0 : (double)(length - prefixLength) / total;
        return 100.0 * (1.0 - distance);
    }

    /// <summary>
    /// Moves the distinct tokens of an ordinally sorted array to its front, returning how many
    /// there are — the order a <c>SortedSet</c> enumerated, without a second array to hold it.
    /// </summary>
    private static int Distinct(string[] tokens)
    {
        if (tokens.Length <= 1)
        {
            return tokens.Length;
        }

        int kept = 1;
        for (int i = 1; i < tokens.Length; i++)
        {
            if (!string.Equals(tokens[i], tokens[kept - 1], StringComparison.Ordinal))
            {
                tokens[kept++] = tokens[i];
            }
        }
        return kept;
    }

    /// <summary>Token and character counts of the shared and either-side-only parts of two token sets.</summary>
    private struct SetShape
    {
        public int SharedCount;
        public int SharedChars;
        public int OnlyACount;
        public int OnlyAChars;
        public int OnlyBCount;
        public int OnlyBChars;

        /// <summary>The joined intersection's length, separators included.</summary>
        public readonly int SharedLength => SharedCount == 0 ? 0 : SharedChars + SharedCount - 1;

        /// <summary>The length of the intersection joined with one side's remainder.</summary>
        public readonly int CombinedLength(int onlyChars, int onlyCount) =>
            onlyCount == 0 ? SharedLength : SharedChars + onlyChars + SharedCount + onlyCount - 1;
    }

    /// <summary>Counts what a merge of two sorted, distinct token lists would put in each part.</summary>
    private static void Measure(string[] first, int firstCount, string[] second, int secondCount, ref SetShape shape)
    {
        int i = 0;
        int j = 0;
        while (i < firstCount && j < secondCount)
        {
            int order = string.CompareOrdinal(first[i], second[j]);
            if (order == 0)
            {
                shape.SharedCount++;
                shape.SharedChars += first[i++].Length;
                j++;
            }
            else if (order < 0)
            {
                shape.OnlyACount++;
                shape.OnlyAChars += first[i++].Length;
            }
            else
            {
                shape.OnlyBCount++;
                shape.OnlyBChars += second[j++].Length;
            }
        }

        for (; i < firstCount; i++)
        {
            shape.OnlyACount++;
            shape.OnlyAChars += first[i].Length;
        }
        for (; j < secondCount; j++)
        {
            shape.OnlyBCount++;
            shape.OnlyBChars += second[j].Length;
        }
    }

    /// <summary>
    /// Writes the intersection followed by each side's remainder, in sorted order, into buffers
    /// already filled with spaces — the strings joining the three lists with one space gave.
    /// </summary>
    private static void Write(
        string[] first,
        int firstCount,
        string[] second,
        int secondCount,
        SetShape shape,
        Span<char> combinedA,
        Span<char> combinedB)
    {
        int shared = 0;
        int restStart = shape.SharedCount == 0 ? 0 : shape.SharedLength + 1;
        int onlyA = restStart;
        int onlyB = restStart;
        int i = 0;
        int j = 0;
        while (i < firstCount && j < secondCount)
        {
            int order = string.CompareOrdinal(first[i], second[j]);
            if (order == 0)
            {
                first[i].AsSpan().CopyTo(combinedA.Slice(shared));
                first[i].AsSpan().CopyTo(combinedB.Slice(shared));
                shared += first[i++].Length + 1;
                j++;
            }
            else if (order < 0)
            {
                first[i].AsSpan().CopyTo(combinedA.Slice(onlyA));
                onlyA += first[i++].Length + 1;
            }
            else
            {
                second[j].AsSpan().CopyTo(combinedB.Slice(onlyB));
                onlyB += second[j++].Length + 1;
            }
        }

        for (; i < firstCount; i++)
        {
            first[i].AsSpan().CopyTo(combinedA.Slice(onlyA));
            onlyA += first[i].Length + 1;
        }
        for (; j < secondCount; j++)
        {
            second[j].AsSpan().CopyTo(combinedB.Slice(onlyB));
            onlyB += second[j].Length + 1;
        }
    }
}
