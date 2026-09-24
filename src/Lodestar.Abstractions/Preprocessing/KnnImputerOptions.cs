namespace Lodestar.Preprocessing;

/// <summary>How many neighbours <c>KnnImputer</c> averages, and how it weights them.</summary>
public sealed record KnnImputerOptions
{
    /// <summary>How many donors each missing value is averaged over; scikit-learn's <c>n_neighbors</c>, default 5.</summary>
    public int NeighbourCount { get; init; } = 5;

    /// <summary>Whether nearer donors count for more; scikit-learn's <c>weights</c>, default <see cref="NeighbourWeights.Uniform"/>.</summary>
    public NeighbourWeights Weights { get; init; } = NeighbourWeights.Uniform;
}
