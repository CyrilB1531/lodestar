using Lodestar.Stats.Internal;

namespace Lodestar.Stats;

/// <summary>One-way analysis of variance: do several groups share one mean?</summary>
/// <remarks>
/// The k-sample generalisation of <see cref="TTest.Independent"/> with <see cref="Variance.Equal"/>:
/// on two groups F is the square of Student's t, and the two p-values agree. A degenerate input
/// where both the between- and within-group sums of squares are exactly zero returns a
/// <c>NaN</c> statistic and p-value, propagated rather than guarded against -- matching scipy's
/// own <c>f_oneway</c>, and unlike <see cref="KruskalWallis"/> (which throws on its analogous
/// input, since the ranks there are provably meaningless rather than merely indeterminate).
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

        // within = 0, between > 0 makes this +Infinity, not NaN -- FisherSf(+Infinity, ...)
        // is already exact and returns 0.0, honest for a perfect, noiseless separation.
        double statistic = (between / dfBetween) / (within / dfWithin);

        return new TestResult(statistic, Beta.FisherSf(statistic, dfBetween, dfWithin));
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
    /// exactly as an empty group passed directly would be (decision 0115).
    /// </remarks>
    public static TestResult Test(NanPolicy nanPolicy, params double[][] groups)
    {
        Guard.NotNull(groups);
        return nanPolicy == NanPolicy.Propagate
            ? Test(groups)
            : Test(NanFilter.ApplyGroups(groups, nanPolicy, nameof(groups)));
    }

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
