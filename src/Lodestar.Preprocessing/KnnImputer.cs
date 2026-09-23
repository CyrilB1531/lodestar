using Lodestar.Preprocessing.Internal;

namespace Lodestar.Preprocessing;

/// <summary>Fills each missing value from the rows most like the one it is missing from, at <c>sklearn.impute.KNNImputer</c> parity.</summary>
/// <remarks>
/// Where <see cref="SimpleImputer"/> answers with a column's own mean or median — the same value
/// for every row — this asks which other rows resemble this one and answers with theirs, at a
/// distance between every pair, which is why <see cref="MaxDistancePairs"/> exists.
/// Distances are <c>nan_euclidean</c>: the squared differences over the coordinates both rows
/// have, scaled by the fraction that were missing, so a pair sharing two features of four is not
/// automatically nearer than a pair sharing all four.
/// </remarks>
public sealed class KnnImputer
{
    // long-comment: the bound below is a measured ceiling, not a round number, and a reviewer
    // should be able to see the measurement without leaving the source.
    // Every receiver row is compared with every fitted row over every feature, so the cost is
    // rows * fitted * features. Measured on a Ryzen 7 8700G (2026-09-23): 100 million pairs of
    // one feature take about 0.35 s, and the same count over ten features about 1.6 s. Past this
    // the call is refused rather than run for however long that takes, as MannWhitney refuses an
    // exact table past its own product.
    internal const long MaxDistancePairs = 100_000_000L;

    private readonly double[] _fitted;
    private readonly double[] _columnMeans;
    private readonly int[] _kept;
    private readonly KnnImputerOptions _settings;

    private KnnImputer(
        int featureCount, int sampleCount, double[] fitted, double[] columnMeans, int[] kept,
        KnnImputerOptions settings)
    {
        FeatureCount = featureCount;
        SampleCount = sampleCount;
        _fitted = fitted;
        _columnMeans = columnMeans;
        _kept = kept;
        _settings = settings;
    }

    /// <summary>How many values each row carries.</summary>
    public int FeatureCount { get; }

    /// <summary>How many rows the imputer was fitted on.</summary>
    public int SampleCount { get; }

    /// <summary>How many values a transformed row carries.</summary>
    /// <remarks>
    /// Fewer than <see cref="FeatureCount"/> when a feature was missing from every fitted row:
    /// there is nothing to impute it from and nothing to impute it with, so the reference drops
    /// it rather than filling it, and so does this. <see cref="KeptFeatures"/> says which.
    /// </remarks>
    public int OutputFeatureCount => _kept.Length;

    /// <summary>The indices of the features a transformed row carries, ascending.</summary>
    public IReadOnlyList<int> KeptFeatures => _kept;

    /// <summary>Keeps the fitted rows, which are the donors every later call draws from.</summary>
    /// <param name="samples">The matrix, row-major: <paramref name="featureCount"/> values per row. A <c>NaN</c> is a missing value.</param>
    /// <param name="featureCount">How many values each row carries.</param>
    /// <param name="options">Neighbour count and weighting; <see langword="null"/> takes the reference's defaults.</param>
    /// <returns>A fitted imputer.</returns>
    /// <exception cref="ArgumentOutOfRangeException"><paramref name="featureCount"/> is not positive, or the neighbour count is below one.</exception>
    /// <exception cref="ArgumentException"><paramref name="samples"/> holds no row, a partial one, or an infinity.</exception>
    public static KnnImputer Fit(
        ReadOnlySpan<double> samples, int featureCount, KnnImputerOptions? options = null)
    {
        Guard.NotLessThan(featureCount, 1);
        KnnImputerOptions settings = options ?? new KnnImputerOptions();
        Guard.NotLessThan(settings.NeighbourCount, 1);

        int sampleCount = SampleMatrix.Rows(samples, featureCount);
        RequireNoInfinity(samples);

        // The column mean over the values that are present, which is what the reference falls
        // back to for a feature no donor can supply.
        var means = new double[featureCount];
        for (int feature = 0; feature < featureCount; feature++)
        {
            double sum = 0.0;
            int present = 0;
            for (int row = 0; row < sampleCount; row++)
            {
                double value = samples[(row * featureCount) + feature];
                if (!double.IsNaN(value))
                {
                    sum += value;
                    present++;
                }
            }

            means[feature] = present == 0 ? double.NaN : sum / present;
        }

        var kept = new List<int>(featureCount);
        for (int feature = 0; feature < featureCount; feature++)
        {
            if (!double.IsNaN(means[feature]))
            {
                kept.Add(feature);
            }
        }

        return new KnnImputer(
            featureCount, sampleCount, samples.ToArray(), means, [.. kept], settings);
    }

