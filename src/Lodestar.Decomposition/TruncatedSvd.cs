using Lodestar.Abstractions;
using Lodestar.Decomposition.Internal;

namespace Lodestar.Decomposition;

/// <summary>A fitted truncated SVD — latent semantic analysis, with nothing centred.</summary>
/// <remarks>
/// <see cref="Fit"/> is the only way to reach one, so there is no unfitted state and no property
/// has to throw. <c>FitTransform</c> is deliberately absent: scikit-learn's returns
/// <c>X · Componentsᵀ</c> for the randomized solver while <c>U · Σ</c> is the other plausible
/// reading of the same words, and the two differ by the approximation error — shipping one under
/// a name that suggests both would be a promise this package cannot keep.
/// </remarks>
public sealed class TruncatedSvd
{
    private readonly double[] _components;
    private readonly double[] _singularValues;
    private readonly double[] _explainedVariance;
    private readonly double[] _explainedVarianceRatio;

    private TruncatedSvd(
        int featureCount,
        double[] components,
        double[] singularValues,
        double[] explainedVariance,
        double[] explainedVarianceRatio)
    {
        FeatureCount = featureCount;
        _components = components;
        _singularValues = singularValues;
        _explainedVariance = explainedVariance;
        _explainedVarianceRatio = explainedVarianceRatio;
    }

    /// <summary>How many components were kept.</summary>
    public int ComponentCount => _singularValues.Length;

    /// <summary>How many columns the fitted matrix had, and every matrix passed to <see cref="Transform"/> must have.</summary>
    public int FeatureCount { get; }

    /// <summary>The right singular vectors, row-major <see cref="ComponentCount"/> × <see cref="FeatureCount"/>.</summary>
    public IReadOnlyList<double> Components => _components;

    /// <summary>The singular values kept, largest first.</summary>
    public IReadOnlyList<double> SingularValues => _singularValues;

    /// <summary>The variance of each column of <see cref="Transform"/>'s answer on the fitted matrix.</summary>
    public IReadOnlyList<double> ExplainedVariance => _explainedVariance;

    /// <summary>Each component's share of the input's total column variance.</summary>
    /// <remarks>
    /// The denominator is the whole matrix's variance, not the kept components', which is why
    /// these sum to less than one — and why the sum is the number that says whether the rank is
    /// enough.
    /// </remarks>
    public IReadOnlyList<double> ExplainedVarianceRatio => _explainedVarianceRatio;

    /// <summary>Fits a truncated SVD of <paramref name="matrix"/> at rank <paramref name="componentCount"/>.</summary>
    /// <param name="matrix">The term-document matrix to factorize, never centred.</param>
    /// <param name="componentCount">How many components to keep.</param>
    /// <param name="options">The randomized solver's settings, or null for scikit-learn's defaults.</param>
    /// <exception cref="ArgumentNullException"><paramref name="matrix"/> is null.</exception>
    /// <exception cref="ArgumentOutOfRangeException"><paramref name="componentCount"/> is not in <c>[1, matrix.ColumnCount)</c>, is above <c>matrix.RowCount</c>, or an option is negative or too large to add to it.</exception>
    /// <exception cref="ArgumentException"><paramref name="matrix"/> holds a NaN or an infinity, or <see cref="TruncatedSvdOptions.RandomMatrix"/> is not <c>matrix.ColumnCount × (componentCount + oversampling)</c>.</exception>
    public static TruncatedSvd Fit(
        CsrMatrix matrix, int componentCount, TruncatedSvdOptions? options = null)
    {
        Guard.NotNull(matrix);
        TruncatedSvdOptions settings = options ?? new TruncatedSvdOptions();
        Validate(matrix, componentCount, settings);
        RequireFinite(matrix);

        int features = matrix.ColumnCount;
        int size = componentCount + settings.Oversampling;
        double[] omega = settings.RandomMatrix
            ?? new GaussianSampler(settings.Seed).Normal(features, size);
        if (omega.Length != (long)features * size)
        {
            throw new ArgumentException(
                $"Ω is {omega.Length} long, not {features} × {size}.", nameof(options));
        }

        // Since 1.6 the estimator asks randomized_svd for flip_sign=False and flips on the
        // right vectors itself, which is why the kernel hands back an unflipped pair.
        (_, double[] s, double[] vt, int rank) = RandomizedSvd.Compute(
            matrix, componentCount, settings.Oversampling, settings.PowerIterations,
            settings.Normalizer, omega);
        SignFlip.Apply(vt, rank, features);

        double[] components = new double[checked(componentCount * features)];
        Array.Copy(vt, components, components.Length);
        double[] singularValues = new double[componentCount];
        Array.Copy(s, singularValues, componentCount);

        (double[] variance, double[] ratio) = ExplainedBy(components, matrix, componentCount);
        return new TruncatedSvd(features, components, singularValues, variance, ratio);
    }

