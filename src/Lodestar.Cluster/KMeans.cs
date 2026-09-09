using Lodestar.Cluster.Internal;

namespace Lodestar.Cluster;

/// <summary>
/// Partitions samples into <see cref="ClusterCount"/> clusters by Lloyd's algorithm, at
/// <c>sklearn.cluster.KMeans(algorithm="lloyd")</c> parity.
/// </summary>
/// <remarks>
/// Spans in, arrays out, row-major — the shape <c>Lodestar.Metrics</c> already uses, so
/// <see cref="Labels"/> feeds its silhouette, adjusted Rand, AMI and V-measure without
/// being reshaped. That is the whole reason this package is cheap: the scoring half
/// already shipped.
/// </remarks>
public sealed class KMeans
{
    private readonly double[] _centres;
    private readonly int[] _labels;

    private KMeans(int clusterCount, int featureCount, double[] centres, int[] labels, double inertia, int iterations)
    {
        ClusterCount = clusterCount;
        FeatureCount = featureCount;
        _centres = centres;
        _labels = labels;
        Inertia = inertia;
        Iterations = iterations;
    }

    /// <summary>How many clusters the fit produced.</summary>
    public int ClusterCount { get; }

    /// <summary>How many values each row of a sample matrix carries.</summary>
    public int FeatureCount { get; }

    /// <summary>The cluster centres, row-major — scikit-learn's <c>cluster_centers_</c>.</summary>
    public IReadOnlyList<double> Centres => _centres;

    /// <summary>The cluster each fitted sample belongs to — scikit-learn's <c>labels_</c>.</summary>
    public IReadOnlyList<int> Labels => _labels;

    /// <summary>Summed squared distance from each sample to its centre — <c>inertia_</c>.</summary>
    public double Inertia { get; }

    /// <summary>How many Lloyd iterations ran — scikit-learn's <c>n_iter_</c>.</summary>
    public int Iterations { get; }

    /// <summary>Fits k-means on a row-major sample matrix.</summary>
    /// <param name="samples">The samples, row-major: <paramref name="featureCount"/> values per row.</param>
    /// <param name="featureCount">How many values each row carries.</param>
    /// <param name="clusterCount">How many clusters to find.</param>
    /// <param name="options">Where to start and when to stop; <see langword="null"/> takes the defaults.</param>
    /// <returns>A fitted clustering.</returns>
    /// <exception cref="ArgumentOutOfRangeException"><paramref name="featureCount"/> or <paramref name="clusterCount"/> is not positive, or <paramref name="options"/> asks for fewer than one iteration.</exception>
    /// <exception cref="ArgumentException"><paramref name="samples"/> holds no row or a partial one, there are fewer rows than clusters, or the given initial centres are the wrong shape.</exception>
    /// <remarks>
    /// The loop is the reference's: assign, update, stop on unchanged labels or on a centre
    /// shift within the scaled tolerance — and when it stops on the shift, a final assignment
    /// runs so <see cref="Labels"/> matches <see cref="Centres"/>. An empty cluster is
    /// relocated onto the sample furthest from its own centre. The reference page carries the
    /// tolerance scaling and the one measured divergence.
    /// </remarks>
    public static KMeans Fit(
        ReadOnlySpan<double> samples, int featureCount, int clusterCount, KMeansOptions? options = null)
    {
        Guard.NotLessThan(featureCount, 1);
        Guard.NotLessThan(clusterCount, 1);
        KMeansOptions settings = options ?? new KMeansOptions();
        Guard.NotLessThan(settings.MaxIterations, 1);

        int sampleCount = Rows(samples, featureCount);
        if (sampleCount < clusterCount)
        {
            throw new ArgumentException(
                $"samples holds {sampleCount} rows, fewer than the {clusterCount} clusters asked for.",
                nameof(clusterCount));
        }

        double[] centres = StartingCentres(samples, featureCount, clusterCount, sampleCount, settings);
        double tolerance = ScaledTolerance(samples, featureCount, sampleCount, settings.Tolerance);

        var labels = new int[sampleCount];
        var previous = new int[sampleCount];
        for (int row = 0; row < sampleCount; row++)
        {
            // -1 is not a cluster, so the first comparison can never report convergence.
            previous[row] = -1;
        }

        bool strict = false;
        int iterations = 0;
        for (int step = 0; step < settings.MaxIterations; step++)
        {
            iterations = step + 1;
            Assign(samples, featureCount, centres, clusterCount, labels);
            double shift = Update(samples, featureCount, clusterCount, sampleCount, labels, centres);

            if (SameLabels(labels, previous))
            {
                strict = true;
                break;
            }

            if (shift <= tolerance)
            {
                break;
            }

            Array.Copy(labels, previous, labels.Length);
        }

        if (!strict)
        {
            // The centres moved after the last assignment, so the labels are one update
            // behind them. The reference re-runs the E-step for exactly this reason.
            Assign(samples, featureCount, centres, clusterCount, labels);
        }

        double inertia = Inertias(samples, featureCount, centres, labels);
        return new KMeans(clusterCount, featureCount, centres, labels, inertia, iterations);
    }

