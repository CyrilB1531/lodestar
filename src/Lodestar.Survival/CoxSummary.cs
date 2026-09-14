namespace Lodestar.Survival;

/// <summary>What a Cox proportional hazards fit reports, at <c>lifelines</c> parity.</summary>
/// <remarks>
/// The per-coefficient lists are parallel and in the design's column order. There is no iteration
/// count: the reference reaches its answer under a different stopping rule, so no oracle could
/// check one, and a fit that did not converge throws instead of returning.
/// </remarks>
public sealed class CoxSummary
{
    /// <summary>Built by <see cref="CoxProportionalHazards.Fit"/> alone; there is no other way to hold one.</summary>
    internal CoxSummary()
    {
    }

    /// <summary>The log hazard ratios, one per column of the design.</summary>
    public IReadOnlyList<double> Coefficients { get; init; } = [];

    /// <summary>The square roots of the inverse observed information's diagonal.</summary>
    public IReadOnlyList<double> StandardErrors { get; init; } = [];

    /// <summary>Each coefficient over its standard error.</summary>
    public IReadOnlyList<double> ZStatistics { get; init; } = [];

    /// <summary>Two-sided, from the normal tail.</summary>
    public IReadOnlyList<double> PValues { get; init; } = [];

    /// <summary>The lower end of each coefficient's interval, at <see cref="ConfidenceLevel"/>.</summary>
    public IReadOnlyList<double> ConfidenceLower { get; init; } = [];

    /// <summary>The upper end of each coefficient's interval.</summary>
    public IReadOnlyList<double> ConfidenceUpper { get; init; } = [];

    /// <summary>The exponential of each coefficient: how much a unit of the covariate multiplies the hazard.</summary>
    public IReadOnlyList<double> HazardRatios { get; init; } = [];

    /// <summary>The exponential of <see cref="ConfidenceLower"/>.</summary>
    public IReadOnlyList<double> HazardRatioLower { get; init; } = [];

    /// <summary>The exponential of <see cref="ConfidenceUpper"/>.</summary>
    public IReadOnlyList<double> HazardRatioUpper { get; init; } = [];

    /// <summary>The log partial likelihood at the fitted coefficients, with Efron's handling of ties.</summary>
    public double LogLikelihood { get; init; }

    /// <summary>The same at every coefficient zero: the model with no covariate.</summary>
    public double NullLogLikelihood { get; init; }

    /// <summary><c>2 · (LogLikelihood − NullLogLikelihood)</c>.</summary>
    public double LikelihoodRatioStatistic { get; init; }

    /// <summary>The chi-squared upper tail of the likelihood-ratio statistic.</summary>
    public double LikelihoodRatioPValue { get; init; }

    /// <summary>The number of covariates.</summary>
    public int LikelihoodRatioDegreesOfFreedom { get; init; }

    /// <summary>Harrell's concordance index between the durations and the negated linear predictor.</summary>
    public double ConcordanceIndex { get; init; }

    /// <summary>The level the intervals were built at.</summary>
    public double ConfidenceLevel { get; init; }
}
