using System.Globalization;
using System.Runtime.CompilerServices;
using System.Text;

namespace Lodestar.Text.Internal;

internal static partial class PythonPattern
{
    // Python's MAXREPEAT (2**32 - 1), and a width standing for "unbounded" that sums without overflowing.
    private const long MaxRepeat = uint.MaxValue;
    private const long Unbounded = long.MaxValue / 4;

    // Python 3.12's parser recurses twice per group, once per conditional, and runs out of its default 1,000 frames past
    // 495 nested groups or 990 conditionals, fewer under a deeper caller: a margin below both, read as written past it.
    private const int MaxFrames = 960;

    private static bool IsVerboseSpace(char c) => c is ' ' or '\t' or '\n' or '\r' or '\v' or '\f';

    private static bool IsAsciiDigit(char c) => c is >= '0' and <= '9';

    private static bool Has(string s, char c) => Position(s, c) >= 0;

    private static string Without(string s, char c) => string.Concat(s.Where(x => x != c));

    private static int Position(string s, char c)
    {
        for (int k = 0; k < s.Length; k++)
        {
            if (s[k] == c)
            {
                return k;
            }
        }
        return -1;
    }

    /// <summary>
    /// Reads a pattern as Python's <c>re._parser</c> does — its escapes, repeats, groups, flags, comments and look-behind
    /// widths — and spells each part so .NET reads it alike. A construct Python refuses throws <see cref="NotPython"/>,
    /// and the token pattern then reads the pattern as 0.7.0 did, by .NET as written (#1650, #1662, #1663).
    /// </summary>
    /// <remarks>
    /// Comments, and a verbose pattern's whitespace, are dropped rather than handed to .NET, which delimits them on other
    /// rules: a <c>[</c> or a <c>)</c> inside one then never moves a class or a group. Every capturing group stays one,
    /// in order, so the groups are numbered as Python numbers them.
    /// </remarks>
    private sealed class Translator
    {
        private readonly string _p;
        private readonly Spelling _spelling;
        private readonly StringBuilder _out;

        // Index n holds group n's width once it is closed, null while it is open; index 0 stands for the whole match.
        private readonly List<(long Lo, long Hi)?> _groups = [(0, 0)];
        private List<int>? _conditionalGroups;
        private int? _lookbehindGroups;
        private bool _globalVerbose;

        // Where the global flags written at the start of the pattern end.
        private int _flagsEnd;

        // Python's IGNORECASE, folded here as _sre folds it rather than handed to .NET, which folds otherwise (#1668).
        private bool _ignoreCase;

        // The last item's class over UTF-16 units when it takes every surrogate and every pair whole, '.', \S or [^a]:
        // repeated without bound, it is spelled as a loop of units, which .NET searches as 0.7.0's pattern (#1666).
        private string? _unitClass;

        // The last item as a class body when Python's parser folds it into a charset, a literal, a class that is not
        // negated or a category; and the branch's, when it is that one item and nothing more.
        private SetItem? _setItem;
        private SetItem? _branchSet;
        private int _depth;
        private int _lookbehinds;
        private int _i;

        public Translator(string pattern, Spelling spelling)
        {
            _p = pattern;
            _spelling = spelling;
            _out = new StringBuilder(pattern.Length * 4);
        }

        private enum Kind
        {
            None,
            At,
            Repeat,
            Item,
        }

        // Whether the pattern can match the empty string, its least width 0, as Python's getwidth counts it.
        public bool MayMatchEmpty { get; private set; }

        // Whether a repeat binds an item that can match the empty string, (?:a|)+?, on which .NET's two engines disagree:
        // such a pattern is read compiled whatever the text's length, as the plain spelling is.
        public bool RepeatsEmpty { get; private set; }

        // Whether a lazy repeat with a bound of 2 or more binds a capturing group, which .NET's two engines read apart
        // (#1676).
        public bool CapturesLazily { get; private set; }

        public string? Result { get; set; }

        public string Run()
        {
            MayMatchEmpty = ParseAlternation(verbose: false, top: true).Lo == 0;
            if (_i < _p.Length)
            {
                throw NotPython();
            }
            if (_conditionalGroups is { } conditional && conditional.Exists(g => g >= _groups.Count))
            {
                throw NotPython();
            }

            return _out.ToString();
        }

