namespace Lodestar.Preprocessing;

/// <summary>What a row transformed by <c>KBinsDiscretizer</c> carries.</summary>
public enum BinEncoding
{
    /// <summary>One column per feature, holding its bin's index. scikit-learn's <c>'ordinal'</c>.</summary>
    Ordinal,

    /// <summary>One column per bin of each feature, one of them set. scikit-learn's <c>'onehot-dense'</c>, and the default shape.</summary>
    OneHot,
}
