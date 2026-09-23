using System.Text.Json;
using Xunit;

namespace Lodestar.Preprocessing.Tests;

/// <summary>Replays <c>tests/oracles/preprocessing_label_encoder.json</c>.</summary>
public sealed class LabelEncoderOracleTests
{
    [Fact]
    public void Every_case_matches_scikit_learn()
    {
        using JsonDocument document = OracleLoader.Load("preprocessing_label_encoder.json");
        int replayed = 0;

        foreach (JsonElement c in document.RootElement.GetProperty("cases").EnumerateArray())
        {
            int[] codes = [.. c.GetProperty("codes").EnumerateArray().Select(v => v.GetInt32())];

            // The element type decides the class order, so the two dtypes are two claims: a
            // string sorts by code point and an integer as a number (#1128).
            if (c.GetProperty("elementType").GetString() == "int")
            {
                ReplayIntegers(c, codes);
            }
            else
            {
                ReplayStrings(c, codes);
            }

            replayed++;
        }

        Assert.True(replayed >= 7, $"only {replayed} cases replayed");
    }

    private static void ReplayStrings(JsonElement c, int[] codes)
    {
        string[] labels = [.. c.GetProperty("labels").EnumerateArray().Select(v => v.GetString()!)];
        string[] classes = [.. c.GetProperty("classes").EnumerateArray().Select(v => v.GetString()!)];

        LabelEncoder<string> encoder = Encoders.Label<string>(labels);

        Assert.Equal(classes, encoder.Classes);
        Assert.Equal(codes, encoder.Transform(labels));
        Assert.Equal(labels, encoder.InverseTransform(codes));
    }

    private static void ReplayIntegers(JsonElement c, int[] codes)
    {
        int[] labels = [.. c.GetProperty("labels").EnumerateArray().Select(v => v.GetInt32())];
        int[] classes = [.. c.GetProperty("classes").EnumerateArray().Select(v => v.GetInt32())];

        LabelEncoder<int> encoder = Encoders.Label<int>(labels);

        Assert.Equal(classes, encoder.Classes);
        Assert.Equal(codes, encoder.Transform(labels));
        Assert.Equal(labels, encoder.InverseTransform(codes));
    }

    /// <summary>There is no <c>handle_unknown</c> on this transformer, on either side.</summary>
    [Fact]
    public void An_unseen_label_is_refused()
    {
        LabelEncoder<string> encoder = Encoders.Label<string>(["a", "b"]);

        Assert.Throws<ArgumentException>(() => encoder.Transform(["c"]));
        Assert.Throws<ArgumentOutOfRangeException>(() => encoder.InverseTransform([2]));
    }
}
