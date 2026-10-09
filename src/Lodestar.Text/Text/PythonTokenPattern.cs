using System.Collections.Concurrent;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Text.RegularExpressions;

namespace Lodestar.Text.Internal;

// SonarLint S127: a surrogate pair advances the loop variable past its low half.
#pragma warning disable S127

/// <summary>A token pattern read as Python's <c>re</c> reads it, scanned to (start, length) matches.</summary>
/// <remarks>
/// The default patterns, <c>\b\w\w+\b</c> and <c>\b\w+\b</c> with or without <c>\b</c> and
/// <c>(?u)</c>, mean every maximal run of word characters of at least two or one code points, and
/// are scanned by hand: the translated regex costs ten times the .NET one (#1239). Any other
/// pattern goes through <see cref="PythonPattern"/>, and yields what <c>re.findall</c> yields: the
/// whole match, or the one capturing group's text when the pattern has one (#1262).
/// </remarks>
internal sealed class PythonTokenPattern
{
    private readonly int _minimumRun;
    private readonly int _group;

    // Whether a pattern read on Python's grammar can match the empty string: after an empty match Python tries a
    // non-empty one at the same place, where .NET moves on a unit, so |a over "a" gave '' alone, not '' and 'a'.
    private readonly bool _mayMatchEmpty;
    private ConcurrentDictionary<Regex, Regex>? _advancing;

    // Spelled for a text without surrogates, and pair-aware, five to eight times slower, for one holding them (#1645).
    // Each is compiled at first use: the first, compiled at construction, took 8 µs of 13 (#1658).
    private readonly Lazy<Regex>? _plain;
    private readonly PairAwareRegex? _pairAware;

    // First, as 0.7.0's Regex refused a null or a pattern it cannot read before anything else here (#1645).
    public PythonTokenPattern(string pattern)
        : this(pattern, RefuseAsWritten(pattern))
    {
    }

    /// <summary>The token pattern <paramref name="written"/> holds, which <see cref="RefuseAsWritten"/> gave for <paramref name="pattern"/>.</summary>
    public PythonTokenPattern(string pattern, Regex written)
    {
        string body = written.ToString();
        _minimumRun = body switch
        {
            @"\b\w\w+\b" or @"\w\w+" => 2,
            @"\b\w+\b" or @"\w+" => 1,
            _ => 0,
        };
        if (_minimumRun == 0)
        {
            int[] groups;
            (_plain, _pairAware, groups, _mayMatchEmpty) = Parse(pattern, written);
            // One group is the token, as re.findall returns it; two or more, which build_tokenizer refuses, the whole
            // match, as 0.7.0 read it (#1262, #1657). Numbered on the spelling compiled, as Python numbers them (#1663).
            _group = groups.Length == 2 ? groups[1] : 0;
        }
    }

    /// <summary>The plain spelling and the pair-aware one, each compiled at its first use, and their group numbers.</summary>
    /// <remarks>
    /// The pattern as written is checked first, as 0.7.0 compiled it: one .NET refuses goes out with 0.7.0's exception and
    /// message, whether or not Python reads it. The plain spelling is then parsed here. The pair-aware one differs from it
    /// only by fragments written here, each an atom where the plain spelling has one and none capturing, so it parses
    /// exactly when the plain one does, numbers its groups alike, and neither fails when compiled at its first use (#1645,
    /// #1658, #1662); one Python refuses, <c>[\w-.]</c> or <c>\p{L}</c>, is read as 0.7.0 read it, by .NET as written
    /// (#1650). The groups are Python's, which a comment can make other than .NET's as written: <c>(?#\)(a)</c> has none.
    /// </remarks>
    private static (Lazy<Regex> Plain, PairAwareRegex? PairAware, int[] Groups, bool MayMatchEmpty) Parse(string pattern, Regex written)
    {
        const RegexOptions Parsed = RegexOptions.CultureInvariant;
        const RegexOptions Compiled = RegexOptions.Compiled | Parsed;
        try
        {
            string plain = PythonPattern.Translate(pattern, surrogateFree: true, out bool mayMatchEmpty, out bool repeatsEmpty);
            // Parsing the pair-aware spelling instead, the longer one, was 8 KB of the construction's 11 (#1662).
            int[] groups = new Regex(plain, Parsed, RegexDefaults.MatchTimeout).GetGroupNumbers();
            var plainRegex = new Lazy<Regex>(() => new Regex(plain, Compiled, RegexDefaults.MatchTimeout));
            return (plainRegex, new PairAwareRegex(pattern, plain, interpret: !repeatsEmpty), groups, mayMatchEmpty);
        }
        catch (ArgumentException)
        {
            string body = written.ToString();
            var raw = new Lazy<Regex>(() => new Regex(body, Compiled, RegexDefaults.MatchTimeout));
            return (raw, null, written.GetGroupNumbers(), false);
        }
    }

