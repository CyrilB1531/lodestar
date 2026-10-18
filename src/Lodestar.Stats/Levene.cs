using Lodestar.Stats.Internal;

namespace Lodestar.Stats;

/// <summary>Levene's test: do several groups share one variance?</summary>
/// <remarks>
/// The assumption <see cref="OneWayAnova"/> makes, and the one
/// <see cref="TTest.Independent"/> makes unless <see cref="Variance.Welch"/> is asked for — so
/// this is how a caller checks the ground those two stand on. It compares each group's absolute
/// deviations from its own centre, which turns a question about spread into a one-way ANOVA on
/// those deviations. <see cref="Bartlett"/> asks the same question assuming normal data and is
/// sharper when that holds; this one survives when it does not.
/// </remarks>
public static class Levene
{
    /// <summary>scipy's <c>proportiontocut</c> default, used by <see cref="Center.Trimmed"/> alone.</summary>
    private const double DefaultProportionToCut = 0.05;

    /// <summary>Compares the spread of two or more groups, around each group's median.</summary>
    /// <param name="groups">The groups; at least two. An empty one answers <c>NaN</c> for both, as scipy's does, and so does a null one.</param>
    /// <returns>The W statistic and the upper-tail p-value.</returns>
    /// <exception cref="ArgumentException">Fewer than two groups.</exception>
    // S2368: groups mirrors scipy.stats.levene's own one-array-per-group shape, as
    // OneWayAnova.Test does.
#pragma warning disable S2368
    public static TestResult Test(params double[][] groups)
#pragma warning restore S2368
        => Test(Center.Median, DefaultProportionToCut, NanPolicy.Propagate, groups);

    /// <summary>The same test, around a chosen centre and with a policy for missing values.</summary>
    /// <param name="center">Which centre each group's deviations are measured from.</param>
    /// <param name="proportionToCut">
    /// How much of each end <see cref="Center.Trimmed"/> drops; scipy's <c>proportiontocut</c>,
    /// ignored by the other two centres.
    /// </param>
    /// <param name="nanPolicy">What to do with a <c>NaN</c>; scipy's <c>nan_policy</c>.</param>
    /// <param name="groups">Two or more groups of observations.</param>
    /// <returns>The W statistic and its p-value.</returns>
    /// <exception cref="ArgumentException">
    /// Fewer than two groups, or a <c>NaN</c> under <see cref="NanPolicy.Raise"/>.
    /// </exception>
    /// <exception cref="ArgumentOutOfRangeException">
    /// <paramref name="proportionToCut"/> is NaN under <see cref="Center.Trimmed"/>, as an undefined option is; or, where a
    /// centre is taken, cuts a count below zero or past a group's middle, as scipy refuses them. An empty group or a
    /// propagated NaN answers NaN before that, and so does a trim that leaves a group nothing. Or an option is outside
    /// its enumeration.
    /// </exception>
    /// <remarks>
    /// The centre and the policy come first because the groups are a <c>params</c> array and C#
    /// allows no parameter after one — the shape <see cref="OneWayAnova"/> uses.
    /// </remarks>
#pragma warning disable S2368
    public static TestResult Test(
        Center center, double proportionToCut, NanPolicy nanPolicy, params double[][] groups)
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
        double[][] samples = NanFilter.ApplyGroups(groups, nanPolicy, nameof(groups));

        if (samples.Length < 2)
        {
            throw new ArgumentException(
                $"Levene's test needs at least two groups; got {samples.Length}.", nameof(groups));
        }

        // An empty group, given or left by omission, where this threw, or a propagated NaN, where an out-of-range trim was
        // refused: NaN, as scipy's levene answers. Only a trimmed centre reads the trim, so only there is the scan needed (#1723).
        if (Groups.AnyEmpty(samples)
            || (center == Center.Trimmed && nanPolicy == NanPolicy.Propagate && Array.Exists(samples, group => Ranks.HasNaN(group))))
        {
            return new TestResult(double.NaN, double.NaN);
        }

        int total = samples.Sum(group => group.Length);

        return Statistic(samples, center, proportionToCut, total);
    }

    private static TestResult Statistic(
        double[][] samples, Center center, double proportionToCut, int total)
    {
        int k = samples.Length;
        double[] groupMeans = new double[k];
        double weighted = 0.0;

        // Zij = |x_ij - centre_i|, and the group means of those deviations.
        double[][] deviations = new double[k][];
        for (int g = 0; g < k; g++)
        {
            double[] group = samples[g];
            double centre = GroupSpread.Centre(group, center, proportionToCut);
            double[] absolute = new double[group.Length];
            double sum = 0.0;
            for (int i = 0; i < group.Length; i++)
            {
                absolute[i] = Math.Abs(group[i] - centre);
                sum += absolute[i];
            }

            deviations[g] = absolute;
            groupMeans[g] = sum / group.Length;
            weighted += group.Length * groupMeans[g];
        }

        double grandMean = weighted / total;

        double between = 0.0;
        double within = 0.0;
        for (int g = 0; g < k; g++)
        {
            double gap = groupMeans[g] - grandMean;
            between += samples[g].Length * gap * gap;
            for (int i = 0; i < deviations[g].Length; i++)
            {
                double residual = deviations[g][i] - groupMeans[g];
                within += residual * residual;
            }
        }

        double numeratorDf = k - 1.0;
        double denominatorDf = total - k;
        double w = denominatorDf * between / (numeratorDf * within);

        // As for Bartlett: a NaN reaches the statistic through the deviations, scipy answers
        // (nan, nan), and the F tail refuses a NaN rather than passing one through.
        return double.IsNaN(w)
            ? new TestResult(double.NaN, double.NaN)
            : new TestResult(w, Distributions.FisherSnedecorSf(w, numeratorDf, denominatorDf));
    }
}
