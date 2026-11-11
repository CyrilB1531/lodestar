namespace Lodestar.Survival.Internal;

/// <summary>The rule the Cox likelihoods share: every risk weight is exp(η − shift), the shift its risk set's largest η, so
/// no sum underflows where the linear predictors spread past 709 (#1756).</summary>
/// <remarks>Efron's likelihood, Schoenfeld's residuals and the robust scores carry their sums from one shift to the next by
/// <see cref="Rescale"/>; the time-varying likelihood, whose risk sets are not nested, scans each one for its largest.</remarks>
internal static class RiskShift
{
    /// <summary>The factor carrying sums held at <paramref name="shift"/> to <paramref name="largest"/>: zero where they hold
    /// nothing yet, one where the shift does not move, as it does not for most rows.</summary>
    public static double Rescale(double shift, double largest)
    {
        if (double.IsNegativeInfinity(shift))
        {
            return 0.0;
        }

        // S1244: an unmoved shift is the same double, and carries exactly one without an exponential.
#pragma warning disable S1244
        return shift == largest ? 1.0 : Math.Exp(shift - largest);
#pragma warning restore S1244
    }
}
