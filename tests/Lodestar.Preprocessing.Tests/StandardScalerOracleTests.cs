using System.Text.Json;
using Xunit;

namespace Lodestar.Preprocessing.Tests;

/// <summary>
/// Replays <c>sklearn.preprocessing.StandardScaler</c> over the eight frozen cases of
/// <c>tests/oracles/preprocessing_standard_scaler.json</c>.
/// </summary>
/// <remarks>
/// Each case is chosen for a branch of the fit rather than for variety, and two of them
/// exist as a pair: <c>1e8 ± 1e-8</c> is called constant by the two-pass error bound while
/// <c>1e8 ± 1e-7</c> is not. An implementation testing <c>variance == 0</c> passes every
/// other case here and fails those two by eight orders of magnitude.
/// </remarks>
public sealed class StandardScalerOracleTests
{
    /// <summary>The tolerance the whole repository uses for oracle replay.</summary>
    private const double Tolerance = 1e-9;

    private static readonly JsonDocument Corpus =
        OracleLoader.Load("preprocessing_standard_scaler.json");

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

    private static double[] Doubles(JsonElement element, string name) =>
        [.. element.GetProperty(name).EnumerateArray().Select(v => v.GetDouble())];

    private static double[]? OptionalDoubles(JsonElement element, string name) =>
        element.GetProperty(name).ValueKind == JsonValueKind.Null ? null : Doubles(element, name);

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
            Assert.Equal(expected[i], actual[i], Tolerance);
        }
    }

    [Theory]
    [MemberData(nameof(Indices))]
    public void The_fitted_statistics_match_the_reference(int index)
    {
        JsonElement frozen = Cases[index];
        StandardScaler scaler = Fit(frozen);

        Assert.Equal(frozen.GetProperty("feature_count").GetInt32(), scaler.FeatureCount);
        Assert.Equal(frozen.GetProperty("n_samples_seen").GetInt32(), scaler.SampleCount);
        AssertSame(OptionalDoubles(frozen, "mean"), scaler.Mean, "mean_");
        AssertSame(OptionalDoubles(frozen, "var"), scaler.Variance, "var_");
        AssertSame(OptionalDoubles(frozen, "scale"), scaler.Scale, "scale_");
    }

    [Theory]
    [MemberData(nameof(Indices))]
    public void The_transform_matches_the_reference(int index)
    {
        JsonElement frozen = Cases[index];
        StandardScaler scaler = Fit(frozen);

        double[] actual = scaler.Transform(Doubles(frozen, "samples"));

        AssertSame(Doubles(frozen, "transformed"), actual, "transform");
    }

    [Theory]
    [MemberData(nameof(Indices))]
    public void The_inverse_transform_matches_the_reference(int index)
    {
        JsonElement frozen = Cases[index];
        StandardScaler scaler = Fit(frozen);

        double[] actual = scaler.InverseTransform(Doubles(frozen, "transformed"));

        AssertSame(Doubles(frozen, "inverse_transformed"), actual, "inverse_transform");
    }

    private static StandardScaler Fit(JsonElement frozen) => StandardScaler.Fit(
        Doubles(frozen, "samples"),
        frozen.GetProperty("feature_count").GetInt32(),
        new StandardScalerOptions
        {
            WithMean = frozen.GetProperty("with_mean").GetBoolean(),
            WithStd = frozen.GetProperty("with_std").GetBoolean(),
        });
}
