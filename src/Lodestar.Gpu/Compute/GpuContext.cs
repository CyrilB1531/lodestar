using ILGPU;
using ILGPU.Runtime;
using ILGPU.Runtime.CPU;

namespace Lodestar.Gpu.Compute;

/// <summary>An ILGPU context and the accelerator its kernels run on.</summary>
/// <remarks>
/// One per process is the intended shape: creating an accelerator is expensive and
/// ILGPU compiles each kernel on first launch, so a context built per call would
/// measure a compiler rather than a kernel (decision 0102).
/// </remarks>
public sealed class GpuContext : IDisposable
{
    private readonly Context _context;

    /// <summary>The accelerator kernels are loaded onto.</summary>
    public Accelerator Accelerator { get; }

    /// <summary>Whether this fell back to ILGPU's CPU accelerator.</summary>
    /// <remarks>
    /// Read it before publishing a number. CI has no GPU, so a benchmark that does not
    /// say which accelerator produced a figure is a figure taken from nothing.
    /// </remarks>
    public bool IsCpuAccelerator => Accelerator.AcceleratorType == AcceleratorType.CPU;

    private GpuContext(Context context, Accelerator accelerator)
    {
        _context = context;
        Accelerator = accelerator;
    }

    /// <summary>Opens the preferred device, or ILGPU's CPU accelerator.</summary>
    /// <param name="preferCpu">
    /// Forces the CPU accelerator. That is what a machine with no GPU runs, and what
    /// proves a kernel <em>correct</em> where the 5–10× gate cannot be evaluated at all.
    /// </param>
    /// <exception cref="InvalidOperationException">No device could be opened.</exception>
    public static GpuContext Create(bool preferCpu = false)
    {
        Context context = Context.Create(builder => builder.Default());
        try
        {
            Device device = context.GetPreferredDevice(preferCpu);
            return new GpuContext(context, device.CreateAccelerator(context));
        }
        catch
        {
            context.Dispose();
            throw;
        }
    }

    /// <summary>Releases the accelerator and the context, in that order.</summary>
    public void Dispose()
    {
        Accelerator.Dispose();
        _context.Dispose();
    }
}
