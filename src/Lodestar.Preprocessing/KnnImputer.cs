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
        KeptFeatures = Array.AsReadOnly(_kept);
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
    public IReadOnlyList<int> KeptFeatures { get; }

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

        // The fallback for a feature no donor can supply: np.ma.array(_fit_X[:, col]).mean(), the column filled and
        // copied contiguous, so summed pairwise (#1230).
        var means = new double[featureCount];
        var counts = new int[featureCount];
        NumpyReduction.MaskedColumnMeans(samples, sampleCount, featureCount, contiguous: true, means, counts);

        // Kept by its mask, as the reference keeps it: a pairwise sum of ±1e308 can be NaN with every value present.
        var kept = new List<int>(featureCount);
        for (int feature = 0; feature < featureCount; feature++)
        {
            if (counts[feature] > 0)
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
        var nearest = new (double Distance, int Donor)[Math.Min(_settings.NeighbourCount, SampleCount)];
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
                    ? Impute(distances, feature, nearest)
                    : value;
            }
        }

        return filled;
    }

    /// <summary>A missing cell's value from its nearest donors.</summary>
    /// <remarks>
    /// The k nearest are kept in a bounded heap rather than found by sorting every donor per cell, and
    /// a tie goes to the lower row: the reference breaks ties by <c>numpy.argpartition</c>, whose pick
    /// varies with the CPU's SIMD tier, so there is no single order of its to match (#1230).
    /// </remarks>
    private double Impute(double[] distances, int feature, (double Distance, int Donor)[] nearest)
    {
        int capacity = nearest.Length;
        int kept = 0;
        for (int donor = 0; donor < SampleCount; donor++)
        {
            double value = _fitted[(donor * FeatureCount) + feature];
            if (double.IsNaN(value) || double.IsNaN(distances[donor]))
            {
                continue;
            }

            var candidate = (distances[donor], donor);
            if (kept < capacity)
            {
                nearest[kept++] = candidate;
                SiftUp(nearest, kept - 1);
            }
            else if (Farther(nearest[0], candidate))
            {
                nearest[0] = candidate;
                SiftDown(nearest, kept);
            }
        }

        if (kept == 0)
        {
            return _columnMeans[feature];
        }

        Array.Sort(nearest, 0, kept, NearestFirst.Instance);

        // S1244: an exact zero distance, which is what the reference tests for; sorted, the nearest says for all.
#pragma warning disable S1244
        bool anyExact = nearest[0].Distance == 0.0;
#pragma warning restore S1244

        double weighted = 0.0;
        double total = 0.0;
        for (int i = 0; i < kept; i++)
        {
            double weight = Weight(nearest[i].Distance, anyExact);
            weighted += weight * _fitted[(nearest[i].Donor * FeatureCount) + feature];
            total += weight;
        }

        // S1244: every weight being an exact zero is the degenerate case, and only that.
#pragma warning disable S1244
        return total == 0.0 ? _columnMeans[feature] : weighted / total;
#pragma warning restore S1244
    }

    /// <summary>Whether <paramref name="left"/> ranks after <paramref name="right"/>: farther, or as far and a later row.</summary>
    private static bool Farther((double Distance, int Donor) left, (double Distance, int Donor) right) =>
        NearestFirst.Instance.Compare(left, right) > 0;

    /// <summary>Restores the farthest-at-the-root order after an append.</summary>
    private static void SiftUp((double Distance, int Donor)[] heap, int child)
    {
        while (child > 0)
        {
            int parent = (child - 1) >> 1;
            if (!Farther(heap[child], heap[parent]))
            {
                return;
            }

            (heap[child], heap[parent]) = (heap[parent], heap[child]);
            child = parent;
        }
    }

    /// <summary>Restores the farthest-at-the-root order after the root is replaced.</summary>
    private static void SiftDown((double Distance, int Donor)[] heap, int count)
    {
        int parent = 0;
        while (true)
        {
            int child = (2 * parent) + 1;
            if (child >= count)
            {
                return;
            }

            if (child + 1 < count && Farther(heap[child + 1], heap[child]))
            {
                child++;
            }

            if (!Farther(heap[child], heap[parent]))
            {
                return;
            }

            (heap[child], heap[parent]) = (heap[parent], heap[child]);
            parent = child;
        }
    }

    /// <summary>Nearest first, the lower row first among equals.</summary>
    private sealed class NearestFirst : IComparer<(double Distance, int Donor)>
    {
        public static readonly NearestFirst Instance = new();

        public int Compare((double Distance, int Donor) x, (double Distance, int Donor) y)
        {
            int byDistance = x.Distance.CompareTo(y.Distance);
            return byDistance != 0 ? byDistance : x.Donor.CompareTo(y.Donor);
        }
    }

    /// <summary>One donor's weight: equal shares, or the reciprocal of its distance.</summary>
    /// <remarks>
    /// A donor at distance zero is identical to the receiver on everything they share, so the
    /// reference gives it the whole weight and none to the rest rather than dividing by zero.
    /// Whether any kept donor sits at zero is read once per cell, not once per donor (#1230).
    /// </remarks>
    private double Weight(double distance, bool anyExact)
    {
        if (_settings.Weights == NeighbourWeights.Uniform)
        {
            return 1.0;
        }

        if (anyExact)
        {
            // S1244: as above.
#pragma warning disable S1244
            return distance == 0.0 ? 1.0 : 0.0;
#pragma warning restore S1244
        }

        return 1.0 / distance;
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