    /// <summary>
    /// Refuses what 0.7.0 refused when it compiled the pattern as written: a null, and a pattern .NET refuses, with .NET's
    /// own exceptions. A leading <c>(?u)</c>, Python's default, is accepted and dropped (#1239, #1645).
    /// </summary>
    /// <returns>The pattern without its <c>(?u)</c>, parsed, which <see cref="PythonTokenPattern(string, Regex)"/> takes.</returns>
    /// <exception cref="ArgumentNullException"><paramref name="pattern"/> is null, its parameter named <c>pattern</c>, as Regex names it.</exception>
    /// <exception cref="ArgumentException">.NET refuses the pattern as written: its <c>RegexParseException</c> where it has one.</exception>
    public static Regex RefuseAsWritten(string pattern)
    {
        string written = pattern?.StartsWith("(?u)", StringComparison.Ordinal) == true ? pattern.Substring(4) : pattern!;
        try
        {
            return new Regex(written, RegexOptions.CultureInvariant, RegexDefaults.MatchTimeout);
        }
        catch (ArgumentException) when (!ReferenceEquals(written, pattern))
        {
            // 0.7.0 parsed the (?u) too, and .NET refuses it: what goes out is that refusal, offset and all (#1656).
            _ = new Regex(pattern!, RegexOptions.CultureInvariant, RegexDefaults.MatchTimeout);
            throw;
        }
    }

    /// <summary>The matches in <paramref name="s"/>, in order, as UTF-16 start and length.</summary>
    public List<(int Start, int Length)> Matches(string s)
    {
        var matches = new List<(int Start, int Length)>();
        Matches(s, matches);
        return matches;
    }

    /// <summary>Replaces the content of <paramref name="matches"/> with the matches in <paramref name="s"/>.</summary>
    public void Matches(string s, List<(int Start, int Length)> matches) => Matches(s, matches, ReadsPairs(s));

    /// <summary>
    /// <see cref="Matches(string, List{ValueTuple{int, int}})"/>, told whether <paramref name="s"/> holds a surrogate the
    /// pair-aware spelling reads, as <c>TryGetGrouplessRegex</c> found, rather than searching a long text again.
    /// </summary>
    public void Matches(string s, List<(int Start, int Length)> matches, bool pairAware)
    {
        matches.Clear();
        pairAware &= _pairAware is not null;
        if (_plain is null)
        {
            ScanRuns(s, _minimumRun, matches);
            return;
        }
        if (!pairAware)
        {
            Scan(_plain.Value, s, matches, pairAware: false);
            return;
        }

        ReadPairs(s, regex => Scan(regex, s, matches, pairAware: true), matches.Clear);
    }

    // The pair-aware spelling's reading of s: where the interpreter, bounded, runs out, the compiled one, unbounded (#1666).
    private void ReadPairs(string s, Action<Regex> read, Action restart)
    {
        Regex regex = _pairAware!.For(s);
        try
        {
            read(regex);
        }
        catch (RegexMatchTimeoutException) when (regex.MatchTimeout != Regex.InfiniteMatchTimeout)
        {
            restart();
            read(_pairAware.Unbounded);
        }
    }

    private void Scan(Regex regex, string s, List<(int Start, int Length)> matches, bool pairAware)
    {
#if NET7_0_OR_GREATER
        if (_group == 0)
        {
            // No group to read, so no Match per token: 0.7.0's scan, which a short-word text ran six times faster (#1645).
            ValueMatches(regex, s, matches, pairAware);
            return;
        }
#endif
        foreach (Match m in Found(regex, s, pairAware))
        {
            matches.Add(Token(m));
        }
    }

