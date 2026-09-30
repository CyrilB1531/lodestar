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
        Mean = _mean is null ? null : Array.AsReadOnly(_mean);
        _variance = variance;
        Variance = _variance is null ? null : Array.AsReadOnly(_variance);
        _scale = scale;
        Scale = _scale is null ? null : Array.AsReadOnly(_scale);
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
    public IReadOnlyList<double>? Mean { get; }

    /// <summary>Per-feature population variance, or <see langword="null"/> when not scaling.</summary>
    public IReadOnlyList<double>? Variance { get; }

    /// <summary>What <see cref="Transform(ReadOnlySpan{double})"/> divides by, or <see langword="null"/> when not scaling.</summary>
    /// <remarks>The square root of <see cref="Variance"/>, except on a near-constant feature — see <see cref="Fit(ReadOnlySpan{double}, int, StandardScalerOptions)"/>.</remarks>
    public IReadOnlyList<double>? Scale { get; }

    /// <summary>Fits a scaler on a row-major sample matrix.</summary>
    /// <param name="samples">The samples, row-major: <paramref name="featureCount"/> values per row.</param>
    /// <param name="featureCount">How many values each row carries.</param>
    /// <param name="options">Which steps to apply; <see langword="null"/> applies both.</param>
    /// <returns>A fitted scaler.</returns>
    /// <exception cref="ArgumentOutOfRangeException"><paramref name="featureCount"/> is not positive.</exception>
    /// <exception cref="ArgumentException"><paramref name="samples"/> holds no row, a partial one, or a non-finite value.</exception>
    /// <remarks>
    /// The population variance (<c>ddof=0</c>) in <c>_incremental_mean_and_var</c>'s passes and numpy's order of
    /// additions, an overflowing column's infinite or NaN as the reference's. <strong>A near-constant feature scales by
    /// 1</strong>: not <c>variance == 0</c> but the two-pass error bound of Chan, Golub and LeVeque, read from
    /// <c>sklearn.preprocessing._data._is_constant_feature</c> (BSD-3, decision 0002); the reference page has the rest.
    /// </remarks>
    public static StandardScaler Fit(
        ReadOnlySpan<double> samples, int featureCount, StandardScalerOptions? options = null)
    {
        Guard.NotLessThan(featureCount, 1);
        int sampleCount = SampleMatrix.Rows(samples, featureCount);
        SampleMatrix.RequireFinite(samples, nameof(samples));
        return FitChecked(samples, featureCount, sampleCount, options ?? new StandardScalerOptions());
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

        CsrMatrix read = SparseColumns.RequireFinite(samples, nameof(samples), SparseColumns.FitReason);

        int featureCount = samples.ColumnCount;
        int sampleCount = samples.RowCount;

        // mean_variance_axis's two-pass form (#1228), over what ToDense reads (#1044).
        (double[] mean, double[] twoPass) = SparseMoments.MeanVarianceOfConsolidated(read);

        if (!settings.WithStd)
        {
            return new StandardScaler(featureCount, sampleCount, mean, null, null, centres: false);
        }

        var variance = new double[featureCount];
        for (int feature = 0; feature < featureCount; feature++)
        {
            // mean_variance_axis's two-pass form, the reference's own sparse path; E[x^2] - E[x]^2
            // cancelled on a column with a large offset (#1228). A rounding below zero stays at zero.
            variance[feature] = Math.Max(twoPass[feature], 0.0);
        }

        return new StandardScaler(
            featureCount, sampleCount, mean, variance, Divisors(variance, mean, sampleCount), centres: false);
    }

    /// <summary>Folds another batch into the fitted statistics, as <c>partial_fit</c> does.</summary>
    /// <param name="samples">The next batch, row-major, with <see cref="FeatureCount"/> values per row.</param>
    /// <returns>A new scaler summarising every batch seen so far; this one is unchanged.</returns>
    /// <exception cref="ArgumentException"><paramref name="samples"/> holds no row, a partial one, or a non-finite value.</exception>
    /// <remarks>
    /// <strong>Returns a new scaler rather than mutating this one</strong>, the one place the
    /// spelling differs from the reference. Chaining batches and fitting the concatenation agree to
    /// a couple of units in the last place — <c>3.3e-16</c> relative, measured on the reference's
    /// own two paths — rather than bit for bit, and the corpus freezes both. A column whose sum overflows takes
    /// numpy's order, as <see cref="Fit(ReadOnlySpan{double}, int, StandardScalerOptions)"/> does: infinite or NaN.
    /// </remarks>
    public StandardScaler PartialFit(ReadOnlySpan<double> samples)
    {
        int rows = SampleMatrix.Rows(samples, FeatureCount);
        SampleMatrix.RequireFinite(samples, nameof(samples));
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
    /// <exception cref="ArgumentException"><paramref name="samples"/> holds no row, a partial one, or a non-finite value.</exception>
    public double[] Transform(ReadOnlySpan<double> samples) => Checked(samples, inverse: false);

    /// <summary>Scales a sparse matrix by the fitted deviations, keeping its structure.</summary>
    /// <param name="samples">The samples to transform, with <see cref="FeatureCount"/> columns.</param>
    /// <returns>A new matrix storing the same positions, divided by <see cref="Scale"/>, or a copy when not scaling.</returns>
    /// <exception cref="ArgumentNullException"><paramref name="samples"/> is <see langword="null"/>.</exception>
    /// <exception cref="ArgumentException"><paramref name="samples"/> holds no row, has another column count, or stores a non-finite value.</exception>
    /// <exception cref="InvalidOperationException">This scaler centres, which a sparse matrix cannot be.</exception>
    /// <remarks>
    /// <c>StandardScaler.transform</c> on a CSR matrix, which raises the same refusal: subtracting the
    /// mean would make every absent zero a stored value. Fit with <see cref="StandardScalerOptions.WithMean"/> off.
    /// </remarks>
    public CsrMatrix Transform(CsrMatrix samples)
    {
        SparseColumns.RefuseCentring(samples, _centres, nameof(StandardScalerOptions.WithMean));
        return SparseColumns.Divided(samples, FeatureCount, _scale, requireFinite: true);
    }

    /// <summary>Undoes <see cref="Transform(ReadOnlySpan{double})"/>, returning values on the original scale.</summary>
    /// <param name="samples">The standardised samples, row-major, with <see cref="FeatureCount"/> values per row.</param>
    /// <returns>A new array of the same length, back on the input scale.</returns>
    /// <exception cref="ArgumentException"><paramref name="samples"/> holds no row, a partial one, or a non-finite value.</exception>
    /// <remarks>
    /// Exact only up to floating-point rounding, and not at all for a feature whose
    /// <see cref="Scale"/> was forced to 1 — that step threw the feature's spread away
    /// rather than recording it, which is the point of forcing it.
    /// </remarks>
    public double[] InverseTransform(ReadOnlySpan<double> samples) => Checked(samples, inverse: true);

    /// <summary>Undoes <see cref="Transform(CsrMatrix)"/> on a sparse matrix, keeping its structure.</summary>
    /// <param name="samples">The standardised samples, with <see cref="FeatureCount"/> columns.</param>
    /// <returns>A new matrix storing the same positions, multiplied by <see cref="Scale"/>, or a copy when not scaling.</returns>
    /// <exception cref="ArgumentNullException"><paramref name="samples"/> is <see langword="null"/>.</exception>
    /// <exception cref="ArgumentException"><paramref name="samples"/> holds no row, has another column count, or stores a non-finite value.</exception>
    /// <exception cref="InvalidOperationException">This scaler centres, which a sparse matrix cannot be.</exception>
    public CsrMatrix InverseTransform(CsrMatrix samples)
    {
        SparseColumns.RefuseCentring(samples, _centres, nameof(StandardScalerOptions.WithMean));
        return SparseColumns.Multiplied(samples, FeatureCount, _scale, requireFinite: true);
    }

    /// <summary>
    /// <see cref="Fit(ReadOnlySpan{double}, int, StandardScalerOptions)"/> past its refusals, for a
    /// <c>PowerTransformer</c> that made them already over the powers it hands in.
    /// </summary>
    internal static StandardScaler FitChecked(
        ReadOnlySpan<double> samples, int featureCount, int sampleCount, StandardScalerOptions settings)
    {
        if (!settings.WithMean && !settings.WithStd)
        {
            return new StandardScaler(featureCount, sampleCount, null, null, null, centres: false);
        }

        // _incremental_mean_and_var's sums, in numpy's order: an overflowing column ends in ∞ or NaN where the
        // reference's does, and a nearly constant one standardises as the reference's does (#1230).
        if (!settings.WithStd)
        {
            // last_variance=None: the reference takes the sum alone, (0 + Σx) / n.
            var mean = new double[featureCount];
            NumpyReduction.ColumnSums(samples, sampleCount, featureCount, mean);
            for (int feature = 0; feature < featureCount; feature++)
            {
                mean[feature] = (0.0 + mean[feature]) / sampleCount;
            }

            return new StandardScaler(featureCount, sampleCount, mean, null, null, settings.WithMean);
        }

        // From nothing, as the reference's first batch: PartialFit's own fold, with no moments behind it.
        (double[] means, double[] variance) = IncrementalMoments.Update(
            samples, featureCount, sampleCount, default, default, 0);
        double[] scale = Divisors(variance, means, sampleCount);
        return new StandardScaler(featureCount, sampleCount, means, variance, scale, settings.WithMean);
    }

    /// <summary>The mean alone, folded — what a scaler that never computed a variance can update.</summary>
    private double[] FoldedMean(ReadOnlySpan<double> samples, int rows, int updated)
    {
        // The reference's order: the batch summed as numpy sums an axis, then added to the last sum (#1230).
        var mean = new double[FeatureCount];
        NumpyReduction.ColumnSums(samples, rows, FeatureCount, mean);
        for (int feature = 0; feature < FeatureCount; feature++)
        {
            double lastSum = _mean![feature] * SampleCount;
            mean[feature] = (lastSum + mean[feature]) / updated;
        }

        return mean;
    }

    /// <summary>
    /// <see cref="Transform(ReadOnlySpan{double})"/> written over <paramref name="values"/>, past its refusals: the
    /// powers a standardising <c>PowerTransformer</c> made and checked, which a second array only copied (#1230).
    /// </summary>
    internal void TransformInPlace(double[] values) => Apply(values, values, inverse: false);

    /// <summary><see cref="InverseTransform(ReadOnlySpan{double})"/> written over <paramref name="values"/>, its twin.</summary>
    internal void InverseTransformInPlace(double[] values) => Apply(values, values, inverse: true);

    /// <summary>Both directions, refused as the previous release refused them, into a new array.</summary>
    private double[] Checked(ReadOnlySpan<double> samples, bool inverse)
    {
        SampleMatrix.Rows(samples, FeatureCount);
        SampleMatrix.RequireFinite(samples, nameof(samples));
        var result = new double[samples.Length];
        Apply(samples, result, inverse);
        return result;
    }

    /// <summary>Both directions, which differ only in the order and sense of the two steps.</summary>
    private void Apply(ReadOnlySpan<double> samples, double[] result, bool inverse)
    {
        // An absent centre is a row of zeros that is still added, not skipped: -0.0 + 0.0 is +0.0,
        // and the element-wise version wrote that sum. _mean alone does not say whether to centre.
        ReadOnlySpan<double> centre = _centres && _mean is not null ? _mean : new double[FeatureCount];
        if (_scale is null)
        {
            Shift(samples, centre, result, inverse);
        }
        else if (inverse)
        {
            Restore(samples, centre, _scale, result);
        }
        else
        {
            Standardise(samples, centre, _scale, result);
        }
    }

    /// <summary>Only the centring step, added back or taken away, row by row.</summary>
    private static void Shift(ReadOnlySpan<double> samples, ReadOnlySpan<double> centre, double[] result, bool add)
    {
        int width = centre.Length;
        for (int start = 0; start < samples.Length; start += width)
        {
            ReadOnlySpan<double> row = samples.Slice(start, width);
            Span<double> output = result.AsSpan(start, width);
            for (int feature = 0; feature < row.Length; feature++)
            {
                output[feature] = add ? row[feature] + centre[feature] : row[feature] - centre[feature];
            }
        }
    }

    private static void Standardise(
        ReadOnlySpan<double> samples, ReadOnlySpan<double> centre, double[] scale, double[] result)
    {
        int width = centre.Length;
        for (int start = 0; start < samples.Length; start += width)
        {
            ReadOnlySpan<double> row = samples.Slice(start, width);
            Span<double> output = result.AsSpan(start, width);
            for (int feature = 0; feature < row.Length; feature++)
            {
                output[feature] = (row[feature] - centre[feature]) / scale[feature];
            }
        }
    }

    private static void Restore(
        ReadOnlySpan<double> samples, ReadOnlySpan<double> centre, double[] scale, double[] result)
    {
        int width = centre.Length;
        for (int start = 0; start < samples.Length; start += width)
        {
            ReadOnlySpan<double> row = samples.Slice(start, width);
            Span<double> output = result.AsSpan(start, width);
            for (int feature = 0; feature < row.Length; feature++)
            {
                output[feature] = (row[feature] * scale[feature]) + centre[feature];
            }
        }
    }

    /// <summary>The divisor, with a near-constant feature forced to 1 — see <see cref="Fit(ReadOnlySpan{double}, int, StandardScalerOptions)"/>.</summary>
    private static double[] Divisors(double[] variance, double[] mean, int sampleCount)
    {
        var scale = new double[variance.Length];
        for (int feature = 0; feature < variance.Length; feature++)
        {
            scale[feature] = ConstantFeature.IsConstant(sampleCount, mean[feature], variance[feature])
                ? 1.0
                : Math.Sqrt(variance[feature]);
        }

        return scale;
    }
}
