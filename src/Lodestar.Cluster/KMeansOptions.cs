namespace Lodestar.Cluster;

/// <summary>How <see cref="KMeans.Fit"/> starts, and when it stops.</summary>
public sealed record KMeansOptions
{
    /// <summary>How many Lloyd iterations to run at most; scikit-learn's <c>max_iter</c>.</summary>
    public int MaxIterations { get; init; } = 300;

    /// <summary>The convergence threshold before scaling; scikit-learn's <c>tol</c>.</summary>
    /// <remarks>
    /// Scaled by the mean feature variance before it is used, exactly as
    /// <c>sklearn.cluster._kmeans._tolerance</c> scales it — so the same number means the
    /// same thing on a matrix of millimetres and a matrix of kilometres. A <c>0</c> keeps
    /// iterating until the labels stop moving.
    /// </remarks>
    public double Tolerance { get; init; } = 1e-4;

    /// <summary>Drives this package's own generator when <see cref="InitialCentres"/> is absent.</summary>
    /// <remarks>
    /// <strong>It reproduces a run of Lodestar, never a run of scikit-learn.</strong> The two
    /// libraries draw from different generators, so a shared seed shares nothing — see
    /// <see cref="InitialCentres"/> for what does.
    /// </remarks>
    public int Seed { get; init; }

    /// <summary>The starting centres, row-major, or <see langword="null"/> to choose them.</summary>
    /// <remarks>
    /// <strong>The centres are an input, not a seed</strong> — decision 0072's move. Given
    /// here they replace the choice entirely, which turns k-means into an ordinary parity
    /// target. Left <see langword="null"/>, k-means++ chooses them using <see cref="Seed"/>,
    /// which is reproducible here and is not scikit-learn's draw.
    /// </remarks>
    // CA1819 (properties should not return arrays): the same bargain decision 0072 struck
    // for Ω. The whole point of accepting a block is that the caller already holds the
    // numbers scikit-learn chose; copying it defensively to hand it back would protect a
    // value this type reads once, at the cost of the allocation the fit exists to avoid.
#pragma warning disable CA1819
    public double[]? InitialCentres { get; init; }
#pragma warning restore CA1819
}
