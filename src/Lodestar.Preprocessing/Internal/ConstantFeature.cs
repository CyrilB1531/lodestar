namespace Lodestar.Preprocessing.Internal;

/// <summary>scikit-learn's <c>_is_constant_feature</c>, which the scalers and the power transform share.</summary>
internal static class ConstantFeature
{
    /// <summary>Whether a feature's variance is within the two-pass error bound of zero.</summary>
    /// <remarks>
    /// Chan, Golub and LeVeque's error bound for the two-pass variance, as
    /// <c>sklearn.preprocessing._data._is_constant_feature</c> applies it, read from the reference rather than
    /// derived. Measured against scikit-learn 1.9.1: three samples at 1e8 +/- 1e-8 have a variance of 1.48e-16,
    /// which is not zero and is below this bound, and the reference scales them by 1.
    /// </remarks>
    public static bool IsConstant(int count, double mean, double variance)
    {
        double shift = count * mean * NearZero.MachineEpsilon;
        return variance <= (count * NearZero.MachineEpsilon * variance) + (shift * shift);
    }
}
