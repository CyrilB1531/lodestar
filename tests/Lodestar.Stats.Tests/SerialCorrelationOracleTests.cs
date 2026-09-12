using System.Text.Json;
using Lodestar.Stats.Tests.Oracles;
using Lodestar.Stats.TimeSeries;
using Xunit;

namespace Lodestar.Stats.Tests;

/// <summary>Replays <c>tests/oracles/stats_timeseries.json</c>.</summary>
public sealed class SerialCorrelationOracleTests
{
    [Fact]
    public void Every_case_matches_statsmodels()
    {
        using JsonDocument document = StatsCorpus.Load("stats_timeseries.json");
        int replayed = 0;

        foreach (JsonElement c in document.RootElement.GetProperty("cases").EnumerateArray())
        {
            string name = c.GetProperty("name").GetString()!;
            double[] series = StatsCorpus.Doubles(c.GetProperty("series"));
            int lagCount = c.GetProperty("lag_count").GetInt32();

            switch (c.GetProperty("call").GetString())
            {
                case "acf":
                    ReplayBand(
                        c, name, SerialCorrelation.Autocorrelation(
                            series, lagCount, new AutocorrelationOptions
                            {
                                ConfidenceLevel = c.GetProperty("level").GetDouble(),
                                Adjusted = c.GetProperty("adjusted").GetBoolean(),
                                BartlettConfidenceInterval = c.GetProperty("bartlett").GetBoolean(),
                            }));
                    break;

                case "pacf":
                    ReplayBand(
                        c, name, SerialCorrelation.PartialAutocorrelation(
                            series, lagCount, new AutocorrelationOptions
                            {
                                ConfidenceLevel = c.GetProperty("level").GetDouble(),
                            }));
                    break;

                default:
                    ReplayLjungBox(
                        c, name, SerialCorrelation.LjungBox(
                            series, lagCount, new LjungBoxOptions
                            {
                                ModelDegreesOfFreedom = c.GetProperty("model_df").GetInt32(),
                                BoxPierce = true,
                            }));
                    break;
            }

            replayed++;
        }

        Assert.Equal(
            document.RootElement.GetProperty("metadata").GetProperty("count").GetInt32(),
            replayed);
    }

    private static void ReplayBand(JsonElement c, string name, AutocorrelationResult result)
    {
        Compare(c.GetProperty("values"), result.Values, $"{name} values");
        Compare(c.GetProperty("lower"), result.ConfidenceLower, $"{name} lower");
        Compare(c.GetProperty("upper"), result.ConfidenceUpper, $"{name} upper");
    }

    private static void ReplayLjungBox(JsonElement c, string name, LjungBoxResult result)
    {
        Compare(c.GetProperty("statistics"), result.Statistics, $"{name} statistic");
        Compare(c.GetProperty("bp_statistics"), result.BoxPierceStatistics, $"{name} bp statistic");
        ComparePValues(c.GetProperty("pvalues"), result.PValues, $"{name} p");
        ComparePValues(c.GetProperty("bp_pvalues"), result.BoxPiercePValues, $"{name} bp p");
    }

    private static void Compare(JsonElement expected, IReadOnlyList<double> actual, string caseName)
    {
        double[] values = StatsCorpus.Doubles(expected);
        Assert.Equal(values.Length, actual.Count);
        for (int i = 0; i < values.Length; i++)
        {
            StatsOracleAsserts.Statistic(values[i], actual[i], $"{caseName}[{i}]");
        }
    }

    private static void ComparePValues(
        JsonElement expected, IReadOnlyList<double> actual, string caseName)
    {
        double[] values = StatsCorpus.Doubles(expected);
        Assert.Equal(values.Length, actual.Count);
        for (int i = 0; i < values.Length; i++)
        {
            StatsOracleAsserts.PValue(values[i], actual[i], $"{caseName}[{i}]");
        }
    }

    [Fact]
    public void The_ACF_and_PACF_hold_more_mass_past_lag_one_in_opposite_fixtures()
    {
        // Which of the two holds more mass past lag 1 reverses between the fixtures, so a swap of
        // Autocorrelation and PartialAutocorrelation flips both comparisons below, not just one.
        using JsonDocument document = StatsCorpus.Load("stats_timeseries.json");

        (double arFull, double arPartial) = MeanAbsolutePastLagOne(document, "AR(1) at 0.7, 40 points");
        Assert.True(
            arFull > arPartial,
            $"AR(1): mean|full[2..8]| is {arFull}, mean|partial[2..8]| is {arPartial}.");

        (double maFull, double maPartial) = MeanAbsolutePastLagOne(document, "MA(1) at 0.6, 60 points");
        Assert.True(
            maPartial > maFull,
            $"MA(1): mean|partial[2..8]| is {maPartial}, mean|full[2..8]| is {maFull}.");
    }

    private static (double FullMean, double PartialMean) MeanAbsolutePastLagOne(
        JsonDocument document, string fixtureName)
    {
        JsonElement c = First(document, $"{fixtureName} | pacf | 0.95");
        double[] series = StatsCorpus.Doubles(c.GetProperty("series"));

        AutocorrelationResult full = SerialCorrelation.Autocorrelation(series, 8);
        AutocorrelationResult partial = SerialCorrelation.PartialAutocorrelation(series, 8);

        return (MeanAbsolute(full.Values), MeanAbsolute(partial.Values));
    }

    private static double MeanAbsolute(IReadOnlyList<double> values)
    {
        double sum = 0.0;
        for (int lag = 2; lag <= 8; lag++)
        {
            sum += Math.Abs(values[lag]);
        }

        return sum / 7.0;
    }

    [Fact]
    public void Lag_zero_is_exactly_one_on_every_corpus_series()
    {
        using JsonDocument document = StatsCorpus.Load("stats_timeseries.json");

        foreach (JsonElement c in document.RootElement.GetProperty("cases").EnumerateArray())
        {
            double[] series = StatsCorpus.Doubles(c.GetProperty("series"));
            Assert.Equal(1.0, SerialCorrelation.Autocorrelation(series, 3).Values[0]);
            Assert.Equal(1.0, SerialCorrelation.PartialAutocorrelation(series, 3).Values[0]);
        }
    }

    private static JsonElement First(JsonDocument document, string name)
    {
        foreach (JsonElement c in document.RootElement.GetProperty("cases").EnumerateArray())
        {
            if (c.GetProperty("name").GetString() == name)
            {
                return c;
            }
        }

        throw new InvalidDataException($"No corpus case named '{name}'.");
    }
}
