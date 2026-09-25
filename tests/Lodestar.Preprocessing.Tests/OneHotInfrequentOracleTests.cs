using System.Globalization;
using System.Text.Json;
using Lodestar.Abstractions;
using Xunit;

namespace Lodestar.Preprocessing.Tests;

/// <summary>
/// Replays <c>OneHotEncoder</c>'s <c>min_frequency</c>, <c>max_categories</c> and <c>infrequent_if_exist</c> over
/// <c>tests/oracles/preprocessing_onehot_infrequent.json</c> (#1161), dense, sparse and by feature name.
/// </summary>
/// <remarks>Every value is 0 or 1 and every name is text, so the comparison is exact.</remarks>
public sealed class OneHotInfrequentOracleTests
{
    private static readonly JsonDocument Corpus = OracleLoader.Load("preprocessing_onehot_infrequent.json");

    private static IReadOnlyList<JsonElement> Cases => [.. Corpus.RootElement.GetProperty("cases").EnumerateArray()];

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
    public void The_encoding_matches_scikit_learn(int index)
    {
        JsonElement c = Cases[index];
        string element = c.GetProperty("elementType").GetString()!;
        if (element == "int")
        {
            Replay(c, v => v.GetInt32(), v => v.ToString(CultureInfo.InvariantCulture));
        }
        else if (element == "double")
        {
            Replay(c, v => v.GetDouble(), v => v.ToString("R", CultureInfo.InvariantCulture));
        }
        else
        {
            Replay(c, v => v.GetString()!, v => v);
        }
    }

    [Fact]
    public void Every_frozen_case_is_replayed() =>
        Assert.Equal(Corpus.RootElement.GetProperty("metadata").GetProperty("count").GetInt32(), Cases.Count);

    private static void Replay<T>(JsonElement c, Func<JsonElement, T> read, Func<T, string> text)
        where T : IComparable<T>, IEquatable<T>
    {
        string name = c.GetProperty("name").GetString()!;
        T[] values = [.. c.GetProperty("values").EnumerateArray().Select(read)];
        OneHotEncoder<T> encoder = Encoders.OneHot<T>(values, c.GetProperty("featureCount").GetInt32(), Options(c));

        Assert.Equal(c.GetProperty("encodedFeatureCount").GetInt32(), encoder.EncodedFeatureCount);
        Assert.Equal(Strings(c, "featureNames"), encoder.FeatureNames());
        JsonElement[] infrequent = [.. c.GetProperty("infrequentCategories").EnumerateArray()];
        for (int feature = 0; feature < infrequent.Length; feature++)
        {
            IReadOnlyList<T>? actual = encoder.InfrequentCategories[feature];
            if (infrequent[feature].ValueKind == JsonValueKind.Null)
            {
                Assert.Null(actual);
            }
            else
            {
                Assert.Equal([.. infrequent[feature].EnumerateArray().Select(read).Select(text)], actual!.Select(text));
            }
        }

        Assert.True(Doubles(c, "encoded").SequenceEqual(encoder.Transform(values)), $"{name}: dense");
        CsrMatrix sparse = encoder.TransformSparse(values);
        Assert.Equal(Doubles(c, "sparseValues"), sparse.Values);
        Assert.Equal(Ints(c, "sparseColumns"), sparse.ColumnIndices);
        Assert.Equal(Ints(c, "sparseRowPointers"), sparse.RowPointers);

        T[] unseen = [.. c.GetProperty("unseenValues").EnumerateArray().Select(read)];
        if (unseen.Length > 0)
        {
            Assert.True(Doubles(c, "unseenEncoded").SequenceEqual(encoder.Transform(unseen)), $"{name}: unseen");
        }
    }

    private static OneHotEncoderOptions Options(JsonElement c)
    {
        JsonElement frequency = c.GetProperty("minFrequency");
        JsonElement most = c.GetProperty("maxCategories");
        JsonElement drop = c.GetProperty("drop");
        bool share = frequency.ValueKind == JsonValueKind.Number && frequency.GetRawText().Contains('.', StringComparison.Ordinal);
        return new OneHotEncoderOptions
        {
            Drop = drop.ValueKind == JsonValueKind.Null ? CategoryDrop.None : DropOf(drop.GetString()!),
            Unknown = c.GetProperty("handleUnknown").GetString() switch
            {
                "ignore" => UnknownCategory.Ignore,
                "infrequent_if_exist" => UnknownCategory.Infrequent,
                _ => UnknownCategory.Refuse,
            },
            MinFrequency = frequency.ValueKind == JsonValueKind.Number && !share ? frequency.GetInt32() : null,
            MinFrequencyShare = share ? frequency.GetDouble() : null,
            MaxCategories = most.ValueKind == JsonValueKind.Number ? most.GetInt32() : null,
        };
    }

    private static CategoryDrop DropOf(string drop) => drop == "first" ? CategoryDrop.First : CategoryDrop.IfBinary;

    private static string[] Strings(JsonElement c, string key) => [.. c.GetProperty(key).EnumerateArray().Select(v => v.GetString()!)];

    private static double[] Doubles(JsonElement c, string key) => [.. c.GetProperty(key).EnumerateArray().Select(v => v.GetDouble())];

    private static int[] Ints(JsonElement c, string key) => [.. c.GetProperty(key).EnumerateArray().Select(v => v.GetInt32())];
}
