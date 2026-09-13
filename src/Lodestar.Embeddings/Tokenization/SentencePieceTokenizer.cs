using System.Buffers;

namespace Lodestar.Embeddings.Tokenization;

// SonarLint S3776: cognitive complexity: a faithful implementation of a published rule-engine; decomposing it would break the 1:1 mapping with the reference that makes divergences auditable.
#pragma warning disable S3776

/// <summary>A vocabulary piece: its string, log-probability score and id.</summary>
public readonly record struct SentencePiece(string Piece, double Score, int Id);

/// <summary>
/// SentencePiece <em>unigram</em> tokenizer: segments text to maximize the sum of
/// piece log-probabilities, via Viterbi over the vocabulary.
/// </summary>
/// <remarks>
/// Reproduces <c>sentencepiece.SentencePieceProcessor(model_file=…).encode</c>; character
/// map (<see cref="PrecompiledNormalizer"/>) and whitespace flags run first. See
/// <c>docs/equivalence.md</c>'s Unigram rows and <c>docs/guides/embeddings.md</c>'s
/// "Models that are refused" list. Thread-safe after construction.
/// </remarks>
public sealed class SentencePieceTokenizer : ISubwordTokenizer
{
    /// <summary>add_dummy_prefix and escape_whitespaces, the pair this path always applies.</summary>
    /// <remarks>
    /// Shared with the BPE path rather than spelled out twice (decision 0050 §2), which is
    /// what makes the two answer alike. <c>remove_extra_whitespaces</c> is the flag that
    /// separates them: set here, and off for the SentencePiece-BPE lineage, which declares
    /// no normalizer to collapse runs with.
    /// </remarks>
    private static readonly MetaspaceEscape Escape =
        new('▁', MetaspacePrependScheme.Always, removeExtraWhitespaces: true, skipPrependWhenAlreadyPrefixed: false);

    // The matchable pieces, each array indexed by the value the trie holds for the piece.
    private readonly CharTrie _trie;
    private readonly string[] _pieceStrings;
    private readonly double[] _scores;
    private readonly int[] _ids;
    private readonly PrecompiledNormalizer? _normalizer;

    // Control/unknown pieces stay out of the trie so they never match text; a
    // special-token template still needs their ids, so only those few are duplicated.
    private readonly Dictionary<string, int> _nonMatchableIds;
    private readonly int _unkId;
    private readonly double _unkScore;

    /// <summary>
    /// Creates a tokenizer from a loaded vocabulary, using each piece's declared
    /// type to decide what may match text.
    /// </summary>
    /// <remarks>
    /// Matches <c>sentencepiece.SentencePieceProcessor(model_file=…)</c> followed by
    /// <c>encode</c>. Control and unknown pieces are excluded because
    /// <see cref="SentencePieceVocabulary.Types"/> says so, not because of id position --
    /// see <c>docs/equivalence.md</c>'s <c>sp.IsControl(i)</c> row.
    /// </remarks>
    /// <param name="vocabulary">A vocabulary from <see cref="Persistence.SentencePieceModelLoader"/> or <see cref="Persistence.TokenizerJsonLoader"/>.</param>
    /// <exception cref="ArgumentException">The vocabulary's pieces and types disagree in length, or its unknown id is out of range.</exception>
    public SentencePieceTokenizer(SentencePieceVocabulary vocabulary)
    {
        Guard.NotNull(vocabulary);
        if (vocabulary.Pieces.Count != vocabulary.Types.Count)
        {
            throw new ArgumentException(
                $"The vocabulary has {vocabulary.Pieces.Count} pieces but {vocabulary.Types.Count} types.",
                nameof(vocabulary));
        }
        if (vocabulary.UnkId < 0 || vocabulary.UnkId >= vocabulary.Pieces.Count)
        {
            throw new ArgumentException(
                $"The unknown id {vocabulary.UnkId} is outside the vocabulary range [0, {vocabulary.Pieces.Count}).",
                nameof(vocabulary));
        }

        var matchable = new List<SentencePiece>(vocabulary.Count);
        _nonMatchableIds = new Dictionary<string, int>(StringComparer.Ordinal);
        double minScore = 0;
        for (int id = 0; id < vocabulary.Count; id++)
        {
            if (!vocabulary.IsMatchable(id))
            {
                _nonMatchableIds[vocabulary.Pieces[id].Piece] = id;
                continue;
            }
            SentencePiece p = vocabulary.Pieces[id];
            matchable.Add(p);
            minScore = Math.Min(minScore, p.Score);
        }

        _pieceStrings = new string[matchable.Count];
        _scores = new double[matchable.Count];
        _ids = new int[matchable.Count];
        for (int i = 0; i < matchable.Count; i++)
        {
            _pieceStrings[i] = matchable[i].Piece;
            _scores[i] = matchable[i].Score;
            _ids[i] = matchable[i].Id;
        }
        _trie = new CharTrie(_pieceStrings);
        _normalizer = vocabulary.Normalizer;
        _unkId = vocabulary.UnkId;
        _unkScore = minScore - 10.0; // heavy penalty; only used for uncovered characters
    }

