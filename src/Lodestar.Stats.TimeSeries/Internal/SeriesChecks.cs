using System.Globalization;

namespace Lodestar.Stats.TimeSeries.Internal;

/// <summary>The refusals the stationarity tests and the decomposition share.</summary>
internal static class SeriesChecks
{
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
