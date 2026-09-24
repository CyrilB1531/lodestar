namespace Lodestar.Preprocessing;

/// <summary>Which category, if any, <c>OneHotEncoder{T}</c> leaves without a column.</summary>
/// <remarks><c>sklearn.preprocessing.OneHotEncoder</c>'s <c>drop</c>, with its three settings.</remarks>
public enum CategoryDrop
{
    /// <summary>Every category gets a column — the reference's <c>drop=None</c>.</summary>
    None,

    /// <summary>Each feature's first category loses its column, so it encodes to all zeros.</summary>
    First,

    /// <summary>The first category loses its column only where the feature has exactly two.</summary>
    IfBinary,
}
