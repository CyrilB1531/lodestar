namespace Lodestar.Embeddings.Tokenization;

/// <summary>The result of tokenizing a piece of text: the sub-word tokens and their vocabulary ids.</summary>
public sealed record TokenizationResult(IReadOnlyList<string> Tokens, IReadOnlyList<int> Ids)
{
    /// <summary>Compares the tokens and ids element by element.</summary>
    /// <remarks>
    /// The generated equality would compare <see cref="Tokens"/> and <see cref="Ids"/>
    /// by reference, so two results holding the same tokens would be unequal — in
    /// the one place a caller has every reason to compare: asserting an encoding
    /// against the result written out by hand.
    /// </remarks>
    public bool Equals(TokenizationResult? other)
    {
        if (ReferenceEquals(this, other))
        {
            return true;
        }
        if (other is null || Tokens.Count != other.Tokens.Count || Ids.Count != other.Ids.Count)
        {
            return false;
        }
        for (int i = 0; i < Tokens.Count; i++)
        {
            if (!string.Equals(Tokens[i], other.Tokens[i], StringComparison.Ordinal))
            {
                return false;
            }
        }
        for (int i = 0; i < Ids.Count; i++)
        {
            if (Ids[i] != other.Ids[i])
            {
                return false;
            }
        }
        return true;
    }

    /// <summary>Hashes the lengths only, which is O(1) and still consistent with equality.</summary>
    /// <remarks>
    /// Equal results necessarily agree on both counts; unequal ones are allowed to
    /// share a hash. Hashing every token would make the cheap operation the
    /// expensive one on a long encoding.
    /// </remarks>
    public override int GetHashCode()
    {
        unchecked
        {
            return (17 * 31 + Tokens.Count) * 31 + Ids.Count;
        }
    }
}

// CA1308: this is the `lowercase` option, HuggingFace's do_lower_case. ToUpperInvariant
// would match no vocabulary entry, giving wrong ids rather than differently-cased tokens.
#pragma warning disable CA1308

/// <summary>WordPiece tokenizer (BERT family), reproducing HuggingFace <c>tokenizers</c>' greedy longest-match algorithm.</summary>
/// <remarks>
/// Pre-tokenization splits on whitespace and isolates punctuation (HuggingFace
/// <c>Whitespace</c> pre-tokenizer, <c>\w+|[^\w\s]+</c> as Oniguruma reads it); each resulting word
/// is then greedily matched against the vocabulary, with <c>##</c>-prefixed
/// continuation pieces -- <c>docs/equivalence.md</c>'s <c>WordPiece(vocab)</c> row.
/// The <c>added_tokens</c> scan runs ahead of all that; see <see cref="Encode"/>.
/// Thread-safe after construction.
/// </remarks>
public sealed class WordPieceTokenizer : ISubwordTokenizer
{
    // The vocabulary's keys and ids, indexed by the value the trie holds for each key.
    private readonly CharTrie _trie;
    private readonly string[] _keys;
    private readonly int[] _ids;

    // Where every continuation piece's walk starts: the node the prefix reaches, or -1
    // when no key begins with it and no continuation can ever match.
    private readonly int _continuationNode;
    private readonly AddedToken[] _addedTokens;
    private readonly AddedTokenScanner _rawScanner;
    private readonly AddedTokenScanner _normalizedScanner;
    private readonly string _unkToken;
    private readonly int _unkId;
    private readonly int _maxCharsPerWord;
    private readonly bool _lowercase;
    private readonly bool _basic;

    /// <summary>Creates a tokenizer from an in-memory vocabulary.</summary>
    /// <param name="vocab">Map from token string to id.</param>
    /// <param name="unkToken">The unknown-token string (must be present in <paramref name="vocab"/>).</param>
    /// <param name="continuationPrefix">Prefix marking non-initial word pieces (default <c>##</c>).</param>
    /// <param name="maxCharsPerWord">Words longer than this become a single unknown token.</param>
    /// <param name="lowercase">Lowercase the text before tokenizing.</param>
    public WordPieceTokenizer(
        IReadOnlyDictionary<string, int> vocab,
        string unkToken = "[UNK]",
        string continuationPrefix = "##",
        int maxCharsPerWord = 100,
        bool lowercase = false)
        : this(vocab, unkToken, continuationPrefix, maxCharsPerWord, lowercase, false, [])
    {
    }

