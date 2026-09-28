using System.Numerics;

namespace Lodestar.Embeddings.Search;

/// <summary>SIMD-accelerated primitives over dense <see cref="float"/> vectors.</summary>
public static class VectorMath
{
    /// <summary>Computes the dot product of two equal-length vectors, vectorized via <see cref="Vector{T}"/>.</summary>
    /// <exception cref="ArgumentException"><paramref name="a"/> and <paramref name="b"/> differ in length.</exception>
    public static float Dot(ReadOnlySpan<float> a, ReadOnlySpan<float> b)
    {
        if (a.Length != b.Length)
        {
            throw new ArgumentException($"length mismatch: {a.Length} vs {b.Length}.", nameof(b));
        }

        float sum = 0;
        int i = 0;
#if NET5_0_OR_GREATER
        // SIMD path (the span-based Vector<T> constructor is net-only).
        int width = Vector<float>.Count;
        var acc = Vector<float>.Zero;
        for (; i <= a.Length - width; i += width)
        {
            acc += new Vector<float>(a.Slice(i, width)) * new Vector<float>(b.Slice(i, width));
        }
        for (int j = 0; j < width; j++)
        {
            sum += acc[j];
        }
#endif
        for (; i < a.Length; i++)
        {
            sum += a[i] * b[i];
        }
        return sum;
    }

    /// <summary>Computes the Euclidean (L2) norm of a vector.</summary>
    /// <remarks>
    /// <see cref="Dot"/><c>(v, v)</c> under a square root, recomputed in <see cref="double"/> only where that float
    /// sum overflowed or fell to where its squares underflow: <c>[1e20f]</c> and <c>[1e-23f]</c> have norms a float
    /// holds, which the float sum lost (#1355).
    /// </remarks>
    public static float L2Norm(ReadOnlySpan<float> v)
    {
        float squares = Dot(v, v);
        if (squares >= SmallestExactSquares && !float.IsInfinity(squares))
        {
            return (float)Math.Sqrt(squares);
        }

        return (float)Math.Sqrt(SquaresInDouble(v));
    }

    /// <summary>Below this a float sum of squares may have lost a square to underflow.</summary>
    internal const float SmallestExactSquares = 1e-24f;

    /// <summary>The sum of squares in <see cref="double"/>, in order: the slow path, for the extremes alone.</summary>
    internal static double SquaresInDouble(ReadOnlySpan<float> v)
    {
        double sum = 0;
        foreach (float value in v)
        {
            sum += (double)value * value;
        }

        return sum;
    }
}