    /// <summary>Each match's token, as <see cref="Matches(string)"/> gives it, with the span the whole match covers.</summary>
    /// <remarks>
    /// The two differ only under a capturing group; the characters the rest of the match consumed
    /// belong to the token's match and are not a gap between two tokens.
    /// </remarks>
    public List<(int Start, int Length, int MatchStart, int MatchEnd)> MatchesWithSpans(string s)
    {
        var spans = new List<(int Start, int Length, int MatchStart, int MatchEnd)>();
        if (_plain is null)
        {
            foreach ((int start, int length) in Matches(s))
            {
                spans.Add((start, length, start, start + length));
            }
            return spans;
        }

        if (!ReadsPairs(s))
        {
            AddSpans(_plain.Value, s, spans, pairAware: false);
            return spans;
        }
        ReadPairs(s, regex => AddSpans(regex, s, spans, pairAware: true), spans.Clear);
        return spans;
    }

    private void AddSpans(Regex regex, string s, List<(int Start, int Length, int MatchStart, int MatchEnd)> spans, bool pairAware)
    {
        foreach (Match m in Found(regex, s, pairAware))
        {
            (int start, int length) = Token(m);
            spans.Add((start, length, m.Index, m.Index + m.Length));
        }
    }

#if NET7_0_OR_GREATER
    /// <summary>The regex to scan <paramref name="s"/> with, when the pattern has one and no group to read a token from.</summary>
    public bool TryGetGrouplessRegex(string s, [NotNullWhen(true)] out Regex? regex, out bool pairAware)
    {
        // The pair-aware spelling's matches go through Matches, whose scan drops one starting inside a pair.
        pairAware = ReadsPairs(s);
        regex = _plain is not null && _group == 0 && !pairAware && !_mayMatchEmpty ? _plain.Value : null;
        return regex is not null;
    }
#endif

    /// <summary>
    /// The matches as Python's <c>re</c> finds them. Over a text holding a surrogate, one starting on the low half of a
    /// pair, a position Python's str has not, is dropped and the search resumes past it: checked here rather than in the
    /// pattern, where it cost .NET its search for what follows a loop (#1666). After an empty match, a non-empty one at
    /// the same place is tried first, as Python 3.7 does.
    /// </summary>
    private IEnumerable<Match> Found(Regex regex, string s, bool pairAware)
    {
        if (!pairAware && !_mayMatchEmpty)
        {
            foreach (Match plain in regex.Matches(s))
            {
                yield return plain;
            }
            yield break;
        }

        var cursor = new Cursor();
        int position = 0;
        while (position <= s.Length)
        {
            Match m = regex.Match(s, position);
            if (!m.Success)
            {
                yield break;
            }
            if (Skipped(s, m.Index, cursor.Floor, pairAware))
            {
                position = cursor.Resume(m.Index);
                continue;
            }
            yield return m;
            if (Longer(regex, s, m.Index, m.Length) is { } longer)
            {
                yield return longer;
                m = longer;
            }
            position = cursor.Kept(m.Index, m.Length);
        }
    }

    /// <summary>
    /// Where the next match kept may start. Each search starts past the last kept: an engine handing back a match that
    /// starts before it is searched again there once, then a unit on, so a scan always ends — .NET's compiled engine did
    /// so over "111" for (?:(.){0,2}?b+\S*(?=a){0,2}?)*|(a|), which 0.7.0 scanned for good.
    /// </summary>
    private struct Cursor
    {
        private int _retriedAt;

        public int Floor { get; private set; }

        // After a match kept: past it, or a unit on after an empty one, as Python moves on.
        public int Kept(int index, int length) => Floor = length > 0 ? index + length : index + 1;

        // After a match not kept: past the low half it started on, or where the search should have started, once.
        public int Resume(int index)
        {
            if (index >= Floor)
            {
                // A pair's low half: past it.
                Floor = index + 1;
                return Floor;
            }
            if (_retriedAt == Floor + 1)
            {
                Floor++;
            }
            _retriedAt = Floor + 1;
            return Floor;
        }
    }

