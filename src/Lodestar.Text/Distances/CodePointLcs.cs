using System.Buffers;
using Lodestar.Text.Internal;

namespace Lodestar.Text.Distances;

/// <summary>The code-point LCS length, over the same bit-parallel kernel the UTF-16 mode reaches.</summary>
/// <remarks>
/// The code-point mode decoded both operands to <see cref="int"/> and ran the dynamic program, so
/// every improvement to the character kernel left it further behind: 4.6× the UTF-16 mode at 20
/// characters, 62× at 512 (#675). A code point equals another exactly when their renamings do, so
/// an injective renaming to <see cref="char"/> reaches that kernel with the same answer.
/// </remarks>
internal static class CodePointLcs
{
    private const char FirstSurrogate = '\uD800';
    private const char LastSurrogate = '\uDFFF';
    private const int SurrogateValues = LastSurrogate - FirstSurrogate + 1;

    /// <summary>
    /// The LCS length of <paramref name="a"/> and <paramref name="b"/> as code points, with the
    /// code-point length of each.
    /// </summary>
    internal static int SubsequenceLength(
        ReadOnlySpan<char> a, ReadOnlySpan<char> b, out int lengthA, out int lengthB)
    {
        // Without a surrogate, a lone one or a pair, the two sequences are the same sequence.
        if (!HasSurrogate(a) && !HasSurrogate(b))
        {
            lengthA = a.Length;
            lengthB = b.Length;
            return Lcs.SubsequenceLengthChars(a, b);
        }

        char[] renamedA = ArrayPool<char>.Shared.Rent(Math.Max(1, a.Length));
        char[] renamedB = ArrayPool<char>.Shared.Rent(Math.Max(1, b.Length));
        try
        {
            if (Renaming.TryRename(a, b, renamedA, renamedB, out lengthA, out lengthB))
            {
                return Lcs.SubsequenceLengthChars(renamedA.AsSpan(0, lengthA), renamedB.AsSpan(0, lengthB));
            }
        }
        finally
        {
            ArrayPool<char>.Shared.Return(renamedA);
            ArrayPool<char>.Shared.Return(renamedB);
        }

        return OverDecoded(a, b, out lengthA, out lengthB);
    }

    private static bool HasSurrogate(ReadOnlySpan<char> text)
    {
#if NET8_0_OR_GREATER
        return text.IndexOfAnyInRange(FirstSurrogate, LastSurrogate) >= 0;
#else
        foreach (char c in text)
        {
            if (c >= FirstSurrogate && c <= LastSurrogate)
            {
                return true;
            }
        }
        return false;
#endif
    }

    /// <summary>The dynamic program over decoded code points, for more distinct astral ones than there are free names.</summary>
    private static int OverDecoded(ReadOnlySpan<char> a, ReadOnlySpan<char> b, out int lengthA, out int lengthB)
    {
        int[] decodedA = ArrayPool<int>.Shared.Rent(Math.Max(1, a.Length));
        int[] decodedB = ArrayPool<int>.Shared.Rent(Math.Max(1, b.Length));
        try
        {
            lengthA = CodePoints.Decode(a, decodedA);
            lengthB = CodePoints.Decode(b, decodedB);
            return Lcs.SubsequenceLength<int>(decodedA.AsSpan(0, lengthA), decodedB.AsSpan(0, lengthB));
        }
        finally
        {
            ArrayPool<int>.Shared.Return(decodedA);
            ArrayPool<int>.Shared.Return(decodedB);
        }
    }

