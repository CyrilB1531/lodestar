namespace Lodestar.Stats.Regression;

/// <summary>The response distribution a <see cref="GeneralizedLinearModel"/> fits, with its canonical link.</summary>
/// <remarks>
/// Closed on purpose. IRLS cannot check that a caller-supplied family is internally consistent,
/// and an incoherent one produces a plausible inference table rather than an error — so a family
/// is chosen from this set rather than described by an interface (#616). Adding a member is not a
/// breaking change, which is what the follow-up families rely on.
/// </remarks>
public enum GlmFamily
{
    /// <summary>A response in <c>{0, 1}</c>, through the logit link.</summary>
    Binomial = 0,

    /// <summary>A non-negative count, through the log link.</summary>
    Poisson = 1,
}
