namespace Lodestar.Stats.Regression;

/// <summary>The inference table of a multinomial logit fit, entry for entry what <c>statsmodels</c>' <c>MNLogitResults</c> prints.</summary>
/// <remarks>
/// The per-coefficient lists are indexed by equation first, then by parameter. Equation <c>j</c> is category
/// <c>Categories[j + 1]</c> against the reference category <c>Categories[0]</c>, and its first parameter is the intercept
/// when one was fitted.
/// </remarks>
public sealed class MultinomialLogitSummary
{
    /// <summary>Built by <see cref="MultinomialLogit.Fit"/> alone; there is no other way to hold one.</summary>
    internal MultinomialLogitSummary()
    {
    }

    /// <summary>The response's distinct labels in ascending order; the first is the reference category.</summary>
    public IReadOnlyList<int> Categories { get; init; } = [];

    /// <summary>The coefficients, one list per non-reference category, intercept first when fitted.</summary>
    public IReadOnlyList<IReadOnlyList<double>> Coefficients { get; init; } = [];

    /// <summary>The standard errors, shaped as <see cref="Coefficients"/>.</summary>
    public IReadOnlyList<IReadOnlyList<double>> StandardErrors { get; init; } = [];

    /// <summary>Each coefficient over its standard error, read against the normal.</summary>
    public IReadOnlyList<IReadOnlyList<double>> ZStatistics { get; init; } = [];

    /// <summary>The two-sided normal p-values of <see cref="ZStatistics"/>.</summary>
    public IReadOnlyList<IReadOnlyList<double>> PValues { get; init; } = [];

    /// <summary>The lower ends of the normal intervals at <see cref="ConfidenceLevel"/>.</summary>
    public IReadOnlyList<IReadOnlyList<double>> ConfidenceLower { get; init; } = [];

    /// <summary>The upper ends of the normal intervals at <see cref="ConfidenceLevel"/>.</summary>
    public IReadOnlyList<IReadOnlyList<double>> ConfidenceUpper { get; init; } = [];

    /// <summary>The log-likelihood at the estimates.</summary>
    public double LogLikelihood { get; init; }

    /// <summary>The log-likelihood of the constant-only model, <c>Σ nⱼ·log(nⱼ/n)</c>.</summary>
    public double NullLogLikelihood { get; init; }

    /// <summary>McFadden's pseudo-R², <c>1 − LogLikelihood / NullLogLikelihood</c>.</summary>
    public double PseudoRSquared { get; init; }

    /// <summary>The likelihood-ratio statistic against the constant-only model, <c>2·(LogLikelihood − NullLogLikelihood)</c>.</summary>
    public double LikelihoodRatio { get; init; }

    /// <summary>The χ² tail of <see cref="LikelihoodRatio"/> on <see cref="ModelDegreesOfFreedom"/>.</summary>
    public double LikelihoodRatioPValue { get; init; }

    /// <summary>Akaike's information criterion.</summary>
    public double Akaike { get; init; }

    /// <summary>Schwarz's Bayesian information criterion.</summary>
    public double Bayesian { get; init; }

    /// <summary><c>(K − 1)·(J − 1)</c>, the reference's <c>df_model</c>, with or without an intercept.</summary>
    public int ModelDegreesOfFreedom { get; init; }

    /// <summary><c>n − ModelDegreesOfFreedom − (J − 1)</c>, the reference's <c>df_resid</c>.</summary>
    public int ResidualDegreesOfFreedom { get; init; }

    /// <summary>Whether each equation carries an intercept as its first coefficient.</summary>
    public bool HasIntercept { get; init; }

    /// <summary>The level the intervals were computed at.</summary>
    public double ConfidenceLevel { get; init; }

    /// <summary>Whether Newton stopped before its iteration budget ran out.</summary>
    public bool Converged { get; init; }

    /// <summary>How many Newton steps were taken.</summary>
    public int Iterations { get; init; }
}
