namespace Lodestar.Preprocessing;

/// <summary>The range <c>MinMaxScaler</c> maps each feature onto, and whether it clips to it.</summary>
/// <remarks>
/// <c>sklearn.preprocessing.MinMaxScaler</c>'s <c>feature_range</c> and <c>clip</c>, same defaults.
/// Clipping applies to <c>MinMaxScaler.Transform</c> only: the reference's
/// <c>inverse_transform</c> does not clip, and a value clipped on the way out cannot be undone.
/// </remarks>
public sealed record MinMaxScalerOptions
{
    /// <summary>The bottom of the range each feature's minimum maps to.</summary>
    public double Low { get; init; }

    /// <summary>The top of the range each feature's maximum maps to.</summary>
    public double High { get; init; } = 1.0;

    /// <summary>Whether <c>MinMaxScaler.Transform</c> clips an unseen value into the range.</summary>
    public bool Clip { get; init; }

    /// <summary>Hashes every member as the generated equality compares it, each <c>NaN</c> alike (#1285).</summary>
    public override int GetHashCode()
    {
        unchecked
        {
            int hash = (17 * 31) + ValueEquality.HashOf(Low);
            hash = (hash * 31) + ValueEquality.HashOf(High);
            return (hash * 31) + ValueEquality.HashOfItem(Clip);
        }
    }
}
