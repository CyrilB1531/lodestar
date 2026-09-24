namespace Lodestar.Metrics;

/// <summary>One class's line in a <c>ClassificationReport</c>.</summary>
/// <param name="Label">The label value this line scores.</param>
/// <param name="Name">The readable name supplied through <c>targetNames</c>, or null.</param>
/// <param name="Precision">Precision for this class.</param>
/// <param name="Recall">Recall for this class.</param>
/// <param name="F1">F1 for this class.</param>
/// <param name="Support">The weight of samples whose true label is this class.</param>
public sealed record ClassRow(
    int Label, string? Name, double Precision, double Recall, double F1, double Support);
