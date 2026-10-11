namespace Lodestar.Internal;

/// <summary>numpy's <c>linspace</c>, <c>percentile</c> (linear and <c>averaged_inverted_cdf</c>) and medians, to the last bit.</summary>
/// <remarks>
/// A bin edge computed as <c>i / n</c> or interpolated the textbook way lands an ulp off numpy's,
/// and a value sitting on the edge then changes bin: <c>linspace(0, 1, 11)[3]</c> is
/// <c>0.30000000000000004</c>, not <c>0.3</c> (#1204). <c>calibration_curve</c> and
/// <c>KBinsDiscretizer</c> place their edges this way, and <c>Lodestar.Preprocessing</c>'s scalers and
/// imputer read their percentiles and medians here (#1230). Written from numpy 2.5.3's <c>linspace</c>,
/// <c>_quantile</c>, <c>_lerp</c>, <c>median</c> and <c>ma.median</c>.
/// </remarks>
internal static class NumpyGrid
{
    /// <summary><c>np.linspace(start, stop, num)</c>: <c>i · step + start</c>, the last point <paramref name="stop"/> exactly.</summary>
    /// <exception cref="ArgumentOutOfRangeException"><paramref name="num"/> is negative.</exception>
    public static double[] Linspace(double start, double stop, int num)
    {
        if (num < 0)
        {
            throw new ArgumentOutOfRangeException(nameof(num), num, "The number of samples must be non-negative.");
        }

        var points = new double[num];
        int div = num - 1;
        double delta = stop - start;
        double step = div > 0 ? delta / div : double.NaN;

        // S1244: numpy's own test, step == 0, which sends a denormal step down the divide-first path.
#pragma warning disable S1244
        bool stepIsZero = div > 0 && step == 0.0;
#pragma warning restore S1244
        for (int i = 0; i < num; i++)
        {
            double offset;
            if (div <= 0)
            {
                offset = i * delta;
            }
            else if (stepIsZero)
            {
                offset = (double)i / div * delta;
            }
            else
            {
                offset = i * step;
            }

            points[i] = offset + start;
        }

        if (num > 1)
        {
            points[num - 1] = stop;
        }

        return points;
    }

    /// <summary><c>np.percentile(values, q)</c> under the default linear method, over values already sorted ascending.</summary>
    /// <param name="sorted">The values, sorted ascending, at least one.</param>
    /// <param name="q">The percentile, in <c>[0, 100]</c>; divided by 100 as numpy divides it.</param>
    public static double Percentile(ReadOnlySpan<double> sorted, double q)
    {
        double quantile = q / 100.0;
        int last = sorted.Length - 1;
        double position = last * quantile;
        if (!(position < last))
        {
            // numpy's _get_indexes puts the top's lower index at -1, so gamma is last + 1 and _lerp takes its upper
            // branch, b − 0·(1 − gamma): a -0.0 kept at one row, +0.0 past it.
            return sorted[last] - (0.0 * (1.0 - (last + 1.0)));
        }

        if (position < 0.0)
        {
            return sorted[0];
        }

        double below = Math.Floor(position);
        int at = (int)below;
        return Lerp(sorted[at], sorted[at + 1], position - below);
    }

    /// <summary>
    /// <c>np.percentile(values, q, method="averaged_inverted_cdf")</c>, over values already sorted ascending.
    /// </summary>
    /// <remarks>
    /// numpy's own route, not the textbook one: the virtual index <c>n · (q / 100) − 1</c>, clipped to the
    /// ends, and a <c>_lerp</c> at <c>0.5</c> where the index is whole and at <c>1</c> otherwise — so an
    /// average is <c>b − (b − a) · 0.5</c>, which can differ from <c>(a + b) / 2</c> in the last bit (#1237).
    /// </remarks>
    /// <param name="sorted">The values, sorted ascending, at least one.</param>
    /// <param name="q">The percentile, in <c>[0, 100]</c>.</param>
    public static double AveragedInvertedCdf(ReadOnlySpan<double> sorted, double q)
    {
        int count = sorted.Length;
        double position = (count * (q / 100.0)) - 1.0;
        if (position >= count - 1)
        {
            return sorted[count - 1];
        }

        if (position < 0.0)
        {
            return sorted[0];
        }

        double below = Math.Floor(position);
        int at = (int)below;

        // S1244: numpy's fix_gamma tests the fractional part against exactly zero.
#pragma warning disable S1244
        double gamma = position - below == 0.0 ? 0.5 : 1.0;
#pragma warning restore S1244
        return Lerp(sorted[at], sorted[at + 1], gamma);
    }

    /// <summary><c>np.median</c> over values already sorted ascending: the middle one, or the two middle ones' mean.</summary>
    /// <remarks>numpy takes either through <c>np.mean</c>, whose sum starts from +0.0: a median of -0.0 is +0.0.</remarks>
    /// <param name="sorted">The values, sorted ascending, at least one.</param>
    public static double Median(ReadOnlySpan<double> sorted)
    {
        int half = sorted.Length / 2;
        return sorted.Length % 2 == 1 ? 0.0 + sorted[half] : (0.0 + (sorted[half - 1] + sorted[half])) / 2.0;
    }

    /// <summary>
    /// <c>np.ma.median</c> along an axis of a masked 2-D array, over values already sorted ascending: the two middle
    /// values added and halved, the middle one added to itself where the count is odd — so <c>[1e308] * 3</c> reads
    /// infinity, as numpy's does there (a plain array goes to <c>np.median</c> instead).
    /// </summary>
    /// <param name="sorted">The values, sorted ascending, at least one.</param>
    public static double MaskedMedian(ReadOnlySpan<double> sorted)
    {
        int half = sorted.Length / 2;
        int low = sorted.Length % 2 == 1 ? half : half - 1;
        // ma.sum's +0.0 identity first, as numpy's: a median of -0.0 is +0.0.
        return (0.0 + (sorted[low] + sorted[half])) / 2.0;
    }

    // numpy's _lerp: from the left end below one half, from the right end at or above it, so
    // the result is exact at both ends and symmetric about the middle.
    private static double Lerp(double a, double b, double t)
    {
        double difference = b - a;
        return t >= 0.5 ? b - (difference * (1.0 - t)) : a + (difference * t);
    }
}
