using Lodestar.Survival.Internal;

namespace Lodestar.Survival;

/// <summary>Harrell's concordance index on any predicted scores, <c>lifelines.utils.concordance_index</c>.</summary>
/// <remarks>Reference behavior: lifelines 0.30.3, whose ties and censoring rules <see cref="CoxSummary.ConcordanceIndex"/> already follows. Thread-safe.</remarks>
public static class Concordance
{
    /// <summary>The concordance of scores with fully observed times: every subject had the event.</summary>
    /// <param name="eventTimes">One duration per subject.</param>
    /// <param name="predictedScores">One score per subject, higher for a longer predicted survival.</param>
    /// <returns>The share of comparable pairs the scores order correctly, a tie counted one half.</returns>
    /// <exception cref="ArgumentException">The spans differ in length, a value is NaN, or no pair is comparable.</exception>
    public static double Index(ReadOnlySpan<double> eventTimes, ReadOnlySpan<double> predictedScores)
    {
        var observed = new bool[eventTimes.Length];
        observed.AsSpan().Fill(true);
        return Index(eventTimes, predictedScores, observed);
    }

    /// <summary>The concordance of scores with right-censored times.</summary>
    /// <param name="eventTimes">One duration per subject.</param>
    /// <param name="predictedScores">One score per subject, higher for a longer predicted survival.</param>
    /// <param name="eventObserved">One flag per subject, <see langword="true"/> where its duration ends in the event.</param>
    /// <returns>The share of comparable pairs the scores order correctly, a tie counted one half.</returns>
    /// <exception cref="ArgumentException">The spans differ in length, a value is NaN, or no pair is comparable.</exception>
    /// <remarks>
    /// A pair is comparable when the shorter duration ends in an event, and two events at one duration are not.
    /// The scores read as predicted survival, so a hazard or a risk score is passed negated, as lifelines asks;
    /// <see cref="CoxSummary.ConcordanceIndex"/> is this index on the negated linear predictor.
    /// </remarks>
    public static double Index(
        ReadOnlySpan<double> eventTimes, ReadOnlySpan<double> predictedScores, ReadOnlySpan<bool> eventObserved)
    {
        int count = eventTimes.Length;
        if (predictedScores.Length != count || eventObserved.Length != count)
        {
            throw new ArgumentException(
                $"One score and one flag are needed per time; got {count} times, {predictedScores.Length} scores and "
                + $"{eventObserved.Length} flags.",
                nameof(predictedScores));
        }

        var negated = new double[count];
        for (int i = 0; i < count; i++)
        {
            if (double.IsNaN(eventTimes[i]) || double.IsNaN(predictedScores[i]))
            {
                throw new ArgumentException($"Subject {i} holds a NaN, which orders against nothing.", nameof(eventTimes));
            }

            negated[i] = -predictedScores[i];
        }

        double index = HarrellConcordance.Index(eventTimes, negated, eventObserved);
        if (double.IsNaN(index))
        {
            throw new ArgumentException(
                "No pair is comparable: no subject's event precedes another subject's time.", nameof(eventObserved));
        }

        return index;
    }
}
