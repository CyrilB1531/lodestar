namespace Lodestar.Metrics;

/// <summary>An averaged line in a <c>ClassificationReport</c>.</summary>
/// <param name="Name">The average's name, as scikit-learn prints it: <c>macro avg</c>, <c>weighted avg</c>, <c>micro avg</c>.</param>
/// <param name="Precision">The averaged precision.</param>
/// <param name="Recall">The averaged recall.</param>
/// <param name="F1">The averaged F1.</param>
/// <param name="Support">The total support the average covers.</param>
public sealed record AverageRow(
    string Name, double Precision, double Recall, double F1, double Support)
{
    /// <summary>Hashes every member as the generated equality compares it, each <c>NaN</c> alike (#1285).</summary>
    public override int GetHashCode()
    {
        unchecked
        {
            int hash = (17 * 31) + ValueEquality.HashOfItem(Name);
            hash = (hash * 31) + ValueEquality.HashOf(Precision);
            hash = (hash * 31) + ValueEquality.HashOf(Recall);
            hash = (hash * 31) + ValueEquality.HashOf(F1);
            return (hash * 31) + ValueEquality.HashOf(Support);
        }
    }
}