    /// <summary>Creates a tokenizer from a loaded vocabulary.</summary>
    /// <remarks>
    /// Matches <c>tokenizers.Tokenizer.from_file("tokenizer.json")</c> (or a
    /// <c>WordPiece</c> model built from a <c>vocab.txt</c>) followed by
    /// <c>encode</c>. The unknown token, the continuation prefix, the lowercasing
    /// flag and the <c>added_tokens</c> table come from the file rather than from
    /// the caller's memory of how the model was trained.
    /// </remarks>
    /// <param name="vocabulary">A vocabulary from <see cref="Persistence.VocabTxtLoader"/> or <see cref="Persistence.TokenizerJsonLoader"/>.</param>
    /// <param name="maxCharsPerWord">Words longer than this become a single unknown token.</param>
    public WordPieceTokenizer(WordPieceVocabulary vocabulary, int maxCharsPerWord = 100)
        : this(
            Checked(vocabulary).Vocab,
            vocabulary.UnkToken,
            vocabulary.ContinuationPrefix,
            maxCharsPerWord,
            vocabulary.Lowercase,
            vocabulary.BasicTokenization,
            vocabulary.AddedTokens)
    {
    }

    /// <summary>The one constructor; the two public ones differ only in where the added tokens come from.</summary>
    private WordPieceTokenizer(
        IReadOnlyDictionary<string, int> vocab,
        string unkToken,
        string continuationPrefix,
        int maxCharsPerWord,
        bool lowercase,
        bool basic,
        IReadOnlyList<AddedToken> addedTokens)
    {
        Guard.NotNull(vocab);
        if (!vocab.TryGetValue(unkToken, out int unkId))
        {
            throw new ArgumentException($"The unknown token '{unkToken}' is not in the vocabulary.", nameof(unkToken));
        }

        _keys = new string[vocab.Count];
        _ids = new int[vocab.Count];
        int at = 0;
        foreach (KeyValuePair<string, int> entry in vocab)
        {
            _keys[at] = entry.Key;
            _ids[at] = entry.Value;
            at++;
        }
        _trie = new CharTrie(_keys);
        _continuationNode = _trie.Walk(CharTrie.Root, continuationPrefix.AsSpan());
        _unkToken = unkToken;
        _unkId = unkId;
        _maxCharsPerWord = maxCharsPerWord;
        _lowercase = lowercase;
        _basic = basic;

        // Two scanners: the two halves of added_tokens are matched against different
        // strings, split by AddedToken.Normalized (Special plays no part). See Encode.
        _addedTokens = [.. addedTokens];
        _rawScanner = new AddedTokenScanner([.. addedTokens.Where(t => !t.Normalized)]);
        _normalizedScanner = new AddedTokenScanner(
            [.. addedTokens.Where(t => t.Normalized).Select(t => t with { Content = NormalizeContent(t.Content, lowercase, basic) })]);
    }

    /// <summary>Tokenizes <paramref name="text"/> into sub-word tokens and their ids.</summary>
    /// <remarks>
    /// The <c>added_tokens</c> table is matched first, by
    /// <see cref="AddedToken.Normalized"/> rather than <see cref="AddedToken.Special"/> --
    /// <c>docs/equivalence.md</c>'s <c>WordPiece(vocab)</c> row covers which pass an
    /// entry runs in and which string it is matched against. "Added tokens are matched
    /// before normalization" is the natural summary and the wrong one -- measured
    /// (<c>wordpiece_added_tokens.json</c>), raw stays case-sensitive, normalized does not.
    /// </remarks>
    /// <param name="text">The text to tokenize.</param>
    public TokenizationResult Encode(string text)
    {
        Guard.NotNull(text);
        var tokens = new List<string>();
        var ids = new List<int>();

        // Raw-text positions hold because ToLowerInvariant keeps length; BERT's normalizer does not,
        // so under it EncodeGap normalizes each gap on its own, as BpeTokenizer's forms do.
        string normalized = _lowercase && !_basic ? text.ToLowerInvariant() : text;

        int pos = 0;
        while (pos < text.Length)
        {
            if (!_rawScanner.TryNext(text, pos, out int start, out int end, out var raw))
            {
                EncodeGap(normalized, pos, normalized.Length, tokens, ids);
                break;
            }
            if (start > pos)
            {
                EncodeGap(normalized, pos, start, tokens, ids);
            }
            // The raw slice, not the entry's content: a match that stripped
            // whitespace consumed that whitespace into the token it emits.
            tokens.Add(text.Substring(start, end - start));
            ids.Add(raw.Id);
            pos = end;
        }
        return new TokenizationResult(tokens, ids);
    }

