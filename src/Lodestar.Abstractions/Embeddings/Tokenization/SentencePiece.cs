namespace Lodestar.Embeddings.Tokenization;

/// <summary>A vocabulary piece: its string, log-probability score and id.</summary>
public readonly record struct SentencePiece(string Piece, double Score, int Id)
{
    /// <summary>Hashes every member as the generated equality compares it, each <c>NaN</c> alike (#1285).</summary>
    public override int GetHashCode()
    {
        unchecked
        {
            int hash = (17 * 31) + ValueEquality.HashOfItem(Piece);
            hash = (hash * 31) + ValueEquality.HashOf(Score);
            return (hash * 31) + Id;
        }
    }
}
