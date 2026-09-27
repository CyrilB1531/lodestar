namespace Lodestar.Decomposition;

/// <summary>What <c>Nmf.Fit</c> is allowed to vary.</summary>
public sealed record NmfOptions
{
    /// <summary>What the factorization minimises.</summary>
    public NmfBetaLoss BetaLoss { get; init; } = NmfBetaLoss.Frobenius;

    /// <summary>Where the iteration starts. Ignored by the overload that is handed W and H.</summary>
    /// <remarks>
    /// scikit-learn's <c>init=None</c> resolves to <c>nndsvda</c> whenever the rank is at most
    /// <c>min(n_samples, n_features)</c>, the only rank a fit here accepts (#1232).
    /// </remarks>
    public NmfInitialization Initialization { get; init; } = NmfInitialization.NndSvda;

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

    /// <summary>Compares every option, <see cref="RandomMatrix"/> element by element.</summary>
    /// <param name="other">The options to compare against.</param>
    /// <remarks>The generated equality would compare <see cref="RandomMatrix"/> by reference, as <c>TruncatedSvdOptions</c>' would.</remarks>
    public bool Equals(NmfOptions? other)
    {
        if (ReferenceEquals(this, other))
        {
            return true;
        }
        if (other is null
            || BetaLoss != other.BetaLoss
            || Initialization != other.Initialization
            || MaxIterations != other.MaxIterations
            || Seed != other.Seed
            // S1244: value equality between two configurations, not arithmetic, so no epsilon;
            // double.Equals also makes NaN equal to NaN, which == gets wrong.
#pragma warning disable S1244
            || !Tolerance.Equals(other.Tolerance))
#pragma warning restore S1244
        {
            return false;
        }
        return ValueEquality.Same(RandomMatrix, other.RandomMatrix);
    }

    /// <summary>Hashes the scalars and the length of Ω, which is O(1).</summary>
    public override int GetHashCode()
    {
        unchecked
        {
            int hash = (17 * 31) + (int)BetaLoss;
            hash = (hash * 31) + (int)Initialization;
            hash = (hash * 31) + MaxIterations;
            hash = (hash * 31) + Seed;
            // .NET Framework hashes NaN payloads apart where double.Equals makes them equal.
            hash = (hash * 31) + (double.IsNaN(Tolerance) ? 0 : Tolerance.GetHashCode());
            return (hash * 31) + ValueEquality.CountOf(RandomMatrix);
        }
    }
}