    /// <summary>Tokenizes <paramref name="text"/> and returns only the token ids.</summary>
    public IReadOnlyList<int> EncodeToIds(string text) => Encode(text).Ids;

    /// <summary>Looks up a literal vocabulary entry, added tokens included.</summary>
    /// <remarks>
    /// Matches <c>tokenizers.Tokenizer.token_to_id(token)</c>. The lookup is exact
    /// and case-sensitive — <c>[CLS]</c> is a vocabulary entry, not text, so the
    /// lowercasing flag deliberately does not apply, and an added token is looked up
    /// under the content the file wrote even when what <see cref="Encode"/> matches
    /// is its normalized form. That is what <c>tokenizers</c> own <c>get_vocab()</c>
    /// reports.
    /// </remarks>
    /// <param name="token">The token string.</param>
    /// <param name="id">Receives the id when the token is present.</param>
    public bool TryGetId(string token, out int id)
    {
        Guard.NotNull(token);
        int key = _trie.Find(token.AsSpan());
        if (key >= 0)
        {
            id = _ids[key];
            return true;
        }
        // A scan, not a second dictionary: added_tokens tables are tiny (Llama-3's
        // 256 is the largest in sight), so copying the vocabulary to hold them would not be.
        AddedToken? added = Array.Find(_addedTokens, t => string.Equals(t.Content, token, StringComparison.Ordinal));
        if (added is null)
        {
            id = 0;
            return false;
        }
        id = added.Id;
        return true;
    }

    /// <summary>Null-checks a vocabulary before its members are read in a constructor initializer.</summary>
    private static WordPieceVocabulary Checked(WordPieceVocabulary vocabulary)
    {
        Guard.NotNull(vocabulary);
        return vocabulary;
    }

    /// <summary>Encodes <c>text[from..to]</c> -- what no raw-matched added token claimed -- normalizing it under BERT, scanning it for normalized added tokens, and handing the rest to the model.</summary>
    /// <remarks>
    /// The gap is scanned as its own string, matching HuggingFace's
    /// <c>AddedVocabulary</c> (read from its structure, not measured).
    /// <c>wordpiece_added_tokens.json</c>'s
    /// <c>the_raw_pass_wins_over_a_normalized_match_further_left</c> cuts a gap at a
    /// word character; no committed case puts a <see cref="AddedToken.SingleWord"/>
    /// or stripping entry at a gap edge.
    /// </remarks>
    private void EncodeGap(string normalized, int from, int to, List<string> tokens, List<int> ids)
    {
        if (_basic)
        {
            // HuggingFace normalizes what the raw pass left, piece by piece, then scans it for normalized tokens.
            string piece = BertBasicTokenization.Normalize(Slice(normalized, from, to), _lowercase);
            EncodeNormalizedGap(piece, 0, piece.Length, tokens, ids);
            return;
        }

        EncodeNormalizedGap(normalized, from, to, tokens, ids);
    }

    /// <summary>The normalized half of <see cref="EncodeGap"/>: the normalized added tokens, then the model.</summary>
    private void EncodeNormalizedGap(string normalized, int from, int to, List<string> tokens, List<int> ids)
    {
        if (_normalizedScanner.IsEmpty)
        {
            EncodeSegment(normalized, from, to, tokens, ids);
            return;
        }

        string gap = Slice(normalized, from, to);
        int pos = 0;
        while (pos < gap.Length)
        {
            if (!_normalizedScanner.TryNext(gap, pos, out int start, out int end, out var added))
            {
                EncodeSegment(gap, pos, gap.Length, tokens, ids);
                break;
            }
            if (start > pos)
            {
                EncodeSegment(gap, pos, start, tokens, ids);
            }
            tokens.Add(gap.Substring(start, end - start));
            ids.Add(added.Id);
            pos = end;
        }
    }

