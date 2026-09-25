namespace Lodestar.Conformal;

/// <summary>How the predictions of several models that held a sample out are combined into one.</summary>
/// <remarks>
/// MAPIE 1.5.0's <c>aggregation_method</c> of its jackknife-after-bootstrap regressor. Under K-fold and leave-one-out a
/// sample is held out by one model, and both members give its prediction unchanged.
/// </remarks>
public enum CrossConformalAggregation
{
    /// <summary>The mean of the models that held the sample out. The default.</summary>
    Mean,

    /// <summary>Their median, the mean of the two middle predictions when their count is even.</summary>
    Median,
}
