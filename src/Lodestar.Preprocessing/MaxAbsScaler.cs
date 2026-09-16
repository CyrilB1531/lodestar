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

    /// <summary>What <see cref="Transform"/> divides by — <c>scale_</c>, which is <see cref="MaximumAbsolute"/> with a near-constant feature floored to 1.</summary>
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
        for (int i = 0; i < samples.Length; i++)
        {
            int feature = i % featureCount;
            maximumAbsolute[feature] = Math.Max(maximumAbsolute[feature], Math.Abs(samples[i]));
        }

        double[] scale = [.. maximumAbsolute];
        ScaleFloor.Apply(scale);

        return new MaxAbsScaler(
            featureCount, sampleCount, maximumAbsolute, scale, (options ?? new MaxAbsScalerOptions()).Clip);
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
        for (int i = 0; i < samples.Length; i++)
        {
            double value = samples[i] / _scale[i % FeatureCount];
            result[i] = _clips ? Bounds.Clamp(value, -1.0, 1.0) : value;
        }

        return result;
    }

    /// <summary>Undoes <see cref="Transform"/>, returning values on the original scale.</summary>
    /// <param name="samples">The transformed samples, row-major, with <see cref="FeatureCount"/> values per row.</param>
    /// <returns>A new array of the same length, back on the input scale.</returns>
    /// <exception cref="ArgumentException"><paramref name="samples"/> holds no row, a partial one, or a non-finite value.</exception>
    /// <remarks>Never clips, as the reference does not — see <see cref="MinMaxScaler.InverseTransform"/> for why.</remarks>
    public double[] InverseTransform(ReadOnlySpan<double> samples)
    {
        SampleMatrix.Rows(samples, FeatureCount);
        SampleMatrix.RequireFinite(samples, nameof(samples));

        var result = new double[samples.Length];
        for (int i = 0; i < samples.Length; i++)
        {
            result[i] = samples[i] * _scale[i % FeatureCount];
        }

        return result;
    }
}