    // A match not to keep: one starting before the search did, or on a pair's low half where the text holds pairs.
    private static bool Skipped(string s, int index, int floor, bool pairAware) =>
        index < floor || (pairAware && InsidePair(s, index));

    // After an empty match, the non-empty one Python 3.7 tries at the same place, or null.
    private Match? Longer(Regex regex, string s, int index, int length) =>
        length == 0 && _mayMatchEmpty && index < s.Length && Advancing(regex).Match(s, index) is { Success: true } longer
            && longer.Index == index
            ? longer
            : null;

    // The pattern anchored where the search starts and forbidden to end there: a non-empty match at that place only.
    private Regex Advancing(Regex regex)
    {
        if (Volatile.Read(ref _advancing) is not { } advancing)
        {
            Interlocked.CompareExchange(ref _advancing, new ConcurrentDictionary<Regex, Regex>(), null);
            advancing = _advancing;
        }
        return advancing.GetOrAdd(regex, r => new Regex(@"\G(?:" + r + @")(?!\G)", r.Options, r.MatchTimeout));
    }

#if NET7_0_OR_GREATER
    // Found with no Match per token: a Match each took 50 MB over a 2 MB text against 0.7.0's 5 (#1665).
    private void ValueMatches(Regex regex, string s, List<(int Start, int Length)> matches, bool pairAware)
    {
        var cursor = new Cursor();
        int position = 0;
        while (position <= s.Length)
        {
            int next = int.MaxValue;
            foreach (ValueMatch m in regex.EnumerateMatches(s.AsSpan(), position))
            {
                if (Skipped(s, m.Index, cursor.Floor, pairAware))
                {
                    next = cursor.Resume(m.Index);
                    break;
                }
                matches.Add((m.Index, m.Length));
                if (Longer(regex, s, m.Index, m.Length) is { } longer)
                {
                    matches.Add((longer.Index, longer.Length));
                    next = cursor.Kept(longer.Index, longer.Length);
                    break;
                }
                cursor.Kept(m.Index, m.Length);
            }
            position = next;
        }
    }
#endif

    private static bool InsidePair(string s, int index) => index > 0 && char.IsSurrogatePair(s, index - 1);

    // Whether the pair-aware spelling reads s: the pattern has one and s holds a surrogate.
    [MemberNotNullWhen(true, nameof(_pairAware))]
    private bool ReadsPairs(string s) => _pairAware is not null && HoldsSurrogate(s);

    private static bool HoldsSurrogate(string s)
    {
#if NET8_0_OR_GREATER
        return s.AsSpan().ContainsAnyInRange('\uD800', '\uDFFF');
#else
        for (int i = 0; i < s.Length; i++)
        {
            if (char.IsSurrogate(s[i]))
            {
                return true;
            }
        }
        return false;
#endif
    }

    // re.findall's item: the match without a group, the group's last capture with one, and the
    // empty string where the group took no part in the match.
    private (int Start, int Length) Token(Match m)
    {
        if (_group == 0)
        {
            return (m.Index, m.Length);
        }
        Group g = m.Groups[_group];
        return g.Success ? (g.Index, g.Length) : (m.Index, 0);
    }

    /// <summary>Every maximal run of word characters at least <paramref name="minimum"/> code points long.</summary>
    private static void ScanRuns(string s, int minimum, List<(int Start, int Length)> matches)
    {
        int i = 0;
        while (i < s.Length)
        {
            int width = WordWidth(s, i);
            if (width == 0)
            {
                i += char.IsSurrogatePair(s, i) ? 2 : 1;
                continue;
            }

            int start = i;
            int run = 0;
            while (width > 0)
            {
                i += width;
                run++;
                width = i < s.Length ? WordWidth(s, i) : 0;
            }
            if (run >= minimum)
            {
                matches.Add((start, i - start));
            }
        }
    }

