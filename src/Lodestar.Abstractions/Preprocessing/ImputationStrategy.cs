namespace Lodestar.Preprocessing;

/// <summary>What <c>SimpleImputer</c> fills a missing value with.</summary>
/// <remarks><c>sklearn.impute.SimpleImputer</c>'s <c>strategy</c>, the four it offers for numbers.</remarks>
public enum ImputationStrategy
{
    /// <summary>The feature's mean over the values that are present.</summary>
    Mean,

    /// <summary>
    /// Its median, numpy's masked median: the average of the two middle values, the middle one with itself for an odd
    /// count, so a middle value past half the largest double gives infinity, as the reference's does.
    /// </summary>
    Median,

    /// <summary>Its most frequent value; a tie goes to the smaller, as the reference's does.</summary>
    MostFrequent,

    /// <summary>A value the caller chose — <see cref="SimpleImputerOptions.FillValue"/>, zero by default.</summary>
    Constant,
}
