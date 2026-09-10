namespace Lodestar.Stats.Regression;

/// <summary>
/// What a fitted ordinary least-squares model reports: the estimates, and how sure it is
/// of each of them.
/// </summary>
/// <remarks>
/// The per-coefficient lists are parallel and in the design's own order, with the intercept
/// first when one was fitted. This is the table
/// <c>statsmodels.regression.linear_model.OLSResults</c> prints, entry for entry.
/// </remarks>
public sealed class OlsSummary
{
    /// <summary>Built by <see cref="OrdinaryLeastSquares.Fit"/> alone; there is no other way to hold one.</summary>
    internal OlsSummary()
    {
    }

    /// <summary>The estimates, intercept first when one was fitted.</summary>
    public IReadOnlyList<double> Coefficients { get; init; } = [];

    /// <summary>The standard error of each estimate.</summary>
    public IReadOnlyList<double> StandardErrors { get; init; } = [];

    /// <summary>Each estimate over its standard error.</summary>
    public IReadOnlyList<double> TStatistics { get; init; } = [];

    /// <summary>The two-sided p-value of each <see cref="TStatistics"/> entry.</summary>
    /// <remarks>Against the null that the coefficient is zero, read on <see cref="ResidualDegreesOfFreedom"/>.</remarks>
    public IReadOnlyList<double> PValues { get; init; } = [];

    /// <summary>The lower end of each interval, at <see cref="ConfidenceLevel"/>.</summary>
    public IReadOnlyList<double> ConfidenceLower { get; init; } = [];

    /// <summary>The upper end of each interval, at <see cref="ConfidenceLevel"/>.</summary>
    public IReadOnlyList<double> ConfidenceUpper { get; init; } = [];

    /// <summary>The variance inflation factor of each <em>regressor</em>, the intercept excluded.</summary>
    /// <remarks>
    /// One entry per column of the caller's design, so this list is shorter than
    /// <see cref="Coefficients"/> by one whenever an intercept was fitted: a constant has no
    /// VIF, since nothing else in the design can explain it.
    /// </remarks>
    public IReadOnlyList<double> VarianceInflationFactors { get; init; } = [];

    /// <summary>Whether an intercept was fitted, which is what shifts the lists by one.</summary>
    public bool HasIntercept { get; init; }

    /// <summary>The confidence level the two interval lists were computed at.</summary>
    public double ConfidenceLevel { get; init; }

    /// <summary>The fraction of variance explained — <em>uncentred</em> when no intercept was fitted.</summary>
    public double RSquared { get; init; }

    /// <summary>R-squared penalised for the number of regressors.</summary>
    public double AdjustedRSquared { get; init; }

    /// <summary>The overall F statistic, against the null that every slope is zero.</summary>
    public double FStatistic { get; init; }

    /// <summary>The upper-tail probability of <see cref="FStatistic"/>.</summary>
    public double FPValue { get; init; }

    /// <summary>Rows less fitted parameters — what every t here is read on.</summary>
    public int ResidualDegreesOfFreedom { get; init; }

    /// <summary>The residual standard error, the square root of the residual mean square.</summary>
    public double ResidualStandardError { get; init; }
}
