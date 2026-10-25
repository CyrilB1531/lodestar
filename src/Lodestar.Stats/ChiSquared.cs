using Lodestar.Stats.Internal;

namespace Lodestar.Stats;

/// <summary>Pearson's chi-square: goodness of fit, and independence in a contingency table.</summary>
public static class ChiSquared
{
    // 256 doubles is 2 KB of stack at most, per marginal; a wider table allocates as before.
    private const int MaxStackMarginal = 256;

    // sqrt(2.220446049250313e-16), numpy's finfo(float64).eps ** 0.5: scipy's chisquare sum check.
    private const double SumTolerance = 1.4901161193847656e-8;

    /// <summary>Tests observed counts against an expected distribution.</summary>
    /// <param name="observed">The observed counts. None, or one alone, gives a NaN p-value, as scipy's does.</param>
    /// <param name="expected">
    /// The expected counts, which must sum to the observed total; omit them for a uniform
    /// expectation, matching <c>scipy.stats.chisquare</c> with <c>f_exp=None</c>.
    /// </param>
    /// <param name="nanPolicy">What to do with a <c>NaN</c>; scipy's <c>nan_policy</c>.</param>
    /// <returns>The statistic and the upper-tail p-value.</returns>
    /// <exception cref="ArgumentException">
    /// Mismatched lengths, a negative expectation (a zero one reads +∞ where a count meets it, <c>NaN</c> once any is 0/0), or
    /// expectations that do not sum to the observations. An explicit <paramref
    /// name="expected"/> is filtered together with <paramref name="observed"/>, so omission
    /// usually raises here -- it drops matched pairs and so breaks the sum-agreement check
    /// by construction (decision 0007). When <paramref name="nanPolicy"/> is
    /// <see cref="NanPolicy.Raise"/> and either span holds a <c>NaN</c>.
    /// </exception>
    /// <exception cref="ArgumentOutOfRangeException">An option is outside its enumeration.</exception>
    public static TestResult GoodnessOfFit(
        ReadOnlySpan<double> observed,
        ReadOnlySpan<double> expected = default,
        NanPolicy nanPolicy = NanPolicy.Propagate)
    {
        ReadOnlySpan<double> counts = observed;
        ReadOnlySpan<double> expectation = expected;
        if (nanPolicy != NanPolicy.Propagate)
        {
            if (expected.IsEmpty)
            {
                counts = NanFilter.Apply(observed, nanPolicy, nameof(observed));
            }
            else
            {
                NanFilter.ApplyAligned(
                    observed, expected, nanPolicy, nameof(observed), nameof(expected), out counts, out expectation);
            }
        }

        // No category, given or left by omission: an empty sum of 0 on -1 degrees of freedom, whose tail Pearson's test leaves
        // undefined, so a NaN p-value, as decision 0010 rules, where this threw; expectations of another length still refused.
        if (counts.Length < 1)
        {
            if (!expectation.IsEmpty)
            {
                throw new ArgumentException(
                    $"There are 0 observations and {expectation.Length} expectations.", nameof(expected));
            }
            Options.Require(nanPolicy);
            return new TestResult(0.0, double.NaN);
        }

        double observedTotal = 0.0;
        for (int i = 0; i < counts.Length; i++)
        {
            observedTotal += counts[i];
        }

        double[] target = expectation.IsEmpty
            ? UniformExpectation(counts.Length, observedTotal)
            : ExplicitExpectation(counts.Length, expectation, observedTotal);
        // After the input's own refusals, which keep their order (#1712's Review A).
        Options.Require(nanPolicy);

        // A zero expectation reads +inf where a count meets it, a p of 0 as scipy's, and 0/0, undefined, where none does: NaN
        // (decision 0010), which wins over any +inf. The arithmetic below gives both.
        double statistic = 0.0;
        for (int i = 0; i < counts.Length; i++)
        {
            double deviation = counts[i] - target[i];
            statistic += deviation * deviation / target[i];
        }

        int dof = counts.Length - 1;

        // One category leaves no degree of freedom, and scipy's p-value is NaN.
        double pValue = dof < 1 ? double.NaN : UpperTail(statistic, dof);

        return new TestResult(statistic, pValue);
    }

