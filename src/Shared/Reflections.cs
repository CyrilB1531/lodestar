using System;

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
        int row = 0;
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
    /// Scalar rather than <c>Vector&lt;T&gt;</c>, so net10.0 and netstandard2.0 add the terms in one order (#771, #782).
    /// </remarks>
    internal static double Dot(ReadOnlySpan<double> left, ReadOnlySpan<double> right)
    {
        int length = left.Length;
        double s0 = 0.0, s1 = 0.0, s2 = 0.0, s3 = 0.0;
        int i = 0;
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
