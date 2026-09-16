namespace Lodestar.Preprocessing.Internal;

/// <summary>The linearly interpolated percentile <c>numpy.percentile</c> computes by default.</summary>
/// <remarks>
/// Hyndman and Fan's type 7, which is <c>method="linear"</c>: at <c>h = (n − 1)·q/100</c> the value is
/// <c>x[⌊h⌋] + (h − ⌊h⌋)·(x[⌊h⌋+1] − x[⌊h⌋])</c> over the sorted column. Written here rather than reused:
/// <c>SplitConformal.Quantile</c> takes the conformal order statistic and <c>Lodestar.Metrics</c>' weighted
/// percentile averages, so neither is this convention (#763).
/// </remarks>
internal static class Percentile
{
    /// <summary>The percentile of an already sorted, finite column.</summary>
    /// <param name="sorted">The column, ascending.</param>
    /// <param name="percent">Where to read it, in <c>[0, 100]</c>.</param>
    public static double Linear(ReadOnlySpan<double> sorted, double percent)
    {
        if (sorted.Length == 1)
        {
            return sorted[0];
        }

        double position = (sorted.Length - 1) * percent / 100.0;
        int lower = (int)Math.Floor(position);
        if (lower >= sorted.Length - 1)
        {
            return sorted[sorted.Length - 1];
        }

        double fraction = position - lower;
        return sorted[lower] + (fraction * (sorted[lower + 1] - sorted[lower]));
    }

    /// <summary>The median of an already sorted column — <c>numpy.median</c>, which is this at 50.</summary>
    public static double Median(ReadOnlySpan<double> sorted) => Linear(sorted, 50.0);

    /// <summary>One feature's values, ascending, as a scaler reads them out of a row-major matrix.</summary>
    public static double[] SortedColumn(ReadOnlySpan<double> samples, int featureCount, int feature, int sampleCount)
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
