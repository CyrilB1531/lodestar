using System.Reflection;
using System.Runtime.Versioning;
using Lodestar.Gpu.Compute;
using Xunit;

namespace Lodestar.Gpu.Tests;

/// <summary>Guards the premise of this project: the suite replays the netstandard2.1 build.</summary>
/// <remarks>
/// Without this, a reference that quietly resolved back to net10.0 would leave every test
/// passing while proving nothing (#529). One assembly rather than several, because
/// <c>Lodestar.Gpu</c> carries no Lodestar edge — decision 0101 forbids one into it, and the
/// CPU baselines this suite compares against are deliberately not pinned: what is being
/// replayed is the GPU assembly, not the paths it is measured against.
/// </remarks>
public sealed class NetStandardAssemblyGuardTests
{
    [Fact]
    public void Suite_runs_against_the_netstandard2_1_build()
    {
        string? framework = typeof(GpuContext).Assembly
            .GetCustomAttribute<TargetFrameworkAttribute>()?.FrameworkName;

        Assert.Equal(".NETStandard,Version=v2.1", framework);
    }
}
