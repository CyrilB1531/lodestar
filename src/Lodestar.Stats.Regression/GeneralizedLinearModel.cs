using Lodestar.Stats.Regression.Internal;

namespace Lodestar.Stats.Regression;

/// <summary>A generalized linear model, fitted by IRLS, with the whole inference table.</summary>
/// <remarks>
/// Reference behavior: <c>statsmodels</c> 0.15.0's <c>GLM(...).fit()</c>. The response is a
/// count or a 0/1 outcome; for a real-valued one, <see cref="OrdinaryLeastSquares"/> is the
/// same table without a link.
/// </remarks>
public static class GeneralizedLinearModel
{
    // The largest Poisson count this fit takes: Internal/LogLikelihood builds an exact log(k!)
    // table up to it -- 8 MB here, 80 MB at 1e7, a wrapped index past 2^31. Lifted by #665.
    private const double PoissonCountBound = 1_000_000.0;

    /// <summary>Fits one model and reports its inference table.</summary>
    /// <param name="design">The regressors, row-major, <paramref name="featureCount"/> per row.</param>
    /// <param name="response">One value per row: 0 or 1 for binomial, a count for Poisson.</param>
    /// <param name="featureCount">How many regressors a row carries.</param>
    /// <param name="family">The response distribution, with its canonical link.</param>
    /// <param name="options">The fit's settings, or null for the defaults.</param>
    /// <exception cref="ArgumentException">
    /// <paramref name="design"/> is not a positive whole number of rows, the lengths disagree, a
    /// response is outside its family, is a Poisson count above one million or is a Poisson
    /// response that is zero in every row, no residual degree of freedom is left, or the design
    /// is rank deficient.
    /// </exception>
    /// <exception cref="ArgumentOutOfRangeException">
    /// <paramref name="featureCount"/> is below one, or <paramref name="family"/> is not a declared
    /// member. A setting outside its own range throws from <see cref="GlmOptions"/> itself.
    /// </exception>
    /// <exception cref="InvalidOperationException">IRLS did not converge and the option says throw.</exception>
    public static GlmSummary Fit(
        ReadOnlySpan<double> design,
        ReadOnlySpan<double> response,
        int featureCount,
        GlmFamily family,
        GlmOptions? options = null)
    {
        Guard.NotLessThan(featureCount, 1);
        GlmOptions settings = options ?? new GlmOptions();
        int rowCount = LeastSquares.Rows(design, response, featureCount);
        int parameterCount = featureCount + (settings.WithIntercept ? 1 : 0);
        RefuseResponseOutsideTheFamily(family, response);

        int residualDegreesOfFreedom = rowCount - parameterCount;
        if (residualDegreesOfFreedom < 1)
        {
            throw new ArgumentException(
                $"{rowCount} rows and {parameterCount} parameters leave no residual degree of "
                + "freedom, so no standard error exists.", nameof(design));
        }

        IrlsResult fit = Irls.Fit(design, response, featureCount, family, settings);
        if (!fit.Converged && settings.ThrowOnNonConvergence)
        {
            throw new InvalidOperationException(
                $"IRLS reached {fit.Iterations} iterations with a deviance change of "
                + $"{fit.DevianceChange:G3}, above the {settings.Tolerance:G3} tolerance. The "
                + $"fit is not usable; set {nameof(GlmOptions.ThrowOnNonConvergence)} to false "
                + "to inspect it.");
        }

        const double dispersion = 1.0;
        double[] errors = LeastSquares.StandardErrors(fit.InverseUpper, parameterCount, dispersion);
        var z = new double[parameterCount];
        var p = new double[parameterCount];
        var lower = new double[parameterCount];
        var upper = new double[parameterCount];
        double multiplier = Distributions.NormalQuantile(
            1.0 - ((1.0 - settings.ConfidenceLevel) / 2.0));

        for (int j = 0; j < parameterCount; j++)
        {
            z[j] = fit.Coefficients[j] / errors[j];
            // The two-sided normal tail, through chi-square(1): z^2 is chi-square(1) distributed,
            // and Lodestar.Stats publishes ChiSquaredSf but no normal CDF to read it off directly.
            p[j] = Distributions.ChiSquaredSf(z[j] * z[j], 1.0);
            lower[j] = fit.Coefficients[j] - (multiplier * errors[j]);
            upper[j] = fit.Coefficients[j] + (multiplier * errors[j]);
        }

        double nullDeviance = NullDeviance(family, response);
        double logLikelihood = LogLikelihood.Of(family, response, fit.Mean);
        double akaike = (2.0 * parameterCount) - (2.0 * logLikelihood);

        return new GlmSummary
        {
            Coefficients = fit.Coefficients,
            StandardErrors = errors,
            ZStatistics = z,
            PValues = p,
            ConfidenceLower = lower,
            ConfidenceUpper = upper,
            Deviance = fit.Deviance,
            NullDeviance = nullDeviance,
            Dispersion = dispersion,
            LogLikelihood = logLikelihood,
            Akaike = akaike,
            ResidualDegreesOfFreedom = residualDegreesOfFreedom,
            HasIntercept = settings.WithIntercept,
            Converged = fit.Converged,
            Iterations = fit.Iterations,
            DevianceChange = fit.DevianceChange,
        };
    }

