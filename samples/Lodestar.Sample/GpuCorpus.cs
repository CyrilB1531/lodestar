using Lodestar.Gpu.Compute;

namespace Lodestar.Sample;

/// <summary>One accelerator and one small corpus the GPU samples share.</summary>
/// <remarks>
/// <c>preferCpu: true</c> throughout, because these run in CI on a machine with no graphics
/// hardware. The answers are identical either way — only the timings are not, and those are
/// measured in <c>bench/</c> rather than printed here.
/// </remarks>
internal static class GpuCorpus
{
    /// <summary>Three two-dimensional vectors, row-major.</summary>
    public static readonly float[] Vectors = [1f, 0f, 0f, 1f, 1f, 1f];

    /// <summary>A CSR matrix of two rows: one value in each.</summary>
    public static readonly int[] RowPointers = [0, 1, 2];

    /// <summary>The column each stored value sits in.</summary>
    public static readonly int[] ColumnIndices = [0, 1];

    /// <summary>The stored values themselves.</summary>
    public static readonly double[] Values = [2.0, 3.0];

    /// <summary>Three documents of token hashes, the third deliberately empty.</summary>
    /// <remarks>
    /// Hashes rather than tokens, because that is what the kernel takes. Real ones come from
    /// the same construction <c>MinHash</c> uses; these are chosen for readability.
    /// </remarks>
    public static readonly uint[][] DocumentHashes = [[11u, 22u, 33u], [22u, 44u], []];

    /// <summary>The <c>a</c> coefficient of each permutation, supplied rather than seeded.</summary>
    public static readonly ulong[] Multipliers = [3UL, 5UL, 7UL, 11UL];

    /// <summary>The <c>b</c> coefficient of each.</summary>
    public static readonly ulong[] Addends = [13UL, 17UL, 19UL, 23UL];

    /// <summary>Opens ILGPU's CPU accelerator, which is what a runner without a card has.</summary>
    public static GpuContext Open() => GpuContext.Create(preferCpu: true);
}
