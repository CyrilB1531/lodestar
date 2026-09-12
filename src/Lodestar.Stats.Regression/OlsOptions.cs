namespace Lodestar.Stats.Regression;

/// <summary>What an ordinary least-squares fit should estimate, and at what confidence.</summary>
public sealed record OlsOptions
{
    private double _confidenceLevel = 0.95;

    /// <summary>Whether to fit an intercept, as <c>statsmodels.api.add_constant</c> would.</summary>
    /// <remarks>
    /// <see langword="true"/> by default. Turning it off does more than drop a coefficient:
    /// <see cref="OlsSummary.RSquared"/> becomes the <em>uncentred</em> one and the overall
    /// F test gains a degree of freedom, both of which follow statsmodels.
    /// </remarks>
    public bool WithIntercept { get; init; } = true;

    /// <summary>How the covariance of the estimates is estimated; the ordinary one by default.</summary>
    /// <remarks>
    /// Anything but <see cref="CovarianceType.Nonrobust"/> also moves the coefficient tests from
    /// Student's t to the normal, following <c>statsmodels</c>, and the overall F becomes a Wald
    /// statistic on the robust covariance. <see cref="OlsSummary.CovarianceType"/> echoes what was
    /// used, so a reader of the summary alone can tell which distribution its p-values came from.
    /// </remarks>
    public CovarianceType CovarianceType { get; init; } = CovarianceType.Nonrobust;

    /// <summary>The confidence level of the reported intervals; 0.95 by default.</summary>
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
}