    /// <summary>The UTF-16 width of the word character at <paramref name="i"/>, or 0 when it is none.</summary>
    private static int WordWidth(string s, int i)
    {
        char c = s[i];
        if (c < 0x80)
        {
            return c is (>= 'a' and <= 'z') or (>= 'A' and <= 'Z') or (>= '0' and <= '9') or '_' ? 1 : 0;
        }
        if (char.IsSurrogatePair(s, i))
        {
            return IsWord(CharUnicodeInfo.GetUnicodeCategory(s, i)) ? 2 : 0;
        }
        return IsWord(CharUnicodeInfo.GetUnicodeCategory(c)) ? 1 : 0;
    }

    // Python's \w over every scalar: a letter or a number of any kind (underscore is ASCII).
    private static bool IsWord(UnicodeCategory c) => c is UnicodeCategory.UppercaseLetter
        or UnicodeCategory.LowercaseLetter or UnicodeCategory.TitlecaseLetter or UnicodeCategory.ModifierLetter
        or UnicodeCategory.OtherLetter or UnicodeCategory.DecimalDigitNumber or UnicodeCategory.LetterNumber
        or UnicodeCategory.OtherNumber;

    /// <summary>
    /// The pair-aware spelling: interpreted for a text of <see cref="Interpreted"/> units or fewer until the texts it
    /// scanned pass <see cref="CompiledAfter"/> units, compiled otherwise. Compiling the spelling of <c>\w</c>, whose
    /// supplementary set is 5,000 characters, took 10 ms and 200 KB, where 0.7.0's whole first fit took 1 ms;
    /// interpreted, it scans a short text in 50 µs (#1665).
    /// </summary>
    /// <remarks>
    /// No match timeout, as Python has none: <c>\W+\W+\d</c> walks back cubically over supplementary letters, 18 s in
    /// Python over 4,096 units, where 0.7.0, reading none as one, took 1 ms; the maintainer chose this on 2026-10-09 (#1666).
    /// </remarks>
    private sealed class PairAwareRegex
    {
        private const long CompiledAfter = 1 << 16;

        // Longer than this, a text is read compiled: interpreted, \W+\d over 2,000 units of emoji took a second against
        // 150 ms, and over 20,000, 100 s against 3.
        private const int Interpreted = 1 << 8;

        private readonly Lazy<Regex> _plain;
        private readonly Lazy<Regex> _interpreted;
        private readonly Lazy<Regex> _compiled;
        private readonly bool _interpret;
        private long _scanned;

        // PublicationOnly: a failure, a thread to translate on that could not start, is not kept for every later text.
        // The plain spelling it falls back to has no timeout either, so a text's outcome does not hang on past failures.
        public PairAwareRegex(string pattern, string plain, bool interpret)
        {
            _interpret = interpret;
            var spelled = new Lazy<string>(() => PythonPattern.Translate(pattern), LazyThreadSafetyMode.PublicationOnly);
            _plain = Build(() => plain, RegexOptions.Compiled, Regex.InfiniteMatchTimeout);
            // Bounded, as the interpreter backtracks several times slower: a text that runs past the second, \W+\W+\W+\d
            // over 128 emoji, is read again compiled and unbounded.
            _interpreted = Build(() => spelled.Value, RegexOptions.None, RegexDefaults.MatchTimeout);
            _compiled = Build(() => spelled.Value, RegexOptions.Compiled, Regex.InfiniteMatchTimeout);
        }

        /// <summary>The compiled spelling, unbounded, or the plain one where translating fails.</summary>
        public Regex Unbounded => Or(_compiled);

        private static Lazy<Regex> Build(Func<string> spelling, RegexOptions options, TimeSpan timeout) => new(
            () => new Regex(spelling(), options | RegexOptions.CultureInvariant, timeout),
            LazyThreadSafetyMode.PublicationOnly);

        public Regex For(string s) =>
            Or(_interpret && Interlocked.Add(ref _scanned, s.Length) <= CompiledAfter && s.Length <= Interpreted ? _interpreted : _compiled);

        private Regex Or(Lazy<Regex> spelled)
        {
            try
            {
                return spelled.Value;
            }
            // CA1031: whatever stops the translation, this text is read with the plain spelling, numbered alike.
#pragma warning disable CA1031
            catch (Exception)
#pragma warning restore CA1031
            {
                return _plain.Value;
            }
        }
    }
}
