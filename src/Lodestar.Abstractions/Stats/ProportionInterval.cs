namespace Lodestar.Stats;

/// <summary>Which interval a binomial proportion is reported with.</summary>
/// <remarks>
/// scipy's <c>method</c> on <c>BinomTestResult.proportion_ci</c>, default <c>'exact'</c>. The three
/// answer the same question and disagree about small samples, which is the only place the question
/// is hard: Clopper-Pearson is guaranteed to cover at least the level asked for and is wider than
/// it needs to be, where Wilson is close to the level on average and can fall below it.
/// </remarks>
public enum ProportionInterval
{
    /// <summary>Clopper-Pearson, inverted from the binomial tails. scipy's <c>'exact'</c>.</summary>
    Exact,

    /// <summary>Wilson's score interval. scipy's <c>'wilson'</c>.</summary>
    Wilson,

    /// <summary>Wilson's interval with the half-unit continuity correction. scipy's <c>'wilsoncc'</c>.</summary>
    WilsonCorrected,
}
