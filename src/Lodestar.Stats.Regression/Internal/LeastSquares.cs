namespace Lodestar.Stats.Regression.Internal;

/// <summary>The least-squares arithmetic the OLS and the GLM both need.</summary>
/// <remarks>
/// Extracted rather than written twice: the GLM's IRLS solves a weighted least squares each
/// iteration, and a weighted solve is this one over rows scaled by the square root of the
/// weight (#616).
/// </remarks>
internal static class LeastSquares
{
    /// <summary>The row count, with the shapes that are not a design refused.</summary>
    /// <remarks>
    /// Shared rather than written per model: an empty design is <c>0 != 0 * featureCount</c>,
    /// which a length comparison alone admits, and it then fails further in as a missing
    /// residual degree of freedom — a diagnosis of the wrong input (#616).
    /// </remarks>
    public static int Rows(
        ReadOnlySpan<double> design, ReadOnlySpan<double> response, int featureCount)
    {
        if (design.Length == 0 || design.Length % featureCount != 0)
        {
            throw new ArgumentException(
                $"design holds {design.Length} values, which is not a positive whole number of "
                + $"rows of {featureCount}.",
                nameof(design));
        }

        int rowCount = design.Length / featureCount;
        if (response.Length != rowCount)
        {
            throw new ArgumentException(
                $"design has {rowCount} rows and response holds {response.Length} values.",
                nameof(response));
        }

        return rowCount;
    }

    /// <summary>The design matrix, with an intercept column prepended when asked.</summary>
    public static double[] Design(
        ReadOnlySpan<double> design, int rowCount, int featureCount, bool withIntercept)
    {
        int parameterCount = featureCount + (withIntercept ? 1 : 0);
        var matrix = new double[rowCount * parameterCount];
        for (int row = 0; row < rowCount; row++)
        {
            int at = row * parameterCount;
            if (withIntercept)
            {
                matrix[at++] = 1.0;
            }

            for (int column = 0; column < featureCount; column++)
            {
                matrix[at + column] = design[(row * featureCount) + column];
            }
        }

        return matrix;
    }

    /// <summary>Least squares through the normal equations when conditioned for them, Householder reflections otherwise, reporting the inverse of R the covariance needs.</summary>
    /// <returns>The coefficients, and the inverse of R the standard errors, the leverages and the sandwich are read from.</returns>
    /// <remarks>
    /// Q is never formed. The reflections are applied to the response as they are built, which is all the
    /// coefficients need, and a leverage is a row of <c>X R⁻¹</c> against itself rather than a row of Q.
    /// Forming Q, as <c>QrDecomposition.Householder</c> does, was a third of a weighted fit's time at 20 000
    /// rows, and the IRLS loop paid it every iteration and discarded it (#782).
    /// </remarks>
    /// <param name="design">The regressors, row-major, <paramref name="featureCount"/> per row, with no constant column.</param>
    /// <param name="rowCount">Rows in the design.</param>
    /// <param name="featureCount">Columns in <paramref name="design"/>.</param>
    /// <param name="withIntercept">Whether a constant is fitted ahead of them, as coefficient 0.</param>
    /// <param name="response">One value per row.</param>
    /// <param name="weights">One weight per row, or empty: a weighted fit is solved on <c>√w·X</c> and <c>√w·y</c> without that copy being made.</param>
    public static (double[] Coefficients, double[] InverseUpper) Solve(
        ReadOnlySpan<double> design,
        int rowCount,
        int featureCount,
        bool withIntercept,
        ReadOnlySpan<double> response,
        ReadOnlySpan<double> weights = default)
    {
        return TryNormalEquations(design, rowCount, featureCount, withIntercept, response, weights)
            ?? SolveByReflections(design, rowCount, featureCount, withIntercept, response, weights);
    }

    /// <summary>The Householder route <see cref="Solve"/> falls back to, reachable on its own so the two can be held to each other.</summary>
    internal static (double[] Coefficients, double[] InverseUpper) SolveByReflections(
        ReadOnlySpan<double> design,
        int rowCount,
        int featureCount,
        bool withIntercept,
        ReadOnlySpan<double> response,
        ReadOnlySpan<double> weights = default)
    {
        int parameterCount = featureCount + (withIntercept ? 1 : 0);
        double[] a = ColumnMajor(Design(design, rowCount, featureCount, withIntercept), rowCount, parameterCount);
        double[] projected = response.ToArray();
        if (!weights.IsEmpty)
        {
            Scale(a, rowCount, parameterCount, projected, weights);
        }

        Triangularize(a, rowCount, parameterCount, projected);

        return FromTriangle(a, rowCount, parameterCount, projected);
    }

