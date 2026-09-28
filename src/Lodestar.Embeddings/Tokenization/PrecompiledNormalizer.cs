using System.Buffers;
using Lodestar.Internal.Persistence;

namespace Lodestar.Embeddings.Tokenization;

/// <summary>
/// The normalization a SentencePiece model carries in its
/// <c>precompiled_charsmap</c>, applied before the text is segmented.
/// </summary>
/// <remarks>
/// Matches <c>sentencepiece</c>'s <c>Normalizer</c>. Interpreting the compiled
/// blob, not reimplementing the named rules on <c>string.Normalize</c>, is what
/// makes byte-exact parity possible — <c>docs/decisions/0005-the-proof-standard-and-the-oracle-each-family-is-frozen-from.md</c>
/// has the measurement. A malformed map raises <see cref="InvalidDataException"/>.
/// </remarks>
public sealed class PrecompiledNormalizer : IEquatable<PrecompiledNormalizer>
{
    // darts-clone packs each 32-bit unit as: bit 8 has_leaf, bit 9 offset scale,
    // bits 0-7 label (plus bit 31), bits 10-31 offset.
    private const uint HasLeafBit = 1u << 8;
    private const uint OffsetScaleBit = 1u << 9;
    private const uint LabelMask = 0x8000_00FFu;
    private const uint ValueMask = 0x7FFF_FFFFu;

    private readonly byte[] _charsMap;
    private readonly uint[] _trie;
    private readonly int _replacementsAt;

    // tokenizers' reading of the same charsmap rather than sentencepiece's: by grapheme, shortest rule (#1260).
    private readonly bool _byGrapheme;

    private PrecompiledNormalizer(byte[] charsMap, uint[] trie, int replacementsAt, bool byGrapheme = false)
    {
        _charsMap = charsMap;
        _trie = trie;
        _replacementsAt = replacementsAt;
        _byGrapheme = byGrapheme;
    }

    /// <summary>The same charsmap read as <c>tokenizers</c>' <c>Precompiled</c> normalizer reads it, for a <c>tokenizer.json</c>.</summary>
    /// <remarks>
    /// <c>spm_precompiled</c> walks the text by grapheme: a cluster under six UTF-8 bytes is replaced whole by the
    /// shortest rule matching its start, marks after that rule dropped, and a longer one character by character.
    /// On NFD text the two readings part — XLM-R's <c>nmt_nfkc</c> gives <c>Nguyên</c> here, <c>Nguyễn</c> to sentencepiece.
    /// </remarks>
    internal PrecompiledNormalizer ByGrapheme() => new(_charsMap, _trie, _replacementsAt, byGrapheme: true);

    /// <summary>Reads a normalizer from a <c>precompiled_charsmap</c>.</summary>
    /// <param name="charsMap">
    /// The blob, as carried by <c>normalizer_spec.precompiled_charsmap</c> in a
    /// <c>spiece.model</c> or, base64-encoded, by a <c>Precompiled</c> normalizer
    /// in a <c>tokenizer.json</c>.
    /// </param>
    /// <exception cref="InvalidDataException">The blob is truncated or its trie is malformed.</exception>
    public static PrecompiledNormalizer FromCharsMap(byte[] charsMap)
    {
        Guard.NotNull(charsMap);

        // 4-byte little-endian trie size, the trie, then the replacements.
        if (charsMap.Length < sizeof(uint))
        {
            throw new InvalidDataException(
                $"The precompiled charsmap is {charsMap.Length} bytes, too short to carry even its own trie size.");
        }
        uint trieBytes = (uint)charsMap[0] | ((uint)charsMap[1] << 8) | ((uint)charsMap[2] << 16) | ((uint)charsMap[3] << 24);
        if (trieBytes % sizeof(uint) != 0)
        {
            throw new InvalidDataException(
                $"The precompiled charsmap declares a {trieBytes}-byte trie, which is not a whole number of 4-byte units.");
        }
        long replacementsAt = sizeof(uint) + (long)trieBytes;
        if (replacementsAt > charsMap.Length)
        {
            throw new InvalidDataException(
                $"The precompiled charsmap declares a {trieBytes}-byte trie but carries only {charsMap.Length - sizeof(uint)} bytes after the header.");
        }

        if (trieBytes == 0)
        {
            throw new InvalidDataException("The precompiled charsmap carries an empty trie.");
        }

        var trie = new uint[trieBytes / sizeof(uint)];
        for (int i = 0; i < trie.Length; i++)
        {
            int at = sizeof(uint) + (i * sizeof(uint));
            trie[i] = (uint)charsMap[at] | ((uint)charsMap[at + 1] << 8) | ((uint)charsMap[at + 2] << 16) | ((uint)charsMap[at + 3] << 24);
        }

        return new PrecompiledNormalizer(charsMap, trie, (int)replacementsAt);
    }

