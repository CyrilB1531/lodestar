using ILGPU.Runtime.OpenCL;
using Lodestar.Gpu.Compute;
using Xunit;

namespace Lodestar.Gpu.Tests;

/// <summary>The Review B findings of <c>Lodestar.Gpu</c> after #1545, one fact or theory each, on ILGPU's CPU accelerator.</summary>
public sealed class ReviewBAfter1545Tests
{
    [Fact]
    public void Signatures_past_one_array_are_refused_before_the_kernel_runs()
    {
        // 300,000 × 8,192 values ran the whole kernel, then failed copying them back (#1558).
        using var context = GpuContext.Create(preferCpu: true);
        IReadOnlyList<uint>[] empty = [.. Enumerable.Repeat<IReadOnlyList<uint>>([], 300_000)];
        using DeviceTokenHashes documents = DeviceTokenHashes.Upload(context, empty);
        var kernel = new TiledMinHashSignatures(context);
        ulong[] multipliers = [.. Enumerable.Range(1, 8_192).Select(i => (ulong)i)];
        ulong[] addends = new ulong[8_192];

        ArgumentException error = Assert.Throws<ArgumentException>(
            () => kernel.Signatures(documents, multipliers, addends, MinHashScheme.Legacy));
        Assert.Equal("multipliers", error.ParamName);
    }

    [Fact]
    public void A_product_past_one_array_is_refused_before_the_launch()
    {
        // 70,000 rows × 32,768 columns is 2.29e9 values, which Download could never return (#1558).
        using var context = GpuContext.Create(preferCpu: true);
        const int Rows = 70_000;
        const int Width = 32_768;
        using DeviceSparseMatrix matrix = DeviceSparseMatrix.Upload(context, new int[Rows + 1], [], [], Rows, 1);
        var kernel = new TiledSparseDenseProduct(context);

        ArgumentException error = Assert.Throws<ArgumentException>(() => kernel.Multiply(matrix, new double[Width], Width));
        Assert.Equal("block", error.ParamName);
    }

    [Fact]
    public void A_text_batch_whose_characters_pass_one_array_is_refused()
    {
        // No string can report a length it does not hold, so the sum is tested on lengths (#1559).
        ArgumentException error = Assert.Throws<ArgumentException>(
            () => DeviceTextBlock.CharacterTotal([int.MaxValue, int.MaxValue, 3], "texts"));

        Assert.Equal("texts", error.ParamName);
        Assert.Equal(12, DeviceTextBlock.CharacterTotal([5, 7], "texts"));
    }

    [Theory]
    [InlineData(CLDeviceType.CL_DEVICE_TYPE_GPU, true)]
    [InlineData((CLDeviceType)((long)CLDeviceType.CL_DEVICE_TYPE_GPU | (long)CLDeviceType.CL_DEVICE_TYPE_DEFAULT), true)]
    [InlineData(CLDeviceType.CL_DEVICE_TYPE_CPU, false)]
    [InlineData(CLDeviceType.CL_DEVICE_TYPE_ACCELERATOR, false)]
    public void An_OpenCL_device_is_graphics_when_its_type_carries_the_GPU_bit(CLDeviceType type, bool expected) =>
        // CI has no OpenCL device, so #1516's branch was never reached (#1559).
        Assert.Equal(expected, GpuContext.HasGpuBit(type));
}
