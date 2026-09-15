namespace Lodestar.Stats.Regression;

/// <summary>How <see cref="OrdinaryLeastSquares"/> estimates the covariance of its estimates.</summary>
/// <remarks>
/// The default assumes the errors share one variance; when they do not, the coefficients stay
/// unbiased and their standard errors stop being trustworthy. <strong>Choosing anything else
/// also changes the distribution the tests are read against</strong>, as it does in
/// <c>statsmodels</c>: the coefficients get <em>z</em> against the normal, the overall test keeps
/// the F, and <see cref="OlsSummary.CovarianceType"/> says which was used. Decision 0115 has why
/// that asymmetry is reproduced rather than tidied (#686).
/// </remarks>
public enum CovarianceType
{
    /// <summary>One variance for every error: <c>σ²(XᵀX)⁻¹</c>, and Student's t.</summary>
    Nonrobust = 0,

    /// <summary>White's estimator, weighting each row by its squared residual.</summary>
    Hc0 = 1,

    /// <summary><see cref="Hc0"/> scaled by <c>n / (n - k)</c>, the degrees-of-freedom correction.</summary>
    Hc1 = 2,

    /// <summary>Each squared residual divided by <c>1 - hᵢᵢ</c>, its own leverage.</summary>
    Hc2 = 3,

    /// <summary>Divided by <c>(1 - hᵢᵢ)²</c> — the jackknife approximation, and the conservative one.</summary>
    Hc3 = 4,

    /// <summary>Newey and West's estimator for errors correlated along the row order: the scores of rows up to <see cref="OlsOptions.HacLags"/> apart enter with Bartlett weights.</summary>
    /// <remarks>
    /// Rows are read as a time series in the order given. <c>statsmodels</c>' <c>cov_type="HAC"</c> with its default
    /// kernel; <see cref="OlsOptions.SmallSampleCorrection"/> scales by <c>n / (n - k)</c> and is off by default, as there.
    /// </remarks>
    Hac = 5,

    /// <summary>One-way cluster-robust: errors free to correlate inside a cluster and independent across clusters.</summary>
    /// <remarks>
    /// Needs the overload taking one cluster label per row. <see cref="OlsOptions.SmallSampleCorrection"/> scales by
    /// <c>G / (G - 1) · (n - 1) / (n - k)</c> and is on by default, and the overall F test reads <c>G - 1</c> denominator
    /// degrees of freedom — both as <c>statsmodels</c>' <c>cov_type="cluster"</c> does.
    /// </remarks>
    Cluster = 6,
}
