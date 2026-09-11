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
        GlmFamily family,
        GlmOptions options)
    {
        int rowCount = response.Length;
        int parameterCount = featureCount + (options.WithIntercept ? 1 : 0);
        double[] matrix = LeastSquares.Design(design, rowCount, featureCount, options.WithIntercept);

        // The reference's start: mu from the response nudged off the boundary, which is what
        // keeps the first link evaluation finite for a binomial 0 or 1.
        var mean = new double[rowCount];
        for (int row = 0; row < rowCount; row++)
        {
            mean[row] = family == GlmFamily.Binomial
                ? (response[row] + 0.5) / 2.0
                : response[row] + 0.1;
        }

        var scaled = new double[rowCount * parameterCount];
        var working = new double[rowCount];
        double[] coefficients = new double[parameterCount];
        double[] inverseUpper = new double[parameterCount * parameterCount];
        double deviance = Deviance(family, response, mean);
        double change = double.PositiveInfinity;
        bool converged = false;
        int iteration = 0;

        while (iteration < options.MaximumIterations)
        {
            iteration++;
            BuildWeightedSystem(family, matrix, response, mean, parameterCount, scaled, working);

            coefficients = LeastSquares.Solve(
                scaled, rowCount, parameterCount, working, out inverseUpper);

            for (int row = 0; row < rowCount; row++)
            {
                double eta = 0.0;
                for (int column = 0; column < parameterCount; column++)
                {
                    eta += matrix[(row * parameterCount) + column] * coefficients[column];
                }

                mean[row] = Clamp(family, Families.InverseLink(family, eta));
            }

            double next = Deviance(family, response, mean);
            change = Math.Abs(deviance - next);
            // numpy.allclose, which is the reference's criterion: neither purely relative nor
            // purely absolute but their sum, with atol and rtol both the tolerance.
            converged = change <= options.Tolerance + (options.Tolerance * Math.Abs(next));
            deviance = next;
            if (converged)
            {
                break;
            }
        }

        return new IrlsResult(
            coefficients, inverseUpper, mean, deviance, converged, iteration, change);
    }

    public static double Deviance(GlmFamily family, ReadOnlySpan<double> response, double[] mean)
    {
        double total = 0.0;
        for (int row = 0; row < response.Length; row++)
        {
            total += Families.UnitDeviance(family, response[row], mean[row]);
        }

        return total;
    }

    /// <summary>The weighted design and working response for one IRLS iteration.</summary>
    private static void BuildWeightedSystem(
        GlmFamily family,
        double[] matrix,
        ReadOnlySpan<double> response,
        double[] mean,
        int parameterCount,
        double[] scaled,
        double[] working)
    {
        int rowCount = mean.Length;
        for (int row = 0; row < rowCount; row++)
        {
            double mu = mean[row];
            double derivative = Families.LinkDerivative(family, mu);
            double weight = 1.0 / (Families.Variance(family, mu) * derivative * derivative);
            double root = Math.Sqrt(weight);
            double eta = Link(family, mu);

            working[row] = root * (eta + ((response[row] - mu) * derivative));
            for (int column = 0; column < parameterCount; column++)
            {
                int at = (row * parameterCount) + column;
                scaled[at] = root * matrix[at];
            }
        }
    }

    /// <summary>Keeps a mean away from the boundary its family's link cannot take.</summary>
    private static double Clamp(GlmFamily family, double mu) => family switch
    {
        GlmFamily.Binomial => Math.Min(Math.Max(mu, BoundaryEpsilon), 1.0 - BoundaryEpsilon),
        GlmFamily.Poisson => Math.Max(mu, BoundaryEpsilon),
        _ => mu,
    };

    private static double Link(GlmFamily family, double mu) => family switch
    {
        GlmFamily.Binomial => Math.Log(mu / (1.0 - mu)),
        GlmFamily.Poisson => Math.Log(mu),
        _ => throw new ArgumentOutOfRangeException(nameof(family), family, null),
    };
}
