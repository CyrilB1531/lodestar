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

    /// <summary>The percentile <c>numpy.percentile(method="averaged_inverted_cdf")</c> computes.</summary>
    /// <remarks>
    /// Hyndman and Fan's type 1, averaged at a discontinuity: at <c>h = n·q/100</c> the value is
    /// <c>x[⌈h⌉-1]</c>, except where <c>h</c> lands exactly on an integer, where the two order
    /// statistics either side are averaged. It is <see cref="Linear"/>'s sibling and not its
    /// equal — over <c>1..6</c> at three bins, this reads <c>2.5</c> and <c>4.5</c> where
    /// <see cref="Linear"/> reads <c>2.667</c> and <c>4.333</c>. <c>KBinsDiscretizer</c> defaults
    /// to this one since scikit-learn 1.9, where <c>RobustScaler</c> stays on <see cref="Linear"/>.
    /// </remarks>
    public static double AveragedInvertedCdf(ReadOnlySpan<double> sorted, double percent)
    {
        int n = sorted.Length;

        // long-comment: the arithmetic order is the whole behaviour here, and a reader who
        // "simplifies" it back has no way to see what broke without this paragraph.
        // The percent is divided before it is multiplied, which is numpy's own order and not a
        // detail: at nine values and a third, `n * percent / 100` rounds to exactly 3 where
        // `n * (percent / 100)` gives 3.0000000000000004, and only the second lands on the
        // order statistic numpy reports (#1122).
        double position = n * (percent / 100.0);

        // S1244: an exact integer position is the discontinuity the averaging is for, and only
        // an exact one -- a position a hair either side takes the single order statistic.
#pragma warning disable S1244
        bool onABoundary = position == Math.Floor(position);
#pragma warning restore S1244
        if (onABoundary)
        {
            int index = (int)position;
            if (index <= 0)
            {
                return sorted[0];
            }
            if (index >= n)
            {
                return sorted[n - 1];
            }

            return 0.5 * (sorted[index - 1] + sorted[index]);
        }

        int ceiling = (int)Math.Ceiling(position) - 1;
        return sorted[Math.Min(Math.Max(ceiling, 0), n - 1)];
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