    /// <summary>The reflection solve over a column-major design already scaled and without an intercept, overwriting both arguments.</summary>
    /// <remarks>
    /// What <see cref="SolveByReflections"/> computes for <c>withIntercept: false</c> and no weights, minus its three
    /// copies of the design and the response, which an iterative caller would otherwise pay on every iteration.
    /// Same values in the same order, so the same bits.
    /// </remarks>
    internal static (double[] Coefficients, double[] InverseUpper) SolveByReflectionsInPlace(
        double[] columnMajor, int rowCount, int parameterCount, double[] projected)
    {
        Triangularize(columnMajor, rowCount, parameterCount, projected);
        return FromTriangle(columnMajor, rowCount, parameterCount, projected);
    }

    /// <summary>R's inverse and the coefficients it gives, from a triangularized block and its projected response.</summary>
    private static (double[] Coefficients, double[] InverseUpper) FromTriangle(
        double[] a, int rowCount, int parameterCount, double[] projected)
    {
        double[] inverseUpper = RequireFullRank(a, rowCount, parameterCount, DesignParameter);
        var coefficients = new double[parameterCount];
        for (int i = 0; i < parameterCount; i++)
        {
            double total = 0.0;
            for (int k = i; k < parameterCount; k++)
            {
                total += inverseUpper[(i * parameterCount) + k] * projected[k];
            }

            coefficients[i] = total;
        }

        return (coefficients, inverseUpper);
    }

    /// <summary>The public parameter every fit reaching the reflections takes its design in, named by a refusal.</summary>
    internal const string DesignParameter = "design";

    /// <summary>Refuses a triangularized design whose columns do not span <paramref name="parameterCount"/> dimensions, and reports R's inverse.</summary>
    /// <remarks>
    /// <see cref="RankBracket"/> holds the test, which is <c>numpy.linalg.matrix_rank</c>'s. A per-column pivot
    /// could not see it: rounding scales with the largest columns, so a dependent column far smaller than the
    /// columns it depends on passed with a ratio of 1e-13 (#978). The inverse is formed here and handed back
    /// rather than recomputed by each caller, because the middle bracket reads it.
    /// </remarks>
    /// <param name="a">The triangularized block, column-major.</param>
    /// <param name="rowCount">How many rows it holds.</param>
    /// <param name="parameterCount">How many columns it holds.</param>
    /// <param name="parameterName">The public parameter the design arrived as, which the refusal names.</param>
    /// <returns>The inverse of R, row-major, which the standard errors and the leverages read.</returns>
    /// <exception cref="ArgumentException">The design is rank-deficient or collinear.</exception>
    internal static double[] RequireFullRank(
        double[] a, int rowCount, int parameterCount, string parameterName)
    {
        double tolerance = RankBracket.Tolerance(rowCount, parameterCount);
        double[] upper = Upper(a, rowCount, parameterCount);
        if (RankBracket.DiagonalProvesDeficient(upper, parameterCount, tolerance, out int weakest))
        {
            throw new ArgumentException(
                $"design is rank-deficient or collinear: coefficient {weakest}'s column, intercept "
                + "first when one is fitted, is the weakest pivot and lies within rounding of the "
                + "span of the others, so the fit has no unique solution. Drop the dependent "
                + "regressor.",
                parameterName);
        }

        double[] inverseUpper = InvertUpper(upper, parameterCount);
        if (!RankBracket.FrobeniusProvesFullRank(upper, inverseUpper, parameterCount, tolerance)
            && RankBracket.SpectrumProvesDeficient(upper, parameterCount, tolerance))
        {
            throw new ArgumentException(
                $"design is rank-deficient or collinear: its {parameterCount} columns, intercept "
                + "first when one is fitted, span fewer dimensions than that, so the fit has no "
                + "unique solution. Drop the dependent regressor.",
                parameterName);
        }

        return inverseUpper;
    }

