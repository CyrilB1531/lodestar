namespace Lodestar.Survival.Internal;

/// <summary>Newton-Raphson to the maximum of a parametric log-likelihood, kept inside the parameters' bounds.</summary>
/// <remarks>
/// long-comment: why Newton rather than lifelines' own optimiser.
/// lifelines runs Nelder-Mead and then L-BFGS-B, which stop 1e-6 to 1e-3 short of the maximum (#1160); the parity
/// target is the maximum itself, which Newton on the exact derivatives reaches from lifelines' start in a handful of
/// steps. A step past the caller's reach is shortened, one that leaves the bounds or lowers the likelihood is halved,
/// and where the Hessian is not negative definite a multiple of the identity is added until it is, which is Levenberg
/// and Marquardt's safeguard.
/// </remarks>
internal static class ParametricNewton
{
    private const double StepTolerance = 1e-12;
    private const int StallLimit = 5;

    // Past this size a parameter is shifted in its own scale: from one, a generalized gamma path at λ = −2 left the
    // maximum at −1.18 for a ridge, where a log-logistic scale of 2e5 needs it.
    private const double ScaledFrom = 10.0;

    /// <summary>The maximum, its log-likelihood and the negated Hessian there.</summary>
    /// <param name="logLikelihood">The likelihood, as a jet over the parameters.</param>
    /// <param name="start">Where Newton starts.</param>
    /// <param name="lowerBounds">Each parameter's lower bound.</param>
    /// <param name="maximumIterations">The step budget.</param>
    /// <param name="maximumReach">A step's largest move, relative to each parameter's size or one, past which it is
    /// shortened to it; unbounded where the model sets none.</param>
    /// <exception cref="InvalidOperationException">No maximum within <paramref name="maximumIterations"/> steps.</exception>
    public static (double[] Parameters, double LogLikelihood, double[] Information) Maximize(
        Func<double[], Jet> logLikelihood, double[] start, double[] lowerBounds, int maximumIterations,
        double maximumReach = double.PositiveInfinity)
    {
        double[] theta = [.. start];
        Jet current = logLikelihood(theta);
        double highest = current.Value;
        (double[] Theta, Jet Jet) flattest = (theta, current);
        int stalled = 0;
        for (int iteration = 0; iteration < maximumIterations; iteration++)
        {
            (theta, current, double largest) = Step(logLikelihood, theta, current, lowerBounds, maximumReach);
            if (double.IsNaN(largest) || (largest <= StepTolerance && Settled(current)))
            {
                return Answer(theta, current, iteration + 1, maximumIterations, early: true);
            }

            // Five steps with no gain: the derivatives' noise now sets the step, and the likelihood differs only by
            // rounding, so the point of smallest gradient is the answer — if it has vanished at all.
            stalled = current.Value > highest + (1e-15 * Math.Abs(highest)) ? 0 : stalled + 1;
            highest = Math.Max(highest, current.Value);
            flattest = Norm(current) < Norm(flattest.Jet) ? (theta, current) : flattest;
            if (stalled >= StallLimit)
            {
                return Answer(flattest.Theta, flattest.Jet, iteration + 1, maximumIterations, early: true);
            }
        }

        // A budget spent while the steps still moved is no maximum, however small the decrement: three steps of a 2,000-time
        // log-normal fit sat 1.2e-6 from it, inside the decrement's bound (#1756).
        throw NotConverged(maximumIterations, maximumIterations, early: false);
    }

    /// <summary>One Newton step, or a step up the gradient where a clip has flattened the Hessian and Newton's does not climb.</summary>
    private static (double[] Next, Jet Evaluated, double Largest) Step(
        Func<double[], Jet> logLikelihood, double[] theta, Jet current, double[] lowerBounds, double maximumReach)
    {
        (double[] Next, Jet Evaluated, double Largest) step = LineSearch(logLikelihood, theta, current, Direction(current, theta), lowerBounds, maximumReach);
        return double.IsNaN(step.Largest) && !Stationary(current, theta)
            ? LineSearch(logLikelihood, theta, current, [.. current.Gradient], lowerBounds, maximumReach)
            : step;
    }

    /// <summary>The point reached, if its gradient has vanished: a stopped search is no maximum by itself.</summary>
    private static (double[] Parameters, double LogLikelihood, double[] Information) Answer(
        double[] theta, Jet at, int taken, int maximumIterations, bool early) =>
        Stationary(at, theta) ? (theta, at.Value, Negated(at.Hessian)) : throw NotConverged(taken, maximumIterations, early);

    private static double Norm(Jet at) => at.Gradient.Sum(g => g * g);

    /// <summary>The refusal, naming whether the budget ran out or the steps stopped short of a maximum, where a larger
    /// budget changes nothing.</summary>
    private static InvalidOperationException NotConverged(int taken, int maximumIterations, bool early) =>
        new(early
            ? $"The parametric fit stopped after {taken} of its {maximumIterations} Newton steps, its steps no longer moving "
                + "it where its gradient had not vanished; the likelihood may have no maximum on this sample, or the model "
                + "may not fit it."
            : $"The parametric fit did not converge within {maximumIterations} Newton steps; the likelihood may have no "
                + "maximum on this sample, or the model may not fit it.");

