namespace Lodestar.Stats.Regression.Panel;

/// <summary>A panel fit's inference table and diagnostics, as <c>linearmodels</c> reports them.</summary>
/// <remarks>Every per-coefficient list runs in one order: the constant when <c>PanelOptions.WithIntercept</c> added it, then the regressors.</remarks>
public sealed class PanelSummary
{
    /// <summary>The estimates.</summary>
    public IReadOnlyList<double> Coefficients { get; init; } = [];

    /// <summary>Their standard errors, under <see cref="CovarianceType"/>.</summary>
    public IReadOnlyList<double> StandardErrors { get; init; } = [];

    /// <summary>Each estimate over its standard error.</summary>
    public IReadOnlyList<double> TStatistics { get; init; } = [];

    /// <summary>Two-sided p-values, against Student's t with <see cref="ResidualDegreesOfFreedom"/> when debiased, else the normal.</summary>
    public IReadOnlyList<double> PValues { get; init; } = [];

    /// <summary>The lower ends of the confidence intervals at <see cref="ConfidenceLevel"/>.</summary>
    public IReadOnlyList<double> ConfidenceLower { get; init; } = [];

    /// <summary>The upper ends of the confidence intervals.</summary>
    public IReadOnlyList<double> ConfidenceUpper { get; init; } = [];

    /// <summary>The covariance the table was computed under.</summary>
    public PanelCovarianceType CovarianceType { get; init; }

    /// <summary>Whether the degrees-of-freedom scaling and the t and F readings were used.</summary>
    public bool Debiased { get; init; }

    /// <summary>The kernel covariance's bandwidth, as given or as defaulted; <see langword="null"/> under any other covariance.</summary>
    public int? Bandwidth { get; init; }

    /// <summary>The level of the confidence intervals.</summary>
    public double ConfidenceLevel { get; init; }

    /// <summary>Whether the regressors hold a constant.</summary>
    public bool HasConstant { get; init; }

    /// <summary>The rows the final regression used: every row, one per entity for <c>Between</c>, one per differenced pair for <c>FirstDifference</c>.</summary>
    public int ObservationCount { get; init; }

    /// <summary>The number of distinct entities.</summary>
    public int EntityCount { get; init; }

    /// <summary>The number of distinct periods.</summary>
    public int PeriodCount { get; init; }

    /// <summary>The residual degrees of freedom: the rows less the coefficients and, for fixed effects, the effects.</summary>
    public int ResidualDegreesOfFreedom { get; init; }

    /// <summary>The R² of the estimated regression, on its own transformed rows.</summary>
    public double RSquared { get; init; }

    /// <summary>The R² of the coefficients applied to the entity-demeaned rows.</summary>
    public double RSquaredWithin { get; init; }

    /// <summary>The R² of the coefficients applied to the entity means.</summary>
    public double RSquaredBetween { get; init; }

    /// <summary>The R² of the coefficients applied to the rows as given.</summary>
    public double RSquaredOverall { get; init; }

    /// <summary>The classical F test that every coefficient but the constant is zero; <see langword="null"/> when there is only a constant.</summary>
    public WaldTest? ModelTest { get; init; }

    /// <summary>The same joint test as a Wald test under <see cref="CovarianceType"/>: an F when debiased, else a χ².</summary>
    public WaldTest? RobustModelTest { get; init; }

    /// <summary>The F test that the fixed effects are zero; <see langword="null"/> without effects and outside <c>FixedEffects</c>.</summary>
    public WaldTest? PoolabilityTest { get; init; }

    /// <summary>The variance of the idiosyncratic error; <see langword="null"/> for <c>Between</c> and <c>FirstDifference</c>.</summary>
    public double? ResidualVariance { get; init; }

    /// <summary>The variance of the effects; <see langword="null"/> for <c>Between</c> and <c>FirstDifference</c>.</summary>
    public double? EffectsVariance { get; init; }

    /// <summary>The share of the variance due to the effects; <see langword="null"/> for <c>Between</c> and <c>FirstDifference</c>.</summary>
    public double? Rho { get; init; }

    /// <summary>Random effects' <c>θ</c>, one per entity in ascending label order; <see langword="null"/> for the other estimators.</summary>
    public IReadOnlyList<double>? Theta { get; init; }
}
