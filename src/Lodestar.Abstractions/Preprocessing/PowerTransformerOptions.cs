namespace Lodestar.Preprocessing;

/// <summary>Which power family <c>PowerTransformer</c> fits, and whether it standardises after.</summary>
public sealed record PowerTransformerOptions
{
    /// <summary>Which family; scikit-learn's <c>method</c>, default <see cref="PowerMethod.YeoJohnson"/>.</summary>
    public PowerMethod Method { get; init; } = PowerMethod.YeoJohnson;

    /// <summary>Whether to centre and scale after transforming; scikit-learn's <c>standardize</c>, default <see langword="true"/>.</summary>
    public bool Standardize { get; init; } = true;
}