    /// <summary>Number of bytes in the underlying <c>precompiled_charsmap</c>.</summary>
    public int CharsMapLength => _charsMap.Length;

    /// <summary>Rewrites <paramref name="text"/> the way the model's normalizer does.</summary>
    /// <param name="text">The text to normalize.</param>
    /// <remarks>
    /// Longest match wins, as in <c>sentencepiece</c>: at each position the longest
    /// prefix the trie knows is replaced, and a position no rule covers keeps its
    /// character. Whitespace handling — <c>remove_extra_whitespaces</c>,
    /// <c>add_dummy_prefix</c>, <c>escape_whitespaces</c> — is <em>not</em> done
    /// here; <see cref="SentencePieceTokenizer"/> applies it after this pass, which
    /// is the order the reference implementation uses.
    /// </remarks>
    /// <exception cref="InvalidDataException">The charsmap points at a replacement it does not contain.</exception>
    public string Normalize(string text)
    {
        Guard.NotNull(text);
        if (text.Length == 0)
        {
            return string.Empty;
        }

        if (_byGrapheme)
        {
            return NormalizeByGrapheme(text);
        }

        // Both buffers are rented: an encode otherwise allocated the input bytes, a list,
        // its copy and the string, each the size of the text.
        byte[] input = ArrayPool<byte>.Shared.Rent(JsonArtifact.Utf8NoBom.GetMaxByteCount(text.Length));
        byte[]? output = null;
        try
        {
            int length = JsonArtifact.Utf8NoBom.GetBytes(text, 0, text.Length, input, 0);
            output = ArrayPool<byte>.Shared.Rent(length);
            var bytes = new ReadOnlySpan<byte>(input, 0, length);
            int written = 0;
            int at = 0;
            while (at < length)
            {
                int matched = Match(bytes, at, length, shortest: false, out int replacementAt);
                if (matched == 0)
                {
                    // No rule covers this position: the character passes through whole.
                    int width = Utf8SequenceLength(bytes, at);
                    Append(ref output, ref written, bytes.Slice(at, width));
                    at += width;
                    continue;
                }
                Append(ref output, ref written, Replacement(replacementAt));
                at += matched;
            }

            return JsonArtifact.Utf8NoBom.GetString(output, 0, written);
        }
        finally
        {
            ArrayPool<byte>.Shared.Return(input);
            if (output is not null)
            {
                ArrayPool<byte>.Shared.Return(output);
            }
        }
    }

    /// <summary>Copies <paramref name="bytes"/> to the end of the rented <paramref name="output"/>, growing it by doubling.</summary>
    private static void Append(ref byte[] output, ref int written, ReadOnlySpan<byte> bytes)
    {
        if (written + bytes.Length > output.Length)
        {
            byte[] grown = ArrayPool<byte>.Shared.Rent(Math.Max(output.Length * 2, written + bytes.Length));
            output.AsSpan(0, written).CopyTo(grown);
            ArrayPool<byte>.Shared.Return(output);
            output = grown;
        }
        bytes.CopyTo(output.AsSpan(written));
        written += bytes.Length;
    }

    /// <summary>tokenizers' walk: by grapheme, a short cluster replaced whole by its shortest rule, a long one per character.</summary>
    private string NormalizeByGrapheme(string text)
    {
        byte[] input = ArrayPool<byte>.Shared.Rent(JsonArtifact.Utf8NoBom.GetMaxByteCount(text.Length));
        byte[]? output = null;
        try
        {
            int length = JsonArtifact.Utf8NoBom.GetBytes(text, 0, text.Length, input, 0);
            output = ArrayPool<byte>.Shared.Rent(length);
            var bytes = new ReadOnlySpan<byte>(input, 0, length);
            int written = 0;
            int at = 0;
            int b = 0;
            while (at < text.Length)
            {
                int units = Graphemes.Length(text, at);
                int width = Utf8Length(text, at, units);
                at += units;
                if (width < 6 && Match(bytes, b, b + width, shortest: true, out int replacementAt) > 0)
                {
                    Append(ref output, ref written, Replacement(replacementAt));
                    b += width;
                    continue;
                }

                // Character by character; a one-character cluster has just been looked up whole.
                bool single = units == 1 || (units == 2 && char.IsSurrogatePair(text, at - 2));
                for (int end = b + width; b < end;)
                {
                    int step = Utf8SequenceLength(bytes, b);
                    int rule = 0;
                    bool replaced = !single && Match(bytes, b, b + step, shortest: true, out rule) > 0;
                    Append(ref output, ref written, replaced ? Replacement(rule) : bytes.Slice(b, step));
                    b += step;
                }
            }

            return JsonArtifact.Utf8NoBom.GetString(output, 0, written);
        }
        finally
        {
            ArrayPool<byte>.Shared.Return(input);
            if (output is not null)
            {
                ArrayPool<byte>.Shared.Return(output);
            }
        }
    }

