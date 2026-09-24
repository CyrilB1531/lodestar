namespace Lodestar.Stats;

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
