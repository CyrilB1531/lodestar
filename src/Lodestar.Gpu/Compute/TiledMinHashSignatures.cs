using System.Diagnostics.CodeAnalysis;
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

    /// <summary><see cref="MinHashScheme.Affine32"/> as the kernel sees it.</summary>
    /// <remarks>
    /// A kernel parameter, not a captured field: ILGPU refuses device code reading a mutable
    /// static, and a constant compared against the argument keeps the branch uniform across a
    /// group, so no thread diverges from another on it.
    /// </remarks>
    private const int Affine32Code = (int)MinHashScheme.Affine32;

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
        ArrayView<ulong>, ArrayView<uint>, int, int> _kernel;

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
            ArrayView<ulong>, ArrayView<ulong>, ArrayView<uint>, int, int>(SignatureKernel);
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
        => Signatures(documents, multipliers, addends, MinHashScheme.Legacy);

    /// <summary>The signature of every document in a resident batch, under one permutation scheme.</summary>
    /// <param name="documents">The resident token hashes, which belong to no scheme.</param>
    /// <param name="multipliers">The <c>a</c> coefficient of each permutation.</param>
    /// <param name="addends">The <c>b</c> coefficient of each, one per multiplier.</param>
    /// <param name="scheme">Which arithmetic the coefficients belong to.</param>
    /// <returns>One signature per document, each as long as there are permutations.</returns>
    /// <exception cref="ArgumentNullException"><paramref name="documents"/> is null.</exception>
    /// <exception cref="ArgumentException">
    /// The two coefficient spans are not the same non-zero length, <paramref name="scheme"/> is
    /// not a declared member, or a coefficient does not fit the scheme it is given.
    /// </exception>
    /// <remarks>
    /// The batch is deliberately not re-uploaded per scheme: the finalizer <c>affine32</c> needs
    /// runs inside the kernel, as the shared tile fills, so one residency serves both families.
    /// </remarks>
    public IReadOnlyList<uint[]> Signatures(
        DeviceTokenHashes documents, ReadOnlySpan<ulong> multipliers, ReadOnlySpan<ulong> addends,
        MinHashScheme scheme)
    {
        Guard.NotNull(documents);
        if (scheme is not (MinHashScheme.Legacy or MinHashScheme.Affine32))
        {
            throw new ArgumentException($"{scheme} is not a permutation scheme.", nameof(scheme));
        }
        if (multipliers.Length == 0 || multipliers.Length != addends.Length)
        {
            throw new ArgumentException(
                $"{multipliers.Length} multipliers and {addends.Length} addends cannot describe "
                + "one permutation set.", nameof(addends));
        }

        if (scheme == MinHashScheme.Affine32)
        {
            MinHashCoefficients.RefuseWhatAffine32CannotRead(
                multipliers, addends, nameof(multipliers));
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
            signatures.View, permutations, (int)scheme);
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
    // long-comment: why a kernel is excluded from coverage instrumentation. Coverlet
    // rewrites an instrumented method to record each hit through a mutable static array,
    // and ILGPU refuses device code that reads a static field which is not read only. So
    // every kernel in this package failed to compile under coverage while passing without
    // it: measured, thirty-six of forty-six tests failed in that job and none locally.
    // The exclusion covers the device method alone; the host code around it is
    // instrumented as usual.
    [ExcludeFromCodeCoverage]
    private static void SignatureKernel(
        ArrayView<uint> hashes, ArrayView<int> offsets, ArrayView<ulong> multipliers,
        ArrayView<ulong> addends, ArrayView<uint> signatures, int permutations, int scheme)
    {
        bool affine = scheme == Affine32Code;
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
                uint hash = hashes[at + Group.IdxX];
                tile[Group.IdxX] = affine ? Fmix32(hash) : hash;
            }

            Group.Barrier();
            if (live)
            {
                best = Minimise(tile, span, multiplier, addend, affine, best);
            }

            Group.Barrier();
        }

        if (live)
        {
            signatures[((long)document * permutations) + permutation] = best;
        }
    }

    /// <summary>One thread's reduction over the tile, under one permutation and one scheme.</summary>
    /// <remarks>
    /// Extracted from the kernel because the second scheme took its cognitive complexity past
    /// the bar S3776 sets — a loop inside a branch inside a loop, twice over. ILGPU inlines a
    /// device method, so this is a reading change and not a call.
    /// </remarks>
    [ExcludeFromCodeCoverage]
    private static uint Minimise(
        ArrayView<uint> tile, int span, ulong multiplier, ulong addend, bool affine, uint best)
    {
        for (int i = 0; i < span; i++)
        {
            // long-comment: which wrap is the specification, and why one branch divides.
            // The reference multiplies in unsigned arithmetic and relies on the overflow, so it
            // is the contract rather than an accident -- in 64 bits for the Mersenne reduction,
            // and in 32 for the affine one, where the wrap is the modulo and this kernel divides
            // by nothing at all.
            uint candidate = affine
                ? ((uint)multiplier * tile[i]) + (uint)addend
                : (uint)((((multiplier * tile[i]) + addend) % MersennePrime) & Mask32);
            if (candidate < best)
            {
                best = candidate;
            }
        }

        return best;
    }

    /// <summary>The MurmurHash3 finalizer on 32 bits, which <c>affine32</c> pre-mixes with.</summary>
    /// <remarks>
    /// The same fixed bijection and the same constants the CPU path carries — they are the
    /// reference's, so they are the contract rather than a choice. Applied as the tile fills,
    /// which costs once per token per group instead of once per token per thread.
    /// </remarks>
    [ExcludeFromCodeCoverage]
    private static uint Fmix32(uint hash)
    {
        hash ^= hash >> 16;
        hash *= 0x85EBCA6B;
        hash ^= hash >> 13;
        hash *= 0xC2B2AE35;
        return hash ^ (hash >> 16);
    }
}
