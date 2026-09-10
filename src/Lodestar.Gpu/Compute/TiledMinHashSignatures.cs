using ILGPU;
using ILGPU.Runtime;

namespace Lodestar.Gpu.Compute;

/// <summary>MinHash signatures for a batch of documents, one thread per permutation.</summary>
/// <remarks>
/// The work splits where the parallelism is: the host hashes, and the accelerator minimises over
/// the permutations, which is the <c>O(tokens × permutations)</c> half. A thread owns one
/// permutation of one document and reduces in a register. Measured, hashing is most of the total —
/// bench/README.md section 26 has what that leaves a caller. The coefficients are supplied rather
/// than seeded, as <c>MinHashPermutations</c> already requires, and taken as spans so this package
/// keeps no edge.
/// </remarks>
public sealed class TiledMinHashSignatures
{
    /// <summary>The widest group the kernel is compiled for, and the shared tile's size.</summary>
    private const int MaxGroupSize = 256;

    /// <summary>The Mersenne prime the permutation reduces through, <c>2^61 - 1</c>.</summary>
    private const ulong MersennePrime = (1UL << 61) - 1UL;

    /// <summary>The 32-bit mask a permuted value is cut to, with an AND and not a modulo.</summary>
    /// <remarks>
    /// The reference masks rather than divides, and the two agree only below the mask — so the
    /// choice moves every signature rather than rounding one. The CPU path records the measured
    /// pair of values that proves it.
    /// </remarks>
    private const ulong Mask32 = (1UL << 32) - 1UL;

    private readonly GpuContext _context;
    private readonly int _groupSize;
    private readonly Action<KernelConfig, ArrayView<uint>, ArrayView<int>, ArrayView<ulong>,
        ArrayView<ulong>, ArrayView<uint>, int> _kernel;

    /// <summary>Loads the kernel onto the accelerator.</summary>
    /// <param name="context">The accelerator to compile for.</param>
    /// <exception cref="ArgumentNullException"><paramref name="context"/> is null.</exception>
    /// <remarks>Loading compiles, so build this once and reuse it (decision 0102).</remarks>
    public TiledMinHashSignatures(GpuContext context)
    {
        Guard.NotNull(context);
        _context = context;
        _groupSize = Math.Min(MaxGroupSize, context.Accelerator.MaxGroupSize.X);
        _kernel = context.Accelerator.LoadStreamKernel<ArrayView<uint>, ArrayView<int>,
            ArrayView<ulong>, ArrayView<ulong>, ArrayView<uint>, int>(SignatureKernel);
    }

    /// <summary>The signature of every document in a resident batch.</summary>
    /// <param name="documents">The resident token hashes.</param>
    /// <param name="multipliers">The <c>a</c> coefficient of each permutation.</param>
    /// <param name="addends">The <c>b</c> coefficient of each, one per multiplier.</param>
    /// <returns>One signature per document, each as long as there are permutations.</returns>
    /// <exception cref="ArgumentNullException"><paramref name="documents"/> is null.</exception>
    /// <exception cref="ArgumentException">The two coefficient spans are not the same non-zero length.</exception>
    /// <remarks>
    /// An empty document gives every slot <c>uint.MaxValue</c>, which is the identity a minimum
    /// starts from rather than a sentinel — so two empty documents estimate a similarity of 1, as
    /// they do on the CPU path.
    /// </remarks>
    public IReadOnlyList<uint[]> Signatures(
        DeviceTokenHashes documents, ReadOnlySpan<ulong> multipliers, ReadOnlySpan<ulong> addends)
    {
        Guard.NotNull(documents);
        if (multipliers.Length == 0 || multipliers.Length != addends.Length)
        {
            throw new ArgumentException(
                $"{multipliers.Length} multipliers and {addends.Length} addends cannot describe "
                + "one permutation set.", nameof(addends));
        }

        int permutations = multipliers.Length;
        Accelerator accelerator = _context.Accelerator;
        using MemoryBuffer1D<ulong, Stride1D.Dense> a = accelerator.Allocate1D(multipliers.ToArray());
        using MemoryBuffer1D<ulong, Stride1D.Dense> b = accelerator.Allocate1D(addends.ToArray());
        using MemoryBuffer1D<uint, Stride1D.Dense> signatures =
            accelerator.Allocate1D<uint>((long)documents.Count * permutations);

        int tiles = (permutations + _groupSize - 1) / _groupSize;
        _kernel(
            new KernelConfig(new Index2D(tiles, documents.Count), new Index2D(_groupSize, 1)),
            documents.Hashes.View, documents.Offsets.View, a.View, b.View,
            signatures.View, permutations);
        accelerator.Synchronize();

        uint[] flat = signatures.GetAsArray1D();
        var result = new uint[documents.Count][];
        for (int row = 0; row < documents.Count; row++)
        {
            result[row] = flat[(row * permutations)..((row + 1) * permutations)];
        }

        return result;
    }

    /// <summary>One thread per permutation, the document's hashes tiled through shared memory.</summary>
    private static void SignatureKernel(
        ArrayView<uint> hashes, ArrayView<int> offsets, ArrayView<ulong> multipliers,
        ArrayView<ulong> addends, ArrayView<uint> signatures, int permutations)
    {
        ArrayView<uint> tile = SharedMemory.Allocate1D<uint>(MaxGroupSize);
        int width = Group.DimX;
        int document = Grid.IdxY;
        int permutation = (Grid.IdxX * width) + Group.IdxX;
        bool live = permutation < permutations;

        ulong multiplier = live ? multipliers[permutation] : 0UL;
        ulong addend = live ? addends[permutation] : 0UL;
        uint best = uint.MaxValue;

        int start = offsets[document];
        int end = offsets[document + 1];
        for (int at = start; at < end; at += width)
        {
            int span = Math.Min(width, end - at);
            if (Group.IdxX < span)
            {
                tile[Group.IdxX] = hashes[at + Group.IdxX];
            }

            Group.Barrier();
            if (live)
            {
                for (int i = 0; i < span; i++)
                {
                    // Unchecked on purpose: the reference multiplies in 64-bit unsigned
                    // arithmetic and relies on the wrap, so overflow is the specification.
                    ulong permuted = ((multiplier * tile[i]) + addend) % MersennePrime;
                    uint candidate = (uint)(permuted & Mask32);
                    if (candidate < best)
                    {
                        best = candidate;
                    }
                }
            }

            Group.Barrier();
        }

        if (live)
        {
            signatures[((long)document * permutations) + permutation] = best;
        }
    }
}
