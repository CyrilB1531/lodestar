using System.Globalization;
using Lodestar.Stats.TimeSeries.Internal;

namespace Lodestar.Stats.TimeSeries;

/// <summary>Classical decomposition of a series into trend, seasonal pattern and residual, by moving averages.</summary>
public static class SeasonalDecomposition
{
    /// <summary>Splits <paramref name="series"/> into its trend, seasonal and residual components.</summary>
    /// <param name="series">The observations, in time order, at least two full periods of them.</param>
    /// <param name="period">
    /// The season's length in observations: 12 for monthly data with a yearly season. Required — the
    /// reference infers it from a pandas index, which a span does not have.
    /// </param>
    /// <param name="options">The model, the filter's sides and the trend extrapolation, or null for the defaults.</param>
    /// <returns>Three lists the length of the series.</returns>
    /// <exception cref="ArgumentOutOfRangeException"><paramref name="period"/> is below two.</exception>
    /// <exception cref="ArgumentException">
    /// <paramref name="series"/> carries a non-finite value, holds fewer than two periods, or, under
    /// <see cref="SeasonalModel.Multiplicative"/>, a value at or below zero.
    /// </exception>
    public static SeasonalComponents Decompose(
        ReadOnlySpan<double> series, int period, SeasonalDecompositionOptions? options = null)
    {
        SeasonalDecompositionOptions settings = options ?? new SeasonalDecompositionOptions();
        Guard.NotLessThan(period, 2);
        SeriesChecks.RefuseNonFinite(series);
        Refuse(series, period, settings.Model);

        bool multiplicative = settings.Model == SeasonalModel.Multiplicative;
        double[] trend = MovingAverage(series, period, settings.TwoSided);
        if (settings.ExtrapolateTrend > 0)
        {
            Extrapolate(trend, settings.ExtrapolateTrend + 1);
        }

        int n = series.Length;
        var detrended = new double[n];
        for (int i = 0; i < n; i++)
        {
            detrended[i] = multiplicative ? series[i] / trend[i] : series[i] - trend[i];
        }

        double[] pattern = PhaseAverages(detrended, period, multiplicative);
        var seasonal = new double[n];
        var residual = new double[n];
        for (int i = 0; i < n; i++)
        {
            seasonal[i] = pattern[i % period];
            residual[i] = multiplicative ? series[i] / seasonal[i] / trend[i] : detrended[i] - seasonal[i];
        }

        return new SeasonalComponents { Trend = trend, Seasonal = seasonal, Residual = residual };
    }

    private static void Refuse(ReadOnlySpan<double> series, int period, SeasonalModel model)
    {
        if (series.Length < 2 * period)
        {
            throw new ArgumentException(
                $"a period of {period} needs two full cycles, {2 * period} observations; the series holds "
                + $"{series.Length}.", nameof(series));
        }

        if (model != SeasonalModel.Multiplicative)
        {
            return;
        }

        for (int row = 0; row < series.Length; row++)
        {
            if (series[row] <= 0.0)
            {
                throw new ArgumentException(
                    $"row {row} carries {series[row].ToString(CultureInfo.InvariantCulture)}: a multiplicative "
                    + "season divides by the level, which must stay above zero.", nameof(series));
            }
        }
    }

    /// <summary>The reference's default filter, applied as <c>scipy.signal.convolve(mode="valid")</c> and padded with NaN.</summary>
    /// <remarks>
    /// An even period weights its two end points by one half over <c>period + 1</c> observations, an odd
    /// one weights <c>period</c> observations equally, so each window is a running sum over its interior
    /// divided by the period: one addition and one subtraction per step rather than a weighted pass over
    /// the whole window, and the corpus's 1e-9 holds the reordered sum.
    /// </remarks>
    private static double[] MovingAverage(ReadOnlySpan<double> series, int period, bool twoSided)
    {
        bool even = period % 2 == 0;
        int length = even ? period + 1 : period;
        int n = series.Length;
        var trend = new double[n];
        int head = twoSided ? ((length + 1) / 2) - 1 : length - 1;
        for (int i = 0; i < head; i++)
        {
            trend[i] = double.NaN;
        }

        for (int i = head + n - length + 1; i < n; i++)
        {
            trend[i] = double.NaN;
        }

        // The interior: every point of an odd window, every point but the two half-weighted ends of an even one.
        int interiorStart = even ? 1 : 0;
        int interiorLength = even ? length - 2 : length;
        double interior = 0.0;
        for (int k = 0; k < interiorLength; k++)
        {
            interior += series[interiorStart + k];
        }

        for (int start = 0; start + length <= n; start++)
        {
            if (start > 0)
            {
                interior += series[start + interiorStart + interiorLength - 1] - series[start + interiorStart - 1];
            }

            double total = even ? interior + (0.5 * (series[start] + series[start + length - 1])) : interior;
            trend[start + head] = total / period;
        }

        return trend;
    }

    /// <summary>Fills the NaN ends by least-squares lines, over the reference's own windows.</summary>
    /// <remarks>
    /// The back window is <c>[back − npoints, back)</c> and so leaves the last defined point out, exactly as
    /// <c>trend[back_first:back]</c> does in statsmodels; kept as written, since the corpus freezes it.
    /// </remarks>
    private static void Extrapolate(double[] trend, int points)
    {
        int n = trend.Length;
        int front = 0;
        while (double.IsNaN(trend[front]))
        {
            front++;
        }

        int back = n - 1;
        while (double.IsNaN(trend[back]))
        {
            back--;
        }

        int frontLast = Math.Min(front + points, back);
        (double frontSlope, double frontIntercept) = FitWindow(trend, front, frontLast);
        for (int i = 0; i < front; i++)
        {
            trend[i] = (i * frontSlope) + frontIntercept;
        }

        int backFirst = Math.Max(front, back - points);
        (double backSlope, double backIntercept) = FitWindow(trend, backFirst, back);
        for (int i = back + 1; i < n; i++)
        {
            trend[i] = (i * backSlope) + backIntercept;
        }
    }

    private static (double Slope, double Intercept) FitWindow(double[] trend, int from, int to)
    {
        var positions = new double[to - from];
        for (int i = 0; i < positions.Length; i++)
        {
            positions[i] = from + i;
        }

        return LineFit.Through(positions, trend.AsSpan(from, to - from));
    }

    /// <summary>The mean detrended value at each phase, ignoring NaN, centred on zero or one.</summary>
    private static double[] PhaseAverages(double[] detrended, int period, bool multiplicative)
    {
        var averages = new double[period];
        for (int phase = 0; phase < period; phase++)
        {
            double sum = 0.0;
            int count = 0;
            for (int i = phase; i < detrended.Length; i += period)
            {
                if (!double.IsNaN(detrended[i]))
                {
                    sum += detrended[i];
                    count++;
                }
            }

            averages[phase] = count == 0 ? double.NaN : sum / count;
        }

        double centre = 0.0;
        foreach (double average in averages)
        {
            centre += average;
        }

        centre /= period;
        for (int phase = 0; phase < period; phase++)
        {
            averages[phase] = multiplicative ? averages[phase] / centre : averages[phase] - centre;
        }

        return averages;
    }
}
