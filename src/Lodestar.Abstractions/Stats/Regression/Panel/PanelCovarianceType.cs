namespace Lodestar.Stats.Regression.Panel;

/// <summary>How a panel fit estimates the covariance of its coefficients: <c>linearmodels</c>' four.</summary>
/// <remarks>
/// The coefficients are read against Student's t and the model tests against F while <c>PanelOptions.Debiased</c>
/// holds, which it does by default, as in the reference; otherwise against the normal and χ².
/// </remarks>
public enum PanelCovarianceType
{
    /// <summary>One variance for every error: <c>s²·(XᵀX)⁻¹</c>, the reference's <c>"unadjusted"</c> and default.</summary>
    Unadjusted = 0,

    /// <summary>White's estimator, <c>"robust"</c>.</summary>
    Robust = 1,

    /// <summary>One- or two-way cluster-robust, <c>"clustered"</c>, by entity, period or a label of the caller's.</summary>
    /// <remarks>No <c>G/(G−1)</c> factor, as in the reference; with no cluster named, each row is its own.</remarks>
    Clustered = 2,

    /// <summary>Driscoll and Kraay's estimator, <c>"kernel"</c>: the scores summed by period, then a kernel over periods.</summary>
    Kernel = 3,
}
