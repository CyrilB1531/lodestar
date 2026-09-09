namespace Lodestar.Preprocessing;

/// <summary>Which of the two standardisation steps <see cref="StandardScaler"/> applies.</summary>
/// <remarks>
/// Both default to <see langword="true"/>, as <c>sklearn.preprocessing.StandardScaler</c>'s
/// <c>with_mean</c> and <c>with_std</c> do. Turning one off changes what
/// <see cref="StandardScaler.Transform"/> does <em>and</em> which fitted statistics exist —
/// see the remarks on <see cref="StandardScaler.Fit"/>, which are the reference's, not this
/// package's invention.
/// </remarks>
public sealed record StandardScalerOptions
{
    /// <summary>Whether to centre each feature on its mean.</summary>
    public bool WithMean { get; init; } = true;

    /// <summary>Whether to divide each feature by its standard deviation.</summary>
    public bool WithStd { get; init; } = true;
}
