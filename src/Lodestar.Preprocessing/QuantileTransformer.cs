using Lodestar.Preprocessing.Internal;
using Lodestar.Stats;

namespace Lodestar.Preprocessing;

/// <summary>
/// Maps each feature onto a uniform or normal distribution by its own ranks, at
/// <c>sklearn.preprocessing.QuantileTransformer</c> parity.
/// </summary>
/// <remarks>
/// The scalers move and stretch a feature; this one reshapes it. What survives is the order of
/// the values and nothing else, so an outlier stops being far away and a skew stops being a skew
/// — which is the point, and also the cost: two values that were a thousand apart can come out
/// adjacent, and no inverse recovers what the ranks discarded.
/// </remarks>
public sealed class QuantileTransformer
{
    /// <summary>How close to an endpoint counts as being on it, on the normal output. The reference's own threshold.</summary>
    private const double BoundsThreshold = 1e-7;

    private readonly double[] _references;
    private readonly double[][] _quantiles;
    private readonly double[] _negatedReferences;
    private readonly double[][] _negatedQuantiles;
    private readonly QuantileOutput _output;
    private readonly double _clipLow;
    private readonly double _clipHigh;

    private QuantileTransformer(
        int featureCount, int sampleCount, double[] references, double[][] quantiles, QuantileOutput output)
    {
        FeatureCount = featureCount;
        SampleCount = sampleCount;
        _references = references;
        _quantiles = quantiles;
        _output = output;

        // Built once rather than per value: the per-value form allocated 1.25 GB and took 104 ms
        // where this takes 10.8 ms, at twenty thousand rows over four features (#1122, measured).
        _negatedReferences = Negated(references);
        _negatedQuantiles = new double[quantiles.Length][];
        for (int feature = 0; feature < quantiles.Length; feature++)
        {
            _negatedQuantiles[feature] = Negated(quantiles[feature]);
        }

        // The reference clips the normal output so its own inverse stays finite, at the quantile
        // of the threshold one ulp in. Computed once rather than per value.
        double edge = BoundsThreshold - Math.Pow(2.0, -52.0);
        _clipLow = Distributions.NormalQuantile(edge);
        _clipHigh = Distributions.NormalQuantile(1.0 - edge);
    }

    /// <summary>How many values each row carries.</summary>
    public int FeatureCount { get; }

    /// <summary>How many rows the transformer was fitted on.</summary>
    public int SampleCount { get; }

    /// <summary>The quantile levels read, evenly spaced over <c>[0, 1]</c> — the reference's <c>references_</c>.</summary>
    public IReadOnlyList<double> References => _references;

    /// <summary>Each feature's values at those levels — the reference's <c>quantiles_</c>, one row per feature.</summary>
    public IReadOnlyList<IReadOnlyList<double>> Quantiles => _quantiles;

    /// <summary>Fits the quantiles of every feature.</summary>
    /// <param name="samples">The matrix, row-major: <paramref name="featureCount"/> values per row.</param>
    /// <param name="featureCount">How many values each row carries.</param>
    /// <param name="options">Quantile count and output distribution; <see langword="null"/> takes the reference's defaults.</param>
    /// <returns>A fitted transformer.</returns>
    /// <exception cref="ArgumentOutOfRangeException"><paramref name="featureCount"/> is not positive, or the quantile count is below one.</exception>
    /// <exception cref="ArgumentException"><paramref name="samples"/> holds no row, a partial one, or a non-finite value.</exception>
    public static QuantileTransformer Fit(
        ReadOnlySpan<double> samples, int featureCount, QuantileTransformerOptions? options = null)
    {
        Guard.NotLessThan(featureCount, 1);
        QuantileTransformerOptions settings = options ?? new QuantileTransformerOptions();
        Guard.NotLessThan(settings.QuantileCount, 1);

        int sampleCount = SampleMatrix.Rows(samples, featureCount);
        SampleMatrix.RequireFinite(samples, nameof(samples));

        int count = Math.Max(1, Math.Min(settings.QuantileCount, sampleCount));
        var references = new double[count];
        for (int i = 0; i < count; i++)
        {
            references[i] = count == 1 ? 0.0 : (double)i / (count - 1);
        }

        var quantiles = new double[featureCount][];
        for (int feature = 0; feature < featureCount; feature++)
        {
            double[] column = Percentile.SortedColumn(samples, featureCount, feature, sampleCount);
            quantiles[feature] = new double[count];
            for (int i = 0; i < count; i++)
            {
                quantiles[feature][i] = Percentile.Linear(column, references[i] * 100.0);
            }
        }

        return new QuantileTransformer(featureCount, sampleCount, references, quantiles, settings.Output);
    }

    /// <summary>Maps each value onto the fitted distribution.</summary>
    /// <param name="samples">The matrix to transform, row-major.</param>
    /// <returns>A new matrix of the same shape.</returns>
    /// <exception cref="ArgumentException"><paramref name="samples"/> holds a partial row or a non-finite value.</exception>
    public double[] Transform(ReadOnlySpan<double> samples) => Map(samples, inverse: false);

    /// <summary>Maps each value back onto the feature's own scale.</summary>
    /// <param name="samples">A matrix this transformer produced.</param>
    /// <returns>A new matrix of the same shape.</returns>
    /// <exception cref="ArgumentException"><paramref name="samples"/> holds a partial row or a non-finite value.</exception>
    /// <remarks>
    /// Exact only where the fitted quantiles are distinct: values that shared a quantile came out
    /// the same and cannot be told apart afterwards.
    /// </remarks>
    public double[] InverseTransform(ReadOnlySpan<double> samples) => Map(samples, inverse: true);

