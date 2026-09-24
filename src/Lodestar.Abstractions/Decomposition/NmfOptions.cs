namespace Lodestar.Decomposition;

/// <summary>What <c>Nmf.Fit</c> is allowed to vary.</summary>
public sealed class NmfOptions
{
    /// <summary>What the factorization minimises.</summary>
    public NmfBetaLoss BetaLoss { get; init; } = NmfBetaLoss.Frobenius;

    /// <summary>Where the iteration starts. Ignored by the overload that is handed W and H.</summary>
    public NmfInitialization Initialization { get; init; } = NmfInitialization.NndSvd;

    /// <summary>The iteration cap. scikit-learn's default is 200.</summary>
    public int MaxIterations { get; init; } = 200;

    /// <summary>The relative improvement below which the iteration stops, checked every ten.</summary>
    /// <remarks>
    /// Zero disables the stop, which turns <see cref="MaxIterations"/> into an input rather than
    /// a cap — that is what the oracle corpus does, so an iteration count cannot silently differ.
    /// </remarks>
    public double Tolerance { get; init; } = 1e-4;

    /// <summary>Seeds the initialisation's own generator when <see cref="RandomMatrix"/> is null.</summary>
    public int Seed { get; init; }

    /// <summary>Ω for the initialisation, row-major <c>features × (components + 10)</c>.</summary>
    // CA1819 (properties should not return arrays): the same bargain
    // TruncatedSvdOptions.RandomMatrix strikes. Ω is a dense block the caller already
    // holds — copying it defensively would double the largest allocation the fit makes,
    // to protect a value this type reads once and never keeps.
#pragma warning disable CA1819
    public double[]? RandomMatrix { get; init; }
#pragma warning restore CA1819
}
