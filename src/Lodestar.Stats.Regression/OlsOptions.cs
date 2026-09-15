using Lodestar.Stats.Regression.Internal;

namespace Lodestar.Stats.Regression;

/// <summary>What an ordinary least-squares fit should estimate, and at what confidence.</summary>
public sealed record OlsOptions
{
    private double _confidenceLevel = 0.95;
    private int? _hacLags;

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

    /// <summary>How many lags <see cref="CovarianceType.Hac"/> reads; required with it and refused with any other type.</summary>
    /// <remarks>
    /// <c>statsmodels</c>' <c>maxlags</c>. Zero is White's HC0; a count at or past the row count is accepted, as there,
    /// and still sets the Bartlett weights <c>1 - l / (L + 1)</c> of the lags that exist.
    /// </remarks>
    /// <exception cref="ArgumentOutOfRangeException">The value is negative.</exception>
    public int? HacLags
    {
        get => _hacLags;
        init
        {
            if (value < 0)
            {
                throw new ArgumentOutOfRangeException(
                    nameof(value), value, "A lag count is zero or more.");
            }

            _hacLags = value;
        }
    }

    /// <summary>Whether <see cref="CovarianceType.Hac"/> or <see cref="CovarianceType.Cluster"/> applies its small-sample correction; <see langword="null"/> takes the reference's default.</summary>
    /// <remarks>
    /// <c>statsmodels</c>' <c>use_correction</c>: off for HAC and on for cluster when left <see langword="null"/>. Refused
    /// with the other types, whose corrections are types of their own (<see cref="CovarianceType.Hc1"/>).
    /// </remarks>
    public bool? SmallSampleCorrection { get; init; }

    /// <summary>The confidence level of the reported intervals; 0.95 by default.</summary>
    /// <exception cref="ArgumentOutOfRangeException">The value does not lie strictly inside (0, 1).</exception>
    public double ConfidenceLevel
    {
        get => _confidenceLevel;
        init => _confidenceLevel = OptionGuards.ConfidenceLevel(value);
    }
}
