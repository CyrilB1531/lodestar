namespace Lodestar.Preprocessing;

/// <summary>What <c>OneHotEncoder{T}</c> does with a value the fit never saw.</summary>
/// <remarks><c>sklearn.preprocessing.OneHotEncoder</c>'s <c>handle_unknown</c>.</remarks>
public enum UnknownCategory
{
    /// <summary>Refuse it, naming the feature — the reference's <c>"error"</c>, and the default.</summary>
    Refuse,

    /// <summary>Encode it as all zeros — the reference's <c>"ignore"</c>.</summary>
    Ignore,
}
