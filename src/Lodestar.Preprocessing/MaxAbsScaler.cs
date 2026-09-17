using Lodestar.Abstractions;
using Lodestar.Preprocessing.Internal;

namespace Lodestar.Preprocessing;

/// <summary>
/// Divides each feature by its largest absolute value, at <c>sklearn.preprocessing.MaxAbsScaler</c> parity.
/// </summary>
/// <remarks>
/// The scaler for data whose zeros mean something: it never subtracts, so a zero stays a zero and
/// sparsity survives — which is exactly what centring would destroy.
/// </remarks>
public sealed class MaxAbsScaler
{
    private readonly double[] _scale;
    private readonly bool _clips;

    private MaxAbsScaler(
        int featureCount, int sampleCount, double[] maximumAbsolute, double[] scale, bool clips)
    {
        FeatureCount = featureCount;
        SampleCount = sampleCount;
        MaximumAbsolute = maximumAbsolute;
        _scale = scale;
        _clips = clips;
    }

    /// <summary>How many values each row of a sample matrix carries.</summary>
    public int FeatureCount { get; }

    /// <summary>How many rows the scaler was fitted on — scikit-learn's <c>n_samples_seen_</c>.</summary>
    public int SampleCount { get; }

    /// <summary>Each feature's largest absolute fitted value — <c>max_abs_</c>.</summary>
    public IReadOnlyList<double> MaximumAbsolute { get; }

    /// <summary>What <see cref="Transform(ReadOnlySpan{double})"/> divides by — <c>scale_</c>, which is <see cref="MaximumAbsolute"/> with a near-constant feature floored to 1.</summary>
    public IReadOnlyList<double> Scale => _scale;

    /// <summary>Fits a scaler on a row-major sample matrix.</summary>
    /// <param name="samples">The samples, row-major: <paramref name="featureCount"/> values per row.</param>
    /// <param name="featureCount">How many values each row carries.</param>
    /// <param name="options">Whether to clip on the way out; <see langword="null"/> does not.</param>
    /// <returns>A fitted scaler.</returns>
    /// <exception cref="ArgumentOutOfRangeException"><paramref name="featureCount"/> is not positive.</exception>
    /// <exception cref="ArgumentException"><paramref name="samples"/> holds no row, a partial one, or a non-finite value.</exception>
    /// <remarks>
    /// A feature that is all zeros divides by 1 rather than by 0, and so does one whose largest
    /// absolute value is below <c>10·eps</c> — the reference's rule, not <c>max == 0</c>.
    /// </remarks>
    public static MaxAbsScaler Fit(
        ReadOnlySpan<double> samples, int featureCount, MaxAbsScalerOptions? options = null)
    {
        Guard.NotLessThan(featureCount, 1);
        int sampleCount = SampleMatrix.Rows(samples, featureCount);
        SampleMatrix.RequireFinite(samples, nameof(samples));

        var maximumAbsolute = new double[featureCount];
        Widen(samples, maximumAbsolute);

        double[] scale = [.. maximumAbsolute];
        ScaleFloor.Apply(scale);

        return new MaxAbsScaler(
            featureCount, sampleCount, maximumAbsolute, scale, (options ?? new MaxAbsScalerOptions()).Clip);
    }

    /// <summary>Fits a scaler on a sparse matrix, which is the shape it suits best.</summary>
    /// <param name="samples">The samples, one row per matrix row.</param>
    /// <param name="options">Whether to clip on the way out; <see langword="null"/> does not.</param>
    /// <returns>A fitted scaler.</returns>
    /// <exception cref="ArgumentNullException"><paramref name="samples"/> is <see langword="null"/>.</exception>
    /// <exception cref="ArgumentException"><paramref name="samples"/> holds no row or a non-finite value.</exception>
    /// <remarks>
    /// The scaler that never subtracts is the one a sparse matrix wants: a zero stays a zero, so the
    /// matrix that went in is the shape that comes out. The reference accepts sparse here for the
    /// same reason.
    /// </remarks>
    public static MaxAbsScaler Fit(CsrMatrix samples, MaxAbsScalerOptions? options = null)
    {
        Guard.NotNull(samples);
        if (samples.RowCount == 0 || samples.ColumnCount == 0)
        {
            throw new ArgumentException("samples holds no row or no column.", nameof(samples));
        }

        SparseColumns.RequireFinite(samples, nameof(samples), SparseColumns.FitReason);

        double[] maximumAbsolute = SparseColumns.MaximumAbsolute(samples);
        double[] scale = [.. maximumAbsolute];
        ScaleFloor.Apply(scale);

        return new MaxAbsScaler(
            samples.ColumnCount,
            samples.RowCount,
            maximumAbsolute,
            scale,
            (options ?? new MaxAbsScalerOptions()).Clip);
    }

