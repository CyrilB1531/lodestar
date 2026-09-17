namespace Lodestar.Decomposition.Internal;

/// <summary>The SVD of a dense block, by one-sided Jacobi rotations.</summary>
/// <remarks>
/// One-sided Jacobi orthogonalizes the columns of a tall block in place by plane rotations; the
/// column norms it converges to are the singular values, the normalized columns are <c>U</c>, and
/// the accumulated rotations are <c>V</c>. It needs no bidiagonalization, no shifts and no
/// deflation logic. A wide block is factored through its transpose, which swaps the roles of
/// <c>U</c> and <c>V</c> — the block this package actually reaches here is <c>B = QᵀA</c>, wide
/// and short.
/// </remarks>
internal static class JacobiSvd
{
    // Rotating a pair whose off-diagonal is already at the rounding floor changes nothing
    // and costs a sweep, so the sweep stops when every pair is below it.
    private const double Threshold = 1e-15;
    private const int MaximumSweeps = 60;

    /// <summary>Factors a row-major <c>rows × columns</c> block of any shape.</summary>
    internal static (double[] U, double[] S, double[] Vt) Decompose(
        ReadOnlySpan<double> a, int rows, int columns)
    {
        RequireLength(a, rows, columns);
        return rows < columns
            ? Sweep(a.ToArray(), columns, rows, transposed: true)
            : Sweep(DenseBlock.Transpose(a, rows, columns), rows, columns, transposed: false);
    }

    /// <summary><see cref="Decompose"/>, free to overwrite <paramref name="a"/> when that spares a copy.</summary>
    /// <remarks>
    /// A wide row-major block, read column-major, is its own transpose, which is the tall block the sweeps
    /// factor: its buffer becomes the working copy as it stands.
    /// </remarks>
    internal static (double[] U, double[] S, double[] Vt) DecomposeOverwriting(
        double[] a, int rows, int columns)
    {
        RequireLength(a, rows, columns);
        return rows < columns
            ? Sweep(a, columns, rows, transposed: true)
            : Sweep(DenseBlock.Transpose(a, rows, columns), rows, columns, transposed: false);
    }

    private static void RequireLength(ReadOnlySpan<double> a, int rows, int columns)
    {
        if (a.Length != checked(rows * columns))
        {
            throw new ArgumentException(
                $"Block length {a.Length} != {rows} × {columns}.", nameof(a));
        }
    }

    /// <summary>Runs the sweeps over a tall column-major <c>rows × columns</c> block, then reads the factors off it.</summary>
    /// <remarks>
    /// Column-major so that a column is one contiguous span: a row-major walk down a column strides a whole
    /// row per element. <paramref name="transposed"/> says the caller's block was the transpose of this one,
    /// so <c>Aᵀ = U₁ Σ V₁ᵀ</c> gives <c>A = V₁ Σ U₁ᵀ</c> and the two factors trade places.
    /// </remarks>
    private static (double[] U, double[] S, double[] Vt) Sweep(
        double[] work, int tallRows, int tallColumns, bool transposed)
    {
        int rows = tallRows;
        int columns = tallColumns;
        double[] v = new double[columns * columns];
        for (int i = 0; i < columns; i++)
        {
            v[(i * columns) + i] = 1.0;
        }

        for (int sweep = 0; sweep < MaximumSweeps; sweep++)
        {
            bool rotated = false;
            for (int p = 0; p < columns - 1; p++)
            {
                for (int q = p + 1; q < columns; q++)
                {
                    rotated |= RotatePair(work, v, rows, columns, p, q);
                }
            }
            if (!rotated)
            {
                break;
            }
        }

        return Finish(work, v, rows, columns, transposed);
    }

