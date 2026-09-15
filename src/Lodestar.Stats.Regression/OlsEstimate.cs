namespace Lodestar.Stats.Regression;

/// <summary>An ordinary least-squares estimate: the coefficients and how precisely each is known, without the inference table.</summary>
/// <remarks>
/// What <see cref="OrdinaryLeastSquares.Estimate"/> returns. A class rather than a record, for
/// <see cref="OlsSummary"/>'s reason: a record's equality would compare the lists by reference.
/// </remarks>
public sealed class OlsEstimate
{
    /// <summary>Built by <see cref="OrdinaryLeastSquares.Estimate"/> alone; there is no other way to hold one.</summary>
    internal OlsEstimate()
    {
    }

    /// <summary>The estimates, intercept first when one was fitted.</summary>
    public IReadOnlyList<double> Coefficients { get; init; } = [];

    /// <summary>The standard error of each estimate, under the non-robust covariance.</summary>
    public IReadOnlyList<double> StandardErrors { get; init; } = [];

    /// <summary>Each estimate over its standard error.</summary>
    public IReadOnlyList<double> TStatistics { get; init; } = [];

    /// <summary>The sum of the squared residuals, which a likelihood or an information criterion is read from.</summary>
    public double ResidualSumOfSquares { get; init; }

    /// <summary>The rows less the parameters estimated.</summary>
    public int ResidualDegreesOfFreedom { get; init; }

    /// <summary>Whether a constant was fitted, and so whether <see cref="Coefficients"/> starts with it.</summary>
    public bool HasIntercept { get; init; }
}
