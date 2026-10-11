using System.Globalization;
using System.Text;
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
        BinEdges = Array.AsReadOnly(Array.ConvertAll(_binEdges, row => (IReadOnlyList<double>)Array.AsReadOnly(row)));
        _encoding = encoding;
        _binCounts = new int[binEdges.Length];
        BinCounts = Array.AsReadOnly(_binCounts);
        for (int feature = 0; feature < binEdges.Length; feature++)
        {
            _binCounts[feature] = binEdges[feature].Length - 1;
        }
    }

    /// <summary>How many values each input row carries.</summary>
    public int FeatureCount { get; }

    /// <summary>How many rows the discretizer was fitted on.</summary>
    public int SampleCount { get; }

    /// <summary>Each feature's bin edges, ascending — the reference's <c>bin_edges_</c>, <c>[-∞, +∞]</c> for a constant feature.</summary>
    public IReadOnlyList<IReadOnlyList<double>> BinEdges { get; }

    /// <summary>
    /// How many bins each feature ended with — the reference's <c>n_bins_</c>, which can be fewer
    /// than asked for when two edges fall within <c>1e-8</c> of each other.
    /// </summary>
    public IReadOnlyList<int> BinCounts { get; }

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
            double[] column = SortedColumns.Of(samples, featureCount, feature, sampleCount);
            edges[feature] = Edges(column, settings);
        }

        return new KBinsDiscretizer(featureCount, sampleCount, edges, settings.Encoding);
    }

    /// <summary>Reads each value as the bin it falls in.</summary>
    /// <param name="samples">The matrix to transform, row-major.</param>
    /// <returns>A new matrix, <see cref="OutputFeatureCount"/> values per row.</returns>
    /// <exception cref="ArgumentException"><paramref name="samples"/> holds a partial row or a non-finite value, or its rows times the output width are more cells than one array holds.</exception>
    public double[] Transform(ReadOnlySpan<double> samples)
    {
        int sampleCount = SampleMatrix.Rows(samples, FeatureCount);
        SampleMatrix.RequireFinite(samples, nameof(samples));

        int width = OutputFeatureCount;
        var transformed = new double[TableLength.Of(sampleCount, width, nameof(samples))];
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
    /// <exception cref="ArgumentException">
    /// <paramref name="encoded"/> holds a partial row or a non-finite value; an ordinal code, truncated
    /// toward zero, is outside <c>[-n, n)</c> for a feature of <c>n</c> bins; or a one-hot block is all zeros.
    /// </exception>
    /// <remarks>
    /// The reference indexes the centres with <c>code.astype(np.int64)</c>, so a code is truncated and a
    /// negative one counts from the last bin, as numpy's indexing does; one past either end is refused
    /// where the reference raises <c>IndexError</c>. A one-hot block is read by its first largest value,
    /// numpy's <c>argmax</c>, and an all-zero block is refused as its encoder refuses it (#1230).
    /// </remarks>
    public double[] InverseTransform(ReadOnlySpan<double> encoded)
    {
        int width = OutputFeatureCount;
        int sampleCount = SampleMatrix.Rows(encoded, width);
        SampleMatrix.RequireFiniteAsChecked(encoded, nameof(encoded));
        if (_encoding == BinEncoding.OneHot)
        {
            RequireNoEmptyBlock(encoded, sampleCount, width, nameof(encoded));
        }

        var original = new double[sampleCount * FeatureCount];

        // Feature by feature, as the reference indexes a whole column at once: of two bad codes in two
        // features, the first feature's is the one refused.
        int column = 0;
        for (int feature = 0; feature < FeatureCount; feature++)
        {
            int count = _binCounts[feature];
            for (int row = 0; row < sampleCount; row++)
            {
                int source = row * width;
                int bin = _encoding == BinEncoding.Ordinal
                    ? CodeBin(encoded[source + feature], count, nameof(encoded))
                    : HotBin(encoded, source + column, count);
                original[(row * FeatureCount) + feature] =
                    (_binEdges[feature][bin + 1] + _binEdges[feature][bin]) * 0.5;
            }

            column += _encoding == BinEncoding.Ordinal ? 0 : count;
        }

        return original;
    }

    /// <summary>An ordinal code as the bin <c>bin_centers[code.astype(np.int64)]</c> reads.</summary>
    private static int CodeBin(double code, int count, string paramName)
    {
        double index = Math.Truncate(code);
        if (index < -count || index >= count)
        {
            // Past int64, astype(np.int64) reads the minimum on x86-64, which numpy's message then prints; aarch64 saturates.
            long printed = index > -9.2233720368547758e18 && index < 9.2233720368547758e18 ? (long)index : long.MinValue;
            throw new ArgumentException(
                $"index {printed.ToString(CultureInfo.InvariantCulture)} is out of bounds for axis 0 with size {count}",
                paramName);
        }

        return index < 0 ? (int)index + count : (int)index;
    }

    /// <summary>A one-hot block as its encoder reads it: the first largest column, numpy's <c>argmax</c>.</summary>
    private static int HotBin(ReadOnlySpan<double> encoded, int start, int count)
    {
        int best = 0;
        for (int i = 1; i < count; i++)
        {
            if (encoded[start + i] > encoded[start + best])
            {
                best = i;
            }
        }

        return best;
    }

    /// <summary>
    /// The encoder's refusal of an all-zero block: feature by feature, the first whose blocks sum to zero
    /// on any row names every such row, as <c>np.flatnonzero</c> prints them.
    /// </summary>
    private void RequireNoEmptyBlock(ReadOnlySpan<double> encoded, int sampleCount, int width, string paramName)
    {
        int column = 0;
        for (int feature = 0; feature < FeatureCount; feature++)
        {
            List<int>? empty = null;
            for (int row = 0; row < sampleCount; row++)
            {
                // np.sum over the block, numpy's pairwise order, which a long block's rounding depends on.
                double sum = NumpyReduction.Sum(encoded.Slice((row * width) + column, _binCounts[feature]));

                // S1244: the encoder refuses a block whose sum is exactly zero, and only that.
#pragma warning disable S1244
                if (sum == 0.0)
#pragma warning restore S1244
                {
                    (empty ??= []).Add(row);
                }
            }

            if (empty is not null)
            {
                throw new ArgumentException(
                    $"Samples {Printed(empty)} can not be inverted when drop=None and handle_unknown='error' because they contain all zeros",
                    paramName);
            }

            column += _binCounts[feature];
        }
    }

    /// <summary>Row indices as <c>str(np.array(rows))</c> prints them: padded to the widest, wrapped at 75, summarised past 1000.</summary>
    private static string Printed(List<int> rows)
    {
        IEnumerable<int> shown = rows.Count > 1000 ? rows.Take(3).Concat(rows.Skip(rows.Count - 3)) : rows;
        string[] words = shown.Select(row => row.ToString(CultureInfo.InvariantCulture)).ToArray();
        int width = words.Max(word => word.Length);
        var tokens = words.Select(word => word.PadLeft(width)).ToList();
        if (rows.Count > 1000)
        {
            tokens.Insert(3, "...");
        }

        // numpy's _formatArray: a word wraps when it would pass 75 less the closing bracket.
        var text = new StringBuilder();
        var line = new StringBuilder("[");
        for (int i = 0; i < tokens.Count; i++)
        {
            if (line.Length + tokens[i].Length > 74 && line.Length > 1)
            {
                text.Append(line.ToString().TrimEnd()).Append('\n');
                line.Clear().Append(' ');
            }

            line.Append(tokens[i]);
            if (i < tokens.Count - 1)
            {
                line.Append(' ');
            }
        }

        return text.Append(line).Append(']').ToString();
    }

    private int BinOf(double value, int feature)
    {
        double[] edges = _binEdges[feature];

        // np.searchsorted(edges[1:-1], value, side='right'): a value outside the fitted range clips to
        // the first or last bin, as the reference's does. Halved without a branch to mispredict (#1232).
        int interior = edges.Length - 2;
        if (interior == 0)
        {
            return 0;
        }

        int first = 1;
        while (interior > 1)
        {
            int half = interior >> 1;
            first += half & -(value >= edges[first + half] ? 1 : 0);
            interior -= half;
        }

        return first - 1 + (value >= edges[first] ? 1 : 0);
    }

    private static double[] Edges(double[] sortedColumn, KBinsDiscretizerOptions settings)
    {
        double low = sortedColumn[0];
        double high = sortedColumn[sortedColumn.Length - 1];

        // S1244: a constant feature is one whose extremes are literally equal, and the reference
        // gives it one bin over the whole line, so its inverse is NaN as the reference's is (#1230).
#pragma warning disable S1244
        if (low == high)
#pragma warning restore S1244
        {
            return [double.NegativeInfinity, double.PositiveInfinity];
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

    /// <summary><c>np.linspace(low, high, binCount + 1)</c>, which <c>low + (high - low) · i / n</c> misses by an ulp (#1226).</summary>
    private static double[] Uniform(double low, double high, int binCount) =>
        NumpyGrid.Linspace(low, high, binCount + 1);

    /// <summary>The percentiles at <c>np.linspace(0, 100, binCount + 1)</c>, the levels the reference reads (#1226, #1237).</summary>
    private static double[] Quantiles(double[] sortedColumn, KBinsDiscretizerOptions settings)
    {
        double[] levels = NumpyGrid.Linspace(0.0, 100.0, settings.BinCount + 1);
        var edges = new double[levels.Length];
        for (int i = 0; i < levels.Length; i++)
        {
            edges[i] = settings.QuantileMethod == QuantileMethod.Linear
                ? NumpyGrid.Percentile(sortedColumn, levels[i])
                : NumpyGrid.AveragedInvertedCdf(sortedColumn, levels[i]);
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
