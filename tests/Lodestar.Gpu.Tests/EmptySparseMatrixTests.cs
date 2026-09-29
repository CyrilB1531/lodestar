using Lodestar.Abstractions;
using Lodestar.Gpu.Compute;
using Xunit;

namespace Lodestar.Gpu.Tests;

/// <summary>A matrix storing nothing uploads and multiplies as the CPU path does (#1265).</summary>
/// <remarks>
/// ILGPU's array overload throws on a zero-length array; a TF-IDF block of stop-word-only documents is
/// the ordinary way a caller hands one over.
/// </remarks>
public sealed class EmptySparseMatrixTests
{
    [Fact]
    public void A_matrix_with_no_stored_value_uploads_and_multiplies_to_zeros()
    {
        var empty = new CsrMatrix(2, 3, [], [], [0, 0, 0]);
        double[] block = [1.0, 2.0, 3.0, 4.0, 5.0, 6.0];
        double[] expected = empty.Multiply(block, 2);

        using var context = GpuContext.Create(preferCpu: true);
        using var device = DeviceSparseMatrix.Upload(context, [0, 0, 0], [], [], 2, 3);
        double[] product = new TiledSparseDenseProduct(context).Multiply(device, block, 2);

        Assert.Equal(0, device.NonZeroCount);
        Assert.Equal(expected, product);
        Assert.All(product, value => Assert.Equal(0.0, value));
    }
}
