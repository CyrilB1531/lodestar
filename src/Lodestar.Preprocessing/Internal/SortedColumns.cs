namespace Lodestar.Preprocessing.Internal;

/// <summary>A feature's values sorted, as the percentiles and medians the scalers read need them.</summary>
/// <remarks>
/// The percentiles themselves are numpy's to the last bit — <c>NumpyGrid.Percentile</c>,
/// <c>NumpyGrid.AveragedInvertedCdf</c>, <c>NumpyGrid.Median</c> and <c>NumpyGrid.MaskedMedian</c>, shared with <c>Lodestar.Metrics</c>:
/// the textbook interpolation <c>Percentile</c> held put <c>[-1e308, 1e308]</c>'s median at infinity where numpy's
/// is 0, and moved edges an ulp off numpy's (#1230, #1237).
/// </remarks>
internal static class SortedColumns
{
    /// <summary>One feature's values, ascending, as a scaler reads them out of a row-major matrix.</summary>
    public static double[] Of(ReadOnlySpan<double> samples, int featureCount, int feature, int sampleCount)
    {
        var column = new double[sampleCount];
        for (int row = 0; row < sampleCount; row++)
        {
            column[row] = samples[(row * featureCount) + feature];
        }

        Array.Sort(column);
        return column;
    }
}