        private (long Lo, long Hi) ParseAlternation(bool verbose, bool top)
        {
            long lo = Unbounded;
            long hi = 0;
            bool first = true;
            int start = _out.Length;
            List<SetItem>? sets = null;
            bool oneItemEach = true;
            while (true)
            {
                // A global (?x) holds for the branches after the first one too, as Python reads it.
                (long Lo, long Hi) branch = ParseSequence(verbose || (top && _globalVerbose), top && first);
                lo = Math.Min(lo, branch.Lo);
                hi = Math.Max(hi, branch.Hi);
                oneItemEach &= _branchSet is not null;
                if (oneItemEach)
                {
                    (sets ??= []).Add(_branchSet!.Value);
                }
                if (_i >= _p.Length || _p[_i] != '|')
                {
                    if (oneItemEach && !first)
                    {
                        MergeBranches(MergeStart(start), sets!);
                    }
                    return (lo, hi);
                }
                _i++;
                _out.Append('|');
                first = false;
            }
        }

        // Past the global flags the first branch wrote, which stand only at the very start: \w|\w spelled the word pairs
        // twice (#1672).
        private int MergeStart(int start) => Math.Max(start, _flagsEnd);

        // Branches of one literal, class or category each are one charset to Python's parser, which backtracks once per
        // position over them: as alternatives, (?:\w|\S)+! over 30 letters ran past the timeout here (#1666).
        private void MergeBranches(int start, List<SetItem> items)
        {
            _out.Length = start;
            _out.Append(PythonPattern.Union(items, _spelling, _ignoreCase));
            GuardInLookbehind(start, Kind.Item);
        }

        /// <summary>One branch: a sequence of items, each possibly repeated; its width as Python's <c>getwidth</c> sums it.</summary>
        private (long Lo, long Hi) ParseSequence(bool verbose, bool first)
        {
            long lo = 0;
            long hi = 0;
            var kind = Kind.None;
            (long Lo, long Hi) last = (0, 0);
            int itemStart = _out.Length;
            int groupsBefore = _groups.Count;
            int items = 0;
            SetItem? set = null;
            while (_i < _p.Length && _p[_i] is not ('|' or ')'))
            {
                if (TrySkip(verbose, first && kind == Kind.None))
                {
                    // A global (?x) holds from where it stands; a scoped (?-x:...) inside the sequence is not undone.
                    verbose |= first && _globalVerbose;
                    continue;
                }

                char c = _p[_i++];
                if (c is '?' or '*' or '+' or '{' && TryRepeat(c, kind, itemStart, _groups.Count > groupsBefore, ref last))
                {
                    kind = Kind.Repeat;
                    set = null;
                    continue;
                }

                lo = Math.Min(lo + last.Lo, Unbounded);
                hi = Math.Min(hi + last.Hi, Unbounded);
                itemStart = _out.Length;
                groupsBefore = _groups.Count;
                _unitClass = null;
                _setItem = null;
                items++;
                (kind, last) = c switch
                {
                    '\\' => ReadEscape(),
                    '[' => ReadClass(),
                    '(' => Group(verbose),
                    '.' => Dot(),
                    '^' => Emit("^", Kind.At, 0),
                    '$' => Emit("$", Kind.At, 0),
                    // A '{' that opens no repeat is a literal, as Python reads it.
                    _ => Literal(CodePointAt(_i - 1)),
                };
                GuardInLookbehind(itemStart, kind);
                set = _setItem;
            }

            _branchSet = items == 1 ? set : null;
            return (Math.Min(lo + last.Lo, Unbounded), Math.Min(hi + last.Hi, Unbounded));
        }

        /// <summary>
        /// Moves past what writes nothing and leaves the item before it the one a repeat binds: a verbose pattern's
        /// whitespace and comment, a <c>(?#...)</c> comment, and a global flag group.
        /// </summary>
        private bool TrySkip(bool verbose, bool atStart)
        {
            char c = _p[_i];
            if (verbose && IsVerboseSpace(c))
            {
                _i++;
                return true;
            }
            if (verbose && c == '#')
            {
                SkipUntil('\n', atEnd: false);
                return true;
            }
            if (c != '(' || _i + 2 >= _p.Length || _p[_i + 1] != '?')
            {
                return false;
            }
            if (_p[_i + 2] == '#')
            {
                SkipComment();
                return true;
            }
            return IsFlagStart(_p[_i + 2]) && TryGlobalFlags(atStart);
        }

