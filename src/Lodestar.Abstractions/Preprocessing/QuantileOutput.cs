namespace Lodestar.Preprocessing;

/// <summary>Which distribution <c>QuantileTransformer</c> maps its ranks onto.</summary>
public enum QuantileOutput
{
    /// <summary>The unit interval. scikit-learn's <c>'uniform'</c>, and the default.</summary>
    Uniform,

    /// <summary>The standard normal, through its quantile function. scikit-learn's <c>'normal'</c>.</summary>
    Normal,
}
