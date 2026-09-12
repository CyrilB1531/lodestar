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
        if (a.Length != checked(rows * columns))
        {
            throw new ArgumentException(
                $"Block length {a.Length} != {rows} × {columns}.", nameof(a));
        }

        if (rows < columns)
        {
            // Aᵀ = U₁ Σ V₁ᵀ gives A = V₁ Σ U₁ᵀ: the two factors trade places.
            //
            // S2234: the transpose swaps rows and columns on purpose; passing them in the
            // caller's order would factor the wrong shape.
#pragma warning disable S2234
            (double[] wideU, double[] wideS, double[] wideVt) =
                Decompose(DenseBlock.Transpose(a, rows, columns), columns, rows);
#pragma warning restore S2234
            return (DenseBlock.Transpose(wideVt, wideS.Length, rows),
                    wideS,
                    DenseBlock.Transpose(wideU, columns, wideS.Length));
        }

        double[] work = a.ToArray();
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

        return Finish(work, v, rows, columns);
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

        double[] work = a.ToArray();
        double[] squared = new double[columns];
        for (int j = 0; j < columns; j++)
        {
            double norm = DenseBlock.ColumnNorm(work, rows, columns, j);
            squared[j] = norm * norm;
        }

        for (int sweep = 0; sweep < MaximumSweeps; sweep++)
        {
            bool rotated = false;
            for (int p = 0; p < columns - 1; p++)
            {
                for (int q = p + 1; q < columns; q++)
                {
                    rotated |= RotateTrackedPair(work, squared, rows, columns, p, q);
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
            norms[j] = DenseBlock.ColumnNorm(work, rows, columns, j);
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
    private static bool RotateTrackedPair(
        double[] work, double[] squared, int rows, int columns, int p, int q)
    {
        double alpha = squared[p];
        double beta = squared[q];
        double gamma = 0;
        for (int i = 0; i < rows; i++)
        {
            gamma += work[(i * columns) + p] * work[(i * columns) + q];
        }

        if (!Rotation(alpha, beta, gamma, out double t, out double cosine, out double sine))
        {
            return false;
        }

        Rotate(work, rows, columns, p, q, cosine, sine);
        squared[p] = alpha - (t * gamma);
        squared[q] = beta + (t * gamma);
        return true;
    }

    /// <summary>Orthogonalizes one pair of columns, and reports whether it had to.</summary>
    private static bool RotatePair(
        double[] work, double[] v, int rows, int columns, int p, int q)
    {
        double alpha = 0;
        double beta = 0;
        double gamma = 0;
        for (int i = 0; i < rows; i++)
        {
            double left = work[(i * columns) + p];
            double right = work[(i * columns) + q];
            alpha += left * left;
            beta += right * right;
            gamma += left * right;
        }

        if (!Rotation(alpha, beta, gamma, out _, out double cosine, out double sine))
        {
            return false;
        }

        Rotate(work, rows, columns, p, q, cosine, sine);
        Rotate(v, columns, columns, p, q, cosine, sine);
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

    private static void Rotate(
        double[] block, int rows, int columns, int p, int q, double cosine, double sine)
    {
        for (int i = 0; i < rows; i++)
        {
            double left = block[(i * columns) + p];
            double right = block[(i * columns) + q];
            block[(i * columns) + p] = (cosine * left) - (sine * right);
            block[(i * columns) + q] = (sine * left) + (cosine * right);
        }
    }

    /// <summary>Reads the norms off the orthogonalized columns and sorts the triplets.</summary>
    private static (double[] U, double[] S, double[] Vt) Finish(
        double[] work, double[] v, int rows, int columns)
    {
        double[] norms = new double[columns];
        for (int j = 0; j < columns; j++)
        {
            norms[j] = DenseBlock.ColumnNorm(work, rows, columns, j);
        }

        int[] order = new int[columns];
        for (int j = 0; j < columns; j++)
        {
            order[j] = j;
        }
        Array.Sort(order, (left, right) => norms[right].CompareTo(norms[left]));

        double[] u = new double[rows * columns];
        double[] s = new double[columns];
        double[] vt = new double[columns * columns];
        for (int j = 0; j < columns; j++)
        {
            int source = order[j];
            double norm = norms[source];
            s[j] = norm;
            // A numerically zero column carries no direction; leaving U's column at zero is
            // what scipy's own factor does for a rank-deficient block, and dividing would not.
            //
            // S1244: whether the norm vanished entirely, not whether two computed
            // quantities are close.
#pragma warning disable S1244
            double scale = norm == 0 ? 0 : 1.0 / norm;
#pragma warning restore S1244
            for (int i = 0; i < rows; i++)
            {
                u[(i * columns) + j] = work[(i * columns) + source] * scale;
            }
            for (int i = 0; i < columns; i++)
            {
                vt[(j * columns) + i] = v[(i * columns) + source];
            }
        }
        return (u, s, vt);
    }
}
