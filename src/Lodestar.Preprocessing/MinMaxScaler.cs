using Lodestar.Preprocessing.Internal;

namespace Lodestar.Preprocessing;

/// <summary>
/// Maps each feature onto a fixed range, at <c>sklearn.preprocessing.MinMaxScaler</c> parity.
/// </summary>
/// <remarks>
/// Spans in, arrays out, row-major — <see cref="FeatureCount"/> values per row, the shape
/// <see cref="StandardScaler"/> and <c>Lodestar.Metrics</c> already use.
/// </remarks>
public sealed class MinMaxScaler
{
    private readonly double[] _scale;
    private readonly double[] _minimum;
    private readonly MinMaxScalerOptions _options;

    private MinMaxScaler(
        int featureCount,
        int sampleCount,
        double[] dataMinimum,
        double[] dataMaximum,
        double[] scale,
        double[] minimum,
        MinMaxScalerOptions options)
    {
        FeatureCount = featureCount;
        SampleCount = sampleCount;
        DataMinimum = dataMinimum;
        DataMaximum = dataMaximum;
        var dataRange = new double[featureCount];
        for (int feature = 0; feature < featureCount; feature++)
        {
            dataRange[feature] = dataMaximum[feature] - dataMinimum[feature];
        }

        DataRange = dataRange;
        _scale = scale;
        _minimum = minimum;
        _options = options;
    }

    /// <summary>How many values each row of a sample matrix carries.</summary>
    public int FeatureCount { get; }

    /// <summary>How many rows the scaler was fitted on — scikit-learn's <c>n_samples_seen_</c>.</summary>
    public int SampleCount { get; }

    /// <summary>Each feature's smallest fitted value — <c>data_min_</c>.</summary>
    public IReadOnlyList<double> DataMinimum { get; }

    /// <summary>Each feature's largest fitted value — <c>data_max_</c>.</summary>
    public IReadOnlyList<double> DataMaximum { get; }

    /// <summary>The fitted spread, <c>DataMaximum − DataMinimum</c> — <c>data_range_</c>.</summary>
    /// <remarks>What <see cref="Transform"/> divides by is <see cref="Scale"/>, not this: a near-constant range is floored.</remarks>
    public IReadOnlyList<double> DataRange { get; }

    /// <summary>What <see cref="Transform"/> multiplies by — <c>scale_</c>.</summary>
    public IReadOnlyList<double> Scale => _scale;

    /// <summary>What <see cref="Transform"/> then adds — <c>min_</c>.</summary>
    public IReadOnlyList<double> Minimum => _minimum;

    /// <summary>Fits a scaler on a row-major sample matrix.</summary>
    /// <param name="samples">The samples, row-major: <paramref name="featureCount"/> values per row.</param>
    /// <param name="featureCount">How many values each row carries.</param>
    /// <param name="options">The range to map onto and whether to clip; <see langword="null"/> is <c>[0, 1]</c> without clipping.</param>
    /// <returns>A fitted scaler.</returns>
    /// <exception cref="ArgumentOutOfRangeException"><paramref name="featureCount"/> is not positive, or the range is empty or not finite.</exception>
    /// <exception cref="ArgumentException"><paramref name="samples"/> holds no row, a partial one, or a non-finite value.</exception>
    /// <remarks>
    /// <strong>A near-constant feature scales by 1</strong>, and the test is not <c>range == 0</c> but
    /// <c>range &lt; 10·eps</c>, which is what the reference's <c>_handle_zeros_in_scale</c> applies when
    /// no constant mask is given. Such a feature lands on the bottom of the range rather than anywhere in it.
    /// </remarks>
    public static MinMaxScaler Fit(
        ReadOnlySpan<double> samples, int featureCount, MinMaxScalerOptions? options = null)
    {
        Guard.NotLessThan(featureCount, 1);
        MinMaxScalerOptions settings = options ?? new MinMaxScalerOptions();
        if (double.IsNaN(settings.Low) || double.IsInfinity(settings.Low)
            || double.IsNaN(settings.High) || double.IsInfinity(settings.High)
            || settings.Low >= settings.High)
        {
            throw new ArgumentOutOfRangeException(
                nameof(options),
                $"({settings.Low}, {settings.High})",
                "A feature range is two finite bounds with the low one below the high one.");
        }

        int sampleCount = SampleMatrix.Rows(samples, featureCount);
        SampleMatrix.RequireFinite(samples, nameof(samples));

        var dataMinimum = new double[featureCount];
        var dataMaximum = new double[featureCount];
        dataMinimum.AsSpan().Fill(double.PositiveInfinity);
        dataMaximum.AsSpan().Fill(double.NegativeInfinity);

        for (int i = 0; i < samples.Length; i++)
        {
            int feature = i % featureCount;
            dataMinimum[feature] = Math.Min(dataMinimum[feature], samples[i]);
            dataMaximum[feature] = Math.Max(dataMaximum[feature], samples[i]);
        }

        return Build(featureCount, sampleCount, dataMinimum, dataMaximum, settings);
    }

