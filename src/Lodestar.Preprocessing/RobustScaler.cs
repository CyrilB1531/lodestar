using Lodestar.Abstractions;
using Lodestar.Preprocessing.Internal;
using Lodestar.Stats;

namespace Lodestar.Preprocessing;

/// <summary>
/// Centres each feature on its median and scales it by an interpercentile range, at
/// <c>sklearn.preprocessing.RobustScaler</c> parity.
/// </summary>
/// <remarks>
/// The scaler for data with outliers: a median and a quartile range move with the bulk of the
/// column, where <see cref="StandardScaler"/>'s mean and deviation move with whatever is furthest
/// from it.
/// </remarks>
public sealed class RobustScaler
{
    private readonly double[]? _centre;
    private readonly double[]? _scale;

    private RobustScaler(int featureCount, int sampleCount, double[]? centre, double[]? scale)
    {
        FeatureCount = featureCount;
        SampleCount = sampleCount;
        _centre = centre;
        _scale = scale;
    }

    /// <summary>How many values each row of a sample matrix carries.</summary>
    public int FeatureCount { get; }

    /// <summary>How many rows the scaler was fitted on.</summary>
    public int SampleCount { get; }

    /// <summary>Each feature's median, or <see langword="null"/> when centring is off — <c>center_</c>.</summary>
    public IReadOnlyList<double>? Centre => _centre;

    /// <summary>The interpercentile range divided by, or <see langword="null"/> when scaling is off — <c>scale_</c>.</summary>
    public IReadOnlyList<double>? Scale => _scale;

    /// <summary>Fits a scaler on a row-major sample matrix.</summary>
    /// <param name="samples">The samples, row-major: <paramref name="featureCount"/> values per row.</param>
    /// <param name="featureCount">How many values each row carries.</param>
    /// <param name="options">Which steps to apply and between which percentiles; <see langword="null"/> is both, at the quartiles.</param>
    /// <returns>A fitted scaler.</returns>
    /// <exception cref="ArgumentOutOfRangeException">
    /// <paramref name="featureCount"/> is not positive; the percentile range is not
    /// <c>0 ≤ low ≤ high ≤ 100</c>; or <see cref="RobustScalerOptions.UnitVariance"/> is set with a
    /// percentile of 0 or 100, which has no finite normal quantile.
    /// </exception>
    /// <exception cref="ArgumentException"><paramref name="samples"/> holds no row, a partial one, or a non-finite value.</exception>
    /// <remarks>
    /// Both statistics read a sorted column: the median is <c>numpy.median</c> and the percentiles are
    /// <c>numpy.percentile</c>'s linear interpolation (Hyndman–Fan type 7), which is the convention the
    /// reference uses and not the one <c>SplitConformal.Quantile</c> does.
    /// </remarks>
    public static RobustScaler Fit(
        ReadOnlySpan<double> samples, int featureCount, RobustScalerOptions? options = null)
    {
        Guard.NotLessThan(featureCount, 1);
        RobustScalerOptions settings = options ?? new RobustScalerOptions();
        RequirePercentiles(settings);

        int sampleCount = SampleMatrix.Rows(samples, featureCount);
        SampleMatrix.RequireFinite(samples, nameof(samples));

        double[]? centre = settings.WithCentring ? new double[featureCount] : null;
        double[]? scale = settings.WithScaling ? new double[featureCount] : null;
        if (centre is null && scale is null)
        {
            return new RobustScaler(featureCount, sampleCount, null, null);
        }

        Populate(samples, featureCount, sampleCount, settings, centre, scale);
        if (scale is not null)
        {
            // Floored first and divided second, which is the reference's order: a feature the floor
            // caught comes out at 1/adjust rather than at 1.
            ScaleFloor.Apply(scale);
            ApplyUnitVariance(scale, settings);
        }

        return new RobustScaler(featureCount, sampleCount, centre, scale);
    }

