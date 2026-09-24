namespace Lodestar.Conformal;

/// <summary>Which order statistic <c>SplitConformal.Quantile</c> reads.</summary>
/// <remarks>
/// MAPIE 1.5.0 uses two rules: its regressors the ceiling rank, its <c>SplitConformalClassifier</c>
/// numpy's <c>higher</c> quantile, one rank above at <c>n = 19, alpha = 0.1</c>. <see cref="Ceiling"/>
/// is the zero value, so a <c>default</c> keeps the rule decision 0007 established (#866).
/// </remarks>
public enum ConformalQuantileRule
{
    /// <summary>The <c>k</c>-th smallest score, <c>k = ceil((n + 1) · (1 − alpha))</c>: MAPIE's regressors, and the default.</summary>
    Ceiling,

    /// <summary><c>numpy.quantile(scores, (n + 1) · (1 − alpha) / n, method="higher")</c>: MAPIE's prediction sets.</summary>
    MapieClassification,
}
