using System.Globalization;
using System.Text;
using System.Text.RegularExpressions;
using Lodestar.Text.Internal;

namespace Lodestar.Text.Vectorization;


// SonarLint S3267: the suggested Select does not compile on netstandard2.0 —
// MatchCollection implements only the non-generic IEnumerable there, so LINQ
// would need a Cast<Match>() and an extra allocation in a per-document path.
// CA1720 (identifier contains type name): AnalyzerKind.Char mirrors
// scikit-learn's analyzer='char', which is the name a reader arrives with, and it
// has been public since 0.1.0 — renaming it breaks consumers for a naming rule.
#pragma warning disable S3267, CA1720
/// <summary>The kind of tokens a vectorizer extracts.</summary>
public enum AnalyzerKind
{
    /// <summary>Word tokens (via the token pattern), then word n-grams.</summary>
    Word,

    /// <summary>Character n-grams over the whole preprocessed string.</summary>
    Char,

    /// <summary>Character n-grams that do not cross word boundaries (words padded with spaces).</summary>
    CharWordBoundary,
}

// CA1308 (normalize to uppercase): lowercasing here is the pipeline's default
// behavior, when `Lowercase` is set, mirroring scikit-learn's text vectorizers.
// ToUpperInvariant would change which terms come out, which breaks the oracle
// corpora this suite is checked against rather than merely recasing them.
#pragma warning disable CA1308

/// <summary>
/// Turns a document into its sequence of terms, mirroring the preprocessing and
/// analysis pipeline of scikit-learn's text vectorizers.
/// </summary>
internal sealed class TextAnalyzer
{
    private readonly bool _lowercase;
    private readonly bool _stripAccents;
    private readonly AnalyzerKind _kind;
    private readonly int _minN;
    private readonly int _maxN;
    private readonly Regex _tokenPattern;
    private readonly StopWordSet? _stopWords;

    public TextAnalyzer(
        bool lowercase,
        bool stripAccents,
        AnalyzerKind kind,
        (int Min, int Max) ngramRange,
        string tokenPattern,
        IReadOnlyCollection<string>? stopWords)
    {
        if (ngramRange.Min < 1 || ngramRange.Max < ngramRange.Min)
        {
            throw new ArgumentException("Invalid ngram range.", nameof(ngramRange));
        }

        _lowercase = lowercase;
        _stripAccents = stripAccents;
        _kind = kind;
        _minN = ngramRange.Min;
        _maxN = ngramRange.Max;
        // The pattern comes from the caller, so an unbounded match would let a
        // crafted pattern/document pair hang the thread. Bound it.
        _tokenPattern = new Regex(tokenPattern, RegexOptions.Compiled | RegexOptions.CultureInvariant, RegexDefaults.MatchTimeout);
        _stopWords = stopWords is null ? null : StopWordSet.Adopt(stopWords);
    }

    /// <summary>Produces the terms of <paramref name="document"/> (with repetition).</summary>
    public List<string> Analyze(string document)
    {
        var sink = new TermList(new List<string>());
        Analyze(document, ref sink);
        return sink.Terms;
    }

    /// <summary>Hands each term of <paramref name="document"/> to <paramref name="sink"/>, in <see cref="Analyze(string)"/>'s order.</summary>
    /// <remarks>
    /// A term arrives as a span over the preprocessed document or over a scratch buffer, so a
    /// sink that only looks a term up or hashes it never makes it a string. The span is valid
    /// only for the duration of the call.
    /// </remarks>
    public void Analyze<TSink>(string document, ref TSink sink)
        where TSink : struct, ITermSink
    {
        string s = Preprocess(document);
        switch (_kind)
        {
            case AnalyzerKind.Char:
                CharNgrams(s, ref sink);
                break;
            case AnalyzerKind.CharWordBoundary:
                CharWordBoundaryNgrams(s, ref sink);
                break;
            default:
                WordNgrams(s, ref sink);
                break;
        }
    }

