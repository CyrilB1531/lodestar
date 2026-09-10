namespace Lodestar.Text.Search;

/// <summary>One document and the score that ranked it.</summary>
/// <param name="Document">The document's row index in the matrix that was scored.</param>
/// <param name="Score">Higher is better. The scale is BM25's and is not comparable across corpora.</param>
public sealed record SearchHit(int Document, double Score);
