namespace Lodestar.Survival;

/// <summary>What a <see cref="CoxProportionalHazards"/> fit may be told.</summary>
/// <remarks>
/// Each setting is checked where it is set, as <c>GlmOptions</c> does, so a budget of zero reaches
/// the caller as an exception naming the setting rather than as a table that never iterated. There
/// is no intercept: the baseline hazard absorbs it. The penalty is lifelines' elastic net on the standardised
/// coefficients, zero by default.
/// </remarks>
public sealed record CoxOptions
{
    private double _confidenceLevel = 0.95;
    private int _maximumIterations = 100;
    private double _penalizer;
    private double _l1Ratio;

    /// <summary>The two-sided level the intervals are reported at. Default 0.95.</summary>
    /// <exception cref="ArgumentOutOfRangeException">The value does not lie strictly inside (0, 1).</exception>
    public double ConfidenceLevel
    {
        get => _confidenceLevel;
        init
        {
            if (double.IsNaN(value) || value <= 0.0 || value >= 1.0)
            {
                throw new ArgumentOutOfRangeException(
                    nameof(value), value, "A confidence level lies strictly inside (0, 1).");
            }

            _confidenceLevel = value;
        }
    }

    /// <summary>How many Newton-Raphson iterations are allowed. Default 100.</summary>
    /// <remarks>An L1 penalty runs lifelines' own loop, which keeps lifelines' limits, 500 steps and 50 for the time-varying fit, and does not read this.</remarks>
    /// <exception cref="ArgumentOutOfRangeException">The value is below one.</exception>
    public int MaximumIterations
    {
        get => _maximumIterations;
        init
        {
            if (value < 1)
            {
                throw new ArgumentOutOfRangeException(
                    nameof(value), value, "An iteration budget is one or more.");
            }

            _maximumIterations = value;
        }
    }

    /// <summary>The penalty's weight, lifelines' <c>penalizer</c>. Default zero, no penalty.</summary>
    /// <remarks>
    /// The penalty is <c>n · penalizer · (l1 · |β| + (1 − l1) · β² / 2)</c> summed over the coefficients of the
    /// covariates standardised by their sample standard deviation, as lifelines fits them, so it does not depend
    /// on the units a covariate is recorded in.
    /// </remarks>
    /// <exception cref="ArgumentOutOfRangeException">The value is negative, infinite or not a number.</exception>
    public double Penalizer
    {
        get => _penalizer;
        init
        {
            if (!(value >= 0.0) || double.IsInfinity(value))
            {
                throw new ArgumentOutOfRangeException(nameof(value), value, "A penalizer is a non-negative, finite weight.");
            }

            _penalizer = value;
        }
    }

    /// <summary>The share of the penalty that is L1, lifelines' <c>l1_ratio</c>. Default zero, a ridge penalty.</summary>
    /// <remarks>
    /// Above zero the fit is lifelines' own: a Newton loop over a smoothed absolute value it sharpens at every step,
    /// whose answer is where that loop stops rather than an optimum, reproduced step for step.
    /// </remarks>
    /// <exception cref="ArgumentOutOfRangeException">The value lies outside [0, 1].</exception>
    public double L1Ratio
    {
        get => _l1Ratio;
        init
        {
            if (!(value >= 0.0 && value <= 1.0))
            {
                throw new ArgumentOutOfRangeException(nameof(value), value, "An L1 ratio lies in [0, 1].");
            }

            _l1Ratio = value;
        }
    }

    /// <summary>Whether the standard errors are the Huber sandwich, lifelines' <c>robust=True</c>. Default false.</summary>
    /// <remarks>Clusters passed to the fit force it, as lifelines' <c>cluster_col</c> does.</remarks>
    public bool Robust { get; init; }
}
