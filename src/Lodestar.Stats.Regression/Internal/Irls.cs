namespace Lodestar.Stats.Regression.Internal;

/// <summary>One IRLS fit: the coefficients, and everything the summary is computed from.</summary>
internal sealed record IrlsResult(
    double[] Coefficients,
    double[] InverseUpper,
    double[] Mean,
    double Deviance,
    bool Converged,
    int Iterations,
    double DevianceChange);

/// <summary>Iteratively reweighted least squares, over the QR the OLS already uses.</summary>
internal static class Irls
{
    // Machine epsilon: keeps a perfectly-separating fit's mu off the exact 0/1 boundary, where
    // LinkDerivative and Link both go infinite and the working response becomes NaN (0 * Infinity).
    private const double BoundaryEpsilon = 2.220446049250313e-16;

    public static IrlsResult Fit(
        ReadOnlySpan<double> design,
        ReadOnlySpan<double> response,
        int featureCount,
        in FamilyShape shape,
        GlmOptions options,
        double[]? offset = null)
    {
        int rowCount = response.Length;
        int parameterCount = featureCount + (options.WithIntercept ? 1 : 0);
        double[] matrix = LeastSquares.Design(design, rowCount, featureCount, options.WithIntercept);

        // The reference's Family.starting_mu, which is (y + mean(y)) / 2 and which Binomial
        // overrides to (y + 0.5) / 2.
        double responseMean = 0.0;
        for (int row = 0; row < rowCount; row++)
        {
            responseMean += response[row];
        }

        responseMean /= rowCount;
        var mean = new double[rowCount];
        for (int row = 0; row < rowCount; row++)
        {
            // Unclamped, as the reference is: both starts sit strictly inside the link's
            // domain once an all-zero Poisson response is refused, which Fit does.
            mean[row] = shape.Family == GlmFamily.Binomial
                ? (response[row] + 0.5) / 2.0
                : (response[row] + responseMean) / 2.0;
        }

        // Reused by every iteration: the weighted design is written column-major, as the reflections walk it, and the
        // working response is what they project, so neither is copied on its way into the solve.
        var scaled = new double[rowCount * parameterCount];
        var working = new double[rowCount];
        double[] coefficients = new double[parameterCount];
        double[] inverseUpper = new double[parameterCount * parameterCount];
        int residualDegreesOfFreedom = rowCount - parameterCount;
        double scale = Scale(shape, response, mean, residualDegreesOfFreedom);
        double deviance = Deviance(shape, response, mean);
        double criterion = deviance / scale;
        double change = double.PositiveInfinity;
        bool converged = false;
        int iteration = 0;

        while (iteration < options.MaximumIterations)
        {
            iteration++;
            BuildWeightedSystem(shape, matrix, response, mean, scaled, working, offset);

            // The reflections, not the normal equations: IRLS stops on an absolute deviance change, which the normal
            // equations' rounding held above 1e-8 for 19 iterations at a Poisson mean of 5e6 (GlmPoissonBenchmarks, #782).
            (coefficients, inverseUpper) = LeastSquares.SolveByReflectionsInPlace(
                scaled, rowCount, parameterCount, working);

            if (!AllFinite(coefficients))
            {
                throw new ArgumentException(
                    $"the weighted least squares of IRLS iteration {iteration} solved to a "
                    + "non-finite coefficient. The usual cause is a rank-deficient or collinear "
                    + "design, whose zero pivot in the QR leaves the system without a unique "
                    + "solution: look for a dependent regressor before reading this fit.",
                    nameof(design));
            }

            UpdateMean(shape, matrix, coefficients, offset, mean);

            RefuseANonPositiveGammaMean(shape, mean, iteration, nameof(design));

            // long-comment: what the criterion is for an estimated scale, which only Gamma has here.
            // _fit_irls compares family.deviance(..., scale) between iterations, and the scale it
            // divides by is the one estimate_scale returned at the end of the iteration before:
            // the Pearson scale of the previous mean. Comparing the unscaled deviance instead stops
            // Gamma fits one iteration off and moves their coefficients by 1e-7 to 2e-6 relative,
            // measured on stats_glm.json (#770). For the other families the scale is exactly 1.
            deviance = Deviance(shape, response, mean);
            double next = deviance / scale;
            scale = Scale(shape, response, mean, residualDegreesOfFreedom);
            change = Math.Abs(criterion - next);
            // numpy.allclose with the reference's own arguments: _fit_irls passes atol=tol and
            // leaves rtol at 0, so the criterion is the absolute deviance change alone.
            converged = change <= options.Tolerance;
            criterion = next;
            if (converged)
            {
                break;
            }
        }

        // long-comment: the covariance handed back here is one step behind on purpose, and a
        // reader who does not know that will "correct" it and lose the parity it buys.
        // inverseUpper is the last solve's, so its weights came from the mean of the iteration
        // before the converged one. The reference does the same: _fit_irls refits
        // lm.WLS(wlsendog, wlsexog, self.weights) after the loop, and both arguments were
        // assigned at the top of that last iteration. The standard errors therefore depend on
        // the path and not only on the fixed point, which is why the start above and the
        // criterion above it are the reference's exactly -- measured, a different start moves
        // the standard errors by 1.3e-6 while the coefficients still agree to 1e-11.
        return new IrlsResult(
            coefficients, inverseUpper, mean, deviance, converged, iteration, change);
    }

