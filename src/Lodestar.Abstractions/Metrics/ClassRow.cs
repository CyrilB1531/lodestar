namespace Lodestar.Metrics;

/// <summary>One class's line in a <c>ClassificationReport</c>.</summary>
/// <param name="Label">The label value this line scores.</param>
/// <param name="Name">The readable name supplied through <c>targetNames</c>, or null.</param>
/// <param name="Precision">Precision for this class.</param>
/// <param name="Recall">Recall for this class.</param>
/// <param name="F1">F1 for this class.</param>
/// <param name="Support">The weight of samples whose true label is this class.</param>
public sealed record ClassRow(
    int Label, string? Name, double Precision, double Recall, double F1, double Support)
{
    /// <summary>Hashes every member as the generated equality compares it, each <c>NaN</c> alike (#1285).</summary>
    public override int GetHashCode()
    {
        unchecked
        {
            int hash = (17 * 31) + Label;
            hash = (hash * 31) + ValueEquality.HashOfItem(Name);
            hash = (hash * 31) + ValueEquality.HashOf(Precision);
            hash = (hash * 31) + ValueEquality.HashOf(Recall);
            hash = (hash * 31) + ValueEquality.HashOf(F1);
            return (hash * 31) + ValueEquality.HashOf(Support);
        }
    }
}
