using Lodestar.Stats;
using Lodestar.Survival.Internal;

namespace Lodestar.Survival;

/// <summary>What an accelerated failure time fit reports, at <c>lifelines</c> parity, and the predictions it makes.</summary>
/// <remarks>
/// The per-coefficient lists are parallel and in lifelines' order: the primary parameter's covariates, then its
/// intercept, then the ancillary parameter's. <see cref="ParameterNames"/> says which parameter a coefficient belongs to,
/// <see cref="CovariateIndices"/> which covariate it multiplies. Every z statistic is measured from zero.
/// </remarks>
public sealed class AftSummary
{
    private AftShape _shape = null!;
    private AftDesign _blocks = null!;
    private double[] _theta = [];

    /// <summary>Built by <see cref="AcceleratedFailureTime"/> alone; there is no other way to hold one.</summary>
    private AftSummary()
    {
    }

    /// <summary>The model fitted.</summary>
    public AftModel Model { get; private init; }

    /// <summary>The parameter each coefficient belongs to, lifelines' names: <c>lambda_</c> and <c>rho_</c> for the Weibull, <c>mu_</c> and <c>sigma_</c>, <c>alpha_</c> and <c>beta_</c>.</summary>
    public IReadOnlyList<string> ParameterNames { get; private init; } = [];

    /// <summary>The design column each coefficient multiplies, <c>-1</c> for an intercept.</summary>
    public IReadOnlyList<int> CovariateIndices { get; private init; } = [];

    /// <summary>The coefficients: the log of the parameter is the sum of each times its covariate.</summary>
    public IReadOnlyList<double> Coefficients { get; private init; } = [];

    /// <summary>The square roots of the covariance's diagonal: the inverse observed information, or the sandwich when <see cref="Robust"/>.</summary>
    public IReadOnlyList<double> StandardErrors { get; private init; } = [];

    /// <summary>Each coefficient over its standard error.</summary>
    public IReadOnlyList<double> ZStatistics { get; private init; } = [];

    /// <summary>Two-sided, from the normal tail.</summary>
    public IReadOnlyList<double> PValues { get; private init; } = [];

    /// <summary>The lower end of each coefficient's interval, at <see cref="ConfidenceLevel"/>.</summary>
    public IReadOnlyList<double> ConfidenceLower { get; private init; } = [];

    /// <summary>The upper end of each coefficient's interval.</summary>
    public IReadOnlyList<double> ConfidenceUpper { get; private init; } = [];

    /// <summary>The exponential of each coefficient: how much a unit of the covariate multiplies the parameter.</summary>
    public IReadOnlyList<double> ExpCoefficients { get; private init; } = [];

    /// <summary>The exponential of <see cref="ConfidenceLower"/>.</summary>
    public IReadOnlyList<double> ExpConfidenceLower { get; private init; } = [];

    /// <summary>The exponential of <see cref="ConfidenceUpper"/>.</summary>
    public IReadOnlyList<double> ExpConfidenceUpper { get; private init; } = [];

    /// <summary>The log-likelihood at the fit, less the penalty on its summed scale: lifelines' <c>log_likelihood_</c>.</summary>
    public double LogLikelihood { get; private init; }

    /// <summary>The log-likelihood of the univariate model, no covariate at all: lifelines' null model.</summary>
    public double NullLogLikelihood { get; private init; }

    /// <summary><c>2 · (LogLikelihood − NullLogLikelihood)</c>.</summary>
    public double LikelihoodRatioStatistic { get; private init; }

    /// <summary>The chi-squared upper tail of the likelihood-ratio statistic.</summary>
    public double LikelihoodRatioPValue { get; private init; }

    /// <summary>The number of coefficients past the univariate model's two.</summary>
    public int LikelihoodRatioDegreesOfFreedom { get; private init; }

    /// <summary><c>−2 · LogLikelihood + 2 · k</c>, over the <c>k</c> coefficients.</summary>
    public double Aic { get; private init; }

    /// <summary>Harrell's concordance index between the times and the predicted medians; NaN for an interval-censored fit, which lifelines does not score.</summary>
    public double ConcordanceIndex { get; private init; }

