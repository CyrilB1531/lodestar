using Lodestar.Abstractions;
using Lodestar.Preprocessing.Internal;

namespace Lodestar.Preprocessing;

/// <summary>
/// Centres each feature on its mean and scales it to unit variance, at
/// <c>sklearn.preprocessing.StandardScaler</c> parity.
/// </summary>
/// <remarks>
/// Spans in, arrays out. A sample matrix is row-major — <see cref="FeatureCount"/>
/// values per row — the shape <c>Lodestar.Metrics</c> already uses, so a matrix does not
/// have to be reshaped to cross between the two packages.
/// </remarks>
public sealed class StandardScaler
{
    private readonly double[]? _mean;
    private readonly double[]? _variance;
    private readonly double[]? _scale;

    // Which steps to apply, kept apart from which statistics exist: with_mean=False still
    // computes mean_, so reporting a mean is not the same as subtracting one.
    private readonly bool _centres;

    private StandardScaler(
        int featureCount,
        int sampleCount,
        double[]? mean,
        double[]? variance,
        double[]? scale,
        bool centres)
    {
        FeatureCount = featureCount;
        SampleCount = sampleCount;
        _mean = mean;
        _variance = variance;
        _scale = scale;
        _centres = centres;
    }

    /// <summary>How many values each row of a sample matrix carries.</summary>
    public int FeatureCount { get; }

    /// <summary>How many rows the scaler was fitted on — scikit-learn's <c>n_samples_seen_</c>.</summary>
    public int SampleCount { get; }

    /// <summary>Per-feature mean, or <see langword="null"/> when neither step needs one.</summary>
    /// <remarks>
    /// <see langword="null"/> only when <see cref="StandardScalerOptions.WithMean"/> and
    /// <see cref="StandardScalerOptions.WithStd"/> are <em>both</em> off, which is what
    /// scikit-learn does: <c>with_mean=False</c> alone still computes <c>mean_</c>.
    /// </remarks>
    public IReadOnlyList<double>? Mean => _mean;

    /// <summary>Per-feature population variance, or <see langword="null"/> when not scaling.</summary>
    public IReadOnlyList<double>? Variance => _variance;

    /// <summary>What <see cref="Transform"/> divides by, or <see langword="null"/> when not scaling.</summary>
    /// <remarks>The square root of <see cref="Variance"/>, except on a near-constant feature — see <see cref="Fit(ReadOnlySpan{double}, int, StandardScalerOptions)"/>.</remarks>
    public IReadOnlyList<double>? Scale => _scale;

    /// <summary>Fits a scaler on a row-major sample matrix.</summary>
    /// <param name="samples">The samples, row-major: <paramref name="featureCount"/> values per row.</param>
    /// <param name="featureCount">How many values each row carries.</param>
    /// <param name="options">Which steps to apply; <see langword="null"/> applies both.</param>
    /// <returns>A fitted scaler.</returns>
    /// <exception cref="ArgumentOutOfRangeException"><paramref name="featureCount"/> is not positive.</exception>
    /// <exception cref="ArgumentException"><paramref name="samples"/> holds no row, or a partial one.</exception>
    /// <remarks>
    /// Variance is the population variance (<c>ddof=0</c>), in two passes as numpy's is.
    /// <strong>A near-constant feature scales by 1</strong>, and the test is not
    /// <c>variance == 0</c> but the two-pass error bound of Chan, Golub and LeVeque, read
    /// from <c>sklearn.preprocessing._data._is_constant_feature</c> (BSD-3, allowed by
    /// decision 0003). The reference page carries the formula and the corpus pair that
    /// separates the two readings.
    /// </remarks>
    public static StandardScaler Fit(
        ReadOnlySpan<double> samples, int featureCount, StandardScalerOptions? options = null)
    {
        Guard.NotLessThan(featureCount, 1);
        int sampleCount = SampleMatrix.Rows(samples, featureCount);
        StandardScalerOptions settings = options ?? new StandardScalerOptions();

        if (!settings.WithMean && !settings.WithStd)
        {
            return new StandardScaler(featureCount, sampleCount, null, null, null, centres: false);
        }

        double[] mean = ColumnMeans(samples, featureCount, sampleCount);
        if (!settings.WithStd)
        {
            return new StandardScaler(featureCount, sampleCount, mean, null, null, settings.WithMean);
        }

        double[] variance = ColumnVariances(samples, featureCount, sampleCount, mean);
        double[] scale = Divisors(variance, mean, sampleCount);
        return new StandardScaler(featureCount, sampleCount, mean, variance, scale, settings.WithMean);
    }

