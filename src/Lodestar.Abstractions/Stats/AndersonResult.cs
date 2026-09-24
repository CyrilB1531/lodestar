namespace Lodestar.Stats;

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