    /// <summary>Projects <paramref name="matrix"/> onto the components, row-major and <see cref="ComponentCount"/> wide.</summary>
    /// <param name="matrix">The matrix to project; it must have <see cref="FeatureCount"/> columns.</param>
    /// <exception cref="ArgumentNullException"><paramref name="matrix"/> is null.</exception>
    /// <exception cref="ArgumentException"><paramref name="matrix"/> does not have <see cref="FeatureCount"/> columns, or holds a NaN or an infinity.</exception>
    public double[] Transform(CsrMatrix matrix)
    {
        Guard.NotNull(matrix);
        if (matrix.ColumnCount != FeatureCount)
        {
            throw new ArgumentException(
                $"This fit has {FeatureCount} features; the matrix has {matrix.ColumnCount}.",
                nameof(matrix));
        }

        RequireFinite(matrix);
        return Project(matrix, _components, ComponentCount, FeatureCount);
    }

    /// <summary>Refuses a stored NaN or infinity, as scikit-learn's input check does.</summary>
    /// <remarks>
    /// Left in, a NaN reaches the QR's sign test and throws <see cref="ArithmeticException"/> from
    /// inside the solver on a fit, and comes back as NaN coordinates from a projection.
    /// </remarks>
    private static void RequireFinite(CsrMatrix matrix)
    {
        int index = Array.FindIndex(matrix.Values, value => double.IsNaN(value) || double.IsInfinity(value));
        if (index >= 0)
        {
            throw new ArgumentException(
                $"A truncated SVD needs finite values, and this matrix holds {matrix.Values[index]}.",
                nameof(matrix));
        }
    }

    /// <summary>Everything refused before a single product is formed.</summary>
    /// <remarks>
    /// The parameter is named <c>options</c> rather than <c>settings</c> so that every
    /// <see cref="ArgumentException.ParamName"/> it produces names a parameter of
    /// <see cref="Fit"/>, which is the only signature a caller catching one can read.
    /// </remarks>
    private static void Validate(CsrMatrix matrix, int componentCount, TruncatedSvdOptions options)
    {
        if (componentCount < 1 || componentCount >= matrix.ColumnCount)
        {
            throw new ArgumentOutOfRangeException(
                nameof(componentCount), componentCount,
                $"A truncated SVD keeps between 1 and {matrix.ColumnCount - 1} components.");
        }
        if (componentCount > matrix.RowCount)
        {
            throw new ArgumentOutOfRangeException(
                nameof(componentCount), componentCount,
                $"A matrix of {matrix.RowCount} rows has no more than that many components.");
        }
        if (options.Oversampling < 0)
        {
            throw new ArgumentOutOfRangeException(
                nameof(options), options.Oversampling,
                $"Oversampling counts extra columns drawn, so it cannot be {options.Oversampling}.");
        }
        if (options.Oversampling > int.MaxValue - componentCount)
        {
            // Unchecked, the sum wraps negative and the block width becomes a diagnostic
            // about something else entirely; refused here rather than survived downstream.
            throw new ArgumentOutOfRangeException(
                nameof(options), options.Oversampling,
                $"Oversampling of {options.Oversampling} and {componentCount} components do not add up within an int.");
        }
        if (options.PowerIterations < 0)
        {
            throw new ArgumentOutOfRangeException(
                nameof(options), options.PowerIterations,
                $"PowerIterations counts repetitions, so it cannot be {options.PowerIterations}.");
        }
    }

