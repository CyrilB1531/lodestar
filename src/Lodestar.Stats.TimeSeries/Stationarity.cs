using Lodestar.Stats.TimeSeries.Internal;

namespace Lodestar.Stats.TimeSeries;

/// <summary>Whether a series may be modelled as it stands: the unit-root test and its complement.</summary>
/// <remarks>
/// The two nulls are opposite. The augmented Dickey-Fuller test assumes a unit root and KPSS assumes
/// stationarity, so a reader runs both: rejecting one and not the other, from opposite sides, is the
/// answer either alone cannot give.
/// </remarks>
public static class Stationarity
{
    // The reference's literal for norm.ppf(0.95), kept as written rather than recomputed.
    private const double OneSidedFivePercent = 1.6448536269514722;

    private static readonly double[] KpssPValues = [0.10, 0.05, 0.025, 0.01];
    private static readonly double[] LevelCritical = [0.347, 0.463, 0.574, 0.739];
    private static readonly double[] TrendCritical = [0.119, 0.146, 0.176, 0.216];

    /// <summary>The augmented Dickey-Fuller test, against the null of a unit root.</summary>
    /// <param name="series">The observations, in time order.</param>
    /// <param name="options">The trend terms, the lag rule and its maximum, or null for the reference's defaults.</param>
    /// <returns>The statistic, MacKinnon's p-value and critical values, and the lag the regression used.</returns>
    /// <exception cref="ArgumentException">
    /// <paramref name="series"/> carries a non-finite value, is constant, or is too short for its trend
    /// terms and default lag; or <paramref name="options"/> asks for a maximum lag above <c>n/2 − terms − 1</c>
    /// or one that leaves the widest regression no degree of freedom.
    /// </exception>
    public static DickeyFullerResult AugmentedDickeyFuller(
        ReadOnlySpan<double> series, DickeyFullerOptions? options = null)
    {
        DickeyFullerOptions settings = options ?? new DickeyFullerOptions();
        SeriesChecks.RefuseNonFinite(series);
        SeriesChecks.RefuseConstant(series);

        int n = series.Length;
        int maxLag = MaxLag(n, settings, nameof(options), nameof(series));

        int usedLag = maxLag;
        double criterion = double.NaN;
        if (settings.LagSelection != LagSelection.Fixed)
        {
            (usedLag, criterion) = SearchLag(series, settings, maxLag, n - maxLag - 1);
        }

        int observations = n - usedLag - 1;
        IReadOnlyList<double> tStatistics = DickeyFullerRegression.Fit(series, settings.Regression, usedLag, observations).TStatistics;
        double statistic = tStatistics[DickeyFullerRegression.LevelIndex(settings.Regression)];

        return new DickeyFullerResult
        {
            Statistic = statistic,
            PValue = MacKinnon.PValue(statistic, settings.Regression),
            UsedLag = usedLag,
            ObservationCount = observations,
            CriticalValues = MacKinnon.CriticalValues(observations, settings.Regression),
            InformationCriterion = criterion,
        };
    }

    /// <summary>The KPSS test, against the null of stationarity around a level or a line.</summary>
    /// <param name="series">The observations, in time order.</param>
    /// <param name="options">The null's trend and the lag window rule, or null for the reference's defaults.</param>
    /// <returns>The statistic, its tabulated p-value, the window used, and whether the p-value was clamped.</returns>
    /// <exception cref="ArgumentException">
    /// <paramref name="series"/> carries a non-finite value, is constant, or lies exactly on a line under
    /// <see cref="TrendTerms.ConstantAndTrend"/>; or <paramref name="options"/> fixes a window at or above the series length.
    /// </exception>
    public static KpssResult Kpss(ReadOnlySpan<double> series, KpssOptions? options = null)
    {
        KpssOptions settings = options ?? new KpssOptions();
        SeriesChecks.RefuseNonFinite(series);
        SeriesChecks.RefuseConstant(series);

        int n = series.Length;
        double[] residuals = KpssResiduals(series, settings.Regression);
        RefuseExactFit(residuals, nameof(series));
        int lagCount = settings.LagRule switch
        {
            KpssLagRule.Legacy => Math.Min(SchwertLag(n), n - 1),
            KpssLagRule.Automatic => Math.Min(HobijnLag(residuals), n - 1),
            KpssLagRule.Fixed when settings.LagCount < n => settings.LagCount,
            KpssLagRule.Fixed => throw new ArgumentException(
                $"a lag window of {settings.LagCount} reaches the {n} observations; it must stay below them.",
                nameof(options)),
            _ => throw new ArgumentOutOfRangeException(nameof(options), settings.LagRule, "Not a KPSS lag rule."),
        };

        double partial = 0.0;
        double eta = 0.0;
        foreach (double residual in residuals)
        {
            partial += residual;
            eta += partial * partial;
        }

        double statistic = eta / ((double)n * n) / LongRunVariance(residuals, lagCount);
        double[] critical = settings.Regression == TrendTerms.ConstantAndTrend ? TrendCritical : LevelCritical;
        double pValue = Interpolate(statistic, critical);

        return new KpssResult
        {
            Statistic = statistic,
            PValue = pValue,
            LagCount = lagCount,
            CriticalValues = (double[])critical.Clone(),
            PValueBound = BoundOf(pValue),
        };
    }

