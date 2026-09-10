namespace Lodestar.Survival;

/// <summary>The Nelson-Aalen estimator of a cumulative hazard function.</summary>
/// <remarks>
/// Reference behavior: <c>lifelines.NelsonAalenFitter</c> 0.30.3, with its smoothing
/// left off — the plain estimator, which is what the fitter reports by default.
/// Right-censored data only. Thread-safe.
/// </remarks>
public static class NelsonAalen
{
    /// <summary>Estimates the cumulative hazard of a right-censored sample.</summary>
    /// <param name="durations">One duration per subject; non-negative.</param>
    /// <param name="eventObserved">
    /// <c>true</c> where the duration ends in the event, <c>false</c> where the subject
    /// was censored at it.
    /// </param>
    /// <exception cref="ArgumentException">The spans differ in length, the sample is empty, or a duration is negative or NaN.</exception>
    /// <remarks>
    /// A sum of hazard increments rather than a product of survival fractions. The difference shows at
    /// the end of a sample whose last duration is observed: survival reaches zero and can
    /// fall no further, while the hazard keeps the size of that last step.
    /// </remarks>
    public static NelsonAalenCurve Estimate(
        ReadOnlySpan<double> durations, ReadOnlySpan<bool> eventObserved)
    {
        RiskTable.Validate(durations, eventObserved, nameof(durations));

        SurvivalStep[] steps = RiskTable.Build(durations, eventObserved);
        double[] hazard = new double[steps.Length];
        double running = 0.0;

        for (int i = 0; i < steps.Length; i++)
        {
            SurvivalStep step = steps[i];
            if (step.Events > 0 && step.AtRisk > 0)
            {
                // Not d/n: the tie increment sums 1/(n - i) per event, so three among 21
                // give 1/21 + 1/20 + 1/19 = 0.150251 and not 3/21. The page has why.
                for (int e = 0; e < step.Events; e++)
                {
                    running += 1.0 / (step.AtRisk - e);
                }
            }

            hazard[i] = running;
        }

        return new NelsonAalenCurve(steps, hazard);
    }
}
