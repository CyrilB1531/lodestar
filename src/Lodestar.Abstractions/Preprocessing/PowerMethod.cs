namespace Lodestar.Preprocessing;

/// <summary>Which power family <c>PowerTransformer</c> fits.</summary>
public enum PowerMethod
{
    /// <summary>Yeo and Johnson's family, which admits zero and negative values. scikit-learn's <c>'yeo-johnson'</c>, and the default.</summary>
    YeoJohnson,

    /// <summary>Box and Cox's family, which needs strictly positive values. scikit-learn's <c>'box-cox'</c>.</summary>
    BoxCox,
}
