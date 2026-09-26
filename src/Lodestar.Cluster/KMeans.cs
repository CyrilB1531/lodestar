using System.Numerics;
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
    /// <exception cref="ArgumentOutOfRangeException"><paramref name="featureCount"/> or <paramref name="clusterCount"/> is not positive, or <paramref name="options"/> asks for fewer than one iteration or a tolerance that is negative, infinite or not a number.</exception>
    /// <exception cref="ArgumentException"><paramref name="samples"/> holds no row, a partial one or a value that is not finite, there are fewer rows than clusters, or the given initial centres are the wrong shape or not finite.</exception>
    /// <remarks>
    /// The loop is the reference's: assign, update, stop on unchanged labels or on a centre
    /// shift within the scaled tolerance — and when it stops on the shift, a final assignment
    /// runs so <see cref="Labels"/> matches <see cref="Centres"/>. An empty cluster is
    /// relocated onto a distinct one of the samples furthest from their centres, as the reference
    /// relocates it. The reference page carries the tolerance scaling and the tie divergences.
    /// </remarks>
    public static KMeans Fit(
        ReadOnlySpan<double> samples, int featureCount, int clusterCount, KMeansOptions? options = null) =>
        FitCore(samples, null, featureCount, clusterCount, options);

    /// <summary>Fits k-means with one weight per sample — scikit-learn's <c>fit(X, sample_weight=w)</c>.</summary>
    /// <param name="samples">The samples, row-major: <paramref name="featureCount"/> values per row.</param>
    /// <param name="sampleWeights">One finite weight per sample, not all zero; negative ones are allowed, as scikit-learn allows them.</param>
    /// <param name="featureCount">How many values each row carries.</param>
    /// <param name="clusterCount">How many clusters to find.</param>
    /// <param name="options">Where to start and when to stop; <see langword="null"/> takes the defaults.</param>
    /// <returns>A fitted clustering whose <see cref="Inertia"/> is weighted.</returns>
    /// <exception cref="ArgumentOutOfRangeException">As <see cref="Fit(ReadOnlySpan{double}, int, int, KMeansOptions)"/>.</exception>
    /// <exception cref="ArgumentException">As <see cref="Fit(ReadOnlySpan{double}, int, int, KMeansOptions)"/>, or the weights are not one finite value per sample, or are all zero.</exception>
    /// <remarks>
    /// A sample standing for several identical rows weighs their count, and the fit is the one the full matrix gives.
    /// Each centre is its members' weighted mean, the inertia is weighted, a cluster whose members weigh nothing is
    /// relocated, and k-means++ draws in proportion to weight; the tolerance's scaling stays unweighted, as the
    /// reference's does.
    /// </remarks>
    public static KMeans Fit(
        ReadOnlySpan<double> samples,
        ReadOnlySpan<double> sampleWeights,
        int featureCount,
        int clusterCount,
        KMeansOptions? options = null)
    {
        Guard.NotLessThan(featureCount, 1);
        int rows = Rows(samples, featureCount);
        if (sampleWeights.Length != rows)
        {
            throw new ArgumentException(
                $"sampleWeights holds {sampleWeights.Length} values, not one per row of samples.", nameof(sampleWeights));
        }

        Finite.Require(sampleWeights, nameof(sampleWeights));
        if (!ContainsNonZero(sampleWeights))
        {
            throw new ArgumentException("The sample weights are all zero, which leaves nothing to cluster.", nameof(sampleWeights));
        }

        return FitCore(samples, sampleWeights.ToArray(), featureCount, clusterCount, options);
    }

    private static KMeans FitCore(
        ReadOnlySpan<double> samples, double[]? weights, int featureCount, int clusterCount, KMeansOptions? options)
    {
        Guard.NotLessThan(featureCount, 1);
        Guard.NotLessThan(clusterCount, 1);
        KMeansOptions settings = options ?? new KMeansOptions();
        Guard.NotLessThan(settings.MaxIterations, 1);
        CheckSettings(settings, nameof(options));

        int sampleCount = Rows(samples, featureCount);
        Finite.Require(samples, nameof(samples));
        if (sampleCount < clusterCount)
        {
            throw new ArgumentException(
                $"samples holds {sampleCount} rows, fewer than the {clusterCount} clusters asked for.",
                nameof(clusterCount));
        }

        double[] weighting = weights ?? Ones(sampleCount);
        double tolerance = ScaledTolerance(samples, featureCount, sampleCount, settings.Tolerance);
        Run? best = null;
        foreach (double[] start in Starts(samples, weights, featureCount, clusterCount, sampleCount, settings))
        {
            Run run = Once(samples, weighting, featureCount, clusterCount, start, settings.MaxIterations, tolerance);

            // scikit-learn's rule: the first fit is kept, and a later one only with a strictly lower inertia and a
            // partition that differs, since a rounding-level gain on the same partition is no gain.
            if (best is null || (run.Inertia < best.Inertia && !SameClustering(run.Labels, best.Labels, clusterCount)))
            {
                best = run;
            }
        }

        return new KMeans(clusterCount, featureCount, best!.Centres, best.Labels, best.Inertia, best.Iterations);
    }

    /// <summary>One Lloyd run from one starting block.</summary>
    private sealed record Run(double[] Centres, int[] Labels, double Inertia, int Iterations);

    /// <remarks>
    /// The loop is the reference's: assign, update, stop on unchanged labels or on a centre shift within the scaled
    /// tolerance — and when it stops on the shift, a final assignment runs so the labels match the centres.
    /// </remarks>
    private static Run Once(
        ReadOnlySpan<double> samples,
        double[] weights,
        int featureCount,
        int clusterCount,
        double[] centres,
        int maxIterations,
        double tolerance)
    {
        int sampleCount = weights.Length;
        var labels = new int[sampleCount];
        var previous = new int[sampleCount];
        for (int row = 0; row < sampleCount; row++)
        {
            // -1 is not a cluster, so the first comparison can never report convergence.
            previous[row] = -1;
        }

        bool strict = false;
        int iterations = 0;
        for (int step = 0; step < maxIterations; step++)
        {
            iterations = step + 1;
            Nearest(samples, featureCount, centres, clusterCount, labels);
            double shift = Update(samples, weights, featureCount, clusterCount, labels, centres);

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

        return new Run(centres, labels, Inertias(samples, weights, featureCount, centres, labels), iterations);
    }

    private static void CheckSettings(KMeansOptions settings, string parameterName)
    {
        // `!(>= 0)` so a NaN is refused rather than silently disabling the shift test; infinity is
        // refused as the reference refuses it, whose range for tol is [0, inf).
        if (!(settings.Tolerance >= 0.0) || double.IsPositiveInfinity(settings.Tolerance))
        {
            throw new ArgumentOutOfRangeException(
                parameterName, settings.Tolerance, "Tolerance must be finite and zero or greater.");
        }

        if (settings.Restarts < 1)
        {
            throw new ArgumentOutOfRangeException(parameterName, settings.Restarts, "Restarts counts fits, at least one.");
        }

        if (settings.InitialCentreSets is { } sets
            && (settings.InitialCentres is not null || settings.Restarts != 1 || sets.Count == 0))
        {
            throw new ArgumentException(
                "InitialCentreSets holds one or more starts on its own, without InitialCentres or Restarts.", parameterName);
        }

        if (settings.InitialCentres is not null && settings.Restarts != 1)
        {
            throw new ArgumentException("Restarts is read when the fit chooses its starts, not beside InitialCentres.", parameterName);
        }
    }

    /// <summary>The starting blocks: the caller's, each checked, or k-means++ from successive seeds.</summary>
    private static List<double[]> Starts(
        ReadOnlySpan<double> samples, double[]? weights, int featureCount, int clusterCount, int sampleCount, KMeansOptions settings)
    {
        var starts = new List<double[]>();
        if (settings.InitialCentreSets is { } sets)
        {
            starts.AddRange(sets.Select(set => Checked(set, featureCount, clusterCount, nameof(KMeansOptions.InitialCentreSets))));
        }
        else if (settings.InitialCentres is { } given)
        {
            starts.Add(Checked(given, featureCount, clusterCount, nameof(KMeansOptions.InitialCentres)));
        }
        else
        {
            for (int restart = 0; restart < settings.Restarts; restart++)
            {
                starts.Add(PlusPlus(samples, weights, featureCount, clusterCount, sampleCount, unchecked(settings.Seed + restart)));
            }
        }

        return starts;
    }

    private static double[] Checked(double[]? block, int featureCount, int clusterCount, string name)
    {
        if (block is null || block.Length != clusterCount * featureCount)
        {
            throw new ArgumentException(
                $"{name} holds a block of {block?.Length ?? 0} values, not the "
                + $"{clusterCount} x {featureCount} a starting block needs.",
                name);
        }

        Finite.Require(block, name);
        return (double[])block.Clone();
    }

    /// <summary>scikit-learn's <c>_is_same_clustering</c>: the first partition maps onto the second, label by label.</summary>
    private static bool SameClustering(int[] left, int[] right, int clusterCount)
    {
        var mapping = new int[clusterCount];
        for (int cluster = 0; cluster < clusterCount; cluster++)
        {
            mapping[cluster] = -1;
        }

        for (int row = 0; row < left.Length; row++)
        {
            if (mapping[left[row]] == -1)
            {
                mapping[left[row]] = right[row];
            }
            else if (mapping[left[row]] != right[row])
            {
                return false;
            }
        }

        return true;
    }

    private static bool ContainsNonZero(ReadOnlySpan<double> weights)
    {
        foreach (double weight in weights)
        {
            // S1244: the reference refuses weights that are all exactly zero, and so does this.
#pragma warning disable S1244
            if (weight != 0.0)
#pragma warning restore S1244
            {
                return true;
            }
        }

        return false;
    }

    private static double[] Ones(int count)
    {
        var ones = new double[count];
        for (int row = 0; row < count; row++)
        {
            ones[row] = 1.0;
        }

        return ones;
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
        if (Vector.IsHardwareAccelerated && clusterCount >= Vector<double>.Count)
        {
            AssignLanes(samples, featureCount, centres, clusterCount, labels);
        }
        else if (featureCount > NarrowFeatures)
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
    /// on an exact tie — decision 0007 has the two configurations that send scikit-learn's
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

    /// <summary><see cref="Assign"/> with one centre per vector lane, over the centres transposed.</summary>
    /// <remarks>
    /// Each lane sums its own centre's squared gaps in feature order, the scalar loop's order, and the JIT
    /// fuses no multiply into an add, so every distance and every label is the scalar one to the bit. It
    /// is the whole E-step's cost: 16 centres of 8 features took 5.4 ms an iteration at 100,000 rows scalar.
    /// </remarks>
    [MethodImpl(MethodImplOptions.NoInlining)]
    private static void AssignLanes(
        ReadOnlySpan<double> samples, int featureCount, double[] centres, int clusterCount, int[] labels)
    {
        int lanes = Vector<double>.Count;
        int vectorised = clusterCount - (clusterCount % lanes);
        var transposed = new double[featureCount * clusterCount];
        for (int cluster = 0; cluster < clusterCount; cluster++)
        {
            for (int feature = 0; feature < featureCount; feature++)
            {
                transposed[(feature * clusterCount) + cluster] = centres[(cluster * featureCount) + feature];
            }
        }

        var distances = new double[clusterCount];
        for (int row = 0; row < labels.Length; row++)
        {
            ReadOnlySpan<double> sample = samples.Slice(row * featureCount, featureCount);
            for (int first = 0; first < vectorised; first += lanes)
            {
                Vector<double> total = Vector<double>.Zero;
                for (int feature = 0; feature < featureCount; feature++)
                {
                    Vector<double> gap = new Vector<double>(sample[feature])
                        - new Vector<double>(transposed, (feature * clusterCount) + first);
                    total += gap * gap;
                }

                total.CopyTo(distances, first);
            }

            for (int cluster = vectorised; cluster < clusterCount; cluster++)
            {
                distances[cluster] = RowDistance(sample, centres.AsSpan(cluster * featureCount, featureCount));
            }

            labels[row] = Smallest(distances);
        }
    }

    /// <summary>The index of the smallest distance, ties to the lower index as <see cref="Assign"/> breaks them.</summary>
    private static int Smallest(double[] distances)
    {
        double best = double.PositiveInfinity;
        int chosen = 0;
        for (int cluster = 0; cluster < distances.Length; cluster++)
        {
            if (distances[cluster] < best)
            {
                best = distances[cluster];
                chosen = cluster;
            }
        }

        return chosen;
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
    /// <remarks>Each cluster's weighted sum and weight, as <c>_update_chunk_dense</c> accumulates them; all ones is the unweighted fit.</remarks>
    private static double Update(
        ReadOnlySpan<double> samples,
        double[] weights,
        int featureCount,
        int clusterCount,
        int[] labels,
        double[] centres)
    {
        var totals = new double[clusterCount * featureCount];
        var mass = new double[clusterCount];

        for (int row = 0; row < weights.Length; row++)
        {
            int cluster = labels[row];
            double weight = weights[row];
            mass[cluster] += weight;
            for (int feature = 0; feature < featureCount; feature++)
            {
                totals[(cluster * featureCount) + feature] += samples[(row * featureCount) + feature] * weight;
            }
        }

        Relocate(samples, weights, featureCount, centres, labels, totals, mass);
        Average(samples, featureCount, totals, mass);

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
    /// <c>_relocate_empty_clusters_dense</c>'s shape: the labels are left alone, the moved sample is
    /// subtracted from its old cluster's sums, and nothing moves when every sample sits on its centre.
    /// The furthest samples are taken in descending distance, ties lowest row first; the reference's
    /// <c>numpy.argpartition</c> varies with numpy's CPU tier, so no row rule matches it (#990, #1043).
    /// </remarks>
    private static void Relocate(
        ReadOnlySpan<double> samples,
        double[] weights,
        int featureCount,
        double[] centres,
        int[] labels,
        double[] totals,
        double[] mass)
    {
        // Listed before any move, as np.where(weight_in_clusters == 0) is: a donor emptied by a
        // relocation stays empty this iteration rather than taking a row past the end (#975).
        int[]? empty = null;
        int emptyCount = 0;
        for (int cluster = 0; cluster < mass.Length; cluster++)
        {
            // S1244: np.equal(weight_in_clusters, 0) — a cluster whose members weigh exactly nothing is empty.
#pragma warning disable S1244
            if (mass[cluster] == 0.0)
#pragma warning restore S1244
            {
                empty ??= new int[mass.Length - cluster];
                empty[emptyCount++] = cluster;
            }
        }

        if (empty is null)
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

        int[] furthest = Furthest(distances, emptyCount);
        for (int index = 0; index < emptyCount; index++)
        {
            int cluster = empty[index];
            int row = furthest[index];
            int donor = labels[row];
            double weight = weights[row];
            mass[donor] -= weight;
            mass[cluster] = weight;

            for (int feature = 0; feature < featureCount; feature++)
            {
                double value = samples[(row * featureCount) + feature] * weight;
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
    private static void Average(ReadOnlySpan<double> samples, int featureCount, double[] totals, double[] mass)
    {
        int largest = 0;
        for (int cluster = 1; cluster < mass.Length; cluster++)
        {
            if (mass[cluster] > mass[largest])
            {
                largest = cluster;
            }
        }

        double[]? means = null;
        for (int cluster = 0; cluster < mass.Length; cluster++)
        {
            int offset = cluster * featureCount;
            if (mass[cluster] > 0.0)
            {
                for (int feature = 0; feature < featureCount; feature++)
                {
                    totals[offset + feature] /= mass[cluster];
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
                    totals[(largest * featureCount) + feature] - ((mass[largest] - 1.0) * means[feature]);
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
        ReadOnlySpan<double> samples, double[] weights, int featureCount, double[] centres, int[] labels)
    {
        double total = 0.0;
        for (int row = 0; row < labels.Length; row++)
        {
            total += weights[row] * SquaredDistance(
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
    /// <remarks>
    /// With weights the draws are in proportion to weight, then to weight times distance, as the reference's are; a
    /// negative weight takes no share of a draw. Without them the draws are the ones this package has always made.
    /// </remarks>
    private static double[] PlusPlus(
        ReadOnlySpan<double> samples, double[]? weights, int featureCount, int clusterCount, int sampleCount, int seed)
    {
        var centres = new double[clusterCount * featureCount];
        var random = new SplitMix64((ulong)seed);

        int first;
        if (weights is null)
        {
            first = Math.Min((int)(random.NextDouble() * sampleCount), sampleCount - 1);
        }
        else
        {
            double[] shares = [.. weights.Select(weight => Math.Max(0.0, weight))];
            first = Weighted(shares, shares.Sum(), random.NextDouble(), sampleCount);
        }

        Copy(samples, first * featureCount, centres, 0, featureCount);

        var closest = new double[sampleCount];
        for (int row = 0; row < sampleCount; row++)
        {
            closest[row] = SquaredDistance(samples, row * featureCount, centres, 0, featureCount);
        }

        for (int cluster = 1; cluster < clusterCount; cluster++)
        {
            double[] shares = weights is null
                ? closest
                : [.. closest.Select((distance, row) => distance * Math.Max(0.0, weights[row]))];
            double total = 0.0;
            foreach (double share in shares)
            {
                total += share;
            }

            int chosen = Weighted(shares, total, random.NextDouble(), sampleCount);
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
