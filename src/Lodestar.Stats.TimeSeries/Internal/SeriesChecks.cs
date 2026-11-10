using System.Globalization;

namespace Lodestar.Stats.TimeSeries.Internal;

/// <summary>The refusals the stationarity tests and the decomposition share.</summary>
internal static class SeriesChecks
{
    /// <summary>2^1023, the largest power of two a double holds.</summary>
    private const double LargestPowerOfTwo = 8.98846567431157953865e307;

    /// <summary>The bias of a double's exponent field.</summary>
    private const int ExponentBias = 1023;

    internal static void RefuseNonFinite(ReadOnlySpan<double> series)
    {
        for (int row = 0; row < series.Length; row++)
        {
            // double.IsFinite is not on netstandard2.0, so the two halves are asked separately.
            if (double.IsNaN(series[row]) || double.IsInfinity(series[row]))
            {
                throw new ArgumentException(
                    $"row {row} carries {series[row].ToString(CultureInfo.InvariantCulture)}, which the "
                    + "reference refuses too: a gapped series needs interpolating first.", nameof(series));
            }
        }
    }

    /// <summary>
    /// The series times the power of two that brings its largest magnitude into <c>[1, 2)</c>, null when that power is
    /// one and the series already is at unit scale, and that power.
    /// </summary>
    /// <remarks>
    /// A power of two scales every value exactly, short of the subnormals, so a statistic invariant to the series' scale
    /// is read at unit scale: no level column far from an intercept's is taken for collinear, and no difference overflows.
    /// </remarks>
    internal static (double[]? Scaled, double Factor) UnitScale(ReadOnlySpan<double> series)
    {
        double largest = 0.0;
        foreach (double value in series)
        {
            largest = Math.Max(largest, Math.Abs(value));
        }

        double factor = UnitFactor(largest);
        // S1244: the factor is a power of two, exactly one or not.
#pragma warning disable S1244
        if (factor == 1.0)
#pragma warning restore S1244
        {
            return (null, factor);
        }

        var scaled = new double[series.Length];
        for (int row = 0; row < series.Length; row++)
        {
            scaled[row] = series[row] * factor;
        }

        return (scaled, factor);
    }

    /// <summary>The power of two that brings <paramref name="largest"/> into <c>[1, 2)</c>, read off its exponent field.</summary>
    /// <remarks>A subnormal stops short of unit scale at 2^1023, the largest power a double holds.</remarks>
    private static double UnitFactor(double largest)
    {
        int field = (int)((BitConverter.DoubleToInt64Bits(largest) >> 52) & 0x7FF);
        if (field == 0)
        {
            return largest > 0.0 ? LargestPowerOfTwo : 1.0;
        }

        // largest lies in [2^e, 2^(e+1)) for e = field − 1023, so the factor is 2^−e; 2^−1023 alone is subnormal.
        int exponent = ExponentBias - (field - ExponentBias);
        return exponent >= 1
            ? BitConverter.Int64BitsToDouble((long)exponent << 52)
            : BitConverter.Int64BitsToDouble(1L << 51);
    }

    internal static void RefuseConstant(ReadOnlySpan<double> series)
    {
        if (series.Length < 2)
        {
            throw new ArgumentException(
                $"a series of {series.Length} cannot be tested: two points are the minimum.", nameof(series));
        }

        // S1244: constant means exactly constant; a series that merely varies little is testable.
#pragma warning disable S1244
        for (int row = 1; row < series.Length; row++)
        {
            if (series[row] != series[0])
            {
                return;
            }
        }
#pragma warning restore S1244

        throw new ArgumentException(
            $"every value is {series[0].ToString(CultureInfo.InvariantCulture)}: a constant series has no "
            + "variance to test.", nameof(series));
    }

    /// <summary>Machine epsilon for <see cref="double"/>, which <see cref="double.Epsilon"/> is not.</summary>
    private const double MachineEpsilon = 2.220446049250313e-16;

    /// <summary>How many ulps of the largest observation a stored line's second difference may reach.</summary>
    /// <remarks>
    /// Four times the worst of 200 random <c>a + b·i</c> per length, which measured 1.4 to 2.2 ulps from
    /// 30 points to a million: a second difference reads three neighbours and sums nothing, so its floor
    /// does not grow with the series the way the line fit's residuals do (#1080).
    /// </remarks>
    private const double SecondDifferenceUlps = 8.0;

    /// <summary>Refuses a series that lies on one straight line, which leaves a regression only rounding.</summary>
    /// <param name="series">The observations, in time order.</param>
    /// <param name="lineResiduals">The same series less its least-squares line, from <see cref="LineFit.Residuals"/>.</param>
    /// <param name="consequence">What a line costs this caller, completing the message after the colon.</param>
    /// <remarks>
    /// A line has to fail both tests. The residual root-mean-square against <c>n·ε</c> of the largest
    /// observation is the global one and cannot be tightened, because the line fit sums naively and a
    /// perfect line already leaves 1.05e4 ulps at a million points. The largest second difference against
    /// eight ulps is the local one and cannot stand alone, because second differences of <c>δ</c> still
    /// allow a bend of <c>δ·n²/8</c>. Both are written <c>&lt;=</c>, so the NaN residuals an overflowing
    /// fit leaves are answered rather than reported as a line (#1080).
    /// </remarks>
    /// <exception cref="ArgumentException"><paramref name="series"/> lies on one straight line.</exception>
    internal static void RefuseStraightLine(
        ReadOnlySpan<double> series, double[] lineResiduals, string consequence)
    {
        double scale = 0.0;
        foreach (double value in series)
        {
            scale = Math.Max(scale, Math.Abs(value));
        }

        double sumOfSquares = 0.0;
        foreach (double residual in lineResiduals)
        {
            sumOfSquares += residual * residual;
        }

        double largestBend = 0.0;
        for (int row = 2; row < series.Length; row++)
        {
            largestBend = Math.Max(largestBend, Math.Abs(series[row] - (2.0 * series[row - 1]) + series[row - 2]));
        }

        if (Math.Sqrt(sumOfSquares / lineResiduals.Length) <= lineResiduals.Length * MachineEpsilon * scale
            && largestBend <= SecondDifferenceUlps * MachineEpsilon * scale)
        {
            throw new ArgumentException($"every value lies on one straight line: {consequence}", nameof(series));
        }
    }
}
