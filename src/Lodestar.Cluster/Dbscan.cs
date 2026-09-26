using Lodestar.Cluster.Internal;

namespace Lodestar.Cluster;

/// <summary>
/// Finds clusters as dense regions separated by sparse ones, at
/// <c>sklearn.cluster.DBSCAN(metric="euclidean")</c> parity.
/// </summary>
/// <remarks>
/// Spans in, arrays out, row-major — the shape <c>Lodestar.Metrics</c> takes. Unlike
/// <see cref="KMeans"/> the cluster count is an output: a sample in no dense region is
/// <see cref="Noise"/> rather than forced into the nearest, and labels match the reference's
/// without a tolerance bar the caveat <see cref="CoreSampleIndices"/> records.
/// </remarks>
public sealed class Dbscan
{
    private readonly int[] _labels;
    private readonly int[] _coreSampleIndices;

    private Dbscan(int featureCount, int[] labels, int[] coreSampleIndices, int clusterCount)
    {
        FeatureCount = featureCount;
        _labels = labels;
        _coreSampleIndices = coreSampleIndices;
        ClusterCount = clusterCount;
    }

    /// <summary>The label of a sample in no dense region — scikit-learn's <c>-1</c>.</summary>
    public static int Noise => Growth.Noise;

    /// <summary>How many values each row of the fitted matrix carried.</summary>
    /// <remarks>
    /// The sample count for a fit from a precomputed matrix, which is square: there are no
    /// features there to count.
    /// </remarks>
    public int FeatureCount { get; }

    /// <summary>How many clusters the fit found; noise is not one of them.</summary>
    public int ClusterCount { get; }

    /// <summary>The cluster each sample belongs to — scikit-learn's <c>labels_</c>.</summary>
    /// <remarks>
    /// <see cref="Noise"/> for a sample no cluster reached. Clusters are numbered from zero in
    /// the order they are grown, which is the order of their first core sample's row.
    /// </remarks>
    public IReadOnlyList<int> Labels => _labels;

    /// <summary>The core samples' rows, ascending — <c>core_sample_indices_</c>.</summary>
    /// <remarks>
    /// A core sample is one whose neighbourhood, itself included, holds at least
    /// <c>minimumSamples</c> samples. Every other labelled sample is a border sample, and the
    /// difference decides which of two neighbouring clusters claims it.
    /// </remarks>
    public IReadOnlyList<int> CoreSampleIndices => _coreSampleIndices;

    /// <summary>Clusters a row-major sample matrix by euclidean distance.</summary>
    /// <param name="samples">The samples, row-major: <paramref name="featureCount"/> values per row.</param>
    /// <param name="featureCount">How many values each row carries.</param>
    /// <param name="epsilon">The inclusive radius of a neighbourhood; scikit-learn's <c>eps</c>.</param>
    /// <param name="minimumSamples">How many samples a neighbourhood needs to be dense, the sample itself counted.</param>
    /// <returns>A fitted clustering.</returns>
    /// <exception cref="ArgumentOutOfRangeException"><paramref name="featureCount"/> or <paramref name="minimumSamples"/> is not positive, or <paramref name="epsilon"/> is not positive or not finite.</exception>
    /// <exception cref="ArgumentException"><paramref name="samples"/> holds no row, a partial one, or a value that is not finite.</exception>
    /// <remarks>
    /// Neither value is defaulted. scikit-learn defaults <c>eps=0.5</c>, which is meaningful only
    /// on data already scaled to unit variance; on a matrix of euros it is one cluster, and on a
    /// matrix of milliseconds all noise. A default wrong on most matrices reads as advice.
    /// </remarks>
    public static Dbscan Fit(
        ReadOnlySpan<double> samples, int featureCount, double epsilon, int minimumSamples)
    {
        Guard.NotLessThan(featureCount, 1);
        Guard.NotLessThan(minimumSamples, 1);
        Positive(epsilon, nameof(epsilon));

        int sampleCount = Rows(samples, featureCount);
        Finite.Require(samples, nameof(samples));
        (int[] offsets, int[] indices) =
            Neighbourhoods.Euclidean(samples, featureCount, sampleCount, epsilon);
        return From(featureCount, offsets, indices, minimumSamples);
    }

