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
            string[] labels = [.. c.GetProperty("labels").EnumerateArray().Select(v => v.GetString()!)];
            string[] classes = [.. c.GetProperty("classes").EnumerateArray().Select(v => v.GetString()!)];
            int[] codes = [.. c.GetProperty("codes").EnumerateArray().Select(v => v.GetInt32())];

            LabelEncoder<string> encoder = Encoders.Label<string>(labels);

            Assert.Equal(classes, encoder.Classes);
            Assert.Equal(codes, encoder.Transform(labels));
            Assert.Equal(labels, encoder.InverseTransform(codes));
            replayed++;
        }

        Assert.True(replayed >= 4, $"only {replayed} cases replayed");
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