    /// <summary>The level the intervals were built at.</summary>
    public double ConfidenceLevel { get; private init; }

    /// <summary>Whether <see cref="StandardErrors"/> and everything built on them are the Huber sandwich.</summary>
    public bool Robust { get; private init; }

    /// <summary>The number of covariates a design row holds.</summary>
    public int FeatureCount { get; private init; }

    internal static AftSummary Build(
        AftModel model,
        AftShape shape,
        AftDesign blocks,
        (double[] Theta, double[] Covariance) estimate,
        (double LogLikelihood, double Null, double Concordance) fit,
        AftOptions settings)
    {
        (double[] theta, double[] covariance) = estimate;
        (double logLikelihood, double nullLogLikelihood, double concordance) = fit;
        int k = theta.Length;
        double critical = Distributions.NormalQuantile(1.0 - ((1.0 - settings.ConfidenceLevel) / 2.0));
        var errors = new double[k];
        var z = new double[k];
        var p = new double[k];
        var lower = new double[k];
        var upper = new double[k];
        for (int j = 0; j < k; j++)
        {
            errors[j] = Math.Sqrt(covariance[(j * k) + j]);
            z[j] = theta[j] / errors[j];
            p[j] = Distributions.ChiSquaredSf(z[j] * z[j], 1.0);
            lower[j] = theta[j] - (critical * errors[j]);
            upper[j] = theta[j] + (critical * errors[j]);
        }

        double statistic = 2.0 * (logLikelihood - nullLogLikelihood);
        int freedom = k - 2;
        return new AftSummary
        {
            _shape = shape,
            _blocks = blocks,
            _theta = theta,
            Model = model,
            ParameterNames = [.. blocks.PrimaryColumns.Select(_ => shape.Primary), .. blocks.AncillaryColumns.Select(_ => shape.AncillaryName)],
            CovariateIndices = [.. blocks.PrimaryColumns, .. blocks.AncillaryColumns],
            Coefficients = theta,
            StandardErrors = errors,
            ZStatistics = z,
            PValues = p,
            ConfidenceLower = lower,
            ConfidenceUpper = upper,
            ExpCoefficients = [.. theta.Select(Math.Exp)],
            ExpConfidenceLower = [.. lower.Select(Math.Exp)],
            ExpConfidenceUpper = [.. upper.Select(Math.Exp)],
            LogLikelihood = logLikelihood,
            NullLogLikelihood = nullLogLikelihood,
            LikelihoodRatioStatistic = statistic,
            LikelihoodRatioPValue = freedom > 0 ? Distributions.ChiSquaredSf(statistic, freedom) : double.NaN,
            LikelihoodRatioDegreesOfFreedom = freedom,
            Aic = (-2.0 * logLikelihood) + (2.0 * k),
            ConcordanceIndex = concordance,
            ConfidenceLevel = settings.ConfidenceLevel,
            Robust = settings.Robust,
            FeatureCount = blocks.FeatureCount,
        };
    }

    /// <summary>The time by which each subject's survival falls to <paramref name="probability"/>, lifelines' <c>predict_percentile</c>.</summary>
    /// <param name="design">The subjects' covariates, row-major, <see cref="FeatureCount"/> per row; empty for the one subject of a fit with no covariate.</param>
    /// <param name="probability">The survival level, strictly inside (0, 1); a half for the median.</param>
    /// <returns>One time per subject.</returns>
    /// <exception cref="ArgumentException">The design is not whole rows of finite covariates.</exception>
    /// <exception cref="ArgumentOutOfRangeException"><paramref name="probability"/> is not strictly inside (0, 1).</exception>
    public double[] PredictPercentile(ReadOnlySpan<double> design, double probability)
    {
        if (!(probability > 0.0 && probability < 1.0))
        {
            throw new ArgumentOutOfRangeException(nameof(probability), probability, "A survival level lies strictly inside (0, 1).");
        }

        return Each(design, (primary, ancillary) => _shape.Percentile(primary, ancillary, probability));
    }

