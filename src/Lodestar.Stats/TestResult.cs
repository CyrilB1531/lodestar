namespace Lodestar.Stats;

/// <summary>A test statistic and the p-value that goes with it.</summary>
/// <remarks>
/// Eight of the ten families return exactly this, because eight of the ten
/// scipy calls return exactly this — measured, not assumed. The three that
/// carry more have their own record below rather than making the other eight
/// pay for fields they would leave empty.
/// </remarks>
/// <param name="Statistic">The test statistic, on whichever scale the family defines.</param>
/// <param name="PValue">The probability of a statistic at least this extreme under the null.</param>
public sealed record TestResult(double Statistic, double PValue);

/// <summary>A t-test's result: the statistic, the p-value and the degrees of freedom.</summary>
/// <param name="Statistic">The t statistic.</param>
/// <param name="PValue">The p-value on the requested tail.</param>
/// <param name="Df">
/// The degrees of freedom. Integral for Student and for the paired and
/// one-sample tests; fractional for Welch, whose Satterthwaite denominator is
/// not a count of anything.
/// </param>
public sealed record TTestResult(double Statistic, double PValue, double Df)
{
    /// <summary>The quantity the test compared: a mean, or a difference of means.</summary>
    /// <remarks>
    /// Internal rather than public. It exists so a later confidence-interval
    /// method can be added to the result instead of a second call that
    /// re-derives everything, and scipy keeps it hidden on its own result for
    /// the same reason.
    /// </remarks>
    internal double Estimate { get; init; }

    /// <summary>The standard error of <see cref="Estimate"/>. Internal, as above.</summary>
    internal double StandardError { get; init; }

    /// <summary>Which tail was tested, which decides whether an interval is half-open.</summary>
    internal Alternative Alternative { get; init; }

    /// <summary>The confidence interval for the difference this test measured.</summary>
    /// <remarks>
    /// A method, not a property, because it takes a level -- scipy exposes it
    /// the same way via <c>TtestResult.confidence_interval</c>; a named tuple
    /// avoids a public record for what is only a two-double carrier. A
    /// one-sided interval is half-open, not narrower: it says nothing about how
    /// far the difference could be, so the far bound is an infinity rather than a number.
    /// </remarks>
    /// <param name="level">The confidence level, strictly between 0 and 1.</param>
    /// <exception cref="ArgumentOutOfRangeException">
    /// <paramref name="level"/> is NaN or outside <c>(0, 1)</c>.
    /// </exception>
    public (double Low, double High) ConfidenceInterval(double level = 0.95)
    {
        if (double.IsNaN(level) || level <= 0.0 || level >= 1.0)
        {
            throw new ArgumentOutOfRangeException(
                nameof(level), level, "The confidence level must lie strictly inside (0, 1).");
        }

        // A one-sided test spends its whole error budget on one side, so the tail
        // is 1 - level rather than half of it, and the other bound is infinite.
        double tail = Alternative == Alternative.TwoSided ? (1.0 - level) / 2.0 : 1.0 - level;
        double half = Internal.Beta.StudentQuantile(tail, Df) * StandardError;

        return Alternative switch
        {
            Alternative.TwoSided => (Estimate - half, Estimate + half),
            Alternative.Greater => (Estimate - half, double.PositiveInfinity),
            Alternative.Less => (double.NegativeInfinity, Estimate + half),
            // CA2208: Alternative is a field, not a parameter here -- a bad
            // value stored on the result is a broken invariant, not a bad call.
            _ => throw new InvalidOperationException($"Unrecognised alternative: {Alternative}."),
        };
    }
}

