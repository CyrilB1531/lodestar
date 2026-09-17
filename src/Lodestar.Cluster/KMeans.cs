using System.Runtime.CompilerServices;
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
    /// <summary>The widest row the indexed assignment keeps; see <see cref="AssignWide"/> for why.</summary>
    private const int NarrowFeatures = 4;

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
    /// <exception cref="ArgumentException"><paramref name="samples"/> holds no row, a partial one or a value that is not finite, there are fewer rows than clusters, or the given initial centres are the wrong shape or not finite.</exception>
    /// <remarks>
    /// The loop is the reference's: assign, update, stop on unchanged labels or on a centre
    /// shift within the scaled tolerance — and when it stops on the shift, a final assignment
    /// runs so <see cref="Labels"/> matches <see cref="Centres"/>. An empty cluster is
    /// relocated onto a distinct one of the samples furthest from their centres, as the reference
    /// relocates it. The reference page carries the tolerance scaling and the tie divergences.
    /// </remarks>
    public static KMeans Fit(
        ReadOnlySpan<double> samples, int featureCount, int clusterCount, KMeansOptions? options = null)
    {
        Guard.NotLessThan(featureCount, 1);
        Guard.NotLessThan(clusterCount, 1);
        KMeansOptions settings = options ?? new KMeansOptions();
        Guard.NotLessThan(settings.MaxIterations, 1);

        int sampleCount = Rows(samples, featureCount);
        Finite.Require(samples, nameof(samples));
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
            Nearest(samples, featureCount, centres, clusterCount, labels);
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
            Nearest(samples, featureCount, centres, clusterCount, labels);
        }

        double inertia = Inertias(samples, featureCount, centres, labels);
        return new KMeans(clusterCount, featureCount, centres, labels, inertia, iterations);
    }

    /// <summary>Assigns unseen samples to the fitted centres.</summary>
    /// <param name="samples">The samples, row-major, with <see cref="FeatureCount"/> values per row.</param>
    /// <returns>One cluster index per row.</returns>
    /// <exception cref="ArgumentException"><paramref name="samples"/> holds no row, a partial one, or a value that is not finite.</exception>
    /// <remarks>Nothing is refitted: this is the assignment step alone, over the centres already found.</remarks>
    public int[] Predict(ReadOnlySpan<double> samples)
    {
        int rows = Rows(samples, FeatureCount);
        Finite.Require(samples, nameof(samples));
        var labels = new int[rows];
        Nearest(samples, FeatureCount, _centres, ClusterCount, labels);
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

            Finite.Require(settings.InitialCentres, nameof(KMeansOptions.InitialCentres));
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

    /// <summary>The E-step, by whichever of the two equal loops is faster at this width.</summary>
    /// <remarks>
    /// Both loops are kept out of line: the version that branched inside the indexed loop measured
    /// 17.2 ms a Lloyd run at two features against main's 16.1, and this shape measured 16.3 against 16.2.
    /// </remarks>
    private static void Nearest(
        ReadOnlySpan<double> samples, int featureCount, double[] centres, int clusterCount, int[] labels)
    {
        if (featureCount > NarrowFeatures)
        {
            AssignWide(samples, featureCount, centres, clusterCount, labels);
        }
        else
        {
            Assign(samples, featureCount, centres, clusterCount, labels);
        }
    }

    /// <summary>The E-step: every sample takes the nearest centre, ties to the lower index.</summary>
    /// <remarks>
    /// <c>numpy.argmin</c>'s rule, and <strong>a measured divergence from the reference</strong>
    /// on an exact tie — decision 0093 has the two configurations that send scikit-learn's
    /// choice both ways, and why no single rule reproduces both.
    /// </remarks>
    [MethodImpl(MethodImplOptions.NoInlining)]
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

    /// <summary><see cref="Assign"/> over sliced rows, the same sum in the same order.</summary>
    /// <remarks>
    /// Measured by Stopwatch on the incumbent benchmark's blobs: slicing each row and centre lets the
    /// JIT drop the bounds checks, 0.99 ms an assignment against 1.28 at sixteen features, but at two
    /// it was the slower of the two. A partial-sum exit was slower at every shape: 0.26, 1.83 and 15.4 ms
    /// against 0.19, 1.28 and 6.8, its branch mispredicting on overlapping blobs.
    /// </remarks>
    [MethodImpl(MethodImplOptions.NoInlining)]
    private static void AssignWide(
        ReadOnlySpan<double> samples, int featureCount, double[] centres, int clusterCount, int[] labels)
    {
        for (int row = 0; row < labels.Length; row++)
        {
            ReadOnlySpan<double> sample = samples.Slice(row * featureCount, featureCount);
            double best = double.PositiveInfinity;
            int chosen = 0;

            for (int cluster = 0; cluster < clusterCount; cluster++)
            {
                double distance = RowDistance(sample, centres.AsSpan(cluster * featureCount, featureCount));
                if (distance < best)
                {
                    best = distance;
                    chosen = cluster;
                }
            }

            labels[row] = chosen;
        }
    }

    private static double RowDistance(ReadOnlySpan<double> sample, ReadOnlySpan<double> centre)
    {
        double total = 0.0;
        for (int feature = 0; feature < sample.Length; feature++)
        {
            double gap = sample[feature] - centre[feature];
            total += gap * gap;
        }

        return total;
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
        Average(samples, featureCount, totals, counts);

        double shift = 0.0;
        for (int index = 0; index < centres.Length; index++)
        {
            double moved = totals[index] - centres[index];
            shift += moved * moved;
            centres[index] = totals[index];
        }

        return shift;
    }

    /// <summary>
    /// Moves each empty cluster onto one of the samples furthest from their own centres, all
    /// taken from one distance pass so no sample is given twice.
    /// </summary>
    /// <remarks>
    /// <c>_relocate_empty_clusters_dense</c>'s shape: the labels are left alone, the moved sample
    /// is subtracted from its old cluster's sums, and nothing moves when every sample sits on its
    /// centre. Equally far samples are taken lowest row first, where the reference's order comes
    /// from <c>numpy.argpartition</c> and follows no rule a row index reproduces.
    /// </remarks>
    private static void Relocate(
        ReadOnlySpan<double> samples,
        int featureCount,
        double[] centres,
        int[] labels,
        double[] totals,
        int[] counts)
    {
        int empty = counts.Count(count => count == 0);
        if (empty == 0)
        {
            return;
        }

        var distances = new double[labels.Length];
        double worst = 0.0;
        for (int row = 0; row < labels.Length; row++)
        {
            distances[row] = SquaredDistance(
                samples, row * featureCount, centres, labels[row] * featureCount, featureCount);
            worst = Math.Max(worst, distances[row]);
        }

        if (!(worst > 0.0))
        {
            return;
        }

        int[] furthest = Furthest(distances, empty);
        int next = 0;
        for (int cluster = 0; cluster < counts.Length; cluster++)
        {
            if (counts[cluster] != 0)
            {
                continue;
            }

            int row = furthest[next++];
            int donor = labels[row];
            counts[donor]--;
            counts[cluster] = 1;

            for (int feature = 0; feature < featureCount; feature++)
            {
                double value = samples[(row * featureCount) + feature];
                totals[(donor * featureCount) + feature] -= value;
                totals[(cluster * featureCount) + feature] = value;
            }
        }
    }

    /// <summary>The <paramref name="count"/> rows with the largest distances, furthest first.</summary>
    private static int[] Furthest(double[] distances, int count)
    {
        var rows = new int[distances.Length];
        for (int row = 0; row < rows.Length; row++)
        {
            rows[row] = row;
        }

        Array.Sort(rows, (left, right) =>
        {
            int order = distances[right].CompareTo(distances[left]);
            return order != 0 ? order : left.CompareTo(right);
        });

        var chosen = new int[count];
        Array.Copy(rows, chosen, count);
        return chosen;
    }

    /// <summary>Turns the sums into means, in place, as <c>_average_centers</c> does.</summary>
    /// <remarks>
    /// A cluster still empty takes the largest cluster's row as the loop has left it, so a later
    /// largest cluster lends its sum rather than its mean. The reference sums centred samples, so
    /// that sum is rebuilt here as it holds it: minus the count times the column mean, plus the
    /// mean added back when the fit ends.
    /// </remarks>
    private static void Average(ReadOnlySpan<double> samples, int featureCount, double[] totals, int[] counts)
    {
        int largest = 0;
        for (int cluster = 1; cluster < counts.Length; cluster++)
        {
            if (counts[cluster] > counts[largest])
            {
                largest = cluster;
            }
        }

        double[]? means = null;
        for (int cluster = 0; cluster < counts.Length; cluster++)
        {
            int offset = cluster * featureCount;
            if (counts[cluster] > 0)
            {
                for (int feature = 0; feature < featureCount; feature++)
                {
                    totals[offset + feature] /= counts[cluster];
                }

                continue;
            }

            if (largest < cluster)
            {
                Array.Copy(totals, largest * featureCount, totals, offset, featureCount);
                continue;
            }

            means ??= ColumnMeans(samples, featureCount);
            for (int feature = 0; feature < featureCount; feature++)
            {
                totals[offset + feature] =
                    totals[(largest * featureCount) + feature] - ((counts[largest] - 1) * means[feature]);
            }
        }
    }

    private static double[] ColumnMeans(ReadOnlySpan<double> samples, int featureCount)
    {
        var means = new double[featureCount];
        int rows = samples.Length / featureCount;
        for (int row = 0; row < rows; row++)
        {
            for (int feature = 0; feature < featureCount; feature++)
            {
                means[feature] += samples[(row * featureCount) + feature];
            }
        }

        for (int feature = 0; feature < featureCount; feature++)
        {
            means[feature] /= rows;
        }

        return means;
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
