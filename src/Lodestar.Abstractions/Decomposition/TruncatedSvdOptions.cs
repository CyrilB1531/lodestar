namespace Lodestar.Decomposition;

/// <summary>What <c>TruncatedSvd.Fit</c> is allowed to vary.</summary>
public sealed record TruncatedSvdOptions
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

    /// <summary>Ω itself, row-major and <c>min(rows, features) × (components + oversampling)</c>, or null to draw one.</summary>
    /// <remarks>
    /// A matrix with fewer rows than features is factored as its transpose, as <c>randomized_svd(transpose="auto")</c>
    /// factors it, so its Ω has one row per row of the matrix (#1256). It is taken and exposed as it is, not copied:
    /// writing to it changes these options, what they equal and a <c>with</c> copy of them, not a fit already run, which
    /// read it once (#1305).
    /// </remarks>
    // CA1819 (properties should not return arrays): Ω is a dense block, and the whole
    // point of accepting one is that the caller already holds the numbers scikit-learn
    // drew. Copying it defensively would double the largest allocation the fit makes,
    // to protect a value this type reads once and never keeps.
#pragma warning disable CA1819
    public double[]? RandomMatrix { get; init; }
#pragma warning restore CA1819

    /// <summary>Compares every option, <see cref="RandomMatrix"/> element by element.</summary>
    /// <param name="other">The options to compare against.</param>
    /// <remarks>
    /// The generated equality would compare <see cref="RandomMatrix"/> by reference, so two
    /// option sets built from separate arrays holding the same Ω would be unequal.
    /// </remarks>
    public bool Equals(TruncatedSvdOptions? other) =>
        ReferenceEquals(this, other)
        || (other is not null
            && Oversampling == other.Oversampling
            && PowerIterations == other.PowerIterations
            && Normalizer == other.Normalizer
            && Seed == other.Seed
            && ValueEquality.Same(RandomMatrix, other.RandomMatrix));

    /// <summary>Hashes the scalars and the length of Ω, which is O(1).</summary>
    public override int GetHashCode()
    {
        unchecked
        {
            int hash = (17 * 31) + Oversampling;
            hash = (hash * 31) + PowerIterations;
            hash = (hash * 31) + (int)Normalizer;
            hash = (hash * 31) + Seed;
            return (hash * 31) + ValueEquality.CountOf(RandomMatrix);
        }
    }
}
