namespace Lodestar.Stats.Regression;

/// <summary>The response distribution a <see cref="GeneralizedLinearModel"/> fits, with the link statsmodels defaults it to.</summary>
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

    /// <summary>A non-negative count whose variance <c>μ + αμ²</c> grows faster than its mean, through the log link.</summary>
    /// <remarks>
    /// <c>α</c> is given rather than estimated, through <see cref="GlmOptions.NegativeBinomialAlpha"/>. The log
    /// link is statsmodels' default for this family rather than its canonical one.
    /// </remarks>
    NegativeBinomial = 2,

    /// <summary>A positive continuous response whose spread grows with its mean, variance <c>φμ²</c>.</summary>
    /// <remarks>
    /// The inverse link by default, as statsmodels', or the log link through <see cref="GlmOptions.Link"/>. The
    /// first family here whose dispersion <c>φ</c> is estimated, as Pearson's χ² over the residual degrees of
    /// freedom, and reported on <see cref="GlmSummary.Dispersion"/>.
    /// </remarks>
    Gamma = 3,
}
