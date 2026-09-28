using Lodestar.Decomposition.Internal;

namespace Lodestar.Decomposition;

/// <summary>How much of a dense matrix's variance each principal component explains.</summary>
/// <remarks>
/// <strong>Not a PCA.</strong> There are no components and no projection here: decision 0004
/// delegates both to ML.NET and NumFlat, and this type answers the one question neither answers
/// below <c>net8.0</c>, which is how many components to keep. Dense by construction, since
/// centring a <c>CsrMatrix</c> densifies it; decision 0003 says why it lives in this package.
/// </remarks>
public sealed class PrincipalComponentVariance
{
    private readonly double[] _explainedVariance;

    private PrincipalComponentVariance(
        int sampleCount,
        int featureCount,
        double totalVariance,
        double[] explainedVariance,
        double[] explainedVarianceRatio,
        double[] cumulativeExplainedVarianceRatio)
    {
        SampleCount = sampleCount;
        FeatureCount = featureCount;
        TotalVariance = totalVariance;
        _explainedVariance = explainedVariance;
        ExplainedVariance = Array.AsReadOnly(_explainedVariance);
        ExplainedVarianceRatio = Array.AsReadOnly(explainedVarianceRatio);
        CumulativeExplainedVarianceRatio = Array.AsReadOnly(cumulativeExplainedVarianceRatio);
    }

    /// <summary>Rows of the matrix, each one sample.</summary>
    public int SampleCount { get; }

    /// <summary>Columns of the matrix, each one feature.</summary>
    public int FeatureCount { get; }

    /// <summary>How many components there are: the smaller of <see cref="SampleCount"/> and <see cref="FeatureCount"/>.</summary>
    public int ComponentCount => _explainedVariance.Length;

    /// <summary>The sum of the columns' sample variances, which the components share out.</summary>
    public double TotalVariance { get; }

    /// <summary>The variance along each component, largest first, with <c>n − 1</c> degrees of freedom.</summary>
    public IReadOnlyList<double> ExplainedVariance { get; }

    /// <summary>Each component's share of <see cref="TotalVariance"/>, largest first.</summary>
    public IReadOnlyList<double> ExplainedVarianceRatio { get; }

    /// <summary>The running sum of <see cref="ExplainedVarianceRatio"/>: the curve a component count is read off.</summary>
    public IReadOnlyList<double> CumulativeExplainedVarianceRatio { get; }

