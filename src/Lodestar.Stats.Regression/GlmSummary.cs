namespace Lodestar.Stats.Regression;

/// <summary>What a generalized linear fit reports, at <c>statsmodels</c> parity.</summary>
public sealed class GlmSummary
{
    /// <summary>Built by <see cref="GeneralizedLinearModel.Fit"/> alone; there is no other way to hold one.</summary>
    internal GlmSummary()
    {
    }

    /// <summary>One per parameter, the intercept first when there is one.</summary>
    public IReadOnlyList<double> Coefficients { get; init; } = [];

    /// <summary>The square root of the covariance's diagonal.</summary>
    public IReadOnlyList<double> StandardErrors { get; init; } = [];

    /// <summary>The Wald statistic, a coefficient over its standard error.</summary>
    public IReadOnlyList<double> ZStatistics { get; init; } = [];

    /// <summary>Two-sided, from the normal tail.</summary>
    public IReadOnlyList<double> PValues { get; init; } = [];

    /// <summary>The lower end of each interval, at the level asked for.</summary>
    public IReadOnlyList<double> ConfidenceLower { get; init; } = [];

    /// <summary>The upper end of each interval.</summary>
    public IReadOnlyList<double> ConfidenceUpper { get; init; } = [];

    /// <summary>Twice the log-likelihood gap to a saturated fit.</summary>
    public double Deviance { get; init; }

    /// <summary>The same, for the intercept-only fit.</summary>
    public double NullDeviance { get; init; }

    /// <summary>Fixed at 1 for both families here; estimated when a Gamma family lands.</summary>
    public double Dispersion { get; init; }

    /// <summary>The fitted log-likelihood.</summary>
    public double LogLikelihood { get; init; }

    /// <summary>Akaike's criterion, <c>2k - 2 logL</c>.</summary>
    public double Akaike { get; init; }

    /// <summary>Rows less parameters.</summary>
    public int ResidualDegreesOfFreedom { get; init; }

    /// <summary>Whether a column of ones was fitted.</summary>
    public bool HasIntercept { get; init; }

    /// <summary>Whether IRLS reached the tolerance. <strong>Read this first.</strong></summary>
    public bool Converged { get; init; }

    /// <summary>How many iterations it took, or the budget when it did not converge.</summary>
    public int Iterations { get; init; }

    /// <summary>The absolute deviance change at the last iteration.</summary>
    public double DevianceChange { get; init; }
}