    /// <summary>Folds another batch into the fitted maxima, as <c>partial_fit</c> does.</summary>
    /// <param name="samples">The next batch, row-major, with <see cref="FeatureCount"/> values per row.</param>
    /// <returns>A new scaler covering every batch seen so far; this one is unchanged.</returns>
    /// <exception cref="ArgumentException"><paramref name="samples"/> holds no row, a partial one, or a non-finite value.</exception>
    /// <remarks>Returns a new scaler rather than mutating this one — see <see cref="StandardScaler.PartialFit"/>.</remarks>
    public MaxAbsScaler PartialFit(ReadOnlySpan<double> samples)
    {
        int rows = SampleMatrix.Rows(samples, FeatureCount);
        SampleMatrix.RequireFinite(samples, nameof(samples));

        var maximumAbsolute = new double[FeatureCount];
        for (int feature = 0; feature < FeatureCount; feature++)
        {
            maximumAbsolute[feature] = MaximumAbsolute[feature];
        }

        Widen(samples, maximumAbsolute);

        double[] scale = [.. maximumAbsolute];
        ScaleFloor.Apply(scale);

        return new MaxAbsScaler(FeatureCount, SampleCount + rows, maximumAbsolute, scale, _clips);
    }

    /// <summary>Divides a row-major sample matrix by the fitted maxima.</summary>
    /// <param name="samples">The samples to transform, row-major, with <see cref="FeatureCount"/> values per row.</param>
    /// <returns>A new array of the same length, in <c>[−1, 1]</c> for any value the fit saw.</returns>
    /// <exception cref="ArgumentException"><paramref name="samples"/> holds no row, a partial one, or a non-finite value.</exception>
    public double[] Transform(ReadOnlySpan<double> samples)
    {
        SampleMatrix.Rows(samples, FeatureCount);
        SampleMatrix.RequireFinite(samples, nameof(samples));

        var result = new double[samples.Length];
        int width = FeatureCount;
        for (int start = 0; start < samples.Length; start += width)
        {
            ReadOnlySpan<double> row = samples.Slice(start, width);
            Span<double> output = result.AsSpan(start, width);
            for (int feature = 0; feature < row.Length; feature++)
            {
                double value = row[feature] / _scale[feature];
                output[feature] = _clips ? Bounds.Clamp(value, -1.0, 1.0) : value;
            }
        }

        return result;
    }

    /// <summary>Divides a sparse matrix by the fitted maxima, keeping its structure.</summary>
    /// <param name="samples">The samples to transform, with <see cref="FeatureCount"/> columns.</param>
    /// <returns>A new matrix storing the same positions, in <c>[−1, 1]</c> for any value the fit saw.</returns>
    /// <exception cref="ArgumentNullException"><paramref name="samples"/> is <see langword="null"/>.</exception>
    /// <exception cref="ArgumentException"><paramref name="samples"/> has another column count, or stores a non-finite value.</exception>
    /// <remarks>A zero stays a zero, so nothing absent becomes stored — <c>MaxAbsScaler.transform</c> on a CSR matrix.</remarks>
    public CsrMatrix Transform(CsrMatrix samples)
    {
        CsrMatrix result = SparseColumns.Divided(samples, FeatureCount, _scale, requireFinite: true);
        if (_clips)
        {
            double[] values = result.Values;
            for (int i = 0; i < values.Length; i++)
            {
                values[i] = Bounds.Clamp(values[i], -1.0, 1.0);
            }
        }

        return result;
    }

    /// <summary>Undoes <see cref="Transform(ReadOnlySpan{double})"/>, returning values on the original scale.</summary>
    /// <param name="samples">The transformed samples, row-major, with <see cref="FeatureCount"/> values per row.</param>
    /// <returns>A new array of the same length, back on the input scale.</returns>
    /// <exception cref="ArgumentException"><paramref name="samples"/> holds no row, a partial one, or a non-finite value.</exception>
    /// <remarks>Never clips, as the reference does not — see <see cref="MinMaxScaler.InverseTransform"/> for why.</remarks>
    public double[] InverseTransform(ReadOnlySpan<double> samples)
    {
        SampleMatrix.Rows(samples, FeatureCount);
        SampleMatrix.RequireFinite(samples, nameof(samples));

        var result = new double[samples.Length];
        int width = FeatureCount;
        for (int start = 0; start < samples.Length; start += width)
        {
            ReadOnlySpan<double> row = samples.Slice(start, width);
            Span<double> output = result.AsSpan(start, width);
            for (int feature = 0; feature < row.Length; feature++)
            {
                output[feature] = row[feature] * _scale[feature];
            }
        }

        return result;
    }

    /// <summary>Undoes <see cref="Transform(CsrMatrix)"/> on a sparse matrix, keeping its structure.</summary>
    /// <param name="samples">The transformed samples, with <see cref="FeatureCount"/> columns.</param>
    /// <returns>A new matrix storing the same positions, back on the input scale.</returns>
    /// <exception cref="ArgumentNullException"><paramref name="samples"/> is <see langword="null"/>.</exception>
    /// <exception cref="ArgumentException"><paramref name="samples"/> has another column count, or stores a non-finite value.</exception>
    /// <remarks>Never clips, as the dense overload does not.</remarks>
    public CsrMatrix InverseTransform(CsrMatrix samples)
    {
        return SparseColumns.Multiplied(samples, FeatureCount, _scale, requireFinite: true);
    }

    /// <summary>Folds every row into the running per-feature largest absolute value, in row order.</summary>
    private static void Widen(ReadOnlySpan<double> samples, double[] maximumAbsolute)
    {
        int width = maximumAbsolute.Length;
        for (int start = 0; start < samples.Length; start += width)
        {
            ReadOnlySpan<double> row = samples.Slice(start, width);
            for (int feature = 0; feature < row.Length; feature++)
            {
                maximumAbsolute[feature] = Math.Max(maximumAbsolute[feature], Math.Abs(row[feature]));
            }
        }
    }
}
