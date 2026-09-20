namespace Lodestar.Stats.Regression.Internal;

/// <summary>The lower Cholesky factor of an error covariance, and the whitening it gives.</summary>
/// <remarks>
/// Written here rather than reached: <c>Lodestar.Survival</c> has one, internal to that package, and this package
/// does not depend on it. Row by row, reading the lower triangle as <c>scipy.linalg.cholesky(lower=True)</c> does;
/// the caller refuses an asymmetric matrix before this sees one (#771).
/// </remarks>
internal static class Cholesky
{
    /// <summary>A pivot at or below this fraction of its own diagonal entry is a covariance that is not safely positive definite.</summary>
    private const double PivotFloor = 1e-12;

    /// <summary>Factors <c>Σ = L Lᵀ</c>, or reports that <paramref name="matrix"/> is not positive definite.</summary>
    /// <remarks>
    /// The factor's cost is its inner products, <c>n³/6</c> of them, taken through <see cref="Reflections.Dot"/>'s
    /// unrolled loop: the whole fit ran 2.2× faster at 500 and 1,000 rows than with the indexed loop it replaced,
    /// measured A/B/A (#771).
    /// </remarks>
    public static bool TryFactor(ReadOnlySpan<double> matrix, int order, out double[] lower)
    {
        lower = new double[checked(order * order)];
        Span<double> factor = lower;
        for (int row = 0; row < order; row++)
        {
            int rowStart = row * order;
            ReadOnlySpan<double> rowPrefix = factor.Slice(rowStart, row);
            for (int column = 0; column < row; column++)
            {
                int columnStart = column * order;
                double reduced = matrix[rowStart + column]
                    - Reflections.Dot(rowPrefix.Slice(0, column), factor.Slice(columnStart, column));
                factor[rowStart + column] = reduced / factor[columnStart + column];
            }

            double diagonal = matrix[rowStart + row];
            double pivot = diagonal - Reflections.Dot(rowPrefix, rowPrefix);
            // The negated comparison also refuses a NaN, which every ordered comparison fails.
            if (!(pivot > PivotFloor * Math.Abs(diagonal)) || double.IsInfinity(pivot))
            {
                return false;
            }

            factor[rowStart + row] = Math.Sqrt(pivot);
        }

        return true;
    }

    /// <summary>Applies <c>L⁻¹</c> to a contiguous vector in place, by forward substitution.</summary>
    /// <param name="lower">The factor from <see cref="TryFactor"/>.</param>
    /// <param name="order">Rows in the factor, and the vector's length.</param>
    /// <param name="values">The vector to whiten.</param>
    public static void ForwardSubstitute(double[] lower, int order, Span<double> values)
    {
        for (int row = 0; row < order; row++)
        {
            int rowStart = row * order;
            values[row] = (values[row] - Reflections.Dot(lower.AsSpan(rowStart, row), values.Slice(0, row))) / lower[rowStart + row];
        }
    }

    /// <summary>Applies <c>L⁻¹</c> to one column of a row-major block in place.</summary>
    /// <remarks>The column is copied out and back so the substitution runs over contiguous memory.</remarks>
    public static void ForwardSubstitute(double[] lower, int order, double[] block, int columnCount, int column)
    {
        var values = new double[order];
        for (int row = 0; row < order; row++)
        {
            values[row] = block[(row * columnCount) + column];
        }

        ForwardSubstitute(lower, order, values);
        for (int row = 0; row < order; row++)
        {
            block[(row * columnCount) + column] = values[row];
        }
    }
}
