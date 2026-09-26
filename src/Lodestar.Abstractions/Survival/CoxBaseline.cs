namespace Lodestar.Survival;

/// <summary>One stratum's baseline of a Cox fit: Breslow's hazard at each time, accumulated, and the survival it implies.</summary>
/// <param name="Stratum">The stratum's label, zero for an unstratified fit.</param>
/// <param name="Times">The times, ascending: every distinct duration of the fit, as lifelines indexes it.</param>
/// <param name="Hazard">The baseline hazard at each time, zero where the stratum has no event.</param>
/// <param name="CumulativeHazard">Its running sum.</param>
/// <param name="Survival"><c>exp(−CumulativeHazard)</c>.</param>
/// <remarks>
/// The baseline is the one of a subject at the covariates' means, as lifelines centres them; multiplying by a
/// subject's partial hazard, <c>exp((x − mean) · β)</c>, gives that subject's cumulative hazard.
/// </remarks>
// CA1819 (properties should not return arrays): the arrays share one index with Times and are read positionally,
// as KaplanMeierCurve's are.
#pragma warning disable CA1819
public sealed record CoxBaseline(int Stratum, double[] Times, double[] Hazard, double[] CumulativeHazard, double[] Survival)
{
    /// <summary>Compares the label and the four arrays, element by element.</summary>
    /// <param name="other">The baseline to compare against.</param>
    /// <remarks>The generated equality would compare the arrays by reference.</remarks>
    public bool Equals(CoxBaseline? other)
    {
        if (ReferenceEquals(this, other))
        {
            return true;
        }

        return other is not null
            && Stratum == other.Stratum
            && ValueEquality.Same(Times, other.Times)
            && ValueEquality.Same(Hazard, other.Hazard)
            && ValueEquality.Same(CumulativeHazard, other.CumulativeHazard)
            && ValueEquality.Same(Survival, other.Survival);
    }

    /// <summary>Hashes the label and the time count, which is O(1).</summary>
    public override int GetHashCode()
    {
        unchecked
        {
            return ((17 * 31) + Stratum) * 31 + ValueEquality.CountOf(Times);
        }
    }
}
#pragma warning restore CA1819
