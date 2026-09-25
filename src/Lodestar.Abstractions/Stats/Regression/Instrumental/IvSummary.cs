namespace Lodestar.Stats.Regression.Instrumental;

/// <summary>An instrumental-variables fit's inference table and diagnostics, as <c>linearmodels</c> reports them.</summary>
/// <remarks>
/// Every per-coefficient list runs in one order: the constant when <c>IvOptions.WithIntercept</c> added it, the
/// exogenous regressors, then the endogenous ones.
/// </remarks>
public sealed class IvSummary
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
    public IvCovarianceType CovarianceType { get; init; }

    /// <summary>Whether the small-sample scaling and the t and F readings were used.</summary>
    public bool Debiased { get; init; }

    /// <summary>The kernel covariance's bandwidth, as given or as chosen; <see langword="null"/> under any other covariance.</summary>
    public int? Bandwidth { get; init; }

    /// <summary>The level of the confidence intervals.</summary>
    public double ConfidenceLevel { get; init; }

    /// <summary>Whether the regressors hold a constant, which is what centres <see cref="RSquared"/>.</summary>
    public bool HasConstant { get; init; }

    /// <summary><c>1 − RSS/TSS</c>, the total sum of squares centred when <see cref="HasConstant"/>.</summary>
    public double RSquared { get; init; }

    /// <summary><see cref="RSquared"/> adjusted for the coefficients spent.</summary>
    public double AdjustedRSquared { get; init; }

    /// <summary>The joint test that every coefficient but the constant is zero: a Wald χ², or an F when debiased.</summary>
    /// <remarks><see langword="null"/> when the model has no coefficient but a constant, where the reference reports the test as invalid.</remarks>
    public WaldTest? ModelTest { get; init; }

    /// <summary><c>n − k</c>.</summary>
    public int ResidualDegreesOfFreedom { get; init; }

    /// <summary>The <c>k</c>-class parameter: 1 for 2SLS, the smallest eigenvalue less Fuller's term for LIML, <see langword="null"/> for GMM.</summary>
    public double? Kappa { get; init; }

    /// <summary>One row per endogenous regressor.</summary>
    public IReadOnlyList<IvFirstStage> FirstStage { get; init; } = [];

    /// <summary>Sargan's test for 2SLS and LIML, Hansen's J for GMM; <see langword="null"/> when the model is just identified.</summary>
    public WaldTest? Overidentification { get; init; }
}
