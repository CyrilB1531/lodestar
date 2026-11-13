using Lodestar.Stats.Regression.Panel;

namespace Lodestar.Stats.Regression.Internal;

/// <summary>What a panel covariance reads besides the regression: its type, its scaling, its clusters and its kernel.</summary>
/// <param name="Type">The covariance.</param>
/// <param name="Debiased">Whether the coefficients are counted out of the effective observations.</param>
/// <param name="ExtraDegreesOfFreedom">The absorbed effects counted out of them too, for fixed effects.</param>
/// <param name="Clusters">Zero, one or two cluster columns, one label per regression row.</param>
/// <param name="Periods">Each regression row's period, which Driscoll-Kraay sums the scores by.</param>
/// <param name="Kernel">Driscoll-Kraay's kernel.</param>
/// <param name="Bandwidth">Its bandwidth, or <see langword="null"/> for the default.</param>
internal readonly record struct PanelCovarianceSpec(
    PanelCovarianceType Type,
    bool Debiased,
    int ExtraDegreesOfFreedom,
    IReadOnlyList<int[]> Clusters,
    int[] Periods,
    KernelType Kernel,
    int? Bandwidth);

/// <summary><c>linearmodels.panel.covariance</c>'s estimators over a regression's rows.</summary>
internal static class PanelCovariance
{
    /// <summary>The coefficients' covariance, the bandwidth a kernel covariance used, and the most rank it can hold.</summary>
    /// <remarks>
    /// Every estimator scales by <c>n / (n − extra − k)</c> when debiased and <c>n / (n − extra)</c> otherwise. The clustered
    /// one takes no <c>G/(G−1)</c> factor, and two ways are <c>S₀ + S₁ − S₀₁</c>; Driscoll-Kraay sums the scores by period,
    /// applies the kernel over periods and scales by <c>T/n</c>.
    /// </remarks>
    /// <exception cref="ArgumentException">A Bartlett or Parzen bandwidth reaches past the periods, which the reference refuses.</exception>
    public static (double[] Covariance, int? Bandwidth, int LargestRank) Estimate(
        double[] x, int k, double[] residuals, PanelCovarianceSpec spec)
    {
        int n = residuals.Length;
        double effective = n - spec.ExtraDegreesOfFreedom - (spec.Debiased ? k : 0);
        double scale = n / effective;
        double[] xx = Dense.Gram(x, k, n);
        if (spec.Type == PanelCovarianceType.Unadjusted)
        {
            double rss = 0.0;
            foreach (double e in residuals)
            {
                rss += e * e;
            }

            double[] unadjusted = IvCore.Invert(xx, k);
            Scale(unadjusted, scale * rss / n);
            Dense.Symmetrise(unadjusted, k);
            return (unadjusted, null, k);
        }

        double[] scores = Scores(x, k, residuals);
        (double[] meat, int? bandwidth, int largestRank) = Meat(scores, k, n, spec);
        Scale(meat, scale);
        Scale(xx, 1.0 / n);
        double[] bread = IvCore.Invert(xx, k);
        double[] covariance = Dense.Multiply(Dense.Multiply(bread, meat, k, k, k), bread, k, k, k);
        Scale(covariance, 1.0 / n);
        Dense.Symmetrise(covariance, k);
        return (covariance, bandwidth, largestRank);
    }

    /// <summary>The meat, its bandwidth, and how many directions its score sums span at most (see <see cref="Estimate"/>).</summary>
    /// <remarks>
    /// Least squares leaves <c>Xᵀe = 0</c>, so <c>G</c> cluster sums that are not zero span at most <c>G − 1</c> directions,
    /// two ways what <see cref="TwoWayRank"/> counts, and <c>T</c> period sums <c>T − 1</c>; robust scores, the rows' own.
    /// </remarks>
    private static (double[] Meat, int? Bandwidth, int LargestRank) Meat(double[] scores, int k, int n, PanelCovarianceSpec spec)
    {
        switch (spec.Type)
        {
            case PanelCovarianceType.Robust:
                (double[] robust, int rows) = IvScores.RobustSpanned(scores, k, n);
                return (robust, null, Math.Max(0, rows - 1));
            case PanelCovarianceType.Clustered:
                (double[] meat, int rank) = ClusteredMeat(scores, k, n, spec.Clusters);
                return (meat, null, rank);
            default:
                return DriscollKraay(scores, k, n, spec);
        }
    }