    /// <summary>The largest condition number of the column-scaled design the normal equations are trusted with.</summary>
    /// <remarks>
    /// The normal equations lose about <c>κ²·ε</c>, and a column's scale does not count towards <c>κ</c> for a Cholesky
    /// factorization (van der Sluis). At 200, <c>200²·ε</c> is 9e-12, a hundred times inside the corpora's 1e-9; a design
    /// past it — the near-collinear fixtures, a polynomial on a narrow range — goes through the reflections instead
    /// (#782, #870). <see cref="WellConditioned"/> estimates <c>κ₂</c> itself when its two bounds disagree, which is
    /// what keeps the limit a statement about conditioning rather than about <c>p</c> (#985).
    /// </remarks>
    private const double NormalEquationsConditionLimit = 200.0;

    /// <summary>Least squares through <c>XᵀX = UᵀU</c>, when the design is conditioned well enough for it.</summary>
    /// <returns><see langword="false"/>, for the QR to answer, when <c>XᵀX</c> is not safely positive definite or the column-scaled design's condition number may pass <see cref="NormalEquationsConditionLimit"/>.</returns>
    /// <remarks>
    /// <c>U</c> stands in for R everywhere R is read — the standard errors, <c>X U⁻¹</c> for the leverages, the sandwich's
    /// bread — since each needs only <c>RᵀR = XᵀX</c>. It is <c>p(p+1)/2</c> products per row where the reflections are
    /// several passes per column: after the QR stopped forming Q, a weighted fit was still 1.4× Math.NET's normal
    /// equations at 2 000 rows (#782).
    /// </remarks>
    private static (double[] Coefficients, double[] InverseUpper)? TryNormalEquations(
        ReadOnlySpan<double> design,
        int rowCount,
        int featureCount,
        bool withIntercept,
        ReadOnlySpan<double> response,
        ReadOnlySpan<double> weights)
    {
        int parameterCount = featureCount + (withIntercept ? 1 : 0);
        (double[] gram, double[] moment) = NormalEquations(design, rowCount, featureCount, withIntercept, response, weights);
        if (!TryUpperCholesky(gram, parameterCount))
        {
            return null;
        }

        double[] inverseUpper = InvertUpper(gram, parameterCount);
        if (!ProvablyFullRank(gram, inverseUpper, rowCount, parameterCount)
            || !WellConditioned(gram, inverseUpper, parameterCount))
        {
            return null;
        }

        var coefficients = new double[parameterCount];
        for (int k = 0; k < parameterCount; k++)
        {
            // (U⁻ᵀ b)ₖ, then β = U⁻¹ (U⁻ᵀ b).
            double projected = 0.0;
            for (int j = 0; j <= k; j++)
            {
                projected += inverseUpper[(j * parameterCount) + k] * moment[j];
            }

            for (int i = 0; i <= k; i++)
            {
                coefficients[i] += inverseUpper[(i * parameterCount) + k] * projected;
            }
        }

        return (coefficients, inverseUpper);
    }

    /// <summary><c>XᵀWX</c>'s upper triangle and <c>XᵀWy</c>, one pass over the rows, the constant read as a column of ones.</summary>
    private static (double[] Gram, double[] Moment) NormalEquations(
        ReadOnlySpan<double> design,
        int rowCount,
        int featureCount,
        bool withIntercept,
        ReadOnlySpan<double> response,
        ReadOnlySpan<double> weights)
    {
        int first = withIntercept ? 1 : 0;
        int parameterCount = featureCount + first;
        var gram = new double[parameterCount * parameterCount];
        var moment = new double[parameterCount];
        var x = new double[parameterCount];
        x[0] = 1.0;
        for (int row = 0; row < rowCount; row++)
        {
            design.Slice(row * featureCount, featureCount).CopyTo(x.AsSpan(first));
            double y = response[row];
            double weight = weights.IsEmpty ? 1.0 : weights[row];
            for (int i = 0; i < parameterCount; i++)
            {
                double xi = weight * x[i];
                moment[i] += xi * y;
                for (int j = i; j < parameterCount; j++)
                {
                    gram[(i * parameterCount) + j] += xi * x[j];
                }
            }
        }

        return (gram, moment);
    }

