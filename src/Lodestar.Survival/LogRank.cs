using Lodestar.Stats;

namespace Lodestar.Survival;

/// <summary>The two-sample log-rank test.</summary>
/// <remarks>
/// Reference behavior: <c>lifelines.statistics.logrank_test</c> 0.30.3, which is the
/// Mantel-Haenszel form with the hypergeometric variance under ties. Its p-value is a
/// chi-squared upper tail on one degree of freedom, taken from
/// <see cref="Distributions.ChiSquaredSf"/> rather than re-derived here — decision 0003
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
        var armA = new SortedArm(durationsA, eventObservedA);
        var armB = new SortedArm(durationsB, eventObservedB);

        double observedMinusExpected = 0.0;
        double variance = 0.0;

        // Both arms sorted once and walked forward with the times, so each distinct time costs
        // the ties at it rather than two full passes per arm: O(n log n) where it was O(n²).
        foreach (double time in times)
        {
            (int riskA, int eventsA) = armA.Advance(time);
            (int riskB, int eventsB) = armB.Advance(time);

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
        var times = new List<double>(durationsA.Length + durationsB.Length);
        Collect(times, durationsA, eventsA);
        Collect(times, durationsB, eventsB);
        times.Sort();

        // Adjacent duplicates removed in place: the ascending distinct times a sorted set gave.
        int kept = 0;
        for (int i = 0; i < times.Count; i++)
        {
            // S1244: equal recorded durations are the same time, as in RiskTable.
#pragma warning disable S1244
            if (kept == 0 || times[i] != times[kept - 1])
#pragma warning restore S1244
            {
                times[kept++] = times[i];
            }
        }

        return [.. times.GetRange(0, kept)];
    }

    private static void Collect(
        List<double> times, ReadOnlySpan<double> durations, ReadOnlySpan<bool> observed)
    {
        for (int i = 0; i < durations.Length; i++)
        {
            if (observed[i])
            {
                times.Add(durations[i]);
            }
        }
    }

    /// <summary>One arm sorted by duration, read forward as the times rise.</summary>
    private sealed class SortedArm
    {
        private readonly double[] _durations;
        private readonly bool[] _observed;
        private int _passed;

        public SortedArm(ReadOnlySpan<double> durations, ReadOnlySpan<bool> observed)
        {
            _durations = durations.ToArray();
            _observed = observed.ToArray();
            Array.Sort(_durations, _observed);
        }

        /// <summary>How many are still at risk at <paramref name="time"/>, and how many have an event there.</summary>
        /// <remarks>Times must arrive ascending; the subjects before one are never read again.</remarks>
        public (int AtRisk, int Events) Advance(double time)
        {
            while (_passed < _durations.Length && _durations[_passed] < time)
            {
                _passed++;
            }

            int events = 0;
            // S1244: as in RiskTable — a tie is the same recorded duration.
#pragma warning disable S1244
            for (int i = _passed; i < _durations.Length && _durations[i] == time; i++)
#pragma warning restore S1244
            {
                if (_observed[i])
                {
                    events++;
                }
            }

            return (_durations.Length - _passed, events);
        }
    }
}
