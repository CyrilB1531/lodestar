using Lodestar.Preprocessing.Internal;

namespace Lodestar.Preprocessing;

/// <summary>
/// Fills the missing values of each feature with a statistic of the ones that are present, at
/// <c>sklearn.impute.SimpleImputer</c> parity.
/// </summary>
/// <remarks>
/// <c>NaN</c> is what marks a value as missing, as it does in the reference — there is no separate
/// mask, and a matrix with no <c>NaN</c> comes back unchanged.
/// </remarks>
public sealed class SimpleImputer
{
    private readonly double[] _statistics;

    private SimpleImputer(int featureCount, int sampleCount, double[] statistics)
    {
        FeatureCount = featureCount;
        SampleCount = sampleCount;
        _statistics = statistics;
    }

    /// <summary>How many values each row carries.</summary>
    public int FeatureCount { get; }

    /// <summary>How many rows the imputer was fitted on.</summary>
    public int SampleCount { get; }

    /// <summary>What each feature's missing values are filled with — the reference's <c>statistics_</c>.</summary>
    public IReadOnlyList<double> Statistics => _statistics;

    /// <summary>Fits an imputer on a row-major sample matrix.</summary>
    /// <param name="samples">The samples, row-major: <paramref name="featureCount"/> values per row, <c>NaN</c> where a value is missing.</param>
    /// <param name="featureCount">How many values each row carries.</param>
    /// <param name="options">Which statistic to fill with; <see langword="null"/> is the mean.</param>
    /// <returns>A fitted imputer.</returns>
    /// <exception cref="ArgumentOutOfRangeException"><paramref name="featureCount"/> is not positive, or the fill value is not finite.</exception>
    /// <exception cref="ArgumentException">
    /// <paramref name="samples"/> holds no row, a partial one, or an infinity; or a feature has no
    /// value at all and <see cref="SimpleImputerOptions.KeepEmptyFeatures"/> is not set.
    /// </exception>
    /// <remarks>
    /// <strong>A feature with nothing in it is refused</strong>, where the reference drops it from
    /// the output and returns a narrower matrix than it was given. Set
    /// <see cref="SimpleImputerOptions.KeepEmptyFeatures"/> to fill it with zero instead, which is
    /// the reference's <c>keep_empty_features=True</c>.
    /// </remarks>
    public static SimpleImputer Fit(
        ReadOnlySpan<double> samples, int featureCount, SimpleImputerOptions? options = null)
    {
        Guard.NotLessThan(featureCount, 1);
        SimpleImputerOptions settings = options ?? new SimpleImputerOptions();
        if (double.IsNaN(settings.FillValue) || double.IsInfinity(settings.FillValue))
        {
            throw new ArgumentOutOfRangeException(
                nameof(options), settings.FillValue, "A fill value is finite; NaN is what marks a value missing.");
        }

        int sampleCount = SampleMatrix.Rows(samples, featureCount);
        RequireNoInfinity(samples);

        var statistics = new double[featureCount];
        for (int feature = 0; feature < featureCount; feature++)
        {
            double[] present = Present(samples, featureCount, feature, sampleCount);
            if (present.Length == 0)
            {
                if (!settings.KeepEmptyFeatures)
                {
                    throw new ArgumentException(
                        $"feature {feature} has no value at all, so there is no statistic to fill it from. "
                        + "The reference drops such a feature and returns a narrower matrix; set "
                        + "KeepEmptyFeatures to fill it with zero instead, which is what its "
                        + "keep_empty_features=True does.",
                        nameof(samples));
                }

                statistics[feature] = 0.0;
                continue;
            }

            statistics[feature] = Statistic(present, settings);
        }

        return new SimpleImputer(featureCount, sampleCount, statistics);
    }

    /// <summary>Fills the missing values of a row-major sample matrix.</summary>
    /// <param name="samples">The samples to fill, row-major, with <see cref="FeatureCount"/> values per row.</param>
    /// <returns>A new array of the same length, with every <c>NaN</c> replaced.</returns>
    /// <exception cref="ArgumentException"><paramref name="samples"/> holds no row, a partial one, or an infinity.</exception>
    /// <remarks>
    /// The width never changes, which is the divergence this package chose: the reference's default
    /// returns one column fewer when a feature was empty at fit time.
    /// </remarks>
    public double[] Transform(ReadOnlySpan<double> samples)
    {
        SampleMatrix.Rows(samples, FeatureCount);
        RequireNoInfinity(samples);

        var result = new double[samples.Length];
        for (int i = 0; i < samples.Length; i++)
        {
            result[i] = double.IsNaN(samples[i]) ? _statistics[i % FeatureCount] : samples[i];
        }

        return result;
    }

    /// <summary>The statistic the strategy names, over the values that are present.</summary>
    private static double Statistic(double[] present, SimpleImputerOptions settings)
    {
        switch (settings.Strategy)
        {
            case ImputationStrategy.Constant:
                return settings.FillValue;
            case ImputationStrategy.Median:
                Array.Sort(present);
                return Percentile.Median(present);
            case ImputationStrategy.MostFrequent:
                Array.Sort(present);
                return MostFrequent(present);
            default:
                return Mean(present);
        }
    }

    private static double Mean(double[] present)
    {
        double total = 0.0;
        for (int i = 0; i < present.Length; i++)
        {
            total += present[i];
        }

        return total / present.Length;
    }

    /// <summary>The most frequent value of a sorted column; a tie goes to the smaller, as the reference's does.</summary>
    /// <remarks>
    /// Measured on <c>[1, 1, 2, 2]</c>, where both appear twice: the reference fills with 1. Reading a
    /// sorted column left to right and keeping only a <em>strictly</em> longer run is what reproduces
    /// that — an implementation using <c>&gt;=</c> fills with 2 and passes every untied case.
    /// </remarks>
    private static double MostFrequent(double[] sorted)
    {
        double best = sorted[0];
        int bestRun = 0;
        int run = 0;
        for (int i = 0; i < sorted.Length; i++)
        {
            run = i > 0 && sorted[i].CompareTo(sorted[i - 1]) == 0 ? run + 1 : 1;
            if (run > bestRun)
            {
                bestRun = run;
                best = sorted[i];
            }
        }

        return best;
    }

    /// <summary>One feature's values that are not missing.</summary>
    private static double[] Present(
        ReadOnlySpan<double> samples, int featureCount, int feature, int sampleCount)
    {
        var present = new List<double>(sampleCount);
        for (int row = 0; row < sampleCount; row++)
        {
            double value = samples[(row * featureCount) + feature];
            if (!double.IsNaN(value))
            {
                present.Add(value);
            }
        }

        return [.. present];
    }

    /// <summary>An infinity is not a missing value and has no place in a mean.</summary>
    private static void RequireNoInfinity(ReadOnlySpan<double> samples)
    {
        for (int i = 0; i < samples.Length; i++)
        {
            if (double.IsInfinity(samples[i]))
            {
                throw new ArgumentException(
                    $"samples[{i}] is {samples[i]}. NaN marks a missing value here; an infinity marks nothing "
                    + "and would carry into every statistic.",
                    nameof(samples));
            }
        }
    }
}