    /// <summary>Clusters by euclidean distance, a neighbourhood's density counted by weight — <c>fit(X, sample_weight=w)</c>.</summary>
    /// <param name="samples">The samples, row-major: <paramref name="featureCount"/> values per row.</param>
    /// <param name="featureCount">How many values each row carries.</param>
    /// <param name="epsilon">The inclusive radius of a neighbourhood; scikit-learn's <c>eps</c>.</param>
    /// <param name="minimumSamples">The summed weight a neighbourhood needs to be dense, the sample's own counted.</param>
    /// <param name="sampleWeights">One finite weight per sample; negative ones are allowed, as scikit-learn allows them.</param>
    /// <returns>A fitted clustering.</returns>
    /// <exception cref="ArgumentOutOfRangeException">As <see cref="Fit(ReadOnlySpan{double}, int, double, int)"/>.</exception>
    /// <exception cref="ArgumentException">As <see cref="Fit(ReadOnlySpan{double}, int, double, int)"/>, or the weights are not one finite value per sample.</exception>
    /// <remarks>
    /// A sample standing for several identical rows weighs their count, so a deduplicated matrix clusters as the full
    /// one would; a negative weight can keep its neighbours from being core, as scikit-learn documents.
    /// </remarks>
    public static Dbscan Fit(
        ReadOnlySpan<double> samples, int featureCount, double epsilon, int minimumSamples, ReadOnlySpan<double> sampleWeights)
    {
        Guard.NotLessThan(featureCount, 1);
        Guard.NotLessThan(minimumSamples, 1);
        Positive(epsilon, nameof(epsilon));

        int sampleCount = Rows(samples, featureCount);
        Finite.Require(samples, nameof(samples));
        double[] weights = Weights(sampleWeights, sampleCount);
        (int[] offsets, int[] indices) = Neighbourhoods.Euclidean(samples, featureCount, sampleCount, epsilon);
        return From(featureCount, offsets, indices, minimumSamples, weights);
    }

    /// <summary>Clusters from a square distance matrix, a neighbourhood's density counted by weight.</summary>
    /// <param name="distances">The pairwise distances, row-major and square.</param>
    /// <param name="sampleCount">The side of that matrix.</param>
    /// <param name="epsilon">The inclusive radius of a neighbourhood; scikit-learn's <c>eps</c>.</param>
    /// <param name="minimumSamples">The summed weight a neighbourhood needs to be dense, the sample's own counted.</param>
    /// <param name="sampleWeights">One finite weight per sample.</param>
    /// <returns>A fitted clustering.</returns>
    /// <exception cref="ArgumentOutOfRangeException">As <see cref="FitPrecomputed(ReadOnlySpan{double}, int, double, int)"/>.</exception>
    /// <exception cref="ArgumentException">As <see cref="FitPrecomputed(ReadOnlySpan{double}, int, double, int)"/>, or the weights are not one finite value per sample.</exception>
    public static Dbscan FitPrecomputed(
        ReadOnlySpan<double> distances, int sampleCount, double epsilon, int minimumSamples, ReadOnlySpan<double> sampleWeights)
    {
        (int[] offsets, int[] indices) = PrecomputedNeighbourhoods(distances, sampleCount, epsilon, minimumSamples);
        return From(sampleCount, offsets, indices, minimumSamples, Weights(sampleWeights, sampleCount));
    }

