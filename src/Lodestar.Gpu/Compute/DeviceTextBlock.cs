using ILGPU;
using ILGPU.Runtime;

namespace Lodestar.Gpu.Compute;

/// <summary>A batch of strings renamed to a dense alphabet and held on the accelerator.</summary>
/// <remarks>
/// A kernel parameter has to be blittable, so nothing here carries a <see cref="string"/>:
/// the batch is renamed on the host into one flat array of symbol codes plus the offsets
/// cutting it back into rows, which is also what lets the equality masks be a 256-entry
/// table rather than a lookup per character. A character the pattern does not hold renames
/// to one reserved code, because a mask no pattern position sets is the same for all of them.
/// </remarks>
public sealed class DeviceTextBlock : IDisposable
{
    /// <summary>The code every character outside the pattern's alphabet renames to.</summary>
    internal const int Unmatched = 255;

    /// <summary>How many distinct characters a pattern may hold before this refuses.</summary>
    public const int MaxPatternAlphabet = 255;

    internal MemoryBuffer1D<byte, Stride1D.Dense> Symbols { get; }

    internal MemoryBuffer1D<int, Stride1D.Dense> Offsets { get; }

    /// <summary>How many strings the block holds.</summary>
    public int Count { get; }

    /// <summary>The alphabet the strings were renamed against, one entry per character.</summary>
    internal IReadOnlyDictionary<char, byte> Alphabet { get; }

    private DeviceTextBlock(
        MemoryBuffer1D<byte, Stride1D.Dense> symbols,
        MemoryBuffer1D<int, Stride1D.Dense> offsets,
        int count,
        IReadOnlyDictionary<char, byte> alphabet)
    {
        Symbols = symbols;
        Offsets = offsets;
        Count = count;
        Alphabet = alphabet;
    }

    /// <summary>Renames a batch against a pattern's alphabet and uploads it.</summary>
    /// <param name="context">The accelerator to upload to.</param>
    /// <param name="pattern">The pattern whose characters define the alphabet.</param>
    /// <param name="texts">The strings to rename; none may be null.</param>
    /// <exception cref="ArgumentNullException">An argument, or one of the texts, is null.</exception>
    /// <exception cref="ArgumentException"><paramref name="texts"/> is empty, or the pattern holds too many distinct characters.</exception>
    public static DeviceTextBlock Upload(GpuContext context, string pattern, IReadOnlyList<string> texts)
    {
        Guard.NotNull(context);
        Guard.NotNull(pattern);
        Guard.NotNull(texts);
        if (texts.Count == 0)
        {
            throw new ArgumentException("A batch holds at least one string.", nameof(texts));
        }

        Dictionary<char, byte> alphabet = [];
        foreach (char symbol in pattern.Distinct())
        {
            if (alphabet.Count >= MaxPatternAlphabet)
            {
                throw new ArgumentException(
                    $"a pattern holds at most {MaxPatternAlphabet} distinct characters.",
                    nameof(pattern));
            }

            alphabet[symbol] = (byte)alphabet.Count;
        }

        int total = 0;
        foreach (string text in texts)
        {
            Guard.NotNull(text);
            total += text.Length;
        }

        byte[] symbols = new byte[total];
        int[] offsets = new int[texts.Count + 1];
        int at = 0;
        for (int row = 0; row < texts.Count; row++)
        {
            offsets[row] = at;
            foreach (char character in texts[row])
            {
                symbols[at++] = alphabet.TryGetValue(character, out byte code) ? code : (byte)Unmatched;
            }
        }

        offsets[texts.Count] = at;
        Accelerator accelerator = context.Accelerator;
        // Same measured reason as DeviceTokenHashes: ILGPU's array overload throws on a
        // zero-length array where the length overload returns an empty buffer.
        MemoryBuffer1D<byte, Stride1D.Dense> deviceSymbols = symbols.Length == 0
            ? accelerator.Allocate1D<byte>(0)
            : accelerator.Allocate1D(symbols);
        MemoryBuffer1D<int, Stride1D.Dense> deviceOffsets = accelerator.Allocate1D(offsets);
        return new DeviceTextBlock(deviceSymbols, deviceOffsets, texts.Count, alphabet);
    }

    /// <summary>Frees the two device buffers.</summary>
    public void Dispose()
    {
        Symbols.Dispose();
        Offsets.Dispose();
    }
}
