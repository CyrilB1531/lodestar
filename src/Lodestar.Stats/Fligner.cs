using Lodestar.Stats.Internal;

namespace Lodestar.Stats;

/// <summary>The Fligner-Killeen test: do several groups share one variance?</summary>
/// <remarks>
/// The third equality-of-variance test beside <see cref="Levene"/> and <see cref="Bartlett"/>, and the one that
/// ranks: each group's absolute deviations from its centre are ranked together, turned into normal scores, and
/// compared across groups by a χ² statistic — so it holds up where the data are neither normal nor free of outliers.
/// </remarks>
public static class Fligner
{
    /// <summary>scipy's <c>proportiontocut</c> default, used by <see cref="Center.Trimmed"/> alone.</summary>
    private const double DefaultProportionToCut = 0.05;

    /// <summary>Compares the spread of two or more groups, around each group's median.</summary>
    /// <param name="groups">The groups; at least two.</param>
    /// <returns>The χ² statistic and its upper-tail p-value on <c>k − 1</c> degrees of freedom.</returns>
    /// <exception cref="ArgumentException">Fewer than two groups.</exception>
    // S2368: groups mirrors scipy.stats.fligner's one-array-per-group shape, as Levene.Test does.
#pragma warning disable S2368
    public static TestResult Test(params double[][] groups)
#pragma warning restore S2368
        => Test(Center.Median, DefaultProportionToCut, NanPolicy.Propagate, groups);

    /// <summary>The same test, around a chosen centre and with a policy for missing values.</summary>
    /// <param name="center">Which centre each group's deviations are measured from.</param>
    /// <param name="proportionToCut">How much of each end <see cref="Center.Trimmed"/> drops; ignored by the other two centres.</param>
    /// <param name="nanPolicy">What to do with a <c>NaN</c>; scipy's <c>nan_policy</c>.</param>
    /// <param name="groups">Two or more groups of observations.</param>
    /// <returns>
    /// The χ² statistic and its p-value; both <c>NaN</c> where a group is empty or a <c>NaN</c> propagates, as scipy answers,
    /// and where every deviation ties, 0/0, where scipy's rounding answers NaN or a finite value by the groups.
    /// </returns>
    /// <exception cref="ArgumentException">Fewer than two groups, or a <c>NaN</c> under <see cref="NanPolicy.Raise"/>.</exception>
    /// <exception cref="ArgumentOutOfRangeException">
    /// <paramref name="proportionToCut"/> is NaN under <see cref="Center.Trimmed"/>, as an undefined option is; or, where a
    /// centre is taken, cuts a count below zero or past a group's middle, as scipy refuses them. An empty group or a
    /// propagated NaN answers NaN before that, and so does a trim that leaves a group nothing. Or an option is outside
    /// its enumeration.
    /// </exception>
#pragma warning disable S2368
    public static TestResult Test(Center center, double proportionToCut, NanPolicy nanPolicy, params double[][] groups)
#pragma warning restore S2368
    {
        // An option outside its enumeration is refused last, once the input has met every refusal of its own (#1704's Review A).
        TestResult result = TestCore(center, proportionToCut, nanPolicy, groups);
        Options.Require(center);
        Options.Require(nanPolicy);
        return result;
    }

    private static TestResult TestCore(
        Center center,
        double proportionToCut,
        NanPolicy nanPolicy,
        double[][] groups)
    {
        Guard.NotNull(groups);
        GroupSpread.RequireProportion(center, proportionToCut);
        if (groups.Length < 2)
        {
            throw new ArgumentException($"The Fligner-Killeen test needs at least two groups; got {groups.Length}.", nameof(groups));
        }

        double[][] samples = NanFilter.ApplyGroups(groups, nanPolicy, nameof(groups));
        // Only Propagate can keep a NaN: omission dropped it and Raise refused it, so the scan runs there alone (#1724).
        if (Groups.AnyEmpty(samples) || (nanPolicy == NanPolicy.Propagate && Array.Exists(samples, group => Ranks.HasNaN(group))))
        {
            return new TestResult(double.NaN, double.NaN);
        }

        return Statistic(samples, center, proportionToCut);
    }

    private static TestResult Statistic(double[][] samples, Center center, double proportionToCut)
    {
        int k = samples.Length;
        int total = samples.Sum(group => group.Length);
        var deviations = new double[total];
        int at = 0;
        // Every centre first, so a trim refused in a later group is refused as scipy refuses it.
        double[] centres = [.. samples.Select(group => GroupSpread.Centre(group, center, proportionToCut))];

        // A group trimmed to nothing has a NaN centre, and an infinite value at an infinite one an inf - inf deviation: scipy
        // answers NaN for both, where ranking the second answered finite values (#1707). Read off the k centres (#1724).
        if (AnyUndefinedDeviation(samples, centres))
        {
            return new TestResult(double.NaN, double.NaN);
        }
        for (int g = 0; g < samples.Length; g++)
        {
            double centre = centres[g];
            foreach (double value in samples[g])
            {
                deviations[at++] = Math.Abs(value - centre);
            }
        }

        // The normal scores of the pooled ranks: Φ⁻¹(r / (2(N + 1)) + 1/2), from Wichura's AS 241 alone, about 1e-16
        // relative: the published quantile, scipy's ndtri, took the test from 8.34 to 9.59 ms at 100,000 rows (#1684).
        double[] ranks = Ranks.AverageWithTies(deviations, out double tieCorrection, out _);
        // Every deviation tied, one group of all N: the scores have no variance, 0/0 (decision 0010), where rounding read 15
        // and scipy NaN or 12. Read off the tie term the ranking already summed, not a pass of its own (#1724).
        // S1244: Ranks forms each group's t³ − t by these same operations, and two or more groups sum at least about 3N² below.
#pragma warning disable S1244
        if (tieCorrection == ((double)total * total * total) - total)
#pragma warning restore S1244
        {
            return new TestResult(double.NaN, double.NaN);
        }
        var scores = new double[total];
        double sum = 0.0;
        for (int i = 0; i < total; i++)
        {
            double level = (ranks[i] / (2.0 * (total + 1.0))) + 0.5;
            scores[i] = Normal.RationalUpperQuantile(1.0 - level);
            sum += scores[i];
        }

        double mean = sum / total;
        double squares = 0.0;
        foreach (double score in scores)
        {
            squares += (score - mean) * (score - mean);
        }

        double variance = squares / (total - 1.0);
        double between = 0.0;
        at = 0;
        for (int g = 0; g < k; g++)
        {
            int size = samples[g].Length;
            double groupSum = 0.0;
            for (int i = 0; i < size; i++)
            {
                groupSum += scores[at + i];
            }

            at += size;
            double gap = (groupSum / size) - mean;
            between += size * gap * gap;
        }

        double statistic = between / variance;

        return double.IsPositiveInfinity(statistic)
            ? new TestResult(statistic, 0.0)
            : new TestResult(statistic, Distributions.ChiSquaredSf(statistic, k - 1.0));
    }

    /// <summary>Whether a centre is NaN, or infinite beside a value equal to it, either of which makes a deviation NaN.</summary>
    private static bool AnyUndefinedDeviation(double[][] samples, double[] centres)
    {
        for (int g = 0; g < samples.Length; g++)
        {
            double centre = centres[g];
            if (double.IsNaN(centre) || (double.IsInfinity(centre) && Array.IndexOf(samples[g], centre) >= 0))
            {
                return true;
            }
        }
        return false;
    }
}
