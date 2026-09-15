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
}