    /// <summary>Fits a scaler on a sparse matrix, which centring cannot be asked of.</summary>
    /// <param name="samples">The samples, one row per matrix row.</param>
    /// <param name="options">Which steps to apply; <see cref="StandardScalerOptions.WithMean"/> must be off.</param>
    /// <returns>A fitted scaler, its mean reported and never subtracted.</returns>
    /// <exception cref="ArgumentNullException"><paramref name="samples"/> is <see langword="null"/>.</exception>
    /// <exception cref="ArgumentException"><paramref name="samples"/> holds no row or a non-finite value.</exception>
    /// <exception cref="ArgumentOutOfRangeException">Centring is asked for.</exception>
    /// <remarks>
    /// <strong>Centring is refused, as the reference refuses it</strong>: subtracting a mean turns
    /// every absent zero into a stored value, so a matrix that fitted in memory sparse does not fit
    /// dense. The variance is still the population variance of the column, the absent zeros counted.
    /// </remarks>
    public static StandardScaler Fit(CsrMatrix samples, StandardScalerOptions? options = null)
    {
        Guard.NotNull(samples);
        StandardScalerOptions settings = options ?? new StandardScalerOptions { WithMean = false };
        if (settings.WithMean)
        {
            throw new ArgumentOutOfRangeException(
                nameof(options),
                settings.WithMean,
                "A sparse matrix cannot be centred: subtracting a mean makes every absent zero a stored "
                + "value. Pass WithMean = false, which is what the reference asks for too.");
        }

        if (samples.RowCount == 0 || samples.ColumnCount == 0)
        {
            throw new ArgumentException("samples holds no row or no column.", nameof(samples));
        }

        SparseColumns.RequireFinite(samples, nameof(samples));

        int featureCount = samples.ColumnCount;
        int sampleCount = samples.RowCount;
        (double[] sums, double[] squares) = SparseColumns.Moments(samples);

        var mean = new double[featureCount];
        for (int feature = 0; feature < featureCount; feature++)
        {
            mean[feature] = sums[feature] / sampleCount;
        }

        if (!settings.WithStd)
        {
            return new StandardScaler(featureCount, sampleCount, mean, null, null, centres: false);
        }

        var variance = new double[featureCount];
        for (int feature = 0; feature < featureCount; feature++)
        {
            // E[x^2] - E[x]^2 over the whole column, the absent zeros contributing to neither sum
            // and to the count of both -- the reference's own sparse path.
            variance[feature] = (squares[feature] / sampleCount) - (mean[feature] * mean[feature]);
            if (variance[feature] < 0.0)
            {
                variance[feature] = 0.0;
            }
        }

        return new StandardScaler(
            featureCount, sampleCount, mean, variance, Divisors(variance, mean, sampleCount), centres: false);
    }

    /// <summary>Folds another batch into the fitted statistics, as <c>partial_fit</c> does.</summary>
    /// <param name="samples">The next batch, row-major, with <see cref="FeatureCount"/> values per row.</param>
    /// <returns>A new scaler summarising every batch seen so far; this one is unchanged.</returns>
    /// <exception cref="ArgumentException"><paramref name="samples"/> holds no row, or a partial one.</exception>
    /// <remarks>
    /// <strong>Returns a new scaler rather than mutating this one</strong>, the one place the
    /// spelling differs from the reference. Chaining batches and fitting the concatenation agree to
    /// a couple of units in the last place — <c>3.3e-16</c> relative, measured on the reference's
    /// own two paths — rather than bit for bit, and the corpus freezes both.
    /// </remarks>
    public StandardScaler PartialFit(ReadOnlySpan<double> samples)
    {
        int rows = SampleMatrix.Rows(samples, FeatureCount);
        int updated = SampleCount + rows;

        if (_mean is null)
        {
            // Both steps off: nothing is fitted, so there is nothing to fold -- only the count moves.
            return new StandardScaler(FeatureCount, updated, null, null, null, _centres);
        }

        if (_variance is null)
        {
            return new StandardScaler(
                FeatureCount, updated, FoldedMean(samples, rows, updated), null, null, _centres);
        }

        (double[] mean, double[] variance) = IncrementalMoments.Update(
            samples, FeatureCount, rows, _mean, _variance, SampleCount);

        return new StandardScaler(
            FeatureCount, updated, mean, variance, Divisors(variance, mean, updated), _centres);
    }

    /// <summary>Standardises a row-major sample matrix with the fitted statistics.</summary>
    /// <param name="samples">The samples to transform, row-major, with <see cref="FeatureCount"/> values per row.</param>
    /// <returns>A new array of the same length, standardised.</returns>
    /// <exception cref="ArgumentException"><paramref name="samples"/> holds no row, or a partial one.</exception>
    public double[] Transform(ReadOnlySpan<double> samples) => Apply(samples, inverse: false);

    /// <summary>Undoes <see cref="Transform"/>, returning values on the original scale.</summary>
    /// <param name="samples">The standardised samples, row-major, with <see cref="FeatureCount"/> values per row.</param>
    /// <returns>A new array of the same length, back on the input scale.</returns>
    /// <exception cref="ArgumentException"><paramref name="samples"/> holds no row, or a partial one.</exception>
    /// <remarks>
    /// Exact only up to floating-point rounding, and not at all for a feature whose
    /// <see cref="Scale"/> was forced to 1 — that step threw the feature's spread away
    /// rather than recording it, which is the point of forcing it.
    /// </remarks>
    public double[] InverseTransform(ReadOnlySpan<double> samples) => Apply(samples, inverse: true);