    /// <summary>Fills every missing value from its nearest donors among the fitted rows.</summary>
    /// <param name="samples">The matrix to fill, row-major.</param>
    /// <returns>
    /// A new matrix, <see cref="OutputFeatureCount"/> values per row; the input is never written to.
    /// </returns>
    /// <exception cref="ArgumentException"><paramref name="samples"/> holds a partial row or an infinity.</exception>
    /// <exception cref="ArgumentOutOfRangeException">
    /// The number of distances needed exceeds <see cref="MaxDistancePairs"/>; the cost is the
    /// product of the two row counts and the feature count.
    /// </exception>
    public double[] Transform(ReadOnlySpan<double> samples)
    {
        int sampleCount = SampleMatrix.Rows(samples, FeatureCount);
        RequireNoInfinity(samples);

        long pairs = (long)sampleCount * SampleCount * FeatureCount;
        if (pairs > MaxDistancePairs)
        {
            throw new ArgumentOutOfRangeException(
                nameof(samples),
                pairs,
                $"Imputing {sampleCount} rows from {SampleCount} over {FeatureCount} features needs "
                + $"{pairs} distance terms, past the {MaxDistancePairs} this refuses at.");
        }

        var filled = new double[sampleCount * _kept.Length];
        var distances = new double[SampleCount];
        for (int row = 0; row < sampleCount; row++)
        {
            int start = row * FeatureCount;
            bool missing = AnyMissing(samples, start);
            if (missing)
            {
                for (int donor = 0; donor < SampleCount; donor++)
                {
                    distances[donor] = NanEuclidean(samples, start, donor);
                }
            }

            int target = row * _kept.Length;
            for (int column = 0; column < _kept.Length; column++)
            {
                int feature = _kept[column];
                double value = samples[start + feature];
                filled[target + column] = missing && double.IsNaN(value)
                    ? Impute(distances, feature)
                    : value;
            }
        }

        return filled;
    }

    private double Impute(double[] distances, int feature)
    {
        // Only a row that has the feature can donate it, and only a row at a finite distance
        // shares any coordinate with the receiver at all.
        var candidates = new List<(double Distance, double Value)>();
        for (int donor = 0; donor < SampleCount; donor++)
        {
            double value = _fitted[(donor * FeatureCount) + feature];
            if (!double.IsNaN(value) && !double.IsNaN(distances[donor]))
            {
                candidates.Add((distances[donor], value));
            }
        }

        if (candidates.Count == 0)
        {
            return _columnMeans[feature];
        }

        candidates.Sort((left, right) => left.Distance.CompareTo(right.Distance));
        int take = Math.Min(_settings.NeighbourCount, candidates.Count);

        double weighted = 0.0;
        double total = 0.0;
        for (int i = 0; i < take; i++)
        {
            double weight = Weight(candidates, take, i);
            weighted += weight * candidates[i].Value;
            total += weight;
        }

        // S1244: every weight being an exact zero is the degenerate case, and only that.
#pragma warning disable S1244
        return total == 0.0 ? _columnMeans[feature] : weighted / total;
#pragma warning restore S1244
    }

    /// <summary>One donor's weight: equal shares, or the reciprocal of its distance.</summary>
    /// <remarks>
    /// A donor at distance zero is identical to the receiver on everything they share, so the
    /// reference gives it the whole weight and none to the rest rather than dividing by zero.
    /// </remarks>
    private double Weight(List<(double Distance, double Value)> candidates, int take, int index)
    {
        if (_settings.Weights == NeighbourWeights.Uniform)
        {
            return 1.0;
        }

        // S1244: an exact zero distance, which is what the reference tests for.
#pragma warning disable S1244
        bool anyExact = false;
        for (int i = 0; i < take; i++)
        {
            anyExact |= candidates[i].Distance == 0.0;
        }

        if (anyExact)
        {
            return candidates[index].Distance == 0.0 ? 1.0 : 0.0;
        }
#pragma warning restore S1244

        return 1.0 / candidates[index].Distance;
    }

    /// <summary>The distance the reference calls <c>nan_euclidean</c>.</summary>
    /// <remarks>
    /// The squared differences over the coordinates both rows carry, scaled by the ratio of all
    /// features to those coordinates — so two rows sharing half their features are as far apart
    /// as the shared half suggests they would be over all of them. With nothing in common the
    /// distance is <c>NaN</c>, and such a donor is not one.
    /// </remarks>
    private double NanEuclidean(ReadOnlySpan<double> samples, int start, int donor)
    {
        double squares = 0.0;
        int present = 0;
        int donorStart = donor * FeatureCount;
        for (int feature = 0; feature < FeatureCount; feature++)
        {
            double left = samples[start + feature];
            double right = _fitted[donorStart + feature];
            if (double.IsNaN(left) || double.IsNaN(right))
            {
                continue;
            }

            double gap = left - right;
            squares += gap * gap;
            present++;
        }

        return present == 0 ? double.NaN : Math.Sqrt(squares * FeatureCount / present);
    }

    private bool AnyMissing(ReadOnlySpan<double> samples, int start)
    {
        for (int feature = 0; feature < FeatureCount; feature++)
        {
            if (double.IsNaN(samples[start + feature]))
            {
                return true;
            }
        }

        return false;
    }

    private static void RequireNoInfinity(ReadOnlySpan<double> samples)
    {
        for (int i = 0; i < samples.Length; i++)
        {
            if (double.IsInfinity(samples[i]))
            {
                throw new ArgumentException(
                    $"samples[{i}] is infinite. A NaN is a missing value here; an infinity is not.",
                    nameof(samples));
            }
        }
    }
}
