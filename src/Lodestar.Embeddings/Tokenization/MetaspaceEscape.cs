using System.Buffers;

namespace Lodestar.Embeddings.Tokenization;

/// <summary>Where the replacement is prepended, which a file spells three ways.</summary>
internal enum MetaspacePrependScheme
{
    /// <summary>Never prepended.</summary>
    Never,

    /// <summary>Prepended to the first piece only.</summary>
    First,

    /// <summary>Prepended to every piece.</summary>
    Always,
}

/// <summary>Escapes whitespace to a meta symbol, the way SentencePiece does.</summary>
internal sealed class MetaspaceEscape
{
    public MetaspaceEscape(
        char replacement,
        MetaspacePrependScheme prependScheme,
        bool removeExtraWhitespaces,
        bool skipPrependWhenAlreadyPrefixed,
        bool declaredAsNormalizer = false)
    {
        Replacement = replacement;
        PrependScheme = prependScheme;
        RemoveExtraWhitespaces = removeExtraWhitespaces;
        SkipPrependWhenAlreadyPrefixed = skipPrependWhenAlreadyPrefixed;
        DeclaredAsNormalizer = declaredAsNormalizer;
    }

    public char Replacement { get; }

    public MetaspacePrependScheme PrependScheme { get; }

    public bool RemoveExtraWhitespaces { get; }

    /// <summary>Whether the prepend is skipped when the escaped text already begins with the replacement.</summary>
    /// <remarks>
    /// The one field the two declarations disagree on: a <c>Metaspace</c> block guards its
    /// prepend on <c>starts_with</c>, and the <c>Prepend</c> + <c>Replace</c> normalizer
    /// sequence prepends unconditionally, since <c>Prepend</c> runs before <c>Replace</c>
    /// and knows nothing of the symbol. docs/equivalence.md's Metaspace rows measure the boundary and amend
    /// 0050 §2's "two writings of one value" to hold everywhere but here.
    /// </remarks>
    public bool SkipPrependWhenAlreadyPrefixed { get; }

    /// <summary>Whether the file spelled this escape as a normalizer rather than as a pre-tokenizer.</summary>
    /// <remarks>
    /// The third place the two spellings part, and the one <see cref="BpeTokenizer"/> reads
    /// when it builds a <c>normalized: true</c> added token's pattern: <c>tokenizers</c>
    /// normalizes that content with the declared normalizer and not with a pre-tokenizer, so
    /// Llama-2 matches on <c>▁&lt;s&gt;</c> where Mistral matches on <c>&lt;s&gt;</c>.
    /// Not the same question as <see cref="SkipPrependWhenAlreadyPrefixed"/>, which the
    /// unigram path also leaves false without being a normalizer, which docs/equivalence.md's Metaspace rows carry.
    /// </remarks>
    public bool DeclaredAsNormalizer { get; }

    /// <summary>Applies the escape to <paramref name="text"/>.</summary>
    /// <param name="text">The piece to escape.</param>
    /// <param name="isFirstSplit">
    /// Whether this piece is the first the input produced, which is what
    /// <see cref="MetaspacePrependScheme.First"/> prepends to. An added token counts as a
    /// piece and so consumes it — measured against <c>tokenizers</c> 0.23.1, where
    /// <c>"&lt;s&gt;the cat"</c> under <c>first</c> is <c>['&lt;s&gt;', 'the', '▁cat']</c>.
    /// </param>
    public string Apply(string text, bool isFirstSplit)
    {
        if (RemoveExtraWhitespaces)
        {
            return CollapseAndPrepend(text, isFirstSplit);
        }
        string escaped = text.Replace(' ', Replacement);

        // Nothing survived the collapse, so there is nothing to prefix — the unigram path
        // has always returned empty here rather than a lone symbol.
        if (escaped.Length == 0 || !Prepends(isFirstSplit))
        {
            return escaped;
        }

        // The guard reads the escaped text, where tokenizers applies it too: a leading
        // space begins with the symbol only once the replace has run.
        return SkipPrependWhenAlreadyPrefixed && escaped[0] == Replacement
            ? escaped
            : Replacement + escaped;
    }

    /// <summary>Whether this piece is one the scheme prepends to at all.</summary>
    private bool Prepends(bool isFirstSplit) =>
        PrependScheme == MetaspacePrependScheme.Always
        || (PrependScheme == MetaspacePrependScheme.First && isFirstSplit);

    /// <summary>Runs of U+0020 become one replacement and the ends lose theirs, then the prepend applies as in <see cref="Apply"/>.</summary>
    /// <remarks>
    /// What splitting on the space, dropping the empties and joining gives, written as one
    /// pass into one buffer: the split built a string per word and the prepend a second copy
    /// of the whole. U+0020 only: a tab no normalizer rewrote stays as it is, which is what
    /// <c>docs/equivalence.md</c>'s Unigram row records.
    /// </remarks>
    private string CollapseAndPrepend(string text, bool isFirstSplit)
    {
        // Slot 0 is held for the prepend, so either answer is one string over the buffer.
        char[] buffer = ArrayPool<char>.Shared.Rent(text.Length + 1);
        try
        {
            int length = 1;
            bool gap = false;
            foreach (char c in text)
            {
                if (c == ' ')
                {
                    gap = length > 1;
                    continue;
                }
                if (gap)
                {
                    buffer[length++] = Replacement;
                    gap = false;
                }
                buffer[length++] = c;
            }

            // Nothing survived the collapse, so there is nothing to prefix — the unigram path
            // has always returned empty here rather than a lone symbol.
            if (length == 1)
            {
                return string.Empty;
            }
            bool prepend = Prepends(isFirstSplit) && !(SkipPrependWhenAlreadyPrefixed && buffer[1] == Replacement);
            buffer[0] = Replacement;
            return prepend ? new string(buffer, 0, length) : new string(buffer, 1, length - 1);
        }
        finally
        {
            ArrayPool<char>.Shared.Return(buffer);
        }
    }
}