    /// <summary>Assigns unseen samples to the fitted centres.</summary>
    /// <param name="samples">The samples, row-major, with <see cref="FeatureCount"/> values per row.</param>
    /// <returns>One cluster index per row.</returns>
    /// <exception cref="ArgumentException"><paramref name="samples"/> holds no row, or a partial one.</exception>
    /// <remarks>Nothing is refitted: this is the assignment step alone, over the centres already found.</remarks>
    public int[] Predict(ReadOnlySpan<double> samples)
    {
        int rows = Rows(samples, FeatureCount);
        var labels = new int[rows];
        Assign(samples, FeatureCount, _centres, ClusterCount, labels);
        return labels;
    }

    /// <summary>The row count, with the two shapes that are not a matrix refused.</summary>
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

    /// <summary>The caller's centres, checked, or k-means++ over this package's generator.</summary>
    private static double[] StartingCentres(
        ReadOnlySpan<double> samples, int featureCount, int clusterCount, int sampleCount, KMeansOptions settings)
    {
        if (settings.InitialCentres is not null)
        {
            if (settings.InitialCentres.Length != clusterCount * featureCount)
            {
                throw new ArgumentException(
                    $"InitialCentres holds {settings.InitialCentres.Length} values, not the "
                    + $"{clusterCount} x {featureCount} a starting block needs.",
                    nameof(settings));
            }

            return (double[])settings.InitialCentres.Clone();
        }

        return PlusPlus(samples, featureCount, clusterCount, sampleCount, settings.Seed);
    }

    /// <summary>scikit-learn's tolerance scaling: the raw value times the mean feature variance.</summary>
    private static double ScaledTolerance(
        ReadOnlySpan<double> samples, int featureCount, int sampleCount, double tolerance)
    {
        // S1244: sklearn.cluster._kmeans._tolerance tests `tol == 0` exactly, and so must
        // this -- zero is a caller's sentinel for "iterate until the labels settle", not a
        // measured quantity that could land near zero by accident.
#pragma warning disable S1244
        if (tolerance == 0.0)
#pragma warning restore S1244
        {
            return 0.0;
        }

        double total = 0.0;
        for (int feature = 0; feature < featureCount; feature++)
        {
            double mean = 0.0;
            for (int row = 0; row < sampleCount; row++)
            {
                mean += samples[(row * featureCount) + feature];
            }

            mean /= sampleCount;

            double squared = 0.0;
            for (int row = 0; row < sampleCount; row++)
            {
                double deviation = samples[(row * featureCount) + feature] - mean;
                squared += deviation * deviation;
            }

            total += squared / sampleCount;
        }

        return total / featureCount * tolerance;
    }

    /// <summary>The E-step: every sample takes the nearest centre, ties to the lower index.</summary>
    /// <remarks>
    /// <c>numpy.argmin</c>'s rule, and <strong>a measured divergence from the reference</strong>
    /// on an exact tie — decision 0093 has the two configurations that send scikit-learn's
    /// choice both ways, and why no single rule reproduces both.
    /// </remarks>
    private static void Assign(
        ReadOnlySpan<double> samples, int featureCount, double[] centres, int clusterCount, int[] labels)
    {
        for (int row = 0; row < labels.Length; row++)
        {
            int offset = row * featureCount;
            double best = double.PositiveInfinity;
            int chosen = 0;

            for (int cluster = 0; cluster < clusterCount; cluster++)
            {
                double distance = SquaredDistance(samples, offset, centres, cluster * featureCount, featureCount);
                if (distance < best)
                {
                    best = distance;
                    chosen = cluster;
                }
            }

            labels[row] = chosen;
        }
    }

    /// <summary>The M-step, returning the summed squared distance the centres moved.</summary>
    private static double Update(
        ReadOnlySpan<double> samples,
        int featureCount,
        int clusterCount,
        int sampleCount,
        int[] labels,
        double[] centres)
    {
        var totals = new double[clusterCount * featureCount];
        var counts = new int[clusterCount];

        for (int row = 0; row < sampleCount; row++)
        {
            int cluster = labels[row];
            counts[cluster]++;
            for (int feature = 0; feature < featureCount; feature++)
            {
                totals[(cluster * featureCount) + feature] += samples[(row * featureCount) + feature];
            }
        }

        Relocate(samples, featureCount, centres, labels, totals, counts);

        double shift = 0.0;
        for (int cluster = 0; cluster < clusterCount; cluster++)
        {
            for (int feature = 0; feature < featureCount; feature++)
            {
                int index = (cluster * featureCount) + feature;
                double moved = (totals[index] / counts[cluster]) - centres[index];
                shift += moved * moved;
                centres[index] = totals[index] / counts[cluster];
            }
        }

        return shift;
    }