    /// <summary>Fits a scaler on a sparse matrix, which centring cannot be asked of.</summary>
    /// <param name="samples">The samples, one row per matrix row.</param>
    /// <param name="options">Which steps to apply; <see cref="RobustScalerOptions.WithCentring"/> must be off.</param>
    /// <returns>A fitted scaler, its centre <see langword="null"/>.</returns>
    /// <exception cref="ArgumentNullException"><paramref name="samples"/> is <see langword="null"/>.</exception>
    /// <exception cref="ArgumentException"><paramref name="samples"/> holds no row or a non-finite value.</exception>
    /// <exception cref="ArgumentOutOfRangeException">Centring is asked for, or the percentile range is out of range.</exception>
    /// <remarks>
    /// <strong>Centring is refused, as the reference refuses it</strong>: subtracting a median turns
    /// every absent zero into a stored value. The percentiles still read the whole column, the
    /// absent zeros included — which is why a sparse column's quartiles are usually zero.
    /// </remarks>
    public static RobustScaler Fit(CsrMatrix samples, RobustScalerOptions? options = null)
    {
        Guard.NotNull(samples);
        RobustScalerOptions settings = options ?? new RobustScalerOptions { WithCentring = false };
        if (settings.WithCentring)
        {
            throw new ArgumentOutOfRangeException(
                nameof(options),
                settings.WithCentring,
                "A sparse matrix cannot be centred: subtracting a median makes every absent zero a stored "
                + "value. Pass WithCentring = false, which is what the reference asks for too.");
        }

        RequirePercentiles(settings);
        if (samples.RowCount == 0 || samples.ColumnCount == 0)
        {
            throw new ArgumentException("samples holds no row or no column.", nameof(samples));
        }

        SparseColumns.RequireFinite(samples, nameof(samples), SparseColumns.FitReason);

        if (!settings.WithScaling)
        {
            return new RobustScaler(samples.ColumnCount, samples.RowCount, null, null);
        }

        var scale = new double[samples.ColumnCount];
        (double[] grouped, int[] offsets) = SparseColumns.ByColumn(samples);
        var buffer = new double[samples.RowCount];
        for (int feature = 0; feature < samples.ColumnCount; feature++)
        {
            double[] column = SparseColumns.SortedColumn(grouped, offsets, feature, buffer);
            scale[feature] = Percentile.Linear(column, settings.UpperPercentile)
                - Percentile.Linear(column, settings.LowerPercentile);
        }

        ScaleFloor.Apply(scale);
        ApplyUnitVariance(scale, settings);

        return new RobustScaler(samples.ColumnCount, samples.RowCount, null, scale);
    }

    /// <summary>The median and the interpercentile range of every feature, from its sorted column.</summary>
    private static void Populate(
        ReadOnlySpan<double> samples,
        int featureCount,
        int sampleCount,
        RobustScalerOptions settings,
        double[]? centre,
        double[]? scale)
    {
        for (int feature = 0; feature < featureCount; feature++)
        {
            double[] column = Percentile.SortedColumn(samples, featureCount, feature, sampleCount);
            if (centre is not null)
            {
                centre[feature] = Percentile.Median(column);
            }

            if (scale is not null)
            {
                scale[feature] = Percentile.Linear(column, settings.UpperPercentile)
                    - Percentile.Linear(column, settings.LowerPercentile);
            }
        }
    }

    /// <summary>Divides the range so a normal column comes out with unit variance, when asked.</summary>
    private static void ApplyUnitVariance(double[] scale, RobustScalerOptions settings)
    {
        if (!settings.UnitVariance)
        {
            return;
        }

        double adjust = UnitVarianceDivisor(settings);
        for (int feature = 0; feature < scale.Length; feature++)
        {
            scale[feature] /= adjust;
        }
    }

    /// <summary>The percentile range the reference accepts: <c>0 ≤ lower ≤ upper ≤ 100</c>.</summary>
    private static void RequirePercentiles(RobustScalerOptions options)
    {
        if (!(options.LowerPercentile >= 0.0
              && options.LowerPercentile <= options.UpperPercentile
              && options.UpperPercentile <= 100.0))
        {
            throw new ArgumentOutOfRangeException(
                nameof(options),
                $"({options.LowerPercentile}, {options.UpperPercentile})",
                "A percentile range satisfies 0 <= lower <= upper <= 100, as the reference requires.");
        }
    }

    /// <summary>What <c>unit_variance</c> divides the range by, from the published normal quantile.</summary>
    /// <remarks>
    /// <c>Φ⁻¹(upper/100) − Φ⁻¹(lower/100)</c>, which is 1.3489795 at the quartiles. The quantile is
    /// <c>Lodestar.Stats</c>' published one rather than a second copy of it, the edge decision 0138
    /// took. A percentile of 0 or 100 has no finite quantile, so it is refused here where the
    /// reference returns an infinity and a scale of zero.
    /// </remarks>
    private static double UnitVarianceDivisor(RobustScalerOptions options)
    {
        if (options.LowerPercentile <= 0.0 || options.UpperPercentile >= 100.0)
        {
            throw new ArgumentOutOfRangeException(
                nameof(options),
                $"({options.LowerPercentile}, {options.UpperPercentile})",
                "Unit variance divides by the normal quantiles of the percentile range, and 0 and 100 "
                + "have none. Narrow the range, or leave UnitVariance off.");
        }

        return Distributions.NormalQuantile(options.UpperPercentile / 100.0)
            - Distributions.NormalQuantile(options.LowerPercentile / 100.0);
    }

