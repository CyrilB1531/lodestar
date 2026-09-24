namespace Lodestar.Preprocessing;

/// <summary>How <c>KnnImputer</c> weights the donors it averages.</summary>
public enum NeighbourWeights
{
    /// <summary>Every donor counts the same. scikit-learn's <c>'uniform'</c>, and the default.</summary>
    Uniform,

    /// <summary>Each donor counts as the reciprocal of its distance. scikit-learn's <c>'distance'</c>.</summary>
    Distance,
}