    /// <summary>Gives every empty cluster the sample furthest from the centre it sits in.</summary>
    private static void Relocate(
        ReadOnlySpan<double> samples,
        int featureCount,
        double[] centres,
        int[] labels,
        double[] totals,
        int[] counts)
    {
        for (int cluster = 0; cluster < counts.Length; cluster++)
        {
            if (counts[cluster] != 0)
            {
                continue;
            }

            int furthest = Furthest(samples, featureCount, centres, labels);
            int donor = labels[furthest];
            labels[furthest] = cluster;
            counts[donor]--;
            counts[cluster]++;

            for (int feature = 0; feature < featureCount; feature++)
            {
                double value = samples[(furthest * featureCount) + feature];
                totals[(donor * featureCount) + feature] -= value;
                totals[(cluster * featureCount) + feature] += value;
            }
        }
    }

    /// <summary>The sample sitting furthest from its own centre, among clusters that can spare one.</summary>
    private static int Furthest(
        ReadOnlySpan<double> samples, int featureCount, double[] centres, int[] labels)
    {
        double worst = -1.0;
        int chosen = 0;

        for (int row = 0; row < labels.Length; row++)
        {
            double distance = SquaredDistance(
                samples, row * featureCount, centres, labels[row] * featureCount, featureCount);
            if (distance > worst)
            {
                worst = distance;
                chosen = row;
            }
        }

        return chosen;
    }

    /// <summary>Summed squared distance from every sample to the centre it was assigned.</summary>
    private static double Inertias(
        ReadOnlySpan<double> samples, int featureCount, double[] centres, int[] labels)
    {
        double total = 0.0;
        for (int row = 0; row < labels.Length; row++)
        {
            total += SquaredDistance(
                samples, row * featureCount, centres, labels[row] * featureCount, featureCount);
        }

        return total;
    }

    private static double SquaredDistance(
        ReadOnlySpan<double> samples, int sampleOffset, double[] centres, int centreOffset, int featureCount)
    {
        double total = 0.0;
        for (int feature = 0; feature < featureCount; feature++)
        {
            double gap = samples[sampleOffset + feature] - centres[centreOffset + feature];
            total += gap * gap;
        }

        return total;
    }

    private static bool SameLabels(int[] left, int[] right)
    {
        for (int i = 0; i < left.Length; i++)
        {
            if (left[i] != right[i])
            {
                return false;
            }
        }

        return true;
    }

    /// <summary>k-means++ over this package's own generator — reproducible here, not portable.</summary>
    private static double[] PlusPlus(
        ReadOnlySpan<double> samples, int featureCount, int clusterCount, int sampleCount, int seed)
    {
        var centres = new double[clusterCount * featureCount];
        var random = new SplitMix64((ulong)seed);

        int first = (int)(random.NextDouble() * sampleCount);
        first = Math.Min(first, sampleCount - 1);
        Copy(samples, first * featureCount, centres, 0, featureCount);

        var closest = new double[sampleCount];
        for (int row = 0; row < sampleCount; row++)
        {
            closest[row] = SquaredDistance(samples, row * featureCount, centres, 0, featureCount);
        }

        for (int cluster = 1; cluster < clusterCount; cluster++)
        {
            double total = 0.0;
            foreach (double distance in closest)
            {
                total += distance;
            }

            int chosen = Weighted(closest, total, random.NextDouble(), sampleCount);
            Copy(samples, chosen * featureCount, centres, cluster * featureCount, featureCount);

            for (int row = 0; row < sampleCount; row++)
            {
                double distance = SquaredDistance(
                    samples, row * featureCount, centres, cluster * featureCount, featureCount);
                if (distance < closest[row])
                {
                    closest[row] = distance;
                }
            }
        }

        return centres;
    }

    /// <summary>Picks a sample with probability proportional to its distance from what is chosen.</summary>
    private static int Weighted(double[] weights, double total, double draw, int sampleCount)
    {
        if (total <= 0.0)
        {
            return Math.Min((int)(draw * sampleCount), sampleCount - 1);
        }

        double target = draw * total;
        double running = 0.0;
        for (int row = 0; row < sampleCount; row++)
        {
            running += weights[row];
            if (running >= target)
            {
                return row;
            }
        }

        return sampleCount - 1;
    }

    private static void Copy(
        ReadOnlySpan<double> source, int sourceOffset, double[] destination, int destinationOffset, int length)
    {
        for (int i = 0; i < length; i++)
        {
            destination[destinationOffset + i] = source[sourceOffset + i];
        }
    }
}
