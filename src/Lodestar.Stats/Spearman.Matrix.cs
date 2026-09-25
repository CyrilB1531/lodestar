using Lodestar.Stats.Internal;

namespace Lodestar.Stats;

public static partial class Spearman
{
    /// <summary>Spearman's rho between every pair of variables — scipy's <c>spearmanr</c> on a 2-D array.</summary>
    /// <param name="data">The observations, row-major: one row per observation, <paramref name="variableCount"/> values each.</param>
    /// <param name="variableCount">How many variables each row holds; at least two.</param>
    /// <param name="alternative">Which tail each p-value covers.</param>
    /// <param name="nanPolicy">
    /// What to do with a <c>NaN</c>. <see cref="NanPolicy.Omit"/> drops rows pair by pair, so each pair keeps every row
    /// complete for its two variables, as scipy's does; <see cref="NanPolicy.Propagate"/> leaves a variable holding a
    /// <c>NaN</c> with <c>NaN</c> in its row and column, its own diagonal included, and every other pair computed.
    /// </param>
    /// <returns>The correlations and p-values, each <c>variableCount × variableCount</c> and row-major.</returns>
    /// <exception cref="ArgumentOutOfRangeException"><paramref name="variableCount"/> is below two.</exception>
    /// <exception cref="ArgumentException">
    /// <paramref name="data"/> is not a whole number of rows, or holds a <c>NaN</c> under <see cref="NanPolicy.Raise"/>.
    /// </exception>
    /// <remarks>Each pair is <see cref="Test"/> on its two columns; scipy returns a scalar for two variables, this the matrix.</remarks>
    public static CorrelationMatrix Matrix(
        ReadOnlySpan<double> data,
        int variableCount,
        Alternative alternative = Alternative.TwoSided,
        NanPolicy nanPolicy = NanPolicy.Propagate)
    {
        if (variableCount < 2)
        {
            throw new ArgumentOutOfRangeException(nameof(variableCount), variableCount, "A correlation matrix needs at least two variables.");
        }

        if (data.Length % variableCount != 0)
        {
            throw new ArgumentException($"{data.Length} values are not whole rows of {variableCount}.", nameof(data));
        }

        (double[][] columns, bool[] missing) = Columns(data, variableCount, nanPolicy);
        // Each column ranked once, where a pair at a time would rank it once per partner (#1162's benchmark); only the
        // pairwise omission of rows needs the pair's own ranks.
        bool pairwise = nanPolicy == NanPolicy.Omit && Array.Exists(missing, flag => flag);
        var ranks = new double[]?[variableCount];
        for (int j = 0; j < variableCount && !pairwise; j++)
        {
            ranks[j] = missing[j] || columns[j].Length < 2 ? null : Ranks.Average(columns[j]);
        }
        var statistics = new double[variableCount * variableCount];
        var pValues = new double[variableCount * variableCount];
        for (int a = 0; a < variableCount; a++)
        {
            for (int b = a; b < variableCount; b++)
            {
                TestResult pair = pairwise ? Pair(columns, missing, a, b, alternative, nanPolicy) : Ranked(ranks, a, b, alternative);
                statistics[(a * variableCount) + b] = statistics[(b * variableCount) + a] = pair.Statistic;
                pValues[(a * variableCount) + b] = pValues[(b * variableCount) + a] = pair.PValue;
            }
        }

        return new CorrelationMatrix(variableCount, statistics, pValues);
    }

    /// <summary>One entry of the matrix, as scipy's two paths compute it.</summary>
    /// <remarks>
    /// Under <c>omit</c>, where the data hold a <c>NaN</c>, scipy takes <c>mstats</c>' path, which writes the diagonal
    /// as <c>(1, 0)</c> whatever the alternative; otherwise it computes it, so a variable against itself reads <c>p = 1</c> under
    /// <see cref="Alternative.Less"/> there. Each is replayed as it is.
    /// </remarks>
    private static TestResult Pair(
        double[][] columns, bool[] missing, int a, int b, Alternative alternative, NanPolicy nanPolicy)
    {
        if (nanPolicy == NanPolicy.Propagate && (missing[a] || missing[b]))
        {
            return new TestResult(double.NaN, double.NaN);
        }

        if (a != b)
        {
            return Test(columns[a], columns[b], alternative, nanPolicy);
        }

        // mstats' path, and its fixed diagonal, is taken only where a NaN is there to omit.
        return nanPolicy == NanPolicy.Omit && Array.Exists(missing, flag => flag)
            ? new TestResult(1.0, 0.0)
            : Diagonal(columns[a], alternative);
    }

    /// <summary>A variable against itself as <c>numpy.corrcoef</c> computes it: <c>(d / √d) / √d</c>, clipped to 1.</summary>
    /// <remarks>
    /// That can round to one ulp below 1, where the Student tail is no longer exactly zero (<c>7.7e-276</c> in a
    /// measured case), so the quotient is taken in numpy's order. <c>d</c>, the ranks' squared deviations, is exact
    /// before the scaling for any sample this runs on: the ranks are halves and their mean a quarter.
    /// </remarks>
    private static TestResult Diagonal(double[] column, Alternative alternative)
    {
        if (column.Length < 2)
        {
            return new TestResult(double.NaN, double.NaN);
        }

        return DiagonalOfRanks(Ranks.Average(column), alternative);
    }

    private static TestResult DiagonalOfRanks(double[] ranks, Alternative alternative)
    {
        double mean = ranks.Average();
        // numpy.cov multiplies by 1 / (n - 1) rather than dividing, and that rounding is what the ulp comes from.
        double d = ranks.Sum(rank => (rank - mean) * (rank - mean)) * (1.0 / (ranks.Length - 1.0));
        double root = Math.Sqrt(d);
        double rho = Math.Max(-1.0, Math.Min(1.0, d / root / root));
        return double.IsNaN(rho)
            ? new TestResult(double.NaN, double.NaN)
            : new TestResult(rho, PValue(rho, ranks.Length - 2.0, alternative));
    }

    /// <summary>One entry from ranks computed once: <see cref="Test"/>'s arithmetic on them, and the diagonal numpy's way.</summary>
    private static TestResult Ranked(double[]?[] ranks, int a, int b, Alternative alternative)
    {
        if (ranks[a] is not { } left || ranks[b] is not { } right)
        {
            return new TestResult(double.NaN, double.NaN);
        }

        if (a == b)
        {
            return DiagonalOfRanks(left, alternative);
        }

        double rho = Correlation.RankCoefficient(left, right);
        return new TestResult(rho, PValue(rho, left.Length - 2.0, alternative));
    }

    /// <summary>Each variable's column, and whether it holds a <c>NaN</c>, refused under <see cref="NanPolicy.Raise"/>.</summary>
    private static (double[][] Columns, bool[] Missing) Columns(ReadOnlySpan<double> data, int variableCount, NanPolicy nanPolicy)
    {
        int rows = data.Length / variableCount;
        var columns = new double[variableCount][];
        var missing = new bool[variableCount];
        for (int j = 0; j < variableCount; j++)
        {
            columns[j] = new double[rows];
            for (int i = 0; i < rows; i++)
            {
                columns[j][i] = data[(i * variableCount) + j];
                missing[j] |= double.IsNaN(columns[j][i]);
            }

            if (missing[j] && nanPolicy == NanPolicy.Raise)
            {
                throw new ArgumentException($"Variable {j} holds a NaN.", nameof(data));
            }
        }

        return (columns, missing);
    }
}
