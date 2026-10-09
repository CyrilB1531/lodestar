namespace Lodestar.Text.Internal;

// CA1308: _sre compares lowercases, so the lowercase is what is asked for here, not a normalisation that could take
// the uppercase instead.
#pragma warning disable CA1308

internal static partial class PythonPattern
{
    // The letters Unicode gave a case after 15.0, which Python 3.12 matches only as themselves: without them the table
    // equals _sre.unicode_tolower with _casefix over every code point, where .NET 10's tables fold 110 more (#1668).
    private static bool CasedAfterUnicode15(int codePoint) => codePoint is (>= 0x1C89 and <= 0x1C8A)
        or (>= 0xA7CB and <= 0xA7CF) or (>= 0xA7D2 and <= 0xA7D5) or (>= 0xA7DA and <= 0xA7DC)
        or (>= 0x10D50 and <= 0x10D65) or (>= 0x10D70 and <= 0x10D85) or (>= 0x16EA0 and <= 0x16ED3);

    // Lowercase letters sharing an uppercase with another, which Python's re also matches each other under IGNORECASE:
    // the groups of CPython 3.12's Lib/re/_casefix.py, Unicode data, which .NET's simple case mapping cannot derive.
    private static readonly int[][] ExtraCases =
    [
        [0x69, 0x131], [0x73, 0x17F], [0xB5, 0x3BC], [0x345, 0x3B9, 0x1FBE], [0x390, 0x1FD3], [0x3B0, 0x1FE3],
        [0x3B2, 0x3D0], [0x3B5, 0x3F5], [0x3B8, 0x3D1], [0x3BA, 0x3F0], [0x3C0, 0x3D6], [0x3C1, 0x3F1], [0x3C2, 0x3C3],
        [0x3C6, 0x3D5], [0x432, 0x1C80], [0x434, 0x1C81], [0x43E, 0x1C82], [0x441, 0x1C83], [0x442, 0x1C84, 0x1C85],
        [0x44A, 0x1C86], [0x463, 0x1C87], [0x1C88, 0xA64B], [0x1E61, 0x1E9B], [0xFB05, 0xFB06],
    ];

    private static readonly Lazy<CaseTable> Cases = new(() => new CaseTable());

    /// <summary>
    /// What Python's <c>re</c> matches <paramref name="codePoint"/> with under IGNORECASE, itself included: every code
    /// point whose lowercase is its lowercase or one <see cref="ExtraCases"/> pairs with it, as <c>_sre</c> compares
    /// lowercases; empty where nothing else. The lowercase is the runtime's simple mapping, as Python's is (#1668).
    /// </summary>
    private static ArraySegment<int> CaseSiblings(int codePoint) => Cases.Value.Siblings(codePoint);

    /// <summary>The code points a case joins to others, sorted, so a range is folded without walking it whole.</summary>
    private static int[] CasedCodePoints() => Cases.Value.Cased;

    private sealed class CaseTable
    {
        private const int Chunk = 4096;
        private const int DottedCapitalI = 0x130;

        // Each cased code point's class, and the classes laid end to end: flat arrays of a few thousand, where
        // dictionaries of lists held 400 KB for good.
        private readonly int[] _classOf;
        private readonly int[] _classStart;
        private readonly int[] _members;

        public CaseTable()
        {
            // Each lowercase another code point lowers to, with every code point lowering to it, itself first.
            var byLower = new Dictionary<int, List<int>>();
            for (int c = 0; c < 0x10000; c++)
            {
                if (c is < 0xD800 or > 0xDFFF)
                {
                    Add(byLower, c, Lower(c));
                }
            }
            AddPlaneOne(byLower);
            foreach (int[] group in ExtraCases)
            {
                var joined = new List<int>();
                foreach (int lower in group)
                {
                    joined.AddRange(byLower.TryGetValue(lower, out List<int>? members) ? members : [lower]);
                }
                foreach (int lower in group)
                {
                    byLower[lower] = joined;
                }
            }

            List<int>[] classes = [.. byLower.Values.Distinct()];
            var classOf = new SortedDictionary<int, int>();
            var starts = new List<int>();
            var flat = new List<int>();
            for (int id = 0; id < classes.Length; id++)
            {
                starts.Add(flat.Count);
                foreach (int member in classes[id].Distinct())
                {
                    flat.Add(member);
                    classOf[member] = id;
                }
            }
            starts.Add(flat.Count);
            Cased = [.. classOf.Keys];
            _classOf = [.. classOf.Values];
            _classStart = [.. starts];
            _members = [.. flat];
        }

        public int[] Cased { get; }

        public ArraySegment<int> Siblings(int codePoint)
        {
            int k = Array.BinarySearch(Cased, codePoint);
            if (k < 0)
            {
                return new ArraySegment<int>(_members, 0, 0);
            }
            int id = _classOf[k];
            return new ArraySegment<int>(_members, _classStart[id], _classStart[id + 1] - _classStart[id]);
        }

        private static void AddPlaneOne(Dictionary<int, List<int>> byLower)
        {
            // Plane 1, the one supplementary plane with cased letters, a chunk at a time lowered as one string: all sixteen
            // took 52 ms and 8.7 MB at the first IGNORECASE pattern.
            char[] units = new char[2 * Chunk];
            for (int start = 0x10000; start < 0x20000; start += Chunk)
            {
                for (int k = 0; k < Chunk; k++)
                {
                    int v = start + k - 0x10000;
                    units[2 * k] = (char)(0xD800 + (v >> 10));
                    units[(2 * k) + 1] = (char)(0xDC00 + (v & 0x3FF));
                }
                string lowered = new string(units).ToLowerInvariant();
                for (int k = 0; k < Chunk; k++)
                {
                    Add(byLower, start + k, char.ConvertToUtf32(lowered[2 * k], lowered[(2 * k) + 1]));
                }
            }
        }

        // The invariant culture leaves U+0130 as it is, where Unicode's simple lowercase, which Python takes, is 'i'.
        private static int Lower(int codePoint) => codePoint == DottedCapitalI ? 'i' : char.ToLowerInvariant((char)codePoint);

        private static void Add(Dictionary<int, List<int>> byLower, int codePoint, int lower)
        {
            if (lower == codePoint || CasedAfterUnicode15(codePoint) || CasedAfterUnicode15(lower))
            {
                return;
            }
            if (!byLower.TryGetValue(lower, out List<int>? lowering))
            {
                byLower[lower] = lowering = [lower];
            }
            lowering.Add(codePoint);
        }
    }
}