    /// <summary>The chi-squared upper tail of <paramref name="statistic"/>, or NaN where it is NaN or negative.</summary>
    /// <remarks>
    /// A NaN or infinite count makes the statistic NaN, finite cells whose marginals overflow too (#1710), and negative counts
    /// can make it negative (#1709): RegularizedQ refused each under its own "x", where scipy's p-value is NaN.
    /// </remarks>
    private static double UpperTail(double statistic, int dof) =>
        statistic >= 0.0 ? Gamma.ChiSquaredTail(dof, statistic, upper: true) : double.NaN;

    private static double[] UniformExpectation(int categories, double observedTotal)
    {
        double[] target = new double[categories];
        double uniform = observedTotal / categories;
        for (int i = 0; i < target.Length; i++)
        {
            target[i] = uniform;
        }

        return target;
    }

    private static double[] ExplicitExpectation(
        int observedLength, ReadOnlySpan<double> expected, double observedTotal)
    {
        if (expected.Length != observedLength)
        {
            throw new ArgumentException(
                $"There are {observedLength} observations and {expected.Length} expectations.",
                nameof(expected));
        }

        double[] target = new double[expected.Length];
        double expectedTotal = 0.0;
        for (int i = 0; i < expected.Length; i++)
        {
            // A negative expectation is no distribution; a zero one is, its cells reading +inf, or NaN at 0/0, and NaN propagates.
            if (expected[i] < 0.0)
            {
                throw new ArgumentException(
                    $"Expectation {i} is {expected[i]}; an expectation cannot be negative.",
                    nameof(expected));
            }

            target[i] = expected[i];
            expectedTotal += expected[i];
        }

        // An expectation summing elsewhere is no distribution over these categories; scipy's bound,
        // sqrt(eps) relative to the smaller total (#1248).
        if (Math.Abs(expectedTotal - observedTotal) / Math.Min(expectedTotal, observedTotal) > SumTolerance)
        {
            throw new ArgumentException(
                $"The expectations sum to {expectedTotal} and the observations to {observedTotal}.",
                nameof(expected));
        }

        return target;
    }

    /// <summary>Tests a contingency table for independence of its two factors.</summary>
    /// <param name="table">The observed counts, row-major and rectangular.</param>
    /// <param name="continuity">
    /// Whether to apply Yates's correction. It is defined for 2x2 tables only,
    /// so asking for it on any other shape changes nothing — the same rule
    /// <c>scipy.stats.chi2_contingency</c> follows with <c>correction=True</c>.
    /// </param>
    /// <returns>The statistic, the p-value, the degrees of freedom and the expected table.</returns>
    /// <exception cref="ArgumentException">
    /// The table is empty, ragged, or holds a negative, infinite or NaN count. A zero row or column
    /// total answers <c>NaN</c> (decision 0010), before one row or one column, which otherwise answers a statistic of 0 and a
    /// p-value of 1.
    /// </exception>
    /// <exception cref="ArgumentOutOfRangeException">An option is outside its enumeration.</exception>
    // S2368: the table arrives from the caller already in this shape -- that is
    // how scipy.stats.chi2_contingency takes it, and how ChiSquaredContingencyResult
    // hands the expected table back. Wrapping one side and not the other buys
    // no safety, only a conversion at the boundary (same reasoning as
    // ChiSquaredContingencyResult's own suppression in TestResult.cs).
#pragma warning disable S2368
    public static ChiSquaredContingencyResult Contingency(
        double[][] table, Continuity continuity = Continuity.Applied)
#pragma warning restore S2368
    {
        Guard.NotNull(table);

        if (table.Length < 1 || table[0] is null || table[0].Length < 1)
        {
            throw new ArgumentException("A contingency table needs at least one row and one column.", nameof(table));
        }

        int rows = table.Length;
        int columns = table[0].Length;

        // The marginals are scratch -- only the expected table they build is returned -- so a
        // table of ordinary shape keeps them on the stack (the 2x2 call went from 200 B to 168 B).
        Span<double> rowTotals = rows <= MaxStackMarginal ? stackalloc double[rows] : new double[rows];
        Span<double> columnTotals =
            columns <= MaxStackMarginal ? stackalloc double[columns] : new double[columns];
        double total = ComputeMarginals(table, rowTotals, columnTotals);
        // Read only as Applied or not before, so (Continuity)9 ran as None; refused after the cells, as the table's own refusals are.
        Options.Require(continuity);
        double[][] expected = ComputeExpected(rowTotals, columnTotals, total, rows, columns);
        int dof = (rows - 1) * (columns - 1);
        if (AnyZeroMarginal(rowTotals, columnTotals))
        {
            return new ChiSquaredContingencyResult(double.NaN, double.NaN, dof, expected);
        }
        // One row or one column leaves no degree of freedom: (0, 1), as scipy's where no expectation reads zero.
        if (dof == 0)
        {
            return new ChiSquaredContingencyResult(0.0, 1.0, dof, expected);
        }

        bool yates = continuity == Continuity.Applied && rows == 2 && columns == 2;
        double statistic = ComputeStatistic(table, expected, rows, columns, yates);

        double pValue = UpperTail(statistic, dof);

        return new ChiSquaredContingencyResult(statistic, pValue, dof, expected);
    }