        private void SkipComment()
        {
            _i += 2;
            SkipUntil(')', atEnd: true);
        }

        /// <summary>
        /// Moves past <paramref name="end"/>, read as Python's tokenizer reads it: an escaped one, <c>\)</c> or a <c>\</c>
        /// before a newline, is a token of its own and ends nothing. Whether the pattern's end refuses it is <paramref name="atEnd"/>.
        /// </summary>
        private void SkipUntil(char end, bool atEnd)
        {
            _i++;
            while (_i < _p.Length)
            {
                char c = _p[_i++];
                if (c == end)
                {
                    return;
                }
                if (c == '\\')
                {
                    // Python's "bad escape (end of pattern)", comment or not.
                    _i = _i < _p.Length ? _i + 1 : throw NotPython();
                }
            }
            if (atEnd)
            {
                throw NotPython();
            }
        }

        // Inside a look-behind, which .NET matches right to left, an item may end where the one before it would start
        // on a low half: there each item checks it does not start inside a pair, as every item did before #1665.
        private void GuardInLookbehind(int itemStart, Kind kind)
        {
            if (_lookbehinds > 0 && _spelling != Spelling.SurrogateFree && kind == Kind.Item)
            {
                _out.Insert(itemStart, "(?:" + NotMidPair).Append(')');
                _unitClass = null;
            }
        }

        // A pair is one character to Python's '.', and no match starts on its low half (#1650).
        private (Kind, (long, long)) Dot()
        {
            _unitClass = ".";
            return Emit(_spelling == Spelling.SurrogateFree ? "." : AnyCodePoint, Kind.Item, 1);
        }

        /// <summary>The code point at <paramref name="index"/>, a pair being one; moves past it.</summary>
        private int CodePointAt(int index)
        {
            _i = index;
            return ReadCodePoint(_p, ref _i);
        }

        private (Kind, (long, long)) Emit(string spelled, Kind kind, long width)
        {
            _out.Append(spelled);
            return (kind, (width, width));
        }

        /// <summary>
        /// One code point outside a class. A supplementary one is grouped, so a repeat binds the pair; a surrogate is
        /// matched only where the text holds it alone, never as half a pair, as Python's str holds it (#1650).
        /// </summary>
        private (Kind, (long, long)) Literal(int codePoint)
        {
            _setItem = new SetItem(null, codePoint);
            if (_ignoreCase && CaseSiblings(codePoint) is { Count: > 1 } siblings)
            {
                _out.Append(PythonPattern.Spelled(siblings, _spelling));
                return (Kind.Item, (1, 1));
            }
            if (codePoint > 0xFFFF)
            {
                string pair = char.ConvertFromUtf32(codePoint);
                _out.Append("(?:").Append(Unit(pair[0])).Append(Unit(pair[1])).Append(')');
            }
            else if (codePoint is >= 0xD800 and <= 0xDBFF)
            {
                _out.Append("(?:").Append(Unit(codePoint)).Append(@"(?![\uDC00-\uDFFF]))");
            }
            else if (codePoint is >= 0xDC00 and <= 0xDFFF)
            {
                // Never the low half of a pair, where a search starting there would run on before the scan drops it.
                _out.Append(@"(?:(?<![\uD800-\uDBFF])").Append(Unit(codePoint)).Append(')');
            }
            else if (codePoint < 0x80 && char.IsLetterOrDigit((char)codePoint))
            {
                _out.Append((char)codePoint);
            }
            else
            {
                _out.Append(Unit(codePoint));
            }
            return (Kind.Item, (1, 1));
        }