    /// <summary>The scale statsmodels estimates: Pearson's χ² over the residual degrees of freedom for Gamma, 1 for the rest.</summary>
    public static double Scale(
        in FamilyShape shape, ReadOnlySpan<double> response, double[] mean, int residualDegreesOfFreedom)
    {
        if (shape.Family != GlmFamily.Gamma)
        {
            return 1.0;
        }

        double total = 0.0;
        for (int row = 0; row < response.Length; row++)
        {
            double residual = response[row] - mean[row];
            total += residual * residual / Families.Variance(shape, mean[row]);
        }

        return total / residualDegreesOfFreedom;
    }

    public static double Deviance(in FamilyShape shape, ReadOnlySpan<double> response, double[] mean)
    {
        double total = 0.0;
        for (int row = 0; row < response.Length; row++)
        {
            total += Families.UnitDeviance(shape, response[row], mean[row]);
        }

        return total;
    }

    /// <summary>The means the solved coefficients give, <c>g⁻¹(xᵢβ + offsetᵢ)</c>, clamped off their family's boundary.</summary>
    private static void UpdateMean(in FamilyShape shape, double[] matrix, double[] coefficients, double[]? offset, double[] mean)
    {
        // Two loops rather than a test per row: the fit without an offset keeps main's loop (#787).
        if (offset is null)
        {
            for (int row = 0; row < mean.Length; row++)
            {
                mean[row] = Clamp(shape.Family, Families.InverseLink(shape, Predictor(matrix, coefficients, row)));
            }
        }
        else
        {
            for (int row = 0; row < mean.Length; row++)
            {
                mean[row] = Clamp(
                    shape.Family, Families.InverseLink(shape, Predictor(matrix, coefficients, row) + offset[row]));
            }
        }
    }

    /// <summary>The weighted design, column-major, and the working response for one IRLS iteration.</summary>
    /// <remarks>
    /// With an offset the working response is <c>η + (y − μ)·g′(μ) − offset</c>, the reference's <c>wlsendog</c>: the
    /// solve estimates only the part of the predictor the regressors carry.
    /// </remarks>
    private static void BuildWeightedSystem(
        in FamilyShape shape,
        double[] matrix,
        ReadOnlySpan<double> response,
        double[] mean,
        double[] scaled,
        double[] working,
        double[]? offset)
    {
        int rowCount = mean.Length;
        int parameterCount = matrix.Length / rowCount;
        if (offset is null)
        {
            for (int row = 0; row < rowCount; row++)
            {
                (double root, double value) = WorkingRow(shape, response[row], mean[row]);
                working[row] = value;
                ScaleRow(matrix, scaled, row, rowCount, parameterCount, root);
            }
        }
        else
        {
            for (int row = 0; row < rowCount; row++)
            {
                (double root, double value) = WorkingRow(shape, response[row], mean[row]);
                working[row] = value - (root * offset[row]);
                ScaleRow(matrix, scaled, row, rowCount, parameterCount, root);
            }
        }
    }

