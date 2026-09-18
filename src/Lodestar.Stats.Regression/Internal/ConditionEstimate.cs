namespace Lodestar.Stats.Regression.Internal;

/// <summary>The condition number of an upper-triangular factor, by power iteration.</summary>
/// <remarks>
/// LAPACK's <c>dtrcon</c> estimates rather than computes for the same reason: the one-sided Jacobi
/// spectrum of a 251 × 251 factor costs 82 ms on a Ryzen 7 8700G, where a whole 4 000-row fit by
/// reflections costs 41 ms, so measuring the condition exactly costs twice the detour it exists to
/// avoid (#985). Power iteration on <c>AᵀA</c> needs two triangular products an iteration, which is
/// <c>O(p²)</c> against the sweep's <c>O(p³)</c>.
/// </remarks>
internal static class ConditionEstimate
{
    // Measured on unit-column factors of order 8 to 251: stops within 1% of the spectrum, 2.4 ms at
    // the largest, where 1e-3 stops at 0.6 ms and 26% under. The cap is for a factor with no gap.
    private const double Converged = 1e-5;
    private const int MaximumIterations = 100;

    /// <summary>Estimates <c>κ₂</c> of an upper-triangular block from it and its inverse.</summary>
    /// <remarks>
    /// Each half is a Rayleigh quotient, so each is a lower bound and the product is one too: the
    /// estimate errs towards accepting. That is affordable here and nowhere else — the caller's
    /// limit of 200 stands for <c>200²·ε</c>, 9e-12 against a corpus compared at 1e-9, so a factor
    /// of two in the estimate stays a hundredfold inside the budget it guards.
    /// </remarks>
    /// <param name="upper">The block, row-major, entries below the diagonal ignored.</param>
    /// <param name="inverse">Its inverse, row-major and upper-triangular too.</param>
    /// <param name="order">The order of both.</param>
    /// <returns>An estimate of <c>σmax/σmin</c>, at or below the true value.</returns>
    internal static double Of(double[] upper, double[] inverse, int order)
        => LargestSingularValue(upper, order) * LargestSingularValue(inverse, order);

    /// <summary>The largest singular value of an upper-triangular block, by power iteration on <c>AᵀA</c>.</summary>
    private static double LargestSingularValue(double[] a, int order)
    {
        var v = new double[order];
        var y = new double[order];
        // Unit, not all-ones: ‖A·v‖ is the Rayleigh quotient only for a unit v, and an inflated
        // first value made the climb below read as converged while 2.8x under the spectrum.
        double start = 1.0 / Math.Sqrt(order);
        for (int i = 0; i < order; i++)
        {
            v[i] = start;
        }

        double previous = 0.0;
        double sigma = 0.0;
        for (int iteration = 0; iteration < MaximumIterations; iteration++)
        {
            sigma = Multiply(a, v, y, order);
            double next = MultiplyTransposed(a, y, v, order);
            if (!Usable(sigma) || !Usable(next))
            {
                // Infinity, not zero: a norm the iteration cannot carry leaves κ unknown, and an
                // unknown κ must send the fit to the reflections rather than through the gate.
                return Usable(sigma) ? sigma : double.PositiveInfinity;
            }

            for (int column = 0; column < order; column++)
            {
                v[column] /= next;
            }

            if (sigma - previous <= Converged * sigma)
            {
                break;
            }

            previous = sigma;
        }

        return sigma;
    }

    /// <summary>A norm the iteration can divide by and carry forward.</summary>
    /// <remarks><c>double.IsFinite</c> is not on netstandard2.0, so the two halves are asked separately.</remarks>
    private static bool Usable(double norm) => norm > 0.0 && !double.IsInfinity(norm) && !double.IsNaN(norm);

    /// <summary>Writes <c>y = A·v</c> for upper-triangular <paramref name="a"/> and reports its norm.</summary>
    private static double Multiply(double[] a, double[] v, double[] y, int order)
    {
        double squared = 0.0;
        for (int row = 0; row < order; row++)
        {
            double sum = 0.0;
            for (int column = row; column < order; column++)
            {
                sum += a[(row * order) + column] * v[column];
            }

            y[row] = sum;
            squared += sum * sum;
        }

        return Math.Sqrt(squared);
    }

    /// <summary>Writes <c>v = Aᵀ·y</c> for upper-triangular <paramref name="a"/> and reports its norm.</summary>
    private static double MultiplyTransposed(double[] a, double[] y, double[] v, int order)
    {
        double squared = 0.0;
        for (int column = 0; column < order; column++)
        {
            double sum = 0.0;
            for (int row = 0; row <= column; row++)
            {
                sum += a[(row * order) + column] * y[row];
            }

            v[column] = sum;
            squared += sum * sum;
        }

        return Math.Sqrt(squared);
    }
}
