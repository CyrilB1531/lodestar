namespace Lodestar.Gpu.Compute;

/// <summary>How many rows one launch may put on a grid's second axis.</summary>
/// <remarks>
/// Every tiled kernel spends the first axis on its lanes and the second on rows, documents or
/// queries. Measured on an RTX 5070 Ti, CUDA caps that axis at 65,535 groups and refused a launch
/// of 100,000 (#897), so the kernels launch in slices of this size with the first row passed in.
/// Read from the accelerator rather than written down, because the cap is the device's.
/// </remarks>
internal static class RowLaunch
{
    /// <summary>The device's largest grid extent on the second axis.</summary>
    internal static int Limit(GpuContext context)
    {
        Guard.NotNull(context);
        return context.Accelerator.MaxGridSize.Y;
    }
}
