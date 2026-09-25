namespace Lodestar.Stats.Regression;

/// <summary>The lag window a kernel covariance weights its autocovariances with: the instrumental-variables and the Driscoll-Kraay panel covariance.</summary>
public enum KernelType
{
    /// <summary>Bartlett's triangle, <c>1 − j/(m+1)</c>: Newey and West's, and the reference's default.</summary>
    Bartlett = 0,

    /// <summary>Parzen's (Gallant's) cubic window, truncated at the bandwidth.</summary>
    Parzen = 1,

    /// <summary>Andrews' quadratic spectral window, which weights every lag in the sample.</summary>
    QuadraticSpectral = 2,
}
