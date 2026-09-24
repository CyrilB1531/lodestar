namespace Lodestar.Preprocessing;

/// <summary>Whether <c>MaxAbsScaler</c> clips a transformed value into <c>[−1, 1]</c>.</summary>
/// <remarks>
/// <c>sklearn.preprocessing.MaxAbsScaler</c>'s <c>clip</c>, off by default as it is there, and applying
/// to <c>MaxAbsScaler.Transform</c> only — the reference's <c>inverse_transform</c> does not clip.
/// </remarks>
public sealed record MaxAbsScalerOptions
{
    /// <summary>Whether a value beyond the fitted maximum is pulled back to the unit interval.</summary>
    public bool Clip { get; init; }
}
