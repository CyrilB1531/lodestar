namespace Lodestar.Stats;

/// <summary>What a test does with a <c>NaN</c> in its input; scipy's <c>nan_policy</c>.</summary>
/// <remarks>
/// Offered on the eleven entry points whose scipy counterpart takes the parameter, and on no
/// others — <c>chi2_contingency</c>, <c>fisher_exact</c> and <c>false_discovery_control</c> do not
/// take it, so neither do their counterparts here. Decision 0116 has the rule.
/// </remarks>
public enum NanPolicy
{
    /// <summary>Carry the <c>NaN</c> into the statistic and the p-value. scipy's default, and this package's behaviour before the parameter existed.</summary>
    Propagate = 0,

    /// <summary>Refuse the input with <see cref="System.ArgumentException"/>.</summary>
    Raise,

    /// <summary>Drop the missing observations and test on the rest.</summary>
    Omit,
}
