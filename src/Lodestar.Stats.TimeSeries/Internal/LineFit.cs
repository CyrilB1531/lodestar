namespace Lodestar.Stats.TimeSeries.Internal;

/// <summary>The least-squares line through a handful of points, in closed form.</summary>
/// <remarks>
/// Not <c>OrdinaryLeastSquares.Fit</c>: that refuses a design with no residual degrees of freedom, which
/// a two-point extrapolation window is, and computes an intercept test and VIFs a line does not need.
/// One point takes the minimum-norm solution <c>numpy.linalg.lstsq</c> returns for the reference.
/// </remarks>
internal static class LineFit
{
    internal static (double Slope, double Intercept) Through(
        ReadOnlySpan<double> abscissae, ReadOnlySpan<double> ordinates)
    {
        int count = abscissae.Length;
        if (count == 1)
        {
            double norm = (abscissae[0] * abscissae[0]) + 1.0;
            return (ordinates[0] * abscissae[0] / norm, ordinates[0] / norm);
        }

        double meanX = 0.0;
        double meanY = 0.0;
        for (int i = 0; i < count; i++)
        {
            meanX += abscissae[i];
            meanY += ordinates[i];
        }

        meanX /= count;
        meanY /= count;

        double sxy = 0.0;
        double sxx = 0.0;
        for (int i = 0; i < count; i++)
        {
            double dx = abscissae[i] - meanX;
            sxy += dx * (ordinates[i] - meanY);
            sxx += dx * dx;
        }

        double slope = sxy / sxx;
        return (slope, meanY - (slope * meanX));
    }

    /// <summary>The series less its least-squares line over 1…n: the residuals a trend null leaves.</summary>
    /// <remarks>
    /// Written here rather than in each caller: the KPSS statistic and the straight-line refusal must
    /// measure the same residuals, or the refusal guards a fit nobody computed (#1080).
    /// </remarks>
    internal static double[] Residuals(ReadOnlySpan<double> series)
    {
        int count = series.Length;
        var time = new double[count];
        for (int i = 0; i < count; i++)
        {
            time[i] = i + 1;
        }

        (double slope, double intercept) = Through(time, series);
        var residuals = new double[count];
        for (int i = 0; i < count; i++)
        {
            residuals[i] = series[i] - (intercept + (slope * time[i]));
        }

        return residuals;
    }
}