    /// <summary>Tokenizes <paramref name="text"/> into unigram pieces and their ids.</summary>
    public TokenizationResult Encode(string text)
    {
        Guard.NotNull(text);
        string s = Preprocess(text);
        if (s.Length == 0)
        {
            return new TokenizationResult([], []);
        }

        int n = s.Length;

        // Rented: three per-call arrays were the rest of #498's bytes. A rental may be
        // longer than asked, so every loop below is bounded by n rather than by Length.
        double[] best = ArrayPool<double>.Shared.Rent(n + 1);
        int[] startAt = ArrayPool<int>.Shared.Rent(n + 1);
        int[] pieceAt = ArrayPool<int>.Shared.Rent(n + 1);
        try
        {
            best.AsSpan(0, n + 1).Fill(double.NegativeInfinity);
            best[0] = 0;

            for (int i = 0; i < n; i++)
            {
                double from = best[i];
                if (double.IsNegativeInfinity(from))
                {
                    continue;
                }

                // One walk finds every piece starting at i, shortest first -- the order the
                // strict > below needs for a tie to keep the earlier path, as sentencepiece's does.
                bool matchedSingle = false;
                int node = CharTrie.Root;
                for (int end = i; end < n; end++)
                {
                    node = _trie.Step(node, s[end]);
                    if (node < 0)
                    {
                        break;
                    }
                    int piece = _trie.ValueAt(node);
                    if (piece < 0)
                    {
                        continue;
                    }
                    matchedSingle |= end == i;
                    double cand = from + _scores[piece];
                    if (cand > best[end + 1])
                    {
                        best[end + 1] = cand;
                        startAt[end + 1] = i;
                        pieceAt[end + 1] = piece;
                    }
                }

                // Uncovered single character -> unknown piece.
                if (!matchedSingle)
                {
                    double cand = from + _unkScore;
                    if (cand > best[i + 1])
                    {
                        best[i + 1] = cand;
                        startAt[i + 1] = i;
                        pieceAt[i + 1] = -1;
                    }
                }
            }

            return Backtrack(s, startAt, pieceAt);
        }
        finally
        {
            ArrayPool<double>.Shared.Return(best);
            ArrayPool<int>.Shared.Return(startAt);
            ArrayPool<int>.Shared.Return(pieceAt);
        }
    }

    /// <summary>Looks up a literal vocabulary piece, control markers included.</summary>
    /// <remarks>
    /// Matches <c>sentencepiece.SentencePieceProcessor.piece_to_id(piece)</c>. The
    /// control pieces a template names — <c>&lt;s&gt;</c>, <c>&lt;/s&gt;</c>,
    /// <c>&lt;pad&gt;</c> — resolve here even though they can never match text.
    /// </remarks>
    /// <param name="token">The piece string.</param>
    /// <param name="id">Receives the id when the piece is present.</param>
    public bool TryGetId(string token, out int id)
    {
        Guard.NotNull(token);
        int piece = _trie.Find(token.AsSpan());
        if (piece >= 0)
        {
            id = _ids[piece];
            return true;
        }
        return _nonMatchableIds.TryGetValue(token, out id);
    }

    /// <summary>Reads the best path back from the end, one unknown piece per run of uncovered characters.</summary>
    /// <remarks>
    /// Right to left, so a run fuses easily: the piece already emitted sits to the right of
    /// the one now emitted. A matched piece's token is the vocabulary's own string, equal to
    /// the slice it covers, so only an unknown run is copied out of the text.
    /// </remarks>
    private TokenizationResult Backtrack(string s, int[] startAt, int[] pieceAt)
    {
        // Counted first, so both arrays are filled from the right in place rather than
        // grown and reversed.
        int count = 0;
        bool inRun = false;
        for (int j = s.Length; j > 0; j = startAt[j])
        {
            bool unknown = IdAt(pieceAt[j]) == _unkId;
            count += unknown && inRun ? 0 : 1;
            inRun = unknown;
        }

        int[] ids = new int[count];
        string[] tokens = new string[count];
        int at = count;
        int runEnd = -1;
        for (int j = s.Length; j > 0;)
        {
            int i = startAt[j];
            int piece = pieceAt[j];
            int id = IdAt(piece);
            if (id == _unkId && runEnd >= 0)
            {
                // One unknown piece per run of uncovered characters -- docs/equivalence.md's
                // Unigram row. Rewriting from the run's start keeps this to one substring per step.
                tokens[at] = s.Substring(i, runEnd - i);
            }
            else
            {
                at--;
                ids[at] = id;
                tokens[at] = piece < 0 ? s.Substring(i, j - i) : _pieceStrings[piece];
                runEnd = id == _unkId ? j : -1;
            }
            j = i;
        }
        return new TokenizationResult(tokens, ids);
    }

    private int IdAt(int piece) => piece < 0 ? _unkId : _ids[piece];

    private string Preprocess(string text)
    {
        // The model's own normalization first -- it turns a tab, a non-breaking space
        // or an ideographic space into an ordinary space, among what else it rewrites.
        string normalized = _normalizer is null ? text : _normalizer.Normalize(text);

        // One text, one piece here: the unigram path splits at nothing, and its scheme
        // is Always, which prepends to every piece anyway.
        return Escape.Apply(normalized, isFirstSplit: true);
    }
}
