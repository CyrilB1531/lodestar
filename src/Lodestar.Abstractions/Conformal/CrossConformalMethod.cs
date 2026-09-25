namespace Lodestar.Conformal;

/// <summary>How <c>CrossConformal</c> turns the out-of-sample predictions at a test point into an interval.</summary>
/// <remarks>
/// MAPIE 1.5.0's <c>method</c> of its cross-conformal regressors. Its <c>base</c> is the split interval over the
/// out-of-sample scores, <c>SplitConformal.Interval</c>, and takes no member here; <c>naive</c> calibrates on the
/// training samples and is not written.
/// </remarks>
public enum CrossConformalMethod
{
    /// <summary>
    /// CV+ and Jackknife+: quantiles, over the training samples, of each one's held-out prediction at the test point
    /// widened by its own score. The default.
    /// </summary>
    Plus,

    /// <summary>The smallest and the largest held-out prediction at the test point, widened by the scores' quantile.</summary>
    MinMax,
}