        /// <summary>
        /// A repeat, read as Python reads it: <c>{,n}</c> is zero to n and <c>{,}</c> unbounded, which .NET reads as text,
        /// so each is written out; a <c>{</c> opening none is a literal, and false is returned for it.
        /// </summary>
        private bool TryRepeat(char c, Kind kind, int itemStart, bool captures, ref (long Lo, long Hi) last)
        {
            long min;
            long max;
            switch (c)
            {
                case '?':
                    (min, max) = (0, 1);
                    break;
                case '*':
                    (min, max) = (0, MaxRepeat);
                    break;
                case '+':
                    (min, max) = (1, MaxRepeat);
                    break;
                default:
                    if (!TryBraces(out min, out max))
                    {
                        return false;
                    }
                    break;
            }

            // Python's "nothing to repeat" and "multiple repeat".
            if (kind is Kind.None or Kind.At or Kind.Repeat)
            {
                throw NotPython();
            }

            // A bound past int.MaxValue, which only {,n} reaches, .NET reading it as text, is no bound on a string.
            long bound = max > int.MaxValue ? MaxRepeat : max;
            // Unbounded, as many code points as units past the first min - 1: a loop of units ending between two (#1666).
            bool unitLoop = _spelling != Spelling.SurrogateFree && _unitClass is not null && bound == MaxRepeat;
            if (unitLoop)
            {
                WriteUnitLoop(itemStart, min);
            }
            else
            {
                _out.Append(Quantifier(min, bound));
            }
            CapturesLazily |= LazyOrPossessive(itemStart) && captures && bound is >= 2 and < MaxRepeat && min < bound;
            if (unitLoop)
            {
                _out.Append(NotMidPair);
            }
            _unitClass = null;

            RepeatsEmpty |= last.Lo == 0;
            last = (Times(last.Lo, min), max == MaxRepeat && last.Hi > 0 ? Unbounded : Times(last.Hi, max));
            return true;
        }

        // The item min - 1 times, then a loop of its units: at least one code point, ending wherever a unit does.
        private void WriteUnitLoop(int itemStart, long min)
        {
            string item = _out.ToString(itemStart, _out.Length - itemStart);
            _out.Length = itemStart;
            if (min > 1)
            {
                _out.Append("(?:").Append(item).Append("){").Append((min - 1).ToString(CultureInfo.InvariantCulture)).Append('}');
            }
            _out.Append(_unitClass).Append(min == 0 ? '*' : '+');
        }

        private static string Quantifier(long min, long bound) => (min, bound) switch
        {
            (0, 1) => "?",
            (0, MaxRepeat) => "*",
            (1, MaxRepeat) => "+",
            (_, MaxRepeat) => "{" + min.ToString(CultureInfo.InvariantCulture) + ",}",
            _ when min == bound => "{" + min.ToString(CultureInfo.InvariantCulture) + "}",
            _ => "{" + min.ToString(CultureInfo.InvariantCulture) + "," + bound.ToString(CultureInfo.InvariantCulture) + "}",
        };

        // Whether the repeat is lazy.
        private bool LazyOrPossessive(int itemStart)
        {
            if (_i < _p.Length && _p[_i] == '?')
            {
                _i++;
                _out.Append('?');
                return true;
            }
            if (_i < _p.Length && _p[_i] == '+')
            {
                // Possessive, which reaches here only after a {,n} or a {,} that .NET reads as text, the rest being
                // refused as written: an atomic group, as Python 3.11 defines it.
                _i++;
                _out.Insert(itemStart, "(?>").Append(')');
            }
            return false;
        }

        private static long Times(long width, long count)
        {
            if (width == 0 || count == 0)
            {
                return 0;
            }
            return width > Unbounded / count ? Unbounded : Math.Min(width * count, Unbounded);
        }

        private bool TryBraces(out long min, out long max)
        {
            (min, max) = (0, MaxRepeat);
            int here = _i;
            if (_i < _p.Length && _p[_i] == '}')
            {
                return false;
            }

            string low = Digits();
            string high = low;
            if (_i < _p.Length && _p[_i] == ',')
            {
                _i++;
                high = Digits();
            }
            if (_i >= _p.Length || _p[_i] != '}')
            {
                _i = here;
                return false;
            }
            _i++;

            if (low.Length > 0)
            {
                min = Count(low);
            }
            if (high.Length > 0)
            {
                max = Count(high);
                if (max < min)
                {
                    throw NotPython();
                }
            }
            return true;
        }

        private string Digits()
        {
            int start = _i;
            while (_i < _p.Length && IsAsciiDigit(_p[_i]))
            {
                _i++;
            }
            return _p.Substring(start, _i - start);
        }

        // Python's OverflowError past MAXREPEAT, leading zeros or not.
        private static long Count(string digits) => Decimal(digits) is var n && n < MaxRepeat ? n : throw NotPython();

