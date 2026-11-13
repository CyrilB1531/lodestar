namespace Lodestar.Stats.Regression.Internal;

/// <summary>Whether a covariance block can be inverted at all, and what a Wald test reads when it cannot.</summary>
/// <remarks>
/// A cluster covariance whose scores sum to zero has rank at most <c>G − 1</c>, and an inverse of a singular block is
/// noise: an F of −1e15 with a p-value of 1 (#1223). The rank is taken with <c>numpy.linalg.matrix_rank</c>'s tolerance
/// on the block scaled to a unit diagonal, because a regressor in the millions beside one in the thousandths is badly
/// scaled, not deficient.
/// </remarks>
internal static class CovarianceRank
{
    /// <summary>The block's rank and, where that is full, its inverse, both read off one factorisation.</summary>
    /// <remarks>
    /// With <c>S</c> the unit-diagonal scaling, <c>SCS = QR</c> gives <c>C⁻¹ = S·R⁻¹Qᵀ·S</c>: the rank test's
    /// reflections are applied to <c>S</c> as they go, so a weight the rank test accepts is not factored again.
    /// </remarks>
    /// <returns>The rank, and the inverse, null short of full rank or for a block that is not finite.</returns>
    public static (int Rank, double[]? Inverse) Inverse(double[] block, int order)
    {
        if (FiniteValues.FirstNonFinite(block) >= 0)
        {
            return (order, null);
        }

        var identity = new double[order * order];
        for (int i = 0; i < order; i++)
        {
            identity[(i * order) + i] = 1.0;
        }

        (int rank, double[]? inverseUpper, double[] scaling, double[]? projected) = Factored(block, order, identity, order);
        if (rank < order)
        {
            return (rank, null);
        }

        // projected holds QᵀS column-major; C⁻¹ = S·R⁻¹·(QᵀS), row-major.
        var inverse = new double[order * order];
        for (int i = 0; i < order; i++)
        {
            for (int j = 0; j < order; j++)
            {
                double total = 0.0;
                for (int m = i; m < order; m++)
                {
                    total += inverseUpper![(i * order) + m] * projected![(j * order) + m];
                }

                inverse[(i * order) + j] = scaling[i] * total;
            }
        }

        return (rank, inverse);
    }

    /// <summary><c>bᵀC⁻¹b</c>, or NaN when the block is not of full rank: the Wald statistic of a singular covariance is undefined.</summary>
    /// <param name="estimate">The tested coefficients.</param>
    /// <param name="block">Their covariance, row-major.</param>
    /// <param name="order">How many are tested.</param>
    /// <param name="largestRank">The most the estimator's construction allows, <c>G − 1</c> for a cluster covariance, which rounding must not raise.</param>
    public static double QuadraticForm(double[] estimate, double[] block, int order, int largestRank)
    {
        (int rank, double form, _) = Measure(block, order, estimate);
        return Math.Min(rank, largestRank) < order ? double.NaN : form;
    }

    /// <summary>The block's rank and, where that is full, <c>bᵀC⁻¹b</c> read off the same factorisation.</summary>
    /// <remarks>
    /// With <c>S</c> the unit-diagonal scaling, <c>SCS = QR</c> gives <c>bᵀC⁻¹b = (Sb)ᵀR⁻¹Qᵀ(Sb)</c>: the reflections the rank
    /// test runs are applied to <c>Sb</c> as they go, so the block is factored once (Review A pass 3 on #1223).
    /// </remarks>
    /// <param name="block">The covariance, row-major.</param>
    /// <param name="order">Its side.</param>
    /// <param name="estimate">The tested coefficients, or null when only the rank is wanted.</param>
    /// <returns>
    /// The rank, <paramref name="order"/> for a block that is not finite, which no rank describes; the form, NaN short of
    /// full rank, without an estimate, or for such a block; and whether the block was finite.
    /// </returns>
    public static (int Rank, double Form, bool Finite) Measure(double[] block, int order, double[]? estimate)
    {
        if (FiniteValues.FirstNonFinite(block) >= 0)
        {
            return (order, double.NaN, false);
        }

        (int rank, double[]? inverseUpper, double[] scaling, double[]? projected) = Factored(block, order, estimate, 1);
        if (rank < order || projected is null)
        {
            return (rank, double.NaN, true);
        }

        double form = 0.0;
        for (int i = 0; i < order; i++)
        {
            double row = 0.0;
            for (int j = i; j < order; j++)
            {
                row += inverseUpper![(i * order) + j] * projected[j];
            }

            form += scaling[i] * estimate![i] * row;
        }

        return (rank, form, true);
    }

