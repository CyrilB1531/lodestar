namespace Lodestar.Stats;

/// <summary>A correlation between every pair of variables, and the p-value of each.</summary>
/// <remarks>
/// What <c>scipy.stats.spearmanr</c> returns for a 2-D array: two square matrices, here row-major, entry
/// <c>i × VariableCount + j</c> relating variable <c>i</c> to variable <c>j</c>. The diagonal is each variable against
/// itself. scipy returns a scalar where there are only two variables; this keeps the matrix.
/// </remarks>
/// <param name="VariableCount">How many variables, the side of both matrices.</param>
/// <param name="Statistics">The correlations, row-major.</param>
/// <param name="PValues">Their p-values, row-major.</param>
// CA1819, S2368: the matrices mirror scipy's two arrays, and a caller indexes them in step; AndersonResult is
// suppressed for the same reason.
#pragma warning disable CA1819
public sealed record CorrelationMatrix(int VariableCount, double[] Statistics, double[] PValues)
{
    /// <summary>Compares the size and both matrices, value by value.</summary>
    /// <param name="other">The result to compare against.</param>
    /// <remarks>The generated equality would compare the matrices by reference, as <c>AndersonResult</c>'s would.</remarks>
    public bool Equals(CorrelationMatrix? other) =>
        other is not null
        && (ReferenceEquals(this, other)
            || (VariableCount == other.VariableCount
                && ValueEquality.Same(Statistics, other.Statistics)
                && ValueEquality.Same(PValues, other.PValues)));

    /// <summary>Hashes the size, which is O(1); equal results agree on it.</summary>
    public override int GetHashCode() => VariableCount;
}
#pragma warning restore CA1819
