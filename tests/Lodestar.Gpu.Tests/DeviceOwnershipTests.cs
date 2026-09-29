using System.Reflection;
using Lodestar.Gpu.Compute;
using Xunit;

namespace Lodestar.Gpu.Tests;

/// <summary>What every multi-buffer upload leans on: a buffer is released when what is built from it fails (#1265).</summary>
public sealed class DeviceOwnershipTests
{
    [Fact]
    public void A_failed_build_releases_what_it_was_handed_and_rethrows()
    {
        using var owned = new Tracked();

        InvalidOperationException thrown = Assert.Throws<InvalidOperationException>(
            () => DeviceOwnership.ReleaseOnFailure<int>(owned, () => throw new InvalidOperationException("launch failed")));

        Assert.Equal("launch failed", thrown.Message);
        Assert.True(owned.Disposed);
    }

    [Fact]
    public void A_successful_build_keeps_what_it_was_handed()
    {
        using var owned = new Tracked();

        Assert.Equal(7, DeviceOwnership.ReleaseOnFailure(owned, () => 7));
        Assert.False(owned.Disposed);
    }

    /// <summary>
    /// The four uploads #1265 names each hand their first buffer to the helper, read from their IL: a call site that
    /// stopped doing so would otherwise stay green, since a device allocation cannot be made to fail here (#1515).
    /// </summary>
    [Theory]
    [InlineData(typeof(DeviceSparseMatrix), nameof(DeviceSparseMatrix.Upload))]
    [InlineData(typeof(DeviceTokenHashes), nameof(DeviceTokenHashes.Upload))]
    [InlineData(typeof(DeviceTextBlock), nameof(DeviceTextBlock.Upload))]
    [InlineData(typeof(TiledSparseDenseProduct), nameof(TiledSparseDenseProduct.Multiply))]
    public void Each_multi_buffer_upload_releases_through_the_helper(Type type, string name)
    {
        Assert.Contains(
            type.GetMethods(BindingFlags.Public | BindingFlags.Static | BindingFlags.Instance | BindingFlags.DeclaredOnly),
            method => method.Name == name && CallsReleaseOnFailure(method));
    }

    private static bool CallsReleaseOnFailure(MethodInfo method)
    {
        byte[] il = method.GetMethodBody()?.GetILAsByteArray() ?? [];
        for (int at = 0; at + 4 < il.Length; at++)
        {
            // 0x28 is call; its operand is a method token, which a false match fails to resolve.
            if (il[at] != 0x28)
            {
                continue;
            }

            try
            {
                MethodBase? callee = method.Module.ResolveMethod(BitConverter.ToInt32(il, at + 1));
                if (callee is { Name: "ReleaseOnFailure", DeclaringType.Name: "DeviceOwnership" })
                {
                    return true;
                }
            }
            catch (ArgumentException)
            {
                // Not a method token at this offset: the byte belonged to another operand.
            }
        }

        return false;
    }

    private sealed class Tracked : IDisposable
    {
        public bool Disposed { get; private set; }

        public void Dispose() => Disposed = true;
    }
}
