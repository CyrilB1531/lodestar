namespace Lodestar.Survival.Internal;

/// <summary>How <see cref="ParametricNewton"/> scales its search.</summary>
/// <param name="MaximumReach">A step's largest move, relative to each parameter's size or one, past which it is shortened to
/// it; infinite where the model sets none.</param>
/// <param name="OwnScale">Whether a parameter far from one is shifted in its own scale, or every one in the same.</param>
/// <param name="WeightScale">The observations' mean weight, which scales the likelihood and floors its convergence bounds.</param>
/// <param name="Restart">Where Newton starts instead if the likelihood is flat at its start; none if null.</param>
/// <param name="OnShift">Told when a step leaves Newton's, whose path alone does not depend on the parameterization.</param>
internal sealed record NewtonSettings(double MaximumReach, bool OwnScale, double WeightScale, double[]? Restart = null, Action? OnShift = null);

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
    private const double StallReach = 1e-6;

    // Past this size a parameter is shifted in its own scale: from one, a generalized gamma path at λ = −2 left the
    // maximum at −1.18 for a ridge, where a log-logistic scale of 2e5 needs it.
    private const double ScaledFrom = 10.0;

    /// <summary>The maximum, its log-likelihood and the negated Hessian there.</summary>
    /// <param name="logLikelihood">The likelihood, as a jet over the parameters.</param>
    /// <param name="start">Where Newton starts.</param>
    /// <param name="lowerBounds">Each parameter's lower bound.</param>
    /// <param name="maximumIterations">The step budget.</param>
    /// <param name="settings">How the search is scaled and where it restarts.</param>
    /// <exception cref="InvalidOperationException">No maximum within <paramref name="maximumIterations"/> steps.</exception>
    public static (double[] Parameters, double LogLikelihood, double[] Information) Maximize(
        Func<double[], Jet> logLikelihood, double[] start, double[] lowerBounds, int maximumIterations,
        NewtonSettings settings)
    {
        double[]? restart = settings.Restart;
        double weightScale = settings.WeightScale;
        double[] theta = [.. start];
        Jet current = logLikelihood(theta);
        if (restart is not null && Flat(current) && logLikelihood(restart) is { } moved && Finite(moved) && !Flat(moved))
        {
            (theta, current) = ([.. restart], moved);
        }

        double highest = current.Value;
        (double[] Theta, Jet Jet) flattest = (theta, current);
        int stalled = 0;
        for (int iteration = 0; iteration < maximumIterations; iteration++)
        {
            (theta, current, double largest) = Step(logLikelihood, theta, current, lowerBounds, settings);
            if (double.IsNaN(largest) || (largest <= StepTolerance && Settled(current, weightScale)))
            {
                return Answer(theta, current, iteration + 1, maximumIterations, early: true, weightScale);
            }

            // Five steps with no gain: the derivatives' noise now sets the step, and the likelihood differs only by
            // rounding, so the point of smallest gradient is the answer — if it has vanished at all.
            stalled = current.Value > highest + (1e-15 * Math.Abs(highest)) ? 0 : stalled + 1;
            highest = Math.Max(highest, current.Value);
            flattest = Norm(current) < Norm(flattest.Jet) ? (theta, current) : flattest;
            if (stalled >= StallLimit)
            {
                return Stalled(flattest.Theta, flattest.Jet, iteration + 1, maximumIterations, settings);
            }
        }

        // A budget spent while the steps still moved is no maximum, however small the decrement: three steps of a 2,000-time
        // log-normal fit sat 1.2e-6 from it, inside the decrement's bound (#1756).
        throw NotConverged(maximumIterations, maximumIterations, early: false);
    }

    /// <summary>The search over the first space, and where it left Newton's path also over the second, with the higher maximum
    /// of the two whose information factors: a shift or a gradient step depends on the parameters, so one path can stop at a
    /// lower maximum or none, where the other does not (#1756).</summary>
    public static (TSpace Space, (double[] Parameters, double LogLikelihood, double[] Information) Fit) HighestMaximum<TSpace>(
        TSpace first, TSpace second, double weightScale, Func<TSpace, Action?, (double[] Parameters, double LogLikelihood, double[] Information)> search)
    {
        bool shifted = false;
        (double[] Parameters, double LogLikelihood, double[] Information) found;
        try
        {
            found = search(first, () => shifted = true);
        }
        catch (InvalidOperationException)
        {
            return (second, search(second, null));
        }

        if (!shifted)
        {
            return (first, found);
        }

        try
        {
            (double[] Parameters, double LogLikelihood, double[] Information) other = search(second, null);
            bool higher = other.LogLikelihood > found.LogLikelihood + (1e-9 * Math.Max(weightScale, Math.Abs(found.LogLikelihood)));
            int k = found.Parameters.Length;
            return Factors(other.Information, k) && (higher || !Factors(found.Information, k)) ? (second, other) : (first, found);
        }
        catch (InvalidOperationException)
        {
            return (first, found);
        }
    }

    private static bool Factors(double[] information, int k) => Cholesky.TryFactor(information, k, out _);

    /// <summary>One Newton step, or a step up the gradient where a clip has flattened the Hessian and Newton's does not climb.</summary>
    private static (double[] Next, Jet Evaluated, double Largest) Step(
        Func<double[], Jet> logLikelihood, double[] theta, Jet current, double[] lowerBounds, NewtonSettings settings)
    {
        (double[] Next, Jet Evaluated, double Largest) step = LineSearch(logLikelihood, theta, current, Direction(current, theta, settings), lowerBounds, settings);
        if (!double.IsNaN(step.Largest) || Stationary(current, theta, settings.WeightScale))
        {
            return step;
        }

        settings.OnShift?.Invoke();
        return LineSearch(logLikelihood, theta, current, [.. current.Gradient], lowerBounds, settings);
    }

    /// <summary>The point reached, if its gradient has vanished: a stopped search is no maximum by itself.</summary>
    private static (double[] Parameters, double LogLikelihood, double[] Information) Answer(
        double[] theta, Jet at, int taken, int maximumIterations, bool early, double weightScale) =>
        Stationary(at, theta, weightScale) ? (theta, at.Value, Negated(at.Hessian)) : throw NotConverged(taken, maximumIterations, early);

    /// <summary>The point a stalled search reached, if Newton's step there is short as well as its gradient vanished: a ridge
    /// stalls too, a generalized gamma's moving λ by 1.6% at 78,991, where 1,117 stalls at a maximum, over 97,920 fits, moved
    /// each parameter by under 1e-9 of its size (#1756).</summary>
    private static (double[] Parameters, double LogLikelihood, double[] Information) Stalled(
        double[] theta, Jet at, int taken, int maximumIterations, NewtonSettings settings) =>
        Reach(Direction(at, theta, settings), theta) > StallReach
            ? throw NotConverged(taken, maximumIterations, early: true)
            : Answer(theta, at, taken, maximumIterations, early: true, settings.WeightScale);

    private static double Norm(Jet at) => at.Gradient.Sum(g => g * g);

    /// <summary>A step's largest move, relative to each parameter's size or one.</summary>
    private static double Reach(double[] direction, double[] theta)
    {
        double reach = 0.0;
        for (int j = 0; j < theta.Length; j++)
        {
            reach = Math.Max(reach, Math.Abs(direction[j]) / Math.Max(1.0, Math.Abs(theta[j])));
        }
        return reach;
    }

    /// <summary>The refusal, naming whether the budget ran out or the steps stopped short of a maximum, where a larger
    /// budget changes nothing.</summary>
    private static InvalidOperationException NotConverged(int taken, int maximumIterations, bool early) =>
        new(early
            ? $"The parametric fit stopped after {taken} of its {maximumIterations} Newton steps, its steps no longer moving "
                + "it short of a maximum; the likelihood may have no maximum on this sample, or the model "
                + "may not fit it."
            : $"The parametric fit did not converge within {maximumIterations} Newton steps; the likelihood may have no "
                + "maximum on this sample, or the model may not fit it.");

    /// <summary>The Newton direction, the Hessian shifted towards negative definiteness until it factors, each parameter
    /// past <see cref="ScaledFrom"/> over its size: shifted in the parameters themselves, a log-logistic scale of 2e5
    /// moved by 13 a step, 7.7 below its maximum.</summary>
    private static double[] Direction(Jet current, double[] theta, NewtonSettings settings)
    {
        int k = current.Gradient.Length;
        double[] size = [.. theta.Select(t => settings.OwnScale && (Math.Abs(t) > ScaledFrom || (t != 0.0 && Math.Abs(t) < 1.0 / ScaledFrom / ScaledFrom)) ? Math.Abs(t) : 1.0)];
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

            settings.OnShift?.Invoke();
            shift = attempt == 0 ? 1e-8 * Math.Max(settings.WeightScale, MaxDiagonal(information, k)) : shift * 10.0;
        }

        return [.. current.Gradient];
    }

    /// <summary>Halves the step until it stays inside the bounds and does not lower the likelihood; a Largest of NaN
    /// where no halving does.</summary>
    private static (double[] Next, Jet Evaluated, double Largest) LineSearch(
        Func<double[], Jet> logLikelihood, double[] theta, Jet current, double[] direction, double[] lowerBounds,
        NewtonSettings settings)
    {
        double maximumReach = settings.MaximumReach;
        int k = theta.Length;
        double reach = Reach(direction, theta);
        if (reach > maximumReach)
        {
            direction = [.. direction.Select(d => d * (maximumReach / reach))];
        }
        double[] next = new double[k];
        Jet evaluated = current;
        double scale = NewtonSearch.Scale(current.Value, Math.Max(settings.WeightScale, Math.Abs(current.Value)), fraction =>
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
    private static bool Stationary(Jet current, double[] theta, double weightScale)
    {
        double size = Math.Max(weightScale, Math.Abs(current.Value));
        int k = current.Gradient.Length;
        if (!Cholesky.TryFactor(Negated(current.Hessian), k, out double[] lower))
        {
            // Each component against its parameter's size, as the decrement weighs it: alone, a scale of 2e5 met the bound at
            // a gradient of 1.6e-5, 7.7 below the maximum.
            return !Flat(current) && current.Gradient.Select((g, j) => Math.Abs(g) * Math.Max(1.0, Math.Abs(theta[j]))).All(g => g <= 1e-6 * size);
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
    private static bool Settled(Jet current, double weightScale)
    {
        int k = current.Gradient.Length;
        return Cholesky.TryFactor(Negated(current.Hessian), k, out double[] lower)
            && Decrement(current, lower) <= 1e-24 * Math.Max(weightScale, Math.Abs(current.Value));
    }

    /// <summary>Whether no parameter moves the likelihood: every observation on lifelines' clip, a plateau and no maximum.</summary>
    private static bool Flat(Jet at) => !at.Gradient.Any(g => Math.Abs(g) > 0.0);

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
