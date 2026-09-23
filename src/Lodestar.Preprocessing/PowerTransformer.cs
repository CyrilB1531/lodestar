using Lodestar.Preprocessing.Internal;

namespace Lodestar.Preprocessing;

/// <summary>Raises each feature to the power that makes it most nearly normal, at <c>sklearn.preprocessing.PowerTransformer</c> parity.</summary>
/// <remarks>
/// Where <see cref="QuantileTransformer"/> discards everything but the order of a feature's
/// values, this keeps the values and looks for a single exponent — so it is reversible and
/// monotone, and a value outside the fitted range is transformed rather than clamped.
/// <strong>Compared at <c>1e-5</c>, not decision 0005's <c>1e-9</c></strong>: the log-likelihood's
/// curvature pins the exponent only to about <c>6e-7</c>, moving the transformed values by
/// <c>9.1e-7</c> relative. <c>docs/equivalence.md</c> carries the arithmetic.
/// </remarks>
public sealed class PowerTransformer
{
    private readonly double[] _lambdas;
    private readonly double[]? _means;
    private readonly double[]? _deviations;
    private readonly PowerMethod _method;

    private PowerTransformer(
        int featureCount, int sampleCount, double[] lambdas, double[]? means, double[]? deviations,
        PowerMethod method)
    {
        FeatureCount = featureCount;
        SampleCount = sampleCount;
        _lambdas = lambdas;
        _means = means;
        _deviations = deviations;
        _method = method;
    }

    /// <summary>How many values each row carries.</summary>
    public int FeatureCount { get; }

    /// <summary>How many rows the transformer was fitted on.</summary>
    public int SampleCount { get; }

    /// <summary>Each feature's fitted exponent — the reference's <c>lambdas_</c>.</summary>
    public IReadOnlyList<double> Lambdas => _lambdas;

    /// <summary>Fits one exponent per feature by maximum likelihood.</summary>
    /// <param name="samples">The matrix, row-major: <paramref name="featureCount"/> values per row.</param>
    /// <param name="featureCount">How many values each row carries.</param>
    /// <param name="options">Family and standardisation; <see langword="null"/> takes the reference's defaults.</param>
    /// <returns>A fitted transformer.</returns>
    /// <exception cref="ArgumentOutOfRangeException"><paramref name="featureCount"/> is not positive.</exception>
    /// <exception cref="ArgumentException">
    /// <paramref name="samples"/> holds no row, a partial one, or a non-finite value; or the
    /// family is <see cref="PowerMethod.BoxCox"/> and a value is not strictly positive.
    /// </exception>
    public static PowerTransformer Fit(
        ReadOnlySpan<double> samples, int featureCount, PowerTransformerOptions? options = null)
    {
        Guard.NotLessThan(featureCount, 1);
        PowerTransformerOptions settings = options ?? new PowerTransformerOptions();
        int sampleCount = SampleMatrix.Rows(samples, featureCount);
        SampleMatrix.RequireFinite(samples, nameof(samples));

        var lambdas = new double[featureCount];
        var column = new double[sampleCount];
        for (int feature = 0; feature < featureCount; feature++)
        {
            for (int row = 0; row < sampleCount; row++)
            {
                column[row] = samples[(row * featureCount) + feature];
            }

            lambdas[feature] = Exponent(column, settings.Method, feature);
        }

        var fitted = new PowerTransformer(featureCount, sampleCount, lambdas, null, null, settings.Method);
        if (!settings.Standardize)
        {
            return fitted;
        }

        double[] powered = fitted.Raise(samples, sampleCount);
        var means = new double[featureCount];
        var deviations = new double[featureCount];
        for (int feature = 0; feature < featureCount; feature++)
        {
            for (int row = 0; row < sampleCount; row++)
            {
                column[row] = powered[(row * featureCount) + feature];
            }

            double sum = 0.0;
            for (int row = 0; row < sampleCount; row++)
            {
                sum += column[row];
            }

            means[feature] = sum / sampleCount;
            deviations[feature] = Math.Sqrt(PowerLikelihood.PopulationVariance(column));
        }

        // A feature the power left constant has no spread to divide by; the floor the scalers
        // share turns that zero into a one, so the standardised column is zeros rather than NaNs.
        ScaleFloor.Apply(deviations);

        return new PowerTransformer(
            featureCount, sampleCount, lambdas, means, deviations, settings.Method);
    }

