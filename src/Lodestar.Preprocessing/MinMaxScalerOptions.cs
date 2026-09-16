namespace Lodestar.Preprocessing;

/// <summary>The range <see cref="MinMaxScaler"/> maps each feature onto, and whether it clips to it.</summary>
/// <remarks>
/// <c>sklearn.preprocessing.MinMaxScaler</c>'s <c>feature_range</c> and <c>clip</c>, same defaults.
/// Clipping applies to <see cref="MinMaxScaler.Transform"/> only: the reference's
/// <c>inverse_transform</c> does not clip, and a value clipped on the way out cannot be undone.
/// </remarks>
public sealed record MinMaxScalerOptions
{
    /// <summary>The bottom of the range each feature's minimum maps to.</summary>
    public double Low { get; init; }

    /// <summary>The top of the range each feature's maximum maps to.</summary>
    public double High { get; init; } = 1.0;

    /// <summary>Whether <see cref="MinMaxScaler.Transform"/> clips an unseen value into the range.</summary>
    public bool Clip { get; init; }
}
