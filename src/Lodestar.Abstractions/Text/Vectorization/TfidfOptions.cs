using Lodestar.Abstractions;

namespace Lodestar.Text.Vectorization;

/// <summary>Options for <c>TfidfTransformer</c> / <c>TfidfVectorizer</c>.</summary>
/// <remarks>Defaults mirror <c>sklearn.feature_extraction.text.TfidfTransformer</c>.</remarks>
public sealed record TfidfOptions
{
    /// <summary>Multiply term frequencies by the inverse document frequency. Default <c>true</c>.</summary>
    public bool UseIdf { get; init; } = true;

    /// <summary>
    /// Smooth idf weights by adding one to document frequencies, as if an extra document
    /// contained every term once: <c>idf = ln((1 + n) / (1 + df)) + 1</c>. Default <c>true</c>.
    /// </summary>
    public bool SmoothIdf { get; init; } = true;

    /// <summary>Replace term frequency <c>tf</c> with <c>1 + ln(tf)</c>. Default <c>false</c>.</summary>
    public bool SublinearTf { get; init; }

    /// <summary>Row normalization, or <c>null</c> for none. Default <see cref="SparseNorm.L2"/>.</summary>
    public SparseNorm? Norm { get; init; } = SparseNorm.L2;
}
