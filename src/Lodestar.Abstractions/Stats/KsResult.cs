namespace Lodestar.Stats;

/// <summary>A two-sample Kolmogorov-Smirnov result.</summary>
/// <param name="Statistic">The supremum distance between the two empirical distributions.</param>
/// <param name="PValue">The p-value on the requested tail.</param>
/// <param name="StatisticLocation">The observed value at which that supremum is attained.</param>
/// <param name="StatisticSign">
/// <c>+1</c> when the first sample's empirical distribution exceeds the second's
/// at that point, <c>-1</c> when it falls below.
/// </param>
public sealed record KsResult(
    double Statistic, double PValue, double StatisticLocation, int StatisticSign)
{
    /// <summary>Hashes every member as the generated equality compares it, each <c>NaN</c> alike (#1285).</summary>
    public override int GetHashCode()
    {
        unchecked
        {
            int hash = (17 * 31) + ValueEquality.HashOf(Statistic);
            hash = (hash * 31) + ValueEquality.HashOf(PValue);
            hash = (hash * 31) + ValueEquality.HashOf(StatisticLocation);
            return (hash * 31) + StatisticSign;
        }
    }
}
