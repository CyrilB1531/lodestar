using System.Globalization;
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
    private readonly StandardScaler? _scaler;
    private readonly PowerMethod _method;

    private PowerTransformer(
        int featureCount, int sampleCount, double[] lambdas, StandardScaler? scaler, PowerMethod method)
    {
        FeatureCount = featureCount;
        SampleCount = sampleCount;
        _lambdas = lambdas;
        Lambdas = Array.AsReadOnly(_lambdas);
        _scaler = scaler;
        _method = method;
    }

    /// <summary>How many values each row carries.</summary>
    public int FeatureCount { get; }

    /// <summary>How many rows the transformer was fitted on.</summary>
    public int SampleCount { get; }

    /// <summary>Each feature's fitted exponent — the reference's <c>lambdas_</c>.</summary>
    public IReadOnlyList<double> Lambdas { get; }

    /// <summary>Fits one exponent per feature by maximum likelihood.</summary>
    /// <param name="samples">The matrix, row-major: <paramref name="featureCount"/> values per row.</param>
    /// <param name="featureCount">How many values each row carries.</param>
    /// <param name="options">Family and standardisation; <see langword="null"/> takes the reference's defaults.</param>
    /// <returns>A fitted transformer.</returns>
    /// <exception cref="ArgumentOutOfRangeException"><paramref name="featureCount"/> is not positive.</exception>
    /// <exception cref="ArgumentException"><paramref name="samples"/> holds no row, a partial one, or a non-finite value; under <see cref="PowerMethod.BoxCox"/>, a value that is not strictly positive, a constant feature, or no bracket for the search; under <see cref="PowerMethod.YeoJohnson"/>, a mixed-sign column whose search bounds invert, past about 1e145; or, standardised, a value the fitted power overflows.</exception>
    /// <remarks>
    /// A constant Yeo-Johnson feature keeps exponent 1, the identity; Box-Cox refuses one, as scipy does (#1253).
    /// Box-Cox searches unbounded from <c>(-2, 2)</c>, pulled back only where the transform would overflow (#1227).
    /// </remarks>
    public static PowerTransformer Fit(
        ReadOnlySpan<double> samples, int featureCount, PowerTransformerOptions? options = null)
    {
        Guard.NotLessThan(featureCount, 1);
        PowerTransformerOptions settings = options ?? new PowerTransformerOptions();
        int sampleCount = SampleMatrix.Rows(samples, featureCount);
        SampleMatrix.RequireFinite(samples, nameof(samples));

        if (settings.Method == PowerMethod.BoxCox)
        {
            RequirePositive(samples, sampleCount, featureCount);
        }

        // Only Yeo-Johnson's constant test reads np.mean and np.var; Box-Cox refuses a constant column itself.
        double[]? means = null;
        double[]? variances = null;
        if (settings.Method == PowerMethod.YeoJohnson)
        {
            means = new double[featureCount];
            variances = new double[featureCount];
            NumpyReduction.ColumnMeansAndVariances(samples, sampleCount, featureCount, means, variances);
        }

        var lambdas = new double[featureCount];
        var untransformed = new bool[featureCount];
        var column = new double[sampleCount];
        for (int feature = 0; feature < featureCount; feature++)
        {
            for (int row = 0; row < sampleCount; row++)
            {
                column[row] = samples[(row * featureCount) + feature];
            }

            untransformed[feature] = means is not null && variances is not null
                && ConstantFeature.IsConstant(sampleCount, means[feature], variances[feature]);
            lambdas[feature] = settings.Method == PowerMethod.BoxCox
                ? BoxCoxExponent(column, nameof(samples))
                : YeoJohnsonExponent(column, untransformed[feature], nameof(samples));
        }

        var fitted = new PowerTransformer(featureCount, sampleCount, lambdas, null, settings.Method);
        if (!settings.Standardize)
        {
            return fitted;
        }

        // The reference standardises through a StandardScaler, whose constant test and floor are
        // its own (#1230), fitted on a constant Yeo-Johnson feature as it was, not as λ = 1 maps it.
        double[] powered = fitted.Raise(samples, sampleCount);
        RequireFinitePower(powered, nameof(samples));
        for (int i = 0; i < powered.Length; i++)
        {
            if (untransformed[i % featureCount])
            {
                powered[i] = samples[i];
            }
        }

        StandardScaler scaler = StandardScaler.FitChecked(powered, featureCount, sampleCount, new StandardScalerOptions());
        return new PowerTransformer(featureCount, sampleCount, lambdas, scaler, settings.Method);
    }

    /// <summary>Raises each value to its feature's exponent, and standardises if asked to.</summary>
    /// <param name="samples">The matrix to transform, row-major.</param>
    /// <returns>A new matrix of the same shape.</returns>
    /// <exception cref="ArgumentException"><paramref name="samples"/> holds a partial row, a non-finite value, or a non-positive one under Box-Cox; or, standardised, a value the fitted power overflows.</exception>
    public double[] Transform(ReadOnlySpan<double> samples)
    {
        int sampleCount = SampleMatrix.Rows(samples, FeatureCount);
        SampleMatrix.RequireFinite(samples, nameof(samples));

        double[] powered = Raise(samples, sampleCount);
        if (_scaler is null)
        {
            return powered;
        }

        RequireFinitePower(powered, nameof(samples));
        _scaler.TransformInPlace(powered);
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

        // One array: the standardisation undone, then the power, written over it.
        double[] original = samples.ToArray();
        _scaler?.InverseTransformInPlace(original);
        for (int row = 0; row < sampleCount; row++)
        {
            int start = row * FeatureCount;
            for (int feature = 0; feature < FeatureCount; feature++)
            {
                original[start + feature] = Lower(original[start + feature], _lambdas[feature]);
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
                        $"Box-Cox needs strictly positive values; feature {feature} holds {Convert.ToString(value, CultureInfo.InvariantCulture)}.",
                        nameof(samples));
                }

                powered[start + feature] = _method == PowerMethod.BoxCox
                    ? PowerLikelihood.BoxCox(value, _lambdas[feature])
                    : PowerLikelihood.YeoJohnson(value, _lambdas[feature]);
            }
        }

        return powered;
    }

    /// <summary>
    /// The refusal the reference's <c>StandardScaler</c> raises on a power that overflowed, in its words: the
    /// samples were finite, what the exponent made of them is not. That scaler lets a NaN through and refuses only
    /// an infinity, and so does this; unstandardised, the infinity is returned.
    /// </summary>
    private static void RequireFinitePower(double[] powered, string paramName)
    {
        SampleMatrix.RejectInfinityAsChecked(powered, paramName, "X");
    }

    /// <summary>
    /// The reference's <c>check_positive</c>, over every value before any exponent is fitted, so it wins over a constant
    /// feature or a search refused after it; feature by feature, in the previous release's words and order.
    /// </summary>
    private static void RequirePositive(ReadOnlySpan<double> samples, int sampleCount, int featureCount)
    {
        for (int feature = 0; feature < featureCount; feature++)
        {
            for (int row = 0; row < sampleCount; row++)
            {
                double value = samples[(row * featureCount) + feature];
                if (value <= 0.0)
                {
                    // CA2208, S3928: the previous release named its own column parameter, and a refusal it made keeps it.
#pragma warning disable CA2208, S3928
                    throw new ArgumentException(
                        $"Box-Cox needs strictly positive values; feature {feature} holds {Convert.ToString(value, CultureInfo.InvariantCulture)}.", "column");
#pragma warning restore CA2208, S3928
                }
            }
        }
    }

    /// <summary>Yeo-Johnson's exponent: 1 on a constant feature, the likelihood's optimum otherwise.</summary>
    private static double YeoJohnsonExponent(double[] column, bool constant, string paramName)
    {
        if (constant)
        {
            return 1.0;
        }

        (double low, double high) = PowerLikelihood.YeoJohnsonBounds(column);
        return BrentMinimum.Locate(new YeoJohnsonObjective(column).Negative, low, high, paramName);
    }

    /// <summary>Box-Cox's exponent as <c>scipy.stats.boxcox</c> fits it.</summary>
    private static double BoxCoxExponent(double[] column, string paramName)
    {
        // S1244: scipy refuses np.all(x == x[0]), an exactly constant column and only that.
#pragma warning disable S1244
        if (Array.TrueForAll(column, value => value == column[0]))
#pragma warning restore S1244
        {
            throw new ArgumentException("Data must not be constant.", paramName);
        }

        double lambda = BrentUnbounded.Locate(new BoxCoxObjective(column).Negative, -2.0, 2.0, paramName);
        return OverflowGuarded(column, lambda, double.MaxValue / 10000.0);
    }

    /// <summary>
    /// <c>boxcox_normmax</c>'s last step: an exponent under which the transform's most extreme value would pass
    /// <paramref name="ceiling"/> — <c>double.MaxValue / 10000</c>, scipy's <c>ymax</c> — is pulled back to the
    /// one that reaches it exactly, through Lambert's W. Internal so a test can lower the ceiling, as
    /// <c>boxcox_normmax(x, ymax=…)</c> can, to reach a branch no finite data of a sane size does.
    /// </summary>
    internal static double OverflowGuarded(double[] column, double lambda, double ceiling)
    {
        double smallest = column[0];
        double largest = column[0];
        for (int i = 1; i < column.Length; i++)
        {
            smallest = Math.Min(smallest, column[i]);
            largest = Math.Max(largest, column[i]);
        }

        double extreme;
        if (smallest >= 1.0)
        {
            extreme = largest;
        }
        else if (largest <= 1.0)
        {
            extreme = smallest;
        }
        else
        {
            extreme = PowerLikelihood.BoxCox(largest, lambda) > Math.Abs(PowerLikelihood.BoxCox(smallest, lambda))
                ? largest
                : smallest;
        }

        if (!(Math.Abs(PowerLikelihood.BoxCox(extreme, lambda)) > ceiling))
        {
            return lambda;
        }

        // _boxcox_inv_lmbda(x, y): the λ at which boxcox(x, λ) = y, y signed as x - 1 is.
        double y = ceiling * Math.Sign(extreme - 1.0);
        double logX = Math.Log(extreme);
        double w = LambertW.LowerBranch(-Math.Pow(extreme, -1.0 / y) * logX / y);
        return (-w / logX) - (1.0 / y);
    }

    /// <summary>The inverse of the power alone, before any standardisation is undone: scipy's <c>inv_boxcox</c>, or
    /// scikit-learn's <c>_yeo_johnson_inverse_transform</c>, each tested as its reference tests the exponent.</summary>
    private double Lower(double value, double lambda)
    {
        if (_method == PowerMethod.BoxCox)
        {
            return PowerLikelihood.InverseBoxCox(value, lambda);
        }

        return PowerLikelihood.InverseYeoJohnson(value, lambda);
    }
}
