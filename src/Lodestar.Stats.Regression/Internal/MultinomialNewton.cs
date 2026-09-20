namespace Lodestar.Stats.Regression.Internal;

/// <summary>One Newton fit of a multinomial logit: the estimate, its covariance's diagonal, and how the loop ended.</summary>
/// <param name="Coefficients">The estimate, equation by equation, <c>(J − 1) × K</c> row-major.</param>
/// <param name="Variances">The diagonal of <c>(−H)⁻¹</c> at the estimate, in the same order.</param>
/// <param name="LogLikelihood">The log-likelihood at the estimate.</param>
/// <param name="Iterations">How many Newton steps were taken.</param>
/// <param name="Converged">Whether the loop stopped before the budget ran out.</param>
internal sealed record MultinomialFit(
    double[] Coefficients, double[] Variances, double LogLikelihood, int Iterations, bool Converged);

/// <summary>Newton-Raphson on the multinomial logit likelihood, as <c>statsmodels</c>' <c>_fit_newton</c> runs it for <c>MNLogit</c>.</summary>
/// <remarks>
/// The reference minimises <c>−loglike/n</c> from zeros, adds <c>1e-10</c> to the diagonal of that function's Hessian
/// before each solve, and stops once no parameter moves by more than the tolerance. The score and Hessian are the
/// analytic ones, which is why the fit reproduces at <c>1e-15</c> where the ordered model's numerical ones do not
/// (decision 0004). The Hessian is factored by Cholesky: a factor that fails is a separated or rank-deficient fit.
/// </remarks>
internal static class MultinomialNewton
{
    /// <summary>The ridge <c>_fit_newton</c> adds to the scaled Hessian's diagonal (statsmodels #1847).</summary>
    private const double Ridge = 1e-10;

    public static MultinomialFit Fit(
        double[] matrix, int[] labels, int columnCount, int categoryCount, MultinomialLogitOptions options, string designName)
    {
        int rowCount = labels.Length;
        int parameterCount = (categoryCount - 1) * columnCount;
        var coefficients = new double[parameterCount];
        var probabilities = new double[rowCount * categoryCount];
        var hessian = new double[parameterCount * parameterCount];
        var score = new double[parameterCount];
        var scaled = new double[hessian.Length];
        var direction = new double[parameterCount];
        var exponentials = new double[categoryCount];
        double largestStep = double.PositiveInfinity;
        int iterations = 0;

        while (iterations < options.MaximumIterations && largestStep > options.Tolerance)
        {
            Probabilities(matrix, coefficients, columnCount, categoryCount, probabilities, exponentials);
            NegativeHessian(matrix, probabilities, columnCount, categoryCount, hessian, (labels, score));

            for (int i = 0; i < hessian.Length; i++)
            {
                scaled[i] = hessian[i] / rowCount;
            }

            for (int i = 0; i < parameterCount; i++)
            {
                scaled[(i * parameterCount) + i] += Ridge;
                direction[i] = score[i] / rowCount;
            }

            double[] step = Solve(scaled, direction, parameterCount, iterations + 1, designName);
            largestStep = 0.0;
            for (int i = 0; i < parameterCount; i++)
            {
                coefficients[i] += step[i];
                largestStep = Math.Max(largestStep, Math.Abs(step[i]));
            }

            iterations++;
        }

        Probabilities(matrix, coefficients, columnCount, categoryCount, probabilities, exponentials);
        NegativeHessian(matrix, probabilities, columnCount, categoryCount, hessian, null);
        return new MultinomialFit(
            coefficients,
            InverseDiagonal(hessian, parameterCount, designName),
            LogLikelihood(labels, probabilities, categoryCount),
            iterations,
            iterations < options.MaximumIterations);
    }

