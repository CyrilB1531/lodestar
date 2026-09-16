using System.Globalization;
using System.Text.Json;
using Xunit;

namespace Lodestar.Preprocessing.Tests;

/// <summary>
/// Replays <c>OneHotEncoder</c>, <c>OrdinalEncoder</c> and <c>SimpleImputer</c> over the sixteen
/// frozen cases of <c>tests/oracles/preprocessing_encoders.json</c> (#764).
/// </summary>
/// <remarks>
/// Every encoder case carries a row the fit never saw, because the branches that matter — an unknown
/// value, a dropped category — are invisible on the matrix the encoder was fitted on.
/// </remarks>
public sealed class EncodersOracleTests
{
    /// <summary>The tolerance the whole repository uses for oracle replay.</summary>
    private const double Tolerance = 1e-9;

    /// <summary>Category text is compared invariantly: a culture would renumber the integers.</summary>
    private static readonly CultureInfo Invariant = CultureInfo.InvariantCulture;

    private static readonly JsonDocument Corpus = OracleLoader.Load("preprocessing_encoders.json");

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
        int featureCount = frozen.GetProperty("feature_count").GetInt32();

        switch (frozen.GetProperty("encoder").GetString())
        {
            case "onehot":
                AssertOneHot(frozen, name, featureCount);
                break;
            case "ordinal":
                AssertOrdinal(frozen, name, featureCount);
                break;
            default:
                AssertImputed(frozen, name, featureCount);
                break;
        }
    }

    [Fact]
    public void Every_frozen_case_is_replayed()
    {
        Assert.Equal(Corpus.RootElement.GetProperty("metadata").GetProperty("count").GetInt32(), Cases.Count);
    }

    private static void AssertOneHot(JsonElement frozen, string name, int featureCount)
    {
        var options = new OneHotEncoderOptions
        {
            Drop = Drop(frozen.GetProperty("drop")),
            Unknown = frozen.GetProperty("handleUnknown").GetString() == "ignore"
                ? UnknownCategory.Ignore
                : UnknownCategory.Refuse,
        };

        if (IsIntegral(frozen))
        {
            OneHotEncoder<int> integers = Encoders.OneHot<int>(Integers(frozen, "values"), featureCount, options);

            AssertCategories(frozen, integers.Categories, name, v => v.GetInt32().ToString(Invariant));
            Assert.Equal(frozen.GetProperty("encodedFeatureCount").GetInt32(), integers.EncodedFeatureCount);
            AssertSame(Doubles(frozen, "encoded"), integers.Transform(Integers(frozen, "values")), $"{name}: encoded");
            AssertSame(
                Doubles(frozen, "unseenEncoded"),
                integers.Transform(Integers(frozen, "unseenValues")),
                $"{name}: unseen");
            return;
        }

        string[] values = Strings(frozen, "values");
        OneHotEncoder<string> encoder = Encoders.OneHot<string>(values, featureCount, options);

        AssertCategories(frozen, encoder.Categories, name, v => v.GetString()!);
        Assert.Equal(frozen.GetProperty("encodedFeatureCount").GetInt32(), encoder.EncodedFeatureCount);
        AssertSame(Doubles(frozen, "encoded"), encoder.Transform(values), $"{name}: encoded");
        AssertSame(
            Doubles(frozen, "unseenEncoded"),
            encoder.Transform(Strings(frozen, "unseenValues")),
            $"{name}: unseen");
    }

    /// <summary>The reference's <c>drop</c>, read back as this package's enum.</summary>
    private static CategoryDrop Drop(JsonElement drop)
    {
        if (drop.ValueKind == JsonValueKind.Null)
        {
            return CategoryDrop.None;
        }

        return drop.GetString() == "first" ? CategoryDrop.First : CategoryDrop.IfBinary;
    }

    private static void AssertOrdinal(JsonElement frozen, string name, int featureCount)
    {
        if (IsIntegral(frozen))
        {
            OrdinalEncoder<int> integers = Encoders.Ordinal<int>(Integers(frozen, "values"), featureCount);

            AssertCategories(frozen, integers.Categories, name, v => v.GetInt32().ToString(Invariant));
            AssertSame(Doubles(frozen, "encoded"), integers.Transform(Integers(frozen, "values")), $"{name}: encoded");
            AssertSame(
                Doubles(frozen, "unseenEncoded"),
                integers.Transform(Integers(frozen, "unseenValues")),
                $"{name}: unseen");
            return;
        }

        string[] values = Strings(frozen, "values");
        OrdinalEncoder<string> encoder = Encoders.Ordinal<string>(values, featureCount);

        AssertCategories(frozen, encoder.Categories, name, v => v.GetString()!);
        AssertSame(Doubles(frozen, "encoded"), encoder.Transform(values), $"{name}: encoded");
        AssertSame(
            Doubles(frozen, "unseenEncoded"),
            encoder.Transform(Strings(frozen, "unseenValues")),
            $"{name}: unseen");
    }

    private static void AssertImputed(JsonElement frozen, string name, int featureCount)
    {
        double[] samples = [.. frozen.GetProperty("samples").EnumerateArray().Select(Number)];
        var options = new SimpleImputerOptions
        {
            Strategy = frozen.GetProperty("strategy").GetString() switch
            {
                "median" => ImputationStrategy.Median,
                "most_frequent" => ImputationStrategy.MostFrequent,
                "constant" => ImputationStrategy.Constant,
                _ => ImputationStrategy.Mean,
            },
            FillValue = frozen.GetProperty("fillValue").ValueKind == JsonValueKind.Null
                ? 0.0
                : frozen.GetProperty("fillValue").GetDouble(),
        };

        SimpleImputer imputer = SimpleImputer.Fit(samples, featureCount, options);

        AssertSame(Doubles(frozen, "statistics"), imputer.Statistics, $"{name}: statistics");
        AssertSame(Doubles(frozen, "imputed"), imputer.Transform(samples), $"{name}: imputed");
    }

    /// <summary>The categories, compared as text so one assertion serves both element types.</summary>
    private static void AssertCategories<T>(
        JsonElement frozen,
        IReadOnlyList<IReadOnlyList<T>> actual,
        string name,
        Func<JsonElement, string> read)
    {
        JsonElement[] expected = [.. frozen.GetProperty("categories").EnumerateArray()];

        Assert.True(
            expected.Length == actual.Count,
            $"{name}: {expected.Length} features of categories expected, {actual.Count} reported");
        for (int feature = 0; feature < expected.Length; feature++)
        {
            string[] want = [.. expected[feature].EnumerateArray().Select(read)];
            string[] got = [.. actual[feature].Select(v => Convert.ToString(v, Invariant)!)];
            Assert.True(
                want.SequenceEqual(got),
                $"{name}: feature {feature} categories [{string.Join(", ", want)}] against [{string.Join(", ", got)}]");
        }
    }

    private static bool IsIntegral(JsonElement frozen) =>
        frozen.TryGetProperty("elementType", out JsonElement type) && type.GetString() == "int";

    private static int[] Integers(JsonElement element, string name) =>
        [.. element.GetProperty(name).EnumerateArray().Select(v => v.GetInt32())];

    /// <summary>A missing value is spelled <c>"NaN"</c>, as every corpus here spells it.</summary>
    private static double Number(JsonElement element) =>
        element.ValueKind == JsonValueKind.String ? double.NaN : element.GetDouble();

    private static string[] Strings(JsonElement element, string name) =>
        [.. element.GetProperty(name).EnumerateArray().Select(v => v.GetString()!)];

    private static double[] Doubles(JsonElement element, string name) =>
        [.. element.GetProperty(name).EnumerateArray().Select(Number)];

    private static void AssertSame(double[] expected, IReadOnlyList<double> actual, string what)
    {
        Assert.True(expected.Length == actual.Count, $"{what}: {expected.Length} values expected, {actual.Count} reported");
        for (int i = 0; i < expected.Length; i++)
        {
            Assert.True(
                Math.Abs(expected[i] - actual[i]) <= Tolerance,
                $"{what}[{i}]: expected {expected[i]}, got {actual[i]}");
        }
    }
}
