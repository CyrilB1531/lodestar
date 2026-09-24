namespace Lodestar.Survival;

/// <summary>A Nelson-Aalen cumulative-hazard curve.</summary>
/// <param name="Steps">The curve's steps, ascending in time, starting at zero.</param>
/// <param name="CumulativeHazard">The cumulative hazard at each step.</param>
/// <remarks>
/// A step with <c>d</c> events among <c>n</c> at risk adds <c>1/n + 1/(n - 1) + … + 1/(n - d + 1)</c>,
/// not <c>d / n</c>, and the hazard sums those rather than multiplying survival fractions. So it
/// keeps rising where a Kaplan-Meier curve that has reached zero can no longer move.
/// </remarks>
// CA1819 (properties should not return arrays): the curves hand back the arrays the fitter produced, and a caller reads them positionally against Steps.
// Copying them defensively would allocate a second copy of every curve to protect values the type only ever returns.
#pragma warning disable CA1819
public sealed record NelsonAalenCurve(SurvivalStep[] Steps, double[] CumulativeHazard)
{
    /// <summary>Compares the steps and the hazard, element by element.</summary>
    /// <param name="other">The curve to compare against.</param>
    /// <remarks>
    /// The generated equality would compare both arrays by reference, so two curves fitted from
    /// the same data would be unequal. a record whose member compares by reference writes its own equality.
    /// </remarks>
    public bool Equals(NelsonAalenCurve? other)
    {
        if (ReferenceEquals(this, other))
        {
            return true;
        }
        return other is not null
            && ValueEquality.Same(Steps, other.Steps)
            && ValueEquality.Same(CumulativeHazard, other.CumulativeHazard);
    }

    /// <summary>Hashes the step count, which is O(1).</summary>
    /// <remarks>
    /// Both arrays share one index, so the step count stands for both. Equal curves agree on it;
    /// unequal ones are allowed to collide.
    /// </remarks>
    public override int GetHashCode()
    {
        unchecked
        {
            return (17 * 31) + ValueEquality.CountOf(Steps);
        }
    }
}
#pragma warning restore CA1819
