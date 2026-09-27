using Lodestar.Stats.Internal;

namespace Lodestar.Stats;

/// <summary>One-way analysis of variance: do several groups share one mean?</summary>
/// <remarks>
/// The k-sample generalisation of <see cref="TTest.Independent"/> with <see cref="Variance.Equal"/>:
/// on two groups F is the square of Student's t, and the two p-values agree. Groups each holding
/// one repeated value answer an infinite statistic and a zero p-value, and a single value repeated
/// across every group answers <c>NaN</c> for both -- matching scipy's own <c>f_oneway</c>, and
/// unlike <see cref="KruskalWallis"/> (which throws on its analogous input, since the ranks there
/// are provably meaningless rather than merely indeterminate).
/// </remarks>
public static class OneWayAnova
{
    /// <summary>Compares the means of two or more groups.</summary>
    /// <param name="groups">The groups; at least two, each holding at least one value.</param>
    /// <returns>The F statistic and the upper-tail p-value.</returns>
    /// <exception cref="ArgumentException">
    /// Fewer than two groups, an empty group, or no group holding more than one value.
    /// </exception>
    // S2368: groups arrives from the caller already in this shape -- that is
    // how scipy.stats.f_oneway takes its samples, one array per group.
    // Wrapping it buys no safety, only a conversion at the boundary.
#pragma warning disable S2368
    public static TestResult Test(params double[][] groups)
#pragma warning restore S2368
    {
        Guard.NotNull(groups);

        if (groups.Length < 2)
        {
            throw new ArgumentException(
                $"An analysis of variance needs at least two groups; got {groups.Length}.",
                nameof(groups));
        }

        (int total, double grandSum) = ValidatedTotals(groups);

        if (total <= groups.Length)
        {
            throw new ArgumentException(
                "The within-group degrees of freedom are zero: every group holds one value.",
                nameof(groups));
        }

        double grandMean = grandSum / total;
        (double between, double within) = SumsOfSquares(groups, grandMean);

        double dfBetween = groups.Length - 1;
        double dfWithin = total - groups.Length;

        // Constant groups are decided by equality, as f_oneway does: three 0.1 leave the sums at
        // 1e-33, not zero (#1244). FisherSnedecorSf(+Infinity, ...) is an exact 0.0.
        double statistic = Constancy(groups) switch
        {
            GroupConstancy.AllEqual => double.NaN,
            GroupConstancy.EachGroup => double.PositiveInfinity,
            _ => (between / dfBetween) / (within / dfWithin),
        };

        return new TestResult(statistic, Beta.FisherSnedecorSf(statistic, dfBetween, dfWithin));
    }

    /// <summary>The same test, with a policy for the <c>NaN</c> values in the groups.</summary>
    /// <param name="nanPolicy">What to do with a <c>NaN</c>; scipy's <c>nan_policy</c>.</param>
    /// <param name="groups">Two or more groups of observations.</param>
    /// <returns>The F statistic and its p-value.</returns>
    /// <exception cref="ArgumentException">
    /// When <paramref name="nanPolicy"/> is <see cref="NanPolicy.Raise"/> and a group holds a
    /// <c>NaN</c>, or when the filtered groups fail this test's own requirements.
    /// </exception>
    /// <remarks>
    /// The policy comes first because the groups are a <c>params</c> array and C# allows no
    /// parameter after one — the shape <c>string.Join</c> uses, for the same reason. Omission is
    /// per group and runs before this test's guards, so a group emptied by it is refused here
    /// exactly as an empty group passed directly would be (decision 0007).
    /// </remarks>
    public static TestResult Test(NanPolicy nanPolicy, params double[][] groups)
    {
        Guard.NotNull(groups);
        return nanPolicy == NanPolicy.Propagate
            ? Test(groups)
            : Test(NanFilter.ApplyGroups(groups, nanPolicy, nameof(groups)));
    }

    private enum GroupConstancy
    {
        None,
        EachGroup,
        AllEqual,
    }

    // S1244: a zero difference is the test itself, scipy's diff(...) == 0, which inf - inf fails.
    // A lone NaN leaves the arithmetic to answer NaN, as scipy's nan_policy wrapper does.
#pragma warning disable S1244
    private static GroupConstancy Constancy(double[][] groups)
    {
        bool allEqual = true;
        for (int g = 0; g < groups.Length; g++)
        {
            double[] group = groups[g];
            if (double.IsNaN(group[0]))
            {
                return GroupConstancy.None;
            }

            for (int i = 1; i < group.Length; i++)
            {
                if (group[i] - group[i - 1] != 0.0)
                {
                    return GroupConstancy.None;
                }
            }

            if (g > 0)
            {
                double[] before = groups[g - 1];
                allEqual &= group[0] - before[before.Length - 1] == 0.0;
            }
        }

        return allEqual ? GroupConstancy.AllEqual : GroupConstancy.EachGroup;
    }
#pragma warning restore S1244

    private static (int Total, double GrandSum) ValidatedTotals(double[][] groups)
    {
        int total = 0;
        double grandSum = 0.0;
        for (int g = 0; g < groups.Length; g++)
        {
            if (groups[g] is not { Length: > 0 })
            {
                throw new ArgumentException($"Group {g} is empty.", nameof(groups));
            }

            for (int i = 0; i < groups[g].Length; i++)
            {
                grandSum += groups[g][i];
                total++;
            }
        }

        return (total, grandSum);
    }

    private static (double Between, double Within) SumsOfSquares(double[][] groups, double grandMean)
    {
        double between = 0.0;
        double within = 0.0;
        for (int g = 0; g < groups.Length; g++)
        {
            double sum = 0.0;
            for (int i = 0; i < groups[g].Length; i++)
            {
                sum += groups[g][i];
            }

            double mean = sum / groups[g].Length;
            double deviation = mean - grandMean;
            between += groups[g].Length * deviation * deviation;

            for (int i = 0; i < groups[g].Length; i++)
            {
                double residual = groups[g][i] - mean;
                within += residual * residual;
            }
        }

        return (between, within);
    }
}
