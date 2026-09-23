namespace Lodestar.Preprocessing;

/// <summary>What <see cref="PolynomialFeatures"/> expands, and how far.</summary>
/// <remarks>
/// scikit-learn's <c>degree</c>, <c>interaction_only</c> and <c>include_bias</c>, with the same
/// defaults. <c>order</c> is absent: it chooses numpy's memory layout, not a different answer.
/// </remarks>
public sealed record PolynomialFeaturesOptions
{
    /// <summary>The highest total power a term may reach; scikit-learn's <c>degree</c>, default 2.</summary>
    public int Degree { get; init; } = 2;

    /// <summary>
    /// Whether to keep only terms in which every feature appears at most once; scikit-learn's
    /// <c>interaction_only</c>, default <see langword="false"/>.
    /// </summary>
    /// <remarks>
    /// With it on, <c>x0 x1</c> survives and <c>x0²</c> does not — which is what a model wants
    /// when the question is whether two features act together rather than whether one curves.
    /// </remarks>
    public bool InteractionOnly { get; init; }

    /// <summary>Whether to emit the constant term; scikit-learn's <c>include_bias</c>, default <see langword="true"/>.</summary>
    public bool IncludeBias { get; init; } = true;
}
