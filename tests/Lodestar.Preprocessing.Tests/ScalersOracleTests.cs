using System.Text.Json;
using Xunit;

namespace Lodestar.Preprocessing.Tests;

/// <summary>
/// Replays <c>MinMaxScaler</c>, <c>MaxAbsScaler</c> and <c>RobustScaler</c> over the eighteen frozen
/// cases of <c>tests/oracles/preprocessing_scalers.json</c> (#763).
/// </summary>
/// <remarks>
/// Every case carries a row the fit never saw, because clipping cannot be seen on fitted values —
/// they are inside the range by construction. The four <c>unit_variance</c> cases pin the order of
/// the two scale rules: floored to 1 first, divided by the quantiles second.
/// </remarks>
public sealed class ScalersOracleTests
{
    /// <summary>The tolerance the whole repository uses for oracle replay.</summary>
    private const double Tolerance = 1e-9;

    private static readonly JsonDocument Corpus = OracleLoader.Load("preprocessing_scalers.json");

    private static IReadOnlyList<JsonElement> Cases =>
        [.. Corpus.RootElement.GetProperty("cases").EnumerateArray()];

    public static TheoryData<int> Indices()
    {
        var data = new TheoryData<int>();
        for (int i = 0; i < Cases.Count; i++)
        {
            data.Add(i);
        }

        return data;
    }

    [Theory]
    [MemberData(nameof(Indices))]
    public void Every_case_matches_scikit_learn(int index)
    {
        JsonElement frozen = Cases[index];
        string name = frozen.GetProperty("name").GetString()!;
        double[] samples = Doubles(frozen, "samples");
        double[] unseen = Doubles(frozen, "unseen");
        int featureCount = frozen.GetProperty("featureCount").GetInt32();
        JsonElement fitted = frozen.GetProperty("fitted");

        double[] transformed;
        double[] inverse;
        double[] unseenTransformed;

        switch (frozen.GetProperty("scaler").GetString())
        {
            case "minmax":
                MinMaxScaler minMax = MinMaxScaler.Fit(
                    samples,
                    featureCount,
                    new MinMaxScalerOptions
                    {
                        Low = frozen.GetProperty("featureLow").GetDouble(),
                        High = frozen.GetProperty("featureHigh").GetDouble(),
                        Clip = frozen.GetProperty("clip").GetBoolean(),
                    });

                AssertSame(Doubles(fitted, "dataMinimum"), minMax.DataMinimum, $"{name}: data minimum");
                AssertSame(Doubles(fitted, "dataMaximum"), minMax.DataMaximum, $"{name}: data maximum");
                AssertSame(Doubles(fitted, "dataRange"), minMax.DataRange, $"{name}: data range");
                AssertSame(Doubles(fitted, "scale"), minMax.Scale, $"{name}: scale");
                AssertSame(Doubles(fitted, "minimum"), minMax.Minimum, $"{name}: minimum");
                transformed = minMax.Transform(samples);
                inverse = minMax.InverseTransform(transformed);
                unseenTransformed = minMax.Transform(unseen);
                break;

            case "maxabs":
                MaxAbsScaler maxAbs = MaxAbsScaler.Fit(
                    samples,
                    featureCount,
                    new MaxAbsScalerOptions { Clip = frozen.GetProperty("clip").GetBoolean() });

                AssertSame(Doubles(fitted, "maximumAbsolute"), maxAbs.MaximumAbsolute, $"{name}: maximum absolute");
                AssertSame(Doubles(fitted, "scale"), maxAbs.Scale, $"{name}: scale");
                transformed = maxAbs.Transform(samples);
                inverse = maxAbs.InverseTransform(transformed);
                unseenTransformed = maxAbs.Transform(unseen);
                break;

            default:
                RobustScaler robust = RobustScaler.Fit(
                    samples,
                    featureCount,
                    new RobustScalerOptions
                    {
                        WithCentring = frozen.GetProperty("withCentring").GetBoolean(),
                        WithScaling = frozen.GetProperty("withScaling").GetBoolean(),
                        LowerPercentile = frozen.GetProperty("lowerPercentile").GetDouble(),
                        UpperPercentile = frozen.GetProperty("upperPercentile").GetDouble(),
                        UnitVariance = frozen.TryGetProperty("unitVariance", out JsonElement unit)
                            && unit.GetBoolean(),
                    });

                AssertSame(OptionalDoubles(fitted, "centre"), robust.Centre, $"{name}: centre");
                AssertSame(OptionalDoubles(fitted, "scale"), robust.Scale, $"{name}: scale");
                transformed = robust.Transform(samples);
                inverse = robust.InverseTransform(transformed);
                unseenTransformed = robust.Transform(unseen);
                break;
        }

        AssertSame(Doubles(frozen, "transformed"), transformed, $"{name}: transformed");
        AssertSame(Doubles(frozen, "inverseTransformed"), inverse, $"{name}: inverse");
        AssertSame(Doubles(frozen, "unseenTransformed"), unseenTransformed, $"{name}: unseen");
    }

    [Fact]
    public void Every_frozen_case_is_replayed()
    {
        Assert.Equal(Corpus.RootElement.GetProperty("metadata").GetProperty("count").GetInt32(), Cases.Count);
    }

    private static double[] Doubles(JsonElement element, string name) =>
        [.. element.GetProperty(name).EnumerateArray().Select(v => v.GetDouble())];

    private static double[]? OptionalDoubles(JsonElement element, string name) =>
        element.GetProperty(name).ValueKind == JsonValueKind.Null ? null : Doubles(element, name);

    /// <summary>
    /// Relative where the values are large: a MinMax scale of 2.5e14 cannot be compared at an
    /// absolute 1e-9, and comparing it that way would pass on any implementation at all.
    /// </summary>
    private static void AssertSame(double[]? expected, IReadOnlyList<double>? actual, string what)
    {
        if (expected is null)
        {
            Assert.True(actual is null, $"{what}: the reference reports none and this reports {actual?.Count} values");
            return;
        }

        Assert.NotNull(actual);
        Assert.Equal(expected.Length, actual.Count);
        for (int i = 0; i < expected.Length; i++)
        {
            double allowed = Math.Max(Tolerance, Math.Abs(expected[i]) * Tolerance);
            Assert.True(
                Math.Abs(expected[i] - actual[i]) <= allowed,
                $"{what}[{i}]: expected {expected[i]}, got {actual[i]}");
        }
    }
}
