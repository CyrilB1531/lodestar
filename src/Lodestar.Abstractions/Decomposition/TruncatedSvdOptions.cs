namespace Lodestar.Decomposition;

/// <summary>What <c>TruncatedSvd.Fit</c> is allowed to vary.</summary>
public sealed class TruncatedSvdOptions
{
    /// <summary>Extra columns drawn beyond the rank asked for. scikit-learn's default is 10.</summary>
    public int Oversampling { get; init; } = 10;

    /// <summary>Power iterations. scikit-learn's <c>TruncatedSVD</c> default is 5.</summary>
    public int PowerIterations { get; init; } = 5;

    /// <summary>What happens to the block between the two products.</summary>
    public PowerIterationNormalizer Normalizer { get; init; } = PowerIterationNormalizer.Auto;

    /// <summary>Seeds this package's own generator when <see cref="RandomMatrix"/> is null.</summary>
    /// <remarks>
    /// It reproduces a run of Lodestar, not a run of scikit-learn: the two draw from different
    /// generators. Pass <see cref="RandomMatrix"/> to compare against Python.
    /// </remarks>
    public int Seed { get; init; }

    /// <summary>Ω itself, row-major and <c>features × (components + oversampling)</c>, or null to draw one.</summary>
    // CA1819 (properties should not return arrays): Ω is a dense block, and the whole
    // point of accepting one is that the caller already holds the numbers scikit-learn
    // drew. Copying it defensively would double the largest allocation the fit makes,
    // to protect a value this type reads once and never keeps.
#pragma warning disable CA1819
    public double[]? RandomMatrix { get; init; }
#pragma warning restore CA1819
}
