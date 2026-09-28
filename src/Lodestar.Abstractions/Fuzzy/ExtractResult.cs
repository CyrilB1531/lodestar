namespace Lodestar.Fuzzy;

/// <summary>A single extraction hit: the matched choice, its score and its index in the input.</summary>
public readonly record struct ExtractResult(string Choice, double Score, int Index)
{
    /// <summary>Hashes every member as the generated equality compares it, each <c>NaN</c> alike (#1285).</summary>
    public override int GetHashCode()
    {
        unchecked
        {
            int hash = (17 * 31) + ValueEquality.HashOfItem(Choice);
            hash = (hash * 31) + ValueEquality.HashOf(Score);
            return (hash * 31) + Index;
        }
    }
}