    /// <summary>Renames each astral code point to a surrogate value neither operand holds alone.</summary>
    /// <remarks>
    /// A code point below U+10000 keeps its value, a lone surrogate included, since
    /// <see cref="CodePoints.Decode"/> passes one through as itself. Only a well-formed pair needs
    /// a name, and the 2,048 surrogate values are the ones text can hold alone and rarely does, so a
    /// name is a value no lone surrogate of either operand has taken. More distinct astral code
    /// points than free values is refused, and the caller takes the dynamic program.
    /// </remarks>
    private ref struct Renaming
    {
        private readonly Span<ulong> _taken;
        private readonly Span<int> _keys;
        private readonly Span<char> _names;
        private readonly int _mask;
        private int _nextFree;

        private Renaming(Span<ulong> taken, Span<int> keys, Span<char> names)
        {
            _taken = taken;
            _keys = keys;
            _names = names;
            _mask = keys.Length - 1;
            _nextFree = 0;
        }

        internal static bool TryRename(
            ReadOnlySpan<char> a, ReadOnlySpan<char> b, Span<char> renamedA, Span<char> renamedB,
            out int lengthA, out int lengthB)
        {
            Span<ulong> taken = stackalloc ulong[SurrogateValues / 64];
            taken.Clear();
            int pairs = MarkLoneSurrogates(a, taken) + MarkLoneSurrogates(b, taken);

            // Twice the distinct names it can ever hold, a power of two, so a probe always ends.
            int slots = 16;
            while (slots < 2 * Math.Min(pairs, SurrogateValues))
            {
                slots *= 2;
            }

            int[] keys = ArrayPool<int>.Shared.Rent(slots);
            char[] names = ArrayPool<char>.Shared.Rent(slots);
            try
            {
                keys.AsSpan(0, slots).Clear();
                var renaming = new Renaming(taken, keys.AsSpan(0, slots), names.AsSpan(0, slots));
                lengthB = 0;
                return renaming.TryWrite(a, renamedA, out lengthA)
                    && renaming.TryWrite(b, renamedB, out lengthB);
            }
            finally
            {
                ArrayPool<int>.Shared.Return(keys);
                ArrayPool<char>.Shared.Return(names);
            }
        }

        /// <summary>Marks the value of each lone surrogate in <paramref name="text"/>, and returns how many pairs it holds.</summary>
        private static int MarkLoneSurrogates(ReadOnlySpan<char> text, Span<ulong> taken)
        {
            int pairs = 0;
            int i = 0;
            while (i < text.Length)
            {
                char c = text[i];
                if (IsPairAt(text, i))
                {
                    pairs++;
                    i += 2;
                    continue;
                }
                if (c >= FirstSurrogate && c <= LastSurrogate)
                {
                    int value = c - FirstSurrogate;
                    taken[value >> 6] |= 1UL << (value & 63);
                }
                i++;
            }
            return pairs;
        }

        private bool TryWrite(ReadOnlySpan<char> text, Span<char> renamed, out int length)
        {
            length = 0;
            int i = 0;
            while (i < text.Length)
            {
                if (!IsPairAt(text, i))
                {
                    renamed[length++] = text[i];
                    i++;
                    continue;
                }
                if (!TryName(char.ConvertToUtf32(text[i], text[i + 1]), out char name))
                {
                    return false;
                }
                renamed[length++] = name;
                i += 2;
            }
            return true;
        }

        private static bool IsPairAt(ReadOnlySpan<char> text, int at) =>
            char.IsHighSurrogate(text[at]) && at + 1 < text.Length && char.IsLowSurrogate(text[at + 1]);

        /// <summary>The name of an astral code point, handing out the next free value the first time it is seen.</summary>
        private bool TryName(int codePoint, out char name)
        {
            // An astral code point is at least U+10000, so a zero key is an empty slot.
            int slot = (int)((uint)(codePoint * -1640531527) >> 16) & _mask;
            while (_keys[slot] != 0)
            {
                if (_keys[slot] == codePoint)
                {
                    name = _names[slot];
                    return true;
                }
                slot = (slot + 1) & _mask;
            }

            while (_nextFree < SurrogateValues && (_taken[_nextFree >> 6] & (1UL << (_nextFree & 63))) != 0)
            {
                _nextFree++;
            }
            if (_nextFree == SurrogateValues)
            {
                name = default;
                return false;
            }

            name = (char)(FirstSurrogate + _nextFree);
            _nextFree++;
            _keys[slot] = codePoint;
            _names[slot] = name;
            return true;
        }
    }
}
