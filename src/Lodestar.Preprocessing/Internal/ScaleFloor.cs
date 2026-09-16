namespace Lodestar.Preprocessing.Internal;

/// <summary>What <c>sklearn.preprocessing._data._handle_zeros_in_scale</c> does to a scale nobody can divide by.</summary>
internal static class ScaleFloor
{
    // long-comment: the threshold is the reference's and is not `scale == 0`, which is the whole
    // reason this is a named helper. With no constant_mask given -- the case for all three scalers
    // here, StandardScaler being the one that passes one -- scikit-learn replaces every scale below
    // 10 * eps by 1. Measured against 1.9.0: a feature whose range is 1e-16 scales by 1, not 1e16.
    private const double NearConstant = 10.0 * 2.220446049250313e-16;

    /// <summary>Replaces each near-constant scale by 1, in place.</summary>
    public static void Apply(double[] scale)
    {
        for (int feature = 0; feature < scale.Length; feature++)
        {
            if (scale[feature] < NearConstant)
            {
                scale[feature] = 1.0;
            }
        }
    }
}
