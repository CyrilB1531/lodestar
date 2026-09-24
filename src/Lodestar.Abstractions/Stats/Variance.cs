namespace Lodestar.Stats;

/// <summary>Whether an independent-samples t-test pools the two variances.</summary>
/// <remarks>
/// scipy's <c>ttest_ind</c> defaults to <c>equal_var=True</c>, which is Student's
/// test, not Welch's. Pooling is only correct when the two population variances
/// really are equal; Welch is the safer default in practice and the deliberate
/// non-default here, so the caller has to say which one they mean.
/// </remarks>
public enum Variance
{
    /// <summary>Pool the two variances — Student's t. scipy's <c>equal_var=True</c>.</summary>
    Equal,

    /// <summary>Do not pool; use the Welch-Satterthwaite degrees of freedom. scipy's <c>equal_var=False</c>.</summary>
    Welch,
}
