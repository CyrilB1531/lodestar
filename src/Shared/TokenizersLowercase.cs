namespace Lodestar.Internal;

/// <summary>
/// tokenizers' <c>normalizers.Lowercase</c>: Rust's <c>char::to_lowercase</c>, one code point at a time, the one copy
/// Lodestar.Embeddings' WordPiece and Lodestar.Onnx's <c>do_lower_case</c> share (#1257).
/// </summary>
/// <remarks>
/// The invariant simple mapping agrees with it everywhere but U+0130, whose full mapping is
/// <c>i</c> + U+0307 — the one unconditional multi-character entry of SpecialCasing.txt, and
/// the reason a capital dotted I finds <c>i</c> then U+0307 in a vocabulary (#1213). Both are escaped below, so no
/// normalization of the source can fold the pair to one character.
/// </remarks>
internal static class TokenizersLowercase
{
    // CA1308: lowercasing is the normalizer's own step. CA1307: Replace(string, string) is ordinal already, and
    // netstandard2.0 has no overload taking a comparison.
#pragma warning disable CA1308, CA1307
    internal static string Of(string text) =>
        (text.Contains('\u0130') ? text.Replace("\u0130", "i\u0307") : text).ToLowerInvariant();
#pragma warning restore CA1308, CA1307
}
