using System;
#if NET
using System.Runtime.InteropServices;
using System.Runtime.Intrinsics;
#endif

namespace Lodestar.Internal;

/// <summary>
/// The two kernels of a Householder least-squares fit, compiled into each library that reflects a design.
/// </summary>
/// <remarks>
/// Shared source rather than a shared member: <c>Lodestar.Stats.TimeSeries</c> reuses one QR across
/// its lag search and must add the terms in <c>OrdinaryLeastSquares.Estimate</c>'s order to stay
/// bit-identical, and the estimate's kernel is internal to <c>Lodestar.Stats.Regression</c> (#843).
/// </remarks>
internal static class Reflections
{
    /// <summary>Applies the reflection <paramref name="vector"/> holds to <paramref name="target"/>, the rows at and below the diagonal.</summary>
    internal static void Reflect(ReadOnlySpan<double> vector, double scale, Span<double> target)
    {
        double factor = Dot(vector, target) / scale;
        int length = vector.Length;
        target = target.Slice(0, length);
        int row = 0;
#if NET
        // Element by element, so a lane computes what the scalar loop does on its own row.
        if (Vector256.IsHardwareAccelerated)
        {
            ref double v = ref MemoryMarshal.GetReference(vector);
            ref double t = ref MemoryMarshal.GetReference(target);
            Vector256<double> f = Vector256.Create(factor);
            for (; row + 4 <= length; row += 4)
            {
                (Vector256.LoadUnsafe(ref t, (nuint)row) - (f * Vector256.LoadUnsafe(ref v, (nuint)row)))
                    .StoreUnsafe(ref t, (nuint)row);
            }
        }
#endif
        for (; row + 4 <= length; row += 4)
        {
            target[row] -= factor * vector[row];
            target[row + 1] -= factor * vector[row + 1];
            target[row + 2] -= factor * vector[row + 2];
            target[row + 3] -= factor * vector[row + 3];
        }

        for (; row < length; row++)
        {
            target[row] -= factor * vector[row];
        }
    }

    /// <summary>An inner product unrolled four terms at a time, the same order on both target frameworks.</summary>
    /// <remarks>
    /// Every reflection is two passes over a column, and every Cholesky pivot one, so this loop is what those fits cost.
    /// Four running sums, one per residue of the index mod 4, added in one order at the end (#771, #782). On net10.0 the
    /// four sums are a <c>Vector256</c>'s four lanes, never <c>Vector&lt;T&gt;</c>, whose width would regroup them.
    /// </remarks>
    internal static double Dot(ReadOnlySpan<double> left, ReadOnlySpan<double> right)
    {
        int length = left.Length;
        right = right.Slice(0, length);
        double s0 = 0.0, s1 = 0.0, s2 = 0.0, s3 = 0.0;
        int i = 0;
#if NET
        if (Vector256.IsHardwareAccelerated && length >= 4)
        {
            ref double l = ref MemoryMarshal.GetReference(left);
            ref double r = ref MemoryMarshal.GetReference(right);
            Vector256<double> sums = Vector256<double>.Zero;
            for (; i + 4 <= length; i += 4)
            {
                sums += Vector256.LoadUnsafe(ref l, (nuint)i) * Vector256.LoadUnsafe(ref r, (nuint)i);
            }

            s0 = sums.GetElement(0);
            s1 = sums.GetElement(1);
            s2 = sums.GetElement(2);
            s3 = sums.GetElement(3);
        }
#endif
        for (; i + 4 <= length; i += 4)
        {
            s0 += left[i] * right[i];
            s1 += left[i + 1] * right[i + 1];
            s2 += left[i + 2] * right[i + 2];
            s3 += left[i + 3] * right[i + 3];
        }

        double total = s0 + s1 + s2 + s3;
        for (; i < length; i++)
        {
            total += left[i] * right[i];
        }

        return total;
    }
}
