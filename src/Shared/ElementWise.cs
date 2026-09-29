using System;
using System.Runtime.CompilerServices;
#if NET
using System.Runtime.InteropServices;
using System.Runtime.Intrinsics;
#endif

namespace Lodestar.Internal;

/// <summary>The element-by-element updates the dense kernels share, vectorized where the target allows.</summary>
/// <remarks>
/// Shared source, compiled into <c>Lodestar.Abstractions</c> and <c>Lodestar.Decomposition</c>: the sparse block
/// products and the dense kernels make the same update, and neither package can reach the other's internals (#845).
/// Abstractions takes this file alone, <c>AddScaled</c> being all its products call; the rotations the dense
/// kernels add live in <c>ElementWise.Kernels.cs</c> (#1417).
/// Every lane computes exactly the scalar expression on its own element, with no fused multiply-add and no
/// reordered sum, so the vector path and the scalar tail produce the same bits as the scalar loop they replace.
/// </remarks>
internal static partial class ElementWise
{
    /// <summary><c>target[j] += factor · source[j]</c>.</summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    internal static void AddScaled(Span<double> target, ReadOnlySpan<double> source, double factor)
    {
        source = source.Slice(0, target.Length);
        int j = 0;
#if NET
        if (Vector256.IsHardwareAccelerated)
        {
            ref double t = ref MemoryMarshal.GetReference(target);
            ref double s = ref MemoryMarshal.GetReference(source);
            Vector256<double> f = Vector256.Create(factor);
            for (; j <= target.Length - 4; j += 4)
            {
                (Vector256.LoadUnsafe(ref t, (nuint)j) + (f * Vector256.LoadUnsafe(ref s, (nuint)j)))
                    .StoreUnsafe(ref t, (nuint)j);
            }
        }
#endif
        for (; j < target.Length; j++)
        {
            target[j] += factor * source[j];
        }
    }
}