    private double[] Map(ReadOnlySpan<double> samples, bool inverse)
    {
        int sampleCount = SampleMatrix.Rows(samples, FeatureCount);
        SampleMatrix.RequireFinite(samples, nameof(samples));

        var mapped = new double[samples.Length];
        for (int row = 0; row < sampleCount; row++)
        {
            int start = row * FeatureCount;
            for (int feature = 0; feature < FeatureCount; feature++)
            {
                mapped[start + feature] = inverse
                    ? Backward(samples[start + feature], _quantiles[feature])
                    : Forward(samples[start + feature], feature);
            }
        }

        return mapped;
    }

    private double Forward(double value, int feature)
    {
        double[] quantiles = _quantiles[feature];
        double lowBound = quantiles[0];
        double highBound = quantiles[quantiles.Length - 1];

        // S1244: on the uniform output the reference tests the endpoints with ==, so a value
        // that merely rounds near one is interpolated rather than pinned.
#pragma warning disable S1244
        bool atLow = _output == QuantileOutput.Normal
            ? value - BoundsThreshold < lowBound
            : value == lowBound;
        bool atHigh = _output == QuantileOutput.Normal
            ? value + BoundsThreshold > highBound
            : value == highBound;
#pragma warning restore S1244

        double rank;
        if (atLow)
        {
            rank = 0.0;
        }
        else if (atHigh)
        {
            rank = 1.0;
        }
        else
        {
            // Interpolated both ways and averaged: with repeated values a quantile repeats too,
            // and a single direction would take one end of the run rather than its middle.
            rank = 0.5 * (Interpolate(value, quantiles, _references)
                - Interpolate(-value, _negatedQuantiles[feature], _negatedReferences));
        }

        if (_output != QuantileOutput.Normal)
        {
            return rank;
        }

        // The reference takes the quantile of an endpoint to an infinity and clips it back; the
        // endpoints are answered directly here because NormalQuantile refuses them, and the
        // clipped infinity is exactly the bound. S1244: only the two pinned literals.
#pragma warning disable S1244
        if (rank <= 0.0)
        {
            return _clipLow;
        }
        if (rank >= 1.0)
        {
            return _clipHigh;
        }
#pragma warning restore S1244

        return Math.Min(_clipHigh, Math.Max(_clipLow, Distributions.NormalQuantile(rank)));
    }

    private double Backward(double value, double[] quantiles)
    {
        double rank = _output == QuantileOutput.Normal ? NormalCdf(value) : value;

        // The same endpoint test the forward map uses, and for the same reason: on the normal
        // output a value clipped to the bound reads back as a rank a threshold away from zero
        // or one, not as zero or one. S1244: the uniform side tests the literals, as the
        // reference does.
#pragma warning disable S1244
        bool atLow = _output == QuantileOutput.Normal ? rank - BoundsThreshold < 0.0 : rank == 0.0;
        bool atHigh = _output == QuantileOutput.Normal ? rank + BoundsThreshold > 1.0 : rank == 1.0;
#pragma warning restore S1244
        if (atLow)
        {
            return quantiles[0];
        }
        if (atHigh)
        {
            return quantiles[quantiles.Length - 1];
        }

        return Interpolate(rank, _references, quantiles);
    }

    /// <summary>The standard normal's distribution function, through the chi-squared tail it is.</summary>
    /// <remarks>
    /// A chi-squared variable with one degree of freedom is a standard normal squared, so
    /// <c>P(X² &gt; x²)</c> is twice the normal tail beyond <c>|x|</c>. That identity reaches the
    /// function through <see cref="Distributions.ChiSquaredSf"/>, which <c>Lodestar.Stats</c>
    /// publishes, rather than through its own <c>Normal.Sf</c>, which it does not — and rather
    /// than writing a second error function here, the duplication #763 decided against.
    /// </remarks>
    private static double NormalCdf(double value)
    {
        double half = 0.5 * Distributions.ChiSquaredSf(value * value, 1.0);

        return value >= 0.0 ? 1.0 - half : half;
    }

    /// <summary>Linear interpolation of <paramref name="ys"/> at <paramref name="x"/> over ascending <paramref name="xs"/>, clamped.</summary>
    private static double Interpolate(double x, double[] xs, double[] ys)
    {
        if (x <= xs[0])
        {
            return ys[0];
        }
        if (x >= xs[xs.Length - 1])
        {
            return ys[ys.Length - 1];
        }

        int low = 0;
        int high = xs.Length - 1;
        while (high - low > 1)
        {
            int middle = low + ((high - low) / 2);
            if (xs[middle] <= x)
            {
                low = middle;
            }
            else
            {
                high = middle;
            }
        }

        double span = xs[high] - xs[low];

        // S1244: two identical knots leave no slope to read, and the reference's own
        // interpolation takes the left value there.
#pragma warning disable S1244
        return span == 0.0 ? ys[low] : ys[low] + ((x - xs[low]) / span * (ys[high] - ys[low]));
#pragma warning restore S1244
    }

    /// <summary>The sequence negated and reversed, which is what interpolating the other way reads.</summary>
    private static double[] Negated(double[] values)
    {
        var flipped = new double[values.Length];
        for (int i = 0; i < values.Length; i++)
        {
            flipped[i] = -values[values.Length - 1 - i];
        }

        return flipped;
    }
}