    /// <summary>Pre-tokenizes and models <c>normalized[start..end]</c>, which holds no added token.</summary>
    /// <remarks>
    /// The slice is already normalized: <see cref="Encode"/> lowercases the whole input once before
    /// the scan, or, under <see cref="WordPieceVocabulary.BasicTokenization"/>, <see cref="EncodeGap"/>
    /// normalized this gap — either way against the string the normalized table is matched in.
    /// </remarks>
    private void EncodeSegment(string normalized, int start, int end, List<string> tokens, List<int> ids)
    {
        if (end <= start)
        {
            return;
        }

        // Words stay spans of the normalized text: a string per word was most of this path's bytes.
        int position = start;
        while (_basic
            ? BertBasicTokenization.TryNext(normalized, end, ref position, out int word)
            : WhitespaceScanner.TryNext(normalized, end, ref position, out word))
        {
            TokenizeWord(normalized.AsSpan(word, position - word), tokens, ids);
        }
    }

    /// <summary>A normalized added token's content, normalized the way the text it is matched in is.</summary>
    private static string NormalizeContent(string content, bool lowercase, bool basic)
    {
        if (basic)
        {
            return BertBasicTokenization.Normalize(content, lowercase);
        }

        return lowercase ? content.ToLowerInvariant() : content;
    }

    /// <summary>The slice, or the string itself when the slice is the whole of it.</summary>
    /// <remarks>
    /// The identity case is the common one — no added token to cut anything out —
    /// and copying the whole input there would be a per-call allocation this
    /// tokenizer did not make before it gained a scan.
    /// </remarks>
    private static string Slice(string text, int start, int end) =>
        start == 0 && end == text.Length ? text : text.Substring(start, end - start);

    /// <summary>The word's length in code points, which is what <c>tokenizers</c> caps (#992).</summary>
    /// <remarks>A surrogate pair counted twice made a 51-character astral word exceed a limit of 100.</remarks>
    private static int CodePointLength(ReadOnlySpan<char> word)
    {
        int length = 0;
        int at = 0;
        while (at < word.Length)
        {
            bool pair = at + 1 < word.Length && char.IsHighSurrogate(word[at]) && char.IsLowSurrogate(word[at + 1]);
            at += pair ? 2 : 1;
            length++;
        }

        return length;
    }

    private void TokenizeWord(ReadOnlySpan<char> word, List<string> tokens, List<int> ids)
    {
        if (CodePointLength(word) > _maxCharsPerWord)
        {
            tokens.Add(_unkToken);
            ids.Add(_unkId);
            return;
        }

        // Appended and rolled back rather than staged in two lists per word: WordPiece is
        // all-or-nothing per word, so the rollback is the staging and costs a Count (#498).
        int tokenMark = tokens.Count;
        int idMark = ids.Count;
        int start = 0;
        bool bad = false;

        while (start < word.Length)
        {
            int found = LongestPieceAt(word, start, out int end);
            if (found < 0)
            {
                bad = true;
                break;
            }

            tokens.Add(_keys[found]);
            ids.Add(_ids[found]);
            start = end;
        }

        if (bad)
        {
            tokens.RemoveRange(tokenMark, tokens.Count - tokenMark);
            ids.RemoveRange(idMark, ids.Count - idMark);
            tokens.Add(_unkToken);
            ids.Add(_unkId);
        }
    }

    /// <summary>The trie value of the longest vocabulary piece starting at <paramref name="start"/>, or <c>-1</c>.</summary>
    /// <remarks>
    /// <c>tokenizers</c>' WordPiece shortens the candidate one character at a time until it
    /// is a key; walking forward and keeping the last key passed answers the same longest
    /// match in one pass, where each shortened candidate was hashed again from its first character.
    /// The token is the key's own string, so a match allocates nothing.
    /// </remarks>
    private int LongestPieceAt(ReadOnlySpan<char> word, int start, out int end)
    {
        int node = start > 0 ? _continuationNode : CharTrie.Root;
        int found = -1;
        end = start;
        for (int i = start; i < word.Length && node >= 0; i++)
        {
            node = _trie.Step(node, word[i]);
            if (node >= 0 && _trie.ValueAt(node) >= 0)
            {
                found = _trie.ValueAt(node);
                end = i + 1;
            }
        }
        return found;
    }
}
