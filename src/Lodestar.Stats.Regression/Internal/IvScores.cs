using Lodestar.Stats.Regression.Instrumental;

namespace Lodestar.Stats.Regression.Internal;

/// <summary>The long-run covariance of a block of scores, <c>S</c>, under each of <c>linearmodels</c>' estimators.</summary>
/// <remarks>
/// The same four estimators serve three callers: the covariance of 2SLS and LIML (scores <c>x̂ᵢ·eᵢ</c>), GMM's second-step
/// weight and GMM's covariance (scores <c>zᵢ·eᵢ</c>). Each returns <c>S</c> divided by <c>n</c>, unscaled; the callers
/// apply the small-sample factors, which differ between them.
/// </remarks>
internal static class IvScores
{
    /// <summary><c>ΣᵢuᵢuᵢᵀΣ/n</c>, White's.</summary>
    public static double[] Robust(double[] scores, int columns, int rows)
    {
        double[] s = Dense.Gram(scores, columns, rows);
        Scale(s, 1.0 / rows);
        return s;
    }

    /// <summary>The kernel estimator: <c>Γ₀ + Σⱼ wⱼ(Γⱼ + Γⱼᵀ)</c> over the rows in order, divided by <c>n</c>.</summary>
    /// <remarks>
    /// Each lag's <c>Γⱼ = Σₜ uₜuₜ₋ⱼᵀ</c> is summed whole before its weight applies, the reference's order: smoothing each
    /// row over its neighbours instead is cheaper, and moved an ill-conditioned GMM model test by <c>2e-9</c>.
    /// </remarks>
    public static double[] Kernel(double[] scores, int columns, int rows, double[] weights)
    {
        double[] s = Dense.Gram(scores, columns, rows);
        var lagged = new double[columns * columns];
        int maximumLag = Math.Min(weights.Length - 1, rows - 1);
        for (int lag = 1; lag <= maximumLag; lag++)
        {
            Array.Clear(lagged, 0, lagged.Length);
            for (int row = lag; row < rows; row++)
            {
                int at = row * columns;
                int before = (row - lag) * columns;
                for (int i = 0; i < columns; i++)
                {
                    double late = scores[at + i];
                    int to = i * columns;
                    for (int j = 0; j < columns; j++)
                    {
                        lagged[to + j] += late * scores[before + j];
                    }
                }
            }

            double weight = weights[lag];
            for (int i = 0; i < columns; i++)
            {
                for (int j = 0; j < columns; j++)
                {
                    s[(i * columns) + j] += weight * (lagged[(i * columns) + j] + lagged[(j * columns) + i]);
                }
            }
        }

        Scale(s, 1.0 / rows);
        return s;
    }

    /// <summary>The one-way cluster estimator: the outer products of each cluster's summed scores, divided by <c>n</c>.</summary>
    public static double[] Clustered(double[] scores, int columns, int rows, ClusterLabels clusters)
    {
        var sums = new double[clusters.Count * columns];
        for (int row = 0; row < rows; row++)
        {
            int at = clusters.Labels[row] * columns;
            for (int j = 0; j < columns; j++)
            {
                sums[at + j] += scores[(row * columns) + j];
            }
        }

        double[] s = Dense.Gram(sums, columns, clusters.Count);
        Scale(s, 1.0 / rows);
        return s;
    }