    /// <summary>Scales a column-major block and its response by the square root of each row's weight, in place.</summary>
    private static void Scale(double[] a, int rowCount, int columnCount, double[] projected, ReadOnlySpan<double> weights)
    {
        for (int row = 0; row < rowCount; row++)
        {
            double root = Math.Sqrt(weights[row]);
            projected[row] *= root;
            for (int column = 0; column < columnCount; column++)
            {
                a[(column * rowCount) + row] *= root;
            }
        }
    }

    /// <summary>Whether <c>U</c>'s Frobenius bracket proves the design has full rank, so the normal equations may answer it.</summary>
    /// <remarks>
    /// <c>UᵀU = XᵀX = RᵀR</c>, so <c>U</c> carries the design's singular values as <c>R</c> does and the bracket
    /// <see cref="RequireFullRank"/> accepts on reads the same here. Both of this path's other gates are scale
    /// invariant and <see cref="RequireFullRank"/> is not, so without this a design refused through the reflections
    /// was answered through the normal equations — <c>x₂ = 1e-14·U(0,1)</c> beside a unit column returned a
    /// coefficient near 1e11 where <c>Estimate</c> threw. Anything the bracket cannot settle goes to the
    /// reflections, which decide it on the spectrum and raise the refusal, so the two entry points cannot diverge.
    /// </remarks>
    private static bool ProvablyFullRank(double[] upper, double[] inverseUpper, int rowCount, int parameterCount)
        => RankBracket.FrobeniusProvesFullRank(
            upper, inverseUpper, parameterCount, RankBracket.Tolerance(rowCount, parameterCount));

    /// <summary>Whether the column-scaled design's condition number stays within <see cref="NormalEquationsConditionLimit"/>.</summary>
    /// <remarks>
    /// With <c>D</c> the column norms, <c>S = U·D⁻¹</c> is the scaled design's factor. <c>√p · ‖D·U⁻¹‖_F</c> bounds its
    /// <c>κ₂</c> from above and the ratio of its diagonal from below, and <see cref="ConditionEstimate"/> settles what
    /// falls between them. The Frobenius bound gives away a factor of <c>p</c> — it is at least <c>p</c> for any design
    /// at all, 200.00 against a <c>κ₂</c> of 1.000 on an orthonormal one — so on its own it sent every design past 200
    /// parameters to the reflections however well conditioned (#985). It still answers first, so a fit of a handful of
    /// regressors reaches nothing new.
    /// </remarks>
    private static bool WellConditioned(double[] upper, double[] inverseUpper, int order)
    {
        var scales = new double[order];
        double total = 0.0;
        for (int i = 0; i < order; i++)
        {
            // Column i of U has the norm of the design's column i, since UᵀU = XᵀX.
            double squaredScale = 0.0;
            double squaredRow = 0.0;
            for (int k = 0; k < order; k++)
            {
                squaredScale += upper[(k * order) + i] * upper[(k * order) + i];
                squaredRow += inverseUpper[(i * order) + k] * inverseUpper[(i * order) + k];
            }

            total += squaredScale * squaredRow;
            scales[i] = Math.Sqrt(squaredScale);
        }

        if (order * total <= NormalEquationsConditionLimit * NormalEquationsConditionLimit)
        {
            return true;
        }

        var scaled = new double[order * order];
        var scaledInverse = new double[order * order];
        double largest = 0.0;
        double smallest = double.PositiveInfinity;
        for (int row = 0; row < order; row++)
        {
            for (int column = row; column < order; column++)
            {
                scaled[(row * order) + column] = upper[(row * order) + column] / scales[column];
                scaledInverse[(row * order) + column] = scales[row] * inverseUpper[(row * order) + column];
            }

            double pivot = Math.Abs(scaled[(row * order) + row]);
            largest = Math.Max(largest, pivot);
            smallest = Math.Min(smallest, pivot);
        }

        if (largest > NormalEquationsConditionLimit * smallest)
        {
            return false;
        }

        return ConditionEstimate.Of(scaled, scaledInverse, order) <= NormalEquationsConditionLimit;
    }

