namespace Lodestar.Stats.Regression;

/// <summary>The link a <see cref="GeneralizedLinearModel"/> maps its linear predictor to the mean through.</summary>
/// <remarks>
/// A second axis beside <see cref="GlmFamily"/>, chosen on <see cref="GlmOptions.Link"/>. Not every family takes
/// every link here: <c>GeneralizedLinearModel.Fit</c> refuses a pairing it does not fit rather than
/// fitting one statsmodels would warn about (#770).
/// </remarks>
public enum GlmLink
{
    /// <summary>The family's statsmodels default: logit for binomial, log for the two count families, inverse for Gamma.</summary>
    Default = 0,

    /// <summary><c>η = log μ</c>. The default for Poisson and the negative binomial, and accepted for Gamma.</summary>
    Log = 1,

    /// <summary><c>η = 1/μ</c>, statsmodels' <c>InversePower</c>. The default for Gamma.</summary>
    Inverse = 2,
}
