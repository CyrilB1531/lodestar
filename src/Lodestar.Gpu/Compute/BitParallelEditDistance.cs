using System.Diagnostics.CodeAnalysis;
using ILGPU;
using ILGPU.Runtime;

namespace Lodestar.Gpu.Compute;

/// <summary>Myers' bit-parallel edit distance, one pattern against a batch of strings.</summary>
/// <remarks>
/// The parallelism is <strong>across pairs, not inside one</strong>: Myers already collapses a
/// dynamic-programming row into one machine word, so a thread runs a whole distance in
/// registers. That shape is why this is the hardest of the three to clear decision 0102's
/// gate — the CPU path it is priced against is bit-parallel too. Written from Myers (1999)
/// as that path was, and limited to a 64-character pattern: the blocked formulation beyond
/// that carries state a thread would loop over, which is a second kernel not a wider one.
/// </remarks>
public sealed class BitParallelEditDistance
{
    /// <summary>The longest pattern one machine word can carry.</summary>
    public const int MaxPatternLength = 64;

    /// <summary>Entries in the equality table, one per renamed symbol.</summary>
    private const int AlphabetSize = 256;

    private readonly GpuContext _context;
    private readonly Action<Index1D, ArrayView<ulong>, ArrayView<byte>, ArrayView<int>,
        ArrayView<int>, int> _kernel;

    /// <summary>Loads the kernel onto the accelerator.</summary>
    /// <param name="context">The accelerator to compile for.</param>
    /// <exception cref="ArgumentNullException"><paramref name="context"/> is null.</exception>
    /// <remarks>Loading compiles, so build this once and reuse it (decision 0102).</remarks>
    public BitParallelEditDistance(GpuContext context)
    {
        Guard.NotNull(context);
        _context = context;
        _kernel = context.Accelerator.LoadAutoGroupedStreamKernel<Index1D, ArrayView<ulong>,
            ArrayView<byte>, ArrayView<int>, ArrayView<int>, int>(DistanceKernel);
    }

    /// <summary>The edit distance from one pattern to every string in a resident batch.</summary>
    /// <param name="pattern">The pattern the batch was renamed against, at most 64 characters.</param>
    /// <param name="texts">The resident batch, uploaded against that same pattern.</param>
    /// <returns>One distance per string, in the batch's own order.</returns>
    /// <exception cref="ArgumentNullException">An argument is null.</exception>
    /// <exception cref="ArgumentException">
    /// The pattern is empty or longer than <see cref="MaxPatternLength"/>, or the batch was
    /// renamed against a different alphabet.
    /// </exception>
    public int[] Distance(string pattern, DeviceTextBlock texts)
    {
        Guard.NotNull(pattern);
        Guard.NotNull(texts);
        if (pattern.Length is 0 or > MaxPatternLength)
        {
            throw new ArgumentException(
                $"a pattern holds between 1 and {MaxPatternLength} characters, not {pattern.Length}.",
                nameof(pattern));
        }

        ulong[] equality = new ulong[AlphabetSize];
        for (int position = 0; position < pattern.Length; position++)
        {
            if (!texts.Alphabet.TryGetValue(pattern[position], out byte code))
            {
                throw new ArgumentException(
                    "the batch was renamed against a different pattern's alphabet.", nameof(texts));
            }

            equality[code] |= 1UL << position;
        }

        Accelerator accelerator = _context.Accelerator;
        using MemoryBuffer1D<ulong, Stride1D.Dense> deviceEquality = accelerator.Allocate1D(equality);
        using MemoryBuffer1D<int, Stride1D.Dense> distances = accelerator.Allocate1D<int>(texts.Count);

        _kernel(texts.Count, deviceEquality.View, texts.Symbols.View, texts.Offsets.View,
            distances.View, pattern.Length);
        accelerator.Synchronize();

        return distances.GetAsArray1D();
    }

    // S3776 (cognitive complexity): the loop below is Myers' 1999 kernel, and its statement
    // order is the algorithm rather than a style. Splitting it would break the one-to-one
    // reading against the paper that makes a bit-manipulation kernel auditable at all.
#pragma warning disable S3776
    /// <summary>One thread, one string, one machine word of dynamic-programming row.</summary>
    // long-comment: why a kernel is excluded from coverage instrumentation. Coverlet
    // rewrites an instrumented method to record each hit through a mutable static array,
    // and ILGPU refuses device code that reads a static field which is not read only. So
    // every kernel in this package failed to compile under coverage while passing without
    // it: measured, thirty-six of forty-six tests failed in that job and none locally.
    // The exclusion covers the device method alone; the host code around it is
    // instrumented as usual.
    [ExcludeFromCodeCoverage]
    private static void DistanceKernel(
        Index1D index, ArrayView<ulong> equality, ArrayView<byte> symbols,
        ArrayView<int> offsets, ArrayView<int> distances, int patternLength)
    {
        int from = offsets[index];
        int to = offsets[index + 1];

        ulong positive = ulong.MaxValue;
        ulong negative = 0UL;
        int score = patternLength;
        ulong top = 1UL << (patternLength - 1);

        for (int at = from; at < to; at++)
        {
            ulong equal = equality[(int)symbols[at]];
            ulong verticalX = equal | negative;
            ulong horizontalX = ((((equal & positive) + positive) ^ positive) | equal);
            ulong horizontalPositive = negative | ~(horizontalX | positive);
            ulong horizontalNegative = positive & horizontalX;

            if ((horizontalPositive & top) != 0UL)
            {
                score++;
            }
            else if ((horizontalNegative & top) != 0UL)
            {
                score--;
            }

            // long-comment: the trailing 1 is the boundary condition separating a global
            // edit distance from an approximate search. Without it the first row of the
            // dynamic program stays at zero, so the text may begin anywhere -- and the two
            // agree often enough that the defect looks like a rounding. Measured: a pattern
            // of "aaaa" against a text of "aaa" answered 0 where the answer is 1.
            horizontalPositive = (horizontalPositive << 1) | 1UL;
            horizontalNegative <<= 1;
            positive = horizontalNegative | ~(verticalX | horizontalPositive);
            negative = horizontalPositive & verticalX;
        }

        distances[index] = score;
    }
#pragma warning restore S3776
}