    /// <summary>Computes the variance each principal component of a row-major matrix explains.</summary>
    /// <param name="matrix">The matrix, row-major: <paramref name="columnCount"/> values per row. It is not modified.</param>
    /// <param name="rowCount">How many samples it has; at least two, since the variance divides by <c>n − 1</c>.</param>
    /// <param name="columnCount">How many features it has.</param>
    /// <returns>The variance, the ratio and the cumulative curve, one entry per component.</returns>
    /// <exception cref="ArgumentOutOfRangeException"><paramref name="rowCount"/> is below two, or <paramref name="columnCount"/> is not positive.</exception>
    /// <exception cref="ArgumentException"><paramref name="matrix"/> does not hold <paramref name="rowCount"/> × <paramref name="columnCount"/> values, holds a value that is not finite or too large to centre, has a variance past the largest double, or has no variance at all.</exception>
    public static PrincipalComponentVariance Compute(
        ReadOnlySpan<double> matrix, int rowCount, int columnCount)
    {
        Guard.NotLessThan(rowCount, 2);
        Guard.NotLessThan(columnCount, 1);
        if (matrix.Length != (long)rowCount * columnCount)
        {
            throw new ArgumentException(
                $"matrix holds {matrix.Length} values, not the {rowCount} x {columnCount} its shape declares.",
                nameof(matrix));
        }

        double[] means = ColumnMeans(matrix, rowCount, columnCount);

        // Only a Gram whose squares would leave the doubles is rescaled, by a power of two, which is
        // exact; at an ordinary scale nothing is scaled and the spectrum is main's, bit for bit (#1255).
        double[] gram = Gram(matrix, means, rowCount, columnCount, 1.0, out int order);
        int exponent = 0;
        if (!Representable(gram, order))
        {
            exponent = LargestExponent(matrix, means, rowCount, columnCount);
            gram = Gram(matrix, means, rowCount, columnCount, Math.Pow(2.0, -exponent), out order);
        }

        // A symmetric positive semi-definite matrix's singular values are its eigenvalues.
        double[] eigenvalues = JacobiSvd.SingularValues(gram, order, order);
        double unscale = Math.Pow(2.0, exponent);

        int components = eigenvalues.Length;
        double[] variance = new double[components];
        double total = 0.0;
        for (int k = 0; k < components; k++)
        {
            variance[k] = eigenvalues[k] * unscale * unscale / (rowCount - 1);
            total += variance[k];
        }

        // Past about 1e154 the variance itself leaves the doubles; scikit-learn's PCA fails there too.
        if (double.IsInfinity(total))
        {
            throw new ArgumentException("The variance overflows: the values are too large.", nameof(matrix));
        }

        // S1244: a block whose every column is constant centres to exactly zero, and that
        // exact zero is the case refused; scikit-learn divides by it and answers NaN.
#pragma warning disable S1244
        if (total == 0.0)
#pragma warning restore S1244
        {
            throw new ArgumentException(
                "Every column of matrix is constant, so there is no variance for a component to explain.",
                nameof(matrix));
        }

        double[] ratio = new double[components];
        double[] cumulative = new double[components];
        double running = 0.0;
        for (int k = 0; k < components; k++)
        {
            ratio[k] = variance[k] / total;
            running += ratio[k];
            cumulative[k] = running;
        }

        return new PrincipalComponentVariance(
            rowCount, columnCount, total, variance, ratio, cumulative);
    }

    /// <summary>The centred block's Gram matrix, both triangles, scaled by <paramref name="scale"/>.</summary>
    /// <remarks>
    /// <c>XᵀX</c> for a tall block and <c>XXᵀ</c> for a wide one share their non-zero eigenvalues,
    /// which are the squared singular values scikit-learn's <c>svd_solver="full"</c> reads, so the
    /// smaller order is the one solved. Forming it squares the condition, as scikit-learn's own
    /// <c>covariance_eigh</c> does for tall blocks; the loss is absolute accuracy near
    /// <c>ε · λ₁</c>, which no ratio can show. Decision 0003 has the timings that chose it.
    /// </remarks>
    private static double[] Gram(
        ReadOnlySpan<double> matrix, double[] means, int rowCount, int columnCount, double scale, out int order)
    {
        bool tall = rowCount >= columnCount;
        order = tall ? columnCount : rowCount;
        double[] gram = tall
            ? ColumnGram(matrix, means, rowCount, columnCount, scale)
            : RowGram(Centre(matrix, means, rowCount, columnCount, scale), rowCount, columnCount);

        for (int a = 0; a < order; a++)
        {
            for (int b = a + 1; b < order; b++)
            {
                gram[(b * order) + a] = gram[(a * order) + b];
            }
        }

        return gram;
    }

    /// <summary>Whether the Jacobi sweep can square this Gram's entries without leaving the normal doubles.</summary>
    private static bool Representable(double[] gram, int order)
    {
        double largest = 0.0;
        for (int a = 0; a < order; a++)
        {
            double diagonal = gram[(a * order) + a];
            if (double.IsNaN(diagonal) || double.IsInfinity(diagonal))
            {
                return false;
            }

            largest = Math.Max(largest, diagonal);
        }

        // The sweep squares these entries, so both ends keep a square inside the normal doubles.
        // S1244: an all-zero Gram is the constant block refused below, not a scale to fix.
#pragma warning disable S1244
        return largest == 0.0 || (largest > 1e-140 && largest < 1e140);
#pragma warning restore S1244
    }

