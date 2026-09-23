using Lodestar.Cluster;
using Lodestar.Preprocessing.Internal;

namespace Lodestar.Preprocessing;

/// <summary>
/// Cuts each feature into bins, at <c>sklearn.preprocessing.KBinsDiscretizer</c> parity.
/// </summary>
/// <remarks>
/// Turns a measurement into a category, which is what a model that reads order but not distance
/// wants — and what ML.NET's <c>NormalizeBinning</c> does not do: that one rescales into
/// <c>[0, 1]</c> by bin, where this emits the bin itself.
/// </remarks>
public sealed class KBinsDiscretizer
{
    /// <summary>Edges closer together than this are the same edge; the reference's own threshold.</summary>
    private const double MinimumBinWidth = 1e-8;

    private readonly double[][] _binEdges;
    private readonly int[] _binCounts;
    private readonly BinEncoding _encoding;

    private KBinsDiscretizer(int featureCount, int sampleCount, double[][] binEdges, BinEncoding encoding)
    {
        FeatureCount = featureCount;
        SampleCount = sampleCount;
        _binEdges = binEdges;
        _encoding = encoding;
        _binCounts = new int[binEdges.Length];
        for (int feature = 0; feature < binEdges.Length; feature++)
        {
            _binCounts[feature] = binEdges[feature].Length - 1;
        }
    }

    /// <summary>How many values each input row carries.</summary>
    public int FeatureCount { get; }

    /// <summary>How many rows the discretizer was fitted on.</summary>
    public int SampleCount { get; }

    /// <summary>Each feature's bin edges, ascending — the reference's <c>bin_edges_</c>.</summary>
    public IReadOnlyList<IReadOnlyList<double>> BinEdges => _binEdges;

    /// <summary>
    /// How many bins each feature ended with — the reference's <c>n_bins_</c>, which can be fewer
    /// than asked for when two edges fall within <c>1e-8</c> of each other.
    /// </summary>
    public IReadOnlyList<int> BinCounts => _binCounts;

    /// <summary>How many values a transformed row carries.</summary>
    public int OutputFeatureCount => _encoding == BinEncoding.Ordinal ? FeatureCount : Sum(_binCounts);

    /// <summary>Fits bin edges on a row-major matrix.</summary>
    /// <param name="samples">The matrix, row-major: <paramref name="featureCount"/> values per row.</param>
    /// <param name="featureCount">How many values each row carries.</param>
    /// <param name="options">Bin count, strategy, encoding and percentile convention; <see langword="null"/> takes the reference's defaults.</param>
    /// <returns>A fitted discretizer.</returns>
    /// <exception cref="ArgumentOutOfRangeException"><paramref name="featureCount"/> is not positive, or the bin count is below two.</exception>
    /// <exception cref="ArgumentException"><paramref name="samples"/> holds no row, a partial one, or a non-finite value.</exception>
    public static KBinsDiscretizer Fit(
        ReadOnlySpan<double> samples, int featureCount, KBinsDiscretizerOptions? options = null)
    {
        Guard.NotLessThan(featureCount, 1);
        KBinsDiscretizerOptions settings = options ?? new KBinsDiscretizerOptions();
        Guard.NotLessThan(settings.BinCount, 2);

        int sampleCount = SampleMatrix.Rows(samples, featureCount);
        SampleMatrix.RequireFinite(samples, nameof(samples));

        var edges = new double[featureCount][];
        for (int feature = 0; feature < featureCount; feature++)
        {
            double[] column = Percentile.SortedColumn(samples, featureCount, feature, sampleCount);
            edges[feature] = Edges(column, settings);
        }

        return new KBinsDiscretizer(featureCount, sampleCount, edges, settings.Encoding);
    }