    /// <summary>The weights of lags <c>0..</c> for a kernel at a bandwidth, as the reference tabulates them.</summary>
    /// <param name="kernel">The kernel.</param>
    /// <param name="bandwidth">The bandwidth; Bartlett and Parzen truncate at it.</param>
    /// <param name="maximumLag">The largest lag the sample has, <c>n − 1</c>, which the quadratic spectral window reaches.</param>
    /// <exception cref="ArgumentException">A Bartlett or Parzen bandwidth past the largest lag, which <c>cov_kernel</c> refuses.</exception>
    public static double[] KernelWeights(KernelType kernel, int bandwidth, int maximumLag)
    {
        if (kernel == KernelType.QuadraticSpectral)
        {
            var weights = new double[maximumLag + 1];
            if (bandwidth == 0)
            {
                // The reference's own convention: a zero bandwidth zeroes every weight, the first included.
                return weights;
            }

            weights[0] = 1.0;
            for (int lag = 1; lag <= maximumLag; lag++)
            {
                double z = 6.0 * Math.PI * (lag / (double)bandwidth) / 5.0;
                weights[lag] = 3.0 / (z * z) * ((Math.Sin(z) / z) - Math.Cos(z));
            }

            return weights;
        }

        if (bandwidth > maximumLag)
        {
            throw new ArgumentException(
                $"A bandwidth of {bandwidth} weights {bandwidth + 1} lags of a sample of {maximumLag + 1}; the reference refuses it too.");
        }

        var truncated = new double[bandwidth + 1];
        for (int lag = 0; lag < truncated.Length; lag++)
        {
            double z = lag / (bandwidth + 1.0);
            if (kernel == KernelType.Bartlett)
            {
                truncated[lag] = 1.0 - z;
            }
            else
            {
                truncated[lag] = z <= 0.5 ? 1.0 - (6.0 * z * z) + (6.0 * z * z * z) : 2.0 * Math.Pow(1.0 - z, 3);
            }
        }

        return truncated;
    }

    /// <summary>Newey and West's (1994) automatic bandwidth for a series of summed scores.</summary>
    /// <remarks>
    /// The pilot lag <c>⌈4(n/100)^e⌉</c>, the rate <c>1/(2q+1)</c> and the constants are each kernel's, as the reference
    /// sets them; the result is capped at <c>n − 1</c>.
    /// </remarks>
    public static int OptimalBandwidth(ReadOnlySpan<double> series, KernelType kernel)
    {
        int n = series.Length;
        (int q, double c, double exponent) = kernel switch
        {
            KernelType.Bartlett => (1, 1.1447, 2.0 / 9.0),
            KernelType.QuadraticSpectral => (2, 1.3221, 2.0 / 25.0),
            _ => (2, 2.6614, 4.0 / 25.0),
        };

        int pilot = (int)Math.Ceiling(4.0 * Math.Pow(n / 100.0, exponent));
        double s0 = Autocovariance(series, 0);
        double sq = 0.0;
        for (int lag = 1; lag <= pilot; lag++)
        {
            double sigma = Autocovariance(series, lag);
            s0 += 2.0 * sigma;
            sq += 2.0 * sigma * Math.Pow(lag, q);
        }

        double rate = 1.0 / ((2.0 * q) + 1.0);
        double gamma = c * Math.Pow(Math.Pow(sq / s0, 2.0), rate);
        return (int)Math.Min(Math.Ceiling(gamma * Math.Pow(n, rate)), n - 1);
    }

    /// <summary>Whether a block holds a constant, or a set of columns spanning one, and where: <c>has_constant</c>.</summary>
    /// <remarks>
    /// A column of ones first, then any column that does not vary and is not zero; failing both, the rank test — the
    /// constant is in the span when appending one does not raise the rank, and a rank-deficient block counts too.
    /// The rank is numpy's: singular values above <c>σ_max·max(n, k)·ε</c>. A projected constant is not exactly one,
    /// which is why the automatic bandwidth reads this test rather than equality.
    /// </remarks>
    public static (bool Found, int Column) FindConstant(double[] block, int columns, int rows)
    {
        int flat = -1;
        for (int j = 0; j < columns; j++)
        {
            (bool allOne, bool constant, bool allZero) = Shape(block, columns, rows, j);
            if (allOne)
            {
                return (true, j);
            }

            if (flat < 0 && constant && !allZero)
            {
                flat = j;
            }
        }

        if (flat >= 0)
        {
            return (true, flat);
        }

        int rank = Rank(block, columns, rows, withOnes: false);
        int augmented = Rank(block, columns, rows, withOnes: true);
        bool found = (augmented == rank && rows > columns) || rank < Math.Min(rows, columns);
        return found ? (true, LeastVaryingColumn(block, columns, rows)) : (false, -1);
    }

