namespace Lodestar.Gpu.Compute;

/// <summary>Hands a device buffer to what is built from it, and releases it when the build fails (#1265).</summary>
/// <remarks>
/// A type holding several resident buffers allocates them one after another; a later allocation, a launch or its
/// constructor can fail, and a buffer already allocated would otherwise stay on the device until the context goes.
/// </remarks>
internal static class DeviceOwnership
{
    /// <summary>The value <paramref name="build"/> returns, <paramref name="owned"/> disposed if it throws instead.</summary>
    internal static T ReleaseOnFailure<T>(IDisposable owned, Func<T> build)
    {
        try
        {
            return build();
        }
        catch
        {
            owned.Dispose();
            throw;
        }
    }
}