    /// <summary>Centres and scales a row-major sample matrix with the fitted statistics.</summary>
    /// <param name="samples">The samples to transform, row-major, with <see cref="FeatureCount"/> values per row.</param>
    /// <returns>A new array of the same length.</returns>
    /// <exception cref="ArgumentException"><paramref name="samples"/> holds no row, a partial one, or a non-finite value.</exception>
    public double[] Transform(ReadOnlySpan<double> samples) => Apply(samples, inverse: false);

    /// <summary>Scales a sparse matrix by the fitted ranges, keeping its structure.</summary>
    /// <param name="samples">The samples to transform, with <see cref="FeatureCount"/> columns.</param>
    /// <returns>A new matrix storing the same positions, divided by <see cref="Scale"/>, or a copy when not scaling.</returns>
    /// <exception cref="ArgumentNullException"><paramref name="samples"/> is <see langword="null"/>.</exception>
    /// <exception cref="ArgumentException"><paramref name="samples"/> has another column count, or stores a non-finite value.</exception>
    /// <exception cref="InvalidOperationException">This scaler centres, which a sparse matrix cannot be.</exception>
    /// <remarks>
    /// <c>RobustScaler.transform</c> on a CSR matrix skips the centring without a word; this refuses it
    /// instead, as the sparse <see cref="Fit(CsrMatrix, RobustScalerOptions)"/> does, since the result would not be centred.
    /// </remarks>
    public CsrMatrix Transform(CsrMatrix samples)
    {
        SparseColumns.RefuseCentring(samples, _centre is not null, nameof(RobustScalerOptions.WithCentring));
        return SparseColumns.Divided(samples, FeatureCount, _scale, requireFinite: true);
    }

    /// <summary>Undoes <see cref="Transform(ReadOnlySpan{double})"/>, returning values on the original scale.</summary>
    /// <param name="samples">The transformed samples, row-major, with <see cref="FeatureCount"/> values per row.</param>
    /// <returns>A new array of the same length, back on the input scale.</returns>
    /// <exception cref="ArgumentException"><paramref name="samples"/> holds no row, a partial one, or a non-finite value.</exception>
    /// <remarks>
    /// Exact up to floating-point rounding, except for a feature whose range was floored to 1 — that
    /// step threw the spread away rather than recording it.
    /// </remarks>
    public double[] InverseTransform(ReadOnlySpan<double> samples) => Apply(samples, inverse: true);

    /// <summary>Undoes <see cref="Transform(CsrMatrix)"/> on a sparse matrix, keeping its structure.</summary>
    /// <param name="samples">The transformed samples, with <see cref="FeatureCount"/> columns.</param>
    /// <returns>A new matrix storing the same positions, multiplied by <see cref="Scale"/>, or a copy when not scaling.</returns>
    /// <exception cref="ArgumentNullException"><paramref name="samples"/> is <see langword="null"/>.</exception>
    /// <exception cref="ArgumentException"><paramref name="samples"/> has another column count, or stores a non-finite value.</exception>
    /// <exception cref="InvalidOperationException">This scaler centres, which a sparse matrix cannot be.</exception>
    public CsrMatrix InverseTransform(CsrMatrix samples)
    {
        SparseColumns.RefuseCentring(samples, _centre is not null, nameof(RobustScalerOptions.WithCentring));
        return SparseColumns.Multiplied(samples, FeatureCount, _scale, requireFinite: true);
    }

    /// <summary>Both directions: subtract then divide, or multiply then add — the reference's order.</summary>
    private double[] Apply(ReadOnlySpan<double> samples, bool inverse)
    {
        SampleMatrix.Rows(samples, FeatureCount);
        SampleMatrix.RequireFinite(samples, nameof(samples));

        var result = new double[samples.Length];
        for (int i = 0; i < samples.Length; i++)
        {
            int feature = i % FeatureCount;
            double value = samples[i];
            double centre = _centre is null ? 0.0 : _centre[feature];
            double scale = _scale is null ? 1.0 : _scale[feature];

            result[i] = inverse ? (value * scale) + centre : (value - centre) / scale;
        }

        return result;
    }
}
