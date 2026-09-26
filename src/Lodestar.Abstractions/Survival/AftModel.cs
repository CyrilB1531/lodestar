namespace Lodestar.Survival;

/// <summary>Which of lifelines' accelerated failure time regressions <c>AcceleratedFailureTime</c> fits.</summary>
public enum AftModel
{
    /// <summary><c>log λ = Xβ</c> and <c>log ρ</c> an intercept or <c>Xγ</c>, lifelines' <c>WeibullAFTFitter</c>.</summary>
    Weibull,

    /// <summary><c>μ = Xβ</c> and <c>log σ</c> an intercept or <c>Xγ</c>, <c>LogNormalAFTFitter</c>.</summary>
    LogNormal,

    /// <summary><c>log α = Xβ</c> and <c>log β</c> an intercept or <c>Xγ</c>, <c>LogLogisticAFTFitter</c>.</summary>
    LogLogistic,
}
