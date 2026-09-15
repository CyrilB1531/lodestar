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
    /// <summary>Fits one model and reports its inference table.</summary>
    /// <param name="design">The regressors, row-major, <paramref name="featureCount"/> per row.</param>
    /// <param name="response">One value per row: 0 or 1 for binomial, a count for Poisson and the negative binomial.</param>
    /// <param name="featureCount">How many regressors a row carries.</param>
    /// <param name="family">The response distribution, with its canonical link.</param>
    /// <param name="options">The fit's settings, or null for the defaults.</param>
    /// <exception cref="ArgumentException">
    /// <paramref name="design"/> is not a positive whole number of rows, the lengths disagree, a
    /// response is outside its family, is an infinite count, or is a count response that
    /// is zero in every row, no residual degree of freedom is left, the design
    /// is rank deficient, or <see cref="GlmOptions.NegativeBinomialAlpha"/> is set for another family.
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
        return FitCore(design, response, null, featureCount, family, options ?? new GlmOptions());
    }

    /// <summary>Fits one model whose linear predictor carries a fixed offset, an exposure, or both, and reports its inference table.</summary>
    /// <param name="design">The regressors, row-major, <paramref name="featureCount"/> per row.</param>
    /// <param name="response">One value per row: 0 or 1 for binomial, a count for Poisson and the negative binomial, a positive value for Gamma.</param>
    /// <param name="offset">One term per row added to the linear predictor with no coefficient; empty for none.</param>
    /// <param name="exposure">One positive value per row whose logarithm is added the same way, for a log link; empty for none.</param>
    /// <param name="featureCount">How many regressors a row carries.</param>
    /// <param name="family">The response distribution, with its canonical link.</param>
    /// <param name="options">The fit's settings, or null for the defaults.</param>
    /// <exception cref="ArgumentException">What the other overload refuses; <paramref name="offset"/> or <paramref name="exposure"/> is not empty and has another length than <paramref name="response"/>; or <paramref name="exposure"/> is given with a link other than log.</exception>
    /// <exception cref="ArgumentOutOfRangeException">What the other overload refuses; an offset is not finite; or an exposure is not finite and above zero.</exception>
    /// <exception cref="InvalidOperationException">IRLS did not converge and the option says throw.</exception>
    /// <remarks>
    /// <c>statsmodels</c>' <c>GLM(y, X, family, offset=, exposure=)</c>. With either given, <see cref="GlmSummary.NullDeviance"/> refits
    /// the intercept-only model with the same term, as the reference does — an offset of zeros included.
    /// </remarks>
    public static GlmSummary Fit(
        ReadOnlySpan<double> design,
        ReadOnlySpan<double> response,
        ReadOnlySpan<double> offset,
        ReadOnlySpan<double> exposure,
        int featureCount,
        GlmFamily family,
        GlmOptions? options = null)
    {
        Guard.NotLessThan(featureCount, 1);
        GlmOptions settings = options ?? new GlmOptions();
        int rowCount = LeastSquares.Rows(design, response, featureCount);
        GlmLink link = ResolveLink(family, settings.Link, nameof(options));
        return FitCore(design, response, CombineOffset(offset, exposure, rowCount, link), featureCount, family, settings);
    }

    /// <summary>Everything from the family checks onward, shared by both overloads.</summary>
    private static GlmSummary FitCore(
        ReadOnlySpan<double> design,
        ReadOnlySpan<double> response,
        double[]? offset,
        int featureCount,
        GlmFamily family,
        GlmOptions options)
    {
        GlmOptions settings = options;
        int rowCount = LeastSquares.Rows(design, response, featureCount);
        int parameterCount = featureCount + (settings.WithIntercept ? 1 : 0);
        RefuseResponseOutsideTheFamily(family, response);
        var shape = new FamilyShape(
            family,
            ResolveLink(family, settings.Link, nameof(options)),
            ResolveAlpha(family, settings, nameof(options)));

        int residualDegreesOfFreedom = rowCount - parameterCount;
        if (residualDegreesOfFreedom < 1)
        {
            throw new ArgumentException(
                $"{rowCount} rows and {parameterCount} parameters leave no residual degree of "
                + "freedom, so no standard error exists.", nameof(design));
        }

        IrlsResult fit = Irls.Fit(design, response, featureCount, shape, settings, offset);
        if (!fit.Converged && settings.ThrowOnNonConvergence)
        {
            throw new InvalidOperationException(
                $"IRLS reached {fit.Iterations} iterations with a deviance change of "
                + $"{fit.DevianceChange:G3}, above the {settings.Tolerance:G3} tolerance. The "
                + $"fit is not usable; set {nameof(GlmOptions.ThrowOnNonConvergence)} to false "
                + "to inspect it.");
        }

        double dispersion = Irls.Scale(shape, response, fit.Mean, residualDegreesOfFreedom);
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

        double nullDeviance = NullDeviance(shape, response, offset);
        double logLikelihood = LogLikelihood.Of(shape, response, fit.Mean, dispersion);
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
    private static double NullDeviance(FamilyShape shape, ReadOnlySpan<double> response, double[]? offset)
    {
        if (offset is not null)
        {
            // GLMResults.null: a column of ones, the same offset, GLM.fit's defaults. The family's own start rather than
            // link(mean(y)) reaches the same fixed point within 3.3e-15, measured in the #787 spec.
            var ones = new double[response.Length];
            for (int row = 0; row < ones.Length; row++)
            {
                ones[row] = 1.0;
            }

            return Irls.Fit(ones, response, 1, shape, new GlmOptions { WithIntercept = false }, offset).Deviance;
        }

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

        return Irls.Deviance(shape, response, constant);
    }

    /// <summary>The one term the predictor carries, <c>offset + log(exposure)</c>, or null when neither is given.</summary>
    private static double[]? CombineOffset(
        ReadOnlySpan<double> offset, ReadOnlySpan<double> exposure, int rowCount, GlmLink link)
    {
        if (offset.IsEmpty && exposure.IsEmpty)
        {
            return null;
        }

        var combined = new double[rowCount];
        if (!offset.IsEmpty)
        {
            AddOffset(offset, combined);
        }

        if (!exposure.IsEmpty)
        {
            if (link != GlmLink.Log)
            {
                throw new ArgumentException(
                    "An exposure enters the predictor as its logarithm, which only a log link reads as a rate; "
                    + "pass log(exposure) as the offset to use it with another link.", nameof(exposure));
            }

            AddLogExposure(exposure, combined);
        }

        return combined;
    }

    /// <summary>Copies a finite offset into the combined term.</summary>
    private static void AddOffset(ReadOnlySpan<double> offset, double[] combined)
    {
        RequireLength(offset.Length, combined.Length, nameof(offset));
        for (int row = 0; row < combined.Length; row++)
        {
            double value = offset[row];
            if (double.IsNaN(value) || double.IsInfinity(value))
            {
                throw new ArgumentOutOfRangeException(nameof(offset), value, $"offset[{row}] is not a finite number.");
            }

            combined[row] = value;
        }
    }

    /// <summary>Adds the logarithm of a positive, finite exposure to the combined term.</summary>
    private static void AddLogExposure(ReadOnlySpan<double> exposure, double[] combined)
    {
        RequireLength(exposure.Length, combined.Length, nameof(exposure));
        for (int row = 0; row < combined.Length; row++)
        {
            double value = exposure[row];
            if (!(value > 0.0) || double.IsPositiveInfinity(value))
            {
                throw new ArgumentOutOfRangeException(
                    nameof(exposure), value, $"exposure[{row}] is not a finite number above zero.");
            }

            combined[row] += Math.Log(value);
        }
    }

    /// <summary>Refuses a per-row span whose length is not the row count.</summary>
    private static void RequireLength(int length, int rowCount, string parameterName)
    {
        if (length != rowCount)
        {
            throw new ArgumentException(
                $"design has {rowCount} rows and {parameterName} holds {length} values.", parameterName);
        }
    }

    /// <summary>The link a family is fitted through, resolved from the option, and a refusal for a pairing not fitted here.</summary>
    private static GlmLink ResolveLink(GlmFamily family, GlmLink link, string optionsName) => (family, link) switch
    {
        (GlmFamily.Binomial, GlmLink.Default) => GlmLink.Default,
        (GlmFamily.Poisson or GlmFamily.NegativeBinomial, GlmLink.Default or GlmLink.Log) => GlmLink.Log,
        (GlmFamily.Gamma, GlmLink.Default or GlmLink.Inverse) => GlmLink.Inverse,
        (GlmFamily.Gamma, GlmLink.Log) => GlmLink.Log,
        (GlmFamily.Binomial or GlmFamily.Poisson or GlmFamily.NegativeBinomial or GlmFamily.Gamma, _) =>
            throw new ArgumentException($"{family} is not fitted through the {link} link here.", optionsName),
        _ => throw Families.Undeclared(family),
    };

    /// <summary>The negative binomial's alpha, the reference's default of 1 when unset, and a refusal for any other family.</summary>
    private static double ResolveAlpha(GlmFamily family, GlmOptions settings, string optionsName)
    {
        if (family == GlmFamily.NegativeBinomial)
        {
            return settings.NegativeBinomialAlpha ?? 1.0;
        }

        if (settings.NegativeBinomialAlpha is { } alpha)
        {
            throw new ArgumentException(
                $"{nameof(GlmOptions.NegativeBinomialAlpha)} is {alpha}, and {family} has no alpha to "
                + "apply it to.", optionsName);
        }

        return 1.0;
    }

    /// <summary>
    /// Refuses a response its family cannot fit: binomial takes only 0 or 1, and Poisson's
    /// <c>log(y!)</c> makes a count of it, so a negative, fractional or unboundedly large value is
    /// refused there too. The last check reads the response whole rather than a value -- an
    /// all-zero Poisson response is each value's own family and none of them together. Gamma takes
    /// a finite value above zero.
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
                // Infinity truncates to itself, so it is refused by name; the count bound that used
                // to catch it went with #665, and an infinite count has no likelihood to maximise.
                GlmFamily.Poisson or GlmFamily.NegativeBinomial =>
                    y >= 0.0 && !double.IsPositiveInfinity(y) && y == Math.Truncate(y),
                // A zero has no Gamma density: the reference's log-likelihood reads log(y) and reports +inf.
                GlmFamily.Gamma => y > 0.0 && !double.IsPositiveInfinity(y),
                _ => throw Families.Undeclared(family),
            };
#pragma warning restore S1244

            if (!ok)
            {
                throw new ArgumentException(
                    $"row {row} carries {y}, which {family} cannot fit: binomial takes 0 or 1 "
                    + "the two count families a finite non-negative integer count, and Gamma a finite "
                    + "positive value.", nameof(response));
            }

            anyPositive |= y > 0.0;
        }

        if (family != GlmFamily.Binomial && !anyPositive && response.Length > 0)
        {
            throw new ArgumentException(
                "every count is zero, so the log link sends the fitted mean to zero and the "
                + "maximum lies at minus infinity: what a fit would report is the iteration the "
                + "tolerance stopped at, not an estimate. The reference refuses the same input.",
                nameof(response));
        }
    }
}
