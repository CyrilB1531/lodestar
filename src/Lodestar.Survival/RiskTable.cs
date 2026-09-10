namespace Lodestar.Survival;

/// <summary>The at-risk table both estimators are built on.</summary>
/// <remarks>
/// Kaplan-Meier and Nelson-Aalen differ only in what they accumulate over this table —
/// a product of survival fractions against a sum of hazard increments — so it is built
/// once here rather than twice, and the two curves are guaranteed to share a timeline.
/// </remarks>
internal static class RiskTable
{
    /// <summary>Builds the ascending step table, time zero first.</summary>
    /// <remarks>
    /// Censorings get their own step when no event shares the time, because a reader
    /// comparing against lifelines' event table expects to see them; the estimate does
    /// not move there, but the risk set does.
    /// </remarks>
    internal static SurvivalStep[] Build(
        ReadOnlySpan<double> durations, ReadOnlySpan<bool> eventObserved)
    {
        int n = durations.Length;
        double[] sorted = new double[n];
        durations.CopyTo(sorted);
        int[] order = new int[n];
        for (int i = 0; i < n; i++)
        {
            order[i] = i;
        }

        Array.Sort(sorted, order);

        List<SurvivalStep> steps = [new SurvivalStep(0.0, n, 0, 0)];
        int remaining = n;
        int index = 0;
        while (index < n)
        {
            double time = sorted[index];
            int events = 0;
            int censored = 0;
            int atRisk = remaining;
            // S1244: exact equality is the rule here, not an approximation of it. A
            // tie in survival data is the same recorded duration, and a tolerance would
            // merge two distinct times into one step and move the estimate.
#pragma warning disable S1244
            while (index < n && sorted[index] == time)
#pragma warning restore S1244
            {
                if (eventObserved[order[index]])
                {
                    events++;
                }
                else
                {
                    censored++;
                }

                index++;
                remaining--;
            }

            steps.Add(new SurvivalStep(time, atRisk, events, censored));
        }

        return [.. steps];
    }

    /// <summary>Validates the pair of spans every public entry point takes.</summary>
    /// <exception cref="ArgumentException">
    /// The spans differ in length, the sample is empty, or a duration is negative or NaN.
    /// </exception>
    internal static void Validate(
        ReadOnlySpan<double> durations, ReadOnlySpan<bool> eventObserved, string name)
    {
        if (durations.Length != eventObserved.Length)
        {
            throw new ArgumentException(
                "A duration and an event flag are needed for each subject, so the two spans "
                + $"must be the same length; got {durations.Length} and {eventObserved.Length}.",
                name);
        }

        if (durations.Length == 0)
        {
            throw new ArgumentException("A survival curve needs at least one subject.", name);
        }

        for (int i = 0; i < durations.Length; i++)
        {
            double d = durations[i];
            if (double.IsNaN(d) || d < 0.0)
            {
                throw new ArgumentException(
                    $"A duration is a non-negative time; durations[{i}] is {d}.", name);
            }
        }
    }
}
