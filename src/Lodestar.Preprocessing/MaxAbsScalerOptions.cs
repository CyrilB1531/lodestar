namespace Lodestar.Preprocessing;

/// <summary>Whether <see cref="MaxAbsScaler"/> clips a transformed value into <c>[−1, 1]</c>.</summary>
/// <remarks>
/// <c>sklearn.preprocessing.MaxAbsScaler</c>'s <c>clip</c>, off by default as it is there, and applying
/// to <see cref="MaxAbsScaler.Transform(ReadOnlySpan{double})"/> only — the reference's <c>inverse_transform</c> does not clip.
/// </remarks>
public sealed record MaxAbsScalerOptions
{
    /// <summary>Whether a value beyond the fitted maximum is pulled back to the unit interval.</summary>
    public bool Clip { get; init; }
}
