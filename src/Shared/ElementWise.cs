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
/// Every lane computes exactly the scalar expression on its own element, with no fused multiply-add and no
/// reordered sum, so the vector path and the scalar tail produce the same bits as the scalar loop they replace.
/// </remarks>
internal static class ElementWise
{
    /// <summary><c>(l, r) ← (c·l − s·r, s·l + c·r)</c> over two equally long spans.</summary>
    internal static void Rotate(Span<double> left, Span<double> right, double cosine, double sine)
    {
        right = right.Slice(0, left.Length);
        int i = 0;
#if NET
        if (Vector256.IsHardwareAccelerated)
        {
            ref double l = ref MemoryMarshal.GetReference(left);
            ref double r = ref MemoryMarshal.GetReference(right);
            Vector256<double> c = Vector256.Create(cosine);
            Vector256<double> s = Vector256.Create(sine);
            for (; i <= left.Length - 4; i += 4)
            {
                Vector256<double> x = Vector256.LoadUnsafe(ref l, (nuint)i);
                Vector256<double> y = Vector256.LoadUnsafe(ref r, (nuint)i);
                ((c * x) - (s * y)).StoreUnsafe(ref l, (nuint)i);
                ((s * x) + (c * y)).StoreUnsafe(ref r, (nuint)i);
            }
        }
#endif
        for (; i < left.Length; i++)
        {
            double x = left[i];
            double y = right[i];
            left[i] = (cosine * x) - (sine * y);
            right[i] = (sine * x) + (cosine * y);
        }
    }

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

    /// <summary><c>target[j] −= factor · source[j]</c>.</summary>
    internal static void SubtractScaled(Span<double> target, ReadOnlySpan<double> source, double factor)
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
                (Vector256.LoadUnsafe(ref t, (nuint)j) - (f * Vector256.LoadUnsafe(ref s, (nuint)j)))
                    .StoreUnsafe(ref t, (nuint)j);
            }
        }
#endif
        for (; j < target.Length; j++)
        {
            target[j] -= factor * source[j];
        }
    }
}
