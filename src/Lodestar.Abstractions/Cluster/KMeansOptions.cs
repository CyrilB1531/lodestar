namespace Lodestar.Cluster;

/// <summary>How <c>KMeans.Fit</c> starts, and when it stops.</summary>
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
    /// <strong>The centres are an input, not a seed</strong> — decision 0004's move. Given
    /// here they replace the choice entirely, which turns k-means into an ordinary parity
    /// target. Left <see langword="null"/>, k-means++ chooses them using <see cref="Seed"/>,
    /// which is reproducible here and is not scikit-learn's draw.
    /// </remarks>
    // CA1819 (properties should not return arrays): the same bargain decision 0004 struck
    // for Ω. The whole point of accepting a block is that the caller already holds the
    // numbers scikit-learn chose; copying it defensively to hand it back would protect a
    // value this type reads once, at the cost of the allocation the fit exists to avoid.
#pragma warning disable CA1819
    public double[]? InitialCentres { get; init; }
#pragma warning restore CA1819

    /// <summary>Several starting blocks, each row-major, the fit keeping the best — scikit-learn's <c>n_init</c> over given starts.</summary>
    /// <remarks>
    /// Each block is fitted in turn; a later one replaces the kept fit only when its inertia is strictly lower and its
    /// partition differs, scikit-learn's rule. Refused together with <see cref="InitialCentres"/> or <see cref="Restarts"/>.
    /// </remarks>
    public IReadOnlyList<double[]>? InitialCentreSets { get; init; }

    /// <summary>How many k-means++ starts to fit and keep the best of, by the same rule; 1, the default, fits one.</summary>
    /// <remarks>
    /// Start <c>i</c> seeds this package's generator with <see cref="Seed"/> plus <c>i</c>, so it reproduces here and is not
    /// scikit-learn's <c>n_init</c> draw. Read only when the fit chooses its own starts.
    /// </remarks>
    public int Restarts { get; init; } = 1;

    /// <summary>Compares every option, the centres element by element.</summary>
    /// <param name="other">The options to compare against.</param>
    /// <remarks>
    /// The generated equality would compare <see cref="InitialCentres"/> by reference, so two
    /// option sets built from separate arrays holding the same centres would be unequal.
    /// the rule has the rule and the six records that reached it first.
    /// </remarks>
    public bool Equals(KMeansOptions? other)
    {
        if (ReferenceEquals(this, other))
        {
            return true;
        }
        if (other is null
            || MaxIterations != other.MaxIterations
            || Seed != other.Seed
            || Restarts != other.Restarts
            // S1244 warns against exact floating-point comparison, which is right for
            // arithmetic and wrong here: this is value equality between two configurations,
            // where "the same tolerance" means the same bits. double.Equals also makes NaN
            // equal to NaN, which a record's equality needs and == gets wrong.
#pragma warning disable S1244
            || !Tolerance.Equals(other.Tolerance))
#pragma warning restore S1244
        {
            return false;
        }
        return ValueEquality.Same(InitialCentres, other.InitialCentres) && SameSets(InitialCentreSets, other.InitialCentreSets);
    }

    private static bool SameSets(IReadOnlyList<double[]>? left, IReadOnlyList<double[]>? right)
    {
        if (left is null || right is null)
        {
            return left is null && right is null;
        }

        return left.Count == right.Count && left.Zip(right, (a, b) => ValueEquality.Same(a, b)).All(same => same);
    }

    /// <summary>Hashes the scalars and the centre count, which is O(1).</summary>
    /// <remarks>
    /// Equal options necessarily agree on the count; unequal ones are allowed to collide.
    /// Hashing the centres themselves would make the cheap operation the expensive one.
    /// </remarks>
    public override int GetHashCode()
    {
        unchecked
        {
            int hash = (17 * 31) + MaxIterations;
            hash = (hash * 31) + Seed;
            hash = (hash * 31) + Restarts;
            hash = (hash * 31) + (InitialCentreSets?.Count ?? -1);
            hash = (hash * 31) + Tolerance.GetHashCode();
            return (hash * 31) + ValueEquality.CountOf(InitialCentres);
        }
    }
}