    /// <summary>Overwrites a symmetric <paramref name="matrix"/>'s upper triangle with its Cholesky factor <c>U</c>, <c>G = UᵀU</c>, zeroing the lower.</summary>
    /// <returns><see langword="false"/> when a pivot is not above 1e-12 of its diagonal entry, NaN included.</returns>
    public static bool TryUpperCholesky(double[] matrix, int order)
    {
        for (int i = 0; i < order; i++)
        {
            double diagonal = matrix[(i * order) + i];
            double pivot = diagonal;
            for (int k = 0; k < i; k++)
            {
                double above = matrix[(k * order) + i];
                pivot -= above * above;
            }

            if (!(pivot > 1e-12 * Math.Abs(diagonal)))
            {
                return false;
            }

            double root = Math.Sqrt(pivot);
            matrix[(i * order) + i] = root;
            for (int j = i + 1; j < order; j++)
            {
                double value = matrix[(i * order) + j];
                for (int k = 0; k < i; k++)
                {
                    value -= matrix[(k * order) + i] * matrix[(k * order) + j];
                }

                matrix[(i * order) + j] = value / root;
                matrix[(j * order) + i] = 0.0;
            }
        }

        return true;
    }

    /// <summary>A row-major block copied column-major, so each reflection walks one contiguous column.</summary>
    public static double[] ColumnMajor(double[] matrix, int rowCount, int columnCount)
    {
        var a = new double[rowCount * columnCount];
        for (int row = 0; row < rowCount; row++)
        {
            int source = row * columnCount;
            for (int column = 0; column < columnCount; column++)
            {
                a[(column * rowCount) + row] = matrix[source + column];
            }
        }

        return a;
    }

    /// <summary>Householder reflections reducing a column-major <paramref name="a"/> to R in its upper triangle, applied to <paramref name="projected"/> too when one is given.</summary>
    public static void Triangularize(double[] a, int rowCount, int columnCount, double[]? projected)
    {
        for (int k = 0; k < columnCount; k++)
        {
            int diagonal = (k * rowCount) + k;
            ReadOnlySpan<double> below = a.AsSpan(diagonal, rowCount - k);
            double norm = Math.Sqrt(Reflections.Dot(below, below));
            double alpha = a[diagonal] > 0.0 ? -norm : norm;

            // v = x - alpha·e1 in place, then H = I - 2vvᵀ/(vᵀv); vᵀv = 2·norm·(norm + |x₀|).
            a[diagonal] -= alpha;
            double scale = norm * (norm + Math.Abs(a[diagonal] + alpha));
            if (scale > 0.0)
            {
                for (int column = k + 1; column < columnCount; column++)
                {
                    Reflections.Reflect(below, scale, a.AsSpan((column * rowCount) + k, rowCount - k));
                }

                if (projected is not null)
                {
                    Reflections.Reflect(below, scale, projected.AsSpan(k, rowCount - k));
                }
            }

            a[diagonal] = alpha;
        }
    }

    /// <summary>R, row-major, read off the upper triangle <see cref="Triangularize"/> left.</summary>
    public static double[] Upper(double[] a, int rowCount, int columnCount)
    {
        var upper = new double[columnCount * columnCount];
        for (int row = 0; row < columnCount; row++)
        {
            for (int column = row; column < columnCount; column++)
            {
                upper[(row * columnCount) + column] = a[(column * rowCount) + row];
            }
        }

        return upper;
    }

    /// <summary>The inverse of an upper-triangular matrix, by back substitution.</summary>
    public static double[] InvertUpper(IReadOnlyList<double> upper, int order)
    {
        var inverse = new double[order * order];
        for (int column = order - 1; column >= 0; column--)
        {
            inverse[(column * order) + column] = 1.0 / upper[(column * order) + column];
            for (int row = column - 1; row >= 0; row--)
            {
                double total = 0.0;
                for (int k = row + 1; k <= column; k++)
                {
                    total += upper[(row * order) + k] * inverse[(k * order) + column];
                }

                inverse[(row * order) + column] = -total / upper[(row * order) + row];
            }
        }

        return inverse;
    }

    /// <summary>The diagonal of the covariance, scaled, square-rooted.</summary>
    public static double[] StandardErrors(
        double[] inverseUpper, int parameterCount, double dispersion)
    {
        var errors = new double[parameterCount];
        for (int i = 0; i < parameterCount; i++)
        {
            double total = 0.0;
            for (int k = i; k < parameterCount; k++)
            {
                double value = inverseUpper[(i * parameterCount) + k];
                total += value * value;
            }

            errors[i] = Math.Sqrt(total * dispersion);
        }

        return errors;
    }
}
