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
    /// <summary>The coefficients' covariance, and the bandwidth a kernel covariance used.</summary>
    /// <remarks>
    /// Every estimator scales by <c>n / (n − extra − k)</c> when debiased and <c>n / (n − extra)</c> otherwise. The clustered
    /// one takes no <c>G/(G−1)</c> factor, and two ways are <c>S₀ + S₁ − S₀₁</c>; Driscoll-Kraay sums the scores by period,
    /// applies the kernel over periods and scales by <c>T/n</c>.
    /// </remarks>
    /// <exception cref="ArgumentException">A Bartlett or Parzen bandwidth reaches past the periods, which the reference refuses.</exception>
    public static (double[] Covariance, int? Bandwidth) Estimate(
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
            return (unadjusted, null);
        }

        double[] scores = Scores(x, k, residuals);
        (double[] meat, int? bandwidth) = Meat(scores, k, n, spec);
        Scale(meat, scale);
        Scale(xx, 1.0 / n);
        double[] bread = IvCore.Invert(xx, k);
        double[] covariance = Dense.Multiply(Dense.Multiply(bread, meat, k, k, k), bread, k, k, k);
        Scale(covariance, 1.0 / n);
        Dense.Symmetrise(covariance, k);
        return (covariance, bandwidth);
    }

    private static (double[] Meat, int? Bandwidth) Meat(double[] scores, int k, int n, PanelCovarianceSpec spec)
    {
        switch (spec.Type)
        {
            case PanelCovarianceType.Robust:
                return (IvScores.Robust(scores, k, n), null);
            case PanelCovarianceType.Clustered:
                return (ClusteredMeat(scores, k, n, spec.Clusters), null);
            default:
                return DriscollKraay(scores, k, n, spec);
        }
    }

    private static double[] ClusteredMeat(double[] scores, int k, int n, IReadOnlyList<int[]> clusters)
    {
        if (clusters.Count == 0)
        {
            // No cluster named: each row its own, as the reference defaults to.
            return IvScores.Robust(scores, k, n);
        }

        if (clusters.Count == 1)
        {
            return IvScores.Clustered(scores, k, n, ClusterLabels.Dense(clusters[0], n));
        }

        double[] first = IvScores.Clustered(scores, k, n, ClusterLabels.Dense(clusters[0], n));
        double[] second = IvScores.Clustered(scores, k, n, ClusterLabels.Dense(clusters[1], n));
        double[] both = IvScores.Clustered(scores, k, n, ClusterLabels.Dense(Union(clusters[0], clusters[1]), n));
        for (int i = 0; i < first.Length; i++)
        {
            first[i] += second[i] - both[i];
        }

        return first;
    }

    private static (double[] Meat, int? Bandwidth) DriscollKraay(double[] scores, int k, int n, PanelCovarianceSpec spec)
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
        return (meat, bandwidth);
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
