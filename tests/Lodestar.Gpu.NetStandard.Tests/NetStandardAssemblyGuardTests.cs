using System.Reflection;
using System.Runtime.Versioning;
using Lodestar.Gpu.Compute;
using Xunit;

namespace Lodestar.Gpu.Tests;

/// <summary>Guards the premise of this project: the suite replays the netstandard2.1 build.</summary>
/// <remarks>
/// Without this, a reference that quietly resolved back to net10.0 would leave every test
/// passing while proving nothing (#529). Two assemblies: the GPU one, and the
/// <c>Lodestar.Abstractions</c> it takes an edge to since #1142. The CPU baselines this suite
/// compares against are deliberately not pinned, since what is replayed is the GPU assembly.
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

    /// <summary>The same guarantee for the Lodestar.Abstractions build the GPU assembly consumes.</summary>
    [Fact]
    public void Suite_runs_against_the_netstandard2_0_build_of_Lodestar_Abstractions()
    {
        string? framework = typeof(GpuSearchResult).Assembly
            .GetCustomAttribute<TargetFrameworkAttribute>()?.FrameworkName;

        Assert.Equal(".NETStandard,Version=v2.0", framework);
    }
}
