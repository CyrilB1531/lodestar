namespace Lodestar.Stats.TimeSeries;

/// <summary>What a vector autoregression reports, entry for entry what <c>statsmodels</c>' <c>VARResults</c> holds.</summary>
/// <remarks>
/// The per-coefficient lists are indexed by equation first, then by parameter. Equation <c>j</c> explains variable
/// <c>j</c>, and its parameters run in the reference's own order: the constant when one was fitted, then lag 1's
/// coefficient for every variable, then lag 2's, and so on.
/// </remarks>
public sealed class VarSummary
{
    /// <summary>Built by <see cref="VectorAutoregression.Fit"/> alone; there is no other way to hold one.</summary>
    internal VarSummary()
    {
    }

    /// <summary>The coefficients, one list per equation.</summary>
    public IReadOnlyList<IReadOnlyList<double>> Coefficients { get; init; } = [];

    /// <summary>The standard errors, shaped as <see cref="Coefficients"/>.</summary>
    public IReadOnlyList<IReadOnlyList<double>> StandardErrors { get; init; } = [];

    /// <summary>Each coefficient over its standard error.</summary>
    public IReadOnlyList<IReadOnlyList<double>> TStatistics { get; init; } = [];

    /// <summary>The two-sided p-values of <see cref="TStatistics"/>, read against the normal as the reference reads them.</summary>
    public IReadOnlyList<IReadOnlyList<double>> PValues { get; init; } = [];

    /// <summary>The residual covariance <c>S / (T − k)</c>, row-major and symmetric: the reference's <c>sigma_u</c>.</summary>
    public IReadOnlyList<double> ResidualCovariance { get; init; } = [];

    /// <summary>The residual covariance <c>S / T</c>, which the criteria below read: the reference's <c>sigma_u_mle</c>.</summary>
    public IReadOnlyList<double> ResidualCovarianceMaximumLikelihood { get; init; } = [];

    /// <summary>The Gaussian log-likelihood at the estimates.</summary>
    public double LogLikelihood { get; init; }

    /// <summary>Akaike's criterion, <c>log|Σ̂| + 2m/T</c>.</summary>
    public double Akaike { get; init; }

    /// <summary>Schwarz's criterion, <c>log|Σ̂| + m·log(T)/T</c>.</summary>
    public double Bayesian { get; init; }

    /// <summary>The Hannan-Quinn criterion, <c>log|Σ̂| + 2m·log(log(T))/T</c>.</summary>
    public double HannanQuinn { get; init; }

    /// <summary>The final prediction error, <c>|Σ̂|·((T + k)/(T − k))^K</c>.</summary>
    public double FinalPredictionError { get; init; }

    /// <summary>How many lags each equation carries.</summary>
    public int LagOrder { get; init; }

    /// <summary>How many variables the system holds.</summary>
    public int VariableCount { get; init; }

    /// <summary>How many observations the fit used, <c>n − LagOrder</c>: the reference's <c>nobs</c>.</summary>
    public int ObservationsUsed { get; init; }

    /// <summary>Parameters per equation, <c>(HasIntercept ? 1 : 0) + VariableCount·LagOrder</c>.</summary>
    public int ModelDegreesOfFreedom { get; init; }

    /// <summary><c>ObservationsUsed − ModelDegreesOfFreedom</c>.</summary>
    public int ResidualDegreesOfFreedom { get; init; }

    /// <summary>Whether each equation carries a constant as its first coefficient.</summary>
    public bool HasIntercept { get; init; }
}