    private static (double[] Meat, int LargestRank) ClusteredMeat(double[] scores, int k, int n, IReadOnlyList<int[]> clusters)
    {
        if (clusters.Count == 0)
        {
            // No cluster named: each row its own, as the reference defaults to.
            (double[] robust, int rows) = IvScores.RobustSpanned(scores, k, n);
            return (robust, Math.Max(0, rows - 1));
        }

        ClusterLabels one = ClusterLabels.Dense(clusters[0], n);
        if (clusters.Count == 1)
        {
            (double[] meat, int spanning) = IvScores.ClusteredSpanned(scores, k, n, one);
            return (meat, Math.Max(0, spanning - 1));
        }

        ClusterLabels other = ClusterLabels.Dense(clusters[1], n);
        ClusterLabels intersection = ClusterLabels.Dense(Union(clusters[0], clusters[1]), n);
        double[] first = IvScores.Clustered(scores, k, n, one);
        double[] second = IvScores.Clustered(scores, k, n, other);
        double[] cellSums = IvScores.Sums(scores, k, n, intersection);
        double[] both = IvScores.Outer(cellSums, k, intersection.Count, n);
        for (int i = 0; i < first.Length; i++)
        {
            first[i] += second[i] - both[i];
        }

        return (first, TwoWayRank(one, other, intersection, cellSums, k));
    }

    /// <summary>The most directions <c>S₀ + S₁ − S₀₁</c> spans, its cell sums totalling zero.</summary>
    /// <remarks>
    /// A cell whose sum is zero adds nothing to any of the three, so only the others are counted. Every term is spanned
    /// by the cell sums, so at most <c>G₀₁ − 1</c>. Grouped by one clustering's clusters, the sum is
    /// the other's meat plus, per cluster of several cells, the cross terms of those cells, and a cluster of one cell adds
    /// nothing: one clustering nested in the other spans what the coarser spans, which rounding of the cancelled terms
    /// would otherwise lift past it.
    /// </remarks>
    private static int TwoWayRank(ClusterLabels one, ClusterLabels other, ClusterLabels cells, double[] cellSums, int k)
    {
        var cellsInOne = new int[one.Count];
        var cellsInOther = new int[other.Count];
        var counted = new bool[cells.Count];
        int live = 0;
        for (int row = 0; row < cells.Labels.Length; row++)
        {
            int cell = cells.Labels[row];
            if (!counted[cell])
            {
                counted[cell] = true;
                if (IvScores.Spanning(cellSums.AsSpan(cell * k, k), k, 1) == 1)
                {
                    live++;
                    cellsInOne[one.Labels[row]]++;
                    cellsInOther[other.Labels[row]]++;
                }
            }
        }

        int groupedByOne = cellsInOther.Count(count => count > 0) - 1 + cellsInOne.Where(count => count > 1).Sum();
        int groupedByOther = cellsInOne.Count(count => count > 0) - 1 + cellsInOther.Where(count => count > 1).Sum();
        return Math.Max(0, Math.Min(live - 1, Math.Min(groupedByOne, groupedByOther)));
    }

    private static (double[] Meat, int? Bandwidth, int LargestRank) DriscollKraay(double[] scores, int k, int n, PanelCovarianceSpec spec)
    {
        // The scores summed by period, the periods in ascending order.
        int[] present = [.. spec.Periods.Distinct().OrderBy(p => p)];
        var slot = new Dictionary<int, int>();
        for (int i = 0; i < present.Length; i++)
        {
            slot[present[i]] = i;
        }

        int periods = present.Length;
        var summed = new double[periods * k];
        for (int row = 0; row < n; row++)
        {
            int to = slot[spec.Periods[row]] * k;
            for (int j = 0; j < k; j++)
            {
                summed[to + j] += scores[(row * k) + j];
            }
        }

        int bandwidth = spec.Bandwidth ?? (int)Math.Floor(4.0 * Math.Pow(periods / 100.0, 2.0 / 9.0));

        double[] meat = IvScores.Kernel(summed, k, periods, IvScores.KernelWeights(spec.Kernel, bandwidth, periods - 1));
        Scale(meat, periods / (double)n);
        return (meat, bandwidth, Math.Max(0, IvScores.Spanning(summed, k, periods) - 1));
    }

    /// <summary>One label per distinct pair of the two columns: the intersection two-way clustering subtracts.</summary>
    private static int[] Union(int[] first, int[] second)
    {
        var pairs = new Dictionary<(int, int), int>();
        var labels = new int[first.Length];
        for (int row = 0; row < first.Length; row++)
        {
            (int, int) key = (first[row], second[row]);
            if (!pairs.TryGetValue(key, out int label))
            {
                label = pairs.Count;
                pairs.Add(key, label);
            }

            labels[row] = label;
        }

        return labels;
    }

    private static double[] Scores(double[] x, int k, double[] residuals)
    {
        var scores = new double[x.Length];
        for (int row = 0; row < residuals.Length; row++)
        {
            for (int j = 0; j < k; j++)
            {
                scores[(row * k) + j] = x[(row * k) + j] * residuals[row];
            }
        }

        return scores;
    }

    private static void Scale(double[] matrix, double factor)
    {
        for (int i = 0; i < matrix.Length; i++)
        {
            matrix[i] *= factor;
        }
    }
}