    /// <summary>Clusters from a square distance matrix — <c>metric="precomputed"</c>.</summary>
    /// <param name="distances">The pairwise distances, row-major and square.</param>
    /// <param name="sampleCount">The side of that matrix.</param>
    /// <param name="epsilon">The inclusive radius of a neighbourhood; scikit-learn's <c>eps</c>.</param>
    /// <param name="minimumSamples">How many samples a neighbourhood needs to be dense, the sample itself counted.</param>
    /// <returns>A fitted clustering.</returns>
    /// <exception cref="ArgumentOutOfRangeException"><paramref name="sampleCount"/> or <paramref name="minimumSamples"/> is not positive, or <paramref name="epsilon"/> is not positive or not finite.</exception>
    /// <exception cref="ArgumentException"><paramref name="distances"/> is not <paramref name="sampleCount"/> squared values, or holds a value that is not finite.</exception>
    /// <remarks>
    /// Named rather than an overload of <see cref="Fit(ReadOnlySpan{double}, int, double, int)"/>: both take a row-major
    /// <see cref="ReadOnlySpan{T}"/> of doubles and an <see cref="int"/>, so an overload pair
    /// would be told apart only by what the second argument <em>means</em>. The diagonal's zero
    /// distance makes a sample its own neighbour, so <paramref name="minimumSamples"/> counts
    /// the same way here as it does there.
    /// </remarks>
    public static Dbscan FitPrecomputed(
        ReadOnlySpan<double> distances, int sampleCount, double epsilon, int minimumSamples)
    {
        (int[] offsets, int[] indices) = PrecomputedNeighbourhoods(distances, sampleCount, epsilon, minimumSamples);
        return From(sampleCount, offsets, indices, minimumSamples);
    }

    /// <summary>The checks both precomputed fits share, then the neighbourhoods.</summary>
    private static (int[] Offsets, int[] Indices) PrecomputedNeighbourhoods(
        ReadOnlySpan<double> distances, int sampleCount, double epsilon, int minimumSamples)
    {
        Guard.NotLessThan(sampleCount, 1);
        Guard.NotLessThan(minimumSamples, 1);
        Positive(epsilon, nameof(epsilon));

        // Squared as a long: from 46341 samples an int product wraps, and 65536 wraps to zero,
        // which an empty span would then match.
        long expected = (long)sampleCount * sampleCount;
        if (distances.Length != expected)
        {
            throw new ArgumentException(
                $"distances holds {distances.Length} values, not the {expected} "
                + $"a square matrix of {sampleCount} samples needs.",
                nameof(distances));
        }

        Finite.Require(distances, nameof(distances));
        return Neighbourhoods.Precomputed(distances, sampleCount, epsilon);
    }

    private static Dbscan From(int featureCount, int[] offsets, int[] indices, int minimumSamples, double[]? weights = null)
    {
        (int[] labels, int[] cores, int clusters) = Growth.Label(offsets, indices, minimumSamples, weights);
        return new Dbscan(featureCount, labels, cores, clusters);
    }

    private static double[] Weights(ReadOnlySpan<double> sampleWeights, int sampleCount)
    {
        if (sampleWeights.Length != sampleCount)
        {
            throw new ArgumentException(
                $"sampleWeights holds {sampleWeights.Length} values for {sampleCount} samples.", nameof(sampleWeights));
        }

        Finite.Require(sampleWeights, nameof(sampleWeights));
        return sampleWeights.ToArray();
    }

    private static void Positive(double epsilon, string paramName)
    {
        // Not `<= 0` alone: a NaN fails every comparison, so it would pass a `<= 0` guard and
        // then make every neighbourhood empty, which reads as data too sparse to cluster.
        if (!(epsilon > 0.0) || double.IsInfinity(epsilon))
        {
            throw new ArgumentOutOfRangeException(
                paramName, epsilon, "Must be a positive, finite radius.");
        }
    }

    private static int Rows(ReadOnlySpan<double> samples, int featureCount)
    {
        if (samples.Length == 0 || samples.Length % featureCount != 0)
        {
            throw new ArgumentException(
                $"samples holds {samples.Length} values, which is not a positive whole number of "
                + $"rows of {featureCount}.",
                nameof(samples));
        }

        return samples.Length / featureCount;
    }
}