    /// <summary>The binary exponent of the largest centred magnitude, which the rescaled Gram divides out.</summary>
    private static int LargestExponent(ReadOnlySpan<double> matrix, double[] means, int rowCount, int columnCount)
    {
        double largest = 0.0;
        for (int row = 0; row < rowCount; row++)
        {
            for (int column = 0; column < columnCount; column++)
            {
                largest = Math.Max(largest, Math.Abs(matrix[(row * columnCount) + column] - means[column]));
            }
        }

        // A column summing past the largest double has no mean to centre on; scikit-learn's
        // LAPACK call fails there too, as LinAlgError.
        if (double.IsNaN(largest) || double.IsInfinity(largest))
        {
            throw new ArgumentException(
                "The column means overflow: the values are too large to centre.", nameof(matrix));
        }

        // Every power of two in ±1022 is a normal double, so the scaled magnitudes land near one.
        return largest > 0.0 ? Math.Max(-1022, Math.Min(1022, (int)Math.Floor(Math.Log(largest, 2.0)))) : 0;
    }

    /// <summary>The upper triangle of the centred <c>XᵀX</c>, one row at a time, so the walk stays row-major.</summary>
    /// <remarks>
    /// Each row is centred into a buffer of one row's width rather than the block being copied:
    /// the copy was the whole of this path's allocation, and centring before multiplying keeps
    /// the accuracy that subtracting <c>n · μμᵀ</c> afterwards would lose on a large mean.
    /// </remarks>
    private static double[] ColumnGram(
        ReadOnlySpan<double> matrix, double[] means, int rowCount, int columnCount, double scale)
    {
        double[] gram = new double[checked(columnCount * columnCount)];
        double[] centred = new double[columnCount];
        for (int row = 0; row < rowCount; row++)
        {
            ReadOnlySpan<double> values = matrix.Slice(row * columnCount, columnCount);
            for (int column = 0; column < columnCount; column++)
            {
                centred[column] = (values[column] - means[column]) * scale;
            }

            for (int a = 0; a < columnCount; a++)
            {
                double left = centred[a];
                int target = a * columnCount;
                for (int b = a; b < columnCount; b++)
                {
                    gram[target + b] += left * centred[b];
                }
            }
        }

        return gram;
    }

    /// <summary>The upper triangle of <c>XXᵀ</c>: one dot product per pair of rows.</summary>
    private static double[] RowGram(double[] centred, int rowCount, int columnCount)
    {
        double[] gram = new double[checked(rowCount * rowCount)];
        for (int i = 0; i < rowCount; i++)
        {
            for (int j = i; j < rowCount; j++)
            {
                double sum = 0.0;
                for (int column = 0; column < columnCount; column++)
                {
                    sum += centred[(i * columnCount) + column] * centred[(j * columnCount) + column];
                }

                gram[(i * rowCount) + j] = sum;
            }
        }

        return gram;
    }

    /// <summary>Each column's mean, refusing a value no mean can be taken over.</summary>
    private static double[] ColumnMeans(ReadOnlySpan<double> matrix, int rowCount, int columnCount)
    {
        double[] means = new double[columnCount];
        for (int row = 0; row < rowCount; row++)
        {
            for (int column = 0; column < columnCount; column++)
            {
                double value = matrix[(row * columnCount) + column];
                if (double.IsNaN(value))
                {
                    throw new ArgumentException("Input X contains NaN.", nameof(matrix));
                }

                if (double.IsInfinity(value))
                {
                    throw new ArgumentException(
                        "Input X contains infinity or a value too large for dtype('float64').", nameof(matrix));
                }

                means[column] += value;
            }
        }

        for (int column = 0; column < columnCount; column++)
        {
            means[column] /= rowCount;
        }

        return means;
    }

    /// <summary>A centred copy of the block, for the wide path whose Gram pairs whole rows.</summary>
    private static double[] Centre(
        ReadOnlySpan<double> matrix, double[] means, int rowCount, int columnCount, double scale)
    {
        double[] centred = matrix.ToArray();
        for (int row = 0; row < rowCount; row++)
        {
            for (int column = 0; column < columnCount; column++)
            {
                int at = (row * columnCount) + column;
                centred[at] = (centred[at] - means[column]) * scale;
            }
        }

        return centred;
    }
}
