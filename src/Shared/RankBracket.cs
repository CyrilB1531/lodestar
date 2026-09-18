namespace Lodestar.Internal;

/// <summary>The rank of a triangularized design, bracketed so its spectrum is read only when it must be.</summary>
/// <remarks>
/// <c>numpy.linalg.matrix_rank</c>'s test is <c>σmin ≤ σmax·max(n, p)·ε</c> on the design, and R carries the
/// design's singular values unchanged since the reflections are orthogonal. Shared source rather than an API:
/// <c>Lodestar.Stats.Regression</c> and <c>Lodestar.Stats.TimeSeries</c> both refuse on it and cannot reach each
/// other's internals across a published floor (#978).
/// </remarks>
internal static class RankBracket
{
    /// <summary>Machine epsilon for <see cref="double"/>, which <see cref="double.Epsilon"/> is not.</summary>
    private const double MachineEpsilon = 2.220446049250313e-16;

    /// <summary><c>numpy.linalg.matrix_rank</c>'s relative tolerance for an <c>n × p</c> block.</summary>
    internal static double Tolerance(int rowCount, int order)
        => Math.Max(rowCount, order) * MachineEpsilon;

    /// <summary>Whether the diagonal alone proves the block rank-deficient, and which pivot is weakest.</summary>
    /// <remarks>
    /// <c>σmin ≤ minₖ|Rₖₖ|</c> and <c>σmax ≥ maxₖ|Rₖₖ|</c>, so a diagonal ratio at or below the tolerance bounds
    /// <c>σmin/σmax</c> there too and settles the refusal with no sweep. It also keeps an exact zero —
    /// <c>x₂ = 2·x₁</c> leaves one there — out of the inversion the caller does next.
    /// </remarks>
    /// <param name="upper">The triangle, row-major, entries below the diagonal ignored.</param>
    /// <param name="order">Its order.</param>
    /// <param name="tolerance">What <see cref="Tolerance"/> gave for this block.</param>
    /// <param name="weakest">The index of the smallest pivot, which the caller's refusal names.</param>
    internal static bool DiagonalProvesDeficient(
        double[] upper, int order, double tolerance, out int weakest)
    {
        double largest = 0.0;
        double smallest = double.PositiveInfinity;
        weakest = 0;
        for (int k = 0; k < order; k++)
        {
            double pivot = Math.Abs(upper[(k * order) + k]);
            if (pivot > largest)
            {
                largest = pivot;
            }

            if (pivot < smallest)
            {
                smallest = pivot;
                weakest = k;
            }
        }

        // Not negated into a > test: a NaN from the caller's data is not a collinear column.
        return smallest <= tolerance * largest;
    }

    /// <summary>Whether the Frobenius bracket proves the block has full rank, with no sweep.</summary>
    /// <remarks>
    /// <c>σmax ≤ ‖R‖_F</c> and <c>σmin ≥ 1/‖R⁻¹‖_F</c>, so a small enough product settles it. The bound gives
    /// away at most a factor <c>p</c>, which clears every block conditioned better than about <c>1/(p·n·ε)</c>.
    /// Written as a refuted "at least", so a NaN in the caller's data is accepted here rather than swept.
    /// </remarks>
    internal static bool FrobeniusProvesFullRank(
        double[] upper, double[] inverse, int order, double tolerance)
    {
        double squaredUpper = 0.0;
        double squaredInverse = 0.0;
        for (int i = 0; i < order * order; i++)
        {
            squaredUpper += upper[i] * upper[i];
            squaredInverse += inverse[i] * inverse[i];
        }

        return !(Math.Sqrt(squaredUpper) * Math.Sqrt(squaredInverse) * tolerance >= 1.0);
    }

    /// <summary>Whether the spectrum says the block is rank-deficient, which is the reference's own test.</summary>
    /// <remarks>The last resort, reached only by a block neither bracket settles.</remarks>
    internal static bool SpectrumProvesDeficient(double[] upper, int order, double tolerance)
    {
        var triangle = new double[order * order];
        for (int row = 0; row < order; row++)
        {
            for (int column = row; column < order; column++)
            {
                triangle[(column * order) + row] = upper[(row * order) + column];
            }
        }

        double[] singular = JacobiSpectrum.SingularValues(triangle, order, order);

        // Not negated into a > test: a NaN from the caller's data is not a collinear column.
        return singular[order - 1] <= tolerance * singular[0];
    }
}