/// <summary>A contingency-table chi-square result.</summary>
/// <param name="Statistic">The chi-square statistic.</param>
/// <param name="PValue">The upper-tail p-value.</param>
/// <param name="Dof">The degrees of freedom, <c>(rows - 1) * (columns - 1)</c>.</param>
/// <param name="ExpectedFrequencies">
/// The table expected under independence, row-major, same shape as the input.
/// </param>
// CA1819 (properties should not return arrays), S2368 (no jagged-array constructor
// parameters): the expected table mirrors the shape of the caller's own input
// table, itself double[][] because that is how chi2_contingency takes it. Wrapping
// one side and not the other buys no safety, only a conversion at the boundary.
#pragma warning disable CA1819, S2368
public sealed record Chi2ContingencyResult(
    double Statistic, double PValue, int Dof, double[][] ExpectedFrequencies)
{
    /// <summary>Compares the scalars and the expected table, row by row.</summary>
    /// <param name="other">The result to compare against.</param>
    /// <remarks>
    /// The generated equality would compare <see cref="ExpectedFrequencies"/> by reference, so
    /// two results holding the same table would be unequal. a record whose member compares by reference writes its own equality.
    /// </remarks>
    public bool Equals(Chi2ContingencyResult? other)
    {
        if (ReferenceEquals(this, other))
        {
            return true;
        }
        if (other is null || Dof != other.Dof)
        {
            return false;
        }
        // S1244: value equality between two stored results, where "the same statistic" means
        // the same bits. double.Equals also makes NaN equal NaN, which equality must.
#pragma warning disable S1244
        if (!Statistic.Equals(other.Statistic) || !PValue.Equals(other.PValue))
#pragma warning restore S1244
        {
            return false;
        }
        return ValueEquality.Same(ExpectedFrequencies, other.ExpectedFrequencies);
    }

    /// <summary>Hashes the scalars and the row count, which is O(1).</summary>
    /// <remarks>
    /// Equal results necessarily agree on the row count; unequal ones may collide. Walking the
    /// table would make the cheap operation cost what the test itself cost.
    /// </remarks>
    public override int GetHashCode()
    {
        unchecked
        {
            int hash = (17 * 31) + Statistic.GetHashCode();
            hash = (hash * 31) + PValue.GetHashCode();
            hash = (hash * 31) + Dof;
            return (hash * 31) + ValueEquality.CountOf(ExpectedFrequencies);
        }
    }
}
#pragma warning restore CA1819, S2368

/// <summary>A two-sample Kolmogorov-Smirnov result.</summary>
/// <param name="Statistic">The supremum distance between the two empirical distributions.</param>
/// <param name="PValue">The p-value on the requested tail.</param>
/// <param name="StatisticLocation">The observed value at which that supremum is attained.</param>
/// <param name="StatisticSign">
/// <c>+1</c> when the first sample's empirical distribution exceeds the second's
/// at that point, <c>-1</c> when it falls below.
/// </param>
public sealed record KsResult(
    double Statistic, double PValue, double StatisticLocation, int StatisticSign);

/// <summary>A Pearson correlation and the p-value that goes with it.</summary>
/// <remarks>
/// Its own record rather than <see cref="TestResult"/> for the reason
/// <see cref="TTestResult"/> is: the interval needs the sample size and the tail that were
/// used, and a second call that re-derived them would be a second chance to disagree.
/// scipy carries the same two on its own <c>PearsonRResult</c>.
/// </remarks>
/// <param name="Statistic">The correlation coefficient, in <c>[-1, 1]</c>.</param>
/// <param name="PValue">The p-value on the requested tail.</param>
public sealed record PearsonResult(double Statistic, double PValue)
{
    /// <summary>The number of pairs the correlation was computed over.</summary>
    internal int N { get; init; }

    /// <summary>Which tail was tested, which decides whether an interval is half-open.</summary>
    internal Alternative Alternative { get; init; }

