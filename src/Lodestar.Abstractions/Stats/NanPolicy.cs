namespace Lodestar.Stats;

/// <summary>What a test does with a <c>NaN</c> in its input; scipy's <c>nan_policy</c>.</summary>
/// <remarks>
/// Offered on the sixteen entry points whose scipy counterpart takes the parameter, and on
/// <c>Pearson.Test</c>, whose counterpart does not: scipy's pull request 22155 gives the
/// reason as the shape of its own result object, not as anything about the statistic.
/// <c>docs/equivalence.md</c> carries the row, and decision 0007 has the rule.
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