    /// <summary>Raises each value to its feature's exponent, and standardises if asked to.</summary>
    /// <param name="samples">The matrix to transform, row-major.</param>
    /// <returns>A new matrix of the same shape.</returns>
    /// <exception cref="ArgumentException"><paramref name="samples"/> holds a partial row, a non-finite value, or a non-positive one under Box-Cox.</exception>
    public double[] Transform(ReadOnlySpan<double> samples)
    {
        int sampleCount = SampleMatrix.Rows(samples, FeatureCount);
        SampleMatrix.RequireFinite(samples, nameof(samples));

        double[] powered = Raise(samples, sampleCount);
        if (_means is null || _deviations is null)
        {
            return powered;
        }

        for (int row = 0; row < sampleCount; row++)
        {
            int start = row * FeatureCount;
            for (int feature = 0; feature < FeatureCount; feature++)
            {
                powered[start + feature] =
                    (powered[start + feature] - _means[feature]) / _deviations[feature];
            }
        }

        return powered;
    }

    /// <summary>Undoes the standardisation and the power, in that order.</summary>
    /// <param name="samples">A matrix this transformer produced.</param>
    /// <returns>A new matrix of the same shape.</returns>
    /// <exception cref="ArgumentException"><paramref name="samples"/> holds a partial row or a non-finite value.</exception>
    public double[] InverseTransform(ReadOnlySpan<double> samples)
    {
        int sampleCount = SampleMatrix.Rows(samples, FeatureCount);
        SampleMatrix.RequireFinite(samples, nameof(samples));

        var original = new double[samples.Length];
        for (int row = 0; row < sampleCount; row++)
        {
            int start = row * FeatureCount;
            for (int feature = 0; feature < FeatureCount; feature++)
            {
                double value = samples[start + feature];
                if (_means is not null && _deviations is not null)
                {
                    value = (value * _deviations[feature]) + _means[feature];
                }

                original[start + feature] = Lower(value, _lambdas[feature]);
            }
        }

        return original;
    }

    private double[] Raise(ReadOnlySpan<double> samples, int sampleCount)
    {
        var powered = new double[samples.Length];
        for (int row = 0; row < sampleCount; row++)
        {
            int start = row * FeatureCount;
            for (int feature = 0; feature < FeatureCount; feature++)
            {
                double value = samples[start + feature];
                if (_method == PowerMethod.BoxCox && value <= 0.0)
                {
                    throw new ArgumentException(
                        $"Box-Cox needs strictly positive values; feature {feature} holds {value}.",
                        nameof(samples));
                }

                powered[start + feature] = _method == PowerMethod.BoxCox
                    ? PowerLikelihood.BoxCox(value, _lambdas[feature])
                    : PowerLikelihood.YeoJohnson(value, _lambdas[feature]);
            }
        }

        return powered;
    }

    private static double Exponent(double[] column, PowerMethod method, int feature)
    {
        if (method == PowerMethod.BoxCox)
        {
            for (int i = 0; i < column.Length; i++)
            {
                if (column[i] <= 0.0)
                {
                    throw new ArgumentException(
                        $"Box-Cox needs strictly positive values; feature {feature} holds {column[i]}.",
                        nameof(column));
                }
            }

            // The reference searches a fixed interval for Box-Cox, where Yeo-Johnson's is read
            // off the data; both are wide enough that the optimum is interior in practice.
            return BrentMinimum.Locate(l => PowerLikelihood.NegativeBoxCox(column, l), -2.0, 2.0);
        }

        (double low, double high) = PowerLikelihood.YeoJohnsonBounds(column);
        return BrentMinimum.Locate(l => PowerLikelihood.NegativeYeoJohnson(column, l), low, high);
    }

    /// <summary>The inverse of the power alone, before any standardisation is undone.</summary>
    private double Lower(double value, double lambda)
    {
        if (_method == PowerMethod.BoxCox)
        {
            // S1244: the exponent at which the forward map is a logarithm.
#pragma warning disable S1244
            return lambda == 0.0 ? Math.Exp(value) : Math.Pow((value * lambda) + 1.0, 1.0 / lambda);
#pragma warning restore S1244
        }

        if (value >= 0.0)
        {
#pragma warning disable S1244
            return lambda == 0.0
#pragma warning restore S1244
                ? Math.Exp(value) - 1.0
                : Math.Pow((value * lambda) + 1.0, 1.0 / lambda) - 1.0;
        }

#pragma warning disable S1244
        return lambda == 2.0
#pragma warning restore S1244
            ? 1.0 - Math.Exp(-value)
            : 1.0 - Math.Pow(((value * (lambda - 2.0)) + 1.0), 1.0 / (2.0 - lambda));
    }
}
