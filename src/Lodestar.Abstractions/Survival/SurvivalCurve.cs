namespace Lodestar.Survival;

/// <summary>A survival curve with its confidence bounds, one value per step: what the Breslow-Fleming-Harrington estimator returns.</summary>
/// <param name="Steps">The curve's steps, ascending in time, starting at zero.</param>
/// <param name="Survival">The survival estimate at each step.</param>
/// <param name="Lower">The lower confidence bound at each step.</param>
/// <param name="Upper">The upper confidence bound at each step.</param>
/// <param name="ConfidenceLevel">The level the bounds were built at.</param>
// CA1819 (properties should not return arrays): the arrays share one index with Steps and are read positionally, as
// KaplanMeierCurve's are.
#pragma warning disable CA1819
public sealed record SurvivalCurve(
    SurvivalStep[] Steps,
    double[] Survival,
    double[] Lower,
    double[] Upper,
    double ConfidenceLevel)
{
    /// <summary>Compares the steps, the estimate and both bounds, element by element.</summary>
    /// <param name="other">The curve to compare against.</param>
    /// <remarks>The generated equality would compare the arrays by reference.</remarks>
    public bool Equals(SurvivalCurve? other)
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
    public override int GetHashCode()
    {
        unchecked
        {
            return (((17 * 31) + ConfidenceLevel.GetHashCode()) * 31) + ValueEquality.CountOf(Steps);
        }
    }
}
#pragma warning restore CA1819