    /// <summary>The scale and offset a range implies — shared so <see cref="Fit"/> and <see cref="PartialFit"/> cannot drift.</summary>
    private static MinMaxScaler Build(
        int featureCount,
        int sampleCount,
        double[] dataMinimum,
        double[] dataMaximum,
        MinMaxScalerOptions settings)
    {
        var divisor = new double[featureCount];
        for (int feature = 0; feature < featureCount; feature++)
        {
            divisor[feature] = dataMaximum[feature] - dataMinimum[feature];
        }

        ScaleFloor.Apply(divisor);

        var scale = new double[featureCount];
        var minimum = new double[featureCount];
        double width = settings.High - settings.Low;
        for (int feature = 0; feature < featureCount; feature++)
        {
            scale[feature] = width / divisor[feature];
            minimum[feature] = settings.Low - (dataMinimum[feature] * scale[feature]);
        }

        return new MinMaxScaler(
            featureCount, sampleCount, dataMinimum, dataMaximum, scale, minimum, settings);
    }

    /// <summary>Folds another batch into the fitted range, as <c>partial_fit</c> does.</summary>
    /// <param name="samples">The next batch, row-major, with <see cref="FeatureCount"/> values per row.</param>
    /// <returns>A new scaler covering every batch seen so far; this one is unchanged.</returns>
    /// <exception cref="ArgumentException"><paramref name="samples"/> holds no row, a partial one, or a non-finite value.</exception>
    /// <remarks>
    /// Returns a new scaler rather than mutating this one — see
    /// <see cref="StandardScaler.PartialFit"/> for why. A batch inside the range already seen leaves
    /// every statistic where it was; one outside it widens the range and moves the scale.
    /// </remarks>
    public MinMaxScaler PartialFit(ReadOnlySpan<double> samples)
    {
        int rows = SampleMatrix.Rows(samples, FeatureCount);
        SampleMatrix.RequireFinite(samples, nameof(samples));

        var dataMinimum = new double[FeatureCount];
        var dataMaximum = new double[FeatureCount];
        for (int feature = 0; feature < FeatureCount; feature++)
        {
            dataMinimum[feature] = DataMinimum[feature];
            dataMaximum[feature] = DataMaximum[feature];
        }

        for (int i = 0; i < samples.Length; i++)
        {
            int feature = i % FeatureCount;
            dataMinimum[feature] = Math.Min(dataMinimum[feature], samples[i]);
            dataMaximum[feature] = Math.Max(dataMaximum[feature], samples[i]);
        }

        return Build(FeatureCount, SampleCount + rows, dataMinimum, dataMaximum, _options);
    }

    /// <summary>Maps a row-major sample matrix onto the fitted range.</summary>
    /// <param name="samples">The samples to transform, row-major, with <see cref="FeatureCount"/> values per row.</param>
    /// <returns>A new array of the same length.</returns>
    /// <exception cref="ArgumentException"><paramref name="samples"/> holds no row, a partial one, or a non-finite value.</exception>
    /// <remarks>
    /// A value outside the fitted minimum and maximum lands outside the range, unless
    /// <see cref="MinMaxScalerOptions.Clip"/> is set — which is the reference's behaviour and the reason
    /// the option exists.
    /// </remarks>
    public double[] Transform(ReadOnlySpan<double> samples)
    {
        SampleMatrix.Rows(samples, FeatureCount);
        SampleMatrix.RequireFinite(samples, nameof(samples));

        var result = new double[samples.Length];
        for (int i = 0; i < samples.Length; i++)
        {
            int feature = i % FeatureCount;
            double value = (samples[i] * _scale[feature]) + _minimum[feature];
            result[i] = _options.Clip ? Bounds.Clamp(value, _options.Low, _options.High) : value;
        }

        return result;
    }

    /// <summary>Undoes <see cref="Transform"/>, returning values on the original scale.</summary>
    /// <param name="samples">The transformed samples, row-major, with <see cref="FeatureCount"/> values per row.</param>
    /// <returns>A new array of the same length, back on the input scale.</returns>
    /// <exception cref="ArgumentException"><paramref name="samples"/> holds no row, a partial one, or a non-finite value.</exception>
    /// <remarks>
    /// Never clips, as the reference does not: a value clipped on the way in is not recoverable, and
    /// clipping again on the way out would hide that rather than undo it.
    /// </remarks>
    public double[] InverseTransform(ReadOnlySpan<double> samples)
    {
        SampleMatrix.Rows(samples, FeatureCount);
        SampleMatrix.RequireFinite(samples, nameof(samples));

        var result = new double[samples.Length];
        for (int i = 0; i < samples.Length; i++)
        {
            int feature = i % FeatureCount;
            result[i] = (samples[i] - _minimum[feature]) / _scale[feature];
        }

        return result;
    }
}
