namespace Lodestar.Survival.Internal;

/// <summary>A Kaplan-Meier curve read as lifelines' fitted frames: one row per distinct time, with Greenwood's sum.</summary>
internal static class CurveTimeline
{
    /// <summary>The distinct times, the estimate at each and the cumulative Greenwood sum lifelines keeps.</summary>
    /// <remarks>
    /// A sample with a zero duration holds two steps at time zero, the start and the zero itself; the later one is
    /// the frame's row. lifelines' sum adds <c>d / (n (n − d))</c> per step and replaces the infinite term, where
    /// every subject left has the event, by zero.
    /// </remarks>
    internal static (double[] Times, double[] Survival, double[] Greenwood) Of(KaplanMeierCurve curve)
    {
        SurvivalStep[] steps = curve.Steps;
        if (steps is null || steps.Length == 0 || curve.Survival is null || curve.Survival.Length != steps.Length)
        {
            throw new ArgumentException(
                "The curve needs at least one step and one estimate per step, as KaplanMeier.Estimate builds it.", nameof(curve));
        }

        var times = new List<double>(steps.Length);
        var survival = new List<double>(steps.Length);
        var greenwood = new List<double>(steps.Length);
        double sum = 0.0;
        for (int i = 0; i < steps.Length; i++)
        {
            SurvivalStep step = steps[i];
            double denominator = (double)step.AtRisk * (step.AtRisk - step.Events);
            if (step.Events > 0 && denominator > 0.0)
            {
                sum += step.Events / denominator;
            }

            // S1244: a repeated time is the zero-duration step, the same instant as the start.
#pragma warning disable S1244
            if (times.Count > 0 && times[times.Count - 1] == step.Time)
#pragma warning restore S1244
            {
                survival[survival.Count - 1] = curve.Survival[i];
                greenwood[greenwood.Count - 1] = sum;
                continue;
            }

            times.Add(step.Time);
            survival.Add(curve.Survival[i]);
            greenwood.Add(sum);
        }

        return ([.. times], [.. survival], [.. greenwood]);
    }

    /// <summary>The estimate at <paramref name="time"/> as a step, and Greenwood's sum there linearly interpolated.</summary>
    /// <remarks>lifelines' <c>predict</c> and <c>interpolate_at_times</c>, which is <c>numpy.interp</c>: clamped at both ends.</remarks>
    internal static (double Survival, double Greenwood) At(KaplanMeierCurve curve, double time)
    {
        (double[] times, double[] survival, double[] greenwood) = Of(curve);
        int last = times.Length - 1;
        int at = 0;
        while (at < last && times[at + 1] <= time)
        {
            at++;
        }

        double interpolated = at == last || time <= times[at]
            ? greenwood[at]
            : (((greenwood[at + 1] - greenwood[at]) / (times[at + 1] - times[at])) * (time - times[at])) + greenwood[at];
        return (survival[at], interpolated);
    }
}
