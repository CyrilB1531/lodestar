namespace Lodestar.Stats.Regression.Instrumental;

/// <summary>How an instrumental-variables fit estimates the covariance of its coefficients: <c>linearmodels</c>' four.</summary>
/// <remarks>
/// Its own type rather than <c>CovarianceType</c>, whose HC1 to HC3 and <c>statsmodels</c> semantics have no counterpart
/// here. Under every choice but <see cref="Unadjusted"/> the coefficients are read against the normal, and against
/// Student's t only when <c>IvOptions.Debiased</c> is set, as in the reference.
/// </remarks>
public enum IvCovarianceType
{
    /// <summary>One variance for every error: <c>s²·(x̂ᵀx̂)⁻¹</c>, the reference's <c>"unadjusted"</c>.</summary>
    Unadjusted = 0,

    /// <summary>White's estimator over the projected regressors, <c>"robust"</c>, and the reference's default.</summary>
    Robust = 1,

    /// <summary>A heteroskedasticity- and autocorrelation-consistent estimator, rows read as a time series, <c>"kernel"</c>.</summary>
    /// <remarks>The kernel is <c>IvOptions.Kernel</c>; the bandwidth is <c>IvOptions.Bandwidth</c>, or Newey and West's automatic choice.</remarks>
    Kernel = 2,

    /// <summary>One-way cluster-robust, <c>"clustered"</c>: needs the overload taking one cluster label per row.</summary>
    /// <remarks>The small-sample factor <c>G/(G−1)·(n−1)/(n−k)</c> applies only with <c>IvOptions.Debiased</c>, as in the reference.</remarks>
    Clustered = 3,
}
