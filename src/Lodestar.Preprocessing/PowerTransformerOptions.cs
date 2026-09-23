namespace Lodestar.Preprocessing;

/// <summary>Which power family <see cref="PowerTransformer"/> fits, and whether it standardises after.</summary>
public sealed record PowerTransformerOptions
{
    /// <summary>Which family; scikit-learn's <c>method</c>, default <see cref="PowerMethod.YeoJohnson"/>.</summary>
    public PowerMethod Method { get; init; } = PowerMethod.YeoJohnson;

    /// <summary>Whether to centre and scale after transforming; scikit-learn's <c>standardize</c>, default <see langword="true"/>.</summary>
    public bool Standardize { get; init; } = true;
}

/// <summary>Which power family <see cref="PowerTransformer"/> fits.</summary>
public enum PowerMethod
{
    /// <summary>Yeo and Johnson's family, which admits zero and negative values. scikit-learn's <c>'yeo-johnson'</c>, and the default.</summary>
    YeoJohnson,

    /// <summary>Box and Cox's family, which needs strictly positive values. scikit-learn's <c>'box-cox'</c>.</summary>
    BoxCox,
}
