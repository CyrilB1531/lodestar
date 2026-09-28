namespace Lodestar.Decomposition.Internal;

/// <summary>The economic QR of a tall block, by Householder reflections.</summary>
/// <remarks>
/// Gram–Schmidt is shorter but loses orthogonality on nearly-parallel columns — exactly what a
/// range finder produces; Householder is unconditionally stable at the same cost. The reflectors
/// are LAPACK's <c>dgeqr2</c> and <c>dorg2r</c> (<c>dlarfg</c>, <c>dlarf</c>), so the factors carry
/// <c>numpy.linalg.qr</c>'s signs, and its infinities and NaNs where the input holds one (#1305).
/// </remarks>
internal static class HouseholderQr
{
    /// <summary>LAPACK's <c>dlamch('S') / dlamch('E')</c>, 2⁻⁹⁶⁹: below it <c>dlarfg</c> rescales the column.</summary>
    private const double SafeMinimum = 2.004168360008973e-292;

    /// <summary>Below this a plain sum of squares may have lost a square to underflow.</summary>
    private const double SmallSum = 1e-280;

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
            Reflector reflector = Generate(work, rows, columns, k);
            reflectors[k] = reflector;

            // Columns left of k hold only what lies below R's diagonal, which nothing reads again.
            ApplyLeft(work, rows, columns, k, reflector, scales);
        }

        return reflectors;
    }

    /// <summary>
    /// <c>dlarfg</c>: the reflector <c>I − τvvᵀ</c>, <c>v₀ = 1</c>, taking column <paramref name="k"/> from its
    /// diagonal down to <c>β·e₀</c>, with <c>β</c> written to the diagonal.
    /// </summary>
    /// <remarks>
    /// The diagonal is set, not computed by reflecting the column onto itself: an infinite column reflected
    /// is <c>∞ − ∞</c>, where LAPACK, setting it, keeps <c>β = ∓∞</c>.
    /// </remarks>
    private static Reflector Generate(double[] work, int rows, int columns, int k)
    {
        double[] v = new double[rows - k];
        double sum = 0;
        for (int i = k + 1; i < rows; i++)
        {
            double value = work[(i * columns) + k];
            v[i - k] = value;
            sum += value * value;
        }

        v[0] = 1.0;
        double alpha = work[(k * columns) + k];
        double norm = Plain(sum) ? Math.Sqrt(sum) : Norm(v.AsSpan(1));

        // S1244: whether the column below the diagonal vanished entirely, in which case the
        // reflector is the identity (τ = 0), as dlarfg has it.
#pragma warning disable S1244
        if (norm == 0)
#pragma warning restore S1244
        {
            return new Reflector(v, 0);
        }

        double beta = -SignOf(alpha, Hypotenuse(alpha, norm));
        int rescaled = 0;
        if (Math.Abs(beta) < SafeMinimum)
        {
            // A column this small loses its digits to subnormals; dlarfg scales it up, at most twenty times.
            do
            {
                rescaled++;
                Scale(v.AsSpan(1), 1.0 / SafeMinimum);
                beta /= SafeMinimum;
                alpha /= SafeMinimum;
            }
            while (Math.Abs(beta) < SafeMinimum && rescaled < 20);

            beta = -SignOf(alpha, Hypotenuse(alpha, Norm(v.AsSpan(1))));
        }

        double tau = (beta - alpha) / beta;
        Scale(v.AsSpan(1), 1.0 / (alpha - beta));
        for (int j = 0; j < rescaled; j++)
        {
            beta *= SafeMinimum;
        }

        work[(k * columns) + k] = beta;
        return new Reflector(v, tau);
    }

    /// <summary>
    /// <c>dorg2r</c>: Q is built from its last column back, each reflector applied to the columns after its own,
    /// and its own column then set to <c>(1 − τ, −τv₁, …)</c>.
    /// </summary>
    private static double[] ComposeQ(Reflector[] reflectors, int rows, int columns)
    {
        double[] q = new double[rows * columns];
        double[] scales = new double[columns];
        for (int k = columns - 1; k >= 0; k--)
        {
            ApplyComposing(q, rows, columns, k, reflectors[k], scales);
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

    /// <summary>
    /// <c>dlarf</c>: applies <c>I − τvvᵀ</c> to rows <paramref name="from"/> on of every column after
    /// <paramref name="from"/>.
    /// </summary>
    /// <remarks>
    /// As <c>dlarf</c>, it skips <c>v</c>'s trailing zeros and the trailing columns zero over the rows left, which
    /// is what keeps a NaN <c>τ</c> from reaching an entry the reflector would not change. Row by row, since a
    /// column of a row-major block strides a whole row per element; each column's dot product still sums its rows
    /// in order. <paramref name="scales"/> is a buffer at least <paramref name="columns"/> long the caller reuses.
    /// </remarks>
    private static void ApplyLeft(double[] block, int rows, int columns, int from, Reflector reflector, double[] scales)
    {
        (int length, int width) = Extent(block, rows, columns, from, reflector);
        if (width == 0)
        {
            return;
        }

        ReadOnlySpan<double> dots = Dots(block, columns, from, length, width, reflector, scales);
        double[] v = reflector.V;
        for (int i = 0; i < length; i++)
        {
            ElementWise.SubtractScaled(block.AsSpan(((from + i) * columns) + from + 1, width), dots, v[i]);
        }
    }

    /// <summary>
    /// <see cref="ApplyLeft"/> on Q, writing Q's column <paramref name="from"/>, <c>(1 − τ, −τv₁, …)</c>, in the
    /// same walk down the rows: a pass of its own strides a whole row per element.
    /// </summary>
    /// <remarks>Its own method, the hot loops kept small enough for the JIT to inline their updates.</remarks>
    private static void ApplyComposing(double[] q, int rows, int columns, int from, Reflector reflector, double[] scales)
    {
        (int length, int width) = Extent(q, rows, columns, from, reflector);
        ReadOnlySpan<double> dots = width == 0 ? default : Dots(q, columns, from, length, width, reflector, scales);
        int updated = width == 0 ? 0 : length;
        double tau = reflector.Tau;
        double[] v = reflector.V;
        q[(from * columns) + from] = 1.0 - tau;
        if (updated > 0)
        {
            ElementWise.SubtractScaled(q.AsSpan((from * columns) + from + 1, width), dots, v[0]);
        }

        for (int i = 1; i < rows - from; i++)
        {
            int row = (from + i) * columns;
            if (i < updated)
            {
                ElementWise.SubtractScaled(q.AsSpan(row + from + 1, width), dots, v[i]);
            }

            q[row + from] = -tau * v[i];
        }
    }

    /// <summary><c>τ · Cᵀv</c> over the rows and columns <see cref="Extent"/> found, in <paramref name="scales"/>.</summary>
    private static Span<double> Dots(
        double[] block, int columns, int from, int length, int width, Reflector reflector, double[] scales)
    {
        Span<double> dots = scales.AsSpan(0, width);
        dots.Clear();
        double[] v = reflector.V;
        for (int i = 0; i < length; i++)
        {
            ElementWise.AddScaled(dots, block.AsSpan(((from + i) * columns) + from + 1, width), v[i]);
        }

        double tau = reflector.Tau;
        for (int j = 0; j < dots.Length; j++)
        {
            dots[j] = tau * dots[j];
        }

        return dots;
    }

    /// <summary>
    /// The rows and columns <c>dlarf</c> touches: <c>v</c> up to its last non-zero, and the columns from
    /// <c>from + 1</c> up to the last holding a non-zero in those rows; none for <c>τ = 0</c>.
    /// </summary>
    private static (int Length, int Width) Extent(double[] block, int rows, int columns, int from, Reflector reflector)
    {
        int firstColumn = from + 1;
        // S1244: τ = 0 is the identity dlarfg returns for a vanished column, and a zero in v
        // is skipped only when exact, both as LAPACK tests them.
#pragma warning disable S1244
        if (reflector.Tau == 0 || firstColumn >= columns)
        {
            return (0, 0);
        }

        double[] v = reflector.V;
        int length = rows - from;
        while (length > 0 && v[length - 1] == 0)
        {
            length--;
        }
#pragma warning restore S1244

        return (length, LastNonZeroColumn(block, columns, from, from + length, firstColumn) - firstColumn);
    }

    /// <summary><c>iladlc</c>: one past the last column from <paramref name="firstColumn"/> with a non-zero in the rows.</summary>
    private static int LastNonZeroColumn(double[] block, int columns, int fromRow, int toRow, int firstColumn)
    {
        for (int j = columns - 1; j >= firstColumn; j--)
        {
            for (int i = fromRow; i < toRow; i++)
            {
                // S1244: a NaN counts, as it does for iladlc; only an exact zero is skipped.
#pragma warning disable S1244
                if (block[(i * columns) + j] != 0)
#pragma warning restore S1244
                {
                    return j + 1;
                }
            }
        }

        return firstColumn;
    }

    /// <summary>The Euclidean norm: NaN on a NaN, and <c>+∞</c> on an infinity, as OpenBLAS's <c>dnrm2</c>.</summary>
    /// <remarks>A plain sum of squares, rescaled only where it overflowed or may have underflowed.</remarks>
    private static double Norm(ReadOnlySpan<double> x)
    {
        double sum = 0;
        foreach (double value in x)
        {
            sum += value * value;
        }

        if (Plain(sum))
        {
            return Math.Sqrt(sum);
        }

        double scale = 0;
        double squares = 1;
        foreach (double value in x)
        {
            double magnitude = Math.Abs(value);
            if (double.IsPositiveInfinity(magnitude))
            {
                return magnitude;
            }

            if (magnitude > scale)
            {
                double ratio = scale / magnitude;
                squares = 1 + (squares * ratio * ratio);
                scale = magnitude;
            }
            else if (magnitude > 0)
            {
                double ratio = magnitude / scale;
                squares += ratio * ratio;
            }
        }

        return scale * Math.Sqrt(squares);
    }

    /// <summary>Whether a plain sum of squares is the norm's square as it stands: NaN, or neither overflowed nor small.</summary>
    private static bool Plain(double sum) => double.IsNaN(sum) || (sum >= SmallSum && sum <= double.MaxValue);

    /// <summary><c>dlapy2</c>: <c>√(x² + y²)</c> without overflow, and NaN when either is.</summary>
    private static double Hypotenuse(double x, double y)
    {
        if (double.IsNaN(x))
        {
            return x;
        }

        if (double.IsNaN(y))
        {
            return y;
        }

        double larger = Math.Max(Math.Abs(x), Math.Abs(y));
        double smaller = Math.Min(Math.Abs(x), Math.Abs(y));

        // S1244: an exact zero, for which the ratio below is not needed.
#pragma warning disable S1244
        if (smaller == 0 || larger > double.MaxValue)
#pragma warning restore S1244
        {
            return larger;
        }

        double ratio = smaller / larger;
        return larger * Math.Sqrt(1 + (ratio * ratio));
    }

    /// <summary>Fortran's <c>SIGN(magnitude, sign)</c>: <paramref name="magnitude"/> with the sign bit of <paramref name="sign"/>.</summary>
    private static double SignOf(double sign, double magnitude) =>
        BitConverter.DoubleToInt64Bits(sign) < 0 ? -Math.Abs(magnitude) : Math.Abs(magnitude);

    private static void Scale(Span<double> values, double factor)
    {
        for (int i = 0; i < values.Length; i++)
        {
            values[i] *= factor;
        }
    }

    /// <summary>One column's reflector <c>I − τvvᵀ</c>, with <c>v₀ = 1</c>.</summary>
    /// <remarks><c>Tau == 0</c> marks the identity: a column already zero below its diagonal.</remarks>
    private readonly struct Reflector(double[] v, double tau)
    {
        internal double[] V { get; } = v;

        internal double Tau { get; } = tau;
    }
}