    private static double ComputeMarginals(
        double[][] table, Span<double> rowTotals, Span<double> columnTotals)
    {
        int rows = rowTotals.Length;
        int columns = columnTotals.Length;
        double total = 0.0;

        for (int i = 0; i < rows; i++)
        {
            if (table[i] is null || table[i].Length != columns)
            {
                throw new ArgumentException($"Row {i} is not {columns} wide.", nameof(table));
            }

            for (int j = 0; j < columns; j++)
            {
                double value = table[i][j];

                // Cells are counts, so a negative, NaN or infinite one is refused under the table rather than propagated: the
                // reference page and equivalence.md's nan_policy row say why.
                if (value < 0.0 || double.IsNaN(value) || double.IsPositiveInfinity(value))
                {
                    throw new ArgumentException(
                        $"Cell [{i}][{j}] is {value}; counts must be finite and non-negative.",
                        nameof(table));
                }

                rowTotals[i] += value;
                columnTotals[j] += value;
                total += value;
            }
        }

        return total;
    }

    // A zero marginal makes an expectation zero, which the statistic divides by: Pearson leaves it undefined, so the test
    // answers NaN (decision 0010), where it threw. S1244: a sum of non-negative cells is zero only where every cell is.
#pragma warning disable S1244
    private static bool AnyZeroMarginal(ReadOnlySpan<double> rowTotals, ReadOnlySpan<double> columnTotals)
    {
        foreach (double total in rowTotals)
        {
            if (total == 0.0)
            {
                return true;
            }
        }
        foreach (double total in columnTotals)
        {
            if (total == 0.0)
            {
                return true;
            }
        }
        return false;
    }
#pragma warning restore S1244

    private static double[][] ComputeExpected(
        ReadOnlySpan<double> rowTotals, ReadOnlySpan<double> columnTotals, double total,
        int rows, int columns)
    {
        double[][] expected = new double[rows][];
        for (int i = 0; i < rows; i++)
        {
            expected[i] = new double[columns];
            for (int j = 0; j < columns; j++)
            {
                expected[i][j] = rowTotals[i] * columnTotals[j] / total;
            }
        }

        return expected;
    }

    private static double ComputeStatistic(
        double[][] table, double[][] expected, int rows, int columns, bool yates)
    {
        double statistic = 0.0;
        for (int i = 0; i < rows; i++)
        {
            for (int j = 0; j < columns; j++)
            {
                double deviation = Math.Abs(table[i][j] - expected[i][j]);

                // Yates moves the observation half a unit toward the expectation,
                // never past it, or a table agreeing within half a count would overshoot.
                if (yates)
                {
                    deviation = Math.Max(0.0, deviation - 0.5);
                }

                statistic += deviation * deviation / expected[i][j];
            }
        }

        return statistic;
    }
}
