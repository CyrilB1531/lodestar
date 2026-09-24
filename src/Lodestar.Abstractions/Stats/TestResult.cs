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