    /// <summary>Reads each value as the bin it falls in.</summary>
    /// <param name="samples">The matrix to transform, row-major.</param>
    /// <returns>A new matrix, <see cref="OutputFeatureCount"/> values per row.</returns>
    /// <exception cref="ArgumentException"><paramref name="samples"/> holds a partial row or a non-finite value.</exception>
    public double[] Transform(ReadOnlySpan<double> samples)
    {
        int sampleCount = SampleMatrix.Rows(samples, FeatureCount);
        SampleMatrix.RequireFinite(samples, nameof(samples));

        int width = OutputFeatureCount;
        var transformed = new double[sampleCount * width];
        for (int row = 0; row < sampleCount; row++)
        {
            int source = row * FeatureCount;
            int target = row * width;
            int column = 0;
            for (int feature = 0; feature < FeatureCount; feature++)
            {
                int bin = BinOf(samples[source + feature], feature);
                if (_encoding == BinEncoding.Ordinal)
                {
                    transformed[target + feature] = bin;
                    continue;
                }

                transformed[target + column + bin] = 1.0;
                column += _binCounts[feature];
            }
        }

        return transformed;
    }

    /// <summary>Reads each bin back as its centre, which is what the reference's inverse returns.</summary>
    /// <param name="encoded">A matrix this discretizer produced.</param>
    /// <returns>One value per input feature, per row.</returns>
    /// <exception cref="ArgumentException"><paramref name="encoded"/> holds a partial row.</exception>
    public double[] InverseTransform(ReadOnlySpan<double> encoded)
    {
        int width = OutputFeatureCount;
        int sampleCount = SampleMatrix.Rows(encoded, width);
        var original = new double[sampleCount * FeatureCount];

        for (int row = 0; row < sampleCount; row++)
        {
            int source = row * width;
            int target = row * FeatureCount;
            int column = 0;
            for (int feature = 0; feature < FeatureCount; feature++)
            {
                int bin = _encoding == BinEncoding.Ordinal
                    ? (int)encoded[source + feature]
                    : HotBin(encoded, source + column, _binCounts[feature]);
                column += _encoding == BinEncoding.Ordinal ? 0 : _binCounts[feature];

                bin = Math.Min(Math.Max(bin, 0), _binCounts[feature] - 1);
                original[target + feature] =
                    0.5 * (_binEdges[feature][bin] + _binEdges[feature][bin + 1]);
            }
        }

        return original;
    }

    private static int HotBin(ReadOnlySpan<double> encoded, int start, int count)
    {
        for (int i = 0; i < count; i++)
        {
            // S1244: a one-hot column holds an exact one where it is set; nothing rounds here.
#pragma warning disable S1244
            if (encoded[start + i] == 1.0)
#pragma warning restore S1244
            {
                return i;
            }
        }

        return 0;
    }

    private int BinOf(double value, int feature)
    {
        double[] edges = _binEdges[feature];

        // The reference searches the interior edges and clips, so a value below the fitted range
        // lands in the first bin and one above it in the last rather than being refused.
        int bin = 0;
        while (bin < edges.Length - 2 && value >= edges[bin + 1])
        {
            bin++;
        }

        return bin;
    }

    private static double[] Edges(double[] sortedColumn, KBinsDiscretizerOptions settings)
    {
        double low = sortedColumn[0];
        double high = sortedColumn[sortedColumn.Length - 1];

        // S1244: a constant feature is one whose extremes are literally equal, and the reference
        // gives it a single bin rather than cutting a zero-wide range into several.
#pragma warning disable S1244
        if (low == high)
#pragma warning restore S1244
        {
            return [low, high];
        }

        double[] edges = settings.Strategy switch
        {
            BinStrategy.Uniform => Uniform(low, high, settings.BinCount),
            BinStrategy.KMeans => KMeansEdges(sortedColumn, low, high, settings.BinCount),
            _ => Quantiles(sortedColumn, settings),
        };

        // Uniform edges are evenly spaced by construction, so only the other two can collapse.
        return settings.Strategy == BinStrategy.Uniform ? edges : Widened(edges);
    }

