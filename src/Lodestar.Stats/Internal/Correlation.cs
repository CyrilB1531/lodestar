namespace Lodestar.Stats.Internal;

/// <summary>The product-moment correlation of two aligned samples.</summary>
/// <remarks>
/// Shared by <see cref="Pearson"/>, which correlates the values, and by <see cref="Spearman"/>,
/// which correlates their mid-ranks: one routine so the two cannot drift apart on the
/// degenerate inputs, where a constant sample has to answer <c>NaN</c> rather than divide.
/// </remarks>
internal static class Correlation
{
    /// <summary>The coefficient in <c>[-1, 1]</c>, or <c>NaN</c> when either sample is constant.</summary>
    /// <remarks>
    /// Each centred sample is divided by its own norm before the dot product, and the norm is
    /// taken after scaling by the largest deviation: a sample reaching 5e210 squares to infinity
    /// otherwise. scipy guards it the same way, and the order matters beyond the guard -- it is
    /// what lands a perfect relationship on exactly <c>1</c>. Each of the four passes reads both
    /// samples; six separate ones cost 11.5% more at ten thousand pairs (#1120, measured
    /// 2026-09-23).
    /// </remarks>
    internal static double Coefficient(ReadOnlySpan<double> x, ReadOnlySpan<double> y)
    {
        int n = x.Length;

        (double xMean, double yMean) = Means(x, y);

        double xScale = 0.0;
        double yScale = 0.0;
        for (int i = 0; i < n; i++)
        {
            double dx = Math.Abs(x[i] - xMean);
            double dy = Math.Abs(y[i] - yMean);
            if (dx > xScale)
            {
                xScale = dx;
            }
            if (dy > yScale)
            {
                yScale = dy;
            }
        }

        // S1244: a constant sample is one whose values are literally equal, so every deviation
        // is an exact zero -- a tolerance would call a merely narrow sample constant.
#pragma warning disable S1244
        if (xScale == 0.0 || yScale == 0.0)
#pragma warning restore S1244
        {
            return double.NaN;
        }

        double xNorm = 0.0;
        double yNorm = 0.0;
        for (int i = 0; i < n; i++)
        {
            double dx = (x[i] - xMean) / xScale;
            double dy = (y[i] - yMean) / yScale;
            xNorm += dx * dx;
            yNorm += dy * dy;
        }

        xNorm = xScale * Math.Sqrt(xNorm);
        yNorm = yScale * Math.Sqrt(yNorm);

        double sum = 0.0;
        for (int i = 0; i < n; i++)
        {
            sum += ((x[i] - xMean) / xNorm) * ((y[i] - yMean) / yNorm);
        }

        // A dot product of two unit vectors can leave the range by a bit of rounding, and a
        // coefficient of 1.0000000000000002 would make the tail below it negative.
        return Math.Min(1.0, Math.Max(-1.0, sum));
    }

    /// <summary>The same coefficient over mid-ranks, through the moment form <c>spearmanr</c> reaches for.</summary>
    /// <remarks>
    /// Not <see cref="Coefficient"/>: <c>spearmanr</c> hands its ranks to <c>numpy.corrcoef</c>,
    /// which scales both moments by <c>n - 1</c> before dividing, where <c>pearsonr</c>
    /// normalises each vector and takes a dot product. The two part only at a perfect monotone
    /// relationship, where one lands on exactly 1 and the other a bit below -- which decides
    /// whether the p-value is an exact zero or 1.3e-8, the Student argument dividing by
    /// <c>1 - rho</c>. Ranks are bounded, so no overflow guard is needed here.
    /// </remarks>
    internal static double RankCoefficient(ReadOnlySpan<double> x, ReadOnlySpan<double> y)
    {
        int n = x.Length;
        (double xMean, double yMean) = Means(x, y);

        double xx = 0.0;
        double yy = 0.0;
        double xy = 0.0;
        for (int i = 0; i < n; i++)
        {
            double dx = x[i] - xMean;
            double dy = y[i] - yMean;
            xx += dx * dx;
            yy += dy * dy;
            xy += dx * dy;
        }

        double ddof = n - 1.0;
        double coefficient = xy / ddof / Math.Sqrt(xx / ddof) / Math.Sqrt(yy / ddof);

        return Math.Min(1.0, Math.Max(-1.0, coefficient));
    }

    /// <summary>Both samples' means in one pass, which is the only pass either needs.</summary>
    private static (double X, double Y) Means(ReadOnlySpan<double> x, ReadOnlySpan<double> y)
    {
        double xSum = 0.0;
        double ySum = 0.0;
        for (int i = 0; i < x.Length; i++)
        {
            xSum += x[i];
            ySum += y[i];
        }

        return (xSum / x.Length, ySum / x.Length);
    }
}
