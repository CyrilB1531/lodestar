using Microsoft.ML.OnnxRuntime;
using Xunit;

namespace Lodestar.Onnx.Tests;

/// <summary>The Review B findings of <c>Lodestar.Onnx</c> after #1532, one fact or theory each.</summary>
public sealed class ReviewBAfter1532Tests
{
    private static string Oracle(string file) => Path.Combine(AppContext.BaseDirectory, "oracles", file);

    [Fact]
    public void MaxSequenceLength_after_dispose_throws_as_the_other_members_do()
    {
        // It answered 16 from its cached value, though Dimension's page says every member throws (#1552).
        var embedder = new OnnxTextEmbedder(Oracle("tiny_embedder_static.onnx"));
        embedder.Dispose();

        ObjectDisposedException error = Assert.Throws<ObjectDisposedException>(() => embedder.MaxSequenceLength);
        Assert.Equal(typeof(OnnxTextEmbedder).FullName, error.ObjectName);
    }

    [Fact]
    public void A_disposed_embedder_is_named_in_full_on_both_targets()
    {
        // nameof gave the short name on netstandard2.0; only the type was asserted (#1553).
        var embedder = new OnnxTextEmbedder(Oracle("tiny_embedder.onnx"));
        embedder.Dispose();

        Assert.Equal(typeof(OnnxTextEmbedder).FullName, Assert.Throws<ObjectDisposedException>(
            () => embedder.Embed([2, 7, 9], [1, 1, 1])).ObjectName);
        Assert.Equal(typeof(OnnxTextEmbedder).FullName, Assert.Throws<ObjectDisposedException>(
            () => embedder.Dimension).ObjectName);
    }

    [Fact]
    public void A_missing_or_unreadable_model_fails_with_the_runtime_exception_the_page_names()
    {
        // #1524 documented OnnxRuntimeException and nothing pinned it (#1554).
        string missing = Path.Combine(Path.GetTempPath(), $"lodestar-missing-{Guid.NewGuid():N}.onnx");
        string garbage = Path.Combine(Path.GetTempPath(), $"lodestar-garbage-{Guid.NewGuid():N}.onnx");
        File.WriteAllBytes(garbage, [0x12, 0x34, 0x56, 0x78, 0x9A]);
        try
        {
            Assert.Throws<OnnxRuntimeException>(() => new OnnxTextEmbedder(missing));
            Assert.Throws<OnnxRuntimeException>(() => new OnnxTextEmbedder(garbage));
        }
        finally
        {
            File.Delete(garbage);
        }
    }

    [Fact]
    public void A_chunk_past_one_array_is_refused_by_name_before_allocating()
    {
        // A fixed batch of 65,536 and a 32,768-token input is 2^31 cells: new long[chunk * width] threw
        // OverflowException (#1555). Only the batch is fixed here, so the caller's length decides it (#1589).
        using var embedder = new OnnxTextEmbedder(Oracle("tiny_embedder_huge_batch.onnx"));
        long[] ids = new long[32_768];
        long[] mask = [.. Enumerable.Repeat(1L, 32_768)];

        ArgumentException error = Assert.Throws<ArgumentException>(() => embedder.Embed(ids, mask));
        Assert.Equal("inputIds", error.ParamName);
        Assert.Null(embedder.MaxSequenceLength);
    }
}
