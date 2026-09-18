using Lodestar.Stats.Regression;
using Lodestar.Stats.TimeSeries.Internal;
using Xunit;

namespace Lodestar.Stats.TimeSeries.Tests;

/// <summary>One QR serving many fits, held bit for bit to the estimate it replaced, one QR per fit.</summary>
public sealed class SharedReflectionsTests
{
    public static TheoryData<TrendTerms, int> Searches() => new()
    {
        { TrendTerms.None, 60 },
        { TrendTerms.Constant, 200 },
        { TrendTerms.ConstantAndTrend, 97 },
        { TrendTerms.ConstantAndQuadraticTrend, 500 },
    };

    [Theory]
    [MemberData(nameof(Searches))]
    public void Every_lag_of_the_search_matches_its_own_fit_bit_for_bit(TrendTerms regression, int n)
    {
        double[] series = Walk(n, seed: n);
        int maxLag = Stationarity.SchwertLag(n);
        int rows = n - maxLag - 1;

        (double[] sums, double[] lastTStatistics) =
            DickeyFullerRegression.Candidates(series, regression, maxLag, rows, withTStatistics: true);

        for (int lag = 0; lag <= maxLag; lag++)
        {
            (IReadOnlyList<double> tStatistics, double residualSumOfSquares) =
                DickeyFullerRegression.Fit(series, regression, lag, rows);
            Assert.Equal(BitConverter.DoubleToInt64Bits(residualSumOfSquares), BitConverter.DoubleToInt64Bits(sums[lag]));
            Assert.Equal(
                BitConverter.DoubleToInt64Bits(tStatistics[tStatistics.Count - 1]),
                BitConverter.DoubleToInt64Bits(lastTStatistics[lag]));
        }
    }

    [Fact]
    public void A_search_reaching_no_degree_of_freedom_is_refused_as_the_estimate_refuses_it()
    {
        double[] series = Walk(12, seed: 3);

        ArgumentException refusal = Assert.Throws<ArgumentException>(() =>
            DickeyFullerRegression.Candidates(series, TrendTerms.None, maxLag: 5, rows: 6, withTStatistics: true));
        Assert.Throws<ArgumentException>(() =>
            DickeyFullerRegression.Candidates(series, TrendTerms.None, maxLag: 5, rows: 6, withTStatistics: false));

        // A full-rank random walk: the design this search runs out of rows for is not collinear, and the
        // translation that names a straight line claimed it was, for naming `design` too (#1080).
        Assert.Equal("design", refusal.ParamName);
        Assert.Contains("degrees of freedom left", refusal.Message, StringComparison.Ordinal);
    }

    [Fact]
    public void A_rank_deficient_design_is_still_translated_into_the_series_the_caller_passed()
    {
        double[] line = [.. Enumerable.Range(0, 40).Select(i => 0.3 + (0.1 * i))];

        ArgumentException refusal = Assert.Throws<ArgumentException>(() =>
            DickeyFullerRegression.Fit(line, TrendTerms.Constant, lag: 1, rows: 38));

        Assert.Equal("series", refusal.ParamName);
        Assert.Contains("no unique least-squares solution", refusal.Message, StringComparison.Ordinal);
    }

    [Theory]
    [InlineData(2, 1, true)]
    [InlineData(3, 4, true)]
    [InlineData(5, 2, false)]
    public void Every_equation_of_a_var_matches_its_own_estimate_bit_for_bit(int variables, int lags, bool withIntercept)
    {
        const int Observations = 150;
        double[] series = new double[Observations * variables];
        for (int variable = 0; variable < variables; variable++)
        {
            double[] one = Walk(Observations, seed: 40 + variable);
            for (int t = 0; t < Observations; t++)
            {
                series[(t * variables) + variable] = one[t];
            }
        }

        VarSummary summary = VectorAutoregression.Fit(series, variables, lags, new VarOptions { WithIntercept = withIntercept });
        double[] design = LagDesign.Stack(series, variables, lags, withIntercept, out double[][] responses);
        int parameters = (withIntercept ? 1 : 0) + (variables * lags);
        for (int equation = 0; equation < variables; equation++)
        {
            OlsEstimate estimate = OrdinaryLeastSquares.Estimate(design, responses[equation], parameters, withIntercept: false);
            Assert.Equal(Bits(estimate.Coefficients), Bits(summary.Coefficients[equation]));
            Assert.Equal(Bits(estimate.StandardErrors), Bits(summary.StandardErrors[equation]));
        }
    }

    private static long[] Bits(IReadOnlyList<double> values) => [.. values.Select(BitConverter.DoubleToInt64Bits)];

    // SonarLint S2245, CA5394: a seeded Random draws a reproducible series; no security use.
#pragma warning disable S2245, CA5394
    private static double[] Walk(int n, int seed)
    {
        var random = new Random(seed);
        var series = new double[n];
        double level = 0.0;
        for (int t = 0; t < n; t++)
        {
            level = (0.6 * level) + random.NextDouble() - 0.5;
            series[t] = level;
        }

        return series;
    }
#pragma warning restore S2245, CA5394
}
