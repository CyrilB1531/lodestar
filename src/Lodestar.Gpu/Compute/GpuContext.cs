using ILGPU;
using ILGPU.Runtime;
using ILGPU.Runtime.OpenCL;

namespace Lodestar.Gpu.Compute;

/// <summary>An ILGPU context and the accelerator its kernels run on.</summary>
/// <remarks>
/// One per process is the intended shape: creating an accelerator is expensive and ILGPU
/// compiles each kernel on first launch, so a context built per call would measure a compiler
/// rather than a kernel; the gate <c>src/Lodestar.Gpu/performance.md</c> applies excludes that warm-up.
/// </remarks>
public sealed class GpuContext : IDisposable
{
    private readonly Context _context;
    private readonly Accelerator _accelerator;
    private bool _disposed;

    /// <summary>The accelerator kernels are loaded onto.</summary>
    /// <exception cref="ObjectDisposedException">The context was disposed: the accelerator behind it is freed (#1512).</exception>
    public Accelerator Accelerator
    {
        get
        {
            EnsureNotDisposed();
            return _accelerator;
        }
    }

    /// <summary>The device's own name, which is what a published figure has to carry.</summary>
    public string DeviceName { get; }

    /// <summary>Whether the kernels are running on real graphics hardware.</summary>
    /// <remarks>
    /// <strong>Not the same question as the accelerator's type, and measured the hard way.</strong>
    /// An OpenCL runtime installed for a processor reports <see cref="AcceleratorType.OpenCL"/>
    /// and executes there: ILGPU enumerated one such device beside a CUDA card, and a check on
    /// the accelerator type alone called it a GPU. This asks OpenCL for the device type, so a
    /// benchmark cannot publish a processor's figure under a graphics heading.
    /// </remarks>
    public bool IsHardwareGpu { get; }

    private GpuContext(Context context, Accelerator accelerator, bool isHardwareGpu)
    {
        _context = context;
        _accelerator = accelerator;
        DeviceName = accelerator.Name;
        IsHardwareGpu = isHardwareGpu;
    }

    /// <summary>Opens the best available device, or ILGPU's CPU accelerator.</summary>
    /// <param name="preferCpu">
    /// Forces the CPU accelerator. That is what a machine with no GPU runs, and what proves a
    /// kernel <em>correct</em> where the 5–10× gate cannot be evaluated at all.
    /// </param>
    /// <remarks>
    /// <c>Context.GetPreferredDevice</c> is asked only for the CPU fallback: for a non-CPU device it returned the
    /// OpenCL-on-CPU runtime ahead of a CUDA card (measured), so this orders CUDA, then graphics OpenCL, itself. A
    /// device that fails to open surfaces ILGPU's own exception (#1513).
    /// </remarks>
    public static GpuContext Create(bool preferCpu = false)
    {
        Context context = Context.Create(builder => builder.Default());
        try
        {
            Device device = Select(context, preferCpu);
            return new GpuContext(context, device.CreateAccelerator(context), IsGpu(device));
        }
        catch
        {
            context.Dispose();
            throw;
        }
    }

    /// <summary>The device to open, CUDA first and graphics OpenCL second.</summary>
    private static Device Select(Context context, bool preferCpu)
    {
        if (preferCpu)
        {
            return context.GetPreferredDevice(preferCPU: true);
        }

        Device? cuda = context.Devices
            .FirstOrDefault(candidate => candidate.AcceleratorType == AcceleratorType.Cuda);
        Device? graphics = cuda ?? context.Devices.FirstOrDefault(IsGpu);
        return graphics ?? context.GetPreferredDevice(preferCPU: true);
    }

    /// <summary>Whether a device is graphics hardware rather than a runtime over a processor.</summary>
    private static bool IsGpu(Device device) => device switch
    {
        { AcceleratorType: AcceleratorType.Cuda } => true,
        // The OpenCL device type is a bitfield: a driver may report GPU together with DEFAULT (#1516).
        // ILGPU's enum carries no [Flags], so the test is on the underlying cl_device_type bits.
        CLDevice open => HasGpuBit(open.DeviceType),
        _ => false,
    };

    /// <summary>Whether an OpenCL device type carries the GPU bit; separate so it is testable without a device.</summary>
    internal static bool HasGpuBit(CLDeviceType type) =>
        ((long)type & (long)CLDeviceType.CL_DEVICE_TYPE_GPU) != 0;

    /// <summary>Whether <see cref="Dispose"/> has run, which every kernel and upload asks first.</summary>
    internal bool IsDisposed => _disposed;

    /// <summary>Releases the accelerator and the context, in that order; a second call does nothing.</summary>
    public void Dispose()
    {
        if (_disposed)
        {
            return;
        }

        _disposed = true;
        _accelerator.Dispose();
        _context.Dispose();
    }

    /// <summary>Throws when this context was disposed, before its accelerator is touched.</summary>
    /// <exception cref="ObjectDisposedException">The context was disposed.</exception>
    internal void EnsureNotDisposed() => Guard.NotDisposed(_disposed, this);
}
