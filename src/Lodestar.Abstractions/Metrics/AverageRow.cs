namespace Lodestar.Metrics;

/// <summary>An averaged line in a <c>ClassificationReport</c>.</summary>
/// <param name="Name">The average's name, as scikit-learn prints it: <c>macro avg</c>, <c>weighted avg</c>, <c>micro avg</c>.</param>
/// <param name="Precision">The averaged precision.</param>
/// <param name="Recall">The averaged recall.</param>
/// <param name="F1">The averaged F1.</param>
/// <param name="Support">The total support the average covers.</param>
public sealed record AverageRow(
    string Name, double Precision, double Recall, double F1, double Support);
