using System.Text.Json;
using Lodestar.Stats.TimeSeries.Tests.Oracles;
using Xunit;

namespace Lodestar.Stats.TimeSeries.Tests;

/// <summary>Replays <c>statsmodels.tsa.api.VAR(...).fit(p)</c> over the frozen cases of <c>tests/oracles/stats_var.json</c> (#786).</summary>
/// <remarks>
/// Relative at 1e-9, as every other corpus here. The fit is least squares, so the reference has no optimiser's
/// stopping point to reproduce: decision 0134 measured it against <c>numpy.linalg.lstsq</c> at a gap of 0.0.
/// </remarks>
public sealed class VectorAutoregressionOracleTests
{
    private const double Relative = 1e-9;

    [Fact]
    public void Every_case_matches_statsmodels()
    {
        using JsonDocument document = StatsCorpus.Load("stats_var.json");
        int replayed = 0;

        foreach (JsonElement frozen in document.RootElement.GetProperty("cases").EnumerateArray())
        {
            string name = frozen.GetProperty("name").GetString()!;
            VarSummary fit = VectorAutoregression.Fit(
                StatsCorpus.Doubles(frozen.GetProperty("series")),
                frozen.GetProperty("variableCount").GetInt32(),
                frozen.GetProperty("lagOrder").GetInt32(),
                new VarOptions { WithIntercept = frozen.GetProperty("withIntercept").GetBoolean() });

            Table(frozen, "coefficients", fit.Coefficients, name);
            Table(frozen, "standardErrors", fit.StandardErrors, name);
            Table(frozen, "tStatistics", fit.TStatistics, name);
            Table(frozen, "pValues", fit.PValues, name);
            Vector(frozen, "residualCovariance", fit.ResidualCovariance, name);
            Vector(frozen, "residualCovarianceMaximumLikelihood", fit.ResidualCovarianceMaximumLikelihood, name);
            Scalar(frozen, "logLikelihood", fit.LogLikelihood, name);
            Scalar(frozen, "akaike", fit.Akaike, name);
            Scalar(frozen, "bayesian", fit.Bayesian, name);
            Scalar(frozen, "hannanQuinn", fit.HannanQuinn, name);
            Scalar(frozen, "finalPredictionError", fit.FinalPredictionError, name);
            Assert.Equal(frozen.GetProperty("observationsUsed").GetInt32(), fit.ObservationsUsed);
            Assert.Equal(frozen.GetProperty("modelDegreesOfFreedom").GetInt32(), fit.ModelDegreesOfFreedom);
            Assert.Equal(frozen.GetProperty("residualDegreesOfFreedom").GetInt32(), fit.ResidualDegreesOfFreedom);
            replayed++;
        }

        Assert.Equal(document.RootElement.GetProperty("metadata").GetProperty("count").GetInt32(), replayed);
    }

    private static void Table(
        JsonElement frozen, string field, IReadOnlyList<IReadOnlyList<double>> actual, string name)
    {
        JsonElement[] equations = [.. frozen.GetProperty(field).EnumerateArray()];

        Assert.Equal(equations.Length, actual.Count);
        for (int equation = 0; equation < equations.Length; equation++)
        {
            double[] expected = StatsCorpus.Doubles(equations[equation]);
            Assert.Equal(expected.Length, actual[equation].Count);
            for (int column = 0; column < expected.Length; column++)
            {
                Relatively(expected[column], actual[equation][column], $"{name}: {field}[{equation}][{column}]");
            }
        }
    }

    private static void Vector(JsonElement frozen, string field, IReadOnlyList<double> actual, string name)
    {
        double[] expected = StatsCorpus.Doubles(frozen.GetProperty(field));

        Assert.Equal(expected.Length, actual.Count);
        for (int i = 0; i < expected.Length; i++)
        {
            Relatively(expected[i], actual[i], $"{name}: {field}[{i}]");
        }
    }

    private static void Scalar(JsonElement frozen, string field, double actual, string name) =>
        Relatively(frozen.GetProperty(field).GetDouble(), actual, $"{name}: {field}");

    private static void Relatively(double expected, double actual, string what)
    {
        double gap = Math.Abs(actual - expected) / Math.Abs(expected);

        Assert.True(gap <= Relative, $"{what}: expected {expected:R}, got {actual:R} — relative gap {gap:R}");
    }
}