        // The value of ASCII digits, saturated past MaxRepeat.
        private static long Decimal(string digits)
        {
            long n = 0;
            foreach (char d in digits)
            {
                n = Math.Min((n * 10) + (d - '0'), MaxRepeat);
            }
            return n;
        }

        private static bool IsFlagStart(char c) => c is 'a' or 'i' or 'L' or 'm' or 's' or 'u' or 'x' or '-';

        /// <summary>
        /// A <c>(?flags)</c> group with no colon, a global flag Python accepts only at the very start, and sets for the
        /// whole pattern. Its verbose flag is applied here and dropped; the rest are written for .NET. False when the group
        /// is a scoped one, which <see cref="Group"/> reads.
        /// </summary>
        private bool TryGlobalFlags(bool atStart)
        {
            int close = _i + 2;
            while (close < _p.Length && _p[close] is not (')' or ':' or '-'))
            {
                close++;
            }
            if (close >= _p.Length || _p[close] != ')')
            {
                return false;
            }
            if (!atStart || _p[_i + 2] == '-')
            {
                throw NotPython();
            }

            string flags = ReadFlags(_i + 2, close);
            _i = close + 1;
            if (Has(flags, 'x'))
            {
                _globalVerbose = true;
            }
            _ignoreCase |= Has(flags, 'i');
            string kept = Without(Without(flags, 'x'), 'i');
            if (kept.Length > 0)
            {
                _out.Append("(?").Append(kept).Append(')');
            }
            _flagsEnd = _out.Length;
            return true;
        }

        // Python's flags; a, u and L, which .NET refused as written, and its own n, which Python refuses, are refused here.
        private string ReadFlags(int start, int end)
        {
            for (int k = start; k < end; k++)
            {
                if (_p[k] is not ('i' or 'm' or 's' or 'x'))
                {
                    throw NotPython();
                }
            }
            return _p.Substring(start, end - start);
        }

        /// <summary>An escape outside a class, as Python's <c>_escape</c> reads it.</summary>
        private (Kind, (long, long)) ReadEscape()
        {
            if (_i >= _p.Length)
            {
                throw NotPython();
            }

            char e = _p[_i++];
            switch (e)
            {
                case 'A':
                    return Emit(@"\A", Kind.At, 0);
                case 'Z':
                    // Python's \Z is the very end; .NET's also matches before a final newline, as its \z does not (#1650).
                    return Emit(@"\z", Kind.At, 0);
                case 'b' or 'B':
                    return Emit(PythonPattern.Escape(e, _spelling)!, Kind.At, 0);
                case 'd' or 'D' or 's' or 'S' or 'w' or 'W':
                    _unitClass = e == 'S' ? PythonPattern.Escape('S', Spelling.SurrogateFree) : null;
                    _setItem = new SetItem("\\" + e, 0);
                    return Emit(PythonPattern.Escape(e, _spelling)!, Kind.Item, 1);
                case 'a':
                    return Literal(7);
                case 'f':
                    return Literal(12);
                case 'n':
                    return Literal(10);
                case 'r':
                    return Literal(13);
                case 't':
                    return Literal(9);
                case 'v':
                    return Literal(11);
                case 'x':
                    return Literal(Hex(_p, ref _i, 2));
                case 'u':
                    return Literal(Hex(_p, ref _i, 4));
                case 'U':
                    return Literal(Hex(_p, ref _i, 8));
                case '0':
                    return Literal(OctalAfterZero());
                case >= '1' and <= '9':
                    return NumberedEscape(e);
                case >= 'a' and <= 'z' or >= 'A' and <= 'Z':
                    // Python's "bad escape" — \z, \G, \p, \k, \c and the like — which .NET as written reads instead; \N{...},
                    // which Python reads by name, .NET refused as written.
                    throw NotPython();
                default:
                    return Literal(CodePointAt(_i - 1));
            }
        }

        private int OctalAfterZero()
        {
            int value = 0;
            for (int k = 0; k < 2 && _i < _p.Length && _p[_i] is >= '0' and <= '7'; k++)
            {
                value = (value * 8) + (_p[_i++] - '0');
            }
            return value;
        }

