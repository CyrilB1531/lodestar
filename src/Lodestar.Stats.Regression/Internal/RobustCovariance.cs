namespace Lodestar.Stats.Regression.Internal;

/// <summary>The robust covariances, and the Wald test read on them.</summary>
/// <remarks>
/// A sandwich: <c>(XᵀX)⁻¹ S (XᵀX)⁻¹</c>, where the bread is the same <c>R⁻¹</c> the ordinary fit
/// already has. HC0 to HC3 differ only in the weight each row's own score takes (#686); HAC adds
/// the scores of nearby rows and cluster sums them by group (#775), and the bread is shared.
/// </remarks>
internal static class RobustCovariance
{
    /// <summary>The leverage of each row, <c>hᵢᵢ</c>, as a row of <c>X R⁻¹</c> against itself.</summary>
    /// <remarks>
    /// For a full-rank design <c>Q = X R⁻¹</c>, so <c>H = QQᵀ</c> has this diagonal without Q ever being formed —
    /// nor the whole hat matrix, which for 50 000 rows would be 20 GB. The solve no longer builds Q (#782), and
    /// <c>p²</c> products per row is the rest of the price.
    /// </remarks>
    public static double[] Leverages(double[] matrix, double[] inverseUpper, int rowCount, int parameterCount)
    {
        var leverages = new double[rowCount];
        var projected = new double[parameterCount];
        for (int row = 0; row < rowCount; row++)
        {
            int at = row * parameterCount;
            for (int column = 0; column < parameterCount; column++)
            {
                double value = 0.0;
                for (int k = 0; k <= column; k++)
                {
                    value += matrix[at + k] * inverseUpper[(k * parameterCount) + column];
                }

                projected[column] = value;
            }

            double total = 0.0;
            for (int column = 0; column < parameterCount; column++)
            {
                total += projected[column] * projected[column];
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
        return type is CovarianceType.Hc2 or CovarianceType.Hc3 ? squared / LeverageDivisor(type, leverage) : squared;
    }

    /// <summary>What HC2 and HC3 divide a squared residual by: <c>1 − h</c> and <c>(1 − h)²</c>; one for the others.</summary>
    private static double LeverageDivisor(CovarianceType type, double leverage) => type switch
    {
        CovarianceType.Hc2 => 1.0 - leverage,
        CovarianceType.Hc3 => (1.0 - leverage) * (1.0 - leverage),
        _ => 1.0,
    };

    /// <summary>The filling of HC0 to HC3, <c>Σ ωᵢ xᵢxᵢᵀ</c>; the leverages are read only by the two types that weight by them.</summary>
    public static double[] HeteroskedasticMeat(
        double[] matrix, double[] inverseUpper, double[] residuals, int rowCount, int parameterCount, CovarianceType type)
    {
        // HC0 and HC1 never read a leverage: they skip computing them and pass an array of the right length in their
        // place, so the one loop below serves all four types without a test per row.
        double[] leverages = type is CovarianceType.Hc2 or CovarianceType.Hc3
            ? Leverages(matrix, inverseUpper, rowCount, parameterCount)
            : residuals;

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

        return meat;
    }

    /// <summary>Newey–West's filling: <c>Γ₀ + Σₗ (1 − l/(L+1)) (Γₗ + Γₗᵀ)</c>, with <c>Γₗ = Σᵢ uᵢ uᵢ₋ₗᵀ</c> on the scores <c>uᵢ = xᵢrᵢ</c>.</summary>
    /// <remarks>
    /// <c>S_hac_simple</c>'s sum in <c>statsmodels</c>. Lags past the last row contribute no pair and are skipped, but the
    /// weights still divide by <c>L + 1</c>, as the reference's do.
    /// </remarks>
    public static double[] HacMeat(double[] matrix, double[] residuals, int rowCount, int parameterCount, int lags)
    {
        double[] scores = Scores(matrix, residuals, rowCount, parameterCount);
        var meat = new double[parameterCount * parameterCount];
        var lagged = new double[parameterCount * parameterCount];
        int reach = Math.Min(lags, rowCount - 1);
        for (int lag = 0; lag <= reach; lag++)
        {
            LaggedCrossProduct(scores, rowCount, parameterCount, lag, lagged);

            // Γ₀ is symmetric already and enters once; every later lag enters with its transpose.
            double weight = lag == 0 ? 1.0 : 1.0 - (lag / (lags + 1.0));
            for (int a = 0; a < parameterCount; a++)
            {
                for (int b = 0; b < parameterCount; b++)
                {
                    double pair = lag == 0
                        ? lagged[(a * parameterCount) + b]
                        : lagged[(a * parameterCount) + b] + lagged[(b * parameterCount) + a];
                    meat[(a * parameterCount) + b] += weight * pair;
                }
            }
        }

        return meat;
    }

    /// <summary><c>Γₗ = Σᵢ uᵢ uᵢ₋ₗᵀ</c>, written over <paramref name="lagged"/>.</summary>
    private static void LaggedCrossProduct(double[] scores, int rowCount, int parameterCount, int lag, double[] lagged)
    {
        Array.Clear(lagged, 0, lagged.Length);
        for (int row = lag; row < rowCount; row++)
        {
            int at = row * parameterCount;
            int before = (row - lag) * parameterCount;
            for (int a = 0; a < parameterCount; a++)
            {
                double value = scores[at + a];
                for (int b = 0; b < parameterCount; b++)
                {
                    lagged[(a * parameterCount) + b] += value * scores[before + b];
                }
            }
        }
    }

    /// <summary>The one-way cluster filling, <c>Σ_g s_g s_gᵀ</c> with <c>s_g</c> the sum of the scores in cluster <c>g</c>, and how many <c>s_g</c> are not zero.</summary>
    /// <param name="matrix">The design the solve ran on, row-major.</param>
    /// <param name="residuals">Its residuals.</param>
    /// <param name="clusters">One dense label per row, from <c>0</c> to <paramref name="clusterCount"/> − 1.</param>
    /// <param name="parameterCount">Columns of <paramref name="matrix"/>.</param>
    /// <param name="clusterCount">How many clusters the labels name.</param>
    public static (double[] Meat, int Spanning, double[] Sums) ClusterMeat(
        double[] matrix, double[] residuals, int[] clusters, int parameterCount, int clusterCount)
    {
        var sums = new double[clusterCount * parameterCount];
        for (int row = 0; row < clusters.Length; row++)
        {
            int at = row * parameterCount;
            int into = clusters[row] * parameterCount;
            for (int a = 0; a < parameterCount; a++)
            {
                sums[into + a] += matrix[at + a] * residuals[row];
            }
        }

        var meat = new double[parameterCount * parameterCount];
        for (int cluster = 0; cluster < clusterCount; cluster++)
        {
            int at = cluster * parameterCount;
            for (int a = 0; a < parameterCount; a++)
            {
                double value = sums[at + a];
                for (int b = 0; b < parameterCount; b++)
                {
                    meat[(a * parameterCount) + b] += value * sums[at + b];
                }
            }
        }

        return (meat, IvScores.Spanning(sums, parameterCount, clusterCount), sums);
    }

    /// <summary>Each cluster's <c>Σ |xᵢ|·mᵢ</c>: with the residuals' sizes, the sizes of its summed scores' terms.</summary>
    public static double[] AbsoluteClusterSums(double[] matrix, double[] multipliers, int[] clusters, int parameterCount, int clusterCount)
    {
        var sums = new double[clusterCount * parameterCount];
        for (int row = 0; row < clusters.Length; row++)
        {
            int at = row * parameterCount;
            int into = clusters[row] * parameterCount;
            for (int a = 0; a < parameterCount; a++)
            {
                sums[into + a] += Math.Abs(matrix[at + a]) * multipliers[row];
            }
        }

        return sums;
    }

    /// <summary>Each residual's size with the terms it was computed from, <c>|eᵢ| + Σ |xᵢⱼbⱼ|</c>, which its rounding scales with.</summary>
    public static double[] ResidualMagnitudes(double[] matrix, double[] residuals, double[] coefficients, int rowCount)
    {
        int parameterCount = coefficients.Length;
        var magnitudes = new double[rowCount];
        for (int row = 0; row < rowCount; row++)
        {
            double total = Math.Abs(residuals[row]);
            for (int a = 0; a < parameterCount; a++)
            {
                total += Math.Abs(matrix[(row * parameterCount) + a] * coefficients[a]);
            }

            magnitudes[row] = total;
        }

        return magnitudes;
    }

    /// <summary>The full covariance of the estimates, <c>(XᵀX)⁻¹ S (XᵀX)⁻¹</c>, times a small-sample factor when one applies.</summary>
    /// <param name="meat">The filling, row-major.</param>
    /// <param name="inverseUpper">R⁻¹, row-major.</param>
    /// <param name="parameterCount">The order of both.</param>
    /// <param name="correction">The factor, or <see langword="null"/> for a type that has none.</param>
    /// <param name="largestRank">The most rank the filling can hold, or null where its construction sets none.</param>
    /// <param name="scores">The score rows whose cross product is <paramref name="meat"/>, built only by a test short of full rank.</param>
    /// <remarks>
    /// <see langword="null"/> rather than a factor of one: multiplying every entry by a one that is not a compile-time
    /// constant cost 85 to 93 ns a fit at 100 rows, measured — 3 % of HC2 and HC3 there (#775).
    /// </remarks>
    public static SandwichParts Sandwich(
        double[] meat, double[] inverseUpper, int parameterCount, double? correction, int? largestRank, Func<ScoreRows> scores)
    {
        double[] bread = Bread(inverseUpper, parameterCount);
        double[] covariance = Multiply(Multiply(bread, meat, parameterCount), bread, parameterCount);
        if (correction is { } factor)
        {
            for (int i = 0; i < covariance.Length; i++)
            {
                covariance[i] *= factor;
            }
        }

        return new SandwichParts(covariance, bread, correction ?? 1.0, scores) { LargestRank = largestRank };
    }

    /// <summary>The rows <c>√ωᵢ·xᵢ</c> whose cross product is the HC0 to HC3 filling, or their sizes from <paramref name="magnitudes"/>.</summary>
    public static ScoreRows HeteroskedasticScores(
        double[] matrix, double[] inverseUpper, double[] residuals, double[] magnitudes, int rowCount, int parameterCount, CovarianceType type)
    {
        double[] leverages = type is CovarianceType.Hc2 or CovarianceType.Hc3
            ? Leverages(matrix, inverseUpper, rowCount, parameterCount)
            : residuals;
        var rows = new double[rowCount * parameterCount];
        var sizes = new double[rowCount * parameterCount];
        for (int row = 0; row < rowCount; row++)
        {
            double scale = 1.0 / Math.Sqrt(LeverageDivisor(type, leverages[row]));
            int at = row * parameterCount;
            for (int a = 0; a < parameterCount; a++)
            {
                rows[at + a] = scale * residuals[row] * matrix[at + a];
                sizes[at + a] = Math.Abs(scale * magnitudes[row] * matrix[at + a]);
            }
        }

        return new ScoreRows(rows, sizes, rowCount);
    }

    /// <summary>The rows whose cross product is Newey–West's filling: each window of <c>L + 1</c> scores summed, over <c>√(L + 1)</c>.</summary>
    /// <remarks>
    /// Scores <c>l</c> apart share <c>L + 1 − l</c> windows, so <c>Σⱼ zⱼzⱼᵀ / (L + 1)</c> weighs <c>Γₗ</c> by Bartlett's
    /// <c>1 − l/(L + 1)</c>. The windows that hold every score, past the last row, are one row of that multiplicity.
    /// </remarks>
    public static (double[] Rows, int Count) HacScores(
        double[] matrix, double[] multipliers, int rowCount, int parameterCount, int lags, bool absolute)
    {
        double[] scores = Scores(absolute ? [.. matrix.Select(Math.Abs)] : matrix, multipliers, rowCount, parameterCount);
        long full = Math.Max(0L, lags - rowCount + 2L);
        long windows = Math.Min(rowCount + (long)lags, (2L * rowCount) - 1L);
        var rows = new double[TableLength.Of(windows, parameterCount, "design")];
        int count = (int)windows;
        double scale = 1.0 / Math.Sqrt(lags + 1.0);
        int written = 0;
        long end = 0;
        while (end < rowCount + (long)lags)
        {
            // Summed afresh, not slid: a running sum leaves its rounding where a window is exactly zero (pass 12).
            int first = (int)Math.Max(0L, end - lags);
            int last = (int)Math.Min(end, rowCount - 1L);
            bool whole = full > 0 && end == rowCount - 1;
            double weight = whole ? scale * Math.Sqrt(full) : scale;
            int at = written * parameterCount;
            for (int row = first; row <= last; row++)
            {
                for (int a = 0; a < parameterCount; a++)
                {
                    rows[at + a] += scores[(row * parameterCount) + a];
                }
            }

            for (int a = 0; a < parameterCount; a++)
            {
                rows[at + a] *= weight;
            }

            written++;
            end = whole ? lags + 1L : end + 1L;
        }

        return (rows, count);
    }

    /// <summary>Each row of the design times its multiplier: with the residuals, the scores the HAC filling sums.</summary>
    private static double[] Scores(double[] matrix, double[] multipliers, int rowCount, int parameterCount)
    {
        var scores = new double[rowCount * parameterCount];
        for (int row = 0; row < rowCount; row++)
        {
            int at = row * parameterCount;
            for (int a = 0; a < parameterCount; a++)
            {
                scores[at + a] = matrix[at + a] * multipliers[row];
            }
        }

        return scores;
    }

    /// <summary><c>(XᵀX)⁻¹</c> as <c>R⁻¹R⁻ᵀ</c>, which the solve already paid for; <c>U</c> from the normal equations serves as R.</summary>
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

    /// <summary>The overall test as a Wald statistic on the covariance above, with the restrictions it was divided by.</summary>
    /// <remarks>
    /// The ordinary F reads off R², which a robust covariance does not enter — so statsmodels
    /// tests the same hypothesis the other way, and this follows it. The p-value stays on the F
    /// distribution even though the coefficient tests move to the normal, which is asymmetric
    /// and is what statsmodels does (#686). A block short of full rank takes the pseudo-inverse's
    /// form over its rank and counts only that rank as restrictions (#1223).
    /// </remarks>
    /// <param name="sandwich">The coefficients' covariance and the parts it was built from.</param>
    /// <param name="coefficients">The estimates, intercept first when one was fitted.</param>
    /// <param name="withIntercept">Whether the first coefficient is left out of the test.</param>
    public static (double Statistic, int Rank) Wald(SandwichParts sandwich, double[] coefficients, bool withIntercept)
    {
        int parameterCount = coefficients.Length;
        // The cluster sums that are not zero, less one, bound a cluster covariance; no other type is bounded below k.
        int largestRank = sandwich.LargestRank ?? parameterCount;
        int offset = withIntercept ? 1 : 0;
        int restrictions = parameterCount - offset;
        var block = new double[restrictions * restrictions];
        var tested = new double[restrictions];
        Array.Copy(coefficients, offset, tested, 0, restrictions);
        for (int i = 0; i < restrictions; i++)
        {
            for (int j = 0; j < restrictions; j++)
            {
                block[(i * restrictions) + j] = sandwich.Covariance[((i + offset) * parameterCount) + j + offset];
            }
        }

        (int measured, double form, bool finite) = CovarianceRank.Measure(block, restrictions, tested);
        // A block that is not finite has no rank to cap, and keeps every restriction unranked.
        int rank = finite ? Math.Min(measured, largestRank) : restrictions;
        if (rank == restrictions)
        {
            return (form / restrictions, restrictions);
        }

        // Zero restrictions divide nothing: an exact fit's covariance is zero, and statsmodels' 0/0 is NaN too.
        (double reduced, int kept) = rank < 1
            ? (double.NaN, 0)
            : ReducedRankWald.Form(sandwich, tested, offset, rank);
        return (reduced / kept, kept);
    }
}
