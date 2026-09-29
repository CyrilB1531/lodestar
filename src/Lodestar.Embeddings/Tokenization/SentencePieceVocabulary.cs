namespace Lodestar.Embeddings.Tokenization;

/// <summary>
/// A pretrained SentencePiece unigram vocabulary, as read from a
/// <c>spiece.model</c> or the <c>model</c> section of a <c>tokenizer.json</c>.
/// </summary>
/// <remarks>
/// The piece <em>types</em> are the point of this record: without them a
/// tokenizer has to guess which entries are control markers by id, which fails
/// silently for any model that does not happen to put <c>&lt;unk&gt;</c>,
/// <c>&lt;s&gt;</c> and <c>&lt;/s&gt;</c> at 0, 1 and 2.
/// </remarks>
/// <param name="Pieces">The pieces with their scores, indexed by id.</param>
/// <param name="Types">The type of each piece, aligned with <paramref name="Pieces"/>.</param>
/// <param name="UnkId">Id of the unknown piece.</param>
/// <param name="BosId">Id of the beginning-of-sentence piece, or <c>-1</c> if the model has none.</param>
/// <param name="EosId">Id of the end-of-sentence piece, or <c>-1</c> if the model has none.</param>
/// <param name="PadId">Id of the padding piece, or <c>-1</c> if the model has none.</param>
public sealed record SentencePieceVocabulary(
    IReadOnlyList<SentencePiece> Pieces,
    IReadOnlyList<SentencePieceType> Types,
    int UnkId,
    int BosId,
    int EosId,
    int PadId)
{
    /// <summary>Number of pieces in the vocabulary.</summary>
    public int Count => Pieces?.Count ?? 0;

    /// <summary>
    /// The normalization the model carries in its <c>precompiled_charsmap</c>, or
    /// <c>null</c> for the <c>identity</c> normalizer.
    /// </summary>
    /// <remarks>
    /// Not a constructor parameter: a vocabulary built by hand has no charsmap,
    /// and <c>null</c> — meaning "the text reaches the tokenizer as it was written"
    /// — is the only sound default. <see cref="Persistence.SentencePieceModelLoader"/>
    /// sets it from the file.
    /// </remarks>
    public PrecompiledNormalizer? Normalizer { get; init; }

    /// <summary>Whether runs of whitespace collapse to one and the ends are trimmed before the pieces are matched.</summary>
    /// <remarks>
    /// sentencepiece's <c>remove_extra_whitespaces</c>, which a <c>.model</c> always sets, hence the default.
    /// Off, the escape is <c>tokenizers</c>' <c>Metaspace</c> whole: every space kept, and no second meta symbol
    /// before text that already starts with one. A <c>tokenizer.json</c> Unigram's loader sets it off (#1259).
    /// </remarks>
    public bool RemoveExtraWhitespaces { get; init; } = true;

    /// <summary>Whether the escaped text is split before each meta symbol, so that no piece spans one.</summary>
    /// <remarks>
    /// <c>tokenizers</c>' <c>Metaspace</c> pre-tokenizer with <c>split</c>, which a <c>tokenizer.json</c> Unigram
    /// always declares here; sentencepiece's <c>.model</c> path splits at nothing (#1259).
    /// </remarks>
    public bool SplitsAtMetaSymbol { get; init; }

    /// <summary>
    /// The tokens the file's <c>post_processor</c> puts before the text, in that order,
    /// empty when it declares none.
    /// </summary>
    /// <remarks>
    /// Read by <see cref="Persistence.TokenizerJsonLoader"/> from a <c>TemplateProcessing</c>, <c>BertProcessing</c>
    /// or <c>RobertaProcessing</c>: <c>[&quot;&lt;s&gt;&quot;]</c> for a RoBERTa-shaped one. Public because the caller composes the
    /// <see cref="SpecialTokenTemplate"/>, which also needs a pad token the vocabulary does not carry (#1210).
    /// </remarks>
    public IReadOnlyList<string> PrefixTokens { get; init; } = [];

    /// <summary>
    /// The tokens the file's <c>post_processor</c> puts after the text, in that order,
    /// empty when it declares none.
    /// </summary>
    /// <remarks>See <see cref="PrefixTokens"/>.</remarks>
    public IReadOnlyList<string> SuffixTokens { get; init; } = [];

    /// <summary>Compares the special-token ids, the normalizer and the post-processor templates, then every piece and every type.</summary>
    /// <param name="other">The vocabulary to compare against.</param>
    /// <remarks>
    /// The generated equality would compare <see cref="Pieces"/> and
    /// <see cref="Types"/> by reference, so two vocabularies read from the same
    /// <c>spiece.model</c> would be unequal.
    /// </remarks>
    public bool Equals(SentencePieceVocabulary? other)
    {
        if (ReferenceEquals(this, other))
        {
            return true;
        }
        if (other is null
            || UnkId != other.UnkId || BosId != other.BosId
            || EosId != other.EosId || PadId != other.PadId
            || !ValueEquality.Same(PrefixTokens, other.PrefixTokens)
            || !ValueEquality.Same(SuffixTokens, other.SuffixTokens)
            || !ValueEquality.Same(Pieces, other.Pieces)
            || !Equals(Normalizer, other.Normalizer)
            || RemoveExtraWhitespaces != other.RemoveExtraWhitespaces
            || SplitsAtMetaSymbol != other.SplitsAtMetaSymbol)
        {
            return false;
        }

        // Total on an absent list, as the rest is (#1370); an enum is not IEquatable, so no shared helper.
        if (Types is null || other.Types is null)
        {
            return Types is null && other.Types is null;
        }

        if (Types.Count != other.Types.Count)
        {
            return false;
        }

        for (int i = 0; i < Types.Count; i++)
        {
            if (Types[i] != other.Types[i])
            {
                return false;
            }
        }
        return true;
    }

    /// <summary>Hashes the counts and the special-token ids, which is O(1).</summary>
    /// <remarks>
    /// Equal vocabularies necessarily agree on all of these; unequal ones are
    /// allowed to collide. Hashing a quarter of a million pieces would defeat the
    /// purpose of a hash.
    /// </remarks>
    public override int GetHashCode()
    {
        unchecked
        {
            int hash = (17 * 31) + ValueEquality.LengthOf(Pieces);
            hash = (hash * 31) + ValueEquality.LengthOf(Types);
            hash = (hash * 31) + UnkId;
            hash = (hash * 31) + BosId;
            hash = (hash * 31) + EosId;
            return (hash * 31) + PadId;
        }
    }

    /// <summary>Whether the piece with this id may be matched against input text.</summary>
    /// <remarks>
    /// Control, unknown and unused pieces are excluded, the last keeping its id as in
    /// sentencepiece (#1213); normal, user-defined and byte pieces are not. This is the check that replaces guessing by id.
    /// </remarks>
    /// <param name="id">A piece id, in <c>[0, Types.Count)</c>.</param>
    /// <exception cref="ArgumentOutOfRangeException">
    /// <paramref name="id"/> is outside <see cref="Types"/>, which an absent list declares none of. The record is a data carrier and
    /// does not itself require <see cref="Pieces"/> and <see cref="Types"/> to agree in length —
    /// <see cref="SentencePieceTokenizer"/> is what refuses a vocabulary where they do not.
    /// Reporting that here as a raw index failure would name neither the argument nor the reason.
    /// </exception>
    public bool IsMatchable(int id)
    {
        // An absent list declares no type, as the record's equality reads it, rather than a null dereference (#1440).
        if (Types is not { } types || (uint)id >= (uint)types.Count)
        {
            throw new ArgumentOutOfRangeException(
                nameof(id),
                id,
                $"The piece id is outside the vocabulary's {Types?.Count ?? 0} declared types.");
        }
        return types[id] is not (SentencePieceType.Control or SentencePieceType.Unknown or SentencePieceType.Unused);
    }
}
