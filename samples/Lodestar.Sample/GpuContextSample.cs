using Lodestar.Gpu.Compute;

namespace Lodestar.Sample;

/// <summary>Which device the kernels will run on, and how to be sure.</summary>
internal static class GpuContextSample
{
    public static void Run()
    {
        Console.WriteLine("GpuContext (Lodestar.Gpu.Compute)");

        using GpuContext context = GpuCorpus.Open();
        Console.WriteLine($"  device           : {context.DeviceName}");

        // Read IsHardwareGpu, never the accelerator type: an OpenCL runtime installed for
        // a processor reports OpenCL and executes on the CPU.
        Console.WriteLine($"  graphics hardware: {context.IsHardwareGpu}");
        Console.WriteLine($"  accelerator type : {context.Accelerator.AcceleratorType}");
        Console.WriteLine();
    }
}
