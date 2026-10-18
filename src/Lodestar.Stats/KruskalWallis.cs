using Lodestar.Stats.Internal;

namespace Lodestar.Stats;

/// <summary>The Kruskal-Wallis H test: the rank-based k-sample comparison.</summary>
/// <remarks>
/// What <see cref="OneWayAnova"/> is to <see cref="TTest.Independent"/>, this is
/// to <see cref="MannWhitney"/>: on two groups it agrees with Mann-Whitney's
/// asymptotic two-sided p-value.
/// </remarks>
public static class KruskalWallis
{
    /// <summary>Compares two or more groups by their ranks in the pooled sample.</summary>
    /// <param name="groups">The groups; at least two. A null or empty one, or a fully tied pool, answers <c>NaN</c> for both.</param>
    /// <returns>The H statistic and the upper-tail p-value.</returns>
    /// <exception cref="ArgumentException">
    /// Fewer than two groups.
    /// </exception>
    // S2368: same reasoning as OneWayAnova.Test -- groups mirrors
    // scipy.stats.kruskal's own one-array-per-group shape.
#pragma warning disable S2368
    public static TestResult Test(params double[][] groups)
#pragma warning restore S2368
    {
        Guard.NotNull(groups);

        if (groups.Length < 2)
        {
            throw new ArgumentException(
                $"Kruskal-Wallis needs at least two groups; got {groups.Length}.", nameof(groups));
        }

        // An empty group, given or left by omission: scipy's kruskal answers NaN, where this threw, and so does a null one here.
        if (Groups.AnyEmpty(groups))
        {
            return new TestResult(double.NaN, double.NaN);
        }

        int total = groups.Sum(group => group.Length);
        // Under the default NanPolicy.Propagate a NaN anywhere propagates rather
        // than taking a false finite rank (Ranks.HasNaN's remark).
        if (Array.Exists(groups, group => Ranks.HasNaN(group)))
        {
            return new TestResult(double.NaN, double.NaN);
        }

        (double weighted, double tieTerm) = RankSumsAndTies(groups, total);
        double h = (12.0 / (total * (total + 1.0)) * weighted) - (3.0 * (total + 1.0));

        // Every value tied leaves nothing: with t = n (one tie group spanning the whole
        // sample), 1 - (t^3-t)/(n^3-n) is exactly 0, not merely close to it.
        double tieCorrection = 1.0 - (tieTerm / (((double)total * total * total) - total));

        // H is then 0/0, which Kruskal and Wallis leave undefined: NaN, as decision 0010 rules, where this threw.
        if (tieCorrection <= 0.0)
        {
            return new TestResult(double.NaN, double.NaN);
        }

        h /= tieCorrection;
        double dof = groups.Length - 1;

        // Rounding can land H a hair below zero on a perfectly balanced split (#1245); scipy's
        // chdtrc answers NaN there, where the regularized Q would throw.
        double pValue = h < 0.0 ? double.NaN : Gamma.ChiSquaredTail(dof, h, upper: true);

        return new TestResult(h, pValue);
    }

    /// <summary>The same test, with a policy for the <c>NaN</c> values in the groups.</summary>
    /// <param name="nanPolicy">What to do with a <c>NaN</c>; scipy's <c>nan_policy</c>.</param>
    /// <param name="groups">Two or more groups of observations.</param>
    /// <returns>The H statistic and its p-value.</returns>
    /// <exception cref="ArgumentException">
    /// When <paramref name="nanPolicy"/> is <see cref="NanPolicy.Raise"/> and a group holds a
    /// <c>NaN</c>. A pooled sample left fully tied, or a group left empty, by omission answers
    /// <c>NaN</c> as it would if passed directly (decision 0010).
    /// </exception>
    /// <remarks>
    /// The policy comes first because the groups are a <c>params</c> array and C# allows no
    /// parameter after one — the shape <c>string.Join</c> uses, for the same reason.
    /// </remarks>
    /// <exception cref="ArgumentOutOfRangeException">An option is outside its enumeration.</exception>
    public static TestResult Test(NanPolicy nanPolicy, params double[][] groups)
    {
        Guard.NotNull(groups);
        // The policy is refused last, once the groups have met the test's own refusals (#1712's and #1704's Review A).
        TestResult result = Test(NanFilter.ApplyGroups(groups, nanPolicy, nameof(groups)));
        Options.Require(nanPolicy);
        return result;
    }

    private static double[] Pool(double[][] groups, int total)
    {
        double[] pooled = new double[total];
        int offset = 0;
        for (int g = 0; g < groups.Length; g++)
        {
            groups[g].CopyTo(pooled, offset);
            offset += groups[g].Length;
        }

        return pooled;
    }

    /// <summary>The weighted rank sum and the tie term, by merging sorted groups where there are few of them.</summary>
    /// <remarks>
    /// Past <see cref="KSampleRanks.MaxGroups"/> groups the pooled sample is ranked by sorting it, as it
    /// always was. Both routes return the same doubles: KSampleRanks' remarks have why.
    /// </remarks>
    private static (double Weighted, double TieTerm) RankSumsAndTies(double[][] groups, int total)
    {
        if (groups.Length > KSampleRanks.MaxGroups)
        {
            double[] pooled = Pool(groups, total);
            double[] ranks = Ranks.AverageWithTies(pooled, out double pooledTieTerm, out _);
            return (WeightedRankSum(groups, ranks), pooledTieTerm);
        }

        Span<double> sums = stackalloc double[groups.Length];
        double tieTerm = KSampleRanks.Compute(groups, total, sums);
        double weighted = 0.0;
        for (int g = 0; g < groups.Length; g++)
        {
            weighted += sums[g] * sums[g] / groups[g].Length;
        }

        return (weighted, tieTerm);
    }

    private static double WeightedRankSum(double[][] groups, double[] ranks)
    {
        double weighted = 0.0;
        int offset = 0;
        for (int g = 0; g < groups.Length; g++)
        {
            double sum = 0.0;
            for (int i = 0; i < groups[g].Length; i++)
            {
                sum += ranks[offset + i];
            }

            weighted += sum * sum / groups[g].Length;
            offset += groups[g].Length;
        }

        return weighted;
    }
}
