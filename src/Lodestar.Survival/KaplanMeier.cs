using Lodestar.Stats;

namespace Lodestar.Survival;

/// <summary>The Kaplan-Meier estimator of a survival function.</summary>
/// <remarks>
/// Reference behavior: <c>lifelines.KaplanMeierFitter</c> 0.30.3. Right-censored data
/// only; left truncation and interval censoring are each their own lot. Thread-safe.
/// </remarks>
public static class KaplanMeier
{
    /// <summary>The default confidence level, which is also lifelines'.</summary>
    private const double DefaultLevel = 0.95;

    /// <summary>Estimates the survival function of a right-censored sample.</summary>
    /// <param name="durations">One duration per subject; non-negative.</param>
    /// <param name="eventObserved">
    /// <c>true</c> where the duration ends in the event, <c>false</c> where the subject
    /// was censored at it.
    /// </param>
    /// <param name="confidenceLevel">A level strictly inside <c>(0, 1)</c>.</param>
    /// <exception cref="ArgumentException">The spans differ in length, the sample is empty, or a duration is negative or NaN.</exception>
    /// <exception cref="ArgumentOutOfRangeException"><paramref name="confidenceLevel"/> is not strictly inside <c>(0, 1)</c>.</exception>
    /// <remarks>
    /// The estimate is the running product of <c>1 - d/n</c> over the times carrying an
    /// event. A time carrying only censorings leaves it where it was and still appears as
    /// a step, because the risk set moved even though the curve did not.
    /// </remarks>
    public static KaplanMeierCurve Estimate(
        ReadOnlySpan<double> durations,
        ReadOnlySpan<bool> eventObserved,
        double confidenceLevel = DefaultLevel)
    {
        RiskTable.Validate(durations, eventObserved, nameof(durations));
        if (double.IsNaN(confidenceLevel) || confidenceLevel <= 0.0 || confidenceLevel >= 1.0)
        {
            throw new ArgumentOutOfRangeException(
                nameof(confidenceLevel), confidenceLevel,
                "A confidence level lies strictly inside (0, 1).");
        }

        SurvivalStep[] steps = RiskTable.Build(durations, eventObserved);
        double[] survival = new double[steps.Length];
        double[] lower = new double[steps.Length];
        double[] upper = new double[steps.Length];

        double product = 1.0;
        // Greenwood's sum, accumulated alongside the product: the variance of the
        // estimate is S² times this, and the log-log interval needs the sum alone.
        double greenwood = 0.0;
        double z = Critical(confidenceLevel);

        for (int i = 0; i < steps.Length; i++)
        {
            SurvivalStep step = steps[i];
            if (step.Events > 0 && step.AtRisk > 0)
            {
                product *= 1.0 - ((double)step.Events / step.AtRisk);
                int denominator = step.AtRisk * (step.AtRisk - step.Events);
                if (denominator > 0)
                {
                    greenwood += (double)step.Events / denominator;
                }
                else
                {
                    // The last subjects all had the event: the product is zero, so the
                    // increment is not finite and the interval below is degenerate.
                    greenwood = double.PositiveInfinity;
                }
            }

            survival[i] = product;
            (lower[i], upper[i]) = LogLogInterval(product, greenwood, z);
        }

        return new KaplanMeierCurve(steps, survival, lower, upper, confidenceLevel);
    }

    /// <summary>The two-sided normal critical value for a level.</summary>
    /// <remarks>
    /// The published normal quantile, not a Student one at a large degrees of freedom.
    /// That substitute was tried and measured: its accuracy peaks near 1e8 degrees of
    /// freedom at about 9e-9 and worsens on either side, which the log-log transform
    /// below amplifies into the seventh digit of a bound — decision 0098 has the table,
    /// and this corpus is what caught it.
    /// </remarks>
    private static double Critical(double level) =>
        Distributions.NormalQuantile(1.0 - ((1.0 - level) / 2.0));

    /// <summary>The interval lifelines reports: built on <c>log(-log S)</c>, not on <c>S</c>.</summary>
    /// <remarks>
    /// The transform keeps both bounds inside <c>[0, 1]</c>, which the plain Greenwood
    /// interval does not — at <c>S = 0.857</c> on 21 subjects it reaches 1.0067. Where the
    /// estimate is one or zero the transform is undefined; lifelines answers the estimate
    /// itself at one and NaN at zero, and so does this.
    /// </remarks>
    private static (double Lower, double Upper) LogLogInterval(
        double survival, double greenwood, double z)
    {
        if (survival >= 1.0)
        {
            return (1.0, 1.0);
        }

        if (survival <= 0.0 || double.IsInfinity(greenwood))
        {
            // The transform is undefined where the curve has reached zero, and both
            // bounds collapse onto the estimate — 0 rather than NaN, as lifelines has it.
            return (0.0, 0.0);
        }

        double logS = Math.Log(survival);
        // The standard error of log(-log S): Greenwood's sum, scaled by |log S|.
        double se = Math.Sqrt(greenwood) / Math.Abs(logS);
        double centre = Math.Log(-logS);
        double lowerPoint = centre - (z * se);
        double upperPoint = centre + (z * se);

        // exp(-exp(.)) is decreasing, so the larger point is the lower bound.
        return (Math.Exp(-Math.Exp(upperPoint)), Math.Exp(-Math.Exp(lowerPoint)));
    }
}
