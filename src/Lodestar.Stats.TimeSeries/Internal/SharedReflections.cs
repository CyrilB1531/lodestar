namespace Lodestar.Stats.TimeSeries.Internal;

/// <summary>
/// One Householder QR of a design, serving several least-squares fits that share its rows: every response
/// on the whole design, or one response on each leading block of its columns.
/// </summary>
/// <remarks>
/// It repeats <c>OrdinaryLeastSquares.Estimate</c>'s arithmetic operation for operation, so each fit is
/// bit-identical to the estimate it replaces: reflection <c>k</c> reads only columns <c>≤ k</c>, and rows
/// <c>&lt; k</c> are final once it is applied. A residual sum is the exception, taken as the reflections pass.
/// Both take their kernels from the shared <c>Reflections</c> source, so the order is one text.
/// </remarks>
internal sealed class SharedReflections
{
    private readonly double[] _a;
    private readonly int _rows;
    private readonly int _columns;
    private readonly double[][] _projected;
    private int _reflected;

    /// <param name="design">The regressors, row-major, with no constant column.</param>
    /// <param name="featureCount">Columns in <paramref name="design"/>.</param>
    /// <param name="withIntercept">Whether a constant column of ones goes first, as the estimate prepends it.</param>
    /// <param name="responses">One response per fit on the whole design, or the single one the prefixes share; each is copied.</param>
    internal SharedReflections(ReadOnlySpan<double> design, int featureCount, bool withIntercept, double[][] responses)
    {
        _rows = design.Length / featureCount;
        _columns = featureCount + (withIntercept ? 1 : 0);
        _a = new double[_rows * _columns];
        for (int row = 0; row < _rows; row++)
        {
            int source = row * featureCount;
            int column = 0;
            if (withIntercept)
            {
                _a[row] = 1.0;
                column = 1;
            }

            for (int feature = 0; feature < featureCount; feature++)
            {
                _a[((column + feature) * _rows) + row] = design[source + feature];
            }
        }

        _projected = new double[responses.Length][];
        for (int i = 0; i < responses.Length; i++)
        {
            _projected[i] = (double[])responses[i].Clone();
        }
    }

    /// <summary>Applies the reflections of columns up to <paramref name="columnCount"/>, exclusive, to every column and response.</summary>
    internal void ReflectThrough(int columnCount)
    {
        for (int k = _reflected; k < columnCount; k++)
        {
            int diagonal = (k * _rows) + k;
            ReadOnlySpan<double> below = _a.AsSpan(diagonal, _rows - k);
            double norm = Math.Sqrt(Reflections.Dot(below, below));
            double alpha = _a[diagonal] > 0.0 ? -norm : norm;

            _a[diagonal] -= alpha;
            double scale = norm * (norm + Math.Abs(_a[diagonal] + alpha));
            if (scale > 0.0)
            {
                for (int column = k + 1; column < _columns; column++)
                {
                    Reflections.Reflect(below, scale, _a.AsSpan((column * _rows) + k, _rows - k));
                }

                foreach (double[] projected in _projected)
                {
                    Reflections.Reflect(below, scale, projected.AsSpan(k, _rows - k));
                }
            }

            _a[diagonal] = alpha;
        }

        _reflected = Math.Max(_reflected, columnCount);
    }

    /// <summary>The residual sum of squares of a fit on the first <paramref name="columnCount"/> columns.</summary>
    /// <remarks>Valid only while exactly that many reflections have been applied: a later one moves the rows it sums.</remarks>
    internal double ResidualSumOfSquares(int response, int columnCount)
    {
        double[] projected = _projected[response];
        double total = 0.0;
        for (int row = columnCount; row < _rows; row++)
        {
            total += projected[row] * projected[row];
        }

        return total;
    }

    /// <summary>The inverse of R's leading <paramref name="order"/> block, row-major, by back substitution.</summary>
    internal double[] InverseUpper(int order)
    {
        var inverse = new double[order * order];
        for (int column = order - 1; column >= 0; column--)
        {
            inverse[(column * order) + column] = 1.0 / _a[(column * _rows) + column];
            for (int row = column - 1; row >= 0; row--)
            {
                double total = 0.0;
                for (int k = row + 1; k <= column; k++)
                {
                    total += _a[(k * _rows) + row] * inverse[(k * order) + column];
                }

                inverse[(row * order) + column] = -total / _a[(row * _rows) + row];
            }
        }

        return inverse;
    }

    /// <summary>The row sums of squares of an inverse, one per coefficient, which every response's standard errors share.</summary>
    internal static double[] SquaredNorms(double[] inverse, int order)
    {
        var norms = new double[order];
        for (int i = 0; i < order; i++)
        {
            double squaredNorm = 0.0;
            for (int k = i; k < order; k++)
            {
                double entry = inverse[(i * order) + k];
                squaredNorm += entry * entry;
            }

            norms[i] = squaredNorm;
        }

        return norms;
    }

    /// <summary>A fit's coefficients and standard errors on the first <paramref name="order"/> columns.</summary>
    internal (double[] Coefficients, double[] StandardErrors) Estimates(
        int response, int order, double[] inverse, double[] squaredNorms, double residualSumOfSquares)
    {
        double[] projected = _projected[response];
        double residualVariance = residualSumOfSquares / (_rows - order);
        var coefficients = new double[order];
        var standardErrors = new double[order];
        for (int i = 0; i < order; i++)
        {
            double coefficient = 0.0;
            for (int k = i; k < order; k++)
            {
                coefficient += inverse[(i * order) + k] * projected[k];
            }

            coefficients[i] = coefficient;
            standardErrors[i] = Math.Sqrt(squaredNorms[i] * residualVariance);
        }

        return (coefficients, standardErrors);
    }
}
