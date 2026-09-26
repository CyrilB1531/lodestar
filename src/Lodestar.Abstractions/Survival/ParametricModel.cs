namespace Lodestar.Survival;

/// <summary>Which of lifelines' parametric univariate models <c>ParametricSurvival</c> fits.</summary>
public enum ParametricModel
{
    /// <summary><c>H(t) = t / λ</c>, lifelines' <c>ExponentialFitter</c>.</summary>
    Exponential,

    /// <summary><c>H(t) = (t / λ)^ρ</c>, <c>WeibullFitter</c>.</summary>
    Weibull,

    /// <summary><c>log T</c> normal with mean <c>μ</c> and deviation <c>σ</c>, <c>LogNormalFitter</c>.</summary>
    LogNormal,

    /// <summary><c>S(t) = 1 / (1 + (t / α)^β)</c>, <c>LogLogisticFitter</c>.</summary>
    LogLogistic,

    /// <summary>A constant hazard <c>1 / λᵢ</c> between consecutive breakpoints, <c>PiecewiseExponentialFitter</c>.</summary>
    PiecewiseExponential,

    /// <summary>The generalized gamma in <c>(μ, log σ, λ)</c>, <c>GeneralizedGammaFitter</c>.</summary>
    GeneralizedGamma,
}
