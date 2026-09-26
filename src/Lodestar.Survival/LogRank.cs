using Lodestar.Stats;
using Lodestar.Survival.Internal;

namespace Lodestar.Survival;

/// <summary>The log-rank family: two groups or more, weighted, pairwise.</summary>
/// <remarks>
/// Reference behavior: <c>lifelines.statistics</c> 0.30.3 — <c>logrank_test</c>, <c>multivariate_logrank_test</c> and
/// <c>pairwise_logrank_test</c>, the Mantel-Haenszel form with the hypergeometric variance under ties. The p-value
/// is a chi-squared upper tail taken from <see cref="Distributions.ChiSquaredSf"/> rather than re-derived here —
/// decision 0003 published that member for exactly this call. Thread-safe.
/// </remarks>
public static class LogRank
{
    private static readonly LogRankOptions Defaults = new();

    /// <summary>Compares the survival of two right-censored samples.</summary>
    /// <param name="durationsA">The first group's durations.</param>
    /// <param name="eventObservedA">The first group's event flags.</param>
    /// <param name="durationsB">The second group's durations.</param>
    /// <param name="eventObservedB">The second group's event flags.</param>
    /// <exception cref="ArgumentException">A group's spans differ in length, a group is empty, or a duration is negative or NaN.</exception>
    /// <remarks>
    /// At each time carrying an event anywhere, the first group's observed events are
    /// compared against what the pooled risk sets would give it. The statistic is the
    /// squared total difference over the summed variance, so it is never negative and is
    /// zero when the two curves agree step for step.
    /// </remarks>
    public static LogRankResult Test(
        ReadOnlySpan<double> durationsA,
        ReadOnlySpan<bool> eventObservedA,
        ReadOnlySpan<double> durationsB,
        ReadOnlySpan<bool> eventObservedB) =>
        Test(durationsA, eventObservedA, durationsB, eventObservedB, Defaults);

    /// <summary>Compares two right-censored samples under a weighting of the family, lifelines' <c>logrank_test(..., weightings=)</c>.</summary>
    /// <param name="durationsA">The first group's durations.</param>
    /// <param name="eventObservedA">The first group's event flags.</param>
    /// <param name="durationsB">The second group's durations.</param>
    /// <param name="eventObservedB">The second group's event flags.</param>
    /// <param name="options">The weighting, its exponents and the truncation.</param>
    /// <exception cref="ArgumentException">As the unweighted overload.</exception>
    /// <exception cref="ArgumentOutOfRangeException"><paramref name="options"/> names no weighting, or has a negative or non-finite exponent, or a truncation that is negative or NaN.</exception>
    public static LogRankResult Test(
        ReadOnlySpan<double> durationsA,
        ReadOnlySpan<bool> eventObservedA,
        ReadOnlySpan<double> durationsB,
        ReadOnlySpan<bool> eventObservedB,
        LogRankOptions options) =>
        Test(durationsA, eventObservedA, [], durationsB, eventObservedB, [], options);

    /// <summary>Compares two right-censored samples of weighted subjects, lifelines' <c>logrank_test(..., weights_A=, weights_B=)</c>.</summary>
    /// <param name="durationsA">The first group's durations.</param>
    /// <param name="eventObservedA">The first group's event flags.</param>
    /// <param name="weightsA">One positive, finite weight per subject of the first group, or empty for ones.</param>
    /// <param name="durationsB">The second group's durations.</param>
    /// <param name="eventObservedB">The second group's event flags.</param>
    /// <param name="weightsB">One positive, finite weight per subject of the second group, or empty for ones.</param>
    /// <param name="options">The weighting, its exponents and the truncation.</param>
    /// <exception cref="ArgumentException">As the unweighted overload, or a weight span is neither empty nor one positive, finite value per subject.</exception>
    /// <exception cref="ArgumentOutOfRangeException">As the weighted overload.</exception>
    /// <remarks>
    /// A subject weighing two counts as two subjects with its duration, in every risk set and every event count —
    /// except in the pooled curve Fleming-Harrington weighs by, which counts subjects unweighted, as lifelines' does.
    /// </remarks>
    public static LogRankResult Test(
        ReadOnlySpan<double> durationsA,
        ReadOnlySpan<bool> eventObservedA,
        ReadOnlySpan<double> weightsA,
        ReadOnlySpan<double> durationsB,
        ReadOnlySpan<bool> eventObservedB,
        ReadOnlySpan<double> weightsB,
        LogRankOptions options)
    {
        RiskTable.Validate(durationsA, eventObservedA, nameof(durationsA));
        RiskTable.Validate(durationsB, eventObservedB, nameof(durationsB));
        Guard.NotNull(options);
        Check(options);
        int a = durationsA.Length;
        int n = a + durationsB.Length;
        var durations = new double[n];
        var observed = new bool[n];
        var group = new int[n];
        durationsA.CopyTo(durations);
        durationsB.CopyTo(durations.AsSpan(a));
        eventObservedA.CopyTo(observed);
        eventObservedB.CopyTo(observed.AsSpan(a));
        for (int i = a; i < n; i++)
        {
            group[i] = 1;
        }

        double[]? weights = null;
        if (!weightsA.IsEmpty || !weightsB.IsEmpty)
        {
            weights = new double[n];
            Weights(weightsA, a, nameof(weightsA)).CopyTo(weights, 0);
            Weights(weightsB, n - a, nameof(weightsB)).CopyTo(weights, a);
        }

        return LogRankFamily.Test(durations, observed, weights, group, 2, options);
    }

