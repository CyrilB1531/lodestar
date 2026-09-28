namespace Lodestar.Stats.Regression.Panel;

/// <summary>What a panel fit estimates, and how it reports it.</summary>
/// <remarks>
/// The defaults are <c>linearmodels</c>' own: no effects, an unadjusted covariance, debiased tests, Bartlett's kernel at
/// Driscoll and Kraay's default bandwidth. <see cref="WithIntercept"/> has no counterpart there, where a constant is a
/// column the caller supplies; it adds that column, first, and must be turned off for first differences. A setting the
/// chosen estimator and covariance do not read is refused by the fit rather than ignored.
/// </remarks>
public sealed record PanelOptions
{
    /// <summary>Whether a constant column is prepended to the regressors; <see langword="true"/> by default.</summary>
    public bool WithIntercept { get; init; } = true;

    /// <summary>Whether <c>FixedEffects</c> absorbs an effect per entity; <see langword="false"/> by default.</summary>
    public bool EntityEffects { get; init; }

    /// <summary>Whether <c>FixedEffects</c> absorbs an effect per period; <see langword="false"/> by default.</summary>
    public bool TimeEffects { get; init; }

    /// <summary>The covariance of the coefficients; <see cref="PanelCovarianceType.Unadjusted"/> by default.</summary>
    public PanelCovarianceType CovarianceType { get; init; }

    /// <summary>Whether to scale by the residual degrees of freedom and read the tests against t and F; <see langword="true"/> by default.</summary>
    public bool Debiased { get; init; } = true;

    /// <summary>Whether a clustered covariance clusters by entity.</summary>
    public bool ClusterEntity { get; init; }

    /// <summary>Whether a clustered covariance clusters by period.</summary>
    public bool ClusterTime { get; init; }

    /// <summary>The kernel of a <see cref="PanelCovarianceType.Kernel"/> covariance; Bartlett's by default.</summary>
    public KernelType Kernel { get; init; } = KernelType.Bartlett;

    /// <summary>The kernel covariance's bandwidth in periods; <see langword="null"/>, the default, is <c>⌊4(T/100)^(2/9)⌋</c>.</summary>
    public int? Bandwidth { get; init; }

    /// <summary>The level of the confidence intervals, strictly inside (0, 1); 0.95 by default.</summary>
    public double ConfidenceLevel { get; init; } = 0.95;

    /// <summary>Hashes every member as the generated equality compares it, each <c>NaN</c> alike (#1285).</summary>
    public override int GetHashCode()
    {
        unchecked
        {
            int hash = (17 * 31) + ValueEquality.HashOfItem(WithIntercept);
            hash = (hash * 31) + ValueEquality.HashOfItem(EntityEffects);
            hash = (hash * 31) + ValueEquality.HashOfItem(TimeEffects);
            hash = (hash * 31) + ValueEquality.HashOfItem(CovarianceType);
            hash = (hash * 31) + ValueEquality.HashOfItem(Debiased);
            hash = (hash * 31) + ValueEquality.HashOfItem(ClusterEntity);
            hash = (hash * 31) + ValueEquality.HashOfItem(ClusterTime);
            hash = (hash * 31) + ValueEquality.HashOfItem(Kernel);
            hash = (hash * 31) + ValueEquality.HashOfItem(Bandwidth);
            return (hash * 31) + ValueEquality.HashOf(ConfidenceLevel);
        }
    }
}
