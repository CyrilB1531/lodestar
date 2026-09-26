using Lodestar.Stats;
using Lodestar.Survival.Internal;

namespace Lodestar.Survival;

/// <summary>The Breslow-Fleming-Harrington estimator of a survival function: the exponential of minus the Nelson-Aalen hazard.</summary>
/// <remarks>
/// Reference behavior: <c>lifelines.BreslowFlemingHarringtonFitter</c> 0.30.3, right-censored with delayed entry. Its
/// fitter passes no weights on to the Nelson-Aalen fit it wraps, so there are none here. Thread-safe.
/// </remarks>
public static class BreslowFlemingHarrington
{
    private const double DefaultLevel = 0.95;

    /// <summary>Estimates the survival function of a right-censored sample.</summary>
    /// <param name="durations">One duration per subject; non-negative.</param>
    /// <param name="eventObserved"><c>true</c> where the duration ends in the event, <c>false</c> where the subject was censored at it.</param>
    /// <param name="confidenceLevel">A level strictly inside <c>(0, 1)</c>.</param>
    /// <returns>The curve at time zero and at every duration, with its bounds.</returns>
    /// <exception cref="ArgumentException">The spans differ in length, the sample is empty, or a duration is negative or NaN.</exception>
    /// <exception cref="ArgumentOutOfRangeException"><paramref name="confidenceLevel"/> is not strictly inside <c>(0, 1)</c>.</exception>
    public static SurvivalCurve Estimate(
        ReadOnlySpan<double> durations, ReadOnlySpan<bool> eventObserved, double confidenceLevel = DefaultLevel) =>
        Estimate(durations, eventObserved, [], confidenceLevel);

    /// <summary>Estimates the survival function of a right-censored sample whose subjects may enter late, lifelines' <c>entry</c>.</summary>
    /// <param name="durations">One duration per subject; non-negative.</param>
    /// <param name="eventObserved"><c>true</c> where the duration ends in the event, <c>false</c> where the subject was censored at it.</param>
    /// <param name="entries">Each subject's entry time, non-negative and at most its duration; empty for everyone entering at zero.</param>
    /// <param name="confidenceLevel">A level strictly inside <c>(0, 1)</c>.</param>
    /// <returns>The curve at every duration and entry time, with its bounds.</returns>
    /// <exception cref="ArgumentException">As the overload without entries, or the entries are neither empty nor one valid time per subject.</exception>
    /// <exception cref="ArgumentOutOfRangeException"><paramref name="confidenceLevel"/> is not strictly inside <c>(0, 1)</c>.</exception>
    /// <remarks>
    /// long-comment: lifelines' risk set with entrants, and its bounds' labels.
    /// A step with <c>d</c> events among <c>n</c> adds <c>1/n + … + 1/(n − d + 1)</c> to the hazard and the squares of
    /// those to its variance, and the bounds are <c>exp(−H e^{±z√V/H})</c>. As lifelines counts them, subjects entering
    /// at a time are not at risk of its events, except at the first step. <see cref="SurvivalStep.AtRisk"/> reports them
    /// with the entrants, lifelines' <c>at_risk</c>. lifelines labels the larger bound its lower one, being the image
    /// of the hazard's lower bound; <see cref="SurvivalCurve.Lower"/> is the smaller.
    /// </remarks>
    public static SurvivalCurve Estimate(
        ReadOnlySpan<double> durations, ReadOnlySpan<bool> eventObserved, ReadOnlySpan<double> entries, double confidenceLevel = DefaultLevel)
    {
        RiskTable.Validate(durations, eventObserved, nameof(durations));
        if (double.IsNaN(confidenceLevel) || confidenceLevel <= 0.0 || confidenceLevel >= 1.0)
        {
            throw new ArgumentOutOfRangeException(nameof(confidenceLevel), confidenceLevel, "A confidence level lies strictly inside (0, 1).");
        }

        double[] entered = Entries(entries, durations);
        List<(double Time, int Removed, int Events, int Entrants)> rows = Rows(durations, eventObserved, entered);
        double z = Distributions.NormalQuantile(1.0 - ((1.0 - confidenceLevel) / 2.0));
        var steps = new SurvivalStep[rows.Count];
        var survival = new double[rows.Count];
        var lower = new double[rows.Count];
        var upper = new double[rows.Count];
        double hazard = 0.0;
        double variance = 0.0;
        int atRisk = 0;
        for (int i = 0; i < rows.Count; i++)
        {
            (double time, int removed, int events, int entrants) = rows[i];
            atRisk += entrants;
            int population = i == 0 ? atRisk : atRisk - entrants;
            for (int e = 0; e < events && population - e > 0; e++)
            {
                double increment = 1.0 / (population - e);
                hazard += increment;
                variance += increment * increment;
            }

            steps[i] = new SurvivalStep(time, atRisk, events, removed - events);
            survival[i] = Math.Exp(-hazard);
            double spread = hazard > 0.0 ? Math.Exp(z * Math.Sqrt(variance) / hazard) : 1.0;
            lower[i] = Math.Exp(-hazard * spread);
            upper[i] = Math.Exp(-hazard / spread);
            atRisk -= removed;
        }

        return new SurvivalCurve(steps, survival, lower, upper, confidenceLevel);
    }

    private static double[] Entries(ReadOnlySpan<double> entries, ReadOnlySpan<double> durations)
    {
        if (entries.IsEmpty)
        {
            return new double[durations.Length];
        }

        if (entries.Length != durations.Length)
        {
            throw new ArgumentException($"entries holds {entries.Length} values for {durations.Length} subjects.", nameof(entries));
        }

        for (int i = 0; i < entries.Length; i++)
        {
            if (!(entries[i] >= 0.0) || entries[i] > durations[i])
            {
                throw new ArgumentException(
                    $"An entry time is non-negative and at most the subject's duration; entries[{i}] is {entries[i]}.", nameof(entries));
            }
        }

        return entries.ToArray();
    }

    /// <summary>lifelines' <c>survival_table_from_events</c>: one row per distinct duration or entry time, ascending.</summary>
    private static List<(double Time, int Removed, int Events, int Entrants)> Rows(
        ReadOnlySpan<double> durations, ReadOnlySpan<bool> eventObserved, double[] entries)
    {
        var table = new SortedDictionary<double, (int Removed, int Events, int Entrants)>();
        for (int i = 0; i < durations.Length; i++)
        {
            table.TryGetValue(durations[i], out var death);
            table[durations[i]] = (death.Removed + 1, death.Events + (eventObserved[i] ? 1 : 0), death.Entrants);
            table.TryGetValue(entries[i], out var birth);
            table[entries[i]] = (birth.Removed, birth.Events, birth.Entrants + 1);
        }

        return [.. table.Select(row => (row.Key, row.Value.Removed, row.Value.Events, row.Value.Entrants))];
    }
}
