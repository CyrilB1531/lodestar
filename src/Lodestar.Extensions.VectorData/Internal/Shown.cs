namespace Lodestar.Extensions.VectorData;

/// <summary>How a refusal shows a value the caller gave it, a name or a key's text.</summary>
internal static class Shown
{
    /// <summary>The text as it is, or quoted when it is empty or blank, so it still shows in the sentence.</summary>
    internal static string Value(string text) => string.IsNullOrWhiteSpace(text) ? $"\"{text}\"" : text;
}
