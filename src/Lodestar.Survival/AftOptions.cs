namespace Lodestar.Survival;

/// <summary>What an <see cref="AcceleratedFailureTime"/> fit may be told.</summary>
/// <remarks>Each setting is checked where it is set, as <see cref="CoxOptions"/> checks its own.</remarks>
public sealed record AftOptions
{
    private double _confidenceLevel = 0.95;
    private int _maximumIterations = 100;
    private double _penalizer;

    /// <summary>The two-sided level the intervals are reported at. Default 0.95.</summary>
    /// <exception cref="ArgumentOutOfRangeException">The value does not lie strictly inside (0, 1).</exception>
    public double ConfidenceLevel
    {
        get => _confidenceLevel;
        init
        {
            if (double.IsNaN(value) || value <= 0.0 || value >= 1.0)
            {
                throw new ArgumentOutOfRangeException(nameof(value), value, "A confidence level lies strictly inside (0, 1).");
            }

            _confidenceLevel = value;
        }
    }

    /// <summary>How many Newton steps the fit may take. Default 100.</summary>
    /// <exception cref="ArgumentOutOfRangeException">The value is below one.</exception>
    public int MaximumIterations
    {
        get => _maximumIterations;
        init
        {
            if (value < 1)
            {
                throw new ArgumentOutOfRangeException(nameof(value), value, "An iteration budget is one or more.");
            }

            _maximumIterations = value;
        }
    }

    /// <summary>Whether the primary parameter's linear predictor carries an intercept, lifelines' <c>fit_intercept</c>. Default true.</summary>
    public bool FitIntercept { get; init; } = true;

    /// <summary>Whether the ancillary parameter is modelled by the same covariates, lifelines' <c>ancillary=True</c>. Default false, an intercept alone.</summary>
    public bool Ancillary { get; init; }

    /// <summary>A ridge penalty's weight, lifelines' <c>penalizer</c> at <c>l1_ratio=0</c>. Default zero.</summary>
    /// <remarks>
    /// <c>penalizer · Σ β² / 2</c> on the coefficients of the covariates scaled by their sample deviations, added to
    /// the mean negative log-likelihood, and an intercept is left out when its block holds covariates, as lifelines
    /// leaves it. The L1 part is not written; docs/equivalence.md has why.
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

    /// <summary>Whether the standard errors are the Huber sandwich, lifelines' <c>robust=True</c>. Default false.</summary>
    public bool Robust { get; init; }
}
