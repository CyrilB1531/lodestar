using System;
#if NET8_0_OR_GREATER
using System.Buffers;
#endif
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;

namespace Lodestar.Embeddings.Tokenization;

/// <summary>
/// Finds the next <see cref="AddedToken"/> in a string. The one place either
/// tokenizer asks that question, so the two cannot answer it differently.
/// </summary>
internal sealed class AddedTokenScanner
{
    private readonly AddedToken[] _tokens;

    /// <summary>The entries by first character, each bucket longest first and otherwise in <see cref="_tokens"/> order.</summary>
    private readonly Dictionary<char, AddedToken[]> _byFirstChar;

#if NET8_0_OR_GREATER
    private readonly SearchValues<char> _firstChars;
#else
    private readonly char[] _firstChars;
#endif

    /// <summary>Keeps the entries that can match; the order does not matter.</summary>
    /// <remarks>
    /// An empty <see cref="AddedToken.Content"/> is dropped: it would match at
    /// every position without advancing the caller's scan, hanging the loop. The
    /// loader bounds a token's upper length but never rejects an empty one, so
    /// this cannot be assumed away.
    /// </remarks>
    internal AddedTokenScanner(IReadOnlyList<AddedToken> tokens)
    {
        _tokens = [.. tokens.Where(t => t.Content.Length > 0)];

        // OrderByDescending is stable, so an equal-length tie keeps the first entry, as the
        // one-scan-per-entry loop this replaced did.
        _byFirstChar = _tokens
            .GroupBy(t => t.Content[0])
            .ToDictionary(g => g.Key, g => g.OrderByDescending(t => t.Content.Length).ToArray());
        char[] firstChars = [.. _byFirstChar.Keys];
#if NET8_0_OR_GREATER
        _firstChars = SearchValues.Create(firstChars);
#else
        _firstChars = firstChars;
#endif
    }

    /// <summary>Whether any entry can ever match.</summary>
    internal bool IsEmpty => _tokens.Length == 0;

    /// <summary>
    /// The earliest match at or after <paramref name="from"/> — the longest one,
    /// on a tie — with the span it consumes once stripping is applied.
    /// </summary>
    /// <param name="text">The text being scanned.</param>
    /// <param name="from">Where to start; a strip never reaches behind it.</param>
    /// <param name="start">The first index the match consumes.</param>
    /// <param name="end">One past the last index the match consumes.</param>
    /// <param name="token">The entry that matched.</param>
    internal bool TryNext(string text, int from, out int start, out int end, [MaybeNullWhen(false)] out AddedToken token)
    {
        AddedToken? best = BestMatch(text, from, out int bestAt);

        if (best is null)
        {
            start = -1;
            end = -1;
            token = null;
            return false;
        }

        // Ties break on the raw match position, before either side's strip applies —
        // untested for a whitespace-content candidate; see docs/equivalence.md's added-token row.
        start = bestAt;
        end = bestAt + best.Content.Length;
        if (best.Lstrip)
        {
            while (start > from && char.IsWhiteSpace(text[start - 1]))
            {
                start--;
            }
        }
        if (best.Rstrip)
        {
            while (end < text.Length && char.IsWhiteSpace(text[end]))
            {
                end++;
            }
        }
        token = best;
        return true;
    }

    /// <summary>The entry that wins at or after <paramref name="from"/> — earliest, then longest — or <see langword="null"/>.</summary>
    /// <param name="text">The text being scanned.</param>
    /// <param name="from">Where to start.</param>
    /// <param name="at">The raw index the winner matched at, before stripping; -1 when none matched.</param>
    /// <remarks>
    /// One pass for every entry, stopping where some entry's first character sits: a scan per
    /// entry cost Llama-3's 256 entries 256 passes over text holding none of them. A rejected
    /// <see cref="AddedToken.SingleWord"/> position just moves the pass on to the next one.
    /// </remarks>
    private AddedToken? BestMatch(string text, int from, out int at)
    {
        int position = from;
        while (position < text.Length)
        {
#if NET8_0_OR_GREATER
            int offset = text.AsSpan(position).IndexOfAny(_firstChars);
            int found = offset < 0 ? -1 : position + offset;
#else
            int found = text.IndexOfAny(_firstChars, position);
#endif
            if (found < 0)
            {
                break;
            }

            AddedToken[] bucket = _byFirstChar[text[found]];
            for (int c = 0; c < bucket.Length; c++)
            {
                if (Matches(text, found, bucket[c]))
                {
                    at = found;
                    return bucket[c];
                }
            }
            position = found + 1;
        }

        at = -1;
        return null;
    }

    /// <summary>Whether <paramref name="candidate"/> matches at <paramref name="found"/>, word boundaries included.</summary>
    private static bool Matches(string text, int found, AddedToken candidate)
    {
        string content = candidate.Content;
        return found + content.Length <= text.Length
            && text.AsSpan(found, content.Length).SequenceEqual(content.AsSpan())
            && (!candidate.SingleWord || IsWholeWord(text, found, found + content.Length));
    }

    private static bool IsWholeWord(string text, int start, int end) =>
        (start == 0 || !IsWordCharacter(text[start - 1]))
        && (end == text.Length || !IsWordCharacter(text[end]));

    private static bool IsWordCharacter(char c) => char.IsLetterOrDigit(c) || c == '_';
}