    /// <summary>Each row's category probabilities, a softmax with the reference category's predictor fixed at zero.</summary>
    /// <remarks><paramref name="exponentials"/> is a buffer one category count long, so each exponential is taken once.</remarks>
    private static void Probabilities(
        double[] matrix, double[] coefficients, int columnCount, int categoryCount, double[] probabilities, double[] exponentials)
    {
        int rowCount = probabilities.Length / categoryCount;
        var eta = new double[categoryCount];
        for (int row = 0; row < rowCount; row++)
        {
            double largest = Predictors(matrix, coefficients, row * columnCount, columnCount, eta);

            // Shifted by the largest predictor so no exponential overflows before the ratio is taken.
            double total = 0.0;
            for (int category = 0; category < categoryCount; category++)
            {
                double exponential = Math.Exp(eta[category] - largest);
                exponentials[category] = exponential;
                total += exponential;
            }

            for (int category = 0; category < categoryCount; category++)
            {
                probabilities[(row * categoryCount) + category] = exponentials[category] / total;
            }
        }
    }

    /// <summary>One row's linear predictors, the reference category's at zero; returns the largest.</summary>
    private static double Predictors(double[] matrix, double[] coefficients, int at, int columnCount, double[] eta)
    {
        eta[0] = 0.0;
        double largest = 0.0;
        for (int category = 1; category < eta.Length; category++)
        {
            int offset = (category - 1) * columnCount;
            double value = 0.0;
            for (int column = 0; column < columnCount; column++)
            {
                value += matrix[at + column] * coefficients[offset + column];
            }

            eta[category] = value;
            largest = Math.Max(largest, value);
        }

        return largest;
    }

    /// <summary>The negated Hessian of the log-likelihood, <c>Σᵢ pᵢⱼ(δⱼₗ − pᵢₗ) xᵢxᵢᵀ</c> block by block, and the score in the same pass when asked.</summary>
    /// <remarks>
    /// Only the blocks with <c>l ≥ j</c> are summed. Block <c>(l, j)</c> weighs each row by <c>pₗ(0 − pⱼ)</c>, the
    /// same product as <c>pⱼ(0 − pₗ)</c> but for the sign of a zero, and a sum that starts at +0 cannot keep a −0, so
    /// copying the block is exact. The score, <c>Σᵢ xᵢ(dᵢⱼ − pᵢⱼ)</c> per equation, sums its rows in the same order.
    /// </remarks>
    private static void NegativeHessian(
        double[] matrix,
        double[] probabilities,
        int columnCount,
        int categoryCount,
        double[] hessian,
        (int[] Labels, double[] Score)? gradient)
    {
        int equations = categoryCount - 1;
        int order = equations * columnCount;
        int rowCount = probabilities.Length / categoryCount;
        Array.Clear(hessian, 0, hessian.Length);
        if (gradient is { } cleared)
        {
            Array.Clear(cleared.Score, 0, cleared.Score.Length);
        }

        for (int row = 0; row < rowCount; row++)
        {
            int at = row * columnCount;
            int p = row * categoryCount;
            for (int j = 0; j < equations; j++)
            {
                double pj = probabilities[p + j + 1];
                for (int l = j; l < equations; l++)
                {
                    double weight = pj * ((j == l ? 1.0 : 0.0) - probabilities[p + l + 1]);
                    AddOuterBlock(hessian, matrix, at, columnCount, order, (j * columnCount * order) + (l * columnCount), weight);
                }
            }

            if (gradient is { } scored)
            {
                AddScoreRow(matrix, scored.Labels[row], probabilities, row, columnCount, categoryCount, scored.Score);
            }
        }

        MirrorBlocks(hessian, columnCount, equations);
    }

    /// <summary>One row's term of the score, <c>xᵢ(dᵢⱼ − pᵢⱼ)</c> per equation.</summary>
    private static void AddScoreRow(
        double[] matrix, int label, double[] probabilities, int row, int columnCount, int categoryCount, double[] score)
    {
        int at = row * columnCount;
        for (int category = 1; category < categoryCount; category++)
        {
            double residual = (label == category ? 1.0 : 0.0) - probabilities[(row * categoryCount) + category];
            int offset = (category - 1) * columnCount;
            for (int column = 0; column < columnCount; column++)
            {
                score[offset + column] += residual * matrix[at + column];
            }
        }
    }

