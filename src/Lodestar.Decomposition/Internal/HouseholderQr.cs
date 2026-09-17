namespace Lodestar.Decomposition.Internal;

/// <summary>The economic QR of a tall block, by Householder reflections.</summary>
/// <remarks>
/// Gram–Schmidt is shorter but loses orthogonality on nearly-parallel columns — exactly what a
/// range finder produces; Householder is unconditionally stable at the same cost. Its factors'
/// signs are not LAPACK's: a flip <c>Q → QD</c> leaves <c>B = QᵀA</c> as <c>DB</c>, whose SVD
/// returns <c>DŨ</c>, and <c>QD · DŨ = QŨ</c> — invariant to what the composed algorithm relies on.
/// </remarks>
internal static class HouseholderQr
{
    /// <summary>Factors a row-major <c>rows × columns</c> block with <c>rows >= columns</c>.</summary>
    internal static (double[] Q, double[] R) Decompose(ReadOnlySpan<double> a, int rows, int columns)
    {
        if (rows < columns)
        {
            throw new ArgumentException(
                $"An economic QR needs at least as many rows as columns; got {rows} × {columns}.",
                nameof(a));
        }
        if (a.Length != checked(rows * columns))
        {
            throw new ArgumentException(
                $"Block length {a.Length} != {rows} × {columns}.", nameof(a));
        }

        // Work in place on a copy: the reflectors are applied to it, and what is left
        // above the diagonal is R.
        double[] work = a.ToArray();
        Reflector[] reflectors = Factorize(work, rows, columns);
        return (ComposeQ(reflectors, rows, columns), ExtractR(work, columns));
    }

    /// <summary>Reduces <paramref name="work"/> to upper-triangular in place, one column at a time.</summary>
    private static Reflector[] Factorize(double[] work, int rows, int columns)
    {
        var reflectors = new Reflector[columns];
        double[] scales = new double[columns];
        for (int k = 0; k < columns; k++)
        {
            Reflector reflector = BuildReflector(work, rows, columns, k);
            reflectors[k] = reflector;

            // S1244: whether the reflector is the identity (an already-zero column), not
            // whether two computed quantities are close.
#pragma warning disable S1244
            if (reflector.NormSquared != 0)
#pragma warning restore S1244
            {
                // Columns left of k hold only what lies below R's diagonal, which nothing reads again.
                ApplyLeft(work, rows, columns, k, k, reflector, scales);
            }
        }
        return reflectors;
    }

    /// <summary>The Householder vector that zeroes column <paramref name="k"/> below its diagonal.</summary>
    private static Reflector BuildReflector(double[] work, int rows, int columns, int k)
    {
        double[] v = new double[rows - k];
        double norm = 0;
        for (int i = k; i < rows; i++)
        {
            double value = work[(i * columns) + k];
            v[i - k] = value;
            norm += value * value;
        }
        norm = Math.Sqrt(norm);

        // A zero column is already reduced. Skipping it keeps a rank-deficient block
        // finite instead of dividing by zero and filling R with NaN.
        //
        // S1244: whether the column vanished entirely, not whether two computed
        // quantities are close.
#pragma warning disable S1244
        if (norm == 0)
#pragma warning restore S1244
        {
            return new Reflector(v, 0);
        }

        double alpha = v[0] >= 0 ? -norm : norm;
        v[0] -= alpha;
        return new Reflector(v, SquaredNorm(v));
    }

    /// <summary>Q is the reflectors applied, in reverse, to the identity's leading columns.</summary>
    /// <remarks>
    /// Never formed as a <c>rows × rows</c> matrix, which is the whole point of "thin".
    /// </remarks>
    private static double[] ComposeQ(Reflector[] reflectors, int rows, int columns)
    {
        double[] q = new double[rows * columns];
        for (int j = 0; j < columns; j++)
        {
            q[(j * columns) + j] = 1.0;
        }
        // A column of the identity left of k is zero from row k down, so reflector k leaves it as it is — to the bit,
        // unless a non-finite reflector turns that zero dot product into NaN, which the full sweep must then keep.
        bool finite = Array.TrueForAll(
            reflectors, reflector => !double.IsNaN(reflector.NormSquared) && !double.IsInfinity(reflector.NormSquared));
        double[] scales = new double[columns];
        for (int k = columns - 1; k >= 0; k--)
        {
            Reflector reflector = reflectors[k];

            // S1244: whether the reflector is the identity (an already-zero column), not
            // whether two computed quantities are close.
#pragma warning disable S1244
            if (reflector.NormSquared != 0)
#pragma warning restore S1244
            {
                ApplyLeft(q, rows, columns, k, finite ? k : 0, reflector, scales);
            }
        }
        return q;
    }

    /// <summary>Copies the upper triangle work was reduced to into its own <c>columns × columns</c> block.</summary>
    private static double[] ExtractR(double[] work, int columns)
    {
        double[] r = new double[columns * columns];
        for (int i = 0; i < columns; i++)
        {
            for (int j = i; j < columns; j++)
            {
                r[(i * columns) + j] = work[(i * columns) + j];
            }
        }
        return r;
    }

    /// <summary>Applies <c>I - 2vvᵀ/vᵀv</c> to the trailing rows of every column from <paramref name="firstColumn"/> on.</summary>
    /// <remarks>
    /// Row by row rather than column by column, since a column of a row-major block strides a whole row per
    /// element: each column's dot product still sums its rows in the same order, and the update is per element.
    /// <paramref name="scales"/> is a buffer at least <paramref name="columns"/> long the caller reuses.
    /// </remarks>
    private static void ApplyLeft(
        double[] block, int rows, int columns, int from, int firstColumn, Reflector reflector, double[] scales)
    {
        double[] v = reflector.V;
        int width = columns - firstColumn;
        Span<double> dots = scales.AsSpan(0, width);
        dots.Clear();
        for (int i = from; i < rows; i++)
        {
            ElementWise.AddScaled(dots, block.AsSpan((i * columns) + firstColumn, width), v[i - from]);
        }

        double normSquared = reflector.NormSquared;
        for (int j = 0; j < dots.Length; j++)
        {
            dots[j] = 2.0 * dots[j] / normSquared;
        }

        for (int i = from; i < rows; i++)
        {
            ElementWise.SubtractScaled(block.AsSpan((i * columns) + firstColumn, width), dots, v[i - from]);
        }
    }

    private static double SquaredNorm(double[] v)
    {
        double sum = 0;
        foreach (double value in v)
        {
            sum += value * value;
        }
        return sum;
    }

    /// <summary>One column's Householder vector, and its squared norm so <see cref="ApplyLeft"/>
    /// never recomputes it.</summary>
    /// <remarks>
    /// <c>NormSquared == 0</c> marks a column already reduced to zero — an exact test, not an
    /// approximate one: it is what tells <see cref="ApplyLeft"/> to skip the reflector rather
    /// than divide by it.
    /// </remarks>
    private readonly struct Reflector(double[] v, double normSquared)
    {
        internal double[] V { get; } = v;

        internal double NormSquared { get; } = normSquared;
    }
}