    /// <summary>Each subject's median survival time, lifelines' <c>predict_median</c>.</summary>
    /// <param name="design">The subjects' covariates, row-major.</param>
    /// <returns>One time per subject.</returns>
    /// <exception cref="ArgumentException">The design is not whole rows of finite covariates.</exception>
    public double[] PredictMedian(ReadOnlySpan<double> design) => PredictPercentile(design, 0.5);

    /// <summary>Each subject's expected survival time, lifelines' <c>predict_expectation</c>, in the model's closed form.</summary>
    /// <param name="design">The subjects' covariates, row-major.</param>
    /// <returns>One time per subject; NaN for a log-logistic whose shape is at most one, where the mean diverges.</returns>
    /// <exception cref="ArgumentException">The design is not whole rows of finite covariates.</exception>
    public double[] PredictExpectation(ReadOnlySpan<double> design) => Each(design, _shape.Expectation);

    /// <summary>Each subject's cumulative hazard at <paramref name="times"/>, lifelines' <c>predict_cumulative_hazard</c>.</summary>
    /// <param name="design">The subjects' covariates, row-major.</param>
    /// <param name="times">The positive times to read at.</param>
    /// <returns>Row-major, one row per subject and one column per time.</returns>
    /// <exception cref="ArgumentException">The design is not whole rows of finite covariates, or a time is not positive and finite.</exception>
    public double[] PredictCumulativeHazard(ReadOnlySpan<double> design, ReadOnlySpan<double> times)
    {
        foreach (double time in times)
        {
            if (!(time > 0.0) || double.IsInfinity(time))
            {
                throw new ArgumentException($"A time to read at is positive and finite; times holds {time}.", nameof(times));
            }
        }

        (double Primary, double Ancillary)[] scores = Scores(design);
        double[] at = times.ToArray();
        var result = new double[scores.Length * at.Length];
        for (int i = 0; i < scores.Length; i++)
        {
            Jet primary = Jet.Constant(scores[i].Primary, 0);
            Jet ancillary = Jet.Constant(scores[i].Ancillary, 0);
            for (int j = 0; j < at.Length; j++)
            {
                result[(i * at.Length) + j] = _shape.CumulativeHazard(primary, ancillary, at[j]).Value;
            }
        }

        return result;
    }

    /// <summary>Each subject's survival at <paramref name="times"/>, lifelines' <c>predict_survival_function</c>.</summary>
    /// <param name="design">The subjects' covariates, row-major.</param>
    /// <param name="times">The positive times to read at.</param>
    /// <returns>Row-major, one row per subject and one column per time: <c>exp(−H)</c> of <see cref="PredictCumulativeHazard"/>.</returns>
    /// <exception cref="ArgumentException">As <see cref="PredictCumulativeHazard"/>.</exception>
    public double[] PredictSurvivalFunction(ReadOnlySpan<double> design, ReadOnlySpan<double> times) =>
        [.. PredictCumulativeHazard(design, times).Select(h => Math.Exp(-h))];

    private double[] Each(ReadOnlySpan<double> design, Func<double, double, double> evaluate) =>
        [.. Scores(design).Select(s => evaluate(s.Primary, s.Ancillary))];

    /// <summary>Both linear predictors of every subject of <paramref name="design"/>.</summary>
    private (double Primary, double Ancillary)[] Scores(ReadOnlySpan<double> design)
    {
        int rows = FeatureCount == 0 ? 1 : design.Length / FeatureCount;
        if ((FeatureCount == 0 && !design.IsEmpty) || (FeatureCount > 0 && (design.IsEmpty || design.Length % FeatureCount != 0)))
        {
            throw new ArgumentException(
                $"A design row holds {FeatureCount} covariates; the design holds {design.Length} values.", nameof(design));
        }

        double[] x = AftDesign.Validate(design, rows, FeatureCount, fitIntercept: true);
        var scores = new (double, double)[rows];
        for (int i = 0; i < rows; i++)
        {
            scores[i] = _blocks.Scores(x, i, _theta);
        }

        return scores;
    }
}