    /// <summary>
    /// Triangularizes the block on its unit diagonal, <c>SCS = QR</c>, carrying the reflections into <c>S</c> times
    /// <paramref name="rightHandSide"/>, and reads its rank: the one factorisation both callers read.
    /// </summary>
    /// <returns>The rank, <c>R⁻¹</c> wherever that is full, <c>S</c>, and <c>QᵀS</c> times the right-hand side, column-major.</returns>
    private static (int Rank, double[]? InverseUpper, double[] Scaling, double[]? Projected) Factored(
        double[] block, int order, double[]? rightHandSide, int rightHandSideColumns)
    {
        double[] scaling = Scaling(block, order);
        double[] columns = Scaled(block, order, scaling);
        double[]? projected = null;
        if (rightHandSide is not null)
        {
            projected = new double[order * rightHandSideColumns];
            for (int i = 0; i < projected.Length; i++)
            {
                projected[i] = scaling[i % order] * rightHandSide[i];
            }
        }

        // The two brackets settle a block of full rank without a sweep; only one they cannot settle pays for the spectrum.
        double tolerance = RankBracket.Tolerance(order, order);
        LeastSquares.Triangularize(columns, order, order, projected, rightHandSideColumns);
        double[] upper = LeastSquares.Upper(columns, order, order);
        double[]? inverseUpper = RankBracket.DiagonalProvesDeficient(upper, order, tolerance, out _)
            ? null
            : LeastSquares.InvertUpper(upper, order);
        // R carries the scaled block's singular values, the reflections being orthogonal, so the spectrum is read on it.
        int rank = inverseUpper is not null && RankBracket.FrobeniusProvesFullRank(upper, inverseUpper, order, tolerance)
            ? order
            : SpectrumRank(LeastSquares.ColumnMajor(upper, order, order), order, order);
        return (rank, rank < order ? null : inverseUpper ?? LeastSquares.InvertUpper(upper, order), scaling, projected);
    }

    /// <summary>The unit-diagonal scaling <c>S</c>, one over the square root of each diagonal entry, one for a zero.</summary>
    internal static double[] Scaling(double[] block, int order)
    {
        var scaling = new double[order];
        for (int i = 0; i < order; i++)
        {
            double diagonal = Math.Abs(block[(i * order) + i]);
            scaling[i] = diagonal > 0.0 ? 1.0 / Math.Sqrt(diagonal) : 1.0;
        }

        return scaling;
    }

    /// <summary>The block scaled to a unit diagonal, <c>SCS</c>, column-major.</summary>
    internal static double[] Scaled(double[] block, int order, double[] scaling)
    {
        var columns = new double[order * order];
        for (int row = 0; row < order; row++)
        {
            for (int column = 0; column < order; column++)
            {
                columns[(column * order) + row] = scaling[row] * block[(row * order) + column] * scaling[column];
            }
        }

        return columns;
    }

    /// <summary><c>numpy.linalg.matrix_rank</c> of a tall column-major block: its singular values past its tolerance.</summary>
    internal static int SpectrumRank(double[] columnMajor, int rows, int columns)
    {
        double[] singular = JacobiSpectrum.SingularValues(columnMajor, rows, columns);
        double tolerance = RankBracket.Tolerance(rows, columns) * singular[0];
        int rank = 0;
        foreach (double value in singular)
        {
            rank += value > tolerance ? 1 : 0;
        }

        return rank;
    }
}
