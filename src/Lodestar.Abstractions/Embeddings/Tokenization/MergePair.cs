namespace Lodestar.Embeddings.Tokenization;

/// <summary>One line of a merge table: the two symbols it joins.</summary>
/// <remarks>
/// The pair's <em>index</em> in <c>BpeVocabulary.Merges</c> is its rank,
/// and rank is the whole algorithm — the lowest-ranked applicable merge is
/// always the next one applied. Reordering the list changes the tokenization.
/// </remarks>
/// <param name="Left">The symbol on the left of the join.</param>
/// <param name="Right">The symbol on the right of the join.</param>
public readonly record struct MergePair(string Left, string Right);