    private static double[] Uniform(double low, double high, int binCount)
    {
        var edges = new double[binCount + 1];
        for (int i = 0; i <= binCount; i++)
        {
            edges[i] = low + ((high - low) * i / binCount);
        }

        edges[binCount] = high;
        return edges;
    }

    private static double[] Quantiles(double[] sortedColumn, KBinsDiscretizerOptions settings)
    {
        var edges = new double[settings.BinCount + 1];
        for (int i = 0; i <= settings.BinCount; i++)
        {
            double percent = 100.0 * i / settings.BinCount;
            edges[i] = settings.QuantileMethod == QuantileMethod.Linear
                ? Percentile.Linear(sortedColumn, percent)
                : Percentile.AveragedInvertedCdf(sortedColumn, percent);
        }

        return edges;
    }

    /// <summary>Edges midway between one-dimensional k-means centres, seeded as the reference seeds them.</summary>
    /// <remarks>
    /// The centres start at the uniform strategy's bin midpoints, which is what makes the fit
    /// deterministic — the reference passes that same <c>init</c> with <c>n_init=1</c> rather than
    /// drawing one. <see cref="KMeans"/> in <c>Lodestar.Cluster</c> runs the loop; rewriting Lloyd's
    /// algorithm here to avoid an edge is the duplication #763 decided against.
    /// </remarks>
    private static double[] KMeansEdges(double[] sortedColumn, double low, double high, int binCount)
    {
        double[] uniform = Uniform(low, high, binCount);
        var initial = new double[binCount];
        for (int i = 0; i < binCount; i++)
        {
            initial[i] = 0.5 * (uniform[i] + uniform[i + 1]);
        }

        KMeans fitted = KMeans.Fit(
            sortedColumn, featureCount: 1, clusterCount: binCount,
            new KMeansOptions { InitialCentres = initial });

        var centres = new double[binCount];
        for (int i = 0; i < binCount; i++)
        {
            centres[i] = fitted.Centres[i];
        }

        // The centres may come back unsorted even from sorted starting points, so the reference
        // sorts them before reading midpoints, and so does this.
        Array.Sort(centres);

        var edges = new double[binCount + 1];
        edges[0] = low;
        for (int i = 1; i < binCount; i++)
        {
            edges[i] = 0.5 * (centres[i - 1] + centres[i]);
        }

        edges[binCount] = high;
        return edges;
    }

    /// <summary>Drops an edge closer to its predecessor than the reference's own threshold.</summary>
    /// <summary>Drops every edge whose gap to the one before it in the original array is too small.</summary>
    // long-comment: the two readings of "too close" part only on a run of narrow gaps, which is
    // exactly the input nobody writes by hand, so the measurement belongs next to the code.
    // Successive gaps, not the distance to the last edge kept. Over
    // [0, 6e-9, 1.2e-8, 1.8e-8, 2.4e-8, 3e-8, 1] cut into five, the reference keeps [0, 2.4e-8, 1]
    // where a running gap keeps [0, 1.2e-8, 2.4e-8, 1] and codes two of the rows differently
    // (#1128, measured against scikit-learn 1.9.1).
    private static double[] Widened(double[] edges)
    {
        var kept = new List<double> { edges[0] };
        for (int i = 1; i < edges.Length; i++)
        {
            if (edges[i] - edges[i - 1] > MinimumBinWidth)
            {
                kept.Add(edges[i]);
            }
        }

        // long-comment: a floor that parts from the reference needs its reason where it is read.
        // The reference lets the mask leave one edge and reports n_bins_ = 0, a state its own
        // inverse_transform and its own one-hot fit both raise IndexError on. One bin instead --
        // a deliberate divergence, docs/equivalence.md carries it (#1128).
        return kept.Count < 2 ? [edges[0], edges[edges.Length - 1]] : [.. kept];
    }

    private static int Sum(int[] counts)
    {
        int total = 0;
        for (int i = 0; i < counts.Length; i++)
        {
            total += counts[i];
        }

        return total;
    }
}
