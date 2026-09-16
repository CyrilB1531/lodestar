namespace Lodestar.Preprocessing.Internal;

/// <summary>Clamping, which <c>netstandard2.0</c> has no <c>Math.Clamp</c> for.</summary>
internal static class Bounds
{
    /// <summary>The value pulled into <c>[low, high]</c>.</summary>
    public static double Clamp(double value, double low, double high)
    {
        if (value < low)
        {
            return low;
        }

        return value > high ? high : value;
    }
}
