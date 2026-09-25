namespace Lodestar.Stats.Regression.Instrumental;

/// <summary>What an instrumental-variables fit estimates, and how it reports it.</summary>
/// <remarks>
/// The defaults are <c>linearmodels</c>' own: a robust covariance, no small-sample scaling, a Bartlett kernel with an
/// automatic bandwidth, and for GMM a robust weight matrix. <see cref="WithIntercept"/> has no counterpart there,
/// where a constant is a column the caller supplies; it adds that column, first. A setting the chosen estimator
/// and covariance do not read is refused by the fit rather than ignored.
/// </remarks>
public sealed record IvOptions
{
    /// <summary>Whether a constant column is prepended to the exogenous regressors; <see langword="true"/> by default.</summary>
    public bool WithIntercept { get; init; } = true;

    /// <summary>The covariance of the coefficients; <see cref="IvCovarianceType.Robust"/> by default.</summary>
    public IvCovarianceType CovarianceType { get; init; } = IvCovarianceType.Robust;

    /// <summary>Whether to scale by <c>n/(n−k)</c> and read the tests against t and F rather than the normal and χ².</summary>
    public bool Debiased { get; init; }

    /// <summary>The kernel of a <see cref="IvCovarianceType.Kernel"/> covariance; Bartlett's by default.</summary>
    public KernelType Kernel { get; init; } = KernelType.Bartlett;

    /// <summary>The kernel covariance's bandwidth; <see langword="null"/>, the default, chooses it by Newey and West's rule.</summary>
    public int? Bandwidth { get; init; }

    /// <summary>The level of the confidence intervals, strictly inside (0, 1); 0.95 by default.</summary>
    public double ConfidenceLevel { get; init; } = 0.95;

    /// <summary>Fuller's <c>α</c> for LIML, which lowers <c>κ</c> by <c>α/(n − L)</c>; 0, plain LIML, by default.</summary>
    public double Fuller { get; init; }

    /// <summary>The weight matrix of GMM's second step; <see cref="IvCovarianceType.Robust"/> by default.</summary>
    public IvCovarianceType GmmWeightType { get; init; } = IvCovarianceType.Robust;

    /// <summary>The kernel of a <see cref="IvCovarianceType.Kernel"/> GMM weight; Bartlett's by default.</summary>
    public KernelType GmmWeightKernel { get; init; } = KernelType.Bartlett;

    /// <summary>A kernel GMM weight's bandwidth; <see langword="null"/>, the default, is <c>n − 2</c>, as the reference's.</summary>
    public int? GmmWeightBandwidth { get; init; }
}
