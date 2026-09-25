namespace Lodestar.Stats.Regression.Instrumental;

/// <summary>The lag window a <see cref="IvCovarianceType.Kernel"/> covariance weights its autocovariances with.</summary>
public enum IvKernel
{
    /// <summary>Bartlett's triangle, <c>1 − j/(m+1)</c>: Newey and West's, and the reference's default.</summary>
    Bartlett = 0,

    /// <summary>Parzen's (Gallant's) cubic window, truncated at the bandwidth.</summary>
    Parzen = 1,

    /// <summary>Andrews' quadratic spectral window, which weights every lag in the sample.</summary>
    QuadraticSpectral = 2,
}