    private static double[] KpssResiduals(ReadOnlySpan<double> series, TrendTerms regression)
    {
        int n = series.Length;
        var residuals = new double[n];
        if (regression == TrendTerms.ConstantAndTrend)
        {
            var time = new double[n];
            for (int i = 0; i < n; i++)
            {
                time[i] = i + 1;
            }

            (double slope, double intercept) = LineFit.Through(time, series);
            for (int i = 0; i < n; i++)
            {
                residuals[i] = series[i] - (intercept + (slope * time[i]));
            }

            return residuals;
        }

        double mean = 0.0;
        foreach (double value in series)
        {
            mean += value;
        }

        mean /= n;
        for (int i = 0; i < n; i++)
        {
            residuals[i] = series[i] - mean;
        }

        return residuals;
    }

    /// <summary>Hobijn, Franses and Ooms' window, as the reference writes it.</summary>
    /// <summary>Refuses a series the null's trend fits exactly, which leaves no variance to test.</summary>
    /// <remarks>
    /// A constant series is refused before this, so only a straight line under <c>ConstantAndTrend</c> gets
    /// here. The statistic is then 0/0 and the Hobijn window casts that NaN to an int, which is undefined
    /// and differs between runtimes (#874). statsmodels answers from the rounding noise of its OLS fit, a
    /// value no second implementation reproduces, so the series is refused as a constant one is.
    /// </remarks>
    private static void RefuseExactFit(double[] residuals, string parameterName)
    {
        // S1244: an exact fit is exactly zero; a series that merely fits closely is testable.
#pragma warning disable S1244
        if (residuals.Any(residual => residual != 0.0))
#pragma warning restore S1244
        {
            return;
        }

        throw new ArgumentException(
            "every value lies on one straight line: the trend leaves no variance to test.", parameterName);
    }

    private static int HobijnLag(double[] residuals)
    {
        int n = residuals.Length;
        int covariances = (int)Math.Pow(n, 2.0 / 9.0);
        double s0 = SumOfSquares(residuals) / n;
        double s1 = 0.0;
        for (int i = 1; i <= covariances; i++)
        {
            double product = LaggedProduct(residuals, i) / (n / 2.0);
            s0 += product;
            s1 += i * product;
        }

        double ratio = s1 / s0;
        double gamma = 1.1447 * Math.Pow(ratio * ratio, 1.0 / 3.0);
        return (int)(gamma * Math.Pow(n, 1.0 / 3.0));
    }

    /// <summary>The Newey-West long-run variance with a Bartlett kernel.</summary>
    private static double LongRunVariance(double[] residuals, int lagCount)
    {
        double sum = SumOfSquares(residuals);
        for (int i = 1; i <= lagCount; i++)
        {
            sum += 2.0 * LaggedProduct(residuals, i) * (1.0 - (i / (lagCount + 1.0)));
        }

        return sum / residuals.Length;
    }