    /// <summary>Compares the survival of several groups at once, lifelines' <c>multivariate_logrank_test</c>.</summary>
    /// <param name="durations">One duration per subject.</param>
    /// <param name="groups">One group label per subject, any integers; at least two distinct.</param>
    /// <param name="eventObserved">One event flag per subject.</param>
    /// <param name="options">The weighting, its exponents and the truncation; <see langword="null"/> for the log-rank test.</param>
    /// <returns>The statistic on one degree of freedom fewer than the groups.</returns>
    /// <exception cref="ArgumentException">The spans differ in length, the sample is empty, a duration is negative or NaN, or fewer than two groups appear.</exception>
    /// <exception cref="ArgumentOutOfRangeException">As the weighted two-sample overload.</exception>
    public static LogRankResult MultiGroup(
        ReadOnlySpan<double> durations,
        ReadOnlySpan<int> groups,
        ReadOnlySpan<bool> eventObserved,
        LogRankOptions? options = null) =>
        MultiGroup(durations, groups, eventObserved, [], options);

    /// <summary>Compares several groups of weighted subjects, lifelines' <c>multivariate_logrank_test(..., weights=)</c>.</summary>
    /// <param name="durations">One duration per subject.</param>
    /// <param name="groups">One group label per subject, any integers; at least two distinct.</param>
    /// <param name="eventObserved">One event flag per subject.</param>
    /// <param name="weights">One positive, finite weight per subject, or empty for ones.</param>
    /// <param name="options">The weighting, its exponents and the truncation; <see langword="null"/> for the log-rank test.</param>
    /// <returns>The statistic on one degree of freedom fewer than the groups.</returns>
    /// <exception cref="ArgumentException">As the unweighted overload, or <paramref name="weights"/> is neither empty nor one positive, finite value per subject.</exception>
    /// <exception cref="ArgumentOutOfRangeException">As the weighted two-sample overload.</exception>
    /// <remarks>
    /// The statistic reads the covariance through a pseudo-inverse, as lifelines does with <c>numpy.linalg.pinv</c>,
    /// so a group nobody in it is at risk for by the first event leaves the others' comparison standing rather than
    /// failing. Which group is dropped from the quadratic form does not change it.
    /// </remarks>
    public static LogRankResult MultiGroup(
        ReadOnlySpan<double> durations,
        ReadOnlySpan<int> groups,
        ReadOnlySpan<bool> eventObserved,
        ReadOnlySpan<double> weights,
        LogRankOptions? options = null)
    {
        RiskTable.Validate(durations, eventObserved, nameof(durations));
        LogRankOptions settings = options ?? Defaults;
        Check(settings);
        (int[] index, int[] labels) = Indexed(groups, durations.Length);
        double[]? checkedWeights = weights.IsEmpty ? null : Weights(weights, durations.Length, nameof(weights));
        return LogRankFamily.Test(durations, eventObserved, checkedWeights, index, labels.Length, settings);
    }

