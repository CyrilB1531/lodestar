namespace Lodestar.Survival.Internal;

/// <summary>Newton-Raphson to the maximum of a parametric log-likelihood, kept inside the parameters' bounds.</summary>
/// <remarks>
/// long-comment: why Newton rather than lifelines' own optimiser.
/// lifelines runs Nelder-Mead and then L-BFGS-B, which stop 1e-6 to 1e-3 short of the maximum (#1160); the parity
/// target is the maximum itself, which Newton on the exact derivatives reaches from lifelines' start in a handful of
/// steps. A step that leaves the bounds or lowers the likelihood is halved; where the Hessian is not negative definite
/// a multiple of the identity is added until it is, which is Levenberg and Marquardt's safeguard.
/// </remarks>
internal static class ParametricNewton
{
    private const double StepTolerance = 1e-12;
    private const int StallLimit = 5;

    /// <summary>The maximum, its log-likelihood and the negated Hessian there.</summary>
    /// <exception cref="InvalidOperationException">No maximum within <paramref name="maximumIterations"/> steps.</exception>
    public static (double[] Parameters, double LogLikelihood, double[] Information) Maximize(
        Func<double[], Jet> logLikelihood, double[] start, double[] lowerBounds, int maximumIterations)
    {
        double[] theta = [.. start];
        Jet current = logLikelihood(theta);
        double highest = current.Value;
        (double[] Theta, Jet Jet) flattest = (theta, current);
        int stalled = 0;
        for (int iteration = 0; iteration < maximumIterations; iteration++)
        {
            (theta, current, double largest) = Step(logLikelihood, theta, current, lowerBounds);
            if (double.IsNaN(largest) || largest <= StepTolerance)
            {
                return Answer(theta, current, maximumIterations);
            }

            // Five steps with no gain: the derivatives' noise now sets the step, and the likelihood differs only by
            // rounding, so the point of smallest gradient is the answer — if it has vanished at all.
            stalled = current.Value > highest + (1e-15 * Math.Abs(highest)) ? 0 : stalled + 1;
            highest = Math.Max(highest, current.Value);
            flattest = Norm(current) < Norm(flattest.Jet) ? (theta, current) : flattest;
            if (stalled >= StallLimit)
            {
                return Answer(flattest.Theta, flattest.Jet, maximumIterations);
            }
        }

        return Answer(flattest.Theta, flattest.Jet, maximumIterations);
    }

    /// <summary>One Newton step, or a step up the gradient where a clip has flattened the Hessian and Newton's does not climb.</summary>
    private static (double[] Next, Jet Evaluated, double Largest) Step(
        Func<double[], Jet> logLikelihood, double[] theta, Jet current, double[] lowerBounds)
    {
        (double[] Next, Jet Evaluated, double Largest) step = LineSearch(logLikelihood, theta, current, Direction(current), lowerBounds);
        return double.IsNaN(step.Largest) && !Stationary(current)
            ? LineSearch(logLikelihood, theta, current, [.. current.Gradient], lowerBounds)
            : step;
    }

    /// <summary>The point reached, if its gradient has vanished: a stopped search is no maximum by itself.</summary>
    private static (double[] Parameters, double LogLikelihood, double[] Information) Answer(
        double[] theta, Jet at, int maximumIterations) =>
        Stationary(at) ? (theta, at.Value, Negated(at.Hessian)) : throw NotConverged(maximumIterations);

    private static double Norm(Jet at) => at.Gradient.Sum(g => g * g);

    private static InvalidOperationException NotConverged(int maximumIterations) =>
        new($"The parametric fit did not converge within {maximumIterations} Newton steps; the likelihood may have no "
            + "maximum on this sample, or the model may not fit it.");

    /// <summary>The Newton direction, the Hessian shifted towards negative definiteness until it factors.</summary>
    private static double[] Direction(Jet current)
    {
        int k = current.Gradient.Length;
        double[] information = Negated(current.Hessian);
        double shift = 0.0;
        for (int attempt = 0; attempt < 60; attempt++)
        {
            double[] shifted = [.. information];
            for (int j = 0; j < k; j++)
            {
                shifted[(j * k) + j] += shift;
            }

            if (Cholesky.TryFactor(shifted, k, out double[] lower))
            {
                return Cholesky.Solve(lower, k, current.Gradient);
            }

            shift = attempt == 0 ? 1e-8 * Math.Max(1.0, MaxDiagonal(information, k)) : shift * 10.0;
        }

        return [.. current.Gradient];
    }

    /// <summary>Halves the step until it stays inside the bounds and does not lower the likelihood.</summary>
    private static (double[] Next, Jet Evaluated, double Largest) LineSearch(
        Func<double[], Jet> logLikelihood, double[] theta, Jet current, double[] direction, double[] lowerBounds)
    {
        int k = theta.Length;
        double scale = 1.0;
        for (int halving = 0; halving < 80; halving++)
        {
            var next = new double[k];
            bool inside = true;
            double largest = 0.0;
            for (int j = 0; j < k; j++)
            {
                next[j] = theta[j] + (scale * direction[j]);
                inside &= next[j] > lowerBounds[j];
                largest = Math.Max(largest, Math.Abs(scale * direction[j]) / Math.Max(1.0, Math.Abs(theta[j])));
            }

            if (inside)
            {
                Jet evaluated = logLikelihood(next);
                if (!double.IsNaN(evaluated.Value) && evaluated.Value >= current.Value - (1e-12 * Math.Abs(current.Value)))
                {
                    return (next, evaluated, largest);
                }
            }

            scale /= 2.0;
        }

        // No step helps: the point is a maximum to rounding, or the search is stuck, which the caller tells apart.
        return (theta, current, double.NaN);
    }

    /// <summary>Whether the gradient has vanished to rounding, relative to the likelihood's own size.</summary>
    private static bool Stationary(Jet current) =>
        current.Gradient.All(g => Math.Abs(g) <= 1e-6 * Math.Max(1.0, Math.Abs(current.Value)));

    private static double[] Negated(double[] matrix) => [.. matrix.Select(v => -v)];

    private static double MaxDiagonal(double[] matrix, int k)
    {
        double largest = 0.0;
        for (int j = 0; j < k; j++)
        {
            largest = Math.Max(largest, Math.Abs(matrix[(j * k) + j]));
        }

        return largest;
    }
}