    /// <summary>The mean alone, folded — what a scaler that never computed a variance can update.</summary>
    private double[] FoldedMean(ReadOnlySpan<double> samples, int rows, int updated)
    {
        var mean = new double[FeatureCount];
        for (int feature = 0; feature < FeatureCount; feature++)
        {
            double total = _mean![feature] * SampleCount;
            for (int row = 0; row < rows; row++)
            {
                total += samples[(row * FeatureCount) + feature];
            }

            mean[feature] = total / updated;
        }

        return mean;
    }

    /// <summary>Both directions, which differ only in the order and sense of the two steps.</summary>
    private double[] Apply(ReadOnlySpan<double> samples, bool inverse)
    {
        SampleMatrix.Rows(samples, FeatureCount);
        var result = new double[samples.Length];

        for (int i = 0; i < samples.Length; i++)
        {
            int feature = i % FeatureCount;
            double value = samples[i];

            if (inverse)
            {
                value = Rescale(value, feature, inverse: true);
                value += Centre(feature);
            }
            else
            {
                value -= Centre(feature);
                value = Rescale(value, feature, inverse: false);
            }

            result[i] = value;
        }

        return result;
    }

    /// <summary>What the centring step subtracts: the mean, or nothing when it is off.</summary>
    /// <remarks><see cref="_mean"/> alone does not answer this — see the field's own note.</remarks>
    private double Centre(int feature) => _centres && _mean is not null ? _mean[feature] : 0.0;

    /// <summary>What the scaling step divides or multiplies by, or nothing when it is off.</summary>
    private double Rescale(double value, int feature, bool inverse)
    {
        if (_scale is null)
        {
            return value;
        }

        return inverse ? value * _scale[feature] : value / _scale[feature];
    }

    /// <summary>Per-feature mean, summed with Neumaier compensation.</summary>
    private static double[] ColumnMeans(ReadOnlySpan<double> samples, int featureCount, int sampleCount)
    {
        var total = new double[featureCount];
        var lost = new double[featureCount];

        for (int i = 0; i < samples.Length; i++)
        {
            Add(ref total[i % featureCount], ref lost[i % featureCount], samples[i]);
        }

        var mean = new double[featureCount];
        for (int feature = 0; feature < featureCount; feature++)
        {
            mean[feature] = (total[feature] + lost[feature]) / sampleCount;
        }

        return mean;
    }

    /// <summary>Per-feature population variance, in a second pass over the deviations.</summary>
    private static double[] ColumnVariances(
        ReadOnlySpan<double> samples, int featureCount, int sampleCount, double[] mean)
    {
        var total = new double[featureCount];
        var lost = new double[featureCount];

        for (int i = 0; i < samples.Length; i++)
        {
            int feature = i % featureCount;
            double deviation = samples[i] - mean[feature];
            Add(ref total[feature], ref lost[feature], deviation * deviation);
        }

        var variance = new double[featureCount];
        for (int feature = 0; feature < featureCount; feature++)
        {
            variance[feature] = (total[feature] + lost[feature]) / sampleCount;
        }

        return variance;
    }

    /// <summary>The divisor, with a near-constant feature forced to 1 — see <see cref="Fit(ReadOnlySpan{double}, int, StandardScalerOptions)"/>.</summary>
    private static double[] Divisors(double[] variance, double[] mean, int sampleCount)
    {
        // long-comment: the threshold is read from a reference rather than derived, so the
        // attribution and the measurement that pins it belong at the line, not on the page.
        // Chan, Golub and LeVeque's error bound for the two-pass variance, as
        // sklearn.preprocessing._data._is_constant_feature applies it. Measured against
        // scikit-learn 1.9.0: three samples at 1e8 +/- 1e-8 have a variance of 1.48e-16,
        // which is not zero and is below this bound, and the reference scales them by 1.
        const double Epsilon = 2.220446049250313e-16;

        var scale = new double[variance.Length];
        for (int feature = 0; feature < variance.Length; feature++)
        {
            double shift = sampleCount * mean[feature] * Epsilon;
            double bound = (sampleCount * Epsilon * variance[feature]) + (shift * shift);
            scale[feature] = variance[feature] <= bound ? 1.0 : Math.Sqrt(variance[feature]);
        }

        return scale;
    }

    /// <summary>Neumaier's compensated addition, so a long column does not drift.</summary>
    private static void Add(ref double total, ref double lost, double value)
    {
        double sum = total + value;
        lost += Math.Abs(total) >= Math.Abs(value)
            ? (total - sum) + value
            : (value - sum) + total;
        total = sum;
    }
}
