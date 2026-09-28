using Xunit;

namespace Lodestar.Onnx.Tests;

/// <summary>The Review B findings of <c>Lodestar.Onnx</c> after #1325.</summary>
public sealed class ReviewBFindingsTests
{
    private static readonly string ModelPath = Path.Combine(AppContext.BaseDirectory, "oracles", "tiny_encoder.onnx");

    [Theory]
    [InlineData("inputIdsName")]
    [InlineData("attentionMaskName")]
    [InlineData("tokenTypeIdsName")]
    public void A_null_input_name_is_refused_under_its_own_name_before_the_model_opens(string name)
    {
        ArgumentNullException error = Assert.Throws<ArgumentNullException>(() => new OnnxTextEmbedder(
            "no-such-model.onnx",
            inputIdsName: name == "inputIdsName" ? null! : "input_ids",
            attentionMaskName: name == "attentionMaskName" ? null! : "attention_mask",
            tokenTypeIdsName: name == "tokenTypeIdsName" ? null! : "token_type_ids"));

        Assert.Equal(name, error.ParamName);
    }

    [Fact]
    public void Spans_of_different_lengths_are_refused_under_attentionMask()
    {
        using var embedder = new OnnxTextEmbedder(ModelPath);

        ArgumentException error = Assert.Throws<ArgumentException>(() => embedder.Embed([101, 102], [1]));

        Assert.Equal("attentionMask", error.ParamName);
    }
}