    private string Preprocess(string document)
    {
        string s = document;
        if (_lowercase)
        {
            s = s.ToLowerInvariant();
        }
        if (_stripAccents)
        {
            s = StripAccents(s);
        }
        return s;
    }

    private static string StripAccents(string s)
    {
        string decomposed = WellFormedNormalization.Normalize(s, NormalizationForm.FormKD);
        var sb = new StringBuilder(decomposed.Length);
        foreach (char c in decomposed)
        {
            if (CharUnicodeInfo.GetUnicodeCategory(c) != UnicodeCategory.NonSpacingMark)
            {
                sb.Append(c);
            }
        }
        return sb.ToString();
    }

    /// <summary>The kept tokens of <paramref name="s"/> as start and length pairs, in document order.</summary>
    private List<(int Start, int Length)> Tokenize<TSink>(string s, ref TSink sink, bool emit)
        where TSink : struct, ITermSink
    {
        var tokens = new List<(int Start, int Length)>();
#if NET9_0_OR_GREATER
        foreach (ValueMatch m in _tokenPattern.EnumerateMatches(s))
        {
            // Judged as a span over the document, so a filtered-out (and by
            // definition frequent) stop word is never allocated as a string.
            ReadOnlySpan<char> token = s.AsSpan(m.Index, m.Length);
            if (_stopWords is null || !_stopWords.Contains(token))
            {
                Keep(tokens, m.Index, m.Length, token, ref sink, emit);
            }
        }
#else
        foreach (Match m in _tokenPattern.Matches(s))
        {
            if (_stopWords is null)
            {
                Keep(tokens, m.Index, m.Length, s.AsSpan(m.Index, m.Length), ref sink, emit);
                continue;
            }

            // netstandard2.0 has no span lookup, so the token is materialised for the
            // filter and handed on as that string rather than copied a second time.
            string tok = m.Value;
            if (!_stopWords.Contains(tok))
            {
                tokens.Add((m.Index, m.Length));
                if (emit)
                {
                    sink.Add(tok);
                }
            }
        }
#endif
        return tokens;
    }

    private static void Keep<TSink>(
        List<(int Start, int Length)> tokens, int start, int length, ReadOnlySpan<char> token, ref TSink sink, bool emit)
        where TSink : struct, ITermSink
    {
        tokens.Add((start, length));
        if (emit)
        {
            sink.Add(token);
        }
    }

    private void WordNgrams<TSink>(string s, ref TSink sink)
        where TSink : struct, ITermSink
    {
        // Unigrams are emitted while matching, in the order the n = 1 pass would give them.
        bool unigramsFirst = _minN == 1;
        List<(int Start, int Length)> tokens = Tokenize(s, ref sink, unigramsFirst);
        if (_maxN == 1)
        {
            return;
        }

        int count = tokens.Count;
        char[] joined = [];
        for (int n = Math.Max(_minN, 2); n <= _maxN; n++)
        {
            for (int i = 0; i + n <= count; i++)
            {
                int length = Join(s, tokens, i, n, ref joined);
                sink.Add(joined.AsSpan(0, length));
            }
        }
    }

    /// <summary>Tokens <c>first..first+n-1</c> joined by single spaces into <paramref name="joined"/>, as <c>string.Join(" ", ...)</c> builds them.</summary>
    private static int Join(string s, List<(int Start, int Length)> tokens, int first, int n, ref char[] joined)
    {
        int length = n - 1;
        for (int k = first; k < first + n; k++)
        {
            length += tokens[k].Length;
        }
        if (joined.Length < length)
        {
            joined = new char[Math.Max(length, joined.Length * 2)];
        }

        int at = 0;
        for (int k = first; k < first + n; k++)
        {
            if (k > first)
            {
                joined[at++] = ' ';
            }
            s.CopyTo(tokens[k].Start, joined, at, tokens[k].Length);
            at += tokens[k].Length;
        }
        return length;
    }

    private void CharNgrams<TSink>(string s, ref TSink sink)
        where TSink : struct, ITermSink
    {
        // scikit-learn rewrites only runs of two or more (\s\s+) as one space; a lone tab stays.
        s = CollapseWhitespaceRuns(s);
        int len = s.Length;
        for (int n = _minN; n <= _maxN; n++)
        {
            for (int i = 0; i + n <= len; i++)
            {
                sink.Add(s.AsSpan(i, n));
            }
        }
    }