    /// <summary>The confidence interval for the correlation, through the Fisher z transform.</summary>
    /// <remarks>
    /// A method rather than a property because it takes a level, as
    /// <c>scipy.stats.pearsonr(...).confidence_interval()</c> does. A one-sided interval is
    /// half-open, its far bound the correlation's own limit of <c>-1</c> or <c>1</c>. Below
    /// four pairs <c>1 / sqrt(n - 3)</c> is undefined and the whole range is returned, as
    /// scipy returns it.
    /// </remarks>
    /// <param name="level">The confidence level, strictly between 0 and 1.</param>
    /// <exception cref="ArgumentOutOfRangeException">
    /// <paramref name="level"/> is NaN or outside <c>(0, 1)</c>.
    /// </exception>
    public (double Low, double High) ConfidenceInterval(double level = 0.95)
    {
        if (double.IsNaN(level) || level <= 0.0 || level >= 1.0)
        {
            throw new ArgumentOutOfRangeException(
                nameof(level), level, "The confidence level must lie strictly inside (0, 1).");
        }

        // Below four pairs there is no standard error, and scipy answers the whole range --
        // a constant input included. Past that a NaN reaches the computed bound alone.
        if (N <= 3)
        {
            return (-1.0, 1.0);
        }

        double z = Internal.Fisher.Atanh(Statistic);
        double error = Math.Sqrt(1.0 / (N - 3.0));

        // A one-sided level spends its whole error budget on one side, so the quantile is
        // taken at the level itself rather than at half of what is left outside it.
        return Alternative switch
        {
            Alternative.TwoSided => Bounds(z, error, Distributions.NormalQuantile(0.5 + (level / 2.0))),
            Alternative.Less => (-1.0, Math.Tanh(z + (Distributions.NormalQuantile(level) * error))),
            Alternative.Greater => (Math.Tanh(z - (Distributions.NormalQuantile(level) * error)), 1.0),
            // CA2208: Alternative is a field, not a parameter here -- a bad value stored on
            // the result is a broken invariant, not a bad call.
            _ => throw new InvalidOperationException($"Unrecognised alternative: {Alternative}."),
        };
    }

    private static (double Low, double High) Bounds(double z, double error, double quantile) =>
        (Math.Tanh(z - (quantile * error)), Math.Tanh(z + (quantile * error)));
}

/// <summary>A binomial test's result: the observed proportion and the p-value.</summary>
/// <remarks>
/// Its own record because the interval needs the counts and the tail that were tested, the same
/// reason <see cref="TTestResult"/> and <see cref="PearsonResult"/> carry theirs. scipy keeps
/// <c>k</c>, <c>n</c> and <c>alternative</c> on <c>BinomTestResult</c> for the same purpose.
/// </remarks>
/// <param name="Statistic">The observed proportion of successes, <c>successes / trials</c>.</param>
/// <param name="PValue">The p-value on the requested tail.</param>
public sealed record BinomialResult(double Statistic, double PValue)
{
    /// <summary>The successes the test was given.</summary>
    internal int Successes { get; init; }

    /// <summary>The trials the test was given.</summary>
    internal int Trials { get; init; }

    /// <summary>Which tail was tested, which decides whether an interval is half-open.</summary>
    internal Alternative Alternative { get; init; }

    /// <summary>The confidence interval for the proportion.</summary>
    /// <remarks>
    /// A method rather than a property because it takes a level and a shape, as
    /// <c>BinomTestResult.proportion_ci</c> does. A one-sided test's interval is half-open at
    /// <c>0</c> or <c>1</c> — the proportion's own limits, so there is no wider bound to give —
    /// and so is a two-sided interval when every trial succeeded or none did.
    /// </remarks>
    /// <param name="level">The confidence level, strictly between 0 and 1.</param>
    /// <param name="method">Which interval to compute.</param>
    /// <exception cref="ArgumentOutOfRangeException">
    /// <paramref name="level"/> is NaN or outside <c>(0, 1)</c>, or <paramref name="method"/> is
    /// not one of the three.
    /// </exception>
    public (double Low, double High) ProportionConfidenceInterval(
        double level = 0.95, ProportionInterval method = ProportionInterval.Exact)
    {
        if (double.IsNaN(level) || level <= 0.0 || level >= 1.0)
        {
            throw new ArgumentOutOfRangeException(
                nameof(level), level, "The confidence level must lie strictly inside (0, 1).");
        }

        // A one-sided interval spends its whole error budget on one side, so the tail is
        // 1 - level rather than half of it, and the other bound is the proportion's own limit.
        double tail = Alternative == Alternative.TwoSided ? (1.0 - level) / 2.0 : 1.0 - level;

        return method switch
        {
            ProportionInterval.Exact => ClopperPearson(tail),
            ProportionInterval.Wilson => Internal.WilsonInterval.Bounds(this, level, corrected: false),
            ProportionInterval.WilsonCorrected => Internal.WilsonInterval.Bounds(this, level, corrected: true),
            _ => throw new ArgumentOutOfRangeException(nameof(method), method, null),
        };
    }

