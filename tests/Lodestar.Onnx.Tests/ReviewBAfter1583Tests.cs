using Lodestar.Tests.Fixtures;
using Xunit;

namespace Lodestar.Onnx.Tests;

/// <summary>The Review B findings of <c>Lodestar.Onnx</c> after #1583, one fact each.</summary>
public sealed class ReviewBAfter1583Tests
{
    private static string Oracle(string file) => Path.Combine(AppContext.BaseDirectory, "oracles", file);

    [Fact]
    public void A_model_whose_fixed_axes_alone_pass_one_array_is_refused_once_when_opened()
    {
        // [65536, 32768] fixes every chunk at 2^31 cells whatever the call; it was refused per call, under an
        // argument no value could fix (#1589).
        ArgumentException error = Assert.Throws<ArgumentException>(
            () => new OnnxTextEmbedder(Oracle("tiny_embedder_huge_static.onnx")));

        Assert.Equal("modelPath", error.ParamName);
        Assert.Contains("65536 × 32768", error.Message, StringComparison.Ordinal);
    }

    [Fact]
    public void The_tokenizer_constructor_refuses_it_too()
    {
        Assert.Equal("modelPath", Assert.Throws<ArgumentException>(
            () => new OnnxTextEmbedder(Oracle("tiny_embedder_huge_static.onnx"), BatchCorpus.Tokenizer())).ParamName);
    }
}
