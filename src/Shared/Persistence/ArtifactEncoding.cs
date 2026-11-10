using System.Text;

namespace Lodestar.Internal.Persistence;

/// <summary>The one strict UTF-8 every artifact and model-side configuration is read and written in.</summary>
/// <remarks>
/// Shared beside <see cref="ArtifactLimits"/> with <c>Lodestar.Onnx</c>, whose sentence-transformers configuration reader
/// refuses invalid bytes as Python's codec does, without the rest of the persistence plumbing (#1257).
/// </remarks>
internal static class ArtifactEncoding
{
    /// <summary>UTF-8 without a byte-order mark, which throws on invalid bytes and surrogates rather than substituting.</summary>
    /// <remarks>
    /// <c>Utf8JsonWriter</c> emits no preamble of its own, so writing is BOM-free by construction; this instance serves
    /// the paths that need an explicit encoder.
    /// </remarks>
    public static readonly UTF8Encoding Utf8NoBom = new(encoderShouldEmitUTF8Identifier: false, throwOnInvalidBytes: true);
}
