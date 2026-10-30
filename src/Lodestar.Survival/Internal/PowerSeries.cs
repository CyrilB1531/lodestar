namespace Lodestar.Survival.Internal;

/// <summary>A power series with its first two derivatives, for the jets the likelihoods are written in.</summary>
internal static class PowerSeries
{
    /// <summary>The series' value at <paramref name="x"/>, by Horner's rule: <see cref="Curvature"/>'s value, alone.</summary>
    internal static double Value(ReadOnlySpan<double> coefficients, double x)
    {
        double value = 0.0;
        for (int j = coefficients.Length - 1; j >= 0; j--)
        {
            value = (value * x) + coefficients[j];
        }
        return value;
    }

    /// <summary>The series' value and its first two derivatives at <paramref name="x"/>, by Horner's rule.</summary>
    internal static (double Value, double First, double Second) Curvature(ReadOnlySpan<double> coefficients, double x)
    {
        double value = 0.0;
        double first = 0.0;
        double second = 0.0;
        for (int j = coefficients.Length - 1; j >= 0; j--)
        {
            second = (second * x) + (2.0 * first);
            first = (first * x) + value;
            value = (value * x) + coefficients[j];
        }
        return (value, first, second);
    }
}