    /// <summary><c>X · Componentsᵀ</c>, one row at a time over the non-zeros.</summary>
    /// <remarks>
    /// The components are transposed once so that a feature's k loadings are contiguous: read
    /// row-major, every non-zero paid k reads a whole feature count apart. Each cell sums the same
    /// products in the same order.
    /// </remarks>
    private static double[] Project(
        CsrMatrix matrix, double[] components, int componentCount, int featureCount)
    {
        double[] byFeature = DenseBlock.Transpose(components, componentCount, featureCount);
        double[] values = matrix.Values;
        int[] columns = matrix.ColumnIndices;
        int[] pointers = matrix.RowPointers;
        double[] result = new double[checked(matrix.RowCount * componentCount)];
        for (int row = 0; row < matrix.RowCount; row++)
        {
            Span<double> target = result.AsSpan(row * componentCount, componentCount);
            for (int index = pointers[row]; index < pointers[row + 1]; index++)
            {
                ElementWise.AddScaled(
                    target, byFeature.AsSpan(columns[index] * componentCount, componentCount), values[index]);
            }
        }
        return result;
    }

    /// <summary>The per-column variance of the projection, over the input's total column variance.</summary>
    /// <remarks>
    /// scikit-learn's <c>TruncatedSVD</c> measures the projection and not <c>U · Σ</c> for the
    /// randomized solver, because the two differ by the approximation error the solver leaves
    /// behind — which on these corpora is the third decimal, not the last bits.
    /// </remarks>
    private static (double[] Variance, double[] Ratio) ExplainedBy(
        double[] components, CsrMatrix matrix, int componentCount)
    {
        double[] projection = Project(matrix, components, componentCount, matrix.ColumnCount);
        double[] variance = ColumnVariance(projection, matrix.RowCount, componentCount);
        double total = TotalVariance(matrix);
        double[] ratio = new double[componentCount];
        for (int component = 0; component < componentCount; component++)
        {
            ratio[component] = variance[component] / total;
        }
        return (variance, ratio);
    }

    /// <summary>Each column's variance about its own mean, over <c>n</c> — numpy's default.</summary>
    private static double[] ColumnVariance(double[] block, int rows, int columns)
    {
        double[] variance = new double[columns];
        for (int column = 0; column < columns; column++)
        {
            double mean = 0;
            for (int row = 0; row < rows; row++)
            {
                mean += block[(row * columns) + column];
            }
            mean /= rows;

            double sum = 0;
            for (int row = 0; row < rows; row++)
            {
                double centred = block[(row * columns) + column] - mean;
                sum += centred * centred;
            }
            variance[column] = sum / rows;
        }
        return variance;
    }

    /// <summary>The input's total column variance — the denominator of the ratio.</summary>
    /// <remarks>
    /// Computed from the sums of a sparse column and of its squares, which is what
    /// <c>mean_variance_axis</c> does: nothing is densified to reach it, and the zeros count
    /// towards the mean exactly as they must.
    /// </remarks>
    private static double TotalVariance(CsrMatrix matrix)
    {
        double[] sums = new double[matrix.ColumnCount];
        double[] squares = new double[matrix.ColumnCount];
        for (int index = 0; index < matrix.Values.Length; index++)
        {
            double value = matrix.Values[index];
            int column = matrix.ColumnIndices[index];
            sums[column] += value;
            squares[column] += value * value;
        }

        double total = 0;
        for (int column = 0; column < matrix.ColumnCount; column++)
        {
            double mean = sums[column] / matrix.RowCount;
            total += (squares[column] / matrix.RowCount) - (mean * mean);
        }
        return total;
    }
}
