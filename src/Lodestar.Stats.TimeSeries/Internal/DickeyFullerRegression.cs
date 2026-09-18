using Lodestar.Stats.Regression;

namespace Lodestar.Stats.TimeSeries.Internal;

/// <summary>One augmented Dickey-Fuller regression, laid out the way the reference lays out its lag search.</summary>
internal static class DickeyFullerRegression
{
    internal static int TrendColumns(TrendTerms regression) => regression switch
    {
        TrendTerms.ConstantAndTrend => 1,
        TrendTerms.ConstantAndQuadraticTrend => 2,
        _ => 0,
    };

    internal static int TermCount(TrendTerms regression) => regression switch
    {
        TrendTerms.None => 0,
        TrendTerms.Constant => 1,
        TrendTerms.ConstantAndTrend => 2,
        TrendTerms.ConstantAndQuadraticTrend => 3,
        _ => throw new ArgumentOutOfRangeException(nameof(regression), regression, "Not a trend specification."),
    };

    /// <summary>Where the lagged level's coefficient sits in the summary: after the intercept and the trend columns.</summary>
    internal static int LevelIndex(TrendTerms regression) =>
        (regression == TrendTerms.None ? 0 : 1) + TrendColumns(regression);

    /// <summary>The parameter the least-squares estimate names, which this package's callers do not have.</summary>
    private const string DesignParameter = "design";

    internal static (IReadOnlyList<double> TStatistics, double ResidualSumOfSquares) Fit(
        ReadOnlySpan<double> series, TrendTerms regression, int lag, int rows)
    {
        double[] design = Design(series, regression, lag, rows, out double[] response);

        // The estimate names `design` for a rank-deficient design and for a fit with no residual degree of
        // freedom alike, and `Candidates` raises the second through here on purpose: only the first is ours (#1080).
        bool hasResidualDegreesOfFreedom = rows - (TermCount(regression) + 1 + lag) >= 1;
        try
        {
            OlsEstimate estimate = OrdinaryLeastSquares.Estimate(
                design, response, TrendColumns(regression) + 1 + lag, withIntercept: regression != TrendTerms.None);
            return (estimate.TStatistics, estimate.ResidualSumOfSquares);
        }
        catch (ArgumentException error) when (error.ParamName == DesignParameter && hasResidualDegreesOfFreedom)
        {
            // The estimate refuses a rank-deficient design naming its own parameter, which this caller's
            // caller never passed (#979).
            throw RankDeficient(error, nameof(series));
        }
    }

    /// <summary>The refusal of a lagged design with no unique solution, worded for a caller who passed only a series.</summary>
    private static ArgumentException RankDeficient(ArgumentException error, string seriesName) => new(
        "the lagged design this series builds has no unique least-squares solution: one of its columns lies "
        + "within rounding of the span of the others, as it does when the series follows a polynomial of low "
        + "degree over the rows the regression reads.",
        seriesName,
        error);

    /// <summary>
    /// The residual sums of squares and the highest lag's t statistic of every lag from 0 to <paramref name="maxLag"/>,
    /// each bit-identical to <see cref="Fit"/> at that lag, from one QR of the widest design.
    /// </summary>
    /// <remarks>
    /// Every candidate's design is a leading block of the widest one's columns over the same rows, which is what lets
    /// one factorization answer them all: 27 QRs became one at 2,000 points.
    /// </remarks>
    internal static (double[] ResidualSumsOfSquares, double[] LastTStatistics) Candidates(
        ReadOnlySpan<double> series, TrendTerms regression, int maxLag, int rows, bool withTStatistics)
    {
        // A fit with no residual degree of freedom is refused by the estimate itself, at the lag the search reaches it first.
        int terms = TermCount(regression);
        if (withTStatistics && rows - (terms + 1 + maxLag) < 1)
        {
            _ = Fit(series, regression, maxLag, rows);
        }

        double[] design = Design(series, regression, maxLag, rows, out double[] response);
        var reflections = new SharedReflections(
            design, TrendColumns(regression) + 1 + maxLag, regression != TrendTerms.None, [response]);

        var sums = new double[maxLag + 1];
        var statistics = new double[withTStatistics ? maxLag + 1 : 0];
        for (int lag = 0; lag <= maxLag; lag++)
        {
            int order = terms + 1 + lag;
            if (rows - order < 1)
            {
                _ = Fit(series, regression, lag, rows);
            }

            reflections.ReflectThrough(order);
            sums[lag] = reflections.ResidualSumOfSquares(0, order);
            if (withTStatistics)
            {
                double[] inverse = reflections.InverseUpper(order);
                (double[] coefficients, double[] errors) = reflections.Estimates(
                    0, order, inverse, SharedReflections.SquaredNorms(inverse, order), sums[lag]);
                statistics[lag] = coefficients[order - 1] / errors[order - 1];
            }
        }

        // Every candidate is a leading block of the widest design, whose singular values interlace inside the
        // whole's: one check answers every lag, where none let a candidate be ranked on rounding (#977).
        try
        {
            _ = reflections.RequireFullRank(terms + 1 + maxLag, nameof(series));
        }
        catch (ArgumentException error)
        {
            throw RankDeficient(error, nameof(series));
        }

        return (sums, statistics);
    }

    private static double[] Design(ReadOnlySpan<double> series, TrendTerms regression, int lag, int rows, out double[] response)
    {
        // long-comment: why the trend columns come first.
        // Row r of rows is series position t = n - rows + r: the response is dx[t], and the regressors
        // are the trend columns r + 1 and (r + 1)^2, then the lagged level x[t - 1], then
        // dx[t - 1] ... dx[t - lag]. The reference's lag search prepends its trend terms, so the last
        // coefficient is the highest lag -- the one the t-statistic rule reads -- and the constant is the
        // fit's intercept. Measured: with the trend columns last instead, the t-statistic rule chose
        // lag 0 where the reference chose 10 on a 200-point AR(1).
        int n = series.Length;
        int trend = TrendColumns(regression);
        int features = trend + 1 + lag;
        var design = new double[rows * features];
        response = new double[rows];

        for (int row = 0; row < rows; row++)
        {
            int t = n - rows + row;
            int offset = row * features;
            response[row] = series[t] - series[t - 1];
            if (trend >= 1)
            {
                design[offset] = row + 1;
            }

            if (trend == 2)
            {
                design[offset + 1] = (double)(row + 1) * (row + 1);
            }

            design[offset + trend] = series[t - 1];
            for (int k = 1; k <= lag; k++)
            {
                design[offset + trend + k] = series[t - k] - series[t - k - 1];
            }
        }

        return design;
    }

    /// <summary>Akaike's or Schwarz's criterion from a fit's residual sum of squares.</summary>
    /// <remarks>
    /// <c>ℓ = −rows/2·(log 2π + log(SSR/rows) + 1)</c>, and the parameter count includes the intercept, as
    /// statsmodels' <c>df_model + k_constant</c> does.
    /// </remarks>
    internal static double Criterion(double residualSumOfSquares, int parameters, int rows, LagSelection selection)
    {
        double logLikelihood = -rows / 2.0 * (Math.Log(2.0 * Math.PI) + Math.Log(residualSumOfSquares / rows) + 1.0);
        double penalty = selection == LagSelection.Schwarz ? Math.Log(rows) * parameters : 2.0 * parameters;
        return (-2.0 * logLikelihood) + penalty;
    }
}
