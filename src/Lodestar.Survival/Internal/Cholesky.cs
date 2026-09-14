namespace Lodestar.Survival.Internal;

/// <summary>The Cholesky factorization of a small symmetric positive-definite matrix, row-major.</summary>
/// <remarks>
/// The Cox fit's observed information is symmetric and positive definite at a well-posed
/// maximum, so one factor answers both the Newton step and the covariance. A refused factor is
/// the diagnostic: separation and collinearity are exactly what make the information singular.
/// </remarks>
internal static class Cholesky
{
    // A pivot this small against its own diagonal entry means a condition number past about
    // 1e12, where no standard error is worth reporting; a duplicated column reaches 1e-16.
    private const double RelativePivotFloor = 1e-12;

    /// <summary>Factors <c>A = L Lᵀ</c>, or reports that <paramref name="matrix"/> is not safely positive definite.</summary>
    internal static bool TryFactor(ReadOnlySpan<double> matrix, int order, out double[] lower)
    {
        lower = new double[checked(order * order)];
        for (int j = 0; j < order; j++)
        {
            double diagonal = matrix[(j * order) + j];
            double pivot = diagonal;
            for (int k = 0; k < j; k++)
            {
                double entry = lower[(j * order) + k];
                pivot -= entry * entry;
            }

            // The negated comparison also refuses a NaN, which every ordered comparison fails.
            if (!(pivot > RelativePivotFloor * Math.Abs(diagonal)) || double.IsInfinity(pivot))
            {
                return false;
            }

            double root = Math.Sqrt(pivot);
            lower[(j * order) + j] = root;
            for (int i = j + 1; i < order; i++)
            {
                double sum = matrix[(i * order) + j];
                for (int k = 0; k < j; k++)
                {
                    sum -= lower[(i * order) + k] * lower[(j * order) + k];
                }

                lower[(i * order) + j] = sum / root;
            }
        }

        return true;
    }

    /// <summary>Solves <c>L Lᵀ x = b</c> by one forward and one backward substitution.</summary>
    internal static double[] Solve(ReadOnlySpan<double> lower, int order, ReadOnlySpan<double> rightHandSide)
    {
        double[] solution = rightHandSide.ToArray();
        for (int i = 0; i < order; i++)
        {
            double sum = solution[i];
            for (int k = 0; k < i; k++)
            {
                sum -= lower[(i * order) + k] * solution[k];
            }

            solution[i] = sum / lower[(i * order) + i];
        }

        for (int i = order - 1; i >= 0; i--)
        {
            double sum = solution[i];
            for (int k = i + 1; k < order; k++)
            {
                sum -= lower[(k * order) + i] * solution[k];
            }

            solution[i] = sum / lower[(i * order) + i];
        }

        return solution;
    }

    /// <summary>The inverse of <c>L Lᵀ</c>, row-major, one solve per column of the identity.</summary>
    internal static double[] Inverse(ReadOnlySpan<double> lower, int order)
    {
        double[] inverse = new double[checked(order * order)];
        double[] unit = new double[order];
        for (int column = 0; column < order; column++)
        {
            Array.Clear(unit, 0, order);
            unit[column] = 1.0;
            double[] solved = Solve(lower, order, unit);
            for (int row = 0; row < order; row++)
            {
                inverse[(row * order) + column] = solved[row];
            }
        }

        return inverse;
    }
}