    private void CharWordBoundaryNgrams<TSink>(string s, ref TSink sink)
        where TSink : struct, ITermSink
    {
        // Words are the runs Python's str.split() yields, separated by IsPythonWhiteSpace.
        char[] padded = [];
        int at = 0;
        while (at < s.Length)
        {
            if (IsPythonWhiteSpace(s[at]))
            {
                at++;
                continue;
            }

            int wordStart = at;
            while (at < s.Length && !IsPythonWhiteSpace(s[at]))
            {
                at++;
            }

            int len = at - wordStart + 2;
            if (padded.Length < len)
            {
                padded = new char[Math.Max(len, padded.Length * 2)];
            }
            padded[0] = ' ';
            s.CopyTo(wordStart, padded, 1, len - 2);
            padded[len - 1] = ' ';
            PaddedWordNgrams(padded.AsSpan(0, len), ref sink);
        }
    }

    private void PaddedWordNgrams<TSink>(ReadOnlySpan<char> w, ref TSink sink)
        where TSink : struct, ITermSink
    {
        // Mirrors scikit-learn's _char_wb_ngrams: always emit w[0:n] (clamped),
        // then slide; a word shorter than n is emitted once and breaks the n-loop.
        int len = w.Length;
        for (int n = _minN; n <= _maxN; n++)
        {
            sink.Add(w.Slice(0, Math.Min(n, len)));
            int offset = 0;
            while (offset + n < len)
            {
                offset++;
                sink.Add(w.Slice(offset, n));
            }
            if (offset == 0)
            {
                break;
            }
        }
    }

    /// <summary>scikit-learn's <c>_white_spaces.sub(" ", doc)</c> over <c>\s\s+</c>.</summary>
    /// <remarks>A single whitespace character is kept as written, so <c>"a\tb"</c> yields the gram <c>"a\t"</c> (#879).</remarks>
    private static string CollapseWhitespaceRuns(string s)
    {
        var sb = new StringBuilder(s.Length);
        int i = 0;
        while (i < s.Length)
        {
            int run = i;
            while (run < s.Length && IsPythonWhiteSpace(s[run]))
            {
                run++;
            }
            if (run - i >= 2)
            {
                sb.Append(' ');
                i = run;
            }
            else
            {
                sb.Append(s[i]);
                i++;
            }
        }
        return sb.ToString();
    }

    /// <summary>Python's <c>str.isspace</c>, which is also what <c>re</c>'s <c>\s</c> matches on a <c>str</c>.</summary>
    /// <remarks>
    /// <see cref="char.IsWhiteSpace(char)"/> plus the four information separators U+001C to U+001F,
    /// which Python counts as whitespace (their bidirectional class is B or S) and .NET does not.
    /// </remarks>
    private static bool IsPythonWhiteSpace(char c) => char.IsWhiteSpace(c) || c is >= '\u001C' and <= '\u001F';
}

/// <summary>Receives the terms a <see cref="TextAnalyzer"/> produces, one call per term.</summary>
/// <remarks>
/// Implemented by structs and passed by reference, so each vectorizer's per-term work is
/// specialised into the analysis loop rather than reached through a delegate.
/// </remarks>
internal interface ITermSink
{
    /// <summary>One term, as a span valid only during the call.</summary>
    void Add(ReadOnlySpan<char> term);

    /// <summary>One term the analyzer already holds as a string.</summary>
    void Add(string term);
}

/// <summary>Collects every term as a string, which is what <see cref="TextAnalyzer.Analyze(string)"/> returns.</summary>
internal readonly struct TermList : ITermSink
{
    public TermList(List<string> terms) => Terms = terms;

    public List<string> Terms { get; }

    public void Add(ReadOnlySpan<char> term) => Terms.Add(term.ToString());

    public void Add(string term) => Terms.Add(term);
}
