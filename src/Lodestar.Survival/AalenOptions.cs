namespace Lodestar.Survival;

/// <summary>What an <see cref="AalenAdditive"/> fit may be told.</summary>
/// <remarks>Each setting is checked where it is set, as <see cref="CoxOptions"/> checks its own.</remarks>
public sealed record AalenOptions
{
    private double _confidenceLevel = 0.95;
    private double _coefficientPenalizer;
    private double _smoothingPenalizer;

    /// <summary>The two-sided level the cumulative coefficients' bounds are reported at. Default 0.95.</summary>
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

    /// <summary>Whether a baseline column of ones is added after the covariates, lifelines' <c>fit_intercept</c>. Default true.</summary>
    public bool FitIntercept { get; init; } = true;

    /// <summary>A ridge penalty on each time's increments, lifelines' <c>coef_penalizer</c>. Default zero.</summary>
    /// <exception cref="ArgumentOutOfRangeException">The value is negative, infinite or not a number.</exception>
    public double CoefficientPenalizer
    {
        get => _coefficientPenalizer;
        init => _coefficientPenalizer = NonNegative(value);
    }

    /// <summary>A penalty pulling each time's increments towards the previous time's, lifelines' <c>smoothing_penalizer</c>. Default zero.</summary>
    /// <exception cref="ArgumentOutOfRangeException">The value is negative, infinite or not a number.</exception>
    public double SmoothingPenalizer
    {
        get => _smoothingPenalizer;
        init => _smoothingPenalizer = NonNegative(value);
    }

    private static double NonNegative(double value) =>
        value >= 0.0 && !double.IsInfinity(value)
            ? value
            : throw new ArgumentOutOfRangeException(nameof(value), value, "A penalizer is a non-negative, finite weight.");
}
