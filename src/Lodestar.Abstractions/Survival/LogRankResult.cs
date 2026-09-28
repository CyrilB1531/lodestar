namespace Lodestar.Survival;

/// <summary>The outcome of a log-rank test, two groups or more.</summary>
/// <param name="Statistic">The log-rank statistic.</param>
/// <param name="PValue">Its upper-tail chi-squared p-value.</param>
/// <param name="DegreesOfFreedom">One for two groups, and one fewer than the groups compared for more.</param>
public sealed record LogRankResult(double Statistic, double PValue, int DegreesOfFreedom)
{
    /// <summary>Hashes every member as the generated equality compares it, each <c>NaN</c> alike (#1285).</summary>
    public override int GetHashCode()
    {
        unchecked
        {
            int hash = (17 * 31) + ValueEquality.HashOf(Statistic);
            hash = (hash * 31) + ValueEquality.HashOf(PValue);
            return (hash * 31) + DegreesOfFreedom;
        }
    }
}
