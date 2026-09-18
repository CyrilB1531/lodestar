namespace Lodestar.Internal;

/// <summary>The singular values of a dense block, by one-sided Jacobi rotations.</summary>
/// <remarks>
/// Shared source rather than an API: <c>Lodestar.Decomposition</c> factors with it, and
/// <c>Lodestar.Stats.Regression</c> and <c>Lodestar.Stats.TimeSeries</c> measure a triangular
/// factor's rank and condition with it. <c>src/</c> reaches its neighbours through a published
/// floor, so one copy compiled into each is what keeps the three in step without a release
/// between them (#978, and #843 for the precedent).
/// </remarks>
internal static class JacobiSpectrum
{
    // Rotating a pair whose off-diagonal is already at the rounding floor changes nothing
    // and costs a sweep, so the sweep stops when every pair is below it.
    private const double Threshold = 1e-15;
    private const int MaximumSweeps = 60;

    /// <summary>Orthogonalizes a tall column-major block's columns in place and reports their norms, largest first.</summary>
    /// <remarks>
    /// One-sided Jacobi: the norms the columns converge to are the singular values, and no
    /// bidiagonalization, shift or deflation is needed to reach them. <c>V</c> is not accumulated,
    /// which is a rotation per pair a caller reading only the spectrum would otherwise pay for a
    /// factor it drops.
    /// </remarks>
    /// <param name="columnMajor">The block, column by column; overwritten with its orthogonalized columns.</param>
    /// <param name="rows">Rows in the block, at least <paramref name="columns"/>.</param>
    /// <param name="columns">Columns in the block.</param>
    /// <returns>The singular values, largest first, <paramref name="columns"/> of them.</returns>
    /// <exception cref="ArgumentException"><paramref name="columnMajor"/> is not a tall <paramref name="rows"/> × <paramref name="columns"/> block.</exception>
    internal static double[] SingularValues(double[] columnMajor, int rows, int columns)
    {
        if (columnMajor is null || rows < columns || columnMajor.Length != checked(rows * columns))
        {
            throw new ArgumentException(
                $"Block length {columnMajor?.Length ?? 0} is not a tall {rows} × {columns}.",
                nameof(columnMajor));
        }

        double[] squared = new double[columns];
        for (int j = 0; j < columns; j++)
        {
            double norm = Norm(columnMajor.AsSpan(j * rows, rows));
            squared[j] = norm * norm;
        }

        for (int sweep = 0; sweep < MaximumSweeps; sweep++)
        {
            bool rotated = false;
            for (int p = 0; p < columns - 1; p++)
            {
                for (int q = p + 1; q < columns; q++)
                {
                    rotated |= RotateTrackedPair(columnMajor, squared, rows, p, q);
                }
            }

            if (!rotated)
            {
                break;
            }
        }

        // The tracked norms steered the sweeps; the answer is read off the columns themselves,
        // so rounding accumulated in the updates never reaches it.
        double[] norms = new double[columns];
        for (int j = 0; j < columns; j++)
        {
            norms[j] = Norm(columnMajor.AsSpan(j * rows, rows));
        }

        Array.Sort(norms, (left, right) => right.CompareTo(left));
        return norms;
    }

    /// <summary>Orthogonalizes one pair of columns with the two squared norms carried rather than recomputed, and reports whether it had to.</summary>
    /// <remarks>
    /// The rotation that zeroes <c>γ</c> moves <c>tγ</c> of squared norm from one column to the
    /// other, so <c>α − tγ</c> and <c>β + tγ</c> are exact up to rounding and one product over
    /// the rows replaces three.
    /// </remarks>
    private static bool RotateTrackedPair(double[] work, double[] squared, int rows, int p, int q)
    {
        double alpha = squared[p];
        double beta = squared[q];
        Span<double> left = work.AsSpan(p * rows, rows);
        Span<double> right = work.AsSpan(q * rows, rows);
        double gamma = 0;
        for (int i = 0; i < left.Length; i++)
        {
            gamma += left[i] * right[i];
        }

        if (!Rotation(alpha, beta, gamma, out double t, out double cosine, out double sine))
        {
            return false;
        }

        ElementWise.Rotate(left, right, cosine, sine);
        squared[p] = alpha - (t * gamma);
        squared[q] = beta + (t * gamma);
        return true;
    }

    /// <summary>The rotation that orthogonalizes a pair, or false when the pair already is.</summary>
    /// <remarks><paramref name="t"/> is its tangent, which a caller tracking the norms needs.</remarks>
    internal static bool Rotation(
        double alpha, double beta, double gamma, out double t, out double cosine, out double sine)
    {
        t = 0;
        cosine = 1;
        sine = 0;
        // S1244: whether the pair is already orthogonal (gamma vanished entirely), not
        // whether two computed quantities are close.
#pragma warning disable S1244
        if (gamma == 0 || Math.Abs(gamma) <= Threshold * Math.Sqrt(alpha * beta))
#pragma warning restore S1244
        {
            return false;
        }

        double zeta = (beta - alpha) / (2.0 * gamma);
        t = Math.Sign(zeta) / (Math.Abs(zeta) + Math.Sqrt(1.0 + (zeta * zeta)));
        // S1244: whether the columns already have equal norm (zeta vanished entirely),
        // not whether two computed quantities are close.
#pragma warning disable S1244
        if (zeta == 0)
#pragma warning restore S1244
        {
            t = 1.0;
        }

        cosine = 1.0 / Math.Sqrt(1.0 + (t * t));
        sine = cosine * t;
        return true;
    }

    /// <summary>A column's Euclidean norm, summed in the order every caller here must share.</summary>
    internal static double Norm(ReadOnlySpan<double> column)
    {
        double sum = 0;
        foreach (double value in column)
        {
            sum += value * value;
        }

        return Math.Sqrt(sum);
    }
}
