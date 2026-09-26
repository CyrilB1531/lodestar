using Lodestar.Stats;
using Lodestar.Survival.Internal;

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
                // In double: the product of two counts overflows int past 46,340 at risk.
                double denominator = (double)step.AtRisk * (step.AtRisk - step.Events);
                if (denominator > 0.0)
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

    /// <summary>Tests whether two survival curves differ at one time, lifelines' <c>survival_difference_at_fixed_point_in_time_test</c>.</summary>
    /// <param name="time">The time both curves are read at; non-negative.</param>
    /// <param name="curveA">The first curve, from <see cref="Estimate"/>.</param>
    /// <param name="curveB">The second curve, from <see cref="Estimate"/>.</param>
    /// <returns>The chi-squared statistic on one degree of freedom and its upper-tail p-value.</returns>
    /// <exception cref="ArgumentNullException">A curve is <see langword="null"/>.</exception>
    /// <exception cref="ArgumentException">A curve holds no step, or not one estimate per step.</exception>
    /// <exception cref="ArgumentOutOfRangeException"><paramref name="time"/> is negative or NaN.</exception>
    /// <remarks>
    /// Klein, Logan, Harhoff and Andersen's test on the <c>log(−log S)</c> scale: the squared difference of the two
    /// transformed estimates over the sum of their delta-method variances. As lifelines reads them, each estimate
    /// is the curve's step at <paramref name="time"/> while its Greenwood sum is interpolated linearly between the
    /// times either side, and a step where every subject left at risk has the event adds nothing to that sum.
    /// A curve at one or zero at <paramref name="time"/> leaves the transform undefined, and the answer is NaN, as
    /// lifelines' is.
    /// </remarks>
    public static TestResult CompareAt(double time, KaplanMeierCurve curveA, KaplanMeierCurve curveB)
    {
        Guard.NotNull(curveA);
        Guard.NotNull(curveB);
        if (!(time >= 0.0))
        {
            throw new ArgumentOutOfRangeException(nameof(time), time, "A time is non-negative.");
        }

        (double survivalA, double varianceA) = CurveTimeline.At(curveA, time);
        (double survivalB, double varianceB) = CurveTimeline.At(curveB, time);
        double logA = Math.Log(survivalA);
        double logB = Math.Log(survivalB);
        double gap = Math.Log(-logA) - Math.Log(-logB);
        double statistic = gap * gap / ((varianceA / (logA * logA)) + (varianceB / (logB * logB)));
        return new TestResult(statistic, Distributions.ChiSquaredSf(statistic, 1.0));
    }

    /// <summary>The unrestricted mean survival time of a curve and its variance, lifelines' <c>restricted_mean_survival_time</c> at its default <c>t=inf</c>.</summary>
    /// <param name="curve">The curve, from <see cref="Estimate"/>.</param>
    /// <returns>The whole area under the curve and its variance: infinite unless the curve reaches zero.</returns>
    /// <exception cref="ArgumentNullException"><paramref name="curve"/> is <see langword="null"/>.</exception>
    /// <exception cref="ArgumentException"><paramref name="curve"/> holds no step, or not one estimate per step.</exception>
    public static RestrictedMeanResult RestrictedMean(KaplanMeierCurve curve) =>
        RestrictedMean(curve, double.PositiveInfinity);

    /// <summary>The restricted mean survival time of a curve and its variance, lifelines' <c>restricted_mean_survival_time</c>.</summary>
    /// <param name="curve">The curve, from <see cref="Estimate"/>.</param>
    /// <param name="horizon">The upper limit of the integral, lifelines' <c>t</c>; non-negative, and infinity allowed.</param>
    /// <returns>The area under the curve up to <paramref name="horizon"/>, and its variance.</returns>
    /// <exception cref="ArgumentNullException"><paramref name="curve"/> is <see langword="null"/>.</exception>
    /// <exception cref="ArgumentException"><paramref name="curve"/> holds no step, or not one estimate per step.</exception>
    /// <exception cref="ArgumentOutOfRangeException"><paramref name="horizon"/> is negative or NaN.</exception>
    /// <remarks>
    /// Both integrals are exact sums over the steps: the mean <c>∫ S</c>, and the variance <c>2 ∫ τ S(τ) dτ</c> less
    /// the squared mean, as lifelines defines it. lifelines computes the mean the same way and the second moment
    /// by numerical quadrature of the step function, which landed up to 1.5 % relative from the exact integral over 300 random curves;
    /// the reference page has the measurement. Past the last step a curve above zero never closes its area, so an
    /// infinite horizon gives an infinite mean and variance unless the curve reaches zero.
    /// </remarks>
    public static RestrictedMeanResult RestrictedMean(KaplanMeierCurve curve, double horizon)
    {
        Guard.NotNull(curve);
        if (!(horizon >= 0.0))
        {
            throw new ArgumentOutOfRangeException(nameof(horizon), horizon, "A horizon is a non-negative time.");
        }

        (double[] times, double[] survival, _) = CurveTimeline.Of(curve);
        double mean = 0.0;
        double second = 0.0;
        for (int j = 0; j < times.Length && times[j] <= horizon; j++)
        {
            double end = j + 1 < times.Length && times[j + 1] <= horizon ? times[j + 1] : horizon;
            // S1244: a curve at exactly zero closes the area, and ∞ · 0 must not turn the sum into NaN.
#pragma warning disable S1244
            if (survival[j] == 0.0)
#pragma warning restore S1244
            {
                continue;
            }

            mean += survival[j] * (end - times[j]);
            second += survival[j] * ((end * end) - (times[j] * times[j]));
        }

        double variance = double.IsInfinity(mean) ? double.PositiveInfinity : second - (mean * mean);
        return new RestrictedMeanResult(mean, variance);
    }

    /// <summary>The two-sided normal critical value for a level.</summary>
    /// <remarks>
    /// The published normal quantile, not a Student one at a large degrees of freedom.
    /// That substitute was tried and measured: its accuracy peaks near 1e8 degrees of
    /// freedom at about 1e-8 and worsens on either side, which the log-log transform
    /// below amplifies into the seventh digit of a bound — decisions 0003 and 0121 have
    /// the table, and this corpus is what caught it.
    /// </remarks>
    private static double Critical(double level) =>
        Distributions.NormalQuantile(1.0 - ((1.0 - level) / 2.0));

    /// <summary>The interval lifelines reports: built on <c>log(-log S)</c>, not on <c>S</c>.</summary>
    /// <remarks>
    /// The transform keeps both bounds inside <c>[0, 1]</c>, which the plain Greenwood
    /// interval does not — at <c>S = 0.857</c> on 21 subjects it reaches 1.0067. Where the
    /// estimate is one or zero the transform is undefined, and lifelines answers the estimate
    /// itself at both: one at one, zero at zero. So does this.
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