        /// <summary>Three octal digits, or else a group reference of one or two digits, as Python decides between them.</summary>
        private (Kind, (long, long)) NumberedEscape(char first)
        {
            string digits = first.ToString();
            if (_i < _p.Length && IsAsciiDigit(_p[_i]))
            {
                digits += _p[_i++];
                if (digits[0] <= '7' && digits[1] <= '7' && _i < _p.Length && _p[_i] is >= '0' and <= '7')
                {
                    int value = ((digits[0] - '0') * 64) + ((digits[1] - '0') * 8) + (_p[_i++] - '0');
                    return value <= 255 ? Literal(value) : throw NotPython();
                }
            }

            int group = int.Parse(digits, CultureInfo.InvariantCulture);
            if (group >= _groups.Count || _groups[group] is not { } width)
            {
                throw NotPython();
            }
            CheckLookbehindGroup(group);
            // A group holding a lone surrogate matches none that is half a pair, as Python's str holds none (#1650).
            // Under IGNORECASE, .NET's own folding compares the group's text: the closest a back-reference gets.
            _out.Append(_ignoreCase ? @"(?i:\" : @"(?:\").Append(group.ToString(CultureInfo.InvariantCulture));
            _out.Append(_spelling == Spelling.SurrogateFree ? ")" : NotMidPair + ")");
            return (Kind.Item, width);
        }

        // Python refuses, inside a look-behind, a reference to a group still open or opened inside it.
        private void CheckLookbehindGroup(int group)
        {
            if (_lookbehindGroups is { } opened && (_groups[group] is null || group >= opened))
            {
                throw NotPython();
            }
        }

        private (Kind, (long, long)) ReadClass()
        {
            int end = ClassEnd(_p, _i - 1);
            if (end < 0)
            {
                throw NotPython();
            }
            string body = _p.Substring(_i, end - _i);
            _out.Append(PythonPattern.Class(body, _spelling, _ignoreCase, out _unitClass));
            _setItem = body.Length > 0 && body[0] == '^' ? null : new SetItem(body, 0);
            _i = end + 1;
            return (Kind.Item, (1, 1));
        }

        /// <summary>A group of any kind but a comment or a global flag, which the sequence reads.</summary>
        private (Kind, (long, long)) Group(bool verbose)
        {
            // Counted as Python's frames: a conditional's branches are read without a Body, in one frame where a group takes two.
            int frames = _i + 1 < _p.Length && _p[_i] == '?' && _p[_i + 1] == '(' ? 1 : 2;
            _depth += frames;
            if (_depth > MaxFrames)
            {
                throw NotPython();
            }
            // Each level takes about 1 KB of stack; Translate starts again on a thread holding enough, where running out
            // would end the process, as no catch survives a stack overflow (#1667).
            RuntimeHelpers.EnsureSufficientExecutionStack();
            (Kind, (long, long)) group = GroupBody(verbose);
            _depth -= frames;
            _unitClass = null;
            _setItem = null;
            return group;
        }

        private (Kind, (long, long)) GroupBody(bool verbose)
        {
            if (_i >= _p.Length || _p[_i] != '?')
            {
                _groups.Add(null);
                int group = _groups.Count - 1;
                _out.Append('(');
                (long Lo, long Hi) width = Body(verbose);
                _groups[group] = width;
                return (Kind.Item, width);
            }

            _i++;
            if (_i >= _p.Length)
            {
                throw NotPython();
            }
            char k = _p[_i++];
            switch (k)
            {
                case ':':
                    _out.Append("(?:");
                    return (Kind.Item, Body(verbose));
                case '>':
                    _out.Append("(?>");
                    return (Kind.Item, Body(verbose));
                case '=' or '!':
                    _out.Append("(?").Append(k);
                    Body(verbose);
                    return (Kind.Item, (0, 0));
                case '<':
                    return Lookbehind(verbose);
                case '(':
                    return Conditional(verbose);
                default:
                    if (IsFlagStart(k))
                    {
                        return ScopedFlags(verbose);
                    }
                    // (?P...), which .NET refused as written, and .NET's own (?'name'...), which Python refuses.
                    throw NotPython();
            }
        }

        /// <summary>The group's contents and its closing parenthesis, written; their width.</summary>
        private (long Lo, long Hi) Body(bool verbose)
        {
            (long Lo, long Hi) width = ParseAlternation(verbose, top: false);
            if (_i >= _p.Length || _p[_i] != ')')
            {
                throw NotPython();
            }
            _i++;
            _out.Append(')');
            return width;
        }

        // A look-behind Python compiles only at a fixed width; .NET's own (?<name>...) Python refuses.
        private (Kind, (long, long)) Lookbehind(bool verbose)
        {
            if (_i >= _p.Length || _p[_i] is not ('=' or '!'))
            {
                throw NotPython();
            }
            _out.Append("(?<").Append(_p[_i++]);
            int? outer = _lookbehindGroups;
            _lookbehindGroups ??= _groups.Count;
            _lookbehinds++;
            (long Lo, long Hi) width = Body(verbose);
            _lookbehinds--;
            _lookbehindGroups = outer;
            // Python 3.12's "looks too much behind" past MAXCODE, 2**32 - 1, which MaxRepeat equals.
            if (width.Lo != width.Hi || width.Hi > MaxRepeat)
            {
                throw NotPython();
            }
            return (Kind.Item, (0, 0));
        }

        // (?(n)yes|no) by group number: a named group never reaches here, (?P<...>) being refused by .NET as written.
        private (Kind, (long, long)) Conditional(bool verbose)
        {
            int close = _p.IndexOf(')', _i);
            if (close <= _i)
            {
                throw NotPython();
            }
            string name = _p.Substring(_i, close - _i);
            if (!name.All(IsAsciiDigit) || (int)Math.Min(Decimal(name), int.MaxValue) is var group && group == 0)
            {
                throw NotPython();
            }
            (_conditionalGroups ??= []).Add(group);
            if (_lookbehindGroups is not null)
            {
                if (group >= _groups.Count)
                {
                    throw NotPython();
                }
                CheckLookbehindGroup(group);
            }
            _i = close + 1;
            _out.Append("(?(").Append(group.ToString(CultureInfo.InvariantCulture)).Append(')');

            (long Lo, long Hi) yes = ParseSequence(verbose, first: false);
            (long Lo, long Hi) no = (0, 0);
            bool hasNo = false;
            if (_i < _p.Length && _p[_i] == '|')
            {
                _i++;
                _out.Append('|');
                no = ParseSequence(verbose, first: false);
                hasNo = true;
                if (_i < _p.Length && _p[_i] == '|')
                {
                    throw NotPython();
                }
            }
            if (_i >= _p.Length || _p[_i] != ')')
            {
                throw NotPython();
            }
            _i++;
            _out.Append(')');
            return (Kind.Item, (hasNo ? Math.Min(yes.Lo, no.Lo) : 0, Math.Max(yes.Hi, no.Hi)));
        }

        // (?flags-flags:...), its verbose flag applied here and dropped for .NET, which would read whitespace otherwise.
        private (Kind, (long, long)) ScopedFlags(bool verbose)
        {
            int start = _i - 1;
            while (_i < _p.Length && _p[_i] != ':')
            {
                // A ')' is a global flag past the start of the pattern, or a removal without a colon: Python refuses both.
                if (_p[_i] == ')')
                {
                    throw NotPython();
                }
                _i++;
            }
            if (_i >= _p.Length)
            {
                throw NotPython();
            }

            string all = _p.Substring(start, _i - start);
            int dash = Position(all, '-');
            string add = ReadFlags(start, dash < 0 ? _i : start + dash);
            string remove = dash < 0 ? "" : ReadFlags(start + dash + 1, _i);
            // Python's "missing flag", and "flag turned on and off".
            if ((dash >= 0 && remove.Length == 0) || add.Any(f => Has(remove, f)))
            {
                throw NotPython();
            }
            _i++;

            bool inner = (verbose || Has(add, 'x')) && !Has(remove, 'x');
            bool outerIgnoreCase = _ignoreCase;
            _ignoreCase = (_ignoreCase || Has(add, 'i')) && !Has(remove, 'i');
            string keepRemove = Without(Without(remove, 'x'), 'i');
            _out.Append("(?").Append(Without(Without(add, 'x'), 'i'));
            if (keepRemove.Length > 0)
            {
                _out.Append('-').Append(keepRemove);
            }
            _out.Append(':');
            (long, long) width = Body(inner);
            _ignoreCase = outerIgnoreCase;
            return (Kind.Item, width);
        }
    }
}
