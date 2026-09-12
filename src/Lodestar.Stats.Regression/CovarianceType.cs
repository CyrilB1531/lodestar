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
}
