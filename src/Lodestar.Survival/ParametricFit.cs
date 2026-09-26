using Lodestar.Stats;
using Lodestar.Survival.Internal;

namespace Lodestar.Survival;

/// <summary>A fitted parametric univariate model: its parameters with their inference, and its curves.</summary>
/// <remarks>
/// The parameter table is lifelines' <c>summary</c>: each z statistic measures the distance from lifelines'
/// <c>_compare_to_values</c> — one for a Weibull shape, zero for a log-normal mean — not from zero. A class rather than
/// a record, as <see cref="CoxSummary"/> is: it carries the model it evaluates its curves with.
/// </remarks>
public sealed class ParametricFit
{
    private readonly UnivariateModel _shape;
    private readonly double[] _parameters;
    private readonly double[] _covariance;
    private readonly double _z;

    internal ParametricFit(
        ParametricModel model, UnivariateModel shape, double[] parameters, double logLikelihood, double[] information, double level)
    {
        int k = parameters.Length;
        _shape = shape;
        _parameters = parameters;
        _covariance = Covariance(information, k);
        _z = Distributions.NormalQuantile(1.0 - ((1.0 - level) / 2.0));
        Model = model;
        ParameterNames = shape.Names;
        ConfidenceLevel = level;
        LogLikelihood = logLikelihood;
        Aic = (-2.0 * logLikelihood) + (2.0 * k);
        var errors = new double[k];
        var z = new double[k];
        var p = new double[k];
        var lower = new double[k];
        var upper = new double[k];
        double[] compare = shape.CompareTo;
        for (int j = 0; j < k; j++)
        {
            errors[j] = Math.Sqrt(_covariance[(j * k) + j]);
            z[j] = (parameters[j] - compare[j]) / errors[j];
            p[j] = Distributions.ChiSquaredSf(z[j] * z[j], 1.0);
            lower[j] = parameters[j] - (_z * errors[j]);
            upper[j] = parameters[j] + (_z * errors[j]);
        }

        StandardErrors = errors;
        ZStatistics = z;
        PValues = p;
        ConfidenceLower = lower;
        ConfidenceUpper = upper;
    }

    /// <summary>The model fitted.</summary>
    public ParametricModel Model { get; }

    /// <summary>lifelines' parameter names, in its order: <c>lambda_</c> and <c>rho_</c> for the Weibull, and so on.</summary>
    public IReadOnlyList<string> ParameterNames { get; }

    /// <summary>The fitted parameters, parallel to <see cref="ParameterNames"/>.</summary>
    public IReadOnlyList<double> Parameters => _parameters;

    /// <summary>The square roots of the inverse observed information's diagonal.</summary>
    public IReadOnlyList<double> StandardErrors { get; }

    /// <summary>Each parameter less lifelines' comparison value, over its standard error.</summary>
    public IReadOnlyList<double> ZStatistics { get; }

    /// <summary>Two-sided, from the normal tail.</summary>
    public IReadOnlyList<double> PValues { get; }

    /// <summary>The lower end of each parameter's interval, at <see cref="ConfidenceLevel"/>.</summary>
    public IReadOnlyList<double> ConfidenceLower { get; }

    /// <summary>The upper end of each parameter's interval.</summary>
    public IReadOnlyList<double> ConfidenceUpper { get; }

    /// <summary>The log-likelihood at the fit, summed over the observations with their weights.</summary>
    public double LogLikelihood { get; }

    /// <summary>Akaike's criterion, <c>−2 · LogLikelihood + 2k</c>.</summary>
    public double Aic { get; }

    /// <summary>The level the intervals were built at.</summary>
    public double ConfidenceLevel { get; }

    /// <summary>The survival function at <paramref name="times"/>.</summary>
    /// <param name="times">Positive times.</param>
    /// <returns>One value per time.</returns>
    public double[] Survival(ReadOnlySpan<double> times) => [.. CumulativeHazard(times).Select(h => Math.Exp(-h))];