    /// <summary>How many UTF-8 bytes <paramref name="length"/> UTF-16 units encode to, without encoding them.</summary>
    private static int Utf8Length(string text, int at, int length)
    {
        int bytes = 0;
        for (int i = at; i < at + length; i++)
        {
            bytes += Utf8Width(text, i, at, at + length);
        }

        return bytes;
    }

    /// <summary>One UTF-16 unit's share of the UTF-8 bytes: a surrogate pair's four sit on its high half.</summary>
    private static int Utf8Width(string text, int i, int start, int end)
    {
        char c = text[i];
        if (c < 0x80)
        {
            return 1;
        }

        if (c < 0x800)
        {
            return 2;
        }

        if (char.IsHighSurrogate(c) && i + 1 < end && char.IsLowSurrogate(text[i + 1]))
        {
            return 4;
        }

        return char.IsLowSurrogate(c) && i > start && char.IsHighSurrogate(text[i - 1]) ? 0 : 3;
    }

    /// <summary>Walks the trie from <paramref name="at"/> up to <paramref name="end"/>, returning the length of the longest rule, or the shortest.</summary>
    private int Match(ReadOnlySpan<byte> input, int at, int end, bool shortest, out int replacementAt)
    {
        replacementAt = 0;
        int matched = 0;

        uint unit = _trie[0];
        long nodePos = Offset(unit);
        for (int i = at; i < end; i++)
        {
            nodePos ^= input[i];
            if (nodePos < 0 || nodePos >= _trie.Length)
            {
                throw Malformed();
            }
            unit = _trie[nodePos];
            if ((unit & LabelMask) != input[i])
            {
                break;
            }
            nodePos ^= Offset(unit);
            if (nodePos < 0 || nodePos >= _trie.Length)
            {
                throw Malformed();
            }
            if ((unit & HasLeafBit) != 0)
            {
                // A rule ends here. sentencepiece keeps walking for a longer one; tokenizers takes this one.
                replacementAt = (int)(_trie[nodePos] & ValueMask);
                matched = i - at + 1;
                if (shortest)
                {
                    break;
                }
            }
        }
        return matched;
    }

    /// <summary>The NUL-terminated replacement at <paramref name="replacementAt"/>, without its terminator.</summary>
    private ReadOnlySpan<byte> Replacement(int replacementAt)
    {
        int from = _replacementsAt + replacementAt;
        if (from >= _charsMap.Length)
        {
            throw Malformed();
        }
        int terminator = Array.IndexOf(_charsMap, (byte)0, from);

        // Every replacement is NUL-terminated; running off the end means the blob
        // was cut short after the trie.
        if (terminator < 0)
        {
            throw Malformed();
        }
        return new ReadOnlySpan<byte>(_charsMap, from, terminator - from);
    }

    private static uint Offset(uint unit) => (unit >> 10) << (int)((unit & OffsetScaleBit) >> 6);

    private static int Utf8SequenceLength(ReadOnlySpan<byte> input, int at)
    {
        int width = 1;
        while (at + width < input.Length && (input[at + width] & 0xC0) == 0x80)
        {
            width++;
        }
        return width;
    }

    private static InvalidDataException Malformed() =>
        new("The precompiled charsmap is malformed: its trie points outside the blob that carries it.");

    /// <summary>Compares two normalizers by the charsmap they were read from.</summary>
    /// <param name="other">The normalizer to compare against.</param>
    /// <remarks>
    /// Two vocabularies read from the same file carry equal normalizers, which
    /// reference equality would not give — the same reason
    /// <see cref="SentencePieceVocabulary"/> writes its own equality.
    /// </remarks>
    public bool Equals(PrecompiledNormalizer? other)
    {
        if (ReferenceEquals(this, other))
        {
            return true;
        }
        if (other is null || _byGrapheme != other._byGrapheme || _charsMap.Length != other._charsMap.Length)
        {
            return false;
        }
        for (int i = 0; i < _charsMap.Length; i++)
        {
            if (_charsMap[i] != other._charsMap[i])
            {
                return false;
            }
        }
        return true;
    }

    /// <inheritdoc/>
    public override bool Equals(object? obj) => Equals(obj as PrecompiledNormalizer);

    /// <summary>Hashes the charsmap's length and its first and last bytes, which is O(1).</summary>
    /// <remarks>
    /// Equal normalizers necessarily agree on all three; unequal ones may collide.
    /// Hashing a quarter of a megabyte would defeat the purpose of a hash — the
    /// same call <see cref="SentencePieceVocabulary.GetHashCode"/> makes.
    /// </remarks>
    public override int GetHashCode()
    {
        unchecked
        {
            int hash = (17 * 31) + _charsMap.Length;
            hash = (hash * 31) + _charsMap[0];
            return (hash * 31) + _charsMap[_charsMap.Length - 1];
        }
    }
}
