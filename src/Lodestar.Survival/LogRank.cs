using Lodestar.Stats;

namespace Lodestar.Survival;

/// <summary>The two-sample log-rank test.</summary>
/// <remarks>
/// Reference behavior: <c>lifelines.statistics.logrank_test</c> 0.30.3, which is the
/// Mantel-Haenszel form with the hypergeometric variance under ties. Its p-value is a
/// chi-squared upper tail on one degree of freedom, taken from
/// <see cref="Distributions.ChiSquaredSf"/> rather than re-derived here — decision 0097
/// published that member for exactly this call. Thread-safe.
/// </remarks>
public static class LogRank
{
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
        ReadOnlySpan<bool> eventObservedB)
    {
        RiskTable.Validate(durationsA, eventObservedA, nameof(durationsA));
        RiskTable.Validate(durationsB, eventObservedB, nameof(durationsB));

        double[] times = DistinctEventTimes(
            durationsA, eventObservedA, durationsB, eventObservedB);

        double observedMinusExpected = 0.0;
        double variance = 0.0;

        foreach (double time in times)
        {
            int riskA = AtRisk(durationsA, time);
            int riskB = AtRisk(durationsB, time);
            int eventsA = EventsAt(durationsA, eventObservedA, time);
            int eventsB = EventsAt(durationsB, eventObservedB, time);

            int risk = riskA + riskB;
            int events = eventsA + eventsB;
            if (risk < 2 || events == 0)
            {
                // One subject left, or a time that only censors: the hypergeometric
                // variance is zero there and the term carries no information.
                continue;
            }

            double expectedA = (double)events * riskA / risk;
            observedMinusExpected += eventsA - expectedA;

            // The hypergeometric variance, which is what makes ties come out right:
            // with one event at a time it reduces to the familiar n1 n2 / n².
            variance += (double)events * riskA * riskB * (risk - events)
                / ((double)risk * risk * (risk - 1));
        }

        if (variance <= 0.0)
        {
            // No time compared both groups, so there is nothing to reject.
            return new LogRankResult(0.0, 1.0, 1);
        }

        double statistic = observedMinusExpected * observedMinusExpected / variance;
        return new LogRankResult(statistic, Distributions.ChiSquaredSf(statistic, 1.0), 1);
    }

    private static double[] DistinctEventTimes(
        ReadOnlySpan<double> durationsA,
        ReadOnlySpan<bool> eventsA,
        ReadOnlySpan<double> durationsB,
        ReadOnlySpan<bool> eventsB)
    {
        SortedSet<double> times = [];
        Collect(times, durationsA, eventsA);
        Collect(times, durationsB, eventsB);
        return [.. times];
    }

    private static void Collect(
        SortedSet<double> times, ReadOnlySpan<double> durations, ReadOnlySpan<bool> observed)
    {
        for (int i = 0; i < durations.Length; i++)
        {
            if (observed[i])
            {
                times.Add(durations[i]);
            }
        }
    }

    /// <summary>How many of a group were still at risk immediately before <paramref name="time"/>.</summary>
    private static int AtRisk(ReadOnlySpan<double> durations, double time)
    {
        int count = 0;
        foreach (double d in durations)
        {
            if (d >= time)
            {
                count++;
            }
        }

        return count;
    }

    private static int EventsAt(
        ReadOnlySpan<double> durations, ReadOnlySpan<bool> observed, double time)
    {
        int count = 0;
        for (int i = 0; i < durations.Length; i++)
        {
            // S1244: as in RiskTable — a tie is the same recorded duration, and the
            // times compared against come from these very spans, so they are equal or
            // they are different events.
#pragma warning disable S1244
            if (observed[i] && durations[i] == time)
#pragma warning restore S1244
            {
                count++;
            }
        }

        return count;
    }
}
