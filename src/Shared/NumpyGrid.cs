namespace Lodestar.Internal;

/// <summary>numpy's <c>linspace</c> and default <c>percentile</c>, to the last bit.</summary>
/// <remarks>
/// A bin edge computed as <c>i / n</c> or interpolated the textbook way lands an ulp off numpy's,
/// and a value sitting on the edge then changes bin: <c>linspace(0, 1, 11)[3]</c> is
/// <c>0.30000000000000004</c>, not <c>0.3</c> (#1204). <c>calibration_curve</c> and
/// <c>KBinsDiscretizer</c> both place their edges this way. Written from numpy 2.5.3's
/// <c>linspace</c>, <c>_quantile</c> and <c>_lerp</c>.
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
            return sorted[last];
        }

        if (position < 0.0)
        {
            return sorted[0];
        }

        double below = Math.Floor(position);
        int at = (int)below;
        return Lerp(sorted[at], sorted[at + 1], position - below);
    }

    // numpy's _lerp: from the left end below one half, from the right end at or above it, so
    // the result is exact at both ends and symmetric about the middle.
    private static double Lerp(double a, double b, double t)
    {
        double difference = b - a;
        return t >= 0.5 ? b - (difference * (1.0 - t)) : a + (difference * t);
    }
}
