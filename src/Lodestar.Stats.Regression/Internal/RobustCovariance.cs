using Lodestar.Decomposition;

namespace Lodestar.Stats.Regression.Internal;

/// <summary>The heteroskedasticity-consistent covariance, and the Wald test read on it.</summary>
/// <remarks>
/// A sandwich: <c>(XᵀX)⁻¹ Xᵀ Ω X (XᵀX)⁻¹</c>, where the bread is the same <c>R⁻¹</c> the
/// ordinary fit already has and the filling weights each row by its squared residual. The four
/// types differ only in that weight, which is why they share one routine rather than four
/// (#686).
/// </remarks>
internal static class RobustCovariance
{
    /// <summary>The leverage of each row, <c>hᵢᵢ</c>, read off the thin Q.</summary>
    /// <remarks>
    /// <c>H = QQᵀ</c> for a thin QR, so the diagonal this needs is a row of Q against itself —
    /// the whole hat matrix is never formed, which for 50 000 rows would be 20 GB.
    /// </remarks>
    public static double[] Leverages(IReadOnlyList<double> q, int rowCount, int parameterCount)
    {
        var leverages = new double[rowCount];
        for (int row = 0; row < rowCount; row++)
        {
            double total = 0.0;
            int at = row * parameterCount;
            for (int column = 0; column < parameterCount; column++)
            {
                double value = q[at + column];
                total += value * value;
            }

            leverages[row] = total;
        }

        return leverages;
    }

    /// <summary>The row weight each type puts in the filling.</summary>
    /// <remarks>
    /// A leverage of one is a row the fit passes through exactly, which leaves HC2 and HC3
    /// dividing by zero. statsmodels returns infinity there rather than raising, and so does
    /// this: the row is what is degenerate, and a summary naming it is more use than an
    /// exception naming the call.
    /// </remarks>
    private static double Weight(CovarianceType type, double residual, double leverage)
    {
        double squared = residual * residual;
        return type switch
        {
            CovarianceType.Hc2 => squared / (1.0 - leverage),
            CovarianceType.Hc3 => squared / ((1.0 - leverage) * (1.0 - leverage)),
            _ => squared,
        };
    }

    /// <summary>The full covariance of the estimates, row-major and symmetric.</summary>
    public static double[] Sandwich(
        double[] matrix,
        double[] inverseUpper,
        double[] residuals,
        double[] leverages,
        int rowCount,
        int parameterCount,
        CovarianceType type)
    {
        double[] bread = Bread(inverseUpper, parameterCount);

        var meat = new double[parameterCount * parameterCount];
        for (int row = 0; row < rowCount; row++)
        {
            double weight = Weight(type, residuals[row], leverages[row]);
            int at = row * parameterCount;
            for (int a = 0; a < parameterCount; a++)
            {
                double scaled = weight * matrix[at + a];
                for (int b = 0; b < parameterCount; b++)
                {
                    meat[(a * parameterCount) + b] += scaled * matrix[at + b];
                }
            }
        }

        double[] covariance = Multiply(Multiply(bread, meat, parameterCount), bread, parameterCount);
        if (type == CovarianceType.Hc1)
        {
            double correction = (double)rowCount / (rowCount - parameterCount);
            for (int i = 0; i < covariance.Length; i++)
            {
                covariance[i] *= correction;
            }
        }

        return covariance;
    }

    /// <summary><c>(XᵀX)⁻¹</c> as <c>R⁻¹R⁻ᵀ</c>, which the QR already paid for.</summary>
    private static double[] Bread(double[] inverseUpper, int order)
    {
        var bread = new double[order * order];
        for (int i = 0; i < order; i++)
        {
            for (int j = 0; j < order; j++)
            {
                double total = 0.0;
                for (int k = Math.Max(i, j); k < order; k++)
                {
                    total += inverseUpper[(i * order) + k] * inverseUpper[(j * order) + k];
                }

                bread[(i * order) + j] = total;
            }
        }

        return bread;
    }

    private static double[] Multiply(double[] left, double[] right, int order)
    {
        var product = new double[order * order];
        for (int i = 0; i < order; i++)
        {
            for (int k = 0; k < order; k++)
            {
                double value = left[(i * order) + k];
                if (value == 0.0)
                {
                    continue;
                }

                for (int j = 0; j < order; j++)
                {
                    product[(i * order) + j] += value * right[(k * order) + j];
                }
            }
        }

        return product;
    }

    /// <summary>The overall test as a Wald statistic on the covariance above, divided by its restrictions.</summary>
    /// <remarks>
    /// The ordinary F reads off R², which a robust covariance does not enter — so statsmodels
    /// tests the same hypothesis the other way, and this follows it. The p-value stays on the F
    /// distribution even though the coefficient tests move to the normal, which is asymmetric
    /// and is what statsmodels does (#686).
    /// </remarks>
    public static double Wald(
        double[] covariance, double[] coefficients, int parameterCount, bool withIntercept)
    {
        int offset = withIntercept ? 1 : 0;
        int restrictions = parameterCount - offset;
        if (restrictions < 1)
        {
            return double.NaN;
        }

        var block = new double[restrictions * restrictions];
        for (int i = 0; i < restrictions; i++)
        {
            for (int j = 0; j < restrictions; j++)
            {
                block[(i * restrictions) + j] =
                    covariance[((i + offset) * parameterCount) + j + offset];
            }
        }

        double[] inverse = Invert(block, restrictions);
        double total = 0.0;
        for (int i = 0; i < restrictions; i++)
        {
            double row = 0.0;
            for (int j = 0; j < restrictions; j++)
            {
                row += inverse[(i * restrictions) + j] * coefficients[j + offset];
            }

            total += coefficients[i + offset] * row;
        }

        return total / restrictions;
    }

    /// <summary>The inverse of a small square matrix, through the QR this package already has.</summary>
    /// <remarks>
    /// <c>A = QR</c> gives <c>A⁻¹ = R⁻¹Qᵀ</c>. A dedicated symmetric route would be faster and
    /// is not worth a second decomposition here: the block is one per fit and its order is the
    /// regressor count, single digits in every case this package was written for.
    /// </remarks>
    private static double[] Invert(double[] square, int order)
    {
        QrDecomposition qr = QrDecomposition.Householder(square, order, order);
        double[] inverseUpper = LeastSquares.InvertUpper(qr.R, order);
        IReadOnlyList<double> q = qr.Q;

        var inverse = new double[order * order];
        for (int i = 0; i < order; i++)
        {
            for (int j = 0; j < order; j++)
            {
                double total = 0.0;
                for (int k = i; k < order; k++)
                {
                    total += inverseUpper[(i * order) + k] * q[(j * order) + k];
                }

                inverse[(i * order) + j] = total;
            }
        }

        return inverse;
    }
}
