namespace Lodestar.Embeddings.Tokenization;

/// <summary>Refuses text that is not well-formed UTF-16, which no reference tokenizer can be handed (#1324).</summary>
/// <remarks>
/// A Python <c>str</c> holding a lone surrogate cannot cross into <c>tokenizers</c>' Rust or
/// sentencepiece's C++, so the references refuse it before they tokenize; here the UTF-8 walks threw
/// <c>EncoderFallbackException</c> on some models and read an unknown piece on others.
/// </remarks>
internal static class WellFormedText
{
    /// <summary>Throws when <paramref name="text"/> holds a surrogate half without its pair.</summary>
    public static void Require(string text, string paramName)
    {
        int i = 0;
        while (i < text.Length)
        {
            char c = text[i];
            if (char.IsHighSurrogate(c) && i + 1 < text.Length && char.IsLowSurrogate(text[i + 1]))
            {
                i += 2;
                continue;
            }

            if (char.IsSurrogate(c))
            {
                throw new ArgumentException(
                    $"{paramName} holds a lone surrogate at position {i}, which is not text any tokenizer can read.", paramName);
            }

            i++;
        }
    }
}
