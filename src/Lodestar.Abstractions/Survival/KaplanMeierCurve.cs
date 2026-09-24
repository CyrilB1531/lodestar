namespace Lodestar.Survival;

/// <summary>A Kaplan-Meier survival curve with its Greenwood variance and interval.</summary>
/// <param name="Steps">The curve's steps, ascending in time, starting at zero.</param>
/// <param name="Survival">The survival estimate at each step.</param>
/// <param name="Lower">The lower confidence bound at each step.</param>
/// <param name="Upper">The upper confidence bound at each step.</param>
/// <param name="ConfidenceLevel">The level the bounds were built at.</param>
/// <remarks>
/// The four arrays share one index with <paramref name="Steps"/>. Bounds are built on the
/// <strong>log-log transform</strong> of the estimate, which is what lifelines reports by
/// default and is not the same as the estimate plus or minus its own standard error — the
/// reference page has the two numbers side by side.
/// </remarks>
// CA1819 (properties should not return arrays): the curves hand back the arrays the fitter produced, and a caller reads them positionally against Steps.
// Copying them defensively would allocate a second copy of every curve to protect values the type only ever returns.
#pragma warning disable CA1819
public sealed record KaplanMeierCurve(
    SurvivalStep[] Steps,
    double[] Survival,
    double[] Lower,
    double[] Upper,
    double ConfidenceLevel)
{
    /// <summary>Compares the steps and all three curves, element by element.</summary>
    /// <param name="other">The curve to compare against.</param>
    /// <remarks>
    /// The generated equality would compare the four arrays by reference, so two curves fitted
    /// from the same data would be unequal. a record whose member compares by reference writes its own equality.
    /// </remarks>
    public bool Equals(KaplanMeierCurve? other)
    {
        if (ReferenceEquals(this, other))
        {
            return true;
        }
        // S1244: the level is a stored configuration, compared by bits so NaN stays reflexive.
#pragma warning disable S1244
        if (other is null || !ConfidenceLevel.Equals(other.ConfidenceLevel))
#pragma warning restore S1244
        {
            return false;
        }
        return ValueEquality.Same(Steps, other.Steps)
            && ValueEquality.Same(Survival, other.Survival)
            && ValueEquality.Same(Lower, other.Lower)
            && ValueEquality.Same(Upper, other.Upper);
    }

    /// <summary>Hashes the level and the step count, which is O(1).</summary>
    /// <remarks>
    /// The four arrays share one index, so the step count stands for all of them. A curve can
    /// hold thousands of steps, which is the walk this avoids.
    /// </remarks>
    public override int GetHashCode()
    {
        unchecked
        {
            int hash = (17 * 31) + ConfidenceLevel.GetHashCode();
            return (hash * 31) + ValueEquality.CountOf(Steps);
        }
    }
}
#pragma warning restore CA1819