    /// <summary>The first column that does not vary, <c>find_constant</c>: what the model test leaves out.</summary>
    public static int FirstFlatColumn(double[] block, int columns, int rows)
    {
        for (int j = 0; j < columns; j++)
        {
            if (Shape(block, columns, rows, j).Constant)
            {
                return j;
            }
        }

        return -1;
    }

    private static (bool AllOne, bool Constant, bool AllZero) Shape(double[] block, int columns, int rows, int column)
    {
        double first = block[column];
        bool allOne = true;
        bool constant = true;
        bool allZero = true;
        for (int row = 0; row < rows; row++)
        {
            double value = block[(row * columns) + column];
            allOne &= Same(value, 1.0);
            constant &= Same(value, first);
            allZero &= Same(value, 0.0);
        }

        return (allOne, constant, allZero);
    }

    /// <summary>Exact equality, which numpy's <c>==</c> and <c>ptp == 0</c> test: a constant column is bit-for-bit constant.</summary>
    private static bool Same(double a, double b) => !(a < b) && !(a > b);

    private static int Rank(double[] block, int columns, int rows, bool withOnes)
    {
        int width = columns + (withOnes ? 1 : 0);
        if (rows < width)
        {
            // numpy ranks a wide block through its transpose; only the count of non-negligible values matters.
            return Math.Min(rows, RankOfTall(Dense.Transpose(Augment(block, columns, rows, withOnes), rows, width), rows, width));
        }

        return RankOfTall(Augment(block, columns, rows, withOnes), width, rows);
    }

    private static double[] Augment(double[] block, int columns, int rows, bool withOnes)
    {
        if (!withOnes)
        {
            return block;
        }

        var augmented = new double[rows * (columns + 1)];
        for (int row = 0; row < rows; row++)
        {
            augmented[row * (columns + 1)] = 1.0;
            Array.Copy(block, row * columns, augmented, (row * (columns + 1)) + 1, columns);
        }

        return augmented;
    }

    private static int RankOfTall(double[] rowMajor, int columns, int rows)
    {
        var columnMajor = new double[rows * columns];
        for (int row = 0; row < rows; row++)
        {
            for (int j = 0; j < columns; j++)
            {
                columnMajor[(j * rows) + row] = rowMajor[(row * columns) + j];
            }
        }

        double[] singular = JacobiSpectrum.SingularValues(columnMajor, rows, columns);
        double tolerance = singular[0] * Math.Max(rows, columns) * 2.220446049250313e-16;
        int rank = 0;
        foreach (double value in singular)
        {
            rank += value > tolerance ? 1 : 0;
        }

        return rank;
    }

    private static int LeastVaryingColumn(double[] block, int columns, int rows)
    {
        int best = 0;
        double bestValue = double.PositiveInfinity;
        for (int j = 0; j < columns; j++)
        {
            double sum = 0.0;
            double largest = 0.0;
            for (int row = 0; row < rows; row++)
            {
                double value = block[(row * columns) + j];
                sum += value;
                largest = Math.Max(largest, Math.Abs(value));
            }

            double mean = sum / rows;
            double spread = 0.0;
            for (int row = 0; row < rows; row++)
            {
                double deviation = block[(row * columns) + j] - mean;
                spread += deviation * deviation;
            }

            double normed = spread / rows / largest;
            if (normed < bestValue)
            {
                bestValue = normed;
                best = j;
            }
        }

        return best;
    }

    private static double Autocovariance(ReadOnlySpan<double> series, int lag)
    {
        double sum = 0.0;
        for (int t = lag; t < series.Length; t++)
        {
            sum += series[t] * series[t - lag];
        }

        return sum / series.Length;
    }

    private static void Scale(double[] matrix, double factor)
    {
        for (int i = 0; i < matrix.Length; i++)
        {
            matrix[i] *= factor;
        }
    }
}