    /// <summary>The deviance of the constant-only fit, whatever the model beside it carried.</summary>
    /// <remarks>
    /// <c>GLMResults.null_deviance</c> is the intercept-only model whether or not the fit it
    /// reports on had an intercept, so there is no second arm here. Its fitted mean is the
    /// response mean in every row: that is the fixed point constant-only IRLS converges to,
    /// and with no offset or exposure the reference reaches it directly, through a weighted
    /// least squares of the response on a column of ones. Taking the mean rather than running
    /// the loop is therefore the reference's own value exactly, not to a tolerance.
    /// </remarks>
    private static double NullDeviance(GlmFamily family, ReadOnlySpan<double> response)
    {
        double total = 0.0;
        for (int row = 0; row < response.Length; row++)
        {
            total += response[row];
        }

        double mean = total / response.Length;
        var constant = new double[response.Length];
        for (int row = 0; row < constant.Length; row++)
        {
            constant[row] = mean;
        }

        return Irls.Deviance(family, response, constant);
    }

    /// <summary>
    /// Refuses a response its family cannot fit: binomial takes only 0 or 1, and Poisson's
    /// <c>log(y!)</c> makes a count of it, so a negative, fractional or unboundedly large value is
    /// refused there too. The last check reads the response whole rather than a value -- an
    /// all-zero Poisson response is each value's own family and none of them together.
    /// </summary>
    private static void RefuseResponseOutsideTheFamily(
        GlmFamily family, ReadOnlySpan<double> response)
    {
        bool anyPositive = false;
        for (int row = 0; row < response.Length; row++)
        {
            double y = response[row];
            // S1244: a count is exactly its own truncation or it is not a count at all --
            // there is no tolerance band a fractional response could fall inside of.
#pragma warning disable S1244
            bool ok = family switch
            {
                GlmFamily.Binomial => y is 0.0 or 1.0,
                GlmFamily.Poisson => y >= 0.0 && y == Math.Truncate(y),
                _ => throw Families.Undeclared(family),
            };
#pragma warning restore S1244

            if (!ok)
            {
                throw new ArgumentException(
                    $"row {row} carries {y}, which {family} cannot fit: binomial takes 0 or 1 "
                    + "and Poisson a non-negative integer count.", nameof(response));
            }

            if (family == GlmFamily.Poisson && y > PoissonCountBound)
            {
                throw new ArgumentException(
                    $"row {row} carries a Poisson count of {y}, above the {PoissonCountBound} "
                    + "this fit bounds the response at: its log-likelihood sums an exact "
                    + "log-factorial table indexed by the largest count, which is 8 MB at the "
                    + "bound and unbounded above it (#665).", nameof(response));
            }

            anyPositive |= y > 0.0;
        }

        if (family == GlmFamily.Poisson && !anyPositive && response.Length > 0)
        {
            throw new ArgumentException(
                "every count is zero, so the log link sends the fitted mean to zero and the "
                + "maximum lies at minus infinity: what a fit would report is the iteration the "
                + "tolerance stopped at, not an estimate. The reference refuses the same input.",
                nameof(response));
        }
    }
}
