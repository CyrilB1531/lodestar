namespace Lodestar.Stats.Regression.Internal;

/// <summary>The small row-major matrix arithmetic the instrumental-variables estimators are written in.</summary>
/// <remarks>
/// Every matrix here but the data is <c>k × k</c> or <c>L × k</c> with <c>k</c> and <c>L</c> the regressor and
/// instrument counts, so plain loops cost nothing next to the <c>n</c>-row cross-products, and a pivoted inverse is
/// what the reference's <c>numpy.linalg.inv</c> computes.
/// </remarks>
internal static class Dense
{
    /// <summary><c>AᵀB</c> of two row-major blocks sharing their row count.</summary>
    public static double[] CrossProduct(double[] left, int leftColumns, double[] right, int rightColumns, int rows)
    {
        var product = new double[leftColumns * rightColumns];
        for (int row = 0; row < rows; row++)
        {
            int leftAt = row * leftColumns;
            int rightAt = row * rightColumns;
            for (int i = 0; i < leftColumns; i++)
            {
                double value = left[leftAt + i];
                int at = i * rightColumns;
                for (int j = 0; j < rightColumns; j++)
                {
                    product[at + j] += value * right[rightAt + j];
                }
            }
        }

        return product;
    }

    /// <summary><c>AᵀA</c> of a row-major block, symmetric by construction.</summary>
    public static double[] Gram(double[] a, int columns, int rows)
    {
        var gram = new double[columns * columns];
        for (int row = 0; row < rows; row++)
        {
            int at = row * columns;
            for (int i = 0; i < columns; i++)
            {
                double left = a[at + i];
                for (int j = i; j < columns; j++)
                {
                    gram[(i * columns) + j] += left * a[at + j];
                }
            }
        }

        for (int i = 0; i < columns; i++)
        {
            for (int j = 0; j < i; j++)
            {
                gram[(i * columns) + j] = gram[(j * columns) + i];
            }
        }

        return gram;
    }

    /// <summary><c>AB</c> for row-major <c>A</c> (<paramref name="rows"/> × <paramref name="inner"/>) and <c>B</c> (<paramref name="inner"/> × <paramref name="columns"/>).</summary>
    public static double[] Multiply(double[] a, double[] b, int rows, int inner, int columns)
    {
        var product = new double[rows * columns];
        for (int i = 0; i < rows; i++)
        {
            for (int m = 0; m < inner; m++)
            {
                double left = a[(i * inner) + m];

                for (int j = 0; j < columns; j++)
                {
                    product[(i * columns) + j] += left * b[(m * columns) + j];
                }
            }
        }

        return product;
    }

    /// <summary><c>Aᵀ</c> of a row-major <paramref name="rows"/> × <paramref name="columns"/> block.</summary>
    public static double[] Transpose(double[] a, int rows, int columns)
    {
        var transposed = new double[a.Length];
        for (int i = 0; i < rows; i++)
        {
            for (int j = 0; j < columns; j++)
            {
                transposed[(j * rows) + i] = a[(i * columns) + j];
            }
        }

        return transposed;
    }

    /// <summary>The inverse of a square matrix with a positive diagonal, by Gauss-Jordan elimination with partial pivoting.</summary>
    /// <param name="matrix">The matrix, row-major; not modified.</param>
    /// <param name="order">Its order.</param>
    /// <param name="inverse">The inverse, when the matrix is not numerically singular.</param>
    /// <returns><see langword="false"/> when a diagonal entry is not positive or a pivot falls to zero, which a collinear design produces.</returns>
    /// <remarks>
    /// Every matrix inverted here is a cross-product, a bread or a covariance, so it is equilibrated first, <c>D A D</c>
    /// with <c>D = diag(|aᵢᵢ|^−½)</c>: a regressor in the tens of thousands beside one in the thousandths is badly
    /// scaled, not collinear, and the pivot test would otherwise read one as the other.
    /// </remarks>
    public static bool TryInvert(double[] matrix, int order, out double[] inverse)
    {
        var scaling = new double[order];
        for (int i = 0; i < order; i++)
        {
            double diagonal = Math.Abs(matrix[(i * order) + i]);
            if (!(diagonal > 0.0) || double.IsInfinity(diagonal))
            {
                inverse = [];
                return false;
            }

            scaling[i] = 1.0 / Math.Sqrt(diagonal);
        }

        var work = new double[order * order];
        for (int i = 0; i < order; i++)
        {
            for (int j = 0; j < order; j++)
            {
                work[(i * order) + j] = scaling[i] * matrix[(i * order) + j] * scaling[j];
            }
        }

        inverse = new double[order * order];
        for (int i = 0; i < order; i++)
        {
            inverse[(i * order) + i] = 1.0;
        }

        if (!Eliminate(work, inverse, order))
        {
            return false;
        }

        for (int i = 0; i < order; i++)
        {
            for (int j = 0; j < order; j++)
            {
                inverse[(i * order) + j] *= scaling[i] * scaling[j];
            }
        }

        return true;
    }

    /// <summary>Reduces an equilibrated matrix to the identity, applying each step to the inverse it builds.</summary>
    private static bool Eliminate(double[] work, double[] inverse, int order)
    {
        for (int column = 0; column < order; column++)
        {
            int pivot = column;
            for (int row = column + 1; row < order; row++)
            {
                if (Math.Abs(work[(row * order) + column]) > Math.Abs(work[(pivot * order) + column]))
                {
                    pivot = row;
                }
            }

            double head = work[(pivot * order) + column];
            // Equilibrated, a unit diagonal: a pivot this far below it is a dependent column.
            if (!(Math.Abs(head) > 1e-14))
            {
                return false;
            }

            SwapRows(work, order, pivot, column);
            SwapRows(inverse, order, pivot, column);
            EliminateColumn(work, inverse, order, column);
        }

        return true;
    }

    /// <summary><c>(M + Mᵀ)/2</c> in place, the symmetrisation the reference applies to every covariance.</summary>
    public static void Symmetrise(double[] matrix, int order)
    {
        for (int i = 0; i < order; i++)
        {
            for (int j = 0; j < i; j++)
            {
                double mean = (matrix[(i * order) + j] + matrix[(j * order) + i]) / 2.0;
                matrix[(i * order) + j] = mean;
                matrix[(j * order) + i] = mean;
            }
        }
    }

    private static void EliminateColumn(double[] work, double[] inverse, int order, int column)
    {
        double head = work[(column * order) + column];
        for (int j = 0; j < order; j++)
        {
            work[(column * order) + j] /= head;
            inverse[(column * order) + j] /= head;
        }

        for (int row = 0; row < order; row++)
        {
            double factor = work[(row * order) + column];
            if (row == column)
            {
                continue;
            }

            for (int j = 0; j < order; j++)
            {
                work[(row * order) + j] -= factor * work[(column * order) + j];
                inverse[(row * order) + j] -= factor * inverse[(column * order) + j];
            }
        }
    }

    private static void SwapRows(double[] matrix, int order, int first, int second)
    {
        if (first == second)
        {
            return;
        }

        for (int j = 0; j < order; j++)
        {
            (matrix[(first * order) + j], matrix[(second * order) + j]) =
                (matrix[(second * order) + j], matrix[(first * order) + j]);
        }
    }
}