    /// <summary>The Newton direction, the Hessian shifted towards negative definiteness until it factors, each parameter
    /// past <see cref="ScaledFrom"/> over its size: shifted in the parameters themselves, a log-logistic scale of 2e5
    /// moved by 13 a step, 7.7 below its maximum.</summary>
    private static double[] Direction(Jet current, double[] theta)
    {
        int k = current.Gradient.Length;
        double[] size = [.. theta.Select(t => Math.Abs(t) > ScaledFrom || (t != 0.0 && Math.Abs(t) < 1.0 / ScaledFrom / ScaledFrom) ? Math.Abs(t) : 1.0)];
        double[] information = Negated(current.Hessian);
        for (int a = 0; a < k; a++)
        {
            for (int b = 0; b < k; b++)
            {
                information[(a * k) + b] *= size[a] * size[b];
            }
        }

        double[] gradient = [.. current.Gradient.Select((g, j) => g * size[j])];
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
                return [.. Cholesky.Solve(lower, k, gradient).Select((v, j) => v * size[j])];
            }

            shift = attempt == 0 ? 1e-8 * Math.Max(1.0, MaxDiagonal(information, k)) : shift * 10.0;
        }

        return [.. current.Gradient];
    }

    /// <summary>Halves the step until it stays inside the bounds and does not lower the likelihood; a Largest of NaN
    /// where no halving does.</summary>
    private static (double[] Next, Jet Evaluated, double Largest) LineSearch(
        Func<double[], Jet> logLikelihood, double[] theta, Jet current, double[] direction, double[] lowerBounds,
        double maximumReach)
    {
        int k = theta.Length;
        double reach = 0.0;
        for (int j = 0; j < k; j++)
        {
            reach = Math.Max(reach, Math.Abs(direction[j]) / Math.Max(1.0, Math.Abs(theta[j])));
        }
        if (reach > maximumReach)
        {
            direction = [.. direction.Select(d => d * (maximumReach / reach))];
        }
        double[] next = new double[k];
        Jet evaluated = current;
        double scale = NewtonSearch.Scale(current.Value, fraction =>
        {
            for (int j = 0; j < k; j++)
            {
                next[j] = theta[j] + (fraction * direction[j]);
                if (!(next[j] > lowerBounds[j]))
                {
                    return double.NaN;
                }
            }

            // A step halved below half an ulp of every parameter no longer moves them: the search ends.
            if (next.AsSpan().SequenceEqual(theta))
            {
                return null;
            }

            evaluated = logLikelihood(next);
            // A point whose derivatives overflowed is no step, whatever its value: a Weibull ρ of 1280 kept its likelihood
            // and lost its Hessian, and every direction after it read NaN (#1756).
            return Finite(evaluated) ? evaluated.Value : double.NaN;
        });
        if (double.IsNaN(scale))
        {
            return (theta, current, double.NaN);
        }

        double largest = 0.0;
        for (int j = 0; j < k; j++)
        {
            largest = Math.Max(largest, Math.Abs(scale * direction[j]) / Math.Max(1.0, Math.Abs(theta[j])));
        }

        return (next, evaluated, largest);
    }

    /// <summary>Whether Newton's decrement has vanished to the likelihood's rounding where the Hessian is negative
    /// definite, the gradient, each component times its parameter's size, where it is not. The gradient alone ignored each parameter's scale: past a maximum it
    /// read a coefficient of 192 for 501,641 on a covariate of spread 1e-6 as converged, and at one, on times of spread
    /// 1e-5 of their size, it held an ulp of μ to 2.3e-3 of gradient against a bound of 8.8e-4 (#1756).</summary>
    private static bool Stationary(Jet current, double[] theta)
    {
        double size = Math.Max(1.0, Math.Abs(current.Value));
        int k = current.Gradient.Length;
        if (!Cholesky.TryFactor(Negated(current.Hessian), k, out double[] lower))
        {
            // Each component against its parameter's size, as the decrement weighs it: alone, a scale of 2e5 met the bound at
            // a gradient of 1.6e-5, 7.7 below the maximum.
            return current.Gradient.Select((g, j) => Math.Abs(g) * Math.Max(1.0, Math.Abs(theta[j]))).All(g => g <= 1e-6 * size);
        }

        return Decrement(current, lower) <= 1e-10 * size;
    }

    /// <summary>Newton's decrement gᵀ(−H)⁻¹g from the factor of −H.</summary>
    private static double Decrement(Jet current, double[] lower)
    {
        double[] direction = Cholesky.Solve(lower, current.Gradient.Length, current.Gradient);
        double decrement = 0.0;
        for (int j = 0; j < direction.Length; j++)
        {
            decrement += current.Gradient[j] * direction[j];
        }
        return decrement;
    }

    /// <summary>Whether a step under the tolerance has also left a decrement at the likelihood's rounding, at a negative
    /// definite Hessian: a step measured against one stopped an exponential's scale of 1e-9 at 5e-9 of its maximum (#1756).</summary>
    private static bool Settled(Jet current)
    {
        int k = current.Gradient.Length;
        return Cholesky.TryFactor(Negated(current.Hessian), k, out double[] lower)
            && Decrement(current, lower) <= 1e-24 * Math.Max(1.0, Math.Abs(current.Value));
    }

    private static bool Finite(Jet at) =>
        at.Gradient.All(g => !double.IsNaN(g) && !double.IsInfinity(g)) && at.Hessian.All(h => !double.IsNaN(h) && !double.IsInfinity(h));

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
