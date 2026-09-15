namespace Lodestar.Stats.TimeSeries.Internal;

/// <summary>MacKinnon's response surfaces for one series: the 1994 p-value and the 2010 critical values.</summary>
/// <remarks>
/// Row <c>N = 1</c> of statsmodels 0.15.0's <c>tsa/adfvalues.py</c> (BSD-3-Clause, a permitted reference
/// under decision 0003). The published coefficients are kept beside their scaling and multiplied here as
/// the reference multiplies them, so a transcription can be checked against the paper digit by digit.
/// </remarks>
internal static class MacKinnon
{
    private static readonly double[] SmallScaling = [1.0, 1.0, 1e-2];
    private static readonly double[] LargeScaling = [1.0, 1e-1, 1e-1, 1e-2];

    private static readonly Surface NoTrend = new(
        double.PositiveInfinity, -19.04, -1.04,
        Scaled([0.6344, 1.2378, 3.2496], SmallScaling),
        Scaled([0.4797, 9.3557, -0.6999, 3.3066], LargeScaling),
        [[-2.56574, -2.2358, -3.627, 0.0], [-1.941, -0.2686, -3.365, 31.223], [-1.61682, 0.2656, -2.714, 25.364]]);

    private static readonly Surface Level = new(
        2.74, -18.83, -1.61,
        Scaled([2.1659, 1.4412, 3.8269], SmallScaling),
        Scaled([1.7339, 9.3202, -1.2745, -1.0368], LargeScaling),
        [[-3.43035, -6.5393, -16.786, -79.433], [-2.86154, -2.8903, -4.234, -40.04], [-2.56677, -1.5384, -2.809, 0.0]]);

    private static readonly Surface Trend = new(
        0.7, -16.18, -2.89,
        Scaled([3.2512, 1.6047, 4.9588], SmallScaling),
        Scaled([2.5261, 6.1654, -3.7956, -6.0285], LargeScaling),
        [[-3.95877, -9.0531, -28.428, -134.155], [-3.41049, -4.3904, -9.036, -45.374], [-3.12705, -2.5856, -3.925, -22.38]]);

    private static readonly Surface QuadraticTrend = new(
        0.54, -17.17, -3.21,
        Scaled([4.0003, 1.658, 4.8288], SmallScaling),
        Scaled([3.0778, 4.9529, -4.1477, -5.9359], LargeScaling),
        [[-4.37113, -11.5882, -35.819, -334.047], [-3.83239, -5.9057, -12.49, -118.284], [-3.55326, -3.6596, -5.293, -63.559]]);

    /// <summary>MacKinnon's approximate p-value: exactly 1 above the table, exactly 0 below it.</summary>
    internal static double PValue(double statistic, TrendTerms regression)
    {
        Surface surface = For(regression);
        if (statistic > surface.Max)
        {
            return 1.0;
        }

        if (statistic < surface.Min)
        {
            return 0.0;
        }

        double[] coefficients = statistic <= surface.Switch ? surface.Small : surface.Large;
        return NormalCdf(Horner(coefficients, statistic));
    }

    /// <summary>The 1 %, 5 % and 10 % critical values for a regression of <paramref name="observations"/> rows.</summary>
    internal static double[] CriticalValues(int observations, TrendTerms regression)
    {
        Surface surface = For(regression);
        double inverse = 1.0 / observations;
        return [Horner(surface.Critical[0], inverse), Horner(surface.Critical[1], inverse), Horner(surface.Critical[2], inverse)];
    }

    /// <summary>The standard normal CDF, through the one tail <c>Lodestar.Stats</c> publishes that reaches it.</summary>
    /// <remarks><c>Φ(z) = ½·P(χ²₁ ≥ z²)</c> below zero, and one minus that above; no normal CDF is published.</remarks>
    internal static double NormalCdf(double z)
    {
        double half = 0.5 * Distributions.ChiSquaredSf(z * z, 1.0);
        return z < 0.0 ? half : 1.0 - half;
    }

    /// <summary><c>c₀ + c₁x + c₂x² + …</c> in numpy's <c>polyval</c> order, highest coefficient first.</summary>
    private static double Horner(double[] coefficients, double x)
    {
        double value = 0.0;
        for (int i = coefficients.Length - 1; i >= 0; i--)
        {
            value = (value * x) + coefficients[i];
        }

        return value;
    }

    private static double[] Scaled(double[] published, double[] scaling)
    {
        var scaled = new double[published.Length];
        for (int i = 0; i < published.Length; i++)
        {
            scaled[i] = published[i] * scaling[i];
        }

        return scaled;
    }

    private static Surface For(TrendTerms regression) => regression switch
    {
        TrendTerms.None => NoTrend,
        TrendTerms.Constant => Level,
        TrendTerms.ConstantAndTrend => Trend,
        TrendTerms.ConstantAndQuadraticTrend => QuadraticTrend,
        _ => throw new ArgumentOutOfRangeException(nameof(regression), regression, "Not a trend specification."),
    };

    private sealed record Surface(double Max, double Min, double Switch, double[] Small, double[] Large, double[][] Critical);
}
