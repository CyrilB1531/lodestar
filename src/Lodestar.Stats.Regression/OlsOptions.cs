namespace Lodestar.Stats.Regression;

/// <summary>What an ordinary least-squares fit should estimate, and at what confidence.</summary>
public sealed class OlsOptions
{
    private double _confidenceLevel = 0.95;

    /// <summary>Whether to fit an intercept, as <c>statsmodels.api.add_constant</c> would.</summary>
    /// <remarks>
    /// <see langword="true"/> by default. Turning it off does more than drop a coefficient:
    /// <see cref="OlsSummary.RSquared"/> becomes the <em>uncentred</em> one and the overall
    /// F test gains a degree of freedom, both of which follow statsmodels.
    /// </remarks>
    public bool WithIntercept { get; set; } = true;

    /// <summary>The confidence level of the reported intervals; 0.95 by default.</summary>
    /// <exception cref="ArgumentOutOfRangeException">The value does not lie strictly inside (0, 1).</exception>
    public double ConfidenceLevel
    {
        get => _confidenceLevel;
        set
        {
            if (double.IsNaN(value) || value <= 0.0 || value >= 1.0)
            {
                throw new ArgumentOutOfRangeException(
                    nameof(value), value, "A confidence level lies strictly inside (0, 1).");
            }

            _confidenceLevel = value;
        }
    }
}