    /// <summary>Runs the two-sample test on every pair of groups, lifelines' <c>pairwise_logrank_test</c>.</summary>
    /// <param name="durations">One duration per subject.</param>
    /// <param name="groups">One group label per subject, any integers; at least two distinct.</param>
    /// <param name="eventObserved">One event flag per subject.</param>
    /// <param name="options">The weighting, its exponents and the truncation; <see langword="null"/> for the log-rank test.</param>
    /// <returns>One result per pair, the labels ascending and each pair lower label first.</returns>
    /// <exception cref="ArgumentException">As <see cref="MultiGroup(ReadOnlySpan{double}, ReadOnlySpan{int}, ReadOnlySpan{bool}, LogRankOptions)"/>.</exception>
    /// <exception cref="ArgumentOutOfRangeException">As the weighted two-sample overload.</exception>
    /// <remarks>
    /// Each pair is tested on its own two groups alone, the others left out of its risk sets and, under
    /// Fleming-Harrington, out of its pooled curve. No correction for multiple comparisons is applied, as lifelines
    /// applies none.
    /// </remarks>
    public static IReadOnlyList<PairwiseLogRankResult> Pairwise(
        ReadOnlySpan<double> durations,
        ReadOnlySpan<int> groups,
        ReadOnlySpan<bool> eventObserved,
        LogRankOptions? options = null)
    {
        RiskTable.Validate(durations, eventObserved, nameof(durations));
        LogRankOptions settings = options ?? Defaults;
        Check(settings);
        (_, int[] labels) = Indexed(groups, durations.Length);
        int[] sorted = [.. labels];
        Array.Sort(sorted);
        var results = new List<PairwiseLogRankResult>();
        for (int first = 0; first < sorted.Length - 1; first++)
        {
            for (int second = first + 1; second < sorted.Length; second++)
            {
                results.Add(new PairwiseLogRankResult(
                    sorted[first], sorted[second], Pair(durations, groups, eventObserved, sorted[first], sorted[second], settings)));
            }
        }

        return results;
    }

    /// <summary>The two groups' subjects, the first group's first, as lifelines slices them.</summary>
    private static LogRankResult Pair(
        ReadOnlySpan<double> durations,
        ReadOnlySpan<int> groups,
        ReadOnlySpan<bool> eventObserved,
        int first,
        int second,
        LogRankOptions options)
    {
        var pairDurations = new List<double>();
        var pairObserved = new List<bool>();
        var pairGroup = new List<int>();
        foreach (int label in (int[])[first, second])
        {
            for (int i = 0; i < groups.Length; i++)
            {
                if (groups[i] == label)
                {
                    pairDurations.Add(durations[i]);
                    pairObserved.Add(eventObserved[i]);
                    pairGroup.Add(label == first ? 0 : 1);
                }
            }
        }

        return LogRankFamily.Test([.. pairDurations], [.. pairObserved], null, [.. pairGroup], 2, options);
    }

    /// <summary>Each subject's group as an index in first-appearance order, and the labels in that order.</summary>
    private static (int[] Index, int[] Labels) Indexed(ReadOnlySpan<int> groups, int count)
    {
        if (groups.Length != count)
        {
            throw new ArgumentException(
                $"groups holds {groups.Length} labels for {count} subjects.", nameof(groups));
        }

        var positions = new Dictionary<int, int>();
        var labels = new List<int>();
        var index = new int[count];
        for (int i = 0; i < count; i++)
        {
            if (!positions.TryGetValue(groups[i], out int position))
            {
                position = labels.Count;
                positions.Add(groups[i], position);
                labels.Add(groups[i]);
            }

            index[i] = position;
        }

        if (labels.Count < 2)
        {
            throw new ArgumentException("A comparison needs at least two groups; one label appears.", nameof(groups));
        }

        return (index, [.. labels]);
    }

    private static double[] Weights(ReadOnlySpan<double> weights, int count, string name)
    {
        if (weights.IsEmpty)
        {
            var ones = new double[count];
            ones.AsSpan().Fill(1.0);
            return ones;
        }

        if (weights.Length != count)
        {
            throw new ArgumentException($"{name} holds {weights.Length} weights for {count} subjects.", name);
        }

        foreach (double weight in weights)
        {
            if (!(weight > 0.0) || double.IsInfinity(weight))
            {
                throw new ArgumentException($"A weight counts subjects, so it is positive and finite; {name} holds {weight}.", name);
            }
        }

        return weights.ToArray();
    }

    private static void Check(LogRankOptions options)
    {
        if (options.Weighting < LogRankWeighting.LogRank || options.Weighting > LogRankWeighting.FlemingHarrington)
        {
            throw new ArgumentOutOfRangeException(nameof(options), options.Weighting, "Weighting names no member of the family.");
        }

        if (!(options.P >= 0.0) || double.IsInfinity(options.P))
        {
            throw new ArgumentOutOfRangeException(nameof(options), options.P, "P is a non-negative, finite exponent.");
        }

        if (!(options.Q >= 0.0) || double.IsInfinity(options.Q))
        {
            throw new ArgumentOutOfRangeException(nameof(options), options.Q, "Q is a non-negative, finite exponent.");
        }

        if (options.Truncation is double truncation && !(truncation >= 0.0))
        {
            throw new ArgumentOutOfRangeException(nameof(options), truncation, "Truncation is a non-negative time.");
        }
    }
}