    /// <summary>Copies block <c>(j, l)</c> onto block <c>(l, j)</c> for every <c>l &gt; j</c>, cell for cell.</summary>
    private static void MirrorBlocks(double[] hessian, int columnCount, int equations)
    {
        int order = equations * columnCount;
        for (int j = 0; j < equations; j++)
        {
            for (int l = j + 1; l < equations; l++)
            {
                for (int k = 0; k < columnCount; k++)
                {
                    int source = (j * columnCount * order) + (k * order) + (l * columnCount);
                    int target = (l * columnCount * order) + (k * order) + (j * columnCount);
                    Array.Copy(hessian, source, hessian, target, columnCount);
                }
            }
        }
    }

    /// <summary>Adds <c>w·xᵢxᵢᵀ</c> into the <c>K × K</c> block that starts at <paramref name="blockStart"/>.</summary>
    private static void AddOuterBlock(
        double[] hessian, double[] matrix, int at, int columnCount, int order, int blockStart, double weight)
    {
        for (int k = 0; k < columnCount; k++)
        {
            double scaled = weight * matrix[at + k];
            int rowStart = blockStart + (k * order);
            for (int r = 0; r < columnCount; r++)
            {
                hessian[rowStart + r] += scaled * matrix[at + r];
            }
        }
    }

    private static double LogLikelihood(int[] labels, double[] probabilities, int categoryCount)
    {
        double total = 0.0;
        for (int row = 0; row < labels.Length; row++)
        {
            total += Math.Log(probabilities[(row * categoryCount) + labels[row]]);
        }

        return total;
    }

    /// <summary>Solves <c>A x = b</c> for a symmetric positive definite <c>A</c> by Cholesky, or refuses a fit whose Hessian is not.</summary>
    private static double[] Solve(double[] matrix, double[] rightHandSide, int order, int iteration, string designName)
    {
        if (!Cholesky.TryFactor(matrix, order, out double[] lower))
        {
            throw NotPositiveDefinite(iteration, designName);
        }

        double[] x = [.. rightHandSide];
        Cholesky.ForwardSubstitute(lower, order, x);
        for (int row = order - 1; row >= 0; row--)
        {
            double value = x[row];
            for (int column = row + 1; column < order; column++)
            {
                value -= lower[(column * order) + row] * x[column];
            }

            x[row] = value / lower[(row * order) + row];
            if (double.IsNaN(x[row]) || double.IsInfinity(x[row]))
            {
                throw NotPositiveDefinite(iteration, designName);
            }
        }

        return x;
    }

    /// <summary>The diagonal of <c>A⁻¹</c> as the squared column norms of <c>L⁻¹</c>, for <c>A = LLᵀ</c>.</summary>
    private static double[] InverseDiagonal(double[] matrix, int order, string designName)
    {
        if (!Cholesky.TryFactor(matrix, order, out double[] lower))
        {
            throw NotPositiveDefinite(0, designName);
        }

        var diagonal = new double[order];
        var column = new double[order];
        for (int unit = 0; unit < order; unit++)
        {
            Array.Clear(column, 0, order);
            column[unit] = 1.0;
            Cholesky.ForwardSubstitute(lower, order, column);
            // L⁻¹eᵤ is column u of L⁻¹, and (LLᵀ)⁻¹ = L⁻ᵀL⁻¹ has that column's squared norm on its diagonal.
            double total = 0.0;
            for (int k = unit; k < order; k++)
            {
                total += column[k] * column[k];
            }

            diagonal[unit] = total;
        }

        return diagonal;
    }

    private static ArgumentException NotPositiveDefinite(int iteration, string designName) => new(
        (iteration > 0 ? $"Newton iteration {iteration}" : "The fitted model")
        + " has a Hessian that is not positive definite. A category that the regressors separate perfectly, or a "
        + "regressor that is a combination of the others, leaves the likelihood without a finite maximum.",
        designName);
}