    /// <summary><c>numpy.interp</c> over the table: linear inside, clamped to the end values outside.</summary>
    private static double Interpolate(double statistic, double[] critical)
    {
        if (statistic <= critical[0])
        {
            return KpssPValues[0];
        }

        if (statistic >= critical[critical.Length - 1])
        {
            return KpssPValues[KpssPValues.Length - 1];
        }

        int segment = 0;
        while (statistic >= critical[segment + 1])
        {
            segment++;
        }

        double slope = (KpssPValues[segment + 1] - KpssPValues[segment]) / (critical[segment + 1] - critical[segment]);
        return (slope * (statistic - critical[segment])) + KpssPValues[segment];
    }

    /// <summary>The reference warns exactly when the returned p-value equals an end of the table.</summary>
    private static PValueBound BoundOf(double pValue)
    {
        // S1244: the reference compares p_value == pvals[-1] exactly; an interpolation never lands there.
#pragma warning disable S1244
        if (pValue == KpssPValues[KpssPValues.Length - 1])
        {
            return PValueBound.ActualIsSmaller;
        }

        return pValue == KpssPValues[0] ? PValueBound.ActualIsGreater : PValueBound.None;
#pragma warning restore S1244
    }

    private static double SumOfSquares(double[] values)
    {
        double sum = 0.0;
        foreach (double value in values)
        {
            sum += value * value;
        }

        return sum;
    }

    private static double LaggedProduct(double[] residuals, int lag)
    {
        double sum = 0.0;
        for (int t = lag; t < residuals.Length; t++)
        {
            sum += residuals[t] * residuals[t - lag];
        }

        return sum;
    }

    /// <summary>Schwert's rule, <c>ceil(12·(n/100)^¼)</c>, which both tests default to.</summary>
    internal static int SchwertLag(int n) => (int)Math.Ceiling(12.0 * Math.Pow(n / 100.0, 0.25));

    private static int MaxLag(int n, DickeyFullerOptions settings, string optionsName, string seriesName)
    {
        int terms = DickeyFullerRegression.TermCount(settings.Regression);
        int ceiling = (n / 2) - terms - 1;
        if (settings.MaxLag is int given)
        {
            if (given > ceiling || n - (2 * given) - terms - 2 < 1)
            {
                throw new ArgumentException(
                    $"a maximum lag of {given} leaves a series of {n} with {terms} trend terms no degree of "
                    + $"freedom; at most {Math.Min(ceiling, (n - terms - 3) / 2)} does.", optionsName);
            }

            return given;
        }

        int schwert = Math.Min(ceiling, SchwertLag(n));
        // The widest candidate fits terms + lag + 1 parameters to n − lag − 1 rows; with none to spare, the
        // regression refused it naming a design the caller never passed (#907). statsmodels answers rank-deficient.
        if (schwert < 0 || n - (2 * schwert) - terms - 2 < 1)
        {
            throw new ArgumentException(
                $"a series of {n} is too short for {terms} trend terms, a lagged level and the lags the default "
                + "search tries: set MaxLag lower, or use a longer series.", seriesName);
        }

        return schwert;
    }

    /// <summary>The reference's lag search, every candidate over the same <paramref name="rows"/> rows.</summary>
    private static (int Lag, double Criterion) SearchLag(
        ReadOnlySpan<double> series, DickeyFullerOptions settings, int maxLag, int rows)
    {
        bool byTStatistic = settings.LagSelection == LagSelection.TStatistic;
        (double[] residualSums, double[] lastTStatistics) =
            DickeyFullerRegression.Candidates(series, settings.Regression, maxLag, rows, byTStatistic);
        if (byTStatistic)
        {
            double absolute = 0.0;
            for (int lag = maxLag; lag >= 0; lag--)
            {
                absolute = Math.Abs(lastTStatistics[lag]);
                if (absolute >= OneSidedFivePercent)
                {
                    return (lag, absolute);
                }
            }

            return (0, absolute);
        }

        int terms = DickeyFullerRegression.TermCount(settings.Regression);
        int best = 0;
        double bestCriterion = double.PositiveInfinity;
        for (int lag = 0; lag <= maxLag; lag++)
        {
            double value = DickeyFullerRegression.Criterion(
                residualSums[lag], terms + 1 + lag, rows, settings.LagSelection);

            // Strictly smaller: a tie keeps the shorter lag, as the reference's min over (criterion, lag) does.
            if (value < bestCriterion)
            {
                bestCriterion = value;
                best = lag;
            }
        }

        return (best, bestCriterion);
    }
}