    /// <summary>A row's root weight, and its working response <c>√w·(η + (y − μ)·g′(μ))</c> before any offset.</summary>
    [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
    private static (double Root, double Working) WorkingRow(in FamilyShape shape, double y, double mu)
    {
        double derivative = Families.LinkDerivative(shape, mu);
        double weight = 1.0 / (Families.Variance(shape, mu) * derivative * derivative);
        double root = Math.Sqrt(weight);
        double eta = Families.Link(shape, mu);
        return (root, root * (eta + ((y - mu) * derivative)));
    }

    /// <summary>Writes row <paramref name="row"/> of the design, scaled by its root weight, into a column-major block.</summary>
    [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
    private static void ScaleRow(double[] matrix, double[] scaled, int row, int rowCount, int parameterCount, double root)
    {
        int source = row * parameterCount;
        for (int column = 0; column < parameterCount; column++)
        {
            scaled[(column * rowCount) + row] = root * matrix[source + column];
        }
    }

    /// <summary>The part of row <paramref name="row"/>'s linear predictor the regressors carry, <c>xᵢβ</c>.</summary>
    [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
    private static double Predictor(double[] matrix, double[] coefficients, int row)
    {
        int parameterCount = coefficients.Length;
        double eta = 0.0;
        for (int column = 0; column < parameterCount; column++)
        {
            eta += matrix[(row * parameterCount) + column] * coefficients[column];
        }

        return eta;
    }

    /// <summary>Whether every solved coefficient is a number the next iteration can use.</summary>
    /// <remarks>
    /// <c>double.IsFinite</c> is not on netstandard2.0, so the two halves are asked separately
    /// rather than through a polyfill for one call site.
    /// </remarks>
    private static bool AllFinite(double[] values)
    {
        for (int i = 0; i < values.Length; i++)
        {
            double value = values[i];
            if (double.IsNaN(value) || double.IsInfinity(value))
            {
                return false;
            }
        }

        return true;
    }

    /// <summary>Keeps a mean away from the boundary its family's link cannot take.</summary>
    private static double Clamp(GlmFamily family, double mu) => family switch
    {
        GlmFamily.Binomial => Math.Min(Math.Max(mu, BoundaryEpsilon), 1.0 - BoundaryEpsilon),
        GlmFamily.Poisson or GlmFamily.NegativeBinomial => Math.Max(mu, BoundaryEpsilon),
        _ => mu,
    };

    /// <summary>Refuses a Gamma mean the inverse link has taken to zero or below, where no Gamma density exists.</summary>
    /// <remarks>
    /// Nothing keeps <c>1/η</c> positive, which is why the reference warns when the link is built; it then
    /// carries on with <c>|μ|</c> in its variance and a clipped deviance. The log link cannot reach here (#770).
    /// </remarks>
    private static void RefuseANonPositiveGammaMean(in FamilyShape shape, double[] mean, int iteration, string designName)
    {
        // One scan after the means are formed rather than a call inside the per-row loop the other families share (#770).
        if (shape.Family != GlmFamily.Gamma)
        {
            return;
        }

        for (int row = 0; row < mean.Length; row++)
        {
            if (!(mean[row] > 0.0))
            {
                throw new ArgumentException(
                    $"IRLS iteration {iteration} took a Gamma mean to {mean[row]} through the inverse link, where the "
                    + "family has no density. GlmLink.Log keeps every mean positive.",
                    designName);
            }
        }
    }
}