    /// <summary>The exact interval, inverted from the binomial tails through the beta they are.</summary>
    private (double Low, double High) ClopperPearson(double tail)
    {
        double low = Successes == 0 || Alternative == Alternative.Less
            ? 0.0
            : Internal.BetaQuantile.Invert(tail, Successes, Trials - Successes + 1.0);
        double high = Successes == Trials || Alternative == Alternative.Greater
            ? 1.0
            : Internal.BetaQuantile.Invert(1.0 - tail, Successes + 1.0, Trials - (double)Successes);

        return (low, high);
    }
}

/// <summary>An Anderson-Darling result: the statistic, and the table it is read against.</summary>
/// <remarks>
/// Two shapes in one record, because scipy is replacing the first with the second: since 1.17
/// the critical-value shape warns, and 1.19 removes it for a p-value interpolated from the same
/// table. Both are carried — the p-value is what a reader of the other families expects, and the
/// critical values are what carries information, the interpolation being clamped to
/// <c>[0.01, 0.15]</c>. <c>docs/equivalence.md</c> has the whole of it.
/// </remarks>
/// <param name="Statistic">The A² statistic; larger means further from normal.</param>
/// <param name="PValue">The p-value interpolated from the table, clamped to its ends.</param>
/// <param name="CriticalValues">The statistic's critical values, one per significance level.</param>
/// <param name="SignificanceLevels">The significance levels, in percent, as scipy reports them.</param>
// CA1819 (properties should not return arrays), S2368 (no jagged-array constructor parameters):
// the two tables mirror what scipy returns and what a caller indexes in step; wrapping one side
// buys no safety, only a conversion at the boundary. Chi2ContingencyResult is suppressed for the
// same reason.
#pragma warning disable CA1819
public sealed record AndersonResult(
    double Statistic, double PValue, double[] CriticalValues, double[] SignificanceLevels)
{
    /// <summary>Compares the two scalars and both tables, value by value.</summary>
    /// <param name="other">The result to compare against.</param>
    /// <remarks>
    /// The generated equality would compare the tables by reference, so two results holding the
    /// same numbers would be unequal — a record whose member compares by reference writes its own.
    /// </remarks>
    public bool Equals(AndersonResult? other)
    {
        if (ReferenceEquals(this, other))
        {
            return true;
        }
        if (other is null)
        {
            return false;
        }

        // S1244: value equality between two stored results, where "the same statistic" means the
        // same bits. double.Equals also makes NaN equal NaN, which equality must.
#pragma warning disable S1244
        if (!Statistic.Equals(other.Statistic) || !PValue.Equals(other.PValue))
#pragma warning restore S1244
        {
            return false;
        }

        return ValueEquality.Same(CriticalValues, other.CriticalValues)
            && ValueEquality.Same(SignificanceLevels, other.SignificanceLevels);
    }

    /// <summary>Hashes the scalars and the table length, which is O(1).</summary>
    /// <remarks>
    /// Equal results necessarily agree on the length; unequal ones may collide. Walking the
    /// tables would make the cheap operation cost what the test itself cost.
    /// </remarks>
    public override int GetHashCode()
    {
        unchecked
        {
            int hash = (17 * 31) + Statistic.GetHashCode();
            hash = (hash * 31) + PValue.GetHashCode();
            return (hash * 31) + CriticalValues.Length;
        }
    }
}
#pragma warning restore CA1819
