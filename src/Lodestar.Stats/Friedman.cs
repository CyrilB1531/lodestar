using Lodestar.Stats.Internal;

namespace Lodestar.Stats;

/// <summary>The Friedman test: do several treatments differ, measured on the same subjects?</summary>
/// <remarks>
/// What <see cref="Wilcoxon"/> is to <see cref="MannWhitney"/>, this is to
/// <see cref="KruskalWallis"/>: the same rank-based comparison of several groups, but with the
/// groups measured on one set of subjects rather than on independent ones. Each subject is ranked
/// against itself, which removes whatever made that subject high or low to begin with — the
/// reason it sees differences a between-subjects test would lose in the noise.
/// </remarks>
public static class Friedman
{
    /// <summary>Compares three or more treatments measured on the same blocks.</summary>
    /// <param name="treatments">
    /// One array per treatment, all the same length; entry <c>i</c> of each is the measurement on
    /// block <c>i</c>. At least three treatments, as <c>scipy.stats.friedmanchisquare</c> requires.
    /// </param>
    /// <returns>The Q statistic and the upper-tail p-value.</returns>
    /// <exception cref="ArgumentException">
    /// Fewer than three treatments, an empty treatment, or treatments of different lengths.
    /// </exception>
    // S2368: treatments mirrors scipy.stats.friedmanchisquare's own one-array-per-treatment shape.
#pragma warning disable S2368
    public static TestResult Test(params double[][] treatments)
#pragma warning restore S2368
        => Test(NanPolicy.Propagate, treatments);

    /// <summary>The same test, with a policy for the <c>NaN</c> values in the measurements.</summary>
    /// <param name="nanPolicy">What to do with a <c>NaN</c>; scipy's <c>nan_policy</c>.</param>
    /// <param name="treatments">Three or more treatments, all the same length.</param>
    /// <returns>The Q statistic and its p-value.</returns>
    /// <exception cref="ArgumentException">
    /// Fewer than three treatments, an empty or ragged treatment, or a <c>NaN</c> under
    /// <see cref="NanPolicy.Raise"/>.
    /// </exception>
    /// <remarks>
    /// The measurements are paired across treatments, so <see cref="NanPolicy.Omit"/> drops the
    /// whole block rather than one measurement of it: a block missing one treatment can no longer
    /// be ranked, and ranking the rest would compare treatments on different subjects.
    /// </remarks>
#pragma warning disable S2368
    public static TestResult Test(NanPolicy nanPolicy, params double[][] treatments)
#pragma warning restore S2368
    {
        Guard.NotNull(treatments);

        if (treatments.Length < 3)
        {
            throw new ArgumentException(
                $"The Friedman test needs at least three treatments; got {treatments.Length}.",
                nameof(treatments));
        }

        int k = treatments.Length;
        int blocks = Validated(treatments);
        double[][] kept = nanPolicy == NanPolicy.Propagate
            ? treatments
            : BlocksWithoutNaN(treatments, blocks, nanPolicy);
        blocks = kept[0].Length;

        if (blocks == 0)
        {
            throw new ArgumentException(
                "Every block holds a NaN, so nothing is left to rank.", nameof(treatments));
        }

        // A NaN must not take a false finite rank: Array.Sort puts it first and it would rank
        // like any value, the failure KruskalWallis.Test guards against the same way.
        if (Array.Exists(kept, treatment => Ranks.HasNaN(treatment)))
        {
            return new TestResult(double.NaN, double.NaN);
        }

        (double tieTerm, double[] rankSums) = RankWithinBlocks(kept, k, blocks);

        double correction = 1.0 - (tieTerm / (k * ((k * (double)k) - 1.0) * blocks));
        double sumOfSquares = 0.0;
        for (int t = 0; t < k; t++)
        {
            sumOfSquares += rankSums[t] * rankSums[t];
        }

        double statistic =
            ((12.0 / (k * (double)blocks * (k + 1.0)) * sumOfSquares) - (3.0 * blocks * (k + 1.0)))
            / correction;

        // Treatments agreeing on every block leave every rank tied, the correction zero and the
        // statistic 0/0. scipy answers NaN where KruskalWallis.Test refuses; each is matched.
        return double.IsNaN(statistic)
            ? new TestResult(double.NaN, double.NaN)
            : new TestResult(statistic, Distributions.ChiSquaredSf(statistic, k - 1.0));
    }

    /// <summary>Ranks each block across the treatments, returning the tie term and each treatment's rank sum.</summary>
    private static (double TieTerm, double[] RankSums) RankWithinBlocks(
        double[][] treatments, int k, int blocks)
    {
        double[] rankSums = new double[k];
        double[] block = new double[k];
        double tieTerm = 0.0;

        for (int b = 0; b < blocks; b++)
        {
            for (int t = 0; t < k; t++)
            {
                block[t] = treatments[t][b];
            }

            double[] ranks = Ranks.AverageWithTies(block, out double tieCorrection, out _);
            tieTerm += tieCorrection;
            for (int t = 0; t < k; t++)
            {
                rankSums[t] += ranks[t];
            }
        }

        return (tieTerm, rankSums);
    }

    private static int Validated(double[][] treatments)
    {
        if (treatments[0] is not { Length: > 0 })
        {
            throw new ArgumentException("Treatment 0 is empty.", nameof(treatments));
        }

        int blocks = treatments[0].Length;
        for (int t = 1; t < treatments.Length; t++)
        {
            if (treatments[t] is not { } treatment || treatment.Length != blocks)
            {
                throw new ArgumentException(
                    $"Treatment {t} holds {treatments[t]?.Length ?? 0} measurements where treatment 0 holds "
                    + $"{blocks}; every treatment is measured on the same blocks.",
                    nameof(treatments));
            }
        }

        return blocks;
    }

    /// <summary>Drops every block in which any treatment is missing, or refuses one under <see cref="NanPolicy.Raise"/>.</summary>
    private static double[][] BlocksWithoutNaN(double[][] treatments, int blocks, NanPolicy nanPolicy)
    {
        bool[] keep = new bool[blocks];
        int kept = 0;
        for (int b = 0; b < blocks; b++)
        {
            keep[b] = Complete(treatments, b);
            if (!keep[b] && nanPolicy == NanPolicy.Raise)
            {
                throw new ArgumentException(
                    "A measurement is NaN and NanPolicy.Raise was requested.", nameof(treatments));
            }

            if (keep[b])
            {
                kept++;
            }
        }

        return Compact(treatments, keep, kept);
    }

    /// <summary>Whether block <paramref name="b"/> was measured on every treatment.</summary>
    private static bool Complete(double[][] treatments, int b)
    {
        for (int t = 0; t < treatments.Length; t++)
        {
            if (double.IsNaN(treatments[t][b]))
            {
                return false;
            }
        }

        return true;
    }

    private static double[][] Compact(double[][] treatments, bool[] keep, int kept)
    {
        double[][] filtered = new double[treatments.Length][];
        for (int t = 0; t < treatments.Length; t++)
        {
            filtered[t] = new double[kept];
            int next = 0;
            for (int b = 0; b < keep.Length; b++)
            {
                if (keep[b])
                {
                    filtered[t][next++] = treatments[t][b];
                }
            }
        }

        return filtered;
    }
}