    /// <summary>The singular values of a tall row-major block, largest first, with no vectors.</summary>
    /// <remarks>
    /// The same sweeps as <see cref="Decompose"/>, without accumulating <c>V</c>: a caller who reads
    /// only the spectrum would otherwise pay a second rotation per pair to build a factor it drops.
    /// </remarks>
    internal static double[] SingularValues(ReadOnlySpan<double> a, int rows, int columns)
    {
        if (rows < columns || a.Length != checked(rows * columns))
        {
            throw new ArgumentException(
                $"Block length {a.Length} is not a tall {rows} × {columns}.", nameof(a));
        }

        double[] work = DenseBlock.Transpose(a, rows, columns);
        double[] squared = new double[columns];
        for (int j = 0; j < columns; j++)
        {
            double norm = Norm(work.AsSpan(j * rows, rows));
            squared[j] = norm * norm;
        }

        for (int sweep = 0; sweep < MaximumSweeps; sweep++)
        {
            bool rotated = false;
            for (int p = 0; p < columns - 1; p++)
            {
                for (int q = p + 1; q < columns; q++)
                {
                    rotated |= RotateTrackedPair(work, squared, rows, p, q);
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
            norms[j] = Norm(work.AsSpan(j * rows, rows));
        }
        Array.Sort(norms, (left, right) => right.CompareTo(left));
        return norms;
    }

    /// <summary><see cref="RotatePair"/> with the two squared norms carried rather than recomputed.</summary>
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

    /// <summary>Orthogonalizes one pair of columns, and reports whether it had to.</summary>
    private static bool RotatePair(
        double[] work, double[] v, int rows, int columns, int p, int q)
    {
        Span<double> left = work.AsSpan(p * rows, rows);
        Span<double> right = work.AsSpan(q * rows, rows);
        double alpha = 0;
        double beta = 0;
        double gamma = 0;
        for (int i = 0; i < left.Length; i++)
        {
            double x = left[i];
            double y = right[i];
            alpha += x * x;
            beta += y * y;
            gamma += x * y;
        }

        if (!Rotation(alpha, beta, gamma, out _, out double cosine, out double sine))
        {
            return false;
        }

        ElementWise.Rotate(left, right, cosine, sine);
        ElementWise.Rotate(v.AsSpan(p * columns, columns), v.AsSpan(q * columns, columns), cosine, sine);
        return true;
    }

    /// <summary>The rotation that orthogonalizes a pair, or false when the pair already is.</summary>
    /// <remarks><paramref name="t"/> is its tangent, which the tracked variant also needs for the norms.</remarks>
    private static bool Rotation(
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

    private static double Norm(ReadOnlySpan<double> column)
    {
        double sum = 0;
        foreach (double value in column)
        {
            sum += value * value;
        }
        return Math.Sqrt(sum);
    }

    /// <summary>Reads the norms off the orthogonalized columns and sorts the triplets.</summary>
    private static (double[] U, double[] S, double[] Vt) Finish(
        double[] work, double[] v, int rows, int columns, bool transposed)
    {
        double[] norms = new double[columns];
        for (int j = 0; j < columns; j++)
        {
            norms[j] = Norm(work.AsSpan(j * rows, rows));
        }

        int[] order = new int[columns];
        for (int j = 0; j < columns; j++)
        {
            order[j] = j;
        }
        Array.Sort(order, (left, right) => norms[right].CompareTo(norms[left]));

        double[] s = new double[columns];
        double[] scales = new double[columns];
        for (int j = 0; j < columns; j++)
        {
            double norm = norms[order[j]];
            s[j] = norm;
            // A numerically zero column carries no direction; leaving U's column at zero is
            // what scipy's own factor does for a rank-deficient block, and dividing would not.
            //
            // S1244: whether the norm vanished entirely, not whether two computed
            // quantities are close.
#pragma warning disable S1244
            scales[j] = norm == 0 ? 0 : 1.0 / norm;
#pragma warning restore S1244
        }

        double[] u = Normalized(work, rows, columns, order, scales, transposed);
        double[] vt = Rotations(v, columns, order, transposed);

        // When the caller's block was this one's transpose, its U is V₁ and its Vᵀ is U₁ᵀ.
        return transposed ? (vt, s, u) : (u, s, vt);
    }

    /// <summary>U₁ row-major, or U₁ᵀ when <paramref name="transposed"/>, whose row j is one column of the work.</summary>
    private static double[] Normalized(
        double[] work, int rows, int columns, int[] order, double[] scales, bool transposed)
    {
        double[] result = new double[rows * columns];
        for (int j = 0; j < columns; j++)
        {
            ReadOnlySpan<double> column = work.AsSpan(order[j] * rows, rows);
            double scale = scales[j];
            int stride = transposed ? 1 : columns;
            int offset = transposed ? j * rows : j;
            for (int i = 0; i < rows; i++)
            {
                result[(i * stride) + offset] = column[i] * scale;
            }
        }
        return result;
    }

    /// <summary>V₁ᵀ row-major, or V₁ when <paramref name="transposed"/>, from the column-major rotations.</summary>
    private static double[] Rotations(double[] v, int columns, int[] order, bool transposed)
    {
        double[] result = new double[columns * columns];
        for (int j = 0; j < columns; j++)
        {
            ReadOnlySpan<double> column = v.AsSpan(order[j] * columns, columns);
            int stride = transposed ? columns : 1;
            int offset = transposed ? j : j * columns;
            for (int i = 0; i < columns; i++)
            {
                result[(i * stride) + offset] = column[i];
            }
        }
        return result;
    }
}
