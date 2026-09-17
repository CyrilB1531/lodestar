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

    /// <summary>Least squares through Householder reflections, reporting the inverse of R the covariance needs.</summary>
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

        double[] inverseUpper = InvertUpper(Upper(a, rowCount, parameterCount), parameterCount);
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

    /// <summary>The largest ratio between the Cholesky factor's diagonal entries the normal equations are trusted with.</summary>
    /// <remarks>
    /// The normal equations square the design's condition number, and the ratio of <c>U</c>'s diagonal is a lower bound
    /// on that number. At 200, <c>200²·ε</c> is 9e-12, a hundred times inside the corpora's 1e-9; a design past it —
    /// the near-collinear fixtures, a column on a scale of its own — goes through the Householder QR instead (#782).
    /// </remarks>
    private const double NormalEquationsRatio = 200.0;

    /// <summary>Least squares through <c>XᵀX = UᵀU</c>, when the design is conditioned well enough for it.</summary>
    /// <returns><see langword="false"/>, for the QR to answer, when <c>XᵀX</c> is not safely positive definite or its factor's diagonal spread passes <see cref="NormalEquationsRatio"/>.</returns>
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
        if (!TryUpperCholesky(gram, parameterCount) || !WellConditioned(gram, parameterCount))
        {
            return null;
        }

        double[] inverseUpper = InvertUpper(gram, parameterCount);
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

    /// <summary>Whether the factor's diagonal entries stay within <see cref="NormalEquationsRatio"/> of one another.</summary>
    private static bool WellConditioned(double[] upper, int order)
    {
        double smallest = double.PositiveInfinity;
        double largest = 0.0;
        for (int i = 0; i < order; i++)
        {
            double entry = upper[(i * order) + i];
            smallest = Math.Min(smallest, entry);
            largest = Math.Max(largest, entry);
        }

        return largest <= NormalEquationsRatio * smallest;
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