    /// <summary>The cumulative hazard at <paramref name="times"/>.</summary>
    /// <param name="times">Positive times.</param>
    /// <returns>One value per time.</returns>
    public double[] CumulativeHazard(ReadOnlySpan<double> times)
    {
        var result = new double[times.Length];
        for (int i = 0; i < times.Length; i++)
        {
            result[i] = _shape.CumulativeHazard(_parameters, times[i]);
        }

        return result;
    }

    /// <summary>The hazard at <paramref name="times"/>.</summary>
    /// <param name="times">Positive times.</param>
    /// <returns>One value per time.</returns>
    public double[] Hazard(ReadOnlySpan<double> times)
    {
        var result = new double[times.Length];
        for (int i = 0; i < times.Length; i++)
        {
            result[i] = _shape.Hazard(_parameters, times[i]);
        }

        return result;
    }

    /// <summary>The time by which the survival falls to <paramref name="probability"/>, lifelines' <c>percentile</c>.</summary>
    /// <param name="probability">The survival level, strictly inside (0, 1).</param>
    /// <returns>The time.</returns>
    /// <exception cref="ArgumentOutOfRangeException"><paramref name="probability"/> is not strictly inside (0, 1).</exception>
    public double Percentile(double probability)
    {
        if (!(probability > 0.0 && probability < 1.0))
        {
            throw new ArgumentOutOfRangeException(nameof(probability), probability, "A survival level lies strictly inside (0, 1).");
        }

        return _shape.Percentile(_parameters, probability);
    }

    /// <summary>The median survival time, lifelines' <c>median_survival_time_</c>.</summary>
    public double MedianSurvivalTime => _shape.Percentile(_parameters, 0.5);

    /// <summary>The delta-method interval of the survival function at <paramref name="times"/>, lifelines' <c>confidence_interval_survival_function_</c>.</summary>
    /// <param name="times">Positive times.</param>
    /// <returns>The lower and upper bounds, one per time; not clipped to [0, 1], as lifelines' are not.</returns>
    public (double[] Lower, double[] Upper) SurvivalBounds(ReadOnlySpan<double> times) =>
        Bounds(times, variables => t => Jet.Exp(-_shape.CumulativeHazard(variables, t)));

    /// <summary>The delta-method interval of the cumulative hazard at <paramref name="times"/>, lifelines' <c>confidence_interval_cumulative_hazard_</c>.</summary>
    /// <param name="times">Positive times.</param>
    /// <returns>The lower and upper bounds, one per time.</returns>
    public (double[] Lower, double[] Upper) CumulativeHazardBounds(ReadOnlySpan<double> times) =>
        Bounds(times, variables => t => _shape.CumulativeHazard(variables, t));

    /// <summary>The survival at <paramref name="time"/> and its delta-method variance, <c>gᵀ V g</c>.</summary>
    internal (double Survival, double Variance) SurvivalWithVariance(double time)
    {
        Jet value = Jet.Exp(-_shape.CumulativeHazard(UnivariateModel.Variables(_parameters), time));
        return (value.Value, Variance(value));
    }

    private double Variance(Jet value)
    {
        int k = _parameters.Length;
        double variance = 0.0;
        for (int a = 0; a < k; a++)
        {
            for (int b = 0; b < k; b++)
            {
                variance += value.Gradient[a] * _covariance[(a * k) + b] * value.Gradient[b];
            }
        }

        return variance;
    }

    private (double[] Lower, double[] Upper) Bounds(ReadOnlySpan<double> times, Func<Jet[], Func<double, Jet>> transform)
    {
        Func<double, Jet> at = transform(UnivariateModel.Variables(_parameters));
        var lower = new double[times.Length];
        var upper = new double[times.Length];
        for (int i = 0; i < times.Length; i++)
        {
            Jet value = at(times[i]);
            double spread = _z * Math.Sqrt(Variance(value));
            lower[i] = value.Value - spread;
            upper[i] = value.Value + spread;
        }

        return (lower, upper);
    }

    private static double[] Covariance(double[] information, int k) =>
        Cholesky.TryFactor(information, k, out double[] lower)
            ? Cholesky.Inverse(lower, k)
            : throw new InvalidOperationException("The observed information at the fit is not positive definite; the parameters are not identified.");
}
